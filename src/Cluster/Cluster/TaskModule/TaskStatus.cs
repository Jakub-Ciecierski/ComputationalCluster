using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    public enum TaskStatus
    {
        /// <summary>
        ///     Task was sent by Computational Client
        ///     and is ready to be taken by next TM
        /// </summary>
        New,
        /// <summary>
        ///     Task is currently being divided by TM
        /// </summary>
        Dividing,
        /// <summary>
        ///     Task has been divided, and is ready
        ///     to be taken by next CN
        /// </summary>
        Divided,
        /// <summary>
        ///     Task is currently being solved by CN
        /// </summary>
        Solving,
        /// <summary>
        ///     Task has been solved by CN
        ///     If it is a base task, it means that 
        ///     all its subtasks were solved
        /// </summary>
        Solved,
        /// <summary>
        ///     Task is being merged by TM
        /// </summary>
        Merging,
        /// <summary>
        ///     Task has been merged, and is ready
        ///     for graps by client.
        /// </summary>
        Merged
    }
}
