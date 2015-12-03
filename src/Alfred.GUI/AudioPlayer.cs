using System;
using System.IO;
using Windows.Media.Core;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Alfred.GUI
{
    public static class AudioPlayer
    {

        public static async void Speak(string text)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    _Speak(text);
                });
        }

        public static async void PlayAudio(string song)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    _PlayAudio(song);
                });
        }

        private static async void _PlayAudio(string text)
        {
            var mediaElement = new MediaElement();
            var storageFile = await StorageFile.GetFileFromPathAsync(text);
            var stream = await storageFile.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream, storageFile.ContentType);
            mediaElement.Play();
            mediaElement.Stop();
        }

        private static async void _Speak(string text)
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
