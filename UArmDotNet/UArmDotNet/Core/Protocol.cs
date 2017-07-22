namespace Baku.UArmDotNet
{
    public static class Protocol
    {
        //public static readonly int CalibrationConfirmFlag = 0x80;

        //Ready & Response

        //public static readonly string READY = "@1";
        public static readonly string OK = "ok";

        //Motion Command
        public static readonly string MovePositionFormat = "G0 X{0:.##} Y{1.##} Z{2.##} F{3.##}";
        public static readonly string MovePositionLaserOnFormat = "G0 X{0:.##} Y{1.##} Z{2.##} F{3.##}";
        public static readonly string MovePolarFormat = "G2201 S{0} R{1} H{2} F{3}";
        public static readonly string MoveServoFormat = "G2202 N{0} V{1}";
        public static readonly string MovePositionRelativeFormat = "G2204 X{0:.##} Y{1:.##} Z{2:.##} F{3:.##}";
        public static readonly string MovePolarRelativeFormat = "G2205 S{0:.##} R{2:.##} H{2:.##} F{3:.##}";


        //Setting Command
        public static readonly string AttachAllMotor = "M17";
        public static readonly string DetachAllMotor = "M2019";
        public static readonly string SetFeedbackFormat = "M2120 V{0:.##}";
        public static readonly string CheckIsMoving = "M2200";
        public static readonly string AttachMotorFormat = "M2201 N{0}";
        public static readonly string DetachMotorFormat = "M2202 N{0}";
        public static readonly string CheckMotorAttachedFormat = "M2203 N{0}";
        public static readonly string BeepFormat = "M2210 F{0} T{1}";

        public static readonly string ReadROMFormat = "M2211 N{0} A{1} T{2}";
        public static readonly string WriteROMFormat = "M2212 N{0} A{1} T{2} V{3}";

        public static readonly string EnableFunctionOfBaseButtonsFormat = "M2213 V{0}";
        public static readonly string GetIKFormat = "M2220 X{0:.##} Y{1.##} Z{2.##}";
        public static readonly string GetFKFormat = "M2221 B{0:.##} L{1.##} R{2.##}";
        public static readonly string CanReachCartesianFormat = "M2222 X{0} Y{1} Z{2} P0";
        public static readonly string CanReachPolarFormat = "M2222 X{0} Y{1} Z{2} P1";
        public static readonly string SetPumpStateFormat = "M2231 V{0}";
        public static readonly string SetGripperStateFormat = "M2232 V{0}";
        public static readonly string EnableBluetoothFormat = "M2234 V{0}";
        public static readonly string SetDigitalOutputStateFormat = "M2240 N{0} V{1}";

        //NOTE: SetBluetooth command must be sent without "#N", message ID.
        public static readonly string SetBluetoothNameFormat = "M2245 V{0}";
        public static readonly int BlueToothNameCharLimit = 11;

        public static readonly string SetModeOfTheArmFormat = "M2400 V{0}";

        public static readonly string SetCurrentPosToReference = "M2401";
        public static readonly string SetHeightZeroPoint = "M2410";
        public static readonly string SetTheHeightOfEndEffectorFormat = "M2411 S{0:.##}";

        // Get Command
        public static readonly string GetServoAngle = "P2200";

        public static readonly string GetDeviceName = "P2201";
        public static readonly string GetHardwareVersion = "P2202";
        public static readonly string GetFirmwareVersion = "P2203";
        public static readonly string GetAPIVersion = "P2204";
        public static readonly string GetUID = "P2205";
        public static readonly string GetAngleOfJointFormat = "P2206 N{0}";

        public static readonly string GetPosition = "P2220";
        public static readonly string GetPolar = "P2221";

        public static readonly string GetPumpState = "P2231";
        public static readonly string GetGripperState = "P2232";
        public static readonly string GetLimitedSwitchState = "P2233";
        public static readonly string GetPowerConectionState = "P2234";

        public static readonly string GetDigitalPinValueFormat = "P2240 N{0}";
        public static readonly string GetAnalogPinValueFormat = "P2241 N{0}";
        public static readonly string GetDefaultValueOfAS5600 = "P2242";

        // Event Resonse
        public static readonly string StartPositionReportFormat = "M120 V{0}";
        public static readonly string ReportPositionResponse = "@3";
    }
}
