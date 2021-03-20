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
        public class BFS : Graph
        {
            public BFS(Graph graf)
            {
                this.adjacent = graf.adjacent;
                this.totalEdges = graf.totalEdges;
                this.totalNodes = graf.totalNodes;
            }
            public string search(string root, string target)
            {
                return null;
            }
            public Queue<Tuple<string, string>> exploreFriends(string root, string target)
            {
                Queue<Tuple<string, Queue<Tuple<string, string>>>> temp = new Queue<Tuple<string, Queue<Tuple<string, string>>>>();
                Tuple<string, Queue<Tuple<string, string>>> path;
                HashSet<string> visited = new HashSet<string>();
                temp.Enqueue(Tuple.Create(root, new Queue<Tuple<string, string>>()));
                while (temp.Count != 0)
                {
                    path = temp.Dequeue();

                    if (path.Item1 == target && path.Item2.Count % 2 == 1)
                    {
                        return path.Item2;
                    }

                    foreach (var node in this.adjacent[path.Item1])
                    {
                        if (!visited.Contains(node))
                        {
                            visited.Add(node);
                            path.Item2.Enqueue(Tuple.Create(path.Item1, node));
                            temp.Enqueue(Tuple.Create(root, path.Item2));
                        }
                    }
                }
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

        public class DFS : Graph
        {
            public string search(string root, string target)
            {
                return null;
            }
        }

    }
}