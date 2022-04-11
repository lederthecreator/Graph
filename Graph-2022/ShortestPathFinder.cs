using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Graph_2022
{
    public class ShortestPathFinder
    {
        private Graph _graph;

        public ShortestPathFinder(Graph g)
        {
            _graph = g;
        }

        public void ResetPaths()
        {
            _graph._v.ForEach(v =>
            {
                v.Visited = false;
                v.PathLength = double.PositiveInfinity;
                v.PathFrom = null;
            });
        }


        public Path GetShortestPath(Vertex from, Vertex to)
        {
            _graph[from].PathLength = 0; 
            var currVertex = from;
            while (currVertex is not null && !currVertex.Visited)
            {
                var nextVertex = GetMinVertex(currVertex);
                if (nextVertex is not null)
                {
                    var edges = _graph.GetEdgesFrom(currVertex);
                    foreach (var edge in edges)
                    {
                        update(edge.GetPairFor(currVertex), edge);
                    }
                }
                currVertex.Visited = true;
                currVertex = nextVertex;
            }
            return new Path(to);
        }

        private void update(Vertex v, Edge e)
        {
            var ln = e.GetPairFor(v).PathLength + e.Weight;
            if (!v.Visited && v.PathLength > ln)
            {
                v.PathLength = ln;
                v.PathFrom = e.GetPairFor(v);
            }
        }

        private Vertex? GetMinVertex(Vertex from)
        {
            var edges = _graph.GetEdgesFrom(from);
            double min_weight = 1000000;
            Vertex? min_vertex = null;
            foreach(var edge in edges)
            {
                var next = edge.V1 == from ? edge.V2 : edge.V1;
                if(edge.Weight < min_weight && !next.Visited)
                {
                    min_weight = edge.Weight;
                    min_vertex = next;
                }
            }
            return min_vertex;

            //var vlist = new List<Vertex>();
            //foreach(var edge in _graph._e)
            //{
            //    var tmp = edge.GetPairFor(from);
            //    if (tmp is null) continue;
            //    vlist.Add(tmp);
            //}

            //Vertex? min = vlist.FirstOrDefault();
            //if(min is null)
            //{
            //    throw new ArgumentNullException(nameof(min));
            //}

            //double min_weight = -1;
            //foreach(var vert in vlist)
            //{
            //    var w = GetWeight(from, vert);
            //    if(w < min_weight)
            //    {
            //        min_weight = w;
            //        min = vert;
            //    }
            //}

            //return min;
        }

        private double GetWeight(Vertex from, Vertex where)
        {
            foreach(var edge in _graph._e)
            {
                var tmp = edge.GetPairFor(from);
                if(tmp is not null && where == tmp)
                {
                    return edge.Weight;
                }
            }
            return 0;
        }
    }
}
