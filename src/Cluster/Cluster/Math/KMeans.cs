using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    public class KMeans
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private List<Point> data;

        private int k;

        public int K
        {
            get { return k; }
            private set { k = value; }
        }

        private double distance_tolerance;

        public List<List<int>> cluster_indices = new List<List<int>>();

        public Point[] centroids;

        private int max_iter;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public KMeans(List<Point> data, int k, int max_iter = 100, double distance_tolerance = 0.0001)
        {
            this.data = data;
            this.k = k;

            this.max_iter = max_iter;

            this.distance_tolerance = distance_tolerance;

            for (int i = 0; i < k; i++)
            {
                List<int> row = new List<int>();
                cluster_indices.Add(row);
            }

            centroids = new Point[k];
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Main algorithm of K-means:
        ///
        ///     1) Randomly select k centers
        ///     2) Group points to their closest centroids
        ///     3) Update centroids by computing mean
        ///     4) Repeat 2. and 3. untill convergence
        ///
        /// </summary>
        private void kmeansLogic()
        {
            int iter = 0;

            // 1) randomly select k centers
            initCentroids();

            // 2) group points to their closest centroids
            updateClusters();

            // 4) repeat 2) and 3) untill convergence
            while (iter++ < max_iter)
            {
                // 3) update centroids by computing mean
                Point[] prevCentroids = centroids;

                updateCentroids();

                // Check if the centroids are converged
                if (isConverged(centroids, prevCentroids))
                    break;

                // 2) group points to their closest centroids
                updateClusters();
            }
        }

        /// <summary>
        ///     Selects initial centroids randomly
        /// </summary>
        private void initCentroids() 
        {
            for (int i = 0; i < k; i++)
            {
                centroids[i] = data[i]; // TODO
            }
        }

        /// <summary>
        ///     Updates the centroids by computing mean of each cluster
        /// </summary>
        private void updateCentroids()
        {
            Point[] last_mean = new Point[k];

            // 3) 
            for (int j = 0; j < k; j++)
            {
                List<int> cluster = cluster_indices.ElementAt(j);
                List<Point> cluster_points = new List<Point>();

                for (int i = 0; i < cluster.Count; i++)
                {
                    cluster_points.Add(data[cluster[i]]);
                }
                centroids[j] = PointMeasures.Mean(cluster_points); // TODO cluster had no points
                last_mean[j] = centroids[j];
            }
        }

        /// <summary>
        ///     Group points to their closest centroids
        /// </summary>
        private void updateClusters()
        {
            cluster_indices = getNewList();
            for (int i = 0; i < data.Count; i++)
            {
                Point point = data[i];
                int min_index = 0;
                double min = PointMeasures.Distance(point, centroids[min_index]);

                // for every centroid
                for (int j = 0; j < k; j++)
                {
                    double distance = PointMeasures.Distance(point, centroids[j]);
                    if (min > distance)
                    {
                        min = distance;
                        min_index = j;
                    }
                }
                cluster_indices.ElementAt(min_index).Add(i);
            }
        }

        /// <summary>
        ///     Returns true if all centroids have been converged
        /// </summary>
        /// <param name="currentCentroids"></param>
        /// <param name="previousCentroids"></param>
        /// <returns></returns>
        private bool isConverged(Point[] currentCentroids, Point[] previousCentroids)
        {
            // Array of flags determining convergances
            // flag[i] = 1, means that that centroid has been converged
            int size = currentCentroids.Length;
            int[] flag = new int[size];
            for (int i = 0; i < size; i++)
            {
                flag[i] = 0;
            }

            // compute the distances and check if something was changed
            // according to the tolerance.
            for (int i = 0; i < size; i++)
            {
                double distance = PointMeasures.Distance(currentCentroids[i], previousCentroids[i]);
                if (distance <= distance_tolerance)
                    flag[i] = 1;
            }

            // Check if all centroids are converged
            int flagSum = 0;
            for (int i = 0; i < size; i++)
            {
                flag[i] += flagSum;
            }
            if (flagSum == size)
                return true;
            else
                return false;
        }

        /// <summary>
        ///     used to update cluster indicies
        /// </summary>
        /// <returns></returns>
        private List<List<int>> getNewList()
        {
            List<List<int>> indices = new List<List<int>>();

            for (int i = 0; i < k; i++)
            {
                List<int> row = new List<int>();
                indices.Add(row);
            }
            return indices;
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void Compute()
        {
            kmeansLogic();
        }

        public bool IndexBelongs(int clusterIndex, int pointIndex)
        {
            for (int i = 0; i < cluster_indices[clusterIndex].Count; i++)
            {
                if (cluster_indices[clusterIndex][i] == pointIndex)
                    return true;
            }
            return false;
        }

        public List<int> GetCluterIndecies(int cluster_index) 
        {
            return cluster_indices[cluster_index];
        }
    }
}
