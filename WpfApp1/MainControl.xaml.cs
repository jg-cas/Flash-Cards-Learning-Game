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
using System.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl ()
        {
            InitializeComponent();

            TopMenu.Content = new TopMenuControl();

            var application = (MainWindow)Application.Current.MainWindow;
            Welcome.Content = $"Welcome {application.Name}!";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //click play button
            var window = (MainWindow)Application.Current.MainWindow;
            window.Content = new MainMenu();

            //ToDo add other functionality to this window
        }
    }
}
