using System;
using System.Diagnostics;
using System.Geometry;
using System.Linq;
using Akka.Actor;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Decoders;

namespace OGM2.Actors
{
    public class MapReader : UntypedActor
    {
        private IActorRef _renderer;
        private readonly IActorRef _audio;
        private readonly Beatmap _beatmap;
        private int _nextNote = 0;
        private Entities Entities { get; }


        public MapReader(IActorRef renderer, IActorRef audio, Entities entities)
        {
            // TODO: Beatmap selector
            _renderer = renderer;
            _audio = audio;
            _beatmap = BeatmapDecoder.Decode("/home/legusie/RiderProjects/OGM2/OGM2/Resources/map.osu");
            Entities = entities;
        }

        protected override void OnReceive(object message)
        {
            if (message is TickMessage.Tick)
            {
                Update();
                RemoveDead();
            }
        }

        private void RemoveDead()
        {
            HitObject tempRemoved;
            foreach (var hitObject in Entities.HitObjects.Values)
            {
                switch (hitObject)
                {
                    case ECircle circle:
                    {
                        if (circle.IsDead())
                        {
                            Entities.HitObjects.TryRemove(hitObject.Id, out tempRemoved);
                        }

                        break;
                    }

                    case ESlider slider:
                    {
                        if (slider.IsDead())
                        {
                            Entities.HitObjects.TryRemove(hitObject.Id, out tempRemoved);
                        }

                        break;
                    }
                }
            }
        }

        private void Update()
        {
            long audioSyncTime = (long) _audio.Ask(AudioMessage.Time).Result;
            while (_beatmap.HitObjects.Count > _nextNote &&
                   _beatmap.HitObjects[_nextNote].StartTime - Program.Lifespan < audioSyncTime)
            {
                switch (_beatmap.HitObjects[_nextNote])
                {
                    case Slider s:
                    {
                        var slider = new ESlider((int) (s.Position.X), (int) s.Position.Y, s.SliderPoints, s.CurveType,
                            s.PixelLength);
                        slider.Lifespan = Program.Lifespan + s.TotalTimeSpan.Milliseconds;
                        Entities.HitObjects.TryAdd(slider.Id, slider);
                        break;
                    }


                    case HitCircle c:
                    {
                        var circle = new ECircle((int) c.Position.X, (int) c.Position.Y);
                        Entities.HitObjects.TryAdd(circle.Id, circle);
                        break;
                    }
                }

                _nextNote++;
            }
        }
    }
}