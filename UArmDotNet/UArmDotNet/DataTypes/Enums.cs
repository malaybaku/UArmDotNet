using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public enum Servos
    {
        Bottom = 0,
        Left = 1,
        Right = 2,
        Hand = 3
    }

    public enum EEPROMDeviceType
    {
        Default = 0,
        User = 1,
        System = 2
    }

    public enum GripperStates
    {
        Stop = 0,
        Working = 1,
        Grabbing = 2
    }

    public enum PumpStates
    {
        Stop = 0,
        Working = 1,
        Grabbing = 2
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
