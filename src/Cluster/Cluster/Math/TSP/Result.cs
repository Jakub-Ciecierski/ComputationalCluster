using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
    [Serializable]
    public class Result
    {
        public int[] route;
        public float length;
        public List<int> nextDay;
        public Result(int[] route, float length, List<int> nextDay)
        {
            this.route = route;
            this.length = length;
            this.nextDay = nextDay;

        }

    }

}
