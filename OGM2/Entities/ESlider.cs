using System;
using System.Collections.Generic;
using System.Numerics;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Enums.Beatmaps;

namespace OGM2
{
    public class ESlider : HitObject
    {
        public List<Vector2> Points { get; }
        public CurveType CurvType { get; }
        public double Length { get; }

        public ESlider(int x, int y, List<Vector2> points, CurveType curveType, double length) : base(x, y)
        {
            Length = length;
            Points = new List<Vector2>(points);
            Points.Insert(0, new Vector2(x,y));
            CurvType = curveType;
        }
        
    }
}