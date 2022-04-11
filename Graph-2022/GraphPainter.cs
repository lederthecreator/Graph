using System.Numerics;

namespace Graph_2022
{
    public delegate void DrawDatShit(Graphics g);
    public static class Extensions
    {
        public static void ToCenterVertex(this Point pt, int vertexSize, out PointF res)
        {
            res = new PointF(pt.X, pt.Y);
            res.X += (float)(vertexSize * Math.Sqrt(2));
            res.Y += (float)(vertexSize * Math.Sqrt(2));
        }

        /// <summary>
        /// Создает точки вершин графа относительно центра формы
        /// </summary>
        /// <param name="graph"> Исходный граф </param>
        /// <param name="g"> Graphics формы</param>
        /// <param name="vertexSize"> Размер вершины </param>
        /// <param name="result">List точек вершин</param>
        public static void CreatePoints(this Graph graph, Graphics g, int vertexSize, out List<Point> result)
        {
            result = new();
            var r = Rectangle.Ceiling(g.VisibleClipBounds);
            g.TranslateTransform(r.Width / 2, r.Height / 2);
            var startpoint = new Point(-vertexSize / 2, -r.Height / 2 + 10);
            var angle = (Math.PI / 180) * 360F / graph.VertexCount;

            for(int i = 0; i < graph.VertexCount; i += 1)
            {
                var res = new Point();
                var ShiftX = 10;
                var ShiftY = -5;

                res.X = ShiftX + Convert.ToInt32(Math.Cos(angle * i) * startpoint.X - Math.Sin(angle * i) * startpoint.Y);
                res.Y = ShiftY + Convert.ToInt32(Math.Sin(angle * i) * startpoint.X + Math.Cos(angle * i) * startpoint.Y);

                if (res.Y > r.Height / 2 - 20) res.Y -= 20;

                result.Add(res);
            }

            g.ResetTransform();
        }
    }

    public class GraphPainter
    {
        private Graph _graph;
        private int vertexSize = 30;
        private List<Point> _points;
        private DrawDatShit drooow;
        private static Random rnd = new Random();
        public bool isResize = false;
        public Path? path;

        public GraphPainter(Graph graph)
        {
            _graph = graph;
            _points = new List<Point>();
            drooow += PaintEdges;
            drooow += PaintVertices;
        }

        public void PathPaint(Graphics g, Path path)
        {
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            _graph.CreatePoints(g, vertexSize, out _points);


            PaintEdges(g);
            PaintPathEdges(g, path);
            PaintVertices(g);
        }
        public void Paint(Graphics g)
        {
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            _graph.CreatePoints(g, vertexSize, out _points);

            drooow.Invoke(g);
        }


        private void PaintVertices(Graphics g)
        {

            var r = Rectangle.Ceiling(g.VisibleClipBounds);
            g.TranslateTransform(r.Width / 2, r.Height / 2);

            for (var i = 0; i < _graph.VertexCount; i += 1)
            {

                var ellipseRec = new RectangleF(
                    _points[i].X,
                    _points[i].Y,
                    vertexSize,
                    vertexSize);

                g.FillEllipse(
                    new SolidBrush(Color.White), ellipseRec);

                g.DrawEllipse(new Pen(Color.BlueViolet, 2), ellipseRec);

                g.DrawString($"{i + 1}", new Font("Arial", 12F, FontStyle.Bold),
                    new SolidBrush(Color.MediumVioletRed),
                    _points[i].X + 5,
                    _points[i].Y + 3);
            }
        }

        private void PaintPathEdges(Graphics g, Path path)
        {
            var r = Rectangle.Ceiling(g.VisibleClipBounds);
            g.TranslateTransform(r.Width / 2, r.Height / 2);
            var pts = new PointF[_points.Count];

            for (var i = 0; i < _points.Count; i += 1)
            {
                pts[i] = new PointF();
                _points[i].ToCenterVertex(12, out pts[i]);
            }

            var edgestoDraw = path.Edges;

            foreach(var edge in edgestoDraw)
            {
                var color = Color.Black;
                var pen = new Pen(color, 5);

                var v1 = edge.Vertices[0];
                var v2 = edge.Vertices[1];

                var v1point = pts[v1.Number - 1];
                var v2point = pts[v2.Number - 1];
                g.DrawLine(
                    pen,
                    v1point, v2point);

                var wpoint = new PointF((v1point.X + v2point.X + rnd.Next(-50, 50)) / 2, (v1point.Y + v2point.Y) / 2);
                g.DrawString(edge.Weight.ToString(),
                    new Font("Arial", 12F, FontStyle.Bold),
                    new SolidBrush(color),
                    wpoint);
            }
            g.ResetTransform();

            g.DrawString(
                path.ToString(),
                new Font("Arial", 12F, FontStyle.Bold),
                new SolidBrush(Color.Black),
                new Point(20, 20));

        }

        private void PaintEdges(Graphics g)
        {
            var r = Rectangle.Ceiling(g.VisibleClipBounds);
            g.TranslateTransform(r.Width / 2, r.Height / 2);
            var pts = new PointF[_points.Count];
            

            for (var i = 0; i < _points.Count; i += 1)
            {
                pts[i] = new PointF();
                _points[i].ToCenterVertex(12, out pts[i]);
            }

            foreach (var edge in _graph._e)
            {
                var color = Color.Gray;
                
                if(!isResize) color = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                if (path is not null && path.isInPath(edge))
                    continue;
                var v1 = edge.Vertices[0];
                var v2 = edge.Vertices[1];

                var v1point = pts[v1.Number - 1];
                var v2point = pts[v2.Number - 1];

                var wpoint = new PointF((v1point.X + v2point.X + rnd.Next(-50, 50)) / 2 , (v1point.Y + v2point.Y ) / 2);
                g.DrawLine(
                    new Pen(color, 3),
                    v1point, v2point);


                g.DrawString(edge.Weight.ToString(),
                    new Font("Arial", 12F, FontStyle.Bold),
                    new SolidBrush(color),
                    wpoint);

            }

            g.ResetTransform();
        }
    }
}
