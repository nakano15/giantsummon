using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace giantsummon.Actions
{
    public class ResurrectPlayerAction : GuardianActions
    {
        public Player Target;

        public ResurrectPlayerAction(Player player)
        {
            ID = (int)ActionIDs.ResurrectPlayer;
            Target = player;
            InUse = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.UsingFurniture)
                guardian.LeaveFurniture(true);
            AvoidItemUsage = true;
            IgnoreCombat = true;
            Vector2 CastingPosition = guardian.CenterPosition;
            CastingPosition.Y -= guardian.Height;
            Target.respawnTimer++;
            guardian.Ducking = false;
            float Percentage = (float)Time / 150;
            if (Target.dead)
            {
                const float MoveSpeed = 0.3f;
                Vector2 Velocity = (CastingPosition - Target.Center) * 0.1f;
                if (Velocity.Length() > 2f)
                {
                    Velocity.Normalize();
                    Velocity *= 2f;
                }
                Target.Center += Velocity;
                Target.immuneAlpha = (int)(255 * (1f - Percentage));
                for (int i = 0; i < 3; i++)
                {
                    float EstimatedDistance = 150 * (1f - Percentage);
                    float Rotation = MathHelper.ToRadians(Time + i * 120);
                    Vector2 EndPosition = new Vector2((float)Math.Sin(Rotation), (float)Math.Cos(Rotation)) * EstimatedDistance;
                    switch (i)
                    {
                        case 0:
                            Target.headPosition += (EndPosition - Target.headPosition) * MoveSpeed;
                            break;
                        case 1:
                            Target.bodyPosition += (EndPosition - Target.bodyPosition) * MoveSpeed;
                            break;
                        case 2:
                            Target.legPosition += (EndPosition - Target.legPosition) * MoveSpeed;
                            break;
                    }
                }
                //target.headPosition += (CastingPosition - target.headPosition) * Percentage;
                //target.bodyPosition += (CastingPosition - target.bodyPosition) * Percentage;
                //target.legPosition += (CastingPosition - target.legPosition) * Percentage;
            }
            if (Time >= 150) //Res
            {
                if (Time == 150)
                {
                    if (Target.dead || Target.ghost)
                    {
                        Target.ghost = false;
                        Target.respawnTimer = 0;
                        //if(target.difficulty == 2)
                        //    Main.ActivePlayerFileData.
                        Target.Spawn();
                        //target.statLife = (int)(target.statLifeMax2 * 0.5f);
                        Target.immuneTime *= 2;
                        //Add cooldown.
                        Main.NewText(guardian.Name + " has resurrected " + Target.name + ".", 0, 255, 0);
                    }
                }
                else if (Time == 151)
                {
                    Target.velocity.X = 0f;
                    Target.velocity.Y = -7.25f;
                    Target.Center = CastingPosition;
                    Target.fallStart = (int)guardian.Position.Y / 16;
                    InUse = false;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int d = Dust.NewDust(CastingPosition, Target.width, Target.height, 5, 0, 0, 175, default(Color), 1.75f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.75f;
                    int XChange = Main.rand.Next(-40, 41), YChange = Main.rand.Next(-40, 41);
                    Main.dust[d].position.X += XChange;
                    Main.dust[d].position.Y += YChange;
                    Main.dust[d].velocity.X -= XChange * 0.075f;
                    Main.dust[d].velocity.Y -= YChange * 0.075f;
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.Base.JumpFrame;
            UsingLeftArmAnimation = UsingRightArmAnimation = true;
        }
    }
}
