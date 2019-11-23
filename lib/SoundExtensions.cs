namespace lib
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using NAudio.Wave;

    public enum Mp3ConversionAlgorithm
    {
        NAudio,
        FFMPEG
    }

    public static class SoundExtensions
    {
        public static Task<byte[]> ConvertMP3(this byte[] mp3Bytes, Mp3ConversionAlgorithm algo)
        {
            return algo switch
            {
                Mp3ConversionAlgorithm.NAudio => Task.FromResult(mp3Bytes.ConvertMP3_NAudio()),
                Mp3ConversionAlgorithm.FFMPEG => mp3Bytes.ConvertMP3_FFMPEG(),
                _ => throw new ArgumentException($"Unknown MP3-to-WAV algorithm {algo}", paramName: nameof(algo)),
            };
        }

        private static byte[] ConvertMP3_NAudio(this byte[] mp3Bytes)
        {
            var waveFormat = new WaveFormat(rate: 16000, bits: 16, channels: 1);
            using var mp3Stream = new MemoryStream(mp3Bytes);
            using var reader = new Mp3FileReader(mp3Stream);
            using var resampler = new WaveFormatConversionStream(waveFormat, reader);
            using var wavStream = new MemoryStream();
            WaveFileWriter.WriteWavFileToStream(wavStream, resampler);
            return wavStream.ToArray();
        }

        private static async Task<byte[]> ConvertMP3_FFMPEG(this byte[] mp3Bytes)
        {
            var result = await ProcessExtensions.RunAsync(
                filename: "ffmpeg",
                arguments: $"-i - -f wav -c:a pcm_s16le -ar 16000 -ac 1 -");
            return result.Stdout.TweakWavLengthInMemory();
        }

        private static byte[] TweakWavLengthInMemory(this byte[] wavByteArray)
        {
            // un-fuck the WAV length...
            // mplayer.exe -demuxer rawaudio -rawaudio rate=16000:channels=1:samplesize=2 -ao pcm out.wav
            // This modifies wavByteArray in memory
            void tweak(int index, int rotate, long subtractionOffset)
            {
                var length = wavByteArray.Length - subtractionOffset;
                wavByteArray[index] = (byte)((length >> (rotate * 8)) & 0xff);
            }
            void patchLong(int startIndex, long subtractionOffset)
            {
                for (var i = 0; i < 4; i++)
                {
                    tweak(startIndex + i, i, subtractionOffset);
                }
            }

            patchLong(0x04, 8);
            patchLong(0x74, 120);

            return wavByteArray;
        }
    }
}