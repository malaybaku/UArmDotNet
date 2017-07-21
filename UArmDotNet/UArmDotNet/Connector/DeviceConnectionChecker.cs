using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Baku.UArmDotNet
{
    /// <summary> UArmの接続状態を確認するメソッドを提供します。 </summary>
    public static class DeviceConnectionChecker
    {
        /// <summary>UArmのハードウェアIDキーワードです。</summary>
        public static readonly string UArmHwidKeyword = "USB VID:PID=0403:6001";

        /// <summary>シリアルポートとして接続可能なUArmの一覧を取得します。</summary>
        /// <returns>利用可能な接続済みのUArm一覧</returns>
        public static IEnumerable<SerialPort> GetUArmPorts()
        {
            throw new NotImplementedException();
        }
    }
}
