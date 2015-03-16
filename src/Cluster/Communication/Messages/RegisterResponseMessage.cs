using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Messages
{
    /// <summary>
    /// Register Response message is sent as a response to the 
    /// Register message giving back the component its
    /// unique ID and informing how often it should sent the Status message.
    /// </summary>
    public partial class RegisterResponseMessage
    {
        private RegisterResponseMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     The ID assigned by the Communication Server
        /// </param>
        /// <param name="timeout">
        ///     The communication timeout configured on Communication Server
        /// </param>
        /// <param name="backupServer">
        ///     Backup server
        /// </param>
        public RegisterResponseMessage(ulong id, uint timeout, BackupCommunicationServer backupServer)
        {
            Id = id;
            Timeout = timeout;

            BackupCommunicationServer[] backupServers = {backupServer};

            BackupCommunicationServers = backupServers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     The ID assigned by the Communication Server
        /// </param>
        /// <param name="timeout">
        ///     The communication timeout configured on Communication Server
        /// </param>
        /// <param name="backupServer">
        ///     Backup servers
        /// </param>
        public RegisterResponseMessage(ulong id, uint timeout, BackupCommunicationServer[] backupServers)
        {
            Id = id;
            Timeout = timeout;

            BackupCommunicationServers = backupServers;
        }
    }
}
