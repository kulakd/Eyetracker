using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using EyeTracker;
using Connections;
using System.Net;

namespace TcpWindows;

public partial class MainPage : ContentPage
{
    private readonly CamTracker faceCam = new CamTracker();

    private static string[] getValidIPAddresses()
    {
        List<string> addresses = new List<string>();
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress adres in host.AddressList)
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

    private P2PTCPVideoConnection connect;

    public MainPage()
    {
        InitializeComponent();

        pokażAdresyIp();

        faceCam.NewFrameEvent += FaceCam_TrackEvent;
        faceCam.Index = 0;
    }

    private void FaceCam_TrackEvent(object sender, MemoryStream e)
    {
        if (connect != null)
            connect.Video = e;
    }

    private void TextReceived(object sender, string s)
    {
        Action a = () => lbrec.Text += $"\n[Odebrano {DateTime.Now}]: {s}";
        if (Dispatcher.IsDispatchRequired) Dispatcher.Dispatch(a); else a();
    }

    private void btnPolacz_Click(object sender, EventArgs e)
    {
        connect = new P2PTCPVideoConnection();

        connect.StringReceived += TextReceived;
        connect.ConnectionStateChanged += Connect_ConnectionStateChanged;
        connect.ExceptionThrown += Connect_ExceptionThrown;

        connect.WaitForConnection(ConnectionSettings.Parse(localAdd.Text));
    }

    private void Connect_ExceptionThrown(object sender, Exception e)
    {
        try
        {
            lbError.Text = $"[{e.GetType()}] {e.Message}";
        }
        catch { }
    }

    private void Connect_ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
    {
        tbStan.Text = e.ToString();
    }

    private void btnRozlacz_Click(object sender, EventArgs e)
    {
        connect.Disconnect();
    }

    private void btnWyslij_Click(object sender, EventArgs e)
    {
        connect.Send(tbTekst.Text);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (connect != null)
            connect.Disconnect();
        faceCam.Stop();
        btnRozlacz_Click(this, null);
    }

}

