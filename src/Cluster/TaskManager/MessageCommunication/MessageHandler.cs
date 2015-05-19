using Cluster.Client.Messaging;
using Communication;
using Communication.MessageComponents;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace TaskManager.MessageCommunication
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
        /************************ PRIVATE METHODS **************************/
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

        private void handleDivideProblemMessage(DivideProblemMessage message)
        {

            for (int i = 0; i < systemTracker.Node.ParallelThreads; i++)
            {
                if (systemTracker.Node.TaskThreads[i].StatusThread.State == StatusThreadState.Idle)
                {
                    systemTracker.Node.TaskThreads[i].StatusThread.State = StatusThreadState.Busy;
                    systemTracker.Node.TaskThreads[i].CurrentTask = new Cluster.Task((int)message.Id, message.ProblemType, message.Data) {Status = Cluster.TaskStatus.Dividing};
                    systemTracker.Node.TaskThreads[i].Start();
                    break;
                }
            }
            ///WE SHOULD CHECK HERE WHETHER THERE WAS IDLE THREAD AVALIABLE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // start computations
        }

        protected override void handle(Message message)
        {
            if (message.GetType() == typeof(NoOperationMessage))
                handleNoOperationMessage((NoOperationMessage)message);

            else if (message.GetType() == typeof(RegisterResponseMessage))
                handleRegisterResponsenMessage((RegisterResponseMessage)message);

            else if (message.GetType() == typeof(DivideProblemMessage))
                handleDivideProblemMessage((DivideProblemMessage)message);

            else
                Console.Write(" >> Unknow message type, can't handle it... \n\n");
        }
    }
}
