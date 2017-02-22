﻿using System;

namespace Baku.UArmDotNet
{

    public class UArmResponse
    {
        public UArmResponse(int id, string[] args)
        {
            Id = id;
            Args = args;
        }

        public int Id { get; }
        public string[] Args { get; }

        public bool IsOK()
        {
            return (Args.Length > 0) && Args[1].StartsWith(Protocol.OK);
        }

        /// <summary>Convert to version info string, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted bool value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public string ToVersion()
        {
            if (Args.Length < 2)
            {
                throw new FormatException();
            }
            return Args[1].Replace("V", "");
        }

        /// <summary>Convert to boolean, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted bool value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public bool ToBool()
        {
            if (Args.Length < 2 ||
                (Args[1] != "V0" && Args[1] != "V1")
                )
            {
                throw new FormatException();
            }
            return (Args[1] == "V1");
        }

        /// <summary>Convert to byte, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public byte ToByte()
        {
            if (Args.Length < 2)
            {
                throw new FormatException();
            }
            return byte.Parse(Args[1].Substring(1));
        }

        /// <summary>Convert to integer, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public int ToInt()
        {
            if (Args.Length < 2)
            {
                throw new FormatException();
            }
            return (int)float.Parse(Args[1].Substring(1));
        }

        /// <summary>Convert to float, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public float ToFloat()
        {
            if (Args.Length < 2)
            {
                throw new FormatException();
            }
            return float.Parse(Args[1].Substring(1));
        }

        /// <summary>Convert to XYZ position, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public Position ToPosition()
        {
            if (Args.Length < 4)
            {
                throw new FormatException();
            }
            return new Position(
                float.Parse(Args[1].Substring(1)),
                float.Parse(Args[2].Substring(1)),
                float.Parse(Args[3].Substring(1))
                );
        }

        /// <summary>Convert to Polar position expr, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public Polar ToPolar()
        {
            if (Args.Length < 4)
            {
                throw new FormatException();
            }
            return new Polar(
                float.Parse(Args[1].Substring(1)),
                float.Parse(Args[2].Substring(1)),
                float.Parse(Args[3].Substring(1))
                );
        }

        /// <summary>Convert to ServoAngles, if subject to UArm serial data's appropriate format</summary>
        /// <returns>Converted value, or <see cref="FormatException"/> will be thrown.</returns>
        /// <exception cref="FormatException"/>
        public ServoAngles ToServoAngles()
        {
            if (Args.Length < 5)
            {
                throw new FormatException();
            }

            return new ServoAngles(
                float.Parse(Args[1].Substring(1)),
                float.Parse(Args[2].Substring(1)),
                float.Parse(Args[3].Substring(1)),
                float.Parse(Args[4].Substring(1))
                );
        }


    }

}
