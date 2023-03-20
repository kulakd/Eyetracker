using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace GameLab.Eyetracking.EyetrackerControls
{
    [DefaultProperty("EyetrackerType")]
    public partial class EyetrackerConnectionSettingsPanel: UserControl
    {
        public EyetrackerConnectionSettingsPanel()
        {
            InitializeComponent();

            cbEyetrackerType.SelectedIndex = 0;
            getValidIPAddresses();
        }

        [Category("Eyetracker")]
        [Description("Eyetracker device")]        
        public EyetrackerType EyetrackerType 
        { 
            //wartość przechowywana w cbEyetrackerType
            get
            {
                return (EyetrackerType)cbEyetrackerType.SelectedIndex;
            }
            set
            {
                cbEyetrackerType.SelectedIndex = (int)value;                                
            }
        }

        [Category("Eyetracker")]
        [Description("Settings of connection to eyetracker server")]                
        [TypeConverter(typeof(EyetrackerConnectionSettingsTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EyetrackerConnectionSettings EyetrackerConnectionSettings
        {
            get
            {
                return gainSettings();
            }
            set
            {
                tbSendIp.Text = value.ServerIp;
                tbSendPort.Text = value.ServerPort.ToString();
                cbReveiveIp.Text = value.ClientIp;
                tbReceivePort.Text = value.ClientPort.ToString();
            }
        }        

        private void cbEyetracker_SelectedIndexChanged(object sender, EventArgs e)
        {            
            cbReveiveIp.Enabled = cbEyetrackerType.SelectedIndex != 2;
            tbReceivePort.Enabled = cbReveiveIp.Enabled;
        }

        private void getValidIPAddresses()
        {
            cbReveiveIp.Items.Clear();
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress adres in host.AddressList)
            {
                cbReveiveIp.Items.Add(adres.ToString());
            }
        }

        private EyetrackerConnectionSettings gainSettings()
        {
            EyetrackerConnectionSettings ecs = new EyetrackerConnectionSettings();
            ecs.ServerIp = tbSendIp.Text;
            ecs.ServerPort = int.Parse(tbSendPort.Text);
            ecs.ClientIp = cbReveiveIp.Text;
            ecs.ClientPort = int.Parse(tbReceivePort.Text);
            return ecs;
        }

        public void LockEyetrackerType()
        {
            cbEyetrackerType.Enabled = false;
        }

        public void LockEyetrackerConnectionSettings()
        {
            gbEyetrackerConnection.Enabled = false;
        }
    }
}
