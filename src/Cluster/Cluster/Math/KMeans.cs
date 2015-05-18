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
        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public KMeans(List<Point> data, int k, double tol = 0.0001)
        {
            this.data = data;
            this.k = k;

            this.tol = tol;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void compute()
        {
            Point[] centroids = new Point[k];
            // 1) randomly select k centers
            for (int i = 0; i < k; i++)
            {
                centroids[i] = data[i];
            }

            // 2) group points to their closest centroids

            // 3) Update centers with the mean of all points belonging to the corresponding cluster
            
            // 4) repeat 3. untill convergence

        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
