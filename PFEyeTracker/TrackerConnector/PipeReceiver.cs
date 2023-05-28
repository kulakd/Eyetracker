using Microsoft.Maui.Dispatching;
using System.IO.Pipes;
using System.Reflection.PortableExecutable;

namespace TrackerConnector
{
    public enum TrackerEventType
    {
        WAKE_UP,
        ALARM,
        SLEEP
    }

    public class PipeReceiver
    {
        IDispatcher dispatcher = Dispatcher.GetForCurrentThread();
        private NamedPipeClientStream pipeClient;
        private StreamReader pipeReader;

        public event EventHandler<TrackerEventType> TrackerEvent;

        private void DispatchEvent(Action a)
        {
            if (dispatcher != null && dispatcher.IsDispatchRequired)
                dispatcher.Dispatch(a);
            else
                a();
        }

        private async void ConnectPipe()
        {
            //try
            //{
            pipeClient = new NamedPipeClientStream(".", "trackerEventPipe", PipeDirection.InOut, PipeOptions.Asynchronous);
            await pipeClient.ConnectAsync();
            pipeReader = new StreamReader(pipeClient);

            // Start reading messages from pipe in a separate Task
            Task.Run(() => StartReading());
            //}
            //catch (Exception ex)
            //{
            //    GazePointLabel.Text = $"Error: {ex.Message}";
            //}
        }

        public async Task StartReading()
        {
            try
            {
                while (pipeClient.IsConnected)
                {
                    string line = await pipeReader.ReadLineAsync();
                    if (line != null)
                    {
                        if (TrackerEvent != null)
                            DispatchEvent(() => TrackerEvent(this, line switch
                            {
                                "W" => TrackerEventType.WAKE_UP,
                                "A" => TrackerEventType.ALARM,
                                "S" => TrackerEventType.SLEEP,
                            }));
                    }
                    else
                    {
                        // End the loop if there are no more lines available
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}