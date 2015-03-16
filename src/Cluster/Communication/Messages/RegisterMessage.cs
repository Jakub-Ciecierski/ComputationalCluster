using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Messages
{
    public partial class RegisterMessage : Message
    {
        private RegisterMessage() { }

        public RegisterMessage(RegisterType type, byte parallelThreads, String[] problems)
        {
            Type = type;
            ParallelThreads = parallelThreads;
            SolvableProblems = problems;

            Deregister = false;

            DeregisterSpecified = true;
            IdSpecified = false;
        }
        public RegisterMessage(RegisterType type, byte parallelThreads, String[] problems, bool deregister, ulong id)
        {
            Type = type;
            ParallelThreads = parallelThreads;
            SolvableProblems = problems;
            Deregister = deregister;
            Id = id;

            DeregisterSpecified = true;
            IdSpecified = true;
        }

    }
}
