using System;
using System.Linq;
using Akka.Actor;

namespace OGM2.Actors
{
    public class Referee : UntypedActor
    {
        private Entities _entities;
        private IActorRef _audio;

        public Referee(Entities entities, IActorRef audio)
        {
            _entities = entities;
            _audio = audio;
        }

        protected override void OnReceive(object message)
        {
            if (message is ECursor cursor)
            {
                int x = cursor.X;
                int y = cursor.Y;

                foreach (var key in _entities.HitObjects.Keys)
                {
                    var circle = _entities.HitObjects[key];
                    double distance = Math.Sqrt((x - circle.X) * (x - circle.X) + (y - circle.Y) * (y - circle.Y));
                    if (distance < Program.CircleSize / 2.0)
                    {
                        _entities.HitObjects.TryRemove(key, out circle);
                        _audio.Tell(AudioMessage.Hit);
                        break;
                    }
                }
            }
        }

        private void HandleHits()
        {
            
        }
    }
}