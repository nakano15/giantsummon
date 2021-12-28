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
    public class ZombieFart : ModProjectile
    {
        private Vector2 ScaledPosition
        {
            get
            {
                Vector2 Pos = projectile.position;
                Pos.X -= projectile.width * (projectile.scale - 1f) * 0.5f;
                Pos.Y -= projectile.height * (projectile.scale - 1f) * 0.5f;
                return Pos;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zombie Fart");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0f;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.velocity.Y -= 0.006f;
            if (projectile.velocity.Y < -0.65f)
                projectile.velocity.Y = -0.65f;
            projectile.scale += 0.23f * (Main.expertMode ? 1.5f : 1);

            for (int p = 0; p < 255; p++)
            {
                if (!Main.player[p].active)
                {
                    continue;
                }
                if (!Main.player[p].dead && !Main.player[p].ghost)
                {
                    Vector2 NosePosition = Main.player[p].Center;
                    NosePosition.Y -= Main.player[p].height * 0.25f;
                    if (NosePosition.X >= ScaledPosition.X && NosePosition.X < ScaledPosition.X + projectile.width * projectile.scale &&
                        NosePosition.Y >= ScaledPosition.Y && NosePosition.Y < ScaledPosition.Y + projectile.height * projectile.scale)
                    {
                        Main.player[p].AddBuff(Terraria.ID.BuffID.Suffocation, 5);
                        Main.player[p].AddBuff(Terraria.ID.BuffID.Weak, (Main.expertMode ? 75 : 15) * 60);
                        Main.player[p].AddBuff(Terraria.ID.BuffID.Slow, 5);
                        Main.player[p].AddBuff(Terraria.ID.BuffID.Stinky, 60 * 10);
                        if (Main.expertMode)
                            Main.player[p].AddBuff(Terraria.ID.BuffID.Poisoned, 60);
                    }
                }
            }
            foreach (int g in MainMod.ActiveGuardians.Keys)
            {
                TerraGuardian Guardian = MainMod.ActiveGuardians[g];
                if (Guardian.Active && !Guardian.Downed)
                {
                    Vector2 NosePosition = Guardian.Position;
                    NosePosition.Y -= Guardian.Height * 0.75f;
                    if (NosePosition.X >= ScaledPosition.X && NosePosition.X < ScaledPosition.X + projectile.width * projectile.scale &&
                        NosePosition.Y >= ScaledPosition.Y && NosePosition.Y < ScaledPosition.Y + projectile.height * projectile.scale)
                    {
                        Guardian.AddBuff(BuffID.Suffocation, 5, true);
                        Guardian.AddBuff(BuffID.Weak, (Main.expertMode ? 75 : 15) * 60, true);
                        Guardian.AddBuff(BuffID.Slow, 5, true);
                        Guardian.AddBuff(BuffID.Stinky, 60 * 10, true);
                        if (Main.expertMode)
                            Guardian.AddBuff(BuffID.Poisoned, 60, true);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int g = 0; g < 4; g++)
            {
                Vector2 GorePos = ScaledPosition;
                GorePos.X += Main.rand.Next((int)(projectile.width * projectile.scale));
                GorePos.Y += Main.rand.Next((int)(projectile.height * projectile.scale));
                float VelX = Main.rand.Next(-100, 101),
                    VelY = Main.rand.Next(-100, 101);
                Dust dust = Main.dust[Dust.NewDust(GorePos, projectile.width, projectile.height, 4, VelX * 0.02f, VelY * 0.02f, 100, new Color(), 1f)];
                dust.scale = Main.rand.Next(50, 166) * 0.01f;
                dust.noLight = true;
                dust.noGravity = true;
                dust.color = new Color(80, 223, 40, 66);
                //Gore.NewGore(GorePos, new Vector2(VelX, VelY) * 0.02f, Main.rand.Next(11, 14), Main.rand.Next(75, 121) * 0.01f);
            }
            /*Rectangle DebugTileDimension = Rectangle.Empty;
            DebugTileDimension.X = (int)(ScaledPosition.X - Main.screenPosition.X);
            DebugTileDimension.Y = (int)(ScaledPosition.Y - Main.screenPosition.Y);
            DebugTileDimension.Width = (int)(projectile.width * projectile.scale);
            DebugTileDimension.Height = (int)(projectile.height * projectile.scale);
            spriteBatch.Draw(Main.blackTileTexture, DebugTileDimension, null, Color.DarkSeaGreen);*/
            return true;
        }
    }
}
