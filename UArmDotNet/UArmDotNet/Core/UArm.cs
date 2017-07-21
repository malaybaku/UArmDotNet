using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Baku.UArmDotNet
{
    public class UArm : IUArm
    {
        //Motion
        public IObservable<Unit> MoveAsync(Position position, float speed)
        {
            return SetCommandTransact(
                string.Format(Protocol.MovePositionFormat, position.X, position.Y, position.Z, speed)
                );
        }
        public IObservable<Unit> MoveRelativeAsync(float dx, float dy, float dz, float speed)
        {
            return SetCommandTransact(
                string.Format(Protocol.MovePositionRelativeFormat, dx, dy, dz, speed)
                );
        }
        public IObservable<Unit> MoveAsync(Polar polar, float speed)
        {
            return SetCommandTransact(
                string.Format(Protocol.MovePolarFormat, polar.Stretch, polar.Rotation, polar.Height, speed)
                );
        }
        public IObservable<Unit> SetServoAsync(Servos servo, float angle)
        {
            return SetCommandTransact(string.Format(Protocol.MoveServoFormat, (int)servo, angle));
        }
        public IObservable<Unit> StopMotionAsync()
        {
            return SetCommandTransact(Protocol.MoveStop);
        }

        //Setting 
        public IObservable<bool> CheckIsMovingAsync()
        {
            return Transact(Protocol.CheckIsMoving).Select(res => res.ToBool());
        }
        public IObservable<Unit> ActivateServoAsync(Servos servo)
        {
            return SetCommandTransact(string.Format(Protocol.ActivateServoFormat, (int)servo));
        }
        public IObservable<Unit> DeactivateServoAsync(Servos servo)
        {
            return SetCommandTransact(string.Format(Protocol.DeactivateServoFormat, (int)servo));
        }
        public IObservable<Unit> BeepAsync(int frequency, float durationSec)
        {
            return SetCommandTransact(string.Format(Protocol.BeepFormat, frequency, durationSec));
        }

        public IObservable<Position> GetFKAsync(ServoAngles angles)
        {
            return Transact(string.Format(Protocol.GetFKFormat, angles.J0, angles.J1, angles.J2))
                .Select(res => res.ToPosition());
        }
        public IObservable<ServoAngles> GetIKAsync(Position position)
        {
            return Transact(string.Format(Protocol.GetIKFormat, position.X, position.Y, position.Z))
                .Select(res => res.ToServoAngles());
        }

        public IObservable<bool> CanReachAsync(Position position)
        {
            return Transact(string.Format(Protocol.CanReachCartesianFormat, position.X, position.Y, position.Z))
                .Select(res => res.ToBool());
        }
        public IObservable<bool> CanReachAsync(Polar polar)
        {
            return Transact(string.Format(Protocol.CanReachPolarFormat, polar.Stretch, polar.Rotation, polar.Height))
                .Select(res => res.ToBool());
        }

        public IObservable<Unit> SetPumpStateAsync(bool active)
        {
            return SetCommandTransact(
                string.Format(Protocol.SetPumpStateFormat, (active ? 1 : 0))
                );
        }
        public IObservable<Unit> SetGripperStateAsync(bool active)
        {
            return SetCommandTransact(
                string.Format(Protocol.SetGripperStateFormat, (active ? 1 : 0))
                );
        }
        public IObservable<Unit> SetDigitalPinOutputAsync(int number, bool high)
        {
            return SetCommandTransact(
                string.Format(Protocol.SetDigitalOutputStateFormat, number, (high ? 1 : 0))
                );
        }
        //Read
        public IObservable<string> GetDeviceNameAsync()
        {
            return Transact(Protocol.GetDeviceName)
                .Select(res => res.ToInfoString());
        }
        public IObservable<string> GetHardwareVersionAsync()
        {
            return Transact(Protocol.GetHardwareVersion)
                .Select(res => res.ToInfoString());
        }
        public IObservable<string> GetFirmwareVersionAsync()
        {
            return Transact(Protocol.GetFirmwareVersion)
                .Select(res => res.ToInfoString());
        }
        public IObservable<string> GetAPIVersionAsync()
        {
            return Transact(Protocol.GetAPIVersion)
                .Select(res => res.ToInfoString());
        }
        public IObservable<string> GetUIDAsync()
        {
            return Transact(Protocol.GetUID)
                .Select(res => res.ToInfoString());
        }

        public IObservable<ServoAngles> GetServoAnglesAsync()
        {
            return Transact(Protocol.GetServoAngle)
                .Select(res => res.ToServoAngles());
        }
        public IObservable<Position> GetPositionAsync()
        {
            return Transact(Protocol.GetPosition)
                .Select(res => res.ToPosition());
        }
        public IObservable<Polar> GetPolarAsync()
        {
            return Transact(Protocol.GetPolar)
                .Select(res => res.ToPolar());
        }

        public IObservable<PumpStates> GetPumpStatusAsync()
        {
            return Transact(Protocol.GetPumpState)
                .Select(res => res.ToPumpState());
        }
        public IObservable<GripperStates> GetGripperStatusAsync()
        {
            return Transact(Protocol.GetGripperState)
                .Select(res => res.ToGripperState());
        }

        public IObservable<bool> GetLimitedSwitchStateAsync()
        {
            return Transact(Protocol.GetLimitedSwitchState)
                .Select(res => res.ToBool());
        }
        public IObservable<bool> GetDigitalPinStateAsync(int number)
        {
            return Transact(string.Format(Protocol.GetDigitalPinValueFormat, number))
                .Select(res => res.ToBool());
        }
        public IObservable<float> GetAnalogPinStateAsync(int number)
        {
            return Transact(string.Format(Protocol.GetAnalogPinValueFormat, number))
                .Select(res => res.ToFloat());
        }

        public IObservable<byte> ReadByteAsync(EEPROMDeviceType device, int addr)
        {
            return Transact(
                string.Format(Protocol.ReadROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Byte)
                )
                .Select(res => res.ToByte());
        }
        public IObservable<int> ReadIntAsync(EEPROMDeviceType device, int addr)
        {
            return Transact(
                string.Format(Protocol.ReadROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Integer)
                )
                .Select(res => res.ToInt());
        }
        public IObservable<float> ReadFloatAsync(EEPROMDeviceType device, int addr)
        {
            return Transact(
                string.Format(Protocol.ReadROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Float)
                )
                .Select(res => res.ToFloat());
        }

        public IObservable<Unit> WriteByteAsync(EEPROMDeviceType device, int addr, byte data)
        {
            return SetCommandTransact(
                string.Format(Protocol.WriteROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Byte, (int)data)
                );
        }
        public IObservable<Unit> WriteIntAsync(EEPROMDeviceType device, int addr, int data)
        {
            return SetCommandTransact(
                string.Format(Protocol.WriteROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Integer, data)
                );
        }
        public IObservable<Unit> WriteFloatAsync(EEPROMDeviceType device, int addr, float data)
        {
            return SetCommandTransact(
                string.Format(Protocol.WriteROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Float, data)
                );
        }

        //NOTE: Recordingのコマンドについては時間管理まわりの方針がついてないのでいったん作らない

        //private void Post(string command)
        //{
        //    throw new NotImplementedException();
        //}
        private IObservable<UArmResponse> Transact(string command)
        {
            throw new NotImplementedException();
        }

        private IObservable<Unit> SetCommandTransact(string command)
        {
            return Transact(command).Select(res =>
            {
                if (res.IsOK) return Unit.Default;
                throw UArmExceptionFactory.CreateExceptionFromResponse(res);
            });
        }

    }
}
