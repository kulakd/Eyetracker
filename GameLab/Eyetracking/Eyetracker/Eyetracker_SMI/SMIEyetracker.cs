using System;
using System.Collections.Generic;
using System.Text;

//stąd nie da się wyrzucić System.Drawing (Biblioteki SMI z niego korzystają)
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using ColorPalette = System.Drawing.Imaging.ColorPalette;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace GameLab.Eyetracking
{
    using EyeTrackingController;
    using GameLab.Geometry;

    public class SMIEyetracker : ICalibratableEyetracker, IEyeCamera, IDataRecordingDevice
    {
        private EyeTrackingController ETDevice;
        public bool Connected { get; private set; }
        public bool Calibrated { get; private set; }

        public string Name
        {
            get
            {
                return "SMI Eyetracker (RED250)";
            }
        }

        public static string Notes
        {
            get
            {
                return "Run iViewX application; for network connection: ip address and port from iViewX configuration (default ports 4444 and 5555)";
            }
        }

        public SMIEyetracker()
        {
            ETDevice = new EyeTrackingController();
            Connected = false;
            Calibrated = false;

            leftEyeData = new EyeDataSample() { EyeSide = EyeSide.LeftEye, PositionF = new PointF(-1,-1), PupilSize = -1, OffsetCorrection = LeftEyeOffset };
            rightEyeData = new EyeDataSample() { EyeSide = EyeSide.RightEye, PositionF = new PointF(-1, -1), PupilSize = -1, OffsetCorrection = RightEyeOffset };
            averagedEyeData = new EyeDataSample() { EyeSide = EyeSide.AveragedOrBestEye, PositionF = new PointF(-1, -1), PupilSize = -1, OffsetCorrection = AveragedEyeOffset };

            LeftEyeDetected = true;
            RightEyeDetected = true;

            LeftEyeOffset = PointF.Zero;
            RightEyeOffset = PointF.Zero;
            AveragedEyeOffset = PointF.Zero;
        }

        private static string ErrorDescription(int errorCode)
        {
            //UZUPEŁNIC Z DOKUMENTACJI!!!!!!!!!!!!!!!!!!!!!!!
            string message;
            switch(errorCode)
            {
                case 1:
                    message = "operacja zakończona powodzeniem";
                    break;
                case 2:
                    message = "nowe dane nie są dostępne";
                    break;
                case 3:
                    message = "operacja została przerwana";
                    break;
                case 100:
                    message = "nie udało się nawiązać połączenia";
                    break;
                case 101:
                    message = "brak połączenia";
                    break;
                case 102:
                    message = "system nie jest skalibrowany";
                    break;
                case 103:
                    message = "system nie jest zwalidowany";
                    break;
                case 104:
                    message = "aplikacja SMI nie jest uruchomiona na serwerze";
                    break;
                case 105:
                    message = "niepoprawne ustawienia portów";
                    break;
                case 111:
                    message = "nie podłączony eyetracker odpowiedni do tej operacji";
                    break;
                case 112:
                    message = "parametr poza zakresem";
                    break;
                case 113:
                    message = "nie podłączony eyetracker wymagany do wybranego sposobu wykonania operacji";
                    break;
                case 121:
                    message = "utworzenie gniazd nie powiodło się";
                    break;
                case 122:
                    message = "połączenie gniazd nie powiodło się";
                    break;
                case 123:
                    message = "nie udało się związać gniazda";
                    break;
                case 124:
                    message = "nie można usunąć gniazd";
                    break;
                case 131:
                    message = "brak odpowiedzi z iViewX";
                    break;
                case 191:
                    message = "bufor nagrywania jest pusty";
                    break;
                case 192:
                    message = "rejestrowanie danych jest już włączone";
                    break;
                case 193:
                    message = "bufor rejestrowania danych jest pełen";
                    break;
                default:
                    message = "nierozpoznany wynik";
                    break;
            }
            message += " (kod błędu: " + errorCode.ToString() + ")";
            return message;
        }

        public bool Connect(string ipServer, int portServer, string ipClient, int portClient, ref string message)
        {
            try
            {
                //ETDevice.iV_SetLogger(Convert.ToInt32(loggerstatus.Text), new StringBuilder("iViewXSDK_cs_Demo.txt"));                

                SetInternalEventCallbacks(); //podłącz wewnętrzne zdarzenia

                int errorCode = ETDevice.iV_Connect(new StringBuilder(ipServer), portServer, new StringBuilder(ipClient), portClient);
                ignoreSelectedErrors(ref errorCode, ErrorContext.Connecting);
                message = ErrorDescription(errorCode);                
                Connected = (errorCode == 1);

                ETDevice.iV_StopRecording(); //bez obsługi błędów

                return Connected;
            }
            catch(Exception exc)
            {
                message = exc.Message;
                Connected = false;
                return false;
            }
        }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {
            return Connect(settings.ServerIp, settings.ServerPort, settings.ClientIp, settings.ClientPort, ref message);
        }

        public bool Disconnect(ref string message)
        {
            try
            {
                int errorCode = ETDevice.iV_Disconnect();
                ignoreSelectedErrors(ref errorCode, ErrorContext.Disconnecting);
                message = ErrorDescription(errorCode);
                if (errorCode == 1) Connected = false;
                return errorCode == 1;
            }
            catch (Exception exc)
            {
                message = exc.Message;
                return false;
            }
        }

        /*
        public bool Kalibracja(int liczbaPunktówKalibracji, int numerMonitora, string ścieżkaPlikuObrazka, 
            int rozmiarObrazka, bool zwiększonaPrędkość, bool akceptacjaWymaganaPoKażdejZmianiePołożenia, 
            int jasnośćTła, ref string komunikat)
        {
            try
            {
                EyeTrackingController.CalibrationStruct m_CalibrationData;
                m_CalibrationData.displayDevice = numerMonitora;
                m_CalibrationData.autoAccept = akceptacjaWymaganaPoKażdejZmianiePołożenia ? 0 : 1;
                m_CalibrationData.method = liczbaPunktówKalibracji;
                m_CalibrationData.visualization = 1;
                m_CalibrationData.speed = zwiększonaPrędkość ? 1 : 0;
                m_CalibrationData.targetShape = 0; //obrazek
                m_CalibrationData.backgroundColor = jasnośćTła; //jasność tła
                m_CalibrationData.foregroundColor = 250;
                m_CalibrationData.targetSize = rozmiarObrazka;
                m_CalibrationData.targetFilename = ścieżkaPlikuObrazka;

                int kodBłędu = ETDevice.iV_SetupCalibration(ref m_CalibrationData);
                komunikat = OpisBłędu(kodBłędu);

                kodBłędu = ETDevice.iV_Calibrate();
                komunikat = OpisBłędu(kodBłędu);

                Skalibrowany = (kodBłędu == 1);
                return Skalibrowany;
            }
            catch (Exception exc)
            {
                komunikat = exc.Message;
                Skalibrowany = false;
                return false;
            }
        }
        */

        private ICustomCalibrationScreen customCalibrationScreen = null;

        public bool IsValidationPossible { get { return true; } }

        public bool Calibrate(
            int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath,
            int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints,
            int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {
            return KalibracjaLubWalidacja(
                true,
                numberOfCalibrationPoints, displayDeviceIndex, imageFilePath,
                imageSize, increasedSpeed, acceptationRequiredForEveryCalibrationPoints,
                backgroundBrightness, ref message, customCalibrationScreen);
        }

        public bool Calibrate(EyetrackerCalibrationSettings settings, ref string message)
        {
            return Calibrate(
                settings.NumberOfCalibrationPoints, settings.DisplayDeviceIndex, settings.ImageFilePath, settings.ImageSize,
                settings.IncreasedSpeed, settings.AcceptationRequiredForEveryCalibrationPoint, settings.BackgroundBrightness, ref message, settings.CustomCalibrationScreen);
        }

        public bool Validate(
            int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath,
            int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints,
            int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {
            return KalibracjaLubWalidacja(
                false,
                numberOfCalibrationPoints, displayDeviceIndex, imageFilePath,
                imageSize, increasedSpeed, acceptationRequiredForEveryCalibrationPoints,
                backgroundBrightness, ref message, customCalibrationScreen);
        }

        public bool Validate(EyetrackerCalibrationSettings settings, ref string message)
        {
            return Validate(
                settings.NumberOfCalibrationPoints, settings.DisplayDeviceIndex, settings.ImageFilePath, settings.ImageSize,
                settings.IncreasedSpeed, settings.AcceptationRequiredForEveryCalibrationPoint, settings.BackgroundBrightness, ref message, settings.CustomCalibrationScreen);
        }

        private bool KalibracjaLubWalidacja(
            bool calibrationOrValidation, //true=kalibracja, false=walidacja
            int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath,
            int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints,
            int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {            
            try
            {
                EyeTrackingController.CalibrationStruct m_CalibrationData;
                m_CalibrationData.displayDevice = displayDeviceIndex;
                m_CalibrationData.autoAccept = acceptationRequiredForEveryCalibrationPoints ? 0 : 1;
                m_CalibrationData.method = numberOfCalibrationPoints;
                m_CalibrationData.visualization = 1;
                m_CalibrationData.speed = increasedSpeed ? 1 : 0;
                m_CalibrationData.targetShape = 0; //obrazek
                m_CalibrationData.backgroundColor = backgroundBrightness; //jasność tła
                m_CalibrationData.foregroundColor = 250;
                m_CalibrationData.targetSize = imageSize;
                m_CalibrationData.targetFilename = imageFilePath;

                this.customCalibrationScreen = customCalibrationScreen;
                if (customCalibrationScreen != null)
                {
                    m_CalibrationData.visualization = 0;
                    //Sterowanie własnym ekranem kalibracji jest przeprowadzane w CalibrationCallbackFunction
                }

                int kodBłędu = ETDevice.iV_SetupCalibration(ref m_CalibrationData);
                ignoreSelectedErrors(ref kodBłędu, ErrorContext.CalibrationOrValidation);
                message = ErrorDescription(kodBłędu);                

                //jeżeli własny ekran, to ta metoda kończy się przed zakończeniem kalibracji
                if(calibrationOrValidation)
                    kodBłędu = ETDevice.iV_Calibrate(); 
                else
                    kodBłędu = ETDevice.iV_Validate();
                ignoreSelectedErrors(ref kodBłędu, ErrorContext.CalibrationOrValidation);
                message = ErrorDescription(kodBłędu);

                //przy kalibracji z własnym oknem tu wiadomo tak naprawdę tylko tyle, czy udało się rozpocząć kalibrację
                Calibrated = (kodBłędu == 1);
                return Calibrated;
            }
            catch (Exception exc)
            {
                message = exc.Message;
                Calibrated = false;
                return false;
            }
            /*
            finally
            {
                if (własnyEkranKalibracji != null)
                {
                    ETDevice.iV_SetCalibrationCallback(null);
                }
            }
            */
        }

        public bool AbortCalibrationOrValidation()
        {
            int errorCode = ETDevice.iV_AbortCalibration();
            return (errorCode==1);
        }

        /*
        public bool Walidacja(ref string komunikat)
        {
            try
            {
                int kodBłędu = ETDevice.iV_Validate();
                komunikat = OpisBłędu(kodBłędu);
                return kodBłędu == 1;
            }
            catch (Exception exc)
            {
                komunikat = exc.Message;
                return false;
            }
        }
        */

        /*
        public bool Walidacja(
            int liczbaPunktówKalibracji, int numerMonitora, string ścieżkaPlikuObrazka,
            int rozmiarObrazka, bool zwiększonaPrędkość, bool akceptacjaWymaganaPoKażdejZmianiePołożenia,
            int jasnośćTła, ref string komunikat, IEkranKalibracji własnyEkranKalibracji)
        {
            try
            {
                int kodBłędu = ETDevice.iV_Validate();
                komunikat = OpisBłędu(kodBłędu);
                return kodBłędu == 1;
            }
            catch (Exception exc)
            {
                komunikat = exc.Message;
                return false;
            }
        }
        */

        public bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription)
        {
            try
            {
                EyeTrackingController.AccuracyStruct accuracy = new EyeTrackingController.AccuracyStruct();
                int errorCode = ETDevice.iV_GetAccuracy(ref accuracy, showCalibrationResultsWindow?1:0);
                ignoreSelectedErrors(ref errorCode, ErrorContext.GettingAccuracyDescription);
                //komunikat = OpisBłędu(kodBłędu);
                if(errorCode == 1)
                {
                    leftEyeDescription = "lewe oko dX=" + accuracy.deviationXLeft.ToString() + "°, dY=" + accuracy.deviationYLeft.ToString() + "°";
                    rightEyeDescription = "prawe oko dX=" + accuracy.deviationXRight.ToString() + "°, dY=" + accuracy.deviationYRight.ToString() + "°";
                    return true;
                }
                else
                {
                    leftEyeDescription = "pobranie informacji nie powiodło się";
                    rightEyeDescription = "---";
                    return false;
                }
            }
            catch
            {
                leftEyeDescription = "pobranie informacji nie powiodło się";
                rightEyeDescription = "---";
                return false;
            }
        }

        private static void UstawPaletęKolorówWSkaliSzarości(Bitmap image)
        {
            ColorPalette pal = image.Palette;
            for (int i = 0; i < 256; i++)
            {
                pal.Entries[i] = Color.FromArgb(i, i, i);
            }
            image.Palette = pal;
        }

        #region Zdarzenia wewnętrzne
        public bool ImagesUpdatingEnabled { get; set; }

        private void SetInternalEventCallbacks()
        {
            m_CalibrationCallback = new CalibrationCallback(CalibrationCallbackFunction);
            //m_SampleCallback = new GetSampleCallback(GetSampleCallbackFunction);
            m_EventCallback = new GetEventCallback(GetEventCallbackFunction); //tu przesyłane dane o położeniu
            m_EyeImageCallback = new GetEyeImageCallback(GetEyeImageCallbackFunction);
            m_SceneVideoCallback = new GetSceneVideoCallback(GetSceneVideoCallbackFunction);
            m_TrackingMonitorCallback = new GetTrackingMonitorCallback(GetTrackingMonitorCallbackFunction);

            ETDevice.iV_SetCalibrationCallback(m_CalibrationCallback);
            //ETDevice.iV_SetSampleCallback(m_SampleCallback);
            ETDevice.iV_SetEventCallback(m_EventCallback);
            ETDevice.iV_SetEyeImageCallback(m_EyeImageCallback);
            ETDevice.iV_SetSceneVideoCallback(m_SceneVideoCallback); //wyłączone
            ETDevice.iV_SetTrackingMonitorCallback(m_TrackingMonitorCallback);
        }

        private delegate void CalibrationCallback(EyeTrackingController.CalibrationPointStruct calibrationPointData);
        //private delegate void GetSampleCallback(EyeTrackingController.SampleStruct sampleData);
        private delegate void GetEventCallback(EyeTrackingController.EventStruct eventData);
        private delegate void GetEyeImageCallback(EyeTrackingController.ImageStruct imageData);
        private delegate void GetSceneVideoCallback(EyeTrackingController.ImageStruct imageData);
        private delegate void GetTrackingMonitorCallback(EyeTrackingController.ImageStruct imageData);

        private CalibrationCallback m_CalibrationCallback;
        //private GetSampleCallback m_SampleCallback;
        private GetEventCallback m_EventCallback;
        private GetEyeImageCallback m_EyeImageCallback;
        private GetSceneVideoCallback m_SceneVideoCallback;
        private GetTrackingMonitorCallback m_TrackingMonitorCallback;

        #region Aktualizacja obrazów
        private void GetEyeImageCallbackFunction(EyeTrackingController.ImageStruct image)
        {
            if (!ImagesUpdatingEnabled) return;
            EyeImage = new Bitmap(image.imageWidth, image.imageHeight, image.imageWidth, PixelFormat.Format8bppIndexed, image.imageBuffer);
            UstawPaletęKolorówWSkaliSzarości(EyeImage);
            OnEyeImageUpdated();
        }

        private void GetSceneVideoCallbackFunction(EyeTrackingController.ImageStruct image)
        {
            if (!ImagesUpdatingEnabled) return;
            SceneImage = new Bitmap(image.imageWidth, image.imageHeight, image.imageWidth * 3, PixelFormat.Format24bppRgb, image.imageBuffer);
            OnSceneImageUpdated();
        }

        private void GetTrackingMonitorCallbackFunction(EyeTrackingController.ImageStruct image)
        {
            if (!ImagesUpdatingEnabled) return;
            EyeTrackingImage = new Bitmap(image.imageWidth, image.imageHeight, image.imageWidth * 3, PixelFormat.Format24bppRgb, image.imageBuffer);
            OnEyeTrackingImageUpdated();
        }

        public Bitmap EyeImage { get; private set; } //można pobrać niezależnie od zdarzenia
        //public delegate void ZmienionyObrazEventHandler(Bitmap obrazOczu);
        public event ImageUpdatedEventHandler EyeImageUpdated;
        private void OnEyeImageUpdated()
        {
            if (EyeImageUpdated != null) EyeImageUpdated(EyeImage);
        }

        public Bitmap SceneImage { get; private set; } //można pobrać niezależnie od zdarzenia
        public event ImageUpdatedEventHandler SceneImageUpdated;
        private void OnSceneImageUpdated()
        {
            if (SceneImageUpdated != null) SceneImageUpdated(SceneImage);
        }

        public Bitmap EyeTrackingImage { get; private set; } //można pobrać niezależnie od zdarzenia
        public event ImageUpdatedEventHandler EyeTrackingImageUpdated;
        private void OnEyeTrackingImageUpdated()
        {
            if (EyeTrackingImageUpdated != null) EyeTrackingImageUpdated(EyeTrackingImage);
        }
        #endregion

        #region Aktualizacja danych

        public static CalibrationPoint KonwertujDoPunktuKalibracji(EyeTrackingController.CalibrationPointStruct calibrationPoint)
        {
            return new CalibrationPoint() { Index = calibrationPoint.number, Position = new Point(calibrationPoint.positionx, calibrationPoint.positiony) };
        }

        private void CalibrationCallbackFunction(EyeTrackingController.CalibrationPointStruct calibrationPoint)
        {
            try
            {
                if (customCalibrationScreen != null)
                {
                    if (calibrationPoint.number > -1) customCalibrationScreen.CalibrationPoint = KonwertujDoPunktuKalibracji(calibrationPoint);
                    //if (calibrationPoint.number == -1) własnyEkranKalibracji.Zamknij(); //zakończenie
                    if (calibrationPoint.number == -1) OnZakończonaKalibracjaLubWalidacja(true);
                }
            }
            //catch(Exception exc)
            catch
            {
                throw;
            }
        }

        public event CalibrationOrValidationFinishedEventHandler CalibrationOrValidationFinished;
        private void OnZakończonaKalibracjaLubWalidacja(bool result)
        {
            if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(result);
        }

        //!!!!!!!!!!!!!!!DateTime updateTimeLeftEye1 = DateTime.Now, updateTimeRightEye1 = DateTime.Now, updateTimeLeftEye2 = DateTime.Now, updateTimeRightEye2 = DateTime.Now, updateTimeLeftEye3 = DateTime.Now, updateTimeRightEye3 = DateTime.Now;
        int numberOfLeftEyeSamplesInARow = 0, numberOfRightEyeSamplesInARow = 0;
        public bool LeftEyeDetected { get; private set; }
        public bool RightEyeDetected { get; private set; }

        public PointF LeftEyeOffset { get; set; } //aplikacja może to użyć lub nie
        public PointF RightEyeOffset { get; set; }
        public PointF AveragedEyeOffset { get; set; }


        private void GetEventCallbackFunction(EyeTrackingController.EventStruct eventData)
        {
            //Punkt pozycjaOka = new Punkt((int)eventData.positionX, (int)eventData.positionY);
            PointF eyePositionF = new PointF((float)eventData.positionX, (float)eventData.positionY);
            float leftEyePupilSize, rightEyePupilSize;
            getEyesPupilSizes(out leftEyePupilSize, out rightEyePupilSize);
            switch(eventData.eye)
            {
                case 'l':
                    numberOfLeftEyeSamplesInARow++;
                    numberOfRightEyeSamplesInARow = 0;
                    leftEyeData.EyeSide = EyeSide.LeftEye;
                    //daneLewegoOka.Pozycja = pozycjaOka;
                    leftEyeData.PositionF = eyePositionF;
                    leftEyeData.PupilSize = leftEyePupilSize;
                    OnLeftEyeDataUpdate();
                    break;
                case 'r':
                    numberOfRightEyeSamplesInARow++;
                    numberOfLeftEyeSamplesInARow = 0;
                    rightEyeData.EyeSide = EyeSide.RightEye;
                    //danePrawegoOka.Pozycja = pozycjaOka;
                    rightEyeData.PositionF = eyePositionF;
                    rightEyeData.PupilSize = rightEyePupilSize;
                    OnRightEyeDataUpdate();
                    break;
            }
            LeftEyeDetected = !(numberOfRightEyeSamplesInARow > 3);
            RightEyeDetected = !(numberOfLeftEyeSamplesInARow > 3);
            averagedEyeData.EyeSide = EyeSide.AveragedOrBestEye;
            //daneUśrednionegoOka.Pozycja = new Punkt((DaneLewegoOka.Pozycja.X + DanePrawegoOka.Pozycja.X)/2, (DaneLewegoOka.Pozycja.Y + DanePrawegoOka.Pozycja.Y)/2);
            averagedEyeData.PositionF = new PointF((LeftEyeData.PositionF.X + RightEyeData.PositionF.X) / 2f, (LeftEyeData.PositionF.Y + RightEyeData.PositionF.Y) / 2f);
            averagedEyeData.PupilSize = (leftEyePupilSize + rightEyePupilSize) / 2f;
            OnAverageDataUpdate();
        }

        /*
        public enum StronaOka { LeweOko, PraweOko, UśrednioneOko };

        public struct DaneOka
        {
            public StronaOka StronaOka;
            public Point Pozycja;
            public PointF PozycjaF;
        }
        */

        private EyeDataSample leftEyeData = new EyeDataSample() { EyeSide = EyeSide.LeftEye, PositionF = new PointF(-1, -1), PupilSize = -1, OffsetCorrection = PointF.Zero };
        private EyeDataSample rightEyeData = new EyeDataSample() { EyeSide = EyeSide.RightEye, PositionF = new PointF(-1, -1), PupilSize = -1, OffsetCorrection = PointF.Zero };
        private EyeDataSample averagedEyeData = new EyeDataSample() { EyeSide = EyeSide.AveragedOrBestEye, PositionF = new PointF(-1, -1), PupilSize = -1, OffsetCorrection = PointF.Zero };
        public EyeDataSample LeftEyeData
        {
            get
            {
                return leftEyeData;
            }
        }
        public EyeDataSample RightEyeData
        {
            get
            {
                return rightEyeData;
            }
        }
        public EyeDataSample AveragedEyeData
        {
            get
            {
                return averagedEyeData;
            }
        }
        //public delegate void ZmienioneDaneOkaEventHandler(DaneOka daneOka);
        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;
        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;
        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;
        private void OnLeftEyeDataUpdate()
        {
            if (LeftEyeDataUpdated != null) LeftEyeDataUpdated(LeftEyeData);
        }
        private void OnRightEyeDataUpdate()
        {
            if (RightEyeDataUpdated != null) RightEyeDataUpdated(RightEyeData);
        }
        private void OnAverageDataUpdate()
        {
            if (AveragedEyeDataUpdated != null) AveragedEyeDataUpdated(AveragedEyeData);
        }
        #endregion
        #endregion

        #region Rejestrowanie danych
        public bool IsDataRecordingPossible
        {
            get
            {
                return true;
            }
        }

        public bool SetMarker(string markerText, ref string message)
        {
            try
            {
                int errorCode = ETDevice.iV_SendImageMessage(new StringBuilder(markerText)); //jeżeli tekst to nazwa pliku - separacja danych
                ignoreSelectedErrors(ref errorCode, ErrorContext.SettingMarker);
                message = ErrorDescription(errorCode);
                return errorCode == 1;
            }
            catch (Exception exc)
            {
                message = exc.Message;
                return false;
            }
        }

        //w wersji 2.0 nie ma string.IsNullOrWhiteSpace
        private bool string_IsNullOrWhiteSpace(string text)
        {
            return string.IsNullOrEmpty(text.Trim());
        }

        public bool SaveData(string filePath, string subjectName, string desciption, ref string message)
        {
            //UWAGA! Zrobić też wersję async-await??
            try
            {
                bool czyNadpisywać = true;
                //to działa synchronicznie
                if (string_IsNullOrWhiteSpace(subjectName)) subjectName = "Brak nazwy badanego";
                if (string_IsNullOrWhiteSpace(desciption)) desciption = "Brak opisu badania";
                int errorCode = ETDevice.iV_SaveData(new StringBuilder(filePath), new StringBuilder(desciption), new StringBuilder(subjectName), czyNadpisywać ? 1 : 0);
                ignoreSelectedErrors(ref errorCode, ErrorContext.SavingData);
                message = ErrorDescription(errorCode);
                return errorCode == 1;
            }
            catch (Exception exc)
            {
                message = exc.Message;
                return false;
            }
        }

        public bool DataRecordingEnabled { get; private set; }

        public bool BeginDataRecording(ref string message)
        {
            try
            {                
                //ETDevice.iV_ClearRecordingBuffer();
                int errorCode = ETDevice.iV_StartRecording();
                ignoreSelectedErrors(ref errorCode, ErrorContext.DataRegisteringStarting);
                message = ErrorDescription(errorCode);
                DataRecordingEnabled = true;
                return errorCode == 1;
            }
            catch (Exception exc)
            {
                message = exc.Message;
                return false;
            }
        }

        public bool EndDataRecording(ref string message)
        {
            try
            {
                if (!DataRecordingEnabled) return true;

                int errorCode = ETDevice.iV_StopRecording();
                ignoreSelectedErrors(ref errorCode, ErrorContext.DataRegisteringFinishing);
                message = ErrorDescription(errorCode);
                DataRecordingEnabled = false;
                return errorCode == 1;
            }
            catch (Exception exc)
            {
                message = exc.Message;
                return false;
            }
        }

        public const bool IgnoreSelectedErrors = true;
        private enum ErrorContext { Connecting, Disconnecting, CalibrationOrValidation, GettingAccuracyDescription, SettingMarker, SavingData, DataRegisteringStarting, DataRegisteringFinishing };

        private void ignoreSelectedErrors(ref int errorCode, ErrorContext context)
        {
            if (!IgnoreSelectedErrors) return;
            //if (kodBłędu == 131 && (kontekst== KontekstWystąpieniaBłędu.RozpocznijRejestrowanieDanych || kontekst== KontekstWystąpieniaBłędu.ZatrzymajRejestrowanieDanych || kontekst== KontekstWystąpieniaBłędu.ZdalnyZapisDanych))
            if (errorCode == 131)
            {
                errorCode = 1;
            }
        }
        #endregion

        #region Pobieranie rozmiaru źrenicy
        private void getEyesPupilSizes(out float leftEyePupilSize, out float rightEyePupilSize)
        {
            //pobieram dane o pozycji oka sfilmowanej przez kamerę eyetrackera
            EyeTrackingController.SampleStruct rawDataSample = new EyeTrackingController.SampleStruct();
            ETDevice.iV_GetSample(ref rawDataSample);
            leftEyePupilSize = (float)rawDataSample.leftEye.diam;
            rightEyePupilSize = (float)rawDataSample.rightEye.diam;
        }
        #endregion
    }
}
