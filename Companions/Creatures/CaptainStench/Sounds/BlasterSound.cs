using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Companions.CaptainStench.Sounds
{
    public class BlasterSound : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * 0.5f;
            return soundInstance;
        }
    }
}
