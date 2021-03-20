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
        private Button next;
        private bool nextClicked = false;
        private void next_Click(object sender, EventArgs e)
        {
            Console.WriteLine("MASUKKKK!!!!!!!!!!");
            this.nextClicked = true;
        }
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
        public void setButton(ref Button next )
        {
            this.next = next;
            this.next.Click += new System.EventHandler(this.next_Click);
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
                                while (nextClicked == false) { };
                                //nextClicked = false;
                                //activateEdge(List<string>)
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
                    while (queue.Count!=0) // Selama queue tidak kosong
                    {
                        head = queue.Peek();
                        if (head.Item2==2)
                        {
                            /* Jika isi queue sudah level 2, keluar dari loop */
                            break;
                        }

                        foreach (var node in this.adjacent[head.Item1].OrderBy(simpul => simpul).ToList())
                        {
                            answer = new List<string> (head.Item3);
                            /* Untuk setiap simpul tetangga 
                             * masukkan simpul tetangga jika belum dikunjungi */
                            if (!visited.Contains(node))
	                        {
                                visited.Add(node);
                                answer.Add(node);
                                //visualisasi graph(answer)
                                //wait until when mouse clicked ()
                                queue.Enqueue(Tuple.Create(node, head.Item2+1,answer));
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
                                list.Add(node);
                                stack.Push(Tuple.Create(node, list));
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