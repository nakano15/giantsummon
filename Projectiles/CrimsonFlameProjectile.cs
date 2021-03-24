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
                const int FrameDuration = 6;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
                projectile.position += projectile.velocity;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 10 * FrameDuration)
                    projectile.frameCounter -= 10 * FrameDuration;
                projectile.frame = projectile.frameCounter / FrameDuration;
            }
            else
            {
                const int FrameDuration = 6;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 5 * FrameDuration)
                    projectile.active = false;
                else
                    projectile.frame = projectile.frameCounter / FrameDuration + 10;
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
			if(Impact)
				return;
            Impact = true;
            projectile.frame = 0;
            projectile.frameCounter = 0;
            //TODO - Add AOE damage effect.
            const float AoeDistance = 70f;
            for(int n = 0; n < 200; n++)
            {
                if(Main.npc[n].active && Main.npc[n].friendly != projectile.hostile && Main.npc[n].CanBeChasedBy(null) && Main.npc[n].Distance(projectile.Center) < AoeDistance)
                {
                    Main.npc[n].StrikeNPC(projectile.damage, 0.8f, 0);
                }
            }
            projectile.damage = 0;
            projectile.velocity = Vector2.Zero;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position + new Vector2(32, 32) - Main.screenPosition, 
                new Rectangle(projectile.frame * 64, 0, 64, 64), Color.White, 
                projectile.rotation, new Vector2(32, 32), projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
