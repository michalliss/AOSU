using System;
using System.Collections.Generic;
using System.Geometry;
using System.Numerics;
using Akka.Actor;
using OGM2;
using OGM2.Utils;
using OsuParsers.Enums.Beatmaps;
using SDL2;
using ECircle = OGM2.ECircle;

namespace OGame
{
    public class Renderer : UntypedActor
    {
        private SDL.SDL_Rect _cRect;
        private SDL.SDL_Rect _sRect;

        private IntPtr _appCircleTexture;
        private IntPtr _trailTexture;
        private IntPtr _circleTexture;
        private IntPtr _cursorTexture;
        private IntPtr _sliderPointTexture;

        private SDL.SDL_Rect _circleRecOut;
        private SDL.SDL_Rect _cursorRecOut;
        private SDL.SDL_Rect _trailRecOut;


        private  Entities _entities = new Entities();
        private ECursor _eCursor = new ECursor(0, 0);

        private int i;
        private readonly IntPtr _renderer;


        private readonly IntPtr window;

        private void InitializeTextures()
        {
            
            //TODO: Change textures
            _circleTexture =
                SDL_image.IMG_LoadTexture(_renderer, "/home/legusie/RiderProjects/OGM2/OGM2/Resources/circle.png");
            _cursorTexture =
                SDL_image.IMG_LoadTexture(_renderer, "/home/legusie/RiderProjects/OGM2/OGM2/Resources/cursor.png");
            _trailTexture = SDL_image.IMG_LoadTexture(_renderer,
                "/home/legusie/RiderProjects/OGM2/OGM2/Resources/cursortrail.png");
            SDL_image.IMG_LoadTexture(_renderer, "/home/legusie/RiderProjects/OGM2/OGM2/Resources/cursor.png");
            _appCircleTexture = SDL_image.IMG_LoadTexture(_renderer,
                "/home/legusie/RiderProjects/OGM2/OGM2/Resources/approachcircle.png");
            _sliderPointTexture = SDL_image.IMG_LoadTexture(_renderer,
                "/home/legusie/RiderProjects/OGM2/OGM2/Resources/sliderscorepoint.png");
        }


        public Renderer(Entities entities)
        {
            _entities = entities;
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            window = SDL.SDL_CreateWindow("Hello", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 1000, 800,
                0);
            _renderer = SDL.SDL_CreateRenderer(window, -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            InitializeTextures();


            SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(_renderer);


            _circleRecOut.w = Program.CircleSize;
            _circleRecOut.h = Program.CircleSize;

            _cursorRecOut.w = Program.CursorSize;
            _cursorRecOut.h = Program.CursorSize;

            _trailRecOut.w = Program.TrailSize;
            _trailRecOut.h = Program.TrailSize;

            _sRect.x = 0;
            _sRect.y = 0;

            _sRect.w = 256;
            _sRect.h = 256;

            _cRect.w = 256;
            _cRect.h = 256;
        }

        protected override void OnReceive(object message)
        {
            if (message is DrawMessage)
                switch (message)
                {
                    case DrawMessage.Update:
                    {
                        Update();
                        break;
                    }
                }

            if (message is Trail)
            {
                _entities.Trail.Enqueue((Trail) message);
            }

            if (message is ECursor)
            {
                _entities.ECursor.X = ((ECursor) message).X;
                _entities.ECursor.Y = ((ECursor) message).Y;
            }
        }

        private void Update()
        {
            SDL.SDL_RenderClear(_renderer);

            foreach (var hitObject in _entities.HitObjects.Values)
            {
                switch (hitObject)
                {
                    case ECircle circle:
                    {
                        // Draw Circle
                        _circleRecOut.h = Program.CircleSize;
                        _circleRecOut.w = Program.CircleSize;
                        _circleRecOut.x = circle.X - Program.CircleSize / 2;
                        _circleRecOut.y = circle.Y - Program.CircleSize / 2;
                        SDL.SDL_RenderCopy(_renderer, _circleTexture, ref _sRect, ref _circleRecOut);


                        // Draw Approcach Circle
                        var scale = (1.0 + circle.Lifespan / 2.0 - circle.Dead.ElapsedMilliseconds) /
                                    circle.Lifespan;

                        _circleRecOut.x =
                            (int) (circle.X - Program.HitCircleSize / 2 * scale - Program.HitCircleSize / 2);
                        _circleRecOut.y =
                            (int) (circle.Y - Program.HitCircleSize / 2 * scale - Program.HitCircleSize / 2);
                        _circleRecOut.h = (int) (Program.HitCircleSize * scale + Program.HitCircleSize);
                        _circleRecOut.w = (int) (Program.HitCircleSize * scale + Program.HitCircleSize);
                        SDL.SDL_RenderCopy(_renderer, _appCircleTexture, ref _sRect, ref _circleRecOut);
                        break;
                    }

                    case ESlider slider:
                    {
                        // Draw Slider Head
                        _circleRecOut.h = Program.CircleSize;
                        _circleRecOut.w = Program.CircleSize;
                        _circleRecOut.x = slider.X - Program.CircleSize / 2;
                        _circleRecOut.y = slider.Y - Program.CircleSize / 2;
                        SDL.SDL_RenderCopy(_renderer, _circleTexture, ref _sRect, ref _circleRecOut);


                        // Draw Slider Body
                        switch (slider.CurvType)
                        {
                            case CurveType.Bezier:
                                for (int i = 0; i < 10; i++)
                                {
                                    var p = BezierCurve.getPoint(i / 10.0, slider.Points);
                                    _circleRecOut.h = Program.SliderPointSize;
                                    _circleRecOut.w = Program.SliderPointSize;
                                    _circleRecOut.x = (int) p.X - Program.SliderPointSize / 2;
                                    _circleRecOut.y = (int) p.Y - Program.SliderPointSize / 2;
                                    SDL.SDL_RenderCopy(_renderer, _sliderPointTexture, ref _sRect, ref _circleRecOut);
                                }

                                break;

                            case CurveType.Linear:

                                List<Line> lines = new List<Line>();
                                for (int i = 0; i < slider.Points.Count - 1; i++)
                                {
                                    lines.Add(new Line(slider.Points[i], slider.Points[i + 1]));
                                }

                                for (int i = 0; i < lines.Count; i++)
                                {
                                    for (int j = 0; j < 10; j++)
                                    {
                                        var p = lines[i].Position((float) j / 10);
                                        _circleRecOut.h = Program.SliderPointSize;
                                        _circleRecOut.w = Program.SliderPointSize;
                                        _circleRecOut.x = (int) p.X - Program.SliderPointSize / 2;
                                        _circleRecOut.y = (int) p.Y - Program.SliderPointSize / 2;
                                        SDL.SDL_RenderCopy(_renderer, _sliderPointTexture, ref _sRect,
                                            ref _circleRecOut);
                                    }
                                }

                                break;


                            case CurveType.PerfectCurve:
                                var circle = new CircleCurve(slider.Points[0], slider.Points[1], slider.Points[2]);
                                var trueLength = slider.Length / circle.Length();
                                for (int i = 0; i < 10; i++)
                                {
                                    var p = circle.Position(trueLength * i / 10);
                                    _circleRecOut.h = Program.SliderPointSize;
                                    _circleRecOut.w = Program.SliderPointSize;
                                    _circleRecOut.x = (int) p.X - Program.SliderPointSize / 2;
                                    _circleRecOut.y = (int) p.Y - Program.SliderPointSize / 2;
                                    SDL.SDL_RenderCopy(_renderer, _sliderPointTexture, ref _sRect, ref _circleRecOut);
                                }

                                break;
                        }

/*
                        foreach (var point in slider.Points)
                        {
                            _circleRecOut.h = Program.CircleSize;
                            _circleRecOut.w = Program.CircleSize;
                            _circleRecOut.x = (int) point.X - Program.SliderPointSize / 2;
                            _circleRecOut.y = (int) point.Y - Program.SliderPointSize / 2;
                            SDL.SDL_RenderCopy(_renderer, _sliderPointTexture, ref _sRect, ref _circleRecOut);
                        }
*/
                        break;
                    }
                }
            }

            
            //Draw Trail
            foreach (var var in _entities.Trail)
            {
                _trailRecOut.x = var.X - Program.TrailSize / 2;
                _trailRecOut.y = var.Y - Program.TrailSize / 2;
                SDL.SDL_RenderCopy(_renderer, _trailTexture, ref _sRect, ref _trailRecOut);
            }


            //Draw Cursor
            Console.WriteLine(_entities.ECursor.X);
            _cursorRecOut.x = _entities.ECursor.X - Program.CursorSize / 2;
            _cursorRecOut.y = _entities.ECursor.Y - Program.CursorSize / 2;
            SDL.SDL_RenderCopy(_renderer, _cursorTexture, ref _sRect, ref _cursorRecOut);
            SDL.SDL_RenderPresent(_renderer);
        }
    }
}