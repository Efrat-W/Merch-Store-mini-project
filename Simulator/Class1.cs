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
        public TupleSimulatorArgs(int d, Order o)
        {
            delay = d;
            ord = o;
        }
    }

    public class IntSimulatorArgs : EventArgs 
    {
        public int state { get; set; }
        public IntSimulatorArgs(int state)
        {
            this.state = state;
        }   
    }
}
