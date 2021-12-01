using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Creatures.Zacks
{
    public class ZacksPullSomeoneAction : GuardianActions
    {
        public Player TargetPlayer;
        public TerraGuardian TargetGuardian;
        public bool TargetIsPlayer = false;

        public ZacksPullSomeoneAction(Player player)
        {
            TargetIsPlayer = true;
            TargetPlayer = player;
        }

        public ZacksPullSomeoneAction(TerraGuardian guardian)
        {
            TargetIsPlayer = false;
            TargetGuardian = guardian;
        }

        public Vector2 PullStartPosition = Vector2.Zero;
        public const int PullMaxTime = 45; //Like his miniboss version

        public override void Update(TerraGuardian Me)
        {
            if (Me.UsingFurniture)
                Me.LeaveFurniture(true);
            if (Time >= PullMaxTime)
            {
                if (Time == PullMaxTime)
                {
                    if (TargetIsPlayer)
                    {
                        PullStartPosition = TargetPlayer.Center;
                        if (TargetPlayer.GetModPlayer<PlayerMod>().BeingCarriedByGuardian)
                        {
                            InUse = false;
                            return;
                        }
                    }
                    else
                    {
                        PullStartPosition = TargetGuardian.CenterPosition;
                        if(TargetGuardian.BeingCarriedByGuardian)
                        {
                            InUse = false;
                            return;
                        }
                    }
                }
                if (TargetIsPlayer)
                {
                    Player player = TargetPlayer;
                    Vector2 MoveDirection = Me.CenterPosition - player.Center;
                    MoveDirection.Normalize();
                    Me.LookingLeft = player.Center.X < Me.Position.X;
                    player.velocity = Vector2.Zero;
                    player.position += MoveDirection * 8f;
                    player.fallStart = (int)player.position.Y / 16;
                    player.immuneTime = 5;
                    player.immuneNoBlink = true;
                    if (player.getRect().Intersects(Me.HitBox))
                    {
                        player.velocity = Vector2.Zero;
                        InUse = false;
                        Me.StartNewGuardianAction(new Actions.CarryDownedAlly(player));
                        return;
                    }
                }
                else
                {
                    TerraGuardian guardian = TargetGuardian;
                    Vector2 MoveDirection = Me.CenterPosition - guardian.CenterPosition;
                    MoveDirection.Normalize();
                    Me.LookingLeft = guardian.Position.X < Me.Position.X;
                    guardian.Velocity = Vector2.Zero;
                    guardian.Position += MoveDirection * 8f;
                    guardian.SetFallStart();
                    guardian.ImmuneTime = 5;
                    guardian.ImmuneNoBlink = true;
                    if (guardian.HitBox.Intersects(Me.HitBox))
                    {
                        guardian.Velocity = Vector2.Zero;
                        InUse = false;
                        Me.StartNewGuardianAction(new Actions.CarryDownedAlly(guardian));
                        return;
                    }
                }
            }
            else
            {
                if(Time == 30)
                {
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            Me.SaySomething("*Better you stay close to me.*");
                            break;
                        case 1:
                            Me.SaySomething("*Let's avoid another death.*");
                            break;
                        case 2:
                            Me.SaySomething("*Come here.*");
                            break;
                        case 3:
                            Me.SaySomething("*Get over here.*");
                            break;
                        case 4:
                            Me.SaySomething("*No, you wont.*");
                            break;
                    }
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            int HandFrame = 0;
            if (Time < 5)
            {
                HandFrame = 14;
            }
            else if (Time < 10)
            {
                HandFrame = 15;
            }
            else if (Time < 15)
            {
                HandFrame = 16;
            }
            else if (Time < 20)
            {
                HandFrame = 17;
            }
            if (Time >= PullMaxTime)
            {
                HandFrame = 15;
            }
            if (!UsingRightArmAnimation)
                guardian.RightArmAnimationFrame = HandFrame;
            else if (!UsingLeftArmAnimation)
                guardian.LeftArmAnimationFrame = HandFrame;
        }

        public override void Draw(TerraGuardian guardian)
        {
            Vector2 EndPosition = Vector2.Zero;
            if (TargetIsPlayer)
            {
                EndPosition = TargetPlayer.Center;
            }
            else
            {
                EndPosition = TargetGuardian.CenterPosition;
            }
            if (Time < PullMaxTime)
            {
                float Percentage = (float)Time / PullMaxTime;
                EndPosition = guardian.CenterPosition + (EndPosition - guardian.CenterPosition) * Percentage;
            }
            GuardianDrawData[] gdds = DrawIntestine(guardian, EndPosition);
            for (int gdd = 0; gdd < TerraGuardian.DrawFront.Count; gdd++)
            {
                if (TerraGuardian.DrawFront[gdd].textureType == GuardianDrawData.TextureType.TGLeftArm)
                {
                    TerraGuardian.DrawFront.InsertRange(gdd, gdds);
                    return;
                }
            }
            for (int gdd = 0; gdd < TerraGuardian.DrawBehind.Count; gdd++)
            {
                if (TerraGuardian.DrawBehind[gdd].textureType == GuardianDrawData.TextureType.TGLeftArm)
                {
                    TerraGuardian.DrawBehind.InsertRange(gdd, gdds);
                    return;
                }
            }
        }

        public GuardianDrawData[] DrawIntestine(TerraGuardian Guardian, Vector2 ChainEndPosition)
        {
            Vector2 ChainStartPosition = Guardian.CenterPosition;
            ChainStartPosition.X -= 8 * Guardian.Direction;
            ChainStartPosition.Y -= 8;
            float DifX = ChainStartPosition.X - ChainEndPosition.X, DifY = ChainStartPosition.Y - ChainEndPosition.Y;
            bool DrawMoreChain = true;
            float Rotation = (float)Math.Atan2(DifY, DifX) - 1.57f;
            List<GuardianDrawData> gdds = new List<GuardianDrawData>();
            while (DrawMoreChain)
            {
                float sqrt = (float)Math.Sqrt(DifX * DifX + DifY * DifY);
                if (sqrt < 40)
                    DrawMoreChain = false;
                else
                {
                    sqrt = (float)Main.chain12Texture.Height / sqrt;
                    DifX *= sqrt;
                    DifY *= sqrt;
                    ChainEndPosition.X += DifX;
                    ChainEndPosition.Y += DifY;
                    DifX = ChainStartPosition.X - ChainEndPosition.X;
                    DifY = ChainStartPosition.Y - ChainEndPosition.Y;
                    Microsoft.Xna.Framework.Color color = Lighting.GetColor((int)ChainEndPosition.X / 16, (int)ChainEndPosition.Y / 16);
                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Main.chain12Texture, ChainEndPosition - Main.screenPosition, null, color, Rotation, new Vector2(Main.chain12Texture.Width * 0.5f, Main.chain12Texture.Height * 0.5f), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                    gdds.Add(gdd);
                    //Main.spriteBatch.Draw(Main.chain12Texture, ChainEndPosition - Main.screenPosition, null, color, Rotation, new Vector2(Main.chain12Texture.Width * 0.5f, Main.chain12Texture.Height * 0.5f), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                }
            }
            return gdds.ToArray();
        }
    }
}
