using GraphCursova.Structures;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphCursova.Windows.alghorithms
{
    public class FloydWarshallAlghorithm
    {
        public class FWResult
        {
            public Dictionary<int, Tuple<double, int>> Pair = new Dictionary<int, Tuple<double, int>>();
            public List<List<int>> ShortestWays = new List<List<int>>();
            public int TargetIndex;
        }

        public FWResult FloydWarshallAlgo(List<int> vertices, Dictionary<int, List<Tuple<int, double>>> connections, Dictionary<int, Tuple<double, int>> pair, int start, int end)
        {
            int n = vertices.Count;
            var shortestWays = new List<List<int>>();
            var distances = new double[n, n];
            var next = new int?[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        distances[i, j] = 0;
                    }
                    else
                    {
                        distances[i, j] = double.MaxValue;
                    }
                    next[i, j] = null;
                }
            }
            foreach (var vertex in vertices)
            {
                var vertexConnections = connections[vertex];
                foreach (var connection in vertexConnections)
                {
                    int u = vertices.IndexOf(vertex);
                    int v = vertices.IndexOf(connection.Item1);
                    distances[u, v] = connection.Item2;
                    next[u, v] = v;
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] != double.MaxValue && distances[k, j] != double.MaxValue &&
                            distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j && next[i, j].HasValue)
                    {
                        var path = new List<int> { vertices[i] };
                        int? current = i;
                        while (current != j)
                        {
                            current = next[current.Value, j];
                            if (current.HasValue)
                                path.Add(vertices[current.Value]);
                        }
                        shortestWays.Add(path);
                    }
                }
            }

            int targetIndex = -1;
            for (int i = 0; i < shortestWays.Count; i++)
            {
                if (shortestWays[i].First() == start && shortestWays[i].Last() == end)
                {
                    targetIndex = i;
                    break;
                }
            }
            foreach (List<int>? path in shortestWays)
            {
                int u = vertices.IndexOf(path.First());
                int v = vertices.IndexOf(path.Last());
                pair[v] = new Tuple<double, int>(0, 0);
            }

            return new FWResult
            {
                Pair = pair,
                ShortestWays = shortestWays,
                TargetIndex = targetIndex
            };
        }
    }
}
