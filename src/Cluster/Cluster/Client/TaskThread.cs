using Communication.MessageComponents;
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
            private set { taskSolver = value; }
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
            private set { thread = value; }
        }
        

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public TaskThread(int id, UCCTaskSolver.TaskSolver taskSolver, string solvableProblem)
        {
            ID = id;
            TaskSolver = taskSolver;

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
