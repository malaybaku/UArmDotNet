using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace Baku.UArmDotNet
{
    /// <summary> UArmの接続状態を確認するメソッドを提供します。 </summary>
    public static class DeviceConnectionChecker
    {
        /// <summary>Get available serial port names with "COMx"</summary>
        /// <returns>利用可能な接続済みのUArm一覧</returns>
        public static IEnumerable<string> GetUArmPortNames()
        {
            var pnpEntity = new ManagementClass("Win32_PnPEntity");
            var comRegex = new Regex(@"\(COM[1-9][0-9]?[0-9]?\)"); // example: "(COM3)"

            return pnpEntity
                .GetInstances()
                .Cast<ManagementObject>()
                .Select(managementObj => managementObj.GetPropertyValue("Name"))
                .Select(nameObj => nameObj?.ToString()) //"FooDevice (COM42)"
                //here we expect the device name contains "Arduino", because uArm is recognized as Arduino board.
                .Where(name => name != null && comRegex.IsMatch(name) && name.Contains("Arduino"))
                .Select(name => comRegex.Match(name).Value) //"(COM42)"
                .Select(name => name.Substring(1, name.Length - 2)); //"COM42"

        }
    }
}
