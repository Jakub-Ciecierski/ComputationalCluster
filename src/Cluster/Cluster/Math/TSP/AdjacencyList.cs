using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    class AdjacencyList
    {
        /**
         * List of wrapped elements.
         */
        public LinkedList[] list;

        /**
         * Basic constructor.
         * 
         * v - Number of vertices to keep.
         */
        public AdjacencyList(int v) 
        {
            list = new LinkedList[v];
            for(int i = 0; i < v; i++) list[i] = new LinkedList();
        }

        /*
         * Return list of adjacent vertices.
         * 
         * index - Index of vertex, which one wants to check.
         * 
         * return Linked list with adjacent vertices. 
         */
        public LinkedList getVertex(int index) 
        {
            if (index < 0 || index >= list.Length) return null;
            return list[index];
        }
    }
}
