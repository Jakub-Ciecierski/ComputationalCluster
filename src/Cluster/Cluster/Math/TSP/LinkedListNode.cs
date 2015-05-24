using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    class LinkedListNode
    {
        public int index = -1;
        public float key = -1;
        public LinkedListNode next = null;

        public LinkedListNode(int index, float key)
        {
            this.index = index;
            this.key = key;
        }
    }
}
