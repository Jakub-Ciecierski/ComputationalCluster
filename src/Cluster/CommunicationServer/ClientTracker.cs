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

        private BackupCommunicationServer[] backupServersArray;

        public BackupCommunicationServer[] BackupServers
        {
            get { return backupServersArray; }
            set { backupServersArray = value; }
        }

        private List<NetworkNode> backupServersList = new List<NetworkNode>();

        private List<NetworkNode> taskManagers = new List<NetworkNode>();

        private List<NetworkNode> compNodes = new List<NetworkNode>();

        public Object lockObject =  new Object();

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
            backupServersArray = new BackupCommunicationServer[0];
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/
        /// <summary>
        ///     Removes a backup server
        /// </summary>
        /// <param name="id"></param>
        private bool removeBackupServer(ulong id)
        {
            for (int i = 0; i < backupServersList.Count; i++)
            {
                if (backupServersList[i].Id == id)
                {
                    backupServersList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     removes a computational node
        /// </summary>
        /// <param name="id"></param>
        private bool removeCompNode(ulong id)
        {
            for (int i = 0; i < compNodes.Count; i++)
            {
                if (compNodes[i].Id == id)
                {
                    compNodes.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     removes a task manager
        /// </summary>
        /// <param name="id"></param>
        private bool removeTaskManager(ulong id)
        {
            for (int i = 0; i < taskManagers.Count; i++)
            {
                if (taskManagers[i].Id == id)
                {
                    taskManagers.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void AddBackupServer(BackupCommunicationServer backupServer)
        {
            int newSize = backupServersArray.Length + 1;
            BackupCommunicationServer[] newBackupServers = new BackupCommunicationServer[newSize];

            for (int i = 0; i < newSize - 1; i++)
            {
                newBackupServers[i] = backupServersArray[i];
            }
            newBackupServers[newSize - 1] = backupServer;

            backupServersArray = newBackupServers;
        }

        public BackupCommunicationServer[] ToBackupServersArray() 
        {
            int backupSize = backupServersList.Count;
            BackupCommunicationServer[] backupServersArr = new BackupCommunicationServer[backupSize];

            for (int i = 0; i < backupSize; i++)
            {
                NetworkNode server = backupServersList[i];
                BackupCommunicationServer backupServer = new BackupCommunicationServer(server.Address.ToString(), server.Port);
                backupServersArr[i] = backupServer;
            }
            
            return backupServersArr;
        }

        public NetworkNode GetNodeByID(ulong id)
        {
            foreach (NetworkNode tm in backupServersList)
            {
                if (tm.Id == id)
                    return tm;
            }

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
                    backupServersList.Add(node);
                    BackupCommunicationServer bserver = new BackupCommunicationServer(node.Address.ToString(), node.Port);
                    AddBackupServer(bserver);
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
        public bool RemoveNode(ulong id, RegisterType type)
        {
            switch (type)
            {
                case RegisterType.CommunicationServer:
                    return(removeBackupServer(id));

                case RegisterType.ComputationalNode:
                    return(removeCompNode(id));

                case RegisterType.TaskManager:
                    return(removeTaskManager(id));

            }
            return false;
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
                for (int i = 0; i < backupServersList.Count; i++)
                {
                    timeDifference = currentTime.Subtract(backupServersList[i].LastSeen);
                    if (timeDifference > new TimeSpan(0, 0, (int)backupServersList[i].Timeout))
                    {
                        lock (lockObject)
                        {
                            backupServersList.RemoveAt(i);
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
