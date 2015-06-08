using Cluster.Client.Messaging;
using Communication.MessageComponents;
using Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cluster
{
    /// <summary>
    ///     TM and CN has many of such TaskThread.
    ///     Each TaskThread is used to solve problems.
    /// </summary>
    public class TaskThread
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     ID of this task thread
        /// </summary>
        private int id;

        public int ID
        {
            get { return id; }
            private set { id = value; } 
        }

        private int nodeID;

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }


        /// <summary>
        ///     Current task.
        ///     Should be nulled when no task is being solved
        /// </summary>
        private Task currentTask;

        public Task CurrentTask
        {
            get { return currentTask; }
            set { currentTask = value; }
        }

        /// <summary>
        ///     Time indicating the time of how long 
        ///     has the thread been working on current task so far.
        ///     In milliseconds.
        /// </summary>
        private long runningTime;

        public long RunningTime
        {
            get { return runningTime; }
            private set { runningTime = value; }
        }

        /// <summary>
        ///     Status of this TaskThread
        /// </summary>
        private StatusThread statusThread;

        public StatusThread StatusThread
        {
            get { return statusThread; }
            private set { statusThread = value; }
        }

        /// <summary>
        ///     The task solver which the thread can work on
        ///     
        ///     Note:
        ///     Getter is currently private
        /// </summary>
        private UCCTaskSolver.TaskSolver taskSolver;

        public UCCTaskSolver.TaskSolver TaskSolver
        {
            get { return taskSolver; }
            //setter is no longer private
            set { taskSolver = value; }
        }

        /// <summary>
        ///     The task it can solve in string
        ///     
        ///     Just for standardization
        /// </summary>
        private string solvableProblem;

        public string SolvableProblem
        {
            get { return solvableProblem; }
            set { solvableProblem = value; }
        }


        /// <summary>
        ///     The system thread which
        ///     runs TaskThread in another kernel thread.
        /// </summary>
        private Thread thread;

        public Thread Thread
        {
            get { return thread; }
            // set is no longer private
            set { thread = value; }
        }

        private MessageProcessor messageProcessor;

        public MessageProcessor MessageProcessor
        {
            get { return messageProcessor; }
            set { messageProcessor = value; }
        }
        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public TaskThread(int id, string solvableProblem, MessageProcessor messageProcessor, int nodeID)
        {

            NodeID = nodeID;
            ID = id;
           // UCCTaskSolver.TaskSolver taskSolver = UCCTaskSolver.TaskSolverCreator();
           //TaskSolver = taskSolver;
            StatusThread = new StatusThread(StatusThreadState.Idle);
            MessageProcessor = messageProcessor;
            SolvableProblem = solvableProblem;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void Start()
        {
            switch (currentTask.Status)
            {
                case TaskStatus.Dividing:

                    byte[][] dividedProblems =  taskSolver.DivideProblem(4);

                    PartialProblem[] partialProblems = new PartialProblem[dividedProblems.Count()];
                    for (int i = 0; i < dividedProblems.Count(); i++)
                    {
                        partialProblems[i] = new PartialProblem((ulong)currentTask.ID, dividedProblems[i], (ulong)NodeID);
                    }
                    SolvePartialProblemsMessage solvePartialProblemsMessage = new SolvePartialProblemsMessage(currentTask.Type, (ulong) currentTask.ID, currentTask.CommonData, (ulong)4, partialProblems);
                    Console.Write(">>Sending solve partial problems message. ");
                    messageProcessor.Communicate(solvePartialProblemsMessage);
                    this.statusThread.State = StatusThreadState.Idle;
                    this.currentTask = null;
                    
                        break;
                    
                case TaskStatus.Solving:

                    byte[] solvedPartialProblem = taskSolver.Solve(currentTask.BaseData, new TimeSpan(0, 0, 5));
                    Solution[] solutions = new Solution[1];
                    //TODO subTask id, timeout checking , computations time
                    solutions[0] = new Solution((ulong)currentTask.ID, false, SolutionsSolutionType.Partial, 4000, solvedPartialProblem);

                    SolutionsMessage solutionMessage = new SolutionsMessage(currentTask.Type, (ulong)currentTask.ID, currentTask.CommonData, solutions);
                    Console.WriteLine(">>Sending solution message. ");
                    messageProcessor.Communicate(solutionMessage);
                    statusThread.State = StatusThreadState.Idle;
                    this.currentTask = null;
                    break;

                case TaskStatus.Merging:
                    byte[][] partialSolutions = new byte[currentTask.subTasks.Count()][];
                    for(int i=0;i<currentTask.subTasks.Count();i++){
                        partialSolutions[i] = currentTask.subTasks[i].BaseData;
                    }
                    byte[] mergedSolution = taskSolver.MergeSolution(partialSolutions);

                    Solution[] solution = new Solution[1];
                    //TODO subTask id, timeout checking , computations time
                    solution[0] = new Solution((ulong)currentTask.ID, false, SolutionsSolutionType.Final, 4000, mergedSolution);
                    SolutionsMessage finalSolutionMessage = new SolutionsMessage(currentTask.Type, (ulong)currentTask.ID, currentTask.CommonData, solution);
                    messageProcessor.Communicate(finalSolutionMessage);
                    this.statusThread.State = StatusThreadState.Idle;
                    this.currentTask = null;
                    break;
                
            }
            // Implement in private methods:
            // Depending on what has to be computed
            // run a task solver method.

            // TODO collect result.
        }

        public void Stop()
        {

        }
    }
}
