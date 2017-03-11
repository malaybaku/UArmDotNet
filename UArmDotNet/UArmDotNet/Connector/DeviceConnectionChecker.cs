using System;
using System.IO.Ports;

namespace Baku.UArmDotNet
{
    public static class DeviceConnectionChecker
    {
        public static readonly string UArmHwidKeyword = "USB VID:PID=0403:6001";

        public static SerialPort[] GetUArmPorts()
        {
            throw new NotImplementedException();
        }
    }
}
