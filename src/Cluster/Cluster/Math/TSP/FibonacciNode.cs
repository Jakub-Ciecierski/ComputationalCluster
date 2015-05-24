using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    class FibonacciNode
    {
        /**
         * In our case key represents distance/key between graph's nodes.
         */
        public float key = -1;
        /**
         * Index of a vertex in graph.
         */
        public int index = -1;
        /**
         * Degree in Fibonacci heap; roots have degree 0.
         */
        public int degree = 0;
        /**
         * To indicate parent during Prim's algorithm.
         */
        public int parent = -1;
        /**
         * To save time on heap searching, because everything happens on references,
         * we can mark if node is in heap or note.
         */
        public bool inQ = false;
        /**
         * Used in consolidate().
         */
        public bool mark = false;
        /**
         * Reference to the next node (node on the right) in Fibonacci heap.
         */
        public FibonacciNode next = null;
        /**
         * Reference to the previous node (node on the left) in Fibonacci heap.
         */
        public FibonacciNode prev = null;
        /**
         * Reference to the parent node. If null, node is a root.
         */
        public FibonacciNode p = null;
        /**
         * List of children nodes for Fibonacci heap.
         */
        public DoublyLinkedList children = null;
        /**
         * List of children for preorder walk.
         */
        public LinkedList ancestors = null;
        /**
         * @param index
         */
        public FibonacciNode(int index){this.index = index;}

        /**
         *
         */
        public FibonacciNode(){}

        /**
         * Because of guard in DoublyLinkedList, extra operations while extracting next node are needed.
         *
         * @return
         */
        public FibonacciNode getNext()
        {
            /* Only guard. */
            if (next.index == -1) return null;
            /* Next node is a guard. */
            if (index != -1 && next.index == -1) return next.next;
            /* Next is normal FibonacciNode. */
            else return next;
        }
    }
}
