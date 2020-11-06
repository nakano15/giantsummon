using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Npcs
{
    public class AlexNPC : ModNPC
    {
        public const int AlexID = 5;
        public GuardianBase AlexGuardianBase { get { return GuardianBase.GetGuardianBase(AlexID); } }
        public int BodyAnimation = 0, LeftArmAnimation = 0;
        public float AnimationTime = 0;
        public const int AI_TYPE = 0, AI_TIMER = 1;
        public int JumpTime = 0;
        public bool PlayerIsMale = false;
        private string MessageText = "";
        private int MessageTime = 0;
        private bool LastOnFloor = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Dog");
        }

        public override void SetDefaults()
        {
            npc.width = AlexGuardianBase.Width;
            npc.height = AlexGuardianBase.Height;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = AlexGuardianBase.InitialMHP;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.knockBackResist = 0.33f;
            npc.aiStyle = -1;
            npc.rarity = 1;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.townNPC = true;
            npc.friendly = true;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CanChat()
        {
            return false;
        }

        public override void AI()
        {
            AlexRecruitScripts.AlexNPCPosition = npc.whoAmI;
            const int Distance = 1024;
            int SpawnPosX = AlexRecruitScripts.TombstoneTileX * 16, SpawnPosY = AlexRecruitScripts.TombstoneTileY * 16;
            bool MoveLeft = false, MoveRight = false, Jump = false, Dash = false;
            string TextToSay = "";
            bool PlayerOnFloor = false;
            npc.npcSlots = 1;
            switch ((int)npc.ai[AI_TYPE])
            {
                case 0: //Wait for Player
                    npc.TargetClosest(true);
                    bool TeleportToSpawn = false;
                    if (npc.target > -1)
                    {
                        Player player = Main.player[npc.target];
                        PlayerIsMale = player.Male;
                        Rectangle FoV = new Rectangle(0, -150, 250, 300);
                        if (npc.direction < 0)
                            FoV.X -= FoV.Width;
                        FoV.X += (int)npc.Center.X;
                        FoV.Y += (int)npc.Center.Y;
                        if (FoV.Intersects(player.getRect()) && Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            //Chase Player
                            npc.ai[AI_TYPE] = 1;
                            npc.ai[AI_TIMER] = 0;
                        }
                        else
                        {
                            //Keep looking for player
                            float XDifference = SpawnPosX - npc.Center.X, YDifference = SpawnPosY - npc.Center.Y;
                            if (Math.Abs(XDifference) >= NPC.sWidth || Math.Abs(YDifference) >= NPC.sHeight)
                            {
                                TeleportToSpawn = true;
                            }
                            else if (Math.Abs(XDifference) >= 16 || Math.Abs(YDifference) >= npc.height)
                            {
                                if (XDifference < 0)
                                    MoveLeft = true;
                                else
                                    MoveRight = true;
                            }
                        }
                    }
                    else
                    {
                        TeleportToSpawn = Math.Abs(npc.Center.X - SpawnPosX) >= 16 || Math.Abs(npc.Center.Y - SpawnPosY) >= npc.height;
                    }
                    if (TeleportToSpawn)
                    {
                        npc.position.X = AlexRecruitScripts.TombstoneTileX - npc.width * 0.5f;
                        npc.position.Y = AlexRecruitScripts.TombstoneTileY - npc.height;
                    }
                    break;
                case 1:
                    if (npc.target == -1)
                    {
                        npc.ai[AI_TYPE] = 0;
                        npc.ai[AI_TIMER] = 0;
                    }
                    else
                    {
                        Player player = Main.player[npc.target];
                        float XDifference = player.Center.X - npc.Center.X, YDifference = player.Center.Y - npc.Center.Y;
                        if (Math.Abs(XDifference) >= NPC.sWidth || Math.Abs(YDifference) >= NPC.sHeight)
                        {
                            //TeleportToSpawn = true;
                            npc.ai[AI_TYPE] = 0;
                            npc.ai[AI_TIMER] = 0;
                        }
                        else
                        {
                            if (XDifference < 0)
                                MoveLeft = true;
                            else
                                MoveRight = true;
                            if (npc.velocity.Y == 0 && Math.Abs(XDifference) <= AlexGuardianBase.Width*2 + AlexGuardianBase.JumpSpeed * 2)
                            {
                                Jump = true;
                            }
                            if (npc.Hitbox.Intersects(player.getRect()))
                            {
                                npc.ai[AI_TYPE] = 2;
                                npc.ai[AI_TIMER] = 0;
                                MessageTime = 30;
                                PlayerOnFloor = true;
                            }
                        }
                    }
                    break;
                case 2: //Speaks with player
                    {
                        npc.npcSlots = 200;
                        if (npc.target == -1)
                        {
                            //Go to the chat where he asks where the player went, then return to AI 0;
                            npc.ai[AI_TYPE] = 3;
                            npc.ai[AI_TIMER] = 0;
                        }
                        else
                        {
                            if (npc.velocity.X != 0 || npc.velocity.Y != 0) //Only chat when lands.
                                break;
                            int DialogueTimer = (int)npc.ai[AI_TIMER] / 30;
                            bool Trigger = npc.ai[AI_TIMER] % 30 == 15;
                            bool HasAlreadyGuardian = false;
                            if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], AlexID))
                            {
                                HasAlreadyGuardian = true;
                            }
                            if (Trigger)
                            {
                                if (HasAlreadyGuardian)
                                {
                                    float XDifference = Main.player[Main.myPlayer].Center.X - npc.Center.X;
                                    if (Math.Abs(XDifference) > AlexGuardianBase.Width)
                                    {
                                        if (XDifference > 0)
                                            MoveRight = true;
                                        else
                                            MoveLeft = true;
                                    }
                                    switch (DialogueTimer / 5)
                                    {
                                        case 0:
                                            TextToSay = "Hey buddy-buddy!";
                                            break;
                                        case 1:
                                            TextToSay = "It's good to see your face again.";
                                            break;
                                        case 2:
                                            TextToSay = "Anything you want, I'm here to protect you.";
                                            break;
                                        case 3:
                                            NpcMod.AddGuardianMet(AlexID);
                                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, AlexID);
                                            //npc.Transform(ModContent.NPCType<GuardianNPC.List.GiantDogGuardian>());
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (DialogueTimer)
                                    {
                                        case 0:
                                            TextToSay = "Hello! Who are you?";
                                            break;
                                        case 5:
                                            TextToSay = "Are you my new friend? Do you want to be my friend?";
                                            break;
                                        case 10:
                                            TextToSay = "You're saying that I'm crushing your chest? Oh! My bad!";
                                            break;
                                        case 12:
                                            npc.velocity.X -= 15f * npc.direction;
                                            npc.velocity.Y -= AlexGuardianBase.JumpSpeed;
                                            break;
                                        case 15:
                                            TextToSay = "By the way, I'm " + AlexGuardianBase.Name + ", let's go on an adventure.";
                                            break;
                                        case 20:
                                            //Guardian GET!
                                            PlayerMod.AddPlayerGuardian(Main.player[npc.target], AlexID);
                                            PlayerMod.GetPlayerGuardian(Main.player[npc.target], AlexID).IncreaseFriendshipProgress(1);
                                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, AlexID);
                                            NpcMod.AddGuardianMet(AlexID);
                                            break;
                                    }
                                }
                            }
                            if (!HasAlreadyGuardian && DialogueTimer < 12)
                            {
                                PlayerOnFloor = true;
                            }
                            npc.ai[AI_TIMER]++;
                        }
                    }
                    break;
                case 3: //Player disappeared/is missing chat
                    {
                        int DialogueTimer = (int)npc.ai[AI_TIMER] / 30;
                        bool Trigger = npc.ai[AI_TIMER] % 30 == 15;
                        if (Trigger)
                        {
                            if (DialogueTimer == 3)
                            {
                                TextToSay = "Where did "+(PlayerIsMale ? "he" : "she")+" go?";
                            }
                            if (DialogueTimer == 7)
                            {
                                TextToSay = "Better I go guard the tombstone then...";
                            }
                            if (DialogueTimer == 13)
                            {
                                npc.ai[AI_TYPE] = 0;
                                npc.ai[AI_TIMER] = 0;
                            }
                        }
                        npc.ai[AI_TIMER]++;
                    }
                    break;
            }
            if (npc.target > -1)
            {
                Player player = Main.player[npc.target];
                    TerraGuardian guardian = player.GetModPlayer<PlayerMod>().Guardian;
                if (PlayerOnFloor)
                {
                    Vector2 NewPlayerPosition = Vector2.Zero;
                    NewPlayerPosition.X = npc.Center.X + (26) * npc.direction;
                    NewPlayerPosition.Y = npc.position.Y + npc.height;
                    player.fullRotationOrigin.X = player.width * 0.5f;
                    player.fullRotationOrigin.Y = player.height * 0.5f;
                    player.velocity = Vector2.Zero;
                    if (player.mount.Active)
                        player.mount.Dismount(player);
                    if (npc.velocity.Y != 0)
                        player.fullRotation = npc.direction * MathHelper.ToRadians(45);
                    else
                        player.fullRotation = npc.direction * MathHelper.ToRadians(90);
                    player.Center = NewPlayerPosition;
                    player.direction = -npc.direction;
                    player.immuneTime = 30;
                    player.immuneNoBlink = true;
                    player.fallStart = (int)player.position.Y / 16;
                    player.breath = player.breathMax;
                    player.statLife++;
                    if (player.statLife > player.statLifeMax2)
                        player.statLife = player.statLifeMax2;
                    if (guardian.Active)
                    {
                        if (guardian.PlayerMounted)
                            guardian.PlayerMounted = false;
                        if (guardian.PlayerControl)
                        {
                            guardian.ImmuneTime = 5;
                            guardian.Rotation = player.fullRotation;
                            guardian.Position.X = player.position.X - guardian.Height * 0.5f * npc.direction;
                            guardian.Position.Y = player.position.Y + guardian.Width * 0.25f;// -guardian.Width * 0.5f;
                        }
                    }
                }
                else if (LastOnFloor)
                {
                    player.fullRotation = 0;
                    player.velocity.Y -= 6.5f;
                    player.direction = -npc.direction;
                    player.fullRotationOrigin = Vector2.Zero;
                    if (guardian.Active)
                    {
                        if (guardian.PlayerControl)
                        {
                            guardian.Rotation = player.fullRotation;
                            guardian.Position.X = player.position.X;
                            guardian.Position.Y = player.position.Y;// -guardian.Height * 0.5f;
                            guardian.Velocity = player.velocity;
                        }
                    }
                }
            }
            LastOnFloor = PlayerOnFloor;
            float Acceleration = AlexGuardianBase.Acceleration, MaxSpeed = AlexGuardianBase.MaxSpeed, Deceleration = AlexGuardianBase.SlowDown,
                JumpSpeed = AlexGuardianBase.JumpSpeed;
            if (Dash)
            {
                MaxSpeed *= 2;
                Acceleration *= 2;
            }
            int MaxJumpHeight = AlexGuardianBase.MaxJumpHeight;
            if (MoveLeft)
            {
                npc.direction = -1;
                npc.velocity.X -= Acceleration;
                if (npc.velocity.X < -MaxSpeed)
                    npc.velocity.X = -MaxSpeed;
                if (npc.collideX && npc.velocity.Y == 0)
                    Jump = true;
            }
            if (MoveRight)
            {
                npc.direction = 1;
                npc.velocity.X += Acceleration;
                if (npc.velocity.X > MaxSpeed)
                    npc.velocity.X = MaxSpeed;
                if (npc.collideX && npc.velocity.Y == 0)
                    Jump = true;
            }
            if (!MoveRight && !MoveLeft)
            {
                if (npc.velocity.X > 0)
                {
                    npc.velocity.X -= Deceleration;
                    if (npc.velocity.X < 0)
                        npc.velocity.X = 0;
                }
                if (npc.velocity.X < 0)
                {
                    npc.velocity.X += Deceleration;
                    if (npc.velocity.X > 0)
                        npc.velocity.X = 0;
                }
            }
            if (Jump)
            {
                if (JumpTime == 0)
                {
                    if (npc.velocity.Y == 0)
                    {
                        JumpTime = MaxJumpHeight;
                        npc.velocity.Y -= JumpSpeed;
                    }
                }
                else
                {
                    if (npc.collideY)
                    {
                        JumpTime = 0;
                    }
                    else
                    {
                        JumpTime--;
                        npc.velocity.Y -= JumpSpeed;
                    }
                }
            }
            else
            {
                if (JumpTime != 0)
                    JumpTime = 0;
            }
            float StepSpeed = 2f, gfxOffY = 0f;
            Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref StepSpeed, ref gfxOffY);
            Vector4 SlopedCollision = Collision.SlopeCollision(npc.position, npc.velocity, npc.width, npc.height, 1f, false);
            npc.position = SlopedCollision.XY();
            npc.velocity = SlopedCollision.ZW();
            if (TextToSay != "")
            {
                MessageTime = 300;
                MessageText = TextToSay;
                /*if (Main.netMode == 0)
                {
                    Main.NewText("Giant Dog: " + TextToSay);
                }
                else
                {

                }*/
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int Frame = 0;
            if (npc.velocity.Y != 0)
            {
                Frame = AlexGuardianBase.JumpFrame;
            }
            else if (npc.velocity.X != 0)
            {
                float MaxTime = AlexGuardianBase.MaxWalkSpeedTime;
                float AnimationSpeed = Math.Abs(npc.velocity.X);
                if ((npc.velocity.X > 0 && npc.direction < 0) || (npc.velocity.X < 0 && npc.direction > 0))
                {
                    AnimationSpeed *= -1;
                }
                npc.frameCounter += AnimationSpeed / AlexGuardianBase.MaxSpeed;
                if (npc.frameCounter < 0)
                    npc.frameCounter += MaxTime;
                if (npc.frameCounter >= MaxTime)
                    npc.frameCounter -= MaxTime;
                Frame = AlexGuardianBase.WalkingFrames[(int)(npc.frameCounter / AlexGuardianBase.WalkAnimationFrameTime)];
            }
            else
            {
                if (npc.ai[AI_TYPE] == 2 && ((!PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], AlexID) && npc.ai[AI_TIMER] < 12 * 30) || (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], AlexID) && npc.ai[AI_TIMER] < 2 * 30))) //&& !PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], AlexID)
                {
                    npc.frameCounter++;
                    Frame = ((int)npc.frameCounter / 5) % 4;
                    if (Frame == 3)
                        Frame = 1;
                    Frame += 19;
                }
                else
                {
                    Frame = AlexGuardianBase.StandingFrame;
                }
            }
            if (Frame >= AlexGuardianBase.FramesInRows)
            {
                npc.frame.Y = Frame / AlexGuardianBase.FramesInRows;
                Frame -= npc.frame.Y * AlexGuardianBase.FramesInRows;
            }
            else
            {
                npc.frame.Y = 0;
            }
            npc.frame.X = Frame;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color drawColor)
        {
            //DrawBodyPart(spriteBatch, drawColor, false);
            foreach (DrawData dd in GetNpcDrawDatas(drawColor, false))
            {
                dd.Draw(spriteBatch);
            }
            if (MessageTime > 0)
            {
                Vector2 TextCenter = npc.position - Main.screenPosition;
                TextCenter.X += npc.width * 0.5f;
                TextCenter.Y -= 18f;
                Utils.DrawBorderString(spriteBatch, MessageText, TextCenter, Color.White,0.85f,0.5f);
            }
            return false;
        }

        public DrawData[] GetNpcDrawDatas(Microsoft.Xna.Framework.Color drawColor, bool FrontPart = false)
        {
            if (!AlexGuardianBase.sprites.IsTextureLoaded)
                AlexGuardianBase.sprites.LoadTextures();
            if (AlexGuardianBase.sprites.HasErrorLoadingOccurred)
            {
                npc.active = false;
                return new DrawData[0];
            }
            Vector2 DrawPosition = npc.position - Main.screenPosition;
            DrawPosition.X += npc.width * 0.5f;
            DrawPosition.Y += npc.height - AlexGuardianBase.SpriteHeight;
            SpriteEffects seffects = npc.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle rect = new Rectangle(AlexGuardianBase.SpriteWidth * npc.frame.X, AlexGuardianBase.SpriteHeight * npc.frame.Y, AlexGuardianBase.SpriteWidth, AlexGuardianBase.SpriteHeight);
            DrawData dd;
            List<DrawData> ddList = new List<DrawData>();
            if (!FrontPart)
            {
                dd = new DrawData(AlexGuardianBase.sprites.BodySprite, DrawPosition, rect, drawColor, 0f, new Vector2(AlexGuardianBase.SpriteWidth * 0.5f, 0f), 1f, seffects, 0);
                ddList.Add(dd);
            }
            dd = new DrawData(AlexGuardianBase.sprites.LeftArmSprite, DrawPosition, rect, drawColor, 0f, new Vector2(AlexGuardianBase.SpriteWidth * 0.5f, 0f), 1f, seffects, 0);
            ddList.Add(dd);
            return ddList.ToArray();
        }

        /*public void DrawBodyPart(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color drawColor, bool FrontPart = false)
        {
            Vector2 DrawPosition = npc.position - Main.screenPosition;
            DrawPosition.X += npc.width * 0.5f;
            DrawPosition.Y += npc.height - AlexGuardianBase.SpriteHeight;
            SpriteEffects seffects = npc.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle rect = new Rectangle(AlexGuardianBase.SpriteWidth * npc.frame.X, AlexGuardianBase.SpriteHeight * npc.frame.Y, AlexGuardianBase.SpriteWidth, AlexGuardianBase.SpriteHeight);
            if(!FrontPart)
                spriteBatch.Draw(AlexGuardianBase.sprites.BodySprite, DrawPosition, rect, drawColor, 0f, new Vector2(AlexGuardianBase.SpriteWidth * 0.5f, 0f), 1f, seffects, 0f);
            spriteBatch.Draw(AlexGuardianBase.sprites.LeftArmSprite, DrawPosition, rect, drawColor, 0f, new Vector2(AlexGuardianBase.SpriteWidth * 0.5f, 0f), 1f, seffects, 0f);
        }*/
    }
}
