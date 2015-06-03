﻿using Cluster.Benchmarks;
using Cluster.Math.TSP;
using Cluster.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            /****************** DESERIALIZE ************************/
            BinaryFormatter formatter = new BinaryFormatter();
            VRPParser dvrpData = (VRPParser)formatter.Deserialize(new MemoryStream(partialData));
            /******************* SOLVE *************************/
            Result results = TSPTrianIneq.calculate(dvrpData);

            byte[] data = DataSerialization.ObjectToByteArray(results);

            return data;
            
           /* byte[] temporarySolution = new byte[5];
            for (int i = 0; i < 5; i++)
            {
                temporarySolution[i] = (byte)i;
            }

            return temporarySolution;*/
        }
    }
}
