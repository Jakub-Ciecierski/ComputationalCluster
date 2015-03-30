using Communication.MessageComponents;
using Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Network
{
    /// <summary>
    ///     NetworkNode encapsulates a network node in the cluster,
    ///     used in TaskManager, Comp. Node and Backupservers
    /// </summary>
    public class NetworkNode
    {
        private byte parallelThreads;

        private RegisterType type;

        private string[] solvableProblems;

        private uint timeout;

        private ulong id;

        private BackupCommunicationServer[] backupServers;

        private StatusThread[] statusThreads;

        public byte ParallelThreads
        {
            get { return parallelThreads; }
            private set { parallelThreads = value; }
        }

        public RegisterType Type
        {
            get { return type; }
            private set { type = value; }
        }

        public string[] SolvableProblems
        {
            get { return solvableProblems; }
            private set { solvableProblems = value; }
        }

        public ulong Id
        {
            get { return id; }
            private set { id = value; }
        }

        public uint Timeout
        {
            get { return timeout; }
            private set { timeout = value; }
        }

        public BackupCommunicationServer[] BackupServers
        {
            get { return backupServers; }
            private set { backupServers = value; }
        }

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

        public StatusMessage ToStatusMessage()
        {
            return new StatusMessage(Id, StatusThreads);
        }

        /// <summary>
        ///     Handles the RegisaterResponse message by setting up
        ///     the properties
        /// </summary>
        /// <param name="message">
        ///     RegisterResponse Message to be handled
        /// </param>
        public void RegisterResponseHandler(RegisterResponseMessage message)
        {
            Id = message.Id;
            Timeout = message.Timeout;
            BackupServers = message.BackupCommunicationServers;
        }
    }
}
