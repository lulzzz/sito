using System;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;

// recognition

namespace ConsoleSpeech
{
    class ConsoleSpeechProgram
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer();
        static SpeechRecognitionEngine sre;
        static bool done = false;
        static bool speechOn = true;

        static void Main(string[] args)
        {
            try
            {
                ss.SetOutputToDefaultAudioDevice();
                var v = ss.GetInstalledVoices().ToArray();

                ss.SelectVoice("Microsoft Zira Desktop");

                Console.WriteLine("\n(Speaking: I am awake)");
                ss.Speak("I am awake");


                CultureInfo ci = new CultureInfo("en-us");
                sre = new SpeechRecognitionEngine(ci);
                sre.SetInputToDefaultAudioDevice();
                sre.SpeechRecognized += sre_SpeechRecognized;

                Choices ch_StartStopCommands = new Choices();
                ch_StartStopCommands.Add("speech on");
                ch_StartStopCommands.Add("speech off");
                ch_StartStopCommands.Add("klatu barada nikto");
                GrammarBuilder gb_StartStop = new GrammarBuilder();
                gb_StartStop.Append(ch_StartStopCommands);
                Grammar g_StartStop = new Grammar(gb_StartStop);

                //string[] numbers = new string[] { "1", "2", "3", "4" };
                //Choices ch_Numbers = new Choices(numbers);

                //string[] numbers = new string[100];
                //for (int i = 0; i < 100; ++i)
                //  numbers[i] = i.ToString();
                //Choices ch_Numbers = new Choices(numbers);

                Choices ch_Numbers = new Choices();
                ch_Numbers.Add("1");
                ch_Numbers.Add("2");
                ch_Numbers.Add("3");
                ch_Numbers.Add("4"); // technically Add(new string[] { "4" });

                //for (int num = 1; num <= 4; ++num)
                //{
                //  ch_Numbers.Add(num.ToString());
                //}

                GrammarBuilder gb_WhatIsXplusY = new GrammarBuilder();
                gb_WhatIsXplusY.Append("What is");
                gb_WhatIsXplusY.Append(ch_Numbers);
                gb_WhatIsXplusY.Append("plus");
                gb_WhatIsXplusY.Append(ch_Numbers);
                Grammar g_WhatIsXplusY = new Grammar(gb_WhatIsXplusY);

                sre.LoadGrammarAsync(g_StartStop);
                sre.LoadGrammarAsync(g_WhatIsXplusY);

                sre.RecognizeAsync(RecognizeMode.Multiple); // multiple grammars

                while (done == false) {; }

                Console.WriteLine("\nHit <enter> to close shell\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        } // Main

        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence; // consider implicit cast to double
            Console.WriteLine("\nRecognized: " + txt);

            if (confidence < 0.60) return;

            if (txt.IndexOf("speech on") >= 0)
            {
                Console.WriteLine("Speech is now ON");
                speechOn = true;
            }

            if (txt.IndexOf("speech off") >= 0)
            {
                Console.WriteLine("Speech is now OFF");
                speechOn = false;
            }

            if (speechOn == false) return;

            if (txt.IndexOf("klatu") >= 0 && txt.IndexOf("barada") >= 0)
            {
                ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
                done = true;
                Console.WriteLine("(Speaking: Farewell)");
                ss.Speak("Farewell");
            }

            if (txt.IndexOf("What") >= 0 && txt.IndexOf("plus") >= 0) // what is 2 plus 3
            {
                string[] words = txt.Split(' ');     // or use e.Result.Words
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[4]);
                int sum = num1 + num2;
                Console.WriteLine("(Speaking: " + words[2] + " plus " + words[4] + " equals " + sum + ")");
                ss.SpeakAsync(words[2] + " plus " + words[4] + " equals " + sum);
            }
        } // sre_SpeechRecognized

    } // Program

} // ns
