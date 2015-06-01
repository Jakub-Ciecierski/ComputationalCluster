using Cluster.Client.Messaging;
using Cluster.Util;
using Communication;
using Communication.MessageComponents;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalClient.MessageCommunication
{
    class MessageHandler : ClientMessageHandler
    {

        public ComputationalClientCheckTimer clientComputationsCheckTimer;

        public KeepAliveTimer keepAliveTimer;

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
                throw new NotImplementedException("Unknow message");
        }

        private void handleSolutionsMessage(SolutionsMessage solutionsMessage)
        {
            bool isOnGoing = false;

            if (solutionsMessage.Solutions[0].Type ==  SolutionsSolutionType.Ongoing)
            {
                SmartConsole.PrintLine("Ongoing computations. Waiting for full solution", SmartConsole.DebugLevel.Basic);
            }
            else
            {
                for (int i = 0; i < solutionsMessage.Solutions.Count(); i++)
                {
                    if (solutionsMessage.Solutions[i].Type == SolutionsSolutionType.Ongoing)
                    {
                        isOnGoing = true;
                        break;
                    }
                }
                if (isOnGoing)
                {
                    SmartConsole.PrintLine("Ongoing computations. Waiting for full solution", SmartConsole.DebugLevel.Basic);
                }
                else
                {
                    SmartConsole.PrintLine("Complete solution has been received", SmartConsole.DebugLevel.Advanced);
                    // TODO print solution
                    keepAliveTimer.Stop();
                }
            }
        }

        private void handleSolverRequestResponseMessage(SolveRequestResponseMessage solveRequestResponseMessage)
        {
            SmartConsole.PrintLine("Solve request respone message has been received", SmartConsole.DebugLevel.Advanced);

            systemTracker.Node.Id = solveRequestResponseMessage.Id;

            keepAliveTimer.Start(solveRequestResponseMessage.Id);
        }
    }
}
