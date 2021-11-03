using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures.Bapha
{
    public class FireballAttack : GuardianSpecialAttack
    {
        public FireballAttack()
        {
            Name = "Fireball";
            CanMove = false;
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
                if (data.Step == 5) //= frame 15
                {
                    Vector2 ProjectileSpawnPosition = tg.GetGuardianLeftHandPosition;
                    Vector2 ShotDirection = new Vector2(tg.AimDirection.X, tg.AimDirection.Y) - ProjectileSpawnPosition;
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
                if (data.Step >= 6)
                {
                    data.EndUse();
                    return;
                }
                data.ChangeStep(TimeToDiscount:6);                
            }
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if(data.Step < 5)
            {
                tg.BodyAnimationFrame = tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = data.Step + 10;
            }
            else
            {
                Vector2 PosDif = tg.AimDirection.ToVector2() - tg.CenterPosition;
                float RotationValue = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(PosDif.Y, PosDif.X)));
                if (RotationValue < 0.0174533f * 25f)
                {
                    tg.BodyAnimationFrame = 15;
                }
                else if (RotationValue < 0.0174533f * 65f)
                {
                    tg.BodyAnimationFrame = 16;
                }
                else if (RotationValue < 0.0174533f * 120)
                {
                    tg.BodyAnimationFrame = 17;
                }
                else
                {
                    tg.BodyAnimationFrame = 18;
                }
                tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = tg.BodyAnimationFrame;
            }
        }
    }
}
