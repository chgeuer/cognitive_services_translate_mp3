namespace lib
{
    using System;
    using System.IO;
    using NAudio.MediaFoundation;
    using NAudio.Wave;

    public static class SoundExtensions
    {
        const int audio_sample_rate = 16000;
        const int audio_bit_per_sample = 16;
        const int audio_channels = 1;
        const int desiredMP3bitRate = 48000;

        /// <summary>
        /// MP3 to WAV conversion.
        /// </summary>
        /// <param name="mp3Stream"></param>
        /// <returns></returns>
        public static byte[] ConvertMP3(this Stream mp3Stream)
        {
            using var reader = new Mp3FileReader(mp3Stream);
            using var resampled = new WaveFormatConversionStream(
                new WaveFormat(
                    rate: audio_sample_rate,
                    bits: audio_bit_per_sample,
                    channels: audio_channels), 
                reader);
            using var wavStream = new MemoryStream();
            WaveFileWriter.WriteWavFileToStream(wavStream, resampled);
            return wavStream.ToArray();
        }

        /// <summary>
        /// Converts WAV content to MP3.
        /// </summary>
        /// <param name="wavStream"></param>
        /// <param name="mp3ResultFile"></param>
        public static void SaveWavToMp3File(this Stream wavStream, string mp3ResultFile)
        {
            MediaFoundationApi.Startup();
            try
            {
                using var reader = new WaveFileReader(inputStream: wavStream);

                //MediaFoundationEncoder.EncodeToMp3(reader, outputFile: mp3ResultFile, desiredBitRate: desiredMP3bitRate);

                var mediaType = MediaFoundationEncoder.SelectMediaType(
                    AudioSubtypes.MFAudioFormat_MP3, 
                    new WaveFormat(sampleRate: audio_sample_rate, channels: audio_channels), 
                    desiredBitRate: desiredMP3bitRate);
                if (mediaType == null)
                {
                    throw new NotSupportedException();
                }
                using var enc = new MediaFoundationEncoder(mediaType);
                enc.Encode(outputFile: mp3ResultFile, reader);
            }
            finally
            {
                MediaFoundationApi.Shutdown();
            }
        }

        #region FFMPEG fragments
        //private static async Task<byte[]> ConvertMP3_FFMPEG(this Stream mp3Stream)
        //{
        //    var audio_codec = audio_bit_per_sample switch
        //    {
        //        16 => "pcm_s16le",
        //        _ => throw new ArgumentException(
        //            $"Unsupported bits_per_sample {audio_bit_per_sample}",
        //            paramName: nameof(audio_bit_per_sample)),
        //    };
        //
        //    string readFromStdin = "-i -";
        //    string produceWAV = $"-f wav -c:a {audio_codec}";
        //    string audioParams = $"-ar {audio_sample_rate} -ac {audio_channels}";
        //    string writeToStdout = "-";
        //    string ffmpeg_args = $"{readFromStdin} {produceWAV} {audioParams} {writeToStdout}";
        //
        //    var result = await ProcessExtensions.RunAsync("ffmpeg", ffmpeg_args, mp3Stream);
        //    var log = System.Text.Encoding.UTF8.GetString(result.Stderr);
        //
        //    return result.Stdout; //.TweakWavLength();
        //}
        //
        //private static async Task<byte[]> ConvertWAV2MP3_FFMPEG(this Stream stream)
        //{
        //    var audio_codec = "mp3";
        //    string readFromStdin = "-i -";
        //    string produceMP3 = $"-c:a {audio_codec}";
        //    string writeToStdout = "-";
        //    string ffmpeg_args = $"{readFromStdin} {produceMP3} {writeToStdout}";
        //    var result = await ProcessExtensions.RunAsync("ffmpeg", ffmpeg_args, stream);
        //    var log = System.Text.Encoding.UTF8.GetString(result.Stderr);
        //    return result.Stdout;
        //}
        //
        //private static byte[] CloneArray(this byte[] array)
        //{
        //    byte[] result = new byte[array.Length];
        //    Buffer.BlockCopy(array, 0, result, 0, array.Length * sizeof(byte));
        //    return result;
        //}
        //
        //private static byte[] TweakWavLength(this byte[] wavByteArray)
        //{
        //    // un-fuck the WAV length...
        //    // mplayer.exe -demuxer rawaudio -rawaudio rate=16000:channels=1:samplesize=2 -ao pcm out.wav
        //    var bytes = wavByteArray.CloneArray();
        //    var byteLength = bytes.Length;
        //    void tweak(int index, int rotate, long subtractionOffset)
        //    {
        //        var length = byteLength - subtractionOffset;
        //        bytes[index] = (byte)((length >> (rotate * 8)) & 0xff);
        //    }
        //    void patchLong(int startIndex, long subtractionOffset)
        //    {
        //        for (var i = 0; i < 4; i++)
        //        {
        //            tweak(startIndex + i, i, subtractionOffset);
        //        }
        //    }
        //    patchLong(0x04, 8);
        //    patchLong(0x74, 120);
        //    return bytes;
        //}
        #endregion
    }
}