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
    public class GuardianActorNPC : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "giantsummon/GuardianNPC/BlankNPC";
            }
        }
        public string NpcAlias = "";
        public int GuardianID = 0;
        public string GuardianModID = "";
        public int BodyAnimationFrame = 0, LeftArmAnimationFrame = 0, RightArmAnimationFrame = 0;
        public float AnimationTime = 0f;
        public int JumpTime = 0;
        public string MessageText = "";
        public int MessageTime = 0;
        public bool FlipVertically = false;
        public bool MoveLeft = false, MoveRight = false, Action = false, Jump = false, Walk = false, Dash = false, Idle = false;
        public GuardianBase Base
        {
            get { return GuardianBase.GetGuardianBase(GuardianID, GuardianModID); }
        }
        private byte IdleBehaviorType = 0;
        private int IdleBehaviorTime = 0;
        public List<int> DrawInFrontOfPlayers = new List<int>();
        public float XOffSet = 0, YOffset = 0;

        public GuardianActorNPC(int ID, string ModID, string Alias = "")
        {
            this.GuardianID = ID;
            this.GuardianModID = ModID;
            if (Alias != "")
                NpcAlias = Alias;
            else
                NpcAlias = Base.Name;
            npc.direction = Main.rand.Next(2) == 0 ? -1 : 1;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Base.Name);
        }

        public override void SetDefaults()
        {
            npc.width = Base.Width;
            npc.height = Base.Height;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = Base.InitialMHP;
            npc.HitSound = Terraria.ID.SoundID.NPCHit1;
            npc.DeathSound = Terraria.ID.SoundID.NPCDeath2;
            npc.knockBackResist = 0.33f;
            npc.aiStyle = -1;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.townNPC = true;
            npc.friendly = true;
            npc.direction = 1;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreAI()
        {
            DrawInFrontOfPlayers.Clear();
            return base.PreAI();
        }

        public bool IsInPerceptionRange(Player player, float CustomRangeX = -1, float CustomRangeY = -1)
        {
            if(CustomRangeX < 0)
                CustomRangeX = NPC.sWidth * 0.5f + NPC.safeRangeX;
            if(CustomRangeY < 0)
                CustomRangeY = NPC.sHeight * 0.5f + NPC.safeRangeY;
            return (Math.Abs(player.Center.X - npc.Center.X) < CustomRangeX &&
                    Math.Abs(player.Center.Y - npc.Center.Y) < CustomRangeY);
        }

        public override void AI()
        {
            if (MessageTime > 0) MessageTime--;
            float Acceleration = Base.Acceleration, MaxSpeed = Base.MaxSpeed, Deceleration = Base.SlowDown,
                JumpSpeed = Base.JumpSpeed;
            if (Idle)
            {
                if (npc.direction == 0)
                    npc.direction = (Main.rand.NextDouble() < 0.5 ? -1 : 1);
                if (IdleBehaviorTime <= 0)
                {
                    IdleBehaviorType = (byte)Main.rand.Next(2);
                    IdleBehaviorTime = 200 + Main.rand.Next(200);
                    npc.direction *= -1;
                }
                if (IdleBehaviorType == 1)
                {
                    Walk = true;
                    if (npc.direction > 0)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
                IdleBehaviorTime--;
                if (MoveLeft || MoveRight)
                {
                    bool HasTileInFront = false;
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            int Tx = (int)npc.Center.X / 16 + (2 + x) * npc.direction, Ty = (int)npc.Bottom.Y / 16 + y;
                            Tile tile = Framing.GetTileSafely(Tx, Ty);
                            if (tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]))
                            {
                                HasTileInFront = true;
                                break;
                            }
                        }
                    }
                    if (!HasTileInFront)
                    {
                        if (MoveRight)
                        {
                            MoveRight = false;
                            MoveLeft = true;
                        }
                        else
                        {
                            MoveRight = true;
                            MoveLeft = false;
                        }
                    }
                }
            }
            if (Walk)
            {
                Acceleration = TerraGuardian.WalkAcceleration;
                MaxSpeed = TerraGuardian.WalkMaxSpeed;
                Deceleration = TerraGuardian.WalkSlowDown;
            }
            else if (Dash)
            {
                MaxSpeed *= 2;
                Acceleration *= 2;
            }
            int MaxJumpHeight = Base.MaxJumpHeight;
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
                        npc.velocity.Y = -JumpSpeed;
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
                        npc.velocity.Y = -JumpSpeed;
                    }
                }
            }
            else
            {
                if (JumpTime != 0)
                    JumpTime = 0;
            }
            MoveLeft = MoveRight = Jump = Action = Dash = Walk = Idle = false;
            Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);
            Vector4 SlopedCollision = Collision.SlopeCollision(npc.position, npc.velocity, npc.width, npc.height, 1f, false);
            npc.position = SlopedCollision.XY();
            npc.velocity = SlopedCollision.ZW();
        }

        public override bool CanChat()
        {
            float PlayerX = Main.player[Main.myPlayer].Center.X;
            if (PlayerX < npc.Center.X)
                npc.direction = -1;
            else
                npc.direction = 1;
            return base.CanChat();
        }

        public int SayMessage(string Text)
        {
            MessageText = Text;
            MessageTime = MainMod.CalculateMessageTime(Text);
            return MessageTime;
        }
        
        public override void FindFrame(int frameHeight)
        {
            int Frame = Base.StandingFrame;
            if (npc.velocity.Y != 0)
            {
                Frame = Base.JumpFrame;
            }
            else if (npc.velocity.X != 0)
            {
                float MaxTime = Base.MaxWalkSpeedTime * npc.scale;
                float AnimationSpeed = Math.Abs(npc.velocity.X);
                if (Walk)
                    AnimationSpeed *= 3;
                if ((npc.velocity.X > 0 && npc.direction < 0) || (npc.velocity.X < 0 && npc.direction > 0))
                {
                    AnimationSpeed *= -1;
                }
                npc.frameCounter += AnimationSpeed / Base.MaxSpeed;
                if (npc.frameCounter < 0)
                    npc.frameCounter += MaxTime;
                if (npc.frameCounter >= MaxTime)
                    npc.frameCounter -= MaxTime;
                Frame = Base.WalkingFrames[(int)(npc.frameCounter / (Base.WalkAnimationFrameTime * npc.scale))];
            }
            else
            {
                Frame = Base.StandingFrame;
            }
            /*if (Frame >= Base.FramesInRows)
            {
                npc.frame.Y += Frame / Base.FramesInRows;
                Frame -= npc.frame.Y * Base.FramesInRows;
            }
            else
            {
                npc.frame.Y = 0;
            }*/
            BodyAnimationFrame = Frame;
            LeftArmAnimationFrame = Frame;
            RightArmAnimationFrame = Frame;
        }
        
        public virtual DrawData? DrawItem(Color drawColor)
        {
            return null;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            foreach (DrawData dd in GetDrawDatas(drawColor, false))
            {
                dd.Draw(spriteBatch);
            }
            return false;
        }

        public void DrawMessage(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (MessageTime > 0)
            {
                Vector2 TextCenter = npc.position - Main.screenPosition;
                TextCenter.X += npc.width * 0.5f;
                TextCenter.Y -= 18f;
                Utils.DrawBorderString(spriteBatch, MessageText, TextCenter, Color.White, 1f, 0.5f);
            }
        }

        public virtual void ModifyDrawDatas(List<DrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, Microsoft.Xna.Framework.Graphics.SpriteEffects seffects)
        {

        }

        public List<DrawData> GetDrawDatas(Microsoft.Xna.Framework.Color drawColor, bool FrontPart = false)
        {
            try
            {
                if (Base.sprites == null || Base.sprites.HasErrorLoadingOccurred)
                    return new List<DrawData>();
                else if (!Base.sprites.IsTextureLoaded)
                {
                    Base.sprites.LoadTextures();
                }
                Base.sprites.ResetCooldown();
                List<DrawData> dds = new List<DrawData>();
                Vector2 DrawPos = npc.position - Main.screenPosition;
                DrawPos.X += XOffSet;
                DrawPos.Y += YOffset;
                DrawPos.X += npc.width * 0.5f;
                DrawPos.Y += npc.height;
                DrawPos.Y += 2;
                Microsoft.Xna.Framework.Graphics.SpriteEffects seffects = (npc.direction < 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally : Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                if (FlipVertically)
                {
                    if (seffects == Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                        seffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically;
                    else
                        seffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically | Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                    //DrawPos.Y -= Base.SpriteHeight;
                }
                Rectangle bodyrect = new Rectangle(BodyAnimationFrame, 0, Base.SpriteWidth, Base.SpriteHeight),
                    bodyfrontrect = new Rectangle(BodyAnimationFrame, 0, Base.SpriteWidth, Base.SpriteHeight),
                    leftarmrect = new Rectangle(LeftArmAnimationFrame, 0, Base.SpriteWidth, Base.SpriteHeight),
                    rightarmrect = new Rectangle(RightArmAnimationFrame, 0, Base.SpriteWidth, Base.SpriteHeight),
                    rightarmfrontrect = new Rectangle(RightArmAnimationFrame, 0, Base.SpriteWidth, Base.SpriteHeight);
                if (Base.SpecificBodyFrontFramePositions)
                {
                    bodyfrontrect.X = Base.GetBodyFrontSprite(bodyfrontrect.X);
                }
                else
                {
                    bodyfrontrect.X = -1;
                }
                if (Base.RightArmFrontFrameSwap.ContainsKey(RightArmAnimationFrame))
                {
                    rightarmfrontrect.X = Base.RightArmFrontFrameSwap[RightArmAnimationFrame];
                }
                else
                {
                    rightarmfrontrect.X = -1;
                }
                if (bodyfrontrect.X >= Base.FramesInRows)
                {
                    bodyfrontrect.Y += bodyfrontrect.X / Base.FramesInRows;
                    bodyfrontrect.X -= bodyfrontrect.Y * Base.FramesInRows;
                }
                if (bodyrect.X >= Base.FramesInRows)
                {
                    bodyrect.Y += bodyrect.X / Base.FramesInRows;
                    bodyrect.X -= bodyrect.Y * Base.FramesInRows;
                }
                if (leftarmrect.X >= Base.FramesInRows)
                {
                    leftarmrect.Y += leftarmrect.X / Base.FramesInRows;
                    leftarmrect.X -= leftarmrect.Y * Base.FramesInRows;
                }
                if (rightarmrect.X >= Base.FramesInRows)
                {
                    rightarmrect.Y += rightarmrect.X / Base.FramesInRows;
                    rightarmrect.X -= rightarmrect.Y * Base.FramesInRows;
                }
                if (rightarmfrontrect.X >= Base.FramesInRows)
                {
                    rightarmfrontrect.Y += rightarmfrontrect.X / Base.FramesInRows;
                    rightarmfrontrect.X -= rightarmfrontrect.Y * Base.FramesInRows;
                }
                bodyrect.X *= bodyrect.Width;
                bodyrect.Y *= bodyrect.Height;

                leftarmrect.X *= leftarmrect.Width;
                leftarmrect.Y *= leftarmrect.Height;

                rightarmrect.X *= rightarmrect.Width;
                rightarmrect.Y *= rightarmrect.Height;

                bodyfrontrect.X *= bodyfrontrect.Width;
                bodyfrontrect.Y *= bodyfrontrect.Height;

                rightarmfrontrect.X *= rightarmfrontrect.Width;
                rightarmfrontrect.Y *= rightarmfrontrect.Height;
                DrawData dd;
                Vector2 Origin = new Vector2(Base.SpriteWidth * 0.5f, Base.SpriteHeight);
                if (!FrontPart)
                {
                    dd = new DrawData(Base.sprites.RightArmSprite, DrawPos, rightarmrect, drawColor, npc.rotation, Origin, npc.scale, seffects, 0);
                    dds.Add(dd);
                    dd = new DrawData(Base.sprites.BodySprite, DrawPos, bodyrect, drawColor, npc.rotation, Origin, npc.scale, seffects, 0);
                    dds.Add(dd);
                }
                /*{
                    DrawData? dd2 = DrawItem(drawColor);
                    if (dd2.HasValue)
                        dds.Add(dd2.Value);
                }*/
                if (bodyfrontrect.X > -1 && Base.sprites.BodyFrontSprite != null)
                {
                    dd = new DrawData(Base.sprites.BodyFrontSprite, DrawPos, bodyfrontrect, drawColor, npc.rotation, Origin, npc.scale, seffects, 0);
                    dds.Add(dd);
                }
                if (rightarmfrontrect.X > -1 && Base.sprites.RightArmFrontSprite != null)
                {
                    dd = new DrawData(Base.sprites.RightArmFrontSprite, DrawPos, rightarmfrontrect, drawColor, npc.rotation, Origin, npc.scale, seffects, 0);
                    dds.Add(dd);
                }
                dd = new DrawData(Base.sprites.LeftArmSprite, DrawPos, leftarmrect, drawColor, npc.rotation, Origin, npc.scale, seffects, 0);
                dds.Add(dd);
                ModifyDrawDatas(dds, DrawPos, bodyrect, leftarmrect, rightarmrect, Origin, drawColor, seffects);
                XOffSet = YOffset = 0f;
                return dds;
            }
            catch
            {
                Main.NewText("The error happened with " + npc.GivenOrTypeName + ".", Color.Purple);
                return new List<DrawData>();
            }
        }
    }
}
