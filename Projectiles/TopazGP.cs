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
    public class TopazGP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Topaz Gemstone Power");
        }

        public override void SetDefaults() //Needs Setup and AI
        {
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.width = projectile.height = 56;
            projectile.maxPenetrate = -1;
        }

        public override void AI()
        {
            if (projectile.velocity.X > 0)
                projectile.direction = 1;
            else
                projectile.direction = -1;
            projectile.position += projectile.velocity;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(Terraria.ID.BuffID.Bleeding, 5 * 60);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(Terraria.ID.BuffID.Bleeding, 5 * 60);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 ProjPos = projectile.position + new Vector2(projectile.width, projectile.height) * 0.5f;
            spriteBatch.Draw(Main.projectileTexture[projectile.type], ProjPos, new Rectangle(0, 0, 58, 54), Color.White, 0f,
                new Vector2(48, 48), projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
    }
}
