using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

//using GameLab.Geometry;
using GameLab.Eyetracking;

namespace GameLab.Eyetracking.EyetrackerControls
{
    public enum EyetrackerReadingMode { SubscribeUpdateEvents, ReadingProperties }

    public partial class ReadingGazePositionForm : Form
    {
        IEyetracker et;
        IEyeCamera ec;
        GazeSmoothingFilter filter;
        IGazeRuntimeAnalyser analyser;
        EyetrackerReadingMode mode;
        List<string> log = new List<string>();

        bool stopUpdateTask = false;

        public ReadingGazePositionForm(IEyetracker et, GazeSmoothingFilter filter, IGazeRuntimeAnalyser analyser, EyetrackerReadingMode mode)
        {
            this.et = et;
            this.ec = et as IEyeCamera;
            this.filter = filter;
            this.analyser = analyser;
            this.mode = mode;
            if(ec != null) ec.ImagesUpdatingEnabled = true;

            InitializeComponent();

            //ustawienia pozwalające uniknąć mrugania obszarów
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            

            lbEyetrackerName.Text += et.Name;
            
            panelDebug.BackColor = Color.FromArgb(30, panelDebug.BackColor);
            pbEyetrackerImage.BackColor = panelDebug.BackColor;

            if (!et.Connected)
            {
                log.Add("Eyetracker is not connected");
                return;
            }

            if (mode == EyetrackerReadingMode.SubscribeUpdateEvents)
            {
                log.Add("Start");
                //czytanie danych o położeniu
                et.LeftEyeDataUpdated += et_LeftEyeDataUpdated;
                et.RightEyeDataUpdated += et_RightEyeDataUpdated;
                et.AveragedEyeDataUpdated += et_AveragedEyeDataUpdated;

                //analiza od razu w trakcie zbierania danych
                analyser.LeftEyeStateChanged += analyser_LeftEyeStateChanged;
                analyser.RightEyeStateChanged += analyser_RightEyeStateChanged;
                analyser.AveragedEyeStateChanged += analyser_AveragedEyeStateChanged;
            }

            if(mode == EyetrackerReadingMode.ReadingProperties)
            {
                log.Add("Start");
                Action updateAction = 
                    () =>
                        {
                            Action _updatePositionInformation = new Action(() => { updatePositionInformation(); });
                            Action _updateAnalyserInformation = new Action(() => { updateAnalyserInformation(); });
                                
                            while (true)
                            {
                                if (stopUpdateTask) break;

                                //averagedEyePosition = et.AveragedEyeData.PositionF;
                                //leftEyePosition = et.LeftEyeData.PositionF;
                                //rightEyePosition = et.RightEyeData.PositionF;
                                averagedEyePosition = et.AveragedEyeData.PositionWithOffsetCorrection;
                                leftEyePosition = et.LeftEyeData.PositionWithOffsetCorrection;
                                rightEyePosition = et.RightEyeData.PositionWithOffsetCorrection;

                                averagedEyeState = analyser.AveragedEyeState;
                                leftEyeState = analyser.LeftEyeState;
                                rightEyeState = analyser.RightEyeState;

                                try
                                {
                                    this.Invoke(_updatePositionInformation);
                                    this.Invoke(_updateAnalyserInformation);
                                }
                                catch
                                {
                                    //wyjątek zgłaszany dopóki okno nie zostanie utworzone
                                }

                                System.Threading.Thread.Sleep(10);
                            }
                        };                
                System.Threading.Tasks.Task updateTask = System.Threading.Tasks.Task.Run(updateAction);
            }
        }        

        private GameLab.Geometry.PointF leftEyePosition, rightEyePosition, averagedEyePosition;

        //private void updateEyetracker(float x, float y)
        private void updatePositionInformation()
        {
            if (panelDebug.Visible)
            {
                lbAverEyePosition.Text = averagedEyePosition.ToString();
                lbLeftEyePosition.Text = leftEyePosition.ToString();
                lbRightEyePosition.Text = rightEyePosition.ToString();
                lbEyeClosed.Text = "Eyes detected: left " + (et.LeftEyeDetected ? "yes" : "no") + ", right - " + (et.RightEyeDetected ? "yes" : "no");

                if (ec != null && ec.ImagesUpdatingEnabled && ec.EyeImage != null)
                {
                    /*
                    Bitmap bmp = (Bitmap)et.EyeImage.Clone();
                    bmp.MakeTransparent(Color.White);
                    pbEyetrackerImage.Image = bmp;
                    */
                    try
                    {
                        pbEyetrackerImage.Image = (Image)ec.EyeImage.Clone();
                    }
                    catch //InvalidOperationException
                    {
                        //problem z dostępem bitmapy z wielu wątków jednocześnie (przez zdalny pulpit, gdy odświeżanie jest dłuższe)
                    }
                }
            }

            //Refresh();
            Invalidate();
        }

        //private delegate void UpdateEyetrackerDelegate(float x, float y);
        private delegate void UpdateEyetrackerInformationDelegate();

        void et_AveragedEyeDataUpdated(EyeDataSample eyeData)
        {
            //averagedEyePosition = eyeData.PositionF;
            averagedEyePosition = eyeData.PositionWithOffsetCorrection;

            if (filter != null) eyeData = filter.SmoothData(eyeData);
            UpdateEyetrackerInformationDelegate updateMethod = this.updatePositionInformation;

            //to uruchamiane jest z osobnego wątka tworzonego przez ET
            if (this.InvokeRequired)
            {
                //TODO: przy zamykaniu formy zgłasz wyjątek że MainForm jest usunięty
                //bo aktualizacja napisów na ekranie
                try
                {
                    //this.Invoke(updateMethod, new object[] { eyeData.PositionF.X, eyeData.PositionF.Y });
                    this.Invoke(updateMethod);
                }
                catch
                { }
            }
            //else updateMethod(eyeData.PositionF.X, eyeData.PositionF.Y);
            else updateMethod();
        }

        void et_LeftEyeDataUpdated(EyeDataSample eyeData)
        {
            //leftEyePosition = eyeData.PositionF;
            leftEyePosition = eyeData.PositionWithOffsetCorrection;
        }

        void et_RightEyeDataUpdated(EyeDataSample eyeData)
        {
            //rightEyePosition = eyeData.PositionF;
            rightEyePosition = eyeData.PositionWithOffsetCorrection;
        }

        #region Analyser
        private void updateAnalyserInformation()
        {
            if (panelDebug.Visible)
            {
                lbEyesState.Text = "Gaze event: left - ";
                if (leftEyeState != null) lbEyesState.Text += leftEyeState.CurrentEvent.ToString();
                if (leftEyeState != null && rightEyeState != null) lbEyesState.Text += ", ";
                if (rightEyeState != null) lbEyesState.Text += "right - " + rightEyeState.CurrentEvent.ToString();
                if (averagedEyeState != null) lbEyesState2.Text = "averaged - " + averagedEyeState.CurrentEvent.ToString();
                //lbEyesState.Text = (analyser as RuntimeAnalysis_Imitation).opis;
            }

            //Refresh();
            Invalidate();
        }

        private delegate void UpdateAnalyserDelegate();

        private EyeState leftEyeState, rightEyeState, averagedEyeState;

        private void analyser_EyeStateChanged()
        {
            UpdateAnalyserDelegate updateMethod = this.updateAnalyserInformation;

            //to uruchamiane jest z osobnego wątka tworzonego przez ET
            if (this.InvokeRequired)
            {
                this.Invoke(updateMethod);
            }
            else updateMethod();
        }

        void analyser_LeftEyeStateChanged(EyeState leftEyeState)
        {
            this.leftEyeState = leftEyeState;
            analyser_EyeStateChanged();
        }

        void analyser_RightEyeStateChanged(EyeState rightEyeState)
        {
            this.rightEyeState = rightEyeState;
            analyser_EyeStateChanged();
        }

        void analyser_AveragedEyeStateChanged(EyeState averagedEyeState)
        {
            this.averagedEyeState = averagedEyeState;
            analyser_EyeStateChanged();
        }
        #endregion

        private const float drawedPointSize = 10;

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //kursor - averaged eye position
            RectangleF rectAveragedEye = new RectangleF(averagedEyePosition.X - drawedPointSize / 2, averagedEyePosition.Y - drawedPointSize / 2, drawedPointSize, drawedPointSize);
            g.FillEllipse(Brushes.Navy, rectAveragedEye);
            g.DrawEllipse(Pens.Black, rectAveragedEye);

            //kursor - left eye position
            //if (leftEyeState != null)
            //if (leftEyePosition.X != 0 && leftEyePosition.Y != 0)
            {
                //leftEyePosition = leftEyeState.StartPosition;
                RectangleF rectLeftEye = new RectangleF(leftEyePosition.X - drawedPointSize / 2, leftEyePosition.Y - drawedPointSize / 2, drawedPointSize, drawedPointSize);
                g.FillEllipse(Brushes.Gray, rectLeftEye);
                g.DrawEllipse(Pens.Black, rectLeftEye);
            }

            //kursor - right eye position
            //if (rightEyeState != null)
            //if(rightEyePosition.X != 0 && rightEyePosition.Y != 0)
            {
                //rightEyePosition = rightEyeState.StartPosition;
                RectangleF rectRightEye = new RectangleF(rightEyePosition.X - drawedPointSize / 2, rightEyePosition.Y - drawedPointSize / 2, drawedPointSize, drawedPointSize);
                g.FillEllipse(Brushes.Gray, rectRightEye);
                g.DrawEllipse(Pens.Black, rectRightEye);
            }

            //kursor - smoothed eye position
            GameLab.Geometry.PointF smoothedAveragedEyePosition = filter.SmoothData(et.AveragedEyeData).PositionF;
            RectangleF rectSmoothedAveragedEye = new RectangleF(smoothedAveragedEyePosition.X - drawedPointSize / 2, smoothedAveragedEyePosition.Y - drawedPointSize / 2, drawedPointSize, drawedPointSize);
            g.FillEllipse(Brushes.Blue, rectSmoothedAveragedEye);
            g.DrawEllipse(Pens.Black, rectSmoothedAveragedEye);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mode == EyetrackerReadingMode.SubscribeUpdateEvents)
            {
                et.AveragedEyeDataUpdated -= et_AveragedEyeDataUpdated;
                analyser.LeftEyeStateChanged -= analyser_LeftEyeStateChanged;
                analyser.RightEyeStateChanged -= analyser_RightEyeStateChanged;
                analyser.AveragedEyeStateChanged -= analyser_AveragedEyeStateChanged;
            }

            if(mode == EyetrackerReadingMode.ReadingProperties)
            {
                stopUpdateTask = true;
            }

            /*
            string komunikat = "";
            if (!et.Disconnect(ref komunikat))
            {
                MessageBox.Show("Rozłączenie od serwera eyetrackera nie powiodło się:\n" + komunikat, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Add(komunikat);
            }
            */
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
