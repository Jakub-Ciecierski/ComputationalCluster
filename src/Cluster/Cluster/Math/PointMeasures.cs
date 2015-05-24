using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    public class PointMeasures
    {
        private PointMeasures() { }

        /*******************************************************************/
        /************************* STATIC METHODS **************************/
        /*******************************************************************/

        public static double Distance(Point p1, Point p2)
        {
            if (p1.DimSize() != p2.DimSize())
                return -1.0;
            int size = p1.DimSize();
            double distance = 0;

            for (int i = 0; i < size; i++)
            {
                double diff = p1.GetDimValue(i) - p2.GetDimValue(i);
                distance += diff * diff;
            }
            return System.Math.Sqrt(distance);
        }

        public static Point Mean(List<Point> points)
        {
            // DANGER ZONE
            // Checking if all points have the same dimension is OMITTED
            // due to performance uses.
            int dimSize = points[0].DimSize();
            int pointsCount = points.Count;

            // init coords of the mean point
            List<double> meanPointCoords = new List<double>();
            for (int i = 0; i < dimSize; i++)
            {
                meanPointCoords.Add(0);
            }

            // compute the sum of each dimension
            for (int i = 0; i < pointsCount; i++)
            {
                Point point = points[i];

                for (int j = 0; j < dimSize; j++)
                {
                    meanPointCoords[j] += point.GetDimValue(j);
                }

            }

            // devide
            for (int i = 0; i < dimSize; i++)
            {
                meanPointCoords[i] /= pointsCount;
            }

            return new Point(meanPointCoords);
        }
    }
}
