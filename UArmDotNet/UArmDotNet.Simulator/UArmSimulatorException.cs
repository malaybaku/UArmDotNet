using System;

namespace Baku.UArmDotNet.Simulator
{
    public class UArmSimulatorException : Exception
    {
    }

    public class UArmSimulatorCommandException : UArmSimulatorException
    {

    }

    public class UArmSimulatorRobotException : UArmSimulatorException
    { }

}
