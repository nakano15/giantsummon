using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Companions.Creatures.Bapha
{
    public class HellSplitterAttack : GuardianSpecialAttack
    {
        private byte Frame = 0, FrameTime = 0;
        private bool AwakenedVersion = false;

        public HellSplitterAttack()
        {
            Name = "Hell Splitter";
            CanMove = false;
            AttackType = SubAttackCombatType.Melee;
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            switch (data.Step)
            {
                case 0:
                    if(tg.ItemAnimationTime == 0)
                    {
                        data.ChangeStep();
                        Frame = 0;
                        FrameTime = 0;
                        AwakenedVersion = BaphaBase.IsAwakened(tg);
                    }
                    break;
                case 1:
                    {
                        bool Hit = false;
                        FrameTime++;
                        if (FrameTime >= 6)
                        {
                            Frame++;
                            FrameTime = 0;
                            Hit = Frame == 2;
                            if (Frame >= 6)
                            {
                                data.EndUse();
                                return;
                            }
                        }
                        if (Hit)
                        {
                            Rectangle Rect = new Rectangle(0, 0, (int)(240 * tg.Scale), (int)(240 * tg.Scale));
                            Rect.X = (int)(tg.Position.X - (tg.LookingLeft ? Rect.Width * 0.55f : Rect.Width * 0.45f));
                            Rect.Y = (int)(tg.CenterY - Rect.Height * 0.45f);
                            int Damage = AwakenedVersion ? 50 : 25;
                            if (tg.SelectedItem > -1)
                            {
                                Damage += (int)(tg.Inventory[tg.SelectedItem].damage * ((float)tg.Inventory[tg.SelectedItem].useAnimation / tg.Inventory[tg.SelectedItem].useTime) * (AwakenedVersion ? 2 : 1.5f));
                            }
                            Damage = (int)(Damage * tg.MeleeDamageMultiplier);
                            GuardianSpecialAttackData.AffectedTargets[] targets = AreaDamage(tg, Damage, AwakenedVersion ? 12 : 8, Rect, BlockConsecutiveHits: false);
                            int BuffID = AwakenedVersion ? Terraria.ModLoader.ModContent.BuffType<Buffs.CrimsonConflagration>() : Terraria.ID.BuffID.OnFire;
                            foreach(GuardianSpecialAttackData.AffectedTargets t in targets)
                            {
                                switch (t.Target)
                                {
                                    case GuardianSpecialAttackData.AffectedTargets.TargetTypes.Player:
                                        Main.player[t.TargetID].AddBuff(BuffID, 5 * 60);
                                        break;
                                    case GuardianSpecialAttackData.AffectedTargets.TargetTypes.NPC:
                                        Main.npc[t.TargetID].AddBuff(BuffID, 5 * 60);
                                        Main.npc[t.TargetID].HitEffect(tg.Direction, t.Damage);
                                        break;
                                    case GuardianSpecialAttackData.AffectedTargets.TargetTypes.TerraGuardian:
                                        MainMod.ActiveGuardians[t.TargetID].AddBuff(BuffID, 5 * 60);
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if(data.Step == 1)
            {
                tg.BodyAnimationFrame = tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = 37 + Frame;
            }
        }

        public override void ModifyDrawing(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            if (data.Step == 1)
            {
                Texture2D blade = tg.Base.sprites.GetExtraTexture(BaphaBase.HellSplitterTextureID);
                Vector2 Pivot = new Vector2(10, 102);
                if (tg.LookingLeft)
                    Pivot.X = 88 - Pivot.X;
                float Rotation = (MathHelper.ToRadians(-90) + (float)(FrameTime + Frame * 6) * (1f / (6 * 6)) * (MathHelper.ToRadians(210f))) * tg.Direction;

                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, blade, tg.GetGuardianLeftHandPosition - Main.screenPosition, new Rectangle(Frame * 88, AwakenedVersion ? 100 : 0, 88, 100), Color.White, Rotation, Pivot, tg.Scale, tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                tg.Base.InjectTextureAfter(GuardianDrawData.TextureType.TGRightArm, gdd);
            }
        }
    }
}
