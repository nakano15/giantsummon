using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Actions
{
    public class OpenGiftBoxAction : GuardianActions
    {
        public int BoxPosition, BoxID;

        public OpenGiftBoxAction(int GiftBoxPosition)
        {
            ID = (int)ActionIDs.OpenGiftBox;
            BoxPosition = GiftBoxPosition;
        }

        public override void Update(TerraGuardian guardian)
        {
            IgnoreCombat = true;
            AvoidItemUsage = true;
            if (guardian.UsingFurniture)
                guardian.LeaveFurniture();
            switch (Step)
            {
                case 0:
                    if (StepStart)
                    {
                        if (Main.rand.NextDouble() < 0.01f)
                            Main.PlaySound(29, (int)guardian.Position.X, (int)guardian.CenterY, 89);
                        guardian.DisplayEmotion(TerraGuardian.Emotions.Question);
                        int ItemPosition = BoxPosition;
                        BoxID = guardian.Inventory[BoxPosition].type;
                        guardian.Inventory[ItemPosition].SetDefaults(0, true);
                    }
                    if (Time >= 120)
                        ChangeStep();
                    break;
                case 1:
                    if (StepStart)
                        guardian.DisplayEmotion(TerraGuardian.Emotions.Alarmed);
                    if (Time % 20 == 0)
                    {
                        //Spawn item every 5 ticks;
                        int ItemID = Terraria.ID.ItemID.CopperCoin, Stack = Main.rand.Next(10, 26);
                        if (Main.rand.Next(100) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.PlatinumCoin;
                            Stack = Main.rand.Next(1, 3);
                        }
                        else if (Main.rand.Next(25) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.GoldCoin;
                            Stack = Main.rand.Next(3, 5);
                        }
                        else if (Main.rand.Next(5) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.SilverCoin;
                            Stack = Main.rand.Next(5, 20);
                        }
                        else if (Main.rand.Next(5) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.WoodenCrate;
                            Stack = Main.rand.Next(3, 6);
                        }
                        else if (Main.rand.Next(15) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.IronCrate;
                            Stack = Main.rand.Next(2, 4);
                        }
                        else if (Main.rand.Next(25) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.GoldenCrate;
                            Stack = Main.rand.Next(1, 3);
                        }
                        else if (Main.rand.Next(3) == 0)
                        {
                            ItemID = Terraria.ID.ItemID.HerbBag;
                            Stack = 1;
                        }
                        Item.NewItem(guardian.CenterPosition, ItemID, Stack);
                    }
                    if (Time >= 120)
                        ChangeStep();
                    break;
                case 2:
                    if (StepStart)
                    {
                        guardian.DisplayEmotion(TerraGuardian.Emotions.Happy);
                        guardian.IncreaseFriendshipProgress(5);
                        guardian.Data.GiftGiven = true;
                        InUse = false;
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            switch (Step)
            {
                case 0:
                case 1:
                    guardian.LeftArmAnimationFrame = guardian.Base.ItemUseFrames[2];
                    guardian.RightArmAnimationFrame = guardian.Base.ItemUseFrames[2];
                    UsingLeftArmAnimation = UsingRightArmAnimation = true;
                    break;
            }
        }

        public override void Draw(TerraGuardian guardian)
        {
            Vector2 BoxPosition = guardian.GetGuardianBetweenHandPosition;
            Texture2D texture = Main.itemTexture[BoxID];
            Main.spriteBatch.Draw(texture, BoxPosition - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
        }
    }
}
