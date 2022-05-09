using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using giantsummon.Trigger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Companions
{
    public class WrathBase : PigGuardianFragmentBase
    {
        public const string WrathDevilBodyTextureID = "wrath_devil_body", WrathDevilBodyFrontTextureID = "wrath_devil_bodyf",
            WrathDevilLeftArmTextureID = "wrath_devil_larm", WrathDevilRightArmTextureID = "wrath_devil_rarm", WrathDevilRightArmFTextureID = "wrath_devil_rarmf";
        public const int WrathDevilSkinID = 1;

        public WrathBase() //I'll need to think how I'll make the cloud form of them work, and toggle.
            : base(AngerPigGuardianID)
        {
            Name = "Wrath";
            PossibleNames = new string[] { "Wrath", "Rage", "Fury", "Irk" };
            Description = "The angry emotional pig piece\nof a TerraGuardian. Very volatile.";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 70;
            SpriteHeight = 68;
            FramesInRows = 28;
            //DuckingHeight = 54;
            //Each pig should definitelly have the same size, birthday age and time, so I moved those infos.
            DefaultTactic = CombatTactic.Charge;
            Genderless = true;
            InitialMHP = 110; //320
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 5;
            Accuracy = 0.67f;
            Mass = 0.40f;
            MaxSpeed = 3.62f;
            Acceleration = 0.12f;
            SlowDown = 0.35f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            DontUseHeavyWeapons = true;
            DrinksBeverage = false;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.RedPhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(1, 0);
            RightArmFrontFrameSwap.Add(2, 1);
            RightArmFrontFrameSwap.Add(3, 2);
            RightArmFrontFrameSwap.Add(4, 2);
            RightArmFrontFrameSwap.Add(5, 1);
            RightArmFrontFrameSwap.Add(6, 0);
            RightArmFrontFrameSwap.Add(7, 0);
            RightArmFrontFrameSwap.Add(8, 0);
            //RightArmFrontFrameSwap.Add(9, 0);
            //RightArmFrontFrameSwap.Add(10, 0);

            //Animation Frames

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 11, 4);
            LeftHandPoints.AddFramePoint2x(11, 23, 11);
            LeftHandPoints.AddFramePoint2x(12, 24, 19);
            LeftHandPoints.AddFramePoint2x(13, 22, 24);

            LeftHandPoints.AddFramePoint2x(17, 25, 28);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 4);
            RightHandPoints.AddFramePoint2x(11, 25, 11);
            RightHandPoints.AddFramePoint2x(12, 27, 19);
            RightHandPoints.AddFramePoint2x(13, 23, 24);

            RightHandPoints.AddFramePoint2x(17, 27, 28);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16 + 2, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23 + 2, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23 + 2, 18);

            GetRequests();
            SkinsAndOutfits();
        }

        public void SkinsAndOutfits()
        {
            AddSkin(WrathDevilSkinID, "Devil Outfit", delegate (GuardianData gd, Player player)
            {
                return gd.HasItem(Terraria.ModLoader.ModContent.ItemType<Items.Outfit.Wrath.UnholyAmulet>());
            });
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(WrathDevilBodyTextureID, "Wrath_Devil");
            sprites.AddExtraTexture(WrathDevilBodyFrontTextureID, "Wrath_Devil_BodyF");
            sprites.AddExtraTexture(WrathDevilLeftArmTextureID, "Wrath_Devil_LeftArm");
            sprites.AddExtraTexture(WrathDevilRightArmTextureID, "Wrath_Devil_RightArm");
            sprites.AddExtraTexture(WrathDevilRightArmFTextureID, "Wrath_Devil_RightArmF");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            switch (guardian.SkinID)
            {
                case WrathDevilSkinID:
                    {
                        Texture2D Body = sprites.GetExtraTexture(WrathDevilBodyTextureID),
                            BodyFront = sprites.GetExtraTexture(WrathDevilBodyFrontTextureID),
                            LeftArm = sprites.GetExtraTexture(WrathDevilLeftArmTextureID),
                            RightArm = sprites.GetExtraTexture(WrathDevilRightArmTextureID),
                            RightArmF = sprites.GetExtraTexture(WrathDevilRightArmFTextureID);
                        ReplaceTexture(GuardianDrawData.TextureType.TGBody, Body);
                        ReplaceTexture(GuardianDrawData.TextureType.TGBodyFront, BodyFront);
                        ReplaceTexture(GuardianDrawData.TextureType.TGLeftArm, LeftArm);
                        ReplaceTexture(GuardianDrawData.TextureType.TGRightArm, RightArm);
                        ReplaceTexture(GuardianDrawData.TextureType.TGRightArmFront, RightArmF);
                    }
                    break;
            }
            base.GuardianPostDrawScript(guardian, DrawPosition, color, armorColor, Rotation, Origin, Scale, seffect);
        }

        public void GetRequests()
        {
            /*AddNewRequest("Sublimation", 350, 
                "*This form is revolting, I'm boiling out of rage due to this. There must be a way of making me solid again, maybe that nerdy guy can help me in this, let's talk to him.*",
                "*Great, or else I would give you a beating.*",
                "*Oh you...! (Insert several different insults here)*",
                "*What are you waiting for?! I'M SICK OF BEING A CLOUD YOU FOOL!*");
            AddRequestRequirement(delegate(Player player)
            {
                return PlayerMod.PlayerHasGuardian(player, Leopold) && !player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs;
            });
            AddTalkToGuardianRequest("*Hm... Actually, I do know about his condition. It seems like his body has vaporized at the moment his personality was split. I can try doing something to make his personality solid, but I can only merge his personalities if you find them.*", Leopold);*/
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.05f;
            g.Defense -= 4;
        }

        /*public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            if(trigger == TriggerTypes.Hurt)
            {
                if(Sender.TargetType == TriggerTarget.TargetTypes.TerraGuardian)
                {
                    if(Sender.TargetID == guardian.WhoAmID)
                    {
                        if (!guardian.HasBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.BurningRage>()))
                        {
                            int BuffChance = 1 + (int)((1f - (float)guardian.HP / guardian.MHP) * 10);
                            if(Main.rand.Next(100) < BuffChance)
                            {
                                guardian.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.BurningRage>(), 45 * 60);
                                guardian.SaySomething("*You are in trouble now!*");
                            }
                        }
                    }
                }
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }*/

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm furious, why I'm furious? I don't know! This is pissing me off!!!*");
            Mes.Add("*Agh!GRRRRR!! UUGGGHHHHH!*");
            Mes.Add("*Who are you?! What?! Something's funny with my face?! Want to taste these hands?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*UGHHH! what is there to talk about?!*");
            Mes.Add("*Stay away! I'm not in the mood! I never am!*");
            Mes.Add("*No talking, only smashing!*");
            Mes.Add("*What do you want?!*");
            Mes.Add("*Just looking at things aggravates me, I need something demolish!*");
            bool CloudForm = player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Companions.PigGuardianFragmentBase.AngerPigGuardianID];
            if (CloudForm)
            {
                Mes.Add("*Don't dare joke about my current form. DON'T... YOU... DARE!*");
                Mes.Add("*Being intangible weakens me, I need a more solid form to pound people harder!*");
            }
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Perfect! more faces to pound!*");
                    Mes.Add("*Bring them on! I'll take care of them!*");
                }
                else
                {
                    Mes.Add("*That lush green grass and the bird chirping sounds is driving me nuts!*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*HAHAHA TONIGHTS MENU?! UNDEAD BUTT CHEEKS!!!*");
                    Mes.Add("*More undead skulls to bash!*");
                }
                else
                {
                    Mes.Add("*Urgh! All those \"Grahs\" during the night are infuriating me! I'm about to go outside and kick their undead a**!*");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*Don't you know privacy! Go away! Im taking a dump here!*");
                Mes.Add("*Want me to put your flush your head in the toilet?! GO AWAY!*");
            }
            if (Main.raining)
            {
                Mes.Add("*Great!, It couldn't get worse could it?!, now I have to be annoyed by rain drops!*");
                Mes.Add("*OG MY GOD! THE SPLASHES ARE INFURIATING!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Alex))
                Mes.Add("*GO AWAY!. Pftt, I thought It was [gn:"+GuardianBase.Alex+"] wanting to play.*");
            if (NpcMod.HasGuardianNPC(GuardianBase.Brutus))
                Mes.Add("Looking at [gn:"+GuardianBase.Brutus+"] try to be tuff makes me want to beat him to a pulp!");
            if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
            {
                Mes.Add("How many times do I have to tell [gn:"+GuardianBase.Malisha+"] that.. I'M NOT GOING TO PARTICIPATE IN ANY OF YOUR GOD DANM EXPERIMENTS!!!.");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
            {
                if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
                    Mes.Add("*The bunny may know something about me that I dont?! Thats maddening!*");
                else
                    Mes.Add("*Tell the white bunny to help me change forms now!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Sardine))
            {
                Mes.Add("*[gn:" + Sardine + "] is the only reason I haven't pounded anyone here since he keeps me busy with fighting monsters.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Domino))
            {
                Mes.Add("*If I ever see [gn:" + Domino + "] making another joke about me, I'll turn his other eye into a sunny side up egg!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Vladimir))
            {
                Mes.Add("*I dont care how big [gn:"+GuardianBase.Vladimir+"] is, I'll pummel his fat a** until he becomes a malnourished bear!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Fluffles))
            {
                if (CloudForm)
                    Mes.Add("*So what im a ghost for the moment? just dont compare me to [gn:" + Fluffles + "]!*");
            }
            if (NpcMod.HasGuardianNPC(Fear))
            {
                Mes.Add("*At least [gn:" + Fear + "] knows what to do when I'm around, to just stay out of my away. But Its really annoying when he screams like a little b*tch.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Great... A room mate. Don't try anything unless you want your a** beat.*");
                if(CloudForm)
                    Mes.Add("*Don't think that we can share beds since I'm not fully tangible.*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (!guardian.KnockedOutCold)
                {
                    Mes.Add("*Ngh... Just you wait... Until I stand up...* (He seems very furious to the one who defeated him.)");
                    Mes.Add("*Ugh... Im boiling right now!.. Pain is increasing!... The rage rises!... Cough...* (He seems very furious to the one who defeated him.)");
                }
                else
                {
                    Mes.Add("(His body is vibrating while he's passed out.)");
                    Mes.Add("(You can hear loud breathing noises coming from his nose.)");
                }
            }
            else if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("(He growls while sleeping, like as if was going to bite someone.)");
                Mes.Add("(He doesn't seems to be having a very peaceful sleep, because of the constant movements he does.)");
                Mes.Add("(It looks like he's fighting against someone in his sleep.)");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Does It look like I'm the same as her?! I'm actually alive!*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I temble out of rage a lot. Stay out of my way for your own safety.*");
            Mes.Add("*I still don't remember anything from before I woke up. I wonder who was I before what ever made me unconscious, it really pisses me off not knowing!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if(!guardian.request.IsTravelRequest)
                Mes.Add("*I'm too mad for this right now. You should do It instead! Just [objective]!*");
            Mes.Add("*Grrr!! There is something I should do that is making me angry, but I can't do it myself. You should [objective]?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*NO!*");
            Mes.Add("*I don't! There isn't anything!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I will direct my animosity else where for now.*");
            Mes.Add("*Okay, I wont hurt you for a few hours, is that a good enough reward?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*No, I'm not happy, I can never be happy!*");
            Mes.Add("*All those people dancing around, meanwhile I'm here, this is making me furious!!*");
            if (!PlayerMod.HasGuardianBeenGifted(player, Wrath))
                Mes.Add("*This gift better be good.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*Come on! get up now!*");
                Mes.Add("*I didn't agree to baby sit you!*");
                Mes.Add("*Rise up now or I'll give you a worse fate then death!*");
            }
            else
            {
                Mes.Add("*Hey, get up before I bash your head in more!*");
                Mes.Add("*I hope you aren't acting like this on purpose, because if you are your as good as dead regardless.*");
                Mes.Add("*This is already making me mad!*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I need a house! Now!*");
            Mes.Add("*This delay is driving me nuts! How long until you give me a house?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompanionRecruitedMessage(GuardianData WhoJoined, out float Weight)
        {
            Weight = 1f;
            return "*Another person, are you serious!*";
        }

        public override string CompanionJoinGroupMessage(GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {

                }
            }
            Weight = 1f;
            return "*Grrr. you wont get a welcome from me!*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*Your lucky you picked me because I was inches away from pounding you!*";
                case MessageIDs.RescueMessage:
                    return "*When you wake up, tell me who did this to you so i can beat their a**.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*You got the nerve of waking me up in the middle of the night. Do you want your sh*t kicked in?!!!*";
                        case 1:
                            return "*You really woke me up!? are you trying to get your a** kicked!*";
                        case 2:
                            return "*Grrr... I already have trouble sleeping and you decided to wake me up?!*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Please, tell me you did my request, tell me!, because If not im going on a rampage!*";
                        case 1:
                            return "*You better have completed my request, because im very close to smashing stuff.*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Good, I could use the opposition as anger relief.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's too many people! I hate mobs! This makes me aggravated!*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Not now!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*You're crazy?! You plan on leaving me here?!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Grr... whatever.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Don't ask me to join back!...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*I thought so.*";
                case MessageIDs.RequestAccepted:
                    return "*Hurry up!*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*No more. Go deal with your other requests first!*";
                case MessageIDs.RequestRejected:
                    return "*Fine! I'll do It myself as always!*";
                case MessageIDs.RequestPostpone:
                    return "*What?! But I wanted It now!*";
                case MessageIDs.RequestFailed:
                    return "*WHAT? SOME BODY'S A** IS ABOUT TO GET KICKED! *";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*What?! You did my request?!*";
                case MessageIDs.RequestRemindObjective:
                    return "*You what?! Fine! Here it goes! [objective]! That is it! Need me to nail that in your head?*";
                case MessageIDs.RestAskForHowLong:
                    return "*Hmph, how long?!*";
                case MessageIDs.RestNotPossible:
                    return "*This is a horrible moment for that!*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*You'll be sorry If you make me fall from the bed.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Wait! [shop] has something I need!*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Don't you dare leave me behind!*";
                case MessageIDs.GenericYes:
                    return "*YES!*";
                case MessageIDs.GenericNo:
                    return "*NO!*";
                case MessageIDs.GenericThankYou:
                    return "*THANKS!*";
                case MessageIDs.ChatAboutSomething:
                    return "*Grrr... whatever. Don't annoy me.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Finally!*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You what?! Tell me that you really didn't mean that!*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Urgh... Whatever then... I'll do it myself. No thanks to you, of course!*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*So you just wanted to irritate me?!*";
                case MessageIDs.LeopoldMessage1:
                    return "*He's already starting to annoy me. I might just pound him.*";
                case MessageIDs.LeopoldMessage2:
                    return "*What?! why do you want to attack me? I'm trying to save you!*";
                case MessageIDs.LeopoldMessage3:
                    return "*Save me?! I'm following that Terrarian because they give me monsters to smash.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Ok im healed get off me!*";
                    return "*Im still as furios!*";
                case MessageIDs.RevivedByRecovery:
                    return "*I hate you all! I didn't need your help anyway?!*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Agh!! Poison!*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Fire! Fire! THIS REALLY PISSES ME OFF!!!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*I fight in the darkness all the time, this is no different!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*I don't care how many of you there are, I'll still smack you down!*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*What the hell?! I cant use my arms!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*Oh my god! WHY AM I SO SLOW!!!*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*ME WEAK?! I could never be!*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Who cares about armor?!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*Great! Something worth unleashing my rage on!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*You piece of sh*t!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*Argh! its cold!*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*Get me off here now!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*RAAAAAAAAAAAAAAHHHHH!!!!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Come on! Hit me!*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Now I can kick a** while on full stomach!!*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Now you made me mad!*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Rush!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Try taking me down now!*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*Harder Striking!*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*This should be fun.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Heart stuff!*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*A trap!*";
                case MessageIDs.FoundMineTile:
                    return "*I'll find whoever placed that.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Don't even think about it.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Whats that? something to smash I hope.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Perfect, lots of faces to smash.*";
                case MessageIDs.FoundTreasureTile:
                    return "*That better not waste our time.*";
                case MessageIDs.FoundGemTile:
                    return "*Gems! There!*";
                case MessageIDs.FoundRareOreTile:
                    return "*Ores here!*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*You need to see that!*";
                case MessageIDs.FoundMinecartRailTile:
                    return "";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Let's get out of here!*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Summon!*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Why do you let him sumther you like that?!*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname]! You're still weak!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*NO!!! YOU SHOULDN'T HAVE DONE THAT!!!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone is down!!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*LOOT!*";
                case MessageIDs.LeavingToSellLoot:
                    return "*Ok whatever, I'll sell this trashy loot.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Watch your health, fool!*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*That's all you've got?!*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Low on potions here!*";
                case MessageIDs.UsesLastPotion:
                    return "*No more potions left!!*";
                case MessageIDs.SpottedABoss:
                    return "*Time to take my pummeling to the big league!*";
                case MessageIDs.DefeatedABoss:
                    return "*Boom!*";
                case MessageIDs.InvasionBegins:
                    return "*Perfect! I was in need of punching bags.*";
                case MessageIDs.RepelledInvasion:
                    return "*Anyone else?! Come on!*";
                case MessageIDs.EventBegins:
                    return "*I was born for this! Bring it on!*";
                case MessageIDs.EventEnds:
                    return "*Already over?! NO! I wanted to smash more faces!*";
                case MessageIDs.RescueComingMessage:
                    return "*What!? No way you're dying!!*";
                case MessageIDs.RescueGotMessage:
                    return "*Wake up now! before I put you back to sleep!*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*Do you know [player]? They somehow managed to defeat me when we met.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*I heard that [player] killed [subject]. I wish I woudlve did it instead!*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*[player] has found a [subject] in their travels!*";
                case MessageIDs.FeatEventFinished:
                    return "*I ended up with sore arms due to beating up several creatures during a [subject]. Good thing [player] managed to end it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*I don't care if [player] has met someone new. If you do, their name was [subject].*";
                case MessageIDs.FeatPlayerDied:
                    return "*I'm not in the mood! [player] died and I could do nothing! NOTHING!! Damn!!*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] opened the door of some kind of temple at [subject]. Luckily there are things to beat in there.*";
                case MessageIDs.FeatCoinPortal:
                    return "*[player] bumped into a coin portal during their travel.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*Yeah, I met [player]. Now what?*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*Maybe I should try fishing. It seems to be working for [player] to manage their patience.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*You should have seen the beating I gave to Moon Lord at [subject]. [player] just helped a bit with the dps, though.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*Even though freaky creatures begun appearing at [subject], beating them up doesn't help much for me.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*Yeah, [player] got [subject] as their buddy! I don't care.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Yu can call me your buddy but I can careless about being buddies. Im always mad and forever will be so what?! gonna cry?!*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*Yes, [subject] got a new house at [world], no one cares.*";
                case MessageIDs.DeliveryGiveItem:
                    return "*You better need this [item], [target].*";
                case MessageIDs.DeliveryItemMissing:
                    return "*What?! Where is it? The item!*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*Your bag is full, [target]!*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
