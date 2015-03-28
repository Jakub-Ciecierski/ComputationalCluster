using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageComponents
{
    public partial class StatusThread
    {
        private StatusThread() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">
        ///     Information if the tread is currently computing something
        /// </param>
        /// <param name="howLong">
        ///     How long it is in given state (in ms)
        /// </param>
        /// <param name="problemInstanceId">
        ///     The ID of the problem assigned when client connected
        /// </param>
        /// <param name="taskId">
        ///     The ID of the task within given problem instance
        /// </param>
        /// <param name="problemType">
        ///     The name of the type as given by TaskSolver
        /// </param>
        public StatusThread(StatusThreadState state, ulong howLong, ulong problemInstanceId, ulong taskId, string problemType) 
        {
            State = state;

            HowLong = howLong;
            HowLongSpecified = true;

            ProblemInstanceId = problemInstanceId;
            problemInstanceIdFieldSpecified = true;

            TaskId = taskId;
            taskIdFieldSpecified = true;

            ProblemType = problemType;
            ProblemTypeFieldSpecified = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">
        ///     Information if the tread is currently computing something
        /// </param>
        public StatusThread(StatusThreadState state)
        {
            State = state;
        }

        public override bool Equals(object obj)
        {
            StatusThread thread = obj as StatusThread;
            if (State == thread.State  && 
                    HowLong == thread.HowLong   &&
                    ProblemInstanceId == thread.ProblemInstanceId &&
                    TaskId == thread.TaskId     &&
                    ProblemType == thread.ProblemType)
                return true;
            return false;
        }
    }
}
