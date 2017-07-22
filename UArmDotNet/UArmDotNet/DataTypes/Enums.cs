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

    public enum ArmModes
    {
        Normal = 0,
        Laser = 1,
        Printing = 2,
        UniversalHolder = 3
    }

    public enum ButtonTypes
    {
        MenuButton = 0,
        PlayButton = 1,
    }

    public enum ButtonActionTypes
    {
        Click = 1,
        LongPress = 2,
    }


}
