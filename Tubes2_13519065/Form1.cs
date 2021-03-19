using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace Connect
{


    public partial class Form1 : Form
    {
        Microsoft.Msagl.Drawing.Graph graph; // The graph that MSAGL accepts
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        // Graph viewer engine
        private Graph graf;
        public Form1()
        {
            InitializeComponent();
        }

        private void button_LoadFile_Click(object sender, EventArgs e)
        {
            // Browse Document
            openFileGraph.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileGraph.InitialDirectory = Directory.GetCurrentDirectory();

            // Show file dialog
            DialogResult result = openFileGraph.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Read input file
                using (StreamReader bacafile = new StreamReader(openFileGraph.OpenFile()))
                {
                    string baca = bacafile.ReadLine();
                    if (baca == null || baca == "0")
                    {
                        MessageBox.Show("File Kosong");
                    }
                    else
                    {
                        this.graf = new Graph(bacafile);
                        DrawGraph(this.graf);
                    }
                }
            }
        }

        private void DrawGraph(Graph graf)
        {

            List<Tuple<string, string>> visited = new List<Tuple<string, string>>();
            graph = new Microsoft.Msagl.Drawing.Graph("graph"); // Initialize new MSAGL graph                

            foreach (KeyValuePair<string, List<string>> entry1 in graf.getAdjacent())
            {
                // do something with entry.Value or entry.Key
                foreach (var entry2 in entry1.Value)
                {
                    if (!visited.Contains(Tuple.Create(entry1.Key, entry2)) && !visited.Contains(Tuple.Create(entry2, entry1.Key)))
                    {
                        graph.AddEdge(entry1.Key,entry2).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    }
                    visited.Add(Tuple.Create(entry1.Key, entry2));
                }
            }
            // Bind graph to viewer engine
            viewer.Graph = graph;
            // Bind viewer engine to the panel
            panel_DrawGraph.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_DrawGraph.Controls.Add(viewer);
            panel_DrawGraph.ResumeLayout();
        }

        private void panel_DrawGraph_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Bikin fungsi dfs bfs, trs panggil disini. Ini button submit ceunah wkwk 
            if (radioButton1.Checked)
            {
                if (comboBox3.Text == "Show Graph")
                {
                    string x = "DFS1";
                    textBox1.Text = x;
                }
                else if (comboBox3.Text == "Friend Recommendation")
                {
                    string x = "DFS2";
                    textBox1.Text = x;
                }
                else if (comboBox3.Text == "Explore Friends")
                {
                    string x = "DFS3";
                    textBox1.Text = x;
                }

            }
            if (radioButton2.Checked)
            {
                if (comboBox3.Text == "Show Graph")
                {
                    string x = "BFS1";
                    textBox1.Text = x;
                }
                else if (comboBox3.Text == "Friend Recommendation")
                {
                    string x = "BFS2";
                    textBox1.Text = x;
                }
                else if (comboBox3.Text == "Explore Friends")
                {
                    string x = "BFS3";
                    textBox1.Text = x;
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}