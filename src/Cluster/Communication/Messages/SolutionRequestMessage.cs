using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Messages
{
    /// <summary>
    /// Solution Request message is sent from the CC in order to check whether 
    /// the cluster has successfully computed the solution.
    /// It allows CC to be shut down and disconnected 
    /// from server during computations.
    /// </summary>
    public partial class SolutionRequestMessage
    {
        private SolutionRequestMessage() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     the ID of the problem instance assigned by the server
        /// </param>
        public  SolutionRequestMessage(ulong id) 
        {
            Id = id;
        }
    }
}
