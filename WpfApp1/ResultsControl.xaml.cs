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
using LogLibrary;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ResultsControl.xaml
    /// </summary>
    public partial class ResultsControl : UserControl
    {
        //Global Variable
        MainWindow mainwindow = (MainWindow)Application.Current.MainWindow;
        public ResultsControl()
        {
            InitializeComponent();

            TopMenu.Content = new TopMenuControl();

            DateLabel.Content = DateTime.Now.ToString();

            //Getting Name from Application Name
            var window = (MainWindow)Application.Current.MainWindow;
            var name = window.Name;

            Logger log = new Logger();
            var lastSession = log.LastSession();

            CorrectsLabel.Content = log.CorrectsQuery(lastSession);
            SkipsLabel.Content = log.SkipsQuery(lastSession);
            PercentageLabel.Content = ($"{(log.CorrectsQuery(lastSession) / log.NumberOfCardsQuery(lastSession)) * 100}%");

        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //back return to choosing player
            var animation = new WelcomeAnimation();

            mainwindow.Content = animation;

            //Wait for Animation to Complete
            await Task.Delay(5000);

            animation.PlayerBoxControl.Content = new SelectPlayerControl();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            //Redo returns to main menu
            mainwindow.Content = new MainControl();
        }
    }
}
