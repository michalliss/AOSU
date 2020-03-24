using System;
using System.Threading;
using Akka;
using Akka.Actor;
using OGame;
using OGM2;
using OGM2.Actors;
using SDL2;

namespace OGM2
{
    class Program
    {
        public const int CircleSize = 64;
        public const int CursorSize = 32;
        public const int TrailSize = 32;
        public const int HitCircleSize = 128;
        public const int SliderPointSize = 20;
        public const int Lifespan = 500;
        static public int Id = 0;
        
        
        static void Main(string[] args)
        {
            var ent = new Entities();
            var system = ActorSystem.Create("system");
            var audio = system.ActorOf<Audio>();
            var renderer = system.ActorOf(Props.Create<Renderer>(ent));
            var referee = system.ActorOf(Props.Create<Referee>(ent, audio));
            var keyboard = system.ActorOf(Props.Create<Input>(renderer, audio, referee));
            var map = system.ActorOf(Props.Create<MapReader>(renderer, audio, ent));
            var tick = system.ActorOf(Props.Create<TickGenerator>(renderer, map, audio));
            audio.Tell(AudioMessage.Play);

            Thread.Sleep(10000000);
        }
    }
}