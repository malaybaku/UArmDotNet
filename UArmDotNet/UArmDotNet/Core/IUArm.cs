using System.Threading.Tasks;

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
        Task MoveAsync(Position position, float speed);

        Task MoveRelativeAsync(float dx, float dy, float dz, float speed);
        Task MoveAsync(Polar polar, float speed);
        Task SetServoAsync(Servos servo, float angle);
        Task StopMotionAsync();

        //Setting 
        Task<bool> CheckIsMovingAsync();
        Task ActivateServoAsync(Servos servo);
        Task DeactivateServoAsync(Servos servo);
        Task BeepAsync(int frequency, float durationSec);

        Task<Position> GetFKAsync(ServoAngles angles);
        Task<ServoAngles> GetIKAsync(Position position);
        Task<bool> CanReachAsync(Position position);
        Task<bool> CanReachAsync(Polar polar);
        Task SetPumpStateAsync(bool active);
        Task SetGripperStateAsync(bool active);
        Task SetDigitalPinOutputAsync(int number, bool high);

        //Read
        Task<string> GetDeviceNameAsync();
        Task<string> GetHardwareVersionAsync();
        Task<string> GetFirmwareVersionAsync();
        Task<string> GetAPIVersionAsync();
        Task<string> GetUIDAsync();

        Task<ServoAngles> GetServoAnglesAsync();
        Task<Position> GetPositionAsync();
        Task<Polar> GetPolarAsync();

        Task<PumpStates> GetPumpStatusAsync();
        Task<GripperStates> GetGripperStatusAsync();

        Task<bool> GetLimitedSwitchStateAsync();
        Task<bool> GetDigitalPinStateAsync(int number);
        Task<float> GetAnalogPinStateAsync(int number);

        Task<byte> ReadByteAsync(EEPROMDeviceType device, int addr);
        Task<int> ReadIntAsync(EEPROMDeviceType device, int addr);
        Task<float> ReadFloatAsync(EEPROMDeviceType device, int addr);

        Task WriteByteAsync(EEPROMDeviceType device, int addr, byte data);
        Task WriteIntAsync(EEPROMDeviceType device, int addr, int data);
        Task WriteFloatAsync(EEPROMDeviceType device, int addr, float data);
        //NOTE: Recordingのコマンドについては時間管理まわりの方針がついてないのでいったん作らない

    }

}
