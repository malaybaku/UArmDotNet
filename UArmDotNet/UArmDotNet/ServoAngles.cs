using System;

namespace Baku.UArmDotNet
{
    public class ServoAngles
    {
        public ServoAngles(float j0, float j1, float j2, float j3)
        {
            J0 = j0;
            J1 = j1;
            J2 = j2;
            J3 = j3;
        }

        public float J0 { get; private set; }
        public float J1 { get; private set; }
        public float J2 { get; private set; }
        public float J3 { get; private set; }

        public float this[int i]
        {
            get
            {
                if (i < 0 || i > 3) throw new IndexOutOfRangeException();

                return (i == 0) ? J0 :
                    (i == 1) ? J1 :
                    (i == 2) ? J2 :
                    J3;
            }
        }

    }
}
