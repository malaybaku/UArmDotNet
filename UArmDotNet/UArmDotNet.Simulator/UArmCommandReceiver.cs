using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.UArmDotNet.Simulator
{
    public class UArmCommandReceiver
    {
        public UArmCommandReceiver(UArm target)
        {
            _target = target;
        }
        private readonly UArm _target;

        public void Receive(byte[] data)
        {
            try
            {
                Encoding.ASCII.GetString(data).Split(' ');
            }
            catch(FormatException)
            {
                throw new UArmException();
            }
        }

    }
}
