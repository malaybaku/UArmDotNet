using System;

namespace Baku.UArmDotNet.Simulator
{
    internal class CommandGetPosition : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P220";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            var pos = robot.Position.Value;
            return $"X{pos.X} Y{pos.Y} Z{pos.Z}";
        }
    }

    internal class CommandSetPosition : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "G0" &&
                command.Args.Length >= 5;
        }

        public string Process(UArmCommand command, UArm robot)
        {
            try
            {
                float x = float.Parse(command.Args[1].Substring(1));
                float y = float.Parse(command.Args[2].Substring(1));
                float z = float.Parse(command.Args[3].Substring(1));
                float f = float.Parse(command.Args[4].Substring(1));
                //NOTE: 位置の一貫性とか知らないので適当にやっています
                robot.Position.Value = new Position(x, y, z);
                robot.Servos.Hand.Angle.Value = f;
                return "";
            }
            catch(FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }

    internal class CommandGetPolar : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P221";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            var polar = robot.Polar.Value;

            return $"S{polar.Stretch} R{polar.Rotation} H{polar.Height}";
        }
    }

    internal class CommandSetPolar : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "G201" &&
                command.Args.Length >= 5;
        }

        public string Process(UArmCommand command, UArm robot)
        {
            try
            {
                float s = float.Parse(command.Args[1].Substring(1));
                float r = float.Parse(command.Args[2].Substring(1));
                float h = float.Parse(command.Args[3].Substring(1));
                float f = float.Parse(command.Args[4].Substring(1));
                //NOTE: 位置の一貫性とか知らないので適当にやっています
                robot.Polar.Value = new Polar(s, r, h);
                robot.Servos.Hand.Angle.Value = f;
                return "";
            }
            catch (FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }

    internal class CommandGetServoAngles : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P200";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            return string.Format("V{0} V{1} V{2} V{3}",
                robot.Servos.Bottom.Angle.Value,
                robot.Servos.Left.Angle.Value,
                robot.Servos.Right.Angle.Value,
                robot.Servos.Hand.Angle.Value
                );
        }
    }

    internal class CommandSetServoAngle : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 3 &&
                command.Args[0] == "G202";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            try
            {
                int servoNumber = int.Parse(command.Args[1].Substring(1));
                Servo target = robot.Servos.GetByIndex(servoNumber);
                target.Angle.Value = float.Parse(command.Args[2].Substring(1));
                return "";
            }
            catch(FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }

    internal class CommandStopMoving : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "G203";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            robot.IsMoving.Value = false;
            return "";
        }
    }

}
