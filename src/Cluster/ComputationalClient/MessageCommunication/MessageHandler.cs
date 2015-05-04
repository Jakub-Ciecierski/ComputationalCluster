using Communication;
using Communication.MessageComponents;
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
            if (solutionsMessage.Solutions[0].Type == SolutionsSolutionType.Ongoing)
            {
                Console.WriteLine("Ongoing computations. Waiting for full solution");
            }
            else
            {
                Console.WriteLine("Complete solution has been received");
            }
        }

        private void handleSolverRequestResponseMessage(SolveRequestResponseMessage solveRequestResponseMessage)
        {
            Console.WriteLine("Solve request respone message has been received");
            systemTracker.Node.Id = solveRequestResponseMessage.Id;

        }
    }
}
