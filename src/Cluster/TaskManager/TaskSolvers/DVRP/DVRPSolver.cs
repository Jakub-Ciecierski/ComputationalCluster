using Cluster.Benchmarks;
using Cluster.Math.TSP;
using Cluster.Math;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cluster.Math.Clustering;

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

        public static void TSPTest(VRPParser benchmark) 
        {
            int k = 1;
            Point[] points = new Point[benchmark.Num_Locations];

            for (int i = 0; i <= benchmark.Num_Visits; i++)
            {
                List<double> point_coords = new List<double>();

                point_coords.Add(benchmark.Location_Coord[i][0]);
                point_coords.Add(benchmark.Location_Coord[i][1]);

                if (i == 0) point_coords.Add(0);
                else point_coords.Add(benchmark.Time_Avail[i - 1] + benchmark.Duration[i - 1]);

                points[i] = new Point(point_coords);
            }

            var watch = Stopwatch.StartNew();
           // int[] route = TSPTrianIneq.calculate(points);
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Console.Write("");
        }

        public static void FullSolveTest(VRPParser benchmark) 
        {
            /******************* DIVIDE *************************/


            // Combine coords (x, y) and time_avail (z)
            List<Point> data = new List<Point>();
            for (int i = 0; i < benchmark.Num_Visits; i++)
            {
                List<double> point_coords = new List<double>();

                // does not include depots - which is what we want
                int loc_index = benchmark.Visit_Location[i];

                point_coords.Add(benchmark.Location_Coord[loc_index][0]);
                point_coords.Add(benchmark.Location_Coord[loc_index][1]);
                point_coords.Add(benchmark.Time_Avail[loc_index - 1] + benchmark.Duration[loc_index - 1]);

                data.Add(new Point(point_coords));
            }

            // get optimal number of clusters
            PredictionStrength ps = new PredictionStrength(data);
            ps.Compute(true);
            int k = ps.BestK;

            // compute clusters
            KMeans clusters = new KMeans(data, k);
            clusters.Compute();

            // create k benchmarks for k solvers
            VRPParser[] partial_benchmarks = new VRPParser[k];
            for (int i = 0; i < k; i++) 
            {
                VRPParser partial_benchmark = new VRPParser();
                List<int> cluster_indecies = clusters.GetCluterIndecies(i);

                /************ COMMON ****************/
                int num_depots = benchmark.Num_Depots;
                int num_visits = cluster_indecies.Count;
                int num_locations = cluster_indecies.Count + num_depots;

                partial_benchmark.Num_Visits = num_visits;
                partial_benchmark.Num_Depots = num_depots;
                partial_benchmark.Name = benchmark.Name;
                partial_benchmark.Num_Capacities = benchmark.Num_Capacities;
                partial_benchmark.Num_Vehicles = 1;
                partial_benchmark.Capacites = benchmark.Capacites;

                partial_benchmark.Depots_IDs = new int[benchmark.Depots_IDs.Length];
                benchmark.Depots_IDs.CopyTo(partial_benchmark.Depots_IDs, 0);

                partial_benchmark.Depot_Location = new int[benchmark.Depot_Location.Length];
                benchmark.Depot_Location.CopyTo(partial_benchmark.Depot_Location, 0);

                partial_benchmark.Depot_Time_Window = new int[benchmark.Depot_Time_Window.Length][];
                for (int p = 0; p < partial_benchmark.Depot_Time_Window.Length; p++) 
                {
                    partial_benchmark.Depot_Time_Window[p] = new int[benchmark.Depot_Time_Window[p].Length];
                    benchmark.Depot_Time_Window[p].CopyTo(partial_benchmark.Depot_Time_Window[p], 0);
                }

                /************ LOCATION_COORD ****************/
                partial_benchmark.Num_Locations = num_locations;

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
                    location_coord[j] = new int[2];
                    int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j - num_depots]];

                    location_coord[j][0] = benchmark.Location_Coord[clientNodeIndex][0];
                    location_coord[j][1] = benchmark.Location_Coord[clientNodeIndex][1];
                }
                partial_benchmark.Location_Coord = location_coord;

                /************ DEMAND ****************/
                int[] demands = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j]];
                    demands[j] = benchmark.Demands[clientNodeIndex - num_depots];
                }
                partial_benchmark.Demands = demands;

                /************ VISIT_LOCATION ****************/
                int[] visit_location = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    visit_location[j] = j + num_depots;
                }
                partial_benchmark.Visit_Location = visit_location;

                /************ DURATION ****************/
                int[] duration = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j]];
                    duration[j] = benchmark.Duration[clientNodeIndex - num_depots];
                }
                partial_benchmark.Duration = duration;

                /************ TIME_AVAIL ****************/
                int[] time_avail = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j]];
                    time_avail[j] = benchmark.Time_Avail[clientNodeIndex - num_depots];
                }
                partial_benchmark.Time_Avail = time_avail;

                partial_benchmarks[i] = partial_benchmark;
            }

            /******************* SOLVE *************************/
            List<Result> results = new List<Result>();
            for (int i = 0; i < partial_benchmarks.Length; i++)
            {
                results.Add(TSPTrianIneq.calculate(partial_benchmarks[i]));
            }
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
            int size = 0;
            for (int i = 0; i < solutions.Count(); i++)
            {
                size += solutions[i].Count();
            }
            byte[] tmpMergedSolution = new byte[size];
            int counter = 0;
            for (int i = 0; i < solutions.Count(); i++)
            {
                for (int j = 0; j < solutions[i].Count(); j++)
                {
                    tmpMergedSolution[counter] = solutions[i][j];
                    counter++;
                }
            }
            return tmpMergedSolution;
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
