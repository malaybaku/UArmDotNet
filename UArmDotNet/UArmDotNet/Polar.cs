namespace Baku.UArmDotNet
{
    public class Polar
    {
        public Polar(float stretch, float rotation, float height)
        {
            Stretch = stretch;
            Rotation = rotation;
            Height = height;
        }

        public float Stretch { get; private set; }
        public float Rotation { get; private set; }
        public float Height { get; private set; }

    }
}
