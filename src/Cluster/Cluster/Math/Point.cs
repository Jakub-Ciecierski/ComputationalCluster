using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    public class Point
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private List<int> coords = new List<int>();

	    public int X
        {
            get { return coords[0]; }
            set { coords[0] = value; }
        }

        public int Y
        {
            get { return coords[1]; }
            set { coords[1] = value; }
        }

        public int Z
        {
            get { return coords[2]; }
            set { coords[2] = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Point(int x, int y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(List<int> coords)
        {
            this.coords = coords;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public int ElementAt(int i)
        {
            return coords[i];
        }

        public int Size()
        {
            return coords.Count;
        }

        /*******************************************************************/
        /************************* STATIC METHODS **************************/
        /*******************************************************************/

        public static double Distance(Point p1, Point p2)
        {
            if(p1.Size() != p2.Size())
                return -1.0;
            int size = p1.Size();
            double distance = 0;

            for (int i = 0; i < size; i++)
            {
                double diff = p1.ElementAt(i) - p2.ElementAt(i);
                distance += diff * diff;
            }
            return System.Math.Sqrt(distance);
        }

        public static Point mean(List<Point> points)
        {
            List<int> mean_coords = new List<int>();
            for(int i=0;i<points[0].Size();i++)
            {
                mean_coords.Add(0);
            }

            for (int i = 0; i < points.Count; i++)
            {
                Point point = points[i];

                for (int j = 0; j < point.Size(); j++) 
                {
                    mean_coords[j] += point.ElementAt(j);
                }
                
            }

            for (int i = 0; i < mean_coords.Count; i++)
            {
                mean_coords[i] /= mean_coords.Count;
            }

            return new Point(mean_coords);
        }
        
    }
}
