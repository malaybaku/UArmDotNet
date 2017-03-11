using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet.Simulator
{
    class UArmCommand
    {
        private UArmCommand(int id, string[] args)
        {
            Id = id;
            Args = args;
        }

        public int Id { get; private set; }

        //NOTE: ほんとはReadOnlyListがいいけど.NET 3.5互換を考えたりして微妙にやりづれえのでこれで放置します
        public string[] Args { get; private set; }

        public static UArmCommand Parse(byte[] data)
        {
            throw new NotImplementedException();
        }

    }
}
