using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Companions.CaptainStench.Attacks
{
    public class ArmBlaster : GuardianSpecialAttack
    {
        public ArmBlaster()
        {
            Name = "Arm Blaster";
            AttackType = SubAttackCombatType.Ranged;
            CanMove = true;
            ManaCost = 2;
            MinRange = 0;
            MaxRange = 1000;
        }

        public int GetDamage(TerraGuardian tg)
        {
            int Damage = 10;
            if (tg.SelectedItem > -1)
            {
                float Dps = (float)tg.Inventory[tg.SelectedItem].useAnimation / (float)tg.Inventory[tg.SelectedItem].useTime;
                Damage += (int)(tg.Inventory[tg.SelectedItem].damage * Dps * 0.5f);
            }
            return (int)(Damage * tg.RangedDamageMultiplier);
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            if (data.FirstFrame)
            {
                Vector2 ProjectilePosition = Vector2.Zero;
                Vector2 AimPosition = tg.AimDirection;
                float Angle = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(AimPosition.Y, AimPosition.X)));
                if (AimPosition.X < 0)
                    Angle = (float)Math.PI - Angle;
                int LeftArmFrame = 57;
                if (Angle < 30 * 0.017453f) //Middle
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 61;
                }
                else if (AimPosition.Y > 0) //Down
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 60;
                    else
                        LeftArmFrame = 58;
                }
                else //Up
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 62;
                    else
                        LeftArmFrame = 59;
                }
                switch (LeftArmFrame)
                {
                    case 57:
                        ProjectilePosition = new Vector2(46, 43);
                        break;
                    case 58:
                        ProjectilePosition = new Vector2(46, 47);
                        break;
                    case 59:
                        ProjectilePosition = new Vector2(48, 34);
                        break;
                    case 60:
                        ProjectilePosition = new Vector2(42, 43);
                        break;
                    case 61:
                        ProjectilePosition = new Vector2(46, 37);
                        break;
                    case 62:
                        ProjectilePosition = new Vector2(45, 30);
                        break;
                }
                ProjectilePosition *= 2;
                ProjectilePosition.X = ProjectilePosition.X - tg.Base.SpriteWidth * 0.5f;
                if (tg.LookingLeft)
                    ProjectilePosition.X *= -1;
                ProjectilePosition.Y = -tg.Base.SpriteHeight + ProjectilePosition.Y;
                ProjectilePosition = tg.PositionWithOffset + ProjectilePosition * tg.Scale;
                AimPosition.Normalize();
                for (int i = 0; i < 4; i++)
                    Dust.NewDust(ProjectilePosition, 2, 2, 132, AimPosition.X, AimPosition.Y);
                int Damage = GetDamage(tg);
                int ID = Projectile.NewProjectile(ProjectilePosition, AimPosition * 14f, Terraria.ModLoader.ModContent.ProjectileType<Projectiles.CannonBlast>(),
                    Damage, 0.06f, tg.GetSomeoneToSpawnProjectileFor);
                //Main.PlaySound(Terraria.ModLoader.SoundLoader.customSoundType, tg.CenterPosition, Terraria.ModLoader.SoundLoader.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Creatures/CaptainStench/Sounds/BlasterSound"));
                Main.PlaySound(2, tg.CenterPosition, 20);
                Main.projectile[ID].scale = tg.Scale;
                Main.projectile[ID].netUpdate = true;
                tg.SetProjectileOwnership(ID);
            }
            if (data.Time >= 8)
                data.EndUse();
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            Vector2 AimPosition = tg.AimDirection;
            float Angle = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(AimPosition.Y, AimPosition.X)));
            if (AimPosition.X < 0)
                Angle = (float)Math.PI - Angle;
            tg.LeftArmAnimationFrame = 57;
            if (Angle < 30 * 0.017453f) //Middle
            {
                if (tg.Velocity.Y != 0)
                    tg.LeftArmAnimationFrame = 61;
            }
            else if (AimPosition.Y > 0) //Down
            {
                if (tg.Velocity.Y != 0)
                    tg.LeftArmAnimationFrame = 60;
                else
                    tg.LeftArmAnimationFrame = 58;
            }
            else //Up
            {
                if (tg.Velocity.Y != 0)
                    tg.LeftArmAnimationFrame = 62;
                else
                    tg.LeftArmAnimationFrame = 59;
            }
        }
    }
}
