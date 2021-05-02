using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Actions
{
    public class ThrowPotionAction : GuardianActions
    {
        public Player target;
        public int PotionPos;

        public ThrowPotionAction(Player target, int PotionPos)
        {
            this.target = target;
            this.PotionPos = PotionPos;
            InUse = true;
            ID = (int)ActionIDs.ThrowPotion;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (Time >= 30) //Do potion effect, end skill effect
            {
                Item i = guardian.Inventory[PotionPos];
                if (i.type != 0 && i.potion && i.healLife > 0 && !target.dead)
                {
                    if (target.HasBuff(Terraria.ID.BuffID.PotionSickness))
                    {
                        string[] Messages = new string[] { "Ow!!", "Hey!!", "Ouch!!", "Watch it!", "That hurts!" };
                        target.chatOverhead.NewMessage(Messages[Main.rand.Next(Messages.Length)], 300);
                        CombatText.NewText(target.getRect(), CombatText.DamagedFriendly, 8);
                    }
                    else
                    {
                        int Value = target.GetHealLife(i, true);
                        target.statLife += Value;
                        if (target.statLife > target.statLifeMax2)
                            target.statLife = target.statLifeMax2;
                        if (target.potionDelay <= 0)
                        {
                            target.potionDelay = target.potionDelayTime;
                            target.AddBuff(21, target.potionDelay);
                        }
                        target.HealEffect(Value, true);
                        i.stack--;
                        if (i.stack == 0)
                            i.SetDefaults(0);
                    }
                    Main.PlaySound(Terraria.ID.SoundID.Item107, target.Center);
                }
                InUse = false;
                return;
            }
            if (Time == 0) //Pick potion, if there is one
            {
                guardian.LookingLeft = target.Center.X < guardian.CenterPosition.X;
            }
            else
            {

            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Time < 15)
            {
                HeldHand hand = HeldHand.Left;
                guardian.PickHandToUse(ref hand);
                int AnimationFrame = Time / 5;
                if (AnimationFrame == 0)
                    AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[0] : guardian.Base.ItemUseFrames[0];
                else if (AnimationFrame == 1)
                {
                    AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[1] : guardian.Base.ItemUseFrames[1];
                }
                else if (AnimationFrame == 2)
                {
                    AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[2] : guardian.Base.ItemUseFrames[3];
                }
                if (hand == HeldHand.Left)
                {
                    guardian.LeftArmAnimationFrame = AnimationFrame;
                    UsingLeftArmAnimation = true;
                }
                else
                {
                    guardian.RightArmAnimationFrame = AnimationFrame;
                    UsingRightArmAnimation = true;
                }
            }
        }

        public override void Draw(TerraGuardian guardian)
        {
            if (Time > 0)
            {
                Vector2 StartPosition = guardian.CenterPosition,
                    EndPosition = target.Center;
                float Percentage = (float)Time / 30;
                Vector2 PotionPosition = StartPosition + (EndPosition - StartPosition) * Percentage;
                PotionPosition.Y -= UtilityMethods.Bezier(Percentage, 0, 368f, 0);
                float Rotation = 0.4363323129985824f * Time * guardian.Direction;
                Microsoft.Xna.Framework.Graphics.Texture2D Texture = Main.itemTexture[guardian.Inventory[PotionPos].type];
                Main.spriteBatch.Draw(Texture, PotionPosition - Main.screenPosition, null, Color.White, Rotation, new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
            }
        }
    }
}
