using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Benchmarks
{
    public class VRPParser
    {
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

        private void parse(string file)
        {

        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
