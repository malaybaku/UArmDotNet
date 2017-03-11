using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet.Simulator
{
    internal interface ICommandProcessor
    {


        /// <summary>このインターフェースが処理できるコマンドかどうかを確認します。</summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool CanProcess(UArmCommand command);

        /// <summary>
        /// コマンドを処理します。マッチしないコマンドに対して呼び出すと適当にNGとかで返します
        /// (本物の動作わかんないので未定義扱い)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string Process(UArmCommand command, UArm robot);
    }

    internal static class ICommandProcessorExtension
    {
        public static void ThrowIfNotMatch(this ICommandProcessor processor, UArmCommand command)
        {
            if (!processor.CanProcess(command))
            {
                throw new UArmSimulatorCommandException();
            }
        }
    }
}
