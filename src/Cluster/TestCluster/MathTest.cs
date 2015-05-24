using Communication;
using Communication.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Cluster.Benchmarks;
using Cluster.Math;
using Cluster.Math.Clustering;

namespace TestCluster
{

    [TestClass]
    public class MathTest
    {
        [TestMethod]
        public void Math_Point_Copy()
        {
            List<double> coords = new List<double>();
            coords.Add(1);
            coords.Add(1);
            Point point1 = new Point(coords);
            Point point2 = point1.Copy();
        }

        [TestMethod]
        public void Kmeans_Test1()
        {
            List<Point> data = new List<Point>();

            for (int i = 0; i < 10; i++) {
                List<double> coords = new List<double>();
                coords.Add(70);
                coords.Add(i);
                data.Add(new Point(coords));
            }
            for (int i = 0; i < 10; i++)
            {
                List<double> coords = new List<double>();
                coords.Add(-30);
                coords.Add(i);
                data.Add(new Point(coords));
            }

            KMeans clustering = new KMeans(data, 2);
            clustering.Compute();
            Console.Write("stoper");
        }


        [TestMethod]
        public void Kmeans_Test2()
        {
            List<Point> data = new List<Point>();

            // 0-7
            data.Add(new Point(5, 8));  // 0
            data.Add(new Point(7, 6));  // 1
            data.Add(new Point(4, 6));  // 2
            data.Add(new Point(3, 5));  // 3
            data.Add(new Point(5, 7));  // 0
            data.Add(new Point(7, 5));  // 1
            data.Add(new Point(3, 7));  // 2
            data.Add(new Point(8, 5));  // 3

            // 8-17
            data.Add(new Point(-6, 4)); // 4
            data.Add(new Point(-4, 3)); // 5
            data.Add(new Point(-5, 4)); // 6
            data.Add(new Point(-4, 5)); // 7
            data.Add(new Point(-3, 7)); // 8
            data.Add(new Point(-6, 4)); // 4
            data.Add(new Point(-4, 7)); // 5
            data.Add(new Point(-2, 5)); // 6
            data.Add(new Point(-3, 5)); // 7
            data.Add(new Point(-4, 6)); // 8

            // 18 -
            data.Add(new Point(5, -8)); // 9
            data.Add(new Point(7, -6)); // 10
            data.Add(new Point(4, -6)); // 11
            data.Add(new Point(3, -5)); // 12
            data.Add(new Point(3, -8)); // 9
            data.Add(new Point(5, -4)); // 10
            data.Add(new Point(4, -5)); // 11
            data.Add(new Point(6, -5)); // 12

            KMeans clustering = new KMeans(data, 3);
            clustering.Compute();
            Console.Write("stoper");
        }

        [TestMethod]
        public void PredictionStrength_KCloundGenerated_BestK()
        {
            List<Point> data = new List<Point>();

            const int CLOUD_SIZE = 500;
            const int CLOUD_COUNT = 2;
            const int DIM_SIZE = 3;

            const double BASE_STD_DEV = 7.5;
            const double CLOUD_STD_DEV = 0.5;

            double value = 0;

            RNG.SetSeedFromSystemTime();

            // for each point generate a cloud
            for (int i = 0; i < CLOUD_COUNT; i++)
            {
                Random rnd = new Random();
                // Get base Point
                List<double> coords = new List<double>();
                for(int l = 0; l < DIM_SIZE; l++)
                {
                    value = rnd.Next(0, 100);
                    coords.Add(value);
               } 
                Point point = new Point(coords);

                // Add CLOUD_SIZE points
                for (int j = 0; j < CLOUD_SIZE; j++)
                {
                    Point newPoint = point.Copy();

                    // Add some random value to each dimension.
                    for (int l = 0; l < newPoint.DimSize(); l++)
                    {
                        value = RNG.GetNormal(0, CLOUD_STD_DEV);
                        newPoint.SetDimValue(l, value + point.GetDimValue(l));
                    }

                    // Add this point to data set.
                    data.Add(newPoint);
                }
            }

            PredictionStrength ps = new PredictionStrength(data);
            ps.Compute();
        }

                [TestMethod]
        public void PredictionStrength_StaticData()
        {
            List<Point> data = new List<Point>();

            // 0-7
            data.Add(new Point(5, 8));  // 0
            data.Add(new Point(7, 6));  // 1
            data.Add(new Point(4, 6));  // 2
            data.Add(new Point(3, 5));  // 3
            data.Add(new Point(5, 7));  // 0
            data.Add(new Point(7, 5));  // 1
            data.Add(new Point(3, 7));  // 2
            data.Add(new Point(8, 5));  // 3

            // 8-17
            data.Add(new Point(-6, 4)); // 4
            data.Add(new Point(-4, 3)); // 5
            data.Add(new Point(-5, 4)); // 6
            data.Add(new Point(-4, 5)); // 7
            data.Add(new Point(-3, 7)); // 8
            data.Add(new Point(-6, 4)); // 4
            data.Add(new Point(-4, 7)); // 5
            data.Add(new Point(-2, 5)); // 6
            data.Add(new Point(-3, 5)); // 7
            data.Add(new Point(-4, 6)); // 8

            // 17 -
            data.Add(new Point(5, -8)); // 9
            data.Add(new Point(7, -6)); // 10
            data.Add(new Point(4, -6)); // 11
            data.Add(new Point(3, -5)); // 12
            data.Add(new Point(3, -8)); // 9
            data.Add(new Point(5, -4)); // 10
            data.Add(new Point(4, -5)); // 11
            data.Add(new Point(6, -5)); // 12

            /*
            for (int i = 0; i < 100; i++)
            {
                List<double> coords = new List<double>();
                coords.Add(70);
                coords.Add(0);
                data.Add(new Point(coords));
            }
            for (int i = 0; i < 100; i++)
            {
                List<double> coords = new List<double>();
                coords.Add(-30);
                coords.Add(0);
                data.Add(new Point(coords));
            }*/

            PredictionStrength ps = new PredictionStrength(data);
            ps.Compute();
        }

    }
}
