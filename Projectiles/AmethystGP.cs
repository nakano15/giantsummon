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
        public const int LifeTime = 30;
        public const float LifeTimeFrameDecimal = 1f / LifeTime;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amethyst Gemstone Power");
        }

        public override void SetDefaults() //Needs Setup and AI
        {
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = LifeTime;
            projectile.alpha = 255;
            projectile.light = 1.1f;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.width = 40;
            projectile.height = 74;
            projectile.maxPenetrate = projectile.penetrate = -1;
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
            projectile.velocity.X *= 0.95f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int FrameX = (int)(10 - 10 * projectile.timeLeft * LifeTimeFrameDecimal);
            Vector2 ProjPos = projectile.position + new Vector2(projectile.width, projectile.height) * 0.5f;
            Lighting.AddLight(ProjPos, 0.165f, 0, 0.165f);
            spriteBatch.Draw(Main.projectileTexture[projectile.type], ProjPos - Main.screenPosition, new Rectangle(FrameX * 96, 0, 96, 96), Color.White, 0f, 
                new Vector2(48, 48), projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
    }
}
