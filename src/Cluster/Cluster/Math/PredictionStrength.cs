using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
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

        private void compute() 
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

            // 2) compute ps 
            int best_k = 1;
            double max_strength = 0;
            double[] strenghs = new double[max_k];

            for (int k = start_k; k <= max_k; k++)
            {
                double strength = ps(learning, testing, k);
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

        private double ps(List<Point> learning, List<Point> testing, int k)
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
                List<int> cluster_indicies = testingCluster.GetCluterIndecies(k);
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
                strenghs[k] = sum / testingClusterPoints.Count;
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
            compute();
        }
    }
}
