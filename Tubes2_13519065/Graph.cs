using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Connect
{
    internal class Graph
    {
        private Dictionary<string,List<string>> adjacent;
        private int totalNodes;
        private int totalEdges;
        public Graph()
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
        public int getEdges()
        {
            return this.totalEdges;
        }
        public Graph(StreamReader file)
        {
            this.totalNodes = 0;
            this.totalEdges = 0;
            this.adjacent = new Dictionary<string, List<string>>();
            while (file.Peek() >= 0)
            {
                string baca;
                List<string> existing;
                baca = file.ReadLine(); // Read file line by line
                string[] cur_line = baca.Split(' ');
                if (!this.adjacent.ContainsKey(cur_line[0]))
                {
                    this.adjacent.Add(cur_line[0], new List<string>());
                }
                if (!this.adjacent.ContainsKey(cur_line[1]))
                {
                    this.adjacent.Add(cur_line[1], new List<string>());
                }
                existing = new List<string>(this.adjacent[cur_line[0]]);
                existing.Add(cur_line[1]);
                this.adjacent[cur_line[0]] = existing;

                existing = new List<string>(this.adjacent[cur_line[1]]);
                existing.Add(cur_line[0]);
                this.adjacent[cur_line[1]] = existing;

                this.totalEdges++;
            }
            this.totalNodes = this.adjacent.Count;
        }
        public Dictionary<string, List<string>> getAdjacent()
        {
            return this.adjacent;
        }
        public class BFS
        {
            public string search(string root, string target)
            {
                return null;
            }

            public void friendsRecommendation(string root, Dictionary<string, List<string>> adjacent, List<List<string>> queue, List<List<string>> solution)
            {
                if (queue.ElementAt(0).Count <= 3) // Jika level < 2
                {
                    if (queue.ElementAt(0).Count == 3) // Jika level == 2, masukkan ke solution
                    {
                        solution.Add(queue.ElementAt(0));
                    }

                    List<string> route; // Rute graf; [A,B,C] = A->B->C
                    foreach (string node in adjacent[root].OrderBy(node => node).ToList())
                    {
                        route = queue.ElementAt(0);     // Ambil elemen pertama queue & tambahkan simpul tetangga
                        route.Add(node);
                        queue.Add(route);               // Tambahkan ke belakang queue
                        route.Clear();
                    }
                    
                    queue.RemoveAt(0);     // Dequeue
                    adjacent.Remove(root); // Hapuskan simpul root
                    List<string> nodeList = adjacent.Keys.ToList();
                    foreach (string node in nodeList)
                    {
                        // Hapuskan sisi root yang berhubungan dengan simpul lain
                        adjacent[node].Remove(root);
                    }

                    List<string> nextNode = queue.ElementAt(0); // Assign node yang ingin diproses
                    string _root = queue.ElementAt(0).Last();     // Node yang ingin diproses terdapat di akhir rute
                    friendsRecommendation(_root, adjacent, queue, solution);
                }
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