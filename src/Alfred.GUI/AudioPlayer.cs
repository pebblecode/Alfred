using System;
using System.Diagnostics;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Alfred.GUI
{
    public static class AudioPlayer
    {

        public static async void PlayAudio(string text)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    _PlayAudio(text);
                });
        }

        private static async void _PlayAudio(string text)
        {
            MediaElement mediaElement = new MediaElement();
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);

            // Send the stream to the media object.
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();

            mediaElement.Stop();
            synth.Dispose();
        }
    }
}
