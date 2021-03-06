using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Creatures
{
    public class BunnyReaperGuardianBase : GuardianBase
    {
        public const string SkeletonBodyID = "skeletonbody", SkeletonLeftArmID = "skeletonlarm", SkeletonRightArmID = "skeletonrarm", 
            MouthID = "mouth", MouthLitID = "mouthlit", ScytheID = "scythe";

        public BunnyReaperGuardianBase()
        {
            Name = "BunnyReaperGuardian";
            Description = "";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 66;
            //DuckingHeight = 62;
            SpriteWidth = 64;
            SpriteHeight = 80;
            FramesInRows = 20;
            Age = 88;
            Male = true;
            CalculateHealthToGive(1200, 0.9f, 0.6f); //Lc: 95, LF: 16
            Accuracy = 0.72f;
            Mass = 0.7f;
            MaxSpeed = 4.9f;
            Acceleration = 0.14f;
            SlowDown = 0.42f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.16f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            SleepsAtBed = false;
            SetTerraGuardian();

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = PlayerMountedArmAnimation = 10;
            //HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            //DuckingFrame = 23;
            //DuckingSwingFrames = new int[] { 24, 25, 26 };
            SittingFrame = 15;
            ChairSittingFrame = 15;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            //DownedFrame = 21;
            ReviveFrame = 18;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);

            SittingPoint2x = new Point(13, 32);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(11, 17, 6);
            LeftHandPoints.AddFramePoint2x(12, 23, 11);
            LeftHandPoints.AddFramePoint2x(13, 25, 19);
            LeftHandPoints.AddFramePoint2x(14, 23, 24);

            LeftHandPoints.AddFramePoint2x(15, 20, 26);

            LeftHandPoints.AddFramePoint2x(18, 21, 29);

            //Right Arm
            RightHandPoints.DefaultCoordinate2x = new Point(27, 18);
            RightHandPoints.AddFramePoint2x(3, 26, 18);
            RightHandPoints.AddFramePoint2x(4, 26, 17);
            RightHandPoints.AddFramePoint2x(5, 26, 17);
            RightHandPoints.AddFramePoint2x(6, 26, 18);
            RightHandPoints.AddFramePoint2x(8, 27, 17);
            RightHandPoints.AddFramePoint2x(9, 27, 17);
            RightHandPoints.AddFramePoint2x(10, 27, 17);

            RightHandPoints.AddFramePoint2x(11, 21, 6);
            RightHandPoints.AddFramePoint2x(12, 25, 11);
            RightHandPoints.AddFramePoint2x(13, 27, 19);
            RightHandPoints.AddFramePoint2x(14, 25, 24);

            RightHandPoints.AddFramePoint2x(15, 27, 18);
            RightHandPoints.AddFramePoint2x(16, 18 + 2, 25);
            RightHandPoints.AddFramePoint2x(17, 15, 25);
            RightHandPoints.AddFramePoint2x(18, 28, 22);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            //g.AddFlag(GuardianFlags.CantReceiveHelpOnReviving);
            //g.AddFlag(GuardianFlags.HideKOBar);
            //g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
            if (g.KnockedOut)
            {
                g.AddFlag(GuardianFlags.DontTakeAggro);
                g.AddFlag(GuardianFlags.CantBeHurt);
            }
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new ReaperGuardianData(ID, ModID);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(SkeletonBodyID, "skeleton_body");
            sprites.AddExtraTexture(SkeletonLeftArmID, "skeleton_left_arm");
            sprites.AddExtraTexture(SkeletonRightArmID, "skeleton_right_arm");
            sprites.AddExtraTexture(MouthID, "mouth");
            sprites.AddExtraTexture(MouthLitID, "mouth_lit");
            sprites.AddExtraTexture(ScytheID, "scythe");
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.GuardianDies:
                    {
                        TerraGuardian tg = MainMod.ActiveGuardians[Value];
                        SpawnSoul(tg.CenterPosition, guardian, TerraGuardian.TargetTypes.Guardian, Value, !guardian.IsGuardianHostile(tg));
                    }
                    break;
                case TriggerTypes.NpcDies:
                    {
                        NPC npc = Main.npc[Value];
                        SpawnSoul(npc.Center, guardian, TerraGuardian.TargetTypes.Npc, Value);
                    }
                    break;
                case TriggerTypes.PlayerDies:
                    {
                        Player player = Main.player[Value];
                        SpawnSoul(player.Center, guardian, TerraGuardian.TargetTypes.Player, Value, !guardian.IsPlayerHostile(player));
                    }
                    break;
            }
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            ReaperGuardianData data = (ReaperGuardianData)guardian.Data;
            if (guardian.SelectedOffhand > -1)
            {
                if (data.HoldingScythe)
                    DetachScythe(guardian, data);
                else
                {
                    Vector2 MoveDirection = (guardian.CenterPosition - data.ScythePosition);
                    float MaxSpeed = (guardian.Velocity.Length() + 1);
                    if (MaxSpeed < 3)
                        MaxSpeed = 3;
                    if (MoveDirection.Length() < 20)
                    {
                        //data.ScytheSpeed *= 0.1f;
                        //data.ScytheSpeed *= 0.33f;
                    }
                    else
                    {
                        MoveDirection.Normalize();
                        data.ScytheSpeed += MoveDirection* 0.1f;
                    }
                    float Velocity = data.ScytheSpeed.Length();
                    if (Velocity > MaxSpeed)
                    {
                        data.ScytheSpeed.Normalize();
                        data.ScytheSpeed *= MaxSpeed;
                        Velocity = MaxSpeed;
                    }
                    data.ScythePosition += data.ScytheSpeed;
                    if (MoveDirection.X < 0)
                        data.ScytheRotation -= Velocity * 0.01f;
                    else
                        data.ScytheRotation += Velocity * 0.01f;
                }
            }
            else
            {
                if (!data.HoldingScythe)
                {
                    Vector2 MoveDirection = (guardian.CenterPosition - data.ScythePosition);
                    if (MoveDirection.Length() < 2)
                    {
                        data.HoldingScythe = true;
                    }
                    else
                    {
                        if (MoveDirection.Length() < 20)
                        {
                            MoveDirection.Normalize();
                            data.ScytheSpeed = MoveDirection * 0.9f;
                        }
                        else
                        {
                            MoveDirection.Normalize();
                            data.ScytheSpeed += MoveDirection * 0.33f;
                        }
                        float Velocity = data.ScytheSpeed.Length();
                        if (Velocity > 8f)
                        {
                            data.ScytheSpeed.Normalize();
                            data.ScytheSpeed *= 8f;
                            Velocity = 8f;
                        }
                        data.ScythePosition += data.ScytheSpeed;
                        if (MoveDirection.X < 0)
                            data.ScytheRotation -= Velocity * 0.01f;
                        else
                            data.ScytheRotation += Velocity * 0.01f;
                    }
                }
            }
            if (data.MouthOpenTime > 0)
                data.MouthOpenTime--;
            const float MaxSoulSpeed = 8f;
            Vector2 SoulEndPos = GetMouthPosition(guardian.BodyAnimationFrame) * guardian.Scale;
            SoulEndPos.X -= guardian.SpriteWidth * 0.5f;
            if (guardian.LookingLeft)
                SoulEndPos.X *= -1;
            SoulEndPos.Y = -guardian.SpriteHeight + SoulEndPos.Y;
            SoulEndPos += guardian.Position;
            int DefeatedAllyCount = 0;
            for (int s = 0; s < data.ActiveSouls.Count; s++)
            {
                ReaperGuardianData.FallenSoul soul = data.ActiveSouls[s];
                Vector2 DirectionComparer = ((soul.HoverOnly ? guardian.GetGuardianRightHandPosition - new Vector2(0, 8) : SoulEndPos) + guardian.Velocity - soul.Position);
                if (soul.HoverOnly)
                {
                    if (!soul.IsOwnerActive)
                        soul.HoverOnly = false;
                    else if (soul.IsOwnerAlive)
                    {
                        switch (data.ActiveSouls[s].Owner)
                        {
                            case TerraGuardian.TargetTypes.Player:
                                {
                                    Player player = Main.player[data.ActiveSouls[s].OwnerID];
                                    player.Bottom = guardian.Position;
                                    player.fallStart = (int)player.position.Y / 16;
                                    player.immuneTime *= 3;
                                }
                                break;
                            case TerraGuardian.TargetTypes.Guardian:
                                {
                                    TerraGuardian tg = MainMod.ActiveGuardians[data.ActiveSouls[s].OwnerID];
                                    tg.Position = guardian.Position;
                                    tg.SetFallStart();
                                    tg.ImmuneTime *= 3;
                                }
                                break;
                        }
                        data.ActiveSouls.RemoveAt(s);
                        continue;
                    }
                    else if(soul.Owner == TerraGuardian.TargetTypes.Player && Main.player[soul.OwnerID].ghost)
                    {
                        soul.HoverOnly = false;
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                guardian.SaySomething("Your quest is over, " + Main.player[soul.OwnerID].name + ".");
                                break;
                            case 1:
                                guardian.SaySomething("Your time has came, " + Main.player[soul.OwnerID].name + ".");
                                break;
                        }
                        if(soul.OwnerID == Main.myPlayer)
                        {
                            MainMod.SoulSaved = true;
                        }
                        if (data.PlayerDeathCounter.ContainsKey(soul.OwnerID))
                            data.PlayerDeathCounter.Remove(soul.OwnerID);
                        data.PlayerDeathCounter.Add(soul.OwnerID, 0);
                    }
                    if(data.LastDefeatedAllyCount > 1)
                        DirectionComparer += new Vector2(5f * (float)Math.Sin((float)DefeatedAllyCount / data.LastDefeatedAllyCount * 360), 5f * (float)Math.Cos((float)DefeatedAllyCount / data.LastDefeatedAllyCount * 360));
                    DefeatedAllyCount++;
                }
                float Distance = DirectionComparer.Length();
                if (Distance < 40)
                {
                    soul.Velocity *= 0.5f;
                    if(!soul.HoverOnly)
                        data.MouthOpenTime = 3;
                }
                if (Distance < 2)
                {
                    if (!soul.HoverOnly)
                    {
                        data.ActiveSouls.RemoveAt(s);
                        data.SoulsLoaded++;
                        Main.PlaySound(Terraria.ID.SoundID.Item3, guardian.CenterPosition).Volume *= 0.333f;
                        for (int effect = 0; effect < 5; effect++)
                        {
                            int dustid = Dust.NewDust(soul.Position, 8, 8, 175, Main.rand.Next(-300, 300) * 0.01f, 0f, 100, default(Color), 2f);
                            Main.dust[dustid].noGravity = true;
                            Dust dust = Main.dust[dustid];
                            dust.velocity *= 0f;
                        }
                        continue;
                    }
                    soul.Velocity *= 0.99f;
                }
                else
                {
                    DirectionComparer.Normalize();
                    soul.Velocity += DirectionComparer;
                }
                if (soul.Velocity.Length() > MaxSoulSpeed)
                {
                    soul.Velocity.Normalize();
                    soul.Velocity *= MaxSoulSpeed;
                }
                soul.Position += soul.Velocity;
                if (soul.Owner == TerraGuardian.TargetTypes.Player && soul.OwnerID == Main.myPlayer)
                    MainMod.PlayerSoulPosition = soul.Position;
                for (int effect = 0; effect < 5; effect++)
                {
                    int dustid = Dust.NewDust(soul.Position, 8, 8, 175, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustid].noGravity = true;
                    Dust dust = Main.dust[dustid];
                    dust.velocity *= 0f;
                }
            }
            int[] Keys = data.PlayerDeathCounter.Keys.ToArray();
            foreach(int k in Keys)
            {
                if (!Main.player[k].active)
                    data.PlayerDeathCounter.Remove(k);
                else
                {
                    Main.player[k].position = Vector2.Zero;
                    if (Main.myPlayer == k && MainMod.SoulSaved)
                    {
                        MainMod.PlayerSoulPosition = guardian.CenterPosition;
                    }
                    data.PlayerDeathCounter[k]++;
                    if (data.PlayerDeathCounter[k] == 150)
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                guardian.SaySomething("Time to take you to your final destination.");
                                break;
                            case 1:
                                guardian.SaySomething("I'll make sure to bring you to your resting place safe and sound.");
                                break;
                            case 2:
                                guardian.SaySomething("You fought well until the very end, time to get you some rest.");
                                break;
                        }
                    }
                }
            }
            data.LastDefeatedAllyCount = (byte)DefeatedAllyCount;
            if(Main.netMode < 2)
            {
                float SoulsValue = data.SoulsLoaded;
                const int SparkleDelay = 2500;
                while (SoulsValue > 0)
                {
                    if (Main.rand.NextFloat() < (float)SoulsValue / (SparkleDelay + SoulsValue))
                    {
                        Dust d = Dust.NewDustDirect(guardian.TopLeftPosition, guardian.Width, guardian.Height, Terraria.ID.DustID.SilverCoin);
                        d.noGravity = true;
                        d.velocity *= 0;
                    }
                    SoulsValue -= SparkleDelay;
                }
            }
        }

        public void SpawnSoul(Vector2 Position, TerraGuardian guardian, TerraGuardian.TargetTypes OwnerType, int OwnerID, bool HoverOnly = false)
        {
            ReaperGuardianData data = (ReaperGuardianData)guardian.Data;
            data.ActiveSouls.Add(new ReaperGuardianData.FallenSoul() { Position = Position, Owner = OwnerType, OwnerID = OwnerID, HoverOnly = HoverOnly });
        }

        public void DetachScythe(TerraGuardian guardian,  ReaperGuardianData data)
        {
            data.HoldingScythe = false;
            data.ScythePosition = guardian.GetGuardianRightHandPosition;
            data.ScytheRotation = 0;
            data.ScytheSpeed = Vector2.Zero;
            data.ScytheFacingLeft = guardian.LookingLeft;
            byte ScytheType = 0;
            switch (guardian.RightArmAnimationFrame)
            {
                default:
                    ScytheType = 1;
                    break;
                case 16:
                    data.ScytheRotation = -1.57079633f * guardian.Direction;
                    break;
                case 17:
                    break;
            }
            data.ScytheType = ScytheType;
        }

        private GuardianDrawData DrawEquippedScythe(ReaperGuardianData data, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            if (!data.HoldingScythe)
                return null;
            Vector2 ScythePosition = guardian.GetGuardianRightHandPosition - Main.screenPosition;
            SpriteEffects ScytheEffect = SpriteEffects.None;
            float ScytheRotation = 0f;
            byte ScytheType = 0;
            switch (guardian.RightArmAnimationFrame)
            {
                default:
                    ScytheType = 1;
                    break;
                case 16:
                    ScytheRotation = -1.57079633f * guardian.Direction;
                    break;
                case 17:
                    break;
            }
            Vector2 ScytheOrigin = (ScytheType == 0 ?
                new Vector2(ReaperGuardianData.ScytheVerticalHoldX, ReaperGuardianData.ScytheVerticalHoldY) :
                new Vector2(ReaperGuardianData.ScytheDiagonalHoldX, ReaperGuardianData.ScytheDiagonalHoldY));
            if (!guardian.LookingLeft)
            {
                switch (ScytheType)
                {
                    case 0:
                        ScytheEffect = SpriteEffects.FlipHorizontally;
                        ScytheOrigin.X = 66 - ScytheOrigin.X;
                        break;
                    case 1:
                        ScytheEffect = SpriteEffects.FlipHorizontally;
                        ScytheOrigin.X = 66 - ScytheOrigin.X;
                        break;
                }
            }
            Texture2D ScytheTexture = sprites.GetExtraTexture(ScytheID);
            return new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, ScytheTexture, ScythePosition, new Rectangle(ScytheType * 66, 0, 66, 66), color, ScytheRotation, ScytheOrigin, Scale, ScytheEffect);
        }

        private GuardianDrawData DrawFlyingScythe(ReaperGuardianData data, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Vector2 ScythePosition = data.ScythePosition - Main.screenPosition;
            SpriteEffects ScytheEffect = SpriteEffects.None;
            float ScytheRotation = data.ScytheRotation;
            byte ScytheType = data.ScytheType;
            Vector2 ScytheOrigin = (ScytheType == 0 ?
                new Vector2(ReaperGuardianData.ScytheVerticalHoldX, ReaperGuardianData.ScytheVerticalHoldY) :
                new Vector2(ReaperGuardianData.ScytheDiagonalHoldX, ReaperGuardianData.ScytheDiagonalHoldY));
            if (!data.ScytheFacingLeft)
            {
                switch (ScytheType)
                {
                    case 0:
                        ScytheEffect = SpriteEffects.FlipHorizontally;
                        ScytheOrigin.X = 66 - ScytheOrigin.X;
                        break;
                    case 1:
                        ScytheEffect = SpriteEffects.FlipHorizontally;
                        ScytheOrigin.X = 66 - ScytheOrigin.X;
                        break;
                }
            }
            Texture2D ScytheTexture = sprites.GetExtraTexture(ScytheID);
            return new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, ScytheTexture, ScythePosition, new Rectangle(ScytheType * 66, 0, 66, 66), color, ScytheRotation, ScytheOrigin, Scale, ScytheEffect);
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            ReaperGuardianData data = (ReaperGuardianData)guardian.Data;
            bool HasSoulNearby = data.MouthOpenTime > 0;
            bool BodyPlaced = false, LeftArmPlaced = false, RightArmPlaced = false, MouthPlaced = false;
            GuardianDrawData gdd;
            float MinOpacity = (float)data.SoulsLoaded / (2500 + data.SoulsLoaded);
            float OpacityRate = (1f - (Math.Max(MinOpacity, (float)(color.R + color.G + color.B) / (255 * 3))));
            Color PlasmaOpacity = Color.White * OpacityRate;
            bool BodyIsFront = false;
            GuardianDrawData ScytheSlot = DrawEquippedScythe(data, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
            byte MouthFrameX = 0;
            switch (guardian.BodyAnimationFrame)
            {
                case 16:
                    MouthFrameX = 1;
                    break;
                case 17:
                    MouthFrameX = 2;
                    break;
                case 18:
                    MouthFrameX = 3;
                    break;
            }
            for (int i = 0; i < TerraGuardian.DrawBehind.Count; i++)
            {
                switch (TerraGuardian.DrawBehind[i].textureType)
                {
                    case GuardianDrawData.TextureType.TGBodyFront:
                        {
                            TerraGuardian.DrawBehind[i].color = PlasmaOpacity;
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        {
                            if (!BodyPlaced)
                            {
                                BodyIsFront = false;
                                bool DrawMouth = false;
                                if (!MouthPlaced)
                                {
                                    if(HasSoulNearby)
                                        DrawMouth = true;
                                    MouthPlaced = true;
                                }
                                TerraGuardian.DrawBehind[i].color = PlasmaOpacity;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(SkeletonBodyID), DrawPosition,
                                    guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), color, Rotation, Origin, Scale, seffect);
                                TerraGuardian.DrawBehind.Insert(i, gdd);
                                if (DrawMouth)
                                {
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthLitID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, SpriteHeight, SpriteWidth, SpriteHeight), PlasmaOpacity, Rotation, Origin, Scale, seffect);
                                    if(i + 3 < TerraGuardian.DrawBehind.Count) TerraGuardian.DrawBehind.Insert(i + 3, gdd);
                                    else TerraGuardian.DrawBehind.Add(gdd);
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, SpriteHeight, SpriteWidth, SpriteHeight), PlasmaOpacity, Rotation, Origin, Scale, seffect);
                                    TerraGuardian.DrawBehind.Insert(i + 3, gdd);
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthLitID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, 0, SpriteWidth, SpriteHeight), Color.White, Rotation, Origin, Scale, seffect);
                                    TerraGuardian.DrawBehind.Insert(i + 1, gdd);
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, 0, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
                                    TerraGuardian.DrawBehind.Insert(i + 1, gdd);
                                }
                                BodyPlaced = true;
                            }
                        }
                        break;
                    case GuardianDrawData.TextureType.TGLeftArm:
                        {
                            if (!LeftArmPlaced)
                            {
                                TerraGuardian.DrawBehind[i].color = PlasmaOpacity;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(SkeletonLeftArmID), DrawPosition,
                                    guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame), color, Rotation, Origin, Scale, seffect);
                                TerraGuardian.DrawBehind.Insert(i, gdd);
                                LeftArmPlaced = true;
                            }
                        }
                        break;
                    case GuardianDrawData.TextureType.TGRightArm:
                        {
                            if (!RightArmPlaced)
                            {
                                TerraGuardian.DrawBehind[i].color = PlasmaOpacity;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(SkeletonRightArmID), DrawPosition,
                                    guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame), color, Rotation, Origin, Scale, seffect);
                                TerraGuardian.DrawBehind.Insert(i, gdd);
                                RightArmPlaced = true;
                                if(ScytheSlot != null)
                                    TerraGuardian.DrawBehind.Insert(i, ScytheSlot);
                            }
                        }
                        break;
                }
            }
            for (int i = 0; i < TerraGuardian.DrawFront.Count; i++)
            {
                switch (TerraGuardian.DrawFront[i].textureType)
                {
                    case GuardianDrawData.TextureType.TGBodyFront:
                        {
                            TerraGuardian.DrawFront[i].color = PlasmaOpacity;
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        {
                            if (!BodyPlaced)
                            {
                                BodyIsFront = true;
                                bool DrawMouth = false;
                                if (!MouthPlaced)
                                {
                                    DrawMouth = true;
                                    MouthPlaced = true;
                                }
                                TerraGuardian.DrawFront[i].color = PlasmaOpacity;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(SkeletonBodyID), DrawPosition,
                                    guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), color, Rotation, Origin, Scale, seffect);
                                TerraGuardian.DrawFront.Insert(i, gdd);
                                if (DrawMouth)
                                {
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthLitID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, SpriteHeight, SpriteWidth, SpriteHeight), PlasmaOpacity, Rotation, Origin, Scale, seffect);
                                    if(i + 3 < TerraGuardian.DrawFront.Count) TerraGuardian.DrawFront.Insert(i + 3, gdd);
                                    else TerraGuardian.DrawFront.Add(gdd);
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, SpriteHeight, SpriteWidth, SpriteHeight), PlasmaOpacity, Rotation, Origin, Scale, seffect);
                                    TerraGuardian.DrawFront.Insert(i + 3, gdd);
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthLitID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, 0, SpriteWidth, SpriteHeight), Color.White, Rotation, Origin, Scale, seffect);
                                    TerraGuardian.DrawFront.Insert(i + 1, gdd);
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MouthID), DrawPosition,
                                        new Rectangle(SpriteWidth * MouthFrameX, 0, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
                                    TerraGuardian.DrawFront.Insert(i + 1, gdd);
                                }
                                BodyPlaced = true;
                            }
                        }
                        break;
                    case GuardianDrawData.TextureType.TGLeftArm:
                        {
                            if (!LeftArmPlaced)
                            {
                                TerraGuardian.DrawFront[i].color = PlasmaOpacity;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(SkeletonLeftArmID), DrawPosition,
                                    guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame), color, Rotation, Origin, Scale, seffect);
                                TerraGuardian.DrawFront.Insert(i, gdd);
                                LeftArmPlaced = true;
                            }
                        }
                        break;
                    case GuardianDrawData.TextureType.TGRightArm:
                        {
                            if (!RightArmPlaced)
                            {
                                TerraGuardian.DrawFront[i].color = PlasmaOpacity;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(SkeletonRightArmID), DrawPosition,
                                    guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame), color, Rotation, Origin, Scale, seffect);
                                TerraGuardian.DrawFront.Insert(i, gdd);
                                RightArmPlaced = true;
                                if (ScytheSlot != null)
                                    TerraGuardian.DrawFront.Insert(i, ScytheSlot);
                            }
                        }
                        break;
                }
            }
            if(ScytheSlot == null)
            {
                TerraGuardian.DrawBehind.Insert(0, DrawFlyingScythe(data, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect));
            }
            if (!Main.projectileLoaded[Terraria.ID.ProjectileID.LostSoulHostile])
                Main.instance.LoadProjectile(Terraria.ID.ProjectileID.LostSoulHostile);
            Texture2D SoulTexture = Main.projectileTexture[Terraria.ID.ProjectileID.LostSoulHostile];
            foreach (ReaperGuardianData.FallenSoul soul in data.ActiveSouls)
            {
                Vector2 SoulPosition = soul.Position - Main.screenPosition;
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, SoulTexture, SoulPosition,
                    null, Color.White, 0f, new Vector2(SoulTexture.Width, SoulTexture.Height) * 0.5f, 1f, SpriteEffects.None);
                if (BodyIsFront)
                    TerraGuardian.DrawFront.Add(gdd);
                else
                    TerraGuardian.DrawBehind.Add(gdd);
            }
        }

        public Vector2 GetMouthPosition(int Frame)
        {
            return new Vector2(40, 34);
        }

        public class ReaperGuardianData : GuardianData
        {
            public List<FallenSoul> ActiveSouls = new List<FallenSoul>();
            public int SoulsLoaded = 0;
            public byte LastDefeatedAllyCount = 0;
            public Dictionary<int, int> PlayerDeathCounter = new Dictionary<int, int>();
            public byte MouthOpenTime = 0;
            public bool HoldingScythe = true;
            public byte ScytheType = 1;
            public const int ScytheDiagonalHoldX = 12, ScytheDiagonalHoldY = 48;
            public const int ScytheVerticalHoldX = 33, ScytheVerticalHoldY = 52;
            public Vector2 ScythePosition = Vector2.Zero, ScytheSpeed = Vector2.Zero;
            public float ScytheRotation = 0f;
            public bool ScytheFacingLeft = false;

            public ReaperGuardianData(int ID, string ModID) : base(ID, ModID)
            {

            }

            public class FallenSoul
            {
                public Vector2 Position = Vector2.Zero;
                public Vector2 Velocity = Vector2.Zero;
                public TerraGuardian.TargetTypes Owner = TerraGuardian.TargetTypes.Npc;
                public int OwnerID = 0;
                public bool HoverOnly = false;

                public bool IsOwnerActive
                {
                    get
                    {
                        switch (Owner)
                        {
                            case TerraGuardian.TargetTypes.Player:
                                return Main.player[OwnerID].active;
                            case TerraGuardian.TargetTypes.Guardian:
                                return MainMod.ActiveGuardians.ContainsKey(OwnerID);
                        }
                        return false;
                    }
                }

                public bool IsOwnerAlive
                {
                    get
                    {
                        switch (Owner)
                        {
                            case TerraGuardian.TargetTypes.Player:
                                return !Main.player[OwnerID].dead && !Main.player[OwnerID].ghost;
                            case TerraGuardian.TargetTypes.Guardian:
                                return !MainMod.ActiveGuardians[OwnerID].Downed;
                        }
                        return false;
                    }
                }
            }
        }
    }
}
