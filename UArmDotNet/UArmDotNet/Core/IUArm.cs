using System;
using System.Reactive;

namespace Baku.UArmDotNet
{
    public interface IUArm
    {
        //Motion
        /// <summary>
        /// Async move by given cartesian coordinate point
        /// </summary>
        /// <param name="position">Destination position</param>
        /// <param name="speed">motion speed in [mm/min]</param>
        /// <returns></returns>
        IObservable<Unit> MoveAsync(Position position, float speed);

        IObservable<Unit> MoveRelativeAsync(float dx, float dy, float dz, float speed);
        IObservable<Unit> MoveAsync(Polar polar, float speed);
        IObservable<Unit> SetServoAsync(Servos servo, float angle);
        IObservable<Unit> StopMotionAsync();

        //Setting 
        IObservable<bool> CheckIsMovingAsync();
        IObservable<Unit> ActivateServoAsync(Servos servo);
        IObservable<Unit> DeactivateServoAsync(Servos servo);
        IObservable<Unit> BeepAsync(int frequency, float durationSec);

        IObservable<Position> GetFKAsync(ServoAngles angles);
        IObservable<ServoAngles> GetIKAsync(Position position);
        IObservable<bool> CanReachAsync(Position position);
        IObservable<bool> CanReachAsync(Polar polar);
        IObservable<Unit> SetPumpStateAsync(bool active);
        IObservable<Unit> SetGripperStateAsync(bool active);
        IObservable<Unit> SetDigitalPinOutputAsync(int number, bool high);

        //Read
        IObservable<string> GetDeviceNameAsync();
        IObservable<string> GetHardwareVersionAsync();
        IObservable<string> GetFirmwareVersionAsync();
        IObservable<string> GetAPIVersionAsync();
        IObservable<string> GetUIDAsync();

        IObservable<ServoAngles> GetServoAnglesAsync();
        IObservable<Position> GetPositionAsync();
        IObservable<Polar> GetPolarAsync();

        IObservable<PumpStates> GetPumpStatusAsync();
        IObservable<GripperStates> GetGripperStatusAsync();

        IObservable<bool> GetLimitedSwitchStateAsync();
        IObservable<bool> GetDigitalPinStateAsync(int number);
        IObservable<float> GetAnalogPinStateAsync(int number);

        IObservable<byte> ReadByteAsync(EEPROMDeviceType device, int addr);
        IObservable<int> ReadIntAsync(EEPROMDeviceType device, int addr);
        IObservable<float> ReadFloatAsync(EEPROMDeviceType device, int addr);

        IObservable<Unit> WriteByteAsync(EEPROMDeviceType device, int addr, byte data);
        IObservable<Unit> WriteIntAsync(EEPROMDeviceType device, int addr, int data);
        IObservable<Unit> WriteFloatAsync(EEPROMDeviceType device, int addr, float data);
        //NOTE: Recordingのコマンドについては時間管理まわりの方針がついてないのでいったん作らない

    }

}
