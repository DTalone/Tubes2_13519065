using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect
{
    internal class Graph
    {
        private List<int>[] sisi;
        private int[] depth;
        private int simpul;

        public Graph(int n)
        {
            simpul = n;
            sisi = new List<int>[n + 1];
            depth = new int[n + 1];
            for (int i = 0; i <= n; i++)
            {
                sisi[i] = new List<int>();
                depth[i] = 0;
            }
        }
        ~Graph()
        {
            for (int i = 0; i <= simpul; i++)
            {
                sisi[i] = null;
            }
            sisi = null;
            depth = null;
        }
    }
}