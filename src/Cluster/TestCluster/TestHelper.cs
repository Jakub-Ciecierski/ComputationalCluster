using Communication.MessageComponents;
using Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCluster
{
    public class TestHelper
    {
        public static StatusMessage CreateStatusMessage()
        {
            ulong id = 12;

            StatusThread statusThread1 = new StatusThread(StatusThreadState.Idle);
            StatusThread statusThread2 = new StatusThread(StatusThreadState.Idle);

            ulong howLong = 1000;
            ulong problemInstanceId = 2;
            ulong taskId = 1;
            string problemType = "TCP";

            StatusThread statusThread3 = new StatusThread(StatusThreadState.Busy, howLong, problemInstanceId, taskId, problemType);

            StatusThread[] statusThreads = { statusThread1, statusThread2, statusThread3 };

            StatusMessage expectedMessage = new StatusMessage(id, statusThreads);
            return expectedMessage;
        }

        public static SolveRequestResponseMessage CreateSolveRequestResponseMessage()
        {
            ulong id = 12;

            SolveRequestResponseMessage expectedMessage = new SolveRequestResponseMessage(id);
            return expectedMessage;
        }

        public static SolveRequestMessage CreateSolveRequestMessage()
        {
            string problemType = "TSP";
            ulong id = 12;
            byte[] data = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };
            ulong solvingTimeout = 1000;

            SolveRequestMessage expectedMessage = new SolveRequestMessage(problemType, data,
                                            solvingTimeout, id);
            return expectedMessage;
        }

        public static SolvePartialProblemsMessage CreateSolvePartialProblemsMessage()
        {
            string problemType = "TSP";
            ulong id = 12;
            byte[] commonData = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };
            ulong solvingTimeout = 1000;

            // fields for PartialProblem1
            ulong taskId1 = 123;
            byte[] data1 = { 24, 252, 6, 43, 57, 88 };
            ulong nodeId1 = 1;
            PartialProblem partialProblem1 = new PartialProblem(taskId1, data1, nodeId1);

            // fields for PartialProblem2
            ulong taskId2 = 321;
            byte[] data2 = { 24, 252, 6, 43, 57, 88 };
            ulong nodeId2 = 2;
            PartialProblem partialProblem2 = new PartialProblem(taskId2, data2, nodeId2);

            PartialProblem[] partialProblems = { partialProblem1, partialProblem2 };

            SolvePartialProblemsMessage expectedMessage = new SolvePartialProblemsMessage(problemType, id, commonData,
                                            solvingTimeout, partialProblems);
            return expectedMessage;
        }

        public static SolutionsMessage CreateSolutionsMessage()
        {
            string problemType = "TSP";
            ulong id = 12;
            byte[] commonData = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };

            // fields for Solution1
            ulong taskId1 = 123;
            bool timeoutOccured1 = false;
            SolutionsSolutionType typeField1 = SolutionsSolutionType.Final;
            ulong computationsTime1 = 12334;
            byte[] data1 = { 24, 252, 6, 43, 57, 88 };
            Solution solution1 = new Solution(taskId1, timeoutOccured1, typeField1, computationsTime1, data1);

            // fields for Solution2
            ulong taskId2 = 321;
            bool timeoutOccured2 = true;
            SolutionsSolutionType typeField2 = SolutionsSolutionType.Ongoing;
            ulong computationsTime2 = 43321;
            byte[] data2 = { 24, 25, 6, 3, 7, 8 };
            Solution solution2 = new Solution(taskId2, timeoutOccured2, typeField2, computationsTime2, data2);

            Solution[] solutions = { solution1, solution2 };

            SolutionsMessage expectedMessage = new SolutionsMessage(problemType, id, commonData, solutions);
            return expectedMessage;
        }

        public static SolutionRequestMessage CreateSolutionRequestMessage()
        {
            ulong id = 9;

            SolutionRequestMessage expectedMessage = new SolutionRequestMessage(id);
            return expectedMessage;
        }


        public static RegisterResponseMessage CreateRegisterResponseMessage()
        {
            ulong id = 9;
            uint timeout = 11111;
            BackupCommunicationServer[] backupServers = 
            {
                new BackupCommunicationServer("192.168.1.10", 80),
                new BackupCommunicationServer("192.168.1.11", 80),
            };

            RegisterResponseMessage expectedMessage = new RegisterResponseMessage(id, timeout, backupServers);
            return expectedMessage;
        }

        public static RegisterMessage CreateRegisterMessage()
        {
            RegisterType type = RegisterType.TaskManager;
            byte threads = 3;
            string[] problems = { "TSP", "GraphColoring" };

            RegisterMessage expectedMessage = new RegisterMessage(type, threads, problems);
            expectedMessage.Deregister = true;
            expectedMessage.Id = 5;
            return expectedMessage;
        }

        public static DivideProblemMessage CreateDivideProblemMessage()
        {
            string problemType = "TSP";
            ulong problemId = 12;
            byte[] data = {
                             1,2,4,6,5,4,32,3
                          };
            ulong nodesCount = 16;
            ulong nodeId = 8;
            DivideProblemMessage expectedMessage = new DivideProblemMessage(
                    problemType, problemId, data, nodesCount, nodeId);
            return expectedMessage;
        }


        public static NoOperationMessage CreateNoOperationMessage()
        {
            BackupCommunicationServer backupServer1
              = new BackupCommunicationServer("192.168.1.0", 80);

            BackupCommunicationServer backupServer2
                = new BackupCommunicationServer("192.168.1.0", 80);

            BackupCommunicationServer[] backupServers =
            {
                backupServer1, backupServer2
            };

            NoOperationMessage expectedMessage = new NoOperationMessage(backupServers);
            return expectedMessage;
        }
    }
}
