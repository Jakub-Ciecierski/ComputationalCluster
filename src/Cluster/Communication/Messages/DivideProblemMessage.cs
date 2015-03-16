using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Messages
{
    /// <summary>
    /// Divide Problem is sent to TM to start the action of dividing the problem 
    /// instance to smaller tasks. TM is provided with information about 
    /// the computational power of the cluster in terms of total number
    /// of available threads. The same message is used to relay information
    /// for synchronizing info with Backup CS.
    /// </summary>
    public partial class DivideProblemMessage : Message
    {
        private DivideProblemMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     the problem type name as given by TaskSolver and Client
        /// </param>
        /// <param name="problemId">
        ///     the ID of the problem instance assigned by the server
        /// </param>
        /// <param name="data">
        ///     the problem data
        /// </param>
        /// <param name="nodesCount">
        ///     the total number of currently available threads
        /// </param>
        /// <param name="nodeId">
        ///     the ID of the TM that is dividing the problem
        /// </param>
        public DivideProblemMessage(string problemType, ulong problemId, byte[] data, ulong nodesCount, ulong nodeId) 
        {
            ProblemType = problemType;
            Id = problemId;
            Data = data;
            ComputationalNodes = nodesCount;
            NodeID = nodeId;
        }
    }
}
