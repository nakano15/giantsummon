using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianBountyQuest
    {
        public static bool SardineTalkedToAboutBountyQuests = false;
        public static bool SpawningBountyMob = false;
        public static int SignID = -1;
        public static bool IsAnnouncementBox = false;
        public static int SpawnStress = 0;
        public static int TargetMonsterID = 0, TargetMonsterSpawnPosition = -1;
        public static string TargetName = "", Suffix = ""; //How to recognize who defeated it?
        public static string TargetFullName { get { return TargetName + " the " + Suffix; } }
        public static SpawnBiome spawnBiome = SpawnBiome.Corruption;
        public static List<Modifiers> modifier = new List<Modifiers>();
        public static int ActionCooldown = 5;
        public const int NewRequestMinTime = 10 * 3600, NewRequestMaxTime = 20 * 3600,
            RequestEndMinTime = 20 * 3600, RequestEndMaxTime = 35 * 3600;
        public static int CoinReward = 0;
        public static Item[] RewardList = new Item[0];
        public const int SardineID = 2;
        public const string NoRequestText = "No bounty right now.";
        private static Dictionary<string, byte> BountyProgress = new Dictionary<string, byte>();
        public const byte BountyKilled = 1, BountyRewardRedeemed = 2;
        public static int[] BountyCounters = new int[5];

        public static void Save(Terraria.ModLoader.IO.TagCompound writer)
        {
            writer.Add("BQSardineTalkedTo", SardineTalkedToAboutBountyQuests);
            writer.Add("BQIsAnnouncementBox", IsAnnouncementBox);
            writer.Add("BQTargetMonsterID", TargetMonsterID);
            if (TargetMonsterID > 0)
            {
                writer.Add("BQSpawnStress", SpawnStress);
                writer.Add("BQTargetName", TargetName);
                writer.Add("BQSuffix", Suffix);
                writer.Add("BQSpawnBiome", (byte)spawnBiome);
                int Count = modifier.Count;
                writer.Add("BQModifier_Count", Count);
                for (int m = 0; m < Count; m++)
                {
                    writer.Add("BQModifier_" + m, (byte)modifier[m]);
                }
                writer.Add("BQCoinReward", CoinReward);
                writer.Add("BQRewardList", RewardList.Length);
                int c = 0;
                foreach (Item i in RewardList)
                {
                    writer.Add("BQReward_" + c, i);
                    c++;
                }
                writer.Add("BQBountyProgressEntries",BountyProgress.Count);
                c = 0;
                foreach (string key in BountyProgress.Keys)
                {
                    writer.Add("BQBountyPlayerName_" + c, key);
                    writer.Add("BQBountyPlayerProgress_" + c, BountyProgress[key]);
                    c++;
                }
            }
            writer.Add("BQActionCooldown", ActionCooldown);
        }

        public static void Load(Terraria.ModLoader.IO.TagCompound reader, int Version)
        {
            SardineTalkedToAboutBountyQuests = reader.GetBool("BQSardineTalkedTo");
            IsAnnouncementBox = reader.GetBool("BQIsAnnouncementBox");
            TargetMonsterID = reader.GetInt("BQTargetMonsterID");
            if (TargetMonsterID > 0)
            {
                SpawnStress = reader.GetInt("BQSpawnStress");
                TargetName = reader.GetString("BQTargetName");
                Suffix = reader.GetString("BQSuffix");
                spawnBiome = (SpawnBiome)reader.GetByte("BQSpawnBiome");
                if (Version < 44)
                {
                    TargetMonsterID = 0;
                }
                else
                {
                    int Count = reader.GetInt("BQModifier_Count");
                    for (int m = 0; m < Count; m++)
                    {
                        modifier.Add((Modifiers)reader.GetByte("BQModifier_" + m));
                    }
                }
                CoinReward = reader.GetInt("BQCoinReward");
                int ItemCount = reader.GetInt("BQRewardList");
                RewardList = new Item[ItemCount];
                for (int i = 0; i < ItemCount; i++ )
                {
                    RewardList[i] = reader.Get<Item>("BQReward_" + i);
                }
                if (MainMod.ModVersion >= 37)
                {
                    ItemCount = reader.GetInt("BQBountyProgressEntries");
                    for (int i = 0; i < ItemCount; i++)
                    {
                        string key = reader.GetString("BQBountyPlayerName_" + i);
                        byte Progress = reader.GetByte("BQBountyPlayerProgress_" + i);
                        BountyProgress.Add(key, Progress);
                    }
                }
            }
            ActionCooldown = reader.GetInt("BQActionCooldown");
        }

        public static void Reset()
        {
            SardineTalkedToAboutBountyQuests = false;
            SignID = -1;
            IsAnnouncementBox = false;
            TargetMonsterID = 0;
            TargetMonsterSpawnPosition = -1;
            BountyProgress.Clear();
            SetDefaultCooldown();
        }

        public static void ApplyModifier(NPC npc)
        {
            npc.lifeMax *= 10;
            npc.damage += 10;
            npc.defense += 5;
            npc.knockBackResist *= 0.6f;
            foreach (Modifiers mod in modifier)
            {
                switch (mod)
                {
                    case Modifiers.Boss:
                        npc.lifeMax *= 5;
                        break;
                    case Modifiers.ExtraHealth:
                        npc.lifeMax += (int)(npc.lifeMax * 0.2f);
                        break;
                    case Modifiers.ExtraDamage:
                        npc.damage += (int)(npc.damage * 0.3f);
                        break;
                    case Modifiers.ExtraDefense:
                        npc.defense += (int)(npc.defense * 0.2f);
                        break;
                    case Modifiers.KBImmunity:
                        npc.knockBackResist = 0;
                        break;
                    case Modifiers.Noob:
                        npc.knockBackResist *= 2;
                        npc.lifeMax = (int)(npc.lifeMax * 0.05f);
                        npc.damage -= (int)(npc.damage * 0.7f);
                        npc.defense -= (int)(npc.defense * 0.5f);
                        break;
                }
            }
            npc.rarity = 10;
        }

        public static void UpdateBountyNPC(NPC npc)
        {
            const int ModifierVariable = 0;
            if (modifier.Contains(Modifiers.HealthRegen))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[0] >= 30)
                {
                    BountyCounters[0] -= 30;
                    npc.life += npc.defense;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                }
            }
            if (modifier.Contains(Modifiers.Reaper))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 180)
                {
                    bool Trigger = BountyCounters[ModifierVariable] % 20 == 0;
                    if (BountyCounters[ModifierVariable] >= 300)
                        BountyCounters[ModifierVariable] -= 300;
                    if (Trigger && npc.target > -1)
                    {
                        Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                        Dir.Normalize();
                        Projectile.NewProjectile(npc.Center, Dir * 8f, 44, (int)(npc.defDamage * 1.2f), 3f);
                    }
                }
            }
            if (modifier.Contains(Modifiers.Cyclops))
            {
                BountyCounters[ModifierVariable]++;
                float DelayTime = 300 - 180 * (1f - (float)npc.life / npc.lifeMax);
                if (BountyCounters[ModifierVariable] >= DelayTime)
                {
                    BountyCounters[ModifierVariable] -= (int)DelayTime;
                    if (npc.target > -1)
                    {
                        Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                        Dir.Normalize();
                        Vector2 SpawnPosition = npc.position;
                        SpawnPosition.X += npc.width * 0.5f;
                        SpawnPosition.Y += npc.height * 0.25f;
                        Projectile.NewProjectile(SpawnPosition, Dir * 10f, 259, (int)(npc.defDamage * 0.8f), 3f);
                    }
                }
            }
            if (modifier.Contains(Modifiers.GoldenShower))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 300)
                {
                    bool Trigger = BountyCounters[ModifierVariable] % 5 == 0;
                    if (BountyCounters[ModifierVariable] >= 360)
                        BountyCounters[ModifierVariable] -= 360;
                    if (Trigger && npc.target > -1)
                    {
                        Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                        Dir.Normalize();
                        Vector2 ShotPosition = npc.position;
                        ShotPosition.X += npc.width * 0.5f;
                        ShotPosition.Y += npc.height * 0.6f;
                        Projectile.NewProjectile(ShotPosition, Dir * 8f, 288, (int)(npc.defDamage * 0.75f), 3f);
                    }
                }
            }
            if (modifier.Contains(Modifiers.Haunted))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 300)
                {
                    BountyCounters[ModifierVariable] -= (int)300;
                    if (npc.target > -1)
                    {
                        Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                        Dir.Normalize();
                        Projectile.NewProjectile(npc.Center, Dir * 10f, 293, (int)(npc.defDamage * 1.25f), 3f);
                    }
                }
            }
            if (modifier.Contains(Modifiers.Alchemist))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 300)
                {
                    BountyCounters[ModifierVariable] -= (int)300;
                    for (int x = -2; x <= 2; x++)
                    {
                        Vector2 Dir = new Vector2(x * 3f, -8f);
                        Projectile.NewProjectile(npc.Center, Dir, 328 - Math.Abs(x), (int)(npc.defDamage * 1.25f), 3f);
                    }
                }
            }
            if (modifier.Contains(Modifiers.Sharknado))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 60 * 30)
                {
                    BountyCounters[ModifierVariable] -= 60 * 30;
                    if (npc.target > -1)
                    {
                        Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                        Dir.Normalize();
                        Projectile.NewProjectile(npc.Center, Dir * 10f, 385, (int)(npc.defDamage * 1.25f), 3f);
                    }
                }
            }
            if (modifier.Contains(Modifiers.Sapper))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 60 * 15)
                {
                    BountyCounters[ModifierVariable] -= 60 * 15;
                    if (npc.target > -1)
                    {
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, 387);
                    }
                }
            }
            if (modifier.Contains(Modifiers.Cursed))
            {
                BountyCounters[ModifierVariable]++;
                if (BountyCounters[ModifierVariable] >= 60)
                {
                    BountyCounters[ModifierVariable] -= 60;
                    if (npc.target > -1)
                    {
                        Projectile.NewProjectile(npc.Center, Vector2.Zero, 596, (int)(npc.defDamage * 1.25f), 3f);
                    }
                }
            }
        }

        public static float GetExtraRewardFromModifiers()
        {
            float Extra = 1;
            for (int m = 0; m < Enum.GetValues(typeof(Modifiers)).Length; m++)
            {
                Modifiers mod = (Modifiers)m;
                if (modifier.Contains(mod))
                {
                    switch (mod)
                    {
                        case Modifiers.Boss:
                            Extra += 0.5f;
                            break;
                        case Modifiers.Noob:
                            Extra -= 0.5f;
                            break;
                        case Modifiers.ExtraHealth:
                            Extra += 0.15f;
                            break;
                        case Modifiers.ExtraDamage:
                            Extra += 0.3f;
                            break;
                        case Modifiers.ExtraDefense:
                            Extra += 0.2f;
                            break;
                        case Modifiers.HealthRegen:
                            Extra += 0.1f;
                            break;
                        case Modifiers.KBImmunity:
                            Extra += 0.4f;
                            break;
                        case Modifiers.Reaper:
                            Extra += 0.03f;
                            break;
                        case Modifiers.Cyclops:
                            Extra += 0.2f;
                            break;
                        case Modifiers.GoldenShower:
                            Extra += 0.1f;
                            break;
                        case Modifiers.Haunted:
                            Extra += 0.25f;
                            break;
                        case Modifiers.Alchemist:
                            Extra += 0.05f;
                            break;
                        case Modifiers.Sharknado:
                            Extra += 0.5f;
                            break;
                        case Modifiers.Sapper:
                            Extra += 0.35f;
                            break;
                        case Modifiers.Cursed:
                            Extra += 0.08f;
                            break;
                    }
                }
            }
            return Extra;
        }

        private static void PlayerHelpedKillBounty(Player player)
        {
            if (!BountyProgress.ContainsKey(player.name))
            {
                BountyProgress.Add(player.name, BountyKilled);
                if (player.whoAmI == Main.myPlayer)
                {
                    if (PlayerMod.HasGuardianSummoned(player, SardineID))
                    {
                        Main.NewText(PlayerMod.GetPlayerSummonedGuardian(player, SardineID).Name + ": Nice job, let's go back home and check the sign so I can give your reward.");
                    }
                    else
                    {
                        Main.NewText("Bounty hunted successfully. Report back to " + NpcMod.GetGuardianNPCName(SardineID) + ".");
                    }
                }
            }
        }

        public static bool PlayerCanRedeemReward(Player player)
        {
            return BountyProgress.ContainsKey(player.name) && BountyProgress[player.name] == BountyKilled;
        }

        public static bool PlayerAlreadyRedeemedReward(Player player)
        {
            return BountyProgress.ContainsKey(player.name) && BountyProgress[player.name] == BountyRewardRedeemed;
        }

        public static bool PlayerRedeemReward(Player player)
        {
            //Can lose items if inventory is full
            if (PlayerCanRedeemReward(player))
            {
                int c = CoinReward, s = 0, g = 0, p = 0;
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
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.CopperCoin);
                    i.stack = c;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                if (s > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.SilverCoin);
                    i.stack = s;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                if (g > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.GoldCoin);
                    i.stack = g;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                if (p > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.PlatinumCoin);
                    i.stack = p;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                foreach (Item i in RewardList)
                {
                    Item ni = i.Clone();
                    GiveRewardToPlayer(ni, player);
                    //player.GetItem(player.whoAmI, ni, true);
                }
                BountyProgress[player.name] = BountyRewardRedeemed;
                return true;
            }
            return false;
        }

        public static void GiveRewardToPlayer(Item i, Player player)
        {
            int ItemPos = Item.NewItem(player.Center, i.type);
            if (ItemPos > -1)
            {
                Main.item[ItemPos] = i;
                Main.item[ItemPos].Center = player.Center;
                //player.GetItem(player.whoAmI, Main.item[ItemPos]);
            }
        }

        public static void OnMobKilled(NPC npc)
        {
            if (npc.target > -1 && npc.target < Main.maxPlayers && Main.player[npc.target].active && !Main.player[npc.target].dead && IsPlayerOnBountyBiome(Main.player[npc.target]))
            {
                SpawnStress--;
            }
        }

        public static void OnBountyMonsterKilled(NPC npc)
        {
            for (int i = 0; i < 255; i++)
            {
                if (npc.playerInteraction[i] && Main.player[i].active)
                {
                    PlayerHelpedKillBounty(Main.player[i]);
                }
            }
        }

        public static byte GetBountyState(Player player)
        {
            if (!BountyProgress.ContainsKey(player.name))
                return 0;
            return BountyProgress[player.name];
        }

        public static void SpawnBountyMobOnPlayer(Player player)
        {
            TargetMonsterSpawnPosition = -1;
            int[] LastNpcList = new int[200];
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active)
                    LastNpcList[n] = Main.npc[n].type;
                else
                    LastNpcList[n] = 0;

            }
            int SpawnMinX = (int)player.Center.X / 16, SpawnMinY = (int)player.Center.Y / 16, SpawnMaxX = SpawnMinX, SpawnMaxY = SpawnMinY;
            SpawnMinX -= (int)(NPC.sWidth / 16 * 0.7);
            SpawnMaxX += (int)(NPC.sWidth / 16 * 0.7);
            SpawnMinY -= (int)(NPC.sWidth / 16 * 0.52);
            SpawnMaxY += (int)(NPC.sWidth / 16 * 0.52);
            if (SpawnMinX < 0) SpawnMinX = 0;
            if (SpawnMinY < 0) SpawnMinY = 0;
            if (SpawnMaxX >= Main.maxTilesX) SpawnMaxX = Main.maxTilesX;
            if (SpawnMaxY >= Main.maxTilesY) SpawnMaxY = Main.maxTilesY;
            for (int attempt = 0; attempt < 40; attempt++)
            {
                int SpawnX = Main.rand.Next(SpawnMinX, SpawnMaxX + 1), SpawnY = Main.rand.Next(SpawnMinY, SpawnMaxY + 1);
                if ((!Main.tile[SpawnX, SpawnY].active() || !Main.tileSolid[Main.tile[SpawnX, SpawnY].type]) && Main.tile[SpawnX, SpawnY + 1].active() && Main.tileSolid[Main.tile[SpawnX, SpawnY + 1].type])
                {
                    bool MaySpawn = true;
                    switch (spawnBiome)
                    {
                        case SpawnBiome.Dungeon:
                            if (!((Main.tile[SpawnX, SpawnY].wall >= 7 && Main.tile[SpawnX, SpawnY].wall <= 9) || (Main.tile[SpawnX, SpawnY].wall >= 94 && Main.tile[SpawnX, SpawnY].wall <= 99)))
                            {
                                MaySpawn = false;
                            }
                            break;
                        case SpawnBiome.Underworld:
                            MaySpawn = SpawnX >= Main.maxTilesY - 130;
                            break;
                    }
                    if (MaySpawn)
                    {
                        SpawningBountyMob = true;
                        int NpcPos = NPC.NewNPC(SpawnX * 16, SpawnY * 16, TargetMonsterID);
                        SpawningBountyMob = false;
                        if (NpcPos < 200 && NpcPos > -1)
                        {
                            if (MainMod.UseNewMonsterModifiersSystem)
                            {
                                Main.npc[NpcPos].GetGlobalNPC<NpcMod>().mobType = NpcMod.GetBossDifficulty(Main.npc[NpcPos], 1);
                                //if (Main.npc[NpcPos].GetGlobalNPC<NpcMod>().mobType < MobTypes.Elite)
                                //    Main.npc[NpcPos].GetGlobalNPC<NpcMod>().mobType = MobTypes.Elite;
                            }
                            TargetMonsterSpawnPosition = NpcPos;
                            Microsoft.Xna.Framework.Vector2 Direction = Main.npc[TargetMonsterSpawnPosition].Center - Main.player[Main.myPlayer].Center;
                            string DirectionText = GetDirectionText(Direction);
                            Main.NewText(TargetFullName + " has appeared to the " + DirectionText + "!");
                            return;
                        }
                    }
                }
            }
        }

        public static string GetDirectionText(Microsoft.Xna.Framework.Vector2 Direction)
        {
            Direction.Normalize();
            bool CountVerticalDiference = Math.Abs(Direction.Y) >= 0.33f, CountHorizontalDiference = Math.Abs(Direction.X) >= 0.33f;
            string DirectionText = "";
            if (CountVerticalDiference && CountHorizontalDiference)
            {
                if (Direction.Y > 0) DirectionText += "South";
                else DirectionText += "North";
                if (Direction.X > 0) DirectionText += "east";
                else DirectionText += "west";
            }
            else if (CountVerticalDiference)
            {
                if (Direction.Y > 0) DirectionText = "South";
                else DirectionText = "North";
            }
            else if (CountHorizontalDiference)
            {
                if (Direction.X > 0) DirectionText = "East";
                else DirectionText = "West";
            }
            return DirectionText;
        }

        public static bool IsPlayerOnBountyBiome(Player player)
        {
            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    return player.ZoneCorrupt;
                case SpawnBiome.Crimson:
                    return player.ZoneCrimson;
                case SpawnBiome.Dungeon:
                    return player.ZoneDungeon;
                case SpawnBiome.Hallow:
                    return player.ZoneHoly;
                case SpawnBiome.Jungle:
                    return player.ZoneJungle;
                case SpawnBiome.Underworld:
                    return player.ZoneUnderworldHeight;
            }
            return false;
        }

        public static void TryFindingASign()
        {
            int SardinePosition = NpcMod.GetGuardianNPC(SardineID);
            SignID = -1;
            if (SardinePosition > -1)
            {
                int HPX = Main.npc[SardinePosition].homeTileX,
                    HPY = Main.npc[SardinePosition].homeTileY;
                if (HPX > -1 && HPY > -1)
                {
                    int TileCount = 0;
                    List<KeyValuePair<int, int>> NextTileCheck = new List<KeyValuePair<int, int>>(),
                        CheckedTiles = new List<KeyValuePair<int, int>>();
                    NextTileCheck.Add(new KeyValuePair<int, int>(HPX, HPY));
                    while (TileCount < 60 && NextTileCheck.Count > 0)
                    {
                        KeyValuePair<int, int> CurrentTile = NextTileCheck[0];
                        NextTileCheck.RemoveAt(0);
                        CheckedTiles.Add(CurrentTile);
                        int TileX = CurrentTile.Key, TileY = CurrentTile.Value;
                        if (TileX >= Main.leftWorld && TileX < Main.rightWorld &&
                            TileY >= Main.topWorld && TileY < Main.bottomWorld)
                        {
                            if (Main.tile[TileX, TileY].active() && Main.tileSign[Main.tile[TileX, TileY].type])
                            {
                                SignID = Sign.ReadSign(TileX, TileY, true);
                                if (SignID > -1)
                                {
                                    IsAnnouncementBox = (Main.tile[TileX, TileY].type == Terraria.ID.TileID.AnnouncementBox);
                                    NextTileCheck.Clear();
                                    CheckedTiles.Clear();
                                    break;
                                }
                            }
                            else
                            {
                                int tx = TileX, ty = TileY;
                                ty--;
                                if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                                {
                                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                    if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                    {
                                        NextTileCheck.Add(key);
                                    }
                                }
                                tx = TileX;
                                ty = TileY;
                                tx++;
                                if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                                {
                                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                    if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                    {
                                        NextTileCheck.Add(key);
                                    }
                                }
                                tx = TileX;
                                ty = TileY;
                                ty++;
                                if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                                {
                                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                    if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                    {
                                        NextTileCheck.Add(key);
                                    }
                                }
                                tx = TileX;
                                ty = TileY;
                                tx--;
                                if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                                {
                                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                    if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                    {
                                        NextTileCheck.Add(key);
                                    }
                                }
                            }
                        }
                        TileCount++;
                    }
                }
                if (SignID > -1)
                {
                    //UpdateBountyBoardText();
                    Main.sign[SignID].text = "Request coming soon...";
                }
            }
        }

        public static void Update()
        {
            int SardinePosition = NpcMod.GetGuardianNPC(SardineID);
            if (SignID > -1 && Main.player[Main.myPlayer].sign == SignID && PlayerRedeemReward(Main.player[Main.myPlayer]))
            {
                string Dialogue = "";
                if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], SardineID))
                {
                    Dialogue = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], SardineID).Name;
                }
                else
                {
                    Dialogue = GuardianBase.GetGuardianBase(SardineID).Name;
                }
                Dialogue += ": Nice job, here's your reward.";
                Main.NewText(Dialogue);
            }
            if (TargetMonsterSpawnPosition > -1)
            {
                if (!Main.npc[TargetMonsterSpawnPosition].active || Main.npc[TargetMonsterSpawnPosition].type != TargetMonsterID)
                {
                    TargetMonsterSpawnPosition = -1;
                }
            }
            if (ActionCooldown % 300 == 0)
            {
                if (SignID > -1 && !SignExists())
                {
                    Main.sign[SignID] = null;
                    SignID = -1;
                }
                if (SignID == -1)
                {
                    TryFindingASign();
                }
            }
            if (ActionCooldown % 60 == 0)
            {
                if (TargetMonsterID > 0 && TargetMonsterSpawnPosition == -1 && SpawnStress <= 0)
                {
                    for (int p = 0; p < 255; p++)
                    {
                        Player player = Main.player[p];
                        if (player.active && !player.dead)
                        {
                            byte Progress = GetBountyState(player);
                            if (Progress == 0)
                            {
                                bool TrySpawning = IsPlayerOnBountyBiome(player);
                                if (TrySpawning)
                                    TrySpawning = Main.rand.NextDouble() < 0.01;
                                if (TrySpawning)
                                {
                                    SpawnBountyMobOnPlayer(player);
                                    if (TargetMonsterSpawnPosition > -1)
                                    {
                                        ResetSpawnStress(false);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                UpdateBountyBoardText();
            }
            if (ActionCooldown <= 0)
            {
                TryFindingASign();
                if (SignID > -1)
                {
                    if (!SardineTalkedToAboutBountyQuests)
                    {
                        SetDefaultCooldown();
                    }
                    else
                    {
                        if (TargetMonsterID == 0)
                        {
                            GenerateRequest();
                        }
                        else
                        {
                            SetDefaultCooldown();
                            TargetMonsterID = 0;
                            UpdateBountyBoardText();
                            Main.NewText("Bounty Hunting Ended", Microsoft.Xna.Framework.Color.MediumPurple);
                        }
                    }
                }
                else
                {
                    SetDefaultCooldown();
                }
            }
            ActionCooldown--;
        }

        public static bool SignExists()
        {
            return SignID > -1 && (Main.sign[SignID] != null && Main.tile[Main.sign[SignID].x, Main.sign[SignID].y].active() && 
                Main.tileSign[Main.tile[Main.sign[SignID].x, Main.sign[SignID].y].type]);
        }

        public static void GenerateRequest()
        {
            for (int i = 0; i < BountyCounters.Length; i++)
            {
                BountyCounters[i] = 0;
            }
            if (!Main.hardMode || (Main.hardMode && Main.rand.Next(2) == 0))
            {
                if (!WorldGen.crimson)
                    spawnBiome = SpawnBiome.Corruption;
                else
                    spawnBiome = SpawnBiome.Crimson;
            }
            else
            {
                spawnBiome = SpawnBiome.Hallow;
            }
            if (Main.rand.Next(2) == 0)
            {
                if (Main.rand.Next(2) == 0 && NPC.downedBoss3)
                {
                    spawnBiome = SpawnBiome.Dungeon;
                }
                else if (Main.rand.Next(3) == 0)
                {
                    spawnBiome = SpawnBiome.Underworld;
                }
                else
                {
                    spawnBiome = SpawnBiome.Jungle;
                }
            }
            TargetName = "";
            Suffix = "";
            TargetMonsterID = 0;
            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Corruptor;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Slimer;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.SeekerHead;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.CursedHammer;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.BigMimicCorruption;
                                    break;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                TargetMonsterID = NPCID.DevourerHead;
                            }
                            else
                            {
                                TargetMonsterID = NPCID.EaterofSouls;
                            }
                        }
                        TargetName = NameGen(new string[]{"di", "sea", "sed", "ea", "ter", "of", "so", "ul", "de", "vou", "rer", "cor", "rup", "tor", "sli", "mer", "see", "ker"});
                        Suffix = GetRandomString(new string[] { "Plague Bearer", "Thousand Souls", "Corruption Spreader", "World Destroyer", "Reaper" });
                    }
                    break;
                case SpawnBiome.Crimson:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Crimslime;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Herpling;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.CrimsonAxe;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.BigMimicCrimson;
                                    break;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                TargetMonsterID = NPCID.FaceMonster;
                            }
                            else
                            {
                                TargetMonsterID = NPCID.Crimera;
                            }
                        }
                        TargetName = NameGen(new string[] { "crim", "son", "blo", "od", "cri", "me", "ra", "fa", "ce", "mons", "ter", "herp", "ling" });
                        Suffix = GetRandomString(new string[] { "Blood Feaster", "Defiler", "Goremancer", "Arterial Traveller", "Heart Breaker" });
                    }
                    break;
                case SpawnBiome.Hallow:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Pixie;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Unicorn;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.ChaosElemental;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.EnchantedSword;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.BigMimicHallow;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "pi", "xie", "cha", "os","ele", "men", "tal", "uni", "corn", "en", "chan", "ted" });
                        Suffix = GetRandomString(new string[] { "Hallowed Inquisitioner", "Rainbow of Suffering", "Nausea Inducer", "Annoying Thing", "Prismatic Ray" });
                    }
                    break;

                case SpawnBiome.Jungle:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(6))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Derpling;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.GiantTortoise;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.GiantFlyingFox;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.MossHornet;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.Moth;
                                    break;
                                case 5:
                                    TargetMonsterID = NPCID.BigMimicJungle;
                                    break;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                TargetMonsterID = NPCID.Hornet;
                            }
                            else
                            {
                                TargetMonsterID = NPCID.JungleBat;
                            }
                        }
                        TargetName = NameGen(new string[] { "fox", "hor", "net", "jun", "gle", "fly", "ing", "tor", "toi", "se", "moss", "derp", "ling" });
                        Suffix = GetRandomString(new string[] { "Deadly Poison", "Arms Dealer Hunter", "Catnapper", "Armor Piercer", "Home Wrecker" });
                    }
                    break;

                case SpawnBiome.Dungeon:
                    {
                        bool IsHardmodeDungeon = Main.hardMode && NPC.downedPlantBoss;
                        if (IsHardmodeDungeon)
                        {
                            switch (Main.rand.Next(12))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.BlueArmoredBonesSword;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.RustyArmoredBonesAxe;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.HellArmoredBonesMace;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.Necromancer;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.RaggedCaster;
                                    break;
                                case 5:
                                    TargetMonsterID = NPCID.DiabolistRed;
                                    break;
                                case 6:
                                    TargetMonsterID = NPCID.SkeletonCommando;
                                    break;
                                case 7:
                                    TargetMonsterID = NPCID.SkeletonSniper;
                                    break;
                                case 8:
                                    TargetMonsterID = NPCID.TacticalSkeleton;
                                    break;
                                case 9:
                                    TargetMonsterID = NPCID.GiantCursedSkull;
                                    break;
                                case 10:
                                    TargetMonsterID = NPCID.BoneLee;
                                    break;
                                case 11:
                                    TargetMonsterID = NPCID.Paladin;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.AngryBones;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.DarkCaster;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.CursedSkull;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.DungeonSlime;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "ske", "le", "ton", "an","gry","bo", "nes", "cas", "ter","cur","sed","dun", "ge", "on", "pa", "la", "din" });
                        Suffix = GetRandomString(new string[] { "Scary Spooky", "Dead Awakener", "Enemy of World", "He-Man's Nemesis", "Bone Breaker" });
                    }
                    break;

                case SpawnBiome.Underworld:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.RedDevil;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Lavabat;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Demon;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.BoneSerpentHead;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.FireImp;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "de", "mon", "bo", "ne", "ser", "pent", "he" , "ad", "fi", "re", "imp", "red", "vil", "la", "va", "bat" });
                        Suffix = GetRandomString(new string[] { "Pact Maker", "Hell Breaker", "Torturer", "Lava Eater" });
                    }
                    break;
            }

            modifier.Clear();
            modifier.Add((Modifiers)Main.rand.Next(7));
            float ChanceCounter = 0.75f;
            while (Main.rand.NextDouble() < ChanceCounter)
            {
                Modifiers newMod = (Modifiers)Main.rand.Next(7);
                if (!modifier.Contains(newMod))
                    modifier.Add(newMod);
                else
                    break;
                ChanceCounter *= 0.5f;
            }
            if (Main.rand.NextDouble() <= 0.333)
            {
                if (NPC.downedGolemBoss && Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.Cyclops);
                else if (NPC.downedFishron && Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.Sharknado);
                else if (Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.GoldenShower);
                else if (Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.Haunted);
                else if (Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.Alchemist);
                else if (Main.hardMode && Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.Sapper);
                else if (Main.rand.NextDouble() < 0.4)
                    modifier.Add(Modifiers.Cursed);
                else
                    modifier.Add(Modifiers.Reaper);
            }
            CoinReward = 5000 + ExtraCoinRewardFromProgress();
            CoinReward = (int)(GetExtraRewardFromModifiers() * CoinReward * (Main.rand.Next(80, 121) * 0.01f));

            CreateRewards(GetExtraRewardFromModifiers());

            ActionCooldown = RequestEndMinTime + Main.rand.Next(RequestEndMaxTime - RequestEndMinTime + 1);

            string AnnounceText = "New Bounty Quest available!";
            if (IsAnnouncementBox)
            {
                AnnounceText += "\nHunt " + TargetFullName + " in the " + spawnBiome.ToString() + ".";
                if (CoinReward > 0 || RewardList.Length > 0)
                {
                    AnnounceText += "\nReward: ";
                }
                if (CoinReward > 0)
                {
                    int p = 0, g = 0, s = 0, c = CoinReward;
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
                    if (p > 0)
                    {
                        AnnounceText += "[i/s" + p + ":" + Terraria.ID.ItemID.PlatinumCoin + "]";
                    }
                    if (g > 0)
                    {
                        AnnounceText += "[i/s" + g + ":" + Terraria.ID.ItemID.GoldCoin + "]";
                    }
                    if (s > 0)
                    {
                        AnnounceText += "[i/s" + s + ":" + Terraria.ID.ItemID.SilverCoin + "]";
                    }
                    if (c > 0)
                    {
                        AnnounceText += "[i/s" + c + ":" + Terraria.ID.ItemID.CopperCoin + "]";
                    }
                    //AnnounceText += Main.ValueToCoins(CoinReward);
                }
                if (RewardList.Length > 0)
                {
                    AnnounceText += " and ";// +RewardList[0].HoverName;
                    if (RewardList[0].prefix > 0)
                    {
                        AnnounceText += "[i/p" + RewardList[0].prefix + ":" + RewardList[0].type + "]";
                    }
                    else
                    {
                        AnnounceText += "[i/s" + RewardList[0].stack + ":" + RewardList[0].type + "]";
                    }
                }
                AnnounceText += ".";
            }
            Main.NewTextMultiline(AnnounceText, false, Microsoft.Xna.Framework.Color.MediumPurple);

            BountyProgress.Clear();

            ResetSpawnStress(true);

            UpdateBountyBoardText();
        }

        public static void ResetSpawnStress(bool OnGeneration)
        {
            if (OnGeneration)
                SpawnStress = Main.rand.Next(20, 41);
            else
                SpawnStress = Main.rand.Next(10, 21);
        }

        public static void CreateRewards(float RewardMod)
        {
            List<Item> Rewards = new List<Item>();
            Item i;
            if (Main.rand.NextDouble() < 0.01 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.PackLeaderNecklace>());
                        break;
                    case 1:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.GoldenShowerPapyrus>());
                        break;
                    case 2:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.FirstSymbol>());
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.1 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ItemID.FishermansGuide);
                        break;
                    case 1:
                        i.SetDefaults(ItemID.WeatherRadio);
                        break;
                    case 2:
                        i.SetDefaults(ItemID.Sextant);
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.2 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ItemID.LifeformAnalyzer);
                        break;
                    case 1:
                        i.SetDefaults(ItemID.MetalDetector);
                        break;
                    case 2:
                        i.SetDefaults(ItemID.Radar);
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.05f * RewardMod)
            {
                i = new Item();
                if (Main.rand.NextDouble() < 0.75)
                {
                    i.SetDefaults(ItemID.MagicMirror);
                }
                else
                {
                    i.SetDefaults(ItemID.PocketMirror);
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.1f * RewardMod)
            {
                i = GetRandomAccessory();
                if (i != null)
                    Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.15 * RewardMod)
            {
                i = GetRandomWeaponID();
                if (i != null)
                {
                    Rewards.Add(i);
                }
            }
            if (Main.rand.NextDouble() < 0.1 * RewardMod)
            {
                i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.SkillResetPotion>());
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.25 * RewardMod)
            {
                i = new Item();
                List<int> BossSpawnItems = new List<int>();
                //Check each boss that has been killed, then add the respective item to the boss spawn item list.
                if (NPC.downedBoss1)
                    BossSpawnItems.Add(ItemID.SuspiciousLookingEye);
                if (NPC.downedBoss2)
                {
                    if (WorldGen.crimson)
                        BossSpawnItems.Add(ItemID.BloodySpine);
                    else
                        BossSpawnItems.Add(ItemID.WormFood);
                }
                if (NPC.downedBoss3)
                    BossSpawnItems.Add(ItemID.ClothierVoodooDoll);
                if (NPC.downedSlimeKing)
                    BossSpawnItems.Add(ItemID.SlimeCrown);
                if (NPC.downedQueenBee)
                    BossSpawnItems.Add(ItemID.Abeemination);
                if (Main.hardMode)
                    BossSpawnItems.Add(ItemID.GuideVoodooDoll);
                if (NPC.downedMechBossAny)
                {
                    BossSpawnItems.Add(ItemID.MechanicalEye);
                    BossSpawnItems.Add(ItemID.MechanicalSkull);
                    BossSpawnItems.Add(ItemID.MechanicalWorm);
                }
                if (NPC.downedGolemBoss)
                    BossSpawnItems.Add(ItemID.LihzahrdPowerCell);
                if (NPC.downedFishron)
                    BossSpawnItems.Add(ItemID.TruffleWorm);
                if (NPC.downedMoonlord)
                    BossSpawnItems.Add(ItemID.CelestialSigil);
                i.SetDefaults(BossSpawnItems[Main.rand.Next(BossSpawnItems.Count)]);
                /*if (i.maxStack > 0)
                {
                    i.stack += Main.rand.Next((int)(3 * RewardMod));
                }*/
                Rewards.Add(i);
            }
            if (!MainMod.NoEtherItems && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.EtherFruit>());
                if (Main.rand.NextDouble() < 0.6 * RewardMod)
                    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (!MainMod.NoEtherItems && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.EtherHeart>());
                //if (Main.rand.NextDouble() < 0.4 * RewardMod)
                //    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.LifeFruit);
                if (Main.rand.NextDouble() < 0.6 * RewardMod)
                    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.LifeCrystal);
                //if (Main.rand.NextDouble() < 0.4 * RewardMod)
                //    i.stack += Main.rand.Next((int)(RewardMod));
                Rewards.Add(i);
            }
            if (spawnBiome != SpawnBiome.Underworld && Main.rand.Next(5) == 0)
            {
                i = new Item();
                switch (spawnBiome)
                {
                    case SpawnBiome.Corruption:
                        i.SetDefaults(ItemID.CorruptFishingCrate);
                        break;
                    case SpawnBiome.Crimson:
                        i.SetDefaults(ItemID.CrimsonFishingCrate);
                        break;
                    case SpawnBiome.Dungeon:
                        i.SetDefaults(ItemID.DungeonFishingCrate);
                        break;
                    case SpawnBiome.Hallow:
                        i.SetDefaults(ItemID.HallowedFishingCrate);
                        break;
                    case SpawnBiome.Jungle:
                        i.SetDefaults(ItemID.JungleFishingCrate);
                        break;
                }
                i.stack += Main.rand.Next((int)(3 * RewardMod));
                Rewards.Add(i);
            }
            if (Main.rand.Next(5) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.HerbBag);
                i.stack += Main.rand.Next((int)(3 * RewardMod));
                Rewards.Add(i);
            }
            RewardList = Rewards.ToArray();
        }

        public static Item GetRandomWeaponID()
        {
            int WeaponID = 0;
            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            WeaponID = ItemID.BallOHurt;
                            break;
                        case 1:
                            WeaponID = ItemID.DemonBow;
                            break;
                        case 2:
                            WeaponID = ItemID.Musket;
                            break;
                        case 3:
                            WeaponID = ItemID.Vilethorn;
                            break;
                        case 4:
                            WeaponID = ItemID.LightsBane;
                            break;
                    }
                    break;

                case SpawnBiome.Crimson:
                    switch (Main.rand.Next(6))
                    {
                        case 0:
                            WeaponID = ItemID.TheRottedFork;
                            break;
                        case 1:
                            WeaponID = ItemID.TheUndertaker;
                            break;
                        case 2:
                            WeaponID = ItemID.TheMeatball;
                            break;
                        case 3:
                            WeaponID = ItemID.TendonBow;
                            break;
                        case 4:
                            WeaponID = ItemID.CrimsonRod;
                            break;
                        case 5:
                            WeaponID = ItemID.BloodButcherer;
                            break;
                    }
                    break;

                case SpawnBiome.Dungeon:
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            WeaponID = ItemID.Muramasa;
                            break;
                        case 1:
                            WeaponID = ItemID.Handgun;
                            break;
                        case 2:
                            WeaponID = ItemID.AquaScepter;
                            break;
                        case 3:
                            WeaponID = ItemID.MagicMissile;
                            break;
                        case 4:
                            WeaponID = ItemID.BlueMoon;
                            break;
                    }
                    break;

                case SpawnBiome.Jungle:
                    if (Main.rand.NextDouble() < 0.5f)
                    {
                        if (Main.rand.Next(2) == 0)
                            WeaponID = ItemID.BladeofGrass;
                        else
                            WeaponID = ItemID.ThornChakram;
                    }
                    else
                    {
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                WeaponID = ItemID.BeeKeeper;
                                break;
                            case 1:
                                WeaponID = ItemID.BeesKnees;
                                break;
                            case 2:
                                WeaponID = ItemID.BeeGun;
                                break;
                            case 3:
                                WeaponID = ItemID.HornetStaff;
                                break;
                        }
                    }
                    break;

                case SpawnBiome.Underworld:
                    switch (Main.rand.Next(6))
                    {
                        case 0:
                            WeaponID = ItemID.FieryGreatsword;
                            break;
                        case 1:
                            WeaponID = ItemID.DarkLance;
                            break;
                        case 2:
                            WeaponID = ItemID.Sunfury;
                            break;
                        case 3:
                            WeaponID = ItemID.FlowerofFire;
                            break;
                        case 4:
                            WeaponID = ItemID.Flamelash;
                            break;
                        case 5:
                            WeaponID = ItemID.HellwingBow;
                            break;
                    }
                    break;

                case SpawnBiome.Hallow:
                    WeaponID = ItemID.PearlwoodSword;
                    break;
            }
            if (WeaponID > 0)
            {
                Item i = new Item();
                i.SetDefaults(WeaponID);
                if (i.melee)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            i.prefix = Terraria.ID.PrefixID.Dangerous;
                            break;
                        case 2:
                            i.prefix = Terraria.ID.PrefixID.Savage;
                            break;
                        case 3:
                            i.prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            i.prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            i.prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            i.prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            i.prefix = Terraria.ID.PrefixID.Savage;
                            break;
                        case 8:
                            i.prefix = Terraria.ID.PrefixID.Legendary;
                            break;
                        case 9:
                            i.prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.ranged)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            i.prefix = Terraria.ID.PrefixID.Deadly2;
                            break;
                        case 2:
                            i.prefix = Terraria.ID.PrefixID.Rapid;
                            break;
                        case 3:
                            i.prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            i.prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            i.prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            i.prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            i.prefix = Terraria.ID.PrefixID.Powerful;
                            break;
                        case 8:
                            i.prefix = Terraria.ID.PrefixID.Unreal;
                            break;
                        case 9:
                            i.prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.magic)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            i.prefix = Terraria.ID.PrefixID.Masterful;
                            break;
                        case 2:
                            i.prefix = Terraria.ID.PrefixID.Celestial;
                            break;
                        case 3:
                            i.prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            i.prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            i.prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            i.prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            i.prefix = Terraria.ID.PrefixID.Mystic;
                            break;
                        case 8:
                            i.prefix = Terraria.ID.PrefixID.Mythical;
                            break;
                        case 9:
                            i.prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.summon)
                {
                    switch (Main.rand.Next(9))
                    {
                        case 1:
                            i.prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 2:
                            i.prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 3:
                            i.prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 4:
                            i.prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 5:
                            i.prefix = Terraria.ID.PrefixID.Murderous;
                            break;
                        case 6:
                            i.prefix = Terraria.ID.PrefixID.Hurtful;
                            break;
                        case 7:
                            i.prefix = Terraria.ID.PrefixID.Unpleasant;
                            break;
                        case 8:
                            i.prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                return i;
            }
            return null;
        }

        public static Item GetRandomAccessory()
        {
            List<int> ItemIDs = new List<int>();
            ItemIDs.Add(Terraria.ID.ItemID.Aglet);
            ItemIDs.Add(Terraria.ID.ItemID.HermesBoots);
            ItemIDs.Add(Terraria.ID.ItemID.ClimbingClaws);
            ItemIDs.Add(Terraria.ID.ItemID.ShoeSpikes);
            ItemIDs.Add(Terraria.ID.ItemID.Flipper);
            ItemIDs.Add(Terraria.ID.ItemID.LuckyHorseshoe);
            ItemIDs.Add(Terraria.ID.ItemID.ShinyRedBalloon);
            //
            ItemIDs.Add(Terraria.ID.ItemID.BandofRegeneration);

            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    ItemIDs.Add(Terraria.ID.ItemID.BandofRegeneration);
                    break;
                case SpawnBiome.Crimson:
                    ItemIDs.Add(Terraria.ID.ItemID.PanicNecklace);
                    break;
                case SpawnBiome.Dungeon:
                    ItemIDs.Add(Terraria.ID.ItemID.CobaltShield);
                    break;
                case SpawnBiome.Jungle:
                    ItemIDs.Add(Terraria.ID.ItemID.AnkletoftheWind);
                    ItemIDs.Add(Terraria.ID.ItemID.FeralClaws);
                    ItemIDs.Add(Terraria.ID.ItemID.FlowerBoots);
                    break;
                case SpawnBiome.Underworld:
                    ItemIDs.Add(Terraria.ID.ItemID.LavaCharm);
                    ItemIDs.Add(Terraria.ID.ItemID.ObsidianRose);
                    ItemIDs.Add(Terraria.ID.ItemID.ObsidianSkull);
                    ItemIDs.Add(Terraria.ID.ItemID.MagmaStone);
                    break;
            }
            if (ItemIDs.Count > 0)
            {
                Item i = new Item();
                i.SetDefaults(ItemIDs[Main.rand.Next(ItemIDs.Count)]);
                if (Main.rand.NextDouble() < 0.8f)
                {
                    switch (Main.rand.Next(12))
                    {
                        case 0:
                            i.prefix = Terraria.ID.PrefixID.Armored;
                            break;
                        case 1:
                            i.prefix = Terraria.ID.PrefixID.Warding;
                            break;
                        case 2:
                            i.prefix = Terraria.ID.PrefixID.Precise;
                            break;
                        case 3:
                            i.prefix = Terraria.ID.PrefixID.Lucky;
                            break;
                        case 4:
                            i.prefix = Terraria.ID.PrefixID.Angry;
                            break;
                        case 5:
                            i.prefix = Terraria.ID.PrefixID.Menacing;
                            break;
                        case 6:
                            i.prefix = Terraria.ID.PrefixID.Hasty;
                            break;
                        case 7:
                            i.prefix = Terraria.ID.PrefixID.Quick;
                            break;
                        case 8:
                            i.prefix = Terraria.ID.PrefixID.Intrepid;
                            break;
                        case 9:
                            i.prefix = Terraria.ID.PrefixID.Violent;
                            break;
                        case 10:
                            i.prefix = Terraria.ID.PrefixID.Arcane;
                            break;

                    }
                }
                return i;
            }
            return null;
        }

        public static string GetDifficultyList()
        {
            string DifficultyString = "";
            bool First = true;
            foreach (Modifiers mod in Enum.GetValues(typeof(Modifiers)))
            {
                if (modifier.Contains(mod))
                {
                    if (First)
                        First = false;
                    else
                        DifficultyString += ", ";
                    DifficultyString += mod.ToString();
                }
            }
            return DifficultyString;
        }

        public static void UpdateBountyBoardText()
        {
            if (SignID > -1)
            {
                string Text = NoRequestText;
                if (!SardineTalkedToAboutBountyQuests)
                {
                    if(NpcMod.HasGuardianNPC(2))
                        Text = "Hey, could you come talk to me about Bounty Quests?\n\n  - " + NpcMod.GetGuardianNPCName(2);
                }
                else if (TargetMonsterID > 0)
                {
                    Text = "Hunt " + TargetFullName + ".";
                    Text += "\n  Difficulty: " + GetDifficultyList();
                    Text += "\n  Last seen in the " + spawnBiome.ToString() + ".";
                    Text += "\n  Reward: " + Main.ValueToCoins(CoinReward);
                    Text += "\n  ";
                    bool First = true;
                    foreach (Item i in RewardList)
                    {
                        if (!First)
                            Text += ", ";
                        else
                            First = false;
                        Text += i.HoverName;
                    }
                    if (!First)
                        Text += ".";
                    Text += "\n Time Left: ";
                    int h = 0, m = 0, s = ActionCooldown / 60;
                    if (s >= 60)
                    {
                        m += s / 60;
                        s -= m * 60;
                    }
                    if (m >= 60)
                    {
                        h += m / 60;
                        m -= h * 60;
                    }
                    First = true;
                    if (h > 0)
                    {
                        Text += h + " Hours";
                        First = false;
                    }
                    if (m > 0)
                    {
                        if (!First)
                        {
                            Text += ", ";
                        }
                        else
                        {
                            First = false;
                        }
                        Text += m + " Minutes";
                    }
                    if (h == 0 && m == 0)
                    {
                        Text += "Ending in a few seconds";
                    }
                    /*if (s > 0)
                    {
                        if (!First)
                        {
                            Text += ", ";
                        }
                        else
                        {
                            First = false;
                        }
                        Text += s + " Seconds";
                    }*/
                    Text += ".";
                    if (PlayerAlreadyRedeemedReward(Main.player[Main.myPlayer]))
                        Text += "\n    Clear!!";
                }
                if (SignExists())
                {
                    Sign.TextSign(SignID, Text);
                    if (Main.sign[SignID] == null)
                        SignID = -1;
                }
            }
        }

        public static int ExtraCoinRewardFromProgress()
        {
            int Extra = 0;
            if (NPC.downedBoss1)
                Extra += 125;
            if (NPC.downedBoss2)
                Extra += 250;
            if (NPC.downedBoss3)
                Extra += 500;
            if (NPC.downedSlimeKing)
                Extra += 350;
            if (NPC.downedQueenBee)
                Extra += 400;
            if (Main.hardMode)
                Extra += 1000;
            if (NPC.downedMechBoss1)
                Extra += 1150;
            if (NPC.downedMechBoss2)
                Extra += 1400;
            if (NPC.downedMechBoss3)
                Extra += 1850;
            if (NPC.downedPlantBoss)
                Extra += 2220;
            if (NPC.downedGolemBoss)
                Extra += 2650;
            if (NPC.downedAncientCultist)
                Extra += 3500;
            if (NPC.downedFishron)
                Extra += 4000;
            if (NPC.downedMoonlord)
                Extra += 5000;
            return Extra;
        }

        public static string GetRandomString(string[] List)
        {
            return Utils.SelectRandom(Main.rand, List);
        }

        public static string NameGen(string[] Syllabes)
        {
            string NewName = "";
            double Chance = 2f;
            bool First = true;
            List<int> UsedSyllabes = new List<int>();
            while (Main.rand.NextDouble() < Chance)
            {
                int SelectedSyllabe = Main.rand.Next(Syllabes.Length);
                int SyllabesDisponible = 0;
                for (int s = 0; s < Syllabes.Length; s++)
                {
                    if (!UsedSyllabes.Contains(s))
                    {
                        SyllabesDisponible++;
                    }
                }
                if (SyllabesDisponible == 0)
                    break;
                if (UsedSyllabes.Contains(SelectedSyllabe))
                    continue;
                UsedSyllabes.Add(SelectedSyllabe);
                string Syllabe = Syllabes[SelectedSyllabe].ToLower();
                foreach (char Letter in Syllabe)
                {
                    NewName += Letter;
                    if (First)
                    {
                        NewName = NewName.ToUpper();
                        First = false;
                    }
                }
                if (Chance > 1f)
                    Chance--;
                else if (Chance > 0.5f)
                    Chance -= 0.2f;
                else
                {
                    Chance *= 0.5f;
                }
            }
            return NewName;
        }

        public static void SetDefaultCooldown()
        {
            ActionCooldown = 5 * 3600;
        }

        public enum SpawnBiome : byte
        {
            Corruption, Crimson, Dungeon, Jungle, Underworld, Hallow
        }

        public enum Modifiers : byte
        {
            ExtraHealth,
            ExtraDamage,
            ExtraDefense,
            KBImmunity,
            HealthRegen,
            Boss,
            Noob,
            Reaper,
            Cyclops,
            GoldenShower,
            Haunted,
            Alchemist,
            Sharknado,
            Sapper,
            Cursed
        }
    }
}
