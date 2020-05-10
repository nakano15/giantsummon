using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class ModTargetting
    {
        public bool TargettingPlayer = true;
        public int TargetPosition = 0;
        public byte AssistSlot = 0;

        public TerraGuardian Guardian { get { return Main.player[TargetPosition].GetModPlayer<PlayerMod>().GetGuardianFromSlot(AssistSlot); } }
        public Player Character { get { return Main.player[TargetPosition]; } }

        public Rectangle GetCollision
        {
            get
            {
                if (TargettingPlayer)
                    return Character.Hitbox;
                else
                    return Guardian.HitBox;
            }
        }

        public Vector2 Position
        {
            get
            {
                if (TargettingPlayer)
                {
                    return Character.position;
                }
                return Guardian.TopLeftPosition;
            }
            set
            {
                if (TargettingPlayer)
                    Character.position = value;
                else
                {
                    Guardian.Position = value;
                    Guardian.Position.X += Guardian.Width * 0.5f;
                    Guardian.Position.Y += Guardian.Height;
                }
            }
        }
        public Vector2 Center
        {
            get
            {
                if (TargettingPlayer)
                    return Character.Center;
                return Guardian.CenterPosition;
            }
            set
            {
                if (TargettingPlayer)
                {
                    Character.Center = value;
                }
                else
                {
                    Vector2 NewPos = value;
                    NewPos.Y += Height * 0.5f;
                    Guardian.Position = NewPos;
                }
            }
        }
        public Vector2 Velocity
        {
            get
            {
                if (TargettingPlayer)
                    return Character.velocity;
                return Guardian.Velocity;
            }
            set
            {
                if (TargettingPlayer)
                    Character.velocity = value;
                else
                    Guardian.Velocity = value;
            }
        }
        public int Width
        {
            get
            {
                if (TargettingPlayer)
                    return Character.width;
                return Guardian.Width;
            }
        }
        public int Height
        {
            get
            {
                if (TargettingPlayer)
                    return Character.height;
                return Guardian.Height;
            }
        }
        public int Direction
        {
            get
            {
                if (TargettingPlayer)
                    return Character.direction;
                return Guardian.Direction;
            }
            set
            {
                if (TargettingPlayer)
                    Character.direction = value;
                else
                    Guardian.LookingLeft = value == -1;
            }
        }
        public int ImmuneTime
        {
            get
            {
                if (TargettingPlayer)
                    return Character.immuneTime;
                return Guardian.ImmuneTime;
            }
            set
            {
                if (TargettingPlayer)
                    Character.immuneTime = value;
                else
                    Guardian.ImmuneTime = value;
            }
        }
        public int Life
        {
            get
            {
                if (TargettingPlayer)
                    return Character.statLife;
                return Guardian.HP;
            }
            set
            {
                if (TargettingPlayer)
                    Character.statLife = value;
                else
                    Guardian.HP = value;
            }
        }
        public int MaxLife
        {
            get
            {
                if (TargettingPlayer)
                    return Character.statLifeMax2;
                return Guardian.MHP;
            }
        }
        public int MaxHealthBonus
        {
            get
            {
                if (TargettingPlayer)
                    return Character.statLifeMax;
                return Guardian.Base.InitialMHP + Guardian.LifeCrystalHealth * Guardian.Base.LifeCrystalHPBonus + Guardian.LifeFruitHealth * Guardian.Base.LifeFruitHPBonus;
            }
        }
        public int Defense
        {
            get
            {
                if (TargettingPlayer)
                    return Character.statDefense;
                return Guardian.Defense;
            }
            set
            {
                if (TargettingPlayer)
                    Character.statDefense = value;
                else
                    Guardian.Defense = value;
            }
        }
        public float DefenseRate
        {
            get
            {
                if (TargettingPlayer)
                    return 0;
                return Guardian.DefenseRate;
            }
            set
            {
                if (!TargettingPlayer)
                    Guardian.DefenseRate = value;
            }
        }
        public int FallStart
        {
            get
            {
                if (TargettingPlayer)
                    return Character.fallStart;
                return Guardian.FallStart;
            }
            set
            {
                if (TargettingPlayer)
                    Character.fallStart = value;
                else
                    Guardian.FallStart = value;
            }
        }
        public int ItemAnimation
        {
            get
            {
                if (TargettingPlayer)
                    return Character.itemAnimation;
                return Guardian.ItemAnimationTime;
            }
        }
        public bool IsActive
        {
            get
            {
                if (TargettingPlayer)
                    return Character.active;
                return Guardian.Active;
            }
        }
        public bool IsDefeated
        {
            get
            {
                if (TargettingPlayer)
                    return Character.dead || Character.ghost;
                return Guardian.Downed;
            }
        }

        public void SetTargetToPlayer(Player player)
        {
            TargettingPlayer = true;
            TargetPosition = player.whoAmI;
        }

        public double Hurt(int Damage, int HitDirection, string HurtMessage = " was slain.")
        {
            if(TargettingPlayer)
                return Character.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Character.name + HurtMessage), Damage, HitDirection);
            return Guardian.Hurt(Damage, HitDirection, false, false, HurtMessage);
        }

        public static ModTargetting GetClosestTarget(Vector2 CallerCenter)
        {
            ModTargetting mt = new ModTargetting();
            float NearestDistance = -1;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    if (!Main.player[p].dead && !Main.player[p].ghost && !Main.player[p].GetModPlayer<PlayerMod>().ControllingGuardian)
                    {
                        if (Math.Abs(CallerCenter.X - Main.player[p].Center.X) < NPC.sWidth * 2 && Math.Abs(CallerCenter.Y - Main.player[p].Center.Y) < NPC.sHeight * 2)
                        {
                            float Distance = (CallerCenter - Main.player[p].Center).Length() - Main.player[p].aggro;
                            if (NearestDistance == -1  || Distance < NearestDistance)
                            {
                                mt.TargettingPlayer = true;
                                mt.TargetPosition = p;
                                mt.AssistSlot = 0;
                                NearestDistance = Distance;
                            }
                        }
                    }
                    byte slot = 0;
                    foreach (TerraGuardian guardian in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                    {
                        if (guardian.Active && !guardian.Downed)
                        {
                            Vector2 GuardianCenter = guardian.CenterPosition;
                            if (Math.Abs(CallerCenter.X - GuardianCenter.X) < NPC.sWidth * 2 && Math.Abs(CallerCenter.Y - GuardianCenter.Y) < NPC.sHeight * 2)
                            {
                                float Distance = (CallerCenter - GuardianCenter).Length() - guardian.Aggro;
                                if (NearestDistance == -1 || Distance < NearestDistance)
                                {
                                    mt.TargettingPlayer = false;
                                    mt.TargetPosition = p;
                                    mt.AssistSlot = slot;
                                    NearestDistance = Distance;
                                }
                            }
                        }
                        slot++;
                    }
                }
            }
            return mt;
        }
    }
}
