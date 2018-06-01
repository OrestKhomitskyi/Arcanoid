using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Arcanoid.Music
{
    class MusicPlayer : IDisposable
    {
        private List<string> list = Directory.GetFiles("MusicSource").Select(Path.GetFullPath).ToList();
        private IEnumerator<string> enumerator;
        private WaveOutEvent OutputDevice;
        private CancellationTokenSource PlayTokenSource;

        public Task Play()
        {
            PlayTokenSource = new CancellationTokenSource();

            return Task.Run(async () =>
            {
                OutputDevice = new WaveOutEvent();
                OutputDevice.Volume = 1f;
                enumerator = list.GetEnumerator();
                return;
                while (!PlayTokenSource.IsCancellationRequested)
                {
                    enumerator.MoveNext();
                    using (var audioFile = new AudioFileReader(enumerator.Current))
                    {
                        OutputDevice.Init(audioFile);
                        OutputDevice.Play();
                        while (OutputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            await Task.Delay(1000);
                        }
                    }
                }
            }, PlayTokenSource.Token);
        }
        public void Dispose()
        {
            PlayTokenSource.Cancel();
            PlayTokenSource.Dispose();
            OutputDevice.Dispose();
        }
    }
}
