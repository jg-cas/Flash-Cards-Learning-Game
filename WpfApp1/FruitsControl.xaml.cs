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
using CardsClassifications;
using LogLibrary;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for FruitsControl.xaml
    /// </summary>
    public partial class FruitsControl : UserControl
    {
        //Global Variable
        Item fruits;
        int skips, correct, cards;
        public FruitsControl()
        {
            InitializeComponent();

            correct = 0;
            skips = 0;
            cards = 0;

            //Initializing
            fruits = new Item();

            TopMenu.Content = new TopMenuControl();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;

            AppleCard();
        }

        private async void AppleCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\AppleQuestion.png", "apple", @"\Resources\AppleAnswer.png");

            BananaCard();
        }
        private async void BananaCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\BananaQuestion.png", "banana", @"\Resources\BananaAnswer.png");

            OrangeCard();
        }
        private async void OrangeCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\OrangeQuestion.png", "orange", @"\Resources\OrangeAnswer.png");

            GrapeCard();
        }
        private async void GrapeCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\GrapeQuestion.png", "grape", @"\Resources\GrapeAnswer.png");

            StrawberryCard();
        }


        private async void StrawberryCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\StrawberryQuestion.png", "strawberry", @"\Resources\StrawberryAnswer.png");

            GrapeCard();

            //save info
            SessionLog();

            //Go to results
            var window = (MainWindow)Application.Current.MainWindow;
            window.Content = new ResultsControl();
        }
        private async Task EvaluateCard(string QuestionSource, string cardName, string AnswerSource)
        {
            await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + QuestionSource);

            var result = fruits.GetCard(cardName);
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
