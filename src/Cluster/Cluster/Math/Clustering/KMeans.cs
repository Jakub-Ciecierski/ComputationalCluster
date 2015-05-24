using Cluster.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.Clustering
{
    /// <summary>
    ///     K-means:
    ///
    ///     1) Randomly select k centers
    ///     2) Group points to their closest centroids
    ///     3) Update centroids by computing mean
    ///     4) Repeat 2. and 3. untill convergence
    ///
    ///     Data boosting:
    ///     Sometimes data might be boosted, i.e. the sample is increased.
    ///     For each point in original data set, a cloud of points of size CLOUD_SIZE 
    ///     is generated around it. Normal distribution is used.
    /// </summary>
    public class KMeans
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Data to be clustered
        /// </summary>
        private List<Point> data;

        /// <summary>
        ///     Number of clusters
        /// </summary>
        private int k;

        public int K
        {
            get { return k; }
            private set { k = value; }
        }

        /// <summary>
        ///     The distance limit before converging
        /// </summary>
        private double distance_tolerance;

        /// <summary>
        ///     Clusters
        /// </summary>
        public List<List<int>> cluster_indices = new List<List<int>>();

        /// <summary>
        ///     centers of each cluster
        /// </summary>
        public Point[] centroids;

        /// <summary>
        ///     The algorithm will not iterate more than max_iter
        /// </summary>
        private int max_iter;

        /// <summary>
        ///     Current iterations index
        /// </summary>
        private int current_iter;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public KMeans(List<Point> data, int k, int max_iter = 100, double distance_tolerance = 0.0001)
        {
            this.data = data;
            this.k = k;

            this.max_iter = max_iter;
            this.current_iter = 0;

            this.distance_tolerance = distance_tolerance;

            this.cluster_indices = refreshIndicesList();

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
            // 1) randomly select k centers
            initCentroids();

            // 2) group points to their closest centroids
            updateClusters();

            // 4) repeat 2) and 3) untill convergence
            while (current_iter++ < max_iter)
            {
                // 3) update centroids by computing mean
                Point[] prevCentroids = new Point[k];
                for (int i = 0; i < k; i++)
                    prevCentroids[i] = centroids[i];

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
                int dataIndex = i % data.Count;
                centroids[i] = data[dataIndex]; // TODO
            }
            //centroids[0] = data[0];
            //centroids[1] = data[10];
        }

        /// <summary>
        ///     Updates the centroids by computing mean of each cluster
        /// </summary>
        private void updateCentroids()
        {
            // 3) 
            for (int j = 0; j < k; j++)
            {
                List<int> cluster = cluster_indices.ElementAt(j);
                List<Point> cluster_points = new List<Point>();

                for (int i = 0; i < cluster.Count; i++)
                {
                    cluster_points.Add(data[cluster[i]]);
                }
                if(cluster_points.Count > 0)
                    centroids[j] = PointMeasures.Mean(cluster_points); // TODO cluster had no points
            }
        }

        /// <summary>
        ///     Group points to their closest centroids
        /// </summary>
        private void updateClusters()
        {
            cluster_indices = refreshIndicesList();
            for (int i = 0; i < data.Count; i++)
            {
                Point point = data[i];
                int min_index = 0;
                double min = PointMeasures.Distance(point, centroids[min_index]);

                // for every centroid
                for (int j = 0; j < k; j++)
                {
                    double distance = PointMeasures.Distance(point, centroids[j]);
                    if (min >= distance)
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
                flagSum += flag[i];
            }
            if (flagSum == size && current_iter > 1)
                return true;
            else
                return false;
        }

        /// <summary>
        ///     used to update cluster indicies
        /// </summary>
        /// <returns></returns>
        private List<List<int>> refreshIndicesList()
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

        /// <summary>
        ///     Computes the k-means
        /// </summary>
        /// <returns></returns>
        public bool Compute()
        {
            SmartConsole.PrintHeader("K-MEANS");
            if (data.Count < k) {
                SmartConsole.PrintLine("Cluster count is too big for data set, returning false");
                return false;
            }

            SmartConsole.PrintLine("The configurations:");
            SmartConsole.PrintLine("k = " + k);
            SmartConsole.PrintLine("max_iter = " + max_iter);
            SmartConsole.PrintLine("distance_tolerance = " + distance_tolerance);
            kmeansLogic();
            SmartConsole.PrintLine("K-means finished after " + current_iter + " iterations");
            return true;
        }

        /// <summary>
        ///     Checks if a given point belongs to the cluster
        /// </summary>
        /// <param name="clusterIndex"></param>
        /// <param name="pointIndex"></param>
        /// <returns></returns>
        public bool IndexBelongs(int clusterIndex, int pointIndex)
        {
            for (int i = 0; i < cluster_indices[clusterIndex].Count; i++)
            {
                if (cluster_indices[clusterIndex][i] == pointIndex)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Get a cluster
        /// </summary>
        /// <param name="cluster_index"></param>
        /// <returns></returns>
        public List<int> GetCluterIndecies(int cluster_index) 
        {
            return cluster_indices[cluster_index];
        }

        public int GetClosestCentroid(Point point) 
        {
            int minIndex = 0;
            double minDistance = PointMeasures.Distance(point, centroids[minIndex]);

            for (int i = 0; i < centroids.Length; i++) 
            {
                double distance = PointMeasures.Distance(point, centroids[i]);
                if (minDistance > distance)
                {
                    minIndex = i;
                    minDistance = distance;
                }
            }
            return minIndex;
        }
    }
}
