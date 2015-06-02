using Communication;
using Communication.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Cluster.Benchmarks;

namespace TestCluster
{

    [TestClass]
    public class DRVPTest
    {
        [TestMethod]
        public void DVRP_TSP_Test()
        {
            VRPParser benchmark = TestCases.Test1();
            TaskManager.TaskSolvers.DVRP.DVRPSolver.TSPTest(benchmark);
        }

        [TestMethod]
        public void DRVP_TestCase1()
        {
            VRPParser benchmark = TestCases.Test1();
            TaskManager.TaskSolvers.DVRP.DVRPSolver.FullSolveTest(benchmark);
        }
        [TestMethod]
        public void LOAD_FILE()
        {
            ComputationalClient.Program.loadDataFromDisc(@"C:\Users\user\Desktop\cipsko2\doc\DVRP\e\io2_10_plain_e_D.vrp");
        }
       
    }



}
