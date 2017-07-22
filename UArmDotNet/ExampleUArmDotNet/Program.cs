using Baku.UArmDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleUArmDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            MainRoutine().Wait();
        }

        static async Task MainRoutine()
        {
            //search target serial port names
            string target = UArmSearch.SearchPortNames().FirstOrDefault();

            //
            var uArm = new UArm(target, 115200);
            uArm.Connector.Connect();

            //ダミー: 1回目のメッセージは先頭に"????"という謎プレフィクスがついてる扱いになって必ずエラーになるので、適当なメッセージをなげこむ
            uArm.Connector.Post("Hello");

            Console.WriteLine("H/W ver: " + await uArm.GetHardwareVersionAsync());
            Console.WriteLine("S/W ver: " + await uArm.GetSoftwareVersionAsync());
            Console.WriteLine("API ver: " + await uArm.GetAPIVersionAsync());


            uArm.Connector.Disconnect();
        }
    }
}
