using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
    class LinkedListNode
    {
        public int index = -1;
        public float key = -1;
        public float distance2D = -1;
        public LinkedListNode next = null;

        public LinkedListNode(int index, float key, float distance2D)
        {
            this.index = index;
            this.key = key;
            this.distance2D = distance2D;
        }
    }
}
