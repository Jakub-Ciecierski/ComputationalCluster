using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Messages
{
    /// <summary>
    /// No Operation message is sent by the CS in response to Status messages.
    /// It is used to inform the components about the current list of backup servers.
    /// It could be send in conjunction with other messages.
    /// </summary>
    public partial class NoOperationMessage
    {
        private NoOperationMessage() { }

        public NoOperationMessage(BackupCommunicationServer backupServer)
        {
            BackupCommunicationServer[] backupServers =
            {
                backupServer
            };

            BackupCommunicationServers = backupServers;
        }

        public NoOperationMessage(BackupCommunicationServer[] backupServers)
        {
            BackupCommunicationServers = backupServers;
        }
    }
}
