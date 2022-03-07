using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class BrutusNPC : GuardianActorNPC
    {
        private int SteelTestingTime = 0;
        private const int MaxSteelTestingTime = 5;
        private int HirePrice
        {
            get
            {
                int Price = 5;
                if (Main.hardMode)
                    Price = 20;
                if (NPC.downedPlantBoss)
                    Price = 80;
                if (NPC.downedGolemBoss)
                    Price = 320;
                if (NPC.downedMoonlord)
                    Price = 840;
                return Price;
            }
        }
        public byte Scene = 0;
        public int SceneTime = 0;
        public const byte SCENE_PLAYERWINS = 1,
            SCENE_PLAYERCHEATS = 2;
        private int LeftArmFrame = -1, RightArmFrame = -1, BodyFrame = -1;
        public const int ProgressCountForBrutusToAppear = 3;

        public BrutusNPC() : base(6, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = true;
            npc.friendly = true;
            npc.knockBackResist = 0;
            npc.dontCountMe = true;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lion TerraGuardian");
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if (LeftArmFrame > -1)
                base.LeftArmAnimationFrame = LeftArmFrame;
            if (RightArmFrame > -1)
                base.RightArmAnimationFrame = RightArmFrame;
            if (BodyFrame > -1)
                base.BodyAnimationFrame = BodyFrame;
        }

        public static int ChanceCounter()
        {
            int Chance = 0;
            if (NPC.downedBoss1)
                Chance++;
            if (NPC.downedBoss2)
                Chance++;
            if (NPC.downedBoss3)
                Chance++;
            if (NPC.downedQueenBee)
                Chance++;
            if (NPC.downedSlimeKing)
                Chance++;
            if (Main.hardMode)
                Chance++;
            if (NPC.downedMechBoss1)
                Chance++;
            if (NPC.downedMechBoss2)
                Chance++;
            if (NPC.downedMechBoss3)
                Chance++;
            if (NPC.downedPlantBoss)
                Chance++;
            if (NPC.downedGolemBoss)
                Chance++;
            if (NPC.downedFishron)
                Chance++;
            if (NPC.downedMoonlord)
                Chance++;
            if (NPC.downedGoblins)
                Chance++;
            if (NPC.downedPirates)
                Chance++;
            if (NPC.downedMartians)
                Chance++;
            if (NPC.downedFrost)
                Chance++;
            return Chance;
        }

        public static void TrySpawningBrutus()
        {
            if (Main.netMode == 1 || NpcMod.HasMetGuardian(6, "") || WorldMod.IsGuardianNpcInWorld(GuardianBase.Brutus))
                return;
            const int SpawnTime = 3 * 3600;
            if (Main.fastForwardTime || Main.eclipse || !Main.dayTime || (Main.time < SpawnTime || WorldMod.LastTime >= 7.5))
            {
                return;
            }
            if (Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return;
            int NpcCount = (int)(NpcMod.GetCompanionNPCCount() * 0.5f);
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].townNPC)
                    NpcCount++;
            }
            if (NpcCount < 5)
                return;
            int SpawnChance = 20 - ChanceCounter() / 2;
            if (SpawnChance > 0 && Main.rand.Next(SpawnChance) > (float)(NpcCount - 5) / 2)
            {
                return;
            }
            List<int> NpcsToSpawnOn = new List<int>();
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<BrutusNPC>())
                {
                    return;
                }
                if (Main.npc[i].active && Main.npc[i].townNPC && !Main.npc[i].homeless && Main.npc[i].type != 37)
                {
                    byte PickedPlayer = Player.FindClosest(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height);
                    if (Math.Abs(Main.player[PickedPlayer].Center.X - Main.npc[i].Center.X) >= NPC.sWidth * 0.5f ||
                        Math.Abs(Main.player[PickedPlayer].Center.Y - Main.npc[i].Center.Y) >= NPC.sHeight * 0.5f)
                    {
                        NpcsToSpawnOn.Add(i);
                    }
                }
            }
            if (NpcsToSpawnOn.Count == 0)
                return;
            int PickedNPC = NpcsToSpawnOn[Main.rand.Next(NpcsToSpawnOn.Count)];
            int SpawnPosX = (int)Main.npc[PickedNPC].Center.X,
                SpawnPosY = (int)(Main.npc[PickedNPC].position.Y + Main.npc[PickedNPC].height);
            int npcPos = NPC.NewNPC(SpawnPosX, SpawnPosY, ModContent.NPCType<BrutusNPC>());
            string Text = "A Lion TerraGuardian has came from the Ether Realm looking for someone to hire him as bodyguard.";
            if (Main.netMode == 0)
                Main.NewText(Text, MainMod.MysteryCloseColor);
            else
                NetMessage.SendData(25, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Text), MainMod.MysteryCloseColor.R, MainMod.MysteryCloseColor.G, MainMod.MysteryCloseColor.B, 255);
            WarnedAboutBrutus = true;
        }

        public static bool WarnedAboutBrutus = false;

        public override void AI()
        {
            LeftArmFrame = -1;
            RightArmFrame = -1;
            BodyFrame = -1;
            //AI Work here
            npc.homeTileX = npc.homeTileY = -1;
            if (Main.player[Main.myPlayer].talkNPC == npc.whoAmI)
                npc.direction = Main.player[Main.myPlayer].Center.X < npc.Center.X ? -1 : 1;
            if (!WarnedAboutBrutus)
            {
                Main.NewText("A Lion TerraGuardian is still visiting your world.", MainMod.MysteryCloseColor);
                WarnedAboutBrutus = true;
            }
            if (SteelTestingTime > 0)
            {
                bool CaughtADebuff = false;
                for (int i = 0; i < npc.buffType.Length; i++)
                {
                    if (npc.buffType[i] > 0 && Main.debuff[npc.buffType[i]])
                    {
                        CaughtADebuff = true;
                        break;
                    }
                }
                if (CaughtADebuff)
                {
                    EndDamageTest();
                    PlayScene(SCENE_PLAYERCHEATS);
                }
            }
            if (SteelTestingTime > 0)
            {
                SteelTestingTime--;
                npc.TargetClosest(true);
                if (SteelTestingTime <= 0)
                {
                    SayMessage("*Growl! Time's up.*");
                    EndDamageTest();
                    PlayScene(0);
                }
            }
            npc.npcSlots = 1;
            switch (Scene)
            {
                default:
                    {
                        SceneTime++;
                        Idle = true;
                        if (false && SteelTestingTime <= 0 && SceneTime >= 600 && npc.velocity.X == 0 && npc.velocity.Y == 0)
                        {
                            BodyFrame = LeftArmFrame = RightArmFrame = Base.DuckingFrame;
                        }
                    }
                    break;
                case SCENE_PLAYERWINS:
                    npc.npcSlots = 200;
                    if (SceneTime == 0)
                    {
                        npc.FaceTarget();
                        npc.velocity.X -= npc.direction * 7.5f;
                        npc.velocity.Y -= 7.5f;
                        SayMessage("*Rooow!! Ugh... Urgh...*");
                        SceneTime = 1;
                    }
                    else if (SceneTime >= 1)
                    {
                        if (npc.velocity.X == 0 && npc.velocity.Y == 0)
                        {
                            int LastSceneTime = SceneTime;
                            SceneTime++;
                            int Frame = Base.DuckingFrame;
                            if (SceneTime == 180)
                            {
                                SayMessage("*I guess I got a bit rusty.*");
                            }
                            if (SceneTime == 480)
                            {
                                SayMessage("*Maybe helping you on your quest will help making me tougher again.*");
                            }
                            if (SceneTime >= 480)
                            {
                                Frame = Base.StandingFrame;
                            }
                            if (SceneTime == 780)
                            {
                                SayMessage("*I am " + Base.Name + ". Your body guard, from now on.*");
                            }
                            if (SceneTime >= 1080)
                            {
                                NpcMod.AddGuardianMet(6);
                                Player player = Main.player[npc.target];
                                PlayerMod.AddPlayerGuardian(player, GuardianBase.Brutus);
                                PlayerMod.GetPlayerGuardian(player, GuardianBase.Brutus).IncreaseFriendshipProgress(1);
                                GuardianData gd = PlayerMod.GetPlayerGuardian(player, GuardianBase.Brutus);
                                if (gd.FriendshipLevel == 0)
                                    gd.IncreaseFriendshipProgress(1);
                                WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Brutus);
                                //npc.Transform(ModContent.NPCType<GuardianNPC.List.LionGuardian>());
                            }
                            BodyFrame = LeftArmFrame = RightArmFrame = Frame;
                        }
                    }
                    break;

                case SCENE_PLAYERCHEATS:
                    {
                        npc.npcSlots = 200;
                        if (SceneTime == 0)
                        {
                            SteelTestingTime = 0;
                            SayMessage("*ROAR*");
                            npc.velocity.Y = -5.5f;
                            npc.FaceTarget();
                            SceneTime = 1;
                        }
                        else
                        {
                            ModTargetting target = new ModTargetting();
                            target.TargettingPlayer = true;
                            if (Main.player[npc.target].GetModPlayer<PlayerMod>().Guardian.Active && Main.player[npc.target].GetModPlayer<PlayerMod>().Guardian.PlayerControl)
                                target.TargettingPlayer = false;
                            target.SetTargetToPlayer(Main.player[npc.target]);
                            if (npc.velocity.Y == 0 && npc.velocity.X == 0)
                            {
                                //Player player = Main.player[npc.target];
                                SceneTime++;
                                bool PlayerPickedUp = false;
                                int RightHandFrame = 0;
                                if (SceneTime >= 30)
                                {
                                    PlayerPickedUp = true;
                                }

                                if (SceneTime == 30)
                                {
                                    SayMessage("*I said no tricks, idiot.*");
                                }
                                int t = 0;
                                const int FrameTime = 8;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightHandFrame = Base.ItemUseFrames[0];
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightHandFrame = Base.ItemUseFrames[1];
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightHandFrame = Base.ItemUseFrames[2];
                                    if (SceneTime == 60 + t * FrameTime + FrameTime / 2)
                                    {
                                        int Damage = (int)((70f / target.MaxHealthBonus) * target.MaxLife);
                                        target.Hurt(Damage, npc.direction, " has got what deserved.");
                                    }
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightHandFrame = Base.ItemUseFrames[1];
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime && SceneTime < 60 + (t + 1) * FrameTime)
                                {
                                    RightHandFrame = Base.ItemUseFrames[0];
                                }
                                t++;
                                if (SceneTime >= 60 + t * FrameTime)
                                {
                                    PlayScene(0);
                                }

                                if (PlayerPickedUp)
                                {
                                    int hx, hy;
                                    Base.LeftHandPoints.GetPositionFromFrame(Base.ItemUseFrames[2], out hx, out hy);
                                    LeftArmFrame = Base.ItemUseFrames[2];
                                    Vector2 Pos = new Vector2(hx, Base.SpriteHeight - hy);
                                    if (npc.direction < 0)
                                        Pos.X = Base.SpriteWidth - Pos.X;
                                    Pos.X += npc.width * 0.5f - Base.SpriteWidth * 0.5f;
                                    Pos.Y += npc.height - Base.SpriteHeight;
                                    Pos += npc.position;
                                    target.Center = Pos;
                                    target.FallStart = (int)(target.Position.Y / 16);
                                }
                                RightArmFrame = RightHandFrame;
                            }
                        }
                    }
                    break;
            }
            base.AI(); //Never.Remove
            if (!Main.dayTime)
            {
                bool PlayerInRange = false;
                for (int p = 0; p < 255; p++)
                {
                    if (!Main.player[p].active)
                        continue;
                    Vector2 PositionDifference = npc.Center - Main.player[p].Center;
                    if (Math.Abs(PositionDifference.X) < NPC.sWidth * 0.5f + NPC.safeRangeX &&
                        Math.Abs(PositionDifference.Y) < NPC.sHeight * 0.5f + NPC.safeRangeY)
                    {
                        PlayerInRange = true;
                        break;
                    }
                }
                if (!PlayerInRange)
                {
                    Main.NewText("The Lion Guardian has returned to the Ether Realm.", Color.OrangeRed);
                    npc.active = false;
                    npc.life = 0;
                }
            }
        }

        public void PlayScene(byte Scene)
        {
            this.Scene = Scene;
            SceneTime = 0;
        }

        public void StartDamageTest()
        {
            SteelTestingTime = MaxSteelTestingTime * 60;
            npc.life = npc.lifeMax;
            for (int b = 0; b < npc.buffTime.Length; b++)
            {
                if (npc.buffType[b] > 0)
                    npc.DelBuff(b);
            }
            npc.friendly = false;
            npc.dontTakeDamage = false;
            SayMessage("*Show me what you are made of.*");
            //Main.CloseNPCChatOrSign();
            Idle = false;
            IdleBehaviorTime = 0;
            IdleBehaviorType = 0;
        }

        public void EndDamageTest()
        {
            SteelTestingTime = 0;
            npc.life = npc.lifeMax;
            for (int b = 0; b < npc.buffTime.Length; b++)
            {
                if (npc.buffType[b] > 0)
                    npc.DelBuff(b);
            }
            npc.friendly = true;
            npc.dontTakeDamage = true;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            defense = 5;
            if (Main.hardMode)
                defense = 10;
            if (NPC.downedMechBossAny)
                defense = 15;
            if (NPC.downedPlantBoss)
                defense = 20;
            if (NPC.downedMoonlord)
                defense = 30;
            damage = (int)((damage - defense) * 0.4);
            crit = false;
            npc.FaceTarget();
            return SteelTestingTime > 0;
        }

        public override bool CheckDead()
        {
            if (npc.life <= 0)
            {
                if (SteelTestingTime > 0)
                {
                    EndDamageTest();
                    npc.life = npc.lifeMax;
                    PlayScene(SCENE_PLAYERWINS);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public override string GetChat()
        {
            string Text = "*I want to test your steel. If you be able to do so in 5 seconds, he'll join you.\nUse poison or any other cheap method and you'll regret it.\nOr you may just pay me to be your body guard. You choose.*";
            if (HasBrutusRecruited)
            {
                Text = "*We still have a contract up. You don't need to pay me again for my job.*";
            }
            return Text;
        }

        private bool HasBrutusRecruited { get { return PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], 6); } }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if(SteelTestingTime > 0)
            {

            }
            else if (HasBrutusRecruited)
            {
                button = "That's great.";
            }
            else
            {
                button = "Let's do this!";
                button2 = "Hire instead (" + HirePrice + " Gold Coins)";
            }
        }

        public override bool CanChat()
        {
            return SteelTestingTime <= 0 && (Scene == 0);
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                if (HasBrutusRecruited)
                {
                    NpcMod.AddGuardianMet(6);
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Brutus);
                }
                else
                {
                    npc.target = Main.myPlayer;
                    StartDamageTest();
                    //Main.player[Main.myPlayer].talkNPC = -1;
                }
            }
            else
            {
                if (Main.player[Main.myPlayer].BuyItem(Item.buyPrice(0, HirePrice)))
                {
                    Main.npcChatText = "*I accept the offer. I, "+Base.Name+", will protect you until the end of my contract.*";
                    PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
                    PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID).IncreaseFriendshipProgress(1);
                    NpcMod.AddGuardianMet(6);
                    Main.NewText("You bought " + Base.Name + "'s help.", MainMod.RecruitColor);
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Brutus);
                }
                else
                {
                    Main.npcChatText = "*He's saying that the only ways of hiring his blade is by either showing how strong you are, or how deep your pocket is.*";
                }
            }
        }
    }
}
