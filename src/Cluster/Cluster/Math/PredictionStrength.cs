using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    /// <summary>
    ///     Computes the prediction strength, ps in short.
    ///     Split data into: Learning and Test sets
    ///     Compute k-means for both sets.
    ///     Computes the co-membership (1.) of each Test cluster
    ///     in respect to Learnings clusters.
    ///     Compute strength of each Test cluster and 
    ///     find the minimum value
    ///     
    ///     1. co-membership:
    ///     Let data be a set of n elements, then co-membership 
    ///     is an n by n matrix with (i,j)th element equal to 1
    ///     if i and j fall into the same cluster from the clusters set, 0 otherwise.
    ///     The clusters and data can come from different samples (of the same population) 
    /// </summary>
    public class PredictionStrength
    {
        private List<Point> data;

        private int start_k;

        private int max_k;

        private int best_k;

        public int BestK
        {
            get { return best_k; }
            set { best_k = value; }
        }

        public PredictionStrength(List<Point> data, int max_k = 5, int start_k = 1)
        {
            this.data = data;

            this.max_k = max_k;

            this.start_k = start_k;
        }

        /// <summary>
        ///     Runs the algorithm for configuration given by constructor
        /// </summary>
        private void psLogic() 
        {
            // 1) get learning and testing set
            List<Point> learning = new List<Point>();
            List<Point> testing = new List<Point>();

            for (int i = 0; i < data.Count; i++) 
            {
                if (i % 2 == 0)
                    learning.Add(data[i]);
                else
                    testing.Add(data[i]);
            }

            // 2) compute ps for each k and find the best_k
            int best_k = 1;
            double max_strength = 0;
            double[] strenghs = new double[max_k];

            for (int k = start_k; k <= max_k; k++)
            {
                double strength = psValue(learning, testing, k);
                if (k == start_k)
                {
                    max_strength = strength;
                    best_k = k;
                }
                else if (max_strength < strength) 
                {
                    max_strength = strength;
                    best_k = k;
                }

            }
        }

        /// <summary>
        ///     Take set of points of each Test cluster and 
        ///     compute its co-membership matrix with respect to
        ///     the set of Learning clusters.
        ///
        ///     Do it for each Test cluster (check of Test points)
        ///     while computing its strength.
        ///     The min of strengths of all clusters will be the
        ///     Predictions strength
        /// </summary>
        /// <param name="learning"></param>
        /// <param name="testing"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private double psValue(List<Point> learning, List<Point> testing, int k)
        {
            double[] strenghs = new double[k];
            
            // get clusters
            KMeans learningCluster = new KMeans(learning, k);
            KMeans testingCluster = new KMeans(testing, k);

            learningCluster.Compute();
            testingCluster.Compute();

            // compute strength for each testing cluster
            for(int i = 0;i<k;i++)
            {
                // Get cluster points from indecies
                List<int> cluster_indicies = testingCluster.GetCluterIndecies(i);
                List<Point> testingClusterPoints = new List<Point>();

                for(int j=0;j<cluster_indicies.Count;j++)
                {
                    testingClusterPoints.Add(testing[cluster_indicies[j]]);
                }

                int[][] m = co_membership(learningCluster, testingClusterPoints);

                // get strength
                double sum = 0;
                for (int j = 0; j < m.Length; j++) 
                {
                    for (int l = 0; l < m[j].Length; l++) 
                    {
                        sum += m[j][l];
                    }
                }
                strenghs[i] = sum / testingClusterPoints.Count;
            }

            // return the min strength of all testing clusters
            double min = strenghs[0];
            for (int i = 0; i < k; i++) 
            {
                if (min > strenghs[i])
                    min = strenghs[i];
            }

            return min;
        }

        /// <summary>
        ///     Let data be a set of n elements, then co-membership 
        ///     is an n by n matrix with (i,j)th element equal to 1
        ///     if i and j fall into the same cluster from the clusters set, 0 otherwise.
        ///     The clusters and data can come from different samples (of the same population) 
        /// </summary>
        /// <param name="learningCluster"></param>
        /// <param name="testingClusterPoints"></param>
        /// <returns></returns>
        private int[][] co_membership(KMeans learningCluster,List<Point> testingClusterPoints)
        {
            int len = testingClusterPoints.Count;
            int[][] m = new int[len][];
            for(int i = 0; i < len; i++)
            {
                m[i] = new int[len];
            }

            for (int i = 0; i < len; i++) 
            {
                for (int j = 0; j < len; j++)
                {
                    if (i != j) 
                    {
                        for (int k = 0; k < learningCluster.K; k++) 
                        {
                            bool i_belongs = false;
                            bool j_belongs = false;

                            i_belongs = learningCluster.IndexBelongs(k, i);

                            j_belongs = learningCluster.IndexBelongs(k, j);

                            if (i_belongs && j_belongs)
                                m[i][j] = 1;
                            else
                                m[i][j] = 0;
                        }
                    }
                }
            }

            return m;
        }

        public void Compute() 
        {
            psLogic();
        }
    }
}
