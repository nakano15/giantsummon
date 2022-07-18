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
        private const string HairBackTextureID = "hairback",HeadWerewolfTextureID = "headwere";
        private const byte MetamorphosisActionID = 0, HuntingActionID = 1;

        public CastellaBase()
        {
            Name = "Castella";
            Description = "A mysterious woman, owner of a castle, \nafflicted by a curse.";
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
            MoveInLevel = 3;

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

            SittingPoint2x = new Point(32, 36);

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

            //Hat Positions
            HeadVanityPosition.DefaultCoordinate2x = new Point(32, 11);
            HeadVanityPosition.AddFramePoint2x(14, 28, 13);
            HeadVanityPosition.AddFramePoint2x(15, 35, 11);
            HeadVanityPosition.AddFramePoint2x(16, 50, 19);

            HeadVanityPosition.AddFramePoint2x(21, 38, 24);

            HeadVanityPosition.AddFramePoint2x(23, 41, 28);
            HeadVanityPosition.AddFramePoint2x(24, 41, 28);
            HeadVanityPosition.AddFramePoint2x(25, 41, 28);
            HeadVanityPosition.AddFramePoint2x(26, 41, 28);

            HeadVanityPosition.AddFramePoint2x(48, 35, 20);

            HeadVanityPosition.AddFramePoint2x(49, 41, 27);
            HeadVanityPosition.AddFramePoint2x(50, 41, 27);
            HeadVanityPosition.AddFramePoint2x(51, 41, 27);
            HeadVanityPosition.AddFramePoint2x(52, 41, 27);

            HeadVanityPosition.AddFramePoint2x(55, 37, 22);
            HeadVanityPosition.AddFramePoint2x(56, 44, 40);
            HeadVanityPosition.AddFramePoint2x(57, 44, 40);
            HeadVanityPosition.AddFramePoint2x(58, 45, 40);
            HeadVanityPosition.AddFramePoint2x(59, 37, 22);

            HeadVanityPosition.AddFramePoint2x(63, 42, 33);
            HeadVanityPosition.AddFramePoint2x(64, 42, 33);
            HeadVanityPosition.AddFramePoint2x(65, 43, 34);
            HeadVanityPosition.AddFramePoint2x(66, 43, 35);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HairBackTextureID, "hair_back");
            sprites.AddExtraTexture(HeadWerewolfTextureID, "head_were");
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new CastellaData(ID, ModID);
        }

        public override void Attributes(TerraGuardian g) //Add transformation action, and replace frames based on her form.
        {
            //g.AddFlag(GuardianFlags.WerewolfAcc);
            if (OnWerewolfForm(g))
            {
                g.MaxSpeed += 1.2f;
                if (g.DashSpeed == 0)
                    g.DashSpeed = MaxSpeed * 2;
                else
                    g.DashSpeed += 2f;
                g.JumpHeight += 0.6f;
                g.MeleeDamageMultiplier += 0.08f;
                g.MeleeCriticalRate += 10;
                g.Defense += 6;
                g.MHP += 1200;
            }
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
            if (IsWerewolf && Main.moonPhase == 0 && !guardian.DoAction.InUse && (guardian.FriendshipLevel < FriendsLevel || guardian.OwnerPos == -1))
            {
                guardian.StartNewGuardianAction(new WereHuntingAction(), HuntingActionID, true);
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

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, SpriteEffects seffect, Vector2 Origin, ref List<GuardianDrawData> gdds)
        {
            if (OnWerewolfForm(guardian))
            {
                foreach(GuardianDrawData gdd in gdds)
                {
                    if(gdd.textureType == GuardianDrawData.TextureType.TGHead)
                    {
                        gdd.Texture = sprites.GetExtraTexture(HeadWerewolfTextureID);
                        break;
                    }
                }
            }
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
                        return "*Generally my prey doesn't try to speak to me, but at least you know that I am " + guardian.Name + ".*";
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
                M("*You came to check me?*", "*I'm not interessed in hunting you right now, if that was what you're wanting to know.*");
                M("*This place is really friendly. I like that.*", "*I really don't like it when people watch me dine. Keep that in mind.*");
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

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight) //Need the companion who triggered the message to be referenced.
        {
            bool Wereform = OnWerewolfForm(WhoReacts);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            Weight = 1f;
            return M("*Oh, hello. I hope... You're friendly.*", "*I hope you remember my face, you will see it whenever I catch you.*");
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            bool Wereform = OnWerewolfForm(WhoReacts);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Malisha:
                        Weight = 1.5f;
                        return M("*I'm starting to like this already.*", "*Finally, a wise choice.*");
                    case Brutus:
                        Weight = 1.5f;
                        return M("*He's going to protect me, right?*", "*I really want to put my paws on you.*");
                    case Bree:
                        Weight = 1.5f;
                        return M("*I'm disliking this already...*", "*I hope you don't bother me.*");
                    case Sardine:
                        Weight = 1.5f;
                        return M("*You think it's safe for him to come with me.*", "*My teeth were needing to bite something.*");
                }
            }
            Weight = 1f;
            return M("*I like having more company.*", "*Just because you joined us doesn't means you're safe from me.*");
        }

        public override string CompanionLeavesGroupMessage(TerraGuardian WhoReacts, GuardianData WhoLeft, out float Weight)
        {
            bool Wereform = OnWerewolfForm(WhoReacts);
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (WhoLeft.ModID == MainMod.mod.Name)
            {
                switch (WhoLeft.ID)
                {
                    case Bree:
                        Weight = 1.5f;
                        return M("*I feel relieved now.*", "*Good riddance.*");
                }
            }
            Weight = 1f;
            return M("*You gotta go? Goodbye.*", "*Watch yourself on your way back... Hehehe..*");
        }

        public override string GetSpecialMessage(string MessageID)
        {
            bool Wereform = false;//OnWerewolfForm(WhoReacts);
            foreach(TerraGuardian tg in MainMod.ActiveGuardians.Values)
            {
                if(tg.ModID == MainMod.mod.Name && tg.ID == Castella)
                {
                    Wereform = OnWerewolfForm(tg);
                    break;
                }
            }
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return M("*You want to be my buddy? Yes, I accept it. That means I should order a bed for you on my castle?*","*Yes, I accept you being my buddy, but doesn't mean you're safe from me. At least you can live in my castle.*");
                case MessageIDs.RescueMessage:
                    return M("*You're safe! Hang on.*", "*I got you. Keep breathing.*");
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return M("*[nickname], it's too late to have a formal chatting. Please, be brief.*", "*What? Just because I'm like this I shouldn't sleep? Or you want me to hunt you?*");
                        case 1:
                            return M("*I'm not in the mood for chatting. What do you want?*", "*Can't I be able to get some rare moment to sleep?*");
                        case 2:
                            return M("*What do you want? It's a really dark night and you want to talk. Go sleep!*", "*[nickname], if you don't stop waking me up, I'll use you as my personal body pillow for the rest of the night.*");
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return M("*W-what? Oh, is it about my request? Be brief, please.*", "*You woke me up. It is really rare of me to sleep, so I hope it's important.*");
                        case 1:
                            return M("*What is it? You did my request? You couldn't have woke me up for no reason, right?*", "*I hope It's about my request, or else I'll make you wake up with pain throughout your body.*");
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return M("*I will join you in your travels.*", "*You want to take me for a walk? Alright. I hope I don't scare you with what I'll do with the monsters.*");
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return M("*There's too many people.*", "*That group is too big. Maybe I could take one member with me, if you don't mind.*");
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return M("*I'm not interessed in travelling right now.*", "*No way. I'd rather try catching someone, and your presence would ruin it.*");
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return M("*Can't I leave the group somewhere safe?*", "*I don't mind going back like this, but wouldn't it be better if I left in a town?*");
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return M("*I'll be back to my house then.*", "*[nickname], remember that you're no longer safe now.*");
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return M("*If you say so, I'll try surviving my way back.*", "*I hope you can run really fast, [nickname].*");
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return M("*Good to see that you're reasonable.*", "*You're safer for now.*");
                case MessageIDs.RequestAccepted:
                    return M("*Please have my request completed.*", "*Leave me happy and I'll give you a friendly nibble.*");
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return M("*I want you to focus on my task, so get your others done.*", "*I don't like having to compete with others requests, take care of those before you try mine.*");
                case MessageIDs.RequestRejected:
                    return M("*Oh... Okay..*", "*Now I didn't liked that. Get out of my sight.*");
                case MessageIDs.RequestPostpone:
                    return M("*I have no hurry, anyways..*", "*Grrr... Fine.*");
                case MessageIDs.RequestFailed:
                    return M("*What a disappointing result, [nickname]. Should I ask someone else to help me?*", "*Grrr... I'm so furious that I could shred something to pieces.*");
                case MessageIDs.RequestAsksIfCompleted:
                    return M("*My request? You completed it, right?*", "*Huh? What about my request? Did you do it?*");
                case MessageIDs.RequestRemindObjective:
                    return M("*You forgot?! Sigh... [objective] is what I need of you... Anything else?*", "*You disturbed me because you forgot my request? [objective] is what I asked you to do, and is what you should be doing now. Go!*");
                case MessageIDs.RestAskForHowLong:
                    return M("*I like the idea of resting. How long should we rest?*", "*You're already tired? I'm still full of energy. Well, how long you'll rest then?*");
                case MessageIDs.RestNotPossible:
                    return M("*Not now!*", "*This looks like a horrible moment to rest.*");
                case MessageIDs.RestWhenGoingSleep:
                    return M("*Yawn... Good night...*", "*While you rest, I'll go try catching something.*");
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return M("*Hm, [shop] has something I must check. Let me get closer.*", "*[shop] seems to have something interesting. Let me check it out.*");
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return M("*Hm...*", "*This... And this...*");
                case MessageIDs.GenericYes:
                    return M("*Yes.*", "*Yes!*");
                case MessageIDs.GenericNo:
                    return M("*No..*", "*No.*");
                case MessageIDs.GenericThankYou:
                    return M("*I appreciate.*", "*Thank you for this.*");
                case MessageIDs.GenericNevermind:
                    return M("*Changed your mind?*", "*Nevermind then!*");
                case MessageIDs.ChatAboutSomething:
                    return M("*You want to know more about me? Alright, what is your question?*", "*You want to know me? That's getting scarily too intimate, but I will answer your questions.*");
                case MessageIDs.NevermindTheChatting:
                    return M("*Had enough questions? Okay. Is there something else you want?*", "*You had enough? Me too. Better we talk about something else.*");
                case MessageIDs.CancelRequestAskIfSure:
                    return M("*You think you can't complete my request? Are you sure you want to drop it?*", "*WHAT?! You want to cancel my request?!*");
                case MessageIDs.CancelRequestYesAnswered:
                    return M("*I'm extremelly disappointed at you, [nickname].*", "*Grr... Why did you accept it then?*");
                case MessageIDs.CancelRequestNoAnswered:
                    return M("*I'm glad that you changed your mind.*", "*I'm still furious about that, though. But at least no longer feel like leaving you with teeth marks.*");
                //
                case MessageIDs.ReviveByOthersHelp:
                    return M("*I thank you all for the help.*", "*Thank you. But don't tell anyone about this.*");
                case MessageIDs.RevivedByRecovery:
                    return M("*I'm fine.. I'm fine...*", "*Grr.. I'm not done yet!*");
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return M("*Argh! My blood!*", "*Vile creature... Grr... You need way more to beat me!*");
                case MessageIDs.AcquiredBurningDebuff:
                    return M("*Aahhhh!! My fur!!*", "*Fire! Fire! Fire!!*");
                case MessageIDs.AcquiredDarknessDebuff:
                    return M("*Ouch! I can't see!*", "*You think you will get away from me?! I can smell and hear you!*");
                case MessageIDs.AcquiredConfusedDebuff:
                    return M("*Everything is spinning!*", "*Which one of those are you? I'll catch you1*");
                case MessageIDs.AcquiredCursedDebuff:
                    return M("*My arms wont obey me!*", "*I can't pounce!!*");
                case MessageIDs.AcquiredSlowDebuff:
                    return M("*I feel sluggish!*", "*I got slowed down! You slowed me down!*");
                case MessageIDs.AcquiredWeakDebuff:
                    return M("*I feel a bit beaten...*", "*I still can tackle you...*");
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return M("*You piece of...*", "*Grrr!! You!*");
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return M("*Oh... My.... What is THAT?!*", "*Wow... [nickname], thank you for invoking my dinner.*");
                case MessageIDs.AcquiredIchorDebuff:
                    return M("*What the... Is this what I think it is?*", "*Hey! Even I don't do that to people!*");
                case MessageIDs.AcquiredChilledDebuff:
                    return M("*S-someone can lend to m-me a c-cotton shirt?*", "*Hey [nickname], want to help warm me up?*");
                case MessageIDs.AcquiredWebbedDebuff:
                    return M("*No. No! No!!*", "*Dare you to approach me!*");
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return M("*Grrr... I'll have your head!*", "*Grrr... Bark! Bow!*");
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return M("*I feel like I can take on the world.*", "*Come at me!*");
                case MessageIDs.AcquiredWellFedBuff:
                    return M("*I have to meet who made this.*", "*Yum.. Delicious. Now I have energy to hunt something.*");
                case MessageIDs.AcquiredDamageBuff:
                    return M("*I'm ready to inflict pain.*", "*Who will challenge me?*");
                case MessageIDs.AcquiredSpeedBuff:
                    return M("*Take my dust!*", "*It will be easier to catch things now.*");
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return M("*I feel better.*", "*I should be able to win fights like this.*");
                case MessageIDs.AcquiredCriticalBuff:
                    return M("*Time to inflict pain!*", "*Even my claws look sharper.*");
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return M("*This should ease things up.*", "*I'm fine with using it on my weapon, but not on my claws.*");
                case MessageIDs.AcquiredTipsyDebuff:
                    return M("*I was needing this.*", "*I could have some meat too.*");
                case MessageIDs.AcquiredHoneyBuff:
                    return M("*I'd swim in a pool full of this.*", "*This should go well with meat.*");
                //
                case MessageIDs.FoundLifeCrystalTile: //TODO : Add dialogues from here and ahead.
                    return M("*I see a life crystal ahead.*", "*Oh, lovely thing ahead.*");
                case MessageIDs.FoundPressurePlateTile:
                    return M("*Watch your step! Pressure plate.*", "*Nothing good might come from stepping on that.*");
                case MessageIDs.FoundMineTile:
                    return M("*Watch your step! A mine.*", "*There's a mine on the ground, beware.*");
                case MessageIDs.FoundDetonatorTile:
                    return M("*I hope you know what you do with that.*", "*Better not pull that.*");
                case MessageIDs.FoundPlanteraTile:
                    return M("*That might be connected to danger.*", "*What a exquisite plant. Smash it.*");
                case MessageIDs.WhenOldOneArmyStarts:
                    return M("*I'll do my best to protect the crystal.*", "*Bring it on!*");
                case MessageIDs.FoundTreasureTile:
                    return M("*Open it!*", "*I'm curious. What is inside?*");
                case MessageIDs.FoundGemTile:
                    return M("*I could have some jewels of those.*", "*You will gift me those, will you?*");
                case MessageIDs.FoundRareOreTile:
                    return M("*You might be interessed in this.*", "*That's shiny, you might be interessed on it.*");
                case MessageIDs.FoundVeryRareOreTile:
                    return M("*That looks rare.*", "*You might want to dig this.*");
                case MessageIDs.FoundMinecartRailTile:
                    return M("*I wonder where that will lead us.*", "*You want to ride that, don't you?*");
                //
                case MessageIDs.TeleportHomeMessage:
                    return M("*Yes, I had enough of exploring too.*", "*Already? Aww..*");
                case MessageIDs.CompanionInvokesAMinion:
                    return M("*I could use my castle minions, but those will do.*", "*I hope they can help pinning my preys.*");
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return M("*I just had to leave my castle to begin seeing weird things.*", "*Why do you accept his hugs but not mine?*");
                //
                case MessageIDs.LeaderFallsMessage:
                    return M("*[nickname]!*", "*[nickname]! No!*");
                case MessageIDs.LeaderDiesMessage:
                    return "*NO! [nickname]!!!*";
                case MessageIDs.AllyFallsMessage:
                    return M("*Someone was knocked out!*", "*Someone fell!*");
                case MessageIDs.SpotsRareTreasure:
                    return M("*Look! That look good.*", "*My eyes sees something unusual there.*");
                case MessageIDs.LeavingToSellLoot:
                    return M("*I'll be right back.*", "*You wont even notice me gone.*");
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return M("*You better watch yourself, [nickname].*", "*Come on, you don't want me to carry you, don't you?*");
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return M("*I'm reaching my limit...*", "*I'm not done yet.*");
                case MessageIDs.RunningOutOfPotions:
                    return M("*I'm running out of potions!*", "*I'm running out of potions!*");
                case MessageIDs.UsesLastPotion:
                    return M("*That was my last potion!*", "*I don't have any more potions!*");
                case MessageIDs.SpottedABoss:
                    return M("*Watch out!*", "*I can't wait to make you fall!*");
                case MessageIDs.DefeatedABoss:
                    return M("*It shouldn't have tried to mess with me.*", "*What should I jump on next?*");
                case MessageIDs.InvasionBegins:
                    return M("*I hope you're ready.*", "*Looks like they made a mistake of appearing with me around.*");
                case MessageIDs.RepelledInvasion:
                    return M("*Everyone is okay? Good.*", "*That was it? Disappointing! My teeth still want to bite things.*");
                case MessageIDs.EventBegins:
                    return M("*I don't like this...*", "*That doesn't seems natural, I like it.*");
                case MessageIDs.EventEnds:
                    return M("*I'm glad it's over.*", "*Too bad that it's over already.*");
                case MessageIDs.RescueComingMessage:
                    return M("*Hang on! I'm coming.*", "*Worry not, I'll get you!*");
                case MessageIDs.RescueGotMessage:
                    return M("*You stay with me for a while.*", "*Got you, now don't you die.*");
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return M("*I have been speaking a lot with [player] recently.*", "*You're not the only prey I've got. [player] is another one I tend to hunt.*");
                case MessageIDs.FeatMentionBossDefeat:
                    return M("*Looks like [player] managed to take down [subject].*", "*I heard about [player] beating up [subject]. What a show off.*");
                case MessageIDs.FeatFoundSomethingGood:
                    return M("*[player] has found a [subject] recently. Did you got anything cool too?*", "*People are all drolling because [player] got a [subject]. I don't see what's so special about that.*");
                case MessageIDs.FeatEventFinished:
                    return M("*It seems like [player] has a [subject] happening in their presence.*", "*I wonder where was you when a [subject] happened? I know [player] was there.*");
                case MessageIDs.FeatMetSomeoneNew:
                    return M("*It seems like [player] met [subject] recently.*", "*Looks like [player] found me a new prey. They're called [subject].*");
                case MessageIDs.FeatPlayerDied:
                    return M("*I'm still saddened about [player]'s death. They were a good friend to me.*", "*Don't look at me, it wasn't me who killed [player]. I'd never do that to a friend.*");
                case MessageIDs.FeatOpenTemple:
                    return M("*I'm curious about what might be hidden inside the temple [player] discovered...*", "*[player] unlocked the entrance to a spooky temple. I wonder what I could find inside.*");
                case MessageIDs.FeatCoinPortal:
                    return M("*[player] must be feeling lucky, after witnessing a coin portal appear before them.*", "*You should have seen [player] running left to right when that coin portal appeared.*");
                case MessageIDs.FeatPlayerMetMe:
                    return M("*I've met [player] recently. I hope they're nice to me.*", "*I've met a new victim recently. They told me they're named [player].*");
                case MessageIDs.FeatCompletedAnglerQuests:
                    return M("*I don't see why [player] keep helping that kid. He's awful.*", "*Only [player] might have patience to deal with that kid.*");
                case MessageIDs.FeatKilledMoonLord:
                    return M("*I think I might have underestimated [player]. They are strong enough to defeat godly creatures.*", "*I wouldn't believe if someone told me that [player] defeated a godly creature, even more since I can knock them out with ease.*");
                case MessageIDs.FeatStartedHardMode:
                    return M("*Looks like things changed on [subject].*", "*New things to kill have appeared recently on [subject].*");
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return M("*I've heard about [player] picking [subject] as their buddy. I wish them a healthy life.*", "*Awww.. It will be harder to hunt down [player] with [subject] around. At least I hope they be good buddies to each other.*");
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return M("*I'm still happy about you picking me as your buddy. You are also a welcome guest on my castle too.*", "*I'm really glad that you are now my buddy, now I have my personal chew toy. Don't worry, I won't hurt you when biting.*");
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return M("*Looks like [subject] has moved in to [world]. That place must be nice to live.*", "*[subject] thinks that can escape from me by moving to [world].*");
                case MessageIDs.DeliveryGiveItem:
                    return M("*[target]! Take this [item].*", "*Hey [target]! Fetch.*");
                case MessageIDs.DeliveryItemMissing:
                    return M("*I thought I had... Nevermind...*", "*What? My head must not be right, I thought I had... Oh well...*");
                case MessageIDs.DeliveryInventoryFull:
                    return M("*[target], I can't give you something until you take care of your inventory.*", "*[target]! Clean your inventory, now!*");
                case MessageIDs.CommandingLeadGroupSuccess:
                    return M("*Very well. I shall carry a group with me then.*", "*I can do that, as long as they help me catch something.*");
                case MessageIDs.CommandingLeadGroupFail:
                    return M("*I hardly know you. Why should I?*", "*I preffer to hunt alone.*");
                case MessageIDs.CommandingDisbandGroup:
                    return M("*Well, was fun while it lasted.*", "*Perfect. Now I will no longer attract attention.*");
                case MessageIDs.CommandingChangeOrder:
                    return M("*I shall change how I lead my group, then.*", "*If that's how you want me to do things, I will do so.*");
                //Popularity Contest Messages
                case MessageIDs.PopContestMessage:
                    return M("*Hey, [nickname]. I'll be helping hosting the TerraGuardians Popularity contest, do you plan on participating?*", "*[nickname], the TerraGuardians Popularity contest is up. Are you going to vote for me? I will bite you less harder if you do.*");
                case MessageIDs.PopContestIntroduction:
                    return M("*You want to know more? From what people told me, is a contest where Terrarians can vote on their favorite companions. That instantly made me interessed.*", "*You don't know what that is? People like you vote on your favorite companions, so they see who wins. Of course I'm gonna win, why wouldn't I?*");
                case MessageIDs.PopContestLinkOpen:
                    return M("*Be sure to pick everyone you like on the contest, before you send your vote.*", "*Be sure to vote for me! Ah, and also vote on some other people you might like too, just so you don't say I'm egoist.*");
                case MessageIDs.PopContestOnReturnToOtherTopics:
                    return M("*Want to speak about something else?*", "*So, are you going to vote, or what?*");
                case MessageIDs.PopContestResultMessage:
                    return M("*[nickname], the results are out. I can show you the results if you want.*", "*[nickname]! [nickname]! They're out! The results of the Popularity Contest came! Hurry! Let's check it out!*");
                case MessageIDs.PopContestResultLinkClickMessage:
                    return M("*I'm just as curious as you, let's see.*", "*So, did I win? Come on, tell me!*");
                case MessageIDs.PopContestResultNevermindMessage:
                    return M("*Not now? Alright. You can check that another time then.*", "*Now now? Come on [nickname], don't leave me curious!*");
            }
            return base.GetSpecialMessage(MessageID);
        }

        public static bool OnWerewolfForm(TerraGuardian tg)
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
