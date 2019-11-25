namespace lib
{
    using System;
    using System.IO;
    using Microsoft.CognitiveServices.Speech.Translation;

    public class SynthesizingWriter
    {
        const string pattern = "{}";
        private readonly string _outputFilenamePattern;
        public SynthesizingWriter(string outputFilenamePattern)
        {
            if (!outputFilenamePattern.Contains(pattern))
            {
                throw new ArgumentException(
                    message: $"The pattern must contain the string '{pattern}'",
                    paramName: nameof(outputFilenamePattern));
            }
            _outputFilenamePattern = outputFilenamePattern;
        }

        private readonly object _lock = new object();
        private int _count = 0;
        private string GetNextFilename()
        {
            lock (_lock)
            {
                _count++;
                return _outputFilenamePattern.Replace(pattern, _count.ToString());
            }
        }

        public void Synthesizing(object sender, TranslationSynthesisEventArgs e)
        {
            byte[] audioBytes = e.Result.GetAudio();
            if (audioBytes.Length == 0) { return; }

            var outputFilename = GetNextFilename();

            if (outputFilename.EndsWith(".wav"))
            {
                using var fs = File.OpenWrite(outputFilename);
                fs.Write(audioBytes, 0, audioBytes.Length);
            }
            else
            {
                using var ms = new MemoryStream(audioBytes);
                ms.SaveWavToMp3File(outputFilename);
            }
        }
    }
}