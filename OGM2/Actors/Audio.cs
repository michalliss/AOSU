using System;
using Akka.Actor;
using ManagedBass;
using OGM2;
using OGM2.Actors;
using MediaPlayer = ManagedBass.MediaPlayer;

namespace OGame
{
    class Audio : UntypedActor
    {
        private MediaPlayer _music;
        private MediaPlayer _hitsounds;


        public Audio()
        {
            // TODO: auto select Pulseaudio
            Bass.Init(20, 44100);


            // TODO: choose sounds
            _music = new MediaPlayer();
            _music.LoadAsync("/home/legusie/RiderProjects/OGM2/OGM2/Resources/audio.mp3").Wait();

            _hitsounds = new MediaPlayer();
            _hitsounds.LoadAsync("/home/legusie/RiderProjects/OGM2/OGM2/Resources/hitsound.wav").Wait();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case AudioMessage msg:
                {
                    switch (msg)
                    {
                        case AudioMessage.Play:
                            _music.Play();
                            break;
                        case AudioMessage.Pause:
                            _music.Pause();
                            break;
                        case AudioMessage.Hit:
                            _hitsounds.Stop();
                            _hitsounds.Play();
                            break;
                        case AudioMessage.Time:
                            Sender.Tell((long) _music.Position.TotalMilliseconds);
                            break;
                    }

                    break;
                }
            }
        }
    }
}