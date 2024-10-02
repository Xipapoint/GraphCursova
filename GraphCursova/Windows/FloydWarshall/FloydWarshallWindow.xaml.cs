using GraphCursova.Controls;
using GraphCursova.Windows.alghorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static GraphCursova.Controls.BaseGraphControl;
using static GraphCursova.Windows.alghorithms.FloydWarshallAlghorithm;

namespace GraphCursova.Windows.FloydWarshall
{
    /// <summary>
    /// Interaction logic for FloydWarshallWindow.xaml
    /// </summary>
    public partial class FloydWarshallWindow : Window, IUndirected
    {
        private BaseGraphControl baseGraphControl;
        private FWResult result;
        public FloydWarshallWindow()
        {
            InitializeComponent();

            baseGraphControl = baseGraphControlXaml;
            baseGraphControl.FindShortestWayClicked += BaseGraphControl_FindShortestWayClicked;
        }

        private void BaseGraphControl_FindShortestWayClicked(object sender, FindShortestWayEventArgs e)
        {
            int firstIndex = e.FirstIndex;
            int secondIndex = e.SecondIndex;
            Dictionary<int, List<Tuple<int, double>>> connections = e.Connections;
            Dictionary<int, Tuple<double, int>> pair = e.Pair;
            List<int> vertices = e.Vertices;
            

            
            FloydWarshallAlghorithm floydWarshallAlghorithm = new FloydWarshallAlghorithm();
            baseGraphControl.ClearShortestWay();
            result = floydWarshallAlghorithm.FloydWarshallAlgo(vertices, connections, pair, firstIndex, secondIndex);

            if (result.Pair.Count > 0 & result.ShortestWays.Count > 0)
            {
               baseGraphControl.UpdateShortestWay(result.ShortestWays, result.Pair, result.TargetIndex, true);
            }

        }
    }
}
