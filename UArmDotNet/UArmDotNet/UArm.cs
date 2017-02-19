using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        //NOTE: SerialPort使わない。人、これを腐敗防止と呼ぶ。
        private ISerialConnector _serial;


        public void Connect()
        {
            _serial.Connect();
        }

        public void Disconnect()
        {
            _serial.Disconnect();
        }

        public bool IsConnected
        {
            get { return _serial.IsConnected; }
        }

        public string GetFirmwareVersion() 
        {
            int id = _serial.PostCommand(Protocol.GET_FIRMWARE_VERSION);
            string res = _serial.WaitCommandResponse(id);

            throw new NotImplementedException();
        }
        public string GetHardwareVersion()
        {
            throw new NotImplementedException();
        }
        public bool CheckTipSensorStatusIsOn()
        {
            throw new NotImplementedException();
        }
        public float[] GetServoAngles()
        {
            //NOTE: 元のほうでは単軸の値もとれるらしいが、どのみちメッセージプロトコル的には全部飛んでくるので分ける意味がない
            throw new NotImplementedException();
        }
        public float[] GetPosition()
        {
            throw new NotImplementedException();
        }
        public float[] GetPolar()
        {
            throw new NotImplementedException();
        }
        public int GetIntRomData(int addr)
        {
            throw new NotImplementedException();
        }
        public float GetFloatRomData(int addr)
        {
            throw new NotImplementedException();
        }
        public byte GetByteRomData(int addr)
        {
            throw new NotImplementedException();
        }
        public int GetAnalogPinValue(int pinNumber)
        {
            throw new NotImplementedException();
        }
        public bool GetDigitalPinValue(int pinNumber)
        {
            throw new NotImplementedException();
        }
        public bool CheckIsMoving()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(float x, float y, float z, float speed, bool relative, bool blocking)
        {
            throw new NotImplementedException();
        }
        public void SetPump(bool isOn)
        {
            throw new NotImplementedException();
        }
        public void SetGripper(bool isCatch)
        {
            throw new NotImplementedException();
        }
        public void SetWrist(float angle)
        {
            throw new NotImplementedException();
        }
        public void SetServoAngle(Servos servo, float angle)
        {
            throw new NotImplementedException();
        }
        public void SetBuzzer(int freq, int durationMillisec)
        {
            throw new NotImplementedException();
        }
        public void SetPolarCoordinate(float rotation, float stretch, float height, float speed, bool isBlocking)
        {
            throw new NotImplementedException();
        }
        public void SetServoAttach(Servos servo, bool moveToCurrentTarget)
        {
            throw new NotImplementedException();
        }
        public void SetServoAllAttach(bool moveToCurrentTarget)
        {
            throw new NotImplementedException();
        }
        public void SetServoDetach(Servos servo)
        {
            throw new NotImplementedException();
        }
        public void SetServoAllDetach()
        {
            throw new NotImplementedException();
        }
        public void SetIntRomData(int addr, int data)
        {
            throw new NotImplementedException();
        }
        public void SetFloatRomData(int addr, float data)
        {
            throw new NotImplementedException();
        }
        public void SetByteRomData(int addr, byte data)
        {
            throw new NotImplementedException();
        }

        //TODO: レコーディング処理もあるけど実装方法がいまいちわからない

        public void Dispose()
        {
            Disconnect();
        }

    }
}
