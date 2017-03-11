namespace Baku.UArmDotNet.Simulator
{
    internal class CommandGetHardwareVersion : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P202";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            return robot.HardwareVersion.Value;
        }
    }

    internal class CommandGetFirmwareVersion : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args[0] == "P203";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            return robot.FirmwareVersion.Value;
        }
    }

}
