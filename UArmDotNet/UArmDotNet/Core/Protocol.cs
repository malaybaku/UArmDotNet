using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public static class Protocol
    {
        //public static readonly int CalibrationConfirmFlag = 0x80;

        //Ready & Response

        //public static readonly string READY = "@1";
        public static readonly string OK = "OK";

        //Motion Command
        public static readonly string MovePositionFormat = "G0 X{0:.##} Y{1.##} Z{2.##} F{3.##}";
        public static readonly string MovePolarFormat = "G201 S{0} R{1} H{2} F{3}";
        public static readonly string MoveServoFormat = "G202 N{0} V{1}";
        public static readonly string MoveStop = "G203";
        public static readonly string MovePositionRelativeFormat = "G204 X{0:.##} Y{1:.##} Z{2:.##} F{3:.##}";

        //Setting Command
        public static readonly string CheckIsMoving = "M200";
        public static readonly string ActivateServoFormat = "M201 N{0}";
        public static readonly string DeactivateServoFormat = "M202 N{0}";
        public static readonly string BeepFormat = "M210 F{0} T{1}";

        public static readonly string GetIKFormat = "M220 X{0:.##} Y{1.##} Z{2.##}";
        public static readonly string GetFKFormat = "M221 B{0:.##} L{1.##} R{2.##}";
        public static readonly string CanReachCartesianFormat = "M222 X{0} Y{1} Z{2} P0";
        public static readonly string CanReachPolarFormat = "M222 X{0} Y{1} Z{2} P1";

        public static readonly string SetPumpStateFormat = "M231 V{0}";
        public static readonly string SetGripperStateFormat = "M232 V{0}";
        public static readonly string SetDigitalOutputStateFormat = "M240 N{0} V{1}";

        // EEPROM Command
        public static readonly string ReadROMFormat = "M211 N0 A{0} T{1}";
        public static readonly string WriteROMFormat = "M212 N0 A{0} T{1} V{2}";

        // Get Command
        public static readonly string GetServoAngle = "P200";

        public static readonly string GetDeviceName = "P201";
        public static readonly string GetHardwareVersion = "P202";
        public static readonly string GetFirmwareVersion = "P203";
        public static readonly string GetAPIVersion = "P204";
        public static readonly string GetUID = "P205";

        public static readonly string GetPosition = "P220";
        public static readonly string GetPolar = "P221";

        public static readonly string GetPumpState = "P231";
        public static readonly string GetGripperState = "P232";
        public static readonly string GetLimitedSwitchState = "P233";

        public static readonly string GetDigitalPinValueFormat = "P240 N{0}";
        public static readonly string GetAnalogPinValueFormat = "P241 N{0}";

        // Report Command
        public static readonly string StartPositionReportFormat = "M120 V{0}";
        public static readonly string ReportPositionResponse = "@3";
    }
}
