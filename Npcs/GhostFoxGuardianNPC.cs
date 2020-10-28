using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class GhostFoxGuardianNPC : GuardianActorNPC
    {
        public static bool GhostFoxHauntLifted = false;
        public byte PlayerChaseTime = 0;
        private bool PostBossKillDialogue = false;
        private bool RevivingSomeone = false;
        private bool PassiveAI = false;
        private float Opacity = 1f;

        public GhostFoxGuardianNPC()
            : base(16, "")
        {

        }

        public void SetPassive()
        {
            PassiveAI = true;
        }

        public void SetPostBossKill(float ImpactDirection)
        {
            GhostFoxHauntLifted = true;
            PostBossKillDialogue = true;
            npc.velocity.Y = -8f;
            npc.velocity.X = 6f * ImpactDirection;
        }

        public static void OnMobKill(int NpcID)
        {
            bool LiftedHaunt = false;
            int LiftedHauntPlayerPosition = -1;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active && !Main.player[p].dead)
                {
                    int BuffID = ModContent.BuffType<Buffs.GhostFoxHaunts.SkullHaunt>();
                    if (NpcID == Terraria.ID.NPCID.SkeletronHead && Main.player[p].HasBuff(BuffID))
                    {
                        LiftedHaunt = true;
                        LiftedHauntPlayerPosition = p;
                        Main.player[p].DelBuff(Main.player[p].FindBuffIndex(BuffID));
                        continue;
                    }
                    BuffID = ModContent.BuffType<Buffs.GhostFoxHaunts.BeeHaunt>();
                    if (NpcID == Terraria.ID.NPCID.QueenBee && Main.player[p].HasBuff(BuffID))
                    {
                        LiftedHaunt = true;
                        LiftedHauntPlayerPosition = p;
                        Main.player[p].DelBuff(Main.player[p].FindBuffIndex(BuffID));
                        continue;
                    }
                    BuffID = ModContent.BuffType<Buffs.GhostFoxHaunts.MeatHaunt>();
                    if ((NpcID == Terraria.ID.NPCID.WallofFlesh || NpcID == Terraria.ID.NPCID.WallofFleshEye) && Main.player[p].HasBuff(BuffID))
                    {
                        LiftedHaunt = true;
                        LiftedHauntPlayerPosition = p;
                        Main.player[p].DelBuff(Main.player[p].FindBuffIndex(BuffID));
                        continue;
                    }
                    BuffID = ModContent.BuffType<Buffs.GhostFoxHaunts.SawHaunt>();
                    if ((NpcID == Terraria.ID.NPCID.TheDestroyer || NpcID == Terraria.ID.NPCID.TheDestroyerBody || NpcID == Terraria.ID.NPCID.TheDestroyerTail) && Main.player[p].HasBuff(BuffID))
                    {
                        LiftedHaunt = true;
                        LiftedHauntPlayerPosition = p;
                        Main.player[p].DelBuff(Main.player[p].FindBuffIndex(BuffID));
                        continue;
                    }
                    BuffID = ModContent.BuffType<Buffs.GhostFoxHaunts.ConstructHaunt>();
                    if (NpcID == Terraria.ID.NPCID.Golem && Main.player[p].HasBuff(BuffID))
                    {
                        LiftedHaunt = true;
                        LiftedHauntPlayerPosition = p;
                        Main.player[p].DelBuff(Main.player[p].FindBuffIndex(BuffID));
                        continue;
                    }
                }
            }
            if (LiftedHaunt)
            {
                Player player = Main.player[LiftedHauntPlayerPosition];
                int NpcPos = NPC.NewNPC((int)player.position.X, (int)player.position.Y, ModContent.NPCType<Npcs.GhostFoxGuardianNPC>());
                Main.npc[NpcPos].target = LiftedHauntPlayerPosition;
                GhostFoxGuardianNPC gnpc = (GhostFoxGuardianNPC)Main.npc[NpcPos].modNPC;
                gnpc.SetPostBossKill(-player.direction);
                Main.NewText("Ghost Fox Guardian haunt has been lifted!", Microsoft.Xna.Framework.Color.Orange);
            }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.friendly = true;
            npc.knockBackResist = 0;
            npc.dontCountMe = npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            SetAnimationFrame(ref BodyAnimationFrame);
            SetAnimationFrame(ref LeftArmAnimationFrame);
            SetAnimationFrame(ref RightArmAnimationFrame);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghost Fox Guardian");
        }

        public void SetAnimationFrame(ref int Frame)
        {
            if (Frame == 0)
            {
                if (YOffset >= 2)
                {
                    Frame = 2;
                }
                else if (YOffset <= -2)
                {
                    Frame = 1;
                }
            }
            if (npc.velocity.Y != 0 && Frame == Base.JumpFrame)
            {
                if (Math.Abs(npc.velocity.Y) < 1)
                    Frame = 1;
                else if (npc.velocity.Y >= 1)
                {
                    Frame = 13;
                }
            }
            else if ((npc.velocity.X > 0 && npc.direction < 0) || (npc.velocity.X < 0 && npc.direction > 0))
            {
                if (Frame >= 3 && Frame <= 5)
                {
                    Frame += 9;
                }
            }
            if (RevivingSomeone)
                Frame = Base.ReviveFrame;
        }

        public override bool CanChat()
        {
            return PostBossKillDialogue || PassiveAI;
        }

        public override string GetChat()
        {
            return PassiveAI ? "(She greets you, but seems unable to speak. She seems to be asking of could join your adventures.)" : "(She seems to be happy with you, but can't speak a single word. She also a bit sad for what she made you pass through, tries to find ways of apologizing, but can't find out how. She seems to be wanting to go with you on your adventures.)";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Accept";
            button2 = "Reject";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                NpcMod.AddGuardianMet(16);
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().AddNewGuardian(16);
                if (PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 16).FriendshipLevel == 0)
                    PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 16).IncreaseFriendshipProgress(1);
                Main.npcChatText = "(She looks happy at you, sketches on the floor her name, " + PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 16).Name + ", then places her hand on your shoulder, telling you that you can count on her.)";
                WorldMod.TurnNpcIntoGuardianTownNpc(npc, 16);
            }
            else
            {
                NpcMod.AddGuardianMet(16, "", false);
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().AddNewGuardian(16);
                if (PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 16).FriendshipLevel == 0)
                    PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 16).IncreaseFriendshipProgress(1);
                Main.npcChatText = "(Her face suddenly changed to a sad look, she sketches her name on the floor, " + PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 16).Name + ", then signals saying that If you need her help, you just need to call.)";
                WorldMod.TurnNpcIntoGuardianTownNpc(npc, 16);
            }
        }

        public override void AI()
        {
            if (GhostFoxHauntLifted)
                SetPassive();
            RevivingSomeone = false;
            YOffset = ((float)Math.Sin(Main.GlobalTime * 2)) * 3;
            bool ReduceOpacity = Main.dayTime && !Main.eclipse && npc.position.Y < Main.worldSurface * 16 && Main.tile[(int)(npc.Center.X) / 16, (int)(npc.Center.Y / 16)].wall == 0;
            if (ReduceOpacity)
            {
                float MinOpacity = !PostBossKillDialogue ? 0 : 0.2f;
                if (Opacity > MinOpacity)
                {
                    Opacity -= 0.005f;
                    if (Opacity <= 0)
                    {
                        npc.active = false;
                        return;
                    }
                    if (Opacity > 0 && Opacity < MinOpacity)
                        Opacity = MinOpacity;
                }
            }
            else
            {
                if (Opacity < 1)
                {
                    Opacity += 0.005f;
                    if (Opacity > 1)
                    {
                        Opacity = 1;
                        return;
                    }
                }
            }
            if (PassiveAI)
            {
                Idle = true;
            }
            else if (PostBossKillDialogue)
            {
                if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 48)
                {
                    if (npc.Center.X < Main.player[npc.target].Center.X)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
                else
                {
                    if (Main.player[npc.target].GetModPlayer<PlayerMod>().KnockedOut)
                    {
                        Main.player[npc.target].GetModPlayer<PlayerMod>().ReviveBoost += 2;
                        if (npc.Center.X < Main.player[npc.target].Center.X)
                            npc.direction = 1;
                        else
                            npc.direction = -1;
                    }
                }
            }
            else if (PlayerChaseTime == 0)
            {
                Idle = true;
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                if (IsInPerceptionRange(player) && Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                {
                    PlayerChaseTime = 200;
                }
            }
            else
            {
                Player player = Main.player[npc.target];
                if (Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                {
                    PlayerChaseTime = 200;
                }
                else
                {
                    PlayerChaseTime--;
                }
                if (PlayerChaseTime > 0)
                {
                    if (player.Center.X < npc.Center.X)
                    {
                        MoveLeft = true;
                    }
                    else
                    {
                        MoveRight = true;
                    }
                    if (player.position.Y < npc.position.Y)
                    {
                        if (JumpTime > 0 || (npc.velocity.Y == 0 && JumpTime == 0))
                        {
                            Jump = true;
                        }
                    }
                    else if (player.position.Y + player.height > npc.position.Y + npc.height && npc.velocity.Y == 0)
                    {
                        npc.position.Y += 8;
                    }
                    Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle((int)npc.Center.X, (int)npc.position.Y + npc.height, Base.Width, Base.Height);
                    rect.X -= (int)(rect.Width * 0.5f);
                    rect.Y -= rect.Height;
                    if (player.getRect().Intersects(rect))
                    {
                        if (PlayerMod.PlayerHasGuardian(player, GuardianBase.Fluffles))
                        {
                            npc.active = false;
                            Main.NewText(PlayerMod.GetPlayerGuardian(player, GuardianBase.Fluffles).Name + " fades away.");
                            return;
                        }
                        else
                        {
                            List<int> CurseIDs = new List<int>();
                            CurseIDs.Add(ModContent.BuffType<Buffs.GhostFoxHaunts.SkullHaunt>());
                            if (NPC.downedBoss2)
                                CurseIDs.Add(ModContent.BuffType<Buffs.GhostFoxHaunts.BeeHaunt>());
                            if (Main.hardMode)
                            {
                                CurseIDs.Add(ModContent.BuffType<Buffs.GhostFoxHaunts.MeatHaunt>());
                                CurseIDs.Add(ModContent.BuffType<Buffs.GhostFoxHaunts.SawHaunt>());
                            }
                            if (NPC.downedPlantBoss)
                                CurseIDs.Add(ModContent.BuffType<Buffs.GhostFoxHaunts.ConstructHaunt>());
                            player.AddBuff(CurseIDs[Main.rand.Next(CurseIDs.Count)], 5);
                            WorldMod.SkipTime(Main.rand.Next(40, 85) * 0.1f);
                            //player.GetModPlayer<PlayerMod>().EnterDownedState(true);
                            if (player.statLife == 1)
                                player.statLife++;
                            player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" didn't survived the haunt..."), 1, 0, false, true, false);
                            /*for (int n = 0; n < 200; n++)
                            {
                                if (Main.npc[n].active && !Main.npc[n].friendly)
                                {
                                    Main.npc[n].active = false;
                                }
                            }*/
                            WorldGen.SaveAndQuit();
                        }
                    }
                }
            }
            base.AI();
        }
        
        public static bool CanGhostFoxSpawn(Player player)
        {
            return !player.GetModPlayer<PlayerMod>().HasGhostFoxHauntDebuff && ((PlayerMod.GetPlayerDefenseCount(player) >= 10 && (Main.halloween || NPC.downedBoss2)) || NPC.downedHalloweenTree);
        }

        public float GetSpawnRate
        {
            get
            {
                if (Main.halloween || NPC.downedHalloweenTree)
                    return 0.02f;
                return 0.0025f;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (((spawnInfo.player.position.Y < Main.worldSurface * 16 && !Main.dayTime && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon && Main.invasionSize == 0) ||
                (spawnInfo.player.position.Y >= Main.worldSurface * 16)) && !spawnInfo.playerSafe && !spawnInfo.playerInTown && CanGhostFoxSpawn(spawnInfo.player) && 
                !NpcMod.HasMetGuardian(16) && !NpcMod.HasGuardianNPC(16) && !NPC.AnyNPCs(npc.type) && Main.rand.NextDouble() < GetSpawnRate)
            {
                return 1f;
            }
            return 0f;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color drawColor)
        {
            drawColor = Creatures.FlufflesBase.GhostfyColor(drawColor, 0.5f * Opacity, Creatures.FlufflesBase.GetColorMod);
            return base.PreDraw(spriteBatch, drawColor);
        }
    }
}
