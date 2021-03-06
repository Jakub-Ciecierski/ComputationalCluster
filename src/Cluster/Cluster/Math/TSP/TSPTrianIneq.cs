﻿using Cluster.Benchmarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cluster.Math.TSP
{
    /// <summary>
    /// Class calculates Travelling Salesman Problem with Triangle Inequality.
    /// [1] Extract locations points from provided benchmark.
    /// [2] Convert [1]'s points into undirect graph with wages based on distances.
    /// [3] Calculate Prim's Algorithm with root set to a car's starting position.
    /// [4] Recursively traverse minimum spanning tree from [3] in PRE-ORDER fashion.
    /// [5] Create hamiltonian cycle by connecting first and last element from [4]'s sequence.
    /// [6] Pack [5] and its length (in terms of graph's wages).
    /// [7] Return [6].
    /// </summary>
    public class TSPTrianIneq
    {

        private static float nieUmiemCSharpaWiecJestTaZmienna = 0;

        public static Result calculate(VRPParser benchmark, float cutOffTime)
        {
            int num_depots = benchmark.Num_Depots;
            List<int> nextDay = new List<int>();

            /* Convert benchmark into points. */
            List<Point> points = new List<Point>();
            for (int j = 0; j < benchmark.Num_Locations; j++)
            {
                int time;
                if (j - 1 < 0) time = 0;
                else time = benchmark.Time_Avail[j - 1];
                
                points.Add(new Point(benchmark.Location_Coord[j][0],benchmark.Location_Coord[j][1], time));
            }

            /* Get rid of the points which are appearing after cut off */
            List<Point> tmp = new List<Point>();
            for (int i = 0; i < points.Count; i++ )
            {
                if (points[i].Z < cutOffTime)
                {
                    tmp.Add(points[i]);

                }
                else nextDay.Add(i); 
            }
            points = tmp;
           

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
            Result result = new Result(route, nieUmiemCSharpaWiecJestTaZmienna, nextDay);

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
