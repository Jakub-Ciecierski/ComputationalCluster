using Communication.MessageComponents;
using Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Network.Client
{
    /// <summary>
    ///     NetworkNode encapsulates a network node in the cluster,
    ///     used in TaskManager, Comp. Node and Backupservers
    /// </summary>
    public class NetworkNode
    {
        private byte parallelThreads;

        public byte ParallelThreads
        {
            get { return parallelThreads; }
            private set { parallelThreads = value; }
        }

        private RegisterType type;

        public RegisterType Type
        {
            get { return type; }
            private set { type = value; }
        }

        private string[] solvableProblems;

        public string[] SolvableProblems
        {
            get { return solvableProblems; }
            private set { solvableProblems = value; }
        }

        private ulong id;

        public ulong Id
        {
            get { return id; }
            set { id = value; }
        }

        private uint timeout;

        public uint Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        private BackupCommunicationServer[] backupServers;

        public BackupCommunicationServer[] BackupServers
        {
            get { return backupServers; }
            set { backupServers = value; }
        }

        private StatusThread[] statusThreads;

        public StatusThread[] StatusThreads
        {
            get { return statusThreads; }
            set { statusThreads = value; }
        }


        /// <summary>
        ///     Creates Network node
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parallelThreads"></param>
        /// <param name="solvableProblems"></param>
        public NetworkNode(RegisterType type, byte parallelThreads, string[] solvableProblems)
        {
            Type = type;
            ParallelThreads = parallelThreads;
            SolvableProblems = solvableProblems;

            StatusThreads = new StatusThread[ParallelThreads];
        }

        public NetworkNode(RegisterType type, byte parallelThreads, string[] solvableProblems, ulong id, uint timeout, BackupCommunicationServer[] backupServers)
        {
            Type = type;
            ParallelThreads = parallelThreads;
            SolvableProblems = solvableProblems;

            Id = id;
            Timeout = timeout;
            BackupServers = backupServers;

            StatusThreads = new StatusThread[ParallelThreads];
        }

        /// <summary>
        ///     Returns RegisterMessage based on this node
        /// </summary>
        /// <returns></returns>
        public RegisterMessage ToRegisterMessage()
        {
            return new RegisterMessage(Type, ParallelThreads, SolvableProblems);
        }

        /// <summary>
        ///     Returns RegisterMessage based on this node, used to
        ///     deregister this node from the server
        /// </summary>
        /// <returns></returns>
        public RegisterMessage ToDeregisterMessage()
        {
            return new RegisterMessage(Type, ParallelThreads, SolvableProblems, true, Id);
        }

        /// <summary>
        ///     Returns a status message based on this object
        /// </summary>
        /// <returns></returns>
        public StatusMessage ToStatusMessage()
        {
            return new StatusMessage(Id, StatusThreads);
        }

    }
}
