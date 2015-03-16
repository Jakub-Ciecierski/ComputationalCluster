using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Messages
{
    /// <summary>
    /// Solutions message is used for sending info about ongoing computations, 
    /// partial and final solutions from
    /// CN, to TM and to CC and to relay information for synchronizing 
    /// info with Backup CS. In addition to
    /// sending task and solution data it also gives information about 
    /// the time it took to compute the solution
    /// and whether computations were stopped due to timeout.
    /// </summary>
    public partial class SolutionsMessage
    {
    }
}
