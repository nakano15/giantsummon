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
    public class CannonBlast : ModProjectile
    {
        byte Frame = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cannon Blast");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.light = 0.3f;
        }

        public override void AI()
        {
            projectile.position += projectile.velocity;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            Frame++;
            if (Frame >= 12)
                Frame -= 12;
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 8; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 132);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 ProjPos = projectile.position + new Vector2(projectile.width, projectile.height) * 0.5f;
            Lighting.AddLight(ProjPos, 0.160f * 1.75f, 0.248f * 1.75f, 0.248f * 1.75f);
            spriteBatch.Draw(Main.projectileTexture[projectile.type], ProjPos - Main.screenPosition, new Rectangle(58 * (int)(Frame * 0.25f), 0, 58, 20), Color.White, projectile.rotation,
                new Vector2(48, 10), projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
