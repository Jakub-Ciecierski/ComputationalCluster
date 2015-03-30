using Communication.MessageComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer
{
    public class SystemTracker
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private ulong nextId;

        private BackupCommunicationServer[] backupServers;

        public BackupCommunicationServer[] BackupServers
        {
            get { return backupServers; }
            set { backupServers = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public SystemTracker()
        {
            // TODO Just a placeholder
            BackupCommunicationServer backupServer = new BackupCommunicationServer("192.168.1.15", 5);
            BackupCommunicationServer[] backupServers = { backupServer };

            BackupServers = backupServers;

            nextId = 1;
        }
        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* STATIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Returns unique Id
        /// </summary>
        /// <returns></returns>
        public ulong GetNextID()
        {
            return nextId++;
        }
    }
}
