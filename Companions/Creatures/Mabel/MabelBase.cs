﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Companions
{
    public class MabelBase: GuardianBase
    {
        /// <summary>
        /// -Objective centered.
        /// -Naive.
        /// -Tries to be like a mother to the Angler.
        /// -Exceeds on caffeine when stressed.
        /// -Loves company.
        /// -Insecure.
        /// -She saw Rococo before, somewhere.
        /// </summary>

        public MabelBase()
        {
            Name = "Mabel";
            Description = "She dreams of being a model. And she still pursues it.";
            Size = GuardianSize.Large;
            Width = 30;
            Height = 84;
            CompanionSlotWeight = 1.15f;
            DefaultTactic = CombatTactic.Charge;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 92f / 84;
            Age = 17;
            SetBirthday(SEASON_AUTUMN, 26);
            Male = false;
            InitialMHP = 150; //975
            LifeCrystalHPBonus = 35;
            LifeFruitHPBonus = 15;
            Accuracy = 0.81f;
            Mass = 0.55f;
            MaxSpeed = 5.78f;
            Acceleration = 0.17f;
            SlowDown = 0.39f;
            MaxJumpHeight = 17;
            JumpSpeed = 7.12f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();
            CallUnlockLevel = 3;
            Roles = GuardianRoles.PopularityContestHost;
            DodgeRate = 10;

            PopularityContestsWon = 0;
            ContestSecondPlace = 1;
            ContestThirdPlace = 1;

            AddInitialItem(Terraria.ID.ItemID.BeeKeeper, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);
            SpecificBodyFrontFramePositions = true;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            HeavySwingFrames = new int[]{10, 14, 15};
            DuckingFrame = 17;
            DuckingSwingFrames = new int[] { 18, 19, 20 };
            SittingFrame = 16;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11 });
            BodyFrontFrameSwap.Add(16, 0);
            ThroneSittingFrame = 21;
            BedSleepingFrame = 22;
            SleepingOffset.X = 16;
            ReviveFrame = 23;
            DownedFrame = 24;

            BackwardStanding = 25;
            BackwardRevive = 26;

            //Mounted Position
            MountShoulderPoints.DefaultCoordinate2x = new Point(19, 14);
            MountShoulderPoints.AddFramePoint2x(17, 19, 20);
            SittingPoint = new Point(24 * 2, 37 * 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 14, 5);
            LeftHandPoints.AddFramePoint2x(11, 37, 8);
            LeftHandPoints.AddFramePoint2x(12, 39, 20);
            LeftHandPoints.AddFramePoint2x(13, 35, 33);

            LeftHandPoints.AddFramePoint2x(14, 42, 20);
            LeftHandPoints.AddFramePoint2x(15, 42, 39);

            LeftHandPoints.AddFramePoint2x(18, 14, 12);
            LeftHandPoints.AddFramePoint2x(19, 37, 15);
            LeftHandPoints.AddFramePoint2x(20, 39, 28);

            LeftHandPoints.AddFramePoint2x(23, 36, 40);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 17, 5);
            RightHandPoints.AddFramePoint2x(11, 40, 8);
            RightHandPoints.AddFramePoint2x(12, 42, 20);
            RightHandPoints.AddFramePoint2x(13, 38, 33);

            RightHandPoints.AddFramePoint2x(14, 45, 20);
            RightHandPoints.AddFramePoint2x(15, 44, 39);

            RightHandPoints.AddFramePoint2x(18, 17, 12);
            RightHandPoints.AddFramePoint2x(19, 40, 15);
            RightHandPoints.AddFramePoint2x(20, 42, 27);

            //Headgear Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(23, 12);
            HeadVanityPosition.AddFramePoint2x(14, 36, 21);
            HeadVanityPosition.AddFramePoint2x(15, 38, 31);

            HeadVanityPosition.AddFramePoint2x(17, 23, 19);

            HeadVanityPosition.AddFramePoint2x(23, 34, 28);
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "*Heeey! Say, your travels involves a lot of walking? Yes?! Woohoo! Take me with you then! That may help me stay fit for the contest.*";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*It must be troublesome to walk around with those small legs of your. I know! You can sit on my shoulders! That can help us move faster. I'm a genius, I know. And also pretty.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*Hey, do you want to meet the Ether Realm? What? You can't visit it? Don't worry, I can take you there.*";
            }
        }
        
        public override bool GuardianWhenAttackedNPC(TerraGuardian guardian, int Damage, NPC Attacker)
        {
            if (Main.rand.Next(5) == 0)
                Attacker.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.Love>(), 5 * 60);
            return true;
        }

        public override bool GuardianWhenAttackedPlayer(TerraGuardian guardian, int Damage, bool Critical, Player Attacker)
        {
            if (Main.rand.Next(5) == 0)
                Attacker.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.Love>(), 10 * 60);
            return true;
        }

        public override bool GuardianWhenAttackedGuardian(TerraGuardian guardian, int Damage, bool Critical, TerraGuardian Attacker)
        {
            if (Main.rand.Next(5) == 0)
                Attacker.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.Love>(), 10 * 60);
            return true;
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "*Hello, is this the place for Miss North Pole selection?*";
                case 1:
                    return "*Oh, Hello. Do you know the direction to the North Pole?*";
                case 2:
                    return "*Hi, have you seen any reindeers around?*";
                default:
                    return "*Uh... I don't know what to say... Hi?*";
            }
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            return (Main.rand.NextDouble() < 0.5 ? "*Huh?! No, I don't want anything.*" : "*Oh! No, there is nothing I want right now.*");
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            return (Main.rand.NextDouble() < 0.5 || guardian.request.Base is TravelRequestBase ?
                "*I've been so busy practicing to be a model, that I forgot that I have something to do. Can you [objective]?*" :
                "*I just don't have the time to deal with this, so can you [objective] for me?*");
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            return (Main.rand.NextDouble() < 0.5 ? "*Amazing! I knew you would be able to do it.*" : "*Thank you, I.. You don't know how happy I am for this.*");
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            bool AnglerInTown = NPC.AnyNPCs(Terraria.ID.NPCID.Angler);
            List<string> Mes = new List<string>();
            Mes.Add("*Hey! How do I look? I've been practicing all the day.*");
            Mes.Add("*The citizens of your town are very kind to me.*");
            Mes.Add("*Hey, do you listen to Deadraccoon5 too? Whaaaaat? You don't know who he is?*");
            if (player.Male)
            {
                Mes.Add("*Where are you looking at? My face is a bit more above.*");
            }
            Mes.Add("*Do you think I have even a bit of chance on the Miss North Pole contest?*");
            Mes.Add("*I was trying to fly earlier, to see If I can get into the contest. Maybe they only accept Reindeers on it, so If I could fly, I could at least participate?*");
            Mes.Add("*Everytime I try to join the Miss North Pole contest, they come with a different excuse.*");
            Mes.Add("*The Miss North Pole is a contest that happens some days before new year. Before happens what your people calls \"X-mas\".*");
            Mes.Add("*My mom used to say that I had the luck of being attracted to merry places. Maybe that explains how I got here.*");
            if (Main.bloodMoon)
            {
                Mes.Add("*How do I look? How do I look-How do I look-How do I look? \"She's blinking her eyes very fast, about 30 frames per second.\"*");
                Mes.Add("*The night will be ended soon-The night will be ended soon.*");
                Mes.Add("(She seems to be drinking a mug of coffee, or half a dozen, through out the night.)");
                Mes.Add("*You'll keep me safe, right? Right? RIGHT?!*");
                if (AnglerInTown)
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] is safe. Kids doesn't dies, right? They turn into smoke and goes away, right? He's safe, right? Right?*");
            }
            else
            {
                if (MainMod.IsPopularityContestRunning)
                {
                    Mes.Add("*Hi! The TerraGuardians Popularity Contest is currently on going. Will you be voting?*");
                    Mes.Add("*If you are interessed in participating of the TerraGuardians Popularity Contest, I can take you to the votings.*");
                }
                if (!Main.dayTime)
                {
                    Mes.Add("*Zzzzz... I'm the prettiest in the contest... Zzzz.... I will win... Zzzz....*");
                    Mes.Add("*What is it? I'm preparing to go sleep, since It seems like beauty is also related to how well you sleep.*");
                    Mes.Add("*Oh, hello. Can't sleep?*");
                }
                else
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("*My mother would be scared of those kinds of creatures outside, she was from around that time.*");
                        Mes.Add("*Why this world have such weird things happening?*");
                    }
                    if (Main.raining)
                    {
                        Mes.Add("*Rain aways hides a beautiful sunny day. That always makes me sad.*");
                        Mes.Add("*The day is so gray outside, I makes me feel gray too.*");
                        Mes.Add("*If the rain doesn't go away before night, It will surelly ruin a great day.*");
                    }
                    else
                    {
                        Mes.Add("*It's a beautiful day outside. I guess I'll go have a walk.*");
                        Mes.Add("*Don't you just love this kind of weather? It always makes me want to go out for a walk.*");
                        Mes.Add("(She's humming while looking through the window.)");
                    }
                }
            }
            if (AnglerInTown)
            {
                string anglername = "[nn:" + Terraria.ID.NPCID.Angler + "]";
                Mes.Add("*I think that "+anglername+" may be in need of a mother. What If I could try being one?*");
                Mes.Add("*I tried to give "+anglername+" some vegetables to eat, instead of just fish. He was very rude at me.*");
                Mes.Add("*How old is "+anglername+"? He's old enough to eat fish, so I guess he doesn't need...*");
                Mes.Add("*It's so sad, "+anglername+" having no parents and living alone. Will I be able to be like a mother for him?*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*There is the correct time and place to do this. But you had to talk to me while I'm using the toilet?*");
                Mes.Add("*Could you... Just... Return another time?*");
                Mes.Add("*Don't you know that there are a few moments one needs privacy? I'm trying to lose some weight here.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Huh? You're going to sleep in my room? That is so cool! I wonder what is It like to share a room with someone.*");
                Mes.Add("*Sharing my room with you is amazing! I really like that.*");
                Mes.Add("*Sometimes gets cold at night, so It's nice having someone help me get warm.*");
            }

            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("*I think I have met [gn:0] before somewhere... I don't remember where.*");
                Mes.Add("*Why [gn:0] eats thing from trash cans? I remember seeing him eating food earlier. Where did he got the food, anyway?*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*[gn:1] thinks she's prettier than me. I will prove that she's wrong.*");
                Mes.Add("*Earlier this day, [gn:1] complained about the way I walk. She didn't liked it when I talked about her hair, though.*");
                if(NpcMod.HasGuardianNPC(2))
                    Mes.Add("*Why is [gn:1] so mean to [gn:2]? He's the nicest guy I've met in a while.*");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("*[gn:2] is a true gentlemen. He's always disponible to help me with whatever I ask.*");
                Mes.Add("*[gn:2] always look at me with a very happy face. But I have to keep reminding him that my face is a bit more above.*");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("*I should be scared by the fact a Zombie is living in your town. But I wont judge your decision. I'll just say that It's fine. \"Sips coffee\"*");
                Mes.Add("*Everytime I talk with [gn:3], he keeps looking around, like as if were checking if there isn't someone around.*");
                if(NpcMod.HasGuardianNPC(1))
                    Mes.Add("*So... [gn:3] is [gn:1]'s boyfriend? No. I wont ask.*");
            }
            if (NpcMod.HasGuardianNPC(4))
            {
                Mes.Add("*[gn:4] and I play the stare game sometimes. He always wins, though...*");
                Mes.Add("*I have to say, It gives me chills when I bump into [gn:4] at night.*");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*Where did you find that cute dog? I want to hug [gn:5] and never stop.*");
                Mes.Add("*I always wanted to have a dog, but my mom always said that they were \"dirty and spacious\". She's kind of right, but hey! There is a dog in the town!*");
            }
            if (NpcMod.HasGuardianNPC(6))
            {
                Mes.Add("*I think I am very lucky. [gn:6] personally came to me, saying that If I want a bodyguard, he's disponible anytime.*");
                Mes.Add("*It's quite weird that sometimes when I talk with [gn:6], he makes some puns with meat. What is that supposed to mean?*");
                Mes.Add("*Sometimes I see [gn:6] watching me from afar. He probably cares about my safety.*");
            }
            if (NpcMod.HasGuardianNPC(7))
            {
                if (NpcMod.HasGuardianNPC(2))
                {
                    Mes.Add("*[gn:7] came to me earlier, and said that she got what I'm doing, and that I should stop doing that with [gn:2], or else she'll give me a beating. But I wonder, what have I been doing to him?*");
                    Mes.Add("*I don't know why [gn:7] is so upset about me, I don't remember being rude or mean to her.*");
                }
                Mes.Add("*I have to say, sometimes I have problems seeing [gn:7] when she's right under me.*");
                Mes.Add("*It seems like [gn:7] is avoiding talking to me. Did I anger her somehow?*");
            }
            if (NpcMod.HasGuardianNPC(Domino))
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                {
                    Mes.Add("*[nn:"+Terraria.ID.NPCID.ArmsDealer+"] gave me an idea of something I can use for the Miss contest, after I asked him what Terrarian Miss uses on his hometown. Gladly, [gn:"+Domino+"] had some for sale, but I feel weird wearing that kind of thing.*");
                }
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*I don't understand. I was talking about some things related to the Miss Contest to [gn:"+Vladimir+"], until he suddenly dropped me on the floor and said that had to go to the toilet urgently. My behind is still hurting from the fall. Ouch~.*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*Did you knew that [gn:" + Michelle + "] will try joining the Miss North Pole contest? I'm so happy, I never wondered I would have a rival. Wait... Is that good or bad?*");
                Mes.Add("*I'm so glad to have met [gn:"+Michelle+"]. She's a great person to have around.*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*I wonder if I'm not accepted into the Miss North Pole contest because I'm not a reindeer. Maybe [gn:"+Malisha+"] could help me solve that?*");
            }
            if (NpcMod.HasGuardianNPC(Wrath))
            {
                Mes.Add("*I tried helping [gn:"+Wrath+"] getting less angry, until he yelled out loud, that made me leave the room very quickly. He's very scary.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*What's with [gn:" + Fluffles + "]? Sometimes when she looks at me, she looks me from the head to the feet.*");
            }
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("*When visitting [gn:" + Cinnamon + "], I always have to resist the temptation of nibbling everything she cooked.*");
            }
            if (NpcMod.HasGuardianNPC(Luna))
            {
                Mes.Add("*Whaaaaaaaaaaat? [gn:" + Luna + "] won a edition of Miss North Pole? Like... Whaaaaaaaaaaaaat?*");
                Mes.Add("*[gn:" + Luna + "] thinks I'm exagerating a bit about the practice of the contest, but I don't think like that.*");
            }
            if (NpcMod.HasGuardianNPC(Green))
            {
                Mes.Add("*I really don't like [gn:"+Green+"]. Everytime I visit him he gives me some diet. Do I look fat, to you?*");
                Mes.Add("*I was expecting to get vitamins from [gn:"+Green+"], not a diet! Hmph...*");
            }
            if (NpcMod.HasGuardianNPC(Cille))
            {
                Mes.Add("*I really don't like [gn:" + Cille + "]. I was all happy, gave her a 'hi', and she told me to 'go away'! Why she had to be so rude?*");
                Mes.Add("*What kind of clothing [gn:" + Cille + "] uses? Is that the kind of thing you Terrarians use?*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("(She's crying, while placing her paws on the chest.)");
                Mes.Add("(She looks very scared.)");
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                {
                    Mes.Add("(While in pain, she said for a moment [n:"+Terraria.ID.NPCID.Angler+"] name.)");
                }
                Mes.Add("(She seems to be breathing hard, to try reducing the pain.)");
            }
            else if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("*I won... I won! Yes...* (She must be dreaming about winning the contest)");
                Mes.Add("*I will prove you wrong... I will be the best miss ever...* (She's speaks while she sleeps)");
                Mes.Add("*I am like this... You can't change me... I'll try anyway...* (She's speaks while she sleeps)");
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                {
                    Mes.Add("*Am I... A good mother...Doing things right...?* (She's speaks while she sleeps)");
                    Mes.Add("*I'm... A failure... Of mother...?* (She's speaks while she sleeps)");
                    Mes.Add("*I'm trying... my best...* (She's speaks while she sleeps)");
                }
                Mes.Add("*" + player.name + "...* (Looked like she was going to ask something in her sleep)");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Who's she, [nickname]? Did you met a new friend?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I need some peaceful place to practice my walk. Of preference, without monsters chasing me.*");
            Mes.Add("*I think I have some time until the next event, but where can I stay until It doesn't happen?*");
            if (!Main.dayTime)
            {
                Mes.Add("*Please! Give me some place! Any place to stay! It's too creepy being alone in the dark!*");
                Mes.Add("*I hate undeads. I hate flying eyes. I hate EVERYTHING that pops out in the dark to kill me! Is there some place I can stay safe?*");
            }
            if (Main.bloodMoon)
            {
                Mes.Add("*HELP ME!!! HEEEEEEEEEEEELP!!!*");
                Mes.Add("*This place is awful!! JUST AWFUL!!! Why did you leave me here all alone by myself?! WHY?!*");
            }
            if (Main.raining)
            {
                Mes.Add("*This isn't going to do good to my fur. Do you have a dry place I can stay?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My mom and I had to move out of a town once. I think It was because the citizens were way too good to me.*");
            Mes.Add("*Sometimes one of my feet starts to hurt, because I place most of my weight on it.*");
            Mes.Add("*I have to admit, I fell sometimes when trying to mantain this pose. That also explains why I'm missing a teeth.*");
            Mes.Add("*I'm pursuing my life time goal, I'm going to be a Miss North Pole. But I wonder, what should I pursue next when I achieve that?*");
            if(guardian.FriendshipLevel >= guardian.Base.CallUnlockLevel)
                Mes.Add("*Don't blush, but I accompany you on your adventures because I like your company.*");
            Mes.Add("*Do you think I'm naive? Too many people says that that's my problem. But I don't see myself as being naive.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
            {
                Mes.Add("*I worry about [nn:"+Terraria.ID.NPCID.Angler+"], what should I do If he feels sick? I don't have any idea of what being a Mom is.*");
                Mes.Add("*Do you think that I'm being a good mother to [nn:" + Terraria.ID.NPCID.Angler + "]? Sometimes I think I'm not...*");
                Mes.Add("*How you takes care of your children? Oh, you're single. Sorry that I asked...*");
                Mes.Add("*Being a mother is scary, most of the time I keep asking myself If what I'm doing is enough, or if is right.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*Don't you dare tell [gn:1], but I like what she has done to her hair.*");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("*Sometimes when I'm alone talking with [gn:2], he looks to the side, and then says that has something else to do, and leaves.*");
                Mes.Add("*There were a few times where I tried to speak with [gn:2], but he seemed to be having difficulties to talk.*");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("*I saw [gn:3] drolling earlier this day. I wonder why is that.*");
                Mes.Add("*I could swear that I heard a faint howling earlier. Are there wolves around?*");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*Oh great one! [gn:5] is so cute! Even more when we pet him and he turns upside down, so we can pet his belly.*");
            }
            if (NpcMod.HasGuardianNPC(6))
            {
                Mes.Add("*Everytime I ask [gn:6] to be my bodyguard, he insists on following me from behind, because most dangerous attacks comes from behind, or something.*");
                Mes.Add("*When someone smiles, is because It's happy. But what kind of facial expression [gn:6] does when looking at me?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Oooohh! A party! You guys are very kind.*");
            Mes.Add("*If I had this kind of spotlight on the Miss North Pole, I surelly would have won.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("*What will you gift me? I'm curious, can't I be?*");
            }
            else
            {
                Mes.Add("*I really enjoyed the gift you gave me. I really mean it.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*Wake up! You still need to see me winning the contest!*");
                Mes.Add("*Come on, wake up! There's a lot for you to see!*");
                Mes.Add("(Crying)");
            }
            else
            {
                Mes.Add("*Hey, I've been practicing hard for the contest to lose someone who could watch me on it. Wake up!*");
                Mes.Add("*I'll take care of those wounds you have. Don't worry.*");
                Mes.Add("*You people have been so nice to me, It's time for me to retribute that.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == Brutus)
                {
                    Weight = 1.2f;
                    return "*Uh, my face is a bit above.*";
                }
                if(WhoJoined.ID == Blue)
                {
                    Weight = 1.2f;
                    return "*Hello. I like your hair, by the way.*";
                }
                if(WhoJoined.ID == Luna)
                {
                    Weight = 1.5f;
                    return "*Uh, hi... Didn't I see you somewhere?*";
                }
            }
            Weight = 1f;
            return "*This world is getting more and more people, I like that.*";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Blue:
                        Weight = 1.5f;
                        return "*She's coming with us? That's nice.*";
                    case Brutus:
                        Weight = 1.5f;
                        return "*I feel safer already.*";
                    case Vladimir:
                        Weight = 1.5f;
                        return "*It's so nice to have you join us.*";
                    case Sardine:
                        Weight = 1.5f;
                        return "*Are you happy for joining us?*";
                    case Miguel:
                        Weight = 1.5f;
                        return "*Are you here to help me do exercises?*";
                }
            }
            Weight = 1f;
            return "Hello! I'm glad you joined us.*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*Buddy? You? Mine? Yes! Let's be buddies!*";
                case MessageIDs.RescueMessage:
                    return "*Don't worry, I'll give you a hand.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*Yawn... Oh, hello [nickname]. I really need some sleep to keep my beauty for the contest. What do you need?*";
                        case 1:
                            return "*Aaaah! Oh, sorry, you scared me. I wasn't having a good dream...*";
                        case 2:
                            return "*Huh? Oh. Do you need something? I was dreaming about... Nevermind...*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Oh, hi [nickname]. Did you do what I asked you?*";
                        case 1:
                            return "*I'm so sleepy, did you woke me up to tell that did what I asked?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*You're calling me to an adventure? Cool! The walking will help me get fit for the contest.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's way too many people following you right now...*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Sorry, but I need to get myself ready for the contest.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*I really dislike the idea of being left here all alone. Do you know what those horrible creatures do to people like me?!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Alright. I enjoyed travelling to you, now back to practicing.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Okay, better I start running, then?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*I'm soooooooo happy that you changed your mind.*";
                case MessageIDs.RequestAccepted:
                    return "*Yay! You know where to find me when you do It, right?*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*It may be dangerous to your health to have many things to do. Why don't you try doing your other requests before?*";
                case MessageIDs.RequestRejected:
                    return "*Aww... No problem. I'm sure you have a good reason to reject my request.*";
                case MessageIDs.RequestPostpone:
                    return "*Alright. Come ask me again if you want to do my request. Bye!*";
                case MessageIDs.RequestFailed:
                    return "*You failed? Don't worry, It's fine. You tried, right? There, don't feel sad.*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*My request? You completed it?*";
                case MessageIDs.RequestRemindObjective:
                    return "*I asked you to [objective]. Is that what you wanted to know?*";
                case MessageIDs.RestAskForHowLong:
                    return "*Yes, my legs are hurting a bit, resting will be nice. How long do you think we'll be fine?*";
                case MessageIDs.RestNotPossible:
                    return "*Now? No way! Seems like a impossible moment!*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Alright. I hope we have a nice dreams.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Wait [nickname], [shop] seems to have\nsomething I need.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Oh! Wait, I need to buy that...*";
                case MessageIDs.GenericYes:
                    return "*Yes!*";
                case MessageIDs.GenericNo:
                    return "*No... No.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you! You're the best!*";
                case MessageIDs.GenericNevermind:
                    return "*Changed your mind?*";
                case MessageIDs.ChatAboutSomething:
                    return "*Sure. So, what do you want to talk about?*";
                case MessageIDs.NevermindTheChatting:
                    return "*Oh, okay.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You can't do my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Oh... I hope I didn't make your nervous, or anything... You don't need to do what I asked anymore.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*I'm so glad...*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Okay, allow me to know you more...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Oh my... You're...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*Better I close my eyes, and try not to think.*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Phew... It's over. My heart can stop beating fast now.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ah... No! My nose is fine... It's just... Nothing.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Oh! My hero! Thank you!*";
                    return "*Thank you, I'm so glad to have you around.*";
                case MessageIDs.RevivedByRecovery:
                    return "*Ouch... It still hurts... But I can still walk..*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Pain... Everywhere...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Aahh!! My skin!! Burns!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes hurts!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Are you there?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I can't attack! I can't do anything!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*I don't feel like running faster..*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I can't seem to be able to fully help now...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*My chest! Argh!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*Eeek!! What is that! Let's get out of here!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Hey! That's very shameful!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*I'm so cold.... Acho~!*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I'm stuck here! Help!!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*You took my blood, now I'll take yours!!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Hey, I feel resistant now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Thank you for the food.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Hey, I like this. But I hope It doesn't change my physique.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*I feel like running a marathon now.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*I think monsters will threaten me less now.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*Now I will hurt those creatures.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*So... I should drink this, right?*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return ""; //Doesn't drinks
                case MessageIDs.AcquiredHoneyBuff:
                    return "*I really love this. This wont make me fat, right?*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*What a beautiful looking crystal.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*That looks dangerous. Watch your step.*";
                case MessageIDs.FoundMineTile:
                    return "*There's a mine there.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Who left that detonator over there?*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Oooohhh!! What a interesting flower! Can we place it on a vase and put It at my home?*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I can't understand why creatures appears when you place a crystal there.*";
                case MessageIDs.FoundTreasureTile:
                    return "*Hey! Let's look inside.*";
                case MessageIDs.FoundGemTile:
                    return "*Shiny gems.*";
                case MessageIDs.FoundRareOreTile:
                    return "*I see some ores there.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*I don't remember seeing that kind of ore often.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*We're going to ride the minecart?*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Nice timing, my feet were aching.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Hello! I'm glad you joined us.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*I wonder if they could teach me proper posing.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Just how much did you missed having another person around?*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname]! Hang on! I'm coming.*";
                case MessageIDs.LeaderDiesMessage:
                    return "*[nickname]!! You couldn't have died! You didn't even watched me being a super popular model yet! [nickname]!!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone fell! They need help!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*That looks cool! That looks rare!*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be right back. I will sell those things and return with the coins.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Watch yourself [nickname]. You are very wounded.*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*Ahh! I should.... Keep... The pose... Stand...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Huh? There's only all those potions?! I will need more.*";
                case MessageIDs.UsesLastPotion:
                    return "*My potions! Where did they go?!*";
                case MessageIDs.SpottedABoss:
                    return "*It's... Huge... I'm scared! We'll be fine?*";
                case MessageIDs.DefeatedABoss:
                    return "*I'm glad we defeated it.*";
                case MessageIDs.InvasionBegins:
                    return "*Who are they? Are they stalkers?!*";
                case MessageIDs.RepelledInvasion:
                    return "*I'm glad that we managed to repell them...*";
                case MessageIDs.EventBegins:
                    return "*What's with the sky? It's not like a usual day.*";
                case MessageIDs.EventEnds:
                    return "*What was that? I hope that doesn't happen again.*";
                case MessageIDs.RescueComingMessage:
                    return "*Ahh!! Wait! I'm coming!*";
                case MessageIDs.RescueGotMessage:
                    return "*Here I am. Now let me try keeping you safe.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*Hi! Do you know [player]? They will also cheer for me when I turn into a Model.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*Hey, did you hear? [player] killed [subject]! That means their world is safer, right?*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*Did you hear? [player] found a [subject]! What is that? I don't know! But must be really rare.*";
                case MessageIDs.FeatEventFinished:
                    return "*It was so scary when a [subject] happened on [player]'s world. Gladly they managed to take care of it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*Hey, [subject] just met [player]! I mean... [player] just met [subject].*";
                case MessageIDs.FeatPlayerDied:
                    return "*My friend [player] died recently... They didn't even managed to see me in the stage...*";
                case MessageIDs.FeatOpenTemple:
                    return "*Is said that [player] has opened a temple door at [subject]. They must be exploring it right now.*";
                case MessageIDs.FeatCoinPortal:
                    return "*A coin portal has appeared before [player]! They're so lucky!*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I've met a cool Terrarian named [player] latelly.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*[player] seems to be enjoying helping the Angler Kid.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*[player] saved us all from danger in [subject]. You should have seen them fighting the giant squid monster.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*It looks like [subject] got more dangerous. I don't know if I'll stay there for longer that way.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*Wow! Did you hear? [player] has picked [subject] as their buddy! That's so amazing!*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Hi? Do you want something? Came to check on me? Sorry, I'm just anxious. I never thought I'd get my own buddy!*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*[subject] got a new cool place to live in [world].*";
                case MessageIDs.DeliveryGiveItem:
                    return "*This [item] is what you need, right, [target]?*";
                case MessageIDs.DeliveryItemMissing:
                    return "*I thought I had the item on my bag... Oh well..*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*Your inventory is too full for my item, [target]..*";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "*Lead a group for you? Of course I will do that!*";
                case MessageIDs.CommandingLeadGroupFail:
                    return "*I have a contest to practice to.*";
                case MessageIDs.CommandingDisbandGroup:
                    return "*Oh, then everybody should go home.*";
                case MessageIDs.CommandingChangeOrder:
                    return "*Alright everyone, that's what we'll do now.*";
                //Popularity Contest Messages
                case MessageIDs.PopContestMessage:
                    return "*I'm so glad to see that you're interessed in the contest. Do you want to vote now, or want to know more about it?*";
                case MessageIDs.PopContestIntroduction:
                    return "*This event allows Terrarians to vote on their favorite companions. Any companion can participate, regardless of the name, which is awesome! Will you give us your vote?*";
                case MessageIDs.PopContestLinkOpen:
                    return "*Have a nice voting. I hope I see you at the nominations.*";
                case MessageIDs.PopContestOnReturnToOtherTopics:
                    return "*Be sure to vote on someone before the event ends.*";
                case MessageIDs.PopContestResultMessage:
                    return "*They're out! The results of the Popularity Contest are out!! Come on! Let's check it!*";
                case MessageIDs.PopContestResultLinkClickMessage:
                    return "*Let's see if I won.*";
                case MessageIDs.PopContestResultNevermindMessage:
                    return "*Oh, alright. Be sure to check back who won later.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
