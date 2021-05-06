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
    public class SapphireGP : ModProjectile
    {
        public bool Detonated = false;
        public byte Frame = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sapphire Gemstone Power");
        }

        public override void SetDefaults() //Needs Setup and AI
        {
            projectile.width = 42;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (!Detonated)
            {
                if (projectile.velocity.X > 0)
                    projectile.direction = 1;
                else
                    projectile.direction = -1;
                projectile.position += projectile.velocity;
                if (OnTileCollide(projectile.velocity))
                {
                    Detonate();
                }
                else
                {
                    Frame++;
                    if (Frame >= 8 * 3)
                        Frame -= 8 * 3;
                }
            }
            else
            {
                Frame++;
                if (Frame >=  (9 + 11) * 3)
                    projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Detonate();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Detonate();
        }

        public void Detonate()
        {
            if (Detonated)
                return;
            this.Detonated = true;
            projectile.velocity = Vector2.Zero;
            Frame = 9;
            projectile.timeLeft = 180;
            if (ProjMod.IsGuardianProjectile(projectile.whoAmI))
            {
                TerraGuardian tg = ProjMod.GuardianProj[projectile.whoAmI];
                Rectangle ExplosionRange = new Rectangle((int)projectile.position.X - 48, (int)projectile.position.Y - 48, 96, 96);
                for (int t = 0; t < 255; t++)
                {
                    if(Main.player[t].active && tg.IsPlayerHostile(Main.player[t]) && Main.player[t].getRect().Intersects(ExplosionRange))
                    {
                        double dmg = Main.player[t].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" didn't survived a electric explosion."), projectile.damage, projectile.direction, true);
                        if(dmg > 0) Main.player[t].AddBuff(Terraria.ID.BuffID.Electrified, 5 * 60);
                    }
                    if(t < 200 && Main.npc[t].active && tg.IsNpcHostile(Main.npc[t]) && Main.npc[t].getRect().Intersects(ExplosionRange))
                    {
                        double dmg = Main.npc[t].StrikeNPC(projectile.damage, projectile.knockBack, projectile.direction);
                        if (dmg > 0) Main.npc[t].AddBuff(Terraria.ID.BuffID.Electrified, 5 * 60);
                    }
                }
                foreach(TerraGuardian othertg in MainMod.ActiveGuardians.Values)
                {
                    if(tg.IsGuardianHostile(othertg) && othertg.HitBox.Intersects(ExplosionRange))
                    {
                        int dmg = othertg.Hurt(projectile.damage, projectile.direction, false, false, " didn't survived a electric explosion.");
                        if (dmg > 0)
                            othertg.AddBuff(Terraria.ID.BuffID.Electrified, 5 * 60);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int FrameX = (int)(Frame * 0.3334f), FrameY = 0;
            if(FrameX > 18)
            {
                FrameY += (int)(FrameX * 0.05263157894736842105263157894737f);
                FrameX -= FrameY * 19;
            }
            Vector2 ProjPos = projectile.position + new Vector2(projectile.width, projectile.height) * 0.5f - Main.screenPosition;
            spriteBatch.Draw(Main.projectileTexture[projectile.type], ProjPos, new Rectangle(FrameX * 96, FrameY * 96, 96, 96), Color.White, 0f,
                new Vector2(48, 48), projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
    }
}
