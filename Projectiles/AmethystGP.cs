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
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 90;
            projectile.alpha = 255;
            projectile.light = 1.1f;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.width = 40;
            projectile.height = 74;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(Terraria.ID.BuffID.ShadowFlame, 5 * 60);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(Terraria.ID.BuffID.ShadowFlame, 5 * 60);
        }

        public override void AI()
        {
            if (projectile.velocity.X > 0)
                projectile.direction = 1;
            else
                projectile.direction = -1;
            projectile.position += projectile.velocity;
            projectile.velocity.X *= 0.9f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int FrameX = (int)(10 - projectile.timeLeft * 0.9f);
            Vector2 ProjPos = projectile.position + new Vector2(projectile.width, projectile.height) * 0.5f;
            spriteBatch.Draw(Main.projectileTexture[projectile.type], ProjPos - Main.screenPosition, new Rectangle(FrameX * 96, 0, 96, 96), Color.White, 0f, 
                new Vector2(48, 48), projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
    }
}
