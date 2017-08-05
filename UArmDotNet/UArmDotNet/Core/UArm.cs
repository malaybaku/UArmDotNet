using System;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public class UArm : IUArm
    {
        public UArm(UArmConnector connector)
        {
            Connector = connector;
            Connector.ReceivedEvent += OnReceivedEvent;
        }

        public UArm(string portName, int baudRate) : this(new UArmConnector())
        {
            Connector.SerialConnector.PortName = portName;
            Connector.SerialConnector.BaudRate = baudRate;
        }
        public UArm(string portName) : this (portName, DefaultBaudRate)
        {
        }

        public UArmConnector Connector { get; }

        /// <summary>Default baud rate of the uArm Swift / uArm Swift Pro</summary>
        public static readonly int DefaultBaudRate = 115200;

        //Motion
        public Task MoveAsync(Position position, float speed)
            => SetCommandTransact(
            string.Format(Protocol.MovePositionFormat, position.X, position.Y, position.Z, speed)
            );
        public Task MoveWithLaserOnAsync(Position position, float speed)
            => SetCommandTransact(
            string.Format(Protocol.MovePositionLaserOnFormat, position.X, position.Y, position.Z, speed)
            );
        public Task MoveAsync(Polar polar, float speed)
            => SetCommandTransact(
            string.Format(Protocol.MovePolarFormat, polar.Stretch, polar.Rotation, polar.Height, speed)
            );
        public Task MoveServoAngleAsync(Servos servo, float angle)
            => SetCommandTransact(string.Format(Protocol.MoveServoFormat, (int)servo, angle));
        public Task MoveRelativeAsync(Position positionDisplacement, float speed)
            => SetCommandTransact(string.Format(
            Protocol.MovePositionRelativeFormat,
            positionDisplacement.X,
            positionDisplacement.Y,
            positionDisplacement.Z,
            speed
            ));
        public Task MoveRelativeAsync(Polar polarDisplacement, float speed)
            => SetCommandTransact(string.Format(
            Protocol.MovePolarRelativeFormat,
            polarDisplacement.Stretch,
            polarDisplacement.Rotation,
            polarDisplacement.Height,
            speed
            ));

        //Setting 
        public Task AttachAllMotorAsync()
            => SetCommandTransact(Protocol.AttachAllMotor);
        public Task DetachAllMotorAsync()
            => SetCommandTransact(Protocol.DetachAllMotor);
        public Task StartFeedbackCycleAsync(float intervalSec)
            => SetCommandTransact(string.Format(Protocol.SetFeedbackFormat, intervalSec));
        //NOTE: by setting "0" to interval, then event will stop (confirmed by an experiment).
        public Task StopFeedbackCycleAsync()
            => SetCommandTransact(string.Format(Protocol.SetFeedbackFormat, 0));
        public Task<bool> CheckIsMovingAsync()
            => BoolTransact(Protocol.CheckIsMoving);
        public Task AttachMotorAsync(Servos servo)
            => SetCommandTransact(string.Format(Protocol.AttachMotorFormat, (int)servo));
        public Task DetachMotorAsync(Servos servo)
            => SetCommandTransact(string.Format(Protocol.DetachMotorFormat, (int)servo));
        public Task<bool> CheckMotorAttachedAsync(Servos servo)
            => BoolTransact(string.Format(Protocol.CheckMotorAttachedFormat, (int)servo));
        public Task BeepAsync(int frequency, int durationMillisec)
            => SetCommandTransact(string.Format(Protocol.BeepFormat, frequency, durationMillisec));

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

        public Task SetEnableDefaultFunctionOfBaseButtonsAsync(bool enable)
            => SetCommandTransact(
            string.Format(Protocol.EnableFunctionOfBaseButtonsFormat, (enable ? 1 : 0))
            );
        public async Task<ServoAngles> GetIKAsync(Position position)
        {
            var res = await Transact(string.Format(Protocol.GetIKFormat, position.X, position.Y, position.Z));
            return res.ToServoAngles();
        }
        public async Task<Position> GetFKAsync(ServoAngles angles)
        {
            var res = await Transact(string.Format(Protocol.GetFKFormat, angles.Bottom, angles.Left, angles.Right));
            return res.ToPosition();
        }

        public Task<bool> CanReachAsync(Position position)
            => BoolTransact(string.Format(Protocol.CanReachCartesianFormat, position.X, position.Y, position.Z));
        public Task<bool> CanReachAsync(Polar polar)
            => BoolTransact(string.Format(Protocol.CanReachPolarFormat, polar.Stretch, polar.Rotation, polar.Height));

        public Task SetPumpStateAsync(bool active)
            => SetCommandTransact(
                string.Format(Protocol.SetPumpStateFormat, (active ? 1 : 0))
            );
        public Task SetGripperStateAsync(bool active)
            => SetCommandTransact(
                string.Format(Protocol.SetGripperStateFormat, (active ? 1 : 0))
            );
        public Task SetBluetoothStateAsync(bool active)
            => SetCommandTransact(
                string.Format(Protocol.EnableBluetoothFormat, (active ? 1 : 0))
            );
        public Task SetDigitalPinOutputAsync(int number, bool high)
            => SetCommandTransact(
            string.Format(Protocol.SetDigitalOutputStateFormat, number, (high ? 1 : 0))
            );

        public Task SetBluetoothNameAsync(string name)
        {
            try
            {
                byte[] testEncode = Encoding.ASCII.GetBytes(name);
            }
            catch(Exception)
            {
                throw new UArmException("Bluetooth name must be written by ASCII char");
            }

            if (name.Length > Protocol.BlueToothNameCharLimit)
            {
                throw new UArmException(
                    $"Bluetooth name is too long, maximum is {Protocol.BlueToothNameCharLimit}"
                    );
            }

            return SetCommandTransact(
                string.Format(Protocol.SetBluetoothNameFormat, name)
                );
        }
        public Task SetArmModeAsync(ArmModes mode)
            => SetCommandTransact(
                string.Format(Protocol.SetModeOfTheArmFormat, (int)mode)
            );
        public Task UpdateReferencePoint()
            => SetCommandTransact(Protocol.SetReferencePositionByCurrentPosition);
        public Task UpdateHeightZeroPoint()
            => SetCommandTransact(Protocol.SetHeightZeroPoint);
        public Task SetEndEffectorHeight(float height)
            => SetCommandTransact(
                string.Format(Protocol.SetTheHeightOfEndEffectorFormat, height)
                );

        //Read
        public async Task<ServoAngles> GetAllServoAnglesAsync()
        {
            var res = await Transact(Protocol.GetAllServoAngles);
            return res.ToServoAngles();
        }
        public Task<string> GetDeviceNameAsync()
            => GetInfoStringTransact(Protocol.GetDeviceName);
        public Task<string> GetHardwareVersionAsync()
            => GetInfoStringTransact(Protocol.GetHardwareVersion);
        public Task<string> GetSoftwareVersionAsync()
            => GetInfoStringTransact(Protocol.GetFirmwareVersion);
        public Task<string> GetAPIVersionAsync()
            => GetInfoStringTransact(Protocol.GetAPIVersion);
        public Task<string> GetUIDAsync()
            => GetInfoStringTransact(Protocol.GetUID);
        public async Task<float> GetServoAngleAsync(Servos servo)
        {
            if (servo == Servos.Hand) { throw new ArgumentException("Only Bottom, Left, Right joint angle can be read"); }

            var res = await Transact(
                string.Format(Protocol.GetServoAngleFormat, (int)servo)
                );

            return res.ToFloat();
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

        public Task<bool> CheckLimitedSwitchTriggeredAsync()
            => BoolTransact(Protocol.GetLimitedSwitchState);
        public Task<bool> GetDigitalPinStateAsync(int number)
            => BoolTransact(string.Format(Protocol.GetDigitalPinValueFormat, number));
        public async Task<float> GetAnalogPinStateAsync(int number)
        {
            var res = await Transact(string.Format(Protocol.GetAnalogPinValueFormat, number));
            return res.ToFloat();
        }

        public async Task<ServoAngles> GetDefaultValueOfAS5600Async()
        {
            var res = await Transact(Protocol.GetDefaultValueOfAS5600);
            return res.ToServoAngles();
        }

        /// <summary>Occurs when robot is ready</summary>
        public event EventHandler ReceivedReady;

        /// <summary>Occurs when received position feedback</summary>
        public event EventHandler<PositionFeedbackEventArgs> ReceivedPositionFeedback;

        /// <summary>Occurs when received button action</summary>
        public event EventHandler<ButtonActionEventArgs> ReceivedButtonAction;

        /// <summary>Occurs when power connection state changed</summary>
        public event EventHandler<PowerConnectionChangedEventArgs> PowerConnectionChanged;

        /// <summary>Occurs when limited switch status changed</summary>
        public event EventHandler<LimitedSwitchEventArgs> LimitedSwitchStateChanged;

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

        private void OnReceivedEvent(object sender, UArmEventMessageEventArgs e)
        {
            CheckReceivedReady(e.EventMessage);
            CheckReceivedPositionFeedback(e.EventMessage);
            CheckReceivedButtonAction(e.EventMessage);
            CheckPowerConnectionChanged(e.EventMessage);
            CheckLimitedSwitchStateChanged(e.EventMessage);
            switch(e.EventMessage.Id)
            {
                case 6:
                    int switchNumber;
                    int switchState;
                    if (e.EventMessage.Args.Length > 1 &&
                        int.TryParse(e.EventMessage.Args[0], out switchNumber) &&
                        int.TryParse(e.EventMessage.Args[1], out switchState)
                        )
                    {
                        LimitedSwitchStateChanged?.Invoke(
                            this,
                            new LimitedSwitchEventArgs(switchNumber, switchState != 0)
                            );
                    }
                    return;
                default:
                    //フォーマット不正だけどいったん無視
                    return;
            }
        }

        private void CheckReceivedReady(UArmEventMessage message)
        {
            if (message.Id == Protocol.EventIdReady)
            {
                ReceivedReady?.Invoke(this, EventArgs.Empty);
            }
        }
        private void CheckReceivedPositionFeedback(UArmEventMessage message)
        {
            float x, y, z, handAngle;
            if (message.Id == Protocol.EventIdTimedFeedback &&
                message.Args.Length > 3 &&
                float.TryParse(message.Args[0].Substring(1), out x) &&
                float.TryParse(message.Args[1].Substring(1), out y) &&
                float.TryParse(message.Args[2].Substring(1), out z) && 
                float.TryParse(message.Args[3].Substring(1), out handAngle)
                )
            {
                ReceivedPositionFeedback?.Invoke(
                    this,
                    new PositionFeedbackEventArgs(new Position(x, y, z), handAngle)
                    );
            }
        }
        private void CheckReceivedButtonAction(UArmEventMessage message)
        {
            int buttonType;
            int buttonActionType;

            if (message.Id == Protocol.EventIdButton && 
                message.Args.Length > 1 &&
                int.TryParse(message.Args[0].Substring(1), out buttonType) &&
                int.TryParse(message.Args[1].Substring(1), out buttonActionType)
                )
            {
                ReceivedButtonAction?.Invoke(
                    this,
                    new ButtonActionEventArgs((ButtonTypes)buttonType, (ButtonActionTypes)buttonActionType)
                    );
            }
        }
        private void CheckPowerConnectionChanged(UArmEventMessage message)
        {
            int powerState;

            if (message.Id == Protocol.EventIdPowerConnection &&
                message.Args.Length > 0 &&
                int.TryParse(message.Args[0].Substring(1), out powerState)
                )
            {
                PowerConnectionChanged?.Invoke(
                    this,
                    new PowerConnectionChangedEventArgs(powerState != 0)
                    );
            }
        }
        private void CheckLimitedSwitchStateChanged(UArmEventMessage message)
        {
            int switchNumber;
            int switchState;
            if (message.Id == Protocol.EventIdLimitedSwitch && 
                message.Args.Length > 1 &&
                int.TryParse(message.Args[0].Substring(1), out switchNumber) &&
                int.TryParse(message.Args[1].Substring(1), out switchState)
                )
            {
                LimitedSwitchStateChanged?.Invoke(
                    this,
                    new LimitedSwitchEventArgs(switchNumber, switchState != 0)
                    );
            }
        }

    }
}
