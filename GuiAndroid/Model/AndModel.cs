using Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class AndModel
    {
        public readonly P2PTCPVideoConnection Connection = new P2PTCPVideoConnection();
     //   private readonly ConnectionSettings settings = new ConnectionSettings();

        public AndModel()
        {
       //     Connection.Connect(settings);
       
        }
    }
}
