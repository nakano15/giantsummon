using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon
{
    public class Group
    {
        public string Name = "";
        public bool RecognizeAsTerraGuardian = false;
        public bool CustomSprite = true, AgeAffectsScale = true;
        public float AgingSpeed = 1f;
    }
}
