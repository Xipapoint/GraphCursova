using GraphCursova.Windows.FloydWarshall;
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

namespace GraphCursova.Windows.Menu
{
    /// <summary>
    /// Interaction logic for ChooseGraphWindow.xaml
    /// </summary>
    public partial class ChooseGraphWindow : Window
    {
        public ChooseGraphWindow()
        {
            InitializeComponent();
        }
        private void Dejkstra_Page(object sender, RoutedEventArgs e)
        {
            DejkstraWindow dejkstraWindow = new DejkstraWindow();
            dejkstraWindow.Show();
            Close();
        }
        private void DirectedDejkstra_Page(object sender, RoutedEventArgs e)
        {
            DirectedDejkstraWindow directedDejkstraWindow = new DirectedDejkstraWindow();
            directedDejkstraWindow.Show();
            Close();
        }
        private void FloydWarshall_Page(object sender, RoutedEventArgs e)
        {
            FloydWarshallWindow FWWindow = new FloydWarshallWindow();
            FWWindow.Show();
            Close();
        }

    }
}
