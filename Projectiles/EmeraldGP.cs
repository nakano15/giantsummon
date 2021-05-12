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
    public class EmeraldGP : ModProjectile
    {
        private byte Frame = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emerald Gemstone Power");
        }

        public override void SetDefaults() //Needs Setup and AI
        {
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 300;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.penetrate = projectile.maxPenetrate = 20;
            projectile.width = 76;
            projectile.height = 76;
        }

        public override void AI()
        {
            projectile.position += projectile.velocity;
            Frame++;
            if(Frame >= 10 * 3)
            {
                Frame -= 10 * 3;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int FrameX = (int)(Frame * 0.3334f);
            Vector2 Position = projectile.position + new Vector2(projectile.width, projectile.height) * 0.5f;
            Color color = Lighting.GetColor((int)(Position.X * (1f / 16)), (int)(Position.Y * (1f / 16)));
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition, new Rectangle(FrameX * 76, 0, 76, 80), color, 0f,
                new Vector2(38, 40), projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
