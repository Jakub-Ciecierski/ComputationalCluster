using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    class Graph
    {
        public int n_v;
        public int n_e;
        public FibonacciNode[] v;
        public AdjacencyList adj;

        public Graph(int n_v) {
            this.n_v = n_v;
            /* New adjacency list. */
            this.adj = new AdjacencyList(n_v);

            /* Init vertices list. */
            this.v = new FibonacciNode[n_v];
            for (int i = 0; i < n_v; i++) v[i] = new FibonacciNode(i);
        }

        public void printAdj() {
            for (int i = 0; i < n_v; i++) {
                Console.Write("[" + i + "] ");
                adj.getVertex(i).printInLine();
            }
        }

        public void addEdge(int v1, int v2, float wage) {
            if (v1 >= n_v || v1 < 0 || v2 >= n_v || v2 < 0) {
                Console.WriteLine("[Graph] addEdge(); index out of bound.");
                return;
            }

            LinkedListNode linkedListNodeV1 = new LinkedListNode(v1, wage);
            LinkedListNode linkedListNodeV2 = new LinkedListNode(v2, wage);

            adj.getVertex(v1).addToHead(linkedListNodeV2);
            adj.getVertex(v2).addToHead(linkedListNodeV1);
        }

        public float wageFunction(int v1, int v2) {
            LinkedListNode node = adj.getVertex(v1).getNodeByIndex(v2);
            return node.key;
        }


        public void printV() {
            for(int i = 0; i < v.Length; i++) {
                Console.WriteLine("("+v[i].index+") p:" + v[i].parent );
            }
        }
    }
}
