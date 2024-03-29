﻿using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Companions.Creatures.Bapha
{
    public class HellFlameAttack : GuardianSpecialAttack
    {
        private byte FrameID = 0;
        protected bool IsAwakenedVersion = false;

        public HellFlameAttack()
        {
            Name = "Hell Flame";
            CanMove = true;
            AttackType = SubAttackCombatType.Magic;
            ManaCost = 15;
        }

        public override bool CanUse(TerraGuardian tg)
        {
            return BaphaBase.IsAwakened(tg) == IsAwakenedVersion;
        }

        public int GetAttackDamage(TerraGuardian tg)
        {
            int Damage = (IsAwakenedVersion ? 40 : 20);
            if (tg.SelectedItem > -1)
                Damage += (int)(tg.Inventory[tg.SelectedItem].damage * (IsAwakenedVersion ? 1.5f : 1.1f));
            Damage = (int)(Damage * tg.MagicDamageMultiplier);
            return Damage;
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            tg.ShowOffHand = false;
            if(data.Step < 4)
            {
                Lighting.AddLight(tg.CenterPosition, Vector3.One);
            }
            if (data.FirstFrame)
            {
                if (data.Step == 4)
                {
                    FrameID = 0;
                    //
                    Vector2 AimDirectionChange = tg.AimPosition;
                    tg.LookingLeft = tg.AimDirection.X < 0;
                    float AngleChecker = MathHelper.WrapAngle((float)Math.Atan2((tg.CenterY - AimDirectionChange.Y) * tg.GravityDirection, tg.Position.X - AimDirectionChange.X));
                    float ArmAngle = tg.CalculateAimingUseAnimation(AngleChecker);
                    if (ArmAngle < -0.75f)
                        FrameID = 0;
                    else if (ArmAngle > 0.6f)
                        FrameID = 3;
                    else if (ArmAngle >= -0.2f)
                        FrameID = 1;
                    else
                        FrameID = 2;
                    //
                    int BackupLArmFrame = tg.LeftArmAnimationFrame, BackupRArmFrame = tg.RightArmAnimationFrame;
                    for (byte i = 0; i < 2; i++)
                    {
                        if ((i == 0 && TerraGuardian.UsingLeftArmAnimation) || (i == 1 && TerraGuardian.UsingRightArmAnimation))
                            continue;
                        if (i == 0) tg.LeftArmAnimationFrame = 17 + FrameID;
                        else tg.RightArmAnimationFrame = 17 + FrameID;
                        Vector2 ProjectileSpawnPosition = (i == 0 ? tg.GetGuardianLeftHandPosition : tg.GetGuardianRightHandPosition);
                        if (i == 0) tg.LeftArmAnimationFrame = BackupLArmFrame;
                        else tg.RightArmAnimationFrame = BackupRArmFrame;
                        Vector2 ShotDirection = tg.AimPosition - ProjectileSpawnPosition;
                        ShotDirection.Normalize();
                        ShotDirection *= 8f;
                        int Damage = GetAttackDamage(tg);
                        int Proj = IsAwakenedVersion ? Terraria.ModLoader.ModContent.ProjectileType<Projectiles.CrimsonFlameProjectile>() : Terraria.ModLoader.ModContent.ProjectileType<Projectiles.HellFlame>();
                        int resultproj = Projectile.NewProjectile(ProjectileSpawnPosition, ShotDirection,
                            Proj,
                            Damage, 6, (tg.OwnerPos > -1 ? tg.OwnerPos : Main.myPlayer));
                        tg.SetProjectileOwnership(resultproj);
                        Main.projectile[resultproj].scale = tg.Scale;
                    }
                }
            }
            if(data.Time >= 6)
            {
                if (data.Step >= 4)
                {
                    data.EndUse();
                    return;
                }
                data.ChangeStep(TimeToDiscount:6);                
            }
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if(data.Step < 3)
            {
                if (!UsingLeftArm) tg.LeftArmAnimationFrame = data.Step + 14;
                if(!UsingRightArm)tg.RightArmAnimationFrame = data.Step + 14;
            }
            else
            {
                int Frame = 17 + FrameID;
                if (!UsingLeftArm) tg.LeftArmAnimationFrame = Frame;
                if (!UsingRightArm) tg.RightArmAnimationFrame = Frame;
            }
        }
    }
}
