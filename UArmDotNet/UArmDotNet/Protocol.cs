//This protocol constants originate from: 
//  https://github.com/uArm-Developer/pyuarm/blob/dev/pyuarm/protocol.py

namespace Baku.UArmDotNet
{
    public static class Protocol
    {
        public static readonly int CalibrationConfirmFlag = 0x80;

        //Ready & Response

        public static readonly string READY = "@1";
        public static readonly string OK = "OK";

        // Set Command

        public static readonly string SET_POSITION            = "G0 X{} Y{} Z{} F{}";
        public static readonly string SET_POSITION_RELATIVE   = "G204 X{} Y{} Z{} F{}";
        public static readonly string SET_SERVO_ANGLE         = "G202 N{} V{}";
        public static readonly string STOP_MOVING             = "G203";
        public static readonly string SET_PUMP                = "M231 V{}";
        public static readonly string GET_PUMP                = "P231";
        public static readonly string SET_GRIPPER             = "M232 V{}";
        public static readonly string SET_BUZZER              = "M210 F{} T{}";
        public static readonly string SET_POLAR               = "G201 S{} R{} H{} F{}";
        public static readonly string ATTACH_SERVO            = "M201 N{}";
        public static readonly string DETACH_SERVO            = "M202 N{}";
        
        // Get Command        public static readonly string GET_SIMULATION          = "M222 X{} Y{} Z{} P0";
        public static readonly string GET_FIRMWARE_VERSION    = "P203";
        public static readonly string GET_HARDWARE_VERSION    = "P202";
        public static readonly string GET_COOR                = "P220";
        public static readonly string GET_SERVO_ANGLE         = "P200";
        public static readonly string GET_IS_MOVE             = "M200";
        public static readonly string GET_TIP_SENSOR          = "P233";
        public static readonly string GET_POLAR               = "P221";
        public static readonly string GET_GRIPPER             = "P232";
        public static readonly string GET_EEPROM              = "M211 N0 A{} T{}";
        public static readonly string SET_EEPROM              = "M212 N0 A{} T{} V{}";
        public static readonly string GET_ANALOG              = "P241 N{}";
        public static readonly string GET_DIGITAL             = "P240 N{}";

        // Report Command        public static readonly string SET_REPORT_POSITION     = "M120 V{}";
        public static readonly string REPORT_POSITION_PREFIX  = "@3";

    }

    public enum Servos
    {
        Bottom = 0,
        Left = 1,
        Right = 2,
        Hand = 3
    }

    public enum CalibrationFlags
    {
        Calibration = 10,
        CalibrationLinear = 11,
        CalibrationServo = 12,
        CalibrationStretch = 13
    }

    public enum EEPROMAddress
    {
        OffsetStrecth = 20,
        OffsetStart = 30,
        LinearSlopeStart = 50,
        LinearInterceptStart = 70,
        SerialNumber = 100,
    }

    public enum EEPROMDataTypeCodes
    {
        Byte = 1,
        Integer = 2,
        Float = 4,
    }

}
