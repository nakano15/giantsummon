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
    public class HellFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Flame");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0.9f;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.maxPenetrate = 1;
        }

        public override void AI()
        {
            const int FrameDuration = 6;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            projectile.position += projectile.velocity;
            projectile.frameCounter++;
            if (projectile.frameCounter >= FrameDuration)
            {
                projectile.frameCounter -= FrameDuration;
                projectile.frame++;
                if (projectile.frame >= 4)
                    projectile.frame = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 5 * 60);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 5 * 60);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 5 * 60);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position + new Vector2(42, 15) - Main.screenPosition, 
                new Rectangle(projectile.frame * 54, 0, 54, 30), Color.White, 
                projectile.rotation, new Vector2(42, 15), projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
