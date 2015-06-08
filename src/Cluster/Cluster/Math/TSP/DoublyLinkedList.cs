using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math.TSP
{
    class DoublyLinkedList
    {
        public FibonacciNode guard = null;
        public int elements = 0;

        public DoublyLinkedList()
        {
            guard = new FibonacciNode();
            guard.next = guard;
            guard.prev = guard;
        }

        public void insert(FibonacciNode x)
        {
            x.next = guard.next;
            guard.next.prev = x;
            guard.next = x;
            x.prev = guard;
            elements++;
        }

        public void delete(FibonacciNode x)
        {
            x.prev.next = x.next;
            x.next.prev = x.prev;
            elements--;
        }

        public FibonacciNode extractFirst()
        {
            FibonacciNode tmp = guard.next;
            guard.next = guard.next.next;
            guard.next.prev = guard;
            return tmp;
        }

        public FibonacciNode search(FibonacciNode k)
        {
            FibonacciNode x = guard.next;
            while (x != guard && x.key != k.key) x = x.next;
            return x;
        }

        public void print() {
            FibonacciNode x = guard.next;
            while (x != guard) {
                               x = x.next;
            }
            Console.WriteLine();
        }
    }
}
