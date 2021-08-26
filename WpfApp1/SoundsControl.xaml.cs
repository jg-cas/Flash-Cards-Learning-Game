﻿using System;
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
    /// Interaction logic for SoundsControl.xaml
    /// </summary>
    public partial class SoundsControl : UserControl
    {
        //Global Variable
        Item sounds;
        int skips, correct, cards;
        public SoundsControl()
        {
            InitializeComponent();

            correct = 0;
            skips = 0;
            cards = 0;

            //Initializing
            sounds = new Item();

            TopMenu.Content = new TopMenuControl();

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;

            CarCard();
        }

        private async void CarCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\CarQuestion.png", "broom", @"\Resources\CarAnswer.png");

            CatCard();
        }
        private async void CatCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\CatQuestion.png", "meaw", @"\Resources\CatAnswer.png");

            DogCard();
        }


        private async void DogCard()
        {
            cards += 1;

            await EvaluateCard(@"\Resources\DogQuestion.png", "woof", @"\Resources\DogAnswer.png");

            //save info
            SessionLog();

            //Go to next card
            var window = (MainWindow)Application.Current.MainWindow;
            window.Content = new ResultsControl();
        }
        private async Task EvaluateCard(string QuestionSource, string cardName, string AnswerSource)
        {
            await ChangeSource(FlashCard, AppDomain.CurrentDomain.BaseDirectory + QuestionSource);

            var result = sounds.GetCard(cardName);
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
