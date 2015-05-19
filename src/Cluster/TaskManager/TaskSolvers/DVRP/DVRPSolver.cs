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

        public static void FullSolveTest(VRPParser benchmark) 
        {
            /******************* DIVIDE *************************/


            // Combine coords (x, y) and time_avail (z)
            List<Point> data = new List<Point>();
            for (int i = 0; i < benchmark.Num_Visits; i++)
            {
                List<int> point_coords = new List<int>();

                // does not include depots - which is what we want
                int loc_index = benchmark.Visit_Location[i];

                point_coords.Add(benchmark.Location_Coord[loc_index][0]);
                point_coords.Add(benchmark.Location_Coord[loc_index][1]);

                point_coords.Add(benchmark.Time_Avail[loc_index - 1] + benchmark.Duration[loc_index - 1]);

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
            VRPParser[] partial_benchmarks = new VRPParser[k];
            for (int i = 0; i < k; i++) 
            {
                int num_depots = benchmark.Num_Depots;
                VRPParser partial_benchmark = benchmark; // remind your self of references

                List<int> clustes_indecies = clusters.GetCluterIndecies(k);
                int num_visits = clustes_indecies.Count;

                /************ LOCATION_COORD ****************/
                int num_locations = clustes_indecies.Count + num_depots;
                int[][] location_coord = new int[num_locations][];
                // get all depots locations
                for (int j = 0; j < num_depots; j++)
                {
                    location_coord[j] = new int[2];
                    location_coord[j][0] = benchmark.Location_Coord[j][0];
                    location_coord[j][1] = benchmark.Location_Coord[j][1];
                }

                // get all partial clients locations
                for (int j = num_depots; j < num_locations; j++) 
                {
                    location_coord[j][0] = benchmark.Location_Coord[clustes_indecies[j]][0];
                    location_coord[j][1] = benchmark.Location_Coord[clustes_indecies[j]][1];
                }
                partial_benchmark.Location_Coord = location_coord;

                /************ DEMAND ****************/
                int[] demands = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    demands[j] = benchmark.Demands[clustes_indecies[j]];
                }
                partial_benchmark.Demands = demands;

                /************ VISIT_LOCATION ****************/
                int[] visit_location = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    visit_location[j] = j;//benchmark.Visit_Location[clustes_indecies[j]];
                }
                partial_benchmark.Visit_Location = visit_location;

                /************ DURATION ****************/
                int[] duration = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    duration[j] = benchmark.Duration[clustes_indecies[j]];
                }
                partial_benchmark.Duration = duration;

                /************ TIME_AVAIL ****************/
                int[] time_avail = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    time_avail[j] = benchmark.Time_Avail[clustes_indecies[j]];
                }
                partial_benchmark.Time_Avail = time_avail;

                partial_benchmarks[i] = partial_benchmark;
            }

            /******************* SOLVE *************************/
            // TSP ...

            /******************* MERGE *************************/
        }

        // 1) Divide
        public override byte[][] DivideProblem(int threadCount)
        {
            byte[][] temporarySolution = new byte[5][];
            for (int i = 0; i < 5; i++)
            {
                temporarySolution[i] = new byte[1];
            }

            return temporarySolution;
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
