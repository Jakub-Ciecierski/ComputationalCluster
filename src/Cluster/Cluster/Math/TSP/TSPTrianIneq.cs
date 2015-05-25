using Cluster.Benchmarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cluster.Math.TSP
{
    public class TSPTrianIneq
    {
        private static float nieUmiemCSharpaWiecJestTaZmienna = 0;

        public static Result calculate(VRPParser benchmark)
        {
            int num_depots = benchmark.Num_Depots;

            /* Convert benchmark into points. */
            List<Point> points = new List<Point>();
            for (int j = 0; j < benchmark.Num_Locations; j++)
            {
                int time;
                if (j - 1 < 0) time = 0;
                else time = benchmark.Time_Avail[j - 1];
                
                points.Add(new Point(benchmark.Location_Coord[j][0],benchmark.Location_Coord[j][1], time));
            }

            /* Convert points into a graph. */
            Graph graph = new Graph(points.Count);
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    graph.addEdge(i, j, euclidDistance(points[i], points[j]), distance2D(points[i], points[j]));
                }
            }
            PrimAlgorithm.calculate(graph, 0);
            int[] route = preorderWalk(graph, 0);
            Result result = new Result(route, nieUmiemCSharpaWiecJestTaZmienna);

            return result;
        }
        /*private static float calculatePath(int[] route, List)
        {
            float distance = 0;
            for (int i = 0; i < route.Length; i++)
            {
                distance += ;
            }
            return distance;
        }*/
        private static float euclidDistance(Point p1, Point p2)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(p1.X - p2.X, 2) + System.Math.Pow(p1.Y - p2.Y, 2) + System.Math.Pow(p1.Z - p2.Z, 2));
        }
        private static float distance2D(Point p1, Point p2)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(p1.X - p2.X, 2) + System.Math.Pow(p1.Y - p2.Y, 2));
        }
        private static int[] preorderWalk(Graph graph, int root)
        {
            nieUmiemCSharpaWiecJestTaZmienna = 0;
            List<int> route = new List<int>();
            traverse(graph, graph.v[root], route, 0);
            route.Add(root);
            return route.ToArray();
        }
        private static void traverse(Graph graph, FibonacciNode node, List<int> route, int index)
        {
            route.Add(node.index);
            nieUmiemCSharpaWiecJestTaZmienna += node.distance2D;
            for (int i = 0; i < graph.v.Length; i++)
            {
                if (graph.v[i].parent == node.index) traverse(graph, graph.v[i], route, ++index);
            }

        }
    }
}
