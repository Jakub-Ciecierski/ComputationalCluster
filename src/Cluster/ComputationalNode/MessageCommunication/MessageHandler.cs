using Cluster.Client.Messaging;
using Communication;
using Communication.MessageComponents;
using Communication.Messages;
using Communication.Network.TCP;
using ComputationalNode.TaskSolvers.DVRP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalNode.MessageCommunication
{
    /// <summary>
    ///     Handles the communication with the server
    ///     Uses SystemTracker to fetch system informations.
    /// </summary>
    public class MessageHandler : ClientMessageHandler
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public MessageHandler(SystemTracker systemTracker, NetworkClient client) : base(systemTracker, client)
        {
        }

        /*******************************************************************/
        /****************** PRIVATE / PROTECTED METHODS ********************/
        /*******************************************************************/

        private void handleNoOperationMessage(NoOperationMessage message)
        {
            systemTracker.Node.BackupServers = message.BackupCommunicationServers;
        }

        private void handleRegisterResponsenMessage(RegisterResponseMessage message)
        {
            systemTracker.Node.Id = message.Id;
            systemTracker.Node.Timeout = message.Timeout;
            systemTracker.Node.BackupServers = message.BackupCommunicationServers;
        }

        private void handleSolvePartialProblemsMessage(SolvePartialProblemsMessage message)
        {

            for (int i = 0; i < systemTracker.Node.ParallelThreads; i++)
            {
                if (systemTracker.Node.TaskThreads[i].StatusThread.State == StatusThreadState.Idle)
                {
                    DVRPSolver dvrpSolver = new DVRPSolver(message.PartialProblems[0].Data);
                    systemTracker.Node.TaskThreads[i].StatusThread.State = StatusThreadState.Busy;
                    systemTracker.Node.TaskThreads[i].CurrentTask = new Cluster.Task((int)message.Id, message.ProblemType, message.PartialProblems[0].Data) { Status = Cluster.TaskStatus.Solving };
                    systemTracker.Node.TaskThreads[i].TaskSolver = dvrpSolver;
                    systemTracker.Node.TaskThreads[i].Thread = new Thread(new ThreadStart(systemTracker.Node.TaskThreads[i].Start));
                    systemTracker.Node.TaskThreads[i].Thread.Start();
                    Console.Write("Thread number: " + i + " is solving partial problem");
                    break;
                }
            }
            ///WE SHOULD CHECK HERE WHETHER THERE WAS IDLE THREAD AVALIABLE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // start computations
        }

        protected override void handle(Message message)
        {
            if (message != null)
            {
                if (message.GetType() == typeof(NoOperationMessage))
                    handleNoOperationMessage((NoOperationMessage)message);

                else if (message.GetType() == typeof(RegisterResponseMessage))
                    handleRegisterResponsenMessage((RegisterResponseMessage)message);

                else if (message.GetType() == typeof(SolvePartialProblemsMessage))
                    handleSolvePartialProblemsMessage((SolvePartialProblemsMessage)message);

                else
                    Console.Write(" >> Unknow message type, can't handle it... \n\n");
            }
        }
    }
}