using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public struct Node
    {
        public ushort px, py;
        public byte TileStep;
        public StepDirection direction;

        public Node(ushort px, ushort py, byte step, StepDirection dir)
        {
            this.px = px;
            this.py = py;
            this.TileStep = step;
            this.direction = dir;
        }

        public enum StepDirection : byte
        {
            Left, Right, Up, Down
        }
    }
}
