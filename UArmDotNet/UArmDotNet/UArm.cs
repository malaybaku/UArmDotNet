using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Baku.UArmDotNet
{
    //TODOメモ
    //1. interface挟むか検討する(主にロガー挟むため)
    //2. そもそもPythonのI/Fが若干アレなので直すべきか検討されたく
    //3. たぶんSwiftになると仕様変更なので仕様変更対策しておくこと



    /// <summary>
    /// 
    /// </summary>
    public class UArm : IDisposable
    {
        public UArm(string portName, bool isDebug, bool isBlock, float timeOutSec)
        {
            throw new NotImplementedException();
        }

        UArmConnector _connector;


        public void Connect()
        {
            _connector.Connect();
        }

        public void Disconnect()
        {
            _connector.Disconnect();
        }

        public bool IsConnected
        {
            get { return _connector.IsConnected; }
        }

        #region Getter

        public IObservable<string> ObserveFirmwareVersion()
        {
            return Transact(Protocol.GET_FIRMWARE_VERSION)
                .Select(res => res.ToVersion());
        }
        public string GetFirmwareVersion() 
        {
            return ObserveFirmwareVersion().Wait();
        }

        public IObservable<string> ObserveHardwareVersion()
        {
            return Transact(Protocol.GET_HARDWARE_VERSION)
                .Select(res => res.ToVersion());
        }
        public string GetHardwareVersion()
        {
            return ObserveHardwareVersion().Wait();
        }

        public IObservable<bool> ObserveTipSensorIsOn()
        {
            return Transact(Protocol.GET_TIP_SENSOR)
                .Select(res => res.Args[1] == "V0" ? true : false);
        }
        public bool GetTipSensorIsOn()
        {
            return ObserveTipSensorIsOn().Wait();
        }

        public IObservable<ServoAngles> ObserveServoAngles()
        {
            return Transact(Protocol.GET_SERVO_ANGLE)
                .Select(res => res.ToServoAngles());
        }
        public ServoAngles GetServoAngles()
        {
            return ObserveServoAngles().Wait();
        }

        public IObservable<Position> ObservePosition()
        {
            return Transact(Protocol.GET_POLAR)
                .Select(res => res.ToPosition());
        }
        public Position GetPosition()
        {
            return ObservePosition().Wait();
        }

        public IObservable<Polar> ObservePolar()
        {
            return Transact(Protocol.GET_POLAR)
                .Select(res => res.ToPolar());
        }
        public Polar GetPolar()
        {
            return ObservePolar().Wait();
        }

        public IObservable<int> ObserveIntRomData(int addr)
        {
            return _connector
                .Transact(string.Format(Protocol.GET_EEPROM, addr, (int)(EEPROMDataTypeCodes.Integer)))
                .Select(res => res.ToInt());
        }
        public int GetIntRomData(int addr)
        {
            return ObserveIntRomData(addr).Wait();
        }

        public IObservable<float> ObserveFloatRomData(int addr)
        {
            return _connector
                .Transact(string.Format(Protocol.GET_EEPROM, addr, (int)(EEPROMDataTypeCodes.Float)))
                .Select(res => res.ToFloat());
        }
        public float GetFloatRomData(int addr)
        {
            return ObserveFloatRomData(addr).Wait();
        }

        public IObservable<byte> ObserveByteRomData(int addr)
        {
            return _connector
                .Transact(string.Format(Protocol.GET_EEPROM, addr, (int)(EEPROMDataTypeCodes.Byte)))
                .Select(res => res.ToByte());
        }
        public byte GetByteRomData(int addr)
        {
            return ObserveByteRomData(addr).Wait();
        }

        public IObservable<int> ObserveAnalogPinValue(int pinNumber)
        {
            return Transact(string.Format(Protocol.GET_ANALOG, pinNumber))
                .Select(res => res.ToInt());
        }
        public int GetAnalogPinValue(int pinNumber)
        {
            return ObserveAnalogPinValue(pinNumber).Wait();
        }

        public IObservable<bool> ObserveDigitalPinValue(int pinNumber)
        {
            return Transact(string.Format(Protocol.GET_DIGITAL, pinNumber))
                .Select(res => res.ToBool());
        }
        public bool GetDigitalPinValue(int pinNumber)
        {
            return ObserveDigitalPinValue(pinNumber).Wait();
        }

        public IObservable<bool> ObserveIsMoving()
        {
            return Transact(Protocol.GET_IS_MOVE)
                .Select(res => res.ToBool());
        }
        public bool CheckIsMoving()
        {
            return ObserveIsMoving().Wait();
        }

        #endregion

        #region Setter

        public IObservable<Unit> ObserveSetPosition(Position position, float speed, bool relative)
        {
            return ObserveSetPosition(position.X, position.Y, position.Z, speed, relative);
        }
        public IObservable<Unit> ObserveSetPosition(float x, float y, float z, float speed, bool relative)
        {
            return (relative ?
                Transact(string.Format(Protocol.SET_POSITION_RELATIVE, x, y, z, speed)) :
                Transact(string.Format(Protocol.SET_POSITION, x, y, z, speed)) 
                )
                .Select(res =>
                {
                    if (res.IsOK()) return System.Reactive.Unit.Default;

                    throw new UArmException("Failed to set position");
                });
        }
        public void SetPosition(Position position, float speed, bool relative)
        {
            SetPosition(position.X, position.Y, position.Z, speed, relative);
        }
        public void SetPosition(float x, float y, float z, float speed, bool relative)
        {
            if (relative)
            {
                Post(string.Format(Protocol.SET_POSITION_RELATIVE, x, y, z, speed));
            }
            else
            {
                Post(string.Format(Protocol.SET_POSITION, x, y, z, speed));
            }
        }

        public void SetPump(bool isOn)
        {
            Post(string.Format(Protocol.SET_PUMP, (isOn ? 1 : 0)));
        }

        public void SetGripper(bool isCatch)
        {
            Post(string.Format(Protocol.SET_GRIPPER, (isCatch ? 1 : 0)));
        }

        public void SetWrist(float angle)
        {
            SetServoAngle(Servos.Hand, angle);
        }

        public void SetServoAngle(Servos servo, float angle)
        {
            Post(string.Format(Protocol.SET_SERVO_ANGLE, (int)servo, angle));
        }

        public void SetBuzzer(int frequency, float durationSec)
        {
            Post(string.Format(Protocol.SET_BUZZER, frequency, durationSec));
        }

        public IObservable<Unit> ObserveSetPolarCoordinate(Polar polar, float speed)
        {
            return ObserveSetPolarCoordinate(polar.Rotation, polar.Stretch, polar.Height, speed);
        }
        public IObservable<Unit> ObserveSetPolarCoordinate(float rotation, float stretch, float height, float speed)
        {
            return Transact(string.Format(Protocol.SET_POLAR, stretch, rotation, height, speed))
                .Select(res =>
                {
                    if (res.IsOK()) return System.Reactive.Unit.Default;

                    throw new UArmException("Failed to set position");
                });
        }
        public void SetPolarCoordinate(Polar polar, float speed)
        {
            SetPolarCoordinate(polar.Rotation, polar.Stretch, polar.Height, speed);
        }
        public void SetPolarCoordinate(float rotation, float stretch, float height, float speed)
        {
            Post(string.Format(Protocol.SET_POLAR, stretch, rotation, height, speed));
        }

        public IObservable<Unit> ObserveSetServoAttach(Servos servo)
        {
            return SetCommandTransact(string.Format(Protocol.ATTACH_SERVO, (int)servo));
        }
        public IObservable<Unit> ObserveSetAllServoAttach()
        {
            var plan = ObserveSetServoAttach(Servos.Bottom)
                .And(ObserveSetServoAttach(Servos.Left))
                .And(ObserveSetServoAttach(Servos.Right))
                .And(ObserveSetServoAttach(Servos.Hand))
                .Then((_, __, ___, ____) => Unit.Default);

            return Observable.When(plan);
        }
        public void SetServoAttach(Servos servo)
        {
            Post(string.Format(Protocol.ATTACH_SERVO, (int)servo));
        }
        public void SetServoAllAttach()
        {
            SetServoAttach(Servos.Bottom);
            SetServoAttach(Servos.Left);
            SetServoAttach(Servos.Right);
            SetServoAttach(Servos.Hand);
        }

        public IObservable<Unit> ObserveSetServoDetach(Servos servo)
        {
            return SetCommandTransact(string.Format(Protocol.DETACH_SERVO, (int)servo));
        }
        public IObservable<Unit> ObserveSetServoAllDetach()
        {
            var plan = ObserveSetServoDetach(Servos.Bottom)
                .And(ObserveSetServoDetach(Servos.Left))
                .And(ObserveSetServoDetach(Servos.Right))
                .And(ObserveSetServoDetach(Servos.Hand))
                .Then((_, __, ___, ____) => Unit.Default);

            return Observable.When(plan);
        }
        public void SetServoDetach(Servos servo)
        {
            Post(string.Format(Protocol.DETACH_SERVO, (int)servo));
        }
        public void SetServoAllDetach()
        {
            SetServoDetach(Servos.Bottom);
            SetServoDetach(Servos.Left);
            SetServoDetach(Servos.Right);
            SetServoDetach(Servos.Hand);
        }

        public IObservable<Unit> ObserveSetIntRomData(int addr, int data)
        {
            return SetCommandTransact(string.Format(Protocol.SET_EEPROM, (int)EEPROMDataTypeCodes.Integer, data));
        }
        public void SetIntRomData(int addr, int data)
        {
            Post(string.Format(Protocol.SET_EEPROM, (int)EEPROMDataTypeCodes.Integer, data));
        }
        public IObservable<Unit> ObserveSetFloatRomData(int addr, float data)
        {
            return SetCommandTransact(string.Format(Protocol.SET_EEPROM, (int)EEPROMDataTypeCodes.Float, data));
        }
        public void SetFloatRomData(int addr, float data)
        {
            Post(string.Format(Protocol.SET_EEPROM, (int)EEPROMDataTypeCodes.Float, data));
        }
        public IObservable<Unit> ObserveSetByteRomData(int addr, byte data)
        {
            return SetCommandTransact(string.Format(Protocol.SET_EEPROM, (int)EEPROMDataTypeCodes.Byte, data));
        }
        public void SetByteRomData(int addr, byte data)
        {
            Post(string.Format(Protocol.SET_EEPROM, (int)EEPROMDataTypeCodes.Byte, data));
        }


        #endregion

        //TODO: レコーディング処理もあるけど実装方法がいまいちわからない

        public void Dispose()
        {
            Disconnect();
        }

        private void Post(string command)
        {
            _connector.Post(command);
        }
        private IObservable<UArmResponse> Transact(string command)
        {
            return _connector.Transact(command);
        }

        private IObservable<Unit> SetCommandTransact(string command)
        {
            return Transact(command).Select(res =>
            {
                if (res.IsOK()) return Unit.Default;

                throw new UArmException();
            });
        }

    }
}
