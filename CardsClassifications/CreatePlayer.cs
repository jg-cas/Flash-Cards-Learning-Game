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
    public class CreatePlayer
    {

        //Global Variable
        SpeechRecognitionEngine SpeechRecognizer;
        string addPlayer;
        string Exit;
        string Computer;
        int result;
        int flag = 1;
        int innerflag = 1;

        public bool MainCreate()
        {
            syncCreate();

            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void syncCreate()
        {
            addPlayer = "Add Player";
            Exit = "Exit";
            Computer = "computer";

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
            flag = 1;
            innerflag = 1;

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
                SpeechSynthesizer.Speak("    Add Player or Exit?");

                //Unloading Computer
                foreach (var gr in SpeechRecognizer.Grammars)
                {
                    if (gr.Name == "computerGrammar")
                    {
                        SpeechRecognizer.UnloadGrammar(gr);
                        break;
                    }
                }
                //Loading Add Player and Exit
                SpeechRecognizer.LoadGrammar(new Grammar(new GrammarBuilder(addPlayer)));
                SpeechRecognizer.LoadGrammar(new Grammar(new GrammarBuilder(Exit)));

                //Request Recognizer Update while still Running
                SpeechRecognizer.RequestRecognizerUpdate();

                while (innerflag == 1)
                {
                    //Recognize Synchronously
                    SpeechRecognizer.Recognize();
                }
            }
            else if (e.Result.Text == addPlayer || e.Result.Text == Exit)
            {
                innerflag = 0;

                if (e.Result.Text == addPlayer)
                {
                    //PC Response
                    SpeechSynthesizer.Speak("    Trying to Add");

                    //Disposing of speech synthesizer
                    SpeechSynthesizer.Dispose();

                    //Returning Boolean value as a Number since Event Handlers cannot return Values
                    result = 1;

                    //Disposin of Recognizer
                    SpeechRecognizer.Dispose();
                }
                else if (e.Result.Text == Exit)
                {
                    //PC Response
                    SpeechSynthesizer.Speak("    Goodbye");

                    //Disposing of speech synthesizer
                    SpeechSynthesizer.Dispose();

                    result = 2;

                    //completed.Set();
                    SpeechRecognizer.Dispose();
                }
            }
        }
    }
}
