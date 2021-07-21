using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class CinnamonBase : GuardianBase
    {
        public CinnamonBase() //Her recruitment could involve Soup.
        {
            Name = "Cinnamon";
            Description = "";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 68;
            DuckingHeight = 60;
            CompanionSlotWeight = 1.5f;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 59f / 68;
            Age = 13;
            SetBirthday(SEASON_SPRING, 28);
            Male = true;
            InitialMHP = 160; //860
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 20;
            Accuracy = 0.43f;
            Mass = 0.36f;
            MaxSpeed = 4.7f;
            Acceleration = 0.33f;
            SlowDown = 0.22f;
            MaxJumpHeight = 18;
            JumpSpeed = 7.19f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();
            CallUnlockLevel = 4;
            MoveInLevel = 2;
            MountUnlockLevel = 6;

            AddInitialItem(Terraria.ID.ItemID.RedRyder, 1);
            AddInitialItem(Terraria.ID.ItemID.LesserHealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.CookedFish, 3);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = 10;
            PlayerMountedArmAnimation = 23;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            DuckingFrame = 18;
            DuckingSwingFrames = new int[] { 20, 21, 22 };
            SittingFrame = 16;
            ChairSittingFrame = 15;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 24;
            BedSleepingFrame = 25;
            SleepingOffset.X = 16;
            ReviveFrame = 19;
            DownedFrame = 17;
            //PetrifiedFrame = 28;

            BackwardStanding = 26;
            BackwardRevive = 27;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);

            RightArmFrontFrameSwap.Add(15, 0);
            RightArmFrontFrameSwap.Add(18, 1);
            RightArmFrontFrameSwap.Add(19, 1);

            //Left Hand
            LeftHandPoints.AddFramePoint2x(11, 16, 4);
            LeftHandPoints.AddFramePoint2x(12, 30, 22);
            LeftHandPoints.AddFramePoint2x(13, 32, 28);
            LeftHandPoints.AddFramePoint2x(14, 29, 34);

            LeftHandPoints.AddFramePoint2x(19, 26, 38);

            LeftHandPoints.AddFramePoint2x(20, 15, 17);
            LeftHandPoints.AddFramePoint2x(21, 29, 25);
            LeftHandPoints.AddFramePoint2x(22, 29, 36);

            //Right Hand
            RightHandPoints.AddFramePoint2x(11, 29, 14);
            RightHandPoints.AddFramePoint2x(12, 32, 22);
            RightHandPoints.AddFramePoint2x(13, 35, 28);
            RightHandPoints.AddFramePoint2x(14, 31, 35);

            RightHandPoints.AddFramePoint2x(20, 26, 17);
            RightHandPoints.AddFramePoint2x(21, 31, 25);
            RightHandPoints.AddFramePoint2x(22, 31, 36);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(18 * 2, 23 * 2);
            MountShoulderPoints.AddFramePoint2x(18, 18, 27);
            MountShoulderPoints.AddFramePoint2x(19, 18, 27);

            //Sitting Position
            SittingPoint = new Point(21 * 2, 39 * 2);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(23, 18 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 23, 22 + 2);
            HeadVanityPosition.AddFramePoint2x(19, 23, 22 + 2);

            //Wing Position
            //WingPosition.DefaultCoordinate2x = new Point(20, 23);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (player.HasItem(Terraria.ID.ItemID.BowlofSoup))
            {
                Mes.Add("*(Snif, Snif) Humm.... (Snif, Snif) You have something that smells delicious. Could you share It with me?*");
            }
            else
            {
                Mes.Add("*Oh, hello. Do you like tasty food too?*");
                Mes.Add("*Hi, I love tasty foods. What do you love?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm loving my time here.*");
            Mes.Add("*(singing) Lalala...*");
            Mes.Add("*Hello [nickname], want something?*");
            Mes.Add("*Are you cooking something? I want to see your cooking secrets, teehee.*");
            if (!guardian.HasBuff(Terraria.ID.BuffID.WellFed))
            {
                Mes.Add("(Growl) *Oh, my stomach is complaining... Is "+(Main.dayTime ? "lunch" : "dinner")+" ready?*");
                Mes.Add("*I hate being hungry... I want to eat something...*");
                Mes.Add("*Ow... I think I should be cooking something to eat.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*I'm quite busy doing the number 2 here... Go away, please.*");
                Mes.Add("*I really didn't wanted you to see me like this.*");
                Mes.Add("*I can't concentrate with you staring at me.*");
            }
            else if (Main.eclipse)
            {
                Mes.Add("*I'm sorry, but I really feel like locking myself inside my house now.*");
                Mes.Add("*Do you think I'll be safe if I lock myself in the toilet?*");
                Mes.Add("*Please, get those horrible creatures away!*");
            }
            else if (Main.dayTime)
            {
                if (!Main.raining)
                {
                    Mes.Add("*What a beautiful day.*");
                    Mes.Add("*I like seeing butterflies flying and critters around.*");
                }
                else
                {
                    Mes.Add("*Oh nice... It's raining... Well.... I'll take the day off.*");
                    Mes.Add("*Acho~! Sorry... I'm alergic to this weather.*");
                    Mes.Add("*I'm getting a bit drowzy...*");
                }
            }
            else
            {
                Mes.Add("*What a silent night.*");
                Mes.Add("*I think I saw something moving in the dark.*");
                Mes.Add("*It's too quiet, and that doesn't makes me feel okay.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*I love having you as my room mate. Your help when making the morning set will be very helpful.*");
                Mes.Add("*It's nice sharing the room with you.*");
            }
            //
            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*[gn:"+Rococo+"] helps me with testing food. He seems to enjoy that.*");
                Mes.Add("*Sometimes [gn:"+Rococo+"] brings trash, wanting me to add them to the food. I keep denying but he keeps bringing them.*");
            }
            if (NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*[gn:" + Blue + "] has a really cool hair.*");
            }
            if (NpcMod.HasGuardianNPC(Bree))
            {
                Mes.Add("*I were learning how to cook from [gn:" + Bree + "].*");
                Mes.Add("*[gn:" + Bree + "] isn't that much grumpy when you talk about something she likes.*");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("*Sometimes I play with [gn:"+Glenn+"]. I like that there's someone of my age around.*");
                Mes.Add("*There are some times where I don't like playing a game with [gn:"+Glenn+"], he can't accept when I win.*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*[gn:"+Brutus+"] told me not to wander on the outside alone... He said that I should call him when I'm going to do so. But I'm already a grown girl, I can take care of myself!*");
                Mes.Add("*I offered [gn:" + Brutus + "] some food earlier, It looked like he liked It.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*A lot of people seems to be scared of [gn:" + Fluffles + "], but I'm not.*");
            }
            if (NpcMod.HasGuardianNPC(Liebre))
            {
                Mes.Add("*I saw [gn:" + Liebre + "] watching me the other day. Am I going to die?*");
                Mes.Add("*It's so scary! Sometimes I'm playing around my house, I see [gn:" + Liebre + "] watching me. Is my time coming? I don't want to die!*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*Sometimes, I ask [gn:" + Vladimir + "] to help me test food. He has an accurate taste for seasoning, he impresses me.*");
                Mes.Add("*I'm curious to meet [gn:" + Vladimir + "]'s brother. Is he as nice as him?*");
            }
            if (NpcMod.HasGuardianNPC(Mabel))
            {
                Mes.Add("*Why people keep staring at [gn:" + Mabel + "]? It's so weird.*");
                Mes.Add("*I asked [gn:" + Mabel + "] if she wanted to test the newest food I cooked, but she said she can't, or else she would gain weight.*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*I love hanging around with [gn:" + Minerva + "], we keep testing each other's meal, to see if we make the best ones.*");
                Mes.Add("*I actually don't exagerate when testing my meals, so you don't need to worry about me ending up like [gn:" + Minerva + "].*");
            }
            if (NpcMod.HasGuardianNPC(Zacks))
            {
                Mes.Add("*The other day, [gn:" + Zacks + "] was at my house and pulled my blanked! I woke up and screamed so loud that he run away.*");
                Mes.Add("*Sometimes, during the night, I see [gn:" + Zacks + "] staring through the window.*");
                Mes.Add("*I fear leaving my house at night, because [gn:" + Zacks + "] may be out there.*");
            }
            //
            if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("*Nom nom nom nom nom nom...* (She seems to be dreaming about eating lots of food)");
                Mes.Add("*Zzzzz.... (Snif snif) Hmmm....* (She smelled something good.)");
                Mes.Add("*Zzzz... Zzzzzz..... More food.... Seasoning.... Eat.... Zzzzz...*");
            }
            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("*What now?!*");
                Mes.Add("*Don't you have someone else to bother?*");
                Mes.Add("*Enough!*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I was a little kid when I got passion for tasty food.*");
            Mes.Add("*My name is the same of a seasoning, since I know. It's funny that my mom picked specifically that name to me.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm not in need of anything now, but come back later.*");
            Mes.Add("*No, I don't have anything I need. Beside we can chat, if you're interessed.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I have something that I reeeeeeeeeeally need done... Could you help me with it?*");
            Mes.Add("*I'm so glad you asked, you're a life saver. I need help with this, can you give me a hand?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Yay! Thank you! You're the best person ever!*");
            Mes.Add("*I'm so glad to have met you. Thanks.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Since I can live in your world, I will need a house for me to stay. Do you have one for me?*");
            Mes.Add("*I need a place to stay and place my things.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You all prepared this party for me? I'm so happy!*");
            Mes.Add("*I really love being around here, you guys are really nice for doing this party for me.*");
            if(!PlayerMod.HasGuardianBeenGifted(player, guardian.ID))
            {
                Mes.Add("*Saaaaaaaay... Do you have some kind of package for me, [nickname]?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
