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
using CardsClassifications;
using System.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SelectPlayerControl.xaml
    /// </summary>
    public partial class SelectPlayerControl : UserControl
    {
        //Global Variable
        Logger players;
        public SelectPlayerControl()
        {
            InitializeComponent();

            //Initializing new Logger Instance
            players = new Logger();

            //Loading ComboBox with Player Names
            foreach (var p in players.ReadDemographics())
            {
                PlayerBox.Items.Add(p.Name);
            }
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            MicrophoneButton1.Visibility = Visibility.Collapsed;
            MicrophoneButton2.Visibility = Visibility.Visible;

            var window = (MainWindow)Application.Current.MainWindow;

            if (!string.IsNullOrWhiteSpace(PlayerBox.Text))
            {

                var application = (MainWindow)Application.Current.MainWindow;


                //This makes the selected name available for data base logging through the different control since mainwindow is inmutable
                application.Name = PlayerBox.Text;

                application.Content = new MainControl();
            }
            else
            {
                LogInButton.Visibility = Visibility.Collapsed;
                NewPlayerButton.Visibility = Visibility.Visible;
                AgeBox.Visibility = Visibility.Visible;
                NameBox.Visibility = Visibility.Visible;
                MicrophoneButton1.Visibility = Visibility.Collapsed;
                MicrophoneButton2.Visibility = Visibility.Visible;
                WarningLabel.Content = "Create a New Player";
            }
        }

        private void NewPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            //Data Validation
            if (!string.IsNullOrWhiteSpace(AgeBox.Text) && !string.IsNullOrWhiteSpace(NameBox.Text) && AgeBox.Text != "Enter Age" && NameBox.Text != "Enter Name")
            {
                //New Player to SQL Server
                var player = new Demographic();
                player.Name = NameBox.Text;

                //Data Validation for Age
                if (decimal.TryParse(AgeBox.Text, out decimal age))
                {
                    player.Age = age;
                    var id = players.MaxID();
                    player.ID = id + 1;

                    //Create Player
                    players.AddPlayer(player);

                    PlayerBox.ItemsSource = null;

                    //Repopulate Box
                    foreach (var p in players.ReadDemographics())
                    {
                        PlayerBox.Items.Add(p.Name);
                    }

                    LogInButton.Visibility = Visibility.Visible;
                    NewPlayerButton.Visibility = Visibility.Collapsed;
                    AgeBox.Visibility = Visibility.Collapsed;
                    NameBox.Visibility = Visibility.Collapsed;
                    MicrophoneButton1.Visibility = Visibility.Visible;
                    MicrophoneButton2.Visibility = Visibility.Collapsed;
                    WarningLabel.Content = "Player Added!";
                }
                else
                {
                    //If no number for age
                    WarningLabel.Content = "Enter a Number for Age";
                }
            }
            else
            {
                //If not all boxes have data
                WarningLabel.Content = "Make Sure all Boxes are Filled";
            }
        }
    


        private void MicrophoneButton1_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();

            var window = (MainWindow)Application.Current.MainWindow;

            if (login.MainLogin() == true && !string.IsNullOrWhiteSpace(PlayerBox.Text)) //Go to Main Application Window
            {
                var application = (MainWindow)Application.Current.MainWindow;

                //This makes the selected name available for data base logging through the different control since mainwindow is inmutable
                application.Name = PlayerBox.Text;

                application.Content = new MainControl();
            }
            else
            {
                LogInButton.Visibility = Visibility.Collapsed;
                NewPlayerButton.Visibility = Visibility.Visible;
                AgeBox.Visibility = Visibility.Visible;
                NameBox.Visibility = Visibility.Visible;
                MicrophoneButton1.Visibility = Visibility.Collapsed;
                MicrophoneButton2.Visibility = Visibility.Visible;
                WarningLabel.Content = "Create a New Player";
            }
        }

        private void MicrophoneButton2_Click_1(object sender, RoutedEventArgs e)
        {
            CreatePlayer create = new CreatePlayer();

            var window = (MainWindow)Application.Current.MainWindow;

            if (create.MainCreate() == true)
            {
                //Data Validation
                if (!string.IsNullOrWhiteSpace(AgeBox.Text) && !string.IsNullOrWhiteSpace(NameBox.Text) && AgeBox.Text != "Enter Age" && NameBox.Text != "Enter Name")
                {
                    //New Player to SQL Server
                    var player = new Demographic();
                    player.Name = NameBox.Text;

                    //Data Validation for Age
                    if (decimal.TryParse(AgeBox.Text, out decimal age))
                    {
                        player.Age = age;
                        var id = players.MaxID();
                        player.ID = id + 1;

                        //Create Player
                        players.AddPlayer(player);

                        PlayerBox.ItemsSource = null;

                        //Repopulate Box
                        foreach (var p in players.ReadDemographics())
                        {
                            PlayerBox.Items.Add(p.Name);
                        }

                        LogInButton.Visibility = Visibility.Visible;
                        NewPlayerButton.Visibility = Visibility.Collapsed;
                        AgeBox.Visibility = Visibility.Collapsed;
                        NameBox.Visibility = Visibility.Collapsed;
                        MicrophoneButton1.Visibility = Visibility.Visible;
                        MicrophoneButton2.Visibility = Visibility.Collapsed;
                        WarningLabel.Content = "Player Added!";
                    }
                    else
                    {
                        //If no number for age
                        WarningLabel.Content = "Enter a Number for Age";
                    }
                }
                else
                {
                    //If not all boxes have data
                    WarningLabel.Content = "Make Sure all Boxes are Filled";
                }
            }
            else
            {
                //If you choose to exit
                window.Close();
            }
        }

        private void PlayerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogInButton.Visibility = Visibility.Visible;
            NewPlayerButton.Visibility = Visibility.Collapsed;
            AgeBox.Visibility = Visibility.Collapsed;
            NameBox.Visibility = Visibility.Collapsed;
            MicrophoneButton1.Visibility = Visibility.Visible;
            MicrophoneButton2.Visibility = Visibility.Collapsed;
        }
    }
}
