using System;
using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public interface IUArm
    {
        //Moving Command
        Task MoveAsync(Position position, float speed);
        Task MoveWithLaserOnAsync(Position position, float speed);
        Task MoveAsync(Polar polar, float speed);
        Task SetServoAsync(Servos servo, float angle);
        Task MoveRelativeAsync(Position displacement, float speed);
        Task MoveRelativeAsync(Polar polarDisplacement, float speed);

        //Setting Commands
        Task AttachAllMotorAsync();
        Task DetachAllMotorAsync();
        //NOTE: 戻り値相当の情報はイベントでしか飛んでこないので非同期にしない
        void SetFeedbackCycleAsync(float durationSec);
        Task<bool> CheckIsMovingAsync();
        Task AttachMotorAsync(Servos servo);
        Task DetachMotorAsync(Servos servo);
        Task<bool> CheckMotorAttachedAsync(Servos servo);
        Task BeepAsync(int frequency, float durationSec);

        Task<byte> ReadByteAsync(EEPROMDeviceType device, int addr);
        Task<int> ReadIntAsync(EEPROMDeviceType device, int addr);
        Task<float> ReadFloatAsync(EEPROMDeviceType device, int addr);

        Task WriteByteAsync(EEPROMDeviceType device, int addr, byte data);
        Task WriteIntAsync(EEPROMDeviceType device, int addr, int data);
        Task WriteFloatAsync(EEPROMDeviceType device, int addr, float data);

        Task SetEnableDefaultFunctionOfBaseButtonsAsync(bool enable);
        Task<ServoAngles> GetIKAsync(Position position);
        Task<Position> GetFKAsync(ServoAngles angles);
        Task<bool> CanReachAsync(Position position);
        Task<bool> CanReachAsync(Polar polar);
        Task SetPumpStateAsync(bool active);
        Task SetGripperStateAsync(bool active);
        Task SetBluetoothState(bool active);
        Task SetDigitalPinOutputAsync(int number, bool high);

        Task SetBluetoothNameAsync(string name);

        Task SetArmModeAsync(ArmModes mode);


        //Query Command
        Task<ServoAngles> GetServoAnglesAsync();
        Task<string> GetDeviceNameAsync();
        Task<string> GetHardwareVersionAsync();
        Task<string> GetSoftwareVersionAsync();
        Task<string> GetAPIVersionAsync();
        Task<string> GetUIDAsync();

        Task<Position> GetPositionAsync();
        Task<Polar> GetPolarAsync();

        Task<PumpStates> GetPumpStatusAsync();
        Task<GripperStates> GetGripperStatusAsync();

        Task<bool> CheckLimitedSwitchTriggeredAsync();
        Task<bool> GetDigitalPinStateAsync(int number);
        Task<float> GetAnalogPinStateAsync(int number);

        Task<ServoAngles> GetDefaultValueOfAS5600Async();


        event EventHandler<PositionFeedbackEventArgs> ReceivedPositionFeedback;
        event EventHandler<ButtonActionEventArgs> ReceivedButtonAction;
        event EventHandler<PowerConnectionChangedEventArgs> PowerConnectionChanged;
        event EventHandler<LimitedSwitchEventArgs> LimitedSwitchStateChanged;
        //NOTE: Recordingのコマンドについては時間管理まわりの方針がついてないのでいったん作らない

    }

    public class PositionFeedbackEventArgs : EventArgs
    {
        public PositionFeedbackEventArgs(Position position)
        {
            Position = position;
        }
        public Position Position { get; }
    }

    public class ButtonActionEventArgs : EventArgs
    {
        public ButtonActionEventArgs(ButtonTypes buttonType, ButtonActionTypes buttonAction)
        {
            ButtonType = buttonType;
            ButtonAction = buttonAction;
        }

        public ButtonTypes ButtonType { get; }
        public ButtonActionTypes ButtonAction { get; }

    }

    public class PowerConnectionChangedEventArgs : EventArgs
    {
        public PowerConnectionChangedEventArgs(bool connected)
        {
            Connected = connected;
        }
        public bool Connected { get; }
    }

    public class LimitedSwitchEventArgs : EventArgs
    {
        public LimitedSwitchEventArgs(int switchNumber, bool isTriggered)
        {
            SwitchNumber = switchNumber;
            IsTriggerd = isTriggered;
        }

        public int SwitchNumber { get; }
        public bool IsTriggerd { get; }
    }


}
