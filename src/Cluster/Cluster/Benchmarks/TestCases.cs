using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Benchmarks
{
    public class TestCases
    {
        public static VRPParser Test1()
        {
            VRPParser parser = new VRPParser();

            parser.Name = "io2_8a";
            parser.Num_Depots = 1;
            parser.Num_Capacities = 1;
            parser.Num_Visits = 8;
            parser.Num_Locations = 9;
            parser.Num_Vehicles = 8;
            parser.Capacites = 100;

            int[] depots = {0};
            parser.Depots_IDs = depots;

            int[] demands = {-23, -26, -26, -29, -13, -17, -36, -25};
            parser.Demands = demands;

            int[][] location_coords = new int[9][]
            {
                new int[] {0,0},
                new int[] {5,61}, 
                new int[] {74,34}, 
                new int[] {-9,92}, 
                new int[] {-8,84}, 
                new int[] {-54,20}, 
                new int[] {-33,72}, 
                new int[] {-94,-79}, 
                new int[] {71,68}
            };
            parser.Location_Coord = location_coords;

            int[] depot_location = { 0 };
            parser.Depot_Location = depot_location;

            int[] visit_location = {1,2,3,4,5,6,7,8};
            parser.Visit_Location = visit_location;

            int[] duration = {20,20,20,20,20,20,20,20};
            parser.Duration = duration;

            int[][] depot_time = new int[1][]
            {
                new int[] {0, 560}
            };
            parser.Depot_Time_Window = depot_time;

            int[] time_avail = { 334, 318, 53, 292, 82, 157, 301, 128 };
            parser.Time_Avail = time_avail;

            return parser;
        }
    }
}
