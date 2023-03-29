using Notifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notifications.ViewModel
{
    public class NotificationsVM
    {
        private readonly NotifModel model = new NotifModel();

        private ICommand otwarte;
        private ICommand rusza;
        private ICommand krztusi;

        public ICommand Otwarte
        {
            get
            {
                if (otwarte == null)
                    otwarte = new RelayCommand((o) =>
                    {
                        model.Alarm1();
                    });
                return otwarte;
            }
        }
        public ICommand Rusza
        {
            get
            {
                if (rusza == null)
                    rusza = new RelayCommand((o) =>
                    {
                        model.Alarm2();
                    });
                return rusza;
            }
        }
        public ICommand Krztusi
        {
            get
            {
                if (krztusi == null)
                    krztusi = new RelayCommand((o) =>
                    {
                        model.Alarm3();
                    });
                return krztusi;
            }
        }
    }
}

