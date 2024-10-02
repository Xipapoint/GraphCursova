using GraphCursova.Structures;
using System.Windows;

namespace GraphCursova.Windows.alghorithms
{
    // Результат алгоритму
    public class DejkstraResult
    {
        public List<List<int>> Vertices = new List<List<int>>();
        public Dictionary<int, Tuple<double, int>> Pair = new Dictionary<int, Tuple<double, int>>();
    }
    public class DejkstraAlghorithm
    {

        public DejkstraResult dejkstraAlgorithm(List<int> vertices, Dictionary<int, List<Tuple<int, double>>> connections, Dictionary<int, Tuple<double, int>> pair, int start, int end)
        {

            if (connections.Count == 0)
            {
                Console.WriteLine("No existing edges");
                return new DejkstraResult
                {
                    Vertices = [],
                    Pair = []
                };
            }
            PriorityQueueHeap pq = new PriorityQueueHeap();
            List<List<int>> shortestWay = [new List<int>()];
            for (int i = 0; i < vertices.Count; i++) pair[vertices[i]] = Tuple.Create(double.MaxValue, -1);
            pair[start] = new Tuple<double, int>(0.0, 0);
            pq.Enqueue(start, 0);

            while (!pq.IsEmpty())
            {
                Tuple<int, double> currentTuple = pq.Dequeue();
                int currentVertex = currentTuple.Item1;
                double currentWeight = currentTuple.Item2;
                if (currentVertex == end)
                {
                    while (true)
                    {
                        shortestWay[0].Add(currentVertex);
                        currentVertex = pair[currentVertex].Item2;
                        if (currentVertex == start) break;
                    }
                    shortestWay[0].Add(currentVertex);
                    shortestWay[0].Reverse();
                    return new DejkstraResult
                    {
                        Vertices = shortestWay,
                        Pair = pair
                    };
                }
                if (currentWeight > pair[currentVertex].Item1) continue;
                if (connections[currentVertex] == null) continue;
                foreach (Tuple<int, double> neighbor in connections[currentVertex])
                {
                    int neighborVertex = neighbor.Item1;
                    double neighborWeight = neighbor.Item2;

                    double newWeight = currentWeight + neighborWeight;

                    if (newWeight < pair[neighborVertex].Item1)
                    {
                        pair[neighborVertex] = Tuple.Create(newWeight, currentVertex);
                        pq.Enqueue(neighborVertex, newWeight);  
                    }
                }
            }

            MessageBox.Show("Couldnt find shortest way");
            return new DejkstraResult
            {
                Vertices = [],
                Pair = []
            };
        }
    }
}
