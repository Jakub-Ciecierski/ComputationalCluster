using Cluster.Client.Messaging;
using Cluster.Math.TSP;
using Cluster.Util;
using Communication;
using Communication.MessageComponents;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            else if (message.GetType() == typeof(NoOperationMessage))
                handleNoOperationMessage((NoOperationMessage)message);
            else
                throw new NotImplementedException("Unknow message");
        }

        private void handleNoOperationMessage(NoOperationMessage message) 
        {
            systemTracker.Node.BackupServers = message.BackupCommunicationServers;
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
                    finalSolutionHelper(solutionsMessage);
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

        private void finalSolutionHelper(SolutionsMessage solutionsMessage)
        {
            byte[] data = solutionsMessage.Solutions[0].Data;


            BinaryFormatter formatter = new BinaryFormatter();
            Result finalResults = (Result)formatter.Deserialize(new MemoryStream(data));

            SmartConsole.PrintHeader("TASK ID: "+ solutionsMessage.Id +" RESULTS");

            int[] finalRoute = finalResults.route;
            float finalDistance = finalResults.length;

            SmartConsole.PrintLine("Distance: " + finalDistance, SmartConsole.DebugLevel.Advanced);
            int vehicleIndex = 0;
            string msg = "";

            for(int i = 0;i < finalRoute.Length; i++)
            {
                if (finalRoute[i] == -1)
                    msg += "\n";
                else
                    msg += finalRoute[i] + ", ";
            }

            SmartConsole.PrintLine("Path: \n" + msg, SmartConsole.DebugLevel.Advanced);

        }
    }
}
