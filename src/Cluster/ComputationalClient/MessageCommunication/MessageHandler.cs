using Communication;
using Communication.Messages;
using Communication.Network.Client.MessageCommunication;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalClient.MessageCommunication
{
    class MessageHandler:ClientMessageHandler
    {
        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public MessageHandler(SystemTracker systemTracker, NetworkClient client)
            : base(systemTracker, client)
        {
        }

        /*******************************************************************/
        /****************** PRIVATE / PROTECTED METHODS ********************/
        /*******************************************************************/

        protected override void handle(Message message)
        {
            if (message.GetType() == typeof(SolveRequestResponseMessage))
                handleSolverRequestResponseMessage((SolveRequestResponseMessage)message);

            else if (message.GetType() == typeof(SolutionsMessage))
                handleSolutionsMessage((SolutionsMessage)message);

            else
                Console.Write(" >> Unknow message type, can't handle it... \n\n");
        }

        private void handleSolutionsMessage(SolutionsMessage solutionsMessage)
        {
            throw new NotImplementedException();
        }

        private void handleSolverRequestResponseMessage(SolveRequestResponseMessage solveRequestResponseMessage)
        {
            throw new NotImplementedException();
        }
    }
}
