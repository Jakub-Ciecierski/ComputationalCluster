using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageComponents
{
    public partial class PartialProblem
    {
        private PartialProblem() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId">
        ///     Id of subproblem given by TaskManager
        /// </param>
        /// <param name="data">
        ///     Data specific for the given subproblem
        /// </param>
        /// <param name="nodeId">
        ///     The ID of the TM that is dividing the problem
        /// </param>
        public PartialProblem(ulong taskId, byte[] data, ulong nodeId)
        {
            TaskId = taskId;
            Data = data;
            NodeID = nodeId;
        }

        public override bool Equals(object obj)
        {
            PartialProblem problem = obj as PartialProblem;


            return (TaskId == problem.TaskId && NodeID == problem.NodeID &&
                             Enumerable.SequenceEqual(Data, problem.Data));
        }
    }
}
