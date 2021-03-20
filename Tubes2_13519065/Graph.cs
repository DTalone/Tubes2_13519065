using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Msagl.Drawing;

namespace Connect
{
    internal class Graph
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
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
        public int getEdges()
        {
            return this.totalEdges;
        }
        public void drawFirstGraph()
        {
            List<Tuple<string, string>> visited = new List<Tuple<string, string>>();
            this.graphVisualizer = new Microsoft.Msagl.Drawing.Graph("graph");
            foreach (KeyValuePair<string, List<string>> entry1 in this.adjacent)
            {
                // do something with entry.Value or entry.Key
                foreach (var entry2 in entry1.Value)
                {
                    if (!visited.Contains(Tuple.Create(entry1.Key, entry2)) || !visited.Contains(Tuple.Create(entry2, entry1.Key)))
                    {
                        this.graphVisualizer.AddEdge(entry1.Key, entry2).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    }
                    visited.Add(Tuple.Create(entry1.Key, entry2));
                }
            }
            drawContainer(this.graphVisualizer);
        }
        
        public void drawEdgewithColor(Microsoft.Msagl.Drawing.Graph graph, List<string> list)
        {
            List<Tuple<string, string>> visited = new List<Tuple<string, string>>();
            Node node;
            this.graphVisualizer = new Microsoft.Msagl.Drawing.Graph();
            for (int i=0;i<list.Count-1;i++)
            {
                Edge tmp = this.graphVisualizer.AddEdge(list[i], list[i + 1]);
                tmp.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                tmp.Attr.Color = Microsoft.Msagl.Drawing.Color.Orange;
                node = this.graphVisualizer.FindNode(list[i]);
                if (node!=null)
                {
                    node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                }
                visited.Add(Tuple.Create(list[i], list[i+1]));
            }
            node = this.graphVisualizer.FindNode(list[list.Count - 1]);
            if (node != null)
            {
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
            }
            foreach (KeyValuePair<string, List<string>> entry1 in this.adjacent)
            {
                // do something with entry.Value or entry.Key
                foreach (var entry2 in entry1.Value)
                {
                    if (!visited.Contains(Tuple.Create(entry1.Key, entry2)) && !visited.Contains(Tuple.Create(entry2, entry1.Key)))
                    {
                        this.graphVisualizer.AddEdge(entry1.Key, entry2).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    }
                    visited.Add(Tuple.Create(entry1.Key, entry2));
                }
            }
            drawContainer(this.graphVisualizer);
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
            Console.WriteLine("Inisialisasi");
            foreach (KeyValuePair<string, List<string>> entry1 in this.adjacent)
            {
                entry1.Value.Sort();
                Console.WriteLine(entry1.Key+" : "+string.Join("\t", entry1.Value));
            }
            this.totalNodes = this.adjacent.Count;
        }
        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

             Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        public Dictionary<string, List<string>> getAdjacent()
        {
            return this.adjacent;
        }
        public List<string> getMutualFriends(string root, string target)
        {
            List<string> adjRoot = this.adjacent[root];
            List<string> adjTrgt = this.adjacent[target];
            List<string> mutualFriends = adjRoot.Intersect(adjTrgt).ToList();
            return mutualFriends;
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
        

        public string friendRecommendationText(string root, List<string> answer)
        {
            List<string> list = new List<string>();
            string text = "";
            foreach(var account in answer)
            {
                list = new List<string>(this.getMutualFriends(root, account));
                text = text + "Nama akun: " + account + "\r\n";
                if (answer == null)
                {
                    text = text + "Tidak dapat menghasilkan friend recommendation.\r\n";
                    text = text + "Harap memperluas koneksi Anda!\r\n";
                }
                else
                {
                    text = text + list.Count.ToString() + " mutual friend";
                    if (list.Count!=1)
                    {
                        text = text + "s";
                    }
                    text = text + ":\r\n";
                    text = text + String.Join("\r\n", list) + "\r\n";
                    text = text + "\r\n";
                }
                text = text + "\r\n";
            }
            return text;
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
                drawFirstGraph();
            }
            public string search(string root, string target)
            {
                return null;
            }
            public List<string> exploreFriends(string root, string target)
            {
                this.graphVisualizer.FindNode(root).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                this.graphVisualizer.FindNode(target).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
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
                        drawEdgewithColor(this.graphVisualizer, path.Item2);
                        this.graphVisualizer.FindNode(root).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                        this.graphVisualizer.FindNode(target).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                        wait(1000);
                        //Console.WriteLine($"current Node: {path.Item1} , Total elements: {path.Item2.Count}");

                        if (path.Item1 == target)
                        {
                            return path.Item2;
                        }
                        foreach (var node in this.adjacent[path.Item1])
                        {
                            tmp = new List<string>(path.Item2);
                            if (!visited.Contains(node))
                            {
                                visited.Add(node);
                                tmp.Add(node);
                                queue.Enqueue(Tuple.Create(node, tmp));
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
            /* Mengembalikan node yang berada 2 level dari simpul & bukan tetangga root */
            public List<string> friendsRecommendation(string root)
            {
                Tuple<string, int, List<String>> head;
                List<string> answer = new List<string>();
                Queue<Tuple<string, int, List<String>>> queue = new Queue<Tuple<string, int, List<string>>>();
                HashSet<string> visited = new HashSet<string>();

                /* Masukkan root ke queue (nama node, level, rute) */
                answer.Add(root);
                queue.Enqueue(Tuple.Create(root, 0, answer)); ;
                visited.Add(root);
                try
                {
                    while (queue.Count != 0) // Selama queue tidak kosong
                    {
                        head = queue.Peek();
                        if (head.Item2 == 2)
                        {
                            /* Jika isi queue sudah level 2, keluar dari loop */
                            break;
                        }

                        foreach (var node in this.adjacent[head.Item1].OrderBy(simpul => simpul).ToList())
                        {
                            answer = new List<string>(head.Item3);
                            /* Untuk setiap simpul tetangga 
                             * masukkan simpul tetangga jika belum dikunjungi */
                            if (!visited.Contains(node))
                            {
                                visited.Add(node);
                                answer.Add(node);
                                //visualisasi graph(answer)
                                //wait until when mouse clicked ()
                                queue.Enqueue(Tuple.Create(node, head.Item2 + 1, answer));
                            }
                        }
                        queue.Dequeue();
                    }
                    /* EOP : Isi queue merupakan simpul level 2 dari root */
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR : {e}");
                }

                Console.WriteLine(queue.Peek().Item2 + " : " + string.Join("\t", queue.Peek().Item1));
                if (queue.Count() > 0)
                {
                    /* Masukkan ke solusi */
                    answer = new List<string>();
                    while (queue.Count() != 0)
                    {
                        head = queue.Peek();
                        queue.Dequeue();
                        answer.Add(head.Item1);
                    }
                    return answer;
                }
                else
                {
                    return null;
                }
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
                this.graphVisualizer = new Microsoft.Msagl.Drawing.Graph();
                drawFirstGraph();
            }
            public string search(string root, string target)
            {
                return null;
            }
            public List<string> exploreFriends(string root, string target)
            {
                List<string> stack = new List<string>();
                HashSet<string> visited = new HashSet<string>();
                this.graphVisualizer.FindNode(root).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                this.graphVisualizer.FindNode(target).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                bool found = false;
                exploreF(root, root, target, visited, ref stack, ref found);
                if (found)
                {
                    return stack;
                }
                return null;
            }
            public void exploreF(string start, string current, string target, HashSet<string> visited, ref List<string> stack, ref bool found)
            {
                stack.Add(current);
                drawEdgewithColor(this.graphVisualizer, stack);
                this.graphVisualizer.FindNode(start).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                this.graphVisualizer.FindNode(target).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                wait(1000);
                visited.Add(current);
                if (current==target)
                {
                    found = true;
                    return;
                }
                foreach (var node in this.adjacent[current])
                {
                    if (!visited.Contains(node) && !found)
                    {
                        exploreF(start, node, target, visited, ref stack, ref found);
                    }
                }
                if (!found)
                {
                    stack.RemoveAt(stack.Count-1);
                }
            }

            /* Mengembalikan node yang berada 2 level dari simpul & bukan tetangga root */
            public List<string> friendsRecommendation(string root)
            {
                Tuple<string, int, List<String>> top;
                List<string> currRoute = new List<string>();
                List<string> answer = new List<string>();
                Stack<Tuple<string, int, List<String>>> stack = new Stack<Tuple<string, int, List<string>>>();
                HashSet<string> visited = new HashSet<string>();

                /* Masukkan root ke stack (nama node, level, rute) */
                currRoute.Add(root);
                stack.Push(Tuple.Create(root, 0, currRoute));
                visited.Add(root);
                try
                {
                    while (stack.Count!=0) // Selama stack tidak kosong
                    {
                        top = stack.Pop();
                        visited.Add(top.Item1);
                        if (top.Item2 < 2)
                        {
                            foreach (var node in this.adjacent[top.Item1].OrderByDescending(simpul => simpul).ToList())
                            {
                                currRoute = new List<string> (top.Item3);
                                /* Untuk setiap simpul tetangga
                                 * masukkan simpul tetangga jika belum dikunjungi */
                                if (!visited.Contains(node))
                                {
                                    currRoute.Add(node);
                                    stack.Push(Tuple.Create(node, top.Item2+1,currRoute));
                                }
                            }
                        }
                        else // Jika simpul level 2
                        {
                            /* Masukkan ke solusi, jika bukan tetangga root */
                            if (!this.adjacent[root].Contains(top.Item1))
                            {
                                answer.Add(top.Item1);
                            }
                        }
                    }
                    /* EOP : Isi stack kosong */
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR : {e}");
                }
                return answer;
            }
        }
    }
}