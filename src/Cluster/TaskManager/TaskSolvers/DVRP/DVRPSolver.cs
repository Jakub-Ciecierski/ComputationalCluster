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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Cluster.Util;

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
       /// <summary>
       /// Cut off coefficient. 
       /// </summary>
       

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
                    int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j]] - num_depots;

                    visit_location[j] = clientNodeIndex;
                    //visit_location[j] = j + num_depots;
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
            float CUT_OFF_COEFFICIENT = 0.2f;
            List<Result> results = new List<Result>();
            List<int> nextDay = new List<int>();
            float cutOffTime = CUT_OFF_COEFFICIENT * benchmark.Depot_Time_Window[0][1];
            for (int i = 0; i < partial_benchmarks.Length; i++)
            {
                results.Add(TSPTrianIneq.calculate(partial_benchmarks[i], cutOffTime));
            }
            /******************* MERGE *************************/

            for(int j = 0; j < results.Count; j++) 
            {
                for (int i = partial_benchmarks[j].Num_Depots; i < results[j].route.Length - partial_benchmarks[j].Num_Depots; i++)
                    results[j].route[i] = partial_benchmarks[j].Visit_Location[results[j].route[i] - partial_benchmarks[j].Num_Depots] + partial_benchmarks[j].Num_Depots;
            }

            for (int j = 0; j < results.Count; j++) 
            for (int i = 0; i < results[j].nextDay.Count; i++)
            {
                results[j].nextDay[i] = partial_benchmarks[j].Visit_Location[results[j].nextDay[i] - partial_benchmarks[j].Num_Depots] + partial_benchmarks[j].Num_Depots;
            }


            Console.Write("asd");
        }

        // 1) Divide
        public byte[][] DivideProblem2(int threadCount)
        {
            /****************** DESERIALIZE ************************/

            BinaryFormatter formatter = new BinaryFormatter();
            VRPParser dvrpData = (VRPParser)formatter.Deserialize(new MemoryStream(_problemData));

            /******************* DIVIDE *************************/

            // Combine coords (x, y) and time_avail (z)
            List<Point> data = new List<Point>();
            for (int i = 0; i < dvrpData.Num_Visits; i++)
            {
                List<double> point_coords = new List<double>();

                // does not include depots - which is what we want
                int loc_index = dvrpData.Visit_Location[i];

                point_coords.Add(dvrpData.Location_Coord[loc_index][0]);
                point_coords.Add(dvrpData.Location_Coord[loc_index][1]);
                point_coords.Add(dvrpData.Time_Avail[loc_index - 1] + dvrpData.Duration[loc_index - 1]);

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
                int num_depots = dvrpData.Num_Depots;
                int num_visits = cluster_indecies.Count;
                int num_locations = cluster_indecies.Count + num_depots;

                partial_benchmark.Num_Visits = num_visits;
                partial_benchmark.Num_Depots = num_depots;
                partial_benchmark.Name = dvrpData.Name;
                partial_benchmark.Num_Capacities = dvrpData.Num_Capacities;
                partial_benchmark.Num_Vehicles = 1;
                partial_benchmark.Capacites = dvrpData.Capacites;

                partial_benchmark.Depots_IDs = new int[dvrpData.Depots_IDs.Length];
                dvrpData.Depots_IDs.CopyTo(partial_benchmark.Depots_IDs, 0);

                partial_benchmark.Depot_Location = new int[dvrpData.Depot_Location.Length];
                dvrpData.Depot_Location.CopyTo(partial_benchmark.Depot_Location, 0);

                partial_benchmark.Depot_Time_Window = new int[dvrpData.Depot_Time_Window.Length][];
                for (int p = 0; p < partial_benchmark.Depot_Time_Window.Length; p++)
                {
                    partial_benchmark.Depot_Time_Window[p] = new int[dvrpData.Depot_Time_Window[p].Length];
                    dvrpData.Depot_Time_Window[p].CopyTo(partial_benchmark.Depot_Time_Window[p], 0);
                }

                /************ LOCATION_COORD ****************/
                partial_benchmark.Num_Locations = num_locations;

                int[][] location_coord = new int[num_locations][];
                // get all depots locations
                for (int j = 0; j < num_depots; j++)
                {
                    location_coord[j] = new int[2];
                    location_coord[j][0] = dvrpData.Location_Coord[j][0];
                    location_coord[j][1] = dvrpData.Location_Coord[j][1];
                }

                // get all partial clients locations
                for (int j = num_depots; j < num_locations; j++)
                {
                    location_coord[j] = new int[2];
                    int clientNodeIndex = dvrpData.Visit_Location[cluster_indecies[j - num_depots]];

                    location_coord[j][0] = dvrpData.Location_Coord[clientNodeIndex][0];
                    location_coord[j][1] = dvrpData.Location_Coord[clientNodeIndex][1];
                }
                partial_benchmark.Location_Coord = location_coord;

                /************ DEMAND ****************/
                int[] demands = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    int clientNodeIndex = dvrpData.Visit_Location[cluster_indecies[j]];
                    demands[j] = dvrpData.Demands[clientNodeIndex - num_depots];
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
                    int clientNodeIndex = dvrpData.Visit_Location[cluster_indecies[j]];
                    duration[j] = dvrpData.Duration[clientNodeIndex - num_depots];
                }
                partial_benchmark.Duration = duration;

                /************ TIME_AVAIL ****************/
                int[] time_avail = new int[num_visits];
                for (int j = 0; j < num_visits; j++)
                {
                    int clientNodeIndex = dvrpData.Visit_Location[cluster_indecies[j]];
                    time_avail[j] = dvrpData.Time_Avail[clientNodeIndex - num_depots];
                }
                partial_benchmark.Time_Avail = time_avail;

                partial_benchmarks[i] = partial_benchmark;
            }
            
            /************ SERIALIZATION ******************/
            byte[][] temporarySolution = new byte[partial_benchmarks.Count()][];   
            for (int i = 0; i < partial_benchmarks.Count(); i++)
            {

                temporarySolution[i] = DataSerialization.ObjectToByteArray(partial_benchmarks[i]);
                
            }
            return temporarySolution;
        }

        public byte[][] DivideProblem_Working(int threadCount)
        {
            /****************** DESERIALIZE ************************/

            BinaryFormatter formatter = new BinaryFormatter();
            VRPParser benchmark = (VRPParser)formatter.Deserialize(new MemoryStream(_problemData));

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
                    int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j]] - num_depots;

                    visit_location[j] = clientNodeIndex;
                    //visit_location[j] = j + num_depots;
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

            /************ SERIALIZATION ******************/
            byte[][] temporarySolution = new byte[partial_benchmarks.Count()][];
            for (int i = 0; i < partial_benchmarks.Count(); i++)
            {

                temporarySolution[i] = DataSerialization.ObjectToByteArray(partial_benchmarks[i]);

            }


            return temporarySolution;
        }

        public override byte[][] DivideProblem(int threadCount)
        {
            /****************** DESERIALIZE ************************/

            BinaryFormatter formatter = new BinaryFormatter();
            VRPParser benchmark = (VRPParser)formatter.Deserialize(new MemoryStream(_problemData));

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
            //PredictionStrength ps = new PredictionStrength(data);
            //ps.Compute(true);
            //int k = ps.BestK;
            
            int max_k = 5;
            int start_k = 1;

            // prepare byte array for all partial solutions
            int solutionsSize = 0;
            for (int i = start_k; i <= max_k; i++)
                solutionsSize += i;

            int temporarySolutionIndex = 0;
            byte[][] temporarySolution = new byte[solutionsSize][];

            for (int k = start_k; k <= max_k; k++)
            {
                // compute clusters
                KMeans clusters = new KMeans(data, k);
                clusters.Compute();

                // create k benchmarks for k solvers
                VRPParser[] partial_benchmarks = new VRPParser[k];
                for (int i = 0; i < k; i++)
                {
                    VRPParser partial_benchmark = new VRPParser();
                    List<int> cluster_indecies = clusters.GetCluterIndecies(i);

                    partial_benchmark.ID = k;

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
                        int clientNodeIndex = benchmark.Visit_Location[cluster_indecies[j]] - num_depots;

                        visit_location[j] = clientNodeIndex;
                        //visit_location[j] = j + num_depots;
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

                /************ SERIALIZATION ******************/
                for (int i = 0; i < partial_benchmarks.Count(); i++)
                {
                    temporarySolution[temporarySolutionIndex++] = DataSerialization.ObjectToByteArray(partial_benchmarks[i]);
                }
            }

            return temporarySolution;
        }

        public byte[] MergeSolution_Working(byte[][] solutions)
        {
            int size = solutions.Length;

            int[] finalRoute;
            float finalDistance = 0;

            Result[] partialSolutions = new Result[size];

            int finalRouteSize = 0;
            for (int i = 0; i < size; i++)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Result partialSolution = (Result)formatter.Deserialize(new MemoryStream(solutions[i]));
                partialSolutions[i] = partialSolution;

                finalDistance += partialSolution.length;
                finalRouteSize += partialSolution.route.Length;
            }

            finalRoute = new int[finalRouteSize + partialSolutions.Length - 1];
            int finalRouteIndex = 0;

            for (int i = 0; i < size; i++)
            {
                Result partialSolution = partialSolutions[i];
                int[] route = partialSolution.route;

                for (int j = 0; j < route.Length; j++)
                {
                    finalRoute[finalRouteIndex++] = route[j];
                }

                if(i != size - 1)
                    finalRoute[finalRouteIndex++] = -1;
            }

            Result finalSolution = new Result(finalRoute, finalDistance, null);

            byte[] data = DataSerialization.ObjectToByteArray(finalSolution);
            return data;
        }

        public override byte[] MergeSolution(byte[][] solutions)
        {
            int start_k = 1;
            int max_k = 5;

            BinaryFormatter formatter = new BinaryFormatter();

            int size = solutions.Length;

            Result[] partialSolutions = new Result[size];
            List<List<Result>> partialSolutionsByID = new List<List<Result>>();

            for (int i = 0; i < max_k; i++) 
            {
                partialSolutionsByID.Add(new List<Result>());
            }

            for (int i = 0; i < size; i++)
            {
                Result result = (Result)formatter.Deserialize(new MemoryStream(solutions[i]));
                partialSolutionsByID[result.ID - 1].Add(result);
            }

            List<int[]> finalRoutes = new List<int[]>();
            List<int> finalRoutesSize = new List<int>();

            List<float> finalDistances = new List<float>();

            List<List<int>> nextDays = new List<List<int>>();

            for(int i = 0; i < partialSolutionsByID.Count; i++)
            {
                finalRoutesSize.Add(0);
                finalDistances.Add(0);
            }

            for(int i = 0; i < partialSolutionsByID.Count; i++)
            {
                for(int j = 0; j < partialSolutionsByID[i].Count; j++)
                {
                    Result result = partialSolutionsByID[i][j];

                    finalDistances[i] += result.length;
                    finalRoutesSize[i] += result.route.Length;
                }
                finalRoutesSize[i] += partialSolutionsByID[i].Count - 1;
            }

            for (int i = 0; i < partialSolutionsByID.Count; i++)
            {
                int[] route = new int[finalRoutesSize[i]];
                finalRoutes.Add(route);
            }


            for(int i = 0; i < partialSolutionsByID.Count; i++)
            {
                nextDays.Add( new List<int>());

                int finalRouteIndex = 0;

                for(int j = 0; j < partialSolutionsByID[i].Count; j++)
                {
                    Result result = partialSolutionsByID[i][j];

                    nextDays[i].AddRange(result.nextDay);

                    int[] route = result.route;

                    for (int l = 0; l < route.Length; l++)
                    {
                        finalRoutes[i][finalRouteIndex++] = route[l];
                    }

                    if (j != partialSolutionsByID[i].Count - 1)
                        finalRoutes[i][finalRouteIndex++] = -1;
                }
            }

             // print
            for (int i = 0; i < finalRoutes.Count; i++) 
            {
                int[] finalRoute = finalRoutes[i];
                float finalDistance = finalDistances[i];

                int k = i + 1;
                SmartConsole.PrintHeader(" RESULT FOR K = " + k);

                SmartConsole.PrintLine("Distance: " + finalDistance, SmartConsole.DebugLevel.Advanced);
                string msg = "";

                for (int l = 0; l < finalRoute.Length; l++)
                {
                    if (finalRoute[l] == -1)
                        msg += "\n";
                    else
                        msg += finalRoute[l] + ", ";
                }

                SmartConsole.PrintLine("Path: \n" + msg, SmartConsole.DebugLevel.Advanced);

                string nextDayStr = "";
                for (int l = 0; l < nextDays[i].Count; l++) 
                {
                    nextDayStr += nextDays[i][l] + ", ";
                }
                if (!nextDayStr.Equals(""))
                {
                    SmartConsole.PrintLine("Next Day: \n" + nextDayStr, SmartConsole.DebugLevel.Advanced);
                }
            }
            
            // Find min
            int minIndex = 0;
            float min = finalDistances[minIndex];

            for (int i = 0; i < finalDistances.Count; i++) 
            {
                float dinstance = finalDistances[i];
                if(min > dinstance )
                {
                    min = dinstance;
                    minIndex = i;
                }
            }

            Result finalSolution = new Result(finalRoutes[minIndex], finalDistances[minIndex], nextDays[minIndex]);
           
            byte[] data = DataSerialization.ObjectToByteArray(finalSolution);
            return data;
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
