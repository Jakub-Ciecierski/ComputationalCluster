using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cluster.Benchmarks
{
    public class VRPParser
    {
        /******************************************************************/
        /************************** CONSTANTS *****************************/
        /******************************************************************/
        /// <summary>
        /// Constants connected with loading 'name' field.
        /// </summary>
        private static String NAME_LABEL = "NAME";

        /// <summary>
        /// Constants connected with loading 'num_depots' field.
        /// </summary>
        private static String NUM_DEPOTS_LABEL = "NUM_DEPOTS";
        private static int NUM_DEPOTS_STRIDE = 0;
        private static int NUM_DEPOTS_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'num_capacities' field.
        /// </summary>
        private static String NUM_CAPACITIES_LABEL = "NUM_CAPACITIES";
        private static int NUM_CAPACITIES_STRIDE = 0;
        private static int NUM_CAPACITIES_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'num_visits' field.
        /// </summary>
        private static String NUM_VISITS_LABEL = "NUM_VISITS";
        private static int NUM_VISITS_STRIDE = 0;
        private static int NUM_VISITS_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'num_locations' field.
        /// </summary>
        private static String NUM_LOCATIONS_LABEL = "NUM_LOCATIONS";
        private static int NUM_LOCATIONS_STRIDE = 0;
        private static int NUM_LOCATIONS_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'num_vehicles' field.
        /// </summary>
        private static String NUM_VEHICLES_LABEL = "NUM_VEHICLES";
        private static int NUM_VEHICLES_STRIDE = 0;
        private static int NUM_VEHICLES_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'capacities' field.
        /// </summary>
        private static String CAPACITIES_LABEL = "CAPACITIES";
        private static int CAPACITIES_STRIDE = 0;
        private static int CAPACITIES_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'depots_id' field.
        /// </summary>
        private static String DEPOTS_ID_LABEL = "DEPOTS";
        private static int DEPOTS_ID_STRIDE = 0;
        private static int DEPOTS_ID_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'demands' field.
        /// </summary>
        private static String DEMANDS_LABEL = "DEMAND_SECTION";
        private static int DEMANDS_STRIDE = 1;
        private static int DEMANDS_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'location_coord' field.
        /// </summary>
        private static String LOCATION_COORD_LABEL = "LOCATION_COORD_SECTION";
        private static int LOCATION_COORD_STRIDE = 1;
        private static int LOCATION_COORD_SEQ_LENGTH = 2;

        /// <summary>
        /// Constants connected with loading 'depot_location' field.
        /// </summary>
        private static String DEPOT_LOCATION_LABEL = "DEPOT_LOCATION_SECTION";
        private static int DEPOT_LOCATION_STRIDE = 1;
        private static int DEPOT_LOCATION_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'visit_location' field.
        /// </summary>
        private static String VISIT_LOCATION_LABEL = "VISIT_LOCATION_SECTION";
        private static int VISIT_LOCATION_STRIDE = 1;
        private static int VISIT_LOCATION_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'duration' field.
        /// </summary>
        private static String DURATION_LABEL = "DURATION_SECTION";
        private static int DURATION_STRIDE = 1;
        private static int DURATION_SEQ_LENGTH = 1;

        /// <summary>
        /// Constants connected with loading 'depot_time_window' field.
        /// </summary>
        private static String DEPOT_TIME_WINDOW_LABEL = "DEPOT_TIME_WINDOW_SECTION";
        private static int DEPOT_TIME_WINDOW_STRIDE = 1;
        private static int DEPOT_TIME_WINDOW_SEQ_LENGTH = 2;

        /// <summary>
        /// Constants connected with loading 'time_avail' field.
        /// </summary>
        private static String TIME_AVAIL_LABEL = "TIME_AVAIL_SECTION";
        private static int TIME_AVAIL_STRIDE = 1;
        private static int TIME_AVAIL_SEQ_LENGTH = 1;

        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int num_depots;

        public int Num_Depots
        {
            get { return num_depots; }
            set { num_depots = value; }
        }

        private int num_capacities;

        public int Num_Capacities
        {
            get { return num_capacities; }
            set { num_capacities = value; }
        }

        private int num_visits;

        public int Num_Visits
        {
            get { return num_visits; }
            set { num_visits = value; }
        }

        private int num_locations;

        public int Num_Locations
        {
            get { return num_locations; }
            set { num_locations = value; }
        }

        private int num_vehicles;

        public int Num_Vehicles
        {
            get { return num_vehicles; }
            set { num_vehicles = value; }
        }

        private int capacites;

        public int Capacites
        {
            get { return capacites; }
            set { capacites = value; }
        }

        /******************************************************************/
        /************************** DATA SECTION **************************/
        /******************************************************************/

        /// <summary>
        ///     IDs of depots
        /// </summary>
        private int[] depots_ids;

        public int[] Depots_IDs
        {
            get { return depots_ids; }
            set { depots_ids = value; }
        }

        /// <summary>
        ///     How much capacitie is taken from each location
        /// </summary>
        private int[] demands;

        public int[] Demands
        {
            get { return demands; }
            set { demands = value; }
        }

        /// <summary>
        ///     Locations coded as follows:
        ///     [i] = [0][1] ... [n]
        ///     where i is the location id
        ///     and [0][1] ... [n] is the location in euclidean space (e.g. x y )
        /// </summary>
        private int[][] location_coord;

        public int[][] Location_Coord
        {
            get { return location_coord; }
            set { location_coord = value; }
        }

        /// <summary>
        ///     Simply map for depots' ids
        /// </summary>
        private int[] depot_location;

        public int[] Depot_Location
        {
            get { return depot_location; }
            set { depot_location = value; }
        }

        /// <summary>
        ///     Simply map for clients' ids
        ///     usually: [i] = i
        /// </summary>
        private int[] visit_location;

        public int[] Visit_Location
        {
            get { return visit_location; }
            set { visit_location = value; }
        }

        /// <summary>
        ///     How long does each client take to unload
        /// </summary>
        private int[] duration;

        public int[] Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        ///     When each depot starts and ends:
        ///     [depot_id] = [0][1],
        ///     where [0] = start and [1] = end
        /// </summary>
        private int[][] depot_time_window;

        public int[][] Depot_Time_Window
        {
            get { return depot_time_window; }
            set { depot_time_window = value; }
        }

        /// <summary>
        ///     When each client sent a request
        /// </summary>
        private int[] time_avail;

        public int[] Time_Avail
        {
            get { return time_avail; }
            set { time_avail = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public VRPParser()
        {
        }

        public VRPParser(string file)
        {
            parse(file);
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        /// Retrieves information about following fields:
        /// 'name',                         'num_depots',
        /// 'num_capacities',               'num_visits',
        /// 'num_locations',                'num_vehicles',
        /// 'capacites',                    'depots_ids',
        /// 'demands'                       'location_coord',
        /// 'depot_location'                'visit_location',
        /// 'duration'                      'time_avail',
        /// 'depot_time_window'
        /// </summary>
        /// <param name="file">Raw file's content.</param>
        private void parse(string file)
        {
            file = preprocessFile(file);

            /* Properties extraction. */
            extractName(file);
            extractNumOfDepots(file);
            extractNumCapacities(file);
            extractNumOfVisits(file);
            extractNumOfLocations(file);
            extractNumOfVehicles(file);
            extractCapacities(file);

            /* Data extraction. */
            extractDepots(file);
            extractDemands(file);
            extractLocationCoord(file);
            extractDepotLocation(file);
            extractVisitLocation(file);
            extractDuration(file);
            extractDepotTimeWindow(file);
            etractTimeAvail(file);
        }

        /// <summary>
        /// Extracting and storing data regarding 'name' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>     
        private void extractName(string file)
        {
            this.name = Regex.Replace(extractBlock(NAME_LABEL, file), @"\s+", "");
        }

        /// <summary>
        /// Extracting and storing data regarding 'num_depots' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>       
        private void extractNumOfDepots(string file)
        {
            this.num_depots = retrieveNumbers(
                extractBlock(NUM_DEPOTS_LABEL, file),
                NUM_DEPOTS_STRIDE,
                NUM_DEPOTS_SEQ_LENGTH).ToArray()[0];
        }

        /// <summary>
        /// Extracting and storing data regarding 'num_capacities' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param> 
        private void extractNumCapacities(string file)
        {
            this.num_capacities = retrieveNumbers(
                extractBlock(NUM_CAPACITIES_LABEL, file),
                NUM_CAPACITIES_STRIDE,
                NUM_CAPACITIES_SEQ_LENGTH).ToArray()[0];
        }

        /// <summary>
        /// Extracting and storing data regarding 'num_visits' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param> 
        private void extractNumOfVisits(string file)
        {
            this.num_visits = retrieveNumbers(
                extractBlock(NUM_VISITS_LABEL, file),
                NUM_VISITS_STRIDE,
                NUM_VISITS_SEQ_LENGTH).ToArray()[0];
        }

        /// <summary>
        /// Extracting and storing data regarding 'num_locations' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param> 
        private void extractNumOfLocations(string file)
        {
            this.num_locations = retrieveNumbers(
                extractBlock(NUM_LOCATIONS_LABEL, file),
                NUM_LOCATIONS_STRIDE,
                NUM_LOCATIONS_SEQ_LENGTH).ToArray()[0];
        }

        /// <summary>
        /// Extracting and storing data regarding 'num_vehicles' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param> 
        private void extractNumOfVehicles(string file)
        {
            this.num_vehicles = retrieveNumbers(
                extractBlock(NUM_VEHICLES_LABEL, file),
                NUM_VEHICLES_STRIDE,
                NUM_VEHICLES_SEQ_LENGTH).ToArray()[0];
        }

        /// <summary>
        /// Extracting and storing data regarding 'capacites' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param> 
        private void extractCapacities(string file)
        {
            this.capacites = retrieveNumbers(
                extractBlock(CAPACITIES_LABEL, file),
                CAPACITIES_STRIDE,
                CAPACITIES_SEQ_LENGTH).ToArray()[0];
        }

        /// <summary>
        /// Extracting and storing data regarding 'depots_ids' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>  
        private void extractDepots(string file)
        {
            this.depots_ids = retrieveNumbers(
                extractBlock(DEPOTS_ID_LABEL, file),
                DEPOTS_ID_STRIDE,
                DEPOTS_ID_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Extracting and storing data regarding 'demands' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>  
        private void extractDemands(string file)
        {
            this.demands = retrieveNumbers(
                extractBlock(DEMANDS_LABEL, file), 
                DEMANDS_STRIDE, 
                DEMANDS_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Extracting and storing data regarding 'location_coord' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>  
        private void extractLocationCoord(string file)
        {
            List<int> data = retrieveNumbers(
                extractBlock(LOCATION_COORD_LABEL, file),
                LOCATION_COORD_STRIDE,
                LOCATION_COORD_SEQ_LENGTH);
            this.location_coord = convertToPoints(data, LOCATION_COORD_SEQ_LENGTH).ToArray();
            
        }

        /// <summary>
        /// Extracting and storing data regarding 'depot_location' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>  
        private void extractDepotLocation(string file)
        {
            this.depot_location = retrieveNumbers(
                extractBlock(DEPOT_LOCATION_LABEL, file),
                DEPOT_LOCATION_STRIDE,
                DEPOT_LOCATION_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Extracting and storing data regarding 'visit_location' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>      
        private void extractVisitLocation(string file)
        {
            this.visit_location = retrieveNumbers(
                extractBlock(VISIT_LOCATION_LABEL, file),
                VISIT_LOCATION_STRIDE,
                VISIT_LOCATION_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Extracting and storing data regarding 'duration' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>
        private void extractDuration(string file)
        {
            this.duration = retrieveNumbers(
                extractBlock(DURATION_LABEL, file),
                DURATION_STRIDE,
                DURATION_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Extracting and storing data regarding 'time_avail' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>
        private void etractTimeAvail(string file)
        {
            this.time_avail = retrieveNumbers(
                extractBlock(TIME_AVAIL_LABEL, file),
                TIME_AVAIL_STRIDE,
                TIME_AVAIL_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Extracting and storing data regarding 'depot_time_window' field.
        /// </summary>
        /// <param name="file">Preprocessed file content.</param>
        private void extractDepotTimeWindow(string file)
        {
            List<int> data = retrieveNumbers(
                extractBlock(DEPOT_TIME_WINDOW_LABEL, file),
                DEPOT_TIME_WINDOW_STRIDE,
                DEPOT_TIME_WINDOW_SEQ_LENGTH);
            this.depot_time_window = convertToPoints(data, DEPOT_TIME_WINDOW_SEQ_LENGTH).ToArray();
        }

        /// <summary>
        /// Removes unwanted symbols from file's content. Obligatory before eny extraction.
        /// </summary>
        /// <param name="file">File's content to process.</param>
        /// <returns>Cleaned file's content.</returns>
        private string preprocessFile(string file)
        {
            return file
                        .Replace("\r\n", "")
                        .Replace("\r", "")
                        .Replace("\n", "")
                        .Replace(":", " ");
        }

        /// <summary>
        /// Returns everything between "blockName" and next string. 
        /// Border constraints are excluded from result string.
        /// </summary>
        /// <param name="blockName">Name of the block we want to extract.</param>
        /// <param name="file">String with file's content. Must be preprocessed with 'preprocessedFile()' function.</param>
        /// <returns></returns>
        private string extractBlock(string blockName, string file)
        {
            Match result = Regex.Match(file, @"(?<=[^_](" + blockName + "))(.)*?(?=[A-Z])");
            return result.Groups[0].ToString();
        }

        /// <summary>
        /// Awesome function for the whole family. Retrieves integers (negatives and non-negativee)
        /// from a provided string. Its flexibility allows to extract sequence of substrings with
        /// specified lengths.
        /// </summary>
        /// <param name="source">String full of numbers one wants to extract.</param>
        /// <param name="stride">
        /// Kind of 'space' between data we want to load e.g. one wants to load numbered integers, 
        /// then stride can be set to 1 (with seqLength = 1) and numbering will be skipped in
        /// resulting string.
        /// </param>
        /// <param name="seqLength"> Length of sequences separated by strides.</param>
        /// <returns> Sieved list of integers. </returns>
        private List<int> retrieveNumbers(string source, int stride, int seqLength)
        {
            List<int> numbers = new List<int>();
            MatchCollection matches = Regex.Matches(source, @"-?\d+");

            for (int i = stride; i < matches.Count; i += stride + seqLength)
            {
                for (int j = 0; j < seqLength; j++)
                    numbers.Add(int.Parse(matches[i + j].Groups[0].Value));
            }

            return numbers;
        }
        /// <summary>
        /// Converts one dimensional list of integers into array of smaller ones of length 'dim'.
        /// Note: method will return null if length of the list is not divisable by 'dim'.
        /// </summary>
        /// <param name="source">List to convert.</param>
        /// <param name="dim">Output points' dimension.</param>
        /// <returns>List of dim-dimensional points.</returns>
        private List<int[]> convertToPoints(List<int> source, int dim)
        {
            if (source.Count % dim != 0) return null;

            List<int[]> result = new List<int[]>();
            for (int i = 0; i < source.Count; i += dim)
            {
                int[] point = new int[dim];
                for (int j = 0; j < dim; j++) point[j] = source[i + j];
                result.Add(point);
            }

            return result;
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
