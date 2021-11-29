using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class DominoBase : GuardianBase
    {
        public DominoBase()
        {
            Name = "Domino";
            Description = "A sly smuggler from the Ether Realm.";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 84;
            CompanionSlotWeight = 1.4f;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 96f / 84;
            Age = 26;
            SetBirthday(SEASON_SPRING, 22);
            Male = true;
            InitialMHP = 200; //1000
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 25;
            Accuracy = 0.97f;
            Mass = 0.5f;
            MaxSpeed = 5.6f;
            Acceleration = 0.22f;
            SlowDown = 0.37f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.45f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            IsNocturnal = true;
            SetTerraGuardian();
            CallUnlockLevel = 2;
            MountUnlockLevel = 6;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.Handgun, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.MusketBall, 999);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 14;
            DuckingSwingFrames = new int[] { 15, 16, 14 };
            SittingFrame = 17;
            ChairSittingFrame = 18;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 12 });
            ThroneSittingFrame = 19;
            BedSleepingFrame = 20;
            SleepingOffset.X = 16;
            ReviveFrame = 21;
            DownedFrame = 22;

            BackwardStanding = 24;
            BackwardRevive = 25;

            BodyFrontFrameSwap.Add(17, 0);
            SpecificBodyFrontFramePositions = true;

            //Mounted Position
            MountShoulderPoints.DefaultCoordinate2x = new Point(16, 14);

            MountShoulderPoints.AddFramePoint2x(14, 28, 20);
            SittingPoint = new Point(25 * 2, 36 * 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 14, 5);
            LeftHandPoints.AddFramePoint2x(11, 32, 11);
            LeftHandPoints.AddFramePoint2x(12, 35, 19);
            LeftHandPoints.AddFramePoint2x(13, 31, 28);

            LeftHandPoints.AddFramePoint2x(14, 39, 30);
            LeftHandPoints.AddFramePoint2x(15, 30, 11);
            LeftHandPoints.AddFramePoint2x(16, 42, 16);

            LeftHandPoints.AddFramePoint2x(21, 43, 40);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 17, 5);
            RightHandPoints.AddFramePoint2x(11, 34, 11);
            RightHandPoints.AddFramePoint2x(12, 39, 19);
            RightHandPoints.AddFramePoint2x(13, 35, 28);

            RightHandPoints.AddFramePoint2x(14, 41, 30);
            RightHandPoints.AddFramePoint2x(15, 33, 11);
            RightHandPoints.AddFramePoint2x(16, 44, 16);

            //Head Vanity Pos
            HeadVanityPosition.DefaultCoordinate2x = new Point(23, 11);
            HeadVanityPosition.AddFramePoint2x(14, 35, 17);

            HeadVanityPosition.AddFramePoint2x(21, 35, 17);

            RequestList();
        }

        public override void SetupShop(int GuardianID, string GuardianModID)
        {
            GuardianShopHandler.GuardianShop shop = CreateShop(GuardianID, GuardianModID);
            GuardianShopHandler.GuardianShopItem item = Shop_AddItem(shop, ModContent.ItemType<Items.Cannon.IronCannon>()); //The modloader isn't being able to find those items, for some reason.
            item = Shop_AddItem(shop, ModContent.ItemType<Items.Cannon.CannonShell>(), -1, "", 25);
            item = Shop_AddItem(shop, Terraria.ID.ItemID.FlintlockPistol);
            item = Shop_AddItem(shop, Terraria.ID.ItemID.MusketBall, -1, "", 50);
            item = Shop_AddItem(shop, ModContent.ItemType<Items.Accessories.TwoHandedMastery>());
            item.SetToBeRandomlySold(0.05f);
            item = Shop_AddItem(shop, ModContent.ItemType<Items.Accessories.HermesSandals>());
            item.SetToBeRandomlySold(0.05f);
            item = Shop_AddItem(shop, ModContent.ItemType<Items.Accessories.FirstSymbol>());
            item = Shop_AddItem(shop, Terraria.ID.ItemID.IllegalGunParts);
            item.SetLimitedDisponibility(GuardianShopHandler.GuardianShopItem.DisponibilityTime.Night);
            item = Shop_AddItem(shop, Terraria.ID.ItemID.Minishark);
            item = Shop_AddItem(shop, Terraria.ID.ItemID.Shotgun);
            item.GetIfItemIsDisponible = delegate(Player player) { return Main.hardMode; };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.SilverBullet, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return Main.bloodMoon; };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.EmptyBullet, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return Main.hardMode; };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.StyngerBolt, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return player.HasItem(Terraria.ID.ItemID.Stynger); };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.Stake, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return player.HasItem(Terraria.ID.ItemID.StakeLauncher); };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.Nail, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return player.HasItem(Terraria.ID.ItemID.NailGun); };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.CandyCorn, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return player.HasItem(Terraria.ID.ItemID.CandyCornRifle); };
            item = Shop_AddItem(shop, Terraria.ID.ItemID.ExplosiveJackOLantern, -1, "", 50);
            item.GetIfItemIsDisponible = delegate(Player player) { return player.HasItem(Terraria.ID.ItemID.JackOLanternLauncher); };
        }

        public override void Attributes(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.CanDualWield);
        }

        public void RequestList()
        {
            AddNewRequest("Pincer Bane", 220, 
                "*Hey [nickname], I have a client looking for Snatcher Jaws for some reason, would you like to help me with this? I can share the profit with you.*",
                "*Perfect. Snatchers can be found in the Jungle. Don't get too close to them, in other words, use guns. Also, try not to damage the jaws too much, I get more profit if they are intact.*",
                "*Psh. Go away.*",
                "*Good job, here is your share. If someone asks, we didn't see each other.*",
                "*Lost? Snatchers appears on the Jungle Surface. Try not to go to the Underground Jungle, since Man Eaters are not in wrapped the package.*");
            AddObjectCollectionRequest("Snatcher Jaw", 5);
            AddObjectDroppingMonster(Terraria.ID.NPCID.Snatcher, 0.75f);
            //
            AddNewRequest("Mysterious Package", 260, 
                "*[nickname], I have a delivery for you to make. I have to warn you, don't tell the target who sent It.*",
                "*Good, send this to my personal annoyance, let's hope this makes him stop bothering me for some time.*",
                "*I wont be doing this myself, so I guess I'll leave this on hold.*",
                "*You did? Good. Now forget that this happened.*",
                "*You don't know who's my personal annoyance? Is that guard-wanna-be who keeps trying to arrest me all the time. Now go, before I change my mind!*");
            AddRequestRequirement(delegate(Player player)
            {
                return PlayerMod.PlayerHasGuardian(player, GuardianBase.Brutus);
            });
            AddTalkToGuardianRequest("*You say that you have a delivery for me? Weird, nobody I care about knows that I'm here. Well, let's see It....! [nickname], where did you get this? Tell me! I mean, who gave you this? You can't tell me? ... Thanks... Anyway..*", GuardianBase.Brutus);
        }

        public override void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if (guardian.BodyAnimationFrame == ThroneSittingFrame && guardian.LookingLeft)
            {
                guardian.FaceDirection(false);
            }
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "*I stayed for too long on this shop, I need to have some walk. Say, would you mind If I travelled with you?*";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*I will need some support fire, If you feel like helping, I can let you mount on my shoulder.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*If there's some place you can't go by yourself, just let me go instead.*";
            }
        }
        
        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*What? Interessed in my seeing my wares?*");
            Mes.Add("*Check out what I have to sell, I'm sure something will catch your eyes.*");
            Mes.Add("*Good, another customer. Want to look at my wares?*");
            if(PlayerMod.PlayerHasGuardianSummoned(player, Brutus))
                Mes.Add("*Hello "+PlayerMod.GetPlayerGuardian(player, Brutus).Name+", missed me?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I have guns for sale, If you want some.*");
            Mes.Add("*None of my goods are stolen. Probably.*");
            Mes.Add("*I've always wanted to have a shop sign. It would attract the attention of guards too, so I never even tried to. Well... I tried once. You know how It ended, though.*");
            Mes.Add("*Check this out, fresh from the Ether Realm.*");
            Mes.Add("*I like this place more than the Ether Realm, I don't need to hide, so I have more clients.*");
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*I think I have enough ammo for a day long combat.*");
                    Mes.Add("*I think I have stored away a magazine that has one of those monsters, I just need to find it.*");
                }
                else
                {
                    Mes.Add("*Ugh. Bunnies. Squirrels. Butterflies. They makes me want to grease the floor with vomit.*");
                    Mes.Add("*I'm not used to days, can you snap your fingers to make the night come?*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*I think I have something for your female citizens. It's right over here...*");
                    Mes.Add("*I was planning on sleep this night. I guess I'll discount my frustration on the monsters outside.*");
                    Mes.Add("*Well, It could have been worse. Right?*");
                }
                else
                {
                    Mes.Add("*So peaceful... Makes me want to take a nap.*");
                    Mes.Add("*What can I sell to you on this beautiful night?*");
                }
            }
            if (Main.raining)
            {
                Mes.Add("*I wasn't planning on going outside anyway...*");
                Mes.Add("*Rwatchooo~! Ugh, I hate this weather...*");
                Mes.Add("*Yes, we can trade. Just don't stay directly in front of my snout.*");
            }
            if (NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*[gn:"+Blue+"] asked me If I had something to help her hair grow. She said looooots of nasty things about me when I gave her chlorophyle.*");
            }
            if (NpcMod.HasGuardianNPC(Sardine))
            {
                Mes.Add("*What? How dare you?! I would never sell catnip to [gn:"+Sardine+"]!*");
            }
            if (NpcMod.HasGuardianNPC(Zacks))
            {
                Mes.Add("*I can't understand [gn:" + Zacks + "]. He got all angry when I tried to sell bandage gauze to him. At least he would be dressed for halloween.*");
            }
            if (NpcMod.HasGuardianNPC(Bree))
            {
                Mes.Add("*I wonder what is inside [gn:" + Bree + "]'s bag. Can it be sold for a good price?*");
            }
            if (NpcMod.HasGuardianNPC(Mabel))
            {
                Mes.Add("*[gn:" + Mabel + "] bought some orthopedic underwears earlier, but I wonder what for. I never saw her using any kind of clothing.*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*[gn:" + Brutus + "] has tried to arrest me several times in the Ether Realm, but I always managed to escape, because I'm smarter than him.*");
                Mes.Add("*Sometimes [gn:"+Brutus+"] comes to me, threatening to arrest me for doing shady deals. I have to keep remembering that there are no laws here, neither he's a guard.*");
                Mes.Add("*[gn:"+Brutus+"] said that will be keeping a closer eyes on me, waiting for me to do something wrong. I solved that by giving him some magazines.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*What?! I never got hugged by [gn:" + Vladimir + "], I don't know why he would say that.*");
                Mes.Add("*I feel like I've lost some weight on my shoulder. Maybe I could try smiling? No.*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*You wont believe when I say. I asked [gn:"+Minerva+"] if she could give me something that would be of my taste. She gave me a bone... She's really the best at guessing what people want.*");
                Mes.Add("*What? Never saw a bone before? I like chewing them sometimes. Gladly [gn:"+Minerva+"] has some stocks of them.*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*Between you and [gn:" + Michelle + "] around, I preffer you. At least you don't bother me all the time.*");
            }
            if (PlayerMod.HasGuardianSummoned(player, Wrath) && player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID])
            {
                Mes.Add("*Hey [nickname], what have you been eating latelly? Because you seems to have released a \"" + PlayerMod.GetPlayerGuardian(player, Wrath).Name + "\", hahaha. Got It? Released a \"" + PlayerMod.GetPlayerGuardian(player, Wrath).Name + "\"?*");
            }
            if (NpcMod.HasGuardianNPC(Green))
            {
                Mes.Add("*I supply [gn:"+Green+"] with things he uses to heal people. Keep that in mind if you wonder why he charges you.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.TravellingMerchant))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.TravellingMerchant + "] has a variety of low quality goods to offer.*");
                Mes.Add("*You wait for so long for [nn:" + Terraria.ID.NPCID.TravellingMerchant + "] to arrive, but what for?*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
            {
                Mes.Add("*I think [nn:" + Terraria.ID.NPCID.ArmsDealer + "] is a good business partner. He has some goods that interests me, though.*");
                Mes.Add("*I tried giving [nn:" + Terraria.ID.NPCID.ArmsDealer + "] some dating tips. I'm expert at that. But I don't understand the red hand marking on his face earlier.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.DD2Bartender))
            {
                Mes.Add("*I'm off to have a few drinks later, want to join?*");
                Mes.Add("*So.. You let [nn:"+Terraria.ID.NPCID.DD2Bartender+"] join your town because of the Old One's Army invasion? I thought It was for the drinks.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*Uh... Couldn't you wait until I'm done? I'm doing some other kind of business here.*");
                Mes.Add("*Do you know what privacy is? Because I need some right now.*");
                Mes.Add("*I don't need hygienic paper at the moment, so unless you tried bringing me some, I don't see the reason why you should enter an occupied toilet.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Alright, I can share my room with you, just don't try stealing my goods while I sleep.*");
                Mes.Add("*Maybe It's a bad idea having you inside my room, because I'm known for snoring, really loud.*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (!guardian.KnockedOutCold)
                {
                    Mes.Add("*What are you looking at? Give me a hand, will you?* (He's trying to stop a open wound from bleeding)");
                    Mes.Add("*Yes, that hurts, is that what you wanted to know?*");
                    Mes.Add("*Argh... [nickname]... I wont be able to do business in your world if I die.*");
                    Mes.Add("*Do you know a thing or two about first aid, buddy? Urf..*");
                }
                else
                {
                    Mes.Add("(He whited out.)");
                    Mes.Add("(He's groaning of pain.)");
                }
            }
            else if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("(His snores are extremelly loud.)");
                Mes.Add("(He seems to be counting something.)");
                Mes.Add("*A few more crates... Here the change...* (He's doing deals in his dreams)");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*I think I have the right thing to solve your ghost problem, but It will cost you a lot.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm glad there is no kind of law about selling weapons here, It's bad for business running from guards.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Nope. Nothing I want, at all.*");
            Mes.Add("*I think I have everything I need right now.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Not everything I can get by myself, so I need your help with this.*");
            Mes.Add("*I have to go get some goods soon from one of my sources, so I can't do this meanwhile.*");
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*I need to stay here, in case \"someone\" tries to confiscate my goods. So please do this for me?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Here, take this as reward. Also, no refund.*");
            Mes.Add("*You don't know how much I needed that done. Here some free things I wanted to charge you for.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I think It is safe for me to build a shop here, but there is nowhere I can build it...*");
            Mes.Add("*It's no good to do business in the open, with slimes trying to eat my feet, and zombies trying to eat my brain.*");
            Mes.Add("*There is no kind of laws here, right? Then It's the best place for me to open a shop. If I had somewhere to open it.*");
            if (Main.raining)
            {
                Mes.Add("*A-a-a-aCHOOOOOO~!! This is horrible, have some dry place I can stay?*");
                Mes.Add("*My goods will all be ruined by the rain, and I think I blew a vein in my nose because of all the sneezing. Do you have some place I can stay?*");
            }
            if (!Main.dayTime)
            {
                Mes.Add("*I swear I saw something sneaking around me. Do you have a way safer place for me to stay?*");
                if (Main.bloodMoon)
                {
                    Mes.Add("*Look, I'm used to have a life of adrenaline, but this is way too much! *");
                }
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*Did [gn:"+Brutus+"] told you not to give me a house? Just don't listen to him. I can't open a shop without a shop.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID && ReviveGuardian.ID == GuardianBase.Brutus)
            {
                Mes.Add("*I still have to torment your life some more, so don't make this too easy for yourself.*");
                Mes.Add("*Now that's a state I never expect to see you. Hahaha.*");
                Mes.Add("*Need a hand? Hah, that was good.*");
            }
            else if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*My chances of doing my trades in this world may die If you die. So please wake up.*");
                Mes.Add("*I'm only doing this to protect my business.*");
                Mes.Add("*I guess this is the moment I have to do an investiment.*");
            }
            Mes.Add("*Sigh...*");
            Mes.Add("*Do you plan on lying down for long?*");
            Mes.Add("*Here, I wont charge for this though. Just wake up and let's go.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*Alright, I can be your buddy. I wont offer you discounts, though.*";
                case MessageIDs.RescueMessage:
                    return "*The kind of things I do to protect my business...*";
                case MessageIDs.StoreOpenMessage:
                    if (Main.bloodMoon || Main.eclipse)
                        return "*This is a bad time for shopping.*";
                    if (!Main.dayTime)
                        return "*Don't comment about this stock.*";
                    return "*Everything is totally legal.*";
                case MessageIDs.StoreSaleHappeningMessage:
                    return "*Good timing, I think that may interest you.*";
                case MessageIDs.StoreBuyMessage:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*Thank you. I will put your coins to good use.*";
                    return "*I love the sound of coins.*";
                case MessageIDs.StoreFullInventoryMessage:
                    return "*I wont drop my merchandise. Clear your inventory.*";
                case MessageIDs.StoreNoCoinsMessage:
                    return "*I wont give you this for free.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*Huh? What? I snore very loud? Then go away.*";
                        case 1:
                            return "*[nickname], one of the things I value is a well slept day.*";
                        case 2:
                            return "*If you want to trade, couldn't you wait until I open my shop?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Yes, I'm very happy for seeing you too, so happy that I'm grinning. Is it about the request or what?*";
                        case 1:
                            return "*I hope you woke me up to say that completed my request, because I was really busy trying to sleep.*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Yes, but I'll take half of the loot.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I don't think you can carry more people right now.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*No, I preffer to stay here and make lucre selling my merchandise.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*You're planning on leaving me here, all alone, in the wilds?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I think I got enough loot from our trip, [nickname]. I'll go back to my store.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Okay... Don't you come asking for discount if I find something cool to sell.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*I thought so. Let's go to a town before you decide to remove me from the team.*";
                case MessageIDs.RequestAccepted:
                    return "*I had the feeling that you would accept. I'll have something prepared for you when you come back.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*I value my request, so try getting rid of those other requests before you ask for mine.*";
                case MessageIDs.RequestRejected:
                    return "*You wont get paid for that.*";
                case MessageIDs.RequestPostpone:
                    return "*It's not like as if It's a one time deal, anyway.*";
                case MessageIDs.RequestFailed:
                    return "*You failed? Well, I should ask you to pay me for that, but instead I'll give you nothing.*";
                case MessageIDs.RestAskForHowLong:
                    return "*You can sleep any time you want, I'll open a shop meanwhile.\nHow long do you plan on sleeping?*";
                case MessageIDs.RestNotPossible:
                    return "*Not now, It's a horrible moment for resting.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Time to do business...*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Do you mind if we visit [shop]'s shop?*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Don't worry, I wont spend much of your time.*";
                case MessageIDs.GenericYes:
                    return "*Uh huh.*";
                case MessageIDs.GenericNo:
                    return "*Nope.*";
                case MessageIDs.GenericThankYou:
                    return "*Alright, that's good.*";
                case MessageIDs.ChatAboutSomething:
                    return "*I can't spend chatting, but I think I can spend a few moments answering you.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Fine. Want to check my wares now?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You know that you wont get anything if you cancel my request, do you?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Okay... You're relieved of my task. Now if you don't mind, I have business to do.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Go do that, [nickname].*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*I don't owe you anything, right?*";
                    return "*Okay, thanks. Happy now?*";
                case MessageIDs.RevivedByRecovery:
                    return "*I'm fine now, not thanks for you.*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Ugh... I should have stayed behind...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Argh! Burn! Damn it!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*I can barelly see!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Why is that tree zig-zagging?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I can't pull the trigger. Help!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*Ouch, I wounded my leg.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*You think that will put me down...?*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Ahh! That... Is It?*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*... I will need more ammo...*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*I'll fill you with holes now!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*S-s-someone c-can l-lend me a-a blanket?*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*Try getting closer to me.*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*Grrr... Bark! Bark!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*My fur feels thicker too. I don't know if I like that.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Well cooked food is always nice.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Whatever crosses my path, is dead, now.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*This would have been handy in the Ether Realm.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Aaaaahh....*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*I can now see vital points clearer.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Always good to ensure victory.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Good...*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*You don't mind if I bottle some to sell later, right?*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Look, we could fetch quite a price for that.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Someone tried to be funny. Watch your step.*";
                case MessageIDs.FoundMineTile:
                    return "*Someone wants someone else dead.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*You're going to press It? I need to know, so I can take a safe distance.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Something tells me not to pop that.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I could use unloading my guns at things. Go on ahead.*";
                case MessageIDs.FoundTreasureTile:
                    return "*You're planning on sharing, right?*";
                case MessageIDs.FoundGemTile:
                    return "*Hm... I could fetch some price for those.*";
                case MessageIDs.FoundRareOreTile:
                    return "*Look, ores.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*You may want to see this.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*Eh, whatever. We can ride the minecart.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*I hope nobody messed on my goods.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Hey, a new follower.*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*Another potential customer. Nice.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*This would have been useful when I was being chased by guards.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*You do anything for a new friend, [nickname]?*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Don't look now, but our leader fell.*";
                case MessageIDs.LeaderDiesMessage:
                    return "*[nickname] died... Mind if I take their items?*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone fell there.*";
                case MessageIDs.SpotsRareTreasure:
                    return "*Hahaha, you plan on sharing that, don't you?*";
                case MessageIDs.LeavingToSellLoot:
                    return "*Time to get some coins out of this.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*You seem in bad shape, [nickname].*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*Ugh... I need to stay behind a while...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I'm running out of potions, and that's not good.*";
                case MessageIDs.UsesLastPotion:
                    return "*That was my last potion!*";
                case MessageIDs.SpottedABoss:
                    return "*Time to fill that thing with hundreds of holes.*";
                case MessageIDs.DefeatedABoss:
                    return "*Let's split the loot 60/40.*";
                case MessageIDs.InvasionBegins:
                    return "*I hope I have enough ammo...*";
                case MessageIDs.RepelledInvasion:
                    return "*Time to see if they were carrying something valuable.*";
                case MessageIDs.EventBegins:
                    return "*Why do I feel that I will spend a long time shooting things?*";
                case MessageIDs.EventEnds:
                    return "*Gladly it's over.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*You terrarians aren't new to me. There is another named [player] who checks me from time to time. Sometimes they don't even check what I'm selling.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*[player] recently took down [subject]. I wonder if what they found is good for bartering.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*I envy [player] for finding a [subject]. They should have accepted my offer of selling it for 20% the price, but nooo... They had to use it.*";
                case MessageIDs.FeatEventFinished:
                    return "*I had enough rounds to deal with the adversities of [subject]. Gladly, [player] managed to end it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*It seems like [player] has just met me a new customer named [subject].*";
                case MessageIDs.FeatPlayerDied:
                    return "*Watch yourself during your travels, [nickname]. Another terrarian named [player] didn't returned from theirs. Yes, they was my best client.*";
                case MessageIDs.FeatOpenTemple:
                    return "*I heard that [player] finally opened the door of a temple in [subject]. I will visit it some time to see what I can take from it.*";
                case MessageIDs.FeatCoinPortal:
                    return "*That lucky Terrarian... [player] managed to bump into one of those coin portals.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I met a potential new client latelly. They are called [player].*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*[player] could have made some money, if he let me smuggle some of the fish they gave to the Angler.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*It seems like [subject] was saved from some weird creature by [player]. By the way, if you're interessed in buying some Luminite, be sure to give me a word.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*[player] managed to get more things to shot at in [subject].*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*Hmph... I heard about [player] picking [subject] as their buddy. I hope they don't hinder each other.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Just because I'm your buddy doesn't means I'll give you freebies, but I guess you could be my business partner.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
