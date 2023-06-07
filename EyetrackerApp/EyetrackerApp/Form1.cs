using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tobii.Interaction;

namespace EyetrackerApp
{
    public partial class Form1 : Form
    {
        private NamedPipeServerStream pipeServer;
        private StreamWriter pipeWriter;
        private Host eyeTrackerHost;
        private bool isEyeTrackerConnected = false;
        private bool isPipeConnected = false;
        private string gazePointData = "Brak danych";
        private DateTime lastGazeUpdateTime = DateTime.Now;
        private const int sleepThresholdInMinutes = 3;
        private bool isUserAsleep = false;
        private string alarmStart="00:00";
        private string alarmEnd="06:00";

        public Form1()
        {
            InitializeComponent();
            // Inicjalizacja przycisków i etykiet
            ConnectButton.Text = "Połącz Pipe";
            ConnectEyeTrackerButton.Text = "Połącz EyeTracker";
            PipeStatusLabel.Text = "Niepołączony";
            EyeTrackerStatusLabel.Text = "Niepołączony";
            GazePointLabel.Text = "Brak danych";
            SleepStatusLabel.Text = "";

            ConnectButton.Enabled = false;

            // Uruchom wątek do sprawdzania, czy użytkownik zasnął
            Thread checkSleepThread = new Thread(new ThreadStart(CheckSleepStatusLoop));
            checkSleepThread.Start();
            for (int i = 0; i < 24; i++)
            {
                string hour = i.ToString("D2") + ":00";
                AlarmStartComboBox.Items.Add(hour);
                AlarmEndComboBox.Items.Add(hour);
            }
            AlarmStartComboBox.SelectedIndex = 0;
            AlarmEndComboBox.SelectedIndex=6;
            AlarmStartComboBox.SelectedIndexChanged += AlarmStartComboBox_SelectedIndexChanged;
            AlarmEndComboBox.SelectedIndexChanged += AlarmEndComboBox_SelectedIndexChanged;
        }
        private void AlarmStartComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            alarmStart = AlarmStartComboBox.SelectedItem.ToString();
            if (isPipeConnected)
            {
                pipeWriter.WriteLine("Zmieniono godzinę alarmu. Nowa godzina rozpoczęcia: " + alarmStart);
                pipeWriter.Flush();
            }
        }

        private void AlarmEndComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            alarmEnd = AlarmEndComboBox.SelectedItem.ToString();
            if (isPipeConnected)
            {
                pipeWriter.WriteLine("Zmieniono godzinę alarmu. Nowa godzina zakończenia: " + alarmEnd);
                pipeWriter.Flush();
            }
        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            ConnectPipe();
        }

        private void ConnectEyeTrackerButton_Click(object sender, EventArgs e)
        {
            ConnectEyeTrackerButton.Enabled = false;
            ConnectEyeTracker();
        }

        private void ConnectEyeTracker()
        {
            try
            {
                eyeTrackerHost = new Host();

                // Ustalamy obserwatora punktów spojrzenia
                var gazePointDataStream = eyeTrackerHost.Streams.CreateGazePointDataStream();

                gazePointDataStream.GazePoint((gazePointX, gazePointY, _) =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {

                        gazePointData = $"X: {gazePointX}, Y: {gazePointY}";
                        GazePointLabel.Text = gazePointData;
                        lastGazeUpdateTime = DateTime.Now;
                        if (isUserAsleep)
                        {
                            SleepStatusLabel.Text = "Użytkownik obudził się";

                            TimeSpan now = DateTime.Now.TimeOfDay;
                            TimeSpan start = TimeSpan.Parse(alarmStart);
                            TimeSpan end = TimeSpan.Parse(alarmEnd);
                            isUserAsleep = false;
                            if ((now > start && now < end)&& isPipeConnected)
                            {
                                // Jeżeli tak, wysyłamy wiadomość "Alarm"
                                if (isPipeConnected)
                                {
                                    SleepStatusLabel.Text = "Alarm";
                                    pipeWriter.WriteLine("A");
                                    pipeWriter.Flush();
                                }
                            }

                            else if (isPipeConnected)
                            {
                                pipeWriter.WriteLine("W");
                                pipeWriter.Flush();
                            }
                        }
                    });
                });

                isEyeTrackerConnected = true;

                EyeTrackerStatusLabel.Text = "Połączony";
                ConnectButton.Enabled = true;
            }
            catch (Exception ex)
            {
                isEyeTrackerConnected = false;

                EyeTrackerStatusLabel.Text = $"Niepołączony: {ex.Message}";
                ConnectEyeTrackerButton.Enabled = true;
            }
        }

        private async void ConnectPipe()
        {
            try
            {
                pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                await Task.Factory.FromAsync(pipeServer.BeginWaitForConnection, pipeServer.EndWaitForConnection, null);

                pipeWriter = new StreamWriter(pipeServer);

                PipeStatusLabel.Text = "Połączony";
                isPipeConnected = true;

                // Uruchom wątek do wysyłania danych
                Thread dataThread = new Thread(new ThreadStart(DataSendingLoop));
                dataThread.Start();
            }
            catch (Exception ex)
            {
                PipeStatusLabel.Text = $"Niepołączony: {ex.Message}";
                ConnectButton.Enabled = isEyeTrackerConnected;
            }
        }

        private void DataSendingLoop()
        {
            while (isPipeConnected)
            {
                //pipeWriter.WriteLine(gazePointData);
                //pipeWriter.Flush();
                Thread.Sleep(100);
            }
        }

        private void CheckSleepStatusLoop()
        {
            while (true)
            {
                if ((DateTime.Now - lastGazeUpdateTime).TotalSeconds >= sleepThresholdInMinutes && !isUserAsleep)
                {
                    isUserAsleep = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        SleepStatusLabel.Text = "Użytkownik śpi";
                        if (isPipeConnected)
                        {
                            pipeWriter.WriteLine("S");
                            pipeWriter.Flush();
                        }
                    });
                }
                else if (isUserAsleep && (DateTime.Now - lastGazeUpdateTime).TotalSeconds < sleepThresholdInMinutes)
                {
                    isUserAsleep = false;
                    this.Invoke((MethodInvoker)delegate
                    {

                        // Sprawdź czy obudzenie nastąpiło w trakcie ustawionego alarmu
                        TimeSpan now = DateTime.Now.TimeOfDay;
                        TimeSpan start = TimeSpan.Parse(alarmStart);
                        TimeSpan end = TimeSpan.Parse(alarmEnd);
                        
                        if (now > start && now < end)
                        {
                            // Jeżeli tak, wysyłamy wiadomość "Alarm"
                            if (isPipeConnected)
                            {
                                SleepStatusLabel.Text = "Alarm";
                                pipeWriter.WriteLine("Alarm");
                                pipeWriter.Flush();
                            }
                        }
                        else
                        {
                             SleepStatusLabel.Text = "Użytkownik obudził się";

                            // Jeżeli nie, wysyłamy wiadomość "Użytkownik obudził się"
                            if (isPipeConnected)
                            {
                                pipeWriter.WriteLine("Użytkownik obudził się");
                                pipeWriter.Flush();
                            }
                        }
                    });
                }
                Thread.Sleep(1000);
            }
        }

    }
}