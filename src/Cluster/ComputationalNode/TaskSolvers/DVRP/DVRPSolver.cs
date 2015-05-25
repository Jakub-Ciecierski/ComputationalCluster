using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalNode.TaskSolvers.DVRP
{
    public class DVRPSolver : UCCTaskSolver.TaskSolver 
    {
        public DVRPSolver(byte[] problemData)
            : base(problemData)
        {

        }

        public override byte[][] DivideProblem(int threadCount)
        {
            throw new NotImplementedException();
        }

        public override byte[] MergeSolution(byte[][] solutions)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override byte[] Solve(byte[] partialData, TimeSpan timeout)
        {

            byte[] temporarySolution = new byte[5];
            for (int i = 0; i < 5; i++)
            {
                temporarySolution[i] = (byte)i;
            }

            return temporarySolution;
        }
    }
}
