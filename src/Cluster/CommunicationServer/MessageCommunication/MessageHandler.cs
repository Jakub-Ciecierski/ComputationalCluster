using Communication;
using Communication.MessageComponents;
using Communication.Messages;
using Communication.Network.Client;
using Communication.Network.TCP;
using CommunicationServer.Communication;
using CommunicationServer.TaskModule;
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
    public class MessageHandler
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
                NetworkNode node = new NetworkNode(message.Type, message.ParallelThreads, message.SolvableProblems, 
                                                    response.Id, response.Timeout, response.BackupCommunicationServers);

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
            NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
            server.Send(messagePackage.Socket, response);
            Console.Write(" >> Sent a NoOperation Message \n");
        }

        /// <summary>
        ///     SolvePartialProblem is sent by TM
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleSolvePartialProblemsMessage(MessagePackage messagePackage)
        {
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
            NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
            server.Send(messagePackage.Socket, response);
            Console.Write(" >> Sent a NoOperation Message \n");
        }

        /// <summary>
        ///     Solutions is sent by every node to give info
        ///     about ongoing computations or final solutions
        /// </summary>
        /// <param name="messagePackage"></param>
        private void handleSolutionsMessage(MessagePackage messagePackage)
        {
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
                Task task = new Task(systemTracker.GetNextTaskID(), message.ProblemType,
                                        message.Data);
                taskTracker.AddTask(task);
                SolveRequestResponseMessage response = new SolveRequestResponseMessage(task.ID);
                server.Send(messagePackage.Socket, response);
            }
            //SolveRequestResponseMessage response = new SolveRequestResponseMessage(task)
           // NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);

            //server.Send(messagePackage.Socket, response);
            Console.Write(" >> Sent a SolveRequest Message \n");
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
            else
                Console.Write(" >> Unknow message type, can't handle it... \n\n");
        }
    }
}
