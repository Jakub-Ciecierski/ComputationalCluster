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

       // private List<double> coords = new List<double>();
        private double[] coords = new double[3];

        public double X
        {
            get { return coords[0]; }
            set { coords[0] = value; }
        }

        public double Y
        {
            get { return coords[1]; }
            set { coords[1] = value; }
        }

        public double Z
        {
            get { return coords[2]; }
            set { coords[2] = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Point(double x, double y)
        {
           
            X = x;
            Y = y;
            Z = 0;
        }

        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(List<double> coords)
        {
           // this.coords = coords;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public double GetDimValue(int i)
        {
            return coords[i];
        }

        public int DimSize()
        {
            return coords.Length;
        }
    }
}
