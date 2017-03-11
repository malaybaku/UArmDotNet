using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet.Simulator
{
    internal class CommandGetPump : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P231";    
        }

        public string Process(UArmCommand command, UArm robot)
        {
            int flag = robot.IsPumpOn.Value ? 1 : 0;
            return $"V{flag}";
        }
    }

    internal class CommandSetPump : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 2 &&
                command.Args[0] == "M231";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            if (command.Args[1] == "V0")
            {
                robot.IsPumpOn.Value = true;
            }
            else if (command.Args[1] == "V1")
            {
                robot.IsPumpOn.Value = false;
            }
            else
            {
                throw new UArmSimulatorCommandException();
            }
            return "";
        }
    }

    internal class CommandGetGripper : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P232";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            int flag = robot.IsGripperCatch.Value ? 1 : 0;
            return $"V{flag}";
        }
    }

    internal class CommandSetGripper : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 2 &&
                command.Args[0] == "M232";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            if (command.Args[1] == "V0")
            {
                robot.IsGripperCatch.Value = true;
            }
            else if (command.Args[1] == "V1")
            {
                robot.IsGripperCatch.Value = false;
            }
            else
            {
                throw new UArmSimulatorCommandException();
            }
            return "";
        }
    }

    internal class CommandGetTipSensor : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P233";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            int flag = robot.IsTipSensorOn.Value ? 1 : 0;
            return $"V{flag}";
        }
    }

    internal class CommandGetDigitalPinValue : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 2 &&
                command.Args[0] == "P240";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            try
            {
                int pinNumber = int.Parse(command.Args[1].Substring(1));
                int flag = robot.Pins.GetDigitalPinValue(pinNumber) ? 1 : 0;
                return $"V{flag}";
            }
            catch(FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }

    internal class CommandGetAnalogPinValue : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 2 &&
                command.Args[0] == "P241";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            try
            {
                int pinNumber = int.Parse(command.Args[1].Substring(1));
                float value = robot.Pins.GetAnalogPinValue(pinNumber);
                return $"V{value}";
            }
            catch (FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }

}
