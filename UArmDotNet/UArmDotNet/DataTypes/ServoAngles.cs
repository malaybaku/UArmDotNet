using System;

namespace Baku.UArmDotNet
{
    public class ServoAngles
    {
        public ServoAngles(float bottom, float left, float right)
        {
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        public float Bottom { get; }
        public float Left { get; }
        public float Right { get; }

        public float this[int i]
        {
            get
            {
                if (i < 0 || i > 2) throw new IndexOutOfRangeException();

                return (i == 0) ? Bottom :
                    (i == 1) ? Left :
                    Right;
            }
        }

        public override string ToString() => $"B:{Bottom:0.##}, L:{Left:0.##}, R:{Right:0.##}";
    }
}
