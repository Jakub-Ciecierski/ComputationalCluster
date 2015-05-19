using Cluster.Benchmarks;
using Cluster.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.TaskSolvers.DVRP
{
    /// <summary>
    ///     Algorithm works as follows
    ///     
    ///     1) Divide 
    ///         Combine coordinates (assume 2d) and time into 3d array
    ///         Compute Cluster evaluation e.g. prediction strength.
    ///         Each of k clusters is assumed to be one different vehicle. 
    ///         If there are more clusters than vehicles, take k = num_vehicles
    ///     2) Solve
    ///         By this time mTSP has been partitioned into k TSP problems.
    ///         K TSP's are computed.
    ///     3) Merge
    ///         Combine costs - maybe some other heuristics ??
    ///         
    /// </summary>
    public class DVRPSolver : UCCTaskSolver.TaskSolver
    {
        public DVRPSolver(byte[] problemData)
            : base(problemData)
        {

        }

        public void FullSolveTest() 
        {
            /******************* DIVIDE *************************/
            // Static problem data
            VRPParser benchmark = TestCases.Test1();

            // Combine coords (x, y) and time_avail (z)
            List<Point> data = new List<Point>();
            for (int i = 0; i < benchmark.Num_Visits; i++)
            {
                List<int> point_coords = new List<int>();

                // does not include depots - which is what we want
                int loc_index = benchmark.Visit_Location[i];

                point_coords.Add(benchmark.Location_Coord[loc_index][0]);
                point_coords.Add(benchmark.Location_Coord[loc_index][1]);

                point_coords.Add(benchmark.Time_Avail[loc_index - 1]);

                data.Add(new Point(point_coords));
            }

            // get optimal number of clusters
            PredictionStrength ps = new PredictionStrength(data);
            ps.Compute();
            int k = ps.BestK;

            // compute clusters
            KMeans clusters = new KMeans(data, k);
            clusters.Compute();

            // create k benchmarks for k solvers
            for (int i = 0; i < k; i++) 
            {
                int num_depots = benchmark.Num_Depots;
                VRPParser partial_benchmark = benchmark; // remind your self of references

                int num_visits = clusters.GetCluterIndecies(k).Count;
                int num_locations = clusters.GetCluterIndecies(k).Count + num_depots;

                int[][] location_coord = new int[num_locations][];
                location_coord[0] = new int[2];
                location_coord[0][0] = benchmark.Location_Coord[0][0];
                location_coord[0][1] = benchmark.Location_Coord[0][1];

                for (int j = num_depots; j < num_locations; j++) 
                {

                }

            }

            /******************* SOLVE *************************/

            /******************* MERGE *************************/
        }

        // 1) Divide
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
            // solve TSP for each cluster


            throw new NotImplementedException();
        }
    }
}
