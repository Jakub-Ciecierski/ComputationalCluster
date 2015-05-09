using Cluster.Client.Messaging;
using Communication;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // start computations
        }

        protected override void handle(Message message)
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