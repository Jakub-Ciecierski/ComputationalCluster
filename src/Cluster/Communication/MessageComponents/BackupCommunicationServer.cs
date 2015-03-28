using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageComponents
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

        public override bool Equals(object obj)
        {
            BackupCommunicationServer server = obj as BackupCommunicationServer;

            return (address == server.address && port == server.port);
        }
    }
}
