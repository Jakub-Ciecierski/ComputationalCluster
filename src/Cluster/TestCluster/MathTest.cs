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
        public void Kmeans_Test()
        {
            List<Point> data = new List<Point>();

            for (int i = 0; i < 10; i++) {
                List<double> coords = new List<double>();
                coords.Add(50);
                coords.Add(i);
                data.Add(new Point(coords));
            }
            for (int i = 0; i < 10; i++)
            {
                List<double> coords = new List<double>();
                coords.Add(-50);
                coords.Add(i);
                data.Add(new Point(coords));
            }

            KMeans clustering = new KMeans(data, 2);
            clustering.Compute();
            Console.Write("stoper");
        }

        [TestMethod]
        public void PredictionStrength_KCloundGenerated_BestK()
        {
            List<Point> data = new List<Point>();

            const int CLOUD_SIZE = 100;
            const int CLOUD_COUNT = 2;
            const int DIM_SIZE = 3;

            const double BASE_STD_DEV = 2.5;
            const double CLOUD_STD_DEV = 0.5;

            double value = 0;

            RNG.SetSeedFromSystemTime();

            // for each point generate a cloud
            for (int i = 0; i < CLOUD_COUNT; i++)
            {
                // Get base Point
                List<double> coords = new List<double>();
                for(int l = 0; l < DIM_SIZE; l++)
                {
                    coords.Add(RNG.GetNormal(0, BASE_STD_DEV));
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
    }
}
