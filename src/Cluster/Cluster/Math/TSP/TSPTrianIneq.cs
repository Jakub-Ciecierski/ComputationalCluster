using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
    public class TSPTrianIneq
    {
        public static int[] calculate(Point[] points)
        {
            /* Convert points into a graph. */
            Graph graph = new Graph(points.Length);
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    graph.addEdge(i, j, euclidDistance(points[i], points[j]));
                }
            }
            PrimAlgorithm.calculate(graph, 0);
            return preorderWalk(graph, 0);
        }

        private static float euclidDistance(Point p1, Point p2)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(p1.X - p2.X, 2) + System.Math.Pow(p1.Y - p2.Y, 2) + System.Math.Pow(p1.Z - p2.Z, 2));
        }

        private static int[] preorderWalk(Graph graph, int root)
        {
            List<int> route = new List<int>();
            traverse(graph, graph.v[root], route, 0);
            route.Add(root);
            return route.ToArray();
        }
        private static void traverse(Graph graph, FibonacciNode node, List<int> route, int index)
        {
            route.Add(node.index);
            for (int i = 0; i < graph.v.Length; i++)
            {
                if (graph.v[i].parent == node.index) traverse(graph, graph.v[i], route, ++index);
            }

        }
    }
}
