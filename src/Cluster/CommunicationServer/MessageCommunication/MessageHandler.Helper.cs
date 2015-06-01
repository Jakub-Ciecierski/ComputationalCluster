using Cluster;
using Cluster.Client;
using Communication.MessageComponents;
using Communication.Messages;
using CommunicationServer.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.MessageCommunication
{
    public partial class MessageHandler
    {
        /// <summary>
        /// function counting avaliable number of threads
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int AvaliableThreadsCount(NetworkNode node){
            int threadCount = 0;
            foreach(TaskThread th in node.TaskThreads){
                if (th.StatusThread.State == global::Communication.MessageComponents.StatusThreadState.Idle)
                    threadCount++;
            }
            return threadCount;
        }

        /// <summary>
        /// function updating status of each thread
        /// </summary>
        /// <param name="node"></param>
        /// <param name="message"></param>
        private void UpdateThreadsStatus(NetworkNode node, StatusMessage message){
            for (int i = 0; i < node.TaskThreads.Count(); i++)
            {
                node.TaskThreads[i].StatusThread.State = message.Threads[i].State;
            }
        }

        /// <summary>
        /// In this function divideProblem message is sent if there is a new task and node is has proper taskSolver.
        /// It returns true in case of completition of sending the message.
        /// </summary>
        /// <param name="numOfTask"></param>
        /// <param name="node"></param>
        /// <param name="messagePackage"></param>
        /// <returns></returns>
        private bool isMessageProblemDividedSent(int numOfTask, NetworkNode node, MessagePackage messagePackage )
        {
            if (taskTracker.Tasks[numOfTask].Status == Cluster.TaskStatus.New && taskTracker.Tasks[numOfTask].Type == node.SolvableProblems[0])
            {
                DivideProblemMessage divideProblemMessage = new DivideProblemMessage(taskTracker.Tasks[numOfTask].Type, (ulong)taskTracker.Tasks[numOfTask].ID, taskTracker.Tasks[numOfTask].BaseData, (ulong)4, (ulong)node.Id);
                taskTracker.Tasks[numOfTask].Status = Cluster.TaskStatus.Dividing;
                server.Send(messagePackage.Socket, divideProblemMessage);
                Console.Write(" >> Divide problem message has been sent.\n");
                return true;
            }
            return false;
        }
        /// <summary>
        /// in this function solution message with solved partial solution is sent (it checks if all subtasks of task are solved.
        /// It returns true in case of completition of sending the message.
        /// </summary>
        /// <param name="numberOfTask"></param>
        /// <param name="node"></param>
        /// <param name="messagePackage"></param>
        /// <returns></returns>
        private bool isMergeSolutionSent(int numberOfTask, NetworkNode node, MessagePackage messagePackage)
        {
            if (taskTracker.Tasks[numberOfTask].Status != Cluster.TaskStatus.Merging && taskTracker.Tasks[numberOfTask].Status != Cluster.TaskStatus.Merged && taskTracker.Tasks[numberOfTask].subTasks.Count != 0)
            {
                for (int j = 0; j < taskTracker.Tasks[numberOfTask].subTasks.Count; j++)
                {
                    if (taskTracker.Tasks[numberOfTask].subTasks[j].Status != Cluster.TaskStatus.Solved)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            if (taskTracker.Tasks[numberOfTask].Type == node.SolvableProblems[0])
            {
                taskTracker.Tasks[numberOfTask].Status = Cluster.TaskStatus.Merging;
                Solution[] solutions = new Solution[taskTracker.Tasks[numberOfTask].subTasks.Count];
                for (int k = 0; k < solutions.Count(); k++)
                {
                    solutions[k] = new Solution(SolutionsSolutionType.Final);
                }
                for (int j = 0; j < taskTracker.Tasks[numberOfTask].subTasks.Count; j++)
                {
                    solutions[j].Data = taskTracker.Tasks[numberOfTask].subTasks[j].Solutions[0].Data;
                }
                SolutionsMessage solutionMessage = new SolutionsMessage(taskTracker.Tasks[numberOfTask].Type, (ulong)taskTracker.Tasks[numberOfTask].ID, taskTracker.Tasks[numberOfTask].CommonData, solutions);

                server.Send(messagePackage.Socket, solutionMessage);
               Console.Write(" >>Solution Message has been sent to Task Manager\n");
               return true;        
            }
            return false;
        }
        /// <summary>
        /// this function is a response to task manager status message
        /// </summary>
        /// <param name="networkNode"></param>
        /// <param name="messagePackage"></param>
        private void ReactToTaskManagerStatusMessage(NetworkNode networkNode,MessagePackage messagePackage)
        {
            //checks if divide or merge solution message has been sent. If not than send noOperation message.
            bool hasMessageBeenSent = false;
            int numberOfAvaliableThreads = AvaliableThreadsCount(networkNode);

            for (int i = 0; i < taskTracker.Tasks.Count && numberOfAvaliableThreads > 0; i++)
            {
                if (isMessageProblemDividedSent(i, networkNode, messagePackage))
                {
                    hasMessageBeenSent = true;
                    numberOfAvaliableThreads--;
                }
            }
            // If there are avaliable threads than try to send merge solution message               
            for (int i = 0; i < taskTracker.Tasks.Count && numberOfAvaliableThreads > 0; i++)
            {
                if (isMergeSolutionSent(i, networkNode, messagePackage))
                {
                    hasMessageBeenSent = true;
                    numberOfAvaliableThreads--;
                }
            }

            //if divideProblemMessage hasn't been sent than send noOperationMessage
            if (hasMessageBeenSent == false)
            {
                NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
                server.Send(messagePackage.Socket, response);
                Console.Write(" >> Sent a NoOperation Message. 0 tasks to divide or 0 apropriate task managers\n");
            }
        }

        /// <summary>
        /// this function is a response to Computational Node status message
        /// </summary>
        /// <param name="networkNode"></param>
        /// <param name="messagePackage"></param>
        private void ReactToComputationalNodeStatusMessage(NetworkNode networkNode, MessagePackage messagePackage)
        {
            bool messageCheck = false;
            for (int i = 0; i < taskTracker.Tasks.Count; i++)
            {
                if (taskTracker.Tasks[i].Status == Cluster.TaskStatus.Divided && taskTracker.Tasks[i].Type == networkNode.SolvableProblems[0])
                {
                    //  REMEBER TO CHECK IF THERE IS A AVALIABLE THREAD ******************************************************************************************************
                    // check number of avaliable threads
                    int avaliableThreads = AvaliableThreadsCount(networkNode);
                    List<PartialProblem> partialList = new List<PartialProblem>();
                    for (int j = 0; j < taskTracker.Tasks[i].subTasks.Count && avaliableThreads > 0; j++)
                    {
                        if (taskTracker.Tasks[i].subTasks[j].Status == Cluster.TaskStatus.New)
                        {
                            avaliableThreads--;
                            partialList.Add(new PartialProblem((ulong)taskTracker.Tasks[i].ID, taskTracker.Tasks[i].subTasks[j].BaseData, (ulong)(0)));
                            taskTracker.Tasks[i].subTasks[j].Status = Cluster.TaskStatus.Solving;
                            // temporary solution @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        }
                    }
                    if (partialList.Count > 0)
                    {
                        messageCheck = SendPartialProblemsMessage(i, partialList, messagePackage);
                    }
                    if (messageCheck)
                    {
                        break;
                    }

                }
            }
            if (messageCheck == false)
            {
                NoOperationMessage response = new NoOperationMessage(systemTracker.BackupServers);
                server.Send(messagePackage.Socket, response);
                Console.Write(" >> Sent a NoOperation Message. 0 subTasks to divide or 0 apropriate computationalNodes\n");
            }
        }

        /// <summary>
        /// function sending PartialProblems message
        /// </summary>
        /// <param name="numOfTask"></param>
        /// <param name="partialList"></param>
        /// <param name="messagePackage"></param>
        /// <returns></returns>
        private bool SendPartialProblemsMessage(int numOfTask, List<PartialProblem> partialList, MessagePackage messagePackage)
        {
            PartialProblem[] partialProblems = new PartialProblem[partialList.Count];
            for (int j = 0; j < partialList.Count; j++)
            {
                partialProblems[j] = partialList[j];
            }
            SolvePartialProblemsMessage solvePartialProblemsMessage = new SolvePartialProblemsMessage(taskTracker.Tasks[numOfTask].Type, (ulong)taskTracker.Tasks[numOfTask].ID, taskTracker.Tasks[numOfTask].CommonData, (ulong)4, partialProblems);
            server.Send(messagePackage.Socket, solvePartialProblemsMessage);
            Console.Write(" >> Solve Partial Problems Message has been send (to Computational node). Number of subTasks." + partialList.Count + " \n");
            return true;
        }
    }

    
}
