using Communication.Messages;
using Communication.Network.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
