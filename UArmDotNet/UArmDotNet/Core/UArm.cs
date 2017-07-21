using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public class UArm : IUArm
    {
        public UArm(UArmConnector connector)
        {
            Connector = connector;
        }
        public UArmConnector Connector { get; }

        //Motion
        public Task MoveAsync(Position position, float speed)
            => SetCommandTransact(
            string.Format(Protocol.MovePositionFormat, position.X, position.Y, position.Z, speed)
            );
        public Task MoveRelativeAsync(float dx, float dy, float dz, float speed)
            => SetCommandTransact(
            string.Format(Protocol.MovePositionRelativeFormat, dx, dy, dz, speed)
            );
        public Task MoveAsync(Polar polar, float speed)
            => SetCommandTransact(
            string.Format(Protocol.MovePolarFormat, polar.Stretch, polar.Rotation, polar.Height, speed)
            );
        public Task SetServoAsync(Servos servo, float angle)
            => SetCommandTransact(string.Format(Protocol.MoveServoFormat, (int)servo, angle));
        public Task StopMotionAsync()
            => SetCommandTransact(Protocol.MoveStop);

        //Setting 
        public Task<bool> CheckIsMovingAsync()
            => BoolTransact(Protocol.CheckIsMoving);

        public Task ActivateServoAsync(Servos servo)
            => SetCommandTransact(string.Format(Protocol.ActivateServoFormat, (int)servo));

        public Task DeactivateServoAsync(Servos servo)
            => SetCommandTransact(string.Format(Protocol.DeactivateServoFormat, (int)servo));

        public Task BeepAsync(int frequency, float durationSec)
            => SetCommandTransact(string.Format(Protocol.BeepFormat, frequency, durationSec));

        public async Task<Position> GetFKAsync(ServoAngles angles)
        {
            var res = await Transact(string.Format(Protocol.GetFKFormat, angles.J0, angles.J1, angles.J2));
            return res.ToPosition();
        }
        public async Task<ServoAngles> GetIKAsync(Position position)
        {
            var res = await Transact(string.Format(Protocol.GetIKFormat, position.X, position.Y, position.Z));
            return res.ToServoAngles();
        }

        public Task<bool> CanReachAsync(Position position)
            => BoolTransact(string.Format(Protocol.CanReachCartesianFormat, position.X, position.Y, position.Z));
        public Task<bool> CanReachAsync(Polar polar)
            => BoolTransact(string.Format(Protocol.CanReachPolarFormat, polar.Stretch, polar.Rotation, polar.Height));

        public async Task SetPumpStateAsync(bool active)
        {
            await SetCommandTransact(
                string.Format(Protocol.SetPumpStateFormat, (active ? 1 : 0))
                );
        }
        public async Task SetGripperStateAsync(bool active)
        {
            await SetCommandTransact(
                string.Format(Protocol.SetGripperStateFormat, (active ? 1 : 0))
                );
        }
        public async Task SetDigitalPinOutputAsync(int number, bool high)
        {
            await SetCommandTransact(
                string.Format(Protocol.SetDigitalOutputStateFormat, number, (high ? 1 : 0))
                );
        }
        //Read
        public Task<string> GetDeviceNameAsync()
            => GetInfoStringTransact(Protocol.GetDeviceName);
        public Task<string> GetHardwareVersionAsync()
            => GetInfoStringTransact(Protocol.GetHardwareVersion);
        public Task<string> GetFirmwareVersionAsync()
            => GetInfoStringTransact(Protocol.GetFirmwareVersion);
        public Task<string> GetAPIVersionAsync()
            => GetInfoStringTransact(Protocol.GetAPIVersion);
        public Task<string> GetUIDAsync()
            => GetInfoStringTransact(Protocol.GetUID);

        public async Task<ServoAngles> GetServoAnglesAsync()
        {
            var res = await Transact(Protocol.GetServoAngle);
            return res.ToServoAngles();
        }
        public async Task<Position> GetPositionAsync()
        {
            var res = await Transact(Protocol.GetPosition);
            return res.ToPosition();
        }
        public async Task<Polar> GetPolarAsync()
        {
            var res = await Transact(Protocol.GetPolar);
            return res.ToPolar();
        }

        public async Task<PumpStates> GetPumpStatusAsync()
        {
            var res = await Transact(Protocol.GetPumpState);
            return res.ToPumpState();
        }
        public async Task<GripperStates> GetGripperStatusAsync()
        {
            var res = await Transact(Protocol.GetGripperState);
            return res.ToGripperState();
        }

        public Task<bool> GetLimitedSwitchStateAsync()
            => BoolTransact(Protocol.GetLimitedSwitchState);
        public Task<bool> GetDigitalPinStateAsync(int number)
            => BoolTransact(string.Format(Protocol.GetDigitalPinValueFormat, number));

        public async Task<float> GetAnalogPinStateAsync(int number)
        {
            var res = await Transact(string.Format(Protocol.GetAnalogPinValueFormat, number));
            return res.ToFloat();
        }

        public async Task<byte> ReadByteAsync(EEPROMDeviceType device, int addr)
        {
            var res = await Transact(
                string.Format(Protocol.ReadROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Byte)
                );
            return res.ToByte();
        }
        public async Task<int> ReadIntAsync(EEPROMDeviceType device, int addr)
        {
            var res = await Transact(
                string.Format(Protocol.ReadROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Integer)
                );
            return res.ToInt();
        }
        public async Task<float> ReadFloatAsync(EEPROMDeviceType device, int addr)
        {
            var res = await Transact(
                string.Format(Protocol.ReadROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Float)
                );
            return res.ToFloat();
        }

        public Task WriteByteAsync(EEPROMDeviceType device, int addr, byte data)
            => SetCommandTransact(
            string.Format(Protocol.WriteROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Byte, (int)data)
            );
        public Task WriteIntAsync(EEPROMDeviceType device, int addr, int data)
            => SetCommandTransact(
            string.Format(Protocol.WriteROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Integer, data)
            );
        public Task WriteFloatAsync(EEPROMDeviceType device, int addr, float data)
            => SetCommandTransact(
            string.Format(Protocol.WriteROMFormat, (int)device, addr, (int)EEPROMDataTypeCodes.Float, data)
            );

        //NOTE: Recordingのコマンドについては時間管理まわりの方針がついてないのでいったん作らない

        //private void Post(string command)
        //{
        //    throw new NotImplementedException();
        //}
        private Task<UArmResponse> Transact(string command)
            => Connector.Transact(command);

        private async Task SetCommandTransact(string command)
        {
            var response = await Transact(command);

            if (response.IsOK)
            {
                return;
            }

            throw UArmExceptionFactory.CreateExceptionFromResponse(response);
        }
        private async Task<string> GetInfoStringTransact(string command)
        {
            var response = await Transact(command);
            if (response.IsOK)
            {
                return response.ToInfoString();
            }
            throw UArmExceptionFactory.CreateExceptionFromResponse(response);
        }
        private async Task<bool> BoolTransact(string command)
        {
            var response = await Transact(command);
            if (response.IsOK)
            {
                return response.ToBool();
            }
            throw UArmExceptionFactory.CreateExceptionFromResponse(response);
        }


    }
}
