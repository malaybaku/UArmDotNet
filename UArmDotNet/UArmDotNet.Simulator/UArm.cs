using System;
using System.Collections.Generic;

namespace Baku.UArmDotNet.Simulator
{
    /// <summary>
    /// Simulated UArm
    /// NOTE: currently it is more just MOCK than the simulator.
    /// </summary>
    public class UArm
    {
        //NOTE: コイツが出来ること(予定)の一覧。主目的は通信コードのテストであることを忘れずに。

        //1. シリアルの口を模擬するため、バイト配列のコマンドに基づいて返答する。
        //  (コマンドと返答のフォーマットはPythonのAPIから予測したやつ)
        //2. メッセージ送信前に設定することで、返信ディレイや、強制的なエラー返信が出来る
        //3. ジョイントの値とかは普通にプロパティで持ってるので事前/事後/不変条件とかが分かる

        private readonly object _propertyLockObject = new object();

        public Locking<string> HardwareVersion { get; } = new Locking<string>("");
        public Locking<string> FirmwareVersion { get; } = new Locking<string>("");

        //TODO: PositionとPolarとServosは本来別じゃなくて等価な情報持ってるんですよ本来は
        public Locking<Position> Position { get; } = new Locking<Position>();
        public Locking<Polar> Polar { get; } = new Locking<Polar>();
        public Servos Servos { get; } = new Servos();
        //根本的にGetter Only, 本当は動作命令に応じて遷移しないとダメだけどシミュレータでそんなの作ったら面倒すぎ
        public Locking<bool> IsMoving { get; } = new Locking<bool>();

        public Locking<bool> IsGripperCatch { get; } = new Locking<bool>();
        public Locking<bool> IsPumpOn { get; } = new Locking<bool>();

        public RomData RomData { get; } = new RomData();

        //根本的にGetter Only, Setはテストのために仮想入力入れるという位置づけ
        public Locking<bool> IsTipSensorOn { get; } = new Locking<bool>();
        public Pins Pins { get; } = new Pins();
    }

    public class RomData
    {
        //NOTE: ここのアドレス値はたかだか200くらいと見積もる(Python側で名前つきのオフセットアドレス値が100くらいしかないので)

        const int RomDataSize = 256;

        private readonly object _lock = new object();
        private byte[] _data = new byte[RomDataSize];

        public int GetInt(int addr)
        {
            CheckAddrRange(addr, sizeof(int));
            lock(_lock) return BitConverter.ToInt32(_data, addr);
        }
        public float GetFloat(int addr)
        {
            CheckAddrRange(addr, sizeof(float));
            lock(_lock) return BitConverter.ToSingle(_data, addr);
        }
        public byte GetByte(int addr)
        {
            CheckAddrRange(addr, sizeof(byte));
            lock (_lock) return _data[addr];
        }

        public void SetInt(int addr, int data)
        {
            CheckAddrRange(addr, sizeof(int));
            lock (_lock)
            {
                byte[] bin = BitConverter.GetBytes(data);
                Array.Copy(bin, 0, _data, addr, bin.Length);
            }
        }
        public void SetFloat(int addr, float data)
        {
            CheckAddrRange(addr, sizeof(float));
            lock (_lock)
            {
                byte[] bin = BitConverter.GetBytes(data);
                Array.Copy(bin, 0, _data, addr, bin.Length);
            }
        }
        public void SetByte(int addr, byte data)
        {
            CheckAddrRange(addr, sizeof(byte));
            lock (_lock)
            {
                _data[addr] = data;
            }
        }

        private void CheckAddrRange(int addr, int size)
        {
            if (addr < 0 || addr > RomDataSize - size)
            {
                throw new IndexOutOfRangeException();
            }

        }
    }

    public class Servos
    {
        public Servo Bottom { get; } = new Servo();
        public Servo Left { get; } = new Servo();
        public Servo Right { get; } = new Servo();
        public Servo Hand { get; } = new Servo();

        public Servo GetByIndex(int i)
        {
            if (i == 0)
            {
                return Bottom;
            }
            else if (i == 1)
            {
                return Left;
            }
            else if (i == 2)
            {
                return Right;
            }
            else if (i == 3)
            {
                return Hand;
            }
            else
            {
                throw new UArmException();
            }
        }
    }

    public class Servo
    {
        /// <summary>範囲チェックとかしないよ</summary>
        public Locking<float> Angle { get; } = new Locking<float>();
        /// <summary>励磁の有無なのでほんとはattachというよりactiveとかがいい？</summary>
        public Locking<bool> IsAttached { get; } = new Locking<bool>();
    }

    //プロパティを全部排他にしたいので。
    public class Locking<T>
    {
        public Locking(T value) { Value = value; }
        public Locking() : this(default(T)) { }

        private readonly object _lock = new object();

        private T _value;
        public T Value
        {
            get { lock (_lock) return _value; }
            set { lock (_lock) _value = value; }
        } 
    }

    public class Pins
    {
        public bool GetDigitalPinValue(int i)
        {
            lock(_digitalLock)
            {
                if (_digital.ContainsKey(i))
                {
                    return _digital[i];
                }
                else
                {
                    throw new UArmSimulatorRobotException();
                }
            }
        }
        public float GetAnalogPinValue(int i)
        {
            lock(_analogLock)
            {
                if (_analog.ContainsKey(i))
                {
                    return _analog[i];
                }
                else
                {
                    throw new UArmSimulatorRobotException();
                }
            }
        }

        //テスト用に仮想入力つっこめる口を用意
        public void SetDigitalPinValue(int i, bool isOn)
        {
            lock(_digitalLock)
            {
                _digital[i] = isOn;
            }
        }
        public void SetAnalogPinValue(int i, float val)
        {
            lock(_analogLock)
            {
                _analog[i] = val;
            }
        }

        //同じテスト中に挿抜やりたいときのために
        public void RemoveDigitalPin(int i)
        {
            lock(_digitalLock)
            {
                if (_digital.ContainsKey(i))
                {
                    _digital.Remove(i);
                }
            }
        }
        public void RemoveAnalogPin(int i)
        {
            lock (_analogLock)
            {
                if (_analog.ContainsKey(i))
                {
                    _analog.Remove(i);
                }
            }
        }


        private readonly object _digitalLock = new object();
        private readonly object _analogLock = new object();
        private readonly Dictionary<int, bool> _digital = new Dictionary<int, bool>();
        private readonly Dictionary<int, float> _analog = new Dictionary<int, float>();
        
    }

}
