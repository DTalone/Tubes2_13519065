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
        private Microsoft.Msagl.Drawing.Graph graphVisualizer;
        private Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        private Panel panel_DrawGraph;
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
        public void drawContainer(Microsoft.Msagl.Drawing.Graph graph)
        {
            graph.LayoutAlgorithmSettings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();
            viewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
            viewer.Graph = graph;
            // Bind graph to viewer engine
            viewer.Graph = graph;
            // Bind viewer engine to the panel
            panel_DrawGraph.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_DrawGraph.Controls.Add(viewer);
            panel_DrawGraph.ResumeLayout();
        }
        public void ganti(string a)
        {
            this.graphVisualizer.FindNode(a).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Black;
            drawContainer(this.graphVisualizer);
        }
        public void activateEdge(List<string> list)
        {
            for (var i=0;i<list.Count;i++)
            {
                this.graphVisualizer.FindNode(list[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                drawContainer(this.graphVisualizer);
            }
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
            foreach (KeyValuePair<string, List<string>> entry1 in this.adjacent)
            {
                entry1.Value.Sort();
            }
            this.totalNodes = this.adjacent.Count;
        }
        public Dictionary<string, List<string>> getAdjacent()
        {
            return this.adjacent;
        }
        public class BFS : Graph
        {
            public BFS(Graph graf, ref Microsoft.Msagl.Drawing.Graph graphVisualizer, ref Panel draw_graph, ref Microsoft.Msagl.GraphViewerGdi.GViewer viewer)
            {
                this.adjacent = graf.adjacent;
                this.totalEdges = graf.totalEdges;
                this.totalNodes = graf.totalNodes;
                this.graphVisualizer = graphVisualizer;
                this.viewer = viewer;
                this.panel_DrawGraph = draw_graph;

            }
            public string search(string root, string target)
            {
                return null;
            }
            public List<string> exploreFriends(string root, string target)
            {
                Queue<Tuple<string, List<string>>> queue = new Queue<Tuple<string, List<string>>>();
                Tuple<string, List<string>> path;
                HashSet<string> visited = new HashSet<string>();
                List<string> tmp= new List<string>();
                tmp.Add(root);
                queue.Enqueue(Tuple.Create(root, tmp));
                visited.Add(root);
                try {
                    while (queue.Count != 0)
                    {
                        path = queue.Peek();
                        Console.WriteLine($"current Node: {path.Item1} , Total elements: {path.Item2.Count}");

                        if (path.Item1 == target)
                        {
                            return path.Item2;
                        }
                        Console.WriteLine($"ini apa {path.Item1}");
                        foreach (var node in this.adjacent[path.Item1])
                        {
                            tmp = new List<string>(path.Item2);
                            if (!visited.Contains(node))
                            {
                                visited.Add(node);
                                tmp.Add(node);
                                queue.Enqueue(Tuple.Create(node, tmp));
                                //activateEdge(path.Item2);
                            }
                        }
                        queue.Dequeue();
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine($"ERROR : {e}");
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