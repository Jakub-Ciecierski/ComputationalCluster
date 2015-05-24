using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
    class FibonacciHeap
    {
        public FibonacciNode min = null;
        public int n = 0;
        public List<FibonacciNode> roots = new List<FibonacciNode>();

        /**
         * Based on passed graph, heap is created.
         *
         * @param g Graph to be stored in Fibonacci heap.
         */
        public FibonacciHeap(Graph g) {
        foreach(FibonacciNode v in g.v) {
            v.inQ = true;
            insert(v);
        }
    }

        public FibonacciHeap(){}

        public void insert(FibonacciNode x)
        {
            if (this.min == null)
            {
                roots.Add(x);
                this.min = x;
            }
            else
            {
                roots.Add(x);
                if (x.key < this.min.key) this.min = x;
            }
            this.n++;
        }

        /**
         * During returning minimum element, heap's roots list is shrunk
         * using {@link #consolidate()} method.
         *
         * @return Minimal element of the heap.
         */
        public FibonacciNode extractMin()
        {
            FibonacciNode z = min;
            if (z != null)
            {
                /*  To avoid NullPointerException. */
                //if (z.children == null) z.children = new DoublyLinkedList();



                for (int i = 0; i < z.children.Count; i++)
                {
                    FibonacciNode x = z.children.First();
                    x.p = null;
                    z.children.RemoveAt(0);
                    roots.Add(x);
                }
                roots.Remove(z);

                /* Heap without z is empty. */
                if (roots.Count == 0) this.min = null;
                /* Or... */
                else
                {
                    this.min = roots.First();
                    consolidate();
                }
                this.n--;
            }
            return z;
        }

        /**
         * Tu się dopiero papieże wyprawiają...
         */
        private void consolidate()
        {
            //int upperBound = upperBoundForDegree(this.n + 1);
            FibonacciNode[] A = new FibonacciNode[500];

            for (int i = 0; i < A.Length; i++) A[i] = null;

            for(int i = 0; i < roots.Count; i++)
            {
                FibonacciNode node = roots.ElementAt(i);
                int d = node.degree;
                while (A[d] != null)
                {
                    /* Another node with the same degree as x. */
                    FibonacciNode y = A[d];
                    /* Swap x with y. */
                    if (node.key > y.key)
                    {
                        swap(node, y);
                    }
                    link(y, node);
                    A[d] = null;
                    d++;
                }
                A[d] = node;
            }

            this.min = null;
            for (int i = 0; i < 500; i++)
            {
                if (A[i] != null)
                {
                    if (this.min == null)
                    {
                        this.min = A[i];
                    }
                    else
                    {
                        roots.Add(A[i]);
                        if (A[i].key < this.min.key) this.min = A[i];
                    }
                }
            }
        }

        void swap(FibonacciNode f1, FibonacciNode f2)
        {
            FibonacciNode f3 = f2;
            f1.index = f2.index;
            f1.key = f2.key;
            f1.next = f2.next;
            f1.prev = f2.prev;
            f1.p = f2.p;
            f1.mark = f2.mark;
            f1.inQ = f2.inQ;
            f1.children = f2.children;
            // f1.degree = f2.degree;

            f2.index = f3.index;
            f2.key = f3.key;
            f2.next = f3.next;
            f2.prev = f3.prev;
            f2.p = f3.p;
            f2.mark = f3.mark;
            f2.inQ = f3.inQ;
            f2.children = f3.children;
            // f2.degree = f3.degree;
        }

        /**
         * Node y becomes x's child.
         *
         * @param y
         * @param x
         */
        private void link(FibonacciNode y, FibonacciNode x)
        {
            roots.Remove(y);
            //if (x.children == null) x.children = new DoublyLinkedList();
            x.children.Add(y);
            x.degree++;
            y.p = x;
            y.mark = false;
        }

        /**
         * Upper bound for a degree value is a floor of logarithm, with base
         * equal to golden proportion, of n.
         *
         * @param n Number of nodes.
         * @return Upper bound for a degree in Fibonacci heap.
         */
        public int upperBoundForDegree(int n)
        {
            double goldenProportion = (1 + System.Math.Sqrt(5)) / 2.0;
            return (int)(System.Math.Log(n) / System.Math.Log(goldenProportion));
        }

        /**
         * Decreases key attribute of a given node.
         *
         * @param x
         * @param k
         */
        public void decreaseKey(FibonacciNode x, float k)
        {
            if (k > x.key) return;

            x.key = k;
            FibonacciNode y = x.p;

            if (y != null && x.key < y.key)
            {
                cut(x, y);
                cascading(y);
            }
            if (x.key < this.min.key) this.min = x;

        }

        private void cut(FibonacciNode x, FibonacciNode y)
        {
            y.children.Remove(x);
            y.degree--;
            roots.Add(x);
            x.p = null;
            x.mark = false;
        }

        private void cascading(FibonacciNode y)
        {
            FibonacciNode z = y.p;
            if (z != null)
            {
                if (y.mark == false) y.mark = true;
                else
                {
                    cut(y, z);
                    cascading(z);
                }
            }
        }


        /**
         * Prints only roots;
         */
        public void print() {
        //roots.print();
       // System.out.println("min: " + min.key);
    }

        void full_print()
        {

        }
    }
}
