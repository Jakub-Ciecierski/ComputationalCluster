using Communication.MessageComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace CommunicationServer
{
    abstract class Module
    {
        private string[] solvableProblemsField;
        private byte parallelThreadsField;
        private StatusThread[] statusThreads;
        private IPAddress ipAddress;
        private bool updateStatus;
        public string[] SolvableProblemsField
        {
            get
            {
                return this.solvableProblemsField;
            }
            set
            {
                this.solvableProblemsField = value;
            }
        }
        public byte ParallelThreadField
        {
            get
            {
                return this.parallelThreadsField;
            }
            set
            {
                this.parallelThreadsField = value;
            }
        }
        public StatusThread[] StatusThreads
        {
            get
            {
                return this.statusThreads;
            }
            set
            {
                this.statusThreads = value;
            }
        }
        public IPAddress IpAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                this.ipAddress = value;
            }
        }
        /// <summary>
        /// Update Status should be changed to false after each timeout and to true after each status message.
        /// </summary>
        public bool UpdateStatus
        {
            get
            {
                return this.updateStatus;
            }
            set
            {
                this.updateStatus = value;
            }
        }

    }
}
