using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace giantsummon.Items
{
	public class GuardianItemPrefab : ModItem
    {
        public Vector2 ItemOrigin = Vector2.Zero;
        public Point ShotSpawnPosition = Point.Zero;
        public float ProjectileFallRate = 0f;
        public Action<TerraGuardian> ItemStatusScript = delegate(TerraGuardian g) { };
        public HeldHand hand = HeldHand.Both;
        public bool Heavy = false, Shield = false, OffHandItem = false;
        public int BlockRate = 0;

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public virtual void ItemUpdateScript(TerraGuardian g)
        {

        }

        public static bool IsHeavyItem(Item i)
        {
            if (i.modItem is GuardianItemPrefab)
                return (i.modItem as GuardianItemPrefab).Heavy;
            return false;
        }

        public static bool IsShield(Item i)
        {
            if (i.modItem is GuardianItemPrefab)
                return (i.modItem as GuardianItemPrefab).Shield;
            return false;
        }

        public static bool IsOffHandItem(Item i)
        {
            if (i.modItem is GuardianItemPrefab)
                return (i.modItem as GuardianItemPrefab).OffHandItem;
            return false;
        }

        public static int GetBlockRate(Item i)
        {
            if (i.modItem is GuardianItemPrefab)
                return (i.modItem as GuardianItemPrefab).BlockRate;
            return 0;
        }

        public static float GetProjFallRate(Item i)
        {
            if (i.modItem is GuardianItemPrefab)
                return (i.modItem as GuardianItemPrefab).ProjectileFallRate;
            return 0;
        }

        public virtual void WhenGuardianUses(TerraGuardian guardian)
        {

        }

        public virtual bool GuardianCanUse(TerraGuardian guardian)
        {
            return true;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
        {
            TerraGuardian guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
            if (guardian.Active)
            {
                /*if (item.healLife > 0) //Moved to ItemMod
                {
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianHealingValue", "Restores " + (int)(item.healLife * guardian.Base.HealthBonus) + " Guardian Health."));
                }*/
                if (item.damage > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        tooltips.RemoveAt(1);
                    }
                    string DamageText = "";
                    bool HadDamageBefore = false;
                    float DamageBonus = 1f;
                    if (item.melee)
                    {
                        DamageText += "melee ";
                        HadDamageBefore = true;
                        DamageBonus += guardian.MeleeDamageMultiplier - 1f;
                    }
                    if (item.ranged)
                    {
                        if (HadDamageBefore)
                            DamageText += ", ";
                        HadDamageBefore = true;
                        DamageText += "ranged ";
                        DamageBonus += guardian.RangedDamageMultiplier - 1f;
                    }
                    if (item.magic)
                    {
                        if (HadDamageBefore)
                            DamageText += ", ";
                        HadDamageBefore = true;
                        DamageText += "magic ";
                        DamageBonus += guardian.MagicDamageMultiplier - 1f;
                    }
                    if (item.summon)
                    {
                        if (HadDamageBefore)
                            DamageText += ", ";
                        HadDamageBefore = true;
                        DamageText += "summon ";
                        DamageBonus += guardian.SummonDamageMultiplier - 1f;
                    }
                    DamageText = (int)(item.damage * DamageBonus) + " " + DamageText + "damage";
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianWeaponDamage", DamageText));
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianCriticalChance", item.crit + "% Critical Hit Chance"));
                }
                else if (item.wornArmor)
                {
                    tooltips.RemoveAt(1);
                    tooltips.RemoveAt(1);
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianDefenseValue", item.defense + " Defense"));
                }
                else if (item.accessory)
                {
                    tooltips.RemoveAt(1);
                }
                if (Shield)
                {
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianShieldValue", BlockRate + "% Block Rate"));
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianIsShield", "Off-Hand Item"));
                }
            }
            else
            {
                tooltips.Insert(1, new TooltipLine(this.mod, "GuardianItemWarning", "The status will only be shown if you have a Guardian summoned."));
            }
            TooltipLine tl = new TooltipLine(this.mod, "GuardianExclusiveWarning", "Terra Guardian Item");
            tooltips.Insert(1, tl);
            base.ModifyTooltips(tooltips);
        }
        
        public override bool CanUseItem(Player player)
        {
            Main.NewText("This kind of item can only be used by Guardians.");
            return false;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            Main.NewText("This kind of item can only be equipped by Guardians.");
            return false;
        }
	}
}
