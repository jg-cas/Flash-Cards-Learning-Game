using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; //Allows to reset tasks since there is no buttons
using System.Speech; //Speech NameSpace
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace CardsClassifications
{
    public class Item
    {
        //Global Variable
        SpeechRecognitionEngine speechRecognition;
        string itemName;
        string skip = "skip";
        int result;
        int flag;

        public bool GetCard(string Item)
        {
            flag = 1;

            itemName = Item;

            SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            SpeechSynthesizer.Rate = -2;
            SpeechSynthesizer.SelectVoice("Microsoft Zira Desktop");
            SpeechSynthesizer.Speak(";;     What;;is;;this");
            SpeechSynthesizer.Dispose();

            RecognizeSync(itemName);

            //Reset variable
            itemName = null;

            if (result == 1)
            {
                //Got it Right
                return true;
            }
            else if (result == 2)
            {
                //Skipped
                return false;
            }
            else
            {
                return false;
            }
        }

        private void RecognizeSync(string itemName)
        {
            speechRecognition = new SpeechRecognitionEngine();
            speechRecognition.LoadGrammar(new Grammar(new GrammarBuilder(itemName)));
            speechRecognition.LoadGrammar(new Grammar(new GrammarBuilder(skip)));
            speechRecognition.SetInputToDefaultAudioDevice();

            //Speech Recognized Event
            speechRecognition.SpeechRecognized += SpeechRecognizer_SpeechRecognized;

            //Speech Rejected
            speechRecognition.SpeechRecognitionRejected += SpeechRecognition_SpeechRecognitionRejected;

            while (flag == 1)
            {
                //Recognize sync
                speechRecognition.Recognize();
            }
        }

        private void SpeechRecognition_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            flag = 1;

                SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
                SpeechSynthesizer.SetOutputToDefaultAudioDevice();
                SpeechSynthesizer.Rate = -2;
                SpeechSynthesizer.SelectVoice("Microsoft Zira Desktop");
                SpeechSynthesizer.Speak($";;    Did you mean {itemName}");
                SpeechSynthesizer.Dispose();
        }

        //Event Handler for Speech Recognized
        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            flag = 0;

            SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            SpeechSynthesizer.Rate = -2;
            SpeechSynthesizer.SelectVoice("Microsoft Zira Desktop");

            if (e.Result.Text == itemName && e.Result.Confidence >= 0.08)
            {
                //PC Response
                SpeechSynthesizer.Speak(";;    Great Job!!!!");

                //Disposing of speech synthesizer
                SpeechSynthesizer.Dispose();


                //Return true
                result = 1;

                //Dispose of Speech Recognizer
                speechRecognition.Dispose();
            }
            else if (e.Result.Text == "skip" && e.Result.Confidence >= 0.05)
            {
                //PC Response
                SpeechSynthesizer.Speak(";;    You got it next time!! Keep it up!!");

                //Disposing of speech synthesizer
                SpeechSynthesizer.Dispose();

                //If Skip is Recognized then return false
                result = 2;

                //Dispose of Speech Recognizer
                speechRecognition.Dispose();
            }
        }
    }
}
