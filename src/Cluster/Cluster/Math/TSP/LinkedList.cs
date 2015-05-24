using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Math
{
    class LinkedList
    {
        public LinkedListNode root = null;
        public int nodes;
        public void addToTail(LinkedListNode linkedListNode)
        {
            if (root == null) root = linkedListNode;
            else
            {
                LinkedListNode tmp = root;
                while (tmp.next != null) tmp = tmp.next;
                tmp.next = linkedListNode;
            }
            nodes++;
        }

        public void addToHead(LinkedListNode linkedListNode)
        {
            linkedListNode.next = root;
            root = linkedListNode;
            nodes++;
        }

        public bool isIndexOnList(int index)
        {
            LinkedListNode tmp = root;
            while (tmp != null)
            {
                if (tmp.index == index) return true;
                tmp = tmp.next;
            }

            return false;
        }

        public LinkedListNode getNodeByIndex(int index)
        {
            LinkedListNode tmp = root;
            while (tmp != null)
            {
                if (tmp.index == index) return tmp;
                tmp = tmp.next;
            }

            return null;
        }

        public void printInLine(){
        LinkedListNode tmp = root;
        while(tmp != null) {
            tmp = tmp.next;
        }
    }

    }
}
