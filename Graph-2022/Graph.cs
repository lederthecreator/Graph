using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_2022
{
    public class Graph
    {
        public List<Vertex> _v = new();
        public List<Edge> _e = new();
        public int VertexCount => _v.Count;
        public Graph(double[,]matrix)
        {
            createGraph(matrix);
        }

        public Vertex? this[Vertex vertex] =>_v.Find(v => v.Number == vertex.Number);
        protected Graph(Graph g)
        {
            g._v.ForEach(v =>
            {
                _v.Add(v.CopyTo(this));
            });
            g._e.ForEach(e =>
            {
                _e.Add(new Edge(this[e.V1], this[e.V2], e.Weight));
            });
        }

        

        public Graph()
        {
        }

        private void createGraph(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                _v.Add(new Vertex(this));
            }

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                for (int j = i + 1; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] > 0) _e.Add(new Edge(_v[i], _v[j], matrix[i,j]));
                }
            }
        }

        public List<Edge> GetEdgesFrom(Vertex v) => _e.FindAll(e => e.ConnectedTo(v));
        
    }
}
