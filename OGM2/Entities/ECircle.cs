using System;
using System.Diagnostics;
using System.Threading;

namespace OGM2
{
    public class ECircle : HitObject
    {
        private AppCircle AppCircle;

        public ECircle(int x, int y) : base(x, y)
        {
            AppCircle = new AppCircle(x, y);
        }
    }
}