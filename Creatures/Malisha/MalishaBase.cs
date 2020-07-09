using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class MalishaBase : GuardianBase
    {
        public MalishaBase()
        {
            Name = "Malisha";
            Description = "Two things are important for her: Practicing\nMagic and Testing. Don't volunteer.";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 84;
            DuckingHeight = 54;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 21;
            Male = false;
            InitialMHP = 135; //960
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 30;
            InitialMP = 30;
            Accuracy = 0.91f;
            Mass = 0.45f;
            MaxSpeed = 5.8f;
            Acceleration = 0.15f;
            SlowDown = 0.41f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.36f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            CallUnlockLevel = 5;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.AmberStaff, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.ManaPotion, 15);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 22;
            DuckingSwingFrames = new int[] { 23, 24, 25 };
            SittingFrame = 18;
            ChairSittingFrame = 17;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            //ThroneSittingFrame = 24;
            BedSleepingFrame = 20;
            SleepingOffset.X = 16;
            ReviveFrame = 19;
            DownedFrame = 21;
            ThroneSittingFrame = 26;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 1);

            SittingPoint = new Point(21 * 2, 36 * 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 13, 2);
            LeftHandPoints.AddFramePoint2x(11, 33, 10);
            LeftHandPoints.AddFramePoint2x(12, 35, 18);
            LeftHandPoints.AddFramePoint2x(13, 31, 27);

            LeftHandPoints.AddFramePoint2x(14, 5, 7);
            LeftHandPoints.AddFramePoint2x(15, 31, 6);
            LeftHandPoints.AddFramePoint2x(16, 41, 40);

            LeftHandPoints.AddFramePoint2x(19, 37, 43);

            LeftHandPoints.AddFramePoint2x(23, 32, 14);
            LeftHandPoints.AddFramePoint2x(24, 42, 24);
            LeftHandPoints.AddFramePoint2x(25, 36, 38);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 2);
            RightHandPoints.AddFramePoint2x(11, 35, 10);
            RightHandPoints.AddFramePoint2x(12, 37, 18);
            RightHandPoints.AddFramePoint2x(13, 33, 27);

            RightHandPoints.AddFramePoint2x(14, 7, 7);
            RightHandPoints.AddFramePoint2x(15, 33, 6);
            RightHandPoints.AddFramePoint2x(16, 43, 40);

            RightHandPoints.AddFramePoint2x(19, 39, 43);

            RightHandPoints.AddFramePoint2x(23, 35, 14);
            RightHandPoints.AddFramePoint2x(24, 45, 24);
            RightHandPoints.AddFramePoint2x(25, 39, 38);

            //MountedPosition
            MountShoulderPoints.DefaultCoordinate2x = new Point(16, 31);
            MountShoulderPoints.AddFramePoint2x(1, 17, 31);
            MountShoulderPoints.AddFramePoint2x(2, 18, 30);
            MountShoulderPoints.AddFramePoint2x(3, 17, 31);
            MountShoulderPoints.AddFramePoint2x(5, 15, 31);
            MountShoulderPoints.AddFramePoint2x(6, 14, 30);
            MountShoulderPoints.AddFramePoint2x(7, 15, 31);

            MountShoulderPoints.AddFramePoint2x(14, 20, 31);
            MountShoulderPoints.AddFramePoint2x(15, 22, 31);
            MountShoulderPoints.AddFramePoint2x(16, 25, 30);

            MountShoulderPoints.AddFramePoint2x(19, 11, 26);

            MountShoulderPoints.AddFramePoint2x(22, 14, 38);
            MountShoulderPoints.AddFramePoint2x(23, 14, 38);
            MountShoulderPoints.AddFramePoint2x(24, 14, 38);
            MountShoulderPoints.AddFramePoint2x(25, 14, 38);

            MountShoulderPoints.AddFramePoint2x(26, 14, 32);

            //Hat Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(22, 10);
            HeadVanityPosition.AddFramePoint2x(16, 36, 21);
            HeadVanityPosition.AddFramePoint2x(19, 33, 24);
            HeadVanityPosition.AddFramePoint2x(22, 33, 25);
            HeadVanityPosition.AddFramePoint2x(23, 33, 25);
            HeadVanityPosition.AddFramePoint2x(24, 33, 25);
            HeadVanityPosition.AddFramePoint2x(25, 33, 25);

            //Wing Position
            WingPosition.AddFramePoint2x(14, -1000, -1000);
            WingPosition.AddFramePoint2x(15, -1000, -1000);
            WingPosition.AddFramePoint2x(16, -1000, -1000);

            WingPosition.AddFramePoint2x(19, 28, 33);
            WingPosition.AddFramePoint2x(23, 28, 33);
            WingPosition.AddFramePoint2x(24, 28, 33);
            WingPosition.AddFramePoint2x(25, 28, 33);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture("tails", "tails");
        }

        public override void GuardianPreDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
            Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
            if (TailTexture == null)
            {
                return;
            }
            GuardianDrawData dd;
            if (!guardian.PlayerMounted)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
            }
            else
            {
                rect.Y += rect.Height * 2;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            if (guardian.PlayerMounted)
            {
                Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
                if (TailTexture == null)
                    return;
                GuardianDrawData dd;
                if (guardian.BodyAnimationFrame == HeavySwingFrames[0])
                {
                    rect.Y += rect.Height * 2;
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                    guardian.AddDrawData(dd, true);
                    rect.Y += rect.Height * 2;
                }
                else
                {
                    rect.Y += rect.Height * 4;
                }
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, true);
            }
            else
            {
                if (guardian.BodyAnimationFrame == HeavySwingFrames[0])
                {
                    Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                    Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
                    if (TailTexture == null)
                        return;
                    GuardianDrawData dd;
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                    guardian.AddDrawData(dd, true);
                }
            }
        }

        //It has been so long that I got this companion idea, that I even forgot her personality. Oops.
        public override string GreetMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Oh, a Terrarian. I think I may have a use for you.");
            Mes.Add("You're a Terrarian? You're smaller than I initially thought.");
            Mes.Add("You're really small, my neck aches a bit trying to look at you. Say, would you mind participating of some experiements?");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Don't mind what people says, I'm one of the best magicians in the Ether Realm.");
            if (player.Male)
                Mes.Add("You're making me a bit uncomfortable with the angle you're looking at me. Just a bit.");
            else
                Mes.Add("You can't see my head, or something?");
            Mes.Add("I once tried to conjure demons, that's when I had to leave the first village I lived in hurry.");
            Mes.Add("Always wondered why you get stronger by using specific kinds of outfits? Well, me too.");
            Mes.Add("It's not easy being a prodigy, but when you're one, you have to keep working hard to continue being.");
            Mes.Add("I have perfect control of my magic! Now, at least. Let's not revive past experiences.");
            Mes.Add("Nobody really complained about my experiements. Here.");

            if (NPC.AnyNPCs(Terraria.ID.NPCID.Wizard))
            {
                Mes.Add("I tried to cast a conjuration spell with " + NPC.firstNPCName(Terraria.ID.NPCID.Wizard) + " once, we ended up spawning a rain of Corrupt Bunnies.");
                Mes.Add("I tend to share my work with " + NPC.firstNPCName(Terraria.ID.NPCID.Wizard) + " sometimes, at least one wont blame the other if something explodes.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add(NPC.firstNPCName(Terraria.ID.NPCID.Stylist) + " says that wants to do magic with my hair, but I sense that her magic level is 0.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
            {
                Mes.Add("Can you tell " + NPC.firstNPCName(Terraria.ID.NPCID.Dryad) + " that I don't need a baby sitter? If the fauna suddenly tries to eat you alive is because... Well, probably not my fault.");
                Mes.Add(NPC.firstNPCName(Terraria.ID.NPCID.Dryad) + " says that I'm a living sign of bad omen. No matter what she says, I will keep experiementing.");
            }

            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("I tried to analyze [gn:" + Rococo + "]'s intelligence once, I got a NotANumber Exception Error at line 297.");
            }
            if (NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("[gn:" + Blue + "] seems a bit bothered for having another girl in the town.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("I wont burn your town to cinder, If that's what's on your mind.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("I have something else to experiement on right now.");
            Mes.Add("No, I'm not looking for test subjects right now.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Hey, I need your help for one of my experiements. Don't worry, I wont harm you in the process.");
            Mes.Add("My magician magnificence stops me from doing this right now, If you could help me.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Thank you! Time for the experiement. I hope this time It doesn't explodes.");
            Mes.Add("Good. If you manage to smell something foul, or hear random screams, don't worry, It's just part of the process.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("...");
                Mes.Add("...WHAT? GIVE ME A HOUSE, FOOL!");
                Mes.Add("What do I have to do, FOR YOU TO GIVE ME A PLACE TO LIVE?!");
            }
            else
            {
                Mes.Add("There is a lot of places to practice, but I need a moment to recharge my mana.");
                Mes.Add("Everything seems to want a piece of me, would be nice If I had some peaceful place to stay.");
                if (Main.raining)
                {
                    Mes.Add("I think I know a spell to stop this rain, but I don't have the mana to cast It. I wouldn't need that If I had a house.");
                    if (player.Male)
                        Mes.Add("I really hate to be wet, even more because It attracts looks of peo... Where are you looking at?");
                    else
                        Mes.Add("You're wet too, maybe we should go to some place dry instead of staying in the rain.");
                }
                if (!Main.dayTime)
                {
                    Mes.Add("It's fascinating that there are dead terrarians roaming the world at night. But It'd like to study that behind walls.");
                    Mes.Add("I really hate the night, so many eyes peeking on me. I really need some place for myself.");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
