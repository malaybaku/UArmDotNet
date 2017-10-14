using System;

namespace Baku.UArmDotNet
{
    /// <summary>Represent servo motor types</summary>
    public enum Servos
    {
        Bottom = 0,
        Left = 1,
        Right = 2,
        Hand = 3
    }

    /// <summary>Represent EEPROM types</summary>
    public enum EEPROMDeviceType
    {
        Default = 0,
        User = 1,
        System = 2
    }

    /// <summary>Represente the gripper states, meaningful only when attached.</summary>
    public enum GripperStates
    {
        Stop = 0,
        Working = 1,
        Grabbing = 2
    }

    /// <summary>Represente the suction pump states, meaningful only when attached.</summary>
    public enum PumpStates
    {
        Stop = 0,
        Working = 1,
        Grabbing = 2
    }

    /// <summary>(not used currently) indicates calibration operation types</summary>
    public enum CalibrationFlags
    {
        Calibration = 10,
        CalibrationLinear = 11,
        CalibrationServo = 12,
        CalibrationStretch = 13
    }

    /// <summary>Represent the data types saved in the EEPROM</summary>
    public enum EEPROMDataTypeCodes
    {
        Byte = 1,
        Integer = 2,
        Float = 4,
    }

    /// <summary>Modes of the arm</summary>
    public enum ArmModes
    {
        Normal = 0,
        Laser = 1,
        Printing = 2,
        UniversalHolder = 3
    }

    /// <summary>Types of the button to which some action happened</summary>
    public enum ButtonTypes
    {
        MenuButton = 0,
        PlayButton = 1,
    }

    /// <summary>Types of the button action</summary>
    public enum ButtonActionTypes
    {
        Click = 1,
        LongPress = 2,
    }

    /// <summary>Types of the Grove (standard) module for the robot</summary>
    public enum GroveModuleTypes
    {
        ColorSensor = 10,
        GestureSensor = 11,
        Ultrasonic = 12,
        Fan = 13,
        Electromagnet = 14,
        TemperatureAndHumiditySensor = 15,
        PirMotion = 16,
        RgbLcd = 17,
    }

    /// <summary>
    /// Types of the Grove LCD power states
    /// </summary>
    public enum GroveLcdPowerStates
    {
        Off = 0,
        On = 1,
        Clear = 2,
    }

    /// <summary>
    /// TODO: Test the actual data is given by "flag" style or not
    /// </summary>
    [Flags]
    public enum GroveGestureSensorStates
    {
        Right = 0x01,
        Left = 0x02,
        Up = 0x04,
        Down = 0x08,
        Forward = 0x10,
        Backward = 0x20,
        Plus = 0x40,
        Minus = 0x80,
    }
}
