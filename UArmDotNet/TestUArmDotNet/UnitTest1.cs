using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baku.UArmDotNet;

namespace TestUArmDotNet
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            var uarm = new UArm();
            var res = await uarm.BeepAsync(440, 500);
        }
    }
}
