using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Size = System.Drawing.Size;

namespace GameLab.Eyetracking.EyetrackerControls
{
    using GameLab.Geometry;
    //using static System.Collections.Specialized.BitVector32;

    public partial class CustomCalibrationForm : Form, ICustomCalibrationScreen
    {
        private const int minImageSize = 50;
        private TimeSpan imageShrinkingTime;
        private int imageInitialSize;
        private int imageResizeStep;
        private bool imageShrinkingEnabled = false;
        private System.Timers.Timer imageShrinkingTimer = new System.Timers.Timer(100);
        private CalibrationPoint currentCalibrationPoint;
        protected int numberOfCalibrationPoints = -1;

        public Geometry.Size ScreenSize { get { return new Geometry.Size(this.Width, this.Height); } }
        public int NumberOfCalibrationPointsShown { get; private set; }
        protected Image DefaultImage;

        public CalibrationPoint CalibrationPoint //to jest sposób kontroli tej formy - przes ustawianie punktu kalibracji
        { 
            set 
            { 
                currentCalibrationPoint = value;

                lbDebug.Text = "Debug: " + currentCalibrationPoint.Index + ". (" + currentCalibrationPoint.Position.X.ToString() + ", " + currentCalibrationPoint.Position.Y.ToString() + "), number of calibration points shown: " + NumberOfCalibrationPointsShown.ToString();
                lbDebug.Refresh();

                updateImage();

                NumberOfCalibrationPointsShown++;
            } 
        }

        //dla designera
        public CustomCalibrationForm()
        {
            InitializeComponent();
        }

        /*
        public new void ShowDialog() //kombinacja, żeby w ICustomCalibrationForm nie było referencji do System.Windows.Forms
        {
            base.Hide();
            base.ShowDialog();
        }
        */

        public CustomCalibrationForm(CustomCalibrationSettings customCalibrationSettings)
        {
            InitializeComponent();
            DefaultImage = pictureBox.Image;

            this.BackColor = customCalibrationSettings.BackgroundColor;
            this.imageShrinkingEnabled = customCalibrationSettings.ImageShrinkingEnabled;
            this.imageShrinkingTime = customCalibrationSettings.ImageShrinkingTime;            

            NumberOfCalibrationPointsShown = 0;
            Settings = EyetrackerCalibrationSettings.Default;

            imageShrinkingTimer.Elapsed += imageShrinkingTimer_Elapsed;

            //ustawienia pozwalające uniknąć mrugania obszarów
            //nie używam, bo tylko kontolki
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        //stopniowe zmniejszanie rysunku
        private void imageShrinkingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    updatePictureBoxSize();
                });
            }
            else
            {
                updatePictureBoxSize();
            }
        }

        public EyetrackerCalibrationSettings Settings//(string imageFilePath , int sizeImage, int backgroundBrightness, int animationTime)
        {
            set
            {
                numberOfCalibrationPoints = value.NumberOfCalibrationPoints;

                //ignorowane
                //BackColor = Color.FromArgb(value.BackgroundBrightness, value.BackgroundBrightness, value.BackgroundBrightness);

                if (!string.IsNullOrEmpty(value.ImageFilePath))
                {
                    pictureBox.Image = new Bitmap(value.ImageFilePath);
                }

                pictureBox.Width = value.ImageSize;
                pictureBox.Height = value.ImageSize;

                //do zmiany rysunku
                imageInitialSize = value.ImageSize;
                imageResizeStep = (int)((imageInitialSize - minImageSize) / (imageShrinkingTime.TotalMilliseconds / 100)); //TODO: przetestować
            }
        }

        private void updatePictureBoxSize()
        {
            pictureBox.Width -= imageResizeStep;
            pictureBox.Height -= imageResizeStep;
            pictureBox.Update();
            pictureBox.Refresh();
        }

        protected virtual void updateImage()
        {
            //tu tylko zmiana pozycji, więcej w klasie potomnej
            imageShrinkingTimer.Stop();
            pictureBox.Location = new System.Drawing.Point(currentCalibrationPoint.Position.X - (pictureBox.Size.Width/2), currentCalibrationPoint.Position.Y - (pictureBox.Size.Width / 2));
            pictureBox.Width = imageInitialSize; //zakładamy kształt kwadratu
            pictureBox.Height= imageInitialSize;            
            pictureBox.Update();
            //pictureBox.Refresh();            
            if(imageShrinkingEnabled) imageShrinkingTimer.Start();
            /*
            if (imageShrinkingEnabled)
            {
                new System.Threading.Thread(
                    () =>
                    {
                        imageShrinkingTimer.Start();
                        System.Threading.Thread.Sleep(3000);
                    }).Start();
            }
            */
        }  
    }
}
