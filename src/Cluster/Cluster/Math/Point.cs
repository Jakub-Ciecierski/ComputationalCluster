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
        const int xIndex = 0;
        const int yIndex = 1;
        const int zIndex = 2;

        private List<double> coords = new List<double>();

        public double X
        {
            get
            {
                if (xIndex < coords.Count)
                    return coords[xIndex];
                else
                    return 0;
            }
            set
            {
                if (xIndex < coords.Count)
                    coords[xIndex] = value;
            }
        }

        public double Y
        {
            get
            {
                if (yIndex < coords.Count)
                    return coords[yIndex];
                else
                    return 0;
            }
            set
            {
                if (yIndex < coords.Count)
                    coords[yIndex] = value;
            }
        }

        public double Z
        {
            get 
            {
                if (zIndex < coords.Count)
                    return coords[zIndex];
                else
                    return 0;
            }
            set
            {
                if (zIndex < coords.Count)
                    coords[zIndex] = value; 
            }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Point(double x, double y)
        {
            coords.Add(x);
            coords.Add(y);
        }

        public Point(double x, double y, double z)
        {
            coords.Add(x);
            coords.Add(y);
            coords.Add(z);
        }

        public Point(List<double> coords)
        {
            this.coords = coords;
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

        public void SetDimValue(int i, double value)
        {
            coords[i] = value;
        }

        public int DimSize()
        {
            return coords.Count;
        }

        public Point Copy()
        {
            List<double> newCoords = new List<double>();
            for (int i = 0; i < coords.Count;i++ )
            {
                newCoords.Add(this.coords[i]);
            }

            Point point = new Point(newCoords);
            return point;
        }
    }
}
