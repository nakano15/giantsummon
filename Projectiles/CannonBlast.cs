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
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cannon Blast");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
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
            projectile.rotation = (int)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition, null, Color.White, projectile.rotation,
                new Vector2(5, 5), projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
