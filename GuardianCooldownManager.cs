using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianCooldownManager
    {
        public CooldownType type;
        public int CooldownTime = 0;
        public bool ResetUponDeath { get { return type != CooldownType.ResurrectionCooldown; } }
        public bool DontDepleteOvertime { get { return type == CooldownType.BeetleCountdown || type == CooldownType.BeetleCounter || type == CooldownType.ShopCheckCooldown; } }

        public enum CooldownType : byte
        {
            HealingPotionCooldown,
            LavaHurt,
            TileHurt,
            SwimTime,
            CloudJump,
            SandstormJump,
            BlizzardJump,
            FartJump,
            WaterJump,
            LavaToleranceCooldown,
            SpeedBootsSoundEffect,
            SuffocationDelay,
            StickyTileBreak,
            RocketSoundDelay,
            RocketDelay,
            CursedEffect,
            ResurrectionCooldown,
            FoodCheckingCooldown,
            DelayedActionCooldown,
            ShadowDodgeCooldown,
            PetalCooldown,
            ManaRegenDelay,
            BeetleCounter,
            BeetleCountdown,
            StealthTime,
            ItemLootStackCheck,
            BiomeCheckStacker,
            SpelunkerEffect,
            PhantasmCooldown,
            NebulaCD,
            TargetMemoryCooldown,
            ShopCheckCooldown,
            CarpetFlightTime,
            SpottingCooldown,
            TownNpcInfoCheckingCooldown
        }
    }
}
