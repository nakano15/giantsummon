using Terraria;

namespace giantsummon.Buffs
{
    public class CrimsonConflagration : GuardianModBuff
    {
        private byte Time = 0;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Crimson Conflagration");
            Description.SetDefault("");
            Main.debuff[Type] = Main.pvpBuff[Type] = Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (UpdateDebuffTime())
            {
                int Damage = GetDamageToInflict(npc.lifeMax, npc.defense);
                CombatText.NewText(npc.getRect(), CombatText.DamagedHostile, Damage, false, true);
                npc.life -= Damage;
                if (npc.life <= 0)
                    npc.checkDead();
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (UpdateDebuffTime())
            {
                int Damage = GetDamageToInflict(player.statLifeMax2, player.statDefense);
                CombatText.NewText(player.getRect(), CombatText.DamagedFriendly, Damage, false, true);
                player.statLife -= Damage;
                if (player.statLife <= 0)
                    player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " left only their ashes."), Damage, 0, false);
            }
        }

        public override void Update(TerraGuardian guardian)
        {
            if (UpdateDebuffTime())
            {
                int Damage = GetDamageToInflict(guardian.MHP, guardian.Defense);
                guardian.HP -= Damage;
                if (guardian.HP <= 0)
                    guardian.Knockout(" left only their ashes.");

            }
        }

        private int GetDamageToInflict(int MaxHealth, int Defense)
        {
            int FinalDamage = (int)((MaxHealth - Defense * 0.5f) * 0.01f);
            if (FinalDamage < 1)
                FinalDamage = 1;
            if (FinalDamage > 50)
                FinalDamage = 50;
            return FinalDamage;
        }

        private bool UpdateDebuffTime()
        {
            Time++;
            if(Time >= 60)
            {
                Time = 0;
                return true;
            }
            return false;
        }
    }
}
