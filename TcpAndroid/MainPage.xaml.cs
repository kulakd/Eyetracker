using Connections;
namespace TcpAndroid;

public partial class MainPage : ContentPage
{
    private P2PTCPVideoConnection connection;


    private static string[] getValidIPAddresses()
    {
        List<string> addresses = new List<string>();
        System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (System.Net.IPAddress adres in host.AddressList)
            addresses.Add(adres.ToString());
        return addresses.ToArray();
    }

    private void pokażAdresyIp()
    {
        string[] addresses = getValidIPAddresses();
        string s = "Lokalne adresy IP:\r\n";
        foreach (string address in addresses)
            s += address + "\r\n";
        tbLokalneAdresyIp.Text = s.TrimEnd('\r', '\n');
    }
    public MainPage()
    {
        InitializeComponent();
        pokażAdresyIp();
    }

    private void TextReceived(object sender, string s)
    {
        Action a = () => recLbl.Text += $"\n[Odebrano {DateTime.Now}]: {s}";
        if (Dispatcher.IsDispatchRequired) Dispatcher.Dispatch(a); else a();
    }

    private MemoryStream ms;
    private MemoryStream imageStream
    {
        get
        {
            return ms;
        } // Nie zmieniac na =>, nie lubi
        set
        {
            MemoryStream old = ms;
            ms = value;
            if (old != null)
                old.Dispose();
        }
    }
    private void ImgReceived(object sender, MemoryStream ms)
    {
        imageStream = ms;
        img.Source = ImageSource.FromStream(() => imageStream);
    }

    private void btnWyslij_Click(object sender, EventArgs e)
    {
        connection.Send(tbTekst.Text);
    }

    private void btnPolacz_Click(object sender, EventArgs e)
    {
        try
        {
            connection = new P2PTCPVideoConnection(true);

            connection.ExceptionThrown += Connection_ExceptionThrown;
            connection.StringReceived += TextReceived;
            connection.ImageReceived += ImgReceived;
            connection.ConnectionStateChanged += Connection_ConnectionStateChanged;

            connection.Connect(ConnectionSettings.Parse(tbAdres.Text));
        }
        catch (Exception exc)
        {
            Connection_ExceptionThrown(null, exc);
        }
    }

    private void Connection_ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
    {
        tbStan.Text = e.ToString();
    }

    private void Connection_ExceptionThrown(object sender, Exception e)
    {
        tberror.Text = $"[{e.GetType()}] {e.Message}";
    }

    private void btnRozlacz_Click(object sender, EventArgs e)
    {
        try
        {
            connection.Disconnect();
        }
        catch (Exception exc)
        {
            Connection_ExceptionThrown(null, exc);
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (connection != null)
            connection.Disconnect();
        btnRozlacz_Click(this, null);
    }
}

