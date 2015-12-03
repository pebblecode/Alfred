﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Alfred.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Grammer File
        private const string SRGS_FILE = "Grammar\\grammar.xml";

        private SpeechRecognizer _recognizer;

        public MainPage()
        {
            this.InitializeComponent();

            Unloaded += MainPage_Unloaded;

            // Initialize Recognizer
            InitializeSpeechRecognizer();
        }

        // Release resources, stop recognizer, release pins, etc...
        private async void MainPage_Unloaded(object sender, object args)
        {
            // Stop recognizing
            await _recognizer.ContinuousRecognitionSession.StopAsync();

            // Release pins
            _recognizer.Dispose();

            _recognizer = null;
        }

        // Initialize Speech Recognizer and start async recognition
        private async void InitializeSpeechRecognizer()
        {
            // Initialize recognizer
            _recognizer = new SpeechRecognizer();

            // Set event handlers
            _recognizer.StateChanged += RecognizerStateChanged;
            _recognizer.ContinuousRecognitionSession.ResultGenerated += RecognizerResultGenerated;

            // Load Grammer file constraint
            string fileName = String.Format(SRGS_FILE);
            StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);

            SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);

            // Add to grammer constraint
            _recognizer.Constraints.Add(grammarConstraint);

            // Compile grammer
            SpeechRecognitionCompilationResult compilationResult = await _recognizer.CompileConstraintsAsync();

            Debug.WriteLine("Status: " + compilationResult.Status);

            // If successful, display the recognition result.
            if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
            {
                Debug.WriteLine("Result: " + compilationResult);

                await _recognizer.ContinuousRecognitionSession.StartAsync();
            }
            else
            {
                Debug.WriteLine("Status: " + compilationResult.Status);
            }
        }

        // Recognizer generated results
        private async void RecognizerResultGenerated(SpeechContinuousRecognitionSession session, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            // Output debug strings
            Debug.WriteLine(args.Result.Status);
            Debug.WriteLine(args.Result.Text);

            var cmd = args.Result.SemanticInterpretation.Properties["cmd"].Single();

            switch (cmd)
            {
                case ("Sick"):
                {
                    Debug.WriteLine("You are sick!");
                    await Task.WhenAll(WfhApi.SetStatus(WfhStatus.Sick), AudioPlayer.PlayAudio("Sorry to hear that, sir"));
                   break;
                }
                case ("Holiday"):
                {
                    Debug.WriteLine("You are on holiday today!");
                    Task.Run(() => WfhApi.SetStatus(WfhStatus.Holiday));
                        Task.Run(() => AudioPlayer.PlayAudio("Lucky you, sir!"));
                        break;
                }
                case ("Home"):
                {
                    Debug.WriteLine("You are working from home today!");
                    Task.Run(() => WfhApi.SetStatus(WfhStatus.OutOfOffice));
                        Task.Run(() => AudioPlayer.PlayAudio("I'll bring you some tea, sir"));
                        break;
                }
                case ("Office"):
                {
                    Debug.WriteLine("You are working from office today!");
                    Task.Run(() => WfhApi.SetStatus(WfhStatus.InOffice));
                    Task.Run(() => AudioPlayer.PlayAudio("Jolly good, sir"));
                        break;
                }
                default:
                {
                    Debug.WriteLine("You are something else");
                        Task.Run(() => AudioPlayer.PlayAudio("I'm sorry, sir, what was that?"));
                        break;
                }
            }
        }

        // Recognizer state changed
        private void RecognizerStateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            Debug.WriteLine("Speech recognizer state: " + args.State);
        }
    }
}
