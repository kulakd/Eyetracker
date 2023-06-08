using GuiAndroid.Model;
using GuiAndroid.ViewModel;
namespace GuiAndroid;


public partial class MainPage : ContentPage
{
    public static GraphicsView Canvas { get; private set; }

    IServiceTest Services;

    public MainPage(IServiceTest Services_)
    {
        InitializeComponent();
        Canvas = GraphicsV;
        ToggleAccelerometer();
        Services = Services_;
    }

    //method to start manually foreground service
    private void OnServiceStartClicked(object sender, EventArgs e)
    {
        Services.Start();
    }

    //method to stop manually foreground service
    private void Button_Clicked(object sender, EventArgs e)
    {
        Services.Stop();
    }

    //method to work with accelerometer
    public void ToggleAccelerometer()
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }
            else
            {
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }
    }

    //on accelerometer property change we call our service and it would send a message
    private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        Services.Start(); //this will never stop until we made some logic here
    }
}
