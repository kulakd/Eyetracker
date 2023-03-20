using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.IO;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;

    public class ImitationEyetracker : ICalibratableEyetracker, IEyeCamera, IDataRecordingDevice
    {
        #region PInvoke
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator System.Drawing.Point(POINT point)
            {
                return new System.Drawing.Point(point.X, point.Y);
            }

            public static implicit operator GameLab.Geometry.Point(POINT point)
            {
                return new GameLab.Geometry.Point(point.X, point.Y);
            }

            /*
            public static implicit operator PunktF(POINT point)
            {
                return new PunktF(point.X, point.Y);
            }
            */
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        private static Point getMouseCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);

            return (Point)lpPoint;
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);
        //public static extern short GetKeyState(UInt16 virtualKeyCode);
        const int VK_LBUTTON = 0x01;
        const int VK_RBUTTON = 0x02;

        private static bool isLeftMouseButtonPressed()
        {
            return GetAsyncKeyState(VK_LBUTTON) != 0;
            //return GetKeyState(VK_LBUTTON) != 0; //tu trzeba by inaczej ustalić warunek
        }
        private static bool isRightMouseButtonPressed()
        {
            return GetAsyncKeyState(VK_RBUTTON) != 0;
            //return GetKeyState(VK_RBUTTON) != 0; //tu trzeba by inaczej ustalić warunek
        }
        #endregion

        #region Timer sprawdzający czy nie zmieniła się pozycja myszy
        //alternatywą jest ustawienie haka, zob. http://support.microsoft.com/en-us/kb/318804
        Timer positionTimer = new Timer(10); //10ms
        #endregion

        #region Timer udający zmianę obrazów
        Timer imagesTimer = new Timer(1000); //1s
        #endregion

        private bool updateOnlyWhenMouseStateChanges;
        private Random random = null;
        private const int noiseRange = 5;

        public string Name
        {
            get
            {
                return "Mouse-based imitation of eyetracker";
            }
        }

        public bool IsValidationPossible { get { return true; } }

        private bool leftEyeEnabled, rightEyeEnabled, averagedEyeEnabled;
        private EyeDataSample emptyLeftEyeDataSample = new EyeDataSample() { EyeSide = EyeSide.LeftEye, PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };
        private EyeDataSample emptyRightEyeDataSample = new EyeDataSample() { EyeSide = EyeSide.LeftEye, PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };
        private EyeDataSample emptyAveragedEyeDataSample = new EyeDataSample() { EyeSide = EyeSide.LeftEye, PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };

        public ImitationEyetracker(bool updateOnlyWhenMouseStateChanges = true, bool addNoise = true, bool leftEyeEnabled = true, bool rightEyeEnabled = true, bool averagedEyeEnabled = true)
        {
            this.updateOnlyWhenMouseStateChanges = updateOnlyWhenMouseStateChanges;
            if (addNoise) random = new Random();

            this.leftEyeEnabled = leftEyeEnabled;
            this.rightEyeEnabled = rightEyeEnabled;
            this.averagedEyeEnabled = averagedEyeEnabled;

            LeftEyeDetected = true;
            RightEyeDetected = true;

            LeftEyeOffset = PointF.Zero;
            RightEyeOffset = PointF.Zero;
            AveragedEyeOffset = PointF.Zero;
        }

        public bool Connected { get; private set; }

        public bool Calibrated { get; private set; }

        public bool Connect(string ipServer, int portServer, string ipClient, int portClient, ref string message)
        {
            //obraz
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("GameLab.Eyetracking.gamelab_249x249.bmp");
            if (stream == null) return false;
            image = new Bitmap(stream);
            imagesTimer.Elapsed += imageTimer_Elapsed;

            //położenie kursora
            positionTimer.Elapsed += positionTimer_Elapsed;
            mouseCursorPosition = getMouseCursorPosition();
            previousMouseCursorPosition = mouseCursorPosition;

            /*
            //rozmiar źrenic
            leftEyePupilSize = isLeftMouseButtonPressed() ? 0 : 25;
            previousLeftEyePupilSize = leftEyePupilSize;
            rightEyePupilSize = isRightMouseButtonPressed() ? 0 : 26;
            previousRightEyePupilSize = rightEyePupilSize;
            */

            //uruchamianie timerów
            positionTimer.Start();
            imagesTimer.Start();
            Connected = true;

            //positionTimer_Elapsed(null, null);

            return true;
        }
        
        private Point previousMouseCursorPosition, mouseCursorPosition;
        private bool previousIsLeftMouseButtonPressed = false, previousIsRightMouseButtonPressed = false;
        private float previousLeftEyePupilSize = 25, leftEyePupilSize = 25;
        private float previousRightEyePupilSize = 26, rightEyePupilSize = 26;
        private PointF leftEyeShift = new PointF(-20, 0), rightEyeShit = new PointF(20, 0);
        private int maximalEyesNoiseShiftX = 30, maximalEyesNoiseShiftY = 5;        
        private EyeDataSample leftEyeData, rightEyeData, averagedEyeData;
        private Bitmap image = null;

        public bool LeftEyeDetected { get; private set; }
        public bool RightEyeDetected { get; private set; }

        public PointF LeftEyeOffset { get; set; } //aplikacje może to użyć lub nie
        public PointF RightEyeOffset { get; set; }
        public PointF AveragedEyeOffset { get; set; }

        void positionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Connected) return;

            mouseCursorPosition = getMouseCursorPosition();
            if(random != null)
            {
                mouseCursorPosition.X += random.Next(-noiseRange, noiseRange);
                mouseCursorPosition.Y += random.Next(-noiseRange, noiseRange);
            }
            bool _isLeftMouseButtonPressed = isLeftMouseButtonPressed();
            bool _isRightMouseButtonPressed = isRightMouseButtonPressed();
            //leftEyePupilSize = isLeftMouseButtonPressed() ? 0 : 25;
            //rightEyePupilSize = isRightMouseButtonPressed() ? 0 : 26;
            bool isMouseStateChanged = mouseCursorPosition != previousMouseCursorPosition || 
                 _isLeftMouseButtonPressed != previousIsLeftMouseButtonPressed || _isRightMouseButtonPressed != previousIsRightMouseButtonPressed ||
                 leftEyePupilSize != previousLeftEyePupilSize || rightEyePupilSize != previousRightEyePupilSize;
            if (!updateOnlyWhenMouseStateChanges || isMouseStateChanged)
            {
                PointF _leftEyeShift = leftEyeShift; if (random != null) _leftEyeShift = new PointF(-random.Next(maximalEyesNoiseShiftX), -random.Next(-maximalEyesNoiseShiftY, maximalEyesNoiseShiftY));
                PointF _rightEyeShift = rightEyeShit; if (random != null) _rightEyeShift = new PointF(+random.Next(maximalEyesNoiseShiftX), -random.Next(-maximalEyesNoiseShiftY, maximalEyesNoiseShiftY));
                leftEyeData = new EyeDataSample()
                {
                    //Pozycja = położenieKursoraMyszy,
                    PositionF = mouseCursorPosition + _leftEyeShift,
                    EyeSide = EyeSide.LeftEye,
                    PupilSize = leftEyePupilSize,
                    OffsetCorrection = LeftEyeOffset
                };
                //if (_isLeftMouseButtonPressed) leftEyeData.PositionF = PointF.Zero;
                rightEyeData = new EyeDataSample()
                {
                    //Pozycja = położenieKursoraMyszy,
                    PositionF = mouseCursorPosition + _rightEyeShift,
                    EyeSide = EyeSide.RightEye,
                    PupilSize = rightEyePupilSize,
                    OffsetCorrection = RightEyeOffset
                };
                //if (_isRightMouseButtonPressed) rightEyeData.PositionF = PointF.Zero;
                averagedEyeData = new EyeDataSample()
                {
                    //Pozycja = położenieKursoraMyszy,
                    PositionF = (leftEyeData.PositionF + rightEyeData.PositionF) / 2f,
                    EyeSide = EyeSide.AveragedOrBestEye,
                    PupilSize = (leftEyePupilSize + rightEyePupilSize) / 2f,
                    OffsetCorrection = AveragedEyeOffset
                };
                LeftEyeDetected = !_isLeftMouseButtonPressed;
                RightEyeDetected = !_isRightMouseButtonPressed;
                if (leftEyeEnabled && LeftEyeDataUpdated != null) LeftEyeDataUpdated(leftEyeData);
                if (rightEyeEnabled && RightEyeDataUpdated != null) RightEyeDataUpdated(rightEyeData);
                if (averagedEyeEnabled && AveragedEyeDataUpdated != null) AveragedEyeDataUpdated(averagedEyeData);
            }
            previousMouseCursorPosition = mouseCursorPosition;
            previousIsLeftMouseButtonPressed = _isLeftMouseButtonPressed;
            previousIsRightMouseButtonPressed = _isRightMouseButtonPressed;
            previousLeftEyePupilSize = leftEyePupilSize;
            previousRightEyePupilSize = rightEyePupilSize;
        }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {
            return Connect(settings.ServerIp, settings.ServerPort, settings.ClientIp, settings.ClientPort, ref message);
        }

        bool imageTimerElapsedExecuting = false;
        private void imageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Connected || !ImagesUpdatingEnabled) return;
            if (imageTimerElapsedExecuting) return; //na wypadek gdyby wykonanie tej metody było dłuższe niż okres timera

            imageTimerElapsedExecuting = true;

            Graphics g = Graphics.FromImage(image);
            Font font = new Font("Calibri", 16);
            string data = DateTime.Now.ToString();
            g.FillRectangle(Brushes.BlanchedAlmond, 0, 0, image.Width, 35);
            g.DrawString(data, font, Brushes.Green, 0, 0, StringFormat.GenericDefault);
            g.Flush();

            if (EyeImageUpdated != null) EyeImageUpdated(EyeImage);
            if (SceneImageUpdated != null) SceneImageUpdated(SceneImage);
            if (EyeTrackingImageUpdated != null) EyeTrackingImageUpdated(EyeTrackingImage);
            imageTimerElapsedExecuting = false;
        }

        public bool Disconnect(ref string komunikat)
        {
            Connected = false;
            Calibrated = false;
            positionTimer.Stop();            
            return true;
        }

        public event CalibrationOrValidationFinishedEventHandler CalibrationOrValidationFinished;
        private void OnCalibrationOrValidationFinished(bool result)
        {
            if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(result);
        }

        private bool abortCalibrationOrValidation = false;

        public bool Calibrate(int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath, int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints, int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {
            if (customCalibrationScreen == null)
            {                
                //throw new GameLabException("Imitacja eyetrackera wspiera tylko kalibrację z własnym ekranem kalibracji");
                message = "Imitacja eyetrackera wspiera tylko kalibrację z własnym ekranem kalibracji (parametry: n=" + numberOfCalibrationPoints + ", jasność=" + backgroundBrightness + ")";
                return false;
            }

            int margin = 100;

            customCalibrationScreen.CalibrationPoint = new CalibrationPoint() { Index = 0, Position = new Point(0, 0) }; //żeby zgadzała się liczba punktów, ZMIENIC!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (customCalibrationScreen != null)
            {
                for (int i = 1; i <= numberOfCalibrationPoints; i++)
                {
                    if (abortCalibrationOrValidation) break;
                    int x = (customCalibrationScreen.ScreenSize.Width - 2 * margin) * i / numberOfCalibrationPoints;
                    int y = (customCalibrationScreen.ScreenSize.Height - 2 * margin) * i / numberOfCalibrationPoints;
                    customCalibrationScreen.CalibrationPoint = new CalibrationPoint() { Index = i, Position = new Point(x, y) };
                    System.Threading.Thread.Sleep(3000); //przerwy między punktami kalibracji
                }
                //własnyEkranKalibracji.Zamknij();
                OnCalibrationOrValidationFinished(true);
            }

            Calibrated = true;            
            return true;
        }

        public bool Calibrate(EyetrackerCalibrationSettings settings, ref string message)
        {
            return Calibrate(
                settings.NumberOfCalibrationPoints, settings.DisplayDeviceIndex, settings.ImageFilePath, settings.ImageSize,
                settings.IncreasedSpeed, settings.AcceptationRequiredForEveryCalibrationPoint, settings.BackgroundBrightness, ref message, settings.CustomCalibrationScreen);
        }

        public bool Validate(int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath, int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints, int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {
            if (customCalibrationScreen == null)
            {
                //throw new GameLabException("Imitacja eyetrackera wspiera tylko kalibrację z własnym ekranem kalibracji");
                message = "Imitacja eyetrackera wspiera tylko kalibrację z własnym ekranem kalibracji";
                return false;
            }

            int margin = 100;

            customCalibrationScreen.CalibrationPoint = new CalibrationPoint() { Index = 1, Position = new Point(0, 0) }; //żeby zgadzała się liczba punktów, ZMIENIC!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (customCalibrationScreen != null)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (abortCalibrationOrValidation) break;
                    int x = margin;
                    int y = margin;
                    if (i == 2 || i == 3) x = customCalibrationScreen.ScreenSize.Width - margin;
                    if (i > 2) y = customCalibrationScreen.ScreenSize.Height - margin;
                    customCalibrationScreen.CalibrationPoint = new CalibrationPoint() { Index = i, Position = new Point(x, y) };
                    System.Threading.Thread.Sleep(500);
                }
                //własnyEkranKalibracji.Zamknij();
                OnCalibrationOrValidationFinished(true);
            }

            return true;
        }

        public bool Validate(EyetrackerCalibrationSettings settings, ref string message)
        {
            return Validate(
                settings.NumberOfCalibrationPoints, settings.DisplayDeviceIndex, settings.ImageFilePath, settings.ImageSize,
                settings.IncreasedSpeed, settings.AcceptationRequiredForEveryCalibrationPoint, settings.BackgroundBrightness, ref message, settings.CustomCalibrationScreen);
        }

        public bool AbortCalibrationOrValidation()
        {
            abortCalibrationOrValidation = true;
            return true; //w tej implementacji kalibracja jest realizowana w bieżącym wątku
        }

        //bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription /*ref string komunikat*/);
        public bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription)
        {
            leftEyeDescription = "<imitacja eyetrackera>";
            rightEyeDescription = "<imitacja eyetrackera>";
            return true;
        }

        public bool ImagesUpdatingEnabled { get; set; }

        public Bitmap EyeImage
        {
            get 
            {
                return image; 
            }
        }

        public event ImageUpdatedEventHandler EyeImageUpdated;

        public Bitmap SceneImage
        {
            get 
            {
                return image;
            }
        }

        public event ImageUpdatedEventHandler SceneImageUpdated;

        public Bitmap EyeTrackingImage
        {
            get 
            {
                return image;
            }
        }

        public event ImageUpdatedEventHandler EyeTrackingImageUpdated;

        public EyeDataSample LeftEyeData
        {
            get
            {
                return leftEyeEnabled ? leftEyeData : emptyLeftEyeDataSample;
            }
        }

        public EyeDataSample RightEyeData
        {
            get 
            {
                return rightEyeEnabled ? rightEyeData : emptyRightEyeDataSample;
            }
        }

        public EyeDataSample AveragedEyeData
        {
            get
            {
                return averagedEyeEnabled ? averagedEyeData : emptyAveragedEyeDataSample;
            }
        }

        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;
        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;
        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        public bool IsDataRecordingPossible
        {
            get
            {
                return true;
            }
        }

        public bool DataRecordingEnabled { get; private set; }

        List<string> localDataRegistry = null; //tylko znaczniki

        public bool BeginDataRecording(ref string message)
        {
            localDataRegistry = new List<string>();
            DataRecordingEnabled = true;
            return true;
        }

        public bool EndDataRecording(ref string message)
        {
            DataRecordingEnabled = false;
            return true;
        }

        public bool SetMarker(string markerText, ref string message)
        {
            string ticks = DateTime.Now.Ticks.ToString();
            localDataRegistry.Add(ticks + " " + markerText);
            return true;
        }

        public bool SaveData(string filePath, string subjectName, string description, ref string message)
        {
            localDataRegistry.Insert(0, "#description: " + description);
            localDataRegistry.Insert(0, "#subject's name: " + subjectName);
            localDataRegistry.Insert(0, "#file path: " + filePath);
            //ignoruję przesłaną ścieżkę i zapisuję w katalogu programu - ścieżka jest natomiast w komentarzu (linia wyżej)
            System.IO.File.WriteAllLines("ImitacjaEyetrackera_" + GameLab.DateTimeHelper.CurrentDateTimePathString + ".txt", localDataRegistry.ToArray());
            return true;
        }
    }
}
