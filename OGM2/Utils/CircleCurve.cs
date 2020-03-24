using System;
using System.Drawing;
using System.Numerics;

namespace OGM2.Utils
{
    public class CircleCurve
    {
        private Vector2 _center;
        private double _radius;
        private double _angle;
        private Vector2 _last;

        public CircleCurve(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var (cen, rad) = FindCircle(p1, p2, p3);
            _radius = rad;
            _center = cen;
            _last = p3;
            var diffVector = p1 - _center;
            _angle = Math.Atan2(diffVector.Y, diffVector.X);
        }

        public double Length()
        {
            return 2 * Math.PI * _radius;
        }

        private Vector2 PosAngle(double angle)
        {
            angle = _angle + angle; 
            var diffVec = new Vector2((float) (_radius * Math.Cos(angle)), (float) (_radius * Math.Sin(angle)));
            return _center + diffVec;
        }

        public Vector2 Position(double t)
        {
            var T = 2 * Math.PI;
            return PosAngle(t * T);
        }

        public static Tuple<Vector2, double> FindCircle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            //Finds circle parameters based on three points
            double x12 = p1.X - p2.X;
            double x13 = p1.X - p3.X;

            double y12 = p1.Y - p2.Y;
            double y13 = p1.Y - p3.Y;

            double y31 = p3.Y - p1.Y;
            double y21 = p2.Y - p1.Y;

            double x31 = p3.X - p1.X;
            double x21 = p2.X - p1.X;
            
            double sx13 = (Math.Pow(p1.X, 2) - Math.Pow(p3.X, 2));
            
            double sy13 = (Math.Pow(p1.Y, 2) - Math.Pow(p3.Y, 2));

            double sx21 = (Math.Pow(p2.X, 2) - Math.Pow(p1.X, 2));

            double sy21 = (Math.Pow(p2.Y, 2) - Math.Pow(p1.Y, 2));

            double f = ((sx13) * (x12)
                        + (sy13) * (x12)
                        + (sx21) * (x13)
                        + (sy21) * (x13))
                       / (2 * ((y31) * (x12) - (y21) * (x13)));
            double g = ((sx13) * (y12)
                        + (sy13) * (y12)
                        + (sx21) * (y13)
                        + (sy21) * (y13))
                       / (2 * ((x31) * (y12) - (x21) * (y13)));

            double c = -(int) Math.Pow(p1.X, 2) - (int) Math.Pow(p1.Y, 2) -
                       2 * g * p1.X - 2 * f * p1.Y;
            
            float h = (float) -g;
            float k = (float) -f;
            double sqr_of_r = h * h + k * k - c;
            
            double r = Math.Round(Math.Sqrt(sqr_of_r), 5);
            return new Tuple<Vector2, double>(new Vector2(h, k), r);
        }
    }
}