using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{   
    /// <summary>
    /// Cóż, hiena to zwykły prymityw. Tak zawsze sądziłem, lecz dziś, 
    /// gdy snuję swój plan znakomity. I hieny przydadzą się mi.
    /// </summary>
     class NaiveProrityQueue
    {
        private List<FibonacciNode> list = new List<FibonacciNode>();

        public NaiveProrityQueue(Graph g) 
        {
            foreach(FibonacciNode v in g.v) {
                v.inQ = true;
                list.Add(v);
        }
        }
        public FibonacciNode extractMin()
        {
            FibonacciNode min = new FibonacciNode();
            min.key = float.MaxValue;
            foreach (FibonacciNode node in list)
            {
                if (node.key < min.key) min = node;
            }
            list.Remove(min);
            return min;
        }

        public void decreaseKey(FibonacciNode node, float key, float distance2D)
        {
            if (node.key < key || key < 0) return;
            node.key = key;
            node.distance2D = distance2D;
        }

        public bool isQueueEmpty()
        {
            return (list.Count == 0) ? true : false;
        }
    }
}
