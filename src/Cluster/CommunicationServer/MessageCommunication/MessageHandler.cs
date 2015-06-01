using Cluster;
using Cluster.Client;
using Communication;
using Communication.MessageComponents;
using Communication.Messages;
using Communication.Network.TCP;
using CommunicationServer.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServer.MessageCommunication
{
    /// <summary>
    ///     This class handles all the messages in the MessageQueue.
    ///     It fetches information about the system then
    ///     changes the state of the system accordingly and (if needed)
    ///     sends appropriate response messages
    /// </summary>
    public partial class MessageHandler
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     The system tracker which is used to fetch info
        ///     about the system
        /// </summary>
        private SystemTracker systemTracker;

        /// <summary>
        ///     A list of all clients
        /// </summary>
        private ClientTracker clientTracker;

        /// <summary>
        ///     Trask all tasks
        /// </summary>
        private TaskTracker taskTracker;

        /// <summary>
        ///     TODO might be fetched from systemTracker
        /// </summary>
        private NetworkServer server;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public MessageHandler(SystemTracker systemTracker, ClientTracker clientTracker , 
                            TaskTracker taskTracker, NetworkServer server)
        {
            this.systemTracker = systemTracker;
            this.clientTracker = clientTracker;
            this.taskTracker = taskTracker;

            this.server = server;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Register is sent by every node in order to register
        ///     to server
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleRegisterMessage(MessagePackage messagePackage)
        {
            RegisterMessage message = (RegisterMessage)messagePackage.Message;
            Socket socket = messagePackage.Socket;

            if (message.Deregister)
            {
                Console.Write(" >> Deregister received, removing client... \n\n");
                socket.Disconnect(false);
                clientTracker.RemoveNode(message.Id, message.Type);
            }
            else
            {
                // Place holder, have to fetch info from the System.
                ulong id = systemTracker.GetNextClientID();
                uint timeout = 4;

                Console.Write(" >> Adding Node to List \n\n");

                RegisterResponseMessage response = new RegisterResponseMessage(id, timeout, systemTracker.BackupServers);
                
                // Create NetworkNode instance
                 //TODO
                NetworkNode node = new NetworkNode(message.Type, response.Id, response.Timeout, message.ParallelThreads, message.SolvableProblems, 
                                                      response.BackupCommunicationServers);
                node.LastSeen = DateTime.Now;
                //NetworkNode node = new NetworkNode();

                // Add the node to system
                clientTracker.AddNode(node);

                server.Send(socket, response);

                Console.Write(" >> Sent a response \n\n");
            }       
        }

        /// <summary>
        ///     Status is sent by everyone as Keep-alive message
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleStatusMessage(MessagePackage messagePackage)
        {
            StatusMessage message = (StatusMessage)messagePackage.Message;
            // check what node
            lock (clientTracker.lockObject)
            {
                NetworkNode networkNode = clientTracker.GetNodeByID(message.Id);

                networkNode.LastSeen = DateTime.Now;
                
                UpdateThreadsStatus(networkNode, message);
                //if status message was send by TaskManager than check if there are any tasks to divide or merge
                if (networkNode.Type == RegisterType.TaskManager)
                {
                    //Response to TaskManager statusMessage
                    ReactToTaskManagerStatusMessage(networkNode, messagePackage);
                }
                //is staty message was send by computational node than check if there are any partial problems to calculate. 
                else if (networkNode.Type == RegisterType.ComputationalNode)
                {
                    ReactToComputationalNodeStatusMessage(networkNode, messagePackage);                }
                else
                {
                    NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
                    server.Send(messagePackage.Socket, response);
                    Console.Write(" >> Sent a NoOperation Message \n");
                }
            }
        }

        /// <summary>
        ///     SolvePartialProblem is sent by TM
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleSolvePartialProblemsMessage(MessagePackage messagePackage)
        {
            /* add partial tasks to subTask list in a task */
            SolvePartialProblemsMessage message = (SolvePartialProblemsMessage)messagePackage.Message;
            Task task = taskTracker.GetTask((int)message.Id);
            task.Status = TaskStatus.Divided;
            for (int i = 0; i < message.PartialProblems.Count(); i++)
            {
                Task subTask = new Task((int)message.Id, message.ProblemType, message.PartialProblems[i].Data);
                subTask.Status = TaskStatus.New;
                task.AddSubTask(subTask);
            }
            /***********************************************/
            NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
            server.Send(messagePackage.Socket, response);
            Console.Write(" >> Sent a NoOperation Message \n");
        }

        /// <summary>
        ///     SolutionRequest is sent by Computational Client
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleSolutionRequestMessage(MessagePackage messagePackage)
        {
            SolutionRequestMessage message = (SolutionRequestMessage)messagePackage.Message;
            
            Task task = taskTracker.GetTask((int)message.Id);
            
            SolutionsMessage solutionMessage = new SolutionsMessage(task.Type, (ulong)task.ID, task.CommonData, task.Solutions);

            server.Send(messagePackage.Socket, solutionMessage);
            if (task.Solutions[0].Type != SolutionsSolutionType.Ongoing)
            {
                taskTracker.Tasks.Remove(task);
            }

        }

        /// <summary>
        ///     Solutions is sent by every node to give info
        ///     about ongoing computations or final solutions
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleSolutionsMessage(MessagePackage messagePackage)
        {
            SolutionsMessage message = (SolutionsMessage)messagePackage.Message;
            Task task = taskTracker.GetTask((int)message.Id);
            //IT HAS TO BE CHANGED AFTER ADDING SUBTASK ID @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            if (message.Solutions[0].Type == SolutionsSolutionType.Final)
            {
                task.Solutions = message.Solutions;
                task.Status = TaskStatus.Merged;
            }
            else
            {
                for (int i = 0; i < task.subTasks.Count; i++)
                {
                    if (task.subTasks[i].Status == TaskStatus.Solving)
                    {
                        task.subTasks[i].Status = TaskStatus.Solved;
                        task.subTasks[i].Solutions = message.Solutions;
                        break;
                    }
                }
            }

            NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
            server.Send(messagePackage.Socket, response);
            Console.Write(" >> Sent a NoOperation Message \n");
        }

        /// <summary>
        ///     SolveRequest is sent by Computational Client
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleSolveRequestMessage(MessagePackage messagePackage)
        {
            SolveRequestMessage message = (SolveRequestMessage)messagePackage.Message;
           
            // if the cluster can solve this problem
            if (clientTracker.CanSolveProblem(message.ProblemType))
            {
                Task task = new Task((int)systemTracker.GetNextTaskID(), message.ProblemType,
                                        message.Data);
                taskTracker.AddTask(task);

               // DivideProblemMessage divideProblemMessage = new DivideProblemMessage(task.Type,task.ID,task.BaseData,(ulong)4,)
               
                SolveRequestResponseMessage response = new SolveRequestResponseMessage((ulong)task.ID);
                server.Send(messagePackage.Socket, response);

                Console.Write(" >> Sent a SolveRequestResponse Message \n");
            }
            else
            {
                //TODO RESPONSE MESSAGE

                Console.Write(" >> TM ERROR\n");
            }
        }

        private void handleNoOperationMessage(MessagePackage package) 
        {
            NoOperationMessage message = (NoOperationMessage)package.Message;
            systemTracker.BackupServers = message.BackupCommunicationServers;

            Console.Write(" >> Received NoOperationMessage \n");
        }

        private void handleRegisterResponsenMessage(MessagePackage package)
        {
            RegisterResponseMessage message = (RegisterResponseMessage)package.Message;

            systemTracker.Node.Id = message.Id;
            systemTracker.Node.Timeout = message.Timeout;
            systemTracker.Node.BackupServers = message.BackupCommunicationServers;

            systemTracker.BackupServers = message.BackupCommunicationServers;
        }
            

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Handles the given package.
        ///     Reads the message, changes the state of the system
        ///     and (if needed) sends a response to the original addressee
        /// </summary>
        /// <param name="package">
        ///     Message to be handled
        /// </param>
        public void HandleMessage(MessagePackage package)
        {
            Message message = package.Message;

            if (message.GetType() == typeof(StatusMessage))
                handleStatusMessage(package);
            else if (message.GetType() == typeof(RegisterMessage))
                handleRegisterMessage(package);
            else if (message.GetType() == typeof(SolvePartialProblemsMessage))
                handleSolvePartialProblemsMessage(package);
            else if (message.GetType() == typeof(SolutionRequestMessage))
                handleSolutionRequestMessage(package);
            else if (message.GetType() == typeof(SolutionsMessage))
                handleSolutionsMessage(package);
            else if (message.GetType() == typeof(SolveRequestMessage))
                handleSolveRequestMessage(package);
            else if (message.GetType() == typeof(NoOperationMessage))
                handleNoOperationMessage(package);
            else if (message.GetType() == typeof(RegisterResponseMessage))
                handleRegisterResponsenMessage(package);
            else
                throw new NotImplementedException("Unknow message type, can't handle it.");
        }
    }
}
