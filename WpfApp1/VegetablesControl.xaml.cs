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
    /// Interaction logic for VegetablesControl.xaml
    /// </summary>
    public partial class VegetablesControl : UserControl
    {
        //Global Variable
        Item vegetables;
        int skips, correct, cards;
        string cardName;
        public VegetablesControl()
        {
            InitializeComponent();

            correct = 0;
            skips = 0;

            //Initializing
            vegetables = new Item();

            TopMenu.Content = new TopMenuControl();

        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;

            CarrotCard();
        }

        private async void CarrotCard()
        {
            cardName = "carrot";

            cards += 1;


            await EvaluateCard(@"\Resources\CarrotQuestion.png", "carrot", @"\Resources\CarrotAnswer.png");

            CabbageCard();
        }
        private async void CabbageCard()
        {
            cardName = "cabbage";

            cards += 1;


            await EvaluateCard(@"\Resources\CabbageQuestion.png", "cabbage", @"\Resources\CabbageAnswer.png");

            TomatoCard();
        }
        private async void TomatoCard()
        {
            cardName = "tomato";

            cards += 1;


            await EvaluateCard(@"\Resources\TomatoQuestion.png", "tomato", @"\Resources\TomatoAnswer.png");

            CornCard();
        }
        private async void CornCard()
        {
            cardName = "corn";

            cards += 1;


            await EvaluateCard(@"\Resources\CornQuestion.png", "corn", @"\Resources\CornAnswer.png");

            BroccoliCard();
        }


        private async void BroccoliCard()
        {
            cardName = "broccoli";

            cards += 1;


            await EvaluateCard(@"\Resources\BroccoliQuestion.png", "broccoli", @"\Resources\BroccoliAnswer.png");

            //save info
            SessionLog();

            //Go to results
            var window = (MainWindow)Application.Current.MainWindow;
            window.Content = new ResultsControl();
        }
        private async Task EvaluateCard(string QuestionSource, string cardName, string AnswerSource)
        {
            await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + QuestionSource);

            var result = vegetables.GetCard(cardName);
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

