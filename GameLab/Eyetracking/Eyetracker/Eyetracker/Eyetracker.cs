using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GameLab.Eyetracking
{
    using Bitmap = System.Drawing.Bitmap; //warto byłoby mieć własny typ
    using GameLab.Geometry;

    public delegate void ImageUpdatedEventHandler(Bitmap eyeImage);
    public enum EyeSide { LeftEye, RightEye, AveragedOrBestEye };
    public struct EyeDataSample
    {
        private PointF positionF;
        private Point position; //przechowywane, żeby uniknąć ciągłego obliczania we własności tylko do odczytu        

        public EyeSide EyeSide;
        public Point Position
        {
            get
            {
                return position;
            }
        }
        public PointF PositionF
        {
            get
            {
                return positionF;
            }
            set
            {
                positionF = value;
                position = positionF.ToPoint();
            }
        }
        public float PupilSize; //rozmiar źrenicy

        public PointF OffsetCorrection { get; set; }

        public PointF PositionWithOffsetCorrection
        {
            get
            {
                return PositionF + OffsetCorrection;
            }
        }

        public override string ToString()
        {
            return "eye side: " + EyeSide + ", position: " + PositionF.ToString() + ", pupil size: " + PupilSize.ToString();
        }
    }

    public delegate void EyeDataUpdatedEventHandler(EyeDataSample eyeData);    

    public class EyetrackerConnectionSettings
    {
        //remote, send
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        
        //local, receive
        public string ClientIp { get; set; }
        public int ClientPort { get; set; }

        public EyetrackerConnectionSettings()
        {
            ServerIp = "127.0.0.1";
            ServerPort = 0;
        
            ClientIp = "127.0.0.1";
            ClientPort = 0;
        }

        public static EyetrackerConnectionSettings Default
        {
            get
            {
                return new EyetrackerConnectionSettings();
            }
        }
    }

    public class EyetrackerConnectionSettingsTypeConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true; //wyświetlanie "+" przed własnością
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(EyetrackerConnectionSettings));
        }
    }

    public enum EyetrackerType : int { ImitationEyetracker = 0, SMIEyetracker, MirametrixEyetracker, TheEyeTribeEyetracker, TobiiEyetracker, GazeDataReplay, ZeroEyetracker };    

    //TODO: rozdzielić eyetrackery dwuoczne i po prostu dające punkt spojrzenia
    public interface IEyetracker
    {
        string Name { get; }

        bool Connected { get; }        

        //bool Connect(string ipServer, int portServer, string ipClient, int portClient, ref string message);
        bool Connect(EyetrackerConnectionSettings settings, ref string message);
        bool Disconnect(ref string message);

        bool LeftEyeDetected { get; } //może też oznaczać zamknięte oko
        bool RightEyeDetected { get; }

        //dane
        EyeDataSample LeftEyeData { get; }
        EyeDataSample RightEyeData { get; }
        EyeDataSample AveragedEyeData { get; }
        event EyeDataUpdatedEventHandler LeftEyeDataUpdated;
        event EyeDataUpdatedEventHandler RightEyeDataUpdated;
        event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        //dodatkowe ustawienia (aplikacje mogą ich używać lub nie)
        PointF LeftEyeOffset { get; set; } //stałe przesunięcie kontrolowane przez użytkownika
        PointF RightEyeOffset { get; set; }
        PointF AveragedEyeOffset { get; set; }
    }

    public interface IEyeCamera
    {
        //obrazy
        bool ImagesUpdatingEnabled { get; set; }

        Bitmap EyeImage { get; }
        event ImageUpdatedEventHandler EyeImageUpdated;

        Bitmap SceneImage { get; } //można pobrać niezależnie od zdarzenia
        event ImageUpdatedEventHandler SceneImageUpdated;

        Bitmap EyeTrackingImage { get; } //można pobrać niezależnie od zdarzenia
        event ImageUpdatedEventHandler EyeTrackingImageUpdated;
    }

    public interface IDataRecordingDevice
    {
        bool Connected { get; }

        //rejestracja danych i znaczniki
        bool IsDataRecordingPossible { get; } //czy możliwa rejestracja danych przez sam eyetracker
        bool DataRecordingEnabled { get; } //trwa rejestracja danych
        bool BeginDataRecording(ref string message);
        bool EndDataRecording(ref string message);
        bool SetMarker(string markerText, ref string message); //ustaw znacznik w nagrywanych danych
        bool SaveData(string filePath, string subjectName, string description, ref string message); //zapisz dane zdalnie
    }
}
