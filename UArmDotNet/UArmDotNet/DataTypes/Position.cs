namespace Baku.UArmDotNet
{
    public class Position
    {
        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public override string ToString() => $"X:{X:0.##}, Y:{Y:0.##}, Z:{Z:0.##}";
    }
}
