using Cluster.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.Clustering
{
    /// <summary>
    ///     Computes the prediction strength, ps in short.
    ///     1) Split training data set into: Learning and Test sets.
    ///     
    ///     2) Compute k-means for both sets.
    ///     
    ///     3) Computes the co-membership of each Test cluster
    ///     in respect to Learnings clusters.
    ///     Definition: co-membership:
    ///     Let data be a set of n elements, then co-membership 
    ///     is an n by n matrix with (i,j)th element equal to 1
    ///     if i and j fall into the same cluster from the clusters set, 0 otherwise.
    ///     The clusters and data can come from different samples (of the same population) 
    ///     
    ///     4) Compute strength of each Test cluster and 
    ///     find the minimum value.
    ///
    ///     Data boosting:
    ///     Sometimes data might be boosted, i.e. the sample is increased.
    ///     For each point in original data set, a cloud of points of size CLOUD_SIZE 
    ///     is generated around it. Normal distribution is used.
    /// </summary>
    public class PredictionStrength
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Number of points in each cloud.
        /// </summary>
        const int CLOUD_SIZE = 200;

        /// <summary>
        ///     Standard deviation which is used in Normal distribution
        /// </summary>
        const double STD_DEV = 0.2;

        /// <summary>
        ///     The training data set
        /// </summary>
        private List<Point> data;

        /// <summary>
        ///     data is split into learning and testing sets
        /// </summary>
        List<Point> learningSet;
        List<Point> testingSet;

        private int start_k;

        private int max_k;

        private int best_k;

        public int BestK
        {
            get { return best_k; }
            set { best_k = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public PredictionStrength(List<Point> data, int max_k = 5, int start_k = 1)
        {
            this.data = data;

            this.max_k = max_k;

            this.start_k = start_k;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Runs the algorithm for configuration given by constructor
        /// </summary>
        private void psLogic() 
        {
            // 1) get learning and testing set
            // 
            splitTrainingData();

            // 2) compute ps for each k and find the best_k
            best_k = 1;
            double max_strength = 0;
            double[] strengths = new double[max_k];

            for (int k = start_k; k <= max_k; k++)
            {
                double strength = psValue(learningSet, testingSet, k);
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
                SmartConsole.PrintLine("ps(" + k + ") = " + strength);
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

            if(!learningCluster.Compute())
                return 0.0;
            if (!testingCluster.Compute())
                return 0.0;

            // 3) Computes the co-membership of each Test cluster
            for(int i = 0;i<k;i++)
            {
                // Get cluster points from indecies
                List<int> cluster_indicies = testingCluster.GetCluterIndecies(i);
                List<Point> testingClusterPoints = new List<Point>();
                for(int j=0;j<cluster_indicies.Count;j++)
                {
                    testingClusterPoints.Add(testing[cluster_indicies[j]]);
                }

                // compute the co_membership
                int[][] M = co_membership(learningCluster, testingClusterPoints);
                int n = testingClusterPoints.Count;

                // 4.1) Compute strength of each Test cluster
                double sum = 0;
                for (int j = 0; j < M.Length; j++) 
                {
                    for (int l = 0; l < M[j].Length; l++) 
                    {
                        sum += M[j][l];
                    }
                }
                double divider = n * (n - 1);
                divider = divider == 0 ? 1 : divider;
                strenghs[i] = sum / divider;
            }

            // 4.2) Find the minimum strenght value.
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
        private int[][] co_membership(KMeans learningCluster, List<Point> testingClusterPoints)
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

        /// <summary>
        ///     Splits the training data
        ///     into learning and testing sets
        /// </summary>
        private void splitTrainingData()
        {
            // 1) get learning and testing set
            // 
            learningSet = new List<Point>();
            testingSet = new List<Point>();

            Random rnd = new Random();

            for (int i = 0; i < data.Count; i++)
            {
                int which_set = rnd.Next(0, 2);
                if (which_set == 0)
                    learningSet.Add(data[i]);
                else
                    testingSet.Add(data[i]);
            }
        }

        /// <summary>
        ///     For each point add a cloud of points using Normal distribution
        /// </summary>
        private void boostData()
        {
            RNG.SetSeedFromSystemTime();

            int originalDataCount = data.Count;

            // for each point generate a cloud
            for (int i = 0; i < originalDataCount; i++)
            {
                Point point = data[i];
                // Add CLOUD_SIZE points
                for (int j = 0; j < CLOUD_SIZE; j++)
                {
                    Point newPoint = point.Copy();

                    // Add some random value to each dimension.
                    for (int l = 0; l < newPoint.DimSize(); l++)
                    {
                        double value = RNG.GetNormal(0, STD_DEV);
                        newPoint.SetDimValue(l, value + point.GetDimValue(l));
                    }

                    // Add this point to data set.
                    data.Add(newPoint);
                }
            }
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runBoostData">
        ///     Whether the data should be boosted:
        ///     We increate amount of observations in order to
        ///     have a more reliable data set.
        /// </param>
        public void Compute(bool runBoostData = false) 
        {
            SmartConsole.PrintHeader("PREDICTION STRENGTH");

            if (runBoostData) {
                SmartConsole.PrintLine("Running Data Boost ... ");
                boostData();
            }
            
            psLogic();
            SmartConsole.PrintLine("The best k = " + best_k);
        }
    }
}
