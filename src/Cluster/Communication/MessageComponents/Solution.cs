using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageComponents
{
    public partial class Solution
    {
        private Solution() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId">
        ///     Id of subproblem given by TaskManager – no TaskId for
        ///     final/merged solution
        /// </param>
        /// <param name="timeoutOccured">
        ///     Indicator that the computations ended because of timeout
        /// </param>
        /// <param name="type">
        ///     Information about the status of result (Partial/Final) or status
        ///     of computations (Ongoing)
        /// </param>
        /// <param name="computationsTime">
        ///     Total amount of time used by all threads in system for computing
        ///     the solution / during the ongoing computations (in ms)
        /// </param>
        /// <param name="data">
        ///     Solution data
        /// </param>
        public Solution(ulong taskId, bool timeoutOccured,
                        SolutionsSolutionType type, ulong computationsTime, byte[] data)
        {
            TaskId = taskId;
            TimeoutOccured = timeoutOccured;
            Type = type;
            ComputationsTime = computationsTime;
            Data = data;

            taskIdFieldSpecified = true;

        }

        public override bool Equals(object obj)
        {
            Solution solution = obj as Solution;

            return (TaskId == solution.TaskId && Type == solution.Type && ComputationsTime == solution.ComputationsTime &&
                             Enumerable.SequenceEqual(Data, solution.Data));
        }
    }
}
