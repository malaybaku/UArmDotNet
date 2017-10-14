using System;
using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public interface IUArm
    {
        #region Motion

        /// <summary>
        /// Move the robot to given position.
        /// </summary>
        /// <param name="position">destination</param>
        /// <param name="speed">speed (mm/min)</param>
        /// <returns></returns>
        Task MoveAsync(Position position, float speed);
        /// <summary>
        /// After change to laser mode by <see cref="SetArmModeAsync(ArmModes)"/>, move the robot with laser on mode.
        /// </summary>
        /// <param name="position">destination</param>
        /// <param name="speed">speed (mm/min)</param>
        /// <returns></returns>
        Task MoveWithLaserOnAsync(Position position, float speed);
        /// <summary>
        /// Move the robot to given polar position.
        /// </summary>
        /// <param name="polar">destination</param>
        /// <param name="speed">speed (mm/min)</param>
        /// <returns></returns>
        Task MoveAsync(Polar polar, float speed);
        /// <summary>
        /// Move a motor to the given angle
        /// </summary>
        /// <param name="servo">servo to move</param>
        /// <param name="angle">target angle (deg)</param>
        /// <returns></returns>
        Task MoveServoAngleAsync(Servos servo, float angle);
        /// <summary>
        /// Move the robot relative current position, by given position.
        /// </summary>
        /// <param name="position">destination</param>
        /// <param name="speed">speed (mm/min)</param>
        /// <returns></returns>
        Task MoveRelativeAsync(Position displacement, float speed);
        /// <summary>
        /// Move the robot relative to current position, by given polar position.
        /// </summary>
        /// <param name="polar">destination</param>
        /// <param name="speed">speed (mm/min)</param>
        /// <returns></returns>
        Task MoveRelativeAsync(Polar polarDisplacement, float speed);
        /// <summary>
        /// Request the time delay (wait) to the robot.
        /// </summary>
        /// <param name="timeMicrosec">Time to wait, in micro second</param>
        /// <returns></returns>
        Task DelayAsync(int timeMicrosec);
        /// <summary>
        /// Stop the current motion of the robot.
        /// </summary>
        /// <returns></returns>
        Task StopMoveAsync();

        #endregion

        #region Setting

        /// <summary>
        /// Activate all motors
        /// </summary>
        /// <returns></returns>
        Task AttachAllMotorAsync();
        /// <summary>
        /// Deactivate all motors
        /// </summary>
        /// <returns></returns>
        Task DetachAllMotorAsync();
        /// <summary>
        /// Start position info streaming by <see cref="ReceivedPositionFeedback"/> event.
        /// </summary>
        /// <param name="intervalSec"></param>
        /// <returns></returns>
        Task StartFeedbackCycleAsync(float intervalSec);
        /// <summary>
        /// Stop position info streaming.
        /// </summary>
        /// <returns></returns>
        Task StopFeedbackCycleAsync();
        /// <summary>
        /// Check if the robot is moving now
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckIsMovingAsync();
        /// <summary>
        /// Activate the specific motor
        /// </summary>
        /// <param name="servo"></param>
        /// <returns></returns>
        Task AttachMotorAsync(Servos servo);
        /// <summary>
        /// Deactivate the specific motor
        /// </summary>
        /// <param name="servo"></param>
        /// <returns></returns>
        Task DetachMotorAsync(Servos servo);
        /// <summary>
        /// Check if the specific motor is activated.
        /// </summary>
        /// <param name="servo"></param>
        /// <returns></returns>
        Task<bool> CheckMotorAttachedAsync(Servos servo);
        /// <summary>
        /// Output beep sound
        /// </summary>
        /// <param name="frequency">frequency of the sound (Hz)</param>
        /// <param name="durationMillisec">duration (ms)</param>
        /// <returns></returns>
        Task BeepAsync(int frequency, int durationMillisec);

        /// <summary>
        /// Read byte data from EEPROM.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        Task<byte> ReadByteAsync(EEPROMDeviceType device, int addr);
        /// <summary>
        /// Read integer data from EEPROM.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        Task<int> ReadIntAsync(EEPROMDeviceType device, int addr);
        /// <summary>
        /// Read float data from EEPROM.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        Task<float> ReadFloatAsync(EEPROMDeviceType device, int addr);

        /// <summary>
        /// Write byte data to EEPROM.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task WriteByteAsync(EEPROMDeviceType device, int addr, byte data);
        /// <summary>
        /// Write integer data to EEPROM.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task WriteIntAsync(EEPROMDeviceType device, int addr, int data);
        /// <summary>
        /// Write float data to EEPROM.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task WriteFloatAsync(EEPROMDeviceType device, int addr, float data);

        /// <summary>
        /// Default function of base buttons.
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        Task SetEnableDefaultFunctionOfBaseButtonsAsync(bool enable);
        /// <summary>
        /// Convert position to angle of joints.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Task<ServoAngles> GetIKAsync(Position position);
        /// <summary>
        /// Convert angle to position.
        /// </summary>
        /// <param name="angles"></param>
        /// <returns></returns>
        Task<Position> GetFKAsync(ServoAngles angles);
        /// <summary>
        /// Check if the position is reachable.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Task<bool> CanReachAsync(Position position);
        /// <summary>
        /// Check if the position is reachable.
        /// </summary>
        /// <param name="polar"></param>
        /// <returns></returns>
        Task<bool> CanReachAsync(Polar polar);
        /// <summary>
        /// Set pump state.
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        Task SetPumpStateAsync(bool active);
        /// <summary>
        /// Set gripper state.
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        Task SetGripperStateAsync(bool active);
        /// <summary>
        /// Set bluetooth state.
        /// NOTE: when you use this function, the connection will be closed suddenly
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        Task SetBluetoothStateAsync(bool active);
        /// <summary>
        /// Set digital output pin state
        /// </summary>
        /// <param name="number"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        Task SetDigitalPinOutputAsync(int number, bool high);

        /// <summary>
        /// Set bluetooth name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task SetBluetoothNameAsync(string name);

        /// <summary>
        /// Set arm mode.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task SetArmModeAsync(ArmModes mode);
        /// <summary>
        /// Set current position into the reference position.
        /// </summary>
        /// <returns></returns>
        Task UpdateReferencePoint();
        /// <summary>
        /// Set height offset zero point.
        /// </summary>
        /// <returns></returns>
        Task UpdateHeightZeroPoint();
        /// <summary>
        /// Set the offset of the end effector
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        Task SetEndEffectorHeight(float height);

        #endregion

        #region Query

        /// <summary>
        /// Get current angle joints except hand servo.
        /// </summary>
        /// <returns></returns>
        Task<ServoAngles> GetAllServoAnglesAsync();
        /// <summary>
        /// Get the device name.
        /// </summary>
        /// <returns></returns>
        Task<string> GetDeviceNameAsync();
        /// <summary>
        /// Get the hardware version.
        /// </summary>
        /// <returns></returns>
        Task<string> GetHardwareVersionAsync();
        /// <summary>
        /// Get the software version.
        /// </summary>
        /// <returns></returns>
        Task<string> GetSoftwareVersionAsync();
        /// <summary>
        /// Get the API version.
        /// </summary>
        /// <returns></returns>
        Task<string> GetAPIVersionAsync();
        /// <summary>
        /// Get the UID.
        /// </summary>
        /// <returns></returns>
        Task<string> GetUIDAsync();
        /// <summary>
        /// Get the angle of a joint.
        /// </summary>
        /// <param name="servo"></param>
        /// <returns></returns>
        Task<float> GetServoAngleAsync(Servos servo);

        /// <summary>
        /// Get the current position.
        /// </summary>
        /// <returns></returns>
        Task<Position> GetPositionAsync();
        /// <summary>
        /// Get the current polar position.
        /// </summary>
        /// <returns></returns>
        Task<Polar> GetPolarAsync();

        /// <summary>
        /// Get the suction cup state.
        /// </summary>
        /// <returns></returns>
        Task<PumpStates> GetPumpStatusAsync();
        /// <summary>
        /// Get the gripper state.
        /// </summary>
        /// <returns></returns>
        Task<GripperStates> GetGripperStatusAsync();

        /// <summary>
        /// Get the limited switch state.
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckLimitedSwitchTriggeredAsync();
        /// <summary>
        /// Get the digital input pin state.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<bool> GetDigitalPinStateAsync(int number);
        /// <summary>
        /// Get the analog input pin state.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<float> GetAnalogPinStateAsync(int number);
        /// <summary>
        /// Get the default value of AS5600 in each joint.
        /// </summary>
        /// <returns></returns>
        Task<ServoAngles> GetDefaultValueOfAS5600Async();
        /// <summary>
        /// Get which of the mode the robot is.
        /// </summary>
        /// <returns></returns>
        Task<ArmModes> GetArmModeAsync();

        #endregion

        #region Grove

        Task GroveInitializeAsync(GroveModuleTypes moduleType);
        Task GroveStartSensorDataUpdateAsync(GroveModuleTypes moduleType, int intervalMicrosec);
        Task GroveStopSensorDataUpdateAsync(GroveModuleTypes moduleType);
        Task GroveSetFanDutyAsync(byte dutyCycle);
        Task GroveEnableElectroMagnetAsync(bool isEnabled);
        Task GroveSetLcdPowerStateAsync(GroveLcdPowerStates state);
        Task GroveSetLcdBackgroundColorAsync(byte r, byte g, byte b);
        Task GroveSetLcdTextAsync(int line, string text);
        Task GroveChangeToLcd2Async();

        #endregion

        /// <summary>Happens when robot is ready</summary>
        event EventHandler ReceivedReady;
        /// <summary>Happens when received position data.</summary>
        event EventHandler<PositionFeedbackEventArgs> ReceivedPositionFeedback;
        /// <summary>Happens when received button action.</summary>
        event EventHandler<ButtonActionEventArgs> ReceivedButtonAction;
        /// <summary>Happens when power connection state changed.</summary>
        event EventHandler<PowerConnectionChangedEventArgs> PowerConnectionChanged;
        /// <summary>Happens when limited switch state changed.</summary>
        event EventHandler<LimitedSwitchEventArgs> LimitedSwitchStateChanged;

        /// <summary>Happens when received color sensor data. </summary>
        event EventHandler<GroveColorSensorDataEventArgs> ReceivedGroveColorSensorData;
        /// <summary>Happens when received gesture sensor data. </summary>
        event EventHandler<GroveGestureSensorDataEventArgs> ReceivedGroveGestureSensorData;
        /// <summary>Happens when received ultrasonic distance sensor data. </summary>
        event EventHandler<GroveUltrasonicDataEventArgs> ReceivedGroveUltrasonicSensorData;
        /// <summary>Happens when received temperature and humidity sensor data. </summary>
        event EventHandler<GroveTempAndHumiditySensorDataEventArgs> ReceivedGroveTemperatureAndHumiditySensorData;
        /// <summary>Happens when received PIR motion sensor data. </summary>
        event EventHandler<GrovePirMotionSensorDataEventArgs> ReceivedGrovePirMotionSensorData;


    }
}
