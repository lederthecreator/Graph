using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_2022
{
    public class Vertex
    {
        public int Number { get; }

        public Graph Owner { get; init; }
        public double PathLength { get; set; }
        public Vertex? PathFrom { get; set; }
        public bool Visited { get; set; }

        public Vertex(Graph owner)
        {
            Owner = owner;
            Number = owner.VertexCount + 1;
            PathLength = double.PositiveInfinity;
            PathFrom = null;
            Visited = false;
        }

        private Vertex(Graph owner, int Number)
        {
            Owner=owner;
            this.Number = Number;
        }

        public static bool operator ==(Vertex v1, Vertex v2)
        {
            return v1.Number == v2.Number;
        }

        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }

        public Vertex CopyTo(Graph g) => new(g, Number);

        public override string ToString()
        {
            return $"{Number}";
        }
    }
}
