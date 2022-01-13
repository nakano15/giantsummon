using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace giantsummon.Companions.CaptainStench.Sounds
{
    public class PhantomBlitzSoundEffect : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * 0.5f;
            return soundInstance;
        }
    }
}
