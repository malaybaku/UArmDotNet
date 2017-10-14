using System;

namespace Baku.UArmDotNet
{

    public class PositionFeedbackEventArgs : EventArgs
    {
        public PositionFeedbackEventArgs(Position position, float handAngle)
        {
            Position = position;
            HandAngle = handAngle;
        }
        public Position Position { get; }
        public float HandAngle { get; }
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

    public class GroveColorSensorDataEventArgs : EventArgs
    {
        public GroveColorSensorDataEventArgs(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
    }

    public class GroveGestureSensorDataEventArgs : EventArgs
    {
        //ちょっと難しいのでは
        public GroveGestureSensorDataEventArgs(GroveGestureSensorStates state)
        {
            State = state;
        }
        public GroveGestureSensorStates State { get; }
    }

    public class GroveUltrasonicDataEventArgs : EventArgs
    {
        public GroveUltrasonicDataEventArgs(int distanceCentimeter)
        {
            DistanceCentimeter = distanceCentimeter;
        }

        public int DistanceCentimeter { get; }
    }

    public class GroveTempAndHumiditySensorDataEventArgs : EventArgs
    {
        public GroveTempAndHumiditySensorDataEventArgs(double temperature, double humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }

        /// <summary>Get the temperature in celcius</summary>
        public double Temperature { get; }

        /// <summary>Get the humidity by [%]</summary>
        public double Humidity { get; }
    }

    public class GrovePirMotionSensorDataEventArgs : EventArgs
    {
        public GrovePirMotionSensorDataEventArgs(bool detectMotion)
        {
            DetectMotion = detectMotion;
        }

        public bool DetectMotion { get; }
    }
}
