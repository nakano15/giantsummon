using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;

namespace giantsummon
{
    public class SoundData
    {
        public Terraria.Audio.LegacySoundStyle SoundStyle = null;
        public int? SoundID = null;

        public SoundData(Terraria.Audio.LegacySoundStyle SoundStyle)
        {
            this.SoundStyle = SoundStyle;
        }

        public SoundData(int SoundID)
        {
            this.SoundID = SoundID;
        }

        public void PlaySound(Vector2 Position)
        {
            if (SoundStyle != null)
            {
                Main.PlaySound(SoundStyle, Position);
            }
            else if (SoundID.HasValue)
            {
                Main.PlaySound(SoundID.Value, Position);
            }
        }
    }
}
