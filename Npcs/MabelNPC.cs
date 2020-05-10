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
    public class MabelNPC : GuardianActorNPC
    {
        public int AiStage { get { return (int)npc.ai[0]; } set { npc.ai[0] = value; } }
        public int AiTimer { get { return (int)npc.ai[1]; } set { npc.ai[1] = value; } }
        private const int DialogueTime = 300;
        public static int Cooldown = 0;

        public MabelNPC()
            : base(8, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.rarity = 5;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            switch (AiStage)
            {
                case 0:
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.JumpFrame;
                    break;
                case 1:
                    float MaxAnimTime = Base.MaxWalkSpeedTime;
                    npc.frameCounter += 0.75f;
                    if (npc.frameCounter < 0)
                        npc.frameCounter += MaxAnimTime;
                    if (npc.frameCounter >= MaxAnimTime)
                        npc.frameCounter -= MaxAnimTime;
                    int Frame = (int)(npc.frameCounter / Base.WalkAnimationFrameTime);
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.JumpFrame;
                    if (Frame >= 0 && Frame < Base.WalkingFrames.Length)
                        BodyAnimationFrame = Base.WalkingFrames[Frame];
                    break;
                case 2:
                    break;
            }
        }

        public static bool CanSpawnMabel { get { return !NPC.AnyNPCs(ModContent.NPCType<MabelNPC>()) && (NPC.AnyNPCs(Terraria.ID.NPCID.PartyGirl) || Terraria.GameContent.Events.BirthdayParty.PartyIsUp); } }

        public static void TrySpawningMabel()
        {
            if (Cooldown > 0)
            {
                Cooldown--;
                return;
            }
            Cooldown = Main.rand.Next(180, 601) * 30;
            if (NpcMod.HasMetGuardian(8) || !CanSpawnMabel)
            {
                return;
            }
            if (Main.rand.Next(5) == 0)
            {
                int TileX = (int)(Main.player[Main.myPlayer].Center.X / 16) + Main.rand.Next(-10, 11), TileY = (int)(Main.player[Main.myPlayer].Center.Y / 16);
                if (Main.tile[TileX, TileY].active() && Main.tileSolid[Main.tile[TileX, TileY].type])
                    return;
                for (int y = 1; y < 65; y++)
                {
                    int ty = TileY - y;
                    if (Main.tile[TileX, ty].active() && Main.tileSolid[Main.tile[TileX, ty].type])
                        return;
                }
                NPC.NewNPC(TileX * 16, TileY * 16, ModContent.NPCType<MabelNPC>());
            }
        }

        public override void AI()
        {
            if (AiStage == 0)
            {
                if (AiTimer == 0)
                {
                    npc.velocity.Y = 0.1f;
                    if (Main.rand.NextDouble() < 0.5)
                        npc.direction = 1;
                    else
                        npc.direction = -1;
                    AiTimer = 1;
                    npc.position.Y -= 1000;
                    if (npc.position.Y < 0)
                    {
                        npc.active = false;
                        return;
                    }
                }
                if (npc.collideY || npc.velocity.Y == 0)
                {
                    AiStage = 1;
                    npc.position.Y += 48;
                    Main.PlaySound(Terraria.ID.SoundID.Item11, npc.Center);
                }
                npc.behindTiles = true;
            }
            else if (AiStage == 1)
            {
                AiTimer += 1;
                if (AiTimer % 300 == 0)
                    SayMessage("*Someone, help me!*");
                npc.noGravity = true;
                npc.velocity.Y = 0;
                npc.velocity.X = 0;
                npc.behindTiles = true;
            }
            else
            {
                npc.npcSlots = 200;
                npc.noGravity = false;
                npc.behindTiles = false;
                float Distance = Main.player[Main.myPlayer].Center.X - npc.Center.X;
                if (Distance > 0)
                    npc.direction = 1;
                else
                    npc.direction = -1;
                MoveRight = MoveLeft = false;
                if (Math.Abs(Distance) > 96f)
                {
                    if (Distance > 0)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
                bool PlayerHasMabel = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], 8);
                if (AiTimer == 30)
                {
                    SayMessage("*Thanks for helping me.*");
                }
                if (PlayerHasMabel)
                {
                    if (AiTimer == 30 + DialogueTime)
                    {
                        SayMessage("*Oh, hello. It's you again.*");
                    }
                    if (AiTimer == 30 + DialogueTime * 2)
                    {
                        SayMessage("*I'm sorry, but I'm still trying to fly...*");
                    }
                    if (AiTimer == 30 + DialogueTime * 3)
                    {
                        SayMessage("*I just can't give up being a model.*");
                    }
                    if (AiTimer == 30 + DialogueTime * 4)
                    {
                        SayMessage("*Since I'm here, I guess I'll hang around.*");
                    }
                    if (AiTimer == 30 + DialogueTime * 5)
                    {
                        NpcMod.AddGuardianMet(8);
                        npc.Transform(ModContent.NPCType<GuardianNPC.List.DeerGuardian>());
                    }
                }
                else
                {
                    if (AiTimer == 30 + DialogueTime)
                    {
                        SayMessage("*I tried to see If I could fly like a Reindeer. It didn't worked well...*");
                    }
                    if (AiTimer == 30 + DialogueTime * 2)
                    {
                        SayMessage("*My name is " + Base.Name + ", by the way.*");
                    }
                    if (AiTimer == 30 + DialogueTime * 3)
                    {
                        SayMessage("*I have to practice for Miss North Pole, It's going to happen soo, close to when the Xmas you people celebrates happen.*");
                    }
                    if (AiTimer == 30 + DialogueTime * 4)
                    {
                        if (DateTime.Now.Month != 12 || DateTime.Now.Day >= 25)
                            SayMessage("*What?! It has already passed? Nooooooooo!!! Well... I guess I can practice for next year?*");
                        else
                        {
                            SayMessage("*It's close to that day?! Oh my... I have so much to practice.*");
                        }
                    }
                    if (AiTimer == 30 + DialogueTime * 5)
                    {
                        SayMessage("*Do you mind If I stay on your world for a while, while I practice?*");
                    }
                    if (AiTimer == 30 + DialogueTime * 6)
                    {
                        SayMessage("*I wonder if the people here are nice like you, too.*");
                    }
                    if (AiTimer == 30 + DialogueTime * 7)
                    {
                        PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], 8);
                        NpcMod.AddGuardianMet(8);
                        npc.Transform(ModContent.NPCType<GuardianNPC.List.DeerGuardian>());
                    }
                }
                AiTimer++;
            }
            if (AiStage < 2 && AiTimer > 3000)
            {
                Vector2 PC = Main.player[Main.myPlayer].Center, MC = npc.Center;
                if (Math.Abs(PC.X - MC.X) > NPC.sWidth || Math.Abs(PC.Y - MC.Y) > NPC.sHeight)
                {
                    npc.active = false;
                    return;
                }
            }
            base.AI();
        }

        public override string GetChat()
        {
            return "*Help! I'm stuck here!*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Help";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                npc.position.Y -= 48;
                AiStage = 2;
                AiTimer = 0;
                SayMessage("*Pull!*");
                npc.velocity.Y -= 6.25f;
                Main.player[Main.myPlayer].talkNPC = -1;
            }
        }

        public override bool CanChat()
        {
            return AiStage == 1;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            base.PreDraw(spriteBatch, drawColor);
            FlipVertically = AiStage < 2;
            if (AiStage == 0)
            {
                Vector2 TextPosition = npc.Bottom;
                TextPosition.Y += 8f;
                float Scale = 1f;
                if (TextPosition.Y < Main.screenPosition.Y + 56)
                {
                    Scale = 1f - (Main.screenPosition.Y + 56 - TextPosition.Y) / 1000f;
                    TextPosition.Y = Main.screenPosition.Y + 56;
                }
                if (Scale < 0.1f)
                    Scale = 0.1f;
                if (TextPosition.X - 48f < Main.screenPosition.X)
                    TextPosition.X = Main.screenPosition.X + 48;
                if (TextPosition.X + 48f > Main.screenPosition.X + Main.screenWidth)
                    TextPosition.X = Main.screenPosition.X + Main.screenWidth - 48;
                if (Scale > 1f)
                    Scale = 1f;
                Utils.DrawBorderString(spriteBatch, "*AAAAAAHHHH!!!*", TextPosition - Main.screenPosition, Color.White, Scale, 0.5f, 0.5f);
            }
            return false;
        }
    }
}
