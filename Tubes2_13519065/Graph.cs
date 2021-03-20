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
                Console.WriteLine(entry1.Key+" : "+string.Join("\t", entry1.Value));
            }
            this.totalNodes = this.adjacent.Count;
        }
        public Dictionary<string, List<string>> getAdjacent()
        {
            return this.adjacent;
        }
        public string exploreFriendsText(List<string> answer)
        {
            string x = "";
            if (answer == null)
            {
                x = x + "Tidak ada jalur koneksi yang tersedia.\r\n";
                x = x + "Anda harus membangun jalur itu sendiri.\r\n";
            }
            else
            {
                x = x + String.Join(" → ", answer) + "\r\n";
                if (answer.Count == 1)
                {
                    x = x + "Its node ";
                }
                else if (answer.Count == 2)
                {
                    x = x + "mutual friend ";
                }
                else if (answer.Count == 3)
                {
                    x = x + (answer.Count - 2).ToString() + "st-degree ";
                }
                else if (answer.Count == 4)
                {
                    x = x + (answer.Count - 2).ToString() + "nd-degree ";
                }
                else if (answer.Count == 5)
                {
                    x = x + (answer.Count - 2).ToString() + "rd-degree ";
                }
                else
                {
                    x = x + (answer.Count - 2).ToString() + "th-degree ";
                }
                x = x + "connection.\r\n";
            }
            return x;
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
            /* Mengembalikan list nama akun yang berada 2 level dari target */
            public List<string> friendsRecommendation(string root)
            {
                Tuple<string, int> head;
                List<string> accRecommendations = new List<string>();
                Queue<Tuple<string, int>> queue = new Queue<Tuple<string, int>>();
                HashSet<string> visited = new HashSet<string>();

                /* Masukkan root ke queue (nama node, level) */
                queue.Enqueue(Tuple.Create(root, 0));
                head = queue.Dequeue();
                while (head.Item2 < 2) // Selama level kurang dari n
                {
                    foreach (var node in this.adjacent[head.Item1])
                    {
                        /* Masukkan simpul tetangga dari head jika belum dikunjungi */
                        if (!visited.Contains(node))
	                    {
                            visited.Add(node);
                            queue.Enqueue(Tuple.Create(node, head.Item2+1));
                    	}
                    }
                    head = queue.Dequeue();
                }
                /* EOP : Isi queue merupakan simpul level n dari root */
                /* Masukkan ke solusi */
                if (queue.Count() > 0)
                {
                    while (queue.Count() != 0)
                	{
                        head = queue.Dequeue();
                        accRecommendations.Add(head.Item1);
                	}
                }
                return accRecommendations;
            }
            /* Mengembalikan mutual friends dari root ke target */
            public List<string> getMutualFriends(string root, string target)
            {
                List<string> adjRoot = this.adjacent[root];
                List<string> adjTrgt = this.adjacent[target];
                List<string> mutualFriends = adjRoot.Intersect(adjTrgt).ToList();
                return mutualFriends;
            }
        }

        public class DFS : Graph
        {
            public DFS(Graph graf, ref Microsoft.Msagl.Drawing.Graph graphVisualizer, ref Panel draw_graph, ref Microsoft.Msagl.GraphViewerGdi.GViewer viewer)
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
                List<string> list = new List<string>();
                Stack<Tuple<string, List<string>>> stack = new Stack<Tuple<string, List<string>>>();
                HashSet<string> visited = new HashSet<string>();
                list.Add(root);
                stack.Push(Tuple.Create(root, list));
                Tuple<string, List<string>> current;
                try
                {
                    while(stack.Count!=0)
                    {
                        current = stack.Peek();
                        stack.Pop();
                        visited.Add(current.Item1);
                        Console.WriteLine($"current Node: {current.Item1} , Total elements: {current.Item2.Count}");
                        if (current.Item1==target)
                        {
                            return current.Item2;
                        }
                        foreach (var node in this.adjacent[current.Item1])
                        {
                            list = new List<string>(current.Item2);
                            if (!visited.Contains(node))
                            {
                                stack.Push(Tuple.Create(node, list));
                                list.Add(node);
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR : {e}");
                }
                return null;
            }
        }

    }
}