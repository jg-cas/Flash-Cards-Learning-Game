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
    /// Interaction logic for AnimalsControl.xaml
    /// </summary>
    public partial class AnimalsControl : UserControl
    {
        //Global Variable
        Item Animals;
        int correct, skips, cards;
        public AnimalsControl()
        {
            InitializeComponent();
            correct = 0;
            skips = 0;
            cards = 0;

            TopMenu.Content = new TopMenuControl();

            //Initializing
            Animals = new Item();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;

          DogCard();
        }
        private async void DogCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\DogQuestion.png", "dog", @"\Resources\DogAnswer.png");

            CatCard();
        }
        private async void CatCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\CatQuestion.png", "cat", @"\Resources\CatAnswer.png");

            CowCard();
        }
        private async void CowCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\CowQuestion.png", "cow", @"\Resources\CowAnswer.png");

            HorseCard();

        }
        private async void HorseCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\HorseQuestion.png", "horse", @"\Resources\HorseAnswer.png");

            SheepCard();
        }


        private async void SheepCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\SheepQuestion.png", "sheep", @"\Resources\SheepAnswer.png");

            //save info
            SessionLog();

            //Go to results
            var window = (MainWindow)Application.Current.MainWindow;
            window.Content = new ResultsControl();
        }

        private async Task EvaluateCard(string QuestionSource, string cardName, string AnswerSource)
        {
            await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + QuestionSource);

            var result = Animals.GetCard(cardName);
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
