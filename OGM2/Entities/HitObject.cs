using System.Diagnostics;
using System.Threading;

namespace OGM2
{
    public class HitObject
    {
        public int X { get; }
        public int Y { get; }
        public int Lifespan { get; set; }
        public int Id { get; }
        public Stopwatch Dead { get; }

        public HitObject(int x, int y)
        {
            Dead = new Stopwatch();
            X = x;
            Y = y;
            Lifespan = Program.Lifespan;
            Id = Interlocked.Increment(ref Program.Id);
            Dead.Start();
        }
        
        public HitObject(int x, int y, int lifespan)
        {
            Dead = new Stopwatch();
            X = x;
            Y = y;
            Lifespan = lifespan;
            Id = Interlocked.Increment(ref Program.Id);
            Dead.Start();
        }

        public virtual bool IsDead()
        {
            return Dead.ElapsedMilliseconds > Lifespan * 1.25;
        }
    }
}