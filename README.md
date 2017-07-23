# UArmDotNet

* Creator: Bakustar (獏☆)
* Release: 2017/July/23

Contents

1. Overview
3. How to use in .NET
4. How to use in Unity
5. Contact

## 1. Overview

`UArmDotNet` is the library to use **[uArm Swift](http://www.ufactory.cc/#/en/uarmswift)** in .NET, Mono, and Unity application.

User of this library can:

* Call uArm gCode APIs written in **[Quick Start Guide](http://download.ufactory.cc/docs/en/swift_quick_start_guide-1.0.0.pdf)**, by `async/await` manner, via SerialPort communication.
* Check raw send/recv message conten

## 2. Hello World

Below is the example to use beep API.

```csharp

using System.Threading.Tasks;
using Baku.UArmDotNet;

namespace HelloUArm
{
    class Program
    {
        static void Main(string[] args)
        {
            MainTask().Wait();
        }
        
        static async Task MainTask()
        {
            // port name depends on the environment
            // NOTE: UArmSearch class supports (and also be an example) to search uArm's serial port name
            var uArm = new UArm("COM1");
            uArm.Connector.Connect();

            // HACK: currently dummy data must be sent for the first message...
            uArm.Connector.Post("dummy");

            // 440Hz, 1000ms beep for hello world
            await uArm.BeepAsync(440, 1000);

            uArm.Connector.Disconnect();
        }
    }
}

```

## 3. How to use in .NET

See **ExampleUArmDotNet** project, in which almost all API is used.

## 4. How to use in Unity

See **ExampleUArmDotNet** project, to understand how the APIs are wrapped.

You can use the UArmDotNet in Unity 2017, by following step.

1. In `File>Build Settings>Player Settings`:
    * Set scripting runtime version to `Experimental(.NET 4.6 Equivalent)`    
2. Add the UArmDotNet source file folders to the asset. The folder structure will be like:
    * UArmDotNet
        + Connector
        + Core
        + DataTypes

And there are some points to remember when use `UArmDotNet` in Unity.

* uArm's **XYZ** coordinate correspondes to Unity's **XZY**, NOT **XYZ** directly.
* UArmDotNet (and original uArm gCode also) uses **milimeter** the unit of length, while  Unity is based on **meter** expression.
* Unity has some thread constraints to do `async/await` in main thread. In some cases you have to cache game layer data (like `transform.position`) to other variable, and use it from `async` function in which you operate uArm Swift.


## 5. Contact

* **[Blog](http://www.baku-dreameater.net/)**
* **[Twitter](https://twitter.com/baku_dreameater)**
