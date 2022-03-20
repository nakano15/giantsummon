using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Companions.Bapha
{
    public class FireballAttack : GuardianSpecialAttack
    {
        private byte FrameID = 0;

        public FireballAttack()
        {
            Name = "Fireball";
            CanMove = true;
            AttackType = SubAttackCombatType.Magic;
        }

        public int GetAttackDamage(TerraGuardian tg)
        {
            int Damage = 20;
            if (tg.SelectedItem > -1)
                Damage += (int)(tg.Inventory[tg.SelectedItem].damage * 0.75f);
            Damage = (int)(Damage * tg.MagicDamageMultiplier);
            return Damage;
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            if (data.FirstFrame)
            {
                if (data.Step == 4)
                {
                    FrameID = 0;
                    //
                    Vector2 AimDirectionChange = tg.AimDirection;
                    tg.LookingLeft = tg.Position.X - tg.AimDirection.X >= 0;
                    float AngleChecker = MathHelper.WrapAngle((float)Math.Atan2((tg.CenterY - AimDirectionChange.Y) * tg.GravityDirection, tg.Position.X - AimDirectionChange.X));
                    float ArmAngle = tg.CalculateAimingUseAnimation(AngleChecker);
                    if (ArmAngle < -0.75f)
                        FrameID = 0;
                    else if (ArmAngle > 0.6f)
                        FrameID = 2;
                    else
                        FrameID = 1;
                    //
                    int BackupFrame = tg.LeftArmAnimationFrame;
                    tg.LeftArmAnimationFrame = 17 + FrameID;
                    Vector2 ProjectileSpawnPosition = tg.GetGuardianLeftHandPosition;
                    tg.LeftArmAnimationFrame = BackupFrame;
                    Vector2 ShotDirection = tg.AimDirection - ProjectileSpawnPosition;
                    ShotDirection.Normalize();
                    ShotDirection *= 8f;
                    int Damage = GetAttackDamage(tg);
                    int resultproj = Projectile.NewProjectile(ProjectileSpawnPosition, ShotDirection,
                        Terraria.ModLoader.ModContent.ProjectileType<Projectiles.CrimsonFlameProjectile>(),
                        Damage, 1.2f, (tg.OwnerPos > -1 ? tg.OwnerPos : Main.myPlayer));
                    tg.SetProjectileOwnership(resultproj);
                    Main.projectile[resultproj].scale = tg.Scale;
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
                /*tg.BodyAnimationFrame = */tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = data.Step + 14;
            }
            else
            {
                //Vector2 PosDif = tg.AimDirection - tg.CenterPosition;
                //float RotationValue = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(PosDif.Y, PosDif.X)));
                int Frame = 17 + FrameID;
                /*if (RotationValue < 0.0174533f * 25f)
                {
                    Frame = 17;
                }
                else if (RotationValue < 0.0174533f * 65f)
                {
                    Frame = 18;
                }
                else if (RotationValue < 0.0174533f * 120)
                {
                    Frame = 19;
                }
                else
                {
                    Frame = 20;
                }*/
                tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = Frame;
            }
        }
    }
}
