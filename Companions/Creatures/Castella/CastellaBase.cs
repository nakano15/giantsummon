using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using giantsummon.Companions.Creatures.Castella;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Companions
{
    public class CastellaBase : GuardianBase
    {
        private const string HairBackTextureID = "hairback";
        private const byte MetamorphosisActionID = 0;

        public CastellaBase()
        {
            Name = "Castella";
            Description = "";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 88;
            FramesInRows = 16;
            DuckingHeight = 56;
            SpriteWidth = 128;
            SpriteHeight = 96;
            Scale = 102f / 96;
            CompanionSlotWeight = 1.1f;
            Age = 36;
            SetBirthday(SEASON_SUMMER, 28);
            DefaultTactic = CombatTactic.Charge;
            Male = false;
            InitialMHP = 250; //1150
            LifeCrystalHPBonus = 40;
            LifeFruitHPBonus = 15;
            Accuracy = 0.36f;
            Mass = 0.5f;
            MaxSpeed = 5.3f;
            Acceleration = 0.21f;
            SlowDown = 0.47f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.81f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();

            //Animations
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
           // HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 23;
            DuckingSwingFrames = new int[] { 24, 25, 26 };
            SittingFrame = 18;
            ChairSittingFrame = 17;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 20;
            BedSleepingFrame = 19;
            //SleepingOffset.X = 32;
            ReviveFrame = 21;
            DownedFrame = 22;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(32, 36);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 0);

            BodyFrontFrameSwap.Add(43, 1);
            BodyFrontFrameSwap.Add(44, 1);

            RightArmFrontFrameSwap.Add(17, 0);
            RightArmFrontFrameSwap.Add(43, 1);

            //Left Hand Positions
            LeftHandPoints.AddFramePoint2x(10, 21, 4);
            LeftHandPoints.AddFramePoint2x(11, 41, 11);
            LeftHandPoints.AddFramePoint2x(12, 46, 23);
            LeftHandPoints.AddFramePoint2x(13, 40, 31);

            LeftHandPoints.AddFramePoint2x(14, 14, 9);
            LeftHandPoints.AddFramePoint2x(15, 46, 7);
            LeftHandPoints.AddFramePoint2x(16, 58, 31);

            LeftHandPoints.AddFramePoint2x(18, 42, 26);

            LeftHandPoints.AddFramePoint2x(21, 40, 37);

            LeftHandPoints.AddFramePoint2x(24, 32, 21);
            LeftHandPoints.AddFramePoint2x(25, 50, 27);
            LeftHandPoints.AddFramePoint2x(26, 45, 41);

            LeftHandPoints.AddFramePoint2x(39, 32, 4);
            LeftHandPoints.AddFramePoint2x(40, 39, 12);
            LeftHandPoints.AddFramePoint2x(41, 43, 20);
            LeftHandPoints.AddFramePoint2x(42, 38, 28);

            LeftHandPoints.AddFramePoint2x(43, 38, 27);
            LeftHandPoints.AddFramePoint2x(44, 38, 27);

            LeftHandPoints.AddFramePoint2x(48, 35, 26);

            LeftHandPoints.AddFramePoint2x(50, 45, 17);
            LeftHandPoints.AddFramePoint2x(51, 52, 35);
            LeftHandPoints.AddFramePoint2x(52, 42, 41);

            LeftHandPoints.AddFramePoint2x(62, 39, 12);

            //Right Hand Positions
            RightHandPoints.AddFramePoint2x(10, 32, 4);
            RightHandPoints.AddFramePoint2x(11, 44, 11);
            RightHandPoints.AddFramePoint2x(12, 48, 23);
            RightHandPoints.AddFramePoint2x(13, 42, 31);

            RightHandPoints.AddFramePoint2x(14, 16, 9);
            RightHandPoints.AddFramePoint2x(15, 48, 7);
            RightHandPoints.AddFramePoint2x(16, 59, 32);

            RightHandPoints.AddFramePoint2x(18, 45, 27);

            RightHandPoints.AddFramePoint2x(21, 44, 37);

            RightHandPoints.AddFramePoint2x(24, 34, 21);
            RightHandPoints.AddFramePoint2x(25, 52, 27);
            RightHandPoints.AddFramePoint2x(26, 47, 42);

            RightHandPoints.AddFramePoint2x(39, 37, 4);
            RightHandPoints.AddFramePoint2x(40, 42, 12);
            RightHandPoints.AddFramePoint2x(41, 47, 20);
            RightHandPoints.AddFramePoint2x(42, 42, 28);

            RightHandPoints.AddFramePoint2x(44, 44, 27);

            RightHandPoints.AddFramePoint2x(48, 49, 36);

            RightHandPoints.AddFramePoint2x(50, 49, 17);
            RightHandPoints.AddFramePoint2x(51, 55, 36);
            RightHandPoints.AddFramePoint2x(52, 48, 41);

            RightHandPoints.AddFramePoint2x(62, 42, 12);

            //Mounted Positions
            MountShoulderPoints.DefaultCoordinate2x = new Point(26, 14);
            MountShoulderPoints.AddFramePoint2x(23, 32, 25);
            MountShoulderPoints.AddFramePoint2x(24, 32, 25);
            MountShoulderPoints.AddFramePoint2x(25, 32, 25);
            MountShoulderPoints.AddFramePoint2x(26, 32, 25);

            MountShoulderPoints.AddFramePoint2x(49, 32, 25);
            MountShoulderPoints.AddFramePoint2x(50, 32, 25);
            MountShoulderPoints.AddFramePoint2x(51, 32, 25);
            MountShoulderPoints.AddFramePoint2x(52, 32, 25);

            MountShoulderPoints.AddFramePoint2x(55, 31, 26);
            MountShoulderPoints.AddFramePoint2x(56, 38, 31);
            MountShoulderPoints.AddFramePoint2x(57, 38, 31);
            MountShoulderPoints.AddFramePoint2x(58, 38, 31);
            MountShoulderPoints.AddFramePoint2x(59, 30, 26);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HairBackTextureID, "hair_back");
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new CastellaData(ID, ModID);
        }

        public override void Attributes(TerraGuardian g) //Add transformation action, and replace frames based on her form.
        {
            //g.AddFlag(GuardianFlags.WerewolfAcc);
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            CastellaData data = (CastellaData)guardian.Data;
            if (!data.LastWereform) return;
            switch (Frame)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    Frame += 29;
                    return;
                case 17:
                case 18:
                    Frame += 26;
                    return;
                case 19:
                    Frame = 46;
                    return;
                case 20:
                    Frame = 45;
                    return;
                case 21:
                    Frame = 48;
                    return;
                case 22:
                    Frame = 47;
                    return;
                case 23:
                case 24:
                case 25:
                case 26:
                    Frame += 26;
                    return;
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            CastellaData data = (CastellaData)guardian.Data;
            bool IsWerewolf = OnWerewolfForm(guardian);
            if (data.LastWereform != IsWerewolf)
            {
                guardian.StartNewGuardianAction(new CastellaMetamorphosis(), MetamorphosisActionID, true);
            }
            data.LastWereform = IsWerewolf;
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Rectangle bodyRect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HairBackTextureID), DrawPosition, bodyRect,
                color, Rotation, Origin, Scale, seffect);
            InjectTextureBefore(GuardianDrawData.TextureType.TGRightArm, gdd);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            if (OnWerewolfForm(guardian))
            {
                switch (Main.rand.Next(3))
                {
                    default:
                        return "*Hello, I am " + guardian.Name + ", and I will hunt you.*";
                    case 1:
                        return "*Generally my prey doesn't try to speak to me, but at least I'll let you know that I'm " + guardian.Name + ".*";
                    case 2:
                        return "*I am " + guardian.Name + ", unhappy to meet me? We'll have enough time to get acquaintanced.*";
                }
            }
            else
            {
                switch (Main.rand.Next(3))
                {
                    default:
                        return "*Um... Hi.. I'm " + guardian.Name + ".*";
                    case 1:
                        return "*Oh... Um.. Hello. " + guardian.Name + ", is my name.*";
                    case 2:
                        return "*Who.. Am I? I am " + guardian.Name + ".*";
                }
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            List<string> Mes = new List<string>();
            Action<string, string> M = new Action<string, string>(delegate(string normal, string were) { if (Wereform) { Mes.Add(were); } else { Mes.Add(normal); } });

            if (guardian.IsSleeping)
            {
                M("(She seems to be sleeping soundly.)", "(As you got close, she gave a sinister smile. Better you back off.)");
                M("(She's murmuring about some kind of king.)", "(She started growling as you got close. If you walk backward slowly...)");
            }
            if (Main.bloodMoon)
            {
                M("*Sorry! I can't talk for much longer. I need to defend myself from hungry undeads!*", "*Don't you see I'm busy killing things?*");
                M("*Didn't you have a worse moment to talk to me?*", "*What? You want me to hunt you? Mind waiting for other day?*");
                M("*What are... Are you alive or dead? Alive?*", "*Don't distract me, we have killing to do!*");
            }
            else if (Main.eclipse)
            {
                M("*Who made those things? I want to hire them.*", "*Good to have things to rip apart.*");
                M("*Why all those monsters seems to be made from Terrarians? Who did that?*", "*I'm busy turning monsters to piece.*");
            }
            else if (guardian.IsUsingToilet)
            {
                M("*Yes, I have necessities too. Mind letting me finish this?*", "*If you don't leave now, I will black you out.*");
                M("*[nickname], this is not the time and place for a conversation.*", "*You really have the nerve of watching me doing my things.*");
            }
            else
            {
                M("*You came to check me?*", "*I'm not interessed in hunting you right now, if that was what you're wanting.*");
                M("*This place looks friendly to me.*", "*So many preys around to catch.*");
                M("*Do I look different some nights? Sorry, I don't actually remember what happens during the nights.*", "*I always leave my prey alive after I finish nibbling them, that way I can try catching them again another time.*");
                M("*It's nice to be outside of my castle for a while.*", "*I'm enjoying my time outside of that castle. There is more space for me to move, and hide.*");
                M("*I hope my servants aren't thrashing my castle during my absence.*", "*If I catch my servants destroying my castle, they will not live to see tomorrow.*");

                if (Main.dayTime)
                {
                    if (!Main.raining)
                    {
                        M("*I really missed this kind of weather when I was in my castle.*", "*It's no fun to chase something during the day, so I will conserve energy for the night.*");
                        M("*Those bird chirping makes this place quite noisy...*", "*There is a reason why I preffer the night: Silence.*");
                        M("*I could surelly nap under a tree like this.*", "*I'm starting to feel a bit bored.*");
                    }
                    else
                    {
                        M("*What a horrible weather out be outside.*", "*Great. Not only it's day, but it's also raining...*");
                        M("*I always enjoyed this kind of weather when I was at my castle.*", "*I hope it isn't raining during the night..*");
                    }
                }
                else
                {
                    M("*Why are you so scared of me?*", "*Want to play a game, [nickname]? Hehehe.*");
                    if (Main.raining)
                    {
                        M("*I'm feeling quite drowzy..*", "*This rain will not stop me from chewing something.*");
                        M("*I now feel like locking myself at home and have some sleep.*", "*You're planning on going outside? Okay.*");
                    }
                    else
                    {
                        M("*Do you know why there are zombies roaming this world?*", "*I really hate it when zombies appear when I'm busy.*");
                        M("*Me? I'm just enjoying the night.*", "*Watch yourself outside, [nickname].*");
                        M("*It's so peaceful here. I like that.*", "*I should definitelly look for something to nibble.*");
                    }
                }
                if (guardian.IsPlayerRoomMate(player))
                {
                    M("*As long as you have your own bed, I don't mind sharing my room with you.*", "*I really enjoy having a chew toy inside my house. It's really convenient, but It's also very boring.*");
                }
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    M("*[gn:" + Rococo + "] keeps asking me what I'm doing, frequently.*", "*[gn:"+Rococo+"] is the only one I actually don't try catching.*");
                }
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    M("*[gn:" + Blue + "] seems to have really liked my hair.*", "*Aaahhh!!! Why my hair can't stay lie [gn:"+Blue+"]'s when I transform?!*");
                    M("*If I am [gn:" + Blue + "]'s parent? No, I'm not.*", "*I wonder why people thinkg [gn:"+Blue+"] and I are parents.*");
                }
                if (NpcMod.HasGuardianNPC(Brutus))
                {
                    M("*I could use a strong bodyguard on my castle, one just like [gn:" + Brutus + "].*", "*For a bodyguard, I can easily manage to blackout [gn:"+Brutus+"] with ease.*");
                    M("*I don't know why [gn:" + Brutus + "] is so eager for the coming of full moon nights.*", "*[gn:"+Brutus+"] looks happy when I nibble him. I think is my eyes playing tricks on me.*");
                }
                if (NpcMod.HasGuardianNPC(Bree))
                {
                    M("*The other day, [gn:" + Bree + "] came to me telling to stop making her husband vanish during nights, and stop using him as chew toy. I can't really do much about that.*", "*If [gn:"+Bree+"] keep bothering me, I'll use her as my chewtoy next time.*");
                }
                if (NpcMod.HasGuardianNPC(Sardine))
                {
                    M("*I had to apologize to [gn:" + Sardine + "] some time ago... For what happened that night...*", "*[gn:"+Sardine+"] really tried to escape from me, but he didn't noticed the tree on his way.*");
                }
                if (NpcMod.HasGuardianNPC(Malisha))
                {
                    M("*[gn:" + Malisha + "] is a witch? I guess this place might be fitting for me then..*", "*That hag [gn:"+Malisha+"] burned my fur when I tried to catch her the other day. I will have my revenge in the future.*");
                    M("*I wonder if [gn:" + Malisha + "] could help me with something... Oh, um... Nevermind.*", "*I think [gn:"+Malisha+"] might actually have a use for me.*");
                }
                if (NpcMod.HasGuardianNPC(Cinnamon))
                {
                    M("*What a cute little girl [gn:" + Cinnamon + "] is, and it's even more impressive that she can cook too.*", "*Whenever I see cute things like [gn:"+Cinnamon+"], I want to try catching them.*");
                }
                if (NpcMod.HasGuardianNPC(Minerva))
                {
                    M("*Hm... I wonder if [gn:" + Minerva + "] would agree to cook for me on my castle, as long as she stays away from me when I'm eating.*", "*Everytime I corner [gn:"+Minerva+"], she buys me with food.*");
                }
                if (NpcMod.HasGuardianNPC(Liebre))
                {
                    M("*You feel uncomfortable with [gn:" + Liebre + "] presence? I wondered so.*", "*It is quite creepy to notice [gn:"+Liebre+"] watching me, when I'm nibbling my prey. He must have been disappointed everytime, since I don't kill them.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            List<string> Mes = new List<string>();
            Action<string, string> M = new Action<string, string>(delegate (string normal, string were) { if (Wereform) { Mes.Add(were); } else { Mes.Add(normal); } });
            M("*As you may notice, I have a kind of lycantropy. My case is specific, since I can't remember things, but I don't end up as feral like other werewolves. That's why you can talk to me in that state.*",
                "*What am I? I'm the feral version of myself, a werewolf you'd say. We are only talking because I'm not as savage as one.*");
            M("*You want to visit my castle? Sorry, but I have to object that.*", "*You lost your mind? Letting you visit my castle? My servants would turn you into their next meal.*");
            M("*Why I was locked inside my castle on my world? It's a complicated story...*", "*I'm so glad to have found this world. This world is a lot better and bigger than the castle I live on. I hardly had anything to hunt there.*");
            M("*Now that you mentioned... I don't remember how I got here.*", "*It's really good that I jumped into that portal, or else I wouldn't be here.*");
            M("*You say that people are disappearing from town sometimes? I don't know what to say... I wonder what my other self is doing.*", "*I hope you don't mind if some of your citizens end up missing. Don't worry, they might be back by morning.*");
            M("*I don't think that my lycantrope version hunts people just to chew them.*", "*I like it when my prey tries to escape from me. It fills me with adrenaline and fun.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (Main.rand.NextDouble() < 0.5f)
                return M("*I don't need anything right now*", "*Other than something to chew, I don't need anything else.*");
            return M("*Right now I don't need anything done.*", "*Maybe if you begin running, and then I... No? Okay.*");
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (Main.rand.NextDouble() < 0.5f)
                return M("*There is actually one thing I need... [objective]. Can you do that?*", "*If you help me [objective], I wont use you as my chew toy for a while. What do you say?*");
            return M("*Yes. [objective] is what I need. What do you say?*", "*Do you think you could help me with [objective]? I have more important things to do right now.*");
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (Main.rand.NextDouble() < 0.5f)
                return M("*Thank you very much for this.*", "*That's my Terrarian. A kiss on the cheek would do for a reward?*");
            return M("*I'm really glad that you managed to help me with that. Thank you.*", "*Congratulations [nickname]. I wont hunt you down for a while as a reward.*");
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (Main.raining)
                return M("*I'm all wet and dripping, eww. Do you have some dry place I could stay?*", "*I smell like wet dog? Don't look at me, you're the one who refuses to give me a house.*");
            if (Main.rand.NextDouble() < 0.5f)
                return M("*Could you at least have some dignity of giving me a house?*", "*As much as I love being in the open space, my other self needs a place to stay.*");
            return M("*Why don't you give me a house to stay? I don't want to return to my castle just yet.*", "*The reason why you don't give me a house is because of the times I blacked you out?*");
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            bool Wereform = OnWerewolfForm(guardian);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian) && Main.rand.NextDouble() < 0.5)
                return M("*Did you... Prepared some kind of surprise to me?*", "*Snif* *Snif Snif* *What is that smell? What are you hiding, [nickname]? Is that a gift?*");
            if (Main.rand.NextDouble() < 0.5)
                return M("*I'm still recovering from this. It has been a long time since someone celebrated my birthday, so I'm in shock here.*", "*I knew that moving to here was a great idea! I didn't had the idea that there were awesome people to celebrate my birthday.*");
            return M("*You guys prepared a birthday part for me? I'm... *Snif* I'm so happy...*", "*You all did all that for me? Is this a trap? Okay, I'm happy, is that what you wanted to hear?*");
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            bool Wereform = OnWerewolfForm(Guardian);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch (Main.rand.Next(3))
            {
                default:
                    return M("*No, no, NO! Wake up! Stay awake!*", "*Oh no! Don't close your eyes! Wake up!*");
                case 1:
                    return M("*Please! Listen to me! Hang on!*", "*Don't die... Don't die... Don't die...*");
                case 2:
                    return M("*Hang on, I'm taking care of your wounds.*", "*You can't die, I wont let it.*");
            }
        }

        public override string CompanionRecruitedMessage(GuardianData WhoJoined, out float Weight) //Need the companion who triggered the message to be referenced.
        {
            return base.CompanionRecruitedMessage(WhoJoined, out Weight);
        }

        public bool OnWerewolfForm(TerraGuardian tg)
        {
            if (tg.Data is CastellaData)
            {
                CastellaData data = (CastellaData)tg.Data;
                return (tg.HasFlag(GuardianFlags.Werewolf) && !Main.dayTime) || (Main.moonPhase == 0 && (!Main.dayTime || tg.HasFlag(GuardianFlags.WerewolfAcc)));
            }
            return tg.HasFlag(GuardianFlags.Werewolf);
        }

        public class CastellaData : GuardianData
        {
            public bool LastWereform = false;

            public CastellaData(int ID, string ModID) : base(ID, ModID)
            {

            }
        }
    }
}
