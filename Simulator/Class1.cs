using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class TupleSimulatorArgs : EventArgs
    {
        public int delay { get; set; }
        public Order ord { get; set; }
        public int state { get; set; }
        public TupleSimulatorArgs(int d, Order o)
        {
            delay = d;
            ord = o;
            state = 1;
        }

        public TupleSimulatorArgs(int s)
        {
            delay = -1;
            ord = null;
            state = s;
        }
    }
}
