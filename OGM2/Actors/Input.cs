using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using Akka.Actor;
using Akka.Event;
using OGM2;
using SDL2;

namespace OGame
{
    public class Input : UntypedActor

    {
        private IActorRef _renderer;
        private IActorRef _audio;
        private IActorRef _referee;

        public Input(IActorRef renderer, IActorRef audio, IActorRef referee)
        {
            _renderer = renderer;
            _audio = audio;
            _referee = referee;
            var cursorLag = new Stopwatch();
            cursorLag.Start();


            SDL.SDL_Event e;
            while (true)
            {
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    SDL.SDL_GetMouseState(out var x, out var y);
                    switch (e.type)
                    {
                        case SDL.SDL_EventType.SDL_KEYDOWN:

                            switch (e.key.keysym.sym)
                            {
                                case SDL.SDL_Keycode.SDLK_f:
                                    referee.Tell(new ECursor(x, y));
                                    break;
                                case SDL.SDL_Keycode.SDLK_g:
                                    referee.Tell(new ECursor(x, y));
                                    break;
                            }
                            break;

                        case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:

                            switch (e.button.button)
                            {
                                case 1:
                                    break;
                                case 3:
                                    break;
                            }

                            break;


                        case SDL.SDL_EventType.SDL_MOUSEMOTION:
                            int mx = e.motion.x;
                            int my = e.motion.y;
                            renderer.Tell(new ECursor(mx, my));
                            break;
                    }
                }

                if (cursorLag.ElapsedMilliseconds > 3)
                {
                    
                    SDL.SDL_GetMouseState(out var x, out var y);
                    renderer.Tell(new Trail(x, y));
                    cursorLag.Restart();
                }
            }
        }

        protected override void OnReceive(object message)
        {
        }
    }
}