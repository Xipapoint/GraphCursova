using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GraphCursova.Structures
{
    public class Graph
    {
        public List<int> vertices;
        public Dictionary<int, List<Tuple<int, double>>> connections;
        public Dictionary<int, Tuple<double, int>> pair;


        public Graph()
        {
            vertices = new List<int>();
            connections = new Dictionary<int, List<Tuple<int, double>>>();
            pair = new Dictionary<int, Tuple<double, int>>();
        }

        public void AddVertex(int vertex)
        {
            if (!vertices.Contains(vertex))
            {
                vertices.Add(vertex);
            }
        }

        public void AddUndirectedEdge(int vertex1, int vertex2, double weight)
        {
            if (!connections.ContainsKey(vertex1))
            {
                connections[vertex1] = new List<Tuple<int, double>>();
            }
            if (!connections.ContainsKey(vertex2))
            {
                connections[vertex2] = new List<Tuple<int, double>>();
            }
            connections[vertex1].Add(new Tuple<int, double>(vertex2, weight));
            connections[vertex2].Add(new Tuple<int, double>(vertex1, weight));
        }

        public void AddDirectedEdge(int startVertex, int endVertex, double weight)
        {
            if (!connections.ContainsKey(startVertex))
            {
                connections[startVertex] = new List<Tuple<int, double>>();
            }
            connections[startVertex].Add(new Tuple<int, double>(endVertex, weight));
        }

        public void AddPair(int parentVertex, double weight, int startVertex)
        {
            pair[parentVertex] = new Tuple<double, int>(weight, startVertex);
        }

        public List<Tuple<int, double>> GetConnectionsByKey(int key)
        {
            if (connections.ContainsKey(key))
            {
                return connections[key];
            }
            return null;
        }

        public void UpdateUndirectedConnectionWeight(int vertex1, int vertex2, double weight)
        {
            for(int i = 0; i < connections[vertex1].Count(); i++)
            {
                if (connections[vertex1][i].Item1 == vertex2)
                {
                    connections[vertex1][i] = new Tuple<int, double>(vertex2, weight);
                    break;
                }
            }

            for (int i = 0; i < connections[vertex2].Count(); i++)
            {

                if (connections[vertex2][i] != null)
                {
                    if (connections[vertex2][i].Item1 == vertex1)
                    {
                        connections[vertex2][i] = new Tuple<int, double>(vertex1, weight);
                        break;
                    }
                }
            }
        }
        public void UpdateDirectedConnectionWeight(int startVeretx, int endVertex, double weight)
        {
            for(int i = 0; i < connections[startVeretx].Count(); i++)
            {
                if (connections[startVeretx][i].Item1 == endVertex)
                {
                    connections[startVeretx][i] = new Tuple<int, double>(endVertex, weight);
                    break;
                }
            }   
        }
        public void Clear()
        {
            vertices.Clear();
            connections.Clear();
            pair.Clear();
        }
    }
}
