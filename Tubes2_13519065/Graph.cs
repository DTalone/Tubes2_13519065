using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect
{
    internal class Graph
    {
        private Dictionary<string,string[]> adjacent;
        private int totalNodes;
        private int totalEdges;

        public Graph(int edges)
        {
            this.totalEdges = 0;
            this.totalNodes = 0;
        }
        ~Graph()
        {
            this.totalNodes = 0;
            this.totalEdges = 0;
            this.adjacent.Clear();
        }

        public class BFS
        {
            public string search(string root, string target)
            {
                return null;
            }
        }

        public class DFS
        {
            public string search(string root, string target)
            {
                return null;
            }
        }

    }
}