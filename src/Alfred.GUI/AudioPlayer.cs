using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Alfred.GUI
{
    public static class AudioPlayer
    {
        public static async Task PlayAudio(string text)
        {
            MediaElement mediaElement = new MediaElement();
            SpeechSynthesizer synth = new SpeechSynthesizer();

            foreach (VoiceInformation voice in SpeechSynthesizer.AllVoices)
            {
                Debug.WriteLine(voice.DisplayName + ", " + voice.Description);
            }

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
