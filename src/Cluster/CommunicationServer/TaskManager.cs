using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer
{
    class TaskManager: Module
    {
        public TaskManager() { }
        public TaskManager(string[] solvableProblemsField,byte parallelThreadsField,IPAddress ipAddress)
        {
            this.ParallelThreadField = parallelThreadsField;
            this.SolvableProblemsField = solvableProblemsField;
            this.IpAddress = ipAddress;
            this.UpdateStatus = true;
        }
    }
}
