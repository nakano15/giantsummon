using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Items.Weapons
{
    class CaitSithScimitar : GuardianItemPrefab
    {
        private byte ItemLevel = 0;
        private int Exp = 0;

        public override void SetDefaults()
        {
            item.damage = 29;
            item.melee = true;
            item.width = 44;
            item.height = 50;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 1;
            item.knockBack = 3.2f;
            item.crit = 12;
            item.rare = 6;
            item.UseSound = Terraria.ID.SoundID.Item1;
            item.autoReuse = false;
			PlayerCanUse = true;
            ItemOrigin = new Vector2(6, 43);
        }

        /*public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if(ItemLevel < 255 && damage > 0 && target.type != Terraria.ID.NPCID.TargetDummy)
            {
                int ExpGot = (int)Math.Max(1, damage * 0.125f);
                Exp += ExpGot;
                if (Exp >= 100 * (ItemLevel +1))
                {
                    ItemLevel++;
                    Exp = 0;
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            base.OnHitPvp(player, target, damage, crit);
        }*/
    }
}
