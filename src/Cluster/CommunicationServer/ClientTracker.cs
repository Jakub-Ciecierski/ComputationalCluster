using Cluster.Client;
using Communication.MessageComponents;
using Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer
{
    /// <summary>
    ///     Keeps track of all clients connected to the server
    /// </summary>
    public class ClientTracker
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private List<NetworkNode> backupServers = new List<NetworkNode>();

        private List<NetworkNode> taskManagers = new List<NetworkNode>();

        private List<NetworkNode> compNodes = new List<NetworkNode>();

        public Object lockObject =  new Object();

       // public Thread timeOutCheckThread = new Thread(new ThreadStart(CheckNodesTimeOut));

        private Thread timeoutThread;

        private bool timeoutActive;

        public bool TimeoutActive
        {
            get { return timeoutActive; }
            private set { timeoutActive = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public ClientTracker()
        {

        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/
        /// <summary>
        ///     Removes a backup server
        /// </summary>
        /// <param name="id"></param>
        private void removeBackupServer(ulong id)
        {
            for (int i = 0; i < backupServers.Count; i++)
            {
                if (backupServers[i].Id == id)
                {
                    backupServers.RemoveAt(i);
                    return;
                }
            }       
        }

        /// <summary>
        ///     removes a computational node
        /// </summary>
        /// <param name="id"></param>
        private void removeCompNode(ulong id)
        {
            for (int i = 0; i < compNodes.Count; i++)
            {
                if (compNodes[i].Id == id)
                {
                    compNodes.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        ///     removes a task manager
        /// </summary>
        /// <param name="id"></param>
        private void removeTaskManager(ulong id)
        {
            for (int i = 0; i < taskManagers.Count; i++)
            {
                if (taskManagers[i].Id == id)
                {
                    taskManagers.RemoveAt(i);
                    return;
                }
            }
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public BackupCommunicationServer[] ToBackupServersArray() 
        {
            int backupSize = backupServers.Count;
            BackupCommunicationServer[] backupServersArr = new BackupCommunicationServer[backupSize];

            for (int i = 0; i < backupSize; i++)
            {
                NetworkNode server = backupServers[i];
                BackupCommunicationServer backupServer = new BackupCommunicationServer(server.Address.ToString(), server.Port);
                backupServersArr[i] = backupServer;
            }
            
            return backupServersArr;
        }

        public NetworkNode GetNodeByID(ulong id)
        {
            foreach (NetworkNode tm in taskManagers)
            {
                if (tm.Id == id)
                    return tm;
            }
            foreach (NetworkNode cn in compNodes)
            {
                if (cn.Id == id)
                    return cn;
            }
            return null;
        }

        /// <summary>
        ///     Checks if the cluster has nodes with given
        ///     problem solver
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public bool CanSolveProblem(string problem)
        {
            bool tmSolve = false;
            bool cnSolve = false;

            foreach(NetworkNode tm in taskManagers)
            {
                if(tm.CanSolveProblem(problem))
                    tmSolve = true;
            }
            foreach(NetworkNode cn in compNodes)
            {
                if(cn.CanSolveProblem(problem))
                    cnSolve = true;
            }

            return (tmSolve && cnSolve);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetComputationalNodeCount()
        {
            return compNodes.Count;
        }

        /// <summary>
        ///     Adds a network node to the list of clients
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(NetworkNode node)
        {
            switch (node.Type)
            {
                case RegisterType.CommunicationServer:
                    backupServers.Add(node);
                    break;
                case RegisterType.ComputationalNode:
                    compNodes.Add(node);
                    break;
                case RegisterType.TaskManager:
                    taskManagers.Add(node);
                    break;
            }
        }

        /// <summary>
        ///     removes a node from the client list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void RemoveNode(ulong id, RegisterType type)
        {
            switch (type)
            {
                case RegisterType.CommunicationServer:
                    removeBackupServer(id);
                    break;
                case RegisterType.ComputationalNode:
                    removeCompNode(id);
                    break;
                case RegisterType.TaskManager:
                    removeTaskManager(id);
                    break;
            }
        }


        public void StartTimeout()
        {
            TimeoutActive = true;
            timeoutThread = new Thread(CheckNodesTimeOut);
            timeoutThread.Start();
        }

        public void StopTimeout()
        {
            TimeoutActive = false;
        }

        /// <summary>
        /// This function is running in another thread. It checks out if the function is timed out or not 
        /// </summary>
        public void CheckNodesTimeOut()
        {
            while (TimeoutActive)
            {
                TimeSpan timeDifference;
                DateTime currentTime = DateTime.Now;
                int minSecMil = currentTime.Minute * 1000 * 60 + currentTime.Second * 1000 + currentTime.Millisecond;
                for (int i = 0; i < backupServers.Count; i++)
                {
                    timeDifference = currentTime.Subtract(backupServers[i].LastSeen);
                    if (timeDifference > new TimeSpan(0, 0, (int)backupServers[i].Timeout))
                    {
                        lock (lockObject)
                        {
                            backupServers.RemoveAt(i);
                            break;
                        } 
                    }
                }
                for (int i = 0; i < compNodes.Count; i++)
                {
                    timeDifference = currentTime.Subtract(compNodes[i].LastSeen);
                    if (timeDifference > new TimeSpan(0, 0, (int)compNodes[i].Timeout))
                    {
                        lock (lockObject)
                        {
                            compNodes.RemoveAt(i);
                            break;
                        } 
                    }
                }
                for (int i = 0; i < taskManagers.Count; i++)
                {
                    timeDifference = currentTime.Subtract(taskManagers[i].LastSeen);
                    if (timeDifference > new TimeSpan(0, 0, (int)taskManagers[i].Timeout))
                    {
                        lock (lockObject)
                        {
                            taskManagers.RemoveAt(i);
                            break;
                        } 
                    }
                }
                Thread.Sleep(500);
           
            }
        }
    }
}
