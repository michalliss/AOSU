using System.Diagnostics;
using System.Threading;
using Akka.Actor;
using OGame;
using OGM2.Actors;

namespace OGM2
{
    public class TickGenerator : UntypedActor
    {
        private IActorRef _renderer;
        private IActorRef _mapReader;
        private IActorRef _audio;
        private Timer _timer;
        

        public TickGenerator(IActorRef renderer, IActorRef mapReader, IActorRef audio)
        {
            _renderer = renderer;
            _mapReader = mapReader;
            _audio = audio;
            _timer = new Timer(Update, null, 0, 10);
        }

        private void Update(object o)
        {
            _renderer.Tell(DrawMessage.Update);
            _mapReader.Tell(TickMessage.Tick);
        }

        protected override void OnReceive(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}