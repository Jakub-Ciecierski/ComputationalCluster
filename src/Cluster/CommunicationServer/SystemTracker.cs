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
            nextClientId = 1;

            Timeout = 2;

            Node = node;
        }

        public SystemTracker()
        {
            nextClientId = 1;

            Timeout = 2;
        }
        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/



        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/


        /// <summary>
        ///     Returns unique Id
        ///     BIG TODO, will not work with nodes added after crash of primary 
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
