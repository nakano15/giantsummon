using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures.Alex
{
    public class HealingLickAction : GuardianActions
    {
        public Player TargetPlayer;
        public TerraGuardian TargetGuardian;
        public bool TargetIsPlayer = false;

        public HealingLickAction(Player HealPlayer)
        {
            this.TargetPlayer = HealPlayer;
            TargetIsPlayer = true;
            this.IsGuardianSpecificAction = true;
            ID = AlexBase.HealingLickAction;
        }

        public HealingLickAction(TerraGuardian HealGuardian)
        {
            this.TargetGuardian = HealGuardian;
            TargetIsPlayer = false;
            this.IsGuardianSpecificAction = true;
            ID = AlexBase.HealingLickAction;
        }

        public override void Update(TerraGuardian guardian)
        {
            AvoidItemUsage = true;
            switch (Step)
            {
                case 0: //Reach Target
                    Vector2 TargetPosition = Vector2.Zero;
                    int TargetWidth = 0, TargetHeight = 0;
                    IgnoreCombat = true;
                    if (TargetIsPlayer)
                    {
                        TargetPosition = TargetPlayer.position;
                        TargetWidth = TargetPlayer.width;
                        TargetHeight = TargetPlayer.height;
                    }
                    else
                    {
                        TargetPosition = TargetGuardian.TopLeftPosition;
                        TargetWidth = TargetGuardian.Width;
                        TargetHeight = TargetGuardian.Height;
                    }
                    if (guardian.HitBox.Intersects(new Rectangle((int)TargetPosition.X, (int)TargetPosition.Y, TargetWidth, TargetHeight)))
                    {
                        ChangeStep();
                    }
                    else
                    {
                        guardian.MoveLeft = guardian.MoveRight = false;
                        if (TargetPosition.X + TargetWidth * 0.5f - guardian.Position.X < 0)
                        {
                            guardian.MoveLeft = true;
                        }
                        else
                        {
                            guardian.MoveRight = true;
                        }
                        if (Time >= 5 * 60)
                        {
                            InUse = false;
                            guardian.DisplayEmotion(TerraGuardian.Emotions.Sad);
                        }
                    }
                    break;

                case 1:
                    {
                        guardian.MoveLeft = guardian.MoveRight = false;
                        NoAggro = AvoidItemUsage = Immune = true;
                        Vector2 EffectPosition = new Vector2(22 * guardian.Scale * guardian.Direction, 0) + guardian.Position;
                        bool AnimationTrigger = Time == 120 + 8 * 2 || Time == 120 + 8 * 5 + 8 * 2 || Time == 120 + 8 * 10 + 8 * 2;
                        bool RestoreCharacters = false;
                        if (Time >= 120 + 8 * 15)
                        {
                            RestoreCharacters = true;
                            InUse = false;
                        }
                        if (TargetIsPlayer)
                        {
                            Player p = TargetPlayer;
                            if (AnimationTrigger)
                            {
                                int HealthRestored = (int)(p.statLifeMax2 * 0.1f);
                                p.statLife += HealthRestored;
                                p.HealEffect(HealthRestored);
                            }
                            if (RestoreCharacters)
                            {
                                p.position = guardian.Position;
                                p.position.X -= p.width * 0.5f;
                                p.position.Y -= p.height;
                                p.fullRotation = 0;
                            }
                            else
                            {
                                p.Center = EffectPosition;
                                p.direction = -guardian.Direction;
                                p.fullRotation = 1.570796326794897f * guardian.Direction;
                                if (p.immuneTime < 5)
                                    p.immuneTime = 5;
                            }
                        }
                        else
                        {
                            TerraGuardian g = TargetGuardian;
                            if (AnimationTrigger)
                            {
                                g.RestoreHP((int)(g.MHP * 0.1f));
                            }
                            if (RestoreCharacters)
                            {
                                g.Position = guardian.Position;
                                g.Rotation = 0;
                            }
                            else
                            {
                                g.Position = EffectPosition;
                                g.Position.Y += g.Height * 0.5f;
                                g.Direction = -g.Direction;
                                g.Rotation = 1.570796326794897f * guardian.Direction;
                                if (g.ImmuneTime < 5)
                                    g.ImmuneTime = 5;
                            }
                        }
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            switch (Step)
            {
                case 0:
                    break;
                case 1:
                    if (Time < 120)
                    {
                        int Animation = (Time / 5) % 4;
                        if (Animation == 3)
                            Animation = 1;
                        guardian.BodyAnimationFrame = 19 + Animation;
                        guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame;
                        UsingLeftArm = UsingRightArm = true;
                    }
                    else
                    {
                        int Animation = (Time / 8) % 5;
                        guardian.BodyAnimationFrame = 24 + Animation;
                        guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame;
                        UsingLeftArm = UsingRightArm = true;
                    }
                    break;
            }
        }
    }
}
