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
            public string search(string root, string target)
            {
                return null;
            }
            public Queue<Tuple<string, string>> exploreFriends(string root, string target)
            {
                Queue<Tuple<string, Queue<Tuple<string,string>>>> temp = new Queue<Tuple<string, Queue<Tuple<string, string>>>>();
                Tuple<string, Queue<Tuple<string, string>>> path;
                HashSet<string> visited= new HashSet<string>();
                temp.Enqueue(Tuple.Create(root, new Queue<Tuple<string, string>>()));
                while (temp.Count!=0)
                {
                    path=temp.Dequeue();
                    
                    if (path.Item1==target && path.Item2.Count%2==1)
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