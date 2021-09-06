using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using giantsummon.Trigger;

namespace giantsummon.Creatures
{
    public class BunnyReaperGuardianBase : GuardianBase
    {
        public const string SkeletonBodyID = "skeletonbody", SkeletonLeftArmID = "skeletonlarm", SkeletonRightArmID = "skeletonrarm", 
            MouthID = "mouth", MouthLitID = "mouthlit", ScytheID = "scythe", HeadPlasmaID = "head_plasma";

        public BunnyReaperGuardianBase()
        {
            Name = "Liebre";
            Description = "Tasked with collecting souls from the\nTerra Realm and deliver to their destination.\nFeared by many, but he only want to have friends.";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 66;
            //DuckingHeight = 62;
            SpriteWidth = 64;
            SpriteHeight = 80;
            Scale = 60f / 66;
            FramesInRows = 20;
            CompanionSlotWeight = 1.2f;
            Age = 88;
            SetBirthday(SEASON_SPRING, 28);
            Male = true;
            //CalculateHealthToGive(1200, 0.9f, 0.6f); //Lc: 95, LF: 16
            InitialMHP = 125; //1100
            LifeCrystalHPBonus = 25;
            LifeFruitHPBonus = 30;
            Accuracy = 0.72f;
            Mass = 0.55f;
            MaxSpeed = 4.9f;
            Acceleration = 0.14f;
            SlowDown = 0.42f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.16f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            SleepsAtBed = false;
            DontUseHeavyWeapons = false;
            SetTerraGuardian();
            VladimirBase.AddCarryBlacklist(Liebre);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = PlayerMountedArmAnimation = 10;
            HeavySwingFrames = new int[] { 11, 12, 14 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            //DuckingFrame = 23;
            //DuckingSwingFrames = new int[] { 24, 25, 26 };
            SittingFrame = 15;
            ChairSittingFrame = 15;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            DownedFrame = 19;
            ReviveFrame = 18;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);

            SittingPoint2x = new Point(13, 32);

            MountShoulderPoints.DefaultCoordinate2x = new Point(14, 16);

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
            g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
            const float TrailLength = 4;
            if(g.TrailLength == 0)
                g.TrailLength = (int)TrailLength;
            else
                g.TrailLength += (int)TrailLength / 2;
            if (g.TrailDelay == 0)
                g.TrailDelay = 3;
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
            sprites.AddExtraTexture(HeadPlasmaID, "head_p");
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.Death:
                    {
                        switch (Sender.TargetType)
                        {
                            case TriggerTarget.TargetTypes.TerraGuardian:
                                TerraGuardian tg = MainMod.ActiveGuardians[Sender.TargetID];
                                SpawnSoul(tg.CenterPosition, guardian, TerraGuardian.TargetTypes.Guardian, Sender.TargetID, !guardian.IsGuardianHostile(tg));
                                break;
                            case TriggerTarget.TargetTypes.NPC:
                                NPC npc = Main.npc[Sender.TargetID];
                                SpawnSoul(npc.Center, guardian, TerraGuardian.TargetTypes.Npc, Sender.TargetID);
                                break;
                            case TriggerTarget.TargetTypes.Player:
                                Player player = Main.player[Sender.TargetID];
                                SpawnSoul(player.Center, guardian, TerraGuardian.TargetTypes.Player, Sender.TargetID, !guardian.IsPlayerHostile(player));
                                break;
                        }
                    }
                    break;
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            ReaperGuardianData data = (ReaperGuardianData)guardian.Data;
            bool CantHoldScythe = (guardian.SelectedOffhand > -1 || (guardian.ItemAnimationTime > 0 && (guardian.HeldItemHand == HeldHand.Right || guardian.HeldItemHand == HeldHand.Both)) || guardian.KnockedOut);
            if (data.HoldingScythe && CantHoldScythe)
            {
                DetachScythe(guardian, data);
            }
            else
            {
                if (CantHoldScythe)
                    data.ScythePickupDelay = 150;
                if (!data.HoldingScythe)
                {
                    Vector2 MoveDirection = (guardian.CenterPosition - data.ScythePosition);
                    const float MaxMoveSpeed = 8f, MoveSpeed = 0.15f;
                    bool AtXPosition = false, AtYPosition = false;
                    if (Math.Abs(MoveDirection.X) < guardian.Width * 0.5f)
                    {
                        if (Math.Abs(data.ScytheSpeed.X) > 1f)
                            data.ScytheSpeed *= 0.8f;
                        AtXPosition = true;
                    }
                    else
                    {
                        if (MoveDirection.X < 0)
                        {
                            data.ScytheSpeed.X -= MoveSpeed;
                            if (data.ScytheSpeed.X < -MaxMoveSpeed)
                                data.ScytheSpeed.X = -MaxMoveSpeed;
                        }
                        else
                        {
                            data.ScytheSpeed.X += MoveSpeed;
                            if (data.ScytheSpeed.X > MaxMoveSpeed)
                                data.ScytheSpeed.X = MaxMoveSpeed;
                        }
                    }
                    if (Math.Abs(MoveDirection.Y) < guardian.Height * 0.5f)
                    {
                        if (Math.Abs(data.ScytheSpeed.Y) > 1f)
                            data.ScytheSpeed *= 0.8f;
                        AtYPosition = true;
                    }
                    else
                    {
                        if (MoveDirection.Y < 0)
                        {
                            data.ScytheSpeed.Y -= MoveSpeed;
                            if (data.ScytheSpeed.Y < -MaxMoveSpeed)
                                data.ScytheSpeed.Y = -MaxMoveSpeed;
                        }
                        else
                        {
                            data.ScytheSpeed.Y += MoveSpeed;
                            if (data.ScytheSpeed.Y > MaxMoveSpeed)
                                data.ScytheSpeed.Y = MaxMoveSpeed;
                        }
                    }
                    data.ScythePosition += data.ScytheSpeed;
                    if (data.ScythePickupDelay == 0 && AtXPosition && AtYPosition)
                    {
                        data.HoldingScythe = true;
                    }
                    data.ScytheRotation += data.ScytheSpeed.Length() * 0.025f * guardian.Direction;
                }
            }
            if (data.MouthOpenTime > 0)
                data.MouthOpenTime--;
            if (data.ScythePickupDelay > 0)
                data.ScythePickupDelay--;
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
                        d.noLight = true;
                        d.velocity = guardian.Velocity;
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
            data.ScythePickupDelay = 150;
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

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, SpriteEffects seffect, ref List<GuardianDrawData> gdds)
        {
            ReaperGuardianData data = (ReaperGuardianData)guardian.Data;
            Color NewColor = Lighting.GetColor((int)(guardian.Position.X * (1f / 16)), (int)(guardian.CenterY * (1f / 16)));
            float MinOpacity = (float)data.SoulsLoaded / (2500 + data.SoulsLoaded);
            float OpacityRate = (1f - (Math.Max(MinOpacity, (float)(NewColor.R + NewColor.G + NewColor.B) / (255 * 3))));
            Color PlasmaOpacity = Color.White * OpacityRate;
            Vector2 DrawPos = DrawPosition;
            DrawPos.Y -= 2;
            DrawPos.X -= 6;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGHead, sprites.GetExtraTexture(HeadPlasmaID), DrawPos, null, PlasmaOpacity,
                0f,Vector2.Zero, 1f, seffect);
            gdds.Add(gdd);
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
            switch (Frame)
            {
                case 16:
                    return new Vector2(32, 32);
                case 17:
                    return new Vector2(32, 34);
                case 18:
                    return new Vector2(42, 40);
                case 19:
                    return new Vector2(27, 38);
            }
            return new Vector2(40, 34);
        }

        public override string MountUnlockMessage => "*It will sound creepy, but I may be able to carry you on my shoulder, in case you need.*";
        public override string ControlUnlockMessage => "*Your life is limited, but mine isn't. If there's something dangerous you need to do, just take me there.*";

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*Don't be afraid, Terrarian. Your time haven't came yet, but the creatures you've killed must be carried to their destination.*";
                case 1:
                    return "*I have came to this place due to the many deaths happening here. It will ease my job if I stay around.*";
                case 2:
                    return "*I didn't came for your soul, if that's what you are thinking. I'm only going to be a ferry for the creatures who dies here.*";
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My body consists of a kind of plasma, which I use to carry the souls I save. When It's filled enough, I deliver them to their destination.*");
            Mes.Add("*You can only see my body in dark places. If there's light, you can only see my bones.*");
            Mes.Add("*The reason why I'm missing a pelvis and my legs, is related to when I died.*");
            Mes.Add("*Depending on the amount of souls I save, they make my body look like the universe full of stars.*");

            Mes.Add("*I try to avoid being sociable with the people around, since I wouldn't feel good when taking them to their end of line.*");
            Mes.Add("*My presence here makes people scared, they may be thinking I'm after them.*");
            Mes.Add("*Due to people knowing who I am, they avoid even speaking to me. That makes It easier for me not to care about them.*");

            Mes.Add("*It may not look like It, but I like seeing children around. It makes me proud of my job.*");
            Mes.Add("*Don't worry if you see me watching over the children playing. You should be more worried about the creatures that could interrupt their fun.*");

            if(Lighting.Brightness((int)(guardian.Position.X * (1f / 16)), (int)(guardian.CenterY * (1f / 16))) < 0.5f)
            {
                Mes.Add("*Don't fear me in this form, I'm still the same person you've met.*");
                Mes.Add("*My skeletal form is less spooky to people than this form, so I avoid letting people see me in the dark.*");
            }
            if (!((ReaperGuardianData)guardian.Data).HoldingScythe)
            {
                Mes.Add("*This scythe will not hurt anyone by It's own, don't worry.*");
                Mes.Add("*I recommend you not to stay too close to my scythe.*");
            }

            if (player.difficulty == 2)
            {
                Mes.Add("*I'm starting to fear the end of your line, [nickname]. Taking you to your destination will end up being a pain to me.*");
            }
            else
            {
                Mes.Add("*If you take me with you on your adventures, I can hold onto your and your allies souls until you resurrect.*");
            }

            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*I shouldn't actually try saving those abominations. I think I should instead burn them, but that's against the rules.*");
                    Mes.Add("*What kind of monster created those monstrosities. Even their souls seems twisted.*");
                }
                else
                {
                    Mes.Add("*I like seeing living things around, beside you may have thought otherwise.*");
                    Mes.Add("*I used to hate being exposed to the sun for long period, but now I don't feel anything.*");
                    Mes.Add("*It's really weird to me when people stares at my bones. At those moments, I wished It was dark.*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*[nickname], there's many of them around... Be careful...*");
                    Mes.Add("*I suggest you to ensure the safety of the people around.*");
                    Mes.Add("*I can take care of some of those zombies who walks around, I'm demi immortal anyways. But I fear for the other people around.*");
                }
                Mes.Add("*I can't save the zombified people that appears during the night until their bodies aren't destroyed.*");
                Mes.Add("*You should stay safe at night, [nickname]. At this moment, dangerous creatures roams the world.*");
                Mes.Add("*I preffer when people see my plasma, but I wasn't always blue. I had white fur, and a yellow circle on my fur that covered my left eye.*");
            }
            if (Main.raining)
            {
                Mes.Add("*I'm glad I don't need an umbrella.*");
                Mes.Add("*[nickname], beware not to catch a flu. Even a simple flu can be dangerous, if you let It evolve further.*");
            }

            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
            {
                Mes.Add("*I went earlier to [nn:" + Terraria.ID.NPCID.Merchant + "]'s shop to try buying some potions, he ended up pleading for his life.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.ArmsDealer + "] got really impressed when he discovered I was into guns. But I had to end the chatting as soon as possible.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
            {
                Mes.Add("*I asked [nn:" + Terraria.ID.NPCID.Nurse + "] if she needed some assistance. She looked upset, so I left.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
            {
                Mes.Add("*Would you mind help me? I visitted [nn:" + Terraria.ID.NPCID.Clothier + "] earlier this day, to see if he could make me a cloak, and he ran away. May you help me explain?*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
            {
                Mes.Add("*Only [nn:" + Terraria.ID.NPCID.Dryad + "] seems to be the only person who's not scared of me.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
            {
                Mes.Add("*I can't really get angry at [nn:" + Terraria.ID.NPCID.Angler + "], having him lost his parents and living all alone must have changed his behavior.*");
            }

            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*I kind of sympatize with [gn:" + Rococo + "], his spirit is of a child.*");
            }
            if (NpcMod.HasGuardianNPC(Sardine))
            {
                Mes.Add("*Sometimes I go along on [gn:" + Sardine + "]'s adventures. It helps me saving many souls.*");
                if(NpcMod.HasGuardianNPC(Bree))
                    Mes.Add("*I think [gn:" + Bree + "] doesn't actually trust me, she always panics when she sees me with [gn:"+Sardine+"].*");
            }
            if (NpcMod.HasGuardianNPC(Alex))
            {
                Mes.Add("*[gn:"+Alex+"] asked me if I carried his old owner to their destination. I only came to this world recently, so maybe another reaper did.*");
                Mes.Add("*I like how [gn:"+Alex+"] is such a good dog. "+AlexRecruitScripts.AlexOldPartner+" have done one fine job.*");
                Mes.Add("*This will sound grim, but [gn:"+Alex+"] told me to bring him to "+AlexRecruitScripts.AlexOldPartner+"'s resting place when he dies. I can do that but.. I would need to find out where did she went to.*");
            }
            bool HasZacks = NpcMod.HasGuardianNPC(Zacks), HasFluffles = NpcMod.HasGuardianNPC(Fluffles);
            if (HasZacks && NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*[gn:"+Blue+"] confronted me earlier, wanting me not to take [gn:"+Zacks+"] to their destination. I replied to her that even if I wanted, I couldn't due to his curse.*");
            }
            if(HasZacks)
            {
                Mes.Add("*I can't take [gn:"+Zacks+"] to their destination due to the curse laid on him. Until that curse is lifted, I can't do anything. Beside I think he may still be willing to help you, so maybe I can open an exception.*");
                Mes.Add("*The curse [gn:"+Zacks+"] carries is different from the ones the zombies in this land carries. That explains why he regained consciousness.*");
            }
            if (HasFluffles)
            {
                Mes.Add("*[gn:"+Fluffles+"] soul is troubled by what happened to her old group. And she seems to have such a strong bond towards you, so I can't take her to her destination.*");
                Mes.Add("*What happened to [gn:"+Fluffles+"] caused on her some kind of post traumatic stress. Maybe time will make her recover from that.*");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                Mes.Add("*I get so bored when [gn:"+Leopold+"] asks me about the life after the life and about death.*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*How many times should I say that I won't... Oh, I thought It was [gn:"+Malisha+"].*");
                Mes.Add("*I sense death at [gn:"+Malisha+"]'s house.*");
            }
            if (NpcMod.HasGuardianNPC(Mabel))
            {
                Mes.Add("*I can't stop staring at [gn:"+Mabel+"]. Wait, I have a job to do, so I can't distract myself!*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*Out of everyone in this world, [gn:"+Vladimir+"] is the only one who seems to want to be my friend.*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*Even though I don't have a digestive system, or the need to eat, I love eating food. Gladly [gn:" + Minerva + "] is such a good cook.*");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                bool HasBreeMet = PlayerMod.PlayerHasGuardian(player, Bree), HasSardineMet = PlayerMod.PlayerHasGuardian(player, Sardine);
                if (HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*I'm glad to see that [gn:"+Glenn+"] found his parents safe and sound. It would be awful if he ended up orphan.*");
                }
                else if (!HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*It seems like [gn:" + Glenn + "] still didn't find his mother. You should try not to think of what could have happened to her if you want to find her.*");
                }
                else if (HasBreeMet && !HasSardineMet)
                {
                    Mes.Add("*It seems like [gn:" + Glenn + "] still didn't find his father. You should try not to think of what could have happened to him if you want to find him.*");
                }
                else
                {
                    Mes.Add("*It seems like [gn:" + Glenn + "] still didn't find any of his parents. You should try not to think of what could have happened if you want to find them.*");
                }
                Mes.Add("*That kid, [gn:"+Glenn+"], is very curious and talkative. Anyways, It's not everyday one have a reaper as neighbor.*");
            }
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("*I was watching [gn:" + Cinnamon + "] earlier, and spooked her to death... Not literally, but she ran away so fast when she noticed. She should really not get scared whenever she sees me watching her.*");
                Mes.Add("*[gn:" + Cinnamon + "]... *");
            }
            if (NpcMod.HasGuardianNPC(Miguel))
            {
                Mes.Add("*Reaping also involves physical exercise. [gn:"+Miguel+"]'s pondering is really stupid.*");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Yes, I see that ghost. I can't remove It from you, but I can help killing what killed her to free you.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Do you think the people here will eventually stop being scared of me?*");
            Mes.Add("*I don't know which moment I like more, the moment I was alive, or after death.*");
            Mes.Add("*I have been around many realms for so long, doing my job. This is the first time I actually laid down somewhere and interacted with living people.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*There is nothing I look for right now.*");
            Mes.Add("*I don't think there is something I want at this moment.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*There's actually one thing you can help me with, if you are interessed...*");
            Mes.Add("*I've been so busy with my job, that I forgot to take care of a thing... Gladly, you can do It instead, if you agree to.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You have my deepest gratitude, [nickname].*");
            Mes.Add("*You have a kind soul.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It may sound weird, but I also like having a place to call my own.*");
            Mes.Add("*This place is too open for me... You could give me some house.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Yes, I celebrate birthday. I'm actually happy that you guys managed to prepare such a thing for me.*");
            Mes.Add("*What? You want to see me dancing? I don't think you can follow my steps.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I shall not allow this.*");
            Mes.Add("*Your time is not now.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*You want me to be your buddy? Fine. I will be your buddy, even after life ceases.*";
                case MessageIDs.RescueMessage:
                    return "*Your time didn't came yet, I'll help ensure that.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*I was just taking a quick rest. I can't really sleep.*";
                        case 1:
                            return "*Hm... Need something, [nickname]? I was taking some rest.*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*I was waiting for you. Did you do what I asked you to do?*";
                        case 1:
                            return "*Hm... Did you do what I asked?*";
                    }
                    break;
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I don't mind joining your group. That will reduce the amount of stray souls in the world.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*...No... It's way too crowded for me right now.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Sorry, but I preffer to stay here for now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*I don't mind leaving the group here, but would be better if I leaved the group on a safe place.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I'll return to my house, then. Safe travels, [nickname].*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*I'll try returning to the village, then. Safe travels, [nickname].*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Sorry, but now is the not the best moment for that.*";
                case MessageIDs.RequestAccepted:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I'm glad you accepted. I will be waiting here.*";
                    return "*Thank you. Please be safe while doing my request.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*Stress actually can kill a person too, take care of the other requests you have before taking mine.*";
                case MessageIDs.RequestRejected:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*It's fine. I can try doing that later, anyways.*";
                    return "*Hm... I also thought that the request wasn't very doable.*";
                case MessageIDs.RequestPostpone:
                    return "*Got some more important matters? Okay.*";
                case MessageIDs.RequestFailed:
                    return "*I really didn't liked the result of that...*";
                case MessageIDs.RestAskForHowLong:
                    return "*Feeling tired? Well, then I agree with that idea too. How long?*";
                case MessageIDs.RestNotPossible:
                    return "*That sounds like a bad idea right now.*";
                case MessageIDs.RestWhenGoingSleep:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I can keep a watch while you rest. I don't sleep anyways.*";
                    return "*I hope I don't cause you to remain awaken during the rest.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*[shop] has something interesting... Let's check it out.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Wait a minute while I shop for a few things...*";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No...*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you. Safe travels.*";
                case MessageIDs.ChatAboutSomething:
                    return "*Curious about me? Or is it about life in general? I don't know if I can answer your questions.*";
                case MessageIDs.NevermindTheChatting:
                    return "*No more doubts? Okay. Want to talk about something else?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*The request is too much for you? If you want I can relieve you from that.*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Very well. I'll carry on my request then.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Alright. If you change your mind, or anything change related to my request, come see me.*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Is he asleep...? Let me try...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*This smell... What is this...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*My heart is pounding so fast... I need to calm down...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Whew... It's done... And reeeeally scary too!*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Yikes!! So scary!*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() <= 0.5f)
                        return "You're a kind soul. Thank you.";
                    return "*I apreciate the help.*";
                case MessageIDs.RevivedByRecovery:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I always return.*";
                    return "*You can't get rid of me for long.*";
                    //
                case MessageIDs.LeopoldMessage1:
                    return "*We're not slaves.*";
                case MessageIDs.LeopoldMessage2:
                    return "*Oh my... The Terrarian even enslaved a Reaper! Things are more serious than I thought!*";
                case MessageIDs.LeopoldMessage3:
                    return "*I already said that we're not slaves. We're aiding the Terrarian on their quest.*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Ugh... Prepare to suffer!*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Argh, this is damaging my plasma.*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*Something is wrong with my eyes!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Is this the right way?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*This will not save you from me.*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*I'll catch you up.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I still can push myself in this fight.*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Ugh... You pierced my plasma coat...*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*There is no escape from this. We need to defeat that twisted creature.*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*This color doesn't fit my plasma.*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*I feel the cold even in my bones...*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I can't do much like this...*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*I will take your soul!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Okay, I can take on the offensive now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*I don't feel the taste of this food.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Even my scythe looks sharper.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Faster!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Health up!*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*This will make the job easier.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*I feel a bit dirty for doing this...*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*...I miss drinking ale... I don't feel the taste of It...*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*You're glad you can taste it, [nickname].*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*I even feel vigorous after seeing that. Life Crystal!*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*[nickname], no. Look, trap.*";
                case MessageIDs.FoundMineTile:
                    return "*A mine, watch out.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*By my experience, a number of souls I gathered are from playing with Detonators.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*I have a bad feeling about this...*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Don't worry, I know where to deliver etherians souls.*";
                case MessageIDs.FoundTreasureTile:
                    return "*Well, that's a colirium to the eyes.*";
                case MessageIDs.FoundGemTile:
                    return "*Gems.*";
                case MessageIDs.FoundRareOreTile:
                    return "*I see some rare ores, if you're looking for them.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Look, over there. Shiny ores.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*As long as you don't scream during the ride, It's okay.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Enough exploration for now?*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Another soul joins us.*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*Welcome, and please, don't freak out.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*I could use some aid on reaping.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*I don't think even the souls in my body believe in what I see.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*...Don't make me take you too, [nickname]... Resist...*";
                case MessageIDs.LeaderDiesMessage:
                    return "*I'm sorry, [nickname]...*";
                case MessageIDs.AllyFallsMessage:
                    return "*An ally fell nearby, we should help them.*";
                case MessageIDs.SpotsRareTreasure:
                    return "*Even I am curious to see that.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be right back. Try not to kill things while I'm away.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*You're hurt, [nickname]. Watch yourself.*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*My plasma body wont hold much more...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I have few healing potions disponible!*";
                case MessageIDs.UsesLastPotion:
                    return "*I just used my last healing potion! Do you have more?*";
                case MessageIDs.SpottedABoss:
                    return "*We should put that thing to rest, [nickname].*";
                case MessageIDs.DefeatedABoss:
                    return "*I'll try avoiding this soul from becoming twisted again.*";
                case MessageIDs.InvasionBegins:
                    return "*So many souls... I hope I can save all of them...*";
                case MessageIDs.RepelledInvasion:
                    return "*We could have solved that peacefully... If they choose that.*";
                case MessageIDs.EventBegins:
                    return "*The air has changed. What is going to happen?*";
                case MessageIDs.EventEnds:
                    return "*That is not something that happens all the time, right?*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*A noble soul known as [player] checks me up from time to time. I'm glad they don't fear me.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*With the help of [player], I recently saved the soul of [subject].*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*[player] found something that seems rather rare: A [subject].*";
                case MessageIDs.FeatEventFinished:
                    return "*I had a lot of work when a [subject] happened. So many souls to save... I'm thankful to [player] for helping me with that.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*[player] just met a new soul. Their name was [subject], if I remember well.*";
                case MessageIDs.FeatPlayerDied:
                    return "*I shouldn't let this shaken me, since It's part of my job, but latelly I had to save the soul of my friend [player]. At least now I know they're resting in a good place.*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] opened the door of a temple recently. Why that place was locked out, is the question.*";
                case MessageIDs.FeatCoinPortal:
                    return "*You wouldn't believe in the face [player] had when they saw coins raining off a portal.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public class ReaperGuardianData : GuardianData
        {
            private const int SaveID = 0;
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
            public byte ScythePickupDelay = 0;

            public ReaperGuardianData(int ID, string ModID) : base(ID, ModID)
            {

            }

            public override void SaveCustom(TagCompound tag, int UniqueID)
            {
                tag.Add(UniqueID + "tg_SaveID", SaveID);
                tag.Add(UniqueID + "tg_SoulsLoaded", SoulsLoaded);
            }

            public override void LoadCustom(TagCompound tag, int ModVersion, int UniqueID)
            {
                int SaveID = tag.GetInt(UniqueID + "tg_SaveID");
                SoulsLoaded = tag.GetInt(UniqueID + "tg_SoulsLoaded");
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
