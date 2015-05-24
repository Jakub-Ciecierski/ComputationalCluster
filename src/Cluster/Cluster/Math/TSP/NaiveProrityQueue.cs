﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
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

        public void decreaseKey(FibonacciNode node, float key)
        {
            if (node.key < key || key < 0) return;
            node.key = key;
        }

        public bool isQueueEmpty()
        {
            return (list.Count == 0) ? true : false;
        }
    }
}
