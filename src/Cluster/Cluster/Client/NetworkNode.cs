using Cluster.Client.Messaging;
using Communication.MessageComponents;
using Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Client
{
    /// <summary>
    ///     NetworkNode encapsulates a network node in the cluster,
    ///     used in TaskManager, Comp. Node and Backupservers
    /// </summary>
    public class NetworkNode
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

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

        private TaskThread[] taskThreads;

        public TaskThread[] TaskThreads
        {
            get { return taskThreads; }
            private set { taskThreads = value; }
        }

        private MessageProcessor messageProcessor;

        public MessageProcessor MessageProcessor
        {
            get { return messageProcessor; }
            set { messageProcessor = value; }
        }

        private DateTime lastSeen;

        public DateTime LastSeen
        {
            get { return lastSeen; }
            set { lastSeen = value; }
        }

        
        /******************************************************************/
        /******************* CONSTRUCTORS (CLIENT SIDE) *******************/
        /******************************************************************/

        /// <summary>
        ///     Creates Network node
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parallelThreads"></param>
        /// <param name="solvableProblems"></param>
       /* public NetworkNode(RegisterType type, TaskThread[] taskThreads)
        {
            Type = type;

            TaskThreads = taskThreads;
            ParallelThreads = (byte)TaskThreads.Count();
        }*/
        public NetworkNode(RegisterType type, byte parallelThreads, string[] problems)
        {
            Type = type;
            TaskThreads = new TaskThread[parallelThreads];
            for (int i = 0; i < parallelThreads; i++)
            {
                TaskThreads[i] = new TaskThread(i, problems[0], messageProcessor,(int)Id);
            }
            ParallelThreads = parallelThreads;
        }

      /*  public NetworkNode(RegisterType type, ulong id, uint timeout, string[] problems)
        {
            Type = type;

            Id = id;
            Timeout = timeout;

            TaskThreads = taskThreads;
            ParallelThreads = (byte)TaskThreads.Count();
        }*/

        /******************************************************************/
        /******************* CONSTRUCTORS (SRERVER SIDE) *******************/
        /******************************************************************/
        public NetworkNode(RegisterType type, ulong id, uint timeout, byte parallelThreads, string[] solvableProblems, BackupCommunicationServer[] backupCommunicationServer)
        {
            Type = type;

            Id = id;
            Timeout = timeout;
            TaskThread[] taskThreads = new TaskThread[parallelThreads];
            for (int i = 0; i < parallelThreads; i++)
            {
                taskThreads[i] = new TaskThread((int)id, solvableProblems[0], messageProcessor,(int)Id);
            }
            TaskThreads = taskThreads;
            ParallelThreads = parallelThreads;
            BackupServers = backupCommunicationServer;
            SolvableProblems = solvableProblems;
        }
        
        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private string[] getSolvableProblems()
        {
            string[] problems = new string[ParallelThreads];
            for(int i =0;i<ParallelThreads;i++){
                problems[i] = TaskThreads[i].SolvableProblem;
            }
            return problems;
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Adds a thread to the network node
        /// </summary>
        /// <param name="taskThread"></param>
        public void AddThread(TaskThread taskThread)
        {
            // TODO
        }

        /// <summary>
        ///     Check if this node can solve a given problem
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public bool CanSolveProblem(string problem)
        {
            foreach (TaskThread taskThread in TaskThreads)
            {
                if (taskThread.SolvableProblem.Equals(problem))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns RegisterMessage based on this node
        /// </summary>
        /// <returns></returns>
        public RegisterMessage ToRegisterMessage()
        {
            return new RegisterMessage(Type, ParallelThreads, getSolvableProblems());
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
            StatusThread[] statusThreads = new StatusThread[ParallelThreads];
            for (int i = 0; i < ParallelThreads; i++)
            {
                statusThreads[i] = TaskThreads[i].StatusThread;
            }
            return new StatusMessage(Id, statusThreads);
        }

    }
}
