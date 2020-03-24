using System.Collections.Concurrent;
using System.Text;
using Akka.Actor;
using OsuParsers.Beatmaps.Objects;
using SDL2;

namespace OGM2
{
    public class Entities
    {
        public ConcurrentDictionary<int, HitObject> HitObjects = new ConcurrentDictionary<int, HitObject>();
        public CircularBuffer<Trail> Trail = new CircularBuffer<Trail>(20);
        public ECursor ECursor = new ECursor(0,0);
    }
}