using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_2022
{
    public class Path : Graph
    {
        private Stack<Vertex> vertices = new();
        private List<Edge> edges = new();


        public List<Edge> Edges => edges;


        public List<Vertex> Vertices
        {
            get
            {
                var r = new List<Vertex>();
                while (vertices.Count > 0)
                {
                    r.Add(vertices.Pop());
                }

                return r;
            }
        }
        public int Length { get; private set; }

        public Path(Graph g) : base(g)
        {
            
        }

        private void FillEdges()
        {
            Stack<Vertex> tmpvertices = new(vertices);
            var currVertex = tmpvertices.Pop();
            while (tmpvertices.Count > 0)
            {
                var nextVertex = tmpvertices.Pop();
                if (nextVertex is null) break;
                foreach (var edge in GetEdgesFrom(currVertex))
                {
                    if (edge.ConnectedTo(nextVertex))
                    {
                        edge.isPath = true;
                        edges.Add(edge);
                        break;
                    }
                }
                currVertex = nextVertex;
            }
        }

        public bool isInPath(Edge edge)
        {
            bool ans = false;
            edges.ForEach(edges =>
            {
                if (edges == edge) ans = true;
            });
            return ans;
        }
        public Path(Vertex v) : base()
        {
           // base._e = v.Owner._e;

            v.Owner._v.ForEach(v =>
            {
                _v.Add(v.CopyTo(this));
            });

            v.Owner._e.ForEach(e =>
            {
                _e.Add(new Edge(this[e.V1], this[e.V2], e.Weight));
            });

            vertices.Push(v);
            var vertexfrom = v.PathFrom;
            if(vertexfrom is not null)
            vertices.Push(vertexfrom);
            while(vertexfrom is not null)
            {
                vertexfrom = vertexfrom.PathFrom;
                if(vertexfrom is not null)
                    vertices.Push(vertexfrom);
            }

            FillEdges();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Stack<Vertex> tmpvertices = new Stack<Vertex>(vertices);
            var lst = tmpvertices.ToArray();

            for(var i = lst.Length - 1; i >= 0; i--)
            {
                sb.Append(lst[i].ToString() + (i > 0 ? " -> " : ""));
            }

            return sb.ToString();
        }
    }
}
