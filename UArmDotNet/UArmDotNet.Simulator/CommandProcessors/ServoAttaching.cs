using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet.Simulator
{
    internal class CommandAttachServo : ICommandProcessor
    {
        public bool CanProcess(UArmCommand command)
        {
            return command.Args.Length >= 2 &&
                command.Args[0] == "201";
        }

        public string Process(UArmCommand command, UArm robot)
        {
            try
            {
                //int servoNumber = int.Parse(command.Args[1].Substring(1));
                //Servo target = 
                //TODO: 作るなら作る、作らないなら作らない
                throw new NotImplementedException();
            }
            catch (FormatException)
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }
}
