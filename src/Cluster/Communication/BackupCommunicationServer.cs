using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public partial class BackupCommunicationServer
    {
        private BackupCommunicationServer() { }

        public BackupCommunicationServer(string address, ushort port)
        {
            this.address = address;
            this.port = port;
            this.portSpecified = true;
        }
    }
}
