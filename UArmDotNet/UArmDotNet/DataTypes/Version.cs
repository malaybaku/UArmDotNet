using System;
using System.Text.RegularExpressions;

namespace Baku.UArmDotNet
{
    public class Version
    {
        public bool CheckIsVersion(string version)
            => Regex.IsMatch(version, @"\d+\.\d+\.\d+\w*");

        public bool CheckIsSupportedVersion(string version)
        {
            //Python側の動作がいまいち納得行ってないのだけど、これだと"0.1.2.2"みたいのも通る？
            throw new NotImplementedException();
        }

        

    }
}
