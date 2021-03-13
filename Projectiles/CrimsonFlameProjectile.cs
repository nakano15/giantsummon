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
    public class CrimsonFlameProjectile : ModProjectile
    {
        public bool Impact = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Flame");
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 1.15f;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (!Impact)
            {
                projectile.rotation = (float)Math.Atan2(Math.Cos(projectile.velocity.Y), Math.Sin(projectile.velocity.X));
                projectile.position += projectile.velocity;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 10 * 6)
                    projectile.frameCounter -= 10 * 6;
                projectile.frame = projectile.frameCounter / 6;
            }
            else
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 5 * 6)
                    projectile.active = false;
                else
                    projectile.frame = projectile.frameCounter / 6 + 10;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            ChangeToImpact();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            ChangeToImpact();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            ChangeToImpact();
        }

        public void ChangeToImpact()
        {
            Impact = true;
            projectile.frame = 0;
            projectile.frameCounter = 0;
            projectile.damage = 0;
            projectile.velocity = Vector2.Zero;
        }
    }
}
