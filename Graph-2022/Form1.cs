namespace Graph_2022
{
    public partial class Form1 : Form
    {
        //private Loader l = new Loader("data.csv");
        private Graph gr;
        private GraphPainter gp;
        private ShortestPathFinder spf;
        private Path? path = null;
        
        
        private string _filename = Directory.GetFiles(
            Directory.GetCurrentDirectory() + "..\\..\\..\\..\\CSVs\\", "*", SearchOption.AllDirectories)[2];
        public Form1()
        {
            InitializeComponent();
            Load(_filename);
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if(path is null)
            gp.Paint(e.Graphics);
            else
                gp.PathPaint(e.Graphics, path);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Load(openFileDialog1.FileName);
                panel1.Refresh();
            }
        }

        private void Load(string filename)
        {
            var l = new Loader(filename);
            l.FileName = filename;
            var d = l.Load();
            gr = new Graph(d);
            gp = new GraphPainter(gr);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            gp.isResize = true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            gp.isResize = false;
            panel1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void pathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form2 = new Form2();
            form2.numericUpDown1.Maximum = gr.VertexCount;
            form2.numericUpDown1.Minimum = 1;
            form2.numericUpDown2.Maximum = gr.VertexCount;
            form2.numericUpDown2.Minimum = 1;
            if(form2.ShowDialog() == DialogResult.OK)
            {
                spf = new ShortestPathFinder(gr);
                spf.ResetPaths();
                var v1ch = (int)form2.numericUpDown1.Value - 1;
                var v2ch = (int)form2.numericUpDown2.Value - 1;
                path = spf.GetShortestPath(gr._v[v1ch < v2ch ? v1ch : v2ch],
                    gr._v[v1ch < v2ch ? v2ch : v1ch]);
                gp.path = path;
                panel1.Refresh();
            }
            form2.Dispose();
        }
    }
}