using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicWrapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var wt = new WrapperTest();
        }
    }

    class WrapperTest
    {
        public WrapperTest()
        {
            var vGen = new vGenBasicWrapper();
            var vj = vGen.vXbox.Device(1);
            vj.SetAxis(1, 100);
            Thread.Sleep(500);
            vj.SetAxis(1, 0);
        }
    }
}
