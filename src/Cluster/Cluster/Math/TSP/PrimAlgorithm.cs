using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    class PrimAlgorithm
    {
        public static void calculate(Graph g, int r)
        {
            /* init vertices list. */
            for (int i = 0; i < g.n_v; i++)
            {
                g.v[i].key = int.MaxValue;
                g.v[i].parent = -1;
            }
            /* Set root. */
            g.v[r].key = 0;
            /* Prepare queue of type min. */
            FibonacciHeap q = new FibonacciHeap(g);

            /* Until queue is empty. */
            while (q.n != 0)
            {
                FibonacciNode u = q.extractMin();
                g.v[u.index].inQ = false;

                LinkedListNode tmp = g.adj.getVertex(u.index).root;
                while (tmp != null)
                {
                    if (g.v[tmp.index].inQ && g.wageFunction(u.index, tmp.index) < g.v[tmp.index].key)
                    {
                        g.v[tmp.index].parent = u.index;
                        q.decreaseKey(g.v[tmp.index], g.wageFunction(u.index, tmp.index));
                    }
                    tmp = tmp.next;
                }
            }
        }
    }
}
