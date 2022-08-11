using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Npcs
{
    public class GuardianActorNPC : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "giantsummon/Npcs/Blank";
            }
        }
        public int GuardianID = 0;
        public string GuardianModID = "";
        public int BodyAnimationFrame = 0, LeftArmAnimationFrame = 0, RightArmAnimationFrame = 0;
        public float AnimationTime = 0f;
        public int JumpTime = 0;
        public string MessageText = "";
        public int MessageTime = 0;
        public bool FlipVertically = false;
        public bool MoveLeft = false, MoveRight = false, DropFromPlatform = false, Action = false, Jump = false, Walk = false, Dash = false, Idle = false;
        private bool IsWalking = false;
        public GuardianBase Base
        {
            get
            {
                if (_Base == null)
                    _Base = GuardianBase.GetGuardianBase(GuardianID, GuardianModID);
                return _Base;
            }
        }
        private GuardianBase _Base;
        public byte IdleBehaviorType = 0;
        public int IdleBehaviorTime = 0;
        public List<int> DrawInFrontOfPlayers = new List<int>();
        public float XOffSet = 0, YOffset = 0;
        private float AgeScale = 1f;

        public GuardianActorNPC(int ID, string ModID = "")
        {
            this.GuardianID = ID;
            this.GuardianModID = ModID;
            if (Main.gameMenu)
                return;
            npc.direction = Main.rand.Next(2) == 0 ? -1 : 1;
        }

        public override void SetDefaults()
        {
            npc.width = Base.Width;
            npc.height = Base.Height - Base.CharacterPositionYDiscount;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = (int)Base.InitialMHP;
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

        public void StareAt(Player player)
        {
            Idle = false;
            npc.direction = player.Center.X - npc.Center.X < 0 ? -1 : 1;
        }

        public void StareAt(TerraGuardian tg)
        {
            Idle = false;
            npc.direction = tg.Position.X - npc.Center.X < 0 ? -1 : 1;
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

        private bool FirstFrame = true;

        public override void AI()
        {
            if (Main.netMode < 2 && FirstFrame)
            {
                FirstFrame = false;
                if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
                {
                    GuardianData gd = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
                    AgeScale = TerraGuardian.GetAgeSizeValue(gd.GetRealAgeDecimal());
                }
                else
                {
                    float AgeValue = TerraGuardian.GetAgeDecimalValue(Base.Age, Base.Birthday, GuardianGlobalInfos.LifeTime, Base.GetGroup.AgingSpeed);
                    AgeScale = TerraGuardian.GetAgeSizeValue(AgeValue);
                }
            }
            npc.scale = AgeScale * Base.GetScale;
            if (MessageTime > 0) MessageTime--;
            float Acceleration = Base.Acceleration, MaxSpeed = Base.MaxSpeed, Deceleration = Base.SlowDown,
                JumpSpeed = Base.JumpSpeed;
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].talkNPC == npc.whoAmI)
                {
                    Idle = false;
                    StareAt(Main.player[i]);
                    break;
                }
            }
            if (Idle)
            {
                if (npc.direction == 0)
                {
                    npc.TargetClosest(true);
                    //npc.direction = (Main.rand.NextDouble() < 0.5 ? -1 : 1);
                }
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
                        for (int y = 0; y < npc.height * (1f / 16) + 1; y++)
                        {
                            int Tx = (int)npc.Center.X / 16 + (2 + x) * npc.direction, Ty = (int)npc.Bottom.Y / 16 + y;
                            Tile tile = MainMod.GetTile(Tx, Ty);
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
            IsWalking = Walk;
            MoveLeft = MoveRight = DropFromPlatform = Jump = Action = Dash = Walk = Idle = false;
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

        public int SayMessage(string Text, bool Faster = false)
        {
            MessageText = Text;
            MessageTime = MainMod.CalculateMessageTime(Text);
            if (Faster)
                MessageTime = (int)(MessageTime * 0.8f);
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
                if (IsWalking)
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
                Frame = (int)(npc.frameCounter / (Base.WalkAnimationFrameTime * npc.scale));
                if (Frame < 0)
                    Frame += Base.WalkingFrames.Length;
                if (Frame >= Base.WalkingFrames.Length)
                    Frame -= Base.WalkingFrames.Length;
                Frame = Base.WalkingFrames[Frame];
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
        
        public virtual GuardianDrawData DrawItem(Color drawColor)
        {
            return null;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            foreach (GuardianDrawData dd in GetDrawDatas(drawColor, false))
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
                TextCenter.Y -= 22f;
                int Lines;
                string[] Message = Utils.WordwrapString(MessageText, Main.fontMouseText, 400, 5, out Lines);
                TextCenter.Y -= 22f * Lines;
                for (int l = 0; l <= Lines; l++)
                {
                    Utils.DrawBorderString(Main.spriteBatch, Message[l], TextCenter, Color.White, 1f, 0.5f);
                    TextCenter.Y += 22;
                }
                //Utils.DrawBorderString(spriteBatch, MessageText, TextCenter, Color.White, 1f, 0.5f);
            }
        }

        public virtual void ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, Microsoft.Xna.Framework.Graphics.SpriteEffects seffects)
        {

        }

        public List<GuardianDrawData> GetDrawDatas(Color drawColor, bool FrontPart = false)
        {
            try
            {
                if (Base.sprites == null || Base.sprites.HasErrorLoadingOccurred)
                    return new List<GuardianDrawData>();
                if (!Base.sprites.IsTextureLoaded)
                {
                    Base.sprites.LoadTextures();
                }
                Vector2 DrawPos = npc.position - Main.screenPosition;
                DrawPos.X += XOffSet;
                DrawPos.Y += YOffset;
                DrawPos.X += npc.width * 0.5f;
                DrawPos.Y += npc.height + Base.CharacterPositionYDiscount;
                DrawPos.Y += 2;
                SpriteEffects seffects = (npc.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                if (FlipVertically)
                {
                    if (seffects == SpriteEffects.None)
                        seffects = SpriteEffects.FlipVertically;
                    else
                        seffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    //DrawPos.Y -= Base.SpriteHeight;
                }
                if (Base.IsTerrarian)
                    return GetTerrarianDrawData(DrawPos, seffects, drawColor, FrontPart);
                return GetTerraGuardianDrawData(DrawPos, seffects, drawColor, FrontPart);
            }
            catch
            {
                return new List<GuardianDrawData>();
            }
        }

        private List<GuardianDrawData> GetTerrarianDrawData(Vector2 DrawPos, SpriteEffects seffects, Color drawColor, bool FrontPart)
        {
            GuardianBase.TerrarianCompanionInfos ci = Base.TerrarianInfo;
            List<GuardianDrawData> dds = new List<GuardianDrawData>();
            Rectangle legrect = new Rectangle(0, 56 * BodyAnimationFrame, 40, 56),
                bodyrect = new Rectangle(0, 56 * LeftArmAnimationFrame, 40, 56),
                hairrect = new Rectangle(0, 56 * LeftArmAnimationFrame - 336, 40, 56),
                eyerect = new Rectangle(0, 0, hairrect.Width, hairrect.Height);
            if (hairrect.Y < 0)
                hairrect.Y = 0;
            int SkinVariant = ci.GetSkinVariant(Base.Male);
            Vector2 Origin = new Vector2(20, 56);
            Color HairColor = ci.HairColor,
                EyesColor = ci.EyeColor,
                EyesWhiteColor = Color.White,
                SkinColor = ci.SkinColor,
                UndershirtColor = ci.UnderShirtColor,
                ShirtColor = ci.ShirtColor,
                PantsColor = ci.PantsColor,
                ShoesColor = ci.ShoeColor,
                ArmorColoring = Color.White;
            //Lighting Change
            int TileX = (int)((npc.position.X + npc.width * 0.5f) * (1f / 16)),
                TileY = (int)((npc.position.Y + npc.height * 0.5f) * (1f / 16));
            HairColor = Lighting.GetColor(TileX, TileY, HairColor);
            EyesColor = Lighting.GetColor(TileX, TileY, EyesColor);
            EyesWhiteColor = Lighting.GetColor(TileX, TileY, EyesWhiteColor);
            SkinColor = Lighting.GetColor(TileX, TileY, SkinColor);
            UndershirtColor = Lighting.GetColor(TileX, TileY, UndershirtColor);
            ShirtColor = Lighting.GetColor(TileX, TileY, ShirtColor);
            PantsColor = Lighting.GetColor(TileX, TileY, PantsColor);
            ShoesColor = Lighting.GetColor(TileX, TileY, ShoesColor);
            ArmorColoring = Lighting.GetColor(TileX, TileY, ArmorColoring);
            int LegSlot = ci.DefaultLeggings, BodySlot = ci.DefaultArmor, HeadSlot = ci.DefaultHelmet;
            bool IsTransformed = HeadSlot >= 38 && HeadSlot <= 39;
            bool DrawNormalHair = HeadSlot == 0 || HeadSlot == 10 || HeadSlot == 12 || HeadSlot == 28 || HeadSlot == 62 || HeadSlot == 97 || HeadSlot == 106 || HeadSlot == 113 || HeadSlot == 116 || HeadSlot == 119 || HeadSlot == 133 || HeadSlot == 138 || HeadSlot == 139 || HeadSlot == 163 || HeadSlot == 178 || HeadSlot == 181 || HeadSlot == 191 || HeadSlot == 198,
                DrawAltHair = HeadSlot == 161 || HeadSlot == 14 || HeadSlot == 15 || HeadSlot == 16 || HeadSlot == 18 || HeadSlot == 21 || HeadSlot == 24 || HeadSlot == 25 || HeadSlot == 26 || HeadSlot == 40 || HeadSlot == 44 || HeadSlot == 51 || HeadSlot == 56 || HeadSlot == 59 || HeadSlot == 60 || HeadSlot == 67 || HeadSlot == 68 || HeadSlot == 69 || HeadSlot == 114 || HeadSlot == 121 || HeadSlot == 126 || HeadSlot == 130 || HeadSlot == 136 || HeadSlot == 140 || HeadSlot == 145 || HeadSlot == 158 || HeadSlot == 159 || HeadSlot == 184 || HeadSlot == 190 || HeadSlot == 92 || HeadSlot == 195;
            bool HideLegs = LegSlot == 143 || LegSlot == 106 || LegSlot == 140;
            bool ShowHair = HeadSlot != 202 && HeadSlot != 201;
            if (!DrawNormalHair && HeadSlot != 23 && HeadSlot != 14 && HeadSlot != 56 && HeadSlot != 158 && HeadSlot != 28 && HeadSlot != 201)
                DrawNormalHair = true;
            if (IsTransformed)
                ArmorColoring = SkinColor;
            GuardianDrawData dd;
            if (!HideLegs)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlLegSkin, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.LegSkin], DrawPos, legrect, SkinColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlBodySkin, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.TorsoSkin], DrawPos, bodyrect, SkinColor, npc.rotation, Origin, npc.scale, seffects);
            dds.Add(dd);
            if (LegSlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorLegs, Main.armorLegTexture[LegSlot], DrawPos, legrect, ArmorColoring, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            else
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultPants, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Pants], DrawPos, legrect, PantsColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultShoes, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Shoes], DrawPos, legrect, ShoesColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            if (BodySlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorBody, Main.armorBodyTexture[BodySlot], DrawPos, bodyrect, ArmorColoring, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            else
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultUndershirt, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Undershirt], DrawPos, bodyrect, UndershirtColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultShirt, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Shirt], DrawPos, bodyrect, ShirtColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHead, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Head], DrawPos, bodyrect, SkinColor, npc.rotation, Origin, npc.scale, seffects);
            dds.Add(dd);
            float EyePositionBonus = 0;
            if ((hairrect.Y + 336 >= 7 * hairrect.Height && hairrect.Y + 336 < 10 * hairrect.Height) ||
                hairrect.Y + 336 >= 14 * hairrect.Height && hairrect.Y + 336 < 17 * hairrect.Height)
            {
                EyePositionBonus -= 2;
            }
            DrawPos.Y += EyePositionBonus;
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEye, MainMod.EyeTexture, DrawPos, new Rectangle(40 * 0, 56, 40, 56), EyesColor, npc.rotation, Origin, npc.scale, seffects);
            dds.Add(dd);
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEyeWhite, MainMod.EyeTexture, DrawPos, new Rectangle(40 * 0, 0, 40, 56), EyesWhiteColor, npc.rotation, Origin, npc.scale, seffects);
            dds.Add(dd);
            DrawPos.Y -= EyePositionBonus;
            //hair
            if (ShowHair && ci.HairStyle >= 0)
            {
                if (DrawNormalHair)
                {
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHair, Main.playerHairTexture[ci.HairStyle], DrawPos, hairrect, HairColor, npc.rotation, Origin, npc.scale, seffects);
                    dds.Add(dd);
                }
                else if (DrawAltHair)
                {
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHair, Main.playerHairAltTexture[ci.HairStyle], DrawPos, hairrect, HairColor, npc.rotation, Origin, npc.scale, seffects);
                    dds.Add(dd);
                }
            }
            if(HeadSlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorHead, Main.armorHeadTexture[HeadSlot], DrawPos, bodyrect, ArmorColoring, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            if(BodySlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorArm, Main.armorArmTexture[BodySlot], DrawPos, bodyrect, ArmorColoring, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            else
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlBodyArmSkin, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmSkin], DrawPos, bodyrect, SkinColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultUndershirtArm, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmUndershirt], DrawPos, bodyrect, UndershirtColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultShirtArm, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmShirt], DrawPos, bodyrect, ShirtColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHand, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmHand], DrawPos, bodyrect, SkinColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            return dds;
        }

        private List<GuardianDrawData> GetTerraGuardianDrawData(Vector2 DrawPos, SpriteEffects seffects, Color drawColor, bool FrontPart)
        {
            List<GuardianDrawData> dds = new List<GuardianDrawData>();
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
            GuardianDrawData dd;
            Vector2 Origin = new Vector2(Base.SpriteWidth * 0.5f, Base.SpriteHeight);
            if (!FrontPart)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGRightArm, Base.sprites.RightArmSprite, DrawPos, rightarmrect, drawColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, Base.sprites.BodySprite, DrawPos, bodyrect, drawColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            {
                GuardianDrawData dd2 = DrawItem(drawColor);
                if (dd2 != null)
                    dds.Add(dd2);
            }
            if (bodyfrontrect.X > -1 && Base.sprites.BodyFrontSprite != null)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, Base.sprites.BodyFrontSprite, DrawPos, bodyfrontrect, drawColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            if (rightarmfrontrect.X > -1 && Base.sprites.RightArmFrontSprite != null)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGRightArmFront, Base.sprites.RightArmFrontSprite, DrawPos, rightarmfrontrect, drawColor, npc.rotation, Origin, npc.scale, seffects);
                dds.Add(dd);
            }
            dd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, Base.sprites.LeftArmSprite, DrawPos, leftarmrect, drawColor, npc.rotation, Origin, npc.scale, seffects);
            dds.Add(dd);
            ModifyDrawDatas(dds, DrawPos, bodyrect, leftarmrect, rightarmrect, Origin, drawColor, seffects);
            XOffSet = YOffset = 0f;
            return dds;
        }
    }
}
