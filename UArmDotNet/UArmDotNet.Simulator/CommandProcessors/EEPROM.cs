using System;

namespace Baku.UArmDotNet.Simulator
{
    internal class CommandGetEEPROM : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 4 &&
                command.Args[0] == "M211";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            int addr = 0;
            if (!int.TryParse(command.Args[2].Substring(1), out addr))
            {
                throw new UArmSimulatorCommandException();
            }
            int dataType = 0;
            if (!int.TryParse(command.Args[3].Substring(1), out dataType))
            {
                throw new UArmSimulatorCommandException();
            }
            
            switch(dataType)
            {
                //リテラルで書くのはテスト整備が目的だから。
                case 1:
                    return $"V{robot.RomData.GetByte(addr)}";
                case 2:
                    return $"V{robot.RomData.GetInt(addr)}";
                case 4:
                    return $"V{robot.RomData.GetFloat(addr)}";
                default:
                    throw new UArmSimulatorCommandException();
            }
        }
    }

    internal class CommandSetEEPROM : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 5 &&
                command.Args[0] == "M212";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            int addr = 0;
            if (!int.TryParse(command.Args[2].Substring(1), out addr))
            {
                throw new UArmSimulatorCommandException();
            }
            int dataType = 0;
            if (!int.TryParse(command.Args[3].Substring(1), out dataType))
            {
                throw new UArmSimulatorCommandException();
            }

            string valSource = command.Args[4].Substring(1);

            try
            {
                switch (dataType)
                {
                    //なるべくリテラルで書くのはテスト整備が目的だから。
                    case 1:
                        robot.RomData.SetByte(addr, byte.Parse(valSource));
                        return "";
                    case 2:
                        robot.RomData.SetInt(addr, int.Parse(valSource));
                        return "";
                    case 4:
                        robot.RomData.SetFloat(addr, float.Parse(valSource));
                        return "";
                    default:
                        throw new UArmSimulatorCommandException();
                }

            }
            catch(FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }

}
