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

        private double tol;

        private List<List<int>> cluster_indecies = new List<List<int>>();
        private Point[] centroids;

        private int max_iter;
        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public KMeans(List<Point> data, int k, int max_iter = 100, double tol = 0.0001)
        {
            this.data = data;
            this.k = k;

            this.max_iter = max_iter;

            this.tol = tol;

            for (int i = 0; i < k; i++)
            {
                List<int> row = new List<int>();
                cluster_indecies.Add(row);
            }

            centroids = new Point[k];
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void compute()
        {
            // 1) randomly select k centers
            for (int i = 0; i < k; i++)
            {
                centroids[i] = data[i];
            }

            // 2) group points to their closest centroids

            // for every point
            for (int i = 0; i < data.Count; i++)
            {
                Point point = data[i];
                int min_index = 0;
                double min = Point.Distance(point, centroids[min_index]);

                // for every centroid
                for (int j = 0; j < k; j++) 
                {
                    double distance = Point.Distance(point, centroids[j]);
                    if (min > distance) 
                    {
                        min = distance;
                        min_index = j;
                    }
                }
                cluster_indecies.ElementAt(min_index).Add(i);
            }

            int iter = 0;
            while (iter++ < max_iter)
            {
                // 3) Update centers with the mean of all points belonging to the corresponding cluster
                for (int j = 0; j < k; j++)
                {
                    List<int> cluster = cluster_indecies.ElementAt(j);
                    List<Point> cluster_points = new List<Point>();

                    for (int i = 0; i < cluster.Count; i++)
                    {
                        cluster_points.Add(data[cluster[i]]);
                    }
                    centroids[j] = Point.mean(cluster_points);
                }

                // Find new centroids
                for (int i = 0; i < data.Count; i++)
                {
                    Point point = data[i];
                    int min_index = 0;
                    double min = Point.Distance(point, centroids[min_index]);

                    // for every centroid
                    for (int j = 0; j < k; j++)
                    {
                        double distance = Point.Distance(point, centroids[j]);
                        if (min > distance)
                        {
                            min = distance;
                            min_index = j;
                        }
                    }
                    cluster_indecies.ElementAt(min_index).Add(i);
                }
            }
            // 4) repeat 2. 3. untill convergence

        }

        private List<List<int>> getNewList()
        { 
        }


        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
