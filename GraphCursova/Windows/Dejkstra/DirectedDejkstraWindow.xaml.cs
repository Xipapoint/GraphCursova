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

namespace GraphCursova.Windows
{
    /// <summary>
    /// Interaction logic for DirectedDejkstraWindow.xaml
    /// </summary>
    public partial class DirectedDejkstraWindow : Window, IDirected
    {
        private BaseGraphControl baseGraphControl;
        private DejkstraAlghorithm dejkstraAlghorithm;
        private DejkstraResult result;
        public DirectedDejkstraWindow()
        {
            InitializeComponent();
            baseGraphControl = baseGraphControlXaml;
            baseGraphControl.FindShortestWayClicked += BaseGraphControl_FindShortestWayClicked;
        }
        private void BaseGraphControl_FindShortestWayClicked(object sender, FindShortestWayEventArgs e)
        {
            // Параметри передачі до алгоритму
            int firstIndex = e.FirstIndex;
            int secondIndex = e.SecondIndex;
            Dictionary<int, List<Tuple<int, double>>> connections = e.Connections;
            Dictionary<int, Tuple<double, int>> pair = e.Pair;
            List<int> vertices = e.Vertices;

            dejkstraAlghorithm = new DejkstraAlghorithm();
            baseGraphControl.ClearShortestWay();
            result = dejkstraAlghorithm.dejkstraAlgorithm(vertices, connections, pair, firstIndex, secondIndex);
            if (result.Pair.Count > 0 & result.Vertices.Count > 0)
            {
                baseGraphControl.UpdateShortestWay(result.Vertices, result.Pair, 0, true);
            }

        }
    }
}
