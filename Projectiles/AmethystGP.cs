using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Projectiles
{
    public class AmethystGP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amethyst Gemstone Power");
        }

        public override void SetDefaults() //Needs Setup and AI
        {
            projectile.width = projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
        }
    }
}
