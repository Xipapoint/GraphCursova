using GraphCursova.Windows.Menu;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphCursova.Controls
{
    /// <summary>
    /// Interaction logic for NavbarControl.xaml
    /// </summary>
    public partial class NavbarControl : UserControl
    {
        public NavbarControl()
        {
            InitializeComponent();
        }


        private void Graphs_page(object sender, RoutedEventArgs e)
        {
            ChooseGraphWindow chooseGraphWindow = new ChooseGraphWindow();
            chooseGraphWindow.Show();
        }

        private void Info_page(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.Show();

        }
        private void Close_pages(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }



        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                textBlock.TextDecorations = TextDecorations.Underline;
                textBlock.Cursor = Cursors.Hand;
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                textBlock.TextDecorations = null;
                textBlock.Cursor = Cursors.Arrow;
            }
        }
    }
}
