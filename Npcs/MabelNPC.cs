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
        private bool Rejected = false;
        public byte DialogueStep = 0;

        public MabelNPC()
            : base(8, "")
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deer Guardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
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

        public static bool CanSpawnMabel
        {
            get
            {
                return !WorldMod.IsGuardianNpcInWorld(GuardianBase.Mabel) && (NPC.AnyNPCs(Terraria.ID.NPCID.PartyGirl) || Terraria.GameContent.Events.BirthdayParty.PartyIsUp);
            }
        }

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
                npc.gfxOffY = 0;
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
                npc.gfxOffY = 0;
                AiTimer += 1;
                int CenterY = (int)npc.Center.Y / 16, CenterX = (int)npc.Center.X / 16 - 1;
                bool FaceOnTheFloor = false;
                for(int x = 0; x < 2; x++)
                {
                    if(Main.tile[CenterX + x, CenterY].active() && Main.tileSolid[Main.tile[CenterX + x, CenterY].type])
                    {
                        FaceOnTheFloor = true;
                        break;
                    }
                }
                if (!FaceOnTheFloor)
                    npc.position.Y += 16;
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
                if (Main.player[Main.myPlayer].talkNPC == npc.whoAmI && Math.Abs(Distance) > 96f)
                {
                    if (Distance > 0)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
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

        private bool IsntXmasClose { get { return DateTime.Now.Month != 12 || DateTime.Now.Day >= 25; } }

        public string GetDialogueMessage(int DialogueID)
        {
            string b1, b2;
            return GetDialogueMessage(DialogueID, out b1, out b2);
        }

        public string GetDialogueMessage(int DialogueID, out string Button1, out string Button2)
        {
            Button1 = Button2 = "";
            string Mes = "";
            switch (DialogueID)
            {
                case 0:
                    Mes = "*Thanks for helping me.*";
                    Button1 = "Did you just fell from the sky?";
                    break;
                case 1:
                    Mes = "*Yes, I tried to see If I could fly like a Reindeer. It didn't worked well...*";
                    Button1 = "You doesn't look like a reindeer.";
                    break;
                case 2:
                    Mes = "*And I'm not. My name is " + Base.Name + ", by the way.*";
                    Button1 = "I'm " + Main.player[Main.myPlayer].name;
                    break;
                case 3:
                    Mes = "*Hello. I have to practice for Miss North Pole, It's going to happen soon, close to when the Xmas you people celebrates happen.*";
                    if (IsntXmasClose)
                    {
                        Button1 = "But the the holiday has passed";
                    }
                    else
                    {
                        Button1 = "The holiday will happen in some days.";
                    }
                    break;
                case 4:
                    if (IsntXmasClose)
                    {
                        Mes = "*What?! It has already passed? Nooooooooo!!! Well... I guess I can practice for next year?*";
                    }
                    else
                    {
                        Mes = "*It's close to that day?! Oh my... I have so much to practice.*";
                    }
                    Button1 = "(Continue)";
                    break;
                case 5:
                    Mes = "*Do you mind If I stay on your world for a while, while I practice?*";
                    Button1 = "Yes, you can stay.";
                    Button2 = "This world? No.";
                    break;
                case 6:
                    if (!Rejected)
                    {
                        Mes = "*Thank you! I wonder if the people here are nice like you, too.*";
                    }
                    else
                    {
                        Mes = "*Aww... By the way, If you change your mind, feel free to call me. Bye.*";
                    }
                    Main.npcChatText = Mes;
                    PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], 8);
                    PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 8).IncreaseFriendshipProgress(1);
                    NpcMod.AddGuardianMet(8, "", !Rejected);
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                    return "";

                case 100:
                    Mes = "*Oh, hello. It's you again.*";
                    Button1 = "Still trying to fly?";
                    break;
                case 101:
                    Mes = "*Yes, but I'm still trying to fly...*";
                    Button1 = "Why don't you stop doing that?";
                    break;
                case 102:
                    Mes = "*I just can't give up being a model.*";
                    Button1 = "You can try being a model without flying";
                    break;
                case 103:
                    Mes = "*I can try... Since I'm here, I guess I'll hang around.*";
                    Button1 = "Be welcome";
                    break;
                case 104:
                    NpcMod.AddGuardianMet(8);
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                    break;
            }
            return Mes;
        }

        public override string GetChat()
        {
            if (AiStage == 2)
            {
                GetDialogueMessage(DialogueStep);
            }
            return "*Help! I'm stuck here!*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (AiStage == 2)
            {
                GetDialogueMessage(DialogueStep, out button, out button2);
                return;
            }
            button = "Help";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                if (AiStage == 2)
                {
                    DialogueStep++;
                }
                else
                {
                    npc.position.Y -= 48;
                    AiStage = 2;
                    AiTimer = 0;
                    SayMessage("*Pull!*");
                    if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
                    {
                        DialogueStep = 100;
                    }
                    npc.velocity.Y -= 6.25f;
                }
            }
            else if (AiStage == 2) //Button 2
            {
                if (DialogueStep == 5)
                    Rejected = true;
                DialogueStep++;
            }
            Main.npcChatText = GetDialogueMessage(DialogueStep);
        }

        public override bool CanChat()
        {
            return AiStage == 1 || AiStage == 2;
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
