using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Projectiles
{
    public class BloodVomit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Vomit");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }

        public override bool PreAI()
        {
            projectile.velocity.Y += 0.05f;
            //projectile.velocity.X *= 0.95f;
            if (projectile.wet)
                projectile.Kill();
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int bd = 0; bd < 4; bd++)
            {
                Vector2 DustSpawnPos = projectile.position;
                DustSpawnPos.X += Main.rand.Next(projectile.width);
                DustSpawnPos.Y += Main.rand.Next(projectile.height);
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f);
            }
            return false;
        }
    }
}
