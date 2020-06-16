using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
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
        /// </summary>

        public MabelBase()
        {
            Name = "Mabel";
            Description = "She dreams of being a model. And she still pursues it.";
            Size = GuardianSize.Large;
            Width = 30;
            Height = 84;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 17;
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
            DodgeRate = 10;

            PopularityContestsWon = 0;
            ContestSecondPlace = 1;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.EnchantedBoomerang, 1);
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

            //Mounted Position
            MountShoulderPoints.DefaultCoordinate2x = new Point(19, 14);
            MountShoulderPoints.AddFramePoint2x(17, 19, 20);
            SittingPoint = new Point(24 * 2, 37 * 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 12, 4);
            LeftHandPoints.AddFramePoint2x(11, 35, 7);
            LeftHandPoints.AddFramePoint2x(12, 37, 20);
            LeftHandPoints.AddFramePoint2x(13, 33, 32);

            LeftHandPoints.AddFramePoint2x(14, 42, 17);
            LeftHandPoints.AddFramePoint2x(15, 42, 37);

            LeftHandPoints.AddFramePoint2x(18, 12, 4 + 6);
            LeftHandPoints.AddFramePoint2x(19, 35, 9 + 6);
            LeftHandPoints.AddFramePoint2x(20, 37, 20 + 6);

            LeftHandPoints.AddFramePoint2x(23, 39, 41);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 4);
            RightHandPoints.AddFramePoint2x(11, 38, 7);
            RightHandPoints.AddFramePoint2x(12, 40, 20);
            RightHandPoints.AddFramePoint2x(13, 36, 32);

            RightHandPoints.AddFramePoint2x(14, 45, 17);
            RightHandPoints.AddFramePoint2x(15, 45, 37);

            RightHandPoints.AddFramePoint2x(18, 15, 4 + 6);
            RightHandPoints.AddFramePoint2x(19, 38, 9 + 6);
            RightHandPoints.AddFramePoint2x(20, 40, 20 + 6);

            //Headgear Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(22, 12);
            HeadVanityPosition.AddFramePoint2x(14 - 1, 37, 17 + 3);
            HeadVanityPosition.AddFramePoint2x(15 - 1, 39, 25 + 3);
            HeadVanityPosition.AddFramePoint2x(17, 23 - 1, 9 + 7 + 3);

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

        public override string FriendLevelMessage
        {
            get
            {
                return "*I really love this place, It's like as if everybody loves me. I'm like a star in your world?*";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "*Oh hello, I wanted to give you this, for being a good friend. *She hugged you* Tee hee.*";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "*Thanks to the people around, I have enough confidence that I will win the next contest. Just you see, I want you to be watching the contest too.*";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "*I just love everything in this world. The vegetation, the towns, the people, and even the monsters. You're a lucky person for having all that. And so am I for having you as friend.*";
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
            return (Main.rand.NextDouble() < 0.5 ? "*I've been so busy practicing to be a model, that I forgot that I have something to do.*" : "*I just don't have the time to deal with this, so can you do this for me?*");
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
            if (player.Male)
            {
                Mes.Add("*Where are you looking at? My face is a bit more above.*");
            }
            Mes.Add("*Do you think I have even a bit of chance on the Miss North Pole contest?*");
            Mes.Add("*I was trying to fly earlier, to see If I can get into the contest. Maybe they only accept Reindeers on it, so If I could fly, I could at least participate?*");
            Mes.Add("*Everytime I try to join the Miss North Pole contest, they come with a different excuse.*");
            Mes.Add("*The Miss North Pole is a contest that happens some days before new year. Before happens what your people calls \"X-mas\".*");
            if (Main.bloodMoon)
            {
                Mes.Add("*How do I look? How do I look-How do I look-How do I look? \"She's blinking her eyes very fast, about 30 frames per second.\"*");
                Mes.Add("*The night will be ended soon-The night will be ended soon.*");
                Mes.Add("*She seems to be drinking a mug of coffee, or half a dozen, through out the night.*");
                Mes.Add("*You'll keep me safe, right? Right? RIGHT?!*");
                if (AnglerInTown)
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] is safe. Kids doesn't dies, right? They turn into smoke and goes away, right? He's safe,right? Right?*");
            }
            else
            {
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
                        Mes.Add("\"She's humming while looking through the window.\"");
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
    }
}
