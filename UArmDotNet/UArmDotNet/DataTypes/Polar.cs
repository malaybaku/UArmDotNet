﻿namespace Baku.UArmDotNet
{
    public class Polar
    {
        public Polar(float stretch, float rotation, float height)
        {
            Stretch = stretch;
            Rotation = rotation;
            Height = height;
        }

        public float Stretch { get; }
        public float Rotation { get; }
        public float Height { get; }

        public override string ToString() => $"S:{Stretch:0.##}, R:{Rotation:0.##}, H:{Height:0.##}";

    }
}
