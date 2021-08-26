using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace CardsClassifications
{
    public class Login
    {
        //Global Variable
        SpeechRecognitionEngine SpeechRecognizer;
        string newPlayer;
        string Continue;
        string Computer;
        int result;
        int flag = 1;
        int innerflag = 1;

        public bool MainLogin()
        {
            syncLogin();
            
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void syncLogin()
        {
            newPlayer = "New Player";
            Continue = "Continue";
            Computer = "Computer";


            //Loading Grammar with an Identifier allows for unloading later
            SpeechRecognizer = new SpeechRecognitionEngine();
            Grammar gr = new Grammar(new GrammarBuilder(Computer));
            gr.Name = "computerGrammar";
            SpeechRecognizer.LoadGrammar(gr);
            
            SpeechRecognizer.SetInputToDefaultAudioDevice();

            //Event Handler for Speech Recognized
            SpeechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;

            //Speech Rejected
            SpeechRecognizer.SpeechRecognitionRejected += SpeechRecognizer_SpeechRecognitionRejected;

            while (flag == 1)
            {
                //Recognize sync
                SpeechRecognizer.Recognize();
            }
        }

        private void SpeechRecognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            innerflag = 1;
            flag = 1;

            if (e.Result.Alternates.Count == 0)
            {

            }
            foreach (var phrase in e.Result.Alternates)
            {
                SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
                SpeechSynthesizer.SetOutputToDefaultAudioDevice();
                SpeechSynthesizer.Rate = -2;
                SpeechSynthesizer.SelectVoice("Microsoft Zira Desktop");
                SpeechSynthesizer.Speak($";;    I don't understand");
                SpeechSynthesizer.Dispose();
            }
        }

        //Event Handler for Speech Recognized
        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            flag = 0;

            SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            SpeechSynthesizer.SelectVoice("Microsoft Zira Desktop");
            if (e.Result.Text == Computer)
            {
                //PC Response
                SpeechSynthesizer.Speak("    add new player or continue?");
                SpeechSynthesizer.Dispose();

                //Unloading Computer
                foreach (var gr in SpeechRecognizer.Grammars)
                {
                    if (gr.Name == "computerGrammar")
                    {
                        SpeechRecognizer.UnloadGrammar(gr);
                        break;
                    }
                }

                //Loading New Player and Continue
                SpeechRecognizer.LoadGrammar(new Grammar(new GrammarBuilder(newPlayer)));
                SpeechRecognizer.LoadGrammar(new Grammar(new GrammarBuilder(Continue)));

                //Request Recognizer Update while still Running
                SpeechRecognizer.RequestRecognizerUpdate();

                while (innerflag == 1)
                {
                    //Recognize Synchronously
                    SpeechRecognizer.Recognize();
                }
            }
            else if (e.Result.Text == Continue || e.Result.Text == newPlayer)
            {
                innerflag = 0;

                if (e.Result.Text == Continue)
                {
                    //PC Response
                    
                    SpeechSynthesizer.Speak("    Welcome!!!");

                    //Disposing of speech synthesizer
                    SpeechSynthesizer.Dispose();

                    //Returning Boolean value as a Number since Event Handlers cannot return Values
                    result = 1;

                    //Disposing of Recognizer
                    SpeechRecognizer.Dispose();
                }
                else if (e.Result.Text == newPlayer)
                {
                    //PC Response
                    SpeechSynthesizer.Speak("    Okay, let's create a new player");

                    //Disposing of speech synthesizer
                    SpeechSynthesizer.Dispose();

                    result = 2;

                    SpeechRecognizer.Dispose();
                }
            }
        }
    }
}
