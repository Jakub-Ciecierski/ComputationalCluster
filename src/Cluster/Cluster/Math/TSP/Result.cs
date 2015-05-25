using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
    public class Result
    {
        public int[] route;
        public float length;

        public Result(int[] route, float length)
        {
            this.route = route;
            this.length = length;
        }

    }

}
