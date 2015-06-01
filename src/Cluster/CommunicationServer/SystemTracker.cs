using Cluster.Client;
using Communication.MessageComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer
{
    public class SystemTracker
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Tracks client ids
        /// </summary>
        private ulong nextClientId;

        private ulong nextTaskId;

        private BackupCommunicationServer[] backupServers;

        public BackupCommunicationServer[] BackupServers
        {
            get { return backupServers; }
            set { backupServers = value; }
        }

        /// <summary>
        ///     Checks if server is in backup or primary mode
        /// </summary>
        private bool backup;

        public bool Backup
        {
            get { return backup; }
            set { backup = value; }
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private IPAddress address;

        public IPAddress Address
        {
            get { return address; }
            set { address = value; }
        }

        public Thread timeOutThread;

        private int timeout;

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        private NetworkNode node;

        public NetworkNode Node
        {
            get { return node; }
            set { node = value; }
        }
        

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public SystemTracker(NetworkNode node)
        {
            // TODO Just a placeholder
            //BackupCommunicationServer backupServer = new BackupCommunicationServer("192.168.1.15", 5);
            //BackupCommunicationServer[] backupServers = { backupServer };

            BackupServers = backupServers;

            nextClientId = 1;

            Timeout = 4;

            Node = node;
        }

        public SystemTracker()
        {
            // TODO Just a placeholder
            //BackupCommunicationServer backupServer = new BackupCommunicationServer("192.168.1.15", 5);
            //BackupCommunicationServer[] backupServers = { backupServer };

            BackupServers = backupServers;

            nextClientId = 1;

            Timeout = 4;
        }
        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/



        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void AddBackupServer(BackupCommunicationServer backupServer)
        {
            int newSize = backupServers.Length + 1;
            BackupCommunicationServer[] newBackupServers = new BackupCommunicationServer[newSize];

            for (int i = 0; i < newSize - 1; i++)
            {
                newBackupServers[i] = backupServers[i];
            }
            newBackupServers[newSize - 1] = backupServer;

            backupServers = newBackupServers;
        }

        /// <summary>
        ///     Returns unique Id
        /// </summary>
        /// <returns></returns>
        public ulong GetNextClientID()
        {
            return nextClientId++;
        }

        public ulong GetNextTaskID()
        {
            return nextTaskId++;
        }
    }
}
