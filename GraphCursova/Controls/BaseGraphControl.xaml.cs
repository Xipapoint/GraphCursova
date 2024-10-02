using GraphCursova.Controls.utils;
using GraphCursova.Structures;
using GraphCursova.Windows;
using GraphCursova.Windows.Menu;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphCursova.Controls
{
    public partial class BaseGraphControl : UserControl
    {
        public class FindShortestWayEventArgs : EventArgs
        {
            public int FirstIndex { get; }
            public int SecondIndex { get; }
            public List<int> Vertices;
            public Dictionary<int, List<Tuple<int, double>>> Connections;
            public Dictionary<int, Tuple<double, int>> Pair;
            public FindShortestWayEventArgs(int firstIndex, int secondIndex, List<int> vertices, Dictionary<int, List<Tuple<int, double>>> connections, Dictionary<int, Tuple<double, int>> pair)
            {
                FirstIndex = firstIndex;
                SecondIndex = secondIndex;
                Vertices = vertices;
                Connections = connections;
                Pair = pair;
            }
        }
        Graph graph = new Graph();
        private Point startPoint;
        private int vertexId = 0;
        private double Weight;
        private int edgeId = 1;
        List<EdgeTextBlockPair> edgeTextBlockPairs = new List<EdgeTextBlockPair>();
        public event EventHandler<FindShortestWayEventArgs> FindShortestWayClicked;

        private class LineInfo
        {
            public int Index1 { get; set; }
            public int Index2 { get; set; }

            public double Weight { get; set; }

            public LineInfo(int index1, int index2, double weight)
            {
                Weight = weight;
                Index1 = index1;
                Index2 = index2;
            }
        }
        List<List<int>> LocalShortestWay = new List<List<int>>();
        int CurrentIndexWay;
        public BaseGraphControl()
        {
            InitializeComponent();

        }

        private void CreateVertexButton_Click(object sender, RoutedEventArgs e)
        {
            AddVertex();
        }
        void Button_SwitchNextWay(object sender, RoutedEventArgs e)
        {
            if(CurrentIndexWay == LocalShortestWay.Count - 1) 
            {
                MessageBox.Show("Cant be bigger");
                return;
            }
            CurrentIndexWay++;
            UpdateShortestWay(LocalShortestWay, graph.pair, CurrentIndexWay, false);
        }
        void Button_SwitchPreviousWay(object sender, RoutedEventArgs e)
        {
            if (CurrentIndexWay == 0)
            {
                MessageBox.Show("Cant be lower than 0");
                return;
            }
            CurrentIndexWay--;
            UpdateShortestWay(LocalShortestWay, graph.pair, CurrentIndexWay, false);
        }

        private void AddVertex()
        {
            Random random = new Random();
            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;

            double randomX = random.Next(0, (int)canvasWidth - 30);
            double randomY = random.Next(0, (int)canvasHeight - 30); 
            Grid vertexGrid = new Grid(); 
            vertexGrid.Width = 30;
            vertexGrid.Height = 30;

            Ellipse vertex = new Ellipse
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.Blue
            };

            TextBlock vertexText = new TextBlock
            {
                Text = vertexId.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center, 
                VerticalAlignment = VerticalAlignment.Center, 
                Foreground = Brushes.White
            };
            graph.AddPair(vertexId, int.MaxValue, 0);
            
            TextBlock pairText = new TextBlock
            {
                Text = "(INF, 0)",
                FontSize = 18,
                FontWeight = FontWeight.FromOpenTypeWeight(500),
                Foreground = Brushes.Black
            };

            vertexGrid.Name = $"vertex{vertexId}";
            pairText.Name = $"vertex{vertexId}";
            vertexGrid.Children.Add(vertex);
            vertexGrid.Children.Add(vertexText);
            canvas.Children.Add(pairText);

            Canvas.SetLeft(pairText, randomX - 15);
            Canvas.SetTop(pairText, randomY - 35);
            Canvas.SetLeft(vertexGrid, randomX);
            Canvas.SetTop(vertexGrid, randomY);


            vertexGrid.MouseDown += Vertex_MouseDown;
            vertexGrid.MouseMove += Vertex_MouseMove;
            vertexGrid.MouseUp += Vertex_MouseUp;
            graph.AddVertex(vertexId);

            canvas.Children.Add(vertexGrid);
            vertexId++;
        }

        private void AddingButton_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            buttonShadow.ShadowDepth = 0;
        }

        private void AddingButton_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            buttonShadow.ShadowDepth = 5;
        }

        public Point GetVertexCenterCoordinates(int vertexIndex)
        {
            foreach (UIElement child in canvas.Children)
            {
                if (child is Grid grid && grid.Name == $"vertex{vertexIndex}")
                {
                    double centerX = Canvas.GetLeft(grid) + grid.Width / 2;
                    double centerY = Canvas.GetTop(grid) + grid.Height / 2;
                    return new Point(centerX, centerY);
                }
            }
            return new Point(double.NaN, double.NaN); 
        }

        private void CreateEdge_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            int firstIndex;
            int secondIndex;
            if (!int.TryParse(Start_vertex.Text, out firstIndex) || !int.TryParse(End_vertex.Text, out secondIndex))
            {
                MessageBox.Show("Please enter valid unsigned integer values for vertices!");
                return;
            }
            else if (firstIndex == secondIndex)
            {
                MessageBox.Show("You can't create an edge with the same vertex");
                return;
            }
            else if (firstIndex < 0 || secondIndex < 0)
            {
                MessageBox.Show("Enter valid values");
                return;
            }
            bool firstExists = ContainsVertex(graph.vertices, firstIndex);
            bool secondExists = ContainsVertex(graph.vertices, secondIndex);
            if(!firstExists || !secondExists)
            {
                MessageBox.Show("Please enter valid integers");
                return;
            }

            if (graph.connections.ContainsKey(firstIndex))
            {
                foreach (var connection in graph.connections[firstIndex])
                {
                    if (connection.Item1 == secondIndex)
                    {
                        MessageBox.Show("Such edge already exists!");
                        return;
                    }
                }
            }
            var centerVertex1 = GetVertexCenterCoordinates(firstIndex);
            var centerVertex2 = GetVertexCenterCoordinates(secondIndex);
            Weight = Math.Abs(Math.Floor(Math.Sqrt(Math.Pow(centerVertex2.X - centerVertex1.X, 2) + Math.Pow(centerVertex2.Y - centerVertex1.Y, 2))));
            if(parentWindow is IUndirected)
            {
                graph.AddUndirectedEdge(firstIndex, secondIndex, Weight);
            }
            else
            {
                graph.AddDirectedEdge(firstIndex, secondIndex, Weight);
            }

            AddEdge(firstIndex, secondIndex, centerVertex1, centerVertex2, Weight);

        }

        private bool ContainsVertex(List<int> vertices, int vertexIndex)
        {
            foreach(int vertex in vertices)
            {
                if(vertex == vertexIndex)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddEdge(int firstIndex, int secondIndex, Point centerVertex1, Point centerVertex2, double weight)
        {
           double MathAngle = Math.Atan2(centerVertex2.Y - centerVertex1.Y, centerVertex2.X - centerVertex1.X) * 180 / Math.PI;
            Line edge = new Line
            {
                X1 = centerVertex1.X,
                Y1 = centerVertex1.Y,
                X2 = centerVertex2.X,
                Y2 = centerVertex2.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 4,
            };

            double textX = (centerVertex1.X + centerVertex2.X) / 2;
            double textY = (centerVertex1.Y + centerVertex2.Y) / 2;

            TextBlock weightBlock = new TextBlock
            {
                Text = $"{weight}",
                FontSize = 20,
                FontWeight = FontWeight.FromOpenTypeWeight(500),
                Margin = new Thickness(textX, textY, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            LineInfo lineInfo = new LineInfo(firstIndex, secondIndex, weight);
            edge.SetValue(FrameworkElement.TagProperty, lineInfo);
            edge.SetValue(FrameworkElement.NameProperty, $"Edge{edgeId}");
            weightBlock.SetValue(FrameworkElement.TagProperty, lineInfo);

            canvas.Children.Add(edge);
            canvas.Children.Add(weightBlock);
            edgeId++;
            foreach (EdgeTextBlockPair pair in edgeTextBlockPairs)
            {
                if (pair.Edge == null)
                {
                    edgeTextBlockPairs.Add(new EdgeTextBlockPair { Edge = edge, WeightBlock = weightBlock });
                    return;
                }
            }
            edgeTextBlockPairs.Add(new EdgeTextBlockPair { Edge = edge, WeightBlock = weightBlock });
        }

        private void FindShortestWay_Click(object sender, RoutedEventArgs e)
        {
            int firstIndex;
            int secondIndex;
            if (!int.TryParse(Start_vertexWAY.Text, out firstIndex) || !int.TryParse(End_vertexWAY.Text, out secondIndex))
            {
                MessageBox.Show("Please enter valid positive integer values for vertices!");
                return;
            }
            else if (firstIndex == secondIndex)
            {
                MessageBox.Show("You can't create an edge with the same vertex");
                return;
            }
            else if (firstIndex < 0 || secondIndex < 0)
            {
                MessageBox.Show("Enter valid values");
                return;
            }
            bool firstExists = ContainsVertex(graph.vertices, firstIndex);
            bool secondExists = ContainsVertex(graph.vertices, secondIndex);
            if (!firstExists || !secondExists)
            {
                MessageBox.Show("Please enter valid integers");
                return;
            }
            FindShortestWayClicked?.Invoke(this, new FindShortestWayEventArgs(firstIndex, secondIndex, graph.vertices, graph.connections, graph.pair));
        }
        private List<Line> GetEdgesByVertex(Grid vertex)
        {
            List<Line> edges = new List<Line>();

            foreach (Line edge in canvas.Children.OfType<Line>())
            {
                var tagValue = edge.GetValue(FrameworkElement.TagProperty);

                if (tagValue is LineInfo lineInfo)
                {
                    if ($"vertex{lineInfo.Index1}" == vertex.Name || $"vertex{lineInfo.Index2}" == vertex.Name)
                    {
                        edges.Add(edge);
                    }
                }
            }
            return edges;
        }
        private void UpdateWeightBlocksPositions(Line edge, double distance)
        {
            foreach (EdgeTextBlockPair pair in edgeTextBlockPairs)
            {
                if (pair.Edge == edge)
                {
                    double textX = (edge.X1 + edge.X2) / 2;
                    double textY = (edge.Y1 + edge.Y2) / 2;
                    pair.WeightBlock.Margin = new Thickness(textX, textY, 0, 0);
                    pair.WeightBlock.Text = distance.ToString();
                    break;
                }
            }
        }
        private void Vertex_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid gridEllipse = sender as Grid;
            startPoint = e.GetPosition(null);
            gridEllipse.CaptureMouse();
        }
        private void Vertex_MouseMove(object sender, MouseEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Grid ellipse = sender as Grid;
                TextBlock PairText = new TextBlock();
                foreach(TextBlock pair in canvas.Children.OfType<TextBlock>())
                {
                    if(pair.Name == ellipse.Name)
                    {
                        PairText = pair;
                        break;
                    }
                }
                Point currentPoint = e.GetPosition(null);
                List<Line> edges = GetEdgesByVertex(ellipse);
                Vector moveVector = startPoint - currentPoint;
                double left = Canvas.GetLeft(ellipse) - moveVector.X;
                double top = Canvas.GetTop(ellipse) - moveVector.Y;
                if (left < 0) left = 0;
                if (top < 0) top = 0;
                if (left + ellipse.ActualWidth > canvas.ActualWidth) left = canvas.ActualWidth - ellipse.ActualWidth;
                if (top + ellipse.ActualHeight > canvas.ActualHeight) top = canvas.ActualHeight - ellipse.ActualHeight;

                Canvas.SetLeft(PairText, left - 15);
                Canvas.SetTop(PairText, top - 35);
                Canvas.SetLeft(ellipse, left);
                Canvas.SetTop(ellipse, top);


                foreach (Line edge in edges)
                {
                    var tagValue = edge.GetValue(FrameworkElement.TagProperty);

                    if (tagValue is LineInfo lineInfo)
                    {
                        if ($"vertex{lineInfo.Index1}" == ellipse.Name || $"vertex{lineInfo.Index2}" == ellipse.Name)
                        {
                            int index1 = lineInfo.Index1;
                            int index2 = lineInfo.Index2;
                            double distance = 0.0;
                            Point centerVertex1 = GetVertexCenterCoordinates(index1);
                            Point centerVertex2 = GetVertexCenterCoordinates(index2);
                            if (ellipse.Name == $"vertex{index1}")
                            {
                                edge.X1 = left + ellipse.ActualWidth / 2;
                                edge.Y1 = top + ellipse.ActualHeight / 2;
                                distance = Math.Floor(Math.Sqrt(Math.Pow(centerVertex2.X - left, 2) + Math.Pow(centerVertex2.Y - top, 2)));
                                UpdateWeightBlocksPositions(edge, distance);
                                graph.UpdateUndirectedConnectionWeight(index1, index2, distance);
                                if (parentWindow is IUndirected)
                                {
                                    graph.AddUndirectedEdge(index1, index2, Weight);
                                }
                                else
                                {
                                    graph.AddDirectedEdge(index1, index2, Weight);
                                }

                            }
                            else if (ellipse.Name == $"vertex{index2}")
                            {
                                edge.X2 = left + ellipse.ActualWidth / 2;
                                edge.Y2 = top + ellipse.ActualHeight / 2;
                                distance = Math.Floor(Math.Sqrt(Math.Pow(centerVertex1.X - left, 2) + Math.Pow(centerVertex1.Y - top, 2)));
                                UpdateWeightBlocksPositions(edge, distance);
                                if (parentWindow is IUndirected)
                                {
                                    graph.AddUndirectedEdge(index1, index2, Weight);
                                }
                                else
                                {
                                    graph.AddDirectedEdge(index1, index2, Weight);
                                }
                            }
                        }
                    }
                }
                startPoint = currentPoint;
            }
        }

        private void Vertex_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid gridEllipse = sender as Grid;
            gridEllipse.ReleaseMouseCapture();
        }

        private void FillShortestVertices(List<List<int>> ShortestWays)
        {
            for(int i = 0; i < ShortestWays.Count; i++)
            {
                LocalShortestWay.Add(new List<int>());
                for(int j = 0; j < ShortestWays[i].Count; j++)
                {
                    LocalShortestWay[i].Add(ShortestWays[i][j]);
                }
            }
        }


        public void UpdateShortestWay(List<List<int>> ShortestWay, Dictionary<int, Tuple<double, int>> Pair, int IndexOfWay, bool IsFirstTry)
        {
            if (!IsFirstTry)
            {
                ClearShortestWay();
            }
            foreach (Grid child in canvas.Children.OfType<Grid>())
            {
                for (int i = 0; i < ShortestWay[IndexOfWay].Count; i++)
                {
                    if (child.Name == $"vertex{ShortestWay[IndexOfWay][i]}")
                    {
                        Ellipse vertex = child.Children.OfType<Ellipse>().FirstOrDefault();
                        vertex.Fill = Brushes.Red;
                    }
                }
            }
            

            if(IsFirstTry)
            {
                foreach (TextBlock textPair in canvas.Children.OfType<TextBlock>())
                {
                    if (textPair.Text == "(INF, 0)")
                    {
                        for (int i = 0; i < Pair.Count; i++)
                        {
                            if (textPair.Name == $"vertex{i}")
                            {
                                if (Pair[i].Item1 == double.MaxValue)
                                {
                                    continue;
                                }
                                textPair.Text = Pair[i].ToString();
                                break;
                            }

                        }
                    }

                }

                FillShortestVertices(ShortestWay);
                CreateTextFile(ShortestWay);
                ShortestWayCountText.Text = (ShortestWay.Count() - 1).ToString();
            }
            CurrentIndexWay = IndexOfWay;
            CurrentIndexWayText.Text = CurrentIndexWay.ToString();
        }





        public void ClearShortestWay()
        {
            foreach (Grid child in canvas.Children.OfType<Grid>())
            {
                for (int i = 0; i < graph.vertices.Count; i++)
                {
                    if (child.Name == $"vertex{i}")
                    {
                        Ellipse vertex = child.Children.OfType<Ellipse>().FirstOrDefault();
                        vertex.Fill = Brushes.Blue;
                    }
                }
            }

            foreach (TextBlock textPair in canvas.Children.OfType<TextBlock>())
            {
                if (textPair.Text != "(INF, 0)")
                {
                    for (int i = 0; i < graph.vertices.Count; i++)
                    {
                        if (textPair.Name == $"vertex{i}")
                        {
                            textPair.Text = "(INF, 0)";
                            break;
                        }

                    }
                }

            }
        }

        private void CreateTextFile(List<List<int>> ShortestWay)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var route in ShortestWay)
                        {
                            string line = string.Join(", ", route);
                            writer.WriteLine(line);
                        }
                    }
                    MessageBox.Show("File successfuly created!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during creating file: " + ex.Message);
                }
            }
        }

        private void GoToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseGraphWindow menuWindow = new ChooseGraphWindow();
            menuWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void ResetGraphButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < graph.vertices.Count; i++)
            {
                List<UIElement> elementsToRemove = new List<UIElement>();

                foreach (UIElement child in canvas.Children)
                {
                    elementsToRemove.Add(child);
                }

                foreach (UIElement element in elementsToRemove)
                {
                    canvas.Children.Remove(element);
                }

            }
            graph.Clear();
            vertexId = 0;
            edgeId = 0;
        }
        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
