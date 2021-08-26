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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ColorsControl.xaml
    /// </summary>
    public partial class ColorsControl : UserControl
    {
        //Global Variable
        Item Colors;
        int skips, correct, cards;
        public ColorsControl()
        {
            InitializeComponent();

            correct = 0;
            skips = 0;
            cards = 0;

            //Initializing
            Colors = new Item();

            TopMenu.Content = new TopMenuControl();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;

            //This took me so long to figure out!!!!!!!!!!!!!!! 2 days!!!!!!
            await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + @"\Resources\ColorBlueQuestion.png");

            BlueCard();
        }
        private async void BlueCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\ColorBlueQuestion.png", "blue", @"\Resources\ColorBlueAnswer.png");

            RedCard();
        }
        private async void RedCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\ColorRedQuestion.png", "red", @"\Resources\ColorRedAnswer.png");

            OrangeCard();
        }
        private async void OrangeCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\CarrotQuestion.png", "carrot", @"\Resources\ColorOrangeAnswer.png");

            YellowCard();
        }
        private async void YellowCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\ColorYellowQuestion.png", "yellow", @"\Resources\ColorYellowAnswer.png");

            GreenCard();
        }


        private async void GreenCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\CabbageQuestion.png", "green", @"\Resources\GreenAnswer.png");

            //save info
            SessionLog();

            //Go to results
            var window = (MainWindow)Application.Current.MainWindow;
            window.Content = new ResultsControl();
        }

        private async Task EvaluateCard(string QuestionSource, string cardName, string AnswerSource)
        {
            await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + QuestionSource);

            var result = Colors.GetCard(cardName);
            if (result == true)
            {
                //Add to counter
                correct += 1;

                //Change picture to answer
                await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + AnswerSource);

                //Wait 2 seconds
                await Task.Delay(2000);
            }
            else if (result == false)
            {
                //Add to counter
                skips += 1;
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }
        static async Task ChangeSource(Image card, string uri)
        {

            card.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));

            await Task.Delay(1000);

        }
        private void SessionLog()
        {
            Logger log = new Logger();
            Session_Log session = new Session_Log();

            var window = (MainWindow)Application.Current.MainWindow;
            var name = window.Name;

            session.ID = log.GetID(name);
            session.Date = DateTime.Now;
            session.Correct = correct;
            session.Skips = skips;
            session.Cards_Used = cards;
            session.SessionNumber = (log.LastSession() + 1);

            log.AddSession(session);
        }
    }
}
