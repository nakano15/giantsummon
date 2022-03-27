using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                        AwakenedVersion = Companions.BaphaBase.IsAwakened(tg);
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
                            Rectangle Rect = new Rectangle(0, 0, 200, 200);
                            Rect.X = (int)(tg.Position.X - 90);
                            Rect.Y = (int)(tg.Position.Y - 90);
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
                Vector2 Pivot = new Vector2(8, 95);
                if (tg.LookingLeft)
                    Pivot.X = 88 - Pivot.X;
                float Rotation = MathHelper.ToRadians(-90) + (float)(FrameTime + Frame * 6) * (1f / (6 * 6)) * (MathHelper.ToRadians(135f));

                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, blade, tg.GetGuardianLeftHandPosition, new Rectangle(Frame * 88, AwakenedVersion ? 0 : 100, 88, 100), Color.White, Rotation, Pivot, tg.Scale, tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                tg.Base.InjectTextureBefore(GuardianDrawData.TextureType.TGLeftArm, gdd);
            }
        }
    }
}
