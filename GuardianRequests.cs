using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianRequests //Need to add quest item rewards, aswell as save and load system for the requests, to avoid abusing.
    {
        public bool Active = false;
        public RequestType type = RequestType.Talk;
        public int[] Ids = new int[] { 0, 0, 0 };
        public byte[] Stacks = new byte[] { 0,0,0 };
        public string[] ObjectiveTexts = new string[] { "", "", "" };
        public int RequestTimer = 1800;
        public float MovementFloat = 0f;
        public const int MinRequestTimer = 1800, MaxRequestTimer = 3600, MinRequestSpawnTime = 600, MaxRequestSpawnTime = 1200;
        public byte FriendshipReward = 0;
        public bool RequiresGuardianActive = false;
        public List<Item> ItemRewards = new List<Item>();

        public byte GetFriendshipReward(GuardianData guardian)
        {
            byte Value = 0;
            for (int i = 0; i < 3; i++)
            {
                if (Ids[i] > 0)
                {
                    Value++;
                }
            }
            if (type == RequestType.TravellingQuest || type == RequestType.Talk)
            {
                Value += (byte)(1 + guardian.FriendshipLevel / 10);
            }
            return Value;
        }

        public void UpdateRequest(Player player, TerraGuardian[] Guardians, GuardianData data)
        {
            if (Active && RequestTimer > 0)
            {
                RequestTimer--;
                bool CountTowardsProgress = false;
                foreach (TerraGuardian guardian in Guardians)
                {
                    if (CountObjective(guardian, data))
                    {
                        CountTowardsProgress = true;
                        break;
                    }
                }
                if (CountTowardsProgress)
                {
                    switch (type)
                    {
                        case RequestType.TravellingQuest:
                            {
                                if (Guardians.Any(x => x.Active && x.ID == data.ID && x.ModID == data.ModID))
                                {
                                    TerraGuardian guardian = Guardians.First(x => x.Active && x.ID == data.ID && x.ModID == data.ModID);
                                    if (MovementFloat > 0 && data.ID == guardian.ID && data.ModID == guardian.ModID)
                                    {
                                        float Speed = guardian.Velocity.X;
                                        if (guardian.MountedOnPlayer || guardian.SittingOnPlayerMount)
                                            Speed = player.velocity.X;
                                        MovementFloat -= Math.Abs(Speed);
                                        if (MovementFloat <= 0)
                                            Main.NewText(guardian.Name + " enjoyed the trip.");
                                    }
                                }
                            }
                            break;
                        case RequestType.EventParticipation:
                            switch ((EventList)Ids[0])
                            {
                                case EventList.DD2Event:
                                case EventList.PumpkinMoon:
                                case EventList.FrostMoon:
                                    {
                                        if (Ids[1] > 0 && Ids[1] != NPC.waveNumber && Stacks[0] > 0)
                                        {
                                            Stacks[0]--;
                                        }
                                        Ids[1] = NPC.waveNumber;
                                    }
                                    break;
                                case EventList.Party:
                                    if (MovementFloat > 0 && Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
                                    {
                                        MovementFloat--;
                                        if (MovementFloat <= 0)
                                        {
                                            MovementFloat = 0;
                                            Main.NewText(data.Name + " has enjoyed the party.");
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (RequestTimer <= 0)
                {
                    if (Active)
                    {
                        Active = false;
                        SetNewRequestTimer();
                        CheckIfGuardianShouldBeUnsummoned(player, data);
                    }
                    else
                    {
                        if ((data.FriendshipLevel == 0 || PlayerMod.GetPlayerActiveRequests(player) < 3) && (PlayerMod.PlayerHasGuardianSummoned(player, data.ID, data.ModID) || NpcMod.HasGuardianNPC(data.ID, data.ModID)))
                        {
                            GenerateRequest(player, data);
                            foreach (TerraGuardian guardian in Guardians)
                            {
                                if (guardian.ID == data.ID) guardian.DisplayEmotion(TerraGuardian.Emotions.Question);
                            }
                            RequestTimer = Main.rand.Next(MinRequestTimer, MaxRequestTimer + 1) * 60;
                            if (player.whoAmI == Main.myPlayer)
                            {
                                Main.NewText(data.Name + " seems to have a new request for you.");
                                if (!player.GetModPlayer<PlayerMod>().TutorialRequestIntroduction)
                                {
                                    player.GetModPlayer<PlayerMod>().TutorialRequestIntroduction = true;
                                    Main.NewText("Someone gave you a request. Helping them will reward with friendship experience, and also with some interesting rewards.");
                                }
                            }
                        }
                        else
                        {
                            if (data.FriendshipLevel == 0)
                            {
                                RequestTimer = 1800;
                            }
                            else
                            {
                                SetNewRequestTimer();
                            }
                        }
                    }
                }
                else
                {
                    RequestTimer--;
                }
            }
        }

        public void SetNewRequestTimer()
        {
            RequestTimer = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime + 1) * 60;
        }

        public void CreateRewards(int Points, Player player)
        {
            ItemRewards.Clear();
            if (Points >= 2000 && Main.rand.Next(15) == 0)
            {
                Points -= 2000;
                AddItemReward(ModContent.ItemType<Items.Consumable.SkillResetPotion>(), 1);
            }
            if (Points >= 1000 && Main.rand.Next(5) == 0)
            {
                Points -= 1000;
                AddItemReward(Terraria.ID.ItemID.LifeCrystal, 1);
            }
            if (Points >= 500 && Main.rand.Next(3) == 0)
            {
                int Stack = Main.rand.Next(1, 4);
                Stack -= (int)((Stack - 1) * 0.5);
                if (Stack * 500 > Points)
                {
                    Stack = Points / 500;
                }
                AddItemReward(ModContent.ItemType<Items.Consumable.EtherHeart>(), Stack);
                Points -= Stack;
            }
            if (Points >= 750 && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
            {
                int Stack = Main.rand.Next(1, 4);
                Stack -= (int)((Stack - 1) * 0.5);
                if (Stack * 750 > Points)
                {
                    Stack = Points / 750;
                }
                AddItemReward(ModContent.ItemType<Items.Consumable.EtherFruit>(), Stack);
                Points -= Stack;
            }
            if (Points >= 500 && Main.rand.Next(5) == 0)
            {
                int GemsAward = Main.rand.Next(1, 6);
                if (GemsAward * 500 > Points)
                {
                    GemsAward = Points / 500;
                }
                Points -= GemsAward * 500;
                if (Main.rand.Next(7) == 0)
                {
                    AddItemReward(999, GemsAward);
                }
                else
                {
                    AddItemReward(177 + Main.rand.Next(6), GemsAward);
                }
            }
            if (Points >= 300 && Main.rand.Next(5) == 0)
            {
                int Box = 2335;
                switch (Main.rand.Next(7))
                {
                    case 0:
                        Box = 3208;
                        break;
                    case 1:
                        Box = 3206;
                        break;
                    case 2:
                        Box = 3203;
                        break;
                    case 3:
                        Box = 3204;
                        break;
                    case 4:
                        Box = 3207;
                        break;
                    case 5:
                        Box = 3205;
                        break;
                }
                int BoxReward = Main.rand.Next(1, 4);
                if (BoxReward * 200 > Points)
                    BoxReward = Points / 200;
                AddItemReward(Box, BoxReward);
                Points -= BoxReward * 200;
            }
            if (Points >= 250 && NPC.downedBoss3 && Main.rand.Next(3) == 0)
            {
                if (Main.rand.Next(3) == 0)
                    AddItemReward(327, Main.rand.Next(3, 6));
                AddItemReward(3085, 1);
                Points -= 250;
            }
            if (Points >= 200 && Main.rand.Next(3) == 0)
            {
                int Box = 2334;
                if (Main.rand.Next(5) == 0)
                    Box = 2335;
                if (Main.rand.Next(10) == 0)
                    Box = 2336;
                int BoxReward = Main.rand.Next(1, 4);
                if (BoxReward * 200 > Points)
                    BoxReward = Points / 200;
                AddItemReward(Box, BoxReward);
                Points -= BoxReward * 200;
            }
            if (Points >= 100 && Main.rand.Next(3) == 0)
            {
                int ItemsToGive = Main.rand.Next(1, 4);
                if (ItemsToGive * 100 > Points)
                    ItemsToGive = Points / 100;
                Points -= ItemsToGive * 100;
                ItemsToGive += 2;
                AddItemReward(Terraria.ID.ItemID.HerbBag, ItemsToGive);
            }
            if (Points >= 50)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int PotionID = 28;
                    if (player.statLifeMax >= 200)
                        PotionID = 188;
                    if (Main.hardMode)
                        PotionID = 499;
                    if (NPC.downedGolemBoss)
                        PotionID = 3544;
                    int PotionCount = Main.rand.Next(3, 6);
                    if (PotionCount * 50 > Points)
                        PotionCount = Points / 50;
                    AddItemReward(PotionID, PotionCount);
                    Points -= PotionCount * 50;
                }
                else
                {
                    int PotionID = 110;
                    if (player.statManaMax >= 100)
                        PotionID = 189;
                    if (Main.hardMode)
                        PotionID = 500;
                    if (NPC.downedGolemBoss)
                        PotionID = 3544;
                    int PotionCount = Main.rand.Next(3, 6);
                    if (PotionCount * 50 > Points)
                        PotionCount = Points / 50;
                    AddItemReward(PotionID, PotionCount);
                    Points -= PotionCount * 50;
                }
            }
            if (Points >= 20 && Main.rand.Next(3) == 0)
            {
                int FoodID = 357;
                if (Main.rand.Next(2) == 0)
                    FoodID = 2425;
                if (Main.halloween && Main.rand.Next(3) == 0)
                    FoodID = 1787;
                int FoodCount = Main.rand.Next(1, 4);
                if (FoodCount * 20 > Points)
                    FoodCount -= Points / 20;
                AddItemReward(FoodID, FoodCount);
                Points -= FoodCount * 20;
            }
            if (Points > 0)
            {
                int c = Points, s = 0, g = 0, p = 0;
                if (c >= 100)
                {
                    s += c / 100;
                    c -= s * 100;
                }
                if (s >= 100)
                {
                    g += s / 100;
                    s -= g * 100;
                }
                if (g >= 100)
                {
                    p += g / 100;
                    g -= p * 100;
                }
                if (c > 0)
                    AddItemReward(Terraria.ID.ItemID.CopperCoin, c);
                if (s > 0)
                    AddItemReward(Terraria.ID.ItemID.SilverCoin, s);
                if (g > 0)
                    AddItemReward(Terraria.ID.ItemID.GoldCoin, g);
                if (p > 0)
                    AddItemReward(Terraria.ID.ItemID.PlatinumCoin, p);
                Points = 0;
            }
            else
            {

            }
            if (Main.xMas)
            {
                AddItemReward(1869, Main.rand.Next(1, 4));
            }
            if (Main.halloween)
            {
                AddItemReward(Terraria.ID.ItemID.GoodieBag, Main.rand.Next(1, 4));
            }
        }

        public void AddItemReward(int id, int stack = 1)
        {
            Item i = new Item();
            i.SetDefaults(id);
            i.stack = stack;
            ItemRewards.Add(i);
        }

        public string RequestInformationSuffix(GuardianData guardian)
        {
            if (RequiresGuardianActive)
            {
                return " with " + guardian.Name + ":";
            }
            return " for "+guardian.Name+":";
        }

        public string GetRequestInformation(GuardianData guardian)
        {
            string Info = "";
            switch (type)
            {
                case RequestType.Talk:
                    if (Stacks[0] == 0)
                        Info = "Talk to " + guardian.Name + ".";
                    else
                        Info = "You talked to " + guardian.Name + ".";
                    break;
                case RequestType.ItemCollection:
                    Info = "Deliver the following items to "+guardian.Name+":";
                    for (int i = 0; i < 3; i++)
                    {
                        if (Ids[i] > 0)
                        {
                            Info += "\n  "+ObjectiveTexts[i];
                        }
                    }
                    break;
                case RequestType.MobHunting:
                    Info = "Defeat the following monsters" + RequestInformationSuffix(guardian);
                    {
                        bool HasMob = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Ids[i] > 0 && Stacks[i] > 0)
                            {
                                HasMob = true;
                                Info += "\n  " + ObjectiveTexts[i] + "x" + Stacks[i];
                            }
                        }
                        if (!HasMob)
                            Info += "\n  All monsters defeated.";
                    }
                    break;
                case RequestType.BossPwning:
                    Info = "Defeat the following boss" + RequestInformationSuffix(guardian);
                    {
                        bool HasMob = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Ids[i] > 0 && Stacks[i] > 0)
                            {
                                HasMob = true;
                                Info += "\n  " + ObjectiveTexts[i] + "x" + Stacks[i];
                            }
                        }
                        if (!HasMob)
                            Info += "\n  All bosses defeated.";
                    }
                    break;
                case RequestType.TravellingQuest:
                    Info = "Explore the world with " + guardian.Name + ":";
                    if (MovementFloat <= 0)
                        Info += "\n  Is satisfied with the exploration.";
                    else
                        Info += "\n  Wants to explore some more.";
                    break;
                case RequestType.EventParticipation:
                    Info = "Participate of an world event" + RequestInformationSuffix(guardian);
                    if ((EventList)Ids[0] == EventList.Party)
                    {
                        if (MovementFloat <= 0)
                        {
                            Info += "\n  " + guardian.Name + " would like to thank you for the party.";
                        }
                        else if (!Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
                        {
                            Info += "\n  Throw a party for " + guardian.Name + ".";
                        }
                        else
                        {
                            if (RequiresGuardianActive)
                            {
                                Info += "\n  Enjoy the Party with " + guardian.Name + ".";
                            }
                            else
                            {
                                Info += "\n  " + guardian.Name + " is enjoying the party.";
                            }
                        }
                        //Info += " " + MovementFloat;
                    }
                    else if (Stacks[0] > 0)
                    {
                        switch ((EventList)Ids[0])
                        {
                            case EventList.GoblinArmy:
                                Info += "\n  Defeat " + Stacks[0] + " enemies on the Goblin Army.";
                                break;
                            case EventList.PirateArmy:
                                Info += "\n  Defeat " + Stacks[0] + " enemies on the Pirate Invasion.";
                                break;
                            case EventList.MartianArmy:
                                Info += "\n  Defeat " + Stacks[0] + " enemies on the Martian Madness.";
                                break;
                            case EventList.FrostArmy:
                                Info += "\n  Defeat " + Stacks[0] + " enemies on the Frost Legion.";
                                break;
                            case EventList.PumpkinMoon:
                                Info += "\n  Survive " + Stacks[0] + " waves on the Pumpkin Moon.";
                                break;
                            case EventList.FrostMoon:
                                Info += "\n  Survive " + Stacks[0] + " waves on the Frost Moon.";
                                break;
                            case EventList.DD2Event:
                                Info += "\n  Survive " + Stacks[0] + " waves on the Old One's Army.";
                                break;
                        }
                    }
                    else
                    {
                        if (!RequiresGuardianActive)
                        {
                            Info += "\n  You did enough progress in the event, report to "+guardian.Name+".";
                        }
                        else
                        {
                            Info += "\n  " + guardian.Name + " enjoyed It's time on the event.";
                        }
                    }
                    break;
            }
            return Info;
        }

        public string GetRequestDuration
        {
            get
            {
                string Time = "";
                int s = RequestTimer / 60, m = 0, h = 0;
                if (s >= 60)
                {
                    m += s / 60;
                    s -= m * 60;
                }
                if (m >= 60)
                {
                    h += m / 60;
                    m -= 60 * h;
                }
                if (h > 0)
                    Time += h + " hour" + (h > 1 ? "s" : "") + " ";
                if (h > 0 || m > 0)
                {
                    if (h > 0)
                        Time += ", ";
                    Time += m + " minute" + (m > 1 ? "s" : "") + " ";
                }
                if (h > 0 || m > 0 || s >= 0)
                {
                    if (h > 0 || m > 0)
                        Time += ", ";
                    Time += s + " second" + (s > 1 ? "s" : "");
                }
                return Time + ".";
            }
        }

        public bool CountObjective(TerraGuardian summonedGuardian, GuardianData requestGuardian)
        {
            return !RequiresGuardianActive || (summonedGuardian.Active && summonedGuardian.ID == requestGuardian.ID && summonedGuardian.ModID == requestGuardian.ModID);
        }

        public void OnMobKill(int MobID)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Ids[i] > 0)
                {
                    switch (type)
                    {
                        case RequestType.MobHunting:
                        case RequestType.BossPwning:
                            {
                                if (Stacks[i] > 0)
                                {
                                    int m = MobID;
                                    bool IsQuestMob = false;
                                    if (m == Ids[i])
                                        IsQuestMob = true;
                                    else
                                    {
                                        switch (Ids[i])
                                        {
                                            case NPCID.Zombie:
                                                IsQuestMob = m == 430 || m == 132 || m == 186 || m == 432 || m == 187 || m == 433 || m == 188 || m == 434 || m == 189 || m == 435 ||
                                                    m == 200 || m == 436;
                                                break;
                                            case NPCID.ZombieEskimo:
                                                IsQuestMob = m == NPCID.ArmedZombieEskimo;
                                                break;
                                            case NPCID.DemonEye:
                                                IsQuestMob = m == 190 || m == 191 || m == 192 || m == 193 || m == 194 || m == 317 || m == 318;
                                                break;
                                            case NPCID.BloodCrawler:
                                                IsQuestMob = m == NPCID.BloodCrawlerWall;
                                                break;
                                            case NPCID.Demon:
                                                IsQuestMob = m == NPCID.VoodooDemon;
                                                break;
                                            case NPCID.JungleCreeper:
                                                IsQuestMob = m == NPCID.JungleCreeperWall;
                                                break;
                                            case NPCID.Hornet:
                                                IsQuestMob = m == NPCID.HornetFatty || m == NPCID.HornetHoney || m == NPCID.HornetLeafy || m == NPCID.HornetSpikey || m == NPCID.HornetStingy;
                                                break;
                                            case NPCID.AngryBones:
                                                IsQuestMob = m == 294 || m == 295 || m == 296;
                                                break;
                                            case NPCID.BlueArmoredBones:
                                                IsQuestMob = m == NPCID.BlueArmoredBonesMace || m == NPCID.BlueArmoredBonesNoPants || m == NPCID.BlueArmoredBonesSword;
                                                break;
                                            case NPCID.RustyArmoredBonesAxe:
                                                IsQuestMob = m == NPCID.RustyArmoredBonesFlail || m == NPCID.RustyArmoredBonesSword || m == NPCID.RustyArmoredBonesSwordNoArmor;
                                                break;
                                            case NPCID.HellArmoredBones:
                                                IsQuestMob = m == NPCID.HellArmoredBonesMace || m == NPCID.HellArmoredBonesSpikeShield || m == NPCID.HellArmoredBonesSword;
                                                break;
                                            case NPCID.EaterofWorldsHead:
                                                IsQuestMob = m == NPCID.EaterofWorldsBody || m == NPCID.EaterofWorldsTail;
                                                break;
                                        }
                                    }
                                    if (IsQuestMob)
                                        Stacks[i]--;
                                }
                            }
                            break;
                        case RequestType.EventParticipation:
                            bool EventMobKilled = false; //Need to check if the monster killed is from the event.
                            switch ((EventList)Ids[0])
                            {
                                case EventList.GoblinArmy:
                                        switch (MobID)
                                        {
                                            case 26:
                                            case 29:
                                            case 27:
                                            case 28:
                                            case 111:
                                            case 417:
                                                EventMobKilled = true;
                                                break;
                                        }
                                    break;
                                case EventList.PirateArmy:
                                    switch (MobID)
                                    {
                                        case 216:
                                        case 213:
                                        case 215:
                                        case 214:
                                        case 212:
                                        case 252:
                                        case 491:
                                            EventMobKilled = true;
                                            break;
                                    }
                                    break;
                                case EventList.MartianArmy:
                                    switch (MobID)
                                    {
                                        case 381:
                                        case 385:
                                        case 382:
                                        case 383:
                                        case 386:
                                        case 389:
                                        case 520:
                                        case 390:
                                        case 395:
                                            EventMobKilled = true;
                                            break;
                                    }
                                    break;
                                case EventList.FrostArmy:
                                    switch (MobID)
                                    {
                                        case 144:
                                        case 143:
                                        case 145:
                                            EventMobKilled = true;
                                            break;
                                    }
                                    break;
                            }
                            if (EventMobKilled)
                            {
                                if (Stacks[i] > 0)
                                    Stacks[i]--;
                            }
                            break;
                    }
                }
            }
        }

        public void GenerateRequest(Player player, GuardianData guardian)
        {
            Active = true;
        retryEventSpawning:
            RequiresGuardianActive = false;
            byte Type = (byte)Main.rand.Next(11);
            if (guardian.FriendshipLevel == 0)
                Type = 0;
            //if (Type == (byte)RequestType.TravellingQuest)
            //    Type = 0;
            Ids = new int[]{0,0,0};
            Stacks = new byte[] { 0, 0, 0 };
            ObjectiveTexts = new string[] { "", "", "" };
            MovementFloat = 0f;
            int ScoreCounter = 0;
            switch (Type)
            {
                case 0: //Talk
                    type = RequestType.Talk;
                    ScoreCounter = 500;
                    break;
                case 1: //Fishing
                    {
                        type = RequestType.ItemCollection;
                        Ids[0] = Terraria.ID.ItemID.Bass;
                        if (Main.rand.Next(2) == 0)
                            Ids[0] = Terraria.ID.ItemID.AtlanticCod;
                        //if (Main.rand.Next(3) == 0 && player.statDefense >= 9)
                        //    Ids[0] = Terraria.ID.ItemID.Salmon;
                        if (false && player.statDefense >= 9)
                        {
                            if (WorldGen.crimson)
                            {
                                if (Main.rand.Next(3) == 0)
                                {
                                    Ids[0] = 2305;
                                }
                            }
                            else
                            {
                                if (Main.rand.Next(5) == 0) //There's no corruption fish :/
                                {

                                }
                            }
                        }
                        if (Main.rand.Next(3) == 0 && player.statDefense >= 9)
                            Ids[0] = Terraria.ID.ItemID.Trout;
                        if (Main.rand.Next(5) == 0 && player.statDefense >= 9)
                            Ids[0] = Terraria.ID.ItemID.NeonTetra;
                        Stacks[0] = (byte)(1 + guardian.FriendshipLevel / 3);
                        ScoreCounter = 1000 + Stacks[0] * 250;
                    }
                    break;
                case 2: //Mob Hunting
                    {
                        type = RequestType.MobHunting;
                        Ids[0] = Terraria.ID.NPCID.MotherSlime;
                        bool RareMob = false;
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(3) == 0)
                                Ids[0] = NPCID.DemonEye;
                            else
                                Ids[0] = NPCID.Zombie;
                        }
                        if (Main.rand.Next(5) == 0)
                        {
                            Ids[0] = NPCID.Piranha;
                        }
                        if (Main.rand.Next(5) == 0)
                        {
                            Ids[0] = NPCID.Antlion;
                        }
                        /*if (Main.rand.Next(3) == 0)
                        {
                            Ids[0] = NPCID.GiantWormHead;
                            RareMob = true;
                        }*/
                        if (Main.rand.Next(3) == 0 && player.statDefense >= 5)
                        {
                            Ids[0] = NPCID.GoblinScout;
                            RareMob = true;
                        }
                        if (Main.hardMode)
                        {
                            /*if (Main.rand.Next(3) == 0)
                            {
                                Ids[0] = NPCID.DiggerHead;
                            }*/
                            if (Main.rand.Next(3) == 0)
                            {
                                RareMob = false;
                                if (Main.rand.Next(2) == 0)
                                    Ids[0] = NPCID.PossessedArmor;
                                else
                                    Ids[0] = NPCID.WanderingEye;
                            }
                        }
                        if (player.statDefense >= 9)
                        {
                            if (Main.rand.Next(3) == 0)
                            {
                                RareMob = false;
                                if (Main.rand.Next(3) == 0)
                                {
                                    Ids[0] = NPCID.Shark;
                                }
                                else
                                {
                                    Ids[0] = NPCID.PinkJellyfish;
                                }
                            }
                            if (WorldGen.crimson)
                            {
                                RareMob = false;
                                if (Main.rand.Next(2) == 0)
                                {
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Ids[0] = NPCID.BloodCrawler;
                                    }
                                    else if (Main.rand.Next(2) == 0)
                                    {
                                        Ids[0] = NPCID.Crimera;
                                    }
                                    else
                                    {
                                        Ids[0] = NPCID.FaceMonster;
                                    }
                                }
                                if (Main.hardMode && Main.rand.Next(2) == 0)
                                {
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Ids[0] = NPCID.Herpling;
                                    }
                                    else
                                    {
                                        Ids[0] = NPCID.Crimslime;
                                    }
                                }
                            }
                            else
                            {
                                RareMob = false;
                                if (Main.rand.Next(2) == 0)
                                {
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Ids[0] = NPCID.DevourerHead;
                                        RareMob = true;
                                    }
                                    else
                                    {
                                        Ids[0] = NPCID.EaterofSouls;
                                    }
                                }
                                if (Main.rand.Next(3) == 0)
                                {
                                    if (Main.hardMode && Main.rand.Next(2) == 0)
                                    {
                                        Ids[0] = NPCID.WyvernHead;
                                        RareMob = true;
                                    }
                                    else
                                    {
                                        Ids[0] = NPCID.Harpy;
                                    }
                                }
                                if (Main.hardMode && Main.rand.Next(2) == 0)
                                {
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Ids[0] = 98; //World Feeder
                                        RareMob = true;
                                    }
                                    else
                                    {
                                        Ids[0] = NPCID.Corruptor;
                                    }
                                }
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                RareMob = false;
                                switch (Main.rand.Next(4))
                                {
                                    case 0:
                                        Ids[0] = NPCID.ManEater;
                                        break;
                                    case 1:
                                        Ids[0] = NPCID.JungleBat;
                                        break;
                                    case 2:
                                        Ids[0] = NPCID.SpikedJungleSlime;
                                        break;
                                    case 3:
                                        Ids[0] = NPCID.Hornet;
                                        break;
                                }
                                if (Main.hardMode && Main.rand.Next(2) == 0)
                                {
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        switch (Main.rand.Next(3))
                                        {
                                            case 0:
                                                Ids[0] = NPCID.JungleCreeper;
                                                break;
                                            case 1:
                                                Ids[0] = NPCID.MossHornet;
                                                break;
                                            case 2:
                                                Ids[0] = NPCID.GiantTortoise;
                                                break;
                                        }
                                        if (Main.rand.Next(5) == 0)
                                            Ids[0] = NPCID.GiantFlyingFox;
                                    }
                                    else if(Main.rand.Next(2) == 0)
                                    {
                                        switch (Main.rand.Next(3))
                                        {
                                            case 0:
                                                Ids[0] = NPCID.IceTortoise;
                                                break;
                                            case 1:
                                                Ids[0] = NPCID.Wolf;
                                                break;
                                            case 2:
                                                Ids[0] = NPCID.IcyMerman;
                                                break;
                                        }
                                    }
                                }
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                RareMob = false;
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        Ids[0] = 62;
                                        break;
                                    case 1:
                                        Ids[0] = 24;
                                        break;
                                    case 2:
                                        Ids[0] = 60;
                                        break;
                                }
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                RareMob = false;
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        Ids[0] = NPCID.Demon;
                                        break;
                                    case 1:
                                        Ids[0] = NPCID.FireImp;
                                        break;
                                    case 2:
                                        Ids[0] = NPCID.Hellbat;
                                        break;
                                }
                            }
                        }
                        if (NPC.downedBoss3 && player.statDefense >= 9)
                        {
                            RareMob = false;
                            if (Main.rand.Next(3) == 0)
                            {
                                if (!Main.hardMode)
                                {
                                    switch (Main.rand.Next(3))
                                    {
                                        case 0:
                                            Ids[0] = 31;
                                            break;
                                        case 1:
                                            Ids[0] = 32;
                                            break;
                                        case 2:
                                            Ids[0] = 34;
                                            break;
                                    }
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Ids[0] = NPCID.DungeonSlime;
                                        RareMob = true;
                                    }
                                }
                                else if(NPC.downedPlantBoss)
                                {
                                    switch (Main.rand.Next(5))
                                    {
                                        case 0:
                                            Ids[0] = NPCID.BlueArmoredBones;
                                            break;
                                        case 1:
                                            Ids[0] = NPCID.RustyArmoredBonesAxe;
                                            break;
                                        case 2:
                                            Ids[0] = NPCID.HellArmoredBones;
                                            break;
                                        case 3:
                                            Ids[0] = NPCID.GiantCursedSkull;
                                            RareMob = true;
                                            break;
                                        case 4:
                                            Ids[0] = NPCID.Paladin;
                                            RareMob = true;
                                            break;
                                    }
                                }
                            }
                        }
                        /*if (Main.hardMode && Main.dayTime && Main.moonPhase == 0 && Main.rand.Next(3) == 0)
                        {
                            Ids[0] = NPCID.Werewolf;
                        }*/
                        if (Main.hardMode)
                        {
                            RareMob = false;
                            if (NPC.downedMechBossAny && Main.rand.Next(3) == 0)
                            {
                                if (Main.rand.Next(2) == 0)
                                    Ids[0] = NPCID.RedDevil;
                                else
                                    Ids[0] = NPCID.Lavabat;
                            }
                        }
                        Stacks[0] = (byte)((RareMob ? 1 : 5) + guardian.FriendshipLevel / (RareMob ? 5 : 3));
                        ScoreCounter = 1000 + Stacks[0] * (RareMob ? 500 : 200);
                    }
                    break;
                case 3: //Boss Pwning
                    {
                        type = RequestType.BossPwning;
                        if (NPC.downedBoss1 && player.statDefense >= 10)
                        {
                            Ids[0] = Terraria.ID.NPCID.EyeofCthulhu;
                        }
                        if (NPC.downedBoss2 && player.statDefense >= 10 && Main.rand.Next(2) == 0)
                        {
                            if (WorldGen.crimson)
                            {
                                Ids[0] = Terraria.ID.NPCID.BrainofCthulhu;
                            }
                            else
                            {
                                Ids[0] = Terraria.ID.NPCID.EaterofWorldsHead;
                            }
                        }
                        if (NPC.downedBoss3 && NPC.AnyNPCs(Terraria.ID.NPCID.Clothier) && player.statDefense >= 10 && Main.rand.Next(3) == 0)
                        {
                            Ids[0] = Terraria.ID.NPCID.SkeletronHead;
                        }
                        if (NPC.downedSlimeKing && player.statDefense >= 10 && Main.rand.Next(5) == 0)
                        {
                            Ids[0] = Terraria.ID.NPCID.KingSlime;
                        }
                        if (NPC.downedQueenBee && player.statDefense >= 10 && Main.rand.Next(5) == 0)
                        {
                            Ids[0] = Terraria.ID.NPCID.QueenBee;
                        }
                        if (Main.hardMode)
                        {
                            if (Main.rand.Next(3) == 0)
                                Ids[0] = Terraria.ID.NPCID.WallofFlesh;
                            if (NPC.downedMechBoss1 && Main.rand.Next(3) == 0)
                            {
                                if (Main.rand.Next(2) == 0)
                                    Ids[0] = Terraria.ID.NPCID.Spazmatism;
                                else
                                    Ids[0] = Terraria.ID.NPCID.Retinazer;
                            }
                            if (NPC.downedMechBoss2 && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.TheDestroyer;
                            }
                            if (NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.SkeletronPrime;
                            }
                            if (NPC.downedPlantBoss && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.Plantera;
                            }
                            if (NPC.downedGolemBoss && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.Golem;
                            }
                            if (NPC.downedAncientCultist && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.CultistBoss;
                            }
                            if (NPC.downedMoonlord && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.MoonLordCore;
                            }
                            if (NPC.downedFishron && Main.rand.Next(3) == 0)
                            {
                                Ids[0] = Terraria.ID.NPCID.DukeFishron;
                            }
                        }
                        if (Ids[0] == Terraria.ID.NPCID.EaterofWorldsHead)
                            Stacks[0] = 10;
                        else
                            Stacks[0] = 1;
                        if (Ids[0] == 0)
                            goto retryEventSpawning;
                        ScoreCounter = 3000;
                    }
                    break;
                case 4: //Travelling
                    type = RequestType.TravellingQuest;
                    MovementFloat = Main.rand.Next(5000, 25001) * (1f + guardian.FriendshipLevel * 0.1f);
                    ScoreCounter = 1000 + (int)(MovementFloat / 50);
                    RequiresGuardianActive = true;
                    break;
                case 5: //Gem Collection
                    type = RequestType.ItemCollection;
                    if (Main.rand.Next(7) == 0)
                    {
                        Ids[0] = 999;
                    }
                    else
                    {
                        Ids[0] = 177 + Main.rand.Next(6);
                    }
                    Stacks[0] = (byte)(1 + guardian.FriendshipLevel / 5);
                    ScoreCounter = 1500 + Stacks[0] * 350;
                    break;
                case 6: //Do Invasion Events
                    type = RequestType.EventParticipation;
                    if (WorldGen.shadowOrbSmashed && player.statLifeMax >= 200 && NPC.downedGoblins)
                    {
                        Ids[0] = (int)EventList.GoblinArmy;
                    }
                    if (Main.hardMode)
                    {
                        if (Main.xMas && NPC.downedFrost && Main.rand.Next(3) == 0)
                        {
                            Ids[0] = (int)EventList.FrostArmy;
                        }
                        if (WorldGen.altarCount > 0 && NPC.downedPirates && Main.rand.Next(2) == 0)
                        {
                            Ids[0] = (int)EventList.PirateArmy;
                        }
                        if (NPC.downedGolemBoss && NPC.downedMartians && Main.rand.Next(3) == 0)
                        {
                            Ids[0] = (int)EventList.MartianArmy;
                        }
                    }
                    if (Ids[0] == 0)
                        goto retryEventSpawning;
                    Stacks[0] = (byte)(50 + 5 * (guardian.FriendshipLevel / 5));
                    if (Stacks[0] > 100)
                        Stacks[0] = 100;
                    ScoreCounter = 1000 + Stacks[0] * 15;
                    break;
                case 7: //Do Wave Events
                    type = RequestType.EventParticipation;
                    if (NPC.AnyNPCs(Terraria.ID.NPCID.DD2Bartender) && Main.rand.Next(2) == 0)
                    {
                        Ids[0] = (int)EventList.DD2Event;
                        byte Tier = 1;
                        if (Main.hardMode && NPC.downedMechBossAny)
                            Tier = 2;
                        if (Main.hardMode && NPC.downedGolemBoss)
                            Tier = 3;
                        switch (Tier)
                        {
                            case 1:
                                Stacks[0] = (byte)(2 + guardian.FriendshipLevel / 10);
                                break;
                            case 2:
                                Stacks[0] = (byte)(3 + guardian.FriendshipLevel / 10);
                                break;
                            case 3:
                                Stacks[0] = (byte)(5 + guardian.FriendshipLevel / 10);
                                break;
                        }
                    }
                    if (Main.hardMode && NPC.downedHalloweenKing && NPC.downedPlantBoss && Main.rand.Next(3) == 0)
                    {
                        Ids[0] = (int)EventList.PumpkinMoon;
                        Stacks[0] = (byte)(3 + guardian.FriendshipLevel / 10);
                        if (Stacks[0] > 15) Stacks[0] = 15;
                    }
                    if (Main.hardMode && NPC.downedChristmasSantank && NPC.downedPlantBoss && Main.rand.Next(3) == 0)
                    {
                        Ids[0] = (int)EventList.FrostMoon;
                        Stacks[0] = (byte)(5 + guardian.FriendshipLevel / 10);
                        if (Stacks[0] > 15) Stacks[0] = 15;
                    }
                    if (Ids[0] == 0)
                        goto retryEventSpawning;
                    ScoreCounter = 1000 + Stacks[0] * 750;
                    break;
                case 8: //Participate of a Party
                    if (!Terraria.GameContent.Events.BirthdayParty.PartyIsUp || !NPC.AnyNPCs(Terraria.ID.NPCID.PartyGirl))
                        goto retryEventSpawning;
                    type = RequestType.EventParticipation;
                    Ids[0] = (int)EventList.Party;
                    MovementFloat = Main.rand.Next(3000, 9001);
                    ScoreCounter = 2000;
                    break;
                case 9:
                    if (!player.HasItem(ItemID.BugNet) && !player.HasItem(ItemID.GoldenBugNet))
                    {
                        goto retryEventSpawning;
                    }
                    type = RequestType.ItemCollection;
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            Ids[0] = ItemID.Bunny;
                            break;
                        case 1:
                            Ids[0] = (Main.rand.NextDouble() < 0.4 ? ItemID.SquirrelRed : ItemID.Squirrel);
                            break;
                        case 2:
                            Ids[0] = ItemID.Bird;
                            break;
                    }
                    if (Main.rand.NextDouble() <= 0.6)
                    {
                        Ids[0] = ItemID.Penguin;
                    }
                    if (Main.rand.NextDouble() <= 0.2 && !Main.hardMode)
                    {
                        Ids[0] = ItemID.Scorpion;
                    }
                    if (Main.rand.NextDouble() <= 0.3 && !Main.hardMode)
                    {
                        Ids[0] = ItemID.Mouse;
                    }
                    if (Main.rand.NextDouble() <= 0.25)
                    {
                        Ids[0] = ItemID.Frog;
                    }
                    Stacks[0] = (byte)(1 + guardian.FriendshipLevel / 10);
                    ScoreCounter = 2200 + Stacks[0] * 200;
                    break;
                case 10:
                    if (player.statDefense < 10)
                        goto retryEventSpawning;
                    Ids[0] = 0;
                    type = RequestType.ItemCollection;
                    if(NPC.downedQueenBee)
                        Ids[0] = ItemID.Bezoar;
                    if (NPC.downedBoss3 && Main.rand.Next(2) == 0)
                        Ids[0] = ItemID.Nazar;
                    if (Main.hardMode && Main.rand.Next(2) == 0)
                    {
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                Ids[0] = ItemID.Vitamins;
                                break;
                            case 1:
                                Ids[0] = ItemID.FastClock;
                                break;
                            case 2:
                                Ids[0] = ItemID.TrifoldMap;
                                break;
                            case 3:
                                Ids[0] = ItemID.Megaphone;
                                break;
                            case 4:
                                Ids[0] = ItemID.Blindfold;
                                break;
                        }
                        if (NPC.downedPlantBoss && Main.rand.Next(2) == 0)
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    Ids[0] = ItemID.AdhesiveBandage;
                                    break;
                                case 1:
                                    Ids[0] = ItemID.ArmorPolish;
                                    break;
                            }
                        }
                    }
                    if (Ids[0] == 0)
                        goto retryEventSpawning;
                    Stacks[0] = 1;
                    ScoreCounter = 5000 + guardian.FriendshipLevel * 725;
                    break;
            }
            ScoreCounter += 100 * guardian.FriendshipLevel;
            ScoreCounter += 50 * (guardian.FriendshipLevel / 5);
            CreateRewards(ScoreCounter, player);
            FriendshipReward = GetFriendshipReward(guardian);
            if (type != RequestType.Talk && type != RequestType.ItemCollection && guardian.CanBeCalled)
                RequiresGuardianActive = Main.rand.NextDouble() < 0.333;
            if (type == RequestType.ItemCollection)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Ids[i] > 0)
                    {
                        Item i2 = new Item();
                        i2.SetDefaults(Ids[i]);
                        ObjectiveTexts[i] = i2.Name + " x" + Stacks[i];
                    }
                }
            }
            if (type == RequestType.BossPwning || type == RequestType.MobHunting)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Ids[i] > 0)
                    {
                        NPC i2 = new NPC();
                        i2.SetDefaults(Ids[i]);
                        ObjectiveTexts[i] = i2.TypeName;
                    }
                }
            }
        }

        public bool CompleteRequest(Player owner, TerraGuardian guardian, GuardianData data)
        {
            bool Completed = false;
            bool GuardianIsQuestGiver = guardian.Active && guardian.ID == data.ID;
            switch (type)
            {
                case RequestType.Talk:
                    {
                        Completed = true;
                        string Message = GuardianNPC.GuardianNPCPrefab.MessageParser(data.Base.TalkMessage(owner, guardian), guardian);
                        if (owner.talkNPC > -1 && NpcMod.GetGuardianNPC(data.ID, data.ModID) == owner.talkNPC)
                        {
                            Main.npcChatText = Message;
                        }
                        else if (owner.whoAmI == Main.myPlayer)
                        {
                            Main.NewText(guardian.Name + ": " + Message);
                        }
                        //if (GuardianIsQuestGiver) guardian.DisplayEmotion((TerraGuardian.Emotions)Ids[0]);
                    }
                    break;
                case RequestType.ItemCollection:
                    {
                        byte ItemCount = 0, CompletedCount = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Ids[i] > 0)
                            {
                                ItemCount++;
                                if (owner.CountItem(Ids[i]) >= Stacks[i])
                                {
                                    CompletedCount++;
                                }
                            }
                        }
                        if (CompletedCount >= ItemCount)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (Ids[i] > 0)
                                {
                                    for (int s = 0; s < 58; s++)
                                    {
                                        while (owner.inventory[s].type == Ids[i] && Stacks[i] > 0)
                                        {
                                            owner.inventory[s].stack--;
                                            Stacks[i]--;
                                            if (owner.inventory[s].stack == 0)
                                                owner.inventory[s].SetDefaults(0);
                                        }
                                    }
                                }
                            }
                            Completed = true;
                        }
                    }
                    break;
                case RequestType.MobHunting:
                case RequestType.BossPwning:
                    {
                        bool HasMobLeft = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Stacks[i] > 0)
                                HasMobLeft = true;
                        }
                        if (!HasMobLeft)
                            Completed = true;
                    }
                    break;
                case RequestType.EventParticipation:
                    {
                        switch ((EventList)Ids[0])
                        {
                            case EventList.DD2Event:
                            case EventList.FrostMoon:
                            case EventList.PumpkinMoon:
                                if (Stacks[0] <= 0)
                                    Completed = true;
                                break;
                            case EventList.Party:
                                if (MovementFloat <= 0)
                                    Completed = true;
                                break;
                            default:
                                if (Stacks[0] <= 0)
                                    Completed = true;
                                break;
                        }
                    }
                    break;
                case RequestType.TravellingQuest:
                    Completed = MovementFloat <= 0;
                    break;
            }
            if (Completed)
            {
                if(GuardianIsQuestGiver) guardian.DisplayEmotion(TerraGuardian.Emotions.Happy);
                foreach (Item reward in ItemRewards)
                {
                    int pos = Item.NewItem(owner.Center, reward.type, reward.stack);
                    if (pos < Main.maxItems)
                    {
                        Vector2 LastItemPos = Main.item[pos].position;
                        Main.item[pos] = reward;
                        Main.item[pos].position = LastItemPos;
                        Main.item[pos].owner = owner.whoAmI;
                        owner.GetItem(owner.whoAmI, Main.item[pos]);
                    }
                }
            }
            Active = !Completed;
            if (Completed)
            {
                SetNewRequestTimer();
                CheckIfGuardianShouldBeUnsummoned(owner, data);
            }
            return Completed;
        }

        public void CheckIfGuardianShouldBeUnsummoned(Player owner, GuardianData data)
        {
            if (type == RequestType.TravellingQuest && data.FriendshipLevel < data.Base.CallUnlockLevel)
            {
                if (PlayerMod.PlayerHasGuardianSummoned(owner, data.ID, data.ModID))
                {
                    Main.NewText(data.Name + " has returned to It's house.");
                    owner.GetModPlayer<PlayerMod>().DismissGuardian(data.ID, data.ModID);
                }
            }
        }

        public void Save(Terraria.ModLoader.IO.TagCompound tag, int? UniqueID)
        {
            string Suffix = (!UniqueID.HasValue ? "" : "_" + UniqueID.Value);
            tag.Add("IsActive" + Suffix, Active);
            tag.Add("RequestType" + Suffix, (byte)type);
            tag.Add("ObjectiveIds" + Suffix, Ids);
            tag.Add("ObjectiveStacks" + Suffix, Stacks);
            for (int o = 0; o < 3; o++ )
                tag.Add("ObjectiveTexts_" + o + Suffix, ObjectiveTexts[o]);
            tag.Add("RequestTimer" + Suffix, RequestTimer);
            tag.Add("MovementStack" + Suffix, MovementFloat);
            tag.Add("RewardCount" + Suffix, ItemRewards.Count);
            for (int r = 0; r < ItemRewards.Count; r++)
            {
                tag.Add("Reward_" + r + Suffix, ItemRewards[r]);
            }
        }

        public void Load(Terraria.ModLoader.IO.TagCompound tag, int ModVersion, int? UniqueID)
        {
            string Suffix = "";
            if (UniqueID.HasValue) Suffix = "_" + UniqueID.Value;
            Active = tag.GetBool("IsActive" + Suffix);
            type = (RequestType)tag.GetByte("RequestType" + Suffix);
            Ids = tag.GetIntArray("ObjectiveIds" + Suffix);
            Stacks = tag.GetByteArray("ObjectiveStacks" + Suffix);
            for (int o = 0; o < 3; o++)
            {
                ObjectiveTexts[o] = tag.GetString("ObjectiveTexts_" + o + Suffix);
            }
            RequestTimer = tag.GetInt("RequestTimer" + Suffix);
            MovementFloat = tag.GetFloat("MovementStack" + Suffix);
            int MaxItems = tag.GetInt("RewardCount" + Suffix);
            for (int r = 0; r < MaxItems; r++)
            {
                if (ModVersion < 45)
                {
                    int ItemID = tag.GetInt("RewardID_" + r + Suffix),
                        Stack = tag.GetInt("RewardStack_" + r + Suffix);
                    Item item = new Item();
                    item.SetDefaults(ItemID);
                    item.stack = Stack;
                    ItemRewards.Add(item);
                }
                else
                {
                    ItemRewards.Add(tag.Get<Item>("Reward_" + r + Suffix));
                }
            }
            if (ModVersion < 10)
                RequestTimer = 1800;
            if (ModVersion < 12)
            {
                Active = false;
                RequestTimer = 1800;
            }
        }

        public enum RequestType : byte
        {
            Talk,
            ItemCollection,
            MobHunting,
            BossPwning,
            TravellingQuest,
            EventParticipation
        }

        public enum EventList : byte
        {
            GoblinArmy,
            PirateArmy,
            MartianArmy,
            FrostArmy,
            DD2Event,
            Party,
            PumpkinMoon,
            FrostMoon
        }
    }
}
