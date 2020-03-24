using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace OGM2.Utils
{
    public static class BezierCurve
    {
        public static Vector2 getPoint(double time, IList<Vector2> points)
        {
            var point = GetBezierPoint(time, points, 0, points.Count);
            return point;
        }


        public static Vector2 GetBezierPoint(double t, IList<Vector2> controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Vector2((int) ((1 - t) * P0.X + t * P1.X), (int) ((1 - t) * P0.Y + t * P1.Y));
        }
    }
}