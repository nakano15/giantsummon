using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class BuffData
    {
        public int ID = 0, Time = 0;

        public BuffData(int id, int time = 300)
        {
            ID = id;
            Time = time;
        }
    }
}
