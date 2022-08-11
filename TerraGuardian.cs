﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.IO;

namespace giantsummon
{
    public class TerraGuardian
    {
        public const float DivisionBy16 = 1f / 16;
        public static bool UpdateAge = false;
        public const int MinimumAgeToDrink = 18;
        public const int TimeUntilCompanionForgetsTarget = 5 * 60;
        public DrawMoment drawMoment = DrawMoment.DontDraw;
        public List<PathFinder.Breadcrumbs> Paths = new List<PathFinder.Breadcrumbs>();
        public int WhoAmID = 0;
        public static int IDStack = 0;
        public static bool ForceKill = false;
        public int GuardianTownNpcInfoPosition = -1;
        public int TalkPlayerID = -1;
        public byte AssistSlot = 0;
        public int SavedPosX = -1, SavedPosY = -1; //Redo the path finding if moving to it was interrupted by any way.
        public bool PathingInterrupted = false, WalkToPath = false;
        public List<byte> NpcsSpotted = new List<byte>(), PlayersSpotted = new List<byte>();
        public List<int> GuardiansSpotted = new List<int>();
        public const int ItemStackCount = 100, ItemStackTurns = Main.maxItems / ItemStackCount;
        public GuardianCommonStatus GetCommonStatus { get { return Data.GetCommonStatus; } }
        public GuardianBase Base { get { return Data.Base; } }// return GuardianBase.GetGuardianBase(ID, Data.ModID); } }
        public GuardianData Data
        {
            get
            {
                /*try
                {
                    if (OwnerPos > -1)
                    {
                        if (AssistSlot >= MainMod.MaxExtraGuardianFollowers + 1)
                        {
                            Active = false;
                        }
                        else
                        {
                            int MyGuardianIndex = (AssistSlot == 0 ? Main.player[OwnerPos].GetModPlayer<PlayerMod>().SelectedGuardian : Main.player[OwnerPos].GetModPlayer<PlayerMod>().SelectedAssistGuardians[AssistSlot - 1]);
                            if (!Main.player[OwnerPos].GetModPlayer<PlayerMod>().MyGuardians.ContainsKey(MyGuardianIndex))
                            {
                                Active = false;
                            }
                            else
                            {
                                return Main.player[OwnerPos].GetModPlayer<PlayerMod>().MyGuardians[MyGuardianIndex];
                            }
                        }
                    }
                    else
                    {
                        if (_Data != null && Main.netMode == 0 && !Main.gameMenu && Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().HasGuardian(_Data.ID, _Data.ModID))
                        {
                            return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetGuardian(_Data.ID, _Data.ModID);
                        }
                    }
                }
                catch { }*/
                if (_Data == null)
                {
                    _Data = new GuardianData();
                }
                return _Data;
            }
            set
            {
                _Data = value;
            }
        }
        private GuardianData _Data = null;
        public Group GetGroup { get { return Base.GetGroup; } }
        public int GetSomeoneToSpawnProjectileFor
        {
            get
            {
                if (OwnerPos > -1)
                {
                    return OwnerPos;
                }
                return Main.myPlayer; 
            }
        }
        public WorldMod.GuardianTownNpcState GetTownNpcInfo
        {
            get
            {
                if(GuardianTownNpcInfoPosition > -1)
                {
                    if(WorldMod.GuardianNPCsInWorld[GuardianTownNpcInfoPosition] == null)
                    {
                        GuardianTownNpcInfoPosition = -1;
                    }
                    else
                    {
                        return WorldMod.GuardianNPCsInWorld[GuardianTownNpcInfoPosition];
                    }
                }
                return null;
            }
        }
        public string Name { get { if (Data.Name != null) return Data.Name; else return RealName; } set { Data.Name = value; } }
        public string RealName { get { return Data.RealName; } }
        public string GroupID { get { return Base.GetGroupID; } }
        public int ID { get { return Data.MyID.ID; } set { Data.MyID.ID = value; } }
        public string ModID { get { return Data.MyID.ModID; } set { Data.MyID.ModID = value; } }
        public GuardianID MyID { get { return Data.MyID; } set { Data.MyID = value; } }
        public int MyGuardianID { get { return Data.MyGuardianID; } set { Data.MyGuardianID = value; } }
        public string PersonalNicknameToPlayer { get { return Data.PersonalNicknameToPlayer; } set { Data.PersonalNicknameToPlayer = value; } }
        public GuardianMood Mood { get { return Data.Mood; } }
        public bool HasRequestActive { get { return Data.request.state == RequestData.RequestState.Active; } }
        public int Age { get { return Data.Age; } }
        private float AgeScale = 0;
        //Trail Handler
        public int TrailLength = 0, TrailDelay = 0;
        public List<TrailPositionLogger> Trails = new List<TrailPositionLogger>();
        public float PulsePower = 0;
        public float PulseValue = 0;
        public sbyte PulseDir = 1;
        //
        public bool Active = false;
        public bool IsStarter
        {
            get { return Data.IsStarter; }
            set { Data.IsStarter = value; }
        }
        public bool IsLeader
        {
            get
            {
                if (OwnerPos == -1) return false;
                TerraGuardian tg = PlayerMod.GetPlayerMainGuardian(Main.player[OwnerPos]); ;
                return tg.Active && tg.WhoAmID == WhoAmID;
            }
        }
        public bool MoveRight = false, MoveLeft = false, MoveUp = false, MoveDown = false;
        public bool LastMoveRight = false, LastMoveLeft = false, LastMoveUp = false, LastMoveDown = false;
        public bool DropFromPlatform { get { return MoveDown && Jump; } set { MoveDown = Jump = value; } }
        public bool LastDroppedFromPlatform { get { return LastMoveDown && LastJump; } }
        public bool Action = false, Jump = false, OffHandAction = false;
        public bool IsDualWielding = false;
        public bool CanDualWield { get { return HeldItemHand != HeldHand.Both && !OffHandAction && HasFlag(GuardianFlags.CanDualWield); } }
        public bool LastAction = false, LastJump = false, LastOffHandAction = false;
        public bool Channeling = false;
        public bool ShowOffHand = false;
        public bool Ducking = false;
        public bool HurtPanic = false;
        public bool LastWasInCombat = false;
        public bool WaitingForManaRecharge = false;
        public byte SkinID { get { return Data.SkinID; } set { Data.SkinID = value; } }
        public byte OutfitID { get { return Data.OutfitID; } set { Data.OutfitID = value; } }
        public Genders Gender { get { if (Base.CanChangeGender) { if (Data.Male) { return Genders.Male; } else { return Genders.Female; } } return Base.Gender; } }
        public bool Male { get { if (Base.CanChangeGender) { return Data.Male; } return Base.Male; } set { if (Base.CanChangeGender) Data.Male = value; } }
        public bool IsGenderless { get { return Base.Genderless; } }
        public bool Tanker { get { return Data.Tanker; } set { Data.Tanker = value; if (Data.Tanker) { Aggro += TankerAggroBonus; } else { Aggro -= TankerAggroBonus; } } }
        public bool PlayerMounted = false;
        public bool PlayerControl = false;
        public bool GuardianHasControlWhenMounted = false;
        public bool AttackMyTarget { get { return Data.AttackMyTarget; } set { Data.AttackMyTarget = value; } }
        public bool Passive { get { return Data.Passive; } set { Data.Passive = value; } }
        public bool Is2PControlled { get { return MainMod.Gameplay2PMode && AssistSlot == 0 && OwnerPos > -1 && OwnerPos == Main.myPlayer; } }
        public bool MayLootItems { get { return Data.MayLootItems; } set { Data.MayLootItems = value; } }
        public bool SetToPlayerSize { get { return Data.SetToPlayerSize; } set { Data.SetToPlayerSize = value; } }
        public bool AvoidCombat { get { return Data.AvoidCombat; } set { Data.AvoidCombat = value; } }
        public bool ChargeAhead { get { return Data.ChargeAhead; } set { Data.ChargeAhead = value; } }
        public bool ProtectMode { get { return Data.ProtectMode && !GuardingPosition.HasValue; } set { Data.ProtectMode = value; } }
        public bool GetItemsISendToTrash { get { return Data.GetItemsISendtoTrash; } set { Data.GetItemsISendtoTrash = value; } }
        public bool OverrideQuickMountToMountGuardianInstead { get { return Data.OverrideQuickMountToMountGuardianInstead; } set { Data.OverrideQuickMountToMountGuardianInstead = value; } }
        public bool UseHeavyMeleeAttackWhenMounted { get { return Data.UseHeavyMeleeAttackWhenMounted; } set { Data.UseHeavyMeleeAttackWhenMounted = value; } }
        public bool UseWeaponsByInventoryOrder { get { return Data.UseWeaponsByInventoryOrder; } set { Data.UseWeaponsByInventoryOrder = value; } }
        public bool HideWereForm { get { return Data.HideWereForm; } set { Data.HideWereForm = value; } }
        public float Accuracy { get { if (_Accuracy < 1f) return _Accuracy; return 1f; } set { _Accuracy = value; } }
        public float Agility { get { if (_Agility < 0.1f) return 0.1f; return _Agility; } set { _Agility = value; } }
        public float Trigger { get { if (_Trigger < 0.1f) return 0.1f; if (_Trigger < 1f) return _Trigger; return 1f; } set { _Trigger = value; } }
        private float _Accuracy = 1f, _Agility = 1f, _Trigger = 1f;
        private byte TriggerStack = 0;
        public bool FallProtection = false;
        public bool LockDirection = false;
        public bool IsBeingPulledByPlayer = false, SuspendedByChains = false;
        public float TownNpcs = 0f, ActiveNpcs = 0f;
        public int WingType = 0, WingFrame = 0, WingCounter = 0;
        public int HeldProj = -1;
        public const int TankerAggroBonus = 100;
        public int Aggro = 0;
        public byte BeetleOrb = 0;
        private bool WingFlapSound = false;
        public float WingFlightTime = 0, WingMaxFlightTime = 0;
        public float LifeStealRate = 0, GhostDamage = 0;
        public int RocketTime = 0, RocketMaxTime = 0;
        public float Stealth = 0f;
        public uint Coins
        {
            get { return Data.Coins; }
            set
            {
                if (value > 0 && unchecked(Data.Coins + value) < 0)
                {
                    Data.Coins = uint.MaxValue;
                }
                else if (value < 0 && unchecked(Data.Coins + value) > 0)
                {
                    Data.Coins = 0;
                }
                else
                {
                    Data.Coins = value;
                }
            }
        }
        public List<BuffData> Buffs { get { return Data.Buffs; } set { Data.Buffs = value; } }
        public List<int> BuffImmunity = new List<int>();
        private bool FreezeItemUseAnimation = false;
        public Point? GuardingPosition = null;
        public bool IsGuardingPlace = false;
        public List<GuardianCooldownManager> Cooldowns { get { return Data.Cooldowns; } set { Data.Cooldowns = value; } }
        public List<GuardianCooldownManager.CooldownType> CooldownException = new List<GuardianCooldownManager.CooldownType>();
        public List<GuardianSkills> SkillList
        {
            get
            {
                if (GuardianCommonStatus.UseSkillProgressShare)
                    return GetCommonStatus.SkillList;
                return Data.SkillList;
            }
        }
        public IdleActions CurrentIdleAction = IdleActions.Wait;
        public int IdleActionTime = 0;
        public Dictionary<int, int> TileCount = new Dictionary<int, int>(), TempTileCount = new Dictionary<int, int>();
        public Dictionary<int, int> SpottedTileMemoryTime = new Dictionary<int, int>();
        public BitsByte TilePresent = new BitsByte();
        public bool HeartLanternNearby { get { return TilePresent[0]; } set { TilePresent[0] = value; } }
        public bool LastHeartLanternNearby { get { return TilePresent[1]; } set { TilePresent[1] = value; } }
        public bool StarLanternNearby { get { return TilePresent[2]; } set { TilePresent[2] = value; } }
        public bool LastStarLanternNearby { get { return TilePresent[3]; } set { TilePresent[3] = value; } }
        public bool FireplaceNearby { get { return TilePresent[4]; } set { TilePresent[4] = value; } }
        public bool LastFireplaceNearby { get { return TilePresent[5]; } set { TilePresent[5] = value; } }
        private Terraria.GameContent.UI.WorldUIAnchor anchor = new Terraria.GameContent.UI.WorldUIAnchor();
        private Terraria.GameContent.UI.EmoteBubble emote; 
        public const int FramesSquaredTileCount = 16; //8
        public float AirHeightBonus { get { return (float)Base.Mass / 0.3f; } }
        public byte WallSlideStyle = 0; //1 = Partial Sliding, 2 = Fully Stopped on the wall.
        public float CalculatedJumpHeight { get { return JumpSpeed * MaxJumpHeight; } }
        public bool SlidingLeft = false;
        public byte ImmuneAlpha = 0;
        public byte MeleeEnchant = 0;
        public bool IncreaseImmuneTime = true;
        public bool MagicMirrorTrigger = false;
        public bool HasMagicMirror { get { return HasItem(Terraria.ID.ItemID.MagicMirror) || HasItem(Terraria.ID.ItemID.IceMirror) || HasItem(Terraria.ID.ItemID.CellPhone); } }
        public bool IsBeingControlledByPlayer
        {
            get { return (PlayerMounted && !ReverseMount && !GuardianHasControlWhenMounted && !Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOut) || PlayerControl; }
        }
        public bool MountedOnPlayer
        {
            get { return (PlayerMounted && ReverseMount) || SittingOnPlayerMount; }
        }
        public bool ReverseMount
        {
            get { return (Base.ReverseMount || (Age < 15 && Base.GetGroup.ReverseMountWhenUnderaged)); }
        }
        public int CarriedByGuardianID
        {
            get
            {
                return _CarriedByGuardianID;
            }
            set
            {
                _CarriedByGuardianID = value;
                _InternalCarryTimer = 5;
            }
        }
        public bool BeingCarriedByGuardian = false;
        private byte _InternalCarryTimer = 0;
        private int _CarriedByGuardianID = -1;
        public bool SittingOnPlayerMount = false;
        public bool PlayerOutOfControl
        {
            get
            {
                return OwnerPos > -1 && (Main.player[OwnerPos].stoned || Main.player[OwnerPos].frozen || Main.player[OwnerPos].webbed);
            }
        }
        public bool CreatureAllowsAFK
        {
            get { return FlagList.Contains(GuardianFlags.StopMindingAfk); }
        }
        public string ReferenceName
        {
            get
            {
                string NewName = Name;
                if (!IsCommander && OwnerPos > -1)
                    NewName = Main.player[OwnerPos].name + "'s " + NewName;
                return NewName;
            }
        }
        public List<MessageScheduler> MessageSchedule = new List<MessageScheduler>();
        public string ChatMessage = "";
        public bool CanBeInterrupted = false;
        public bool DisplayMessageOnChat = false;
        public int MessageTime = 0;
        public List<GuardianFlags> FlagList = new List<GuardianFlags>();
        public Vector2 Position = Vector2.Zero, Velocity = Vector2.Zero;
        public Vector2 FeetPosition
        {
            get
            {
                if (UsingFurniture)
                {
                    return new Vector2(furniturex, furniturey) * 16;
                }
                return Position;
            }
        }
        public float CenterY { get { return Position.Y - Height * 0.5f * GravityDirection; } }
        public Vector2 PositionWithOffset
        {
            get
            {
                Vector2 p = Position;
                p.X += OffsetX;
                p.Y += OffsetY + Base.CharacterPositionYDiscount * Scale;
                if (mount.Active)
                {
                    p.Y -= mount.HeightBoost;
                    //p.X -= ((Base.SittingPoint.X - 8) * Scale - Width * 0.5f) * Direction;
                    p.Y -= ((Base.SittingPoint.Y + 8) * Scale - Height);
                }
                return p;
            }
        }
        public Vector2 OriginalPosition = Vector2.Zero;
        public float OffsetX = 0, OffsetY = 0;
        public float StepSpeed = 1f, gfxOffY = 0;
        public float Rotation = 0f, Scale = 1f;
        public float ScaleMult = 1f;
        public float FinalScale = 1f;
        public HitTile hitTileData
        {
            get
            {
                if (OwnerPos > -1)
                {
                    return Main.player[OwnerPos].hitTile;
                }
                else
                {
                    if (_hitTileData == null)
                        _hitTileData = new HitTile();
                    return _hitTileData;
                }
            }
        }
        public HitTile _hitTileData = null;
        public Vector2 TopLeftPosition
        {
            get
            {
                Vector2 NewPos = Position;
                NewPos.X -= Width * 0.5f;
                if(GravityDirection > 0)
                    NewPos.Y -= Height;
                return NewPos;
            }
            set
            {
                Vector2 NewPos = value;
                NewPos.X += Width * 0.5f;
                NewPos.Y += Height;
                Position = NewPos;
            }
        }
        public Vector2 CenterPosition
        {
            get
            {
                Vector2 NewPos = Position;
                NewPos.Y -= Height * 0.5f * GravityDirection;
                return NewPos;
            }
        }
        public Vector2 CollisionPosition
        {
            get
            {
                Vector2 NewPos = Position;
                if (MountedOnPlayer && OwnerPos > -1)
                {
                    NewPos = Main.player[OwnerPos].position;
                }
                else
                {
                    NewPos.X -= CollisionWidth * 0.5f;
                    if (GravityDirection > 0) NewPos.Y -= CollisionHeight * GravityDirection;
                }
                //if (GravityDirection < 0)
                //    NewPos.Y -= CollisionHeight;
                return NewPos;
            }
        }
        public Vector2 PositionDifference(Vector2 OtherCollision)
        {
            return OtherCollision - CollisionPosition;
        }
        public bool ExecutingAttackAnimation { get { return ItemAnimationTime > 0 || FreezeItemUseAnimation; } }
        public byte TurnLock = 0;
        public const byte TurnLockTime = 12;
        public Vector2 PrioritaryMovementTarget = Vector2.Zero;
        public PrioritaryBehavior PrioritaryBehaviorType = PrioritaryBehavior.None;
        public GuardianSpecialAttackData SpecialAttack = new GuardianSpecialAttackData(null);
        private Dictionary<ushort, float> SubAttackCooldown = new Dictionary<ushort, float>();
        public bool SubAttackInUse { get { return SpecialAttack.InUse; } }
        public int MyDrawOrder = 0; //For getting when the companion is drawn. The lower the number, the more behind the companion is drawn.
        public static int CurrentDrawnOrderID = 0;
        public short CommanderCharacterID = -1;
        public int GetCommanderLeaderID
        {
            get
            {
                if (CommanderCharacterID == -1)
                    return -1;
                return Main.player[CommanderCharacterID].GetModPlayer<PlayerMod>().CompanionCommanderLeaderPlayer;
            }
        }
        public bool IsCommander { get { return CommanderCharacterID > -1; } }
        public PlayerMod.CommandingOrders GetCommandingOrder
        {
            get
            {
                if (CommanderCharacterID > -1)
                {
                    return Main.player[CommanderCharacterID].GetModPlayer<PlayerMod>().CommandingOrder;
                }
                return PlayerMod.CommandingOrders.Defend;
            }
        }

        public bool SetAsCommander(int LeaderPlayerID)
        {
            if (CommanderCharacterID > -1 || LeaderPlayerID < 0 || LeaderPlayerID > 255)
                return false;
            for(int i = 254; i >= 0; i--)
            {
                if (!Main.player[i].active)
                {
                    if (OwnerPos > -1)
                        Main.player[OwnerPos].GetModPlayer<PlayerMod>().DismissGuardian(ID, ModID, false);
                    Main.player[i] = new Player() { whoAmI = i };
                    Player p = Main.player[i];
                    PlayerMod pm = p.GetModPlayer<PlayerMod>();
                    for (int j = 0; j < 50; j++)
                    {
                        p.inventory[j].SetDefaults(0);
                    }
                    /*for(int j = 0; j < 50; j++)
                    {
                        p.inventory[j] = Inventory[j];
                        if (j < 9)
                            p.armor[j] = Equipments[j];
                    }
                    p.dye[0] = BodyDye;*/
                    p.name = Name;
                    p.Center = CenterPosition;
                    p.active = true;
                    pm.CompanionCommanderLeaderPlayer = (short)LeaderPlayerID;
                    pm.GetSharedProgress(Main.player[Main.myPlayer].GetModPlayer<PlayerMod>());
                    pm.MyGuardians = Main.player[LeaderPlayerID].GetModPlayer<PlayerMod>().MyGuardians;
                    CommanderCharacterID = (short)i;
                    if(OwnerPos > -1)
                    {
                        if (PlayerMounted)
                            ToggleMount(true, false);
                        if (PlayerControl)
                            TogglePlayerControl(true);
                        if (SittingOnPlayerMount)
                            DoSitOnPlayerMount(false);
                        Main.player[OwnerPos].GetModPlayer<PlayerMod>().DismissGuardian();
                    }
                    pm.CallGuardian(ID, ModID);
                    return true;
                }
            }
            return false;
        }

        public void RemoveFromCommanding()
        {
            if (!IsCommander)
                return;
            Main.player[CommanderCharacterID].active = false;
            PlayerMod pm = Main.player[CommanderCharacterID].GetModPlayer<PlayerMod>();
            bool First = true;
            foreach (TerraGuardian tg in pm.GetAllGuardianFollowers)
            {
                if (tg.Active)
                {
                    pm.DismissGuardian(tg.ID, tg.ModID, !First);
                }
                First = false;
            }
            CommanderCharacterID = -1;
        }

        public bool IsSameAs(GuardianData guardian)
        {
            return IsSameAs(guardian.MyID);
        }

        public bool IsSameAs(TerraGuardian guardian)
        {
            return IsSameAs(guardian.MyID);
        }

        public bool IsSameAs(GuardianID GiD)
        {
            return IsSameAs(GiD.ID, GiD.ModID);
        }

        public bool IsSameAs(int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            if (MyID.IsSameID(ID, ModID))
            {
                return true;
            }
            foreach(GuardianID id in Base.IsSameAs)
            {
                if (id.IsSameID(ID, ModID))
                    return true;
            }
            return false;
        }

        public bool IsBeingCarriedByThisGuardian(TerraGuardian tg)
        {
            return tg.WhoAmID == CarriedByGuardianID;
        }

        public bool IsBeingCarriedBySomeone()
        {
            return CarriedByGuardianID != -1;
        }

        public void GetBirthday(out Season season, out int Day)
        {
            Data.GetBirthday(out season, out Day);
        }

        public class MessageScheduler
        {
            public string Message = "";
            public int MessageDelay = 0;

            public MessageScheduler(string Message, int Delay = 0)
            {
                this.Message = Message;
                MessageDelay = Delay;
            }
        }

        public void TryFindingTownNpcInfo()
        {
            GuardianTownNpcInfoPosition = -1;
            for (int t = 0; t < WorldMod.MaxGuardianNpcsInWorld; t++)
            {
                if (WorldMod.GuardianNPCsInWorld[t] != null && WorldMod.GuardianNPCsInWorld[t].CharID.IsSameID(Data))
                {
                    GuardianTownNpcInfoPosition = t;
                    return;
                }
            }
        }

        public Rectangle GetCollisionRectangle
        {
            get
            {
                Vector2 CollisionStart = CollisionPosition;
                return new Rectangle((int)CollisionStart.X, (int)CollisionStart.Y, CollisionWidth, CollisionHeight);
            }
        }

        public int CollisionWidth
        {
            get
            {
                if (MountedOnPlayer && OwnerPos > -1)
                {
                    return Main.player[OwnerPos].width;
                }
                else
                {
                    if (MainMod.TileCollisionIsSameAsHitCollision)
                    {
                        return Base.Width;
                    }
                    else
                    {
                        return 20;
                    }
                }
            }
        }
        public int CollisionHeight
        {
            get
            {
                if (MountedOnPlayer && OwnerPos > -1)
                {
                    return Main.player[OwnerPos].height;
                }
                else
                {
                    int height = (MainMod.TileCollisionIsSameAsHitCollision ? Base.Height : 42) - CollisionHeightDiscount;
                    if (mount.Active)
                        height += mount.HeightBoost;
                    return height;
                }
            }
        }
        private int CollisionHeightDiscount = 0;
        public int Direction
        {
            get
            {
                return (LookingLeft ? -1 : 1);
            }
            set
            {
                LookingLeft = value == -1;
            }
        }
        public int Width 
        {
            get { return (int)(Base.Width * Scale); }
        }
        public int Height
        {
            get
            {
                int Height = (int)((Ducking ? Base.DuckingHeight : Base.Height) * Scale);
                if (mount.Active)
                    Height += mount.HeightBoost;
                return Height;
            }
        }
        public int DuckingHeight
        {
            get
            {
                int Height = (int)(Base.DuckingHeight* Scale);
                if (mount.Active)
                    Height += mount.HeightBoost;
                return Height;
            }
        }
        public int GetHighestDamageMeleeWeaponPosition
        {
            get
            {
                int HighestDamageMelee = 0, WeaponPosition = -1;
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].modItem is Items.GuardianItemPrefab && this.Inventory[i].melee && this.Inventory[i].damage > HighestDamageMelee)
                    {
                        HighestDamageMelee = this.Inventory[i].damage;
                        WeaponPosition = i;
                        if (UseWeaponsByInventoryOrder)
                            break;
                    }
                }
                return WeaponPosition;
            }
        }
        public int GetMeleeWeaponRangeX(int ItemPosition = -1, bool Kneeling = false)
        {
            int Range = 40;
            if (ItemPosition > -1)
            {
                Terraria.ModLoader.Config.ItemDefinition def = new Terraria.ModLoader.Config.ItemDefinition(Inventory[ItemPosition].type);
                if (MainMod.ItemAttackRange.ContainsKey(def))
                {
                    Range = MainMod.ItemAttackRange[def];
                }
                else if (Main.netMode < 2 && !MainMod.IsGuardianItem(this.Inventory[ItemPosition]))
                {
                    if (Main.itemTexture[this.Inventory[ItemPosition].type].Height >= Main.itemTexture[this.Inventory[ItemPosition].type].Width)
                        Range = Main.itemTexture[this.Inventory[ItemPosition].type].Height;
                    else
                        Range = Main.itemTexture[this.Inventory[ItemPosition].type].Width;
                }
                else
                {
                    if (this.Inventory[ItemPosition].height >= this.Inventory[ItemPosition].width)
                        Range = this.Inventory[ItemPosition].height;
                    else
                        Range = this.Inventory[ItemPosition].width;
                }
                Range = (int)(Range * Inventory[ItemPosition].scale);
            }
            {
                int AttackRangeX = 0;
                int y;
                if ((ItemPosition == -1 && !Base.DontUseHeavyWeapons) || (ItemPosition > -1 && MainMod.IsGuardianItem(Inventory[ItemPosition])))
                {
                    GetBetweenHandsPosition(Base.HeavySwingFrames[0], true, out AttackRangeX, out y);
                }
                else
                {
                    GetLeftHandPosition((Kneeling && Base.CanDuck) ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[2], true, out AttackRangeX, out y);
                }
                Range += (int)(AttackRangeX * Scale);
            }
            return Range;
        }
        public void GetMeleeWeaponRangeY(int ItemPosition, out float RangeYUpper, out float RangeYLower, bool Kneeling = false)
        {
            RangeYLower = 40;
            RangeYUpper = 40;
            if (ItemPosition > -1)
            {
                Terraria.ModLoader.Config.ItemDefinition def = new Terraria.ModLoader.Config.ItemDefinition(Inventory[ItemPosition].type);
                if (MainMod.ItemAttackRange.ContainsKey(def))
                {
                    RangeYLower = MainMod.ItemAttackRange[def];
                }
                else if (Main.netMode < 2 && !MainMod.IsGuardianItem(this.Inventory[ItemPosition]))
                {
                    if (Main.itemTexture[this.Inventory[ItemPosition].type].Height >= Main.itemTexture[this.Inventory[ItemPosition].type].Width)
                    {
                        RangeYLower = Main.itemTexture[this.Inventory[ItemPosition].type].Height;
                    }
                    else
                    {
                        RangeYLower = Main.itemTexture[this.Inventory[ItemPosition].type].Width;
                    }
                }
                else
                {
                    if (this.Inventory[ItemPosition].height >= this.Inventory[ItemPosition].width)
                        RangeYLower = this.Inventory[ItemPosition].height;
                    else
                        RangeYLower = this.Inventory[ItemPosition].width;
                }
                RangeYLower = (int)(RangeYLower * Inventory[ItemPosition].scale);
            }
            RangeYUpper = -RangeYLower;
            RangeYLower *= 0.5f;
            {
                int AttackRangeYUpper = 0, AttackRangeYLower = 0;
                int x;
                if ((ItemPosition == -1 && !Base.DontUseHeavyWeapons) || (ItemPosition > -1 && MainMod.IsGuardianItem(Inventory[ItemPosition])))
                {
                    GetBetweenHandsPosition(Base.HeavySwingFrames[0], out x, out AttackRangeYUpper);
                }
                else
                {
                    GetLeftHandPosition((Kneeling && Base.CanDuck) ? Base.DuckingSwingFrames[0] : Base.ItemUseFrames[0], out x, out AttackRangeYUpper);
                }
                //AttackRangeYUpper = -SpriteHeight + AttackRangeYUpper;
                RangeYUpper += (int)(AttackRangeYUpper * Scale);
                if ((ItemPosition == -1 && !Base.DontUseHeavyWeapons) || (ItemPosition > -1 && MainMod.IsGuardianItem(Inventory[ItemPosition])))
                {
                    GetBetweenHandsPosition(Base.HeavySwingFrames[2], out x, out AttackRangeYLower);
                }
                else
                {
                    GetLeftHandPosition((Kneeling && Base.CanDuck) ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[3], out x, out AttackRangeYLower);
                }
                //AttackRangeYLower = -SpriteHeight + AttackRangeYLower;
                RangeYLower += (int)(AttackRangeYLower * Scale);
            }
        }
        public Rectangle HitBox = Rectangle.Empty;
        public int SpriteWidth { get { return Base.SpriteWidth; } }
        public int SpriteHeight { get { return Base.SpriteHeight; } }
        public int HP { get { return Data.HP; } set { Data.HP = value; } }
        public int MHP { get { return Data.MHP; } set { Data.MHP = value; } }
        public int MP { get { return Data.MP; } set { Data.MP = value; } }
        public int MMP { get { return Data.MMP; } set { Data.MMP = value; } }
        public bool ExtraAccessorySlot {
            get
            {
                if (OwnerPos > -1 && Main.player[OwnerPos].extraAccessory)
                    return true;
                if (OwnerPos == -1 && Main.expertMode && Main.hardMode)
                    return true;
                return false;
            }
        }
        public float HealthHealMult { get { return Data.HealthHealMult; } set { Data.HealthHealMult = value; } }
        public float ManaHealMult { get { return Data.ManaHealMult; } set { Data.ManaHealMult = value; } }
        public int Damage = 20, Defense = 0;
        public int MeleeCriticalRate = 0, RangedCriticalRate = 0, MagicCriticalRate = 0;
        public float MeleeSpeed = 1f, RangedSpeed = 1f, MagicSpeed = 1f;
        public float BlockRate = 0, DodgeRate = 0, CoverRate = 0;
        public float DefenseRate = 0;
        public bool LookingLeft = false;
        public int GravityDirection = 1;
        public bool Wet = false, LavaWet = false, HoneyWet = false, Drowning = false;
        public bool NoGravity = false;
        public bool Falling = false;
        public float Mass = 0.5f, MaxSpeed = 4.5f, Acceleration = 0.1f, SlowDown = 0.3f, MoveSpeed = 1f; //Mass = Gravity Strength
        public const float WalkMaxSpeed = 1f,
            WalkSlowDown = 0.1f,
            WalkAcceleration = 0.07f;
        public float DashSpeed = 0f;
        public int DashCooldown = 0;
        public byte RocketType = 0;
        public const float DefaultMaxFallSpeed = 10f;
        public float AnimationTime = 0f;
        public int BodyAnimationFrame = 0, LeftArmAnimationFrame = 0, RightArmAnimationFrame = 0;
        public int MaxJumpHeight = 15;
        public int FallHeightTolerance = 15;
        public float MaxFallSpeed = DefaultMaxFallSpeed;
        public float JumpSpeed = 7.08f;
        public float JumpHeight = 0;
        public bool IgnoreMass = false;
        public bool WalkMode = false;
        public float JumpUntilHeight = -1;
        public int ExtraJump = 0;
        public int ItemAnimationTime = 0, ItemMaxAnimationTime = 0, ItemUseTime = 0, ItemMaxUseTime = 0, ToolUseTime = 0;
        public bool IsDelay = false;
        public int ItemPositionX = 0, ItemPositionY = 0;
        public int OffHandPositionX = 0, OffHandPositionY = 0;
        private sbyte OffhandOrientation = 0;
        public HeldHand HeldItemHand = HeldHand.Both, HeldOffHand = HeldHand.Right;
        public float ItemRotation = 0f, ItemScale = 1f;
        public byte ArmOrientation = 0;
        public int ImmuneTime = 0;
        public bool ImmuneNoBlink = false;
        public bool OnImmuneTime { get { return ImmuneTime > 0; } }
        public const int DefaultImmuneTime = 40;
        private List<int> PlayerHit = new List<int>(),
            NpcHit = new List<int>();
        private int doorx = -1, doory = -1;
        public bool HasDoorOpened { get { return closeDoor; } }
        public int furniturex = -1, furniturey = -1;
        public bool UsingFurniture = false, SittingOnBed = false, AttemptedToPathFindFurniture = false;
        private bool closeDoor = false;
        private bool CollisionX = false, CollisionY = false;
        public bool Downed = false;
        public bool KnockedOut { get { return Data.KnockedOut; } set { Data.KnockedOut = value; } }
        public bool KnockedOutCold { get { return Data.KnockedOutCold; } set { Data.KnockedOutCold = value; } }
        public bool WofFood { get { return Data.WofFood; } set { Data.WofFood = value; } }
        public bool FriendlyDuelDefeat = false;
        public byte ReviveBoost = 0;
        public float ReviveStack = 0f;
        public const int MaxReviveStack = 90, MinReviveStack = -90; //Was 150
        public bool NegativeReviveBoost = false;
        public int WakeupTime = 0;
        public float KnockdownRotation = 0f;
        public int SeekingItem = -1;
        public int TargetID = -1;
        public bool AttackingTarget = false;
        public bool IsAttackingSomething { get { return TargetID > -1 && AttackingTarget; } }
        public bool TargetInAim = false;
        public TargetTypes TargetType = TargetTypes.Npc;
        public Item[] Equipments { get { return Data.Equipments; } set { Data.Equipments = value; } }
        public Item[] Inventory { get { return Data.Inventory; } set { Data.Inventory = value; } }
        public GuardianItemSlotFlag[] InventorySlotFlags { get { return Data.InventorySlotFlags; } set { Data.InventorySlotFlags = value; } }
        public Item BodyDye { get { return Data.BodyDye; } set { Data.BodyDye = value; } }
        public int SelectedItem = 0, SelectedOffhand = -1, LastSelectedItem = -2, LastSelectedOffhand = -2;
        public int LastUsedSummonItem = 0;
        public float OffhandRotation = 0f;
        public ItemUseTypes ItemUseType = ItemUseTypes.HeavyVerticalSwing;
        public float MeleeDamageMultiplier = 1, RangedDamageMultiplier = 1, MagicDamageMultiplier = 1, SummonDamageMultiplier = 1, NeutralDamageMultiplier = 1f;
        public float MeleeKnockback = 1f, RangedKnockback = 1f;
        public float ShotSpeedMult = 1f;
        public float ManaCostMult = 1f;
        public int NumMinions = 0, MaxMinions = 1;
        public int NumSentries = 0, MaxSentries = 1;
        public float MinionSlotCount = 0;
        public int OwnerPos = -1;
        public int RefPlayer { get { if (OwnerPos > -1) { return OwnerPos; } return 0; } }
        public byte GetUsedLifeCrystal
        {
            get
            {
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                    return GetCommonStatus.LifeCrystalsUsed;
                return Data.LifeCrystalHealth;
            }
            set
            {
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                {
                    GetCommonStatus.LifeCrystalsUsed = value;
                }
                else
                {
                    Data.LifeCrystalHealth = value;
                }
            }
        }
        public byte GetUsedLifeFruit
        {
            get
            {
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                    return GetCommonStatus.LifeFruitsUsed;
                return Data.LifeFruitHealth;
            }
            set
            {
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                {
                    GetCommonStatus.LifeFruitsUsed = value;
                }
                else
                    Data.LifeFruitHealth = value;
            }
        }
        public byte GetUsedManaCrystal
        {
            get
            {
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                    return GetCommonStatus.ManaCrystalsUsed;
                return Data.ManaCrystals;
            }
            set
            {
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                {
                    GetCommonStatus.ManaCrystalsUsed = value;
                }
                else
                    Data.ManaCrystals = value;
            }
        }
        public byte LifeCrystalHealth
        {
            get
            {
                if (MainMod.SharedCrystalValues && !Main.gameMenu)
                {
                    if (OwnerPos > -1)
                    {
                        return Main.player[OwnerPos].GetModPlayer<PlayerMod>().LifeCrystalsUsed;
                    }
                    else if (Main.netMode == 0)
                    {
                        return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().LifeCrystalsUsed;
                    }
                }
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                {
                    return GetCommonStatus.LifeCrystalsUsed;
                }
                return Data.LifeCrystalHealth;
            }
        }
        public byte LifeFruitHealth
        {
            get
            {
                if (MainMod.SharedCrystalValues && !Main.gameMenu)
                {
                    if (OwnerPos > -1)
                    {
                        return Main.player[OwnerPos].GetModPlayer<PlayerMod>().LifeFruitsUsed;
                    }
                    else if (Main.netMode == 0)
                    {
                        return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().LifeFruitsUsed;
                    }
                }
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                {
                    return GetCommonStatus.LifeFruitsUsed;
                }
                return Data.LifeFruitHealth;
            }
        }

        public byte ManaCrystals
        {
            get
            {
                if (MainMod.SharedCrystalValues && !Main.gameMenu)
                {
                    if (OwnerPos > -1)
                        return Main.player[OwnerPos].GetModPlayer<PlayerMod>().ManaCrystalsUsed;
                    else if (Main.netMode == 0)
                        return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ManaCrystalsUsed;
                }
                if (GuardianCommonStatus.UseMaxHealthAndManaShare)
                {
                    return GetCommonStatus.ManaCrystalsUsed;
                }
                return Data.ManaCrystals;
            }
        }
        public const int MaxLifeCrystals = 15, MaxLifeFruit = 20; //-
        public bool UpdateStatus = true, UpdateWeapons = true;
        public int HealthRegenPower = 0, HealthRegenTime = 0, ManaRegenTime = 0, ManaRegenBonus = 0;
        public int MaxRegenDelay { get { return (int)(((1f - (float)MP / MMP) * 60 * 4 + 45) * 0.7f); } }
        public Vector2 AimDirection = Vector2.Zero;
        public Vector2 AimPosition { get { return CenterPosition + AimDirection; } set{ AimDirection = value + CenterPosition; } }
        public int StuckTimer = 0;
        public CombatTactic tactic { get { return Data.tactic; } set { Data.tactic = value; } }
        private const byte Melee = 0, Ranged = 1, Magic = 2, Summon = 3;
        public static bool StuckTimerChanged = false;
        public byte FriendshipLevel { get { return Data.FriendshipLevel; } set { Data.FriendshipLevel = value; } }
        public byte FriendshipProgression { get { return Data.FriendshipProgression; } set { Data.FriendshipProgression = value; } }
        public byte FriendshipGrade
        {
            get
            {
                return Data.FriendshipGrade;
            }
        }
        public string FriendshipGradeText
        {
            get
            {
                string Text = "Just Met";
                if (FriendshipLevel >= Base.KnownLevel)
                    Text = "Known";
                if (FriendshipLevel >= Base.FriendsLevel)
                    Text = "Friends";
                if (FriendshipLevel >= Base.BestFriendLevel)
                    Text = "Best Friends";
                if (FriendshipLevel >= Base.BestFriendForeverLevel)
                    Text = "BFF";
                if (FriendshipLevel >= Base.BuddiesForLife)
                    Text = "Buddies for Life";
                return Text;
            }
        }
        //For the display.
        public byte LastFriendshipLevel = 0, LastFriendshipValue = 0;
        public float FriendshipHeartDisplayTime = 0f;
        public float StuckCheckX = 0;
        public float TravellingStacker { get { return Data.TravellingStacker; } set { Data.TravellingStacker = value; } }
        public float DamageStacker { get { return Data.DamageStacker; } set { Data.DamageStacker = value; } }
        public byte FoodStacker { get { return Data.FoodStacker; } set { Data.FoodStacker = value; } }
        public byte DrinkStacker { get { return Data.DrinkStacker; } set { Data.DrinkStacker = value; } }
        public const int MaxComfortStack = 10 * 60;
        public float ComfortStack { get { return Data.ComfortStack; } set { Data.ComfortStack = value; } }
        public byte MaxComfortExp { get { return checked((byte)(5 + FriendshipLevel / 3)); } }
        public byte ComfortPoints { get { return Data.ComfortPoints; } set { Data.ComfortPoints = value; } }
        public Emotions CurrentEmotion = Emotions.Neutral;
        public float EmotionDisplayTime = 0f;
        public const float MaxEmotionDisplaytime = 3f;
        public RequestData request { get { return Data.request; } set { Data.request = value; } }
        public const byte MaxFriendshipLevel = 100;
        public const int MaxHealingPotionCooldown = 60 * 60;
        public int FallStart = 0;
        public int Breath = 200, BreathCooldown = 0, BreathMax = 200;
        public ushort AfkCounter = 0, IdleCounter = 0;
        public int IdlePosition = 0;
        public bool GrabbingPlayer = false, PlayerCanEscapeGrab = true, ProtectingPlayerFromHarm = false;
        public bool WofFacing
        {
            get { return HasBuff(Terraria.ID.BuffID.Horrified); }
            set { if (value) { AddBuff(Terraria.ID.BuffID.Horrified, 5, true); } else { RemoveBuff(Terraria.ID.BuffID.Horrified); } }
        }
        public bool WofTongued
        {
            get { return HasBuff(Terraria.ID.BuffID.TheTongue); }
            set { if (value) { AddBuff(Terraria.ID.BuffID.TheTongue, 5, true); } else { RemoveBuff(Terraria.ID.BuffID.TheTongue); } }
        }
        public GuardianActions DoAction = new GuardianActions() { InUse = false };
        public bool BehindWall = false;
        public BitsByte Zone1 = new BitsByte(), Zone2 = new BitsByte(), Zone3 = new BitsByte(), Zone4 = new BitsByte();
        private bool SunflowerNearby = false, LifeCrystalChainNearby = false;
        public GuardianMount mount = new GuardianMount();
        public int ActivityCount = 0;

        public bool ZoneDungeon
        {
            get
            {
                return this.Zone1[0];
            }
            set
            {
                this.Zone1[0] = value;
            }
        }

        public bool ZoneCorrupt
        {
            get
            {
                return this.Zone1[1];
            }
            set
            {
                this.Zone1[1] = value;
            }
        }

        public bool ZoneHoly
        {
            get
            {
                return this.Zone1[2];
            }
            set
            {
                this.Zone1[2] = value;
            }
        }

        public bool ZoneMeteor
        {
            get
            {
                return this.Zone1[3];
            }
            set
            {
                this.Zone1[3] = value;
            }
        }

        public bool ZoneJungle
        {
            get
            {
                return this.Zone1[4];
            }
            set
            {
                this.Zone1[4] = value;
            }
        }

        public bool ZoneSnow
        {
            get
            {
                return this.Zone1[5];
            }
            set
            {
                this.Zone1[5] = value;
            }
        }

        public bool ZoneCrimson
        {
            get
            {
                return this.Zone1[6];
            }
            set
            {
                this.Zone1[6] = value;
            }
        }

        public bool ZoneWaterCandle
        {
            get
            {
                return this.Zone1[7];
            }
            set
            {
                this.Zone1[7] = value;
            }
        }

        public bool ZonePeaceCandle
        {
            get
            {
                return this.Zone2[0];
            }
            set
            {
                this.Zone2[0] = value;
            }
        }

        public bool AnyTower
        {
            get
            {
                return ZoneTowerNebula || ZoneTowerSolar || ZoneTowerStardust || ZoneTowerVortex;
            }
        }

        public bool ZoneTowerSolar
        {
            get
            {
                return this.Zone2[1];
            }
            set
            {
                this.Zone2[1] = value;
            }
        }

        public bool ZoneTowerVortex
        {
            get
            {
                return this.Zone2[2];
            }
            set
            {
                this.Zone2[2] = value;
            }
        }

        public bool ZoneTowerNebula
        {
            get
            {
                return this.Zone2[3];
            }
            set
            {
                this.Zone2[3] = value;
            }
        }

        public bool ZoneTowerStardust
        {
            get
            {
                return this.Zone2[4];
            }
            set
            {
                this.Zone2[4] = value;
            }
        }

        public bool ZoneDesert
        {
            get
            {
                return this.Zone2[5];
            }
            set
            {
                this.Zone2[5] = value;
            }
        }

        public bool ZoneGlowshroom
        {
            get
            {
                return this.Zone2[6];
            }
            set
            {
                this.Zone2[6] = value;
            }
        }

        public bool ZoneUndergroundDesert
        {
            get
            {
                return this.Zone2[7];
            }
            set
            {
                this.Zone2[7] = value;
            }
        }

        public bool ZoneSkyHeight
        {
            get
            {
                return this.Zone3[0];
            }
            set
            {
                this.Zone3[0] = value;
            }
        }

        public bool ZoneOverworldHeight
        {
            get
            {
                return this.Zone3[1];
            }
            set
            {
                this.Zone3[1] = value;
            }
        }

        public bool ZoneDirtLayerHeight
        {
            get
            {
                return this.Zone3[2];
            }
            set
            {
                this.Zone3[2] = value;
            }
        }

        public bool ZoneRockLayerHeight
        {
            get
            {
                return this.Zone3[3];
            }
            set
            {
                this.Zone3[3] = value;
            }
        }

        public bool ZoneUnderworldHeight
        {
            get
            {
                return this.Zone3[4];
            }
            set
            {
                this.Zone3[4] = value;
            }
        }

        public bool ZoneBeach
        {
            get
            {
                return this.Zone3[5];
            }
            set
            {
                this.Zone3[5] = value;
            }
        }

        public bool ZoneRain
        {
            get
            {
                return this.Zone3[6];
            }
            set
            {
                this.Zone3[6] = value;
            }
        }

        public bool ZoneSandstorm
        {
            get
            {
                return this.Zone3[7];
            }
            set
            {
                this.Zone3[7] = value;
            }
        }

        public bool ZoneOldOneArmy
        {
            get
            {
                return this.Zone4[0];
            }
            set
            {
                this.Zone4[0] = value;
            }
        }

        public int GetImmuneTime
        {
            get
            {
                return DefaultImmuneTime;
            }
        }

        public Vector2 GetGuardianShoulderPosition
        {
            get
            {
                Vector2 Pos = AnimationPositionValueTranslator(Base.MountShoulderPoints.GetPositionFromFramePoint(BodyAnimationFrame).ToVector2() * Scale);
                /*if (LookingLeft) Pos.X = SpriteWidth - Pos.X;
                Pos.X -= SpriteWidth * 0.5f;
                Pos.Y -= SpriteHeight;
                Pos *= Scale;*/
                Pos += PositionWithOffset;
                return Pos;
            }
        }

        public Vector2 GetGuardianBetweenHandPosition
        {
            get
            {
                return GetBetweenHandsPosition(LeftArmAnimationFrame) + PositionWithOffset;
            }
        }

        public Vector2 GetGuardianLeftHandPosition
        {
            get
            {
                return GetLeftHandPosition(LeftArmAnimationFrame) + PositionWithOffset;
            }
        }

        public Vector2 GetGuardianRightHandPosition
        {
            get
            {
                return GetRightHandPosition(RightArmAnimationFrame) + PositionWithOffset;
            }
        }

        public TerraGuardian(int CreateByType = -1, string ModID = "")
        {
            if (CreateByType != -1)
            {
                if (ModID == "")
                    ModID = MainMod.mod.Name;
                if(Main.netMode == 0 && PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], CreateByType, ModID))
                {
                    _Data = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], CreateByType, ModID);
                }
                else//if(_Data == null)
                    _Data = GuardianBase.GetGuardianBase(CreateByType, ModID).GetGuardianData(CreateByType, ModID);
                TryFindingTownNpcInfo();
                Data.UpdateAge();
            }
            else
            {
                _Data = new GuardianData();
            }
            AddCooldown(GuardianCooldownManager.CooldownType.SpottingCooldown, 60);
            if (CreateByType > -1)
                WhoAmID = IDStack++;
        }

        public TerraGuardian(GuardianData gd)
        {
            Data = gd;
            AddCooldown(GuardianCooldownManager.CooldownType.SpottingCooldown, 60);
            TryFindingTownNpcInfo();
            Data.UpdateAge();
            WhoAmID = IDStack++;
        }

        public bool InPerceptionRange(Vector2 Position, float DistanceBonus = 1f)
        {
            return Position.X >= this.Position.X - 576f * DistanceBonus && Position.X < this.Position.X + 576f * DistanceBonus &&
                Position.Y >= this.Position.Y - Height * 0.5f - 256f * DistanceBonus && Position.Y < this.Position.Y - Height * 0.5f + 256f * DistanceBonus;
        }

        public Vector2 AnimationPositionValueTranslator(Vector2 Pos)
        {
            Pos.X -= SpriteWidth * Scale * 0.5f;
            if (LookingLeft)
                Pos.X *= -1;
            if (GravityDirection > 0)
                Pos.Y -= SpriteHeight * GravityDirection * Scale;
            //Pos *= Scale;
            return Pos;
        }

        public void DisplayEmotion(Emotions emotion)
        {
            if (EmotionDisplayTime <= 0 || CurrentEmotion != emotion)
            {
                CurrentEmotion = emotion;
                EmotionDisplayTime = MaxEmotionDisplaytime;
            }
        }

        public void ClearMessagesSaid()
        {
            MessageTime = 0;
            MessageSchedule.Clear();
        }

        public int SaySomething(string[] Message, bool ChatDisplay = false, bool CanInterrupt = false)
        {
            return SaySomething(Message.ToList(), ChatDisplay);
        }

        public int SaySomething(List<string> Message, bool ChatDisplay = false, bool CanInterrupt = false)
        {
            if (!CanBeInterrupted && MessageTime > 0)
                return MessageTime;
            CanBeInterrupted = true;
            ChatMessage = Message[0];
            Message.RemoveAt(0);
            foreach(string mes in Message)
            {
                MessageScheduler m = new MessageScheduler(mes, 30);
                MessageSchedule.Add(m);
            }
            DisplayMessageOnChat = ChatDisplay;
            MessageTime = MainMod.CalculateMessageTime(ChatMessage);
            if (ChatDisplay)
            {
                Main.NewText(Name + ": " + Message);
            }
            return MessageTime;
        }

        public int SaySomethingCanSchedule(string Message, bool ChatDisplay = false, int DelayUntilSaying = 0)
        {
            CanBeInterrupted = false;
            DisplayMessageOnChat = ChatDisplay;
            if (MessageTime > 0 || DelayUntilSaying > 0)
            {
                MessageSchedule.Add(new MessageScheduler(Message, DelayUntilSaying));
                return MessageTime + MainMod.CalculateMessageTime(Message);
            }
            else
            {
                return SaySomething(Message, ChatDisplay);
            }
        }

        public int SaySomething(string Message, bool ChatDisplay = false)
        {
            if (!CanBeInterrupted && MessageTime > 0)
                return MessageTime;
            CanBeInterrupted = true;
            this.ChatMessage = Message;
            DisplayMessageOnChat = ChatDisplay;
            if (ChatDisplay)
            {
                Main.NewText(Name + ": " + Message);
            }
            MessageTime = MainMod.CalculateMessageTime(Message);
            return MessageTime;
        }

        public void SetTargetNpc(NPC n)
        {
            if (TargetID == -1)
            {
                TargetID = n.whoAmI;
                TargetType = TargetTypes.Npc;
            }
        }

        public void SetTargetPlayer(Player p)
        {
            if (TargetID == -1)
            {
                TargetID = p.whoAmI;
                TargetType = TargetTypes.Player;
            }
        }

        public byte MaxFriendshipProgression
        {
            get
            {
                byte Progress = 1;
                if (FriendshipLevel > 0)
                    Progress *= 2;
                if (FriendshipLevel > 1)
                {
                    Progress += (byte)(FriendshipLevel / 2);
                }
                return Progress;
            }
        }

        public void AddCooldown(GuardianCooldownManager.CooldownType type, int CooldownTime)
        {
            if (Cooldowns.Any(x => x.type == type))
            {
                Cooldowns.First(x => x.type == type).CooldownTime = CooldownTime;
            }
            else
            {
                Cooldowns.Add(new GuardianCooldownManager() { type = type, CooldownTime = CooldownTime });
            }
        }

        public void IncreaseCooldownValue(GuardianCooldownManager.CooldownType type, int Value = 1)
        {
            if (Cooldowns.Any(x => x.type == type))
            {
                Cooldowns.First(x => x.type == type).CooldownTime += Value;
            }
            else
            {
                AddCooldown(type, Value);
            }
            CooldownException.Add(type);
        }

        public void DecreaseCooldownValue(GuardianCooldownManager.CooldownType type, int Value = 1)
        {
            if (Cooldowns.Any(x => x.type == type))
            {
                GuardianCooldownManager c = Cooldowns.First(x => x.type == type);
                c.CooldownTime -= Value;
                if (c.CooldownTime <= 0)
                    Cooldowns.Remove(c);
            }
        }

        public bool HasCooldown(GuardianCooldownManager.CooldownType type)
        {
            return Cooldowns.Any(x => x.type == type);
        }

        public void RemoveCooldown(GuardianCooldownManager.CooldownType type)
        {
            if (Cooldowns.Any(x => x.type == type))
            {
                for (int c = 0; c < Cooldowns.Count; c++)
                {
                    if (Cooldowns[c].type == type)
                        Cooldowns.RemoveAt(c);
                }
            }
        }

        public int GetCooldownValue(GuardianCooldownManager.CooldownType type)
        {
            if (Cooldowns.Any(x => x.type == type))
                return Cooldowns.First(x => x.type == type).CooldownTime;
            return 0;
        }

        public void SetCooldownValue(GuardianCooldownManager.CooldownType type, int Value)
        {
            AddCooldown(type, Value);
        }

        public bool HasNpcBeenSpotted(int NpcID)
        {
            foreach(byte b in NpcsSpotted)
            {
                if (Main.npc[b].active && Main.npc[b].type == NpcID)
                    return true;
            }
            return false;
        }

        public void BuffCommentary(int ID)
        {
            if (HasCooldown(GuardianCooldownManager.CooldownType.BuffCommentCooldown) || Downed || KnockedOut)
                return;
            string Mes = "";
            bool IsFoodBuff = false;
            switch (ID)
            {
                case Terraria.ID.BuffID.Poisoned:
                case Terraria.ID.BuffID.Venom:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredPoisonedDebuff);
                    break;
                case Terraria.ID.BuffID.OnFire:
                case Terraria.ID.BuffID.CursedInferno:
                case Terraria.ID.BuffID.Frostburn:
                case Terraria.ID.BuffID.ShadowFlame:
                case Terraria.ID.BuffID.Burning:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredBurningDebuff);
                    break;
                case Terraria.ID.BuffID.Darkness:
                case Terraria.ID.BuffID.Blackout:
                case Terraria.ID.BuffID.Obstructed:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredDarknessDebuff);
                    break;
                case Terraria.ID.BuffID.Confused:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredConfusedDebuff);
                    break;
                case Terraria.ID.BuffID.Cursed:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredCursedDebuff);
                    break;
                case Terraria.ID.BuffID.Slow:
                case Terraria.ID.BuffID.OgreSpit:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredSlowDebuff);
                    break;
                case Terraria.ID.BuffID.Weak:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredWeakDebuff);
                    break;
                case Terraria.ID.BuffID.BrokenArmor:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredBrokenArmorDebuff);
                    break;
                case Terraria.ID.BuffID.Horrified:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredHorrifiedDebuff);
                    break;
                case Terraria.ID.BuffID.Ichor:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredIchorDebuff);
                    break;
                case Terraria.ID.BuffID.Chilled:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredChilledDebuff);
                    break;
                case Terraria.ID.BuffID.Webbed:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredWebbedDebuff);
                    break;
                case Terraria.ID.BuffID.Rabies:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredFeralBiteDebuff);
                    break;
                //
                case Terraria.ID.BuffID.Endurance:
                case Terraria.ID.BuffID.Ironskin:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredDefenseBuff);
                    break;
                case Terraria.ID.BuffID.WellFed:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredWellFedBuff);
                    IsFoodBuff = true;
                    break;
                case Terraria.ID.BuffID.Wrath:
                case Terraria.ID.BuffID.Archery:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredDamageBuff);
                    break;
                case Terraria.ID.BuffID.Swiftness:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredSpeedBuff);
                    break;
                case Terraria.ID.BuffID.Lifeforce:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredHealthIncreaseBuff);
                    break;
                case Terraria.ID.BuffID.Rage:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredCriticalBuff);
                    break;
                case Terraria.ID.BuffID.Honey:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredHoneyBuff);
                    break;
                case Terraria.ID.BuffID.WeaponImbueConfetti:
                case Terraria.ID.BuffID.WeaponImbueCursedFlames:
                case Terraria.ID.BuffID.WeaponImbueFire:
                case Terraria.ID.BuffID.WeaponImbueGold:
                case Terraria.ID.BuffID.WeaponImbueIchor:
                case Terraria.ID.BuffID.WeaponImbueNanites:
                case Terraria.ID.BuffID.WeaponImbuePoison:
                case Terraria.ID.BuffID.WeaponImbueVenom:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredMeleeWeaponBuff);
                    break;
                case Terraria.ID.BuffID.Tipsy:
                    Mes = GetMessage(GuardianBase.MessageIDs.AcquiredTipsyDebuff);
                    IsFoodBuff = true;
                    break;
            }
            if (Mes != "" && (IsFoodBuff || Main.rand.Next(3) == 0)) {
                SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Mes, this));
                if (!IsFoodBuff)
                {
                    AddCooldown(GuardianCooldownManager.CooldownType.BuffCommentCooldown, Main.rand.Next(45 * 60, 80 * 60));
                }
            }
        }

        public void AddBuff(int ID, int Time, bool NoSync = false)
        {
            if (ID < 1)
                return;
            if (BuffImmunity.Contains(ID)) return;
            foreach (BuffData b in Buffs)
            {
                if (b.ID == ID)
                {
                    if (ID == 94)
                    {
                        b.Time += Time;
                        if (b.Time > Player.manaSickTimeMax)
                            b.Time = Player.manaSickTimeMax;
                    }
                    else
                    {
                        b.Time = Time;
                    }
                    return;
                }
            }
            Buffs.Add(new BuffData(ID, Time));
            TriggerHandler.FireGuardianBuffAcquiredTrigger(CenterPosition, this, ID);
            BuffCommentary(ID);
            UpdateStatus = true;
            if (!NoSync && (OwnerPos == Main.myPlayer || (OwnerPos == -1 && Main.netMode == 2)))
                Netplay.SendGuardianBuffUpdate(WhoAmID, ID, Time);
        }

        public bool HasBuff(int ID)
        {
            for (int b = 0; b < Buffs.Count; b++)
            {
                if (Buffs[b].ID == ID)
                    return true;
            }
            return false;
        }

        public BuffData GetBuff(int ID)
        {
            for (int b = 0; b < Buffs.Count; b++)
            {
                if (Buffs[b].ID == ID)
                    return Buffs[b];
            }
            return null;
        }

        public void RemoveBuff(int ID)
        {
            for (int b = 0; b < Buffs.Count; b++)
            {
                if (Buffs[b].ID == ID)
                {
                    Buffs.RemoveAt(b);
                    TriggerHandler.FireGuardianBuffRemovedTrigger(CenterPosition, this, ID);
                    return;
                }
            }
        }

        public void PlayAppearDisappearEffect()
        {
            for(int i = 0; i < 40; i++)
            {
                Dust.NewDust(TopLeftPosition, Width, Height, 43, Main.rand.Next(-10, 11) * 0.01f);
            }
            Main.PlaySound(3, CenterPosition, 5);
        }

        public bool CanApplyFlag(GuardianFlags type)
        {
            bool Has = FlagList.Contains(type);
            if (!Has) FlagList.Add(type);
            return Has;
        }

        public bool HasFlag(GuardianFlags type)
        {
            return FlagList.Contains(type);
        }

        public bool CCed
        {
            get { return FlagList.Contains(GuardianFlags.Petrified) || FlagList.Contains(GuardianFlags.Frozen); }
        }

        public void RemoveFlag(GuardianFlags type)
        {
            if (FlagList.Contains(type)) FlagList.Remove(type);
        }

        public void AddFlag(GuardianFlags type)
        {
            if (!FlagList.Contains(type))
                FlagList.Add(type);
        }

        public bool HasMeleeWeapon
        {
            get
            {
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].type > 0 && CanUseItem(this.Inventory[i]))
                    {
                        if (this.Inventory[i].melee)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool HasNonHeavyMeleeWeapon
        {
            get
            {
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].type > 0 && CanUseItem(this.Inventory[i]) && !Items.GuardianItemPrefab.IsHeavyItem(this.Inventory[i]))
                    {
                        if (this.Inventory[i].melee)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool HasRangedWeapon
        {
            get
            {
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].type > 0 && CanUseItem(this.Inventory[i]))// && this.Inventory[i].modItem is Items.GuardianItemPrefab)
                    {
                        if (this.Inventory[i].ranged || this.Inventory[i].thrown)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool HasMagicWeapon
        {
            get
            {
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].type > 0 && CanUseItem(this.Inventory[i]))
                    {
                        if (this.Inventory[i].magic)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool HasSummonWeapon
        {
            get
            {
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].type > 0 && CanUseItem(this.Inventory[i]))
                    {
                        if (this.Inventory[i].summon)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public enum TargetTypes : byte
        {
            Npc,
            Player,
            Guardian
        }

        public bool IsPlayerBuddy(Player player = null)
        {
            if (player is null)
            {
                if (OwnerPos > -1)
                    player = Main.player[OwnerPos];
                else
                    player = Main.player[Main.myPlayer];
            }
            PlayerMod pl = player.GetModPlayer<PlayerMod>();
            return pl.IsBuddiesMode && pl.IsPlayerBuddy(this);
        }

        public bool NpcHasBeenHit(int id)
        {
            return NpcHit.Contains(id);
        }

        public bool PlayerHasBeenHit(int id)
        {
            return PlayerHit.Contains(id);
        }

        public void AddNpcHit(int id)
        {
            if (!NpcHasBeenHit(id))
                NpcHit.Add(id);
        }

        public void AddPlayerHit(int id)
        {
            if (!PlayerHasBeenHit(id))
                PlayerHit.Add(id);
        }

        public void EquipmentChanged()
        {
            UpdateStatus = true;
        }

        public void AddSkillProgress(float Value, GuardianSkills.SkillTypes Type)
        {
            if (OwnerPos == -1 || !MainMod.UseSkillsSystem || (Main.netMode == 1 && OwnerPos != Main.myPlayer)) return;
            Data.AddSkillProgress(Value, Type);
        }

        public bool HasTileMemory(int TileID)
        {
            return SpottedTileMemoryTime.ContainsKey(TileID);
        }

        public void RefreshTileMemory(int TileID)
        {
            if (SpottedTileMemoryTime.ContainsKey(TileID))
            {
                SpottedTileMemoryTime[TileID] = 60 * 30;
                return;
            }
            SpottedTileMemoryTime.Add(TileID, 60 * 30);
        }

        public void AddTileCount(int TileID)
        {
            if (TempTileCount.ContainsKey(TileID))
                TempTileCount[TileID]++;
            else
                TempTileCount.Add(TileID, 1);
        }

        public int GetTileCount(int TileID)
        {
            if (TileCount.ContainsKey(TileID))
                return TileCount[TileID];
            return 0;
        }

        public int GetTileCount(int[] TileIDs)
        {
            int c = 0;
            foreach (int t in TileIDs)
                c += GetTileCount(t);
            return c;
        }

        public void CheckNearbyTiles()
        {
            int TileCheckValue = GetCooldownValue(GuardianCooldownManager.CooldownType.BiomeCheckStacker);
            const int TileDimX = (int)(1920 * DivisionBy16), TileDimY = (int)(1080 * DivisionBy16), TotalDimRange = TileDimX * TileDimY,
                TileCheckStartPositionX = (int)(-TileDimX * 0.5f), TileCheckStartPositionY = (int)(-TileDimY * 0.5f),
                TileRangeX = (int)(TileDimX / (FramesSquaredTileCount * 0.5f)), TileRangeY = (int)(TileDimY / (FramesSquaredTileCount * 0.5f)), //MaxRange = TileRangeX * TileRangeY,
                AFourthTileCount = (int)(FramesSquaredTileCount * 0.25f);
            int TilePositionX = (int)(Position.X * DivisionBy16) + TileCheckStartPositionX, TilePositionY = (int)(CenterY * DivisionBy16) + TileCheckStartPositionY;
            TilePositionX += TileRangeX * (TileCheckValue % AFourthTileCount);
            TilePositionY += TileRangeY * (TileCheckValue / AFourthTileCount);
            for (int y = TilePositionY; y < TilePositionY + TileRangeY; y++)
            {
                for (int x = TilePositionX; x < TilePositionX + TileRangeX; x++)
                {
                    if (x >= Main.leftWorld * DivisionBy16 && x < Main.rightWorld * DivisionBy16 && y >= Main.topWorld * DivisionBy16 && y < Main.bottomWorld * DivisionBy16)
                    {
                        if (Main.tile[x, y] != null && Main.tile[x, y].active())
                        {
                            AddTileCount(Main.tile[x, y].type);
                            //if(OwnerPos != -1 && TownNpcs == 0 && Lighting.Brightness(x, y) > 0.5f && !TileMemories.Contains(Main.tile[x, y].type))
                            {
                                if (Main.tile[x, y].type == 42) //Lantern
                                {
                                    if (Main.tile[x, y].frameY >= 324 && Main.tile[x, y].frameY <= 358)
                                        LastHeartLanternNearby = true;
                                    else if (Main.tile[x, y].frameY >= 252 && Main.tile[x, y].frameY <= 286)
                                        LastStarLanternNearby = true;
                                }
                                else if (Main.tile[x, y].type == 215)
                                {
                                    if (Main.tile[x, y].frameY < 36)
                                    {
                                        LastFireplaceNearby = true;
                                    }
                                }
                                //if (OwnerPos > -1)
                                {
                                    if (!HasTileMemory(Main.tile[x, y].type) && Lighting.Brightness(x, y) > 0.5f)
                                    {
                                        //Comment tile
                                        Vector2 VisionPosition = Vector2.Zero;
                                        VisionPosition.X = Position.X;
                                        VisionPosition.Y = Position.Y - Height * 0.75f;
                                        bool Found = false;
                                        for (int x2 = -1; x2 < 2; x2++)
                                        {
                                            for (int y2 = -1; y2 < 2; y2++)
                                            {
                                                if (!Main.tileSolid[Main.tile[x, y].type] || (x2 != 0 && y2 != 0))
                                                {
                                                    if (Collision.CanHitLine(VisionPosition, 1, 1, new Vector2((x + x2) * 16, (y + y2) * 16), 16, 16))
                                                    {
                                                        Found = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (Found)
                                                break;
                                        }
                                        if (Found)
                                            OnTileSpotted(Main.tile[x, y].type, x, y);
                                    }
                                    RefreshTileMemory(Main.tile[x, y].type);
                                }
                            }
                        }
                    }
                }
            }
            IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BiomeCheckStacker, 1);
            if (TileCheckValue + 1 >= FramesSquaredTileCount)
            {
                DecreaseCooldownValue(GuardianCooldownManager.CooldownType.BiomeCheckStacker, FramesSquaredTileCount);
                TileCount = TempTileCount;
                FireplaceNearby = LastFireplaceNearby;
                HeartLanternNearby = LastHeartLanternNearby;
                StarLanternNearby = LastStarLanternNearby;
                LastFireplaceNearby = LastHeartLanternNearby = LastStarLanternNearby = false;
                TempTileCount = new Dictionary<int,int>();
                BiomeZoneChecker();
            }
            int[] Keys = SpottedTileMemoryTime.Keys.ToArray();
            foreach(int key in Keys)
            {
                SpottedTileMemoryTime[key]--;
                if (SpottedTileMemoryTime[key] <= 0)
                    SpottedTileMemoryTime.Remove(key);
            }
        }

        public void OnTileSpotted(int TileID, int TileX, int TileY)
        {
            bool Comment = MainMod.GeneralIdleCommentCooldown <= 0 && !HasCooldown(GuardianCooldownManager.CooldownType.TileCommentCooldown);
            string Mes = "";
            switch (TileID)
            {
                case Terraria.ID.TileID.Gold:
                case Terraria.ID.TileID.Platinum:
                    if (!Main.hardMode)
                    {
                        Mes = GuardianBase.MessageIDs.FoundRareOreTile;
                    }
                    break;
                case Terraria.ID.TileID.Mythril:
                case Terraria.ID.TileID.Orichalcum:
                    Mes = GuardianBase.MessageIDs.FoundRareOreTile;
                    break;
                case Terraria.ID.TileID.Adamantite:
                case Terraria.ID.TileID.Titanium:
                case Terraria.ID.TileID.LunarOre:
                    Mes = GuardianBase.MessageIDs.FoundVeryRareOreTile;
                    break;
                case Terraria.ID.TileID.Heart:
                case Terraria.ID.TileID.LifeFruit:
                    Mes = GuardianBase.MessageIDs.FoundLifeCrystalTile;
                    break;
                case Terraria.ID.TileID.PlanteraBulb:
                    Mes = GuardianBase.MessageIDs.FoundPlanteraTile;
                    break;
                case Terraria.ID.TileID.Detonator:
                    Mes = GuardianBase.MessageIDs.FoundDetonatorTile;
                    break;
                case Terraria.ID.TileID.PressurePlates:
                    Mes = GuardianBase.MessageIDs.FoundPressurePlateTile;
                    break;
                case 21:
                case 467:
                    if (Main.tile[TileX, TileY] != null)
                    {
                        int ChestPos = Chest.FindChest(TileX, TileY);
                        if (ChestPos >= 0 && ChestPos < Main.maxChests)
                        {
                            Chest c = Main.chest[ChestPos];
                            bool HasInterestingItem = false;
                            for(int i = 0; i < c.item.Length; i++)
                            {
                                if(c.item[i].type > 0)
                                {
                                    if(c.item[i].rare > Terraria.ID.ItemRarityID.White && 
                                        (c.item[i].damage > 0 || 
                                        c.item[i].defense > 0 ||
                                        c.item[i].accessory))
                                    {
                                        HasInterestingItem = true;
                                        break;
                                    }
                                }
                            }
                            if(HasInterestingItem)
                                Mes = GuardianBase.MessageIDs.FoundTreasureTile;
                        }
                    }
                    break;
                case Terraria.ID.TileID.ExposedGems:
                    Mes = GuardianBase.MessageIDs.FoundGemTile;
                    break;
                case Terraria.ID.TileID.LandMine:
                    Mes = GuardianBase.MessageIDs.FoundMineTile;
                    break;
                case Terraria.ID.TileID.MinecartTrack:
                    Mes = GuardianBase.MessageIDs.FoundMinecartRailTile;
                    break;
            }
            if (Comment && Mes != "" && Main.rand.Next(3) == 0)
            {
                Mes = GetMessage(Mes);
                if (Mes != "")
                {
                    SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Mes, this));
                    MainMod.SetIdleCommentCooldown();
                    AddCooldown(GuardianCooldownManager.CooldownType.TileCommentCooldown, Main.rand.Next(45 * 60, 80 * 60));
                }
            }
        }

        public void BiomeZoneChecker()
        {
            Point CenterTilePoint = CenterPosition.ToTileCoordinates();
            ZoneDungeon = false;
            if (CenterY >= Main.worldSurface * 16 && GetTileCount(MainMod.DungeonTileIDs) >= 250)
            {
                ZoneDungeon = Main.wallDungeon[(int)Main.tile[CenterTilePoint.X, CenterTilePoint.Y].wall];
            }
            Tile tile = MainMod.GetTile(CenterTilePoint);
            if (tile != null)
            {
                BehindWall = tile.wall > 0;
            }
            ZoneHoly = GetTileCount(MainMod.HolyTileIDs) >= 200;
            ZoneMeteor = GetTileCount(37) >= 50;
            ZoneJungle = GetTileCount(MainMod.JungleTileIDs) >= 80;
            ZoneSnow = GetTileCount(MainMod.SnowTileIDs) >= 300;
            ZoneCrimson = GetTileCount(MainMod.CrimsonTileIDs) + (GetTileCount(352) - 5 * GetTileCount(27)) >= 200;
            ZoneWaterCandle = (GetTileCount(49) > 0);
            ZonePeaceCandle = (GetTileCount(372) > 0);
            ZoneDesert = (GetTileCount(MainMod.DesertTileIDs) > 1000);
            ZoneGlowshroom = (GetTileCount(MainMod.GlowshroomTileIDs) > 100);
            ZoneUnderworldHeight = (CenterTilePoint.Y > Main.maxTilesY - 200);
            ZoneRockLayerHeight = (CenterTilePoint.Y <= Main.maxTilesY - 200 && (double)CenterTilePoint.Y > Main.rockLayer);
            ZoneDirtLayerHeight = ((double)CenterTilePoint.Y <= Main.rockLayer && (double)CenterTilePoint.Y > Main.worldSurface);
            ZoneOverworldHeight = ((double)CenterTilePoint.Y <= Main.worldSurface && (double)CenterTilePoint.Y > Main.worldSurface * 0.34999999403953552);
            ZoneSkyHeight = ((double)CenterTilePoint.Y <= Main.worldSurface * 0.34999999403953552);
            ZoneBeach = (this.ZoneOverworldHeight && (CenterTilePoint.X < 380 || CenterTilePoint.X > Main.maxTilesX - 380));
            ZoneRain = (Main.raining && (double)CenterTilePoint.Y <= Main.worldSurface);
            ZoneSandstorm = ((double)CenterTilePoint.Y <= Main.worldSurface && this.ZoneDesert && !this.ZoneBeach && Terraria.GameContent.Events.Sandstorm.Happening);
            ZoneTowerSolar = this.ZoneTowerVortex = this.ZoneTowerNebula = this.ZoneTowerStardust = false;
            ZoneOldOneArmy = false;
            if (ZoneDesert && CenterY > 3200)
            {
                if (Terraria.ID.WallID.Sets.Conversion.Sandstone[(int)tile.wall] || Terraria.ID.WallID.Sets.Conversion.HardenedSand[(int)tile.wall])
                    ZoneUndergroundDesert = true;
            }
            else
            {
                ZoneUndergroundDesert = false;
            }
            for (int i = 0; i < 200; i++)
            {
                if (!Main.npc[i].active) continue;
                if (Main.npc[i].type == 493 && (CenterPosition - Main.npc[i].Center).Length() <= 4000)
                {
                    ZoneTowerStardust = true;
                }
                if (Main.npc[i].type == 507 && (CenterPosition - Main.npc[i].Center).Length() <= 4000)
                {
                    ZoneTowerNebula = true;
                }
                if (Main.npc[i].type == 422 && (CenterPosition - Main.npc[i].Center).Length() <= 4000)
                {
                    ZoneTowerVortex = true;
                }
                if (Main.npc[i].type == 517 && (CenterPosition - Main.npc[i].Center).Length() <= 4000)
                {
                    ZoneTowerSolar = true;
                }
                if (Main.npc[i].type == 549 && (CenterPosition - Main.npc[i].Center).Length() <= 4000)
                {
                    ZoneOldOneArmy = true;
                }
            }
            SunflowerNearby = GetTileCount(Terraria.ID.TileID.Sunflower) > 0;
            //LifeCrystalChainNearby = GetTileCount(Terraria.ID.TileID.HangingLanterns);
        }

        public void ChangeGravity(int Direction)
        {
            if (Direction != GravityDirection)
            {
                Position.Y -= CollisionHeight * GravityDirection;
                this.GravityDirection = Direction;
            }
        }

        public void FlipGravity()
        {
            ChangeGravity(GravityDirection * -1);
        }

        public bool StartNewGuardianAction(GuardianActions action, bool Force = false)
        {
            return StartNewGuardianAction(action, action.ID, Force);
        }

        public bool StartNewGuardianAction(GuardianActions action, int ID, bool Force = false)
        {
            if ((DoAction.Cancellable && DoAction.InUse) || Force)
            {
                DoAction.OnActionEnd(this);
                DoAction.InUse = false;
            }
            if (!DoAction.InUse)
            {
                action.IsGuardianSpecificAction = true;
                action.ID = ID;
                DoAction = action;
                return true;
            }
            return false;
        }

        public void AddInjury(byte Value)
        {
            if (!MainMod.UsingGuardianNecessitiesSystem)
                return;
            Data.AddInjury(Value);
            UpdateStatus = true;
        }

        public void CountNearbyNpcs()
        {
            TownNpcs = 0;
            Vector2 MyCenter = CenterPosition;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].townNPC)
                {
                    Vector2 NpcCenter = Main.npc[n].Center;
                    if (MyCenter.X >= NpcCenter.X - NPC.sWidth && MyCenter.X < NpcCenter.X + NPC.sWidth && MyCenter.Y >= NpcCenter.Y - NPC.sHeight && MyCenter.Y < NpcCenter.Y + NPC.sHeight)
                    {
                        TownNpcs += Main.npc[n].npcSlots;
                    }
                }
            }
            foreach (TerraGuardian t in WorldMod.GuardianTownNPC)
            {
                Vector2 NpcCenter = t.CenterPosition;
                if (t.WhoAmID != WhoAmID && t.OwnerPos == -1 && 
                    MyCenter.X >= NpcCenter.X - NPC.sWidth && MyCenter.X < NpcCenter.X + NPC.sWidth && 
                    MyCenter.Y >= NpcCenter.Y - NPC.sHeight && MyCenter.Y < NpcCenter.Y + NPC.sHeight)
                {
                    TownNpcs += t.Base.TownNpcSlot;
                }
            }
        }

        public void ToggleGuardianFullControl(bool TurnOn)
        {
            if (PlayerMounted && HasFlag(GuardianFlags.StopMindingAfk))
            {
                if (TurnOn)
                {
                    GuardingPosition = new Point((int)(Position.X * DivisionBy16), (int)(Position.Y * DivisionBy16));
                    IsGuardingPlace = false;
                }
                else
                {
                    GuardingPosition = null;
                }
                GuardianHasControlWhenMounted = TurnOn;
            }
        }

        public void UpdateKnockoutState()
        {
            if (Downed)
                return;
            {
                float ReviveValue = 0;
                if (NegativeReviveBoost)
                {
                    ReviveValue -= 1f + 0.1f * ReviveBoost;
                }
                else if (HasBuff(Terraria.ID.BuffID.Bleeding))
                {
                    ReviveValue = -1f - (1f / (ReviveBoost + 1));
                }
                else
                {
                    ReviveValue += 1f + 0.15f * ReviveBoost;
                }
                ReviveStack += ReviveValue;
                if (ReviveStack >= MaxReviveStack)
                {
                    int HealValue = (int)(Math.Max(1, MHP * 0.05f));
                    CombatText.NewText(HitBox, CombatText.HealLife, HealValue, false, true);
                    HP += HealValue;
                    ReviveStack -= MaxReviveStack;
                }
                else if (ReviveStack <= MinReviveStack)
                {
                    int DamageValue = (int)(Math.Max(1, MHP * 0.05f));
                    CombatText.NewText(HitBox, CombatText.DamagedHostile, DamageValue, false, true);
                    HP -= DamageValue;
                    ReviveStack -= MinReviveStack;
                }
            }
            if (HP >= MHP)
            {
                ExitDownedState();
                return;
            }
            else if(HP <= 0)
            {
                if (MainMod.GuardiansDontDiesAfterDownedDefeat)
                {
                    if (HP < 0)
                        HP = 0;
                }
                else
                {
                    DoForceKill(" couldn't resist longer...");
                }
            }
            MoveLeft = MoveRight = MoveUp = MoveDown = Jump = Action = Ducking = OffHandAction = false;
            if (HP < 0)
                HP = 0;
            if (HasFlag(GuardianFlags.Petrified))
            {
                Rotation = 0;
            }
            else
            {
                if (Velocity.Y == 0)
                {
                    if (!Base.IsCustomSpriteCharacter)
                    {
                        Rotation = -1.570796326794897f * Direction;
                    }
                    else if (Base.DownedFrame > -1 || Base.DuckingFrame > -1)
                        Rotation = 0;
                    else
                        Rotation += 1.570796326794897f * Direction;
                }
                else
                {
                    Rotation += Velocity.X * 0.05f;
                }
            }
            if (ReviveBoost > 0)
            {
                if (Breath < BreathMax - 1)
                {
                    BreathCooldown += 1 + (int)(ReviveBoost * 0.5f);
                    if(BreathCooldown > 5)
                    {
                        BreathCooldown -= 5;
                        Breath++;
                    }
                }
                if (ReviveBoost + TownNpcs > 255)
                    ReviveBoost = 255;
                else
                    ReviveBoost += (byte)TownNpcs;
            }
            foreach (BuffData buff in Buffs)
            {
                if (buff.ID == ModContent.BuffType<giantsummon.Buffs.Injury>() || buff.ID == ModContent.BuffType<giantsummon.Buffs.HeavyInjury>())
                {
                    buff.Time++;
                }
                else if (ReviveBoost > 0 && !NegativeReviveBoost && buff.ID != Terraria.ID.BuffID.PotionSickness && Main.debuff[buff.ID])
                {
                    buff.Time -= ReviveBoost;
                }
            }
            if (KnockedOutCold)
            {
                if (HasFlag(GuardianFlags.CantBeKnockedOutCold))
                {
                    KnockedOutCold = false;
                }
                if (Breath <= 0 && !MainMod.GuardiansDontDiesAfterDownedDefeat)
                {
                    DoForceKill(" didn't had more air.");
                }
                if (LavaWet && !HasFlag(GuardianFlags.LavaImmunity))
                {
                    DoForceKill(" turned into ash.");
                }
            }
            NegativeReviveBoost = false;
            ReviveBoost = 0;
        }

        public void AddDrawMomentToPlayer(Player player, bool DrawInFrontOfTarget = false)
        {
            GuardianDrawMoment gdm = new GuardianDrawMoment(WhoAmID, TargetTypes.Player, player.whoAmI, DrawInFrontOfTarget);
            MainMod.DrawMoment.Add(gdm);
        }

        public void AddDrawMomentToNpc(NPC npc, bool DrawInFrontOfTarget = false)
        {
            GuardianDrawMoment gdm = new GuardianDrawMoment(WhoAmID, TargetTypes.Npc, npc.whoAmI, DrawInFrontOfTarget);
            MainMod.DrawMoment.Add(gdm);
        }

        public void AddDrawMomentToTerraGuardian(TerraGuardian tg, bool DrawInFrontOfTarget = false)
        {
            GuardianDrawMoment gdm = new GuardianDrawMoment(WhoAmID, TargetTypes.Guardian, tg.WhoAmID, DrawInFrontOfTarget);
            MainMod.DrawMoment.Add(gdm);
        }

        public bool InCameraRange()
        {
            Vector2 CameraCenter = MainMod.GetScreenCenter;
            return (Math.Abs(Position.X - CameraCenter.X) < (Main.screenWidth + SpriteWidth) * 0.5f + 20 &&
                Math.Abs(CenterY - CameraCenter.Y) < (Main.screenHeight + SpriteHeight) * 0.5f + 15);
        }

        public void UpdatePerception()
        {
            if (HasCooldown(GuardianCooldownManager.CooldownType.SpottingCooldown))
                return;
            AddCooldown(GuardianCooldownManager.CooldownType.SpottingCooldown, 60 + Main.rand.Next(90));
            List<byte> NpcsSpotted = new List<byte>(),
                PlayersSpotted = new List<byte>();
            List<int> GuardiansSpotted = new List<int>();
            const float DetectionDistance = 200;
            Vector2 CenterPosition = this.CenterPosition;
            int Width = this.Width;
            int Height = this.Height;
            for(byte i = 0; i < 255; i++)
            {
                if (Math.Abs(Main.player[i].position.X + Main.player[i].width * 0.5f - CenterPosition.X) < (Main.player[i].width + Width) * 0.5f + DetectionDistance &&
                    Math.Abs(Main.player[i].position.Y + Main.player[i].height * 0.5f - CenterPosition.Y) < (Main.player[i].height + Height) * 0.5f + DetectionDistance)
                {
                    if (!this.PlayersSpotted.Contains(i))
                    {
                        this.DoTrigger(TriggerTypes.Spotted, new giantsummon.Trigger.TriggerTarget(Main.player[i]));
                    }
                    PlayersSpotted.Add(i);
                }
                if(i < 200)
                {
                    if (Math.Abs(Main.npc[i].Center.X - CenterPosition.X) < (Main.npc[i].width + Width) + DetectionDistance &&
                            Math.Abs(Main.npc[i].Center.Y - CenterPosition.Y) < (Main.npc[i].height + Height) + DetectionDistance)
                    {
                        if (!this.NpcsSpotted.Contains(i))
                        {
                            DoTrigger(TriggerTypes.Spotted, new Trigger.TriggerTarget(Main.npc[i]));
                        }
                        NpcsSpotted.Add(i);
                    }
                }
            }
            foreach(int key in MainMod.ActiveGuardians.Keys)
            {
                if(key != this.WhoAmID)
                {
                    TerraGuardian guardian = MainMod.ActiveGuardians[key];
                    if (Math.Abs(guardian.Position.X - CenterPosition.X) < (Width + guardian.Width) * 0.5f + DetectionDistance &&
                        Math.Abs(guardian.Position.Y - CenterPosition.Y) < (Height + guardian.Height) * 0.5f + DetectionDistance)
                    {
                        if (!this.GuardiansSpotted.Contains(key))
                        {
                            this.DoTrigger(TriggerTypes.Spotted, new giantsummon.Trigger.TriggerTarget(guardian));
                        }
                        GuardiansSpotted.Add(key);
                    }
                }
            }
            this.NpcsSpotted = NpcsSpotted;
            this.PlayersSpotted = PlayersSpotted;
            this.GuardiansSpotted = GuardiansSpotted;
        }

        public static bool IsBlacklisted(TerraGuardian guardian)
        {
            return IsBlacklisted(guardian.MyID);
        }

        public static bool IsBlacklisted(GuardianData data)
        {
            return IsBlacklisted(data.MyID);
        }

        public static bool IsBlacklisted(GuardianID gid)
        {
            return MainMod.CompanionBlacklist.Any(x => x.IsSameID(gid));
        }

        public static bool IsBlacklisted(int id, string modid = "")
        {
            if (modid == "")
                modid = MainMod.mod.Name;
            return MainMod.CompanionBlacklist.Any(x => x.IsSameID(id, modid));
        }

        private static bool LoadedWorldRegion = true;
        //private byte Counter = 0;

        public void Update(Player Owner = null)
        {
            if (!Active)
                return;
            //SaySomething("WhoAmID: " + WhoAmID + " Owner: " + OwnerPos);
            if (Position.X < 0 || Position.Y < 0)
                Spawn();
            CollisionHeightDiscount = 0;
            FinalScale = ScaleMult;
            if (TurnLock > 0)
                TurnLock--;
            if (Owner != null)
            {
                //For testing
                //if (false && !this.mount.Active)
                //    RideMount(Mount.Bunny);
                OwnerPos = Owner.whoAmI;
                if (!NpcMod.HasGuardianNPC(ID, ModID))
                    NpcMod.AddGuardianMet(ID, ModID);
            }
            else
            {
                OwnerPos = -1;
            }
            if (TalkPlayerID > -1)
            {
                Player talkPlayer = Main.player[TalkPlayerID];
                PlayerMod pm = talkPlayer.GetModPlayer<PlayerMod>();
                if (!talkPlayer.active || talkPlayer.dead || !pm.IsTalkingToAGuardian || pm.TalkingGuardianPosition != WhoAmID)
                    TalkPlayerID = -1;
            }
            /*if(!mount.Active && Counter < 120)
            {
                Counter++;
                if(Counter == 1)
                    RideMount(Terraria.ID.MountID.Bunny);
            }*/
            LoadedWorldRegion = false;
            if(Position.X >= Main.leftWorld && Position.X < Main.rightWorld && 
                Position.Y >= Main.topWorld && Position.Y < Main.bottomWorld)
                LoadedWorldRegion = Main.tile[(int)(Position.X * DivisionBy16), (int)(Position.Y * DivisionBy16)] != null;
            OffsetX = OffsetY = 0;
            MainMod.AddActiveGuardian(this);
            WalkMode = false;
            Rotation = 0f;
            ProtectingPlayerFromHarm = false;
            PlayerCanEscapeGrab = true;
            StuckTimerChanged = false;
            if (Main.dayTime != WorldMod.LastWasDay)
            {
                UpdateStatus = true;
            }
            if (Main.dayTime == false && HasFlag(GuardianFlags.WerewolfAcc))
            {
                AddBuff(28, 2, true);
            }
            if (Compatibility.NExperienceCompatibility.IsModActive)
            {
                if(Compatibility.NExperienceCompatibility.LevelChanged(this))
                    UpdateStatus = true;
            }
            if (UpdateStatus || MainMod.ForceUpdateGuardiansStatus)
                DoUpdateGuardianStatus();
            gfxOffY = 0;
            CountNearbyNpcs();
            if (Position.X == 0 && Position.Y == 0)
                Spawn();
            if(LoadedWorldRegion)
                BehaviorHandler();
            else
            {
                if(OwnerPos > -1)
                {
                    TeleportToPlayer();
                }
            }
            if (SubAttackInUse && !SpecialAttack.CanMove)
            {
                MoveLeft = MoveRight = MoveUp = MoveDown = Jump = Action = Ducking = OffHandAction = false;
            }
            else if (HasFlag(GuardianFlags.Confusion))
            {
                bool MovingLeft = MoveLeft, MovingRight = MoveRight, MovingUp = MoveUp, MovingDown = MoveDown;
                MoveLeft = MovingRight;
                MoveRight = MovingLeft;
                MoveUp = MovingDown;
                MoveDown = MovingUp;
            }
            if (HasBuff(ModContent.BuffType<giantsummon.Buffs.Sleeping>()))
            {
                MoveLeft = MoveRight = MoveUp = MoveDown = Jump = Action = Ducking = OffHandAction = false;
                DisplayEmotion(Emotions.Sleepy);
                if (PlayerMounted)
                    ToggleMount(true, false);
            }
            if (KnockedOutCold && !KnockedOut)
            {
                KnockedOutCold = false;
            }
            if (KnockedOut)
            {
                UpdateKnockoutState();
            }
            if (IsBeingPulledByPlayer && (PlayerMounted || PlayerControl || Downed))
                IsBeingPulledByPlayer = false;
            if (ItemAnimationTime > 0 && Velocity.Y == 0 && ItemUseType != ItemUseTypes.AimingUse && ItemUseType != ItemUseTypes.CursedAttackAttempt && ItemUseType != ItemUseTypes.ItemDrink2h && ItemUseType != ItemUseTypes.OverHeadItemUse && ItemUseType != ItemUseTypes.LightVerticalSwing) //avoiding movement when attacking
            {
                MoveLeft = false;
                MoveRight = false;
                MoveUp = false;
                MoveDown = false;
                Jump = false;
            }
            if (HasFlag(GuardianFlags.Frozen) || HasFlag(GuardianFlags.Petrified))
            {
                MoveLeft = MoveRight = MoveUp = MoveDown = Jump = Action = false;
            }
            if (UsingFurniture || (DoAction.InUse && DoAction.BlockOffHandUsage) || (ItemAnimationTime > 0 && (ItemUseType == ItemUseTypes.HeavyVerticalSwing || ItemUseType == ItemUseTypes.ItemDrink2h || Base.DontUseRightHand || !Base.IsCustomSpriteCharacter)) || (FreezeItemUseAnimation && HeldItemHand == HeldHand.Both) || HasFlag(GuardianFlags.Cursed) || (PlayerMounted && ItemAnimationTime > 0) || (CurrentIdleAction == IdleActions.LookingAtTheBackground))
            {
                OffHandAction = false;
            }
            if (OffHandAction)
            {
                if (ItemAnimationTime == 0 || !IsDualWielding)
                {
                    PickOffhandForTheSituation(IsAttackingSomething);
                }
            }
            else if (LastOffHandAction)
            {
                UpdateStatus = true;
            }
            if (DoAction.InUse)
            {
                if (Downed || KnockedOut)
                {
                    DoAction.InUse = false;
                }
                else
                {
                    //StuckTimer = 0;
                    DoAction.UpdateAction(this);
                }
            }
            if (ItemAnimationTime == 0)
            {
                if (MoveDown && Base.CanDuck && !SittingOnPlayerMount && (!ReverseMount || !PlayerMounted) && Velocity.Y == 0)
                {
                    Ducking = true;
                }
                else
                {
                    Ducking = false;
                }
            }
            if (LoadedWorldRegion)
            {
                UpdateHorizontalMovement();
            }
            if (Velocity.Y == 0)
            {
                bool MayDash = !SittingOnPlayerMount && !MountedOnPlayer;
                if (CollisionX)
                    DashCooldown = 30;
                else if (MayDash && Math.Abs(Velocity.X) >= this.MaxSpeed && Velocity.Y == 0)
                    DashCooldown--;
                else if (DashCooldown < 30)
                {
                    DashCooldown++;
                }
            }
            LockDirection = false;
            if(LoadedWorldRegion)
                UpdateVerticalMovement();
            if (OwnerPos > -1 && (Downed || Main.player[OwnerPos].dead))
            {
                PlayerControl = PlayerMounted = false;
            }
            if (IsBeingPulledByPlayer && SittingOnPlayerMount)
                IsBeingPulledByPlayer = false;
            if (SittingOnPlayerMount && Owner != null && (Owner.dead || !Owner.mount.Active || GrabbingPlayer))
                DoSitOnPlayerMount(false);
            Base.ModifyVelocity(this, ref Velocity);
            if (Downed)
            {
                this.Position += this.Velocity;
                WakeupTime--;
                this.KnockdownRotation += this.Velocity.X * 0.0174532925199433f * 2.5f;
                this.ItemRotation += this.Velocity.X * 0.0174532925199433f * 2.5f;
                if (WakeupTime <= 0)
                    Spawn();
            }
            else
            {
                UpdateMountedPosition();
                bool WofPassing = CheckForPassingWof();
                if (!UpdatePullByPlayer() && LoadedWorldRegion && !WofPassing)
                {
                    CheckForObstacles();
                    UpdateCollision();
                }
                HitBox.X = (int)(Position.X - HitBox.Width * 0.5f);
                HitBox.Y = (int)(Position.Y);
                if (GravityDirection > 0)
                    HitBox.Y -= HitBox.Height;
                if (LoadedWorldRegion && OwnerPos != -1)
                    CheckNearbyTiles();
                CheckForNpcContact();
                MagicMirrorTrigger = false;
                AimDirection += MoveDir;
                MoveDir = Vector2.Zero;
                ItemUseScript();
                CheckForOutOfBounds();
                bool MayRocket = Jump && !LastJump;
                if(LoadedWorldRegion)
                    JumpControl(ref MayRocket);
                if (RocketTime < RocketMaxTime)
                    MayRocket = true;
                if (Velocity.Y == 0)
                    MayRocket = false;
                if (LoadedWorldRegion && !HasFlag(GuardianFlags.Frozen) && !HasFlag(GuardianFlags.Petrified) && !MoveDown)
                {
                    WingMovement();
                    RocketMovement(MayRocket);
                }
                UpdateHealthRegen();
                UpdateManaRegen();
                UpdateExtraStuff();
            }
            UpdateSubAttack();
            if (InCameraRange())
            {
                UpdateAnimation();
            }
            if(Main.netMode == 0 || (Main.netMode == 1 && OwnerPos == Main.myPlayer))
                CheckFriendshipProgression();
            if (ImmuneTime > 0)
            {
                ImmuneTime--;
                if (ImmuneTime == 0)
                    ImmuneNoBlink = false;
            }
            if (StuckTimerChanged && (OwnerPos == Main.myPlayer || IsCommander || (OwnerPos > -1 && Main.player[OwnerPos].GetModPlayer<PlayerMod>().IsCompanionParty)))
            {
                if (StuckTimer >= 60)
                {
                    if (GuardingPosition.HasValue && !WofFacing)
                    {
                        TeleportToGuardedPosition();
                    }
                    else if ((OwnerPos > -1 || IsCommander) && !Main.player[OwnerPos].dead && (!WofFacing || Main.player[OwnerPos].gross))
                    {
                        BePulledByPlayer();
                        //TeleportToPlayer();
                    }
                    StuckTimer = 0;
                }
            }
            else
            {
                StuckTimer = 0;
            }
            if (ImmuneTime > 0 && !ImmuneNoBlink)
            {
                if (IncreaseImmuneTime)
                {
                    ImmuneAlpha += 50;
                }
                else
                {
                    ImmuneAlpha -= 50;
                }
                if (ImmuneAlpha <= 50)
                    IncreaseImmuneTime = true;
                if (ImmuneAlpha >= 200)
                    IncreaseImmuneTime = false;
            }
            else
            {
                ImmuneAlpha = 0;
                IncreaseImmuneTime = true;
            }
            if (!HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown))
                AddCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown, 10);
            if (Channeling && !Action)
                Channeling = false;
            ShowOffHand = OffHandAction;
            if (CanSyncMe) //TODO - A spamfest of netmessage will happen here if a companion is following the player. Find a way of fixing that.
            {
                /*if (MoveUp != LastMoveUp || MoveRight != LastMoveRight || MoveDown != LastMoveDown || MoveLeft != LastMoveLeft || Jump != LastJump)
                {
                    Netplay.SendGuardianMovementUpdate(WhoAmID, -1, OwnerPos);
                }*/
                if(LastAction != Action || LastOffHandAction != OffHandAction)
                {
                    Netplay.SendGuardianItemUseUpdate(WhoAmID, -1, OwnerPos);
                }
            }
            LastMoveRight = MoveRight;
            LastMoveLeft = MoveLeft;
            LastMoveUp = MoveUp;
            LastMoveDown = MoveDown;
            LastJump = Jump;
            LastAction = Action;
            LastOffHandAction = OffHandAction;
            //if (Main.netMode == 0 || OwnerPos == Main.myPlayer)
            {
                MoveRight = MoveLeft = MoveUp = MoveDown = Action = Jump = OffHandAction = false;
            }
            for (int c = 0; c < Cooldowns.Count; c++)
            {
                if (!Cooldowns[c].DontDepleteOvertime && !CooldownException.Contains(Cooldowns[c].type))
                    Cooldowns[c].CooldownTime--;
                if (Cooldowns[c].CooldownTime <= 0)
                    Cooldowns.RemoveAt(c);
            }
            for (int b = 0; b < Buffs.Count; b++)
            {
                Buffs[b].Time--;
                if (Buffs[b].Time <= 0)
                {
                    Buffs.RemoveAt(b);
                    UpdateStatus = true;
                }
            }
            if (FinalScale != Scale)
            {
                float ScaleChange = (FinalScale - Scale) * 0.02f;
                Scale += ScaleChange;
                if (Math.Abs(Scale - FinalScale) < 0.01f)
                    Scale = FinalScale;
                /*if (FinalScale > Scale)
                {
                    Scale += 0.01f;
                    if (FinalScale < Scale)
                        Scale = FinalScale;
                }
                else if (FinalScale < Scale)
                {
                    Scale -= 0.01f;
                    if (FinalScale > Scale)
                        Scale = FinalScale;
                }*/
            }
            NumMinions = 0;
            NumSentries = 0;
            MinionSlotCount = 0;
            if (HeldProj > -1)
            {
                HeldProj = -1;
            }
            UpdateWeapons = false;
            CooldownException.Clear();
            ActiveNpcs = 0f;
            if (MessageTime > 0)
            {
                MessageTime--;
            }
            if (MessageTime <= 0 && MessageSchedule.Count > 0)
            {
                if (MessageSchedule[0].MessageDelay > 0)
                    MessageSchedule[0].MessageDelay--;
                else
                {
                    SaySomething(MessageSchedule[0].Message, DisplayMessageOnChat);
                    MessageSchedule.RemoveAt(0);
                }
            }
            anchor.pos = Position;
            anchor.pos.Y -= Height;
        }

        public bool CanSyncMe { get { return (Main.netMode == 1 && OwnerPos == Main.myPlayer) || (Main.netMode == 2 && OwnerPos == -1); } }

        public void FloorVisual(bool Falling)
        {
            Point TilePosition = Position.ToTileCoordinates();
            TilePosition.Y++;
            if (GravityDirection == -1)
            {
                TilePosition.Y = (int)((Position.Y - Height - 0.1f) * DivisionBy16);
            }
            int TileType = -1;
            for (int x = -1; x < 2; x++)
            {
                if (Main.tile[TilePosition.X + x, TilePosition.Y] == null)
                {
                    Main.tile[TilePosition.X + x, TilePosition.Y] = new Tile();
                }
            }
            if (Main.tile[TilePosition.X, TilePosition.Y].nactive() && Main.tileSolid[Main.tile[TilePosition.X, TilePosition.Y].type])
            {
                TileType = Main.tile[TilePosition.X, TilePosition.Y].type;
            }
            else if (Main.tile[TilePosition.X - 1, TilePosition.Y].nactive() && Main.tileSolid[Main.tile[TilePosition.X - 1, TilePosition.Y].type])
            {
                TileType = Main.tile[TilePosition.X - 1, TilePosition.Y].type;
            }
            else if (Main.tile[TilePosition.X + 1, TilePosition.Y].nactive() && Main.tileSolid[Main.tile[TilePosition.X + 1, TilePosition.Y].type])
            {
                TileType = Main.tile[TilePosition.X + 1, TilePosition.Y].type;
            }
            if (Main.tile[TilePosition.X - 1, TilePosition.Y].slope() != 0
                || Main.tile[TilePosition.X, TilePosition.Y].slope() != 0 ||
                Main.tile[TilePosition.X + 1, TilePosition.Y].slope() != 0)
                TileType = -1;
            if (TileType > -1 && !Wet)
                MakeFloorDust(Falling, TileType);
        }

        private void MakeFloorDust(bool Falling, int type)
        {
            if (type == 147 || type == 25 || type == 53 || type == 189 || type == 0 || type == 123 || type == 57 || type == 112 || type == 116 || type == 196 || type == 193 || type == 195 || type == 197 || type == 199 || type == 229 || type == 371)
            {
                int Attempts = 1;
                if (Falling)
                    Attempts = 20;
                for (int i = 0; i < Attempts; i++)
                {
                    bool Create = true;
                    int DustType = 76;
                    if (type == 53)
                    {
                        DustType = 32;
                    }
                    if (type == 189)
                    {
                        DustType = 16;
                    }
                    if (type == 0)
                    {
                        DustType = 0;
                    }
                    if (type == 123)
                    {
                        DustType = 53;
                    }
                    if (type == 57)
                    {
                        DustType = 36;
                    }
                    if (type == 112)
                    {
                        DustType = 14;
                    }
                    if (type == 116)
                    {
                        DustType = 51;
                    }
                    if (type == 196)
                    {
                        DustType = 108;
                    }
                    if (type == 193)
                    {
                        DustType = 4;
                    }
                    if (type == 195 || type == 199)
                    {
                        DustType = 5;
                    }
                    if (type == 197)
                    {
                        DustType = 4;
                    }
                    if (type == 229)
                    {
                        DustType = 153;
                    }
                    if (type == 371)
                    {
                        DustType = 243;
                    }
                    if (type == 25)
                    {
                        DustType = 37;
                    }
                    if (DustType == 32 && Main.rand.Next(2) == 0)
                    {
                        Create = false;
                    }
                    if (DustType == 14 && Main.rand.Next(2) == 0)
                    {
                        Create = false;
                    }
                    if (DustType == 51 && Main.rand.Next(2) == 0)
                    {
                        Create = false;
                    }
                    if (DustType == 36 && Main.rand.Next(2) == 0)
                    {
                        Create = false;
                    }
                    if (DustType == 0 && Main.rand.Next(3) != 0)
                    {
                        Create = false;
                    }
                    if (DustType == 53 && Main.rand.Next(3) != 0)
                    {
                        Create = false;
                    }
                    Color color = default(Color);
                    if (type == 193)
                    {
                        color = new Color(30, 100, 255, 100);
                    }
                    if (type == 197)
                    {
                        color = new Color(97, 200, 255, 100);
                    }
                    if (!Falling)
                    {
                        if (Main.rand.Next(100) > (Math.Abs(Velocity.X) / 3) * 100)
                        {
                            Create = false;
                        }
                    }
                    if (Create)
                    {
                        float VelX = Velocity.X;
                        if (VelX > 6)
                            VelX = 6;
                        if (VelX < -6)
                            VelX = -6;
                        if (Velocity.X != 0 || Falling)
                        {
                            int DustPos = Dust.NewDust(new Vector2(Position.X - CollisionWidth * 0.5f, Position.Y - 2f), CollisionWidth, 6, DustType, 0, 0, 50, color, 1f);
                            if (GravityDirection == -1)
                            {
                                Main.dust[DustPos].position.Y -= CollisionHeight + 4;
                            }
                            if (DustType == 76)
                            {
                                Main.dust[DustPos].scale += (float)Main.rand.Next(3) * 0.1f;
                                Main.dust[DustPos].noLight = true;
                            }
                            if (DustType == 16 || DustType == 108 || DustType == 153)
                            {
                                Main.dust[DustPos].scale += (float)Main.rand.Next(6) * 0.1f;
                            }
                            if (DustType == 37)
                            {
                                Main.dust[DustPos].scale += 0.25f;
                                Main.dust[DustPos].alpha = 50;
                            }
                            if (DustType == 5)
                            {
                                Main.dust[DustPos].scale += Main.rand.Next(2, 8) * 0.1f;
                            }
                            Main.dust[DustPos].noGravity = true;
                            if (Attempts > 1)
                            {
                                Main.dust[DustPos].velocity.X *= 1.2f;
                                Main.dust[DustPos].velocity.Y *= 0.8f;
                                Main.dust[DustPos].velocity.Y -= 1f;
                                Main.dust[DustPos].velocity *= 0.8f;
                                Main.dust[DustPos].scale += Main.rand.Next(3) * 0.1f;
                                Main.dust[DustPos].velocity.X = (Main.dust[DustPos].position.X - Position.X) * 0.2f;
                                if (Main.dust[DustPos].velocity.Y > 0)
                                {
                                    Main.dust[DustPos].velocity.Y *= -1f;
                                }
                                Main.dust[DustPos].velocity.X += VelX * 0.3f;
                            }
                            else
                            {
                                Main.dust[DustPos].velocity *= 0.2f;
                            }
                            Main.dust[DustPos].position.X -= VelX;
                            if (GravityDirection == -1)
                            {
                                Main.dust[DustPos].velocity.Y *= -1;
                            }
                        }
                    }
                }
            }
        }

        public void HandleHermesBootsEffect()
        {
            if (Velocity.Y != 0)
                return;
            for (int fx = 0; fx < 2; fx++)
            {
                int dust;
                if (Velocity.Y == 0)
                {
                    dust = Dust.NewDust(new Vector2(Position.X - Width * 0.5f, Position.Y - 4), Width, 8, 31, 0, 0, 100, default(Color), 1.4f);
                }
                else
                {
                    dust = Dust.NewDust(new Vector2(Position.X - Width * 0.5f, CenterY - 8), Width, 16, 31, 0, 0, 100, default(Color), 1.4f);
                }
                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
            }
            if (!HasCooldown(GuardianCooldownManager.CooldownType.SpeedBootsSoundEffect) && Velocity.Y == 0)
            {
                Main.PlaySound(17, Position, -1);
                AddCooldown(GuardianCooldownManager.CooldownType.SpeedBootsSoundEffect, 9);
            }
        }

        public bool AdjascentTileChecker()
        {
            return false;
            Player player = Main.player[OwnerPos];
            const int XDistancing = 3, YDistancing = 4;
            int cx = (int)(Position.X * DivisionBy16), cy = (int)(Position.Y * DivisionBy16);
            bool Change = false;
            bool water = player.adjWater, lava = player.adjLava, honey = player.adjHoney;
            for (int x = cx - XDistancing; x <= cx + XDistancing; x++)
            {
                for (int y = cy - YDistancing; y <= cy + YDistancing; y++)
                {
                    Tile tile = MainMod.GetTile(x, y);
                    if (tile.liquid > 200)
                    {
                        switch (tile.liquid)
                        {
                            case 0:
                                player.adjWater = true;
                                break;
                            case 1:
                                player.adjLava = true;
                                break;
                            case 2:
                                player.adjHoney = true;
                                break;
                        }
                    }
                    if (!tile.active()) continue;
                    if (!player.adjTile[tile.type])
                        Change = true;
                    player.adjTile[(int)tile.type] = true;
                    if (tile.type == 302)
                    {
                        player.adjTile[17] = true;
                    }
                    if (tile.type == 77)
                    {
                        player.adjTile[17] = true;
                    }
                    if (tile.type == 133)
                    {
                        player.adjTile[17] = true;
                        player.adjTile[77] = true;
                    }
                    if (tile.type == 134)
                    {
                        player.adjTile[16] = true;
                    }
                    if (tile.type == 354)
                    {
                        player.adjTile[14] = true;
                    }
                    if (tile.type == 355)
                    {
                        player.adjTile[13] = true;
                        player.adjTile[14] = true;
                        player.alchemyTable = true;
                    }
                }
            }
            if (!Change)
            {
                if (player.adjWater != water || player.adjLava != lava || player.adjHoney != honey)
                    Change = true;
            }
            return Change;
        }

        public void Rebirth(int RebirthAge) //Needs some more work
        {
            GuardianData data = Data;
            data.SavedAge = RebirthAge;
            data.LifeTime = new TimeSpan(0); //The time must be set in away, where the companion grows older exactly at its birthday.
            AgeScale = Scale = FinalScale = GetAgeSize();
            Main.NewText(Name + " has rebirth to age " + RebirthAge + "!");
        }

        public static float GetAgeDecimalValue(int StartAge, double BirthdayAge, TimeSpan Time, float AgingSpeed = 1f)
        {
            return (float)(StartAge * AgingSpeed + ((Time.TotalDays - BirthdayAge) * AgingSpeed) / GuardianData.DaysInYear);
        }

        public static float GetAgeSizeValue(float Age)
        {
            //Age = Data.GetRealAgeDecimal();
            if (Age >= 18)
            {
                return 1f + (float)Age / (Age + 1) - 0.94f;
            }
            return Age / 18 * 0.9f + 0.1f;
        }

        public float GetAgeSize()
        {
            if (Base.IsTerrarian || !Base.GetGroup.AgeAffectsScale)
                return 1f;
            return GetAgeSizeValue(Data.GetRealAgeDecimal());
            /*float Age = Data.GetRealAgeDecimal();
            if (Age >= 18)
                return 1f;
            return Age / 18 * 0.9f + 0.1f;*/
        }

        public void UpdateLifeStealAndGhostDamageRate()
        {
            if(GhostDamage > 0)
            {
                GhostDamage -= 2.5f;
                if (GhostDamage < 0)
                    GhostDamage = 0;
            }
            float Rate = (float)MHP / HP;
            if (Main.expertMode)
            {
                if (LifeStealRate < Rate * 70)
                {
                    LifeStealRate += Rate * 0.5f;
                    if (LifeStealRate > Rate * 70)
                    {
                        LifeStealRate = Rate * 70f;
                    }
                }
            }
            else
            {
                if (LifeStealRate < Rate * 80)
                {
                    LifeStealRate += Rate * 0.6f;
                    if (LifeStealRate > Rate * 80)
                    {
                        LifeStealRate = Rate * 80f;
                    }
                }
            }
        }

        public void EnforceScale()
        {
            //FinalScale = ScaleMult * AgeScale;
            DoUpdateGuardianStatus();
            UpdateScale(true, true);
        }

        public void UpdateScale(bool ForceAgeScale = false, bool ForceSetScale = false)
        {
            if (SetToPlayerSize)
            {
                FinalScale = (float)42 / (Base.Height - (Base.Size == GuardianBase.GuardianSize.Large ? 10 : 0));
            }
            else
            {
                FinalScale = ScaleMult;
                //FinalScale *= 3;
            }
            if (ForceAgeScale || WorldMod.DayChange)
            {
                AgeScale = GetAgeSize();
            }
            FinalScale *= AgeScale;
            if (ForceSetScale)
                Scale = FinalScale * ScaleMult;
        }

        public void UpdateExtraStuff()
        {
            if (UpdateAge)
            {
                Data.UpdateAge();
            }
            if(GuardianTownNpcInfoPosition == -1 && !HasCooldown(GuardianCooldownManager.CooldownType.TownNpcInfoCheckingCooldown))
            {
                AddCooldown(GuardianCooldownManager.CooldownType.TownNpcInfoCheckingCooldown, Main.rand.Next(3 * 3600, 5 * 3600));
                TryFindingTownNpcInfo();
            }
            if (OwnerPos > -1)
            {
                bool HasFirstSymbol = Main.player[OwnerPos].GetModPlayer<PlayerMod>().HasFirstSymbol;
                if (HasFirstSymbol != HasFlag(GuardianFlags.FirstSymbolEffect))
                {
                    UpdateStatus = true;
                }
            }
            UpdateLifeStealAndGhostDamageRate();
            UpdateScale();
            if(!UsingFurniture && HasCarpet())
            {
                if (!HasCooldown(GuardianCooldownManager.CooldownType.CarpetFlightTime))
                    AddCooldown(GuardianCooldownManager.CooldownType.CarpetFlightTime, 3000);
                float FlightHeight = (float)Math.Sin((float)GetCooldownValue(GuardianCooldownManager.CooldownType.CarpetFlightTime) * 0.06f) * 4 + 2;
                OffsetY -= FlightHeight;
            }
            if (GravityDirection < 0 && !HasFlag(GuardianFlags.GravityChange))
                FlipGravity();
            if (MoveUp && !LastMoveUp && HasFlag(GuardianFlags.GravityChange))
                FlipGravity();
            if (HasFlag(GuardianFlags.VortexDebuff))
            {
                Velocity.Y = Mass * 0.8f + (float)Math.Cos(CenterPosition.X % 120f / 120f * 6.28318548f) - Mass;
            }
            if (HasFlag(GuardianFlags.LightPotion) || HasFlag(GuardianFlags.MiningHelmet))
            {
                Lighting.AddLight((int)(Position.X * DivisionBy16), (int)(CenterY * DivisionBy16), 0.8f, 0.95f, 1f);
            }
            if (HasFlag(GuardianFlags.Rabid) && Main.rand.Next(1200) == 0)
            {
                float DebuffTime = Main.rand.Next(60, 100) * 0.01f;
                switch (Main.rand.Next(6))
                {
                    case 0:
                        AddBuff(22, (int)(60 * DebuffTime * 3));
                        break;
                    case 1:
                        AddBuff(23, (int)(60 * DebuffTime * 0.75f));
                        break;
                    case 2:
                        AddBuff(31, (int)(60 * DebuffTime * 1.5f));
                        break;
                    case 3:
                        AddBuff(32, (int)(60 * DebuffTime * 3.5f));
                        break;
                    case 4:
                        AddBuff(33, (int)(60 * DebuffTime * 5));
                        break;
                    case 5:
                        AddBuff(35, (int)(60 * DebuffTime * 1));
                        break;
                }
            }
            if (LoadedWorldRegion && HasFlag(GuardianFlags.FlowerBoots) && Velocity.Y == 0)
            {
                int tx = (int)(Position.X * DivisionBy16), ty = (int)((Position.Y - 1) * DivisionBy16);
                if (tx >= Main.leftWorld * DivisionBy16 && tx < Main.rightWorld * DivisionBy16 && ty >= Main.topWorld * DivisionBy16 && ty < Main.bottomWorld * DivisionBy16 - 1)
                {
                    if (Main.tile[tx, ty] == null)
                    {
                        Main.tile[tx, ty] = new Tile();
                    }
                    if (!Main.tile[tx, ty].active() && Main.tile[tx, ty].liquid == 0 && Main.tile[tx, ty + 1] != null && WorldGen.SolidTile(tx, ty + 1))
                    {
                        if (Main.tile[tx, ty + 1].type == 2)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.tile[tx, ty].active(true);
                                Main.tile[tx, ty].type = 3;
                                Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(6, 11));
                                while (Main.tile[tx, ty].frameX == 144)
                                {
                                    Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(6, 11));
                                }
                            }
                            else
                            {
                                Main.tile[tx, ty].active(true);
                                Main.tile[tx, ty].type = 73;
                                Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(6, 21));
                                while (Main.tile[tx, ty].frameX == 144)
                                {
                                    Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(6, 21));
                                }
                            }
                            if (Main.netMode == 1)
                            {
                                //NetMessage.SendTileSquare(-1, tx, ty, 1, TileChangeType.None);
                            }
                        }
                        else if (Main.tile[tx, ty + 1].type == 109)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.tile[tx, ty].active(true);
                                Main.tile[tx, ty].type = 110;
                                Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(4, 7));
                                while (Main.tile[tx, ty].frameX == 90)
                                {
                                    Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(4, 7));
                                }
                            }
                            else
                            {
                                Main.tile[tx, ty].active(true);
                                Main.tile[tx, ty].type = 113;
                                Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(2, 8));
                                while (Main.tile[tx, ty].frameX == 90)
                                {
                                    Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(2, 8));
                                }
                            }
                            if (Main.netMode == 1)
                            {
                                //NetMessage.SendTileSquare(-1, tx, ty, 1, TileChangeType.None);
                            }
                        }
                        else if (Main.tile[tx, ty + 1].type == 60)
                        {
                            Main.tile[tx, ty].active(true);
                            Main.tile[tx, ty].type = 74;
                            Main.tile[tx, ty].frameX = (short)(18 * Main.rand.Next(9, 17));
                            if (Main.netMode == 1)
                            {
                                //NetMessage.SendTileSquare(-1, tx, ty, 1, TileChangeType.None);
                            }
                        }
                    }
                }
            }
            if (HasFlag(GuardianFlags.BeetleOffenseEffect))
            {
                if(HasCooldown(GuardianCooldownManager.CooldownType.BeetleCounter))
                    DecreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, 3 + GetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCountdown) / 10);
                IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCountdown);
                const int T1 = 400, T2 = 1200, T3 = 4600;
                if (GetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter) > T1 + T2 * 2 + T3)
                {
                    SetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, T1 + T2 * 2 + T3);
                }
                int cdValue = GetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter);
                byte BuffID = 0;
                if (cdValue >= T1 + T2 + T3)
                {
                    AddBuff(100, 5, true);
                    BuffID = 3;
                }
                else if (cdValue >= T1 + T2)
                {
                    AddBuff(99, 5, true);
                    BuffID = 2;
                }
                else if (cdValue >= T1)
                {
                    AddBuff(98, 5, true);
                    BuffID = 1;
                }
                if (BuffID > BeetleOrb)
                    RemoveCooldown(GuardianCooldownManager.CooldownType.BeetleCountdown);
                if (BuffID != BeetleOrb)
                {
                    for (int b = 0; b < 3; b++)
                    {
                        if (b != BuffID - 1 && HasBuff(98 + b))
                        {
                            RemoveBuff(98 + b);
                        }
                    }
                }
                if (BuffID > 0)
                    BeetleOrb = (byte)(BuffID - 1);
                else
                    BeetleOrb = 0;
            }
            if (HasFlag(GuardianFlags.BeetleDefenseEffect))
            {
                IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter);
                const int MaxValue = 180;
                BeetleOrb = (byte)(GetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter) / MaxValue);
                //if (GetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter) >= MaxValue)
                {
                    if (BeetleOrb > 0 && BeetleOrb < 3)
                    {
                        for (int b = 0; b < 3; b++)
                        {
                            if (BeetleOrb != b && HasBuff(95 + b))
                                RemoveBuff(95 + b);
                        }
                    }
                    if (BeetleOrb > 0)
                    {
                        if (BeetleOrb <= 3)
                        {
                            AddBuff(94 + BeetleOrb, 5, true);
                            //RemoveCooldown(GuardianCooldownManager.CooldownType.BeetleCounter);
                        }
                        else
                        {
                            BeetleOrb = 3;
                            SetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, MaxValue * 3);
                        }
                    }
                }
            }
            if (HasFlag(GuardianFlags.ChlorophyteSetEffect) && !HasCooldown(GuardianCooldownManager.CooldownType.PetalCooldown))
            {
                //shot petal
            }
            if (SelectedItem > -1 && Inventory[SelectedItem].type == Terraria.ID.ItemID.PsychoKnife)
            {
                if (ItemAnimationTime > 0)
                {
                    SetCooldownValue(GuardianCooldownManager.CooldownType.StealthTime, 15);
                    if (Stealth > 0f)
                        Stealth += 0.1f;
                }
                else if (Velocity.X > -0.1f && Velocity.X < 0.1f && Velocity.Y > 0.1f && Velocity.Y < 0.1f)
                {
                    if (!HasCooldown(GuardianCooldownManager.CooldownType.StealthTime) && Stealth > 0f)
                    {
                        Stealth -= 0.02f;
                        if (Stealth < 0)
                        {
                            Stealth = 0;
                        }
                    }
                }
                else
                {
                    if (Stealth > 0)
                        Stealth += 0.1f;
                }
                if (Stealth > 1f)
                    Stealth = 1f;
            }
            else if (HasFlag(GuardianFlags.ShroomiteSetEffect))
            {
                if (ItemAnimationTime > 0)
                {
                    SetCooldownValue(GuardianCooldownManager.CooldownType.StealthTime, 5);
                }
                if (Velocity.X > -0.1f && Velocity.X < 0.1f && Velocity.Y > 0.1f && Velocity.Y < 0.1f)
                {
                    if (GetCooldownValue(GuardianCooldownManager.CooldownType.StealthTime) == 0)
                    {
                        this.Stealth -= 0.015f;
                        if (Stealth < 0)
                            Stealth = 0;
                    }
                }
                else
                {
                    float Deduction = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y);
                    Stealth += Deduction * 0.0075f;
                    if (Stealth > 1f)
                        Stealth = 1f;
                }
            }
            if (LoadedWorldRegion && ZoneSandstorm)
            {
                int CenterX = (int)(Position.X * DivisionBy16), CenterY = (int)(this.CenterY * DivisionBy16);
                if (Main.tile[CenterX, CenterY].wall == 0)
                {
                    AddBuff(194, 2, true);
                }
            }
            /*if (LoadedWorldRegion && HasCarpet())
            {
                int TileX = (int)(Position.X * DivisionBy16) - 1, TileY = (int)(Position.Y * DivisionBy16) + 1;
                bool HasSolidGround = false;
                for (int t = 0; t < 2; t++)
                {
                    if (Main.tile[TileX + t, TileY].active() && (Main.tileSolid[Main.tile[TileX + t, TileY].type] || Main.tileSolidTop[Main.tile[TileX + t, TileY].type]))
                    {
                        HasSolidGround = true;
                    }
                }
                TileX += 1 + 2 * Direction;
                TileY -= 5;
                if (HasSolidGround && !(Main.tile[TileX, TileY].active() && Main.tileSolid[Main.tile[TileX, TileY].type]))
                {
                    Velocity.Y -= Mass + 0.5f;
                    if (Velocity.Y < -(Mass + 0.5f))
                        Velocity.Y = -(Mass + 0.5f);
                    SetFallStart();
                }
            }*/
            if (SunflowerNearby)
                AddBuff(Terraria.ID.BuffID.Sunflower, 5, true);
            if (ZoneWaterCandle)
                AddBuff(Terraria.ID.BuffID.WaterCandle, 5, true);
            if (ZonePeaceCandle)
                AddBuff(Terraria.ID.BuffID.PeaceCandle, 5, true);
            int DamageStackLevelValue = (100 + 10 * (FriendshipLevel - 1));
            if (DamageStacker >= DamageStackLevelValue)
            {
                DamageStacker -= DamageStackLevelValue;
                IncreaseFriendshipProgress(1);
            }
            if (MainMod.UsingGuardianNecessitiesSystem)
            {
                GuardianMoodAndStatus();
            }
            UpdateSpelunkerGlowstickEffect();
            Base.GuardianUpdateScript(this);
            if(LoadedWorldRegion)
                UpdateComfortStack();
            ManageTrail();
            ManagePulse();
            UpdateTileBuff();
            //FloorVisual(Velocity.Y != 0);
            if(_InternalCarryTimer > 0)
            {
                _InternalCarryTimer--;
                if (_InternalCarryTimer == 0)
                    _CarriedByGuardianID = -1;
            }
        }

        public void UpdateTileBuff()
        {
            if (FireplaceNearby || GetTileCount(Terraria.ID.TileID.Campfire) > 0)
                AddBuff(Terraria.ID.BuffID.Campfire, 5, true);
            if (GetTileCount(Terraria.ID.TileID.Sunflower) > 0)
                AddBuff(Terraria.ID.BuffID.Sunflower, 5, true);
            if (HeartLanternNearby)
                AddBuff(Terraria.ID.BuffID.HeartLamp, 5, true);
            if (StarLanternNearby)
                AddBuff(Terraria.ID.BuffID.StarInBottle, 5, true);
        }

        private void SporeSacScript()
        {
            int Damage = 70;
            float kb = 1.5f;
            if(Main.rand.Next(15) == 0)
            {
                int SporesSpawned = 0;
                for(int i = 0; i < Main.maxProjectiles; i++)
                {
                    if(Main.projectile[i].active && (Main.projectile[i].type == 567 || Main.projectile[i].type == 568) && ProjMod.IsGuardianProjectile(i) && ProjMod.GuardianProj[i].WhoAmID == this.WhoAmID)
                    {
                        SporesSpawned++;
                        if (SporesSpawned >= 10)
                            return;
                    }
                }
            }
        }

        public void ManageTrail()
        {
            int MaxTrails = TrailLength * TrailDelay;
            int TrailCount = Trails.Count;
            if (TrailCount > 0 && TrailCount >= MaxTrails)
                Trails.RemoveAt(0);
            if(MaxTrails > 0 && Trails.Count < MaxTrails)
            {
                TrailPositionLogger trail = new TrailPositionLogger()
                {
                    Position = Position,
                    BodyFrame = BodyAnimationFrame,
                    LeftArmFrame = LeftArmAnimationFrame,
                    RightArmFrame = RightArmAnimationFrame,
                    FacingLeft = LookingLeft
                };
                Trails.Add(trail);
            }
        }

        public void ManagePulse()
        {
            if(PulsePower > 0)
            {
                PulseValue += PulseDir * 0.075f;
                if (PulseValue > 0.9f)
                {
                    PulseDir *= -1;
                    PulseValue = 0.9f;
                }
                if (PulseValue < 0.1f)
                {
                    PulseDir *= -1;
                    PulseValue = 0.1f;
                }
            }
            else
            {
                PulseValue = 0;
            }
        }

        public void GuardianMoodAndStatus()
        {
            if (Data.Injury >= GuardianData.HeavyWoundCount)
            {
                AddBuff(ModContent.BuffType<Buffs.VeryWounded>(), 5, true);
            }
            else if (Data.Injury >= GuardianData.LightWoundCount)
            {
                AddBuff(ModContent.BuffType<Buffs.LightWound>(), 5, true);
            }
            if (Data.Fatigue >= GuardianData.HeavyFatigueCount)
            {
                AddBuff(ModContent.BuffType<Buffs.VeryFatigued>(), 5, true);
            }
            else if (Data.Fatigue >= GuardianData.LightFatigueCount)
            {
                AddBuff(ModContent.BuffType<Buffs.LightFatigue>(), 5, true);
            }
            if (OwnerPos == Main.myPlayer || (OwnerPos == -1 && Main.netMode != 1))
            {
                if (WorldMod.HourChange && HasBuff(ModContent.BuffType<giantsummon.Buffs.VeryFatigued>()) && !HasBuff(ModContent.BuffType<Buffs.Sleeping>()) && Main.rand.Next(3) == 0)
                {
                    AddBuff(ModContent.BuffType<Buffs.Sleeping>(), 600 + Main.rand.Next(2400));
                }
                if (HasBuff(ModContent.BuffType<Buffs.VeryWounded>()) && Main.rand.Next(16) == 0 && Math.Abs(Velocity.X) >= 2f)
                {
                    int Pain = (int)Math.Abs(Velocity.X) - 1;
                    Hurt(Pain, 0, false, true, " couldn't endure more the pain.");
                }
                if (MoveLeft || MoveRight || Jump || ItemAnimationTime > 0)
                {
                    ActivityCount++;
                }
            }
            if (OwnerPos > -1 && WorldMod.HourChange)
            {
                const int ActivityCountToFatigue = 600;
                //Data.AddFatigue(1);
                if (ActivityCount >= ActivityCountToFatigue)
                {
                    Data.AddFatigue(1);
                    /*if (ActivityCount >= 1800)
                    {
                        Data.AddFatigue(1);
                    }*/
                }
                ActivityCount = 0;
            }
        }

        public void UpdateSpelunkerGlowstickEffect()
        {
            if ((SelectedItem > -1 && Inventory[SelectedItem].type == 3002) || (SelectedOffhand > -1 && Inventory[SelectedOffhand].type == 3002))
                IncreaseCooldownValue(GuardianCooldownManager.CooldownType.SpelunkerEffect);
            if (GetCooldownValue(GuardianCooldownManager.CooldownType.SpelunkerEffect) >= 10)
            {
                RemoveCooldown(GuardianCooldownManager.CooldownType.SpelunkerEffect);
                const int range = 30;
                int cx = (int)(this.Position.X * DivisionBy16),
                    cy = (int)(this.CenterY * DivisionBy16);
                for (int x = cx - range; x < cx + range; x++)
                {
                    for (int y = cy - range; y < cy + range; y++)
                    {
                        if (Main.rand.Next(4) == 0)
                        {
                            float Length = new Vector2(cx - x, cy - y).Length();
                            if (Length < range && x > 0 && x < Main.maxTilesX - 1 && y > 0 && y < Main.maxTilesY - 1)
                            {
                                Tile tile = MainMod.GetTile(x, y);
                                if (tile != null && tile.active())
                                {
                                    bool SpelunkerOre = false;
                                    if (tile.type == 185 && tile.frameY == 18 && tile.frameX >= 576 && tile.frameX <= 882)
                                        SpelunkerOre = true;
                                    else if (tile.type == 186 && tile.frameX >= 864 && tile.frameX <= 1170)
                                    {
                                        SpelunkerOre = true;
                                    }
                                    else
                                    {
                                        SpelunkerOre = Main.tileSpelunker[tile.type] || Main.tileAlch[tile.type] && tile.type != 82;
                                    }
                                    if (SpelunkerOre)
                                    {
                                        int d = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, 204, 0, 0, 150, default(Color), 0.3f);
                                        Main.dust[d].fadeIn = 0.75f;
                                        Main.dust[d].velocity *= 0.1f;
                                        Main.dust[d].noLight = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void BePulledByPlayer()
        {
            if (OwnerPos > -1 && !PlayerMounted && !SittingOnPlayerMount)
            {
                if (!IsBeingPulledByPlayer && (!KnockedOut || !HasFlag(GuardianFlags.NotPulledWhenKOd)))
                {
                    Paths.Clear();
                    IsBeingPulledByPlayer = true;
                    SuspendedByChains = false;
                }
            }
        }

        public void CheckForOutOfBounds()
        {
            if (Position.X - Width * 0.5f < Main.leftWorld + 640 + 16)
            {
                Position.X = Main.leftWorld + 640 + 16 + Width * 0.5f;
                Velocity.X = 0;
            }
            if (Position.X + Width * 0.5f >= Main.rightWorld - 640 - 32)
            {
                Position.X = Main.rightWorld - 640 - 32 - Width * 0.5f;
                Velocity.X = 0;
            }
            if (Position.Y - Height < Main.topWorld + 640 + 16)
            {
                Position.Y = Main.topWorld + 640 + 16 + Height;
                if(Velocity.Y < Mass)
                    Velocity.Y = Mass;
                GravityDirection = 1;
            }
            if (Position.Y > Main.bottomWorld - 640 - 32)
            {
                Position.Y = Main.bottomWorld - 640 - 32;
                Velocity.Y = 0;
                CollisionY = true;
            }
        }

        public void DoSitOnPlayerMount(bool Value)
        {
            if (Value && HasFlag(GuardianFlags.DisableMountSharing))
                return;
            SittingOnPlayerMount = Value;
            PlayerMounted = false;
            PlayerControl = false;
            if (!Value)
            {
                Position.X = Main.player[OwnerPos].position.X + Main.player[OwnerPos].width * 0.5f;
                Position.Y = Main.player[OwnerPos].position.Y + Main.player[OwnerPos].height - 1;
            }
        }

        public bool UpdatePullByPlayer()
        {
            bool IgnoreCollisions = false;
            if(IsBeingPulledByPlayer && IsCommander)
            {
                IsBeingPulledByPlayer = false;
                if (GetCommandingOrder == PlayerMod.CommandingOrders.FollowLeader)
                    TeleportToPlayer(true, Main.player[GetCommanderLeaderID]);
                return true;
            }
            if (Downed || OwnerPos == -1)
                IsBeingPulledByPlayer = false;
            if (IsBeingPulledByPlayer && OwnerPos > -1)
            {
                Player p = Main.player[OwnerPos];
                if (p.dead)
                {
                    IsBeingPulledByPlayer = false;
                    return false;
                }
                if (!p.gross && WofFacing)
                {
                    IsBeingPulledByPlayer = false;
                    return false;
                }
                Vector2 ResultingPosition = p.Center;
                Vector2 MovementDirection = (ResultingPosition - CenterPosition);
                float Distance = MovementDirection.Length();
                float Speed = 12f;
                if (furniturex > -1)
                {
                    if (UsingFurniture)
                        LeaveFurniture();
                    else
                    {
                        furniturex = furniturey = -1;
                    }
                }
                //if (Distance > 512) Speed = 8f;
                if (Distance >= 1512f || p.GetModPlayer<PlayerMod>().KnockedOut)
				{
					TeleportToPlayer();
					return false;
				}
                if (Speed < p.velocity.Length())
                    Speed = p.velocity.Length() + 6f;
                bool PlayerIsFlying = p.velocity.Y != 0 || p.pulley,
                    PlayerInAMount = p.mount.Active || p.GetModPlayer<PlayerMod>().MountedOnGuardian,
                    PlayerInMinecart = p.mount.Active && p.mount.Cart,
                    IsCommander = p.GetModPlayer<PlayerMod>().IsCompanionParty;
                bool CollidingWithSolidTile = (LoadedWorldRegion && Collision.SolidCollision(TopLeftPosition, Width, Height));
                if (CollidingWithSolidTile || IsCommander || (PlayerInAMount && (Data.SitOnTheMount || PlayerInMinecart)) ||!PlayerIsFlying || Distance >= 144f)
                {
                    DashCooldown = 30;
                    MovementDirection.Normalize();
                    if (SuspendedByChains && !PlayerInAMount && PlayerIsFlying)
                    {
                        //Vector2 MoveDir = MovementDirection * (Distance - 144);
                        Velocity.Y -= Mass;
                        Velocity += MovementDirection * (Distance - 144) * 0.02f;
                    }
                    else
                    {
                        MovementDirection *= Speed;
                        MovementDirection.Y -= Mass;
                        if (PlayerInAMount && (PlayerInMinecart || Data.SitOnTheMount) && (CenterPosition - p.Center).Length() <= Width)
                        {
                            DoSitOnPlayerMount(true);
                            IsBeingPulledByPlayer = false;
                            SetStuckCheckPositionToMe();
                        }
                        Velocity += MovementDirection;
                        if (Velocity.Length() > MovementDirection.Length())
                            Velocity = MovementDirection;
                    }
                    Position += Velocity;
                    IgnoreCollisions = true;
                }
                else
                {
                    if (SuspendedByChains)
                    {
                        //Position += Distance.ToRotationVector2();
                        //Velocity.Y = 0;
                    }
                    else
                    {
                        SuspendedByChains = true;
                    }
                }
                SetFallStart();
                if (Distance < Speed * 2 && ((!PlayerIsFlying && !PlayerInMinecart) || IsCommander))
                {
                    IsBeingPulledByPlayer = false;
                    FallProtection = true;
                    TeleportToPlayer();
                    Velocity = Main.player[OwnerPos].velocity;
                    if (PlayerInAMount && Data.SitOnTheMount)
                        DoSitOnPlayerMount(true);
                }
            }
            return IgnoreCollisions;
        }

        public bool CheckForPassingWof()
        {
            bool GotByWofTongue = false;
            if (Main.wof >= 0 && Main.npc[Main.wof].active)
            {
                NPC wof = Main.npc[Main.wof];
                float WallStartX = wof.position.X + 40f;
                if (wof.direction > 0)
                    WallStartX -= 96f;
                if (WofFacing)
                    WofFacing = true;
                if (WofFacing && this.HitBox.X + this.Width > WallStartX && this.HitBox.X < WallStartX + 140)
                {
                    this.Hurt(50, wof.direction, false, false, " was unable to endure so much pushing around...");
                }
                if (!WofFacing && TopLeftPosition.Y > (Main.maxTilesY - 250) * 16 && Math.Abs(CenterPosition.X - WallStartX) < 1920)
                {
                    WofFacing = true;
                }
                if (WofFood)
                {
                    Position = Main.npc[Main.wof].Center;
                    Position.Y -= Height * 0.5f;
                    Velocity = Vector2.Zero;
                    KnockedOut = KnockedOutCold = true;
                    HP = 0;
                    if (wof.position.X < 608 || wof.position.X > (Main.maxTilesX - 38) * 16)
                    {
                        DoForceKill(" couldn't be saved from Wall of Flesh in time...");
                    }
                }
                else
                {
                    if (WofFacing)
                    {
                        if (TopLeftPosition.Y < (Main.maxTilesY - 200) * 16)
                        {
                            WofTongued = true;
                        }
                        if ((wof.direction < 0 && Position.X > wof.Center.X + 40) ||
                            (wof.direction > 0 && Position.X < wof.Center.X - 40))
                            WofTongued = true;
                    }
                    if (WofTongued)
                    {
                        WofTongued = true;
                        GotByWofTongue = true;
                        Action = false;
                        float difx = wof.Center.X - Position.X, dify = wof.Center.Y - CenterY;
                        float Distance = (wof.Center - CenterPosition).Length();
                        if (Distance > 3000)
                        {
                            DoForceKill(" has tried to escape.");
                        }
                        else if (wof.position.X < 608 || wof.position.X > (Main.maxTilesX - 38) * 16)
                        {
                            Knockout(" didn't liked the tour.");
                        }
                        else
                        {
                            Vector2 ResultingPosition = wof.Center;
                            //if (!KnockedOut)
                            ResultingPosition.X += wof.direction * 200;
                            float Speed = 11f;
                            Vector2 MovementDirection = (ResultingPosition - CenterPosition);
                            Distance = MovementDirection.Length();
                            MovementDirection.Normalize();
                            Velocity = MovementDirection * Speed;
                            Position += Velocity;
                            if (KnockedOut)
                            {
                                ResultingPosition = wof.Center;
                                MovementDirection = (ResultingPosition - CenterPosition);
                                Distance = MovementDirection.Length();
                                MovementDirection.Normalize();
                                Position.X += MovementDirection.X * Speed;
                            }
                            if (Distance < Speed)
                            {
                                if (KnockedOut)
                                {
                                    Main.PlaySound(Terraria.ID.SoundID.NPCDeath13, ResultingPosition);
                                    Main.NewText(Name + " was swallowed by the Wall of Flesh.", Color.Red);
                                    WofFood = true;
                                }
                                else
                                {
                                    WofTongued = false;
                                    GotByWofTongue = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                WofTongued = WofFacing = WofFood = false;
            }
            return GotByWofTongue;
        }

        private static Vector2 MoveDir = Vector2.Zero;

        public bool MoveCursorToPosition(Vector2 TargetPosition, int Width = 0, int Height = 0) //TODO - Rework the Aiming position to take a position around the character, instead.
        {
            bool ReachedLocation = false;
            MoveDir = Vector2.Zero;
            int Width2 = Width, Height2 = Height;
            TargetPosition.X -= Position.X;
            TargetPosition.Y -= CenterY;
            /*if (Width2 < 40)
                Width2 = 40;
            if (Height2 < 40)
                Height2 = 40;*/
            if (AimDirection.X >= TargetPosition.X + Width2 * 0.2f && AimDirection.X < TargetPosition.X + Width2 * 0.8f && AimDirection.Y >= TargetPosition.Y + Height2 * 0.2f && AimDirection.Y < TargetPosition.Y + Height2 * 0.8f)
            {
                if (AimDirection.X != (int)(TargetPosition.X + Width2 * 0.5f) && AimDirection.Y != (int)(TargetPosition.Y + Height2 * 0.5f))
                {
                    MoveDir.X = (TargetPosition.X + Width2 * 0.5f - AimDirection.X);
                    MoveDir.Y = (TargetPosition.Y + Height2 * 0.5f - AimDirection.Y);
                }
                ReachedLocation = true;
            }
            else
            {
                MoveDir.X = (TargetPosition.X + Width2 * 0.5f - AimDirection.X);
                MoveDir.Y = (TargetPosition.Y + Height2 * 0.5f - AimDirection.Y);
            }
            const float MaxMouseMoveSpeed = 26f;
            if (MoveDir.Length() > MaxMouseMoveSpeed * Agility)
            {
                MoveDir.Normalize();
                MoveDir *= MaxMouseMoveSpeed * Agility;
            }
            else
            {
                MoveDir *= 0.7f * Agility;
            }
            //AimDirection.X += MoveDir.X;
            //AimDirection.Y += MoveDir.Y;
            if (!ReachedLocation && AimDirection.X + MoveDir.X >= TargetPosition.X && AimDirection.X + MoveDir.X < TargetPosition.X + Width2 && AimDirection.Y + MoveDir.Y >= TargetPosition.Y && AimDirection.Y + MoveDir.Y < TargetPosition.Y + Height2)
                return true;
            return ReachedLocation;
        }

        public void CheckFriendshipProgression()
        {
            if (FriendshipLevel < MaxFriendshipLevel && FriendshipProgression >= MaxFriendshipProgression)
            {
                FriendshipProgression -= MaxFriendshipProgression;
                FriendshipLevel++;
                UpdateStatus = true;
                if (Main.netMode < 2 && OwnerPos == Main.myPlayer && (!PlayerMod.HasBuddiesModeOn(Main.player[OwnerPos]) || !PlayerMod.GetPlayerBuddy(Main.player[OwnerPos]).IsSameID(this)))
                {
                    if (FriendshipLevel == Base.CallUnlockLevel && !IsStarter && !PlayerMod.HasBuddiesModeOn(Main.player[OwnerPos]))
                        Main.NewText("You can now call " + this.Name + " to help you.");
                    if (FriendshipLevel == Base.MountUnlockLevel)
                        Main.NewText(this.Name + "'s Mounting ability unlocked.");
                    if (FriendshipLevel == Base.MoveInLevel)
                        Main.NewText(this.Name + " will now agree to live in your world.");
                    if (FriendshipLevel == Base.ControlUnlockLevel && !Base.IsTerrarian)
                        Main.NewText(this.Name + "'s Control ability unlocked.");
                    if (FriendshipLevel == Base.StopMindingAFK)
                        Main.NewText(this.Name + " no longer mind afk in combat.");
                    if (FriendshipLevel == Base.LootingUnlockLevel)
                        Main.NewText(this.Name + " can now collect loot.");
                    if (FriendshipLevel == Base.FriendshipBondUnlockLevel)
                        Main.NewText(this.Name + " now gained a status buff based on their best friend status.");
                    if (FriendshipLevel == Base.FallDamageReductionLevel)
                        Main.NewText(this.Name + " now has received some fall damage tolerance, and the impact of it on you greatly reduces too.");
                    if (FriendshipLevel == Base.MountDamageReductionLevel)
                        Main.NewText("You will receive less damage while mounted on " + this.Name + ".");
                    if (FriendshipLevel == Base.MaySellYourLoot)
                        Main.NewText("You can now send " + this.Name + " to the town to sell your loot.");
                    if (FriendshipLevel == Base.LeadGroupUnlockLevel)
                        Main.NewText(this.Name + " can now lead a group for you.");
                    if (FriendshipLevel == Base.KnownLevel || FriendshipLevel == Base.FriendsLevel || FriendshipLevel == Base.BestFriendLevel || FriendshipLevel == Base.BestFriendForeverLevel || FriendshipLevel == Base.BuddiesForLife)
                        Main.NewText("Your friendship with " + this.Name + " has been improved.");
                }
            }
        }

        public bool TriggerCover(int Damage, int DamageDirection)
        {
            bool Trigger = false;
            if (!Downed && Main.rand.NextDouble() * 100 < CoverRate && Math.Abs(Position.X - Main.player[OwnerPos].Center.X) < 1248f && Math.Abs(CenterY - Main.player[OwnerPos].Center.Y) < 768f && !GrabbingPlayer && !PlayerControl)
            {
                Trigger = true;
                CombatText.NewText(Main.player[OwnerPos].getRect(), Color.Silver, "Cover");
                Vector2 FXStartPos = Main.player[OwnerPos].Center, FXEndPos = CenterPosition, FXDirection = FXEndPos - FXStartPos;
                FXDirection.Normalize();
                float EffectDistance = 8f;
                Color c = new Color();
                byte maxcount = 0;
                while (Math.Abs((FXStartPos - FXEndPos).Length()) > EffectDistance)
                {
                    Vector2 EffectSpawnPos = FXStartPos;
                    EffectSpawnPos.X += Main.rand.Next(-5, 6);
                    EffectSpawnPos.Y += Main.rand.Next(-5, 6);
                    int i = Dust.NewDust(EffectSpawnPos, 1, 1, 235, 0, 0, 0, c, 1f);
                    FXStartPos += EffectDistance * FXDirection;
                    maxcount++;
                    if (maxcount >= 50) //In case something wrong happens.
                        break;
                }
                //LookingLeft = Main.player[OwnerPos].direction == -1;
                int Result = Hurt(Damage, DamageDirection, false, true, " was slain while protecting " + Main.player[OwnerPos].name + ".");
                IncreaseDamageStacker(Result, MHP, true);
            }
            return Trigger;
        }

        public void WhenPlayerDies()
        {
            if(!KnockedOut)
                DisplayEmotion(Emotions.Sad);
        }

        public void GetBuddyModeBenefits(out float HealthBonus, out float DamageBonus, out int DefenseBonus)
        {
            float Effective = Main.player[OwnerPos].GetModPlayer<PlayerMod>().BuddiesModeEffective;
            HealthBonus = FriendshipLevel * 0.01f * Effective;
            DamageBonus = FriendshipLevel * 0.01f * Effective;
            DefenseBonus = (int)(FriendshipLevel * 0.3334f * Effective);
        }

        public void DoUpdateGuardianStatus()
        {
            UpdateStatus = false;
            BuffImmunity.Clear();
            HitBox.Width = Width;
            HitBox.Height = Height;
            MoveSpeed = 1f;
            MaxSpeed = Base.MaxSpeed;
            Mass = 0.4f;//Base.Mass;
            Acceleration = Base.Acceleration;
            SlowDown = Base.SlowDown;
            MaxJumpHeight = Base.MaxJumpHeight;
            MaxFallSpeed = DefaultMaxFallSpeed * ((1f / 0.3f) * Base.Mass);
            JumpSpeed = Base.JumpSpeed;
            FallHeightTolerance = 15;
            ScaleMult = Base.GetScale;
            bool LastAgeScaleWas0 = AgeScale == 0;
            AgeScale = GetAgeSize();
            if (LastAgeScaleWas0)
                Scale = FinalScale = AgeScale;
            TrailLength = TrailDelay = 0;
            PulsePower = 0;
            Base.GuardianResetStatus(this);

            DashSpeed = 0;
            WingMaxFlightTime = 0;
            WingType = 0;
            RocketType = 0;
            RocketMaxTime = 7;
            MeleeSpeed = RangedSpeed = MagicSpeed = 1f;
            Accuracy = Base.Accuracy;
            Agility = Base.Agility;
            Trigger = Base.Trigger;
            ManaRegenBonus = 0;
            float LastHealthPercentage = 1f, LastManaPercentage = 1f;
            if (HP < MHP) LastHealthPercentage = (float)HP / MHP;
            if (MP < MMP) LastManaPercentage = (float)MP / MMP;
            if (MainMod.SetGuardiansHealthAndManaToPlayerStandards)
            {
                MHP = 100 + 20 * LifeCrystalHealth + 5 * LifeFruitHealth;
                MMP = 20 + 20 * ManaCrystals;
                HealthHealMult = 1f;
                ManaHealMult = 1f;
            }
            else
            {
                MHP = (int)(Base.InitialMHP + Base.LifeCrystalHPBonus * LifeCrystalHealth + Base.LifeFruitHPBonus * LifeFruitHealth);
                MMP = (int)(Base.InitialMP + Base.ManaCrystalMPBonus * ManaCrystals);
                HealthHealMult = (Base.InitialMHP + Base.LifeCrystalHPBonus * 15 + Base.LifeFruitHPBonus * 20) * (1f / 500);
                ManaHealMult = Base.InitialMP * (1f / 20);
            }
            if (OwnerPos > -1)
                MHP += (int)(Main.player[OwnerPos].GetModPlayer<PlayerMod>().ExtraMaxHealthValue * HealthHealMult);
            Aggro = 0;
            Defense = 0;
            BlockRate = Base.BlockRate;
            DodgeRate = Base.DodgeRate;
            CoverRate = 0;
            BreathMax = Base.MaxBreath;
            MaxMinions = 1;
            MaxSentries = 1;
            MeleeDamageMultiplier = RangedDamageMultiplier = MagicDamageMultiplier = SummonDamageMultiplier = NeutralDamageMultiplier = 1f;
            MeleeCriticalRate = RangedCriticalRate = MagicCriticalRate = 0;
            MeleeKnockback = RangedKnockback = 1f;
            ManaCostMult = 1f;
            ShotSpeedMult = 1f;
            FlagList.Clear();
            byte HeavyArmorPieces = 0;
            for (int e = 0; e < 9; e++)
            {
                if (Equipments[e].type > 0 && (e < 8 || (e == 8 && ExtraAccessorySlot && Main.expertMode)))
                {
                    Defense += Equipments[e].defense;
                    if (MainMod.IsGuardianItem(Equipments[e]))
                    {
                        if (e < 3 && Items.GuardianItemPrefab.IsHeavyItem(Equipments[e]))
                        {
                            HeavyArmorPieces++;
                        }
                        (Equipments[e].modItem as Items.GuardianItemPrefab).ItemStatusScript(this);
                    }
                    GuardianAccessoryEffects.ApplyAccessoryEffect(this, Equipments[e]);
                    GuardianArmorAndSetEffects.GetArmorPieceEffect(this, Equipments[e]);
                    GuardianArmorAndSetEffects.DoPrefixEffect(Equipments[e], this);
                    if (Equipments[e].wingSlot > 0)
                        WingType = Equipments[e].wingSlot;
                }
            }
            if (OwnerPos > -1 && PlayerMod.HasBuddiesModeOn(Main.player[OwnerPos]) && PlayerMod.GetPlayerBuddy(Main.player[OwnerPos]).IsSameID(this))
            {
                float HealthMod, DamageMod;
                int DefenseMod;
                GetBuddyModeBenefits(out HealthMod, out DamageMod, out DefenseMod);
                MHP += (int)(HealthMod * MHP);
                MeleeDamageMultiplier += DamageMod;
                RangedDamageMultiplier += DamageMod;
                MagicDamageMultiplier += DamageMod;
                SummonDamageMultiplier += DamageMod;
                Defense += DefenseMod;
            }
            GuardianArmorAndSetEffects.GetArmorSetEffect(this);
            if (Main.expertMode)
            {
                Defense += (int)(Defense * 0.5f);
                if (false && !PlayerControl)
                {
                    MHP *= 2;
                }
            }
            if (SelectedOffhand > -1 && MainMod.IsGuardianItem(Inventory[SelectedOffhand]))
            {
                ((Items.GuardianItemPrefab)Inventory[SelectedOffhand].modItem).ItemStatusScript(this);
                BlockRate += ((Items.GuardianItemPrefab)Inventory[SelectedOffhand].modItem).BlockRate;
            }
            if (HasFlag(GuardianFlags.JumpHeightBoost))
            {
                MaxJumpHeight = (int)(MaxJumpHeight * 1.333f);
                JumpSpeed *= 1.3f;
            }
            if ((Main.dayTime && HasFlag(GuardianFlags.SunBuff)) || (!Main.dayTime && HasFlag(GuardianFlags.MoonBuff)))
            {
                Defense += 4;
                MeleeDamageMultiplier += 0.1f;
                RangedDamageMultiplier += 0.1f;
                MagicDamageMultiplier += 0.1f;
                SummonDamageMultiplier += 0.1f;
                MeleeSpeed += 0.1f;
                RangedSpeed += 0.1f;
                MagicSpeed += 0.1f;
            }
            DefenseRate = (Defense * 0.2f) * 0.01f;
            //if (DefenseRate > 0.9f)
            //    DefenseRate = 0.9f;
            if (OwnerPos > -1 && HasFlag(GuardianFlags.FriendshipBondBuff))
            {
                //float StatusSum = (Main.player[OwnerPos].meleeDamage + Main.player[OwnerPos].rangedDamage + Main.player[OwnerPos].magicDamage + Main.player[OwnerPos].minionDamage - 4f) * 0.05f;
                MeleeDamageMultiplier += 0.05f; //(Main.player[OwnerPos].meleeDamage) * 0.05f;
                RangedDamageMultiplier += 0.05f; //(Main.player[OwnerPos].rangedDamage) * 0.05f;
                MagicDamageMultiplier += 0.05f; //(Main.player[OwnerPos].magicDamage) * 0.05f;
                SummonDamageMultiplier += 0.05f; //(Main.player[OwnerPos].minionDamage) * 0.05f;
                Defense += 5; //(int)(Main.player[OwnerPos].statDefense * 0.05f);
            }
            if (OwnerPos > -1 && Main.player[OwnerPos].GetModPlayer<PlayerMod>().HasFirstSymbol)
            {
                AddFlag(GuardianFlags.FirstSymbolEffect);
                float SummonDamageBonus = (Main.player[OwnerPos].minionDamage - 1f) * 0.5f;
                MeleeDamageMultiplier += SummonDamageBonus;
                RangedDamageMultiplier += SummonDamageBonus;
                MagicDamageMultiplier += SummonDamageBonus;
                SummonDamageMultiplier += SummonDamageBonus;
            }
            PlayerControlStatusShare();
            if (PlayerMounted && !ReverseMount)
                CoverRate += 20;
            Base.Attributes(this);
            UpdateBuffEffects();
            MeleeSpeed -= 0.05f * HeavyArmorPieces;
            MoveSpeed -= 0.05f * HeavyArmorPieces;
            if (Compatibility.NExperienceCompatibility.IsModActive)
                Compatibility.NExperienceCompatibility.ScaleStatus(this);
            if (MainMod.UseSkillsSystem && !Base.IsTerrarian)
            {
                List<GuardianSkills> SkillList = Data.GetSkillList;
                for (int s = 0; s < SkillList.Count; s++)
                {
                    SkillList[s].OnStatusUpdate(this);
                }
            }
            if (FriendshipLevel >= Base.LootingUnlockLevel)
                AddFlag(GuardianFlags.MayLootItems);
            if (Base.MountUnlockLevel != 255 && FriendshipLevel >= Base.MountUnlockLevel)
                AddFlag(GuardianFlags.AllowMount);
            if (FriendshipLevel >= Base.ControlUnlockLevel)
                AddFlag(GuardianFlags.PlayerControl);
            if (FriendshipLevel >= Base.StopMindingAFK)
                AddFlag(GuardianFlags.StopMindingAfk);
            if (FriendshipLevel >= Base.FriendshipBondUnlockLevel)
                AddFlag(GuardianFlags.FriendshipBondBuff);
            if (FriendshipLevel >= Base.FallDamageReductionLevel)
                AddFlag(GuardianFlags.FallDamageImpactReduction);
            if (FriendshipLevel >= Base.MountDamageReductionLevel)
                AddFlag(GuardianFlags.MountDamageReceivedReduction);
            if (FriendshipLevel >= Base.MaySellYourLoot)
                AddFlag(GuardianFlags.MayGoSellLoot);
            if (KnockedOut && HasFlag(GuardianFlags.CantBeKnockedOutCold))
                AddFlag(GuardianFlags.DontTakeAggro);
            if (KnockedOut && HasFlag(GuardianFlags.CantTakeDamageWhenKod))
            {
                AddFlag(GuardianFlags.CantBeHurt);
            }
            if (HasBuff(Terraria.ID.BuffID.Titan) && !HasFlag(GuardianFlags.TitanGuardian))
                ScaleMult *= 2;
            if (PlayerMounted && !ReverseMount)
                MoveSpeed -= MoveSpeed * Base.MountBurdenPercentage;
            if (Tanker)
                Aggro += TankerAggroBonus;
            if (AvoidCombat)
                Aggro -= 200;
            MeleeSpeed = 1f / MeleeSpeed;
            RangedSpeed = 1f / RangedSpeed;
            MagicSpeed = 1f / MagicSpeed;
            Acceleration *= MoveSpeed;
            MaxSpeed *= MoveSpeed;
            HP = (int)(LastHealthPercentage * MHP);
            MP = (int)(LastManaPercentage * MMP);
            if (KnockedOut)
            {
                Aggro = -50 + Aggro / 2;
                if (HasFlag(GuardianFlags.WaterWalking))
                    RemoveFlag(GuardianFlags.WaterWalking);
            }
        }

        public void PlayerControlStatusShare()
        {
            if (OwnerPos != -1 && PlayerControl)
            {
                Player owner = Main.player[OwnerPos];
                MHP += owner.statLifeMax2;
                MMP += owner.statManaMax2;
                MeleeDamageMultiplier += owner.meleeDamage;
                RangedDamageMultiplier += owner.rangedDamage;
                MagicDamageMultiplier += owner.magicDamage;
                SummonDamageMultiplier += owner.minionDamage;
                Defense += owner.statDefense;
            }
        }

        public void CheckForInformationAccs(Player player)
        {
            if (Math.Abs(player.Center.X - Position.X) >= NPC.sWidth || Math.Abs(player.Center.Y - CenterY) >= NPC.sHeight)
                return;
            List<int> IdList = new List<int>();
            for (int eq = 0; eq < 8; eq++)
            {
                if (Equipments[eq].type != 0 && !IdList.Contains(Equipments[eq].type))
                    IdList.Add(Equipments[eq].type);
            }
            for (int inv = 0; inv < 50; inv++)
            {
                if (Inventory[inv].type != 0 && !IdList.Contains(Inventory[inv].type))
                    IdList.Add(Inventory[inv].type);
            }
            foreach (int id in IdList)
            {
                int type2 = id;
                if ((type2 == 15 || type2 == 707) && player.accWatch < 1)
                {
                    player.accWatch = 1;
                }
                if ((type2 == 16 || type2 == 708) && player.accWatch < 2)
                {
                    player.accWatch = 2;
                }
                if ((type2 == 17 || type2 == 709) && player.accWatch < 3)
                {
                    player.accWatch = 3;
                }
                if (type2 == 393)
                {
                    player.accCompass = 1;
                }
                if (type2 == 18)
                {
                    player.accDepthMeter = 1;
                }
                if (type2 == 395 || type2 == 3123 || type2 == 3124)
                {
                    player.accWatch = 3;
                    player.accDepthMeter = 1;
                    player.accCompass = 1;
                }
                if (type2 == 3120 || type2 == 3036 || type2 == 3123 || type2 == 3124)
                {
                    player.accFishFinder = true;
                }
                if (type2 == 3037 || type2 == 3036 || type2 == 3123 || type2 == 3124)
                {
                    player.accWeatherRadio = true;
                }
                if (type2 == 3096 || type2 == 3036 || type2 == 3123 || type2 == 3124)
                {
                    player.accCalendar = true;
                }
                if (type2 == 3084 || type2 == 3122 || type2 == 3123 || type2 == 3124)
                {
                    player.accThirdEye = true;
                }
                if (type2 == 3095 || type2 == 3122 || type2 == 3123 || type2 == 3124)
                {
                    player.accJarOfSouls = true;
                }
                if (type2 == 3118 || type2 == 3122 || type2 == 3123 || type2 == 3124)
                {
                    player.accCritterGuide = true;
                }
                if (type2 == 3099 || type2 == 3121 || type2 == 3123 || type2 == 3124)
                {
                    player.accStopwatch = true;
                }
                if (type2 == 3102 || type2 == 3121 || type2 == 3123 || type2 == 3124)
                {
                    player.accOreFinder = true;
                }
                if (type2 == 3119 || type2 == 3121 || type2 == 3123 || type2 == 3124)
                {
                    player.accDreamCatcher = true;
                }
                if (type2 == 3619)
                {
                    player.InfoAccMechShowWires = true;
                }
            }
        }

        public void AddBuffImmunity(int ID)
        {
            if (!BuffImmunity.Contains(ID))
                BuffImmunity.Add(ID);
        }

        public void UpdateBuffEffects()
        {
            foreach (BuffData b in Buffs)
            {
                ModBuff mb = ModContent.GetModBuff(b.ID);
                if (mb != null && mb is Buffs.GuardianModBuff)
                {
                    (mb as Buffs.GuardianModBuff).Update(this);
                    continue;
                }
                switch (b.ID)
                {
                    case Terraria.ID.BuffID.ManaSickness:
                        AddFlag(GuardianFlags.ManaSickness);
                        break;
                    case Terraria.ID.BuffID.Dazed:
                        AddFlag(GuardianFlags.Dazed);
                        MoveSpeed *= 0.5f;
                        break;
                    case Terraria.ID.BuffID.Slow:
                        AddFlag(GuardianFlags.Slow);
                        MoveSpeed *= 0.5f;
                        break;
                    case 1:
                        AddFlag(GuardianFlags.FireblocksImmunity);
                        AddFlag(GuardianFlags.LavaImmunity);
                        AddBuffImmunity(24);
                        break;
                    case 2:
                        AddFlag(GuardianFlags.LifeRegenPotion);
                        break;
                    case 3:
                        MoveSpeed += 0.25f;
                        break;
                    case 4:
                        AddFlag(GuardianFlags.BreathUnderwater);
                        break;
                    case 5:
                        Defense += 8;
                        break;
                    case 6:
                        //Mana regen buff
                        break;
                    case 7:
                        MagicDamageMultiplier += 0.2f;
                        break;
                    case 8:
                        AddFlag(GuardianFlags.FeatherfallPotion);
                        break;
                    case 9:
                        AddFlag(GuardianFlags.SpelunkerPotion);
                        break;
                    case 10:
                        AddFlag(GuardianFlags.Invisibility);
                        break;
                    case 11:
                        AddFlag(GuardianFlags.LightPotion);
                        break;
                    case 12:
                        //Night vision potion
                        AddFlag(GuardianFlags.NightVisionPotion);
                        break;
                    case 13:
                        AddFlag(GuardianFlags.EnemySpawnBoostPotion);
                        break;
                    case 14:
                        //Thorns potion
                        AddFlag(GuardianFlags.ThornsEffectPotion);
                        break;
                    case 15:
                        AddFlag(GuardianFlags.WaterWalking);
                        break;
                    case 16:
                        AddFlag(GuardianFlags.ArcheryPotion);
                        break;
                    case 17:
                        //Detect Creature potion
                        AddFlag(GuardianFlags.HunterPotion);
                        break;
                    case 18:
                        //Gravity control potion
                        break;
                    case 20:
                        AddFlag(GuardianFlags.Poisoned);
                        break;
                    case 21:
                        b.Time = GetCooldownValue(GuardianCooldownManager.CooldownType.HealingPotionCooldown);
                        break;
                    case 22:
                        //Blind
                        Accuracy *= 0.7f;
                        AddFlag(GuardianFlags.Blind);
                        break;
                    case 23:
                        AddFlag(GuardianFlags.Cursed);
                        break;
                    case 24:
                        AddFlag(GuardianFlags.OnFire);
                        break;
                    case 28: //Werewolf buff
                        MaxJumpHeight += 2;
                        JumpSpeed += 0.2f;
                        MeleeDamageMultiplier += 0.05f;
                        MeleeSpeed += 0.05f;
                        Defense += 3;
                        Acceleration += 0.05f;
                        AddFlag(GuardianFlags.Werewolf);
                        break;
                    case 34: //Merfolk Buff
                        AddFlag(GuardianFlags.Merfolk);
                        AddFlag(GuardianFlags.SwimmingAbility);
                        break;
                    case 103:
                        AddFlag(GuardianFlags.Dripping);
                        break;
                    case 137:
                        AddFlag(GuardianFlags.DrippingSlime);
                        break;
                    case 68:
                        AddFlag(GuardianFlags.Suffocating);
                        break;
                    case 39:
                        AddFlag(GuardianFlags.OnCursedFire);
                        break;
                    case 44:
                        AddFlag(GuardianFlags.FrostBurn);
                        break;
                    case 48:
                        AddFlag(GuardianFlags.Honey);
                        break;
                    case 163:
                        //Brain Suckler debuff?
                        AddFlag(GuardianFlags.Bleeding);
                        break;
                    case 164:
                        AddFlag(GuardianFlags.VortexDebuff); //?
                        break;
                    case 194:
                        AddFlag(GuardianFlags.WindPushed);
                        break;
                    case 195:
                        AddFlag(GuardianFlags.WitheredArmor);
                        Defense /= 2;
                        DefenseRate *= 0.5f;
                        break;
                    case 196:
                        AddFlag(GuardianFlags.WitheredWeapon);
                        MeleeDamageMultiplier *= 0.5f;
                        RangedDamageMultiplier *= 0.5f;
                        MagicDamageMultiplier *= 0.5f;
                        SummonDamageMultiplier *= 0.5f;
                        break;
                    case 197:
                        AddFlag(GuardianFlags.OgreSpit);
                        MoveSpeed *= 0.33f;
                        break;
                    case 145:
                        AddFlag(GuardianFlags.MoonLeech);
                        break;
                    case 149:
                        AddFlag(GuardianFlags.Webbed);
                        break;
                    case 29:
                        MagicCriticalRate += 2;
                        MagicDamageMultiplier += 0.05f;
                        break;
                    case 33:
                        MeleeDamageMultiplier -= 0.05f;
                        MeleeSpeed -= 0.05f;
                        Defense -= 4;
                        MoveSpeed -= 0.1f;
                        break;
                    case 25:
                        MeleeDamageMultiplier += 0.1f;
                        MeleeSpeed += 0.1f;
                        MeleeCriticalRate += 2;
                        Defense -= 4;
                        break;
                    case 26:
                        Defense += 2;
                        MeleeDamageMultiplier += 0.05f;
                        RangedDamageMultiplier += 0.05f;
                        MagicDamageMultiplier += 0.05f;
                        SummonDamageMultiplier += 0.05f;
                        MeleeCriticalRate += 2;
                        RangedCriticalRate += 2;
                        MagicCriticalRate += 2;
                        MoveSpeed += 0.02f;
                        break;
                    case 71:
                    case 72:
                    case 73:
                    case 74:
                    case 75:
                    case 76:
                    case 77:
                    case 78:
                    case 79:
                        MeleeEnchant = (byte)(b.ID - 70);
                        break;
                    case 205:
                        //Ballista panic
                        break;
                    case 80:
                        //Blackout
                        AddFlag(GuardianFlags.Blackout);
                        Accuracy *= 0.2f;
                        break;
                    case 30:
                        AddFlag(GuardianFlags.Bleeding);
                        break;
                    case 31:
                        AddFlag(GuardianFlags.Confusion);
                        break;
                    case 35:
                        AddFlag(GuardianFlags.Silence);
                        break;
                    case 36:
                        Defense /= 2;
                        DefenseRate *= 0.5f;
                        break;
                    case 46:
                        AddFlag(GuardianFlags.Chilled);
                        break;
                    case 47:
                        AddFlag(GuardianFlags.Frozen);
                        break;
                    case 49:
                        //Pygmy summon buff
                        break;
                    case 58:
                        //Palladium Regen
                        break;
                    case 59:
                        //Shadow Dodge?
                        break;
                    case 62:
                        AddFlag(GuardianFlags.IceTurtleShell);
                        break;
                    case 63:
                        MoveSpeed += 1f;
                        break;
                    case 64:
                        //Slime summon buff
                        break;
                    case 67:
                        AddFlag(GuardianFlags.Burned);
                        break;
                    case 69:
                        AddFlag(GuardianFlags.Ichor);
                        Defense -= 20;
                        break;
                    case 70:
                        AddFlag(GuardianFlags.Venom);
                        break;
                    case 83:
                        //Raven summon buff
                        break;
                    case 88:
                        //Chaos State
                        break;
                    case 93:
                        //Ammo box
                        break;
                    case 95:
                    case 96:
                    case 97:
                        {
                            byte OrbValue = (byte)(1 + b.ID - 95);
                            if (BeetleOrb != OrbValue)
                                b.Time = 0;
                        }
                        break;
                    case 98:
                    case 99:
                    case 100:
                        {
                            byte OrbValue = (byte)(1 + b.ID - 98);
                            MeleeDamageMultiplier += 0.1f * OrbValue;
                            MeleeSpeed += 0.1f * OrbValue;
                            if (BeetleOrb != OrbValue)
                                b.Time = 0;
                        }
                        break;
                    case 104:
                        //Pickaxe Speed
                        break;
                    case 105:
                        //Life magnet
                        break;
                    case 106:
                        AddFlag(GuardianFlags.EnemySpawnNerfPotion);
                        break;
                    case 107:
                        //Tile placement speed
                        break;
                    case 108:
                        AddFlag(GuardianFlags.KnockbackPowerPotion);
                        break;
                    case 109:
                        AddFlag(GuardianFlags.SwimmingAbility);
                        break;
                    case 110:
                        //Minion bonus
                        break;
                    case 111:
                        //Dangersense potion
                        break;
                    case 112:
                        //Ammo Potion...
                        break;
                    case 113:
                        MHP += (int)(MHP * 0.2f);
                        AddFlag(GuardianFlags.LifeForcePotion);
                        break;
                    case 114:
                        DefenseRate += 0.1f;
                        break;
                    case 115:
                        MeleeCriticalRate += 10;
                        RangedCriticalRate += 10;
                        MagicCriticalRate += 10;
                        break;
                    case 116:
                        AddFlag(GuardianFlags.InfernoPotion);
                        break;
                    case 117:
                        MeleeDamageMultiplier += 0.1f;
                        RangedDamageMultiplier += 0.1f;
                        MagicDamageMultiplier += 0.1f;
                        SummonDamageMultiplier += 0.1f;
                        break;
                    case 119:
                        AddFlag(GuardianFlags.LovestruckPotion);
                        break;
                    case 120:
                        AddFlag(GuardianFlags.StinkyPotion);
                        break;
                    case 121:
                        //Fishing skill bonus +15
                        break;
                    case 122:
                        //Sonar potion effect
                        break;
                    case 123:
                        //Crate potion effect
                        break;
                    case 124:
                        AddFlag(GuardianFlags.ResistColdPotion);
                        break;
                    case 125:
                        //Hornet summon buff
                        break;
                    case 126:
                        //Imp summon buff
                        break;
                    case 133:
                        //Spider summon buff
                        break;
                    case 134:
                        //Twins summon buff
                        break;
                    case 135:
                        //Pirate summon buff
                        break;
                    case 139:
                        //Sharknado summon buff
                        break;
                    case 140:
                        //Ufo summon buff
                        break;
                    case 141:
                        AddFlag(GuardianFlags.Rabid);
                        MeleeDamageMultiplier += 0.2f;
                        RangedDamageMultiplier += 0.2f;
                        MagicDamageMultiplier += 0.2f;
                        SummonDamageMultiplier += 0.2f;
                        break;
                    case 144:
                        AddFlag(GuardianFlags.Electrified);
                        break;
                    case 146:
                        AddFlag(GuardianFlags.SunflowerBuff);
                        MoveSpeed += 0.1f;
                        MoveSpeed *= 1.1f;
                        break;
                    case 150:
                        //Minion bonus
                        break;
                    case 156:
                        AddFlag(GuardianFlags.Petrified);
                        break;
                    case 161:
                        //Deadly Sphere summon
                        break;
                    case 165:
                        Defense += 8;
                        AddFlag(GuardianFlags.DryadWard);
                        break;
                    case 170:
                    case 171:
                    case 172:
                        //Solar armor buff
                        break;
                    case 173:
                    case 174:
                    case 175:
                        //Nebula armor life buff
                        break;
                    case 176:
                    case 177:
                    case 178:
                        //Nebula armor mana buff
                        break;
                    case 179:
                    case 180:
                    case 181:
                        //Nebula armor damage buff
                        break;
                    case 182:
                        //Cell summon
                        break;
                    case 186:
                        AddFlag(GuardianFlags.DryadBane);
                        break;
                    case 187:
                        //Stardust Dragon
                        break;
                    case 188:
                        //Stardust dragon summon
                        break;
                }
            }
        }

        public bool IsTileInSameHouse(int StartX, int StartY, int TileX, int TileY)
        {
            const byte MaxCheckDistance = 40;
            List<Node> CurNodes = new List<Node>(), NextNodes = new List<Node>();
            while (true)
            {
                Tile floortile = MainMod.GetTile(StartX, StartY + 1);
                if (!floortile.active() || !Main.tileSolid[floortile.type])
                {
                    StartY++;
                }
                else
                {
                    break;
                }
                if (StartY >= Main.bottomWorld)
                    return false;
            }
            NextNodes.Add(new Node((ushort)StartX, (ushort)StartY, 0, Node.StepDirection.Left));
            NextNodes.Add(new Node((ushort)StartX, (ushort)StartY, 0, Node.StepDirection.Right));
            NextNodes.Add(new Node((ushort)StartX, (ushort)StartY, 0, Node.StepDirection.Up));
            NextNodes.Add(new Node((ushort)StartX, (ushort)StartY, 0, Node.StepDirection.Down));
            while (NextNodes.Count > 0)
            {
                CurNodes = NextNodes;
                NextNodes = new List<Node>();
                while (CurNodes.Count > 0)
                {
                    Node n = CurNodes[0];
                    byte Step = n.TileStep;
                    ushort tx = n.px, ty = n.py;
                    Gore.NewGorePerfect(new Vector2(tx * 16 + 8, ty * 16 + 8), Vector2.Zero, Terraria.ID.GoreID.ButcherSaw);
                    if (tx == TileX && ty == TileY)
                        return true;
                    CurNodes.RemoveAt(0);
                    if (Step >= MaxCheckDistance) continue;
                    switch (n.direction)
                    {
                        case Node.StepDirection.Left:
                            if (tx > Main.leftWorld)
                            {
                                Tile floortile = MainMod.GetTile(tx, ty + 1);
                                if (floortile.active() && floortile.type == Terraria.ID.TileID.Platforms) //Check for stairs
                                {
                                    Node newnode = new Node((ushort)(tx - 1), ty, (byte)(Step + 1), Node.StepDirection.Left);
                                    NextNodes.Add(newnode);
                                    for (int i = -1; i < 2; i += 2)
                                    {
                                        if (i == -1 && tx <= Main.leftWorld)
                                            continue;
                                        ushort ntx = (ushort)(i + tx), nty = (ushort)(ty);
                                        Tile othertile = MainMod.GetTile(ntx, nty);
                                        if (othertile.active() && floortile.type == Terraria.ID.TileID.Platforms)
                                        {
                                            newnode = new Node(tx, (ushort)(ty - 1), (byte)(Step + 1), (i == -1 ? Node.StepDirection.Left : Node.StepDirection.Right));
                                            NextNodes.Add(newnode);
                                        }
                                    }
                                }
                                else
                                {
                                    tx--;
                                    for (int y = 0; y < 8; y++)
                                    {
                                        int nty = ty - (y + 1);
                                        Tile tile = MainMod.GetTile(tx, nty);
                                        if (tile.active() && tile.type == Terraria.ID.TileID.Platforms)
                                        {
                                            Node newnode = new Node(tx, (ushort)(nty - 1), (byte)(Step + 1), Node.StepDirection.Up);
                                            NextNodes.Add(newnode);
                                        }
                                    }
                                    if (MainMod.IsSolidTile(tx, ty))
                                    {
                                        bool Blocked = false;
                                        for (int y = 0; y < 5; y++)
                                        {
                                            ty--;
                                            Blocked = MainMod.IsSolidTile(tx, ty);
                                            if (!Blocked)
                                                break;
                                        }
                                        if (Blocked)
                                            break;
                                        Node newnode = new Node(tx, ty, (byte)(Step + 1), n.direction);
                                        NextNodes.Add(newnode);
                                    }
                                    else if (!MainMod.IsSolidTile(tx, ty + 1))
                                    {
                                        bool Blocked = false;
                                        for (int y = 0; y < 5; y++)
                                        {
                                            ty++;
                                            Blocked = MainMod.IsSolidTile(tx, ty);
                                            if (Blocked)
                                                break;
                                        }
                                        if (!Blocked)
                                            break;
                                        Node newnode = new Node(tx, (ushort)(ty - 1), (byte)(Step + 1), n.direction);
                                        NextNodes.Add(newnode);
                                    }
                                }
                            }
                            break;
                        case Node.StepDirection.Right:
                            if (tx < Main.rightWorld)
                            {
                                Tile floortile = MainMod.GetTile(tx, ty + 1);
                                if (floortile.active() && floortile.type == Terraria.ID.TileID.Platforms) //Check for stairs
                                {
                                    Node newnode = new Node((ushort)(tx + 1), ty, (byte)(Step + 1), Node.StepDirection.Right);
                                    NextNodes.Add(newnode);
                                    for (int i = -1; i < 2; i += 2)
                                    {
                                        if (i == -1 && tx >= Main.rightWorld)
                                            continue;
                                        ushort ntx = (ushort)(i + tx), nty = (ushort)(ty);
                                        Tile othertile = MainMod.GetTile(ntx, nty);
                                        if (othertile.active() && floortile.type == Terraria.ID.TileID.Platforms)
                                        {
                                            newnode = new Node(tx, (ushort)(ty - 1), (byte)(Step + 1), (i == -1 ? Node.StepDirection.Left : Node.StepDirection.Right));
                                            NextNodes.Add(newnode);
                                        }
                                    }
                                }
                                else
                                {
                                    tx++;
                                    for (int y = 0; y < 8; y++)
                                    {
                                        int nty = ty - (y + 1);
                                        Tile tile = MainMod.GetTile(tx, nty);
                                        if (tile.active() && tile.type == Terraria.ID.TileID.Platforms)
                                        {
                                            Node newnode = new Node(tx, (ushort)(nty - 1), (byte)(Step + 1), Node.StepDirection.Up);
                                            NextNodes.Add(newnode);
                                        }
                                    }
                                    if (MainMod.IsSolidTile(tx, ty))
                                    {
                                        bool Blocked = false;
                                        for (int y = 0; y < 5; y++)
                                        {
                                            ty--;
                                            Blocked = MainMod.IsSolidTile(tx, ty);
                                            if (!Blocked)
                                                break;
                                        }
                                        if (Blocked)
                                            break;
                                        Node newnode = new Node(tx, ty, (byte)(Step + 1), n.direction);
                                        NextNodes.Add(newnode);
                                    }
                                    else if (!MainMod.IsSolidTile(tx, ty + 1))
                                    {
                                        bool Blocked = false;
                                        for (int y = 0; y < 5; y++)
                                        {
                                            ty++;
                                            Blocked = MainMod.IsSolidTile(tx, ty);
                                            if (Blocked)
                                                break;
                                        }
                                        if (!Blocked)
                                            break;
                                        Node newnode = new Node(tx, (ushort)(ty - 1), (byte)(Step + 1), n.direction);
                                        NextNodes.Add(newnode);
                                    }
                                }
                            }
                            break;
                        case Node.StepDirection.Up:
                            if (ty > Main.topWorld)
                            {
                                for (int x = -1; x < 2; x += 2)
                                {
                                    Tile floorTile = MainMod.GetTile(tx + x, ty + 1);
                                    if (floorTile.active() && MainMod.IsSolidTile(tx + x, ty + 1) && floorTile.type != Terraria.ID.TileID.Platforms)
                                    {
                                        Node newnode = new Node((ushort)(tx + x), ty, (byte)(Step + 1), (x == -1 ? Node.StepDirection.Left : Node.StepDirection.Right));
                                        NextNodes.Add(newnode);
                                    }
                                }
                            }
                            break;
                        case Node.StepDirection.Down:
                            if (ty < Main.bottomWorld)
                            {
                                Tile floorTile = MainMod.GetTile(tx, ty + 1);
                                if (floorTile.type != Terraria.ID.TileID.Platforms)
                                    break;
                                for (int y = 2; y < 9; y++)
                                {
                                    if (ty + y >= Main.bottomWorld)
                                        break;
                                    floorTile = MainMod.GetTile(tx, ty + y);
                                    if (floorTile.active() && Main.tileSolid[floorTile.type])
                                    {
                                        Node newnode = new Node(tx, ty, (byte)(Step + 1), Node.StepDirection.Left);
                                        NextNodes.Add(newnode);
                                        newnode = new Node(tx, ty, (byte)(Step + 1), Node.StepDirection.Right);
                                        NextNodes.Add(newnode);
                                        if (floorTile.type == Terraria.ID.TileID.Platforms)
                                        {
                                            newnode = new Node(tx, ty, (byte)(Step + 1), Node.StepDirection.Down);
                                            NextNodes.Add(newnode);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            return false;
        }

        public bool CreatePathingTo(Vector2 Position, bool WalkToPath = false)
        {
            return CreatePathingTo((int)(Position.X * (1f / 16)), (int)(Position.Y * (1f / 16)), WalkToPath);
        }

        public bool CreatePathingTo(int X, int Y, bool WalkToPath = false)
        {
            byte Attempts = 0;
            const byte MaxAttempts = 8;
            while (true)
            {
                bool HasSolidGround = false, HasSolidTile = false;
                Tile tile = MainMod.GetTile(X, Y);
                if (tile == null)
                    return false;
                if (PathFinder.CheckForSolidBlocks(X, Y))
                {
                    Y--;
                    Attempts++;
                    HasSolidTile = true;
                }
                else if (!PathFinder.CheckForSolidGroundUnder(X, Y))
                {
                    Y++;
                }
                else
                {
                    HasSolidGround = true;
                }
                if (!HasSolidTile && !HasSolidGround)
                {
                    Attempts++;
                    //Y++;
                }
                if (HasSolidGround && !HasSolidTile)
                    break;
                if (Attempts >= MaxAttempts)
                    return false;
            }
            if (!PathFinder.CheckForSolidBlocks(X, Y))
            {
                this.WalkToPath = WalkToPath;
                SavedPosX = X;
                SavedPosY = Y;
                PathingInterrupted = false;
                Paths = PathFinder.DoPathFinding(Position, X, Y, (int)(CalculatedJumpHeight * DivisionBy16), FallHeightTolerance);
                RemoveCooldown(GuardianCooldownManager.CooldownType.PathFindingStuckTimer);
                return true;
            }
            return false;
        }

        public bool FollowPathingGuide()
        {
            if (Paths.Count > 0)
            {
                if (IsAttackingSomething)
                {
                    PathingInterrupted = true;
                    return false;
                }
                if (PathingInterrupted)
                {
                    PathingInterrupted = false;
                    if (SavedPosX > -1 && SavedPosY > -1)
                    {
                        Paths = PathFinder.DoPathFinding(Position, SavedPosX, SavedPosY, (int)(CalculatedJumpHeight * DivisionBy16), FallHeightTolerance);
                        if (Paths.Count == 0)
                            return false;
                    }
                    else
                    {
                        Paths.Clear();
                        return false;
                    }
                }
                if (WalkToPath)
                {
                    WalkMode = true;
                }
                if (GetCooldownValue(GuardianCooldownManager.CooldownType.PathFindingStuckTimer) >= 180)
                {
                    RemoveCooldown(GuardianCooldownManager.CooldownType.PathFindingStuckTimer);
                    //Main.NewText("Retracting steps of '" + Name + "'.");
                    if(!CreatePathingTo(SavedPosX, SavedPosY, WalkToPath))
                    {
                        Paths.Clear();
                        return false;
                    }
                }
                PathFinder.Breadcrumbs path = Paths[0];
                bool ReachedNode = false;
                switch (path.NodeOrientation)
                {
                    case PathFinder.Node.NONE:
                        {
                            ReachedNode = true;
                        }
                        break;
                    case PathFinder.Node.DIR_UP:
                        {
                            float Y = path.Y * 16;
                            float X = path.X * 16;
                            if(Position.X < X - 4 || Position.X > X + 4)
                            {
                                if (Position.X < X)
                                    MoveRight = true;
                                else
                                    MoveLeft = true;
                                TryJumpingTallTiles();
                            }
                            else if (Position.Y > Y + 16)
                            {
                                if ((Velocity.Y == 0 && !LastJump) || (JumpHeight > 0))
                                {
                                    Jump = true;
                                }
                            }
                            else
                            {
                                if (Velocity.Y == 0 && Position.Y < Y - 8)
                                {
                                    DropFromPlatform = true;
                                }
                                else
                                {
                                    ReachedNode = true;
                                }
                            }
                            IncreaseCooldownValue(GuardianCooldownManager.CooldownType.PathFindingStuckTimer);
                        }
                        break;
                    case PathFinder.Node.DIR_RIGHT:
                    case PathFinder.Node.DIR_LEFT:
                        {
                            float X = path.X * 16 + 8;
                            if (Position.X >= X - CollisionWidth * 0.5f && Position.X < X + CollisionWidth * 0.5f)
                            {
                                ReachedNode = true;
                            }
                            else
                            {
                                if (Position.X < X)
                                    MoveRight = true;
                               else
                                    MoveLeft = true;
                                if (CollisionX && Velocity.Y == 0)
                                    IncreaseStuckTimer();
                                TryJumpingTallTiles();
                            }
                        }
                        break;
                    case PathFinder.Node.DIR_DOWN:
                        {
                            float Y = path.Y * 16;
                            float X = path.X * 16;
                            if (Position.X < X - 4 || Position.X > X + 4)
                            {
                                if (Position.X < X)
                                    MoveRight = true;
                                else
                                    MoveLeft = true;
                                TryJumpingTallTiles();
                            }
                            else if (Position.Y < Y + 8)
                            {
                                DropFromPlatform = true;
                                IncreaseCooldownValue(GuardianCooldownManager.CooldownType.PathFindingStuckTimer);
                            }
                            else
                            {
                                ReachedNode = true;
                            }
                        }
                        break;
                }
                TryJumpingTallTiles();
                if (ReachedNode)
                {
                    Paths.RemoveAt(0);
                    RemoveCooldown(GuardianCooldownManager.CooldownType.PathFindingStuckTimer);
                }
                return true;
            }
            return false;
        }

        public void AddCoins(int p = 0, int g = 0, int s = 0, int c = 0)
        {
            Data.AddCoins(p, g, s, c);
        }

        public void AddCoins(int Sum)
        {
            Data.AddCoins(Sum);
        }

        public bool SubtractCoins(int p = 0, int g = 0, int s = 0, int c = 0)
        {
            return Data.SubtractCoins(p, g, s, c);
        }

        public bool SubtractCoins(int Sub)
        {
            return Data.SubtractCoins(Sub);
        }

        public bool CheckAttackRange(int WeaponPosition, Vector2 TargetPosition, int TargetWidth, int TargetHeight, bool Kneeling = false)
        {
            bool a, b;
            return CheckAttackRange(WeaponPosition, TargetPosition, TargetWidth, TargetHeight, Kneeling, out a, out b);
        }

        public bool CheckAttackRange(int WeaponPosition, Vector2 TargetPosition, int TargetWidth, int TargetHeight, bool Kneeling, out bool InRangeX, out bool InRangeY)
        {
            InRangeX = false;
            InRangeY = false;
            if (WeaponPosition > -1)
            {
                Item weapon = Inventory[WeaponPosition];
                float AttackWidth = GetMeleeWeaponRangeX(WeaponPosition, Kneeling) + TargetWidth * 0.5f, UpperY, LowerY;
                GetMeleeWeaponRangeY(WeaponPosition, out UpperY, out LowerY, Kneeling);
                InRangeX = Math.Abs(Position.X - (TargetPosition.X + TargetWidth * 0.5f)) < AttackWidth;
                InRangeY = (TargetPosition.Y + TargetHeight >= UpperY + Position.Y && TargetPosition.Y < LowerY + Position.Y) || 
                    (UpperY + Position.Y - Height >= TargetPosition.Y && LowerY + Position.Y <= TargetPosition.Y + TargetHeight);
            }
            else
            {
                InRangeX = Math.Abs((TargetPosition.X + TargetWidth * 0.5f) - Position.X) <= GetMeleeWeaponRangeX() + TargetWidth * 0.5f;
                float UpperY, LowerY;
                GetMeleeWeaponRangeY(-1, out UpperY, out LowerY, false);
                InRangeY = (TargetPosition.Y + TargetHeight >= UpperY + Position.Y && TargetPosition.Y < LowerY + Position.Y) ||
                    (UpperY + Position.Y - Height >= TargetPosition.Y && LowerY + Position.Y <= TargetPosition.Y + TargetHeight);
            }
            return InRangeX && InRangeY;
        }

        public bool CheckIfCanUsePoisonOnWeapons()
        {
            if (ItemAnimationTime > 0) return false;
            foreach (BuffData buff in Buffs)
            {
                if (buff.ID == Terraria.ID.BuffID.WeaponImbueConfetti ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbueCursedFlames ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbueFire ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbueGold ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbueIchor ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbueNanites ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbuePoison ||
                    buff.ID == Terraria.ID.BuffID.WeaponImbueVenom)
                    return false;
            }
            for (int i = 0; i < 10; i++)
            {
                Item item = Inventory[i];
                if (item.type == Terraria.ID.ItemID.FlaskofCursedFlames ||
                    item.type == Terraria.ID.ItemID.FlaskofFire ||
                    item.type == Terraria.ID.ItemID.FlaskofGold ||
                    item.type == Terraria.ID.ItemID.FlaskofIchor ||
                    item.type == Terraria.ID.ItemID.FlaskofNanites ||
                    item.type == Terraria.ID.ItemID.FlaskofParty ||
                    item.type == Terraria.ID.ItemID.FlaskofPoison ||
                    item.type == Terraria.ID.ItemID.FlaskofVenom)
                {
                    SelectedItem = i;
                    Action = true;
                    return true;
                }
            }
            return false;
        }

        public const int NormalTargetMemoryTime = 5 * 60, BossTargetMemoryTime = 30 * 60;
        public sbyte MeleePosition = -1, RangedPosition = -1, MagicPosition = -1;
        private int LastCheckedWeapon = -1, LastCheckedWeaponAttackRange = -1;

        public void NewCombatScript()
        {
            if (TargetID == -1 || (DoAction.InUse && DoAction.IgnoreCombat))
            {
                return;
            }
            CombatTactic Tactic = this.tactic;
            if (OwnerPos != -1)
            {
                PlayerMod.BehaviorChanges behavior = Main.player[OwnerPos].GetModPlayer<PlayerMod>().CurrentTactic;
                if (behavior != PlayerMod.BehaviorChanges.FreeWill)
                {
                    switch (behavior)
                    {
                        case PlayerMod.BehaviorChanges.ChargeOnTarget:
                            Tactic = CombatTactic.Charge;
                            break;
                        case PlayerMod.BehaviorChanges.AvoidContact:
                            Tactic = CombatTactic.Assist;
                            break;
                        case PlayerMod.BehaviorChanges.StayAwayFromTarget:
                            Tactic = CombatTactic.Snipe;
                            break;
                    }
                }
            }
            if (DoAction.InUse && DoAction.ForcedTactic.HasValue)
                Tactic = DoAction.ForcedTactic.Value;
            Vector2 TargetPosition = Vector2.Zero, TargetVelocity = Vector2.Zero;
            int TargetWidth = 0, TargetHeight = 0;
            bool TargetIsBoss = false;
            bool ThroughWall = false;
            bool Aggressive = (Main.bloodMoon || AnyTower || ((Main.eclipse || Main.invasionType >= Terraria.ID.InvasionID.GoblinArmy) && Position.Y < Main.worldSurface * 16));
            switch (TargetType)
            {
                case TargetTypes.Npc:
                    if (!Main.npc[TargetID].active || !IsNpcHostile(Main.npc[TargetID]) || Main.npc[TargetID].dontTakeDamage || 
                        Main.npc[TargetID].reflectingProjectiles)
                    {
                        TargetID = -1;
                        AttackingTarget = false;
                        return;
                    }
                    TargetIsBoss = Main.npc[TargetID].boss || Terraria.ID.NPCID.Sets.TechnicallyABoss[Main.npc[TargetID].type];
                    TargetPosition = Main.npc[TargetID].position;
                    TargetVelocity = Main.npc[TargetID].velocity;
                    TargetWidth = Main.npc[TargetID].width;
                    TargetHeight = Main.npc[TargetID].height;
                    ThroughWall = Main.npc[TargetID].noTileCollide;
                    break;
                case TargetTypes.Player:
                    if (!Main.player[TargetID].active || Main.player[TargetID].dead || !IsPlayerHostile(Main.player[TargetID]))
                    {
                        TargetID = -1;
                        AttackingTarget = false;
                        return;
                    }
                    TargetPosition = Main.player[TargetID].position;
                    TargetVelocity = Main.player[TargetID].velocity;
                    TargetWidth = Main.player[TargetID].width;
                    TargetHeight = Main.player[TargetID].height;
                    break;
                case TargetTypes.Guardian:
                    if (!MainMod.ActiveGuardians.ContainsKey(TargetID) || MainMod.ActiveGuardians[TargetID].Downed || !IsGuardianHostile(MainMod.ActiveGuardians[TargetID]))
                    {
                        TargetID = -1;
                        AttackingTarget = false;
                        return;
                    }
                    TargetPosition = MainMod.ActiveGuardians[TargetID].TopLeftPosition;
                    TargetVelocity = MainMod.ActiveGuardians[TargetID].Velocity;
                    TargetWidth = MainMod.ActiveGuardians[TargetID].Width;
                    TargetHeight = MainMod.ActiveGuardians[TargetID].Height;
                    break;
            }
            if (!HasCooldown(GuardianCooldownManager.CooldownType.MemoryOfTarget))
            {
                TargetID = -1;
                AttackingTarget = false;
                return;
            }
            else
            {
                if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X) < 600 &&
                    Math.Abs(TargetPosition.Y + TargetWidth * 0.5f - CenterY) < 400 && 
                    CanHit(TargetPosition, TargetWidth, TargetHeight))
                {
                    if (OwnerPos == -1 || Math.Abs(Main.player[OwnerPos].Center.X - Position.X) < 300)
                    {
                        SetCooldownValue(GuardianCooldownManager.CooldownType.MemoryOfTarget, TimeUntilCompanionForgetsTarget);
                    }
                }
            }
            bool Approach = false, Retreat = false, Jump = false, Duck = false, Attack = false;
            bool TargetInAim = false;
            if (AttackingTarget)
                TargetInAim = MoveCursorToPosition(TargetPosition + TargetVelocity, TargetWidth, TargetHeight);
            if (AvoidCombat)
            {
                float DistanceFromTarget = 60 + (TargetWidth + Width) * 0.5f;
                switch (Tactic)
                {
                    case CombatTactic.Assist:
                        DistanceFromTarget = 120;
                        break;
                    case CombatTactic.Snipe:
                        DistanceFromTarget = 200;
                        break;
                }
                float Distance = Math.Abs(Position.X - TargetPosition.X + TargetWidth * 0.5f + TargetVelocity.X);
                AttackingTarget = false;
                if (Distance < DistanceFromTarget)
                {
                    Retreat = true;
                }
                else if (Distance < DistanceFromTarget + 20)
                {
                    MoveLeft = MoveRight = false;
                    if (Velocity.Length() == 0)
                    {
                        LookingLeft = Position.X - TargetPosition.X + TargetWidth * 0.5f >= 0;
                    }
                }
            }
            else
            {
                if (!AttackingTarget)
                {
                    float LCX = Position.X, RCX = Position.X;
                    int XCheckDistance = 320, YCheckDistance = 280;
                    if (TargetIsBoss)
                    {
                        XCheckDistance *= 2;
                        YCheckDistance *= 2;
                    }
                    XCheckDistance += (int)(Width * 0.5f);
                    YCheckDistance += (int)(Height * 0.5f);
                    if (OwnerPos > -1 && !GuardingPosition.HasValue)
                    {
                        Player p = Main.player[OwnerPos];
                        float PlayerCenter = p.Center.X;
                        if (Position.X > PlayerCenter)
                            LCX = PlayerCenter;
                        else if (Position.X < PlayerCenter)
                            RCX = PlayerCenter;
                    }
                    if (Aggressive || ((Math.Abs(RCX - TargetPosition.X + TargetWidth / 2) <= XCheckDistance || Math.Abs(LCX - TargetPosition.X + TargetWidth / 2) <= XCheckDistance) &&
                    Math.Abs(TargetPosition.Y + TargetHeight * 0.5f - Position.Y - Height * 0.5f) <= YCheckDistance))
                    {
                        if (!CheckIfCanUsePoisonOnWeapons())
                            AttackingTarget = true;
                    }
                    else
                        return;
                }
                bool TargetOnSight = CanHit(TargetPosition, TargetWidth, TargetHeight), NeedsDucking = false;
                if (!TargetOnSight)
                {
                    if (Base.CanDuck && CanHit(TargetPosition, TargetWidth, TargetHeight, true))
                    {
                        NeedsDucking = true;
                        TargetOnSight = true;
                    }
                }
                if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X) >= 1080 || Math.Abs(TargetPosition.Y + TargetHeight * 0.5f - Position.Y - Height * 0.5f) >= 760)
                {
                    TargetOnSight = false;
                }
                if (TargetOnSight)
                {
                    SetCooldownValue(GuardianCooldownManager.CooldownType.TargetMemoryCooldown, (TargetType == TargetTypes.Npc && Terraria.ID.NPCID.Sets.TechnicallyABoss[Main.npc[TargetID].type]) ? BossTargetMemoryTime : NormalTargetMemoryTime);
                }
                else
                {
                    if (!HasCooldown(GuardianCooldownManager.CooldownType.TargetMemoryCooldown))
                    {
                        TargetID = -1;
                        AttackingTarget = false;
                        return;
                    }
                }
                if (WaitingForManaRecharge && MP >= MMP)
                    WaitingForManaRecharge = false;
                bool HasHurtPanic = HasCooldown(GuardianCooldownManager.CooldownType.HurtPanic);
                bool NearDeath = HasFlag(GuardianFlags.VergeOfDeathAlert);
                if(!NearDeath)
                {
                    if (HP < MHP * 0.3f && !PlayerMounted && !SittingOnPlayerMount)
                    {
                        AddFlag(GuardianFlags.VergeOfDeathAlert);
                        NearDeath = true;
                    }
                }
                else
                {
                    if(HP >= MHP * 0.7f || PlayerMounted || SittingOnPlayerMount)
                    {
                        RemoveFlag(GuardianFlags.VergeOfDeathAlert);
                    }
                }
                if (HasFlag(GuardianFlags.Confusion))
                {
                    if (OwnerPos > -1)
                        TargetPosition.X += (TargetPosition.X + TargetWidth * 0.5f - Main.player[OwnerPos].Center.X) * 2;
                }
                //AimDirection.X = (int)(TargetPosition.X + TargetVelocity.X + TargetWidth * 0.5f);
                //AimDirection.Y = (int)(TargetPosition.Y + TargetVelocity.Y + TargetHeight * 0.5f);
                float DistanceDiscount = 8f * AssistSlot;
                if (ItemAnimationTime == 0 && !SubAttackInUse && !Base.SpecialAttackBasedCombat && !Action)
                {
                    if (TargetOnSight) //Check weapons
                    {
                        MeleePosition = -1;
                        RangedPosition = -1;
                        MagicPosition = -1;
                        //int MeleePosition = -1, RangedPosition = -1, MagicPosition = -1;
                        int HighestMeleeDamage = 0, HighestRangedDamage = 0, HighestMagicDamage = 0;
                        bool MagicItemNeedsRecharge = false, NoManaMagicItem = false;
                        bool MayUseHeavyWeapon = UseHeavyMeleeAttackWhenMounted || (ReverseMount || !PlayerMounted);
                        for (int i = 0; i < 10; i++)
                        {
                            if (Inventory[i].type == 0)
                                continue;
                            if (Inventory[i].melee)
                            {
                                if (Inventory[i].damage > HighestMeleeDamage && (!UseWeaponsByInventoryOrder || MeleePosition == -1) && (!Items.GuardianItemPrefab.IsHeavyItem(Inventory[i]) || MayUseHeavyWeapon))
                                {
                                    HighestMeleeDamage = Inventory[i].damage;
                                    MeleePosition = (sbyte)i;
                                }
                            }
                            if ((Inventory[i].ranged && Inventory[i].ammo == 0) || Inventory[i].thrown)
                            {
                                if (Inventory[i].damage > HighestRangedDamage && (!UseWeaponsByInventoryOrder || RangedPosition == -1) && HasAmmo(Inventory[i]))
                                {
                                    HighestRangedDamage = Inventory[i].damage;
                                    RangedPosition = (sbyte)i;
                                }
                            }
                            if (Inventory[i].magic)
                            {
                                if (Inventory[i].damage > HighestMagicDamage && (!UseWeaponsByInventoryOrder || MagicPosition == -1))
                                {
                                    int ManaCost = (int)(Inventory[i].mana * ManaCostMult);
                                    if (Inventory[i].type == Terraria.ID.ItemID.SpaceGun && HasFlag(GuardianFlags.MeteorSetEffect))
                                    {
                                        HighestMagicDamage = Inventory[i].damage;
                                        MagicPosition = (sbyte)i;
                                        ManaCost = 0;
                                        NoManaMagicItem = true;
                                    }
                                    else if (MP >= Inventory[i].mana * ManaCostMult)
                                    {
                                        HighestMagicDamage = Inventory[i].damage;
                                        MagicPosition = (sbyte)i;
                                        MagicItemNeedsRecharge = false;
                                        NoManaMagicItem = false;
                                    }
                                    else if (MagicPosition == -1)
                                    {
                                        HighestMagicDamage = Inventory[i].damage;
                                        MagicPosition = (sbyte)i;
                                        MagicItemNeedsRecharge = true;
                                        NoManaMagicItem = false;
                                    }
                                }
                            }
                        }
                        if (MagicItemNeedsRecharge && (!HasFlag(GuardianFlags.AutoManaPotion) || !Inventory.Any(x => x.type > 0 && x.healMana > 0)))
                        {
                            WaitingForManaRecharge = true;
                        }
                        SelectedItem = -1;
                        bool InMeleeRange = false;
                        if (MeleePosition > -1)
                        {
                            if (CheckAttackRange(MeleePosition, TargetPosition, TargetWidth, TargetHeight, false))
                            {
                                InMeleeRange = true;
                            }
                            else if (CheckAttackRange(MeleePosition, TargetPosition, TargetWidth, TargetHeight, true))
                            {
                                InMeleeRange = true;
                                NeedsDucking = true;
                            }
                        }
                        if (RangedPosition > -1)
                        {
                            SelectedItem = RangedPosition;
                        }
                        if (MagicPosition > -1 && ((!WaitingForManaRecharge || NoManaMagicItem) || ManaCostMult == 0) && !HasFlag(GuardianFlags.Silence))
                        {
                            SelectedItem = MagicPosition;
                        }
                        if (MeleePosition > -1 && (SelectedItem == -1 || InMeleeRange) && !HasProjectileOfThisTypeSpawned(Inventory[MeleePosition]))
                        {
                            SelectedItem = MeleePosition;
                        }
                    }
                    else
                    {
                        SelectedItem = -1;
                        int HighestDamage = 0;
                        MeleePosition = -1;
                        for (int m = 0; m < 10; m++)
                        {
                            if (Inventory[m].type > 0 && Inventory[m].melee && Inventory[m].damage > HighestDamage)
                            {
                                MeleePosition = (sbyte)m;
                                HighestDamage = Inventory[m].damage;
                            }
                        }
                        if (MeleePosition > -1)
                            SelectedItem = MeleePosition;
                    }
                    /*if (HasFlag(GuardianFlags.Confusion) && OwnerPos > -1)
                    {
                        TargetPosition.X += (Main.player[OwnerPos].Center.X - TargetPosition.X + TargetWidth * 0.5f) * 2;
                    }*/
                }
                if (AttackingTarget)
                {
                    bool InRangeX, InRangeY;
                    CheckAttackRange(MeleePosition, TargetPosition, TargetWidth, TargetHeight, false, out InRangeX, out InRangeY);
                    bool GoMelee = SelectedItem == -1 || Inventory[SelectedItem].melee;
                    float MaxAttackRange = -1;
                    if (SelectedItem > -1)
                    {
                        if (LastCheckedWeapon != Inventory[SelectedItem].type)
                        {
                            LastCheckedWeapon = Inventory[SelectedItem].type;
                            if (MainMod.ItemAttackRange.Any(x => x.Key.Type == Inventory[SelectedItem].type))
                            {
                                LastCheckedWeaponAttackRange = MainMod.ItemAttackRange.First(x => x.Key.Type == Inventory[SelectedItem].type).Value;
                            }
                            else
                            {
                                LastCheckedWeaponAttackRange = -1;
                            }
                        }
                        MaxAttackRange = LastCheckedWeaponAttackRange;
                    }
                    if (!TargetOnSight)
                    {
                        float Distance = 16;
                        switch (Tactic)
                        {
                            case CombatTactic.Assist:
                                Distance = 64f;
                                break;
                            case CombatTactic.Snipe:
                                Distance = 96f;
                                break;
                        }
                        if (NearDeath)
                        {
                            Distance *= 2;
                        }
                        if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X + Width * 0.5f + Velocity.X) > Distance)
                        {
                            Approach = true;
                        }
                        else if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X + Width * 0.5f + Velocity.X) <= Distance * 0.5f)
                        {
                            Retreat = true;
                        }
                    }
                    else if (Base.SpecialAttackBasedCombat)
                    {
                        if (!Action && ItemUseTime == 0)
                        {
                            if (Base.SpecialAttackList.Count == 0)
                                return;
                            bool ExecuteBehavior;
                            ushort SubAttackPicked = Base.GuardianSubAttackBehaviorAI(this, tactic, TargetPosition, TargetVelocity, TargetWidth, TargetHeight,
                                ref Approach, ref Retreat, ref Jump, ref MoveDown, out ExecuteBehavior);
                            if (SubAttackPicked < ushort.MaxValue)
                            {
                                GuardianSpecialAttack gsa = Base.SpecialAttackList[SubAttackPicked];
                                SelectedItem = -1;
                                int LastHighestDamage = 0;
                                for (int i = 0; i < 10; i++)
                                {
                                    switch (gsa.AttackType)
                                    {
                                        case GuardianSpecialAttack.SubAttackCombatType.Melee:
                                            if (Inventory[i].type > 0 && Inventory[i].melee && Inventory[i].damage > LastHighestDamage)
                                            {
                                                SelectedItem = i;
                                                LastHighestDamage = Inventory[i].damage;
                                            }
                                            break;
                                        case GuardianSpecialAttack.SubAttackCombatType.Ranged:
                                            if (Inventory[i].type > 0 && Inventory[i].ranged && Inventory[i].damage > LastHighestDamage)
                                            {
                                                SelectedItem = i;
                                                LastHighestDamage = Inventory[i].damage;
                                            }
                                            break;
                                        case GuardianSpecialAttack.SubAttackCombatType.Magic:
                                            if (Inventory[i].type > 0 && Inventory[i].magic && Inventory[i].damage > LastHighestDamage)
                                            {
                                                SelectedItem = i;
                                                LastHighestDamage = Inventory[i].damage;
                                            }
                                            break;
                                    }
                                }
                                float Distance = Math.Abs(TargetPosition.X + TargetWidth * 0.5f + TargetVelocity.X - Position.X + Velocity.X) - Width * 0.5f;
                                if (Distance < gsa.MinRange + TargetWidth * 0.5f)
                                {
                                    if (ExecuteBehavior) Retreat = true;
                                }
                                else if (Distance >= gsa.MaxRange + TargetWidth * 0.5f)
                                {
                                    if (ExecuteBehavior) Approach = true;
                                }
                                else if (!SubAttackInUse)
                                {
                                    UseSubAttack(SubAttackPicked);
                                    if (SubAttackInUse)
                                    {
                                        if (TargetPosition.X + TargetWidth * 0.5f > Position.X)
                                            LookingLeft = false;
                                        else
                                            LookingLeft = true;
                                        LockDirection = true;
                                    }
                                }
                            }
                        }
                        GoMelee = false;
                    }
                    else
                    {
                        float HalfWidth = Width * 0.5f;
                        switch (Tactic)
                        {
                            case CombatTactic.Charge:
                                {
                                    float DistanceX = Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X + Velocity.X); //Testing the addition of Slowdown
                                    float ApproachDistance = GetMeleeWeaponRangeX(MeleePosition, false), // + TargetWidth * 0.5f,
                                        RetreatDistance = HalfWidth + 8;
                                    /*if (MaxAttackRange > -1)
                                    {
                                        ApproachDistance = MaxAttackRange;
                                    }*/
                                    if (HasHurtPanic)
                                    {
                                        ApproachDistance += 20;
                                        RetreatDistance += 12;
                                    }
                                    if (DistanceX <= ApproachDistance + 8 && InRangeY && !NearDeath)//(SelectedItem == -1 || (SelectedItem > -1 && Inventory[SelectedItem].melee))
                                    {
                                        GoMelee = true;
                                    }
                                    else
                                    {
                                        if (NearDeath)
                                        {
                                            ApproachDistance += 44;
                                            RetreatDistance += 20;
                                        }
                                        //Main.NewText("Approach Distance: " + ApproachDistance + "  Current Distance: " + Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X));
                                        //if (DistanceX < 300 + HalfWidth)
                                        {
                                            if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X) < RetreatDistance)
                                                Retreat = true;
                                            else if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X) >= ApproachDistance)
                                                Approach = true;
                                        }
                                        if (TargetInAim && SelectedItem > -1 && !Inventory[SelectedItem].melee && DistanceX < 180 + HalfWidth)
                                            Attack = true;
                                    }
                                    if (!NearDeath && Math.Abs(Position.X - TargetPosition.X + TargetWidth * 0.5f) < (TargetWidth + Width) * 0.5f + 40 && Position.Y - (Height + 16) > TargetPosition.Y + TargetHeight)
                                    {
                                        Jump = true;
                                    }
                                }
                                break;
                            case CombatTactic.Assist:
                                {
                                    if ((SelectedItem == -1 || (SelectedItem > -1 && Inventory[SelectedItem].melee)) && !NearDeath)
                                    {
                                        GoMelee = true;
                                    }
                                    else
                                    {
                                        const float AssistDistance = 120f;
                                        float DistanceX = Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X + Velocity.X);
                                        float ApproachDistance = (TargetWidth + Width) * 0.5f + AssistDistance + 80 + SlowDown + DistanceDiscount,
                                            RetreatDistance = (TargetWidth + Width) * 0.5f + AssistDistance + DistanceDiscount;
                                        if (HasHurtPanic)
                                        {
                                            ApproachDistance += 42 + Math.Abs(TargetVelocity.X * 2);
                                            RetreatDistance += 30 + Math.Abs(TargetVelocity.X * 2);
                                        }
                                        //if (DistanceX < 325 + HalfWidth)
                                        {
                                            if (DistanceX >= ApproachDistance)
                                                Approach = true;
                                            if (DistanceX <= RetreatDistance)
                                                Retreat = true;
                                        }
                                        if (TargetInAim && SelectedItem > -1 && !Inventory[SelectedItem].melee && DistanceX < 250 + HalfWidth + DistanceDiscount)
                                            Attack = true;
                                    }
                                }
                                break;
                            case CombatTactic.Snipe:
                                {
                                    if ((SelectedItem == -1 || (SelectedItem > -1 && Inventory[SelectedItem].melee)) && !NearDeath)
                                    {
                                        GoMelee = true;
                                    }
                                    float DistanceX = Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Position.X + Velocity.X);
                                    const float SnipeDistance = 220f;
                                    float ApproachDistance = (TargetWidth + Width) * 0.5f + SnipeDistance + 150 + DistanceDiscount,
                                        RetreatDistance = (TargetWidth + Width) * 0.5f + SnipeDistance;
                                    if (MaxAttackRange > -1)
                                    {
                                        ApproachDistance = MaxAttackRange + TargetWidth;
                                        RetreatDistance = MaxAttackRange - 50;
                                        if (RetreatDistance < HalfWidth + 8)
                                            RetreatDistance = HalfWidth + 8;
                                    }
                                    if (HasHurtPanic)
                                    {
                                        ApproachDistance += Math.Abs(TargetVelocity.X * 2);
                                        RetreatDistance += Math.Abs(TargetVelocity.X * 2);
                                    }
                                    //if (DistanceX < 475 + HalfWidth)
                                    {
                                        if (DistanceX >= ApproachDistance)
                                            Approach = true;
                                        else if (DistanceX <= RetreatDistance)
                                            Retreat = true;
                                    }
                                    if (TargetInAim && SelectedItem > -1 && !Inventory[SelectedItem].melee && DistanceX < 400 + HalfWidth + DistanceDiscount)
                                        Attack = true;
                                }
                                break;
                        }
                    }
                    if (GoMelee)
                    {
                        if (InRangeX)
                        {
                            if (!InRangeY)
                            {
                                if (TargetPosition.Y + TargetHeight * 0.5 < CenterPosition.Y)
                                {
                                    if ((!LastJump && Velocity.Y == 0) || JumpHeight > 0)
                                    {
                                        Jump = true;
                                    }
                                }
                                else
                                {
                                    if (CheckAttackRange(SelectedItem, TargetPosition, TargetWidth, TargetHeight, true, out InRangeX, out InRangeY))
                                        NeedsDucking = true;
                                }
                            }
                        }
                        if (InRangeX && InRangeY)
                        {
                            Attack = true;
                            if (NeedsDucking)
                                Duck = true;
                        }
                        float AttackRange = GetMeleeWeaponRangeX(SelectedItem, NeedsDucking) + (TargetWidth * 0.5f),
                            DistanceAbs = Math.Abs((Position.X + Velocity.X + SlowDown * Direction) - (TargetPosition.X + TargetWidth * 0.5f));
                        Approach = Retreat = false;
                        if (HurtPanic)
                        {
                            AttackRange += Math.Abs(TargetVelocity.X * 2);
                        }
                        if ((NearDeath && DistanceAbs < (Width + TargetWidth) * 0.5f + 64) || DistanceAbs < (Width + TargetWidth) * 0.5f + 8)
                            Retreat = true;
                        else if (DistanceAbs > AttackRange)
                            Approach = true;
                    }
                }
            }
            if (Duck && ItemAnimationTime == 0)
                MoveDown = true;
            if (Ducking)
            {
                Approach = Retreat = false;
            }
            if (Approach || Retreat)
                MoveLeft = MoveRight = false;
            if (Approach)
            {
                if (TargetPosition.X + TargetWidth * 0.5f > Position.X)
                    MoveRight = true;
                else
                    MoveLeft = true;
            }
            if (Retreat)
            {
                if (TargetPosition.X + TargetWidth * 0.5f <= Position.X)
                    MoveRight = true;
                else
                    MoveLeft = true;
            }
            if (OwnerPos > -1 && !GuardingPosition.HasValue)
            {
                Vector2 PlayerPosition = Main.player[OwnerPos].Center;
                float Distance = Math.Abs(PlayerPosition.X - Position.X);
                const float MaxAwayDistance = 500; //368f
                if (Distance >= MaxAwayDistance)
                {
                    MoveLeft = MoveRight = false;
                    if (PlayerPosition.X - Position.X > 0)
                    {
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                    }
                }
                else if (Distance >= MaxAwayDistance - 32)
                {
                    if (PlayerPosition.X - Position.X > 0)
                    {
                        if (MoveLeft)
                            MoveLeft = false;
                    }
                    else
                    {
                        if (MoveRight)
                            MoveRight = false;
                    }
                }
            }
            if (Jump && ((JumpHeight == 0 && Velocity.Y == 0) || (JumpHeight > 0)))
            {
                this.Jump = true;
            }
            if (Attack && (TargetInAim || (SelectedItem == -1 || Inventory[SelectedItem].melee)) && ItemAnimationTime == 0 && !Action && !LastAction)// || (SelectedItem > -1 && Inventory[SelectedItem].autoReuse)))
            {
                //if(TargetInAim)
                bool UseItem = true;
                if ((SelectedItem == -1 || !Inventory[SelectedItem].autoReuse) && ItemAnimationTime == 0)
                {
                    int CurrentStack = (int)((0.5f + Main.rand.NextFloat() * 0.5f) * Trigger * 100);
                    if (TriggerStack + CurrentStack >= 100)
                    {
                        TriggerStack = 0;
                    }
                    else
                    {
                        TriggerStack += (byte)CurrentStack;
                        UseItem = false;
                    }
                }
                if (UseItem)
                {
                    if (ItemAnimationTime == 0 && (SelectedItem == -1 || Inventory[SelectedItem].melee))
                    {
                        LookingLeft = TargetPosition.X + TargetWidth * 0.5f < Position.X;
                        LockDirection = true;
                    }
                    this.Action = true;
                }
            }
        }

        public void CheckIfSomeoneNeedsRevive(bool MountedEdition = false)
        {
            if (!MountedEdition && OwnerPos > -1 && !Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOut && (PlayerMounted || SittingOnPlayerMount))
                return;
            if (MountedEdition && !MoveDown)
                return;
            if (!DoAction.InUse) //&& !HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown))
            {
                if (IsAttackingSomething)
                {
                    Vector2 TargetPos;
                    int TargetWidth, TargetHeight;
                    GetTargetInformation(out TargetPos, out TargetWidth, out TargetHeight);
                    TargetPos.X += TargetWidth * 0.5f;
                    TargetPos.Y += TargetHeight * 0.5f;
                    if ((TargetPos - CenterPosition).Length() < 168f + TargetWidth * 0.5f) //Avoid trying to resurrect something if there is a foe nearby. 
                        return;
                }
                int Priority = -1;
                byte LowestReviveBoost = 255;
                bool IsGuardian = false;
                float PerceptionRange = 1f + TownNpcs * 0.1f;
                if (OwnerPos > -1 && Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOut)
                {
                    Priority = OwnerPos;
                    IsGuardian = false;
                }
                else
                {
                    for (int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active && !Main.player[p].dead && Main.player[p].GetModPlayer<PlayerMod>().KnockedOut && !IsPlayerHostile(Main.player[p]) && (!MountedEdition && InPerceptionRange(Main.player[p].Center, PerceptionRange) || Main.player[p].getRect().Intersects(HitBox)))
                        {
                            byte ReviveBoost = Main.player[p].GetModPlayer<PlayerMod>().ReviveBoost;
                            if (ReviveBoost < LowestReviveBoost)
                            {
                                Priority = p;
                                IsGuardian = false;
                                LowestReviveBoost = ReviveBoost;
                            }
                        }
                    }
                    foreach (int key in MainMod.ActiveGuardians.Keys)
                    {
                        TerraGuardian g = MainMod.ActiveGuardians[key];
                        if (!g.Downed && g.KnockedOut && !IsGuardianHostile(g) && !g.HasFlag(GuardianFlags.CantReceiveHelpOnReviving) && ((!MountedEdition && InPerceptionRange(g.CenterPosition, PerceptionRange)) || g.HitBox.Intersects(HitBox)))
                        {
                            byte ReviveBoost = g.ReviveBoost;
                            if (ReviveBoost < LowestReviveBoost)
                            {
                                Priority = key;
                                IsGuardian = true;
                                LowestReviveBoost = ReviveBoost;
                            }
                        }
                    }
                }
                if (Priority > -1 || IsGuardian)
                {
                    if (!IsGuardian)
                    {
                        GuardianActions.TryRevivingPlayer(this, Main.player[Priority]);
                    }
                    else
                    {
                        GuardianActions.TryRevivingGuardian(this, MainMod.ActiveGuardians[Priority]);
                    }
                }
            }
        }

        public void BehaviorHandler()
        {
            if (Main.netMode == 1 && OwnerPos != Main.myPlayer)
            {
                if (!Downed && !KnockedOut && !KnockedOutCold)
                    FollowPlayerAI();
                return;
            }
            if (KnockedOut)
            {
                if (OwnerPos == -1 && IsTownNpc && !GetTownNpcInfo.Homeless && ((Base.IsNocturnal && Main.dayTime) || (!Base.IsNocturnal && !Main.dayTime) || Breath <= 0))
                {
                    bool IsPlayerNearby = false;
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && InPerceptionRange(Main.player[i].Center))
                        {
                            IsPlayerNearby = true;
                            break;
                        }
                    }
                    if (!IsPlayerNearby)
                    {
                        WorldMod.GuardianTownNpcState townnpc = GetTownNpcInfo;
                        if (Math.Abs(Position.X - townnpc.HomeX * 16) > 64f || Math.Abs(Position.Y - townnpc.HomeY * 16) > 64)
                        {
                            Position.X = townnpc.HomeX * 16;
                            Position.Y = townnpc.HomeY * 16;
                        }
                    }
                }
            }
            if(PlayerControl && IsCommander)
            {
                GuardingPosition = null;
            }
            if (Downed || PlayerControl || HasFlag(GuardianFlags.Frozen) || HasFlag(GuardianFlags.Petrified) || KnockedOut) return;
            if (!Is2PControlled)
            {
                bool LastHadTargetClose = TargetID > -1;
                if(OwnerPos == -1 && GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 6 && Main.invasionType > 0 && !DoAction.InUse)
                {
                    StartNewGuardianAction(new Actions.DefendHouseOnInvasion());
                }
                LookForThreatsV2();
                CheckIfNeedsToUsePotion();
                FoodAndDrinkScript();
                if (IsCommander)
                {
                    CommandingAI();
                }
                else
                {
                    FollowPlayerAI();
                }
                if (!Dialogue.InDialogue && !CheckForPlayerAFK() || Dialogue.UpdateDialogueParticipationGuardian(this))
                {
                    NewCombatScript();
                }
                CheckForPlayerControl();
                UpdatePerception();
                //
                /*if (OwnerPos > -1 && Main.mouseRight && Main.mouseRightRelease && !PlayerMounted && !Main.LocalPlayer.mouseInterface)
                {
                    if (CreatePathingTo((int)((Main.screenPosition.X + Main.mouseX) * DivisionBy16), (int)((Main.screenPosition.Y + Main.mouseY) * DivisionBy16)))
                        Main.NewText("Success!!");
                    else
                        Main.NewText("Failure...");
                }*/
                //
                if (!IsBeingControlledByPlayer && (!MountedOnPlayer || GuardianHasControlWhenMounted))
                {
                    if (!FollowPathingGuide() && !IsCommander)
                    {
                        if (!NewIdleBehavior())
                        {
                            GuardingPositionAI();
                        }
                    }
                }
                Base.GuardianBehaviorModScript(this);
                if (SittingOnPlayerMount || (PlayerMounted && ReverseMount && !GuardianHasControlWhenMounted) || HasFlag(GuardianFlags.DisableMovement))
                {
                    MoveLeft = MoveRight = Jump = MoveDown = false;
                }
                if (!PlayerControl && (TargetID == -1 || !AttackingTarget))
                    MoveCursorToPosition(CenterPosition + new Vector2(SpriteWidth * 0.5f * Direction, -(SpriteHeight - Base.CharacterPositionYDiscount) * 0.25f));
                if (!PlayerControl)
                {
                    if (!CanDualWield && OwnerPos > -1)
                        OffHandAction = true;
                    if (TargetID > -1 && GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 3 && 
                        (!DoAction.InUse || (!DoAction.IsGuardianSpecificAction && DoAction.ID != (int)GuardianActions.ActionIDs.CarryDownedAlly)))//!LastHadTargetClose && TargetID > -1)
                    {
                        CheckIfSomeoneNeedsPickup();
                    }
                    else if(TargetID == -1 && GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 3)
                    {
                        CheckIfSomeoneNeedsRevive();
                    }
                    else
                    {
                        if (TargetID == -1)
                            CheckForVendors();
                        CheckIfCanSummon();
                        CheckIfCanDoAction();
                        CheckForSituationalPotionUsage();
                        CheckForLifeCrystals();
                        CheckForPullSave();
                    }
                    if (Paths.Count == 0 && !SittingOnPlayerMount && !PlayerMounted)
                    {
                        TryJumpingTallTiles();
                        bool SafeAhead = CheckIfIsSafeAhead();
                        //CheckIfNeedsJumping();
                        if(Velocity.Y == 0)
                            CheckIfIsSteppingOnDamageTiles();
                        if(SafeAhead)
                            TryLandingOnSafeSpot();
                    }
                    //if (OwnerPos > -1 && AssistSlot == 0 && Main.player[OwnerPos].dead)
                    ///    GuardianActions.UseResurrectOnPlayer(this, Main.player[OwnerPos]);
                }
                if (JumpUntilHeight > -1)
                {
                    if (Jump && JumpHeight == 0)
                    {
                        Jump = false;
                        JumpUntilHeight = -1;
                    }
                    else
                    {
                        Jump = true;
                    }
                    if (Position.Y <= JumpUntilHeight)
                        JumpUntilHeight = -1;
                }
                if (Paths.Count == 0 && !PlayerMounted && Velocity.Y > 0 && Position.Y * DivisionBy16 - FallStart > FallHeightTolerance)
                {
                    if (!Jump && HasSolidGroundUnder())
                    {
                        Jump = true;
                    }
                }
            }
            else
            {
                if (Math.Abs(Position.X - Main.player[OwnerPos].Center.X) > (Main.screenWidth + Width) * 0.5f ||
                    Math.Abs(CenterY - Main.player[OwnerPos].Center.Y) > (Main.screenHeight + Height) * 0.5f)
                    IncreaseStuckTimer();
                CheckIfSomeoneNeedsRevive(true);
                if (!MoveDown && LastMoveDown)
                {
                    CheckIfSomeoneNeedsPickup();
                }
                if (PlayerControl) TogglePlayerControl();
                if (GuardingPosition.HasValue) ToggleWait();
            }
            UpdateFurnitureUsageScript();
            DoLootItems();
            /*if (TargetID == -1 && !UsingFurniture && Velocity.X == 0) //Needs some review
            {
                if (DoAction.InUse && !DoAction.IsGuardianSpecificAction && DoAction.ID == (int)GuardianActions.ActionIDs.BuySomethingFromNpcShop)
                {
                    if (HasCooldown(GuardianCooldownManager.CooldownType.ShopCheckCooldown))
                        RemoveCooldown(GuardianCooldownManager.CooldownType.ShopCheckCooldown);
                }
                else
                {
                    IncreaseCooldownValue(GuardianCooldownManager.CooldownType.ShopCheckCooldown);
                    if (GetCooldownValue(GuardianCooldownManager.CooldownType.ShopCheckCooldown) >= 3 * 60)
                    {
                        DecreaseCooldownValue(GuardianCooldownManager.CooldownType.ShopCheckCooldown, 30 * 60);
                        CheckForVendors();
                    }
                }
            }*/
        }

        private void CommandingAI()
        {
            if (Main.myPlayer == CommanderCharacterID)
                return;
            switch (GetCommandingOrder)
            {
                case PlayerMod.CommandingOrders.Idle:
                    {
                        NewIdleBehavior();
                    }
                    break;
                case PlayerMod.CommandingOrders.FollowLeader:
                    {
                        FollowPlayerAI();
                        /*Player Leader = Main.player[GetCommanderLeaderID];
                        PlayerMod pm = Leader.GetModPlayer<PlayerMod>();
                        Vector2 LeaderPosition = Leader.Bottom;
                        float DistanceX = 30 * PlayerMod.CompanyFollowOrder + Math.Abs(Leader.velocity.X);
                        if(Math.Abs(LeaderPosition.X - Position.X) > DistanceX)
                        {
                            if(LeaderPosition.X < Position.X)
                            {
                                MoveRight = false;
                                MoveLeft = true;
                            }
                            else
                            {
                                MoveRight = true;
                                MoveLeft = false;
                            }
                        }*/
                        if (GuardingPosition.HasValue)
                            GuardingPosition = null;
                    }
                    break;

                case PlayerMod.CommandingOrders.Defend:
                    {
                        if(!GuardingPosition.HasValue)
                        {
                            GuardingPosition = Position.ToTileCoordinates();
                        }
                        GuardingPositionAI();
                    }
                    break;

                case PlayerMod.CommandingOrders.Explore:
                    {
                        if (GuardingPosition.HasValue)
                            GuardingPosition = null;
                        WalkMode = !IsAttackingSomething;
                    }
                    break;
            }
        }

        public void CheckIfSomeoneNeedsPickup()
        {
            if (PlayerMounted || SittingOnPlayerMount)
                return;
            float NearestKOdAllyDistance = 400;
            Trigger.TriggerTarget NearestAlly = null;
            for(int p = 0; p < 255; p++)
            {
                if(Main.player[p].active && !Main.player[p].dead && !IsPlayerHostile(Main.player[p]))
                {
                    PlayerMod pm = Main.player[p].GetModPlayer<PlayerMod>();
                    if (pm.KnockedOut && pm.CarriedByGuardianID == -1)
                    {
                        float Distance = (Main.player[p].Center - CenterPosition).Length() - 20 - Width * 0.5f;
                        if(Distance < NearestKOdAllyDistance)
                        {
                            NearestAlly = new Trigger.TriggerTarget(Main.player[p]);
                            NearestKOdAllyDistance = Distance;
                        }
                    }
                }
            }
            foreach(int i in MainMod.ActiveGuardians.Keys)
            {
                if(!MainMod.ActiveGuardians[i].Downed && !IsGuardianHostile(MainMod.ActiveGuardians[i]))
                {
                    TerraGuardian tg = MainMod.ActiveGuardians[i];
                    if(tg.KnockedOut && tg.CarriedByGuardianID == -1)
                    {
                        float Distance = (tg.CenterPosition - CenterPosition).Length() - (tg.Width + Width) * 0.5f;
                        if(Distance < NearestKOdAllyDistance)
                        {
                            NearestAlly = new Trigger.TriggerTarget(tg);
                            NearestKOdAllyDistance = Distance;
                        }
                    }
                }
            }
            if(NearestAlly != null)
            {
                switch (NearestAlly.TargetType)
                {
                    case giantsummon.Trigger.TriggerTarget.TargetTypes.Player:
                        StartNewGuardianAction(new Actions.CarryDownedAlly(Main.player[NearestAlly.TargetID]));
                        break;
                    case giantsummon.Trigger.TriggerTarget.TargetTypes.TerraGuardian:
                        StartNewGuardianAction(new Actions.CarryDownedAlly(MainMod.ActiveGuardians[NearestAlly.TargetID]));
                        break;
                }
                if (UsingFurniture)
                    LeaveFurniture();
            }
        }

        public void CheckForPullSave()
        {
            if (OwnerPos == -1 || GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 3) return;
            bool LavaImmunity = HasFlag(GuardianFlags.LavaImmunity) || HasFlag(GuardianFlags.LavaTolerance);
            if (Velocity.Y > 0)
            {
                bool WillTakeFallDamage = Position.Y * DivisionBy16 - FallStart >= FallHeightTolerance;
                int TileStartX = (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16),
                    TileEndX = (int)((Position.X + CollisionWidth * 0.5f + 1) * DivisionBy16);
                int TileY = (int)(Position.Y * DivisionBy16);
                for (byte y = 1; y <= 3; y++)
                {
                    for (int x = TileStartX; x <= TileEndX; x++)
                    {
                        if (WillTakeFallDamage && MainMod.IsSolidTile(x, y + TileY))
                        {
                            BePulledByPlayer();
                            return;
                        }
                        Tile tile = MainMod.GetTile(x, y + TileY);
                        if (tile.lava() && tile.liquid > 32 && !LavaImmunity)
                        {
                            BePulledByPlayer();
                            return;
                        }
                    }
                }
            }
            if(LavaWet && !LavaImmunity)
            {
                BePulledByPlayer();
                return;
            }
            if(Breath < BreathMax * 0.1f && Main.player[OwnerPos].breath >= Main.player[OwnerPos].breathMax)
            {
                BePulledByPlayer();
                return;
            }
        }

        public void CheckForSituationalPotionUsage()
        {
            if (ItemAnimationTime > 0 || GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) != 5)
                return;
            if (LavaWet || Position.Y >= (Main.maxTilesY - 130) * 16)
            {
                if (HasItem(Terraria.ID.ItemID.ObsidianSkinPotion))
                {
                    for(int i = 0; i < 50; i++)
                    {
                        if(Inventory[i].type == Terraria.ID.ItemID.ObsidianSkinPotion)
                        {
                            SelectedItem = i;
                            Action = true;
                            return;
                        }
                    }
                }
            }
            if (Wet && Breath < BreathMax * 0.2f)
            {
                if (HasItem(Terraria.ID.ItemID.GillsPotion))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        if (Inventory[i].type == Terraria.ID.ItemID.GillsPotion)
                        {
                            SelectedItem = i;
                            Action = true;
                            return;
                        }
                    }
                }
            }
        }

        public bool CheckIfIsSafeAhead()
        {
            if(Velocity.X == 0 && !MoveLeft && !MoveRight)
            {
                return true;
            }
            //if (MoveLeft || MoveRight)
            //    PredictedMoveSpeed += MoveSpeed;
            byte DistXCheck = 3;
            int Direction = Velocity.X == 0 ? (MoveLeft ? -1 : 1) : Velocity.X > 0 ? 1 : -1;
            int CenterX = (int)(Position.X * DivisionBy16), 
                CenterY = (int)(Position.Y * DivisionBy16) - 2;
            bool HasPitfall = false, HasDangerousTileBellow = false;
            const int FallCheckDistY = 12;
            bool FireBlockProtection = HasFlag(GuardianFlags.FireblocksImmunity);
            byte Distance = 0;
            byte GapDistance = 0;
            float DangerPosition = 0;
            if (PrioritaryBehaviorType != PrioritaryBehavior.Jump)
            {
                bool CanSwim = HasFlag(GuardianFlags.SwimmingAbility) || HasFlag(GuardianFlags.Merfolk);
                for (byte x = 0; x <= DistXCheck; x++)
                {
                    byte Dangerous = 0;
                    bool SolidTile = false;
                    int LastFallCheckDistY = 0;
                    for (int CheckX = 0; CheckX < 2; CheckX++)
                    {
                        for (int y = -1; y < FallCheckDistY; y++)
                        {
                            int TileX = (x + CheckX) * Direction + CenterX,
                                TileY = CenterY + y;
                            Tile tile = MainMod.GetTile(TileX, TileY);
                            if (tile != null)
                            {
                                if (tile.lava() || MainMod.IsDangerousTile(TileX, TileY, FireBlockProtection))
                                {
                                    Dangerous++;
                                    LastFallCheckDistY = y;
                                    break;
                                }
                                else if (tile.active() && Main.tileSolid[tile.type])
                                {
                                    SolidTile = true;
                                    if (Dangerous == 1 && LastFallCheckDistY > y)
                                        Dangerous = 0;
                                    LastFallCheckDistY = y;
                                    break;
                                }
                                else if (tile.liquid > 20 && !tile.lava() && CanSwim)
                                {
                                    SolidTile = true;
                                }
                            }
                        }
                    }
                    if (Dangerous == 2)
                    {
                        HasDangerousTileBellow = true;
                        Distance = x;
                        if (Direction < 0)
                            DangerPosition = (x + CenterX + 1) * 16;
                        else
                            DangerPosition = (x + CenterX) * 16;
                        break;
                    }
                    if (!SolidTile)
                    {
                        if(GapDistance == 0)
                            Distance = x;
                        GapDistance++;
                        DistXCheck++;
                        if (DistXCheck >= 12)
                            break;
                        if(GapDistance >= 4)
                        {
                            HasPitfall = true;
                            break;
                        }
                        continue;
                        //HasPitfall = true;
                        //break;
                    }
                    else
                    {
                        GapDistance = 0;
                    }
                    Distance++;
                }
            }
            float OwnerY = 0;
            if (OwnerPos > -1)
            {
                OwnerY = Main.player[OwnerPos].position.Y + 42;
            }
            CheckIfNeedsJumping();
            //if (HasPitfall)
            //{
                if (PrioritaryBehaviorType == PrioritaryBehavior.Jump)
                    return true;
            //}
            if (HasDangerousTileBellow || (HasPitfall && OwnerY - 64 < Position.Y))
            {
                float DangerDistance = DangerPosition - Position.X;
                if (Math.Abs(DangerDistance) < 10)
                {
                    if (Direction < 0)
                    {
                        MoveLeft = false;
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                        MoveRight = false;
                    }
                }
                else
                {
                    MoveLeft = false;
                    MoveRight = false;
                    if (Velocity.X == 0 && Velocity.Y == 0)
                    {
                        LookingLeft = DangerPosition >= Position.X;
                    }
                    IncreaseStuckTimer();
                }
                return false;
                /*if (Distance <= 1)
                {
                    if(Velocity.X == 0)
                    {
                        LookingLeft = MoveLeft;
                    }
                    MoveRight = false;
                    MoveLeft = false;
                    if(OwnerPos > -1)
                    {
                        if(Main.player[OwnerPos].velocity.Y == 0 && 
                            (Math.Abs(Main.player[OwnerPos].Center.X - Position.X) >= 4 * 16 ||
                            Math.Abs(Main.player[OwnerPos].Center.Y - CenterY) >= 4 * 16))
                        {
                            IncreaseStuckTimer();
                        }
                    }
                }
                else
                {
                    MoveRight = false;
                    MoveLeft = false;
                    if (Velocity.X > 0)
                    {
                        MoveLeft = true;
                    }
                    else
                    {
                        MoveRight = true;
                    }
                }
                //if (Math.Abs(PredictedMoveSpeed * DivisionBy16 - Distance) < 1 && OwnerY - 32 < Position.Y)
                //    Jump = true;
                return false;*/
            }
            return true;
        }

        public void CheckIfIsSteppingOnDamageTiles()
        {
            if (Velocity.Y != 0)// || MoveLeft || MoveRight)
                return;
            bool OnDamageTile = false;
            int MinTileX = (int)((Position.X - Width * 0.5f) * DivisionBy16), MaxTileX = (int)((Position.X + Width * 0.5f) * DivisionBy16), TileY = (int)(Position.Y * DivisionBy16);
            {
                for (int y = 0; y < 3; y++)
                {
                    bool HasSolidTileOnTheWay = false;
                    for (int x = MinTileX; x <= MaxTileX; x++)
                    {
                        Tile tile = Main.tile[x, TileY + y];
                        if (tile.active())
                        {
                            if (Terraria.ID.TileID.Sets.TouchDamageHot[tile.type] > 0 || Terraria.ID.TileID.Sets.TouchDamageOther[tile.type] > 0)
                            {
                                OnDamageTile = true;
                                break;
                            }
                            if (Main.tileSolid[tile.type])
                                HasSolidTileOnTheWay = true;
                        }
                    }
                    if(!OnDamageTile && HasSolidTileOnTheWay)
                    {
                        break;
                    }
                }
            }
            if (!OnDamageTile)
                return;
            bool MoveToLeft = LookingLeft;
            bool FoundDirection = false;
            /*if (OwnerPos > -1)
            {
                MoveToLeft = Main.player[OwnerPos].Center.X < Position.X;
            }*/
            //else
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int d = -1; d < 2; d += 2)
                    {
                        if (d == 1 && x == 0)
                            break;
                        for (int y = 0; y < 3; y++)
                        {
                            int tx = (int)(Position.X * DivisionBy16) + x * d,
                                ty = (int)(Position.Y * DivisionBy16) + y;
                            Tile tile = Framing.GetTileSafely(tx, ty);
                            if (tile.active())
                            {
                                if (!(Terraria.ID.TileID.Sets.TouchDamageHot[tile.type] > 0 || Terraria.ID.TileID.Sets.TouchDamageOther[tile.type] > 0) && Main.tileSolid[tile.type])
                                {
                                    bool Success = false;
                                    for (int ytry = 0; ytry < 5; ytry++)
                                    {
                                        bool Blocked = false;
                                        for (int x2 = -1; x2 < 1; x2++)
                                        {
                                            for (int y2 = 0; y2 < 3; y2++)
                                            {
                                                tile = Framing.GetTileSafely(tx + x2, ty - y2 - 1);
                                                if ((tile.active() && Main.tileSolid[tile.type]) || tile.lava())
                                                {
                                                    Blocked = true;
                                                    break;
                                                }
                                            }
                                            if (Blocked) break;
                                        }
                                        if (!Blocked)
                                        {
                                            Success = true;
                                            break;
                                        }
                                    }
                                    if (Success)
                                    {
                                        MoveToLeft = d == -1;
                                        FoundDirection = true;
                                    }
                                    break;
                                }
                            }
                        }
                        if (FoundDirection)
                            break;
                    }
                    if (FoundDirection)
                        break;
                }
            }
            MoveLeft = MoveRight = false;
            if (MoveToLeft)
                MoveLeft = true;
            else
                MoveRight = true;
        }

        public void CheckForVendors()
        {
            if (DoAction.InUse || GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) != 5)
                return;
            byte MerchantPosition = 255, ArmsDealerPosition = 255, CyborgPosition = 255;
            Vector2 MyCenter = CenterPosition;
            for (byte i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].townNPC)
                {
                    Vector2 NpcCenter = Main.npc[i].Center;
                    if (MyCenter.X >= NpcCenter.X - 300 && MyCenter.X < NpcCenter.X + 200 &&
                        MyCenter.Y >= NpcCenter.Y - 300 && MyCenter.Y < NpcCenter.Y + 200)
                    {
                        if (Main.npc[i].type == Terraria.ID.NPCID.Merchant)
                            MerchantPosition = i;
                        if (Main.npc[i].type == Terraria.ID.NPCID.ArmsDealer)
                            ArmsDealerPosition = i;
                        if (Main.npc[i].type == Terraria.ID.NPCID.Cyborg)
                            CyborgPosition = i;
                    }
                }
            }
            if (MerchantPosition < 255 || ArmsDealerPosition < 255 || CyborgPosition < 255)
            {
                int HealingPotionCount = 0, ManaPotionCount = 0;
                int EmptyInventorySlots = 0;
                int MedKits = 0;
                Dictionary<int, int> AmmoAndTheirStack = new Dictionary<int, int>();
                bool HasMagicWeapon = false;
                int MedKitID = (MainMod.UsingGuardianNecessitiesSystem ? ModContent.ItemType<Items.Consumable.FirstAidKit>() : 0);
                for (int i = 0; i < 50; i++)
                {
                    if (Inventory[i].healLife > 0)
                        HealingPotionCount += Inventory[i].stack;
                    if (Inventory[i].healMana > 0)
                        ManaPotionCount += Inventory[i].stack;
                    if (i < 10 && Inventory[i].useAmmo > 0 && !AmmoAndTheirStack.ContainsKey(Inventory[i].useAmmo)) //Let's only get ammo for weapons the companion will use.
                    {
                        int AmmoStack = 0;
                        for (int a = 0; a < 50; a++)
                        {
                            if (a != i && Inventory[a].ammo == Inventory[i].useAmmo)
                            {
                                //if (AmmoStack >= 999)
                                //    break;
                                AmmoStack += Inventory[a].stack;
                            }
                        }
                        AmmoAndTheirStack.Add(Inventory[i].useAmmo, AmmoStack);
                    }
                    if (i < 10 && Inventory[i].mana > 0)
                    {
                        HasMagicWeapon = true;
                    }
                    if (MedKitID > 0 && Inventory[i].type == MedKitID)
                    {
                        MedKits += Inventory[i].stack;
                    }
                    /*if (Inventory[i].ammo > 0)
                    {
                        if (AmmoAndTheirStack.ContainsKey(Inventory[i].ammo))
                        {
                            AmmoAndTheirStack[Inventory[i].ammo] += Inventory[i].stack;
                        }
                        else
                        {
                            //AmmoAndTheirStack.Add(Inventory[i].ammo, Inventory[i].stack);
                        }
                    }*/
                    if (Inventory[i].type == 0)
                        EmptyInventorySlots++;
                }
                if (MerchantPosition < 255 && HealingPotionCount < 30)
                {
                    if (TryBuyingItem(MerchantPosition, Terraria.ID.ItemID.LesserHealingPotion, 300, 60 - HealingPotionCount))
                        return;
                }
                if (MerchantPosition < 255 && HasMagicWeapon && ManaPotionCount < 30)
                {
                    if (TryBuyingItem(MerchantPosition, Terraria.ID.ItemID.LesserManaPotion, 100, 60 - ManaPotionCount))
                        return;
                }
                if (MerchantPosition < 255 && MedKitID > 0 && MedKits < 1)
                {
                    if (TryBuyingItem(MerchantPosition, MedKitID, 6000, 1 - MedKits))
                        return;
                }
                const int MaxAmmoRefull = 3996, RefillThreshould = 1998;
                foreach (int key in AmmoAndTheirStack.Keys)
                {
                    int ToRefill = RefillThreshould - AmmoAndTheirStack[key];
                    if (ToRefill <= 0)
                        continue;
                    ToRefill += MaxAmmoRefull - RefillThreshould;
                    if (MerchantPosition < 255)
                    {
                        if (key == Terraria.ID.AmmoID.Arrow)
                        {
                            if (TryBuyingItem(MerchantPosition, Terraria.ID.ItemID.WoodenArrow, 5, ToRefill))
                                return;
                        }
                    }
                    if (ArmsDealerPosition < 255)
                    {
                        if (key == Terraria.ID.AmmoID.Bullet)
                        {
                            if (TryBuyingItem(ArmsDealerPosition, Terraria.ID.ItemID.MusketBall, 5, ToRefill))
                                return;
                        }
                        if (key == Terraria.ID.AmmoID.StyngerBolt)
                        {
                            if (TryBuyingItem(ArmsDealerPosition, Terraria.ID.ItemID.StyngerBolt, 75, ToRefill))
                                return;
                        }
                        if (key == Terraria.ID.AmmoID.Stake)
                        {
                            if (TryBuyingItem(ArmsDealerPosition, Terraria.ID.ItemID.Stake, 15, ToRefill))
                                return;
                        }
                        if (key == Terraria.ID.AmmoID.NailFriendly)
                        {
                            if (TryBuyingItem(ArmsDealerPosition, Terraria.ID.ItemID.Nail, 100, ToRefill))
                                return;
                        }
                        if (key == Terraria.ID.AmmoID.CandyCorn)
                        {
                            if (TryBuyingItem(ArmsDealerPosition, Terraria.ID.ItemID.CandyCorn, 5, ToRefill))
                                return;
                        }
                    }
                    if (CyborgPosition < 255)
                    {
                        if (key == Terraria.ID.AmmoID.Rocket)
                        {
                            if (TryBuyingItem(CyborgPosition, Terraria.ID.ItemID.RocketI, 50, ToRefill))
                                return;
                        }
                    }
                }
            }
        }

        public bool TryBuyingItem(int NpcPos, int ID, int ItemPrice, int Stack)
        {
            if(Coins < (uint)(ItemPrice * Stack))
            {
                int BuyableStacks = (int)(Coins / (uint)(ItemPrice));
                if (BuyableStacks == 0)
                    return false;
                if (BuyableStacks < Stack)
                    Stack = BuyableStacks;

            }
            if (Coins >= (uint)(ItemPrice * Stack))
            {
                GuardianActions.BuyItemFromShopAction(this, NpcPos, ID, Stack, ItemPrice);
                return true;
            }
            return false;
        }

        public int BuyItem(int ID, int ItemPrice, int Stack, bool SetAsFavorites = false)
        {
            int ItemsBought = 0; //TODO - It's broken. Review how this works.
            int PossibleToBuy = (int)(Coins / (uint)(ItemPrice));
            if (PossibleToBuy < 0)
                return 0;
            if (PossibleToBuy < Stack)
                Stack = PossibleToBuy;
            bool HasEmptySlot = false;
            for(int i = 0; i < 50; i++)
            {
                if (Stack == 0)
                    break;
                if (Inventory[i].type == 0)
                    HasEmptySlot = true;
                else if(Inventory[i].type == ID && Inventory[i].stack < Inventory[i].maxStack)
                {
                    int LeftStack = Inventory[i].maxStack - Inventory[i].stack;
                    if(LeftStack > Stack)
                    {
                        LeftStack = Stack;
                    }
                    if (LeftStack > 0)
                    {
                        ItemsBought += LeftStack;
                        Inventory[i].stack += LeftStack;
                        if (SetAsFavorites)
                            Inventory[i].favorited = true;
                        Stack -= LeftStack;
                        //Main.NewText("Coins: " + Coins + "  To Spend: " + (LeftStack * ItemPrice));
                        Coins -= (uint)(LeftStack * ItemPrice);
                    }
                }
            }
            if (HasEmptySlot && Stack > 0)
            {
                for(int i = 0; i < 50; i++)
                {
                    if (Stack == 0)
                        break;
                    if(Inventory[i].type == 0)
                    {
                        Inventory[i].SetDefaults(ID, true);
                        int LeftStack = Stack;
                        if (LeftStack > Inventory[i].maxStack)
                            LeftStack = Inventory[i].maxStack;
                        if (LeftStack > 0)
                        {
                            ItemsBought += LeftStack;
                            Inventory[i].stack = LeftStack;
                            if (SetAsFavorites)
                                Inventory[i].favorited = true;
                            Stack -= LeftStack;
                            //Main.NewText("Coins: " + Coins + "  To Spend: " + (LeftStack * ItemPrice));
                            Coins -= (uint)(LeftStack * ItemPrice);
                        }
                    }
                }
            }
            return ItemsBought;
        }

        public void AvoidHazard()
        {

        }

        public void CheckIfNeedsJumping()
        {
            if(PrioritaryBehaviorType == PrioritaryBehavior.Jump)
            {
                MoveLeft = MoveRight = false;
                if (PrioritaryMovementTarget.X > Position.X)
                    MoveRight = true;
                else
                    MoveLeft = true;
                bool InRangeY = Math.Abs(PrioritaryMovementTarget.Y - Position.Y + Velocity.Y) < 8;
                if (Math.Abs(PrioritaryMovementTarget.X - Position.X + Velocity.X) < 8)
                {
                    PrioritaryBehaviorType = PrioritaryBehavior.None;
                    LockDirection = false;
                    TurnLock = 0;
                }
                else
                {
                    if (JumpHeight > 0)
                        Jump = true;
                    else if (Velocity.Y == 0)
                    {
                        PrioritaryBehaviorType = PrioritaryBehavior.None;
                        LockDirection = false;
                        TurnLock = 0;
                    }
                }
                return;
            }
            if (PlayerControl || PlayerMounted || (JumpHeight <= 0 && Velocity.Y != 0))
                return;
            if(MoveRight || MoveLeft)
            {
                int CheckDirection = MoveLeft ? -1 : 1;
                int MyX = (int)(Position.X * DivisionBy16), MyY = (int)(Position.Y * DivisionBy16);
                for (int x = 1; x < 4; x++)
                {
                    for(int y = 0; y < 3; y++)
                    {
                        Tile tile = MainMod.GetTile(MyX + x * CheckDirection, MyY + y);
                        if (tile.active() && Main.tileSolid[tile.type])// && !Terraria.ID.TileID.Sets.Platforms[tile.type])
                        {
                            return;
                        }
                    }
                }
                if (OwnerPos > -1 && Main.player[OwnerPos].Bottom.Y > Position.Y + 8)
                    return;
                int uy = MyY - (int)(JumpSpeed * Base.MaxJumpHeight * DivisionBy16), ly = MyY + 2;
                for (int X = 1; X < (MaxSpeed * Base.MaxJumpHeight * DivisionBy16 * 2); X++)
                {
                    for (int y = uy; y < ly; y++)
                    {
                        Tile tile = MainMod.GetTile(MyX + X * CheckDirection, y);
                        if (tile.active() && Main.tileSolid[tile.type] && !Terraria.ID.TileID.Sets.Platforms[tile.type])
                        {
                            Tile uppertile = MainMod.GetTile(MyX + X * CheckDirection, y - 1);
                            if (!uppertile.active() || !Main.tileSolid[uppertile.type])
                            {
                                //Jump here!
                                PrioritaryBehaviorType = PrioritaryBehavior.Jump;
                                PrioritaryMovementTarget = new Vector2((MyX + X * CheckDirection) * 16 + 8, (y - 1) * 16 + 8);
                                Jump = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void TryLandingOnSafeSpot()
        {
            if (Velocity.Y <= 0)
            {
                return;
            }
            int XStart = (int)(Position.X * DivisionBy16);
            int YStart = (int)(Position.Y * DivisionBy16),
                YSum = YStart;
            byte VerticalCheck = 12;
            List<int> XCheck = new List<int>();
            XCheck.Add(XStart);
            XCheck.Add(XStart);
            float FallHorizontalSpeedSum = MoveSpeed * DivisionBy16, HorizontalSum = 0f;
            bool FireProtection = HasFlag(GuardianFlags.FireblocksImmunity);
            int NearestTile = 255;
            bool HasDangerousTile = false;
            byte DangerousTileDistance = 0;
            while (VerticalCheck-- > 0 && XCheck.Count > 0)
            {
                List<int> PossibleXToFall = new List<int>();
                for (int x = 0; x < XCheck.Count; x++)
                {
                    int XPos = XCheck[x], YPos = YSum;
                    if (MainMod.IsDangerousTile(XPos, YPos, FireProtection))
                    {
                        HasDangerousTile = true;
                        XCheck.RemoveAt(x);
                        if(DangerousTileDistance == 0)
                            DangerousTileDistance = (byte)(YPos - YSum);
                    }
                    else if (MainMod.IsSolidTile(XPos, YPos))
                    {
                        PossibleXToFall.Add(XPos);
                        XCheck.RemoveAt(x);
                    }
                }
                HorizontalSum += FallHorizontalSpeedSum;
                if ((int)(HorizontalSum - FallHorizontalSpeedSum) < (int)HorizontalSum)
                {
                    XCheck.Add(XStart - (int)HorizontalSum - 1);
                    XCheck.Add(XStart + (int)HorizontalSum);
                }
                if (PossibleXToFall.Count > 0) //More debugging later...
                {
                    foreach (int x in PossibleXToFall)
                    {
                        bool Safe = false;
                        for (int x2 = -1; x2 < 1; x2++)
                        {
                            if (MainMod.IsSolidTile(x2 + x, YSum))
                                Safe = true;
                            if (MainMod.IsDangerousTile(x2 + x, YSum, FireProtection))
                            {
                                Safe = false;
                                break;
                            }
                        }
                        if (Safe)
                        {
                            int X = x * 16;
                            if (NearestTile == 0 || Math.Abs(Position.X - X) < Math.Abs(Position.X - NearestTile))
                                NearestTile = X;
                        }
                    }
                }
                YSum++;
            }
            if (HasDangerousTile && NearestTile > 0)
            {
                MoveRight = MoveLeft = false;
                if (Position.X < NearestTile * 16)
                    MoveRight = true;
                else
                    MoveLeft = true;
                if (DangerousTileDistance <= 2 && WingType > 0 && WingFlightTime > 0)
                {
                    Jump = true;
                }
            }
        }

        public bool HasPlatformAbove()
        {
            int FeetX = (int)(Position.X * DivisionBy16), FeetY = (int)(Position.Y * DivisionBy16);
            FeetX -= (int)((CollisionWidth * 0.5f) * DivisionBy16) + 1;
            bool PlatformAbove = false;
            int PlatformHeight = 0;
            //Main.NewText("Jump Tile Height = " + (MaxJumpHeight * JumpSpeed) * DivisionBy16 + 1);
            for (int x = 0; x < CollisionWidth * DivisionBy16 + 2; x++)
            {
                for (int y = 0; y < (MaxJumpHeight * JumpSpeed) * DivisionBy16 + 1; y++)
                {
                    int TileX = FeetX + x, TileY = FeetY - y;
                    if (TileY >= 0)
                    {
                        Tile tile = MainMod.GetTile(TileX, TileY);
                        if (tile.active())
                        {
                            if (tile.type == Terraria.ID.TileID.Platforms)
                            {
                                PlatformAbove = true;
                                PlatformHeight = y;
                                break;
                            }
                            if ((PlatformHeight == 0 || y < PlatformHeight) && Main.tileSolid[tile.type])
                                return false;
                        }
                    }
                }
            }
            return PlatformAbove;
        }

        public bool IsStandingOnAPlatform()
        {
            bool stair;
            return IsStandingOnAPlatform(out stair);
        }

        public bool IsStandingOnAPlatform(out bool IsStair)
        {
            int FeetLeftX = (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16), FeetRightX = (int)((Position.X + CollisionWidth * 0.5f) * DivisionBy16), FeetY = (int)(Position.Y * DivisionBy16);
            //FeetLeftX -= (int)((CollisionWidth * 0.5f) * DivisionBy16) + 1;
            bool StandingOnPlatform = false;
            IsStair = false;
            for (int x = FeetLeftX; x <= FeetRightX; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    int TileX = x, TileY = FeetY + y;
                    if (TileY >= 0)
                    {
                        Tile tile = MainMod.GetTile(TileX, TileY);
                        if (tile.active() && Main.tileSolid[tile.type])
                        {
                            if (tile.type == Terraria.ID.TileID.Platforms)
                            {
                                byte slope = tile.slope();
                                if (!IsStair)
                                    IsStair = slope == 1 || slope == 2;
                                StandingOnPlatform = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (tile.active() && Main.tileSolidTop[tile.type])
                        {
                            StandingOnPlatform = true;
                        }
                    }
                }
            }
            if (StandingOnPlatform)
                return true;
            return false;
        }

        public float Distance(Vector2 OtherPosition)
        {
            return (CenterPosition - OtherPosition).Length();
        }

        public bool HasSolidGroundUnder()
        {
            int FeetX = (int)(Position.X * DivisionBy16), FeetY = (int)(Position.Y * DivisionBy16);
            for (int y = 0; y < FallHeightTolerance; y++)
            {
                int TileY = FeetY + y;
                if (TileY >= 0 && y > 1)
                {
                    Tile tile = MainMod.GetTile(FeetX, TileY);
                    if (tile.active() && (Main.tileSolidTop[tile.type] || Main.tileSolid[tile.type]))
                    {
                        return true;
                    }
                    if (tile.liquid > 0 && tile.liquidType() == 1)
                        return false;
                }
            }
            return false;
        }

        public void CheckForStairways(out bool HasPlatformUnder, out bool HasPlatformAbove)
        {
            int FeetX = (int)(Position.X * DivisionBy16), FeetY = (int)(Position.Y * DivisionBy16);
            HasPlatformUnder = false;
            HasPlatformAbove = false;
            int WidthRange = 1;
            for (int y = 0; y <= 2; y += 2)
            {
                for (int x = -WidthRange; x <= WidthRange; x++)
                {
                    int TileX = FeetX + x, TileY = FeetY + y;
                    Tile tile = MainMod.GetTile(TileX, TileY);
                    if (tile != null && tile.active())
                    {
                        if (tile.type == Terraria.ID.TileID.Platforms)
                        {
                            if (y > 1)
                            {
                                HasPlatformUnder = true;
                            }
                            if (y < 1)
                            {
                                HasPlatformAbove = true;
                            }
                        }
                    }
                }
            }
        }

        public void UseNearbyFurniture(int XDist = 9, ushort SpecificFurniture = ushort.MaxValue, bool UseBeds = false, bool Home = false, bool Teleport = false)
        {
            List<ushort> TilesToUse = new List<ushort>();
            TilesToUse.Add(Terraria.ID.TileID.Chairs);
            TilesToUse.Add(Terraria.ID.TileID.Benches);
            TilesToUse.Add(Terraria.ID.TileID.Thrones);
            if (UseBeds)
                TilesToUse.Add(Terraria.ID.TileID.Beds);
            Point[] Furnitures = TryFindingFurniture(TilesToUse.ToArray(), false, XDist, Home);
            if (Furnitures.Length > 0)
            {
                Point point = Furnitures[Main.rand.Next(Furnitures.Length)];
                UseFurniture(point.X, point.Y, Teleport);
            }
        }

        public Point[] TryFindingFurniture(ushort TileType, bool StopOnPlatformWalls, int XDist = -1, bool AtHome = false)
        {
            return TryFindingFurniture(new ushort[] { TileType }, StopOnPlatformWalls, XDist, AtHome);
        }

        public Point[] TryFindingFurniture(ushort[] TileType, bool StopOnPlatformWalls, int XDist = -1, bool AtHome = false)
        {
            List<Point> Found = new List<Point>();
            if (TileType.Length == 0)
                return Found.ToArray();
            Point GuardianPosition = new Point((int)(Position.X * DivisionBy16), (int)(Position.Y * DivisionBy16));
            WorldMod.GuardianTownNpcState townstate = GetTownNpcInfo;
            if (townstate != null && !townstate.Homeless && AtHome)
            {
                GuardianPosition.X = townstate.HomeX;
                GuardianPosition.Y = townstate.HomeY;
            }
            if (townstate == null || townstate.Homeless || townstate.HouseInfo == null)
                AtHome = false;
            if (AtHome)
            {
                WorldMod.GuardianBuildingInfo ghi = townstate.HouseInfo;
                foreach (WorldMod.GuardianBuildingInfo.FurnitureInfo furniture in ghi.furnitures)
                {
                    if (TileType.Contains(furniture.FurnitureID))
                    {
                        int x = furniture.FurnitureX, y = furniture.FurnitureY;
                        if(!AnyoneUsingFurniture(x, y))
                        {
                            Found.Add(new Point(x, y));
                        }
                    }
                }
                return Found.ToArray();
            }
            else if (WorldGen.StartRoomCheck(GuardianPosition.X, GuardianPosition.Y))
            {
                for (int t = 0; t < WorldGen.numTileCount; t++)
                {
                    int x = WorldGen.roomX[t], y = WorldGen.roomY[t];
                    if (y >= 10 && TileType.Contains(Main.tile[x, y].type) && TileType.Contains(Main.tile[x, y - 1].type))
                    {
                        Found.Add(new Point(x, y));
                    }
                }
                return Found.ToArray();
            }
            const int SearchWidth = 20;
            if (XDist == -1)
                XDist = SearchWidth;
            bool LeftWall = false, RightWall = false;
            int LeftTileY = GuardianPosition.Y, RightTileY = GuardianPosition.Y;
            for (int x = 0; x < XDist; x++)
            {
                for (int Direction = -1; Direction <= 1; Direction += 2)
                {
                    if ((Direction == 1 && x == 0) || (Direction == -1 && LeftWall) || (Direction == 1 && RightWall))
                    {
                        continue;
                    }
                    int TileX = GuardianPosition.X + x * Direction, TileY = (Direction == -1 ? LeftTileY : RightTileY);
                    byte Attempts = 0;
                    bool TileUnder, BlockedPosition;
                    while (Attempts < 3)
                    {
                        Tile FloorTile = MainMod.GetTile(TileX, TileY + 1),
                            PositionTile = MainMod.GetTile(TileX, TileY);
                        TileUnder = BlockedPosition = false;
                        if (FloorTile.active() && ((Main.tileSolid[FloorTile.type] && !Main.tileSolidTop[FloorTile.type]) || Terraria.ID.TileID.Sets.Platforms[FloorTile.type]))
                            TileUnder = true;
                        if (PositionTile.active() && ((Main.tileSolid[PositionTile.type] && !Main.tileSolidTop[PositionTile.type]) || (StopOnPlatformWalls && Terraria.ID.TileID.Sets.Platforms[Terraria.ID.TileID.Platforms])))
                            BlockedPosition = true;
                        if (BlockedPosition)
                        {
                            TileY--;
                        }
                        else if (!TileUnder)
                        {
                            TileY++;
                        }
                        else
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                Tile tile = MainMod.GetTile(TileX, TileY - y);
                                if (tile.active() && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (StopOnPlatformWalls && Terraria.ID.TileID.Sets.Platforms[tile.type])))
                                {
                                    BlockedPosition = true;
                                    Attempts = 99;
                                    break;
                                }
                            }
                        }
                        if (!BlockedPosition && TileUnder)
                        {
                            Attempts = 0;
                            break;
                        }
                        Attempts++;
                    }
                    if (Attempts >= 3)
                    {
                        if (Direction == -1)
                            LeftWall = true;
                        else
                            RightWall = true;
                    }
                    else
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            int TileY2 = TileY - y;
                            Tile tile = MainMod.GetTile(TileX, TileY2);
                            //Dust.NewDust(new Vector2(TileX, TileY2) * 16, 16, 16, 235, 0, 0, 0, Color.White, 1f);
                            if (tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
                            {
                                break;
                            }
                            if (tile.active() && TileType.Any(z => z == tile.type))
                            {
                                switch (tile.type)
                                {
                                    case Terraria.ID.TileID.TallGateOpen:
                                    case Terraria.ID.TileID.OpenDoor:
                                        {
                                            if (tile.frameX == 18 || tile.frameX == 36)
                                            {
                                                if (Direction == -1)
                                                    LeftWall = true;
                                                else
                                                    RightWall = true;
                                            }
                                        }
                                        break;
                                    case Terraria.ID.TileID.Chairs:
                                        {
                                            int NTX = TileX, NTY = TileY2 + 1 - (tile.frameY / 18) % 2;
                                            Point TilePos = new Point(NTX, NTY);
                                            if (!Found.Contains(TilePos) && !AnyoneUsingFurniture(TilePos.X, TilePos.Y))
                                                Found.Add(TilePos);
                                        }
                                        break;
                                    case Terraria.ID.TileID.Beds:
                                        {
                                            int NTX = TileX + 2 - (tile.frameX / 18) % 4, NTY = TileY2 + 1 - (tile.frameY / 18) % 2;
                                            Point TilePos = new Point(NTX, NTY);
                                            if (!Found.Contains(TilePos) && !AnyoneUsingFurniture(TilePos.X, TilePos.Y))
                                                Found.Add(TilePos);
                                        }
                                        break;
                                    case Terraria.ID.TileID.Benches:
                                        {
                                            int NTX = TileX + 1 - (tile.frameX / 18) % 3, NTY = TileY2 + 1 - (tile.frameY / 18) % 2;
                                            Point TilePos = new Point(NTX, NTY);
                                            if (!Found.Contains(TilePos) && !AnyoneUsingFurniture(TilePos.X, TilePos.Y))
                                                Found.Add(TilePos);
                                        }
                                        break;
                                    case Terraria.ID.TileID.Thrones:
                                        {
                                            int NTX = TileX + 1 - (tile.frameX / 18) % 4, NTY = TileY2 + 3 - (tile.frameY / 18) % 4;
                                            Point TilePos = new Point(NTX, NTY);
                                            if (!Found.Contains(TilePos) && !AnyoneUsingFurniture(TilePos.X, TilePos.Y))
                                                Found.Add(TilePos);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    if (Direction == -1)
                        LeftTileY = TileY;
                    else
                        RightTileY = TileY;
                }
            }
            return Found.ToArray();
        }

        public void TryFindingNearbyBed(bool AtHome = false)
        {
            if (IsUsingBed)
                return;
            Point[] NearbyBeds = TryFindingFurniture(Terraria.ID.TileID.Beds, true, -1, AtHome);
            if (NearbyBeds.Length > 0)
            {
                int PickedBed = Main.rand.Next(NearbyBeds.Length);
                UseFurniture(NearbyBeds[PickedBed].X, NearbyBeds[PickedBed].Y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Furniture Tile X</param>
        /// <param name="y">Furniture Tile Y</param>
        /// <returns>Returns true if It is going to try using it.</returns>
        public bool UseFurniture(int x, int y, bool Teleport = false)
        {
            Tile tile = MainMod.GetTile(x, y);
            UsingFurniture = false;
            if (tile != null && tile.active())
            {
                switch (tile.type)
                {
                    case Terraria.ID.TileID.Chairs:
                        {
                            furniturex = x;
                            furniturey = y;
                            if ((tile.frameY % 40) < 18)
                                furniturey++;
                        }
                        break;
                    case Terraria.ID.TileID.Thrones:
                    case Terraria.ID.TileID.Benches:
                        {
                            int FramesY = (tile.type == Terraria.ID.TileID.Thrones ? 4 : 2);
                            furniturex = x;
                            furniturey = y;
                            int framex = (int)(tile.frameX * (1f / 18)) % 3,
                                framey = (int)(tile.frameY * (1f / 18)) % FramesY;
                            furniturex += 1 - framex;
                            furniturey += (FramesY - 1) - framey;
                        }
                        break;
                    case Terraria.ID.TileID.Beds:
                        {
                            furniturex = x;
                            furniturey = y;
                            bool FacingLeft = tile.frameX < 72;
                            int framex = (int)(tile.frameX * (1f / 18)) % 4,
                                framey = (int)(tile.frameY * (1f / 18)) % 2;
                            furniturex += 2 - framex;
                            furniturey += 1 - framey;
                        }
                        break;
                }
                //Add a checking if anything else is using the same furniture.
                bool GuardianUsingFurniture = AnyoneUsingFurniture(furniturex, furniturey);
                if (GuardianUsingFurniture)
                {
                    furniturex = furniturey = -1;
                    UsingFurniture = false;
                }
                /*else
                {
                    CreatePathingTo(furniturex, furniturey);
                }*/
                if(furniturex > -1 && furniturey > -1 && Teleport)
                {
                    Position.X = furniturex * 16;
                    Position.Y = furniturey * 16;
                    SetFallStart();
                    AttemptedToPathFindFurniture = true;
                }
                return !GuardianUsingFurniture;
            }
            return false;
        }

        public bool AnyoneUsingFurniture(int x, int y)
        {
            foreach (int key in MainMod.ActiveGuardians.Keys)
            {
                if (key == this.WhoAmID)
                    continue;
                TerraGuardian g = MainMod.ActiveGuardians[key];
                if (g.furniturex == x && g.furniturey == y)
                {
                    return true;
                }
            }
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].aiStyle == 7 && Main.npc[n].ai[0] == 5)
                {
                    int TileX = (int)(Main.npc[n].Bottom.X * DivisionBy16), TileY = (int)(Main.npc[n].Bottom.Y * DivisionBy16);
                    if (TileX == x && TileY == y)
                        return true;
                }
            }
            return false;
        }

        public void SitOnBed()
        {
            if(IsUsingBed)
            {
                SittingOnBed = true;
            }
        }

        public void LeaveFurniture(bool StillGoingToUseFurniture = false)
        {
            if (UsingFurniture)
            {
                float FurnitureBottomY = (furniturey + 1) * 16;
                Position.Y = FurnitureBottomY;
                AttemptedToPathFindFurniture = false;
            }
            if (!StillGoingToUseFurniture)
            {
                furniturex = furniturey = -1;
            }
            UsingFurniture = false;
            SittingOnBed = false;
        }

        public bool IsUsingToilet
        {
            get
            {
                if (furniturex > -1 && furniturey > -1 && UsingFurniture)
                {
                    Tile tile = MainMod.GetTile(furniturex, furniturey);
                    if (tile.active() && tile.type == 15 && (tile.frameY / 36 == 1 || tile.frameY / 36 == 20))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsUsingThrone
        {
            get
            {
                if (furniturex > -1 && furniturey > -1 && UsingFurniture)
                {
                    Tile tile = MainMod.GetTile(furniturex, furniturey);
                    if (tile.active() && tile.type == Terraria.ID.TileID.Thrones)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsUsingBench
        {
            get
            {
                if (furniturex > -1 && furniturey > -1 && UsingFurniture)
                {
                    Tile tile = MainMod.GetTile(furniturex, furniturey);
                    if (tile.active() && tile.type == Terraria.ID.TileID.Benches)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsTownNpc
        {
            get
            {
                return GetTownNpcInfo != null;
            }
        }

        public bool IsSleeping
        {
            get
            {
                if (furniturex > -1 && furniturey > -1 && UsingFurniture && !SittingOnBed)
                {
                    Tile tile = MainMod.GetTile(furniturex, furniturey);
                    if (tile.active() && tile.type == Terraria.ID.TileID.Beds)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsUsingBed
        {
            get
            {
                if (furniturex > -1 && furniturey > -1 && UsingFurniture)
                {
                    Tile tile = MainMod.GetTile(furniturex, furniturey);
                    if (tile.active() && tile.type == Terraria.ID.TileID.Beds)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsUsingChair
        {
            get
            {
                if (furniturex > -1 && furniturey > -1 && UsingFurniture)
                {
                    Tile tile = MainMod.GetTile(furniturex, furniturey);
                    if (tile.active() && tile.type == Terraria.ID.TileID.Chairs)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsPlayerRoomMate(Player player)
        {
            player.FindSpawn();
            if (player.SpawnX > -1 && player.SpawnY > -1)
            {
                WorldMod.GuardianTownNpcState townnpc = GetTownNpcInfo;
                if (townnpc != null && !townnpc.Homeless && townnpc.HouseInfo != null)
                {
                    if (townnpc.HouseInfo.BelongsToThisHousing(player.SpawnX, player.SpawnY - 1))
                        return true;
                }
            }
            return false;
        }

        public void UpdateFurnitureUsageScript()
        {
            if (furniturex != -1 && furniturey != -1)
            {
                if (TalkPlayerID > -1 && (!UsingFurniture || SittingOnBed))
                {
                    return;
                }
                SittingOnBed = false;
                if (IsAttackingSomething)
                {
                    if (UsingFurniture)
                    {
                        LeaveFurniture(true);
                    }
                    return;
                }
                if (!AttemptedToPathFindFurniture)
                {
                    if (Math.Abs(furniturex * 16 + 8 - Position.X) > 8 || Math.Abs(furniturey * 16 + 8 - Position.Y) > 8)
                    {
                        CreatePathingTo(furniturex, furniturey);
                    }
                    AttemptedToPathFindFurniture = true;
                    return;
                }
                if (Paths.Count > 0)
                {
                    WalkMode = true;
                    return;
                }
                float TileCenterX = furniturex * 16 + 8;
                Tile tile = MainMod.GetTile(furniturex, furniturey);
                if (tile == null || !tile.active())
                {
                    furniturex = furniturey = -1;
                    UsingFurniture = false;
                    return;
                }
                if (tile.type == Terraria.ID.TileID.Beds)
                    TileCenterX -= 8;
                if (Paths.Count == 0 && !UsingFurniture)
                {
                    float DistanceFromTile = Math.Abs(Position.X + Velocity.X - TileCenterX);
                    if (DistanceFromTile > 8)
                    {
                        MoveLeft = MoveRight = false;
                        if (Position.X < TileCenterX)
                        {
                            MoveRight = true;
                        }
                        else
                        {
                            MoveLeft = true;
                        }
                        if (DistanceFromTile < 16)
                            WalkMode = true;
                    }
                    else
                    {
                        if (AnyoneUsingFurniture(furniturex, furniturey))
                        {
                            LeaveFurniture(false);
                            if (CurrentIdleAction == IdleActions.UseNearbyFurniture || CurrentIdleAction == IdleActions.UseNearbyFurnitureHome)
                            {
                                ChangeIdleAction(IdleActions.Wait, 300);
                            }
                        }
                        else
                        {
                            UsingFurniture = true;
                            AttemptedToPathFindFurniture = false;
                            Paths.Clear();
                        }
                    }
                }
                if (UsingFurniture)
                {
                    if (furniturex == -1 || furniturey == -1 || !tile.active())
                    {
                        UsingFurniture = false;
                        return;
                    }
                    Velocity.X = Velocity.Y = 0; //TODO - Do changes here to implement the new offset positioning.
                    switch (tile.type)
                    {
                        default:
                            furniturex = furniturey = -1;
                            UsingFurniture = false;
                            Main.NewText("Furniture " + tile.type + " can't be used by guardians...");
                            break;
                        case Terraria.ID.TileID.Chairs:
                            {
                                bool FacingRight = tile.frameX == 18;
                                Vector2 SittingPosition = new Vector2(furniturex * 16, furniturey * 16);
                                FaceDirection(!FacingRight);
                                if (!FacingRight)
                                {
                                    SittingPosition.X += 2;
                                }
                                else
                                {
                                    SittingPosition.X += 14;
                                }
                                float SittingOffset = Base.SittingPoint.X;
                                if (LookingLeft)
                                    SittingOffset = Base.SpriteWidth - SittingOffset;
                                SittingOffset -= Base.SpriteWidth * 0.5f;
                                SittingPosition.X -= SittingOffset * Scale;
                                float YDistancing = (Base.SittingPoint.Y - SpriteHeight) * Scale;
                                //if (YDistancing > 8)
                                //    YDistancing = 8;
                                SittingPosition.Y -= YDistancing;
                                Position = SittingPosition;
                                //if (Position.Y > (furniturey + 3) * 16 + 2)
                                //    Position.Y = (furniturey + 3) * 16 + 2;
                            }
                            break;
                        case Terraria.ID.TileID.Thrones:
                        case Terraria.ID.TileID.Benches:
                            {
                                Vector2 SittingPosition = new Vector2(furniturex * 16 + 8, furniturey * 16 + 16);
                                LookingLeft = false;
                                if (Base.ThroneSittingFrame == -1)
                                {
                                    SittingPosition.Y -= Base.SittingPoint.Y * Scale - SpriteHeight + 16;
                                    float SitPointX = Base.SittingPoint.X * Scale;
                                    if (LookingLeft)
                                        SitPointX = SpriteWidth - SitPointX;
                                    SitPointX -= SpriteWidth * 0.5f;
                                    SittingPosition.X += SitPointX;
                                }
                                Position = SittingPosition;
                            }
                            break;
                        case Terraria.ID.TileID.Beds:
                            {
                                Vector2 RestPosition = new Vector2(furniturex * 16, furniturey * 16 + 16);
                                if (Base.BedSleepingFrame == -1)
                                {
                                    RestPosition.Y -= Base.SittingPoint.Y * Scale - SpriteHeight + 16;
                                    float SitPointX = Base.SittingPoint.X * Scale;
                                    if (LookingLeft)
                                        SitPointX = SpriteWidth - SitPointX;
                                    SitPointX -= SpriteWidth * 0.5f;
                                    RestPosition.X += SitPointX;
                                }
                                else
                                {
                                    bool BedFacingLeft = tile.frameX < 18 * 4;
                                    RestPosition.X += Base.SleepingOffset.X * (BedFacingLeft ? -1 : 1);
                                    //if (BedFacingLeft)
                                    //    RestPosition.X += 18;
                                    RestPosition.Y += Base.SleepingOffset.Y;
                                    LookingLeft = BedFacingLeft;
                                }
                                if (!Base.IsCustomSpriteCharacter)
                                    Rotation = -1.570796326794897f * Direction;
                                Position = RestPosition;
                                //Position.Y -= 10 - (10 * Scale);
                            }
                            break;
                    }
                    MoveLeft = MoveRight = MoveUp = MoveDown = Jump = false;
                }
            }
            else
            {
                UsingFurniture = false;
            }
        }

        public void UpdateComfortStack()
        {
            if (IsAttackingSomething)
                return;
            float ComfortSum = 0;
            if (!Main.bloodMoon && !Main.eclipse)
            {
                if (UsingFurniture)
                {
                    ComfortSum += 0.03f;
                    if (furniturex == -1 && furniturey == -1)
                    {
                        UsingFurniture = false;
                    }
                    else
                    {
                        switch (Main.tile[furniturex, furniturey].type)
                        {
                            case Terraria.ID.TileID.Chairs:
                                ComfortSum += 0.035f;
                                break;
                            case Terraria.ID.TileID.Thrones:
                                ComfortSum += 0.05f;
                                break;
                            case Terraria.ID.TileID.Benches:
                                ComfortSum += 0.043f;
                                break;
                            case Terraria.ID.TileID.Beds:
                                ComfortSum += 0.06f;
                                break;
                        }
                    }
                }
                else
                {
                    if (Velocity.Y == 0 && Velocity.X == 0)
                        ComfortSum += 0.01f;
                    else
                        ComfortSum += 0.0033f;
                }
                ComfortSum += ComfortSum * 0.05f * (TownNpcs > 5 ? 5 : TownNpcs);
                if (ZoneCorrupt || ZoneCrimson)
                    ComfortSum *= 0.6f;
                if (Main.invasionProgress > 0)
                    ComfortSum *= 0.5f;
                if (NPC.TowerActiveNebula || NPC.TowerActiveSolar || NPC.TowerActiveStardust || NPC.TowerActiveVortex || NPC.MoonLordCountdown > 0 || NPC.AnyNPCs(Terraria.ID.NPCID.MoonLordCore))
                    ComfortSum *= 0.3f;
                ComfortStack += ComfortSum;
            }
            if (ComfortStack >= MaxComfortStack)
            {
                ComfortStack = 0;
                if(ComfortPoints < MaxComfortExp)
                {
                    ComfortPoints++;
                }
                /*if (ComfortPoints >= MaxComfortExp)
                {
                    ComfortPoints -= (byte)(10 + FriendshipLevel / 3);
                    IncreaseFriendshipProgress(1);
                }*/
            }
        }

        public void TryJumpingTallTiles()
        {
            if (!IsBeingControlledByPlayer)
            {
                if ((!LastJump || JumpHeight > 0) && (MoveLeft || MoveRight))
                {
                    if (CollisionX)
                    {
                        int CenterX = (int)(Position.X * DivisionBy16);
                        int BottomY = (int)(Position.Y * DivisionBy16);
                        int yCeiling = -1;
                        int LeftX = (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16),
                            RightX = (int)((Position.X + CollisionWidth * 0.5f - 1) * DivisionBy16);
                        for (int i = 3; i < (int)(MaxJumpHeight * JumpSpeed) + 3; i++)
                        {
                            for (int x = LeftX; x <= RightX; x++)
                            {
                                Tile tile = Framing.GetTileSafely(x, BottomY - i);
                                //Dust.NewDust(new Vector2(AheadX, BottomY - i) * 16, 16, 16, 5);
                                if (tile != null && tile.active() && Main.tileSolid[tile.type] && tile.type != Terraria.ID.TileID.ClosedDoor)
                                {
                                    yCeiling = i;
                                    break;
                                }
                            }
                            if (yCeiling > -1)
                                break;
                        }
                        //if (yCeiling > -1)
                        /*{
                            AheadX = (int)((Position.X + (CollisionWidth * 0.5f + 2) * Direction) * DivisionBy16);
                            byte HoleSize = 0;
                            for (int i = 0; i < FallHeightTolerance; i++)
                            {
                                Tile tile = Framing.GetTileSafely(AheadX, BottomY + i);
                                if (tile != null)
                                {
                                    if (tile.active() && Main.tileSolid[tile.type])
                                    {
                                        HoleSize = 0;
                                    }
                                    else
                                    {
                                        HoleSize++;
                                    }
                                    if (HoleSize >= 3)
                                        break;
                                }
                            }
                            if (HoleSize < 3)
                                return;
                        }*/
                        if (yCeiling > -1)
                        {
                            int AheadX = (int)((Position.X + (CollisionWidth * 0.5f + 2) * Direction) * DivisionBy16);
                            byte HoleSize = 0;
                            for (int i = yCeiling; i > 0; i--)
                            {
                                Tile tile = Framing.GetTileSafely(AheadX, BottomY - i);
                                if (tile != null)
                                {
                                    if (tile.active() && Main.tileSolid[tile.type] && tile.type != Terraria.ID.TileID.ClosedDoor)
                                    {
                                        HoleSize = 0;
                                    }
                                    else
                                    {
                                        HoleSize++;
                                    }
                                    if (HoleSize >= 3)
                                        break;
                                }
                            }
                            if (HoleSize < 3)
                                return;
                        }
                        Jump = true;
                    }
                }
            }
        }

        public void CheckIfCanSummon()
        {
            if (OwnerPos == -1 || ItemAnimationTime > 0 || HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown))
            {
                return;
            }
            //Try picking up a summon item and using it.
            int SummonerWeaponPosition = -1;
            for (int i = 0; i < 10; i++)
            {
                if (Inventory[i].type != 0 && Inventory[i].summon && !Inventory[i].sentry)
                {
                    SummonerWeaponPosition = i;
                    break;
                }
            }
            if (SummonerWeaponPosition > -1)
            {
                if (LoadedWorldRegion && AimDirection.X >= Main.leftWorld && AimDirection.X < Main.rightWorld && AimDirection.Y >= Main.topWorld && AimDirection.Y < Main.bottomWorld)
                {
                    if (MinionSlotCount >= MaxMinions)
                        return;
                    SelectedItem = SummonerWeaponPosition;
                    Action = true;
                    if (Inventory[SelectedItem].type != LastUsedSummonItem)
                    {
                        KillAllMySummons();
                        LastUsedSummonItem = Inventory[SelectedItem].type;
                    }
                    if (NumMinions == 0 && MainMod.GeneralIdleCommentCooldown <= 0)
                    {
                        string Mes = GetMessage(GuardianBase.MessageIDs.CompanionInvokesAMinion);
                        if (Mes != "")
                        {
                            SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Mes, this));
                            MainMod.SetIdleCommentCooldown();
                        }
                    }
                }
            }
            else
            {
                if(LastUsedSummonItem > 0)
                {
                    KillAllMySummons();
                    LastUsedSummonItem = 0;
                }
            }
        }

        public void KillAllMySummons()
        {
            for(int p = 0; p < Main.maxProjectiles; p++)
            {
                if (Main.projectile[p].active && ProjMod.IsGuardianProjectile(p) && ProjMod.GuardianProj[p].WhoAmID == this.WhoAmID)
                {
                    Projectile proj = Main.projectile[p];
                    if (proj.minion)
                    {
                        proj.Kill();
                    }
                }
            }
        }

        public void FoodAndDrinkScript()
        {
            if (OwnerPos == -1 || HasCooldown(GuardianCooldownManager.CooldownType.FoodCheckingCooldown))
                return;
            if (ItemUseTime == 0 && !IsAttackingSomething && !HasBuff(Terraria.ID.BuffID.WellFed))
            {
                for (int i = 0; i < 50; i++)
                {
                    if (this.Inventory[i].type != 0 && this.Inventory[i].buffType == Terraria.ID.BuffID.WellFed)
                    {
                        SelectedItem = i;
                        Action = true;
                        break;
                    }
                }
            }
            if (ItemUseTime == 0 && IsAttackingSomething && Base.DrinksBeverage && !HasBuff(Terraria.ID.BuffID.Tipsy) && Age >= MinimumAgeToDrink)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (this.Inventory[i].type != 0 && this.Inventory[i].buffType == Terraria.ID.BuffID.Tipsy)
                    {
                        SelectedItem = i;
                        Action = true;
                        break;
                    }
                }
            }
            AddCooldown(GuardianCooldownManager.CooldownType.FoodCheckingCooldown, 150);
        }

        public void CheckIfCanDoAction()
        {
            if (DoAction.InUse) return;
            if (ItemAnimationTime == 0)
            {
                int MostHurtPlayer = -1;
                float LowestHealth = 1f;
                int DeadPlayer = -1;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && Main.player[p].Distance(CenterPosition) < 1024f)
                    {
                        if (!Main.player[p].dead && !IsPlayerHostile(Main.player[p]) && Main.player[p].potionDelay <= 0 && !Main.player[p].GetModPlayer<PlayerMod>().KnockedOut)
                        {
                            float HealthValue = (float)Main.player[p].statLife / Main.player[p].statLifeMax2;
                            if (HealthValue < LowestHealth)
                            {
                                MostHurtPlayer = p;
                                LowestHealth = HealthValue;
                            }
                        }
                        else if (Main.player[p].dead)
                        {
                            DeadPlayer = p;
                        }
                    }
                }
                if (LowestHealth <= 0.33f && MostHurtPlayer > -1)
                {
                    FreezeItemUseAnimation = false;
                    GuardianActions.UseThrowPotion(this, Main.player[MostHurtPlayer]);
                }
                else if (DeadPlayer > -1 && HasFlag(GuardianFlags.ResurrectionAbility))
                {
                    FreezeItemUseAnimation = false;
                    GuardianActions.UseResurrectOnPlayer(this, Main.player[DeadPlayer]);
                }
            }
        }

        public void FaceDirection(int Direction)
        {
            LookingLeft = Direction == -1;
        }

        public void FaceDirection(bool Left)
        {
            if (!FreezeItemUseAnimation && BodyAnimationFrame != Base.ThroneSittingFrame)
                LookingLeft = Left;
        }

        public bool MayTryGoingSleep
        {
            get
            {
                if (!Main.eclipse && !Main.bloodMoon)
                {
                    if (!Base.IsNocturnal)
                    {
                        return (!Main.dayTime && Main.time >= 9000) || (Main.dayTime && Main.time < 3600);
                    }
                    else
                    {
                        return Main.dayTime && Main.time >= 19800 && Main.time < 48600;
                    }
                }
                return false;
            }
        }
        public bool IsPlayerAFK { get { return (!MainMod.GuardiansIdleEasierOnTowns && AfkCounter >= 180 * 60) || (MainMod.GuardiansIdleEasierOnTowns && TownNpcs >= 3 && AfkCounter >= 60 * 30); } }
        public bool IsPlayerIdle { get { return (!MainMod.GuardiansIdleEasierOnTowns && IdleCounter >= 180 * 60) || (MainMod.GuardiansIdleEasierOnTowns && TownNpcs >= 2 && IdleCounter >= 60 * 30); } }

        public void ShowEmote(int Type, int Time = 180)
        {
            emote = Terraria.GameContent.UI.EmoteBubble.byID[Terraria.GameContent.UI.EmoteBubble.NewBubble(Type, anchor, Time)]; //Broken
        }

        public int GetLastEmoteID()
        {
            if (emote == null)
                return -2;
            return emote.ID;
        }

        public void ShowEmoteAboutSomething(Terraria.GameContent.UI.WorldUIAnchor other = null)
        {
            if (emote == null)
                ShowEmote(0);
            emote.PickNPCEmote(other);
        }

        public bool IsEmoteActive
        {
            get { return emote != null && emote.lifeTime > 0; }
        }

        public bool IsInCompanionActivityTime
        {
            get
            {
                if (Base.IsNocturnal)
                    return !Main.dayTime;
                return Main.dayTime;
            }
        }

        public bool IsCompanionAtHome
        {
            get
            {
                WorldMod.GuardianTownNpcState TownNpcState = GetTownNpcInfo;
                if (TownNpcState == null)
                    return false;
                return TownNpcState.IsAtHome(FeetPosition);
            }
        }

        public bool NewIdleBehavior()
        {
            if (!IsAttackingSomething && TalkPlayerID > -1)
            {
                if ((CurrentIdleAction != IdleActions.Wait && CurrentIdleAction != IdleActions.UseNearbyFurniture && CurrentIdleAction != IdleActions.TryGoingSleep && CurrentIdleAction != IdleActions.UseNearbyFurnitureHome) || IdleActionTime < 5)
                    ChangeIdleAction(IdleActions.Listening, 200);
                float PlayerCenterX = Main.player[TalkPlayerID].Center.X;
                {
                    PlayerMod pm = Main.player[TalkPlayerID].GetModPlayer<PlayerMod>();
                    if (pm.MountedOnGuardian)
                    {
                        PlayerCenterX = pm.MountGuardian.Position.X;
                    }
                }
                bool PlayerToTheLeft = PlayerCenterX < Position.X;
                if (furniturex == -1 && furniturey == -1)
                {
                    if (Math.Abs(Position.X - PlayerCenterX) >= Width + 12)
                    {
                        if (PlayerToTheLeft)
                            MoveLeft = true;
                        else
                            MoveRight = true;
                    }
                    else if (Velocity.X == 0)
                    {
                        if (PlayerToTheLeft)
                            LookingLeft = true;
                        else
                            LookingLeft = false;
                    }
                }
                return true;
            }
            if (CurrentIdleAction == IdleActions.Listening)
            {
                ChangeIdleAction(IdleActions.Wait, 50);
            }
            if (CurrentIdleAction == IdleActions.LookingAtTheBackground && Velocity.Length() > 0)
                CurrentIdleAction = IdleActions.Wait;
            if (OwnerPos > -1 && !Main.player[OwnerPos].ghost && (!IsPlayerIdle || (DoAction.InUse && DoAction.BlockIdleAI) || PlayerControl || 
                (PlayerMounted && !GuardianHasControlWhenMounted) || SittingOnPlayerMount) || IsAttackingSomething || 
                (GuardingPosition.HasValue && !GuardianHasControlWhenMounted))
            {
                CurrentIdleAction = IdleActions.Wait;
                IdleActionTime = 50;
                return false;
            }
            bool IsTownNpc = OwnerPos == -1;
            bool DoIdleMovement = true;
            int HouseX = -1, HouseY = -1;
            if(IsTownNpc && CurrentIdleAction == IdleActions.DefendVillage)
            {
                if(Main.invasionType > Terraria.ID.InvasionID.None)
                {
                    IdleActionTime++;
                    DoIdleMovement = false;
                    int CenterX = (int)(Position.X * DivisionBy16), 
                        CenterY = (int)(this.CenterY * DivisionBy16);
                    Tile tile = MainMod.GetTile(CenterX, CenterY);
                    if(tile.wall > 0 && Main.wallHouse[tile.wall])
                    {
                        if (LookingLeft)
                        {
                            MoveLeft = true;
                            MoveRight = false;
                        }
                        else
                        {
                            MoveRight = true;
                            MoveLeft = false;
                        }
                    }
                }
            }
            else if (IsTownNpc)
            {
                bool MoveIndoors = Main.raining || Main.eclipse || Main.snowMoon || Main.pumpkinMoon;
                if (!MoveIndoors)
                {
                    if (!Base.IsNocturnal)
                    {
                        MoveIndoors = !Main.dayTime || (Main.dayTime && Main.time < 5400);
                    }
                    else
                    {
                        MoveIndoors = (Main.dayTime && Main.time >= 5400 && Main.time < 46800);
                    }
                }
                WorldMod.GuardianTownNpcState TownNpcInfo = GetTownNpcInfo;
                if (TownNpcInfo != null)
                {
                    if (MoveIndoors)
                    {
                        bool AtHome = TownNpcInfo.IsAtHome(FeetPosition);
                        if (!TownNpcInfo.Homeless)
                        {
                            HouseX = TownNpcInfo.HomeX;
                            HouseY = TownNpcInfo.HomeY;
                            if (((!Base.IsNocturnal && !Main.dayTime && Main.time == 0) || (Base.IsNocturnal && Main.dayTime && Main.time >= 5400 && WorldMod.LastTime < 5400)) && !AtHome)
                            {
                                if (UsingFurniture)
                                    LeaveFurniture();
                                ChangeIdleAction(IdleActions.GoHome, 5);
                            }
                        }
                        if (HouseX > -1 && HouseY > -1)
                        {
                            while (!Main.tile[HouseX, HouseY].active() || !Main.tileSolid[Main.tile[HouseX, HouseY].type])
                            {
                                HouseY++;
                                if (HouseY >= Main.maxTilesY - 20)
                                    break;
                            }
                            if (Main.tile[HouseX - 1, HouseY].type == Terraria.ID.TileID.OpenDoor || Main.tile[HouseX - 1, HouseY].type == Terraria.ID.TileID.TallGateOpen)
                            {
                                HouseX++;
                            }
                            else if (Main.tile[HouseX, HouseY].type == Terraria.ID.TileID.OpenDoor || Main.tile[HouseX, HouseY].type == Terraria.ID.TileID.TallGateOpen)
                            {
                                HouseX--;
                            }
                            else if (Main.tile[HouseX + 1, HouseY].type == Terraria.ID.TileID.OpenDoor || Main.tile[HouseX + 1, HouseY].type == Terraria.ID.TileID.TallGateOpen)
                            {
                                HouseX--;
                            }
                        }
                        if (TownNpcInfo.Homeless)
                        {
                            HouseX = (int)Position.X;
                            HouseY = (int)Position.Y;
                            if (IdleActionTime <= 0)
                            {
                                int cx = (int)(HouseX * DivisionBy16), cy = (int)((HouseY - CollisionHeight * 0.5f) * DivisionBy16);
                                Tile tile = MainMod.GetTile(cx, cy);
                                if (tile != null)
                                {
                                    if (!Main.wallHouse[tile.wall])
                                    {
                                        int NearestNpcPosition = -1;
                                        float NearestDistance = float.MaxValue;
                                        for (int n = 0; n < 200; n++)
                                        {
                                            if (Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless && Main.npc[n].type != Terraria.ID.NPCID.OldMan)
                                            {
                                                float Distance = Main.npc[n].Distance(CenterPosition);
                                                if (Distance < NearestDistance)
                                                {
                                                    NearestNpcPosition = n;
                                                    NearestDistance = Distance;
                                                }
                                            }
                                        }
                                        if (NearestNpcPosition > -1)
                                        {
                                            if (Main.player[Main.myPlayer].Distance(CenterPosition) >= Main.screenWidth)
                                            {
                                                Position.X = Main.npc[NearestNpcPosition].position.X + Main.npc[NearestNpcPosition].width * 0.5f;
                                                Position.Y = Main.npc[NearestNpcPosition].position.Y + Main.npc[NearestNpcPosition].height;
                                                SetFallStart();
                                                ChangeIdleAction(IdleActions.Wander, 10);
                                            }
                                            else
                                            {
                                                float NpcPosX = Main.npc[NearestNpcPosition].position.X + Main.npc[NearestNpcPosition].width * 0.5f;
                                                if (Position.X < NpcPosX)
                                                {
                                                    FaceDirection(false);
                                                }
                                                else
                                                {
                                                    FaceDirection(true);
                                                }
                                                ChangeIdleAction(IdleActions.Wander, 60 + Main.rand.Next(30));
                                            }
                                            DoIdleMovement = false;
                                        }
                                        else
                                        {
                                            DoIdleMovement = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Main.rand.NextDouble() < 0.333f)
                                        {
                                            ChangeIdleAction(IdleActions.UseNearbyFurniture, 400 + Main.rand.Next(800));
                                        }
                                        else
                                        {
                                            ChangeIdleAction(IdleActions.Wait, 400 + Main.rand.Next(200));
                                            FaceDirection(!LookingLeft);
                                        }
                                        DoIdleMovement = false;
                                    }
                                }
                            }
                        }
                        else //Has House
                        {
                            HouseX *= 16;
                            HouseY *= 16;
                            float XDif = Position.X - HouseX, YDif = Position.Y - HouseY - 16;
                            if (MayTryGoingSleep)
                            {
                                if (!AtHome)
                                {
                                    ChangeIdleAction(IdleActions.GoHome, 5);
                                }
                                else if (IdleActionTime == 0)
                                {
                                    ChangeIdleAction(IdleActions.TryGoingSleep, 200 + Main.rand.Next(200));
                                }
                                if (IsStandingOnAPlatform() && Position.Y - 8 < HouseY)
                                {
                                    DropFromPlatform = true;
                                }
                            }
                            else
                            {
                                if (!AtHome)
                                {
                                    if (CurrentIdleAction != IdleActions.GoHome)
                                        ChangeIdleAction(IdleActions.GoHome, 5);
                                }
                                else if (IdleActionTime <= 0)
                                {
                                    if (Main.rand.NextDouble() < 0.6f)
                                        ChangeIdleAction(IdleActions.UseNearbyFurnitureHome, 800 + Main.rand.Next(600));
                                    else if (Main.rand.NextDouble() < 0.4f)
                                    {
                                        ChangeIdleAction(IdleActions.WanderHome, 100 + Main.rand.Next(200));
                                    }
                                    else
                                    {
                                        if(CurrentIdleAction == IdleActions.Wait )
                                            FaceDirection(!LookingLeft);
                                        ChangeIdleAction(IdleActions.Wait, 200 + Main.rand.Next(200));
                                    }
                                }
                            }
                            WalkMode = true;
                        }
                        DoIdleMovement = false;
                    }
                }
            }
            if (DoIdleMovement)
            {
                if (IdleActionTime <= 0)
                {
                    switch (CurrentIdleAction)
                    {
                        case IdleActions.TryGoingSleep:
                            {
                                if (MayTryGoingSleep)
                                    ChangeIdleAction(IdleActions.TryGoingSleep, 200 + Main.rand.Next(200));
                                else
                                {
                                    LeaveFurniture();
                                    ChangeIdleAction(IdleActions.Wait, 50 + Main.rand.Next(200));
                                }
                            }
                            break;
                        case IdleActions.UseNearbyFurniture:
                        case IdleActions.UseNearbyFurnitureHome:
                            {
                                LeaveFurniture();
                                ChangeIdleAction(IdleActions.Wait, 600 + Main.rand.Next(400));
                            }
                            break;
                        case IdleActions.Wait:
                            {
                                if(MainMod.ShowBackwardAnimations && Base.BackwardStanding > -1 && Main.rand.NextDouble() < 0.2f)
                                {
                                    bool HasWindowOrOpenPlace = false;
                                    int StartX = (int)(Position.X * DivisionBy16), StartY = (int)((Position.Y - Height) * DivisionBy16);
                                    for(int x = -1; x < 0; x++)
                                    {
                                        Tile tile = MainMod.GetTile(StartX + x, StartY);
                                        if(tile != null && !tile.active() && (tile.wall == 0 || Terraria.ID.WallID.Sets.Transparent[tile.wall]))
                                        {
                                            HasWindowOrOpenPlace = true;
                                            break;
                                        }
                                    }
                                    if (HasWindowOrOpenPlace)
                                    {
                                        ChangeIdleAction(IdleActions.LookingAtTheBackground, 600 + Main.rand.Next(200));
                                        break;
                                    }
                                }
                                if (Main.rand.Next(3) == 0)
                                {
                                    ChangeIdleAction(IdleActions.UseNearbyFurniture, 1200 + Main.rand.Next(1600));
                                }
                                else if (Main.rand.Next(2) == 0)
                                    ChangeIdleAction(IdleActions.Wait, 200 + Main.rand.Next(200));
                                else
                                    ChangeIdleAction(IdleActions.Wander, 200 + Main.rand.Next(200));
                            }
                            break;
                        case IdleActions.GoHome:
                            {
                                ChangeIdleAction(IdleActions.Wait, 200 + Main.rand.Next(200));
                            }
                            break;
                        default:
                            {
                                ChangeIdleAction(IdleActions.Wait, 200 + Main.rand.Next(200));
                            }
                            break;
                    }
                }
            }
            switch (CurrentIdleAction)
            {
                case IdleActions.Wait:
                    {

                    }
                    break;
                case IdleActions.UseNearbyFurniture:
                case IdleActions.UseNearbyFurnitureHome:
                case IdleActions.TryGoingSleep:
                    WalkMode = true;
                    break;
                case IdleActions.GoHome:
                    {
                        if (HouseX > -1 && HouseY > -1)
                        {
                            float DistanceFromHome = HouseX - Position.X;
                            if (Math.Abs(DistanceFromHome) > 8f)
                            {
                                WalkMode = true;
                                if (DistanceFromHome > 0)
                                    MoveRight = true;
                                else
                                    MoveLeft = true;
                                IdleActionTime = 5;
                            }
                            else
                            {
                                IdleActionTime = 1;
                            }
                            Player player = Main.player[Main.myPlayer];
                            if (player.Distance(Position) >= Main.screenWidth && player.Distance(new Vector2(HouseX, HouseY)) >= Main.screenWidth)
                            {
                                Position.X = HouseX;
                                Position.Y = HouseY;
                                SetFallStart();
                                IdleActionTime = 1;
                            }
                        }
                    }
                    break;
                case IdleActions.WanderHome:
                case IdleActions.Wander:
                    {
                        WalkMode = true;
                        if (GuardingPosition.HasValue)
                        {
                            float XDif = GuardingPosition.Value.X * 16 - Position.X;
                            if (Math.Abs(XDif) >= 192f)
                            {
                                if (XDif < 0 && !LookingLeft)
                                {
                                    FaceDirection(true);
                                }
                                if (XDif > 0 && LookingLeft)
                                {
                                    FaceDirection(false);
                                }
                            }
                        }
                        if(IsPlayerIdle)
                        {
                            float XDif = (GuardingPosition.HasValue ? GuardingPosition.Value.X * 16 : Main.player[Main.myPlayer].Center.X) - Position.X;
                            if (Math.Abs(XDif) >= 256f)
                            {
                                if (XDif < 0 && !LookingLeft)
                                {
                                    FaceDirection(true);
                                }
                                if (XDif > 0 && LookingLeft)
                                {
                                    FaceDirection(false);
                                }
                            }
                        }
                        int CheckAheadX = (int)((Position.X + (CollisionWidth * 0.5f + 8) * Direction) * DivisionBy16),
                            CheckAheadStartY = (int)(Position.Y * DivisionBy16);
                        if (CurrentIdleAction == IdleActions.WanderHome)
                        {
                            bool Door = false;
                            for (int y = 0; y < 4; y++)
                            {
                                Tile tile = MainMod.GetTile(CheckAheadX, CheckAheadStartY);
                                if (tile.active() && (tile.type == Terraria.ID.TileID.ClosedDoor || (tile.type == Terraria.ID.TileID.OpenDoor && (tile.frameX >= 18 && tile.frameX <= 36)) ||
                                    tile.type == Terraria.ID.TileID.TallGateClosed || tile.type == Terraria.ID.TileID.TallGateOpen))
                                {
                                    Door = true;
                                    break;
                                }
                            }
                            if (Door)
                            {
                                FaceDirection(!LookingLeft);
                                break;
                            }
                        }
                        if (IsBlockedAhead())
                            FaceDirection(!LookingLeft);
                        if (LookingLeft)
                            MoveLeft = true;
                        else
                            MoveRight = true;
                    }
                    break;
            }
            IdleActionTime--;
            if (MoveRight || MoveLeft)
            {
                int TileCheckX = (int)(Position.X * DivisionBy16) + 2 * Direction,
                    TileCheckY = (int)(Position.Y * DivisionBy16);
                bool Pitfall = true;
                bool HasOpening = false;
                {
                    byte YCheck = 0;
                    byte OpenSpaceCounter = 0;
                    while (true)
                    {
                        Tile tile = MainMod.GetTile(TileCheckX, TileCheckY - YCheck);
                        bool Obstruct = false;
                        if (tile.active() && Main.tileSolid[tile.type])
                        {
                            if (tile.type == Terraria.ID.TileID.ClosedDoor || tile.type == Terraria.ID.TileID.TallGateClosed)
                            {
                                HasOpening = true;
                                break;
                            }
                            else
                            {
                                Obstruct = true;
                            }
                        }
                        if (Obstruct)
                        {
                            YCheck++;
                            OpenSpaceCounter = 0;
                        }
                        else
                        {
                            YCheck++;
                            OpenSpaceCounter++;
                        }
                        if(OpenSpaceCounter >= 3 || YCheck >= 8)
                        {
                            HasOpening = OpenSpaceCounter >= 3;
                            break;
                        }
                    }
                }
                //HasOpening = true;
                /*for (int y = 0; y < 5; y++)
                {
                    if (MainMod.CanPassThroughThisTile(TileCheckX, TileCheckY - y))
                    {
                        bool Obstruction = false;
                        for (int y2 = 1; y2 < 3; y2++)
                        {
                            if (!MainMod.CanPassThroughThisTile(TileCheckX, TileCheckY - (y + y2)))
                            {
                                Obstruction = true;
                                break;
                            }
                        }
                        if (Obstruction)
                        {
                            HasOpening = false;
                            break;
                        }
                        else
                        {
                            HasOpening = true;
                        }
                    }
                }*/
                if (HasOpening)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            Tile undertile = MainMod.GetTile(TileCheckX + x * Direction, TileCheckY + y);
                            if (undertile.active() && (Main.tileSolid[undertile.type] || Main.tileSolidTop[undertile.type]))
                            {
                                Pitfall = false;
                            }
                        }
                    }
                }
                if (Pitfall || !HasOpening)
                {
                    //FaceDirection(!LookingLeft);
                    if (MoveLeft)
                    {
                        MoveLeft = false;
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                        MoveRight = false;
                    }
                    /*MoveLeft = MoveRight = false;
                    if (LookingLeft)
                        MoveLeft = true;
                    else
                        MoveRight = true;*/
                }
            }
            if (Wet || LavaWet)
            {
                ChangeIdleAction(IdleActions.Wander, 5);
            }
            if (PlayerMounted && GuardianHasControlWhenMounted)
                return false;
            if (!IsAttackingSomething && furniturex == -1 && !MoveLeft && !MoveRight)
            {
                foreach (int key in MainMod.ActiveGuardians.Keys)
                {
                    if (key != WhoAmID)
                    {
                        TerraGuardian g = MainMod.ActiveGuardians[key];
                        if (g.OwnerPos == -1 && g.Velocity.X == 0 && g.Velocity.Y == 0 && g.Position.Y == Position.Y && g.HitBox.Intersects(HitBox))
                        {
                            if (LookingLeft)
                                MoveLeft = true;
                            else
                                MoveRight = true;
                            break;
                        }
                    }
                }
            }
            return true;
        }

        public void ChangeIdleAction(IdleActions action, int Time)
        {
            this.CurrentIdleAction = action;
            IdleActionTime = Time;
            bool IsPlayerNearby = false;
            for(int p = 0; p < 255; p++)
            {
                Player player = Main.player[p];
                if (player.active && Math.Abs(player.Center.X - Position.X) < 1000 && Math.Abs(player.Center.Y - CenterY) < 800)
                {
                    IsPlayerNearby = true;
                    break;
                }
            }
            switch (action)
            {
                case IdleActions.Wait:
                case IdleActions.Wander:
                    if (UsingFurniture)
                        LeaveFurniture();
                    if(action == IdleActions.Wander && Main.rand.Next(2) == 0)
                    {
                        int UpperFloor = 0, LowerFloor = 0;
                        int sx = (int)(Position.X * DivisionBy16), sy = (int)(Position.Y * DivisionBy16);
                        for(int i = 1; i < 20; i++)
                        {
                            int uy = sy - i, by = sy + i;
                            if (PathFinder.CheckForSolidGroundUnder(sx, uy))
                            {
                                UpperFloor = -i;
                            }
                            if (PathFinder.CheckForSolidGroundUnder(sx, by))
                            {
                                LowerFloor = i;
                            }
                        }
                        if (UpperFloor > -3)
                        {
                            UpperFloor = -3;
                        }
                        if (LowerFloor < 3)
                        {
                            LowerFloor = 3;
                        }
                        int DestinationX = (int)(Position.X * DivisionBy16 + Main.rand.Next(-8, 8)),
                            DestinationY = (int)(Position.Y * DivisionBy16 + Main.rand.Next(UpperFloor, LowerFloor));
                        Tile tile = Framing.GetTileSafely(DestinationX, DestinationY);
                        bool DoPathing = true;
                        if(tile != null)
                        {
                            if (tile.liquid > 0)
                            {
                                switch (tile.liquidType())
                                {
                                    case 1:
                                        {
                                            if (HasFlag(GuardianFlags.BreathUnderwater))
                                                break;
                                            bool HasWaterTile = true;
                                            for(byte Y = 0; Y < Height * DivisionBy16; Y++)
                                            {
                                                for(sbyte i = -1; i < 1; i++)
                                                {
                                                    Tile subTile = Framing.GetTileSafely(DestinationX + i, DestinationY - Y);
                                                    if((!subTile.active() || !Main.tileSolid[subTile.type]) && tile.liquid == 0)
                                                    {
                                                        HasWaterTile = false;
                                                        Y = 255;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (HasWaterTile)
                                            {
                                                DoPathing = false;
                                            }
                                        }
                                        break;
                                    case 2:
                                        if (!HasFlag(GuardianFlags.LavaImmunity))
                                        {
                                            DoPathing = false;
                                        }
                                        break;
                                }
                            }
                            if(MainMod.IsDangerousTile(DestinationX, DestinationY + 1, HasFlag(GuardianFlags.FireblocksImmunity)))
                            {
                                DoPathing = false;
                            }
                        }
                        if(DoPathing)
                            CreatePathingTo(new Vector2(DestinationX, DestinationY) * 16, true);
                    }
                    break;
                case IdleActions.WanderHome:
                    if (UsingFurniture)
                        LeaveFurniture();
                    if (action == IdleActions.Wander && Main.rand.Next(2) == 0)
                    {
                        if (IsCompanionAtHome && GetTownNpcInfo.HouseInfo != null)
                        {
                            int UpperFloor = 0, LowerFloor = 0;
                            int sx = (int)(Position.X * DivisionBy16), sy = (int)(Position.Y * DivisionBy16);
                            for (int i = 1; i < 20; i++)
                            {
                                int uy = sy - i, by = sy + i;
                                if (PathFinder.CheckForSolidGroundUnder(sx, uy))
                                {
                                    UpperFloor = -i;
                                }
                                if (PathFinder.CheckForSolidGroundUnder(sx, by))
                                {
                                    LowerFloor = i;
                                }
                            }
                            if (UpperFloor > -3)
                            {
                                UpperFloor = -3;
                            }
                            if (LowerFloor < 3)
                            {
                                LowerFloor = 3;
                            }
                            Vector2 EndPosition = Position + new Vector2(Main.rand.Next(-8, 8), Main.rand.Next(UpperFloor, LowerFloor)) * 16;
                            if (GetTownNpcInfo.HouseInfo.BelongsToThisHousing((int)(EndPosition.X * (1f / 16)), (int)(EndPosition.Y * (1f / 16))))
                            {
                                if (!IsPlayerNearby)
                                {
                                    Position = EndPosition;
                                    SetFallStart();
                                }
                                else
                                {
                                    CreatePathingTo(EndPosition, true);
                                }
                            }
                        }
                    }
                    break;
                case IdleActions.UseNearbyFurniture:
                    if (!UsingFurniture)
                    {
                        UseNearbyFurniture(Teleport: !IsPlayerNearby);
                        if (furniturex == -1 || furniturey == -1)
                            CurrentIdleAction = IdleActions.Wait;
                    }
                    break;
                case IdleActions.UseNearbyFurnitureHome:
                    if (IsCompanionAtHome)
                    {
                        if (!UsingFurniture)
                            UseNearbyFurniture(-1, ushort.MaxValue, false, true, !IsPlayerNearby);
                    }
                    else
                    {
                        ChangeIdleAction(IdleActions.UseNearbyFurniture, Time);
                        return;
                    }
                    break;
                case IdleActions.TryGoingSleep:
                    if (UsingFurniture)
                    {
                        if (IsUsingBed)
                        {
                            return;
                        }
                        LeaveFurniture();
                    }
                    TryFindingNearbyBed(true);
                    if (furniturex == -1 && furniturey == -1)
                    {
                        if (!GetTownNpcInfo.IsAtHome(FeetPosition))
                        {
                            ChangeIdleAction(IdleActions.GoHome, 5);
                        }
                        else if (Main.rand.NextDouble() < 0.4f)
                        {
                            ChangeIdleAction(IdleActions.Wait, 400 + Main.rand.Next(600));
                        }
                        else
                        {
                            ChangeIdleAction(IdleActions.UseNearbyFurnitureHome, 800 + Main.rand.Next(600));
                        }
                    }
                    break;
                case IdleActions.GoHome:
                    if (UsingFurniture)
                        LeaveFurniture();
                    if (IsTownNpc)
                    {
                        WorldMod.GuardianTownNpcState townnpc = GetTownNpcInfo;
                        if(townnpc != null && !townnpc.Homeless && townnpc.HouseInfo != null && !IsCompanionAtHome)
                        {
                            if(Math.Abs(townnpc.HomeX - Position.X * DivisionBy16) < 50 && Math.Abs(townnpc.HomeY - Position.Y * DivisionBy16) < 50)
                            {
                                CreatePathingTo(townnpc.HomeX * 16, townnpc.HomeY * 16, true);
                            }
                        }
                    }
                    IdleActionTime = 5;
                    break;
            }
        }

        public void DoorHandler()
        {
            if(GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 5)
            {
                int CheckStartX = (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16), CheckEndX = (int)((Position.X + CollisionWidth * 0.5f) * DivisionBy16);
                int CheckStartY = (int)((Position.Y - CollisionHeight) * DivisionBy16), CheckEndY = (int)(Position.Y * DivisionBy16);
                for(int x = CheckStartX; x <= CheckEndX; x++)
                {
                    for(int y = CheckStartY; y <= CheckEndY; y++)
                    {
                        Tile tile = MainMod.GetTile(x, y);
                        if (tile.type == 10)
                        {
                            WorldGen.OpenDoor(x, y, Direction);
                            closeDoor = true;
                            doorx = x;
                            doory = y;
                        }
                        else if (tile.type == 388)
                        {
                            WorldGen.ShiftTallGate(x, y, false);
                            closeDoor = true;
                            doorx = x;
                            doory = y;
                        }
                    }
                }
            }
            if (closeDoor)
            {
                if (doorx == -1 || doory == -1)
                {
                    closeDoor = false;
                    return;
                }
                float DistanceFromDoor = Math.Abs(Position.X - (doorx * 16 + 8));
                bool ForceCloseDoor = (Velocity.X == 0 && Velocity.Y == 0 && DistanceFromDoor > CollisionWidth + 8) || UsingFurniture;
                if (ForceCloseDoor || DistanceFromDoor > CollisionWidth)
                {
                    Tile doorTile = MainMod.GetTile(doorx, doory);
                    if (doorTile.type == 11)
                    {
                        if (WorldGen.CloseDoor(doorx, doory, false))
                        {
                            closeDoor = false;
                        }
                    }
                    else if (doorTile.type == 389)
                    {
                        if (WorldGen.ShiftTallGate(doorx, doory, true))
                        {
                            closeDoor = false;
                        }
                    }
                    if (ForceCloseDoor)
                        closeDoor = false;
                    if (!closeDoor)
                        doorx = doory = -1;
                }
            }
            if (!MoveLeft && !MoveRight) return;
            float TileCheckDir = Position.X;
            int Dir = 0;
            if (MoveLeft)
            {
                TileCheckDir -= CollisionWidth * 0.5f;
                Dir = -1;
            }
            else if (MoveRight)
            {
                TileCheckDir += CollisionWidth * 0.5f;
                Dir = 1;
            }
            if (CollisionWidth > 40)
            {
                Dir *= (CollisionWidth / 32) + 1;
            }
            for (int dx = 0; dx < 2; dx++)
            {
                int TileX = (int)(TileCheckDir * DivisionBy16) + Dir * dx, TileY = (int)(Position.Y * DivisionBy16) - 2;
                Tile nextTile = MainMod.GetTile(TileX, TileY);
                if (nextTile != null && nextTile.active() && (nextTile.type == 10 || nextTile.type == 388))
                {
                    if (WorldGen.OpenDoor(TileX, TileY, Dir))
                    {
                        closeDoor = true;
                        doorx = TileX;
                        doory = TileY;
                    }
                    else if (WorldGen.OpenDoor(TileX, TileY, -Dir))
                    {
                        closeDoor = true;
                        doorx = TileX;
                        doory = TileY;
                    }
                    else if (WorldGen.ShiftTallGate(TileX, TileY, false))
                    {
                        closeDoor = true;
                        doorx = TileX;
                        doory = TileY;
                    }
                }
            }
        }

        public void DoLootItems()
        {
            if (this.OwnerPos == -1 || (DoAction.InUse && (DoAction.Inactivity || DoAction.CantUseInventory))) return;
            bool ChaseItems = MayLootItems && !PlayerControl && !PlayerMounted;
            Vector2 Center = CenterPosition;
            float GrabItemDistance = 65f;
            if (SeekingItem > -1 && (!Main.item[SeekingItem].active || Main.item[SeekingItem].type == 0 || Main.item[SeekingItem].stack == 0 || (Center - Main.item[SeekingItem].Center).Length() >= 368))
                SeekingItem = -1;
            bool CheckForLoot = SeekingItem == -1 && IsAttackingSomething && !HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown);
            float NearestPos = 368f;
            if (SeekingItem > -1)
            {
                NearestPos = (Main.item[SeekingItem].Center - CenterPosition).Length();
            }
            int OwnerPos = this.CommanderCharacterID > -1 ? this.CommanderCharacterID : this.OwnerPos;
            bool SeekHeart = HP < MHP * 0.5f || Main.player[OwnerPos].statLife < Main.player[OwnerPos].statLifeMax2 * 0.5f,
                SeekMana = Main.player[OwnerPos].statMana < Main.player[OwnerPos].statManaMax2 * 0.5f;
            int CurrentStack = GetCooldownValue(GuardianCooldownManager.CooldownType.ItemLootStackCheck) * ItemStackCount;
            int MinStack = CurrentStack, MaxStack = CurrentStack + ItemStackCount;
            if (PlayerMounted)
            {
                MinStack = 0;
                MaxStack = Main.maxItems;
            }
            for (int i = 0; i < Main.item.Length; i++)
            {
                if (Main.item[i].active && Main.item[i].type > 0 && Main.item[i].stack > 0 && Main.item[i].noGrabDelay == 0 && (Main.item[i].owner == OwnerPos || (Main.netMode == 0 && Main.item[i].owner == CommanderCharacterID)))
                {
                    bool IsHealthPickup = Main.item[i].type == 58 || Main.item[i].type == 1734 || Main.item[i].type == 1867,
                        IsManaPickup = Main.item[i].type == Terraria.ID.ItemID.Star || Main.item[i].type == Terraria.ID.ItemID.SoulCake || Main.item[i].type == Terraria.ID.ItemID.SugarPlum;
                    if (HitBox.Intersects(Main.item[i].getRect()))
                    {
                        bool GetItem = true;
                        if (IsHealthPickup)
                        {
                            int LifeRestoreValue = (int)(20 * HealthHealMult);
                            if (OwnerPos != -1)
                            {
                                Main.item[i].healLife = 20;
                                int PlayerLifeRestoreValue = Main.player[OwnerPos].GetHealLife(Main.item[i]);
                                Main.player[OwnerPos].GetModPlayer<PlayerMod>().ShareHealthReplenishWithGuardians(20);
                                if (!Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOutCold)
                                {
                                    Main.player[OwnerPos].statLife += PlayerLifeRestoreValue;
                                    Main.player[OwnerPos].HealEffect(PlayerLifeRestoreValue);
                                    if (Main.player[OwnerPos].statLife > Main.player[OwnerPos].statLifeMax2)
                                        Main.player[OwnerPos].statLife = Main.player[OwnerPos].statLifeMax2;
                                }
                            }
                            else
                            {
                                this.RestoreHP(LifeRestoreValue);
                            }
                            Main.item[i].active = false;
                        }
                        else if (IsManaPickup)
                        {
                            int ManaRestoreValue = (int)(100 * ManaHealMult);
                            if (OwnerPos != -1)
                            {
                                Main.item[i].healMana = 100;
                                int PlayerManaRestoreValue = Main.player[OwnerPos].GetHealMana(Main.item[i]);
                                Main.player[OwnerPos].GetModPlayer<PlayerMod>().ShareManaReplenishWithGuardians(100);
                                if (!Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOutCold)
                                {
                                    Main.player[OwnerPos].statMana += PlayerManaRestoreValue;
                                    Main.player[OwnerPos].ManaEffect(PlayerManaRestoreValue);
                                    if (Main.player[OwnerPos].statMana > Main.player[OwnerPos].statManaMax2)
                                        Main.player[OwnerPos].statMana = Main.player[OwnerPos].statManaMax2;
                                }
                            }
                            else
                            {
                                this.RestoreMP(ManaRestoreValue, false);
                            }
                            Main.item[i].active = false;
                        }
                        else
                        {
                            if ((PlayerMounted || SittingOnPlayerMount) && Main.player[OwnerPos].ItemSpace(Main.item[i]))
                            {
                                Main.item[i].position.X = Main.player[OwnerPos].Center.X - Main.item[i].width * 0.5f;
                                Main.item[i].position.Y = Main.player[OwnerPos].Center.Y - Main.item[i].height * 0.5f;
                                GetItem = false;
                            }
                            else if (MayLootItems)
                            {
                                MoveItemToInventory(Main.item[i]);
                            }
                            else
                            {
                                GetItem = false;
                            }
                        }
                        if (GetItem)
                        {
                            if (Main.item[i].type == 0 || Main.item[i].stack == 0)
                                Main.item[i].active = false;
                            if (SeekingItem == i)
                                SeekingItem = -1;
                        }
                    }
                    else
                    {
                        if (((ChaseItems && i >= CurrentStack && i < CurrentStack + ItemStackCount) || (IsHealthPickup && SeekHeart) || (IsManaPickup && SeekMana)) && CanFitInInventory(Main.item[i]))
                        {
                            if (Math.Abs(Center.X - Main.item[i].Center.X) < GrabItemDistance + Height * 0.5f && Math.Abs(Center.Y - Main.item[i].Center.Y) < GrabItemDistance + Width * 0.5f)
                            {
                                if (Main.item[i].Center.X < Center.X)
                                {
                                    Main.item[i].velocity.X += 0.45f;
                                }
                                else
                                {
                                    Main.item[i].velocity.X -= 0.45f;
                                }
                                if (Main.item[i].Center.Y < Center.Y)
                                {
                                    Main.item[i].velocity.Y += 0.45f;
                                }
                                else
                                {
                                    Main.item[i].velocity.Y -= 0.45f;
                                }
                                if (Main.item[i].velocity.X > 5f)
                                    Main.item[i].velocity.X = 5f;
                                if (Main.item[i].velocity.X < -5f)
                                    Main.item[i].velocity.X = -5f;
                                if (Main.item[i].velocity.Y > 5f)
                                    Main.item[i].velocity.Y = 5f;
                                if (Main.item[i].velocity.Y < -5f)
                                    Main.item[i].velocity.Y = -5f;
                                Main.item[i].beingGrabbed = true;
                            }
                            else if (CheckForLoot)
                            {
                                float ThisDistance = (Center - Main.item[i].Center).Length();
                                if (ThisDistance < NearestPos && CanHit(Main.item[i].position, Main.item[i].width, Main.item[i].height))
                                {
                                    SeekingItem = i;
                                    NearestPos = ThisDistance;
                                }
                            }
                        }
                        else if ((PlayerMounted || SittingOnPlayerMount) && Main.player[OwnerPos].ItemSpace(Main.item[i]))
                        {
                            if (Math.Abs(Center.X - Main.item[i].Center.X) < GrabItemDistance + Width * 0.5f && Math.Abs(Center.Y - Main.item[i].Center.Y) < GrabItemDistance + Height * 0.5f)
                            {
                                Main.item[i].beingGrabbed = true;
                                if (Main.item[i].Center.X < Center.X)
                                {
                                    Main.item[i].velocity.X += 0.45f;
                                }
                                else
                                {
                                    Main.item[i].velocity.X -= 0.45f;
                                }
                                if (Main.item[i].Center.Y < Center.Y)
                                {
                                    Main.item[i].velocity.Y += 0.45f;
                                }
                                else
                                {
                                    Main.item[i].velocity.Y -= 0.45f;
                                }
                            }
                        }
                    }
                }
            }
            if (ChaseItems && SeekingItem > -1 && !IsAttackingSomething)
            {
                MoveLeft = MoveRight = false;
                if (Math.Abs(Main.item[SeekingItem].Center.X - Center.X) >= Width)
                {
                    if (Main.item[SeekingItem].Center.X < Center.X)
                        MoveLeft = true;
                    else
                        MoveRight = true;
                }
            }
            if (!HasCooldown(GuardianCooldownManager.CooldownType.ItemLootStackCheck))
                AddCooldown(GuardianCooldownManager.CooldownType.ItemLootStackCheck, ItemStackTurns - 1);
        }

        public bool HasItem(int ItemID)
        {
            return Inventory.Any(x => x.type == ItemID);
        }

        public int GetItemPosition(int ItemID)
        {
            for (int i = 0; i < 50; i++)
            {
                if (Inventory[i].type == ItemID)
                    return i;
            }
            return -1;
        }

        public int GetItemCount(int ItemID)
        {
            int Count = 0;
            for (int i = 0; i < 50; i++)
            {
                if (Inventory[i].type == ItemID)
                {
                    Count += Inventory[i].stack;
                }
            }
            return Count;
        }

        public void GiveItemToPlayer(Player player, int ItemSlot, int Stack = 1, bool Formally = true)
        {
            if (ItemSlot < 0 || ItemSlot >= Inventory.Length || !player.active || Inventory[ItemSlot].type == 0)
                return;
            StartNewGuardianAction(new Actions.GiveItemToSomeone(player, ItemSlot, Stack));
        }

        public void GiveItemToGuardian(TerraGuardian tg, int ItemSlot, int Stack = 1, bool Formally = true)
        {
            if (ItemSlot < 0 || ItemSlot >= Inventory.Length || !tg.Active || Inventory[ItemSlot].type == 0)
                return;
            StartNewGuardianAction(new Actions.GiveItemToSomeone(tg, ItemSlot, Stack));
        }

        public bool MoveItemToInventory(Item item, bool DropItemWhenInventoryIsFull = false)
        {
            //Just move items to the inventory, how hard it can be?
            bool GotItem = false;
            int LastEmptySlotCount = 0;
            for (int i = (MainMod.WarnAboutSaleableInventorySlotsLeft ? 10 : 0); i < 50; i++)
            {
                if (Inventory[i].stack == 0)
                {
                    LastEmptySlotCount++;
                }
            }
            int EmptyInventorySlots = 0, FirstEmptySlot = -1;
            for (int i = 0; i < 50; i++)
            {
                if (Inventory[i].type > 0 && Inventory[i].type == item.type)
                {
                    bool IsCoin = item.type >= Terraria.ID.ItemID.CopperCoin && item.type <= Terraria.ID.ItemID.GoldCoin;
                    int StackToReduce = Inventory[i].maxStack - Inventory[i].stack;
                    if (IsCoin)
                        StackToReduce = item.stack;
                    else if (StackToReduce > item.stack)
                        StackToReduce = item.stack;
                    Inventory[i].stack += StackToReduce;
                    item.stack -= StackToReduce;
                    if (Inventory[i].stack >= Inventory[i].maxStack)
                    {
                        if (IsCoin)
                        {
                            Item newCoin = new Item();
                            newCoin.SetDefaults(Inventory[i].type + 1, true);
                            newCoin.stack = Inventory[i].stack / 100;
                            Inventory[i].stack = Inventory[i].stack % 100;
                            MoveItemToInventory(newCoin, true);
                        }
                    }
                    if (item.stack == 0)
                    {
                        GotItem = true;
                        item.SetDefaults(0, true);
                        item.active = false;
                    }
                }
                if (Inventory[i].type == 0)
                {
                    if (!MainMod.WarnAboutSaleableInventorySlotsLeft || i >= 10)
                    {
                        EmptyInventorySlots++;
                    }
                    if (FirstEmptySlot == -1)
                    {
                        FirstEmptySlot = i;
                    }
                }
            }
            if (item.type > 0)
            {
                if (FirstEmptySlot > -1)
                {
                    Inventory[FirstEmptySlot] = item.Clone();
                    Inventory[FirstEmptySlot].newAndShiny = true;
                    item.SetDefaults(0, true);
                    item.active = false;
                    EmptyInventorySlots--;
                    GotItem = true;
                }
                else if (DropItemWhenInventoryIsFull)
                {
                    int ItemPos = Item.NewItem(HitBox, item.type);
                    Vector2 ItemPosition = Main.item[ItemPos].position, Velocity = Main.item[ItemPos].velocity;
                    Main.item[ItemPos] = item.Clone();
                    Main.item[ItemPos].position = ItemPosition;
                    Main.item[ItemPos].velocity = Velocity;
                    Main.item[ItemPos].active = true;
                    item.SetDefaults(0, true);
                    item.active = false;
                }
            }
            if (EmptyInventorySlots <= 0 && Data.AutoSellWhenInvIsFull && !DoAction.InUse)
            {
                GuardianActions.SellLootCommand(this);
                if (DoAction.InUse)
                {
                    Main.NewText(Name + " went to town to sell items");
                }
            }
            else
            {
                if (EmptyInventorySlots == 0)
                {
                    if(LastEmptySlotCount > 0)
                        Main.NewText(Name + "'s inventory is full.", Color.Red);
                }
                else if (EmptyInventorySlots <= 5 && EmptyInventorySlots < LastEmptySlotCount)
                {
                    Main.NewText(Name + " has " + EmptyInventorySlots + " empty inventory slots left.", Color.Yellow);
                }
            }
            if (GotItem)
                Main.PlaySound(7, CenterPosition);
            return GotItem;
        }

        public bool CanFitInInventory(Item i)
        {
            if (i.type == 0) return false;
            int Stack = i.stack;
            for (int s = 0; s < 50; s++)
            {
                if (Inventory[s].type == i.type)
                {
                    int SlotsToFill = i.stack;
                    if (SlotsToFill > Inventory[s].maxStack - Inventory[s].stack)
                    {
                        SlotsToFill = Inventory[s].maxStack - Inventory[s].stack;
                    }
                    Stack -= SlotsToFill;
                }
                else if (Inventory[s].type == 0)
                {
                    return true;
                }
                if (Stack <= 0)
                {
                    return true;
                }
            }
            if (Stack <= 0)
            {
                return true;
            }
            return false;
        }

        public void TryToDisableAfkPunishment()
        {
            if (GrabbingPlayer && PlayerCanEscapeGrab)
            {
                ProtectingPlayerFromHarm = false;
                GrabbingPlayer = false;
                AfkCounter = 0;
                DisplayEmotion(Emotions.Happy);
                Main.player[OwnerPos].velocity.Y = -7.5f;
                Main.player[OwnerPos].position.X = Position.X - Main.player[OwnerPos].width * 0.5f; //To avoid the player of getting stuck, or out of bounds.
            }
        }

        public void SetAimPositionToCenter()
        {
            AimDirection = Vector2.Zero;
        }

        public bool CheckForPlayerAFK()
        {
            if (OwnerPos == -1 || Is2PControlled || PlayerControl || Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOut || KnockedOut || Downed || (GuardingPosition.HasValue && Main.player[OwnerPos].Distance(CenterPosition) >= 320) || IsCommander) return false;
            Player owner = Main.player[OwnerPos];
            if (owner.GetModPlayer<PlayerMod>().CompanionCommanderLeaderPlayer > -1)
                return false;
            bool NoInputForIdle = !owner.controlLeft && !owner.controlRight && !owner.controlJump && !owner.controlDown, 
                NoInput = NoInputForIdle && owner.itemAnimation <= 0;
            if (NoInput && owner.GetModPlayer<PlayerMod>().ControllingGuardian)
            {
                TerraGuardian tg = owner.GetModPlayer<PlayerMod>().Guardian;
                NoInputForIdle = !tg.MoveLeft && !tg.MoveRight && !tg.Jump && !tg.MoveDown;
                NoInput = NoInputForIdle && tg.ItemAnimationTime <= 0;
            }
            bool IsAfkAbuse = (NoInput && IsAttackingSomething && owner.townNPCs == 0);
            if (NoInputForIdle)
            {
                if (IdleCounter < 180 * 60)
                {
                    ushort LastIdleCounter = IdleCounter;
                    IdleCounter++;
                    if (LastIdleCounter < 60 * 10 && IdleCounter >= 60 * 10)
                    {
                        IdlePosition = (int)(owner.Center.X * DivisionBy16);
                    }
                }
            }
            else
            {
                if((IdleCounter >= 60 * 10 && Math.Abs(owner.Center.X * DivisionBy16 - IdlePosition) >= 5) || IdleCounter < 60 * 10)
                {
                    IdleCounter = 0;
                }
            }
            if (NoInput)
            {
                ushort LastAFKCounter = AfkCounter;
                if (AfkCounter < 180 * 60)
                {
                    AfkCounter++;
                    if (LastAFKCounter < 60 * 60 && AfkCounter >= 60 * 60)
                    {
                        DisplayEmotion(Emotions.Neutral);
                    }
                    else if (LastAFKCounter < 180 * 60 && AfkCounter >= 180 * 60)
                    {
                        DisplayEmotion(Emotions.Sleepy);
                    }
                }
                if (CreatureAllowsAFK)
                {
                    bool PlayerInCriticalHealth = AfkCounter >= 60 * 60 && owner.statLife < owner.statLifeMax2 * 0.5f;
                    if (PlayerInCriticalHealth && !GrabbingPlayer && !owner.GetModPlayer<PlayerMod>().BeingGrabbedByGuardian)
                    {
                        if (AttemptToGrabPlayer())
                        {
                            DisplayEmotion(Emotions.Sweat);
                        }
                        else if (CurrentEmotion != Emotions.Alarmed)
                            DisplayEmotion(Emotions.Alarmed);
                        return true;
                    }
                    if (GrabbingPlayer)
                    {
                        ProtectingPlayerFromHarm = true;
                    }
                }
            }
            else
            {
                AfkCounter = 0;
            }
            return false;
        }

        public bool AttemptToGrabPlayer()
        {
            if (!Main.player[OwnerPos].dead && !IsBeingPulledByPlayer)
            {
                if (PlayerMounted)
                {
                    PlayerMounted = false;
                    GrabbingPlayer = true;
                }
                else if (HitBox.Intersects(Main.player[OwnerPos].getRect()))
                {
                    GrabbingPlayer = true;
                }
                else
                {
                    if (Main.player[OwnerPos].Center.X < Position.X)
                    {
                        MoveLeft = true;
                    }
                    else
                    {
                        MoveRight = true;
                    }
                }
            }
            return GrabbingPlayer;
        }

        public void ReleasePlayerFromGrab()
        {
            Player player = Main.player[OwnerPos];
            GrabbingPlayer = false;
            player.position.X = Position.X - player.width * 0.5f;
            player.position.Y = Position.Y - player.height - 1;
        }

        public void CheckIfNeedsToUsePotion()
        {
            if (KnockedOut)
                return;
            if (ItemAnimationTime == 0 && !HasCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown))
            {
                if (HP < MHP * 0.25f && AttemptToUseHealingPotion())
                {
                    return;
                }
                /*else
                {
                    PickWeaponForSituation();
                }*/
            }
            if (MP < MMP * 0.25f && AttemptToUseManaPotion())
            {
                return;
            }
            if (MainMod.UsingGuardianNecessitiesSystem && !Action && !HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown) && !HasBuff(ModContent.BuffType<Buffs.FirstAidCooldown>()) && Data.Injury >= GuardianData.HeavyWoundCount && HasItem(ModContent.ItemType<Items.Consumable.FirstAidKit>()))
            {
                for (int i = 0; i < 50; i++)
                {
                    if (this.Inventory[i].type == ModContent.ItemType<Items.Consumable.FirstAidKit>())
                    {
                        SelectedItem = i;
                        Action = true;
                        break;
                    }
                }
            }
        }

        public void CheckForPlayerControl()
        {
            if (OwnerPos == -1)
                return;
            if (PlayerMounted && !ReverseMount && !GuardianHasControlWhenMounted)
            {
                Player owner = Main.player[OwnerPos];
                if (!owner.stoned && !owner.frozen && !owner.webbed && !owner.GetModPlayer<PlayerMod>().KnockedOut)
                {
                    if (UsingFurniture && (owner.controlLeft || owner.controlRight || owner.controlJump))
                    {
                        LeaveFurniture();
                    }
                    MoveUp = owner.controlUp;
                    MoveDown = owner.controlDown;
                    MoveLeft = owner.controlLeft;
                    MoveRight = owner.controlRight;
                    Jump = owner.controlJump;
                    owner.controlJump = false;
                }
                if (!DoAction.InUse && !HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown) && MoveDown)
                {
                    CheckIfSomeoneNeedsRevive(true);
                }
            }
        }

        public void UpdateHealthRegen()
        {
            if (KnockedOut)
            {
                HealthRegenPower = HealthRegenTime = 0;
                return;
            }
            if (HP < 0)
                HP = 0;
            if (HasFlag(GuardianFlags.Poisoned))
            {
                HealthRegenPower -= 4;
            }
            if (HasFlag(GuardianFlags.Venom))
            {
                HealthRegenPower -= 12;
            }
            if (HasFlag(GuardianFlags.OnFire))
            {
                HealthRegenPower -= 8;
            }
            if (HasFlag(GuardianFlags.FrostBurn))
            {
                HealthRegenPower -= 12;
            }
            if (HasFlag(GuardianFlags.OnCursedFire))
            {
                HealthRegenPower -= 12;
            }
            if (HasFlag(GuardianFlags.Burned))
            {
                HealthRegenPower -= 60;
            }
            if (HasFlag(GuardianFlags.Suffocating))
            {
                HealthRegenPower -= 40;
            }
            if (HasFlag(GuardianFlags.DryadBane))
            {
                float damagepower = 1f;
                if (NPC.downedBoss1)
                    damagepower += 0.1f;
                if (NPC.downedBoss2)
                    damagepower += 0.1f;
                if (NPC.downedBoss3)
                    damagepower += 0.1f;
                if (NPC.downedQueenBee)
                    damagepower += 0.1f;
                if (Main.hardMode)
                    damagepower += 0.4f;
                if (NPC.downedMechBoss1)
                    damagepower += 0.15f;
                if (NPC.downedMechBoss2)
                    damagepower += 0.15f;
                if (NPC.downedMechBoss3)
                    damagepower += 0.15f;
                if (NPC.downedPlantBoss)
                    damagepower += 0.15f;
                if (NPC.downedGolemBoss)
                    damagepower += 0.15f;
                if (NPC.downedAncientCultist)
                    damagepower += 0.15f;
                if (Main.expertMode)
                    damagepower *= Main.expertNPCDamage;
                HealthRegenPower -= (int)(2 * (4 * damagepower));
            }
            if (HasFlag(GuardianFlags.Electrified))
            {
                HealthRegenPower -= 8;
                if (MoveLeft || MoveRight)
                    HealthRegenPower -= 32;
            }
            if (WofTongued && Main.expertMode)
            {
                HealthRegenPower -= 100;
            }
            if (HoneyWet && HealthRegenPower < 0)
            {
                HealthRegenPower += 4;
                if (HealthRegenPower > 0)
                    HealthRegenPower = 0;
            }
            /*if (AfflictedByDebuff)
            {
                HealthRegenTime = 0;
                if (HealthRegenPower > 0)
                    HealthRegenPower = 0;
            }*/
            if(HP >= MHP && HealthRegenPower >= 0)
            {
                return;
            }
            HealthRegenTime++;
            if (HasFlag(GuardianFlags.Bleeding))
            {
                HealthRegenTime = 0;
            }
            float HealthRegenValue = 0;
            if (HealthRegenTime >= 3000)
            {
                HealthRegenValue = 10;
                if (HealthRegenTime > 3600)
                {
                    HealthRegenValue++;
                    HealthRegenValue = 3600;
                }
            }
            else
            {
                HealthRegenValue = HealthRegenTime * (1f / 300);
            }
            HealthRegenValue *= (float)MHP / Data.MaxLifeCrystalHealth * 0.85f + 0.15f;
            if (HasFlag(GuardianFlags.CrimsonSetEffect))
            {
                HealthRegenTime++;
                HealthRegenValue *= 1.5f;
            }
            HealthRegenPower += (int)HealthRegenValue;
            if (HasFlag(GuardianFlags.PalladiumSetEffect))
                HealthRegenTime += 6;
            if (HasFlag(GuardianFlags.Honey))
            {
                HealthRegenTime += 2;
                HealthRegenPower += 2;
            }
            if (HasBuff(ModContent.BuffType<giantsummon.Buffs.Hug>()))
            {
                HealthRegenTime += 3;
            }
            if (HasBuff(ModContent.BuffType<giantsummon.Buffs.Sleeping>()))
            {
                HealthRegenTime += 10;
            }
            if (HasFlag(GuardianFlags.LifeRegenPotion))
                HealthRegenPower += 4;
            if (HasFlag(GuardianFlags.Werewolf))
                HealthRegenPower++;
            if (HasFlag(GuardianFlags.ImprovedHealthRegeneration))
                HealthRegenPower++;
            if (HasFlag(GuardianFlags.SunBuff) || HasFlag(GuardianFlags.MoonBuff))
                HealthRegenPower += 2;
            if (HasBuff(151))
                HealthRegenPower += 2;
            if (HasFlag(GuardianFlags.DryadWard))
            {
                HealthRegenPower += 6;
            }
            while (HealthRegenPower >= 120)
            {
                HealthRegenPower -= 120;
                if (HP < MHP)
                {
                    int HPRestore = (int)HealthHealMult;
                    if (HPRestore < 1) HPRestore = 1;
                    HP += HPRestore;

                    if (HasFlag(GuardianFlags.CrimsonSetEffect))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int dustPos = Dust.NewDust(TopLeftPosition, Width, Height, 5, 0, 0, 175, default(Color), 1.75f);
                            Main.dust[dustPos].noGravity = true;
                            Main.dust[dustPos].velocity *= 0.75f;
                            float VelX = Main.rand.Next(-40, 41), VelY = Main.rand.Next(-40, 41);
                            Main.dust[dustPos].position.X += VelX;
                            Main.dust[dustPos].position.Y += VelY;
                            Main.dust[dustPos].velocity.X -= VelX * 0.075f;
                            Main.dust[dustPos].velocity.Y -= VelY * 0.075f;
                        }
                    }
                }
            }
            if (!HasFlag(GuardianFlags.Burned) && !HasFlag(GuardianFlags.Suffocating))
            {
                int HealthDamage = 0;
                if (!WofTongued || !Main.expertMode)
                {
                    while (HealthRegenPower <= -120)
                    {
                        if (HealthRegenPower <= -480)
                        {
                            HealthDamage = 4;
                            HealthRegenPower += 480;
                        }
                        else if (HealthRegenPower <= -360)
                        {
                            HealthDamage = 3;
                            HealthRegenPower += 360;
                        }
                        else if (HealthRegenPower <= -240)
                        {
                            HealthDamage = 2;
                            HealthRegenPower += 240;
                        }
                        else if (HealthRegenPower <= -120)
                        {
                            HealthDamage = 1;
                            HealthRegenPower += 120;
                        }
                    }
                }
                if (HealthDamage > 0)
                {
                    HealthDamage = (int)(HealthDamage * HealthHealMult);
                    if (HealthDamage < 1) HealthDamage = 1;
                    this.HP -= HealthDamage;
                    CombatText.NewText(HitBox, CombatText.LifeRegen, HealthDamage.ToString(), false, true);
                    if (HP <= 0)
                    {
                        string DeathMessage = "";
                        if (HasFlag(GuardianFlags.Poisoned) || HasFlag(GuardianFlags.Venom))
                            DeathMessage = " lost It's fight against the poisons.";
                        else if (HasFlag(GuardianFlags.Electrified))
                            DeathMessage = " got tired of It's shocking experience.";
                        else
                            DeathMessage = " turned into coal.";
                        Knockout(DeathMessage);
                    }
                }
            }
            else
            {
                while (HealthRegenPower <= -600)
                {
                    HealthRegenPower += 600;
                    int Damage = (int)(5 * HealthHealMult);
                    if (Damage < 1) Damage = 1;
                    HP -= Damage;
                    CombatText.NewText(HitBox, CombatText.LifeRegen, Damage.ToString(), false, true);
                    if (HP <= 0)
                    {
                        string DeathMessage = " got tired of dancing on hot tiles.";
                        if (HasFlag(GuardianFlags.Suffocating)) DeathMessage = " couldn't hold their breath for longer...";
                        Knockout(DeathMessage);
                    }
                }
            }
            if (HP > MHP)
                HP = MHP;
        }

        public void UpdateManaRegen()
        {
            if (MP >= MMP)
                return;
            if (HasCooldown(GuardianCooldownManager.CooldownType.ManaRegenDelay))
            {
                if (HasFlag(GuardianFlags.ManaRegenDelayReduction))
                    DecreaseCooldownValue(GuardianCooldownManager.CooldownType.ManaRegenDelay);
                if (Velocity.X == 0 && Velocity.Y == 0)
                {
                    DecreaseCooldownValue(GuardianCooldownManager.CooldownType.ManaRegenDelay);
                }
            }
            int ManaRegenStack = 0;
            if (!HasCooldown(GuardianCooldownManager.CooldownType.ManaRegenDelay))
            {
                ManaRegenStack = (int)(MMP * (1f / 7)) + 1 + ManaRegenBonus;
                if (Velocity.Y == 0 && Velocity.X == 0)
                    ManaRegenStack += (int)(MMP * 0.5f);
                float RegenStack = (float)MP / MMP * 0.8f + 0.2f;
                ManaRegenStack = (int)((ManaRegenStack * RegenStack) * 1.15f);
            }
            ManaRegenTime += ManaRegenStack;
            while (ManaRegenTime >= 120)
            {
                ManaRegenTime -= 120;
                bool LastRegeneratedMana = false;
                if (MP < MMP)
                {
                    MP += ManaRegenBonus > 0 ? ManaRegenBonus : 1;
                    LastRegeneratedMana = true;
                }
                if (MP >= MMP)
                {
                    if (LastRegeneratedMana)
                    {
                        Main.PlaySound(25, CenterPosition);
                        for (int i = 0; i < 5; i++)
                        {
                            int dustPos = Dust.NewDust(TopLeftPosition, Width, Height, 45, 0f, 0f, 255, default(Color), Main.rand.Next(20, 26) * 0.01f);
                            Main.dust[dustPos].noLight = true;
                            Main.dust[dustPos].noGravity = true;
                            Main.dust[dustPos].velocity *= 0.5f;
                        }
                    }
                }
            }
            if (WaitingForManaRecharge && MP >= MMP)
            {
                WaitingForManaRecharge = false;
            }
            if (MP > MMP)
                MP = MMP;
        }

        public static void DoTriggerGroup(List<TerraGuardian> terraguardians, TriggerTypes trigger, Trigger.TriggerTarget Target, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0f)
        {
            if (terraguardians.Count == 0)
                return;
            //Add here actions that can only be triggered by one of those.
            switch (trigger)
            {
                case TriggerTypes.Downed:
                    {
                        switch (Target.TargetType)
                        {
                            case giantsummon.Trigger.TriggerTarget.TargetTypes.Player:
                            case giantsummon.Trigger.TriggerTarget.TargetTypes.TerraGuardian:
                                {
                                    bool IsPlayer = Target.TargetType == giantsummon.Trigger.TriggerTarget.TargetTypes.Player;
                                    TerraGuardian ClosestGuardian = null;
                                    float ClosestDistance = 600;
                                    Player DownedPlayer = IsPlayer ? Main.player[Target.TargetID] : null;
                                    TerraGuardian DownedGuardian = !IsPlayer ? MainMod.ActiveGuardians[Target.TargetID] : null;
                                    if(terraguardians.Count > 0)
                                    {
                                        TerraGuardian[] PossibleMentioners = terraguardians.Where(x => (Target.TargetType != giantsummon.Trigger.TriggerTarget.TargetTypes.TerraGuardian && !x.IsPlayerHostile(Main.player[Target.TargetID])) || 
                                        (x.WhoAmID != Target.TargetID && !x.IsGuardianHostile(MainMod.ActiveGuardians[Target.TargetID]))).ToArray();
                                        if (PossibleMentioners.Length > 0)
                                        {
                                            TerraGuardian WhoMentionsThis = PossibleMentioners[Main.rand.Next(PossibleMentioners.Length)];
                                            if ((IsPlayer && Target.TargetID == WhoMentionsThis.OwnerPos))
                                            {
                                                WhoMentionsThis.SaySomething(WhoMentionsThis.GetMessage(GuardianBase.MessageIDs.LeaderFallsMessage, "*They tell you a terrarian fell.*"));
                                            }
                                            else
                                            {
                                                if (Target.TargetID != WhoMentionsThis.WhoAmID)
                                                {
                                                    WhoMentionsThis.SaySomething(WhoMentionsThis.GetMessage(GuardianBase.MessageIDs.AllyFallsMessage, "*They tell you an ally fell.*"));
                                                }
                                            }
                                        }
                                    }
                                    foreach(TerraGuardian tg in terraguardians)
                                    {
                                        if (!tg.DoAction.InUse && !tg.PlayerMounted && !tg.SittingOnPlayerMount)
                                        {
                                            if (IsPlayer)
                                            {
                                                if (!tg.IsPlayerHostile(DownedPlayer) && !PlayerMod.IsBeingCarriedBySomeone(DownedPlayer) &&
                                                    Actions.CarryDownedAlly.CanCarryAlly(tg, DownedPlayer))
                                                {
                                                    float Distance = tg.Distance(DownedPlayer.Center) - (DownedPlayer.width * 0.5f + tg.Width * 0.5f);
                                                    if (Distance < ClosestDistance)
                                                    {
                                                        ClosestGuardian = tg;
                                                        ClosestDistance = Distance;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (tg.WhoAmID != DownedGuardian.WhoAmID && !tg.IsGuardianHostile(DownedGuardian) && !DownedGuardian.IsBeingCarriedBySomeone() &&
                                                    Actions.CarryDownedAlly.CanCarryAlly(tg, DownedGuardian))
                                                {
                                                    float Distance = tg.Distance(DownedGuardian.CenterPosition) - (DownedGuardian.Width * 0.5f + tg.Width * 0.5f);
                                                    if (Distance < ClosestDistance)
                                                    {
                                                        ClosestGuardian = tg;
                                                        ClosestDistance = Distance;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if(ClosestGuardian != null)
                                    {
                                        if (Target.TargetType == giantsummon.Trigger.TriggerTarget.TargetTypes.Player)
                                            ClosestGuardian.StartNewGuardianAction(new Actions.CarryDownedAlly(Main.player[Target.TargetID]));
                                        else
                                            ClosestGuardian.StartNewGuardianAction(new Actions.CarryDownedAlly(MainMod.ActiveGuardians[Target.TargetID]));
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        public void DoTrigger(TriggerTypes trigger, Trigger.TriggerTarget Target, int Value = 0, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
        {
            if (!Base.WhenTriggerActivates(this, trigger, Target, Value, Value2, Value3, Value4, Value5))
                return;
            switch (trigger)
            {
                case TriggerTypes.Hurt:
                    {
                        switch (Target.TargetType)
                        {
                            case giantsummon.Trigger.TriggerTarget.TargetTypes.Player:
                                {
                                    Player player = Main.player[Target.TargetID];
                                    if (!IsPlayerHostile(player))
                                    {
                                        int Damage = Value2;
                                        bool Pvp = Value4 == 1;
                                        byte HostileType = 255;
                                        int HostileID = 0;
                                        float HostileDistance = 512f;
                                        bool Urgency = player.statLife < player.statLifeMax2 * 0.5f;
                                        if (Pvp)
                                        {
                                            for (int p = 0; p < 255; p++)
                                            {
                                                if (p != Value && Main.player[p].active && !Main.player[p].dead && IsPlayerHostile(Main.player[p]))
                                                {
                                                    float Distance = (Main.player[p].Center - player.Center).Length();
                                                    if (Distance < HostileDistance)
                                                    {
                                                        HostileDistance = Distance;
                                                        HostileType = 0;
                                                        HostileID = p;
                                                    }
                                                }
                                            }
                                        }
                                        for (int n = 0; n < 200; n++)
                                        {
                                            if (Main.npc[n].active && !Main.npc[n].townNPC && IsNpcHostile(Main.npc[n]))
                                            {
                                                float Distance = (Main.npc[n].Center - player.Center).Length();
                                                if (Distance < HostileDistance)
                                                {
                                                    HostileDistance = Distance;
                                                    HostileType = 1;
                                                    HostileID = n;
                                                }
                                            }
                                        }
                                        foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                                        {
                                            if (IsGuardianHostile(tg))
                                            {
                                                float Distance = (tg.CenterPosition - player.Center).Length();
                                                if (Distance < HostileDistance)
                                                {
                                                    HostileDistance = Distance;
                                                    HostileType = 2;
                                                    HostileID = tg.WhoAmID;
                                                }
                                            }
                                        }
                                        if (HostileType < 255)
                                        {
                                            switch (HostileType)
                                            {
                                                case 0:
                                                    if (Urgency || !AttackingTarget)
                                                    {
                                                        TargetID = HostileID;
                                                        TargetType = TargetTypes.Player;
                                                    }
                                                    break;
                                                case 1:
                                                    if (Urgency || !AttackingTarget)
                                                    {
                                                        TargetID = HostileID;
                                                        TargetType = TargetTypes.Npc;
                                                    }
                                                    break;
                                                case 2:
                                                    if (Urgency || !AttackingTarget)
                                                    {
                                                        TargetID = HostileID;
                                                        TargetType = TargetTypes.Guardian;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;

                            case giantsummon.Trigger.TriggerTarget.TargetTypes.TerraGuardian:
                                {
                                    if (MainMod.ActiveGuardians.ContainsKey(Target.TargetID))
                                    {
                                        TerraGuardian tg = MainMod.ActiveGuardians[Target.TargetID];
                                        if (!IsGuardianHostile(tg))
                                        {
                                            int Damage = Value2;
                                            bool Pvp = Value4 == 1;
                                            byte HostileType = 255;
                                            int HostileID = 0;
                                            float HostileDistance = 512f;
                                            bool Urgency = tg.HP < tg.MHP * 0.5f;
                                            if (Pvp)
                                            {
                                                for (int p = 0; p < 255; p++)
                                                {
                                                    if (p != Value && Main.player[p].active && !Main.player[p].dead && IsPlayerHostile(Main.player[p]))
                                                    {
                                                        float Distance = (Main.player[p].Center - tg.CenterPosition).Length();
                                                        if (Distance < HostileDistance)
                                                        {
                                                            HostileDistance = Distance;
                                                            HostileType = 0;
                                                            HostileID = p;
                                                        }
                                                    }
                                                }
                                            }
                                            for (int n = 0; n < 200; n++)
                                            {
                                                if (Main.npc[n].active && !Main.npc[n].townNPC && IsNpcHostile(Main.npc[n]))
                                                {
                                                    float Distance = (Main.npc[n].Center - tg.CenterPosition).Length();
                                                    if (Distance < HostileDistance)
                                                    {
                                                        HostileDistance = Distance;
                                                        HostileType = 1;
                                                        HostileID = n;
                                                    }
                                                }
                                            }
                                            foreach (TerraGuardian tg2 in MainMod.ActiveGuardians.Values)
                                            {
                                                if (tg.WhoAmID != tg2.WhoAmID && IsGuardianHostile(tg2))
                                                {
                                                    float Distance = (tg2.CenterPosition - tg2.CenterPosition).Length();
                                                    if (Distance < HostileDistance)
                                                    {
                                                        HostileDistance = Distance;
                                                        HostileType = 2;
                                                        HostileID = tg2.WhoAmID;
                                                    }
                                                }
                                            }
                                            if (HostileType < 255)
                                            {
                                                switch (HostileType)
                                                {
                                                    case 0:
                                                        if (Urgency || !AttackingTarget)
                                                        {
                                                            TargetID = HostileID;
                                                            TargetType = TargetTypes.Player;
                                                        }
                                                        break;
                                                    case 1:
                                                        if (Urgency || !AttackingTarget)
                                                        {
                                                            TargetID = HostileID;
                                                            TargetType = TargetTypes.Npc;
                                                        }
                                                        break;
                                                    case 2:
                                                        if (Urgency || !AttackingTarget)
                                                        {
                                                            TargetID = HostileID;
                                                            TargetType = TargetTypes.Guardian;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case giantsummon.Trigger.TriggerTarget.TargetTypes.NPC:
                                {
                                    NPC npc = Main.npc[Target.TargetID];
                                    if (!IsNpcHostile(npc) && npc.townNPC)
                                    {
                                        int Damage = Value2;
                                        bool Pvp = Value4 == 1;
                                        byte HostileType = 255;
                                        int HostileID = 0;
                                        float HostileDistance = 512f;
                                        bool Urgency = npc.life < npc.lifeMax * 0.5f;
                                        if (Pvp)
                                        {
                                            for (int p = 0; p < 255; p++)
                                            {
                                                if (p != Value && Main.player[p].active && !Main.player[p].dead && IsPlayerHostile(Main.player[p]))
                                                {
                                                    float Distance = (Main.player[p].Center - npc.Center).Length();
                                                    if (Distance < HostileDistance)
                                                    {
                                                        HostileDistance = Distance;
                                                        HostileType = 0;
                                                        HostileID = p;
                                                    }
                                                }
                                            }
                                        }
                                        for (int n = 0; n < 200; n++)
                                        {
                                            if (Main.npc[n].active && !Main.npc[n].townNPC && IsNpcHostile(Main.npc[n]))
                                            {
                                                float Distance = (Main.npc[n].Center - npc.Center).Length();
                                                if (Distance < HostileDistance)
                                                {
                                                    HostileDistance = Distance;
                                                    HostileType = 1;
                                                    HostileID = n;
                                                }
                                            }
                                        }
                                        foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                                        {
                                            if (IsGuardianHostile(tg))
                                            {
                                                float Distance = (tg.CenterPosition - npc.Center).Length();
                                                if (Distance < HostileDistance)
                                                {
                                                    HostileDistance = Distance;
                                                    HostileType = 2;
                                                    HostileID = tg.WhoAmID;
                                                }
                                            }
                                        }
                                        if (HostileType < 255)
                                        {
                                            switch (HostileType)
                                            {
                                                case 0:
                                                    if (Urgency || !AttackingTarget)
                                                    {
                                                        TargetID = HostileID;
                                                        TargetType = TargetTypes.Player;
                                                    }
                                                    break;
                                                case 1:
                                                    if (Urgency || !AttackingTarget)
                                                    {
                                                        TargetID = HostileID;
                                                        TargetType = TargetTypes.Npc;
                                                    }
                                                    break;
                                                case 2:
                                                    if (Urgency || !AttackingTarget)
                                                    {
                                                        TargetID = HostileID;
                                                        TargetType = TargetTypes.Guardian;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                        
                    }
                    break;

                case TriggerTypes.Spotted:
                    switch (Target.TargetType)
                    {
                        case giantsummon.Trigger.TriggerTarget.TargetTypes.NPC:
                            if (!MainMod.LastBossSpotted && MessageTime <= 0)
                            {
                                if (Terraria.ID.NPCID.Sets.TechnicallyABoss[Main.npc[Target.TargetID].type])
                                {
                                    string Message = GetMessage(GuardianBase.MessageIDs.SpottedABoss);
                                    if (Message != "")
                                    {
                                        MainMod.LastBossSpotted = true;
                                        SaySomethingCanSchedule(GuardianMouseOverAndDialogueInterface.MessageParser(Message, this), false, Main.rand.Next(20, 60));
                                    }
                                }
                            }
                            break;
                        case giantsummon.Trigger.TriggerTarget.TargetTypes.Player:
                            {
                                Player player = Main.player[Target.TargetID];
                                if (!DoAction.InUse && !PlayerMounted && !SittingOnPlayerMount && !player.dead && player.GetModPlayer<PlayerMod>().KnockedOut && 
                                    !IsPlayerHostile(player) && 
                                    !player.GetModPlayer<PlayerMod>().MountedOnGuardian && !PlayerMod.IsBeingCarriedBySomeone(player) && 
                                    Actions.CarryDownedAlly.CanCarryAlly(this, player))
                                {
                                    StartNewGuardianAction(new Actions.CarryDownedAlly(player));
                                }
                            }
                            break;
                        case giantsummon.Trigger.TriggerTarget.TargetTypes.TerraGuardian:
                            {
                                TerraGuardian guardian = MainMod.ActiveGuardians[Target.TargetID];
                                if (!guardian.Downed && !DoAction.InUse && !PlayerMounted && !SittingOnPlayerMount && !guardian.Downed && guardian.KnockedOut && !IsGuardianHostile(guardian) && !guardian.IsBeingCarriedBySomeone() &&
                                    Actions.CarryDownedAlly.CanCarryAlly(this, guardian))
                                {
                                    StartNewGuardianAction(new Actions.CarryDownedAlly(guardian));
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        public void ResetHealthRegen()
        {
            HealthRegenTime = 0;
            HealthRegenPower = 0;
        }

        public void Spawn()
        {
            Paths.Clear();
            if (PlayerControl && OwnerPos > -1 && !Main.player[OwnerPos].dead && Main.player[OwnerPos].SpawnX > -1 && Main.player[OwnerPos].SpawnY > -1)
            {
                Player Owner = Main.player[OwnerPos];
                Position.X = Owner.SpawnX * 16;
                Position.Y = Owner.SpawnY * 16 - Height;
            }
            else if (!PlayerControl && OwnerPos > -1 && !Main.player[OwnerPos].dead)
            {
                Player Owner = Main.player[OwnerPos];
                Position.X = Owner.Center.X;
                Position.Y = Owner.position.Y + Owner.height;
            }
            else
            {
                WorldMod.GuardianTownNpcState townstate = GetTownNpcInfo;
                if (townstate != null && !townstate.Homeless)
                {
                    Position.X = townstate.HomeX * 16;
                    Position.Y = townstate.HomeY * 16;
                }
                else
                {
                    Position.X = Main.spawnTileX * 16;
                    Position.Y = Main.spawnTileY * 16;
                }
            }
            TargetID = -1;
            SetAimPositionToCenter();
            Breath = BreathMax;
            BreathCooldown = 0;
            if (Downed)
            {
                HP = (int)(100 * HealthHealMult);
                MP = MMP;
                if (HasCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown))
                    RemoveCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown);
                KnockedOut = KnockedOutCold = false;
            }
            Active = true;
            Wet = LavaWet = HoneyWet = false;
            Downed = false;
            WakeupTime = 0;
            ImmuneTime = GetImmuneTime;
            Falling = true;
            ItemAnimationTime = 0;
            SetFallStart();
            TargetID = -1;
            GrabbingPlayer = false;
            UpdateStatus = true;
            if (OwnerPos == Main.myPlayer && OwnerPos > -1 && AfkCounter >= 180 * 60 && !CreatureAllowsAFK && (!PlayerMod.HasBuddiesModeOn(Main.player[OwnerPos]) || !PlayerMod.GetPlayerBuddy(Main.player[OwnerPos]).IsSameID(this)))
            {
                Main.player[OwnerPos].GetModPlayer<PlayerMod>().DismissGuardian();
                Main.NewText(Name + " has left your battle.", Color.Red);
            }
        }

        public void SetFallStart()
        {
            FallStart = (int)(this.Position.Y * DivisionBy16);
            IgnoreMass = false;
        }

        public void EnterDownedState()
        {
            NegativeReviveBoost = false;
            bool LastWasKOd = KnockedOut;
            if (!KnockedOut)
            {
                ChatMessage = "";
                MessageSchedule.Clear();
            }
            KnockedOut = true;
            if (!LastWasKOd && !IsPlayerHostile(Main.player[Main.myPlayer]) && InPerceptionRange(Main.player[Main.myPlayer].Center))
            {
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.AllyFallsMessage);
            }
            if (HasFlag(GuardianFlags.HealthGoesToZeroWhenKod))
            {
                HP = 0;
            }
            else
            {
                float LifeMaxValue = 0.5f;
                if (HasBuff(ModContent.BuffType<giantsummon.Buffs.Injury>()))
                    LifeMaxValue = 0.25f;
                HP += (int)(MHP * LifeMaxValue);
                if (HP > MHP * 0.5f)
                    HP = (int)(MHP * 0.5f);
                if (HP <= 0 || HasBuff(ModContent.BuffType<giantsummon.Buffs.HeavyInjury>()))
                {
                    if (MainMod.GuardiansDontDiesAfterDownedDefeat || HasFlag(GuardianFlags.CantDie))
                        KnockedOutCold = true;
                    else
                        Knockout(" wasn't able to endure the damage.");
                }
            }
            if (PlayerMounted)
                ToggleMount(true, false);
            if (KnockedOutCold && HasFlag(GuardianFlags.CantBeKnockedOutCold))
                KnockedOutCold = false;
            TriggerHandler.FireGuardianDownedTrigger(this.CenterPosition, this, 0, false);
            UpdateStatus = true;
            DoAction.InUse = false;
            if (!LastWasKOd && OwnerPos == Main.myPlayer)
            {
                Main.NewText(Name + " has been knocked out.", Color.OrangeRed);
                if (!Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialKnockOutIntroduction)
                {
                    Main.NewText("Someone got knocked out! Companions that aren't attacking enemies can help revive the one knocked out if they are in range. You can resurrect too, by standing on the position and holding the left mouse button on the character.");
                    if (MainMod.GuardiansDontDiesAfterDownedDefeat)
                    {
                        Main.NewText("They can still be hurt in this state. If their health reaches 0 while in this state, they enter a Knocked Out cold state, where they can't be hurt, and there is no natural health regeneration. They can still be revived.");
                    }
                    else
                    {
                        Main.NewText("They can still be hurt in this state. If their health reaches 0 while in this state, they dies.");
                    }
                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialKnockOutIntroduction = true;
                }
            }
        }

        public void ExitDownedState()
        {
            Paths.Clear();
            TargetID = -1;
            WofFood = false;
            KnockedOut = KnockedOutCold = false;
            HealthRegenPower = HealthRegenTime = 0;
            float HealthRegenValue = 1;
            if (HasBuff(ModContent.BuffType<Buffs.HeavyInjury>()))
            {
                AddBuff(ModContent.BuffType<Buffs.HeavyInjury>(), 600 * 60);
                HealthRegenValue = 0.5f;
            }
            else if (HasBuff(ModContent.BuffType<Buffs.Injury>()))
            {
                RemoveBuff(ModContent.BuffType<Buffs.Injury>());
                AddBuff(ModContent.BuffType<Buffs.HeavyInjury>(), 300 * 60);
                HealthRegenValue = 0.75f;
            }
            else
            {
                AddBuff(ModContent.BuffType<Buffs.Injury>(), 90 * 60);
            }
            HP = (int)((MHP / 2 + ReviveBoost * 10) * HealthRegenValue);
            Rotation = 0f;
            CombatText.NewText(HitBox, Color.Green, "Revived!", true);
            if (OwnerPos == Main.myPlayer || OwnerPos == -1)
            {
                string Mes = GetMessage(ReviveBoost > 0 ? GuardianBase.MessageIDs.ReviveByOthersHelp : GuardianBase.MessageIDs.RevivedByRecovery);
                if(Mes != "")
                {
                    SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Mes, this));
                }
            }
            //    Main.NewText(Name + " woke up!", Color.Green);

            ImmuneTime = (HasFlag(GuardianFlags.ImprovedImmuneTime) ? 120 : 60) * 3;
            UpdateStatus = true;
        }

        public void DoForceKill(string DeathMessage = "")
        {
            ForceKill = true;
            Knockout(DeathMessage);
        }

        public void Knockout(string DeathText = "")
        {
            if (!Active)
                return;
            if (ForceKill && !HasFlag(GuardianFlags.CantDie))
            {
                ForceKill = false;
            }
            else if (FriendlyDuelDefeat || HasFlag(GuardianFlags.CantDie) || (MainMod.GuardiansGetKnockedOutUponDefeat && !KnockedOut && (HP + MHP / 2 > 0 || MainMod.GuardiansDontDiesAfterDownedDefeat)))
            {
                FriendlyDuelDefeat = false;
                EnterDownedState();
                return;
            }
            else if (MainMod.GuardiansDontDiesAfterDownedDefeat && KnockedOut)
            {
                KnockedOutCold = true;
                if (HP < 0) HP = 0;
                HealthRegenTime = HealthRegenPower = 0;
                return;
            }
            WofFood = false;
            if (HP > 0)
                HP = 0;
            Downed = true;
            WakeupTime = 60 * 10;
            if (DeathText == "")
                DeathText = " has been knocked out.";
            string Message = ReferenceName + DeathText;
            if (OwnerPos > -1)
            {
                if (PlayerControl)
                {
                    Main.player[OwnerPos].immuneTime = 0;
                    Main.player[OwnerPos].immune = false;
                    Main.player[OwnerPos].KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("'s " + Name + " has been defeated while under control."), 9999, 0);
                }
            }
            //
            ChatMessage = "";
            MessageSchedule.Clear();
            //
            UsingFurniture = false;
            furniturex = furniturey = -1;
            IsBeingPulledByPlayer = false;
            SittingOnPlayerMount = false;
            if (GrabbingPlayer || PlayerMounted)
            {
                Main.player[OwnerPos].position.X = Position.X - Main.player[OwnerPos].width * 0.5f;
            }
            GrabbingPlayer = false;
            PlayerMounted = false;
            Base.DeadSound.PlaySound(CenterPosition);
            Main.NewText(Message, Color.Red);
            ItemAnimationTime = 0;
            JumpHeight = 0;
            KnockdownRotation = 0f;
            Velocity.X *= 1.5f;
            Velocity.Y = -12.5f;
            ResetHealthRegen();
            WofFacing = WofTongued = false;
            for (int c = 0; c < Cooldowns.Count; c++)
            {
                if (Cooldowns[c].ResetUponDeath)
                    Cooldowns.RemoveAt(c);
            }
            CooldownException.Clear();
            if (FoodStacker > 0 && HasBuff(Terraria.ID.BuffID.WellFed))
            {
                FoodStacker--;
            }
            if (DrinkStacker > 0 && HasBuff(Terraria.ID.BuffID.Tipsy))
            {
                DrinkStacker--;
            }
            for (int b = 0; b < Buffs.Count; b++)
            {
                BuffData buff = Buffs[b];
                if (buff.ID != ModContent.BuffType<giantsummon.Buffs.FirstAidCooldown>())
                {
                    Buffs.Remove(buff);
                }
            }
            SubAttackCooldown.Clear();
            TriggerHandler.FireGuardianDeathTrigger(CenterPosition, this, false);
            //Buffs.Clear();
        }

        public void LookAt(Vector2 TargetPosition, bool Force = false) {
            if(Force || Velocity.X == 0)
                LookingLeft = TargetPosition.X - Position.X < 0;
        }

        public bool HasDuplicateEquipped(int id)
        {
            for (int e = 0; e < 8; e++)
            {
                if (this.Equipments[e].type == id)
                    return true;
            }
            return false;
        }

        public bool AttemptToUseHealingPotion()
        {
            if (ItemAnimationTime > 0 || HasCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown))
            {
                return false;
            }
            int PotionPosition = -1;
            int LastHealingDifference = int.MaxValue;
            int ToHealValue = MHP - HP;
            int PotionCount = 0;
            for (int i = 0; i < 50; i++)
            {
                if (this.Inventory[i].type > 0 && this.Inventory[i].potion && this.Inventory[i].healLife > 0)// && giantsummon.IsGuardianItem(this.Inventory[i]))
                {
                    if (!(this.Inventory[i].modItem is Items.GuardianItemPrefab) || ((Items.GuardianItemPrefab)this.Inventory[i].modItem).GuardianCanUse(this))
                    {
                        PotionCount += this.Inventory[i].stack;
                        int RestoreResult = Math.Abs((int)(this.Inventory[i].healLife * HealthHealMult) - ToHealValue);
                        if (RestoreResult < LastHealingDifference)
                        {
                            LastHealingDifference = RestoreResult;
                            PotionPosition = i;
                        }
                    }
                }
            }
            if (PotionPosition > -1)
            {
                SelectedItem = PotionPosition;
                Action = true;
                string Message = "";
                if (PotionCount == 1)
                {
                    Message = GetMessage(GuardianBase.MessageIDs.UsesLastPotion);
                }
                else if (PotionCount == 5)
                {
                    Message = GetMessage(GuardianBase.MessageIDs.RunningOutOfPotions);
                }
                if (Message != "")
                {
                    SaySomethingCanSchedule(GuardianMouseOverAndDialogueInterface.MessageParser(Message, this), false, Inventory[SelectedItem].useAnimation * 3 + Main.rand.Next(20, 60));
                }
                return true;
            }
            return false;
        }

        public bool AttemptToUseManaPotion()
        {
            if (ItemAnimationTime > 0) return false;
            int PotionPosition = PickManaPotion();
            if (PotionPosition > -1)
            {
                SelectedItem = PotionPosition;
                Action = true;
                return true;
            }
            return false;
        }

        public int PickManaPotion()
        {
            int HighestHealingValue = 0, PotionPosition = -1;
            for (int i = 0; i < 50; i++)
            {
                if (this.Inventory[i].type > 0 && this.Inventory[i].healMana > HighestHealingValue)// && giantsummon.IsGuardianItem(this.Inventory[i]))
                {
                    HighestHealingValue = i;
                    PotionPosition = i;
                }
            }
            return PotionPosition;
        }

        public void ToggleWait(bool Guarding = false)
        {
            bool OwnerIsPlayer = OwnerPos > -1 && OwnerPos == Main.myPlayer;
            if (GuardingPosition.HasValue)
            {
                GuardingPosition = null;
                //if(OwnerIsPlayer) Main.NewText(this.Name + " is now following you.");
            }
            else
            {
                if (OwnerIsPlayer)
                {
                    GuardingPosition = new Point((int)(Main.player[OwnerPos].Center.X * DivisionBy16), (int)((Main.player[OwnerPos].position.Y + Main.player[OwnerPos].height - 1) * DivisionBy16));
                    //if (OwnerIsPlayer) Main.NewText(this.Name + " is now guarding some place.");
                }
                else
                {
                    GuardingPosition = new Point((int)(Position.X * DivisionBy16), (int)(Position.Y * DivisionBy16));
                }
                IsGuardingPlace = Guarding;
            }
        }
		
		public void TogglePlayerControl(bool Forced = false)
		{
            if (Base.IsTerrarian)
            {
                Main.NewText("You can't control human companions.");
                return;
            }
			if(OwnerPos > -1)
            {
                if (!Forced && (HasFlag(GuardianFlags.Frozen) || HasFlag(GuardianFlags.Petrified)))
                {
                    Main.NewText("You can't dismount while the guardian is on this state.");
                }
                else if (!PlayerControl)
                {
                    if (PlayerMounted || SittingOnPlayerMount || this.HitBox.Intersects(Main.player[OwnerPos].getRect()))
                    {
                        if (SittingOnPlayerMount)
                            DoSitOnPlayerMount(false);
                        if (PlayerMounted)
                            ToggleMount(true, false);
                        PlayerControl = true;
                    }
                    else
                    {
                        Main.NewText("Get closer to the Guardian to be able to use this.");
                    }
                }
                else if (PlayerControl)
                {
                    PlayerControl = false;
                    Main.player[OwnerPos].position.X = Position.X - Main.player[OwnerPos].width * 0.5f;
                    Main.player[OwnerPos].position.Y = Position.Y - Main.player[OwnerPos].height * GravityDirection;
                }
			}
		}

        public bool ToggleMount(bool Forced = false, bool CheckIfInArea = true)
        {
            if (DoAction.InUse && DoAction.Inactivity)
                return false;
            if (PlayerControl)
            {
                //Main.NewText("You can't use this while Player Control is active.");
            }
            else
            {
                if (!Forced && (HasFlag(GuardianFlags.Frozen) || HasFlag(GuardianFlags.Petrified)))
                {
                    //Main.NewText("You can't dismount while the guardian is on this state.");
                }
                else if (PlayerMounted)
                {
                    if (ReverseMount)
                    {
                        this.Position.X = Main.player[OwnerPos].Center.X;
                        this.Position.Y = Main.player[OwnerPos].position.Y + (Main.player[OwnerPos].height - 1) * Main.player[OwnerPos].gravDir;
                        this.Velocity = Main.player[OwnerPos].velocity;
                        SetFallStart();
                    }
                    else
                    {
                        Main.player[OwnerPos].position.X = Position.X - Main.player[OwnerPos].width * 0.5f;
                        Main.player[OwnerPos].position.Y = Position.Y - Main.player[OwnerPos].height * GravityDirection;
                        Main.player[OwnerPos].velocity = this.Velocity;
                        Main.player[OwnerPos].fallStart = this.FallStart;
                    }
                    SittingOnPlayerMount = false;
                    PlayerMounted = false;
                    UpdateStatus = true;
                    GuardianHasControlWhenMounted = false;
                    return true;
                }
                else
                {
                    if (!CheckIfInArea || Main.player[OwnerPos].getRect().Intersects(HitBox))
                    {
                        PlayerMounted = true;
                        UpdateStatus = true;
                        return true;
                    }
                    else
                    {
                        //Main.NewText("Can't mount while away from the Guardian.");
                    }
                }
            }
            return false;
        }

        public bool HasMana(int ManaCost)
        {
            return MP >= (int)(ManaCost * this.ManaCostMult);
        }

        public byte PickWeaponToUseByInventoryOrder()
        {
            if (ItemAnimationTime == 0)
            {
                SelectedItem = -1;
                for (int i = 0; i < 10; i++)
                {
                    if (this.Inventory[i].type > 0 && this.Inventory[i].damage > 0 && (this.Inventory[i].melee || this.Inventory[i].ranged || this.Inventory[i].magic || this.Inventory[i].thrown))
                    {
                        bool Allow = true;
                        if (this.Inventory[i].mana > 0 && (!HasMana(this.Inventory[i].mana) || HasFlag(GuardianFlags.Silence)))
                        {
                            Allow = false;
                        }
                        else if (MainMod.IsGuardianItem(this.Inventory[i]) && ((Items.GuardianItemPrefab)this.Inventory[i].modItem).Heavy && ((PlayerMounted && !ReverseMount && !UseHeavyMeleeAttackWhenMounted)))
                        {
                            Allow = false;
                        }
                        if (Allow)
                        {
                            SelectedItem = i;
                            break;
                        }
                    }
                }
            }
            if (SelectedItem == -1)
                return Melee;
            Item item = Inventory[SelectedItem];
            if (item.melee)
                return Melee;
            if (item.ranged)
                return Ranged;
            if (item.magic)
                return Magic;
            if (item.summon)
                return Summon;
            return Ranged;
        }

        public bool IsBlockedAhead()
        {
            byte CeilingMaxJumpHeightCheck = 12; //8
            int FeetY = (int)(Position.Y * DivisionBy16);
            {
                int StartTileX = (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16),
                    EndTileX = (int)((Position.X + CollisionWidth * 0.5f + 1) * DivisionBy16);
                for (byte y = 3; y < CeilingMaxJumpHeightCheck; y++)
                {
                    for(int x = StartTileX; x < EndTileX; x++)
                    {
                        if(MainMod.IsSolidTile(x, FeetY - y) && !Terraria.ID.TileID.Sets.Platforms[MainMod.GetTile(x, FeetY - y).type])
                        {
                            CeilingMaxJumpHeightCheck = y;
                            break;
                        }
                    }
                    if (CeilingMaxJumpHeightCheck >= y)
                        break;
                }
            }
            int CheckAhead = (int)((Position.X + (CollisionWidth * 0.5f + 8) * Direction) * DivisionBy16);
            byte Gap = 0;
            for(int y = 0; y < CeilingMaxJumpHeightCheck; y++)
            {
                bool BlocksWay = false;
                if(MainMod.IsSolidTile(CheckAhead, FeetY - y))
                {
                    BlocksWay = true;
                    Tile tile = MainMod.GetTile(CheckAhead, FeetY - y);
                    if (Terraria.ID.TileID.Sets.Platforms[tile.type] ||
                        tile.type == Terraria.ID.TileID.ClosedDoor ||
                        tile.type == Terraria.ID.TileID.TallGateClosed)
                    {
                        BlocksWay = false;
                    }
                }
                if (!BlocksWay)
                {
                    Gap++;
                    if (Gap >= 3)
                        return false;
                }
                else
                    Gap = 0;
            }
            return true;
        }

        public bool UseMagicMirror()
        {
            bool CanUseItem = !LastAction && ItemAnimationTime == 0;
            if (HasItem(Terraria.ID.ItemID.MagicMirror))
            {
                if (CanUseItem)
                {
                    SelectedItem = GetItemPosition(Terraria.ID.ItemID.MagicMirror);
                    Action = true;
                    return true;
                }
            }
            else if (HasItem(Terraria.ID.ItemID.IceMirror))
            {
                if (CanUseItem)
                {
                    SelectedItem = GetItemPosition(Terraria.ID.ItemID.IceMirror);
                    Action = true;
                    return true;
                }
            }
            else if (HasItem(Terraria.ID.ItemID.CellPhone))
            {
                if (CanUseItem)
                {
                    SelectedItem = GetItemPosition(Terraria.ID.ItemID.CellPhone);
                    Action = true;
                    return true;
                }
            }
            return false;
        }

        public bool HasAmmo(Item weapon)
        {
            if(weapon.useAmmo > 0)
            {
                for(int i = 0; i < 50; i++)
                {
                    if (Inventory[i].ammo == weapon.useAmmo && Inventory[i].stack > 0)
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public void GetAmmoInfo(Item i, bool UseAmmo, out int ProjID, out float ShotSpeed, out int Damage, out float Knockback)
        {
            ProjID = i.shoot;
            ShotSpeed = i.shootSpeed;
            Damage = i.damage;
            Knockback = i.knockBack;
            if (i.consumable)
            {
                if (UseAmmo)
                {
                    i.stack--;
                    if (i.stack <= 0)
                        i.SetDefaults(0, true);
                }
                return;
            }
            if (i.useAmmo == Terraria.ID.AmmoID.None && !i.consumable)
				return;
            bool HasAmmo = false;
            for (int j = 0; j < 50; j++)
            {
                if (Inventory[j].ammo == i.useAmmo)
                {
                    if (Inventory[j].type == Terraria.ID.ItemID.FallenStar)
                        ProjID = 12;
                    else
                        ProjID = i.shoot;
                    HasAmmo = true;
                    ShotSpeed += Inventory[j].shootSpeed;
                    Damage += Inventory[j].damage;
                    if (this.Inventory[j].ammo == Terraria.ID.AmmoID.Arrow && HasFlag(GuardianFlags.ArcheryPotion))
                    {
                        Damage += (int)(Damage * 0.2f);
                        if (ShotSpeed < 20f)
                        {
                            ShotSpeed *= 1.2f;
                            if (ShotSpeed > 20) ShotSpeed = 20f;
                        }
                    }
                    if (i.type == 1946)
                    {
                        ProjID = 338 + this.Inventory[j].type - 771;
                    }
                    else if (i.useAmmo == Terraria.ID.AmmoID.Rocket)
                    {
                        ProjID += this.Inventory[j].shoot;
                    }
                    else if (i.useAmmo == 780)
                    {
                        ProjID += this.Inventory[j].shoot;
                    }
                    else if (this.Inventory[j].shoot > 0)
                    {
                        ProjID = this.Inventory[j].shoot;
                    }
                    if (i.type == 3019 && ProjID == 1)
                        ProjID = 485;
                    if (i.type == 3052)
                        ProjID = 495;
                    if (i.type == 3245 && ProjID == 21)
                        ProjID = 532;
                    if (ProjID == 42)
                    {
                        if (this.Inventory[j].type == 370)
                        {
                            ProjID = 65;
                            Damage += 5;
                        }
                        else if (this.Inventory[j].type == 408)
                        {
                            ProjID = 68;
                            Damage += 5;
                        }
                        else if (this.Inventory[j].type == 1246)
                        {
                            ProjID = 354;
                            Damage += 5;
                        }
                    }
                    if (i.type == 2888 && ProjID == 1)
                    {
                        ProjID = 469;
                    }
                    if (HasFlag(GuardianFlags.ArrowBuff) && (i.useAmmo == Terraria.ID.AmmoID.Arrow || i.useAmmo == Terraria.ID.AmmoID.Stake))
                    {
                        Knockback *= 1.1f;
                        ShotSpeed *= 1.1f;
                    }
                    bool DontUseAmmo = !UseAmmo;
                    if (!DontUseAmmo)
                    {
                        if (OwnerPos == -1)
                        {
                            DontUseAmmo = true;
                        }
                        else if (i.type == 3245)
                        {
                            if (Main.rand.Next(3) == 0)
                                DontUseAmmo = true;
                        }
                        else if ((i.type == 3475 || i.type == 3540) && Main.rand.Next(3) != 0)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.ArrowBuff) && i.useAmmo == Terraria.ID.AmmoID.Arrow && Main.rand.Next(5) == 0)
                        {
                            DontUseAmmo = true;
                        }
                        else if ((i.type == 1782 || i.type == 98) && Main.rand.Next(3) == 0)
                        {
                            DontUseAmmo = true;
                        }
                        else if ((i.type == 2270 || i.type == 533 || i.type == 1929 || i.type == 1553) && Main.rand.Next(2) == 0)
                        {
                            DontUseAmmo = true;
                        }
                        else if (i.type == 434 && ItemAnimationTime < i.useAnimation - 2)
                            DontUseAmmo = true;
                        else if (ProjID == 85 && ItemAnimationTime < i.useAnimation - 6)
                            DontUseAmmo = true;
                        else if (i.type >= 145 && i.type <= 149 && this.ItemAnimationTime < ItemAnimationTime - 5)
                            DontUseAmmo = true;
                        else if (i.thrown && HasFlag(GuardianFlags.NinjaSetEffect) && Main.rand.NextDouble() < 0.33)
                            DontUseAmmo = true;
                        else if (i.thrown && HasFlag(GuardianFlags.FossilSetEffect) && Main.rand.NextDouble() < 0.5)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.NecroSetEffect) && Main.rand.NextDouble() < 0.2)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.MythrilSetAmmoCostReduction) && Main.rand.NextDouble() < 0.2)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.AdamantiteSetAmmoCostReduction) && Main.rand.NextDouble() < 0.25)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.HallowedAmmoReduction) && Main.rand.NextDouble() < 0.25)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.ChlorophyteAmmoCostReduction) && Main.rand.NextDouble() < 0.2)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.ShroomiteAmmoReduction) && Main.rand.NextDouble() < 0.2)
                            DontUseAmmo = true;
                        else if (HasFlag(GuardianFlags.VortexAmmoUseReduction) && Main.rand.NextDouble() < 0.25)
                            DontUseAmmo = true;
                        if (!DontUseAmmo && this.Inventory[j].consumable)
                        {
                            bool DepleteThisItemAmmo = true, LastItem = this.Inventory[j].stack == 1;
                            for (int s = j + 1; s < 50; s++)
                            {
                                if (Inventory[j].type == Inventory[s].type)
                                {
                                    Inventory[s].stack--;
                                    if (Inventory[s].stack <= 0)
                                        Inventory[s].SetDefaults(0, true);
                                    DepleteThisItemAmmo = false;
                                    LastItem = false;
                                    break;
                                }
                            }
                            if (MainMod.SaveAtLeastOneAmmo && LastItem)
                            {
                                DepleteThisItemAmmo = false;
                                Damage = (int)(Damage * 0.7f);
                            }
                            if (DepleteThisItemAmmo)
                                Inventory[j].stack--;
                            if (Inventory[j].stack <= 0)
                                Inventory[j].SetDefaults(0, true);
                        }
                    }
                    ShotSpeed *= ShotSpeedMult;
                    return;
                }
            }
            if (!HasAmmo)
            {
                if (i.useAmmo == Terraria.ID.AmmoID.Arrow)
                {
                    ShotSpeed += 3f;
                }
                else if (i.useAmmo == Terraria.ID.AmmoID.Bullet)
                {
                    ShotSpeed += 4f;
                }
                if (i.shoot != 10)
                    ProjID = i.shoot;
                else
                {
                    if (i.useAmmo == Terraria.ID.AmmoID.Arrow)
                        ProjID = Terraria.ID.ProjectileID.WoodenArrowFriendly;
                    if (i.useAmmo == Terraria.ID.AmmoID.Bullet)
                        ProjID = Terraria.ID.ProjectileID.Bullet;
                    if (i.useAmmo == Terraria.ID.AmmoID.FallenStar)
                        ProjID = 12;
                    if (i.useAmmo == Terraria.ID.AmmoID.Sand)
                        ProjID = Terraria.ID.ProjectileID.SandBallGun;
                    if (i.useAmmo == Terraria.ID.AmmoID.Dart)
                        ProjID = 267;
                    if (i.useAmmo == Terraria.ID.AmmoID.Gel)
                        ProjID = Terraria.ID.ProjectileID.Flames;
                    if (i.useAmmo == Terraria.ID.AmmoID.Rocket)
                        ProjID = Terraria.ID.ProjectileID.RocketI;
                    if (i.useAmmo == Terraria.ID.AmmoID.StyngerBolt)
                        ProjID = Terraria.ID.ProjectileID.Stynger;
                    if (i.useAmmo == Terraria.ID.AmmoID.Snowball)
                        ProjID = Terraria.ID.ProjectileID.SnowBallFriendly;
                    if (i.useAmmo == Terraria.ID.AmmoID.CandyCorn)
                        ProjID = Terraria.ID.ProjectileID.CandyCorn;
                    if (i.useAmmo == Terraria.ID.AmmoID.JackOLantern)
                        ProjID = Terraria.ID.ProjectileID.JackOLantern;
                    if (i.useAmmo == Terraria.ID.AmmoID.Stake)
                        ProjID = Terraria.ID.ProjectileID.Stake;
                    if (i.useAmmo == Terraria.ID.AmmoID.NailFriendly)
                        ProjID = Terraria.ID.ProjectileID.NailFriendly;
                }
            }
        }

        public void ShareBuffsWithPlayer()
        {
            if (OwnerPos == -1) return;
            Player player = Main.player[OwnerPos];
            if (HasFlag(GuardianFlags.HunterPotion))
                player.detectCreature = true;
            if (HasFlag(GuardianFlags.SpelunkerPotion))
                player.findTreasure = true;

            if (PlayerControl)
            {
                if (HasFlag(GuardianFlags.Blind)) player.blind = true;
                if (HasFlag(GuardianFlags.Blackout)) player.blackout = true;
                if (HasFlag(GuardianFlags.HeadCovered)) player.headcovered = true;
                if (HasFlag(GuardianFlags.NightVisionPotion)) player.nightVision = true;
                if (HasFlag(GuardianFlags.Scope)) player.scope = true;
            }
            if (PlayerMounted)
            {
                if (HasFlag(GuardianFlags.Scope)) player.scope = true;
            }
        }

        public void LookForThreatsV2()
        {
            if (HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown))
                return;
            Player Owner = null;
            if (OwnerPos > -1)
                Owner = Main.player[OwnerPos];
            if (Owner != null && !Owner.dead && !WofFacing && !GuardingPosition.HasValue && Math.Abs(Owner.Center.X - Position.X) >= 512)
            {
                return; //Ignore attempt to look for targets
            }
            float ThreatDetectionDistance = 460f;
            float ViewDistance = 380f;// + AssistSlot * 32f;
            List<Vector4> PartyMembersPosition = new List<Vector4>();
            Vector2 AwarenessCenter = CenterPosition, MyCenter = AwarenessCenter;
            PartyMembersPosition.Add(new Vector4(TopLeftPosition, Width, Height));
            {
                float ViewDistanceMod = 1f, ThreatDetectionMod = 1f;
                if (HasFlag(GuardianFlags.LightPotion))
                    ViewDistanceMod *= 1.2f;
                if (HasFlag(GuardianFlags.NightVisionPotion))
                {
                    ViewDistanceMod *= 1.25f;
                    ThreatDetectionMod *= 1.25f;
                }
                if (HasFlag(GuardianFlags.Blind))
                {
                    ViewDistanceMod *= 0.66f;
                    ThreatDetectionMod *= 0.66f;
                }
                if (HasFlag(GuardianFlags.Blackout))
                {
                    ViewDistanceMod *= 0.25f;
                    ThreatDetectionMod *= 0.25f;
                }
                if (HasFlag(GuardianFlags.HunterPotion))
                {
                    ThreatDetectionMod *= 1.5f;
                }
                if (HurtPanic)
                {
                    ThreatDetectionDistance *= 2f;
                }
                ViewDistance *= ViewDistanceMod;
                ThreatDetectionDistance *= ThreatDetectionMod;
            }
            if (Passive)
            {
                ThreatDetectionDistance = 96f;
            }
            else
            {
                if (Owner != null) //Be reserved for party members only.
                {
                    if (!IsPlayerHostile(Owner))
                    {
                        Vector2 Center = Owner.Center;
                        if (Math.Abs(Center.X - MyCenter.X) < (Owner.width + Width) * 0.5f + ViewDistance &&
                            Math.Abs(Center.Y - MyCenter.Y) < (Owner.height + Height) * 0.5f + ViewDistance)
                        {
                            PartyMembersPosition.Add(new Vector4(Owner.position, Owner.width, Owner.height));
                            if (ProtectMode)
                                AwarenessCenter = Owner.Center;
                        }
                    }
                    foreach (TerraGuardian tg in Owner.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                    {
                        if (tg.Active && tg.WhoAmID != WhoAmID && !IsGuardianHostile(tg))
                        {
                            Vector2 Center = tg.CenterPosition;
                            if (Math.Abs(Center.X - MyCenter.X) < (tg.Width + Width) * 0.5f + ViewDistance &&
                                Math.Abs(Center.Y - MyCenter.Y) < (tg.Height + Height) * 0.5f + ViewDistance)
                            {
                                PartyMembersPosition.Add(new Vector4(tg.TopLeftPosition, tg.Width, tg.Height));
                            }
                        }
                    }
                }
                /*else if (false) //Try helping the ones in their view range. - Disabled because seems to be causing lags
                {
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].GetModPlayer<PlayerMod>().KnockedOutCold && !IsPlayerHostile(Main.player[i]))
                        {
                            Vector2 Center = Main.player[i].Center;
                            if (Math.Abs(Center.X - MyCenter.X) < (Main.player[i].width + Width) * 0.5f + ViewDistance &&
                                Math.Abs(Center.Y - MyCenter.Y) < (Main.player[i].height + Height) * 0.5f + ViewDistance)
                            {
                                PartyMembersPosition.Add(new Vector4(Main.player[i].position, Main.player[i].width, Main.player[i].height));
                            }
                        }
                        if (i < 200 && Main.npc[i].active && Main.npc[i].townNPC)
                        {
                            Vector2 Center = Main.npc[i].Center;
                            if (Math.Abs(Center.X - MyCenter.X) < (Main.npc[i].width + Width) * 0.5f + ViewDistance &&
                                Math.Abs(Center.Y - MyCenter.Y) < (Main.npc[i].height + Height) * 0.5f + ViewDistance)
                            {
                                PartyMembersPosition.Add(new Vector4(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height));
                            }
                        }
                    }
                    foreach (int i in MainMod.ActiveGuardians.Keys)
                    {
                        TerraGuardian guardian = MainMod.ActiveGuardians[i];
                        if (i != WhoAmID && !guardian.Downed && !guardian.KnockedOutCold && !IsGuardianHostile(guardian))
                        {
                            Vector2 Center = guardian.CenterPosition;
                            if (Math.Abs(Center.X - MyCenter.X) < (guardian.Width + Width) * 0.5f + ViewDistance &&
                                Math.Abs(Center.Y - MyCenter.Y) < (guardian.Height + Height) * 0.5f + ViewDistance)
                            {
                                PartyMembersPosition.Add(new Vector4(guardian.TopLeftPosition, guardian.Width, guardian.Height));
                            }
                        }
                    }
                }*/
            }
            float NearestTargetDistance = ThreatDetectionDistance;
            if (IsAttackingSomething)
            {
                switch (TargetType)
                {
                    case TargetTypes.Npc:
                        if (Main.npc[TargetID].active && IsNpcHostile(Main.npc[TargetID]))
                        {
                            NearestTargetDistance = Main.npc[TargetID].Distance(CenterPosition);
                        }
                        else
                        {
                            TargetID = -1;
                            AttackingTarget = false;
                        }
                        break;
                    case TargetTypes.Player:
                        if (Main.player[TargetID].active && IsPlayerHostile(Main.player[TargetID]) && !Main.player[TargetID].dead && !Main.player[TargetID].GetModPlayer<PlayerMod>().KnockedOutCold)
                        {
                            NearestTargetDistance = Main.player[TargetID].Distance(CenterPosition);
                        }
                        else
                        {
                            TargetID = -1;
                            AttackingTarget = false;
                        }
                        break;
                    case TargetTypes.Guardian:
                        if (MainMod.ActiveGuardians.ContainsKey(TargetID) && IsGuardianHostile(MainMod.ActiveGuardians[TargetID]) && !MainMod.ActiveGuardians[TargetID].Downed && !MainMod.ActiveGuardians[TargetID].KnockedOutCold)
                        {
                            NearestTargetDistance = MainMod.ActiveGuardians[TargetID].Distance(CenterPosition);
                        }
                        else
                        {
                            TargetID = -1;
                            AttackingTarget = false;
                        }
                        break;
                }
            }
            Vector2 MyTopLeftPosition = TopLeftPosition;
            for (byte i = 0; i < 255; i++)
            {
                if (i < 200 && Main.npc[i].active && IsNpcHostile(Main.npc[i]))
                {
                    Vector4 NearestMember = Vector4.Zero;
                    float NearestDistance = float.MaxValue;
                    foreach (Vector4 MemberPosition in PartyMembersPosition)
                    {
                        float Distance = ((MemberPosition.XY() + MemberPosition.ZW() * 0.5f) - Main.npc[i].Center).Length();
                        if (Distance < NearestDistance)
                        {
                            NearestDistance = Distance;
                            NearestMember = MemberPosition;
                        }
                    }
                    if (NearestMember.Length() > 0)
                    {
                        Vector2 NpcCenter = Main.npc[i].Center;
                        Vector2 MemberCenter = NearestMember.XY() + NearestMember.ZW() * 0.5f;
                        {
                            float Distance = Main.npc[i].Distance(MemberCenter);
                            if (Terraria.ID.NPCID.Sets.TechnicallyABoss[Main.npc[i].type] || Main.npc[i].boss)
                                Distance *= 0.5f;
                            if (Distance < NearestTargetDistance)
                            {
                                if (Collision.CanHitLine(MyTopLeftPosition, Width, Height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                                {
                                    NearestTargetDistance = Distance;
                                    TargetID = i;
                                    TargetType = TargetTypes.Npc;
                                    SetCooldownValue(GuardianCooldownManager.CooldownType.MemoryOfTarget, TimeUntilCompanionForgetsTarget);
                                }
                            }
                        }
                    }
                }
                if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].GetModPlayer<PlayerMod>().KnockedOutCold && IsPlayerHostile(Main.player[i]))
                {
                    Vector4 NearestMember = Vector4.Zero;
                    float NearestDistance = float.MaxValue;
                    foreach (Vector4 mp in PartyMembersPosition)
                    {
                        float Distance = ((mp.XY() + mp.ZW() * 0.5f) - Main.player[i].Center).Length();
                        if (Distance < NearestDistance)
                        {
                            NearestDistance = Distance;
                            NearestMember = mp;
                        }
                    }
                    if (NearestMember.Length() == 0)
                        continue;
                    Vector4 MemberPosition = NearestMember;
                    Vector2 MemberCenter = MemberPosition.XY() + MemberPosition.ZW() * 0.5f;
                    {
                        float Distance = Main.player[i].Distance(MemberCenter);
                        if (Distance < NearestTargetDistance)
                        {
                            if (Collision.CanHitLine(MyTopLeftPosition, Width, Height, Main.player[i].position, Main.player[i].width, Main.player[i].height))
                            {
                                NearestTargetDistance = Distance;
                                TargetID = i;
                                TargetType = TargetTypes.Player;
                                SetCooldownValue(GuardianCooldownManager.CooldownType.MemoryOfTarget, TimeUntilCompanionForgetsTarget);
                            }
                        }
                    }
                }
            }
            bool LastHadTarget = IsAttackingSomething;
            foreach (int key in MainMod.ActiveGuardians.Keys)
            {
                if (key != WhoAmID)
                {
                    TerraGuardian guardian = MainMod.ActiveGuardians[key];
                    if (IsGuardianHostile(guardian) && !guardian.KnockedOutCold && !guardian.Downed)
                    {
                        Vector4 NearestMember = Vector4.Zero;
                        float NearestDistance = float.MaxValue;
                        foreach (Vector4 mp in PartyMembersPosition)
                        {
                            float Distance = ((mp.XY() + mp.ZW() * 0.5f) - guardian.CenterPosition).Length();
                            if (Distance < NearestDistance)
                            {
                                NearestDistance = Distance;
                                NearestMember = mp;
                            }
                        }
                        if (NearestMember.Length() == 0)
                            continue;
                        Vector4 MemberPosition = NearestMember;
                        Vector2 MemberCenter = MemberPosition.XY() + MemberPosition.ZW() * 0.5f;
                        {
                            float Distance = guardian.Distance(MemberCenter);
                            if (Distance < NearestTargetDistance)
                            {
                                if (Collision.CanHitLine(MyTopLeftPosition, Width, Height, guardian.TopLeftPosition, guardian.Width, guardian.Height))
                                {
                                    NearestTargetDistance = Distance;
                                    TargetID = key;
                                    TargetType = TargetTypes.Guardian;
                                    SetCooldownValue(GuardianCooldownManager.CooldownType.MemoryOfTarget, TimeUntilCompanionForgetsTarget);
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool IsGuardianHostile(TerraGuardian g, bool NoOtherCheck = false)
        {
            if (g.WhoAmID != WhoAmID)
            {
                if (DoAction != null && DoAction.InUse)
                {
                    bool? Hostile = DoAction.ModifyGuardianHostile(this, g);
                    if (Hostile.HasValue)
                        return Hostile.Value;
                }
                if (g.OwnerPos > -1 && IsPlayerHostile(Main.player[g.OwnerPos]))
                {
                    return true;
                }
                if (!NoOtherCheck)
                {
                    return g.IsGuardianHostile(this, true);
                }
            }
            return false;
        }

        public bool IsPlayerHostile(Player p)
        {
            if (DoAction != null && DoAction.InUse)
            {
                bool? Hostile = DoAction.ModifyPlayerHostile(this, p);
                if (Hostile.HasValue)
                    return Hostile.Value;
            }
            if (p.whoAmI != OwnerPos)
            {
                if (p.hostile)
                {
                    if (OwnerPos == -1 || OwnerPos != p.whoAmI && (p.team == 0 || p.team != Main.player[OwnerPos].team))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsNpcHostile(NPC n)
        {
            if (DoAction != null && DoAction.InUse)
            {
                bool? Hostile = DoAction.ModifyNPCHostile(this, n);
                if (Hostile.HasValue)
                    return Hostile.Value;
            }
            if ((n.catchItem == 0 || n.damage > 0) && n.CanBeChasedBy(null))
            {
                return true;
            }
            return false;
        }

        public void GuardingPositionAI()
        {
            bool PlayerMountedButHasControl = PlayerMounted && GuardianHasControlWhenMounted;
            if (false && PlayerMountedButHasControl && OwnerPos > -1 && OwnerPos == Main.myPlayer && Main.player[Main.myPlayer].mouseInterface == false && Main.mouseRight)
            {
                int MousePositionX = (int)((Main.mouseX + Main.screenWidth) * DivisionBy16),
                    MousePositionY = (int)((Main.mouseY + Main.screenHeight) * DivisionBy16);
                if (!GuardingPosition.HasValue || (GuardingPosition.Value.X != MousePositionX))
                {
                    GuardingPosition = new Point(MousePositionX, MousePositionY);
                }
            }
            if (!GuardingPosition.HasValue) return;
            Vector2 PositionDifference = new Vector2(GuardingPosition.Value.X * 16, GuardingPosition.Value.Y * 16 - Height * 0.5f);
            PositionDifference -= CenterPosition;
            if (Math.Abs(PositionDifference.X) >= 200f && IsGuardingPlace)
            {
                if (PositionDifference.X < 0)
                {
                    if (MoveRight)
                    {
                        MoveRight = false;
                    }
                    if (Math.Abs(PositionDifference.X) >= 240)
                        MoveLeft = true;
                }
                if (PositionDifference.X > 0)
                {
                    if (MoveLeft)
                    {
                        MoveLeft = false;
                    }
                    if (Math.Abs(PositionDifference.X) >= 240)
                        MoveRight = true;
                }
            }
            /*if(PlayerMountedButHasControl && Math.Abs(PositionDifference.X) <= 16f)
            {
                GuardingPosition = null;
                return;
            }*/
            if (PositionDifference.Y > 180f || Math.Abs(PositionDifference.X) >= 300f)
            {
                IncreaseStuckTimer();
            }
            if (!IsAttackingSomething && (!PlayerMountedButHasControl || !IsPlayerIdle))
            {
                if (Math.Abs(PositionDifference.X) > Width)
                {
                    MoveRight = MoveLeft = false;
                    if (PositionDifference.X < 0)
                        MoveLeft = true;
                    else
                        MoveRight = true;
                }
                //MoveCursorToPosition(CenterPosition + new Vector2(SpriteWidth * 0.5f * Direction, - SpriteHeight * 0.25f));
            }
        }

        public void FollowPlayerAI()
        {
            if (Main.netMode == 0 && TalkPlayerID > -1)
                return;
            if (GuardingPosition.HasValue || Paths.Count > 0 || PlayerMounted || (OwnerPos == -1 && !IsCommander) || Main.player[OwnerPos].dead) return; //If there is no player, follow nobody
            Player Owner = IsCommander ? Main.player[GetCommanderLeaderID] : Main.player[OwnerPos];
            TerraGuardian LeaderGuardian = PlayerMod.GetPlayerMainGuardian(Owner);
            {
                PlayerMod pm = Owner.GetModPlayer<PlayerMod>();
                if (ChargeAhead)
                    pm.FollowFrontOrder++;
                else
                    pm.FollowBackOrder++;
            }
            const int Distance = 20; //48
            Vector2 PositionDifference = Vector2.Zero;
            float LeaderBottom = 0, LeaderCenterX = 0, LeaderSpeedX = 0, LeaderSpeedY = 0;
            int LeaderHeight = 1;
            bool LeaderWet = false;
            /*if(Owner.whoAmI == Main.myPlayer && !DoAction.InUse && GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 7)
            {
                if(Owner.controlUseItem && Owner.inventory[Owner.selectedItem].type > 0 && Owner.inventory[Owner.selectedItem].pick > 0)
                {
                    int MX = (int)((Main.screenPosition.X + Main.mouseX) * (1f / 16)), MY = (int)((Main.screenPosition.Y + Main.mouseY) * (1f / 16));
                    Tile t = Main.tile[MX, MY];
                    if(t != null && t.active())// && (Terraria.ID.TileID.Sets.Ore[t.type] || Terraria.ID.TileID.Sets.TouchDamageOther[t.type] > 0))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (Inventory[i].type > 0 && Inventory[i].pick > 0)
                            {
                                StartNewGuardianAction(new Actions.MineAction(MX, MY));
                                break;
                            }
                        }
                    }
                }
            }*/
            if(MainMod.Gameplay2PMode && LeaderGuardian != null && AssistSlot % 2 == 0)
            {
                PositionDifference = LeaderGuardian.CenterPosition;
                LeaderCenterX = PositionDifference.X;
                LeaderSpeedX = LeaderGuardian.Velocity.X;
                LeaderSpeedY = LeaderGuardian.Velocity.Y;
                LeaderBottom = LeaderGuardian.Position.Y;
                LeaderHeight = LeaderGuardian.Height;
                LeaderWet = LeaderGuardian.Wet;
            }
            else if (Owner.GetModPlayer<PlayerMod>().MountedOnGuardian)
            {
                TerraGuardian guardian = Owner.GetModPlayer<PlayerMod>().MountGuardian;
                PositionDifference = guardian.CenterPosition;
                LeaderCenterX = PositionDifference.X;
                LeaderSpeedX = guardian.Velocity.X;
                LeaderSpeedY = guardian.Velocity.Y;
                LeaderBottom = guardian.Position.Y;
                LeaderHeight = guardian.Height;
                LeaderWet = guardian.Wet;
            }
            else if (Owner.GetModPlayer<PlayerMod>().ControllingGuardian || IsCommander || LeaderGuardian.GrabbingPlayer)
            {
                TerraGuardian guardian = LeaderGuardian;
                PositionDifference = guardian.CenterPosition;
                LeaderCenterX = PositionDifference.X;
                LeaderSpeedX = guardian.Velocity.X;
                LeaderSpeedY = guardian.Velocity.Y;
                LeaderBottom = guardian.Position.Y;
                LeaderHeight = guardian.Height;
                LeaderWet = guardian.Wet;
            }
            else
            {
                PositionDifference = Owner.Center;
                LeaderCenterX = PositionDifference.X;
                LeaderSpeedX = Owner.velocity.X;
                LeaderSpeedY = Owner.velocity.Y;
                LeaderBottom = Owner.position.Y + Owner.height;
                LeaderHeight = Owner.height;
                LeaderWet = Owner.wet;
            }
            float XDiscount = 0;
            bool Confused = HasFlag(GuardianFlags.Confusion) && OwnerPos > -1;
            /*if (ChargeAhead)
            {
                PositionDifference.X += (Distance + Math.Abs(Owner.velocity.X)) * Owner.direction * (Confused ? -1 : 1);
            }*/
            float DistanceMult = IsAttackingSomething ? 1.5f : 1f, 
                  DistanceBonus = IsCommander ? 30 * PlayerMod.CompanyFollowOrder : Distance * (ChargeAhead ? Owner.GetModPlayer<PlayerMod>().FollowFrontOrder++ : Owner.GetModPlayer<PlayerMod>().FollowBackOrder++);
            float BottomDistanceY = LeaderBottom - Position.Y;
            if (ChargeAhead)
            {
                PositionDifference.X += (Distance + Math.Abs(Owner.velocity.X) + DistanceBonus) * Owner.direction * (Confused ? -1 : 1);
                XDiscount = DistanceBonus + 20;
            }
            if (IsAttackingSomething)
                DistanceBonus += 300;
            PositionDifference -= Position;
            DistanceMult *= Scale;
            if (ProtectMode && IsAttackingSomething)
                DistanceMult *= 0.2f;
            bool PlayerIsHooked = Owner.grapCount > 0;
            if (!Downed && !PlayerIsHooked && 
                (((!IsAttackingSomething && Math.Abs(PositionDifference.Y) >= 320f * DistanceMult) || 
                Math.Abs(PositionDifference.Y) >= 640f * DistanceMult) ||
                Math.Abs(PositionDifference.X) >= 640f * DistanceMult))
            {
                IncreaseStuckTimer();
            }
            if (TalkPlayerID > -1)
            {
                if (Math.Abs(PositionDifference.X) > 400 || Math.Abs(PositionDifference.Y) > 300)
                {
                    TalkPlayerID = -1;
                }
                else
                {
                    return;
                }
            }
            //if (true || !IsAttackingSomething)
            {
                if (UsingFurniture && Math.Abs(PositionDifference.X) >= 160f)
                {
                    LeaveFurniture();
                }
                bool IsOnSameGroundAsPlayer = false;
                {
                    int MyPositionX = (int)(Position.X * DivisionBy16), MyPositionY = (int)(Position.Y * DivisionBy16),
                        PlayerPositionX = (int)(LeaderCenterX * DivisionBy16), PlayerPositionY = (int)(LeaderBottom * DivisionBy16);
                    for (int attempt = 0; attempt < 20; attempt++)
                    {
                        Tile tile = MainMod.GetTile(MyPositionX, MyPositionY);
                        if (tile.active() && Main.tileSolid[tile.type])
                        {
                            MyPositionY--;
                        }
                        else
                        {
                            Tile undertile = MainMod.GetTile(MyPositionX, MyPositionY + 1);
                            if (!undertile.active() || !Main.tileSolid[undertile.type])
                            {
                                MyPositionY++;
                            }
                            else
                            {
                                if (MyPositionX < PlayerPositionX)
                                    MyPositionX++;
                                else
                                    MyPositionX--;
                            }
                        }
                        if (MyPositionX == PlayerPositionX && MyPositionY >= PlayerPositionY - 1 && MyPositionY <= PlayerPositionY + 1)
                        {
                            IsOnSameGroundAsPlayer = true;
                            break;
                        }
                    }
                }
                if (!IsOnSameGroundAsPlayer && !IsAttackingSomething && LeaderSpeedY == 0)
                {
                    //if (Math.Abs(PositionDifference.X) >= 8)
                    int LeaderPosX = (int)(LeaderCenterX * DivisionBy16);
                    if((int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16) < LeaderPosX - 1 || (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16) > LeaderPosX)
                    {
                        if (PositionDifference.X < 0)
                            MoveLeft = true;
                        else
                            MoveRight = true;
                    }
                    //else
                    {
                        if (BottomDistanceY > 0)
                        {
                            if (IsStandingOnAPlatform(out bool stair) && !stair)
                            {
                                MoveDown = true;
                                Jump = true;
                            }
                        }
                        else
                        {
                            if (BottomDistanceY < 0 && BottomDistanceY > -2 * 16 && ((!Jump && Velocity.Y == 0) || JumpHeight > 0))
                            {
                                int XStart = (int)((Position.X - CollisionWidth * 0.5f) * DivisionBy16), 
                                    XEnd = (int)((Position.X + CollisionWidth * 0.5f) * DivisionBy16);
                                bool BlockedAbove = false, PlatformAbove = false;
                                int YStart = (int)(Position.Y * DivisionBy16);
                                float PlatformLevel = -1f;
                                for (int y = -2; y >= -Math.Abs(BottomDistanceY * DivisionBy16); y--)
                                {
                                    for (int x = XStart; x < XEnd; x++)
                                    {
                                        Tile tile = MainMod.GetTile(x, YStart + y);
                                        if (tile.active() && Main.tileSolid[tile.type])
                                        {
                                            if (!Terraria.ID.TileID.Sets.Platforms[tile.type])
                                            {
                                                BlockedAbove = true;
                                                PlatformAbove = false;
                                                break;
                                            }
                                            else
                                            {
                                                byte SlopeType = tile.slope();
                                                if (SlopeType != 1 && SlopeType != 2)
                                                    PlatformAbove = true;
                                            }
                                        }
                                        if (PlatformAbove || BlockedAbove)
                                        {
                                            PlatformLevel = (YStart + y) * 16;
                                            break;
                                        }
                                    }
                                }
                                if (!BlockedAbove && PlatformAbove)
                                {
                                    JumpUntilHeight = PlatformLevel;
                                    Jump = true;
                                }
                                else if (BlockedAbove && Math.Abs(PositionDifference.X) >= 8)
                                {
                                    if (PositionDifference.X < 0)
                                        MoveLeft = true;
                                    else
                                        MoveRight = true;
                                }
                            }
                        }
                    }
                }
                else if (Math.Abs(PositionDifference.X - LeaderSpeedX) > Distance + DistanceBonus - XDiscount)
                {
                    if (Math.Abs(PositionDifference.X) > 8)
                    {
                        if (PositionDifference.X < 0)
                            MoveLeft = true;
                        else
                            MoveRight = true;
                    }
                    else
                    {
                        if (Velocity.X == 0 && Velocity.Y == 0 && !UsingFurniture && !IsAttackingSomething)
                        {
                            if (Position.X >= LeaderCenterX)
                                Direction = 1;
                            else
                                Direction = -1;
                        }
                    }
                }
                /*else if (ChargeAhead && LeaderSpeedX != 0) //To try keeping up with the player when on charge AI.
                {
                    if (LeaderSpeedX < 0)
                    {
                        if (Velocity.X >= LeaderSpeedX)
                        {
                            MoveLeft = true;
                        }
                    }
                    else if (LeaderSpeedX > 0)
                    {
                        if (Velocity.X <= LeaderSpeedX)
                        {
                            MoveRight = true;
                        }
                    }
                    //StuckTimer = 0;
                }*/
                else if (LeaderSpeedX != 0)
                {
                    if (Math.Abs(PositionDifference.X - LeaderSpeedX) > Distance * 0.75f + DistanceBonus - XDiscount) //36
                    {
                        if (Position.X < LeaderCenterX)
                        {
                            if (Velocity.X <= LeaderSpeedX)
                            {
                                MoveRight = true;
                            }
                        }
                        else
                        {
                            if (Velocity.X >= LeaderSpeedX)
                            {
                                MoveLeft = true;
                            }
                        }
                    }
                }
                else
                {
                    if(Velocity.X == 0 && Velocity.Y == 0 && !UsingFurniture && !IsAttackingSomething)
                    {
                        if (Position.X < LeaderCenterX)
                            Direction = 1;
                        else
                            Direction = -1;
                    }
                }
                bool TryFlying = WingType > 0,
                    TrySwimming = Wet && (HasFlag(GuardianFlags.SwimmingAbility) || HasFlag(GuardianFlags.Merfolk));
                bool PlayerAboveMe = Owner.velocity.Y != 0 && (Velocity.Y == 0 ? LeaderBottom < Position.Y - (LeaderHeight * 2 + Height) : LeaderBottom < Position.Y - LeaderHeight * 0.5f),
                    PlayerAboveMeSwim = TrySwimming && ((LeaderWet && LeaderBottom < Position.Y - 8) || (!LeaderWet && LeaderBottom < Position.Y + Height - 8));
                bool CarryingPlayer = false;
                if (DoAction.InUse && DoAction.IsGuardianSpecificAction && DoAction.ID == (int)GuardianActions.ActionIDs.CarryDownedAlly)
                {
                    Actions.CarryDownedAlly CarryAction = (Actions.CarryDownedAlly)DoAction;
                    if (CarryAction.CarriedPlayer != null && CarryAction.CarriedPlayer.whoAmI == OwnerPos)
                    {
                        CarryingPlayer = true;
                    }
                    else if(CarryAction.CarriedGuardian != null && CarryAction.CarriedGuardian.OwnerPos == OwnerPos && CarryAction.CarriedGuardian.PlayerControl)
                    {
                        CarryingPlayer = true;
                    }
                    if (CarryingPlayer)
                    {
                        PlayerAboveMe = false;
                    }
                }
                if (PlayerAboveMeSwim)
                {
                    Jump = true;
                }
                else if (PlayerAboveMe)
                {
                    if (TryFlying)
                    {
                        Jump = true;
                        WingFlightTime++;
                    }
                }
                else if (Breath < BreathMax && Wet && (!LeaderWet || CarryingPlayer)) //!TryFlying && !TrySwimming && 
                {
                    //Try leaving water when drowning by going to where the leader is, If the leader isn't at water.
                    if (IsBlockedAhead())
                    {
                        LookingLeft = !LookingLeft;
                    }
                    if (Position.X < LeaderCenterX)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
                //MoveCursorToPosition(CenterPosition + new Vector2(SpriteWidth * 0.5f * Direction, -SpriteHeight * 0.25f));
                //MoveCursorToPosition(Owner.position, Owner.width, Owner.height);
                if (Confused)
                {
                    bool moveleft = MoveLeft;
                    MoveLeft = MoveRight;
                    MoveRight = moveleft;
                }
                /*if (IsBlockedAhead() && ((MoveLeft && LookingLeft) || (MoveRight && !LookingLeft)))
                {
                    MoveLeft = MoveRight = Jump = false;
                    if(!ChargeAhead || 
                        (Direction > 0 && Position.X < LeaderCenterX) || 
                        (Direction < 0 && Position.X > LeaderCenterX))
                        IncreaseStuckTimer();
                }*/
                if(!IsAttackingSomething && Velocity.X == 0 && Velocity.Y == 0 && (LeaderBottom - 32 > Position.Y || (LeaderBottom + 48 < Position.Y && LeaderSpeedY == 0)) && !IsOnSameGroundAsPlayer && Paths.Count == 0 && GetCooldownValue(GuardianCooldownManager.CooldownType.DelayedActionCooldown) == 3)
                {
                    if(!CreatePathingTo(new Vector2(LeaderCenterX, LeaderBottom)))
                    {
                        if(LeaderSpeedY == 0)
                            BePulledByPlayer();
                    }
                }
            }
        }

        public void CheckForNpcContact()
        {
            if (IsBeingPulledByPlayer && !SuspendedByChains) return;
            if ((Main.netMode == 1 && OwnerPos != Main.myPlayer) || (Main.netMode == 2 && OwnerPos > -1))
                return;
            int DamageStack = -1, AttackDirection = 0; // *evil face*
            float CounteredDamage = 0;
            if (HasFlag(GuardianFlags.ThornsEffectPotion))
                CounteredDamage += 0.3334f;
            if (HasFlag(GuardianFlags.DryadWard))
                CounteredDamage += 0.2f;
            if (HasFlag(GuardianFlags.TurtleSetEffect))
                CounteredDamage += 1;
            bool StackDamages = !MainMod.UseNewMonsterModifiersSystem && Base.Size == GuardianBase.GuardianSize.Large;
            List<int> NpcsPos = new List<int>();
            int LastAttacker = -1;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && !Main.npc[n].friendly && Main.npc[n].damage > 0)
                {
                    if (Main.npc[n].getRect().Intersects(HitBox))
                    {
                        if (Base.GuardianWhenAttackedNPC(this, Main.npc[n].damage, Main.npc[n]) && (Main.npc[n].damage > DamageStack || StackDamages) && CanHit(Main.npc[n].position, Main.npc[n].width, Main.npc[n].height, false, true))
                        {
                            if (StackDamages)
                            {
                                DamageStack += Main.npc[n].damage;
                                AttackDirection += Main.npc[n].direction;
                            }
                            else
                            {
                                DamageStack = Main.npc[n].damage;
                                AttackDirection = Main.npc[n].direction;
                            }
                            if(GuardianBountyQuest.TargetMonsterSpawnPosition == n)
                            {
                                GuardianBountyQuest.OnBountyMonsterHitTerraGuardian(this);
                            }
                            LastAttacker = n;
                        }
                        NpcsPos.Add(n);
                        if (Main.npc[n].type == Terraria.ID.NPCID.NebulaHeadcrab)
                        {
                            AddBuff(163, 59, true);
                        }
                    }
                    if (Main.netMode < 2)
                    {
                        if ((Main.npc[n].type == Terraria.ID.NPCID.DD2WitherBeastT2 || Main.npc[n].type == Terraria.ID.NPCID.DD2WitherBeastT3) && Main.npc[n].ai[0] == 1 && 
                            (CenterPosition - Main.npc[n].Center).Length() < 400)
                        {
                            AddBuff(195, 3, true);
                        }
                        if ((Main.npc[n].type == Terraria.ID.NPCID.DD2WitherBeastT2 || Main.npc[n].type == Terraria.ID.NPCID.DD2WitherBeastT3) && (CenterPosition - Main.npc[n].Center).Length() < 400)
                        {
                            AddBuff(195, 3, true);
                        }
                        if (!HasBuff(156) && Main.npc[n].type == Terraria.ID.NPCID.Medusa)
                        {
                            bool NearDeath = Main.npc[n].life < Main.npc[n].lifeMax * 0.333f;
                            int PetrifyTime = 180;
                            if (NearDeath)
                                PetrifyTime = 240;
                            if (Main.npc[n].ai[2] >= -PetrifyTime && Main.npc[n].ai[2] < 0)
                            {
                                Vector2 PosDif = CenterPosition - Main.npc[n].Center;
                                float Distance = PosDif.Length();
                                if (Distance >= 700)
                                    continue;
                                bool ForcePetrify = Distance < 30;
                                if (!ForcePetrify)
                                {
                                    float x = 0.7853982f.ToRotationVector2().X;
                                    Vector2 XChecker = Vector2.Normalize(PosDif);
                                    if (XChecker.X > x || XChecker.X < -x)
                                    {
                                        ForcePetrify = true;
                                    }
                                }
                                if (((Position.X > Main.npc[n].Center.X && Main.npc[n].direction > 0 && LookingLeft) ||
                                    (Position.X <= Main.npc[n].Center.X && Main.npc[n].direction < 0 && !LookingLeft) ||
                                ForcePetrify) && ((Collision.CanHitLine(CenterPosition, 1, 1, Main.npc[n].Center, 1, 1) ||
                                    Collision.CanHitLine(Main.npc[n].Center - Vector2.UnitY * 16, 1, 1, CenterPosition, 1, 1) ||
                                    Collision.CanHitLine(Main.npc[n].Center + Vector2.UnitY * 8, 1, 1, CenterPosition, 1, 1))))
                                {
                                    int DebuffDuration = 60;
                                    if (NearDeath)
                                        DebuffDuration = 90;
                                    AddBuff(156, DebuffDuration + (int)Main.npc[n].ai[2] * -1);
                                }
                            }
                        }
                    }
                }
            }
            if (DamageStack > -1)
            {
                int DefPenalty = (NpcsPos.Count - 1) * 2;

                string DeathText = (StackDamages && NpcsPos.Count > 1 ? " was slain by " + Main.npc[LastAttacker].GivenOrTypeName + " and It's friends." : " was slain by " + Main.npc[LastAttacker].GivenOrTypeName + ".");
                int Value = Hurt(DamageStack, AttackDirection, false, false, DeathText);
                if (Value > 0)
                    AddSkillProgress(Value, GuardianSkills.SkillTypes.Endurance);
                IncreaseDamageStacker(Value, MHP, true);
                if (AttackDirection < 0) AttackDirection = -1;
                if (AttackDirection > 0) AttackDirection = 1;
                if (Value > 0)
                {
                    foreach (int n in NpcsPos)
                    {
                        if (CounteredDamage > 0 && !Main.npc[n].dontTakeDamage && Main.npc[n].type != 68)
                        {
                            Main.npc[n].StrikeNPC((int)(Value * CounteredDamage), 0f, Main.npc[n].direction * -1);
                        }
                        TrySimulatingGettingDebuffFromNpc(Main.npc[n]);
                    }
                }
            }
        }

        public void TrySimulatingGettingDebuffFromNpc(NPC n)
        {
            Player dummyPlayer = Main.player[255];
            dummyPlayer.StatusPlayer(n);
            for (int b = 0; b < dummyPlayer.buffType.Length; b++)
            {
                if (dummyPlayer.buffType[b] > 0)
                {
                    AddBuff(dummyPlayer.buffType[b], dummyPlayer.buffTime[b]);
                }
                dummyPlayer.buffType[b] = 0;
                dummyPlayer.buffTime[b] = 0;
            }
        }

        public void OnHitSomething(Entity Victim)
        {
            if (HasFlag(GuardianFlags.TitaniumSetEffect) && !HasCooldown(GuardianCooldownManager.CooldownType.ShadowDodgeCooldown) && Main.rand.Next(4) == 0)
            {
                if (!HasBuff(59))
                {
                    AddCooldown(GuardianCooldownManager.CooldownType.ShadowDodgeCooldown, 1800);
                }
                AddBuff(59, 1800);
            }
            if (HasFlag(GuardianFlags.OrichalcumSetEffect) && !HasCooldown(GuardianCooldownManager.CooldownType.PetalCooldown))
            {
                AddCooldown(GuardianCooldownManager.CooldownType.PetalCooldown, 20);
                Vector2 SpawnPos = Vector2.Zero;
                SpawnPos.X = Main.screenPosition.X;
                if (Direction < 0)
                    SpawnPos.X += Main.screenWidth;
                SpawnPos.Y = Main.screenPosition.Y + Main.rand.Next(Main.screenHeight);
                Vector2 Speed = Victim.Center - SpawnPos;
                Speed.X += Main.rand.Next(-50, 51) * 0.01f;
                Speed.Y += Main.rand.Next(-50, 51) * 0.01f;
                const float SpeedFactor = 24;
                float Multiplier = SpeedFactor / (float)Math.Sqrt(Speed.X * Speed.X + Speed.Y + Speed.Y);
                Speed *= Multiplier;
                int p = Projectile.NewProjectile(SpawnPos, Speed, 221, 36, 0f, RefPlayer);
                SetProjectileOwnership(p);
            }
        }

        public int Hurt(int Damage, int HitDirection, bool Critical = false, bool ForceHurt = false, string DeathMessage = "", bool PvP = false)
        {
            if (!ForceHurt && ImmuneTime > 0 || Downed || (DoAction.InUse && DoAction.Immune) || KnockedOutCold || HasFlag(GuardianFlags.CantBeHurt))
                return 0;
            if (KnockedOut)
                AddBuff(Terraria.ID.BuffID.Bleeding, 5 * 60);
            bool EvadedAttack = false;
            if (!ForceHurt)
            {
                HurtPanic = true;
                if (HasBuff(Terraria.ID.BuffID.ShadowDodge))
                {
                    CombatText.NewText(HitBox, Color.Silver, "Nullified");
                    EvadedAttack = true;
                    RemoveBuff(Terraria.ID.BuffID.ShadowDodge);
                }
                if (!KnockedOut && Main.rand.NextDouble() * 100 < DodgeRate)
                {
                    CombatText.NewText(HitBox, Color.Silver, "Dodged");
                    if (HasFlag(GuardianFlags.KnockbackImmunity))
                    {
                        Velocity.X = 4.5f * Direction;
                        Velocity.Y = -3.5f * AirHeightBonus;
                    }
                    EvadedAttack = true;
                    AddSkillProgress(Damage * 4, GuardianSkills.SkillTypes.Acrobatic);
                }
                if (Main.rand.NextDouble() * 100 < BlockRate)
                {
                    CombatText.NewText(HitBox, Color.Silver, "Blocked");
                    EvadedAttack = true;
                    AddSkillProgress(Damage * 4, GuardianSkills.SkillTypes.Endurance);
                }
            }
            int FinalDamage = 0;
            if (!EvadedAttack)
            {
                AddCooldown(GuardianCooldownManager.CooldownType.HurtPanic, 3 * 60);
                float CurrentDefenseRate = DefenseRate;
                if (HP < MHP * 0.5f && HasFlag(GuardianFlags.FrozenTurtleShell))
                    CurrentDefenseRate += 0.25f;
                if (HasFlag(GuardianFlags.BeetleDefenseEffect) && BeetleOrb > 0)
                {
                    CurrentDefenseRate += 0.15f * BeetleOrb;
                    BeetleOrb--;
                    SetCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, 180 * BeetleOrb);
                    for (int b = 0; b < 3; b++)
                    {
                        if (HasBuff(95 + b)) RemoveBuff(95 + b);
                    }
                    if (BeetleOrb > 0)
                        AddBuff(94 + BeetleOrb, 5, true);
                }
                FinalDamage = (int)(Damage * (1f - CurrentDefenseRate)) - Defense / 2;
                if (PlayerMounted && HasFlag(GuardianFlags.MountDamageReceivedReduction))
                    FinalDamage -= (int)(FinalDamage * 0.2f);
                if (KnockedOut && FinalDamage > 1)
                    FinalDamage -= (int)(FinalDamage * 0.5f);
                if (FinalDamage < 1)
                    FinalDamage = 1;
                if (HasBuff(ModContent.BuffType<giantsummon.Buffs.Sleeping>()))
                {
                    RemoveBuff(ModContent.BuffType<giantsummon.Buffs.Sleeping>());
                    FinalDamage += (int)(FinalDamage * 0.5f);
                }
                HP -= FinalDamage;
                if (HP > 0 && !KnockedOut)
                {
                    int Thereshould = (int)(MHP * 0.3f);
                    if (HP < Thereshould && HP + FinalDamage >= Thereshould)
                    {
                        string Message = GetMessage(GuardianBase.MessageIDs.CompanionHealthAtDangerousLevel);
                        if (Message != "")
                            SaySomethingCanSchedule(GuardianMouseOverAndDialogueInterface.MessageParser(Message, this), false, Main.rand.Next(20, 60));
                    }
                }
                if (MainMod.UsingGuardianNecessitiesSystem && FinalDamage >= MHP * 0.15f && OwnerPos > -1)
                {
                    AddInjury(1);
                }
                ResetHealthRegen();
                CombatText.NewText(HitBox, (Critical ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly), FinalDamage);
                float MaximumBlood = (float)FinalDamage * 200 / MHP;
                if (MaximumBlood > 250) MaximumBlood = 250;
                float BloodSize = 1f;
                if (Base.Size == GuardianBase.GuardianSize.Large)
                    BloodSize = 1.5f;
                if (Base.Size == GuardianBase.GuardianSize.Small)
                    BloodSize = 0.75f;
                for (int i = 0; i < MaximumBlood; i++)
                {
                    Dust.NewDust(TopLeftPosition, Width, Height, 5, 4 * HitDirection * Main.rand.NextFloat(), -2f, 0, default(Color), BloodSize);
                }
            }
            else
            {
                FinalDamage = 0;
            }
            if (!ForceHurt)
            {
                ImmuneTime = GetImmuneTime;
                if (HasFlag(GuardianFlags.ImprovedImmuneTime))
                    ImmuneTime *= 2;
                if (PvP)
                    ImmuneTime = (int)(ImmuneTime * 0.1f);
                if (HasFlag(GuardianFlags.PanicNecklace))
                    AddBuff(63, 300);
                if (!EvadedAttack && Damage > 0) AddSkillProgress(Damage * 2, GuardianSkills.SkillTypes.Endurance);
                if (MP < MMP && HasFlag(GuardianFlags.MagicCuffs))
                {
                    int RegenValue = Damage;
                    if (RegenValue < 10)
                        RegenValue = 10;
                    MP += RegenValue;
                    if (MP > MMP)
                        MP = MMP;
                }
            }

            if (HP > 0 && !EvadedAttack)
            {
                bool PlayingAnotherSound = false;
                if (!Base.IsCustomSpriteCharacter)
                {
                    if (HasFlag(GuardianFlags.Petrified))
                    {
                        Main.PlaySound(0, (int)Position.X, (int)CenterY, 1, 1f, 0f);
                        PlayingAnotherSound = true;
                    }
                    else if (HasFlag(GuardianFlags.FrostSetEffect))
                    {
                        Main.PlaySound(Terraria.ID.SoundID.Item27, CenterPosition);
                    }
                    else if (!HideWereForm && (HasFlag(GuardianFlags.Werewolf) || Inventory.Take(10).Any(x => x.type == Terraria.ID.ItemID.MoonCharm)))
                    {
                        Main.PlaySound(3, (int)Position.X, (int)CenterY, 6, 1f, 0f);
                        PlayingAnotherSound = true;
                    }
                    else if (HasFlag(GuardianFlags.NecroSetEffect))
                    {
                        Main.PlaySound(3, (int)Position.X, (int)CenterY, 2, 1f, 0f);
                        PlayingAnotherSound = true;
                    }
                }
                if (!PlayingAnotherSound)
                {
                    if (!Base.IsCustomSpriteCharacter && Base.HurtSound.SoundID == Terraria.ID.SoundID.PlayerHit && !Male)
                    {
                        MainMod.FemaleHitSound.PlaySound(CenterPosition);
                    }
                    else
                    {
                        Base.HurtSound.PlaySound(CenterPosition);
                    }
                }
            }

            if (HitDirection != 0 && !KnockedOut && (!HasFlag(GuardianFlags.KnockbackImmunity) || HP <= 0))
            {
                Velocity.X = 4.5f * HitDirection;
                Velocity.Y = -3.5f;
                Velocity.Y *= AirHeightBonus;
                JumpHeight = 0;
            }
            if (HasFlag(GuardianFlags.StarCounter))
            {
                for (int n = 0; n < 3; n++)
                {
                    float x = Position.X + Main.rand.Next(-400, 401),
                          y = Position.Y - Height - Main.rand.Next(500, 801);
                    Vector2 MoveDir = new Vector2(Position.X - x + Main.rand.Next(-100, 101),
                        CenterY - y);
                    MoveDir.Normalize();
                    int proj = Projectile.NewProjectile(x, y, MoveDir.X * 23, MoveDir.Y * 23, 92, 30, 5f, RefPlayer, 0, 0);
                    SetProjectileOwnership(proj);
                    Main.projectile[proj].ai[1] = Position.Y - Height;
                }
            }
            if (HasFlag(GuardianFlags.BeeCounter))
            {
                int bees = 1;
                if (Main.rand.Next(3) == 0)
                {
                    bees++;
                }
                if (Main.rand.Next(3) == 0)
                {
                    bees++;
                }
                bool StrongBee = HasFlag(GuardianFlags.BeeBuff);
                if (StrongBee && Main.rand.Next(3) == 0)
                {
                    bees++;
                }
                for (int num19 = 0; num19 < bees; num19++)
                {
                    float speedX = (float)Main.rand.Next(-35, 36) * 0.02f;
                    float speedY = (float)Main.rand.Next(-35, 36) * 0.02f;
                    int proj = Projectile.NewProjectile(Position.X, CenterY, speedX, speedY, (StrongBee ? 566 : 181), 7 + (StrongBee ? Main.rand.Next(1, 4) : Main.rand.Next(2)), 0f, RefPlayer, 0f, 0f);
                    SetProjectileOwnership(proj);
                }
            }

            if (HP <= 0)
            {
                if (Main.netMode == 0 || OwnerPos == Main.myPlayer || (Main.netMode == 2 && OwnerPos == -1))
                {
                    Knockout(DeathMessage);
                    if (OwnerPos > -1 && Downed && MainMod.UsingGuardianNecessitiesSystem)
                        AddInjury((byte)(4 * ((float)(MHP - HP) / MHP)));
                    if(OwnerPos == Main.myPlayer || (Main.netMode == 2 && OwnerPos == -1))
                    {
                        Netplay.SendGuardianHurt(WhoAmID, Damage, HitDirection, Critical, DeathMessage, -1, OwnerPos);
                    }
                }
            }
            else
            {
                TriggerHandler.FireGuardianHurtTrigger(CenterPosition, this, FinalDamage, Critical, false);
            }
            FriendlyDuelDefeat = false;
            return FinalDamage;
        }

        public bool CanHit(Vector2 TargetPosition, int TargetWidth, int TargetHeight, bool Ducking = false, bool CollisionRelated = false)
        {
            Vector2 TargetCenter = TargetPosition;
            TargetCenter.X += TargetWidth * 0.5f;
            TargetCenter.Y += TargetHeight * 0.5f;
            Vector2 MyCenterPosition = this.Position;
            int Width = this.Width;
            int Height = 42;
            if (CollisionRelated)
            {
                Width = CollisionWidth;
                Height = CollisionHeight;
            }
            else
            {
                if (Ducking)
                {
                    Height = (int)(Base.DuckingHeight * Scale);
                }
                else
                {
                    Height = (int)(Base.Height * Scale);
                }
            }
            MyCenterPosition.Y -= Height * 0.5f;
            if (mount.Active)
                Height += mount.HeightBoost;
            return Collision.CanHit(TopLeftPosition, Width, Height, TargetPosition, TargetWidth, TargetHeight) ||
                Collision.CanHitLine(MyCenterPosition + new Vector2(0, GravityDirection * (-(float)Height) * 0.3f), 0, 0, TargetPosition, TargetWidth, TargetHeight) ||
                Collision.CanHitLine(MyCenterPosition + new Vector2(0, 0), 0, 0, TargetPosition, TargetWidth, TargetHeight) ||
                Collision.CanHitLine(MyCenterPosition + new Vector2(0, GravityDirection * ((float)Height) * 0.3f), 0, 0, TargetPosition, TargetWidth, TargetHeight);
        }

        public bool CanUseItem(Item i)
        {
            if (true && i.ammo == 0 || MainMod.IsGuardianItem(i) || i.useStyle == 1)
            {
                return true;
            }
            return false;
        }

        public void PickWeaponForSituation(byte PrefferedType = 255, bool HeavyToo = true)
        {
            SelectedItem = -1;
            Vector2 TargetCenter;
            int TargetWidth, TargetHeight;
            GetTargetInformation(out TargetCenter, out TargetWidth, out TargetHeight);
            TargetCenter.X += TargetWidth * 0.5f;
            TargetCenter.Y += TargetWidth * 0.5f;
            int HighestDamageMelee = 0, MeleePosition = -1, HighestDamageRanged = 0, RangedPosition = -1,
                HighestDamageMagic = 0, MagicPosition = -1;
            int MeleeWeaponRange = 0;
            if (Base.DontUseHeavyWeapons)
                HeavyToo = false;
            bool PickFirstOfType = UseWeaponsByInventoryOrder;
            for (int i = 0; i < 10; i++)
            {
                if (this.Inventory[i].type > 0 && CanUseItem(Inventory[i]) && (HeavyToo || (!MainMod.IsGuardianItem(Inventory[i]) || !Items.GuardianItemPrefab.IsHeavyItem(Inventory[i]))))
                {
                    if (this.Inventory[i].melee && ((PickFirstOfType && MeleePosition == -1) || (!PickFirstOfType && this.Inventory[i].damage > HighestDamageMelee)))
                    {
                        HighestDamageMelee = this.Inventory[i].damage;
                        MeleePosition = i;
                        MeleeWeaponRange = this.Inventory[i].height;
                        if (this.Inventory[i].width > MeleeWeaponRange)
                            MeleeWeaponRange = this.Inventory[i].width;
                    }
                    if (((this.Inventory[i].ranged && this.Inventory[i].ammo == Terraria.ID.AmmoID.None) || this.Inventory[i].thrown) && 
                        ((PickFirstOfType && RangedPosition == -1 ) || (!PickFirstOfType && this.Inventory[i].damage > HighestDamageRanged)))
                    {
                        HighestDamageRanged = this.Inventory[i].damage;
                        RangedPosition = i;
                    }
                    if (this.Inventory[i].magic && ((PickFirstOfType && MagicPosition == -1) || (!PickFirstOfType && this.Inventory[i].damage > HighestDamageMagic)))
                    {
                        HighestDamageMagic = this.Inventory[i].damage;
                        MagicPosition = i;
                    }
                }
            }
            if (PrefferedType != 255)
            {
                if (PrefferedType == 0 && MeleePosition > -1)
                {
                    SelectedItem = MeleePosition;
                    return;
                }
                if (PrefferedType == 1 && RangedPosition > -1)
                {
                    SelectedItem = RangedPosition;
                    return;
                }
                if (PrefferedType == 2 && MagicPosition > -1)
                {
                    SelectedItem = MagicPosition;
                    return;
                }
            }
            float Distance = (TargetCenter - CenterPosition).Length();
            if (Distance >= MeleeWeaponRange + TargetWidth * 0.5f && RangedPosition > -1)
            {
                SelectedItem = RangedPosition;
            }
            else if (MeleePosition > -1)
            {
                SelectedItem = MeleePosition;
            }
            else
            {
                SelectedItem = -1;
            }
        }

        public void PickOffhandForTheSituation(bool Combat)
        {
            int LastOffHand = SelectedOffhand;
            SelectedOffhand = -1;
            bool DefenseEquipment = Combat;
            int HighestShieldValue = 0;
            for (int s = 0; s < 10; s++)
            {
                if (MainMod.IsGuardianItem(Inventory[s]))
                {
                    Items.GuardianItemPrefab gip = (Inventory[s].modItem as Items.GuardianItemPrefab);
                    if (DefenseEquipment && gip.Shield && gip.BlockRate > HighestShieldValue)
                    {
                        SelectedOffhand = s;
                        HighestShieldValue = gip.BlockRate;
                    }
                    if (!DefenseEquipment && gip.OffHandItem)
                    {
                        SelectedOffhand = s;
                    }
                }
                if (Inventory[s].type == 946 && (SelectedOffhand == -1 || Velocity.Y > 0)) //Umbrella
                {
                    SelectedOffhand = s;
                    if(Velocity.Y > 0)
                        HighestShieldValue = 999;
                }
                if (Inventory[s].type == Terraria.ID.ItemID.WaterCandle || Inventory[s].type == Terraria.ID.ItemID.PeaceCandle)
                {
                    SelectedOffhand = s;
                }
                if (SelectedOffhand == -1 && Inventory[s].type == Terraria.ID.ItemID.UnicornonaStick)
                {
                    SelectedOffhand = s;
                }
            }
            if (SelectedOffhand == -1)
            {
                /*bool NeedOfUsingLight = false;
                float LightingCounter = 0;
                if (LastSelectedOffhand > -1)
                    LightingCounter -= 1f;
                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        if (x == 0 && y == 0)
                            continue;
                        int TileX = (int)(Position.X * DivisionBy16) + 10 * x, TileY = (int)(CenterY * DivisionBy16) + 10 * y;
                        if (TileX >= 0 && TileY >= 0 && TileX < Main.maxTilesX && TileY < Main.maxTilesY)
                        {
                            //Tile tile = MainMod.GetTile(TileX, TileY);
                            LightingCounter += Lighting.Brightness(TileX, TileY);
                        }
                    }
                }
                NeedOfUsingLight = LightingCounter * (1f / 9) < 0.6f;
                if (NeedOfUsingLight)*/
                {
                    for (int i = 0; i < 50; i++)
                    {
                        int type2 = this.Inventory[i].type;
                        if (type2 == 282 || type2 == 286 || type2 == 523 || type2 == 1333 || type2 == 3002 || type2 == 3112)
                        {
                            SelectedOffhand = i;
                            break;
                        }
                        if (!Wet)
                        {
                            if (type2 == 8 || type2 == 427 || type2 == 428 || type2 == 429 || type2 == 430 || type2 == 431 || type2 == 432 || type2 == 433 || type2 == 974 || type2 == 1245 || type2 == 2274 || type2 == 3004 || type2 == 3045 || type2 == 3114)
                            {
                                SelectedOffhand = i;
                                break;
                            }
                        }
                    }
                }
            }
            //if (DefenseEquipment)
            {
                /*if (HeldItemHand == GuardianItemExtraData.HeldHand.Both && (ItemAnimationTime > 0 || Action))
                {
                    SelectedOffhand = -1;
                    HighestShieldValue = 0;
                }*/
            }
            if (SelectedOffhand > -1 && SelectedOffhand != LastOffHand && MainMod.IsGuardianItem(this.Inventory[SelectedOffhand]))
            {
                this.UpdateStatus = true;
                BlockRate += HighestShieldValue;
            }
        }

        public void GetTargetInformation(out Vector2 Position, out int Width, out int Height)
        {
            if (IsAttackingSomething)
            {
                switch (TargetType)
                {
                    case TargetTypes.Player:
                        Position = Main.player[TargetID].position;
                        Width = Main.player[TargetID].width;
                        Height = Main.player[TargetID].height;
                        break;
                    case TargetTypes.Npc:
                        Position = Main.npc[TargetID].position;
                        Width = Main.npc[TargetID].width;
                        Height = Main.npc[TargetID].height;
                        break;
                    case TargetTypes.Guardian:
                        Position = MainMod.ActiveGuardians[TargetID].TopLeftPosition;
                        Width = MainMod.ActiveGuardians[TargetID].Width;
                        Height = MainMod.ActiveGuardians[TargetID].Height;
                        break;
                    default:
                        Position = Vector2.Zero;
                        Width = Height = 0;
                        break;
                }
                return;
            }
            Position = Vector2.Zero;
            Width = Height = 0;
        }

        public float GetDamageMultipliersFromItem(Item I)
        {
            float DamageMult = 1f;
            if (I.melee)
                DamageMult *= MeleeDamageMultiplier;
            else if (I.ranged || I.thrown)
                DamageMult *= RangedDamageMultiplier;
            else if (I.magic)
            {
                float Mult = MagicDamageMultiplier;
                if (HasBuff(Terraria.ID.BuffID.ManaSickness))
                {
                    Mult *= 1f - (Player.manaSickLessDmg * ((float)GetBuff(Terraria.ID.BuffID.ManaSickness).Time / Player.manaSickTime));
                }
                DamageMult *= Mult;
            }
            else if (I.summon)
                DamageMult *= SummonDamageMultiplier;
            else
                DamageMult *= NeutralDamageMultiplier;
            return DamageMult;// *ItemScaleMod(I, true);
        }

        public HeldHand GetItemUseHand(Item i)
        {
            HeldHand hand = HeldHand.Both;
            if (MainMod.IsGuardianItem(i)) hand = (i.modItem as Items.GuardianItemPrefab).hand;
            else hand = HeldHand.Left;
            PickHandToUse(ref hand);
            return hand;
        }

        public Point GetItemShotSpawnPos(Item i, bool Drawing = false)
        {
            Point Origin = Point.Zero;
            if (MainMod.IsGuardianItem(i))
                Origin = (i.modItem as Items.GuardianItemPrefab).ShotSpawnPosition;
            else
            {
                int ItemWidth = Main.netMode < 2 ? Main.itemTexture[i.type].Width : i.width,
                    ItemHeight = Main.netMode < 2 ? Main.itemTexture[i.type].Height : i.height;

            }
            return Origin;
        }

        public Vector2 GetItemOrigin(Item i, bool Drawing = false)
        {
            Vector2 Origin = Vector2.Zero;
            if (MainMod.IsGuardianItem(i))
            {
                Origin = (i.modItem as Items.GuardianItemPrefab).ItemOrigin;
                if (GravityDirection < 0)
                    Origin.Y = i.height - Origin.Y;
            }
            else
            {
                int ItemWidth = Main.netMode < 2 ? Main.itemTexture[i.type].Width : i.width,
                    ItemHeight = Main.netMode < 2 ? Main.itemTexture[i.type].Height : i.height;
                if (i.type == Terraria.ID.ItemID.PeaceCandle)
                {
                    //OffHandPositionX -= 16 * Direction;
                    Origin.Y += ItemHeight - 2;
                    if (Direction < 0)
                        Origin.X = ItemWidth - Origin.X;
                }
                else if (i.type == Terraria.ID.ItemID.WaterCandle)
                {
                    //OffHandPositionX -= 12 * Direction;
                    Origin.Y += ItemHeight - 2;
                    if (Direction < 0)
                        Origin.X = ItemWidth - Origin.X;
                }
                else if (i.type == 282 || i.type == 286 || i.type == 3002 || i.type == 3112 || i.type == 974 || i.type == 8 || i.type == 1245 || i.type == 2274 || i.type == 3004 || i.type == 3045 || i.type == 3114 || (i.type >= 427 && i.type <= 433) || i.type == 523 || i.type == 1333)
                {
                    Origin.X = 2;
                    if (Direction < 0) Origin.X = ItemWidth - 2;
                    Origin.Y = ItemHeight - 2;
                }
                else if (false && Drawing)
                {
                    Origin.X = 4f;
                    Origin.Y = ItemWidth - 4f;
                }
                else
                {
                    switch (i.useStyle)
                    {
                        case 1:
                            Origin.X = 10;
                            if (ItemWidth > 32)
                                Origin.X += 8;
                            if (ItemWidth >= 52)
                                Origin.X += 6;
                            if (ItemWidth >= 64)
                                Origin.X += 4;
                            if (ItemWidth > 92)
                                Origin.X += 10;
                            Origin.X = (ItemWidth * 0.5f - Origin.X);
                            Origin.Y = 10;
                            if (ItemHeight > 32)
                                Origin.Y -= 2;
                            if (ItemHeight > 52)
                                Origin.Y += 4;
                            if (ItemHeight > 64)
                                Origin.Y += 2;
                            if (i.type == 2330 || i.type == 2320 || i.type == 2341)
                                Origin.Y += 4;
                            Origin.Y = ItemHeight - Origin.Y;
                            break;
                        default:
                            Origin.X = (int)(ItemWidth * 0.5f);
                            Origin.Y = (int)(ItemHeight * 0.5f);
                            break;
                        case 5:
                            if (i.type == 3779)
                            {

                            }
                            else if (Item.staff[i.type])
                            {
                                Origin.Y = ItemHeight;
                            }
                            else
                            {
                                Origin.X = ItemWidth * 0.25f;// - Direction * 2;
                                Origin.Y = ItemHeight * 0.5f;
                            }
                            break;
                    }
                    //Origin.X = 4f;
                    //Origin.Y = i.width - 4f;
                }
                if (GravityDirection < 0)
                {
                    Origin.Y = ItemHeight - Origin.Y;
                }
            }
            return Origin;
        }

        public ItemUseTypes GetItemUseType(Item i)
        {
            ItemUseTypes type = (ItemUseTypes)(i.useStyle - 1);
            //Main.NewText("{"+ i.Name + " Guardian Item? " +giantsummon.IsGuardianItem(i) + " Is heavy weapon? " +  (giantsummon.IsGuardianItem(i) && !giantsummon.GetGuardianItemData(i.type).HeavyWeapon) +"}");
            if (i.useStyle == 1 && (!MainMod.IsGuardianItem(i) || MainMod.IsGuardianItem(i) && !(i.modItem as Items.GuardianItemPrefab).Heavy))
            {
                type = ItemUseTypes.LightVerticalSwing;
            }
            return type;
        }

        public bool Inclined45Degrees(Item i)
        {
            return (!MainMod.IsGuardianItem(i) && i.useStyle != 5);
        }

        public Vector2 GetLeftHandPosition(int Frame)
        {
            int x, y;
            GetLeftHandPosition(Frame, false, out x, out y);
            return new Vector2(x, y);
        }

        public Vector2 GetLeftHandPosition(int Frame, bool IgnoreDirection)
        {
            int x, y;
            GetLeftHandPosition(Frame, IgnoreDirection, out x, out y);
            return new Vector2(x, y);
        }

        public void GetLeftHandPosition(int Frame, out int X, out int Y)
        {
            GetLeftHandPosition(Frame, false, out X, out Y);
        }

        public void GetLeftHandPosition(int Frame, bool IgnoreDirection, out int X, out int Y)
        {
            UsingLeftArmAnimation = true;
            Base.GuardianAnimationOverride(this, 1, ref Frame);
            Base.LeftHandPoints.GetPositionFromFrame(Frame, out X, out Y);
            if (X < -100 || Y < -100)
                return;
            if (!IgnoreDirection && LookingLeft)
                X = SpriteWidth - X;
            X -= (int)(SpriteWidth * 0.5f);
            if (GravityDirection > 0) Y -= SpriteHeight;
            else
                Y = SpriteHeight + Base.CharacterPositionYDiscount - Y;
            X = (int)(X * Scale);
            Y = (int)(Y * Scale);
        }

        public Vector2 GetRightHandPosition(int Frame)
        {
            int x, y;
            GetRightHandPosition(Frame, out x, out y);
            return new Vector2(x, y);
        }

        public Vector2 GetRightHandPosition(int Frame, bool IgnoreDirection)
        {
            int x, y;
            GetRightHandPosition(Frame, IgnoreDirection, out x, out y);
            return new Vector2(x, y);
        }

        public void GetRightHandPosition(int Frame, out int X, out int Y)
        {
            GetRightHandPosition(Frame, false, out X, out Y);
        }

        public void GetRightHandPosition(int Frame, bool IgnoreDirection, out int X, out int Y)
        {
            UsingRightArmAnimation = true;
            Base.GuardianAnimationOverride(this, 2, ref Frame);
            Base.RightHandPoints.GetPositionFromFrame(Frame, out X, out Y);
            if (X < -100 || Y < -100)
                return;
            if (!IgnoreDirection && LookingLeft)
                X = SpriteWidth - X;
            X -= (int)(SpriteWidth * 0.5f);
            if (GravityDirection > 0) Y -= SpriteHeight;
            else
                Y = SpriteHeight + Base.CharacterPositionYDiscount - Y;
            X = (int)(X * Scale);
            Y = (int)(Y * Scale);
        }
        
        public void TryApplyingWeaponDebuffsToNpc(Item weapon, NPC target)
        {
            Player p = Main.player[255];
            p.meleeEnchant = MeleeEnchant;
            p.frostBurn = HasFlag(GuardianFlags.FrostBurn) || HasFlag(GuardianFlags.FrostSetEffect);
            p.magmaStone = HasFlag(GuardianFlags.MagmaStone);
            p.StatusNPC(weapon.type, target.whoAmI);
            p.frostBurn = false;
            p.magmaStone = false;
        }

        public void TryApplyingWeaponDebuffsToPlayer(Item weapon, Player target)
        {
            Player p = Main.player[255];
            p.meleeEnchant = MeleeEnchant;
            p.frostBurn = HasFlag(GuardianFlags.FrostBurn);
            p.magmaStone = HasFlag(GuardianFlags.MagmaStone);
            p.StatusPvP(weapon.type, target.whoAmI);
            p.frostBurn = false;
            p.magmaStone = false;
        }

        public void TryApplyingDebuffsToGuardian(Item weapon, TerraGuardian target)
        {
            Player p = Main.player[255];
            p.meleeEnchant = MeleeEnchant;
            p.frostBurn = HasFlag(GuardianFlags.FrostBurn);
            p.magmaStone = HasFlag(GuardianFlags.MagmaStone);
            for(int i = p.buffTime.Length - 1; i >= 0; i--)
            {
                if(p.buffTime[i] > 0)
                {
                    p.DelBuff(i);
                }
            }
            p.StatusPvP(weapon.type, 255);
            p.frostBurn = false;
            p.magmaStone = false;
            for (int i = p.buffTime.Length - 1; i >= 0; i--)
            {
                if (p.buffTime[i] > 0)
                {
                    target.AddBuff(p.buffType[i], p.buffTime[i]);
                    p.DelBuff(i);
                    i++;
                }
            }
        }

        public bool UseMana(int value)
        {
            if (value == 0)
                return true;
            bool Used = false;
            bool FirstTry = true;
        retry:
            if (MP >= value)
            {
                MP -= value;
                Used = true;
                AddCooldown(GuardianCooldownManager.CooldownType.ManaRegenDelay, MaxRegenDelay);
                return true;
            }
            if (FirstTry && !Used && HasFlag(GuardianFlags.AutoManaPotion))
            {
                FirstTry = false;
                int PotionPosition = PickManaPotion();
                if (PotionPosition > -1)
                {
                    int ManaHealValue = (int)(Inventory[SelectedItem].healMana * ManaHealMult);
                    RestoreMP(ManaHealValue);
                    goto retry;
                }
            }
            if (!Used) AttemptToUseManaPotion();
            return Used;
        }

        public Vector2 GetBetweenHandsPosition(int Frame)
        {
            int x, y;
            GetBetweenHandsPosition(Frame, false, out x, out y);
            return new Vector2(x, y);
        }

        public Vector2 GetBetweenHandsPosition(int Frame, bool IgnoreDirection)
        {
            int x, y;
            GetBetweenHandsPosition(Frame, IgnoreDirection, out x, out y);
            return new Vector2(x, y);
        }

        public void GetBetweenHandsPosition(int Frame, out int ItemPositionX, out int ItemPositionY)
        {
            GetBetweenHandsPosition(Frame, false, out ItemPositionX, out ItemPositionY);
        }

        public void GetBetweenHandsPosition(int Frame, bool IgnoreDirection, out int ItemPositionX, out int ItemPositionY)
        {
            Base.GetBetweenHandsPosition(Frame, out ItemPositionX, out ItemPositionY);
            if (ItemPositionX < -100 || ItemPositionY < -100)
                return;
            if (!IgnoreDirection && LookingLeft)
                ItemPositionX = SpriteWidth - ItemPositionX;
            ItemPositionX -= (int)(SpriteWidth * 0.5f);
            if (GravityDirection > 0) ItemPositionY -= SpriteHeight;
            else
                ItemPositionY = SpriteHeight + Base.CharacterPositionYDiscount - ItemPositionY;
            ItemPositionX = (int)(ItemPositionX * Scale);
            ItemPositionY = (int)(ItemPositionY * Scale);
        }

        public void GetBeeAttributes(out int Type, ref int Damage, ref float KB)
        {
            bool MakeStrongBees = HasFlag(GuardianFlags.BeeBuff) && Main.rand.Next(2) == 0;
            if (MakeStrongBees)
            {
                Type = 566;
                Damage += Main.rand.Next(1, 4);
                KB = 0.5f + KB * 1.1f;
                return;
            }
            Type = 181;
            Damage += Main.rand.Next(2);
        }

        private bool HasProjectileOfThisTypeSpawned(Item item)
        {
            switch (item.shoot)
            {
                case 6:
                case 13:
                case 19:
                case 32:
                case 33:
                case 52:
                case 106:
                case 113:
                case 182:
                case 230:
                case 231:
                case 232:
                case 233:
                case 234:
                case 235:
                case 272:
                case 301:
                case 315:
                case 320:
                case 331:
                case 333:
                case 372:
                    int Count = 0;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == GetSomeoneToSpawnProjectileFor && ProjMod.IsGuardianProjectile(i) && Main.projectile[i].type == item.shoot && Main.projectile[i].ai[0] != 2f)
                        {
                            Count++;
                            if (Count >= item.stack)
                                return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public void RideMount(int ID)
        {
            if(mount.CanMount(ID, this))
            {
                mount.SetMount(ID, this, false);
            }
        }

        public void Dismount()
        {
            mount.Dismount(this);
        }

        private float GetTwoHandedItemUsePercentage(float Animation)
        {
            float Result = Animation * 1.2f - 0.1f;
            if (Result < 0)
                Result = 0;
            else if (Result > 1)
                Result = 1;
            return Result;
        }

        public void ItemUseScript()
        {
            if (HasFlag(GuardianFlags.Frozen) || HasFlag(GuardianFlags.Petrified))
                return;
            if(ItemAnimationTime > 0 && KnockedOut)
            {
                ItemAnimationTime = ItemUseTime = 0;
            }
            bool TriggerItem = false, ToolTrigger = false;
            float Knockback = 8f;
            int CriticalRate = 0;
            if (Action && !Channeling && ItemAnimationTime == 0 && (!DoAction.InUse || !DoAction.AvoidItemUsage))
            {
                ItemScale = Scale;
                bool AllowItemUsage = true;
                if (SelectedItem > -1)
                {
                    if (HeldProj > -1)
                        AllowItemUsage = false;
                    else if (Inventory[SelectedItem].type > 0 && Inventory[SelectedItem].useStyle == 0)
                        AllowItemUsage = false;
                    else if (LastAction && !Inventory[SelectedItem].autoReuse)
                        AllowItemUsage = false;
                    else if (!PlayerControl && (Inventory[SelectedItem].createTile > -1 || Inventory[SelectedItem].createWall > -1))
                    {
                        AllowItemUsage = false;
                    }
                    else if (HasProjectileOfThisTypeSpawned(Inventory[SelectedItem]))
                    {
                        AllowItemUsage = false;
                    }
                    else if (Inventory[SelectedItem].potion && HasCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown))
                    {
                        AllowItemUsage = false;
                    }
                    else if (Inventory[SelectedItem].modItem != null && Inventory[SelectedItem].modItem is Items.GuardianItemPrefab && 
                        !((Items.GuardianItemPrefab)Inventory[SelectedItem].modItem).GuardianCanUse(this))
                        AllowItemUsage = false;
                    else if (Inventory[SelectedItem].mana > 0)
                    {
                        AllowItemUsage = false;
                        if (HasFlag(GuardianFlags.Silence))
                        {
                            AllowItemUsage = false;
                        }
                        else if (Inventory[SelectedItem].type == Terraria.ID.ItemID.SpaceGun && HasFlag(GuardianFlags.MeteorSetEffect))
                        {
                            AllowItemUsage = true;
                        }
                        else
                        {
                            AllowItemUsage = UseMana((int)(Inventory[SelectedItem].mana * ManaCostMult));
                        }
                    }
                    else if(Inventory[SelectedItem].useAmmo > 0)
                    {
                        AllowItemUsage = HasAmmo(Inventory[SelectedItem]);
                    }
                }
                if (GrabbingPlayer && !Base.DontUseRightHand)
                    AllowItemUsage = false;
                if (SelectedItem > -1 && Base.DontUseHeavyWeapons && MainMod.IsGuardianItem(Inventory[SelectedItem]) && (Inventory[SelectedItem].modItem as Items.GuardianItemPrefab).Heavy)
                {
                    AllowItemUsage = false;
                }
                if (AllowItemUsage)
                {
                    ItemScale = Scale;
                    if(SelectedItem != LastSelectedItem) IsDualWielding = false;
                    if (HasFlag(GuardianFlags.Cursed))
                    {
                        ItemUseType = ItemUseTypes.CursedAttackAttempt;
                        ItemUseTime = ItemAnimationTime = ItemAnimationTime = ItemMaxUseTime = 50;
                        SelectedItem = -1;
                        Base.HurtSound.PlaySound(CenterPosition);
                        AddCooldown(GuardianCooldownManager.CooldownType.CursedEffect, 50);
                    }
                    else if (SelectedItem == -1 || Inventory[SelectedItem].type == 0)
                    {
                        int AtkSpeed = (int)(30 * MeleeSpeed);
                        ItemAnimationTime = ItemMaxAnimationTime = AtkSpeed;
                        ItemUseTime = ItemMaxUseTime = AtkSpeed;
                        Damage = 20;
                        if (Base.DontUseHeavyWeapons)
                            ItemUseType = ItemUseTypes.LightVerticalSwing;
                        else
                            ItemUseType = ItemUseTypes.HeavyVerticalSwing;
                        CriticalRate = 5;
                        Knockback *= MeleeKnockback;
                        LastSelectedItem = SelectedItem;
                        LastSelectedOffhand = SelectedOffhand;
                    }
                    else
                    {
                        if (Base.BeforeUsingItem(this, ref SelectedItem))
                        {
                            ItemUseType = GetItemUseType(Inventory[SelectedItem]);
                            int AnimationTime = Inventory[SelectedItem].useAnimation,
                                UseTime = Inventory[SelectedItem].useTime;
                            Channeling = Inventory[SelectedItem].channel;
                            ItemScale *= Inventory[SelectedItem].scale;
                            float MeleeSpeed = this.MeleeSpeed;
                            if ((Inventory[SelectedItem].potion && !MainMod.IsGuardianItem(Inventory[SelectedItem])) || Inventory[SelectedItem].buffType == Terraria.ID.BuffID.WellFed || Inventory[SelectedItem].buffType == Terraria.ID.BuffID.Tipsy)
                            {
                                AnimationTime *= 3;
                                UseTime *= 3;
                            }
                            if (MainMod.IsGuardianItem(Inventory[SelectedItem]) && ((Items.GuardianItemPrefab)Inventory[SelectedItem].modItem).Heavy)
                            {
                                MeleeSpeed -= (MeleeSpeed - 1f) * 0.5f;
                            }
                            if (Inventory[SelectedItem].buffType > 0)
                            {
                                AnimationTime *= 2;
                                UseTime *= 2;
                            }
                            if (Inventory[SelectedItem].melee)
                            {
                                AnimationTime = (int)(AnimationTime * MeleeSpeed);
                                UseTime = (int)(UseTime * MeleeSpeed);
                            }
                            if (Inventory[SelectedItem].ranged || Inventory[SelectedItem].thrown)
                            {
                                AnimationTime = (int)(AnimationTime * RangedSpeed);
                                UseTime = (int)(UseTime * RangedSpeed);
                            }
                            if (Inventory[SelectedItem].magic)
                            {
                                AnimationTime = (int)(AnimationTime * MagicSpeed);
                                UseTime = (int)(UseTime * MagicSpeed);
                            }
                            ItemAnimationTime = ItemMaxAnimationTime = AnimationTime;
                            if (SelectedItem != LastSelectedItem)
                                ItemUseTime = ToolUseTime = 0;
                            ItemMaxUseTime = UseTime;
                            Damage = (int)(Inventory[SelectedItem].damage * GetDamageMultipliersFromItem(Inventory[SelectedItem]));
                            if (ItemUseType != ItemUseTypes.AimingUse && Inventory[SelectedItem].useAnimation != Inventory[SelectedItem].useTime)
                            {
                                float DamageScaler = (float)Inventory[SelectedItem].useAnimation / Inventory[SelectedItem].useTime;
                                Damage += (int)(Damage * DamageScaler);
                            }
                            if (Inventory[SelectedItem].type == Terraria.ID.ItemID.GoldenShower && HasFlag(GuardianFlags.GoldenShowerStance))
                            {
                                Damage += (int)(Damage * 0.2f);
                            }
                            if (CanDualWield && (ItemUseType == ItemUseTypes.AimingUse || ItemUseType == ItemUseTypes.LightVerticalSwing))
                            {
                                if (LastSelectedItem != SelectedItem)
                                {
                                    if (MainMod.DualwieldWhitelist.Count == 0)
                                        MainMod.GetDefaultDualwieldableItems();
                                    IsDualWielding = MainMod.DualwieldWhitelist.Any(x => x.Type == Inventory[SelectedItem].type);
                                    Damage = (int)(Damage * 0.8f);
                                }
                                //IsDualWielding = true;
                            }
                            Knockback = Inventory[SelectedItem].knockBack;
                            Main.PlaySound(Inventory[SelectedItem].UseSound, CenterPosition);
                            if (Damage < 1) Damage = 1;
                            CriticalRate = Inventory[SelectedItem].crit;
                            if (Inventory[SelectedItem].melee)
                                Knockback *= MeleeKnockback;
                            if (Inventory[SelectedItem].ranged)
                                Knockback *= RangedKnockback;
                            LastSelectedItem = SelectedItem;
                            LastSelectedOffhand = SelectedOffhand;
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (ItemUseType == ItemUseTypes.HeavyVerticalSwing && MountedOnPlayer)
                        ItemUseType = ItemUseTypes.LightVerticalSwing;
                    if (HasFlag(GuardianFlags.KnockbackPowerPotion))
                        Knockback *= 1.5f;
                    PlayerHit.Clear();
                    NpcHit.Clear();
                    TriggerItem = true;
                    ToolTrigger = true;
                }
            }
            if (FreezeItemUseAnimation)
            {
                FreezeItemUseAnimation = false;
                return;
            }
            HeldItemHand = HeldHand.Left;
            PickHandToUse(ref HeldItemHand);
            ItemUseEffect(true, ref ItemAnimationTime, ref ItemUseTime, ref SelectedItem, ref HeldItemHand, ref ItemPositionX, ref ItemPositionY, ref ItemRotation, ref CriticalRate, ref Knockback, ref TriggerItem, ref ToolTrigger);
            if (IsDualWielding)
            {
                HeldOffHand = HeldItemHand;
                PickOffHandToUse(ref HeldOffHand);
                ItemUseEffect(false, ref ItemAnimationTime, ref ItemUseTime, ref SelectedItem, ref HeldOffHand, ref OffHandPositionX, ref OffHandPositionY, ref OffhandRotation, ref CriticalRate, ref Knockback, ref TriggerItem, ref ToolTrigger);
            }
            if (SelectedOffhand > -1 && OffHandAction)
            {
                OffHandPositionX = 0;
                OffHandPositionY = 0;
                //if (HeldItemHand != GuardianItemExtraData.HeldHand.Both)
                {
                    HeldOffHand = HeldItemHand;
                    if (HeldOffHand != HeldHand.Both)
                        PickOffHandToUse(ref HeldOffHand);
                    float AngleChecker = CalculateAimingUseAnimation(ItemRotation);
                    OffhandRotation = 0;
                    //bool IsUmbrella = Inventory[SelectedOffhand].type == 946;
                    int Frame = 0;
                    /*if (Inventory[SelectedOffhand].type == 946)
                    {
                        if (Ducking)
                            Frame = Base.DuckingSwingFrames[1];
                        else
                            Frame = Base.ItemUseFrames[1];
                    }
                    else*/
                    {
                        if (Ducking)
                        {
                            Frame = Base.DuckingSwingFrames[2];
                        }
                        else
                        {
                            Frame = Base.ItemUseFrames[2];
                        }
                    }
                    OffhandOrientation = 0;
                    bool CheckingAgain = false;
                    GetPositionAgain:
                    if (HeldOffHand == HeldHand.Both)
                    {
                        GetBetweenHandsPosition(Frame, out OffHandPositionX, out OffHandPositionY);
                    }
                    if (HeldOffHand == HeldHand.Left)
                    {
                        GetLeftHandPosition(Frame, out OffHandPositionX, out OffHandPositionY);
                    }
                    if (HeldOffHand == HeldHand.Right)
                    {
                        GetRightHandPosition(Frame, out OffHandPositionX, out OffHandPositionY);
                    }
                    if (!CheckingAgain)
                    {
                        CheckingAgain = true;
                        Tile t = MainMod.GetTile((int)((Position.X + OffHandPositionX) * DivisionBy16), (int)((Position.Y + OffHandPositionY) * DivisionBy16));
                        if (!Ducking && t.active() && Main.tileSolid[t.type])
                        {
                            if (OwnerPos > -1 && !PlayerMounted && Main.player[OwnerPos].Center.Y < Position.Y - Height * 0.5f)
                            {
                                Frame = Base.ItemUseFrames[1];
                                OffhandOrientation = -1;
                            }
                            else
                            {
                                Frame = Base.ItemUseFrames[3];
                                OffhandOrientation = 1;
                            }
                            goto GetPositionAgain;
                        }
                    }
                }
                if (Inventory[SelectedOffhand].type == Terraria.ID.ItemID.PeaceCandle)
                {
                    AddBuff(Terraria.ID.BuffID.PeaceCandle, 3, true);
                    if (!this.Wet)
                    {
                        Lighting.AddLight(PositionWithOffset + new Vector2(OffHandPositionX, OffHandPositionY), 0.9f, 0.1f, 0.75f);
                    }
                }
                if (Inventory[SelectedOffhand].type == Terraria.ID.ItemID.WaterCandle)
                {
                    AddBuff(Terraria.ID.BuffID.WaterCandle, 3, true);
                    if (!this.Wet)
                    {
                        Lighting.AddLight(PositionWithOffset + new Vector2(OffHandPositionX, OffHandPositionY), 0, 0.5f, 1f);
                    }
                }
                if (Inventory[SelectedOffhand].type == Terraria.ID.ItemID.UnicornonaStick)
                {
                    //OffHandPositionY += 38 / 2;
                    OffHandPositionY -= 12;
                    OffHandPositionX += 12 * Direction;
                    //OffHandPositionX += 38 / 2 * Direction;
                }
                if (Inventory[SelectedOffhand].type == 946)
                {
                    //OffHandPositionX -= (int)(4 * Direction);
                    OffHandPositionY -= 22;
                }
                else if (MainMod.IsGuardianItem(Inventory[SelectedOffhand]))
                    ((Items.GuardianItemPrefab)Inventory[SelectedOffhand].modItem).ItemUpdateScript(this);
                TorchLightingHandler(Inventory[SelectedOffhand], true);
            }
            if (ItemAnimationTime < 0)
                ItemAnimationTime = 0;
        }

        private void ItemUseEffect(bool MainHand, ref int ItemAnimationTime, ref int ItemUseTime, ref int SelectedItem, ref HeldHand HeldItemHand, ref int ItemPositionX, ref int ItemPositionY, ref float ItemRotation, ref int CriticalRate, ref float Knockback, ref bool TriggerItem, ref bool ToolTrigger)
        {
            if (ItemAnimationTime > 0)
            {
                if (MainHand)
                {
                    TriggerItem = ItemUseTime == 0;
                    if (SelectedItem > -1 && (Inventory[SelectedItem].pick > 0 || Inventory[SelectedItem].axe > 0 || Inventory[SelectedItem].hammer > 0))
                    {
                        ToolTrigger = ToolUseTime == 0;
                    }
                    if (ItemUseTime <= 0)
                    {
                        ItemUseTime += ItemMaxUseTime;
                        if (SelectedItem > -1)
                        {
                            if (Inventory[SelectedItem].createTile >= 0) //Tile speed mult
                            {
                                ItemUseTime = (int)(ItemUseTime * 1f);
                            }
                            if (Inventory[SelectedItem].createWall >= 0) //Wall speed mult
                            {
                                ItemUseTime = (int)(ItemUseTime * 1f);
                            }
                        }
                    }
                    if (ToolUseTime <= 0)
                    {
                        if (SelectedItem > -1)
                        {
                            if ((Inventory[SelectedItem].pick > 0 || Inventory[SelectedItem].axe > 0 || Inventory[SelectedItem].hammer > 0))
                            {
                                ToolUseTime += Inventory[SelectedItem].useTime;
                            }
                            else
                            {
                                ToolUseTime += (int)(Inventory[SelectedItem].useTime); //Pickspeed mult
                            }
                        }
                    }
                }
                float AnimationPercentage = 1f - (float)ItemAnimationTime / ItemMaxAnimationTime;
                Vector2 ItemOrigin = Vector2.Zero;
                int ItemWidth = 28, ItemHeight = 46;
                bool InclinedWeapon = false;
                if (SelectedItem > -1)
                {
                    if (!MainMod.IsGuardianItem(Inventory[SelectedItem]) && Main.netMode == 0)
                    {
                        ItemWidth = (int)(Main.itemTexture[Inventory[SelectedItem].type].Width * ItemScale);
                        ItemHeight = (int)(Main.itemTexture[Inventory[SelectedItem].type].Height * ItemScale);
                        if (Main.itemAnimationsRegistered.Contains(Inventory[SelectedItem].type))
                            ItemHeight /= Main.itemAnimations[Inventory[SelectedItem].type].FrameCount;
                    }
                    else
                    {
                        ItemWidth = (int)(Inventory[SelectedItem].width * ItemScale);
                        ItemHeight = (int)(Inventory[SelectedItem].height * ItemScale);
                    }

                    ItemOrigin = GetItemOrigin(Inventory[SelectedItem]) * ItemScale;//giantsummon.GetGuardianItemData(Inventory[SelectedItem].type).ItemOrigin;
                    InclinedWeapon = Inclined45Degrees(Inventory[SelectedItem]);
                }
                Rectangle WeaponCollision = new Rectangle(0, 0, 0, 0);
                if (ItemUseType == ItemUseTypes.ClawAttack) //Claw
                {
                    HeldItemHand = HeldHand.Both;
                    if (!PlayerControl)
                        FaceDirection(AimDirection.X < 0);
                    if (AnimationPercentage >= 0.4f)
                    {
                        WeaponCollision.X = (int)(PositionWithOffset.X + (Width * 0.25f) * Direction);
                        WeaponCollision.Y = (int)PositionWithOffset.Y;
                        if (GravityDirection > 0)
                            WeaponCollision.Y -= Height;
                        WeaponCollision.Width = (int)(Width * 0.5f);
                        WeaponCollision.Height = Height;
                        if (LookingLeft)
                            WeaponCollision.X -= WeaponCollision.Width;
                        if (GravityDirection < 0)
                            WeaponCollision.Y += WeaponCollision.Height;
                    }
                }
                else if (ItemUseType == ItemUseTypes.HeavyVerticalSwing)
                {
                    if (!PlayerControl)
                        FaceDirection(AimDirection.X < 0);
                    int Frame = 0;
                    float ThisAnimationPercentage = GetTwoHandedItemUsePercentage(AnimationPercentage);
                    if (ThisAnimationPercentage < 0.45f)
                    {
                        Frame = Base.HeavySwingFrames[0];
                    }
                    else if (ThisAnimationPercentage < 0.65f)
                    {
                        Frame = Base.HeavySwingFrames[1];
                    }
                    else
                    {
                        Frame = Base.HeavySwingFrames[2];
                    }
                    if (!Base.OneHanded2HWeaponWield)
                    {
                        HeldItemHand = HeldHand.Both;
                        GetBetweenHandsPosition(Frame, out ItemPositionX, out ItemPositionY);
                    }
                    else
                    {
                        HeldItemHand = HeldHand.Left;
                        GetLeftHandPosition(Frame, out ItemPositionX, out ItemPositionY);
                    }
                    float RotationValue = (float)Math.Sin(ThisAnimationPercentage * 1.35f) * (300 * ThisAnimationPercentage);
                    ItemRotation = MathHelper.ToRadians(-158 + RotationValue);
                    if (InclinedWeapon)
                        ItemRotation -= MathHelper.ToRadians(-45);
                    ItemRotation *= (LookingLeft ? -1 : 1);
                    if (GravityDirection < 0)
                        ItemRotation *= -1;
                    WeaponCollision.Width = (int)(ItemHeight * Math.Sin(ItemRotation) + ItemWidth * Math.Cos(ItemRotation));
                    WeaponCollision.Height = (int)(ItemHeight * Math.Cos(ItemRotation) + ItemWidth * Math.Sin(ItemRotation)) * -1;
                    WeaponCollision.X -= (int)((ItemHeight - ItemOrigin.Y) * Math.Sin(ItemRotation) + (ItemWidth - ItemOrigin.X) * Math.Cos(ItemRotation));
                    WeaponCollision.Y -= (int)((ItemHeight - ItemOrigin.Y) * Math.Cos(ItemRotation) + (ItemWidth - ItemOrigin.X) * Math.Sin(ItemRotation)) * -1;
                    if (WeaponCollision.Width < 0)
                    {
                        WeaponCollision.X += WeaponCollision.Width;
                        WeaponCollision.Width *= -1;
                    }
                    if (WeaponCollision.Height < 0)
                    {
                        WeaponCollision.Y += WeaponCollision.Height;
                        WeaponCollision.Height *= -1;
                    }
                    WeaponCollision.X += ItemPositionX + (int)PositionWithOffset.X;
                    WeaponCollision.Y += ItemPositionY + (int)PositionWithOffset.Y;
                    /*for(int i = 0; i < 4; i++)
                    {
                        Vector2 Position = new Vector2(WeaponCollision.X, WeaponCollision.Y);
                        switch (i)
                        {
                            case 1:
                                Position.X += WeaponCollision.Width;
                                break;
                            case 2:
                                Position.Y += WeaponCollision.Height;
                                break;
                            case 3:
                                Position.X += WeaponCollision.Width;
                                Position.Y += WeaponCollision.Height;
                                break;
                        }
                        Dust d = Dust.NewDustDirect(Position, 1, 1, 50);
                        d.velocity = Vector2.Zero;
                        d.noGravity = true;
                    }*/
                }
                else if (ItemUseType == ItemUseTypes.LightVerticalSwing)
                {
                    if (!PlayerControl)
                        FaceDirection(AimDirection.X < 0);
                    byte Frame = 0;
                    int AnimationFrame = 0;
                    if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                    {
                        if (AnimationPercentage < 0.333f)
                        {
                            AnimationFrame = Base.SittingItemUseFrames[0];
                        }
                        else if (AnimationPercentage < 0.666f)
                        {
                            Frame = 1;
                            AnimationFrame = Base.SittingItemUseFrames[1];
                        }
                        else
                        {
                            Frame = 2;
                            AnimationFrame = Base.SittingItemUseFrames[2];
                        }
                    }
                    else if (!Ducking)
                    {
                        if (AnimationPercentage < 0.25f)
                        {
                            AnimationFrame = Base.ItemUseFrames[0];
                        }
                        else if (AnimationPercentage < 0.5f)
                        {
                            Frame = 1;
                            AnimationFrame = Base.ItemUseFrames[1];
                        }
                        else if (AnimationPercentage < 0.75f)
                        {
                            Frame = 2;
                            AnimationFrame = Base.ItemUseFrames[2];
                        }
                        else
                        {
                            Frame = 3;
                            AnimationFrame = Base.ItemUseFrames[3];
                        }
                    }
                    else
                    {
                        if (AnimationPercentage < 0.333f)
                        {
                            AnimationFrame = Base.DuckingSwingFrames[0];
                        }
                        else if (AnimationPercentage < 0.666f)
                        {
                            Frame = 1;
                            AnimationFrame = Base.DuckingSwingFrames[1];
                        }
                        else
                        {
                            Frame = 2;
                            AnimationFrame = Base.DuckingSwingFrames[2];
                        }
                    }
                    if (HeldItemHand == HeldHand.Left)
                    {
                        GetLeftHandPosition(AnimationFrame, out ItemPositionX, out ItemPositionY);
                    }
                    else if (HeldItemHand == HeldHand.Right)
                    {
                        GetRightHandPosition(AnimationFrame, out ItemPositionX, out ItemPositionY);
                    }
                    ItemRotation = ((float)ItemAnimationTime / ItemMaxAnimationTime - 0.5f) * -Direction * 3.5f - Direction * 0.3f;
                    if (!InclinedWeapon)
                        ItemRotation += MathHelper.ToRadians(-45) * -Direction;
                    if (GravityDirection < 0)
                        ItemRotation *= -1;
                    //WeaponCollision.X = ItemPositionX + (int)PositionWithOffset.X;
                    //WeaponCollision.Y = ItemPositionY + (int)PositionWithOffset.Y;
                    WeaponCollision.Width = ItemWidth;
                    WeaponCollision.Height = ItemHeight;
                    if (Frame == 0)
                    {
                        WeaponCollision.X -= (int)(WeaponCollision.Width * 0.8f * Direction);
                        WeaponCollision.Width = (int)(WeaponCollision.Width * 1.4f);
                        WeaponCollision.Y += (int)(WeaponCollision.Height * 0.2f * GravityDirection);
                        WeaponCollision.Height = (int)(WeaponCollision.Height * 1.5f);
                    }
                    else if (Frame == 2)
                    {
                        WeaponCollision.Width *= 2;
                        WeaponCollision.Y += (int)(WeaponCollision.Height * 0.8f * GravityDirection);
                        WeaponCollision.Height = (int)(WeaponCollision.Height * 2.2f);
                    }
                    if (LookingLeft)
                    {
                        //WeaponCollision.X *= -1;
                        WeaponCollision.X -= WeaponCollision.Width;
                    }
                    if (GravityDirection == 1)
                    {
                        //WeaponCollision.Y *= -1;
                        WeaponCollision.Y -= WeaponCollision.Height;
                    }
                    WeaponCollision.X += ItemPositionX + (int)PositionWithOffset.X;
                    WeaponCollision.Y += ItemPositionY + (int)PositionWithOffset.Y;
                    //if (Frame == 0)
                    /*{
                        Dust.NewDust(new Vector2(WeaponCollision.X, WeaponCollision.Y), 1, 1, 50);
                        Dust.NewDust(new Vector2(WeaponCollision.X + WeaponCollision.Width, WeaponCollision.Y), 1, 1, 50);
                        Dust.NewDust(new Vector2(WeaponCollision.X, WeaponCollision.Y + WeaponCollision.Height), 1, 1, 50);
                        Dust.NewDust(new Vector2(WeaponCollision.X + WeaponCollision.Width, WeaponCollision.Y + WeaponCollision.Height), 1, 1, 50);
                    }*/
                }
                else if (ItemUseType == ItemUseTypes.AimingUse)
                {
                    if (TriggerItem)
                    {
                        //HeldItemHand = Hand;
                        float ShotDirection = (float)Main.rand.NextDouble() * 6.283185307179586f;
                        float DistanceAccuracy = 5 * ((CenterPosition - AimPosition).Length() * 0.0625f);
                        Vector2 AimDirectionChange = AimPosition + ShotDirection.ToRotationVector2() * (DistanceAccuracy - Accuracy * DistanceAccuracy);
                        LookingLeft = AimDirection.X < 0;//Position.X - AimPosition.X >= 0;
                        float AngleChecker = MathHelper.WrapAngle((float)Math.Atan2((CenterY - AimDirectionChange.Y) * GravityDirection, Position.X - AimDirectionChange.X));
                        float ArmAngle = CalculateAimingUseAnimation(AngleChecker);
                        //if (GravityDirection < 0)
                        //    ArmAngle *= -1;
                        int Frame = 0;
                        bool WeaponIsBlowpipe = SelectedItem > -1 && (Inventory[SelectedItem].type == Terraria.ID.ItemID.Blowpipe || Inventory[SelectedItem].type == Terraria.ID.ItemID.Blowgun);
                        if (SelectedItem > -1)
                        {
                            if (Inventory[SelectedItem].type == Terraria.ID.ItemID.GoldenShower && HasFlag(GuardianFlags.GoldenShowerStance))
                            {
                                ArmAngle = 0.65f;
                            }
                        }
                        if (ArmAngle < -0.75f || WeaponIsBlowpipe) //up
                        {
                            if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                            {
                                Frame = Base.SittingItemUseFrames[0];
                            }
                            else
                            {
                                Frame = (Ducking ? Base.DuckingSwingFrames[1] : Base.ItemUseFrames[1]);
                            }
                            ArmOrientation = 0;
                        }
                        else if (ArmAngle > 0.6f) //down
                        {
                            if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                            {
                                Frame = Base.SittingItemUseFrames[2];
                            }
                            else
                            {
                                Frame = (Ducking ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[3]);
                            }
                            ArmOrientation = 2;
                        }
                        else //center
                        {
                            if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                            {
                                Frame = Base.SittingItemUseFrames[1];
                            }
                            else
                            {
                                Frame = (Ducking ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[2]);
                            }
                            ArmOrientation = 1;
                        }
                        if (SelectedItem > -1)
                        {
                            if (Inventory[SelectedItem].type == Terraria.ID.ItemID.GoldenShower && HasFlag(GuardianFlags.GoldenShowerStance))
                            {
                                ArmOrientation = 2;
                                HeldItemHand = HeldHand.Both;
                            }
                        }
                        if (HeldItemHand == HeldHand.Both)
                        {
                            GetBetweenHandsPosition(Frame, out ItemPositionX, out ItemPositionY);
                        }
                        else if (HeldItemHand == HeldHand.Left)
                        {
                            GetLeftHandPosition(Frame, out ItemPositionX, out ItemPositionY);
                        }
                        else if (HeldItemHand == HeldHand.Right)
                        {
                            GetRightHandPosition(Frame, out ItemPositionX, out ItemPositionY);
                        }
                        if (WeaponIsBlowpipe) //Fix this...
                        {
                            //ItemPositionX = (int)((SpriteWidth + 8) * 0.5f);
                            //ItemPositionY = (int)(SpriteHeight * 0.25f) - SpriteHeight;
                        }
                        //AngleChecker = MathHelper.WrapAngle((float)Math.Atan2(ItemPositionY + Position.X - AimDirectionChange.Y, ItemPositionX + Position.Y - AimDirectionChange.X));
                        ItemRotation = AngleChecker;
                    }
                }
                else if (ItemUseType == ItemUseTypes.ItemDrink2h)
                {
                    byte AnimationFrame = 2;
                    HeldItemHand = HeldHand.Both;
                    if (AnimationPercentage < 0.05f)
                    {
                        AnimationFrame = 2;
                    }
                    else if (AnimationPercentage < 0.1f)
                    {
                        AnimationFrame = 1;
                    }
                    else if (AnimationPercentage < 0.9f)
                    {
                        AnimationFrame = 0;
                    }
                    else if (AnimationPercentage < 0.95f)
                    {
                        AnimationFrame = 1;
                    }
                    else
                    {
                        AnimationFrame = 2;
                    }
                    switch (AnimationFrame)
                    {
                        case 2:
                            ItemRotation = MathHelper.ToRadians(15);
                            break;
                        case 1:
                            ItemRotation = -MathHelper.ToRadians(70);
                            break;
                        case 0:
                            ItemRotation = -MathHelper.ToRadians(135);
                            break;
                    }
                    if (Ducking)
                    {
                        if (AnimationFrame == 0)
                        {
                            GetBetweenHandsPosition(Base.DuckingSwingFrames[1], out ItemPositionX, out ItemPositionY);
                        }
                        else
                        {
                            GetBetweenHandsPosition(Base.DuckingSwingFrames[2], out ItemPositionX, out ItemPositionY);
                        }
                    }
                    else
                    {
                        if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                        {
                            GetBetweenHandsPosition(Base.SittingItemUseFrames[AnimationFrame], out ItemPositionX, out ItemPositionY);
                        }
                        else
                        {
                            GetBetweenHandsPosition(Base.ItemUseFrames[AnimationFrame + 1], out ItemPositionX, out ItemPositionY);
                        }
                    }
                    if (LookingLeft)
                    {
                        //ItemPositionX = SpriteWidth - ItemPositionX;
                        ItemRotation *= -1f;
                    }
                    if (GravityDirection < 0)
                        ItemRotation *= -1;
                    /*ItemPositionX -= (int)(SpriteWidth * 0.5f);
                    ItemPositionY -= SpriteHeight;*/
                    if (SelectedItem > -1 && ItemAnimationTime < ItemMaxAnimationTime && (ItemMaxAnimationTime - ItemAnimationTime) % 50 == 0)
                        Main.PlaySound(Inventory[SelectedItem].UseSound, CenterPosition);
                }
                else if (ItemUseType == ItemUseTypes.OverHeadItemUse)
                {
                    ItemPositionX = 60;
                    ItemPositionY = 22;
                    //HeldItemHand = PrefferedHand;
                    ItemRotation = 0f;
                    if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                    {
                        if (HeldItemHand == HeldHand.Left)
                        {
                            GetLeftHandPosition(Base.SittingItemUseFrames[0], out ItemPositionX, out ItemPositionY);
                            //Base.LeftHandPoints.GetPositionFromFrame(16, out ItemPositionX, out ItemPositionY);
                        }
                        else
                        {
                            GetRightHandPosition(Base.SittingItemUseFrames[0], out ItemPositionX, out ItemPositionY);
                            //Base.RightHandPoints.GetPositionFromFrame(16, out ItemPositionX, out ItemPositionY);
                        }
                    }
                    else if (Ducking)
                    {
                        if (HeldItemHand == HeldHand.Left)
                        {
                            GetLeftHandPosition(Base.DuckingSwingFrames[1], out ItemPositionX, out ItemPositionY);
                            //Base.LeftHandPoints.GetPositionFromFrame(16, out ItemPositionX, out ItemPositionY);
                        }
                        else
                        {
                            GetRightHandPosition(Base.DuckingSwingFrames[1], out ItemPositionX, out ItemPositionY);
                            //Base.RightHandPoints.GetPositionFromFrame(16, out ItemPositionX, out ItemPositionY);
                        }
                    }
                    else
                    {
                        if (HeldItemHand == HeldHand.Left)
                        {
                            GetLeftHandPosition(Base.ItemUseFrames[1], out ItemPositionX, out ItemPositionY);
                            //Base.LeftHandPoints.GetPositionFromFrame(16, out ItemPositionX, out ItemPositionY);
                        }
                        else
                        {
                            GetRightHandPosition(Base.ItemUseFrames[1], out ItemPositionX, out ItemPositionY);
                            //Base.RightHandPoints.GetPositionFromFrame(16, out ItemPositionX, out ItemPositionY);
                        }
                    }
                    /*if (LookingLeft)
                        ItemPositionX = SpriteWidth - ItemPositionX;
                    ItemPositionX -= (int)(SpriteWidth * 0.5f);
                    ItemPositionY -= SpriteHeight;*/
                }
                //Collision Checker
                /*
                if (GravityDirection < 0)
                {
                    weaponCollision.Y -= weaponCollision.Height;
                }*/
                if (GravityDirection < 0)
                {
                    //WeaponCollision.Y = (int)(Position.Y + Position.Y - WeaponCollision.Y); //???
                }
                if (SelectedItem == -1 || (Inventory[SelectedItem].melee && !Inventory[SelectedItem].noMelee))
                {
                    for (int t = 0; t < 255; t++)
                    {
                        if (Main.player[t].active && !Main.player[t].dead && IsPlayerHostile(Main.player[t]))
                        {
                            if (Main.netMode == 0 || (Main.netMode == 1 && (OwnerPos == Main.myPlayer || t == Main.myPlayer)))
                            {
                                if (!PlayerHasBeenHit(t) && Main.player[t].getRect().Intersects(WeaponCollision) && CanHit(Main.player[t].position, Main.player[t].width, Main.player[t].height))
                                {
                                    int HitDirection = Direction;
                                    if ((HitDirection == -1 && Position.X < Main.player[t].Center.X) ||
                                        (HitDirection == 1 && Position.X > Main.player[t].Center.X))
                                    {
                                        HitDirection *= -1;
                                    }
                                    bool Critical = (Main.rand.Next(100) < CriticalRate);
                                    double resultingDamage = Main.player[t].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[t].name + " has been slain by " + Name + "."), Damage, HitDirection, (OwnerPos > -1 && Main.player[OwnerPos].hostile), false, Critical);
                                    IncreaseDamageStacker((int)resultingDamage, Main.player[t].statLifeMax2);
                                    if (resultingDamage > 0 && SelectedItem > -1)
                                        TryApplyingWeaponDebuffsToPlayer(Inventory[SelectedItem], Main.player[t]);
                                    if (resultingDamage > 0)
                                    {
                                        OnHitSomething(Main.player[t]);
                                        AddSkillProgress((float)resultingDamage, GuardianSkills.SkillTypes.Strength); //(float)resultingDamage
                                        if (Critical)
                                            AddSkillProgress((float)resultingDamage, GuardianSkills.SkillTypes.Luck); //(float)resultingDamage
                                    }
                                }
                            }
                        }
                        if (Main.netMode >= 1 && this.OwnerPos != Main.myPlayer)
                            continue;
                        if (t < 200 && Main.npc[t].active && !Main.npc[t].friendly && !NpcHasBeenHit(t) && Main.npc[t].getRect().Intersects(WeaponCollision) && (Main.npc[t].noTileCollide || CanHit(Main.npc[t].position, Main.npc[t].width, Main.npc[t].height)))
                        {
                            if (!Main.npc[t].dontTakeDamage)
                            {
                                int HitDirection = Direction;
                                if ((HitDirection == -1 && Position.X < Main.npc[t].Center.X) ||
                                    (HitDirection == 1 && Position.X > Main.npc[t].Center.X))
                                {
                                    HitDirection *= -1;
                                }
                                bool Critical = (Main.rand.Next(100) < CriticalRate);
                                int NewDamage = Damage;
                                if (OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
                                {
                                    float DamageMult = Main.player[OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                                    NewDamage = (int)(NewDamage * DamageMult);
                                }
                                if (GuardianBountyQuest.TargetMonsterID == t)
                                {
                                    byte AttackType = 0;
                                    if (Inventory[SelectedItem].ranged)
                                        AttackType = 1;
                                    if (Inventory[SelectedItem].magic)
                                        AttackType = 2;
                                    if (Inventory[SelectedItem].summon)
                                        AttackType = 3;
                                    GuardianBountyQuest.ModifyBountyMonsterHitByGuardianAttack(this, AttackType, ref NewDamage, ref Knockback, ref Critical);
                                }
                                double result = Main.npc[t].StrikeNPC(NewDamage, Knockback, HitDirection, Critical);
                                Main.PlaySound(Main.npc[t].HitSound, Main.npc[t].Center);
                                AddNpcHit(t);
                                IncreaseDamageStacker((int)result, Main.npc[t].lifeMax);
                                if (result > 0 && SelectedItem > -1)
                                    TryApplyingWeaponDebuffsToNpc(Inventory[SelectedItem], Main.npc[t]);
                                if (result > 0)
                                {
                                    if (HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    OnHitSomething(Main.npc[t]);
                                    AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (Critical)
                                        AddSkillProgress((float)result, GuardianSkills.SkillTypes.Luck); //(float)result
                                }
                            }
                            else
                            {
                                if (Main.npc[t].type == 63 || Main.npc[t].type == 64 || Main.npc[t].type == 103 || Main.npc[t].type == 242)
                                {
                                    AddNpcHit(t);
                                    this.Hurt((int)(Main.npc[t].damage * 1.3f), -this.Direction, false, false, " couldn't endure " + Main.npc[t].GivenOrTypeName + " electricity.");
                                }
                            }
                        }
                    }
                    foreach(TerraGuardian tg in MainMod.ActiveGuardians.Values)
                    {
                        if(IsGuardianHostile(tg) && tg.HitBox.Intersects(WeaponCollision) && CanHit(tg.TopLeftPosition, tg.Width, tg.Height))
                        {
                            int HitDirection = Direction;
                            if ((HitDirection == -1 && Position.X < tg.Position.X) ||
                                (HitDirection == 1 && Position.X > tg.Position.X))
                            {
                                HitDirection *= -1;
                            }
                            bool Critical = (Main.rand.Next(100) < CriticalRate);
                            int NewDamage = Damage;
                            if (OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
                            {
                                float DamageMult = Main.player[OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                                NewDamage = (int)(NewDamage * DamageMult);
                            }
                            int result = tg.Hurt(NewDamage, HitDirection, Critical, false, " was slain by "+Name + ".", true);
                            IncreaseDamageStacker(result, tg.MHP);
                            if(result > 0)
                            {
                                TryApplyingDebuffsToGuardian(Inventory[SelectedItem], tg);
                                if (HasFlag(GuardianFlags.BeetleOffenseEffect))
                                    IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                //OnHitSomething(Main.npc[t]);
                                AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                if (Critical)
                                    AddSkillProgress((float)result, GuardianSkills.SkillTypes.Luck); //(float)result
                            }
                        }
                    }
                }
                if (TriggerItem && (Main.netMode == 0 || OwnerPos == Main.myPlayer) && SelectedItem > -1 && (Inventory[SelectedItem].useAmmo != 0 || Inventory[SelectedItem].shoot != 0))
                {
                    bool DoShoot = true;
                    if (Inventory[SelectedItem].type == Terraria.ID.ItemID.ClockworkAssaultRifle && ItemAnimationTime <= Inventory[SelectedItem].useAnimation - Inventory[SelectedItem].useTime * 3)
                    {
                        DoShoot = false;
                    }
                    if (DoShoot) ItemShotScript(ItemWidth, ItemHeight, ItemOrigin, ref SelectedItem, ref ItemPositionX, ref ItemPositionY, ref ItemRotation);
                }
                if (ToolTrigger)
                {
                    if (SelectedItem > -1 && ItemAnimationTime > 0)
                    {
                        Item item = this.Inventory[SelectedItem];
                        int CenterX = (int)(Position.X * DivisionBy16), CenterY = (int)(this.CenterY * DivisionBy16);
                        int TileTargetX = (int)(AimPosition.X * DivisionBy16), TileTargetY = (int)(AimPosition.Y * DivisionBy16);
                        if (this.Inventory[SelectedItem].pick > 0 || this.Inventory[SelectedItem].axe > 0 || this.Inventory[SelectedItem].hammer > 0)
                        {
                            int ToolUseDistance = (int)((5 + Inventory[SelectedItem].tileBoost) * Scale);
                            if (Math.Abs(CenterX - TileTargetX) < ToolUseDistance && Math.Abs(CenterY - TileTargetY) < ToolUseDistance) //Can trigger tool effect
                            {
                                Tile tile = MainMod.GetTile(TileTargetX, TileTargetY);
                                if (Action && tile != null)
                                {
                                    bool HitWall = true;
                                    if (tile.active())
                                    {
                                        if ((item.pick > 0 && !Main.tileAxe[tile.type] && !Main.tileHammer[tile.type]) || (item.axe > 0 && Main.tileAxe[tile.type]) || (item.hammer > 0 && Main.tileHammer[tile.type]))
                                            HitWall = false;
                                        int TileDamage = 0;
                                        int tileID = this.hitTileData.HitObject(TileTargetX, TileTargetY, 1);
                                        if (Main.tileNoFail[tile.type])
                                        {
                                            TileDamage = 100;
                                        }
                                        bool CorrectTool = false;
                                        if (Main.tileHammer[tile.type])
                                        {
                                            if (item.hammer > 0)
                                            {
                                                if (WorldGen.CanKillTile(TileTargetX, TileTargetY))
                                                    TileDamage += item.hammer;
                                                else
                                                    TileDamage = 0;
                                                if (tile.type == 26 && (!Main.hardMode || item.hammer < 80))
                                                {
                                                    TileDamage = 0;
                                                    this.Hurt(this.HP / 2, -Direction, false, false, " couldn't break the altar.");
                                                }
                                            }
                                        }
                                        else if (Main.tileAxe[tile.type])
                                        {
                                            if (item.axe > 0)
                                            {
                                                if (tile.type == 80)
                                                    TileDamage += item.axe * 3;
                                                else
                                                    TileDamage += item.axe;
                                            }
                                        }
                                        else
                                        {
                                            if (item.pick > 0)
                                            {
                                                int pickPower = item.pick;
                                                if (Main.tileNoFail[tile.type])
                                                    TileDamage += 100;
                                                else if (Main.tileDungeon[tile.type] || tile.type == 25 || tile.type == 58 || tile.type == 117 || tile.type == 203 ||
                                                    tile.type == 107 || tile.type == 221)
                                                {
                                                    TileDamage += pickPower / 2;
                                                }
                                                else if (tile.type == 108 || tile.type == 222)
                                                {
                                                    TileDamage += pickPower / 3;
                                                }
                                                else if (tile.type == 48 || tile.type == 232 || tile.type == 226 || tile.type == 111 || tile.type == 223)
                                                {
                                                    TileDamage += pickPower / 4;
                                                }
                                                else if (tile.type == 211)
                                                {
                                                    TileDamage += pickPower / 5;
                                                }
                                                else
                                                {
                                                    TileDamage += pickPower;
                                                }
                                                if ((tile.type == 221 && pickPower < 200) ||
                                                    ((tile.type == 25 || tile.type == 203) && pickPower < 65) ||
                                                    (tile.type == 117 && pickPower < 65) ||
                                                    (tile.type == 37 && pickPower < 50) ||
                                                    (tile.type == 404 && pickPower < 65) ||
                                                    ((tile.type == 22 || tile.type == 204) && TileTargetY > Main.worldSurface && pickPower < 55) ||
                                                    (tile.type == 56 && pickPower < 65) ||
                                                    (tile.type == 58 && pickPower < 65) ||
                                                    ((tile.type == 226 || tile.type == 237) && pickPower < 210) ||
                                                    ((Main.tileDungeon[tile.type] && pickPower < 65) && (TileTargetX < Main.maxTilesX * 0.35 || TileTargetX > Main.maxTilesX * 0.65)) ||
                                                    (tile.type == 107 && pickPower < 100) ||
                                                    (tile.type == 108 && pickPower < 110) ||
                                                    (tile.type == 111 && pickPower < 150) ||
                                                    (tile.type == 221 && pickPower < 100) ||
                                                    (tile.type == 222 && pickPower < 110) ||
                                                    (tile.type == 223 && pickPower < 150))
                                                {
                                                    TileDamage = 0;
                                                }
                                                if (tile.type == 147 || tile.type == 0 || tile.type == 40 || tile.type == 53 || tile.type == 57 || tile.type == 59 || tile.type == 123 ||
                                                    tile.type == 224 || tile.type == 397)
                                                    TileDamage += pickPower;
                                                if (tile.type == 165 || Main.tileRope[tile.type] || tile.type == 199 || Main.tileMoss[tile.type])
                                                    TileDamage += 100;
                                                if (tile.type == 2 || tile.type == 23 || tile.type == 60 || tile.type == 70 || tile.type == 109 || tile.type == 199 || Main.tileMoss[tile.type])
                                                    CorrectTool = true;

                                            }
                                        }
                                        if (!WorldGen.CanKillTile(TileTargetX, TileTargetY))
                                            TileDamage = 0;
                                        if (hitTileData.AddDamage(tileID, TileDamage, true) >= 100)
                                        {
                                            if (CorrectTool)
                                            {
                                                WorldGen.KillTile(TileTargetX, TileTargetY, true, false, false);
                                                hitTileData.Clear(tileID);
                                            }
                                            else
                                            {
                                                hitTileData.Clear(tileID);
                                                WorldGen.KillTile(TileTargetX, TileTargetY, false, false, false);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(17, -1, -1, null, TileTargetX, TileTargetY);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WorldGen.KillTile(TileTargetX, TileTargetY, true, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(17, -1, -1, null, TileTargetX, TileTargetY, 1);
                                            }
                                        }
                                        if (TileDamage != 0)
                                        {
                                            this.hitTileData.Prune();
                                        }
                                    }
                                    if (HitWall && tile.wall > 0 && !tile.active() && item.hammer > 0)
                                    {
                                        bool DamageTile = true;
                                        if (!Main.wallHouse[tile.wall])
                                        {
                                            DamageTile = false;
                                            for (int x = TileTargetX - 1; x < TileTargetX + 2; x++)
                                            {
                                                for (int y = TileTargetY - 1; y < TileTargetY + 2; y++)
                                                {
                                                    DamageTile = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (DamageTile)
                                        {
                                            int tileid = hitTileData.HitObject(TileTargetX, TileTargetY, 2);
                                            int Damage = (int)(item.hammer * 1.5f);
                                            if (this.hitTileData.AddDamage(tileid, Damage, true) >= 100)
                                            {
                                                this.hitTileData.Clear(tileid);
                                                WorldGen.KillWall(TileTargetX, TileTargetY, false);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(17, -1, -1, null, 2, TileTargetX, TileTargetY);
                                                }
                                            }
                                            else
                                            {
                                                WorldGen.KillWall(TileTargetX, TileTargetY, true);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(17, -1, -1, null, 2, TileTargetX, TileTargetY, 1);
                                                }
                                            }
                                            if (Damage > 0)
                                                hitTileData.Prune();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Time reduce
                if (MainHand) ItemAnimationTime--;
                if (IsDelay)
                {
                    if (ItemAnimationTime == 0)
                    {
                        IsDelay = false;
                        FreezeItemUseAnimation = true;
                    }
                }
                else if (ItemAnimationTime == 0) //End use item script
                {
                    FreezeItemUseAnimation = true;
                    bool Failed = false, ForceUse = false;
                    if (SelectedItem > -1 && (Main.netMode == 0 || OwnerPos == Main.myPlayer))
                    {
                        Item item = this.Inventory[SelectedItem];
                        if (MainMod.IsGuardianItem(item))
                        {
                            Items.GuardianItemPrefab ip = (Items.GuardianItemPrefab)item.modItem;
                            ip.WhenGuardianUses(this);
                        }
                        if (item.healLife > 0)
                        {
                            int HealthChangeValue = (int)(item.healLife * HealthHealMult);
                            if (HealthChangeValue > 0)
                                RestoreHP(HealthChangeValue);
                            else if (HealthChangeValue < 0)
                                Hurt(HealthChangeValue, Direction * -1, false, false, " drank something harmful.");
                            int HealingPotionCooldown = MaxHealingPotionCooldown;
                            if (HasFlag(GuardianFlags.ReduceHealingPotionCooldown))
                                HealingPotionCooldown = (int)(HealingPotionCooldown * 0.75f);
                            AddCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown, HealingPotionCooldown);
                            AddBuff(21, HealingPotionCooldown);
                        }
                        if (item.healMana > 0)
                        {
                            int ManaChangeValue = (int)(item.healMana * ManaHealMult);
                            RestoreMP(ManaChangeValue);
                        }
                        byte MaxUseableStacker = (byte)(5 + FriendshipLevel * 0.2f);
                        if (item.buffType == Terraria.ID.BuffID.WellFed && !HasBuff(Terraria.ID.BuffID.WellFed))
                        {
                            FoodStacker++;
                            if (FoodStacker >= MaxUseableStacker)
                            {
                                FoodStacker -= MaxUseableStacker;
                                IncreaseFriendshipProgress(1);
                            }
                        }
                        if (item.buffType == Terraria.ID.BuffID.Tipsy && !HasBuff(Terraria.ID.BuffID.Tipsy))
                        {
                            DrinkStacker++;
                            if (DrinkStacker >= 40)
                            {
                                DrinkStacker -= 40;
                                IncreaseFriendshipProgress(1);
                            }
                        }
                        if (false && item.mountType > -1 && GravityDirection == 1)
                        {
                            RideMount(item.mountType);
                        }
                        if (item.buffType != 0 && item.buffTime > 0)
                        {
                            AddBuff(item.buffType, item.buffTime);
                        }
                        if ((item.type == ModContent.ItemType<Items.Consumable.EtherHeart>() || item.type == ModContent.ItemType<Items.Consumable.EtherFruit>()) &&
                            !Base.IsTerraGuardian)
                        {
                            Failed = true;
                            Main.NewText("This companion can't use this item.");
                        }
                        else
                        {
                            if (item.type == Terraria.ID.ItemID.LifeCrystal || item.type == ModContent.ItemType<Items.Consumable.EtherHeart>())
                            {
                                if (GetUsedLifeCrystal < MaxLifeCrystals)
                                {
                                    ForceUse = true;
                                    GetUsedLifeCrystal++;
                                    UpdateStatus = true;
                                }
                                else Failed = true;
                            }
                            if (item.type == Terraria.ID.ItemID.LifeFruit || item.type == ModContent.ItemType<Items.Consumable.EtherFruit>())
                            {
                                if (GetUsedLifeCrystal == MaxLifeCrystals && GetUsedLifeFruit < MaxLifeFruit)
                                {
                                    ForceUse = true;
                                    GetUsedLifeFruit++;
                                    UpdateStatus = true;
                                }
                                else Failed = true;
                            }
                        }
                        if (item.type == Terraria.ID.ItemID.ManaCrystal)
                        {
                            if (GetUsedManaCrystal < GuardianData.MaxManaCrystals)
                            {
                                ForceUse = true;
                                GetUsedManaCrystal++;
                                UpdateStatus = true;
                            }
                            else Failed = true;
                        }
                        if (item.createTile >= 0)
                        {
                            int TileX = (int)(AimPosition.X / 16);
                            int TileY = (int)(AimPosition.Y / 16);
                            int tileType = item.createTile;
                            int tileStyle = item.placeStyle;
                            if (item.createTile == 20)
                            {
                                Tile underTile = Main.tile[TileX, TileY + 1];
                                if (underTile.active())
                                    TileLoader.SaplingGrowthType(underTile.type, ref tileType, ref tileStyle);
                            }
                            if (TileObject.CanPlace(TileX, TileY, tileType, tileStyle, Direction, out TileObject to))
                                TileObject.Place(to);
                        }
                        if(item.createWall > 0)
                        {
                            int TileX = (int)(AimPosition.X / 16);
                            int TileY = (int)(AimPosition.Y / 16);
                            WorldGen.PlaceWall(TileX, TileY, item.createTile);
                        }
                        if (!Failed && item.consumable && (OwnerPos > -1 || ForceUse))
                        {
                            bool IsWeapon = item.damage > 0;
                            if (IsWeapon && item.stack == 2)
                            {
                                for (int s = SelectedItem + 1; s < 50; s++)
                                {
                                    if (Inventory[s].type == item.type)
                                    {
                                        int StackToChange = item.maxStack - item.stack;
                                        if (StackToChange > Inventory[s].stack)
                                            StackToChange = Inventory[s].stack;
                                        item.stack += StackToChange;
                                        Inventory[s].stack -= StackToChange;
                                    }
                                }
                            }
                            if (!IsWeapon || item.stack > 1)
                            {
                                item.stack--;
                            }
                            if (item.stack <= 0)
                                item.SetDefaults(0, true);
                        }
                        if (item.reuseDelay > 0)
                        {
                            IsDelay = true;
                            ItemAnimationTime = item.reuseDelay;
                            ItemUseTime = item.reuseDelay;
                            FreezeItemUseAnimation = false;
                        }
                    }
                }
                else
                {
                    if (SelectedItem > -1 && (this.Inventory[SelectedItem].type == 50 || this.Inventory[SelectedItem].type == 3124 || this.Inventory[SelectedItem].type == 3199))
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Dust.NewDust(TopLeftPosition, Width, Height, 15, 0, 0, 150, default(Color), 1.1f);
                        }
                        if (ItemUseTime == 0)
                            ItemUseTime = Inventory[SelectedItem].useTime;
                        else if (ItemUseTime == Inventory[SelectedItem].useTime / 2)
                        {
                            for (int i = 0; i < 70; i++)
                            {
                                Dust.NewDust(TopLeftPosition, Width, Height, 15, Velocity.X * 0.5f, Velocity.Y * 0.5f, 150, default(Color), 1.5f);
                            }
                            MagicMirrorTrigger = true;
                            if (!DoAction.InUse || !DoAction.EffectOnlyMirror)
                            {
                                Spawn();
                                for (int i = 0; i < 70; i++)
                                {
                                    Dust.NewDust(TopLeftPosition, Width, Height, 15, Velocity.X * 0.5f, Velocity.Y * 0.5f, 150, default(Color), 1.5f);
                                }
                            }
                        }
                    }
                }
            }
            if (MainHand)
            {
                if (ItemUseTime > 0)
                    ItemUseTime--;
                if (ToolUseTime > 0)
                    ToolUseTime--;
            }
        }

        public void ItemShotScript(int ItemWidth, int ItemHeight, Vector2 ItemOrigin, ref int SelectedItem, ref int ItemPositionX, ref int ItemPositionY, ref float ItemRotation)
        {
            bool MayShot = true;
            Item i = this.Inventory[SelectedItem];
            int ProjID = 0;
            float ProjSpeed = 0, ProjKnockback = 0;
            int Damage = i.damage;
            int ShotCount = 1, ShotSpread = 0;
            bool DontSpawnOnSamePosition = false;
            float SpeedBonusPerShot = 0f;
            GetAmmoInfo(i, true, out ProjID, out ProjSpeed, out Damage, out ProjKnockback);
            Damage = (int)(GetDamageMultipliersFromItem(i) * i.damage);
            //
            Vector2 AimDirection = this.AimPosition;
            Vector2 ShotSpawnPosition = Vector2.Zero;
            if (MainMod.IsGuardianItem(Inventory[SelectedItem]))
                ShotSpawnPosition = (Inventory[SelectedItem].modItem as Items.GuardianItemPrefab).ShotSpawnPosition.ToVector2() * ItemScale;
            if (ItemUseType != ItemUseTypes.AimingUse)
            {
                ShotSpawnPosition.Y -= Height * 0.5f;
                ShotSpawnPosition.X += Position.X;
                ShotSpawnPosition.Y += Position.Y;
            }
            else
            {
                ShotSpawnPosition = ItemOrigin - ShotSpawnPosition; //Test with inverted direction
                ShotSpawnPosition = ItemRotation.ToRotationVector2() * ShotSpawnPosition;
                ShotSpawnPosition.X += ItemPositionX + Position.X;
                ShotSpawnPosition.Y += ItemPositionY + Position.Y;
            }
            ShotSpawnPosition.Y += Base.CharacterPositionYDiscount * Scale;
            // Specific Item Scripts.
            if (i.type == 3094 || i.type == 3378 || i.type == 3543) //Javelins
            {
                ShotSpawnPosition.X = Position.X;
                ShotSpawnPosition.Y = Position.Y - Height * 0.66f;
            }
            if (ProjID == 9)
            {
                ShotSpawnPosition.X = Position.X + Main.rand.Next(201) * -Direction + AimDirection.X - Position.X;
                ShotSpawnPosition.Y = CenterY - 600f;
                Damage *= 2;
                ProjKnockback = 0;
            }
            if (i.type == 986 || i.type == 281) //Blowgun and Blowpipe
            {
                ShotSpawnPosition.X += 6 * Direction;
                ShotSpawnPosition.Y -= 6 * GravityDirection;
            }
            if (i.type == 3007) //Dart pistol
            {
                //Also changes shot spawn position
                ShotSpawnPosition.X -= 4 * Direction;
                ShotSpawnPosition.Y -= 1 * GravityDirection;
            }
            if (i.type == 757) //Terra Blade
            {
                Damage = (int)(Damage * 1.25f);
            }
            if (ProjID == 250)
            {
                for (int num85 = 0; num85 < 1000; num85++)
                {
                    if (Main.projectile[num85].active && Main.projectile[num85].owner == GetSomeoneToSpawnProjectileFor && (Main.projectile[num85].type == 250 || Main.projectile[num85].type == 251) && ProjMod.IsGuardianProjectile(num85) && ProjMod.GuardianProj[num85] == this)
                    {
                        Main.projectile[num85].Kill();
                    }
                }
            }
            //
            float ShotDirection = (float)Main.rand.NextDouble() * 6.283185307179586f;
            float DistanceAccuracy = 6 * ((CenterPosition - AimDirection).Length() * 0.0625f);
            if (PlayerControl)
                DistanceAccuracy = 0;
            else
                DistanceAccuracy -= Accuracy * DistanceAccuracy;
            Vector2 AimDirectionChange = AimDirection + ShotDirection.ToRotationVector2() * DistanceAccuracy; //(ItemUseType == ItemUseTypes.AimingUse ? ShotSpawnPosition - ItemRotation.ToRotationVector2() * 6 : AimDirection + ShotDirection.ToRotationVector2() * (DistanceAccuracy - Accuracy * DistanceAccuracy));
            //ItemRotation = AimDirection.ToRotation();
            Vector2 MovementDirection = AimDirectionChange - ShotSpawnPosition;
            //float AngleChecker = MathHelper.WrapAngle((float)Math.Atan2((CenterPosition.Y - AimDirectionChange.Y) * GravityDirection, CenterPosition.X - AimDirectionChange.X));
            ItemRotation = ((float)Math.Atan2((CenterY - AimDirectionChange.Y) * GravityDirection, Position.X - AimDirectionChange.X));
            if (i.type == Terraria.ID.ItemID.Boomstick)
            {
                ShotCount = Main.rand.Next(3, 5);
                ShotSpread = 20;
            }
            if (i.type == Terraria.ID.ItemID.Shotgun)
            {
                ShotCount = Main.rand.Next(3, 6);
                ShotSpread = 20;
            }
            if (i.type == Terraria.ID.ItemID.TacticalShotgun)
            {
                ShotCount = 6;
                ShotSpread = 20;
            }
            if (i.type == Terraria.ID.ItemID.OnyxBlaster)
            {
                ShotCount = 4;
                ShotSpread = 20;
                Vector2 NewDir = MovementDirection;
                NewDir.Normalize();
                CreateProjectile(ShotSpawnPosition, NewDir * ProjSpeed * 1.3f, 661, Damage * 2, ProjKnockback);
            }
            if (i.type == Terraria.ID.ItemID.PoisonStaff)
            {
                ShotCount = Main.rand.Next(3, 5);
                ShotSpread = 10;
            }
            if (i.type == Terraria.ID.ItemID.VenomStaff)
            {
                ShotCount = Main.rand.Next(4, 8);
                ShotSpread = 10;
            }
            if (i.type == Terraria.ID.ItemID.ChlorophyteShotbow)
            {
                ShotCount = Main.rand.Next(2, 5);
                ShotSpread = 5;
            }
            if (i.type == Terraria.ID.ItemID.Phantasm)
            {
                ShotCount = 4;
                DontSpawnOnSamePosition = true;
            }
            if (i.type == Terraria.ID.ItemID.VortexBeater)
            {
                ShotSpread = 25;
            }
            if (i.type == 3870)
            {
                ShotSpread = 25;
                ShotCount = 3;
            }
            if (i.type == Terraria.ID.ItemID.BatScepter)
            {
                ShotSpread = 5;
                ShotCount = Main.rand.Next(1, 4);
            }
            if (i.type == Terraria.ID.ItemID.LeafBlower)
            {
                ShotSpread = 3;
            }
            if (i.type == Terraria.ID.ItemID.CrystalStorm)
            {
                ShotSpread = 5;
            }
            if (i.type == Terraria.ID.ItemID.Xenopopper)
            {
                ShotCount = 4;
                if (Main.rand.Next(4) == 0)
                    ShotCount++;
                ShotSpread = 25;
            }
            if (i.type == Terraria.ID.ItemID.Tsunami)
            {
                ShotCount = 5;
                DontSpawnOnSamePosition = true;
            }
            if (i.type == 1929 || i.type == 2270)
            {
                MovementDirection.X += Main.rand.Next(-50, 51) * 0.03f;
                MovementDirection.Y += Main.rand.Next(-50, 51) * 0.03f;
            }
            /*if (ProjID == 12)
            {
                ShotSpawnPosition.X += 3 * MovementDirection.X;
                ShotSpawnPosition.Y += 3 * MovementDirection.Y;
            }*/
            if (i.type == 3029)
            {
                ItemRotation = (float)Math.Atan2((AimDirection.Y - 1000f) * Direction, AimDirectionChange.X * -Direction);
            }
            if (i.type == 3779)
                ItemRotation = 0;
            if (ProjID == 76)
            {
                MayShot = false;
                ProjID += Main.rand.Next(3);
                MovementDirection.X += Main.rand.Next(-40, 41) * 0.01f;
                MovementDirection.Y += Main.rand.Next(-40, 41) * 0.01f;
                MovementDirection.Normalize();
                MovementDirection += MovementDirection * 0.25f;
                Projectile proj = CreateProjectile(ShotSpawnPosition, MovementDirection * ProjSpeed, ProjID, Damage, ProjKnockback);
                proj.ai[1] = 1f;
                if (MovementDirection.X > 0)
                    proj.ai[0] = 1f;
                else
                    proj.ai[0] = -1f;
            }
            else if (i.type == 3029)
            {
                MayShot = false;
                int Arrows = 3;
                if (Main.rand.NextDouble() < 0.333)
                    Arrows++;
                for (int shot = 0; shot < Arrows; shot++)
                {
                    Vector2 ProjSpawnPos = new Vector2(Main.rand.Next(201) * -Direction + AimDirection.X, CenterY - 600);
                    ProjSpawnPos.X = (ProjSpawnPos.X * 10 + Position.X) / 11f + Main.rand.Next(-100, 101);
                    ProjSpawnPos.Y -= 150 * shot;
                    float projvelx = AimDirection.X - ProjSpawnPos.X, projvely = AimDirection.Y - ProjSpawnPos.Y;
                    if(projvely < 0)
                        projvely *= -1f;
                    if (projvely < 20)
                        projvely = 20;
                    float sqrt = (float)Math.Sqrt(projvelx * projvelx + projvely * projvely);
                    sqrt = ProjSpeed / sqrt;
                    projvelx *= sqrt;
                    projvely *= sqrt;
                    projvelx = projvelx + Main.rand.Next(-40, 41) * 0.03f;
                    projvely = projvely + Main.rand.Next(-40, 41) * 0.03f;
                    projvelx *= Main.rand.Next(75, 150) * 0.01f;
                    //Main.NewText(ProjSpawnPos.ToString());
                    Projectile proj = CreateProjectile(ProjSpawnPos, new Vector2(projvelx, projvely), ProjID, Damage, ProjKnockback);
                    proj.noDropItem = true;
                    /*ShotSpawnPosition.X = Main.rand.Next(201) * -Direction + AimDirection.X;
                    ShotSpawnPosition.Y = CenterPosition.Y - 600;
                    ShotSpawnPosition.X = (ShotSpawnPosition.X * 10 + Position.X) / 11f + Main.rand.Next(-100, 101);
                    ShotSpawnPosition.Y -= 150 * shot;
                    MovementDirection.X = AimDirection.X + Main.screenPosition.X - ShotSpawnPosition.X;
                    MovementDirection.Y = AimDirection.Y + Main.screenPosition.Y - ShotSpawnPosition.Y;
                    if (MovementDirection.Y < 0)
                        MovementDirection.Y *= -1;
                    if (MovementDirection.Y < 20)
                        MovementDirection.Y = 20;
                    MovementDirection.Normalize();
                    MovementDirection.X = MovementDirection.X + Main.rand.Next(-40, 41) * 0.03f;
                    MovementDirection.Y = MovementDirection.Y + Main.rand.Next(-40, 41) * 0.03f;
                    MovementDirection.X *= Main.rand.Next(75, 151) * 0.01f;
                    ShotSpawnPosition.X += Main.rand.Next(-50, 51);
                    Projectile proj = CreateProjectile(ShotSpawnPosition, MovementDirection * ProjSpeed, ProjID, Damage, ProjKnockback);
                    proj.noDropItem = true;*/
                }
            }
            else if (i.type == 98 || i.type == 533)
            {
                MovementDirection.X += Main.rand.Next(-40, 41) * 0.01f;
                MovementDirection.Y += Main.rand.Next(-40, 41) * 0.01f;
                MovementDirection.Normalize();
            }
            else if (i.type == 1319)
            {
                MovementDirection.X += Main.rand.Next(-40, 41) * 0.02f;
                MovementDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
                MovementDirection.Normalize();
                Projectile proj = CreateProjectile(ShotSpawnPosition, MovementDirection * ProjSpeed, ProjID, Damage, ProjKnockback);
                proj.ranged = true;
                proj.thrown = false;
            }
            else if (i.type == 3107)
            {
                MovementDirection.X += Main.rand.Next(-40, 41) * 0.02f;
                MovementDirection.Y += Main.rand.Next(-40, 41) * 0.02f;
            }
            else if (i.type == 3053)
            {
                MayShot = false;
                MovementDirection.Normalize();
                Vector2 ShotDirMod = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                ShotDirMod.Normalize();
                MovementDirection = MovementDirection * 4 + ShotDirMod;
                float ai0 = Main.rand.Next(10, 80) * 0.001f,
                    ai1 = Main.rand.Next(10, 80) * 0.001f;
                if (Main.rand.NextDouble() < 0.5)
                    ai0 *= -1;
                if (Main.rand.NextDouble() < 0.5)
                    ai1 *= -1;
                Projectile proj = CreateProjectile(ShotSpawnPosition, MovementDirection * ProjSpeed, ProjID, Damage, ProjKnockback);
                proj.ai[0] = ai0;
                proj.ai[1] = ai1;
            }
            else if (i.type == 3019) //Hellwing bow...
            {
                ShotSpread = 100;
                //For real?!
                //MovementDirection.X += Main.rand.Next(-100, 101) * 0.01f;
            }
            else if (i.type == 2797)
            {

            }
            if (i.type == 3006)
            {
                Vector2 ShotPosition = AimDirection;//CenterPosition;
                /*while (Collision.CanHitLine(this.TopLeftPosition, Width, Height, ShotPosition, 1, 1))
                {
                    ShotPosition.X += MovementDirection.X;
                    ShotPosition.Y += MovementDirection.Y;
                    if ((AimDirection - ShotPosition).Length() < 20f + Math.Abs(MovementDirection.X * ProjSpeed) + Math.Abs(MovementDirection.Y * ProjSpeed))
                    {
                        ShotPosition = AimDirection;
                        break;
                    }
                }*/
                ShotSpawnPosition = ShotPosition;
                CreateProjectile(ShotSpawnPosition, Vector2.Zero, ProjID, Damage, ProjKnockback);
                MayShot = false;
            }
            if (i.type == 3859)
            {
                ProjID = 710;
                ShotSpread = 6;
                Damage = (int)(Damage * 0.7f);
                SpeedBonusPerShot = 0.5f;
            }
            if(i.type == 2535)
            {
                Vector2 SpawnRotation = Vector2.Zero.RotatedBy(1.570796705062866);
                CreateProjectile(AimDirection + SpawnRotation, SpawnRotation, ProjID, Damage, ProjKnockback);
                SpawnRotation = Vector2.Zero.RotatedBy(-Math.PI);
                CreateProjectile(AimDirection + SpawnRotation, SpawnRotation, ProjID + 1, Damage, ProjKnockback);
                MayShot = false;
            }
            //GuardianItemUseEffects.ShotSpawnEffect(this, i, ProjID, Damage, ProjSpeed, ProjKnockback, ShotSpawnPosition, MovementDirection, out MayShot);
            if (MayShot)
            {
                for (int s = 0; s < ShotCount; s++)
                {
                    Vector2 ShotDir = MovementDirection;
                    if (ShotSpread > 0)
                        ShotDir += new Vector2(Main.rand.Next(-ShotSpread, ShotSpread + 1), Main.rand.Next(-ShotSpread, ShotSpread + 1));
                    else if (DontSpawnOnSamePosition && s > 0)
                    {
                        int distance = (s - 1) / 2;
                        float Direction = s % 2 == 0 ? -1 : 1;
                        ShotDir += (Direction + 1.570796326794897f).ToRotationVector2() * distance * 4f;
                    }
                    ShotDir.Normalize();
                    int ProjType = ProjID;
                    if (i.type == Terraria.ID.ItemID.Xenopopper)
                    {
                        ProjType = 444;
                    }
                    Projectile proj = CreateProjectile(ShotSpawnPosition, ShotDir * ProjSpeed, ProjType, Damage, ProjKnockback); //Simple Projectile
                    if (i.type == Terraria.ID.ItemID.Xenopopper)
                    {
                        proj.localAI[0] = ProjID;
                        proj.localAI[1] = ProjSpeed;
                    }
                    ProjSpeed += SpeedBonusPerShot;
                }
            }
        }

        private Projectile CreateProjectile(Vector2 Position, Vector2 Speed, int ID, int Damage, float Knockback)
        {
            int p = Projectile.NewProjectile(Position, Speed, ID, Damage, Knockback, GetSomeoneToSpawnProjectileFor);
            Main.projectile[p].noDropItem = true;
            Main.projectile[p].noEnchantments = true;
            //Main.projectile[p].friendly = true;
            //Main.projectile[p].hostile = false;
            if (OwnerPos > -1) Main.projectile[p].playerImmune[OwnerPos] = int.MaxValue;
            //Main.projectile[p].scale *= Scale;
            SetProjectileOwnership(p);
            return Main.projectile[p];
        }

        public void TorchLightingHandler(Item item, bool OffHand)
        {
            float Scale = Math.Max(1f, this.Scale);
            if ((((item.type == 974 || item.type == 8 || item.type == 1245 || item.type == 2274 || item.type == 3004 || item.type == 3045 || item.type == 3114 || (item.type >= 427 && item.type <= 433)) && !this.Wet) || item.type == 523 || item.type == 1333))
            {
                float num52 = 1f;
                float num53 = 0.95f;
                float num54 = 0.8f;
                int num55 = 0;
                if (item.type == 523)
                {
                    num55 = 8;
                }
                else if (item.type == 974)
                {
                    num55 = 9;
                }
                else if (item.type == 1245)
                {
                    num55 = 10;
                }
                else if (item.type == 1333)
                {
                    num55 = 11;
                }
                else if (item.type == 2274)
                {
                    num55 = 12;
                }
                else if (item.type == 3004)
                {
                    num55 = 13;
                }
                else if (item.type == 3045)
                {
                    num55 = 14;
                }
                else if (item.type == 3114)
                {
                    num55 = 15;
                }
                else if (item.type >= 427)
                {
                    num55 = item.type - 426;
                }
                if (num55 == 1)
                {
                    num52 = 0f;
                    num53 = 0.1f;
                    num54 = 1.3f;
                }
                else if (num55 == 2)
                {
                    num52 = 1f;
                    num53 = 0.1f;
                    num54 = 0.1f;
                }
                else if (num55 == 3)
                {
                    num52 = 0f;
                    num53 = 1f;
                    num54 = 0.1f;
                }
                else if (num55 == 4)
                {
                    num52 = 0.9f;
                    num53 = 0f;
                    num54 = 0.9f;
                }
                else if (num55 == 5)
                {
                    num52 = 1.3f;
                    num53 = 1.3f;
                    num54 = 1.3f;
                }
                else if (num55 == 6)
                {
                    num52 = 0.9f;
                    num53 = 0.9f;
                    num54 = 0f;
                }
                else if (num55 == 7)
                {
                    num52 = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
                    num53 = 0.3f;
                    num54 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
                }
                else if (num55 == 8)
                {
                    num54 = 0.7f;
                    num52 = 0.85f;
                    num53 = 1f;
                }
                else if (num55 == 9)
                {
                    num54 = 1f;
                    num52 = 0.7f;
                    num53 = 0.85f;
                }
                else if (num55 == 10)
                {
                    num54 = 0f;
                    num52 = 1f;
                    num53 = 0.5f;
                }
                else if (num55 == 11)
                {
                    num54 = 0.8f;
                    num52 = 1.25f;
                    num53 = 1.25f;
                }
                else if (num55 == 12)
                {
                    num52 *= 0.75f;
                    num53 *= 1.3499999f;
                    num54 *= 1.5f;
                }
                else if (num55 == 13)
                {
                    num52 = 0.95f;
                    num53 = 0.65f;
                    num54 = 1.3f;
                }
                else if (num55 == 14)
                {
                    num52 = (float)Main.DiscoR / 255f;
                    num53 = (float)Main.DiscoG / 255f;
                    num54 = (float)Main.DiscoB / 255f;
                }
                else if (num55 == 15)
                {
                    num52 = 1f;
                    num53 = 0f;
                    num54 = 1f;
                }
                int num56 = num55;
                if (num56 == 0)
                {
                    num56 = 6;
                }
                else if (num56 == 8)
                {
                    num56 = 75;
                }
                else if (num56 == 9)
                {
                    num56 = 135;
                }
                else if (num56 == 10)
                {
                    num56 = 158;
                }
                else if (num56 == 11)
                {
                    num56 = 169;
                }
                else if (num56 == 12)
                {
                    num56 = 156;
                }
                else if (num56 == 13)
                {
                    num56 = 234;
                }
                else if (num56 == 14)
                {
                    num56 = 66;
                }
                else if (num56 == 15)
                {
                    num56 = 242;
                }
                else
                {
                    num56 = 58 + num56;
                }
                int maxValue = 30;
                if (ItemAnimationTime > 0)
                {
                    maxValue = 7;
                }
                Vector2 ItemLocation = PositionWithOffset;
                ItemLocation.X += (OffHand ? OffHandPositionX : ItemPositionX);
                ItemLocation.Y += (OffHand ? OffHandPositionY : ItemPositionY);
                if (Main.rand.Next(maxValue) == 0)
                {
                    int num57 = Dust.NewDust(new Vector2(ItemLocation.X + 7f * Direction, ItemLocation.Y - 8f * GravityDirection), 4, 4, num56, 0f, 0f, 100, default(Color), 1f);
                    if (Main.rand.Next(3) != 0)
                    {
                        Main.dust[num57].noGravity = true;
                    }
                    Main.dust[num57].velocity *= 0.3f;
                    Dust expr_3671_cp_0 = Main.dust[num57];
                    expr_3671_cp_0.velocity.Y = expr_3671_cp_0.velocity.Y - 1.5f;
                    Main.dust[num57].position = Main.dust[num57].position;
                    if (num56 == 66)
                    {
                        Main.dust[num57].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                        Main.dust[num57].noGravity = true;
                    }
                }
                Vector2 position = new Vector2(ItemLocation.X + 6f * Direction + Velocity.X, ItemLocation.Y - 14f + Velocity.Y);
                Lighting.AddLight(position, num52 * Scale, num53 * Scale, num54 * Scale);
            }
            if (item.type == 282)
            {
                Vector2 position11 = new Vector2(PositionWithOffset.X + ItemPositionX + Velocity.X, PositionWithOffset.Y + ItemPositionY);
                Lighting.AddLight(position11, 0.7f * Scale, 1f * Scale, 0.8f * Scale);
            }
            if (item.type == 286)
            {
                Vector2 position11 = new Vector2(PositionWithOffset.X + ItemPositionX + Velocity.X, PositionWithOffset.Y + ItemPositionY);
                Lighting.AddLight(position11, 0.7f * Scale, 0.8f * Scale, 1f * Scale);
            }
            if (item.type == 3002)
            {
                Vector2 position11 = new Vector2(PositionWithOffset.X + ItemPositionX + Velocity.X, PositionWithOffset.Y + ItemPositionY);
                Lighting.AddLight(position11, 1.05f * Scale, 0.95f * Scale, 0.55f * Scale);
            }
            if (item.type == 3112)
            {
                Vector2 position11 = new Vector2(PositionWithOffset.X + ItemPositionX + Velocity.X, PositionWithOffset.Y + ItemPositionY);
                Lighting.AddLight(position11, 1f * Scale, 0.6f * Scale, 0.85f * Scale);
            }
        }

        public void IncreaseDamageStacker(int Damage, int TargetMaxHealth, bool Hurt = false)
        {
            if (OwnerPos > -1 && Damage > 1 && Damage < TargetMaxHealth && Damage < 1000) //There's no fun if it dies in 1 hit
            {
                float DamageValue = (float)Damage / TargetMaxHealth;
                if (Hurt)
                    DamageValue *= 2;
                DamageStacker += DamageValue;
            }
        }

        public void SetProjectileOwnership(int projPos)
        {
            if (!ProjMod.GuardianProj.ContainsKey(projPos))
                ProjMod.GuardianProj.Add(projPos, this);
            else
                ProjMod.GuardianProj[projPos] = this;
            Main.projectile[projPos].noDropItem = true;
            Main.projectile[projPos].noEnchantments = true;
            Main.projectile[projPos].GetGlobalProjectile<ProjMod>().SpawnClearOwnership = false;
            if (OwnerPos > -1) Main.projectile[projPos].playerImmune[OwnerPos] = int.MaxValue;
            Main.projectile[projPos].minionSlots *= -1;
        }

        public void PickOffHandToUse(ref HeldHand hand)
        {
            if (hand == HeldHand.Left && (Base.IsTerrarian || Base.DontUseRightHand))
                hand = HeldHand.Left;
            else if (hand == HeldHand.Right && (Base.IsTerrarian || Base.DontUseLeftHand))
                hand = HeldHand.Right;
            else if (hand == HeldHand.Left) hand = HeldHand.Right;
            else if (hand == HeldHand.Right) hand = HeldHand.Left;
        }

        public void PickHandToUse(ref HeldHand hand)
        {
            if (Base.UsesRightHandByDefault)
            {
                hand = HeldHand.Right;
                if (Base.ForceWeaponUseOnMainHand)
                    return;
            }
            if (Base.DontUseRightHand || Base.ForceWeaponUseOnMainHand)
            {
                hand = HeldHand.Left;
                return;
            }
            if (PlayerMounted)
            {
                if (SelectedOffhand == -1)
                {
                    if (hand == HeldHand.Left)
                        hand = HeldHand.Right;
                    else
                        hand = HeldHand.Left;
                }
            }
            else if (hand == HeldHand.Right && GrabbingPlayer)
                hand = HeldHand.Left;
        }

        public void RestoreHP(int Value)
        {
            this.HP += Value;
            if (this.HP > this.MHP)
                this.HP = this.MHP;
            CombatText.NewText(this.HitBox, CombatText.HealLife, Value.ToString());
        }

        public void RestoreMP(int Value, bool Sickness = true)
        {
            this.MP += Value;
            if (this.MP > this.MMP)
                this.MP = this.MMP;
            CombatText.NewText(this.HitBox, CombatText.HealMana, Value.ToString());
            if(Sickness)AddBuff(94, Player.manaSickTime);
        }

        public void CheckForObstacles()
        {
            int CenterTileX = (int)(Position.X * DivisionBy16), CenterTileY = (int)(Position.Y * DivisionBy16);
            if (Velocity.Y == 0)
            {
                //if (HasFlag(GuardianFlags.JumpingPitfall))
                //    RemoveFlag(GuardianFlags.JumpingPitfall);
                if (MoveRight || MoveLeft)
                {
                    int TileCheckX = (int)((Position.X + (int)(Width * 0.25f * Direction)) * DivisionBy16),
                        TileCheckY = (int)(Position.Y * DivisionBy16);
                    bool Pitfall = true;

                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            Tile t = MainMod.GetTile(TileCheckX + x * Direction, TileCheckY + y);
                            if (t != null && t.active() && (Main.tileSolid[t.type] || Main.tileSolidTop[t.type]))
                            {
                                Pitfall = false;
                            }
                        }
                    }
                    if (Pitfall)
                    {
                        bool TryJump = false;
                        for (int y = -5; y <= 5; y++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                if (TryJump)
                                    break;
                                int TileX = TileCheckX + x * Direction, TileY = TileCheckY + y;
                                Tile t = MainMod.GetTile(TileX, TileY);
                                if (t != null && t.active() && (Main.tileSolid[t.type] || Main.tileSolidTop[t.type]))
                                {
                                    int UpperTileCount = 0;
                                    int MaxHeight = (int)(CollisionHeight * DivisionBy16)+ 1;
                                    for (int u = -1; u < -MaxHeight; u--)
                                    {
                                        Tile upperTile = MainMod.GetTile(TileX, TileY + y);
                                        if (upperTile.active() && !Main.tileSolid[upperTile.type])
                                        {
                                            UpperTileCount++;
                                            break;
                                        }
                                    }
                                    if (UpperTileCount >= MaxHeight)
                                        TryJump = true;
                                }
                            }
                        }
                        if (TryJump)
                        {
                            Jump = true;
                        }
                        else
                        {
                            if (Velocity.X > 0)
                            {
                                MoveLeft = true;
                                MoveRight = false;
                            }
                            else if(Velocity.X < 0)
                            {
                                MoveRight = true;
                                MoveLeft = false;
                            }
                        }
                        //AddFlag(GuardianFlags.JumpingPitfall);
                    }
                }
            }
            int FloorX = (int)(Position.X * DivisionBy16),
                FloorY = (int)(Position.Y * DivisionBy16) + 1;
            byte HurtTileReaction = 0;
            for (int x = -1; x <= (CollisionWidth * DivisionBy16) + 2; x++)
            {
                int ThisFloorX = FloorX + x * Direction;
                Tile t = MainMod.GetTile(ThisFloorX, FloorY);
                bool AboveTile = Math.Abs(x) < (Width * DivisionBy16) + 1;
                if (t != null && t.active() && Main.tileSolid[t.type])
                {
                    if (t.type == Terraria.ID.TileID.Spikes || t.type == Terraria.ID.TileID.WoodenSpikes || ((t.type == Terraria.ID.TileID.Hellstone || t.type == Terraria.ID.TileID.HellstoneBrick) && !HasFlag(GuardianFlags.FireblocksImmunity)))
                    {
                        if (AboveTile)
                        {
                            HurtTileReaction = 1; //try getting out of the spikes.
                            break;
                        }
                        else
                        {
                            HurtTileReaction = 2; //Don't go that way.
                        }
                    }
                }
            }
            if (HurtTileReaction == 1)
            {
                /*int lpx = FloorX, lpy = FloorY, rpx = FloorX, rpy = FloorY;
                bool LeftFail = false, RightFail = false;
                byte EscapeDirection = 0;
                byte MaxAttempts = 16;
                int TileCollisionWidth = CollisionWidth * DivisionBy16 + 1;
                while (EscapeDirection == 0 && MaxAttempts > 0)
                {
                    for (byte Dir = 0; Dir < 2; Dir++)
                    {
                        bool HurtTileCollision = false;
                        int tx = (Dir == 0 ? lpx : rpx), ty = (Dir == 0 ? lpy : rpy);
                        Tile t = MainMod.GetTile(tx, ty);
                        byte Attempt = 5;
                        while (Attempt > 0)
                        {
                            if (t != null && t.active() && Main.tileSolid[t.type])
                            {

                            }
                            else
                            {

                            }
                            Attempt--;
                        }
                        if (!HurtTileCollision)
                        {
                            if (Dir == 0)
                                EscapeDirection = 1;
                            else if (Dir == 1)
                                EscapeDirection = 2;
                        }
                        if (Dir == 0)
                            lpx--;
                        else
                            rpx++;
                    }
                    MaxAttempts--;
                }*/

            }
            else if (HurtTileReaction == 2)
            {
                if (Direction > 0 && MoveRight)
                    MoveRight = false;
                if (Direction < 0 && MoveLeft)
                    MoveLeft = false;
            }
        }

        public int GetExtraJumpCount
        {
            get
            {
                int MaxJumpCount = 0;
                if (HasFlag(GuardianFlags.ExtraJumpSand))
                {
                    MaxJumpCount++;
                }
                if (HasFlag(GuardianFlags.ExtraJumpBlizzard))
                {
                    MaxJumpCount++;
                }
                if (HasFlag(GuardianFlags.ExtraJumpFart))
                {
                    MaxJumpCount++;
                }
                if (HasFlag(GuardianFlags.ExtraJumpWater))
                {
                    MaxJumpCount++;
                }
                if (HasFlag(GuardianFlags.ExtraJumpCloud))
                {
                    MaxJumpCount++;
                }
                return MaxJumpCount;
            }
        }

        public void JumpControl(ref bool MayRocket)
        {
            bool SlidingInWall = WallSlideStyle > 0 && Velocity.Y >= 0 && CollisionX && !CollisionY;
            if (Jump && !MoveDown)
            {
                int MaxJumpCount = 0;
                byte CurrentJump = 0;
                if (HasFlag(GuardianFlags.ExtraJumpSand))
                {
                    MaxJumpCount++;
                    if (ExtraJump == MaxJumpCount) CurrentJump = 1;
                }
                if (HasFlag(GuardianFlags.ExtraJumpBlizzard))
                {
                    MaxJumpCount++;
                    if (ExtraJump == MaxJumpCount) CurrentJump = 2;
                }
                if (HasFlag(GuardianFlags.ExtraJumpFart))
                {
                    MaxJumpCount++;
                    if (ExtraJump == MaxJumpCount) CurrentJump = 3;
                }
                if (HasFlag(GuardianFlags.ExtraJumpWater))
                {
                    MaxJumpCount++;
                    if (ExtraJump == MaxJumpCount) CurrentJump = 4;
                }
                if (HasFlag(GuardianFlags.ExtraJumpCloud))
                {
                    MaxJumpCount++;
                    if (ExtraJump == MaxJumpCount) CurrentJump = 5;
                }
                if (!Wet && HasCooldown(GuardianCooldownManager.CooldownType.SwimTime))
                    RemoveCooldown(GuardianCooldownManager.CooldownType.SwimTime);
                bool Jumped = false;
                if (Wet && !LavaWet && ((HasFlag(GuardianFlags.Merfolk) && GetCooldownValue(GuardianCooldownManager.CooldownType.SwimTime) < 10) || (HasFlag(GuardianFlags.SwimmingAbility) && (!LastJump || HasCooldown(GuardianCooldownManager.CooldownType.SwimTime)))))
                {
                    MayRocket = false;
                    JumpHeight = MaxJumpHeight;
                    Velocity.Y = -JumpSpeed * GravityDirection;
                    FallStart = (int)(Position.Y * DivisionBy16);
                    if (GetCooldownValue(GuardianCooldownManager.CooldownType.SwimTime) < 10) AddCooldown(GuardianCooldownManager.CooldownType.SwimTime, 30);
                }
                else if (Velocity.Y != 0 && JumpHeight == 0 && ExtraJump < MaxJumpCount && !LastJump)
                {
                    Jumped = true;
                    MayRocket = false;
                    ExtraJump++;
                    Velocity.Y = -JumpSpeed * GravityDirection;
                    FallStart = (int)(Position.Y * DivisionBy16);
                    if(CurrentJump > 0)
                        IgnoreMass = true;
                    switch (CurrentJump)
                    {
                        case 1:
                            JumpHeight = MaxJumpHeight * 3;
                            Main.PlaySound(16, (int)this.Position.X, (int)this.CenterY, 1, 1f, 0f);
                            break;
                        case 2:
                            JumpHeight = (int)(MaxJumpHeight * 1.5);
                            Main.PlaySound(16, (int)this.Position.X, (int)this.CenterY, 1, 1f, 0f);
                            break;
                        case 3:
                            JumpHeight = MaxJumpHeight * 2;
                            {
                                Main.PlaySound(Terraria.ID.SoundID.Item16, this.CenterPosition);
                                for (int k = 0; k < 10; k++)
                                {
                                    int num5 = Dust.NewDust(new Vector2(this.Position.X - 34f, this.Position.Y - 16f), 102, 32, 188, -this.Velocity.X * 0.5f, this.Velocity.Y * 0.5f, 100, default(Color), 1.5f);
                                    Main.dust[num5].velocity.X = Main.dust[num5].velocity.X * 0.5f - this.Velocity.X * 0.1f;
                                    Main.dust[num5].velocity.Y = Main.dust[num5].velocity.Y * 0.5f - this.Velocity.Y * 0.3f;
                                }
                                int num6 = Gore.NewGore(new Vector2(this.Position.X + (float)(this.Width / 2) - 16f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(435, 438), 1f);
                                Main.gore[num6].velocity.X = Main.gore[num6].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                                Main.gore[num6].velocity.Y = Main.gore[num6].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                                num6 = Gore.NewGore(new Vector2(this.Position.X - this.Width - 4f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(435, 438), 1f);
                                Main.gore[num6].velocity.X = Main.gore[num6].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                                Main.gore[num6].velocity.Y = Main.gore[num6].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                                num6 = Gore.NewGore(new Vector2(this.Position.X + (float)this.Width + 4f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(435, 438), 1f);
                                Main.gore[num6].velocity.X = Main.gore[num6].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                                Main.gore[num6].velocity.Y = Main.gore[num6].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                            }
                            break;
                        case 4:
                            JumpHeight = (int)(MaxJumpHeight * 1.25);
                            {
                                Main.PlaySound(16, (int)this.Position.X, (int)this.CenterY, 1, 1f, 0f);
                                for (int j = 0; j < 30; j++)
                                {
                                    int num3 = Dust.NewDust(new Vector2(this.Position.X, this.Position.Y), this.Width, 12, 253, this.Velocity.X * 0.3f, this.Velocity.Y * 0.3f, 100, default(Color), 1.5f);
                                    if (j % 2 == 0)
                                    {
                                        Main.dust[num3].velocity.X += (float)Main.rand.Next(30, 71) * 0.1f;
                                    }
                                    else
                                    {
                                        Main.dust[num3].velocity.X -= (float)Main.rand.Next(30, 71) * 0.1f;
                                    }
                                    Main.dust[num3].velocity.Y += (float)Main.rand.Next(-10, 31) * 0.1f;
                                    Main.dust[num3].noGravity = true;
                                    Main.dust[num3].scale += (float)Main.rand.Next(-10, 41) * 0.01f;
                                    Main.dust[num3].velocity *= Main.dust[num3].scale * 0.7f;
                                    Vector2 value = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                                    value.Normalize();
                                    value *= (float)Main.rand.Next(81) * 0.1f;
                                }
                            }
                            break;
                        case 5:
                            JumpHeight = (int)(MaxJumpHeight * 0.75);
                            {
                                Main.PlaySound(16, (int)Position.X, (int)CenterY, 1, 1f, 0f);
                                for (int m = 0; m < 10; m++)
                                {
                                    int num10 = Dust.NewDust(new Vector2(Position.X - 18f, Position.Y - 16f), 102, 32, 16, -this.Velocity.X * 0.5f, this.Velocity.Y * 0.5f, 100, default(Color), 1.5f);
                                    Main.dust[num10].velocity.X = Main.dust[num10].velocity.X * 0.5f - this.Velocity.X * 0.1f;
                                    Main.dust[num10].velocity.Y = Main.dust[num10].velocity.Y * 0.5f - this.Velocity.Y * 0.3f;
                                }
                                int num11 = Gore.NewGore(new Vector2(this.Position.X - 16f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(11, 14), 1f);
                                Main.gore[num11].velocity.X = Main.gore[num11].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                                Main.gore[num11].velocity.Y = Main.gore[num11].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                                num11 = Gore.NewGore(new Vector2(this.Position.X - Width - 4, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(11, 14), 1f);
                                Main.gore[num11].velocity.X = Main.gore[num11].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                                Main.gore[num11].velocity.Y = Main.gore[num11].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                                num11 = Gore.NewGore(new Vector2(this.Position.X + (float)this.Width + 4f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(11, 14), 1f);
                                Main.gore[num11].velocity.X = Main.gore[num11].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                                Main.gore[num11].velocity.Y = Main.gore[num11].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                            }
                            break;
                    }
                    {
                        for (int m = 0; m < 10; m++)
                        {
                            int num10 = Dust.NewDust(new Vector2(Position.X - 18f, Position.Y - 16f), 102, 32, 16, -this.Velocity.X * 0.5f, this.Velocity.Y * 0.5f, 100, default(Color), 1.5f);
                            Main.dust[num10].velocity.X = Main.dust[num10].velocity.X * 0.5f - this.Velocity.X * 0.1f;
                            Main.dust[num10].velocity.Y = Main.dust[num10].velocity.Y * 0.5f - this.Velocity.Y * 0.3f;
                        }
                        int num11 = Gore.NewGore(new Vector2(this.Position.X - 16f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(11, 14), 1f);
                        Main.gore[num11].velocity.X = Main.gore[num11].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                        Main.gore[num11].velocity.Y = Main.gore[num11].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                        num11 = Gore.NewGore(new Vector2(this.Position.X - Width - 4, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(11, 14), 1f);
                        Main.gore[num11].velocity.X = Main.gore[num11].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                        Main.gore[num11].velocity.Y = Main.gore[num11].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                        num11 = Gore.NewGore(new Vector2(this.Position.X + (float)this.Width + 4f, this.Position.Y - 16f), new Vector2(-this.Velocity.X, -this.Velocity.Y), Main.rand.Next(11, 14), 1f);
                        Main.gore[num11].velocity.X = Main.gore[num11].velocity.X * 0.1f - this.Velocity.X * 0.1f;
                        Main.gore[num11].velocity.Y = Main.gore[num11].velocity.Y * 0.1f - this.Velocity.Y * 0.05f;
                    }
                }
                else if ((Velocity.Y == 0 || SlidingInWall) && JumpHeight <= 0 && (!LastJump || HasFlag(GuardianFlags.AllowHopping)))
                {
                    Jumped = true;
                    JumpHeight = MaxJumpHeight; //Resets after the jump when sliding on the wall...
                    Velocity.Y = -JumpSpeed * GravityDirection;
                    //CollisionX = false;
                    if (SlidingInWall)
                    {
                        Velocity.X = -4 * Direction;
                        WallSlideStyle = 0;
                    }
                    FallStart = (int)(Position.Y * DivisionBy16);
                }
                else if (JumpHeight > 0)
                {
                    if (Velocity.Y == 0 || CollisionY)
                    {
                        JumpHeight = 0;
                        Velocity.Y = Mass * GravityDirection;
                    }
                    else
                    {
                        Velocity.Y = -JumpSpeed * GravityDirection;
                        if (Wet)
                            JumpHeight -= 0.5f;
                        else
                            JumpHeight--;
                    }
                }

                if(Jumped)
                    AddSkillProgress(JumpHeight, GuardianSkills.SkillTypes.Acrobatic);
            }
            else
            {
                JumpHeight = 0;
                if(Velocity.Y == 0)
                    ExtraJump = 0;
            }
        }

        public void IncreaseSkillByProjDamage(Projectile proj, int Damage, bool crit)
        {
            if (proj.melee)
            {
                AddSkillProgress(Damage, GuardianSkills.SkillTypes.Strength);
                if (crit) AddSkillProgress(Damage, GuardianSkills.SkillTypes.Luck);
            }
            if (proj.ranged || proj.thrown)
            {
                AddSkillProgress(Damage, GuardianSkills.SkillTypes.Ballistic);
                if (crit) AddSkillProgress(Damage, GuardianSkills.SkillTypes.Luck);
            }
            if (proj.magic)
            {
                AddSkillProgress(Damage, GuardianSkills.SkillTypes.Mysticism);
                if (crit) AddSkillProgress(Damage, GuardianSkills.SkillTypes.Luck);
            }
            if (proj.minion || proj.type == 376 || proj.type == 374 || proj.type == 378 || proj.type == 379 ||
                proj.type == Terraria.ID.ProjectileID.StardustCellMinionShot || proj.type == 389 || proj.type == 195 || proj.type == 408)
            {
                AddSkillProgress(Damage, GuardianSkills.SkillTypes.Leadership);
            }
        }

        public float CalculateAimingUseAnimation(float Rotation)
        {
            float AngleChecker = Rotation;
            if (!LookingLeft)
            {
                if (AngleChecker < 0)
                {
                    AngleChecker = 6.283185307179586f + AngleChecker;
                }
                AngleChecker = AngleChecker - 3.141592653589793f;
            }
            return AngleChecker * Direction * GravityDirection;
        }

        public bool HasCarpet()
        {
            for (int i = 0; i < 10; i++)
            {
                if (Inventory[i].type == Terraria.ID.ItemID.FlyingCarpet ||
                    (i >= 3 && i < 8 && Equipments[i].type == Terraria.ID.ItemID.FlyingCarpet))
                {
                    return true;
                }
            }
            return false;
        }

        public void UseSubAttack(ushort ID)
        {
            if (ID >= Base.SpecialAttackList.Count)
                return;
            GuardianSpecialAttack gsa = Base.SpecialAttackList[ID];
            int ManaCost = (int)(gsa.ManaCost * ManaCostMult);
            if(SubAttackCooldown.ContainsKey(ID) || !UseMana(ManaCost))
            {
                return;
            }
            LookingLeft = AimDirection.X < 0;
            SpecialAttack = gsa.GetSpecialAttackData;
            SpecialAttack.ID = ID;
            gsa.OnUse(this, SpecialAttack);
        }

        public bool CanUseSubAttack(ushort ID)
        {
            GuardianBase b = Base;
            if (ID >= b.SpecialAttackList.Count)
                return false;
            return MP >= (int)(b.SpecialAttackList[ID].ManaCost * ManaCostMult) && !SubAttackInCooldown(ID);
        }

        public bool SubAttackInCooldown(ushort ID)
        {
            return SubAttackCooldown.ContainsKey(ID);
        }

        public void ResetSubAttackCooldown(ushort ID)
        {
            if (SubAttackInCooldown(ID))
            {
                SubAttackCooldown.Remove(ID);
            }
        }

        public void ChangeSubAttackCooldown(ushort ID, float Value)
        {
            if (SubAttackInCooldown(ID))
            {
                SubAttackCooldown[ID] += Value;
            }
            else
            {
                SubAttackCooldown.Add(ID, Value);
            }
        }

        public void ChangeSubAttackCooldown (ushort ID, int Time)
        {
            if (SubAttackInCooldown(ID))
            {
                SubAttackCooldown[ID] = Time;
            }
            else
            {
                SubAttackCooldown.Add(ID, Time);
            }
        }

        public void UpdateSubAttack()
        {
            ushort[] CooldownKeys = SubAttackCooldown.Keys.ToArray();
            foreach(byte k in CooldownKeys)
            {
                SubAttackCooldown[k]--;
                if(SubAttackCooldown[k] <= 0)
                {
                    SubAttackCooldown.Remove(k);
                }
            }
            if (!SubAttackInUse)
                return;
            LockDirection = true;
            SpecialAttack.Update(this);
            return;
            /*int LastAttackTime = SubAttackTime - 1;
            if(SubAttackTime == 0 && SubAttackID >= Base.SpecialAttackList.Count)
            {
                _SubAttack = 0;
                return;
            }
            GuardianSpecialAttack subattack = Base.SpecialAttackList[SubAttackID];
            float StackCounter = 0;
            int LastFrame = -1, CurrentFrame = 0;
            int FrameCount = 0;
            int FrameTime = 0;
            foreach(GuardianSpecialAttackFrame frame in subattack.SpecialAttackFrames)
            {
                float FrameStart = StackCounter, FrameEnd = StackCounter + (int)(frame.Duration * SubAttackSpeed);
                if (SubAttackTime >= FrameStart && SubAttackTime < FrameEnd)
                {
                    CurrentFrame = FrameCount;
                    FrameTime = (int)(SubAttackTime - FrameStart);
                }
                if (LastAttackTime >= FrameStart && LastAttackTime < FrameEnd)
                {
                    LastFrame = FrameCount;
                }
                StackCounter += frame.Duration * SubAttackSpeed;
                FrameCount++;
            }
            if(CurrentFrame > LastFrame)
            {
                GuardianSpecialAttackFrame frame = subattack.SpecialAttackFrames[CurrentFrame];
                if (frame.BodyFrame > -1) BodyAnimationFrame = frame.BodyFrame;
                if (frame.LeftArmFrame > -1) LeftArmAnimationFrame = frame.LeftArmFrame;
                if (frame.RightArmFrame > -1) RightArmAnimationFrame = frame.RightArmFrame;
                subattack.WhenFrameBeginsScript(this, CurrentFrame);
            }
            SubAttackFrame = CurrentFrame;
            if (SubAttackTime >= StackCounter)
            {
                subattack.WhenSubAttackEnds(this);
                if(subattack.Cooldown > 0)
                {
                    SubAttackCooldown.Add(_SubAttack, subattack.Cooldown);
                }
                _SubAttack = 0;
                NpcHit.Clear();
                PlayerHit.Clear();
                return;
            }
            else
            {
                GuardianSpecialAttackFrame frame = subattack.SpecialAttackFrames[CurrentFrame];
                SubAttackBodyFrame = frame.BodyFrame;
                SubAttackLeftArmFrame = frame.LeftArmFrame;
                SubAttackRightArmFrame = frame.RightArmFrame;
                subattack.AnimationReplacer(this, SubAttackFrame, SubAttackTime, ref SubAttackBodyFrame, ref SubAttackLeftArmFrame, ref SubAttackRightArmFrame);
            }
            subattack.WhenFrameUpdatesScript(this, CurrentFrame, FrameTime);
            SubAttackTime++;*/
        }

        public static bool UsingLeftArmAnimation = false, UsingRightArmAnimation = false;
        public enum AnimationState : byte
        {
            Standing,
            Moving,
            InAir,
            UsingFurniture,
            Defeated,
            WallSliding,
            RidingMount,
            Swim
        }
        public AnimationState LastAnimationState = AnimationState.Standing;
        
        public void UpdateAnimation()
        {
            if (HasFlag(GuardianFlags.Petrified))
            {
                if (Base.PetrifiedFrame > -1)
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.PetrifiedFrame;
                    ApplyFrameAnimationChangeScripts();
                }
                return;
            }
            if (HasFlag(GuardianFlags.Frozen)) return;
            AnimationState NewState = AnimationState.Standing; //The conditions are actually in priority orders.
            if (HasCooldown(GuardianCooldownManager.CooldownType.SwimTime)) NewState = AnimationState.Swim;
            else if (Velocity.Y != 0) NewState = AnimationState.InAir;
            else if (KnockedOut) NewState = AnimationState.Defeated;
            else if (UsingFurniture) NewState = AnimationState.UsingFurniture;
            else if (SittingOnPlayerMount || mount.Active || (PlayerMounted && ReverseMount)) NewState = AnimationState.RidingMount;
            else if (WallSlideStyle > 0) NewState = AnimationState.WallSliding;
            else if (Velocity.X != 0) NewState = AnimationState.Moving;
            UpdateMountAnimation();
            if (NewState != LastAnimationState)
                AnimationTime = 0;
            LastAnimationState = NewState;
            if (CarriedByGuardianID > -1 && BeingCarriedByGuardian)
            {
                BodyAnimationFrame = Base.StandingFrame;
                LeftArmAnimationFrame = RightArmAnimationFrame = Base.JumpFrame;
                ApplyFrameAnimationChangeScripts();
                return;
            }
            if (HasBuff(ModContent.BuffType<giantsummon.Buffs.Sleeping>()) && Velocity.X == 0 && Velocity.Y == 0 && !IsBeingPulledByPlayer)
            {
                if (Base.DownedFrame > -1)
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.DownedFrame;
                }
                else if (Base.DuckingFrame > -1)
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.DuckingFrame;
                }
                else
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.StandingFrame;
                }
                ApplyFrameAnimationChangeScripts();
                return;
            }
            if (KnockedOut && !Downed && (Velocity.Y == 0 || WofTongued))
            {
                if (Base.DownedFrame > -1)
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.DownedFrame;
                }
                else if (Base.DuckingFrame > -1)
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.DuckingFrame;
                }
                else
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.StandingFrame;
                }
                AnimationTime++;
                ApplyFrameAnimationChangeScripts();
                return;
            }
            UsingLeftArmAnimation = false;
            UsingRightArmAnimation = false;
            UpdateWingAnimation();
            bool FurnitureOverridingAnimation = false;
            bool GuardianHasCarpet = HasCarpet();
            if (UsingFurniture && furniturex > -1 && furniturey > -1 && !IsAttackingSomething)
            {
                Tile tile = MainMod.GetTile(furniturex, furniturey);
                if (tile != null)
                {
                    switch (tile.type)
                    {
                        case Terraria.ID.TileID.Chairs:
                            {
                                int AnimationID = Base.SittingFrame;
                                if (Base.ChairSittingFrame > -1)
                                    AnimationID = Base.ChairSittingFrame;
                                BodyAnimationFrame = AnimationID;
                                LeftArmAnimationFrame = AnimationID;
                                RightArmAnimationFrame = AnimationID;
                            }
                            break;
                        case Terraria.ID.TileID.Benches:
                        case Terraria.ID.TileID.Thrones:
                            {
                                FurnitureOverridingAnimation = true;
                                int AnimationID = Base.ThroneSittingFrame;
                                if (AnimationID == -1)
                                {
                                    if (Base.ChairSittingFrame > -1)
                                        AnimationID = Base.ChairSittingFrame;
                                    else
                                        AnimationID = Base.SittingFrame;
                                }
                                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = AnimationID;
                            }
                            break;
                        case Terraria.ID.TileID.Beds:
                            {
                                FurnitureOverridingAnimation = true;
                                int AnimationID = Base.BedSleepingFrame;
                                if (SittingOnBed)
                                {
                                    AnimationID = Base.ThroneSittingFrame;
                                    if (AnimationID == -1)
                                    {
                                        if (Base.ChairSittingFrame > -1)
                                            AnimationID = Base.ChairSittingFrame;
                                        else
                                            AnimationID = Base.SittingFrame;
                                    }
                                }
                                else if (AnimationID == -1)
                                {
                                    if (Base.ChairSittingFrame > -1)
                                        AnimationID = Base.ChairSittingFrame;
                                    else
                                        AnimationID = Base.SittingFrame;
                                }
                                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = AnimationID;
                                if (EmotionDisplayTime <= 0)
                                    DisplayEmotion(Emotions.Sleepy);
                            }
                            break;
                    }
                    AnimationTime++;
                }
            }
            else if (MountedOnPlayer)
            {
                if (PlayerMounted && ReverseMount)
                {
                    if (Base.ReverseMount)
                    {
                        if (Base.PlayerMountedArmAnimation > -1)
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.PlayerMountedArmAnimation;
                        }
                        else
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.SittingFrame;
                        }
                    }
                    else
                    {
                        BodyAnimationFrame = Base.SittingFrame;
                        LeftArmAnimationFrame = RightArmAnimationFrame = Base.ItemUseFrames[2];
                    }
                }
                else
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.SittingFrame;
                }
                if (Base.IsTerrarian && ItemAnimationTime == 0)
                {
                    LeftArmAnimationFrame = 3;
                    UsingLeftArmAnimation = true;
                }
                AnimationTime++;
            }
            else
            {
                if ((PlayerControl || IsCommander) && Main.player[OwnerPos].GetModPlayer<PlayerMod>().MountGuardian != null)
                {
                    TerraGuardian otherguardian = Main.player[OwnerPos].GetModPlayer<PlayerMod>().MountGuardian;
                    int Animation = Base.SittingFrame;
                    BodyAnimationFrame = Animation;
                    LeftArmAnimationFrame = Animation;
                    RightArmAnimationFrame = Animation;
                    AnimationTime++;
                }
                else if (mount.Active)
                {
                    BodyAnimationFrame = Base.SittingFrame;
                    LeftArmAnimationFrame = Base.SittingFrame;
                    RightArmAnimationFrame = Base.SittingFrame;
                    AnimationTime++;
                }
                else if (WallSlideStyle > 0)
                {
                    BodyAnimationFrame = Base.JumpFrame;
                    if (Base.DontUseRightHand)
                    {
                        LeftArmAnimationFrame = Base.ItemUseFrames[2];
                        UsingLeftArmAnimation = true;
                    }
                    else
                    {
                        RightArmAnimationFrame = Base.ItemUseFrames[2];
                        UsingRightArmAnimation = true;
                    }
                    AnimationTime++;
                }
                else if (Wet && HasCooldown(GuardianCooldownManager.CooldownType.SwimTime))
                {
                    int SwimValue = GetCooldownValue(GuardianCooldownManager.CooldownType.SwimTime);
                    float MaxAnimationTime = Base.MaxWalkSpeedTime;
                    AnimationTime++;
                    if (AnimationTime < 0f)
                        AnimationTime += MaxAnimationTime;
                    if (AnimationTime >= MaxAnimationTime)
                        AnimationTime -= MaxAnimationTime;
                    int PickedFrame = (int)(AnimationTime / Base.WalkAnimationFrameTime);
                    BodyAnimationFrame = PickedFrame;
                    int ArmFrame = Base.StandingFrame;
                    if (SwimValue >= 10 && SwimValue < 20)
                        ArmFrame = Base.JumpFrame;
                    LeftArmAnimationFrame = ArmFrame;
                    RightArmAnimationFrame = ArmFrame;
                }
                else if (GuardianHasCarpet)
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.StandingFrame;
                    AnimationTime++;
                }
                else if (Velocity.Y == 0)
                {
                    if (Ducking)
                    {
                        BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.DuckingFrame;
                    }
                    else if (Velocity.X != 0 && (!HasFlag(GuardianFlags.WindPushed) || (MoveLeft == true || MoveRight == true)))
                    {
                        float MaxAnimationTime = Base.MaxWalkSpeedTime * Scale;
                        float SpeedX = Math.Abs(Velocity.X);
                        if (WalkMode)
                            SpeedX *= 3;
                        if ((Velocity.X > 0 && Direction < 0) || (Velocity.X < 0 && Direction > 0))
                            SpeedX *= -1;
                        AnimationTime += SpeedX / MaxSpeed;
                        if (AnimationTime < 0f)
                            AnimationTime += MaxAnimationTime;
                        if (AnimationTime >= MaxAnimationTime)
                            AnimationTime -= MaxAnimationTime;
                        int PickedFrame = (int)(AnimationTime / (Base.WalkAnimationFrameTime * Scale));
                        if (PickedFrame >= 0 && PickedFrame < Base.WalkingFrames.Length)
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.WalkingFrames[PickedFrame];
                    }
                    else
                    {
                        if (CurrentIdleAction == IdleActions.LookingAtTheBackground)
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.BackwardStanding;
                        }
                        else
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.StandingFrame;
                        }
                        AnimationTime++;
                    }
                }
                else
                {
                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.JumpFrame;
                    AnimationTime++;
                }
            }
            if (!FurnitureOverridingAnimation)
            {
                if (PlayerMounted && !Ducking)
                {
                    if (ReverseMount)
                    {
                        LeftArmAnimationFrame = Base.SittingFrame;
                    }
                    else if (Base.PlayerMountedArmAnimation > -1 && !Main.player[OwnerPos].GetModPlayer<PlayerMod>().KnockedOut)
                    {
                        LeftArmAnimationFrame = Base.PlayerMountedArmAnimation;
                        UsingLeftArmAnimation = true;
                    }
                }
                if (GrabbingPlayer && (ItemAnimationTime == 0 || ItemUseType != ItemUseTypes.HeavyVerticalSwing))
                {
                    if (!Base.DontUseRightHand)
                    {
                        RightArmAnimationFrame = Base.ItemUseFrames[2];
                        UsingRightArmAnimation = true;
                    }
                    else
                    {
                        LeftArmAnimationFrame = Base.ItemUseFrames[2];
                        UsingLeftArmAnimation = true;
                    }
                }
            }
            UpdateItemUseAnimationEffect(HeldItemHand);
            if (IsDualWielding)
            {
                UpdateItemUseAnimationEffect(HeldOffHand);
            }
            else if (SelectedOffhand > -1 && OffHandAction)
            {
                //if (HeldItemHand != GuardianItemExtraData.HeldHand.Both)
                {
                    HeldHand hand = HeldOffHand;
                    float AngleChecker = CalculateAimingUseAnimation(ItemRotation);
                    bool IsUmbrella = Inventory[SelectedOffhand].type == 946;
                    int Frame = 0;
                    /*if (Inventory[SelectedOffhand].type == 946)
                    {
                        if (Ducking)
                        {
                            Frame = Base.DuckingSwingFrames[1];
                        }
                        else
                        {
                            Frame = Base.ItemUseFrames[1];
                        }
                    }
                    else*/
                    {
                        if (Ducking)
                        {
                            Frame = Base.DuckingSwingFrames[2];
                        }
                        else
                        {
                            Frame = Base.ItemUseFrames[2 + OffhandOrientation];
                        }
                    }
                    if (hand == HeldHand.Left)
                    {
                        LeftArmAnimationFrame = Frame;
                        UsingLeftArmAnimation = true;
                    }
                    if (hand == HeldHand.Right)
                    {
                        RightArmAnimationFrame = Frame;
                        UsingRightArmAnimation = true;
                    }
                }
            }
            ApplyFrameAnimationChangeScripts();
            UpdateMountAnimation();
        }

        public void UpdateMountAnimation()
        {
            if (mount.Active)
            {
                if (Velocity.Y != 0f)
                {
                    if (this.mount.FlyTime > 0 && JumpHeight == 0 && Jump && !this.mount.CanHover)
                    {
                        if (this.mount.Type == 0)
                        {
                            if (!LookingLeft)
                            {
                                if (Main.rand.Next(4) == 0)
                                {
                                    int num14 = Dust.NewDust(new Vector2(Position.X - 22f, Position.Y - 6f), 20, 10, 64, Velocity.X * 0.25f, Velocity.Y * 0.25f, 255, default(Color), 1f);
                                    Main.dust[num14].velocity *= 0.1f;
                                    Main.dust[num14].noLight = true;
                                }
                                if (Main.rand.Next(4) == 0)
                                {
                                    int num15 = Dust.NewDust(new Vector2(Position.X + 12f, Position.Y - 6f), 20, 10, 64, Velocity.X * 0.25f, Velocity.Y * 0.25f, 255, default(Color), 1f);
                                    Main.dust[num15].velocity *= 0.1f;
                                    Main.dust[num15].noLight = true;
                                }
                            }
                            else
                            {
                                if (Main.rand.Next(4) == 0)
                                {
                                    int num16 = Dust.NewDust(new Vector2(Position.X - 32f, Position.Y - 6f), 20, 10, 64, Velocity.X * 0.25f, Velocity.Y * 0.25f, 255, default(Color), 1f);
                                    Main.dust[num16].velocity *= 0.1f;
                                    Main.dust[num16].noLight = true;
                                }
                                if (Main.rand.Next(4) == 0)
                                {
                                    int num17 = Dust.NewDust(new Vector2(Position.X + 2f, Position.Y - 6f), 20, 10, 64, Velocity.X * 0.25f, Velocity.Y * 0.25f, 255, default(Color), 1f);
                                    Main.dust[num17].velocity *= 0.1f;
                                    Main.dust[num17].noLight = true;
                                }
                            }
                        }
                        mount.UpdateFrame(this, 3, Velocity);
                    }
                    else if (Wet)
                    {
                        mount.UpdateFrame(this, 4, Velocity);
                    }
                    else
                    {
                        mount.UpdateFrame(this, 2, Velocity);
                    }
                }
                else if (Velocity.X == 0f)
                {
                    this.mount.UpdateFrame(this, 0, Velocity);
                }
                else
                {
                    this.mount.UpdateFrame(this, 1, Velocity);
                }
            }
        }

        private void UpdateItemUseAnimationEffect(HeldHand Hand)
        {
            if (ItemAnimationTime > 0 || FreezeItemUseAnimation)
            {
                float AnimationPercentage = 1f - (float)ItemAnimationTime / ItemMaxAnimationTime;
                if (ItemUseType == ItemUseTypes.CursedAttackAttempt)
                {
                    int AnimationFrame = (Ducking ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[2]);
                    if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                        AnimationFrame = Base.SittingItemUseFrames[1];
                    if (Hand == HeldHand.Left)
                    {
                        LeftArmAnimationFrame = AnimationFrame;
                        UsingLeftArmAnimation = true;
                    }
                    else if (Hand == HeldHand.Right)
                    {
                        RightArmAnimationFrame = AnimationFrame;
                        UsingRightArmAnimation = true;
                    }
                    else
                    {
                        LeftArmAnimationFrame = AnimationFrame;
                        UsingLeftArmAnimation = true;
                        RightArmAnimationFrame = AnimationFrame;
                        UsingRightArmAnimation = true;
                    }
                }
                else if (ItemUseType == ItemUseTypes.ClawAttack) //So dated attack... If I revamp the basic attack, this will get an use.
                {
                    if (AnimationPercentage < 0.33f)
                    {
                        BodyAnimationFrame = 13;
                    }
                    else if (AnimationPercentage < 0.7f)
                    {
                        BodyAnimationFrame = 14;
                    }
                    else
                    {
                        BodyAnimationFrame = 15;
                    }
                    LeftArmAnimationFrame = RightArmAnimationFrame = BodyAnimationFrame;
                    UsingLeftArmAnimation = UsingRightArmAnimation = true;
                }
                else if (ItemUseType == ItemUseTypes.HeavyVerticalSwing)
                {
                    float ThisAnimationPercentage = GetTwoHandedItemUsePercentage(AnimationPercentage);
                    if (ThisAnimationPercentage < 0.45f)
                    {
                        BodyAnimationFrame = Base.HeavySwingFrames[0];
                    }
                    else if (ThisAnimationPercentage < 0.65f)
                    {
                        BodyAnimationFrame = Base.HeavySwingFrames[1];
                    }
                    else
                    {
                        BodyAnimationFrame = Base.HeavySwingFrames[2];
                    }
                    LeftArmAnimationFrame = RightArmAnimationFrame = BodyAnimationFrame;
                    UsingLeftArmAnimation = UsingRightArmAnimation = true;
                }
                else if (ItemUseType == ItemUseTypes.LightVerticalSwing)
                {
                    int AnimationFrame = 0;
                    if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                    {
                        if (AnimationPercentage < 0.333f)
                        {
                            AnimationFrame = Base.SittingItemUseFrames[0];
                        }
                        else if (AnimationPercentage < 0.666f)
                        {
                            AnimationFrame = Base.SittingItemUseFrames[1];
                        }
                        else
                        {
                            AnimationFrame = Base.SittingItemUseFrames[2];
                        }
                    }
                    else if (!Ducking)
                    {
                        if (AnimationPercentage < 0.25f)
                        {
                            AnimationFrame = Base.ItemUseFrames[0];
                        }
                        else if (AnimationPercentage < 0.5f)
                        {
                            AnimationFrame = Base.ItemUseFrames[1];
                        }
                        else if (AnimationPercentage < 0.75f)
                        {
                            AnimationFrame = Base.ItemUseFrames[2];
                        }
                        else
                        {
                            AnimationFrame = Base.ItemUseFrames[3];
                        }
                    }
                    else
                    {
                        if (AnimationPercentage < 0.333f)
                        {
                            AnimationFrame = Base.DuckingSwingFrames[0];
                        }
                        else if (AnimationPercentage < 0.666f)
                        {
                            AnimationFrame = Base.DuckingSwingFrames[1];
                        }
                        else
                        {
                            AnimationFrame = Base.DuckingSwingFrames[2];
                        }
                    }
                    if (Hand == HeldHand.Left)
                    {
                        LeftArmAnimationFrame = AnimationFrame;
                        UsingLeftArmAnimation = true;
                    }
                    else if (Hand == HeldHand.Right)
                    {
                        RightArmAnimationFrame = AnimationFrame;
                        UsingRightArmAnimation = true;
                    }
                }
                else if (ItemUseType == ItemUseTypes.AimingUse)
                {
                    float AngleChecker = CalculateAimingUseAnimation(ItemRotation);
                    int Frame = 0;
                    bool WeaponIsBlowpipe = SelectedItem > -1 && (Inventory[SelectedItem].type == Terraria.ID.ItemID.Blowpipe || Inventory[SelectedItem].type == Terraria.ID.ItemID.Blowgun);
                    if (ArmOrientation == 0 || WeaponIsBlowpipe) //up
                    {
                        if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                        {
                            Frame = Base.SittingItemUseFrames[0];
                        }
                        else
                        {
                            Frame = (Ducking ? Base.DuckingSwingFrames[1] : Base.ItemUseFrames[1]);
                        }
                    }
                    else if (ArmOrientation == 2) //down
                    {
                        if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                        {
                            Frame = Base.SittingItemUseFrames[2];
                        }
                        else
                        {
                            Frame = (Ducking ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[3]);
                        }
                    }
                    else //center
                    {
                        if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                        {
                            Frame = Base.SittingItemUseFrames[1];
                        }
                        else
                        {
                            Frame = (Ducking ? Base.DuckingSwingFrames[2] : Base.ItemUseFrames[2]);
                        }
                    }
                    if (Hand == HeldHand.Both || Hand == HeldHand.Left)
                    {
                        LeftArmAnimationFrame = Frame;
                        UsingLeftArmAnimation = true;
                    }
                    if (Hand == HeldHand.Both || Hand == HeldHand.Right)
                    {
                        RightArmAnimationFrame = Frame;
                        UsingRightArmAnimation = true;
                    }
                }
                else if (ItemUseType == ItemUseTypes.ItemDrink2h)
                {
                    byte AnimationFrame = 2;
                    if (AnimationPercentage < 0.05f)
                    {
                        AnimationFrame = 2;
                    }
                    else if (AnimationPercentage < 0.1f)
                    {
                        AnimationFrame = 1;
                    }
                    else if (AnimationPercentage < 0.9f)
                    {
                        AnimationFrame = 0;
                    }
                    else if (AnimationPercentage < 0.95f)
                    {
                        AnimationFrame = 1;
                    }
                    else
                    {
                        AnimationFrame = 2;
                    }
                    if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                    {
                        LeftArmAnimationFrame = RightArmAnimationFrame = Base.SittingItemUseFrames[AnimationFrame];
                    }
                    else if (Ducking)
                    {
                        if (AnimationFrame == 0)
                        {
                            LeftArmAnimationFrame = RightArmAnimationFrame = Base.DuckingSwingFrames[1];
                        }
                        else
                        {
                            LeftArmAnimationFrame = RightArmAnimationFrame = Base.DuckingSwingFrames[2];
                        }
                    }
                    else
                    {
                        LeftArmAnimationFrame = RightArmAnimationFrame = Base.ItemUseFrames[1 + AnimationFrame];
                    }
                    UsingLeftArmAnimation = UsingRightArmAnimation = true;
                }
                else if (ItemUseType == ItemUseTypes.OverHeadItemUse)
                {
                    if (SittingOnPlayerMount && Base.SittingItemUseFrames != null)
                    {
                        if (Hand == HeldHand.Left)
                        {
                            LeftArmAnimationFrame = Base.SittingItemUseFrames[0];
                            UsingLeftArmAnimation = true;
                        }
                        else if (Hand == HeldHand.Right)
                        {
                            RightArmAnimationFrame = Base.SittingItemUseFrames[0];
                            UsingRightArmAnimation = true;
                        }
                    }
                    else if (Ducking)
                    {
                        if (Hand == HeldHand.Left)
                        {
                            LeftArmAnimationFrame = Base.DuckingSwingFrames[1];
                            UsingLeftArmAnimation = true;
                        }
                        else if (Hand == HeldHand.Right)
                        {
                            RightArmAnimationFrame = Base.DuckingSwingFrames[1];
                            UsingRightArmAnimation = true;
                        }
                    }
                    else
                    {
                        if (Hand == HeldHand.Left)
                        {
                            LeftArmAnimationFrame = Base.ItemUseFrames[1];
                            UsingLeftArmAnimation = true;
                        }
                        else if (Hand == HeldHand.Right)
                        {
                            RightArmAnimationFrame = Base.ItemUseFrames[1];
                            UsingRightArmAnimation = true;
                        }
                    }
                }
            }
        }

        public void ApplyFrameAnimationChangeScripts()
        {
            Base.GuardianAnimationScript(this, ref UsingLeftArmAnimation, ref UsingRightArmAnimation);
            Base.GuardianAnimationOverride(this, 0, ref BodyAnimationFrame);
            Base.GuardianAnimationOverride(this, 1, ref LeftArmAnimationFrame);
            Base.GuardianAnimationOverride(this, 2, ref RightArmAnimationFrame);
            if (!FreezeItemUseAnimation && DoAction.InUse)
            {
                DoAction.UpdateAnimation(this, ref UsingLeftArmAnimation, ref UsingRightArmAnimation);
            }
            if (SubAttackInUse)
            {
                SpecialAttack.UpdateAnimation(this, ref UsingLeftArmAnimation, ref UsingRightArmAnimation);
            }
        }

        public void UpdateWingAnimation()
        {
            if (WingType > 0)
            {
                if (Jump && Velocity.Y != 0)
                {
                    if (WingFlightTime > 0 && JumpHeight == 0)
                    {
                        WingCounter++;
                        if (WingCounter >= 4)
                        {
                            WingCounter -= 4;
                            WingFrame++;
                            if (WingFrame >= 4)
                                WingFrame -= 4;
                        }
                    }
                    else
                    {
                        WingCounter = 0;
                        WingFrame = 2;
                    }
                }
                else
                {
                    WingCounter = 0;
                    WingFrame = 0;
                }
            }
        }

        public void ItemPlacedInInventory(int InventorySlot)
        {
            if (this.Inventory[InventorySlot].type == ModContent.ItemType<Items.Misc.BirthdayPresent>() && !Data.GiftGiven && Data.IsBirthday)
            {
                GuardianActions.OpenBirthdayPresent(this, InventorySlot);
                Data.GiftGiven = true;
            }
        }

        public void DoSandstormPushing()
        {
            float SpeedMod = (float)Math.Sign(Main.windSpeed) * 0.07f;
            if (Math.Abs(Main.windSpeed) > 0.5f)
            {
                SpeedMod *= 1.37f;
            }
            if (Velocity.Y != 0)
            {
                SpeedMod *= 1.5f;
            }
            if (MoveLeft || MoveRight)
            {
                SpeedMod *= 0.8f;
            }
            /*if (Math.Sign(Direction) != Math.Sign(SpeedMod))
            {
                SpeedMod -= Math.Abs(SpeedMod) * 40f;
            }*/
            switch (Base.Size)
            {
                case GuardianBase.GuardianSize.Large:
                    SpeedMod *= 0.5f;
                    break;
                case GuardianBase.GuardianSize.Small:
                    SpeedMod *= 4f;
                    break;
            }
            if (SpeedMod < 0 && Velocity.X > SpeedMod)
            {
                Velocity.X += SpeedMod;
                if (Velocity.X < SpeedMod)
                    Velocity.X = SpeedMod;
            }
            if (SpeedMod > 0 && Velocity.X < SpeedMod)
            {
                Velocity.X += SpeedMod;
                if (Velocity.X > SpeedMod)
                    Velocity.X = SpeedMod;
            }
        }

        public void UpdateMountedPosition()
        {
            if (OwnerPos == -1)
                return;
            Player Owner = Main.player[OwnerPos];
            if (Owner.GetModPlayer<PlayerMod>().KnockedOut)
            {
                if (SittingOnPlayerMount)
                {
                    DoSitOnPlayerMount(false);
                }
                else if (MountedOnPlayer)
                {
                    ToggleMount(true);
                }
                else if (PlayerMounted)
                {
                    Owner.GetModPlayer<PlayerMod>().ReviveBoost += 2;
                }
            }
            if (MountedOnPlayer && (Owner.GetModPlayer<PlayerMod>().ControllingGuardian || Owner.GetModPlayer<PlayerMod>().IsCompanionParty))
            {
                TerraGuardian otherGuardian = Owner.GetModPlayer<PlayerMod>().Guardian;
                //Add a script for when the guardian is downed.
                if (ItemAnimationTime == 0 && !FreezeItemUseAnimation)
                {
                    LookingLeft = otherGuardian.LookingLeft;
                }
                Position = otherGuardian.GetGuardianShoulderPosition;
                float PosX = Base.SittingPoint.X;
                if (LookingLeft)
                    PosX = SpriteWidth - PosX;
                PosX -= SpriteWidth * 0.5f;
                Position.X += PosX;
                Position.Y += SpriteHeight - Base.SittingPoint.Y;
                return;
            }
            if ((PlayerControl || IsCommander) && Owner.GetModPlayer<PlayerMod>().MountGuardian != null)
            {
                TerraGuardian otherGuardian = Owner.GetModPlayer<PlayerMod>().MountGuardian;
                if (ItemAnimationTime == 0 && !FreezeItemUseAnimation)
                {
                    LookingLeft = otherGuardian.LookingLeft;
                }
                Position = otherGuardian.GetGuardianShoulderPosition + otherGuardian.Velocity;
                float PosX = Base.SittingPoint.X;
                if (LookingLeft)
                    PosX = SpriteWidth - PosX;
                PosX -= SpriteWidth * 0.5f;
                Position.X += PosX;
                Position.Y += SpriteHeight - Base.SittingPoint.Y;
            }
            if (MountedOnPlayer || SittingOnPlayerMount)
            {
                if (ItemAnimationTime == 0 && !FreezeItemUseAnimation)
                    FaceDirection(Owner.direction == -1);
                bool InMineCart = Owner.mount.Active && (Owner.mount.Type == Terraria.ID.MountID.MineCart || Owner.mount.Type == Terraria.ID.MountID.MineCartMech || Owner.mount.Type == Terraria.ID.MountID.MineCartWood);
                Vector2 MountPosition = Base.SittingPoint.ToVector2();
                bool SitOnMount = SittingOnPlayerMount;//SittingOnPlayerMount;
                bool IsLookingLeft = LookingLeft;
                if (!InMineCart && IsLookingLeft) MountPosition.X = SpriteWidth - MountPosition.X;
                MountPosition.X = SpriteWidth * 0.5f - MountPosition.X;
                MountPosition.Y = SpriteHeight - MountPosition.Y;
                if (!Owner.mount.Active || (PlayerMounted && ReverseMount && !SittingOnPlayerMount && !InMineCart))
                {
                    Point PositionFrame = Base.LeftHandPoints.GetPositionFromFramePoint(Base.PlayerMountedArmAnimation);
                    if (PositionFrame == Base.LeftHandPoints.DefaultCoordinate || !Base.ReverseMount)
                    {
                        PositionFrame = Base.LeftHandPoints.GetPositionFromFramePoint(Base.ItemUseFrames[2]);
                        MountPosition.Y = Owner.position.Y + Owner.height + 6 + Owner.gfxOffY;// -12;
                        MountPosition.X += Owner.Center.X - 18 * Direction;
                        MountPosition.Y += -SpriteHeight + PositionFrame.Y;
                    }
                    else
                    {
                        MountPosition.X = Owner.Center.X - 10 * Direction;
                        MountPosition.X += (PositionFrame.X - (int)(Base.SpriteWidth * 0.5f) - 4) * Direction;
                        MountPosition.Y = Owner.position.Y + 56 - 30 - (PositionFrame.Y - Base.SpriteHeight) * Scale - 12 + Owner.gfxOffY; // -12;
                    }
                    if (Owner.mount.Active && !PlayerMounted)
                    {
                        MountPosition.Y += (-Owner.mount.PlayerOffset - Owner.mount.YOffset);// +6;
                    }
                    /*else
                    {
                        MountPosition.Y += Owner.position.Y + 12;
                    }*/
                }
                else
                {
                    int MountedPosition = Owner.GetModPlayer<PlayerMod>().MountSitOrder++;
                    if (InMineCart && false)
                    {
                        //float CartDirection = 1f;
                        float Direction = 1f;
                        if ((Direction < 0 && Main.player[Main.myPlayer].velocity.X > 0) ||
                            (Direction > 0 && Main.player[Main.myPlayer].velocity.X < 0))
                        {
                            MountPosition.X += 6;
                        }
                        else
                        {
                            Direction = -1f;
                            MountPosition.X += -16;// *Direction;
                        }
                        MountPosition.Y += (SitOnMount ? -2 : -14) - 14 - Owner.mount.PlayerOffset - Owner.mount.YOffset + 6; //-14 - POffset
                        //MountPosition = Mount.GetMinecartMechPoint(Owner, (int)MountPosition.X, (int)MountPosition.Y);
                        MountPosition.X += Owner.Center.X + MountedPosition * Direction * 16f;
                        MountPosition.Y += Owner.position.Y + Owner.height;
                    }
                    else
                    {
                        MountPosition.X += -16 * Direction + -16 * MountedPosition * Direction;
                        if (InMineCart)
                        {
                            if (Direction < 0)
                            {
                                MountPosition.X -= 10;
                            }
                            else
                            {
                                MountPosition.X -= 2;
                            }
                            MountPosition.Y += 8f;
                        }
                        MountPosition.Y += (SitOnMount ? -2 : -14) - 14 - Owner.mount.PlayerOffset - Owner.mount.YOffset + 6; //-14 - POffset
                        //MountPosition = MountPosition.RotatedBy(Rotation);
                        MountPosition.X += Owner.Center.X;
                        MountPosition.Y += Owner.position.Y + Owner.height;
                    }
                }
                if (PlayerMod.GiveHeightBoost(Owner))
                    MountPosition.Y -= 2;
                //Rotation = Owner.fullRotation;
                GravityDirection = (int)Owner.gravDir;
                Position = MountPosition;
                Velocity = Owner.velocity;
                SetFallStart();
                Jump = false;
                if (CollisionPosition.Y + CollisionHeight > Owner.position.Y + Owner.height)
                {
                    CollisionHeightDiscount = (int)((CollisionPosition.Y + CollisionHeight) - (Owner.position.Y + Owner.height));
                }
            }
        }

        public void UpdateVerticalMovement()
        {
            if (!NoGravity && !WofTongued)
            {
                float FallSpeed = (IgnoreMass ? 0.3f : Mass);
                if (HasFlag(GuardianFlags.FeatherfallPotion))
                {
                    if (MoveUp)
                        FallSpeed *= 0.1f; // Divided by 10
                    else
                        FallSpeed *= 0.333f; // Divided by 3
                    SetFallStart();
                    if ((MoveDown && GravityDirection == 1) || (MoveUp && GravityDirection == -1))
                    {
                        FallSpeed = 4;
                    }
                    else
                    {
                        //FallSpeed = (-FallSpeed + 1E-05f);
                    }
                }
                if (WallSlideStyle > 0 && !CollisionY)
                {
                    bool GenerateSlideDust = false;
                    if (MoveDown)
                    {
                        FallSpeed = 4;
                        GenerateSlideDust = true;
                    }
                    else
                    {
                        if (WallSlideStyle == 2 && Velocity.Y + FallSpeed > 0)
                        {
                            Velocity.Y = 0;
                            FallSpeed = 0;
                        }
                        else if (WallSlideStyle == 1 && Velocity.Y > 0)
                        {
                            FallSpeed *= 0.5f;
                            Velocity.Y = 0;
                            GenerateSlideDust = true;
                        }
                    }
                    JumpHeight = 0;
                    if (GenerateSlideDust)
                    {
                        int dustPos = Dust.NewDust(this.CenterPosition, 8, 8, 31, 0,0,0, default(Color), 1f);
                        Dust dust = Main.dust[dustPos];
                        if (SlidingLeft)
                            dust.position.X -= Width * 0.5f;
                        else
                            dust.position.X += Width * 0.5f;
                        if (GravityDirection > 1)
                            dust.position.Y += Height * 0.5f;
                        else
                            dust.position.Y -= Height * 0.5f;
                        dust.position.X -= 4;
                        dust.position.Y -= 4;
                        dust.velocity *= 0.1f;
                        dust.scale = 1.2f;
                        dust.noGravity = true;
                    }
                }
                if (!HasFlag(GuardianFlags.NoGravity))
                {
                    Velocity.Y += FallSpeed * GravityDirection;
                    if (Velocity.Y * GravityDirection > MaxFallSpeed)
                        Velocity.Y = MaxFallSpeed * GravityDirection;
                }
                if (SelectedItem > -1 && Inventory[SelectedItem].type == 946 || SelectedOffhand > -1 && Inventory[SelectedOffhand].type == 946)
                {
                    if (Velocity.Y * GravityDirection > 2f)
                    {
                        Velocity.Y = 2f * GravityDirection;
                        SetFallStart();
                    }
                }
            }
        }

        public void UpdateHorizontalMovement()
        {
            bool UnallowDirectionChange = LockDirection || FreezeItemUseAnimation || 
                (ItemAnimationTime > 0 && (ItemUseType == ItemUseTypes.AimingUse || ItemUseType == ItemUseTypes.HeavyVerticalSwing || 
                ItemUseType == ItemUseTypes.LightVerticalSwing) || TurnLock > 0);
            float MaxSpeed = this.MaxSpeed;
            float Acceleration = this.Acceleration;
            float SlowDown = this.SlowDown;
            float DashSpeed = this.DashSpeed;
            if (WalkMode)
            {
                Acceleration = WalkAcceleration;
                MaxSpeed = WalkMaxSpeed;
                SlowDown = WalkSlowDown;
                DashCooldown = 30;
            }
            if (DashCooldown < 0 && DashSpeed > 0)
            {
                MaxSpeed = Math.Max(DashSpeed, MaxSpeed);
                DashCooldown = -1;
                HandleHermesBootsEffect();
            }
            Base.GuardianHorizontalMovementScript(this, ref MaxSpeed, ref Acceleration, ref SlowDown, ref DashSpeed);
            if (Downed)
            {
                if (Velocity.X > 0.025f)
                    Velocity.X -= 0.025f;
                else if (Velocity.X < -0.025f)
                    Velocity.X += 0.025f;
                else
                    Velocity.X *= 0.7f;
            }
            else
            {
                float SpeedFactor = Acceleration;
                if ((!MoveLeft && !MoveRight) || (Velocity.X > 0 && MoveLeft) || (Velocity.X < 0 && MoveRight))
                {
                    SpeedFactor = SlowDown;
                    if(Velocity.X != 0 && TurnLock == 0) SetTurnLock();
                }
                if (Ducking)
                    SpeedFactor = 0;
                if (MoveLeft && (!Ducking || Velocity.X == 0))
                {
                    Velocity.X -= SpeedFactor;
                    if (!UnallowDirectionChange) LookingLeft = true;
                    if (Velocity.X < -MaxSpeed)
                        Velocity.X = -MaxSpeed;
                }
                else if (MoveRight && (!Ducking || Velocity.X == 0))
                {
                    Velocity.X += SpeedFactor;
                    if (!UnallowDirectionChange) LookingLeft = false;
                    if (Velocity.X > MaxSpeed)
                        Velocity.X = MaxSpeed;
                }
                else if(Velocity.X != 0)
                {
                    if (Velocity.X > SlowDown)
                        Velocity.X -= SlowDown;
                    else if (Velocity.X < -SlowDown)
                        Velocity.X += SlowDown;
                    else if(Velocity.X != 0)
                    {
                        Velocity.X = 0;
                        DashCooldown = 30;
                    }
                    else
                    {
                        DashCooldown = 30;
                    }
                }
            }
        }

        public string GetMessage(string MessageID, string DefaultMessage = "")
        {
            return Data.GetMessage(MessageID, DefaultMessage);
        }

        public void SetTurnLock()
        {
            if(!PlayerControl && !PlayerMounted)
                TurnLock = TurnLockTime;
        }

        public Point[] UpdateTouchingTiles()
        {
            List<Point> TileList1 = new List<Point>(),
            TileList2 = new List<Point>();
            Vector2 Position = TopLeftPosition;
            if (!Collision.IsClearSpotTest(Position + Velocity, 16, Width, Height, false, false, GravityDirection, true, true))
            {
                TileList1 = Collision.FindCollisionTile(Math.Sign(Velocity.Y) == 1 ? 2 : 3, Position + Velocity, 16, Width, Height, false, false, GravityDirection, true, false);
            }
            if (!Collision.IsClearSpotTest(Position, Math.Abs(Velocity.Y), Width, Height, false, false, GravityDirection, true, true))
            {
                TileList2 = Collision.FindCollisionTile(Math.Sign(Velocity.Y) == 1 ? 2 : 3, Position + Velocity, Math.Abs(Velocity.Y), Width, Height, false, false, GravityDirection, true, true);
            }
            return TileList1.Union(TileList2).ToArray();
        }

        public void TryBouncingOnTiles(Point[] AdjTiles)
        {
            if (Math.Abs(Velocity.Y) >= 5f && !this.Wet)
            {
                int y = -1;
                foreach (Point current in AdjTiles)
                {
                    Tile tile = Main.tile[current.X, current.Y];
                    if (tile != null && tile.active() && tile.nactive() && Main.tileBouncy[tile.type])
                    {
                        y = current.Y;
                        break;
                    }
                }
                if (y > -1)
                {
                    Velocity.Y *= 0.8f;
                    if (Jump)
                        Velocity.Y = MathHelper.Clamp(Velocity.Y, -13, 13);
                    Position.Y = y * 16 - (Velocity.Y < 0 ? CollisionHeight : -16);
                    Velocity.Y = MathHelper.Clamp(Velocity.Y, -20f, 20);
                    if (Velocity.Y * GravityDirection < 0)
                    {
                        FallStart = (int)(Position.Y * DivisionBy16);
                    }
                }
            }
        }

        public void WhenLandingOnDetonatorScript()
        {
            if (OwnerPos != Main.myPlayer)
                return;
            if (Velocity.Y >= 3f)
            {
                Point point = (Position + new Vector2(0, 0.01f)).ToTileCoordinates();
                Tile tile = MainMod.GetTile(point.X, point.Y);
                if (tile.active() && tile.type == Terraria.ID.TileID.Detonator && tile.frameY == 0 && tile.frameX < 36)
                {
                    Wiring.HitSwitch(point.X, point.Y);
                    NetMessage.SendData(59, -1, -1, null, point.X, point.Y, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public void UpdateCollision()
        {
            if (HasFlag(GuardianFlags.Webbed))
            {
                Velocity = Vector2.Zero;
            }
            float LastPositionX = this.Position.X;
            int PositionX = (int)(this.Position.X * DivisionBy16);
            bool Fall = DropFromPlatform;
            Point[] TouchingTile = UpdateTouchingTiles();
            bool CanHurt = Main.netMode == 0 || OwnerPos == Main.myPlayer || (OwnerPos == -1 && Main.netMode == 2);
            Collision_Water(Collision_Lava());
            Collision_WalkDownSlopes();
            TryBouncingOnTiles(TouchingTile);
            WhenLandingOnDetonatorScript();
            if (HasFlag(GuardianFlags.WindPushed) && (Velocity.Y != 0 || !(MoveLeft || MoveRight)))
            {
                DoSandstormPushing();
            }
            if (Velocity.Y * GravityDirection == Mass)
            {
                Vector2 NewCollisionPosition = CollisionPosition;
                Collision.StepDown(ref NewCollisionPosition, ref Velocity, CollisionWidth, CollisionHeight, ref StepSpeed, ref gfxOffY, GravityDirection, HasFlag(GuardianFlags.WaterWalking));
                Position += NewCollisionPosition - CollisionPosition;
            }
            if (Velocity.Y * GravityDirection >= 0)
            {
                Vector2 NewCollisionPosition = CollisionPosition;
                Collision.StepUp(ref NewCollisionPosition, ref Velocity, CollisionWidth, CollisionHeight, ref StepSpeed, ref gfxOffY, GravityDirection, MoveUp, 0);
                Position += NewCollisionPosition - CollisionPosition;
            }
            if (!Wet)
            {
                LavaWet = HoneyWet = false;
            }
            else
            {
                SetFallStart();
            }
            Vector2 VelocityBackup = Velocity;
            StickyTileCollision();
            if (!HasFlag(GuardianFlags.NoTileCollision))
            {
                Velocity = Collision.TileCollision(CollisionPosition, Velocity, CollisionWidth, CollisionHeight, Fall, false, GravityDirection);
                if (HasFlag(GuardianFlags.WaterWalking))
                {
                    Vector2 CurVelocity = Velocity;
                    Velocity = Collision.WaterCollision(CollisionPosition, Velocity, CollisionWidth, CollisionHeight, Fall, false, true);
                    if (Velocity != CurVelocity)
                        FallStart = (int)(Position.Y * DivisionBy16);
                }
                if (!HasFlag(GuardianFlags.IceSkating) && !FallProtection)
                    CheckForIceBreak();
                Collision.SwitchTiles(CollisionPosition, CollisionWidth, CollisionHeight, CollisionPosition - Velocity, 1);
            }
            if (Wet)
            {
                if (HoneyWet)
                {
                    Collision_Move(VelocityBackup, 0.25f);
                }
                else
                {
                    if (!HasFlag(GuardianFlags.Merfolk))
                    {
                        Collision_Move(VelocityBackup, 0.5f);
                    }
                    else
                    {
                        Collision_Move(VelocityBackup, 1f);
                    }
                }
                if (HasFlag(GuardianFlags.UnderwaterJellyfishGlow))
                {
                    Lighting.AddLight((int)(Position.X * DivisionBy16), (int)(CenterY * DivisionBy16), 0.9f, 0.2f, 0.6f);
                }
            }
            else
            {
                Collision_Move(VelocityBackup, 1f);
            }
            Collision_MoveSlopesAndStairFall(Fall);
            Falling = false;
            if (!Jump && Velocity.Y != 0)
                Falling = true;
            if (OwnerPos == Main.myPlayer && !PlayerControl && !GrabbingPlayer && FriendshipLevel > 0 && AfkCounter < 60 * 60)
            {
                float Value = Velocity.X;
                if (MountedOnPlayer || SittingOnPlayerMount)
                    Value = Main.player[OwnerPos].velocity.X;
                TravellingStacker += Math.Abs(Value);
                float TravelDistance = 50000f * FriendshipLevel;
                if (TravellingStacker >= TravelDistance)
                {
                    TravellingStacker -= TravelDistance;
                    IncreaseFriendshipProgress(1);
                }
            }
            if (Velocity.Y == 0)
            {
                AddSkillProgress(Math.Abs(Velocity.X * 0.25f), GuardianSkills.SkillTypes.Athletic);
                int FallValue = (int)((Position.Y * DivisionBy16) - FallStart) * GravityDirection;
                if (!FallProtection && FallValue >= FallHeightTolerance && !HasFlag(GuardianFlags.FallDamageImmunity) && CanHurt)
                {
                    FallValue -= FallHeightTolerance;
                    bool ReduceFallDamage = HasFlag(GuardianFlags.FallDamageImpactReduction);
                    float FallVelocity = Velocity.Y;
                    bool PlayerWasMounted = PlayerMounted;
                    this.Hurt((int)((FallValue * 10 * HealthHealMult) * (ReduceFallDamage ? 0.5f : 1f)), 0, false, true, (Main.rand.NextDouble() < 0.5 ? " has reached the lobby." : " bounced back."));
                    if (PlayerWasMounted)
                    {
                        if (!Downed)
                        {
                            Main.player[OwnerPos].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[OwnerPos].name + " didn't braced for impact."), (int)((FallValue * 3) * (ReduceFallDamage ? 0.3f : 1f)), 0);
                        }
                        else
                        {
                            Main.player[OwnerPos].fallStart = (int)(Main.player[OwnerPos].Center.Y * DivisionBy16) - FallValue;
                            Main.player[OwnerPos].velocity.Y = FallVelocity;
                        }
                    }
                }
                ResetRocket();
                WingFlightTime = WingMaxFlightTime;
                FallStart = (int)(Position.Y * DivisionBy16);
                FallProtection = false;
            }
            else
            {
                if(!IsBeingPulledByPlayer)
                    AddSkillProgress(Math.Abs(Velocity.Y * 0.25f), GuardianSkills.SkillTypes.Acrobatic);
            }
            if (!DoAction.InUse || !DoAction.Immune)
            {
                UpdateLavaScript(CanHurt);
                UpdateDrowningScript(CanHurt);
                if(!MountedOnPlayer && !IsBeingPulledByPlayer)
                    CheckForHurtTileCollision(CanHurt);
            }
            WallSlideStyle = 0;
            if (CollisionX && !CollisionY && !SittingOnPlayerMount && !WalkMode && Velocity.Y >= 0)
            {
                int SolidWallsY = (int)(CollisionHeight * DivisionBy16);
                int CheckX = (int)((Position.X + (Width * 0.5f + 1) * Direction) * DivisionBy16);
                for (int y = -SolidWallsY; y < 0; y++)
                {
                    int CheckY = (int)(Position.Y * DivisionBy16) + y;
                    if (Main.tile[CheckX, CheckY].active() && Main.tileSolid[Main.tile[CheckX, CheckY].type])
                    {
                        SolidWallsY++;
                    }
                }
                if (SolidWallsY == 0)
                {
                    SlidingLeft = MoveLeft;
                    if (HasFlag(GuardianFlags.ClimbingClaws))
                        WallSlideStyle++;
                    if (HasFlag(GuardianFlags.ClimbingPaws))
                        WallSlideStyle++;
                    SetFallStart();
                }
            }
            DoorHandler();
            if (OwnerPos == Main.myPlayer && (MoveLeft || MoveRight) && !MoveDown)
            {
                if (MoveRight != LastMoveRight || MoveLeft != LastMoveLeft)
                {
                    StuckTimer = 0;
                }
                else if ((MoveRight && StuckCheckX < Position.X) || (MoveLeft && StuckCheckX > Position.X))
                {
                    SetStuckCheckPositionToMe();
                }
                else
                {
                    IncreaseStuckTimer();
                }
            }
        }

        public void SetStuckCheckPositionToMe()
        {
            StuckCheckX = Position.X;
        }

        public void CheckForIceBreak()
        {
            float VelocityToBreakIce = 7f;
            switch (Base.Size)
            {
                case GuardianBase.GuardianSize.Large:
                    VelocityToBreakIce *= 0.5f;
                    break;
                case GuardianBase.GuardianSize.Small:
                    VelocityToBreakIce *= 1.2f;
                    break;
            }
            if (Velocity.Y > VelocityToBreakIce)
            {
                int sx = (int)((CollisionPosition.X + Velocity.X) * DivisionBy16), ex = (int)((CollisionPosition.X + Velocity.X + CollisionWidth) * DivisionBy16),
                    py = (int)(Position.Y * DivisionBy16);
                for (int x = sx; x <= ex; x++)
                {
                    for (int y = py; y <= py + 1; y++)
                    {
                        if (Main.tile[x, y].nactive() && Main.tile[x, y].type == 162 && !WorldGen.SolidTile(x, y - 1))
                        {
                            WorldGen.KillTile(x, y);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)y, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
            }
        }

        public void StickyTileCollision()
        {
            Vector2 TilePosition = Collision.StickyTiles(CollisionPosition, Velocity, CollisionWidth, CollisionHeight);
            if (TilePosition.X > -1 && TilePosition.Y > -1)
            {
                int tx = (int)TilePosition.X, ty = (int)TilePosition.Y;
                int type = Main.tile[tx, ty].type;
                if (type == 51 && (Velocity.X != 0 || Velocity.Y != 0))
                {
                    if (Base.Size != GuardianBase.GuardianSize.Small)
                    {
                        IncreaseCooldownValue(GuardianCooldownManager.CooldownType.StickyTileBreak);
                        if (GetCooldownValue(GuardianCooldownManager.CooldownType.StickyTileBreak) > Main.rand.Next(20, 100))
                        {
                            RemoveCooldown(GuardianCooldownManager.CooldownType.StickyTileBreak);
                            WorldGen.KillTile(tx, ty, false, false, false);
                            //send to the server the change on tile
                            if (Main.netMode == 1 && !Main.tile[tx, ty].active() && Main.netMode == 1)
                            {
                                NetMessage.SendData(17, -1, -1, null, 0, (float)tx, (float)ty, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
                SetFallStart();
                switch (Base.Size)
                {
                    case GuardianBase.GuardianSize.Small:
                        Velocity = Vector2.Zero;
                        break;
                    case GuardianBase.GuardianSize.Medium:
                        if (Velocity.X > 1) Velocity.X = 1;
                        if (Velocity.X < -1) Velocity.X = -1;
                        if (Velocity.Y > 1) Velocity.Y = 1;
                        if (Velocity.Y < -5) Velocity.Y = -5;
                        break;
                    case GuardianBase.GuardianSize.Large:
                        {
                            float MaxSpeed = this.MaxSpeed * 0.33f;
                            if (Velocity.X > MaxSpeed) Velocity.X = MaxSpeed;
                            if (Velocity.X < -MaxSpeed) Velocity.X = -MaxSpeed;
                            if (Velocity.Y > MaxSpeed) Velocity.Y = MaxSpeed;
                            if (Velocity.Y < -5) Velocity.Y = -5;
                        }
                        break;
                }
                if(type != 229)
                {
                    JumpHeight = 0;
                    return;
                }
            }
        }

        public void GetItem(int ID, int Stack)
        {
            for (int i = 0; i < 50; i++)
            {
                if (this.Inventory[i].type == ID)
                {
                    int StackToGet = this.Inventory[i].maxStack - this.Inventory[i].stack;
                    if (StackToGet > Stack)
                        StackToGet = Stack;
                    this.Inventory[i].stack += StackToGet;
                    Stack -= StackToGet;
                    if (Stack == 0)
                        return;
                }
            }
            for (int i = 0; i < 50; i++)
            {
                if (this.Inventory[i].type == 0)
                {
                    this.Inventory[i].SetDefaults(ID, true);
                    this.Inventory[i].stack = Stack;
                    return;
                }
            }
        }

        public void ResetRocket()
        {
            RocketTime = RocketMaxTime;
            RemoveCooldown(GuardianCooldownManager.CooldownType.RocketDelay);
        }

        public void RocketMovement(bool MayRocket)
        {
            bool CanRocket = Velocity.Y * GravityDirection > -JumpSpeed && Velocity.Y != 0;
            if ((WingFlightTime == 0 || WingType == 0) && RocketType > 0 && CanRocket)
            {
                if (Jump && (MayRocket || RocketTime < RocketMaxTime) && !HasCooldown(GuardianCooldownManager.CooldownType.RocketDelay) && ExtraJump == GetExtraJumpCount && JumpHeight <= 0)
                {
                    if (RocketTime > 0)
                    {
                        IgnoreMass = true;
                        RocketTime--;
                        AddCooldown(GuardianCooldownManager.CooldownType.RocketDelay, 10);
                        if (!HasCooldown(GuardianCooldownManager.CooldownType.RocketSoundDelay))
                        {
                            if (RocketType == 1)
                            {
                                Main.PlaySound(Terraria.ID.SoundID.Item13, CenterPosition);
                                AddCooldown(GuardianCooldownManager.CooldownType.RocketSoundDelay, 30);
                            }
                            else if (RocketType == 2 || RocketType == 3)
                            {
                                Main.PlaySound(Terraria.ID.SoundID.Item24, CenterPosition);
                                AddCooldown(GuardianCooldownManager.CooldownType.RocketSoundDelay, 15);
                            }
                        }
                    }
                }
            }
            if (HasCooldown(GuardianCooldownManager.CooldownType.RocketDelay))
            {
                int SpawnY = 0;
                if (GravityDirection < 0)
                    SpawnY = -Height;
                for (int i = -1; i < 2; i += 2)
                {
                    int Type = 6;
                    float Scale = 2.5f;
                    int Alpha = 100;
                    if (RocketType == 2)
                    {
                        Type = 16;
                        Scale = 1.5f;
                        Alpha = 20;
                    }
                    else if (RocketType == 3)
                    {
                        Type = 76;
                        Scale = 1f;
                    }
                    int dust = Dust.NewDust(new Vector2(Position.X + (Width - 2) * i, Position.Y + SpawnY - 10 * GravityDirection), 8, 8, Type, 0f,0f, Alpha, default(Color), Scale);
                    Dust d = Main.dust[dust];
                    if (RocketType == 1)
                    {
                        d.noGravity = true;
                    }
                    d.velocity.X = 2f * i - Velocity.X * 0.3f;
                    d.velocity.Y = 2f * GravityDirection - Velocity.Y * 0.3f;
                    if (RocketType == 2)
                    {
                        d.velocity *= 0.1f;
                    }
                    if (RocketType == 3)
                    {
                        d.velocity *= 0.05f;
                        d.velocity.Y += 0.15f;
                        d.noLight = true;
                        if (Main.rand.Next(2) == 0)
                        {
                            d.noGravity = true;
                            d.scale = 1.75f;
                        }
                    }
                }
                float YBonus = 0.2f * AirHeightBonus;//(Mass - 0.4f);
                /*if (Base.Size == GuardianBase.GuardianSize.Large)
                    YBonus *= 2;
                else if (Base.Size == GuardianBase.GuardianSize.Small)
                    YBonus *= 0.75f;*/
                Velocity.Y -= (0.1f + YBonus) * GravityDirection;
                if (Velocity.Y * GravityDirection > 0f)
                {
                    Velocity.Y -= (0.5f + YBonus) * GravityDirection;
                }
                else if (Velocity.Y * GravityDirection > -JumpSpeed * 0.5f)
                {
                    Velocity.Y -= (0.1f + YBonus) * GravityDirection;
                }
                if (Velocity.Y * GravityDirection < -JumpSpeed * 1.5f)
                {
                    Velocity.Y = -JumpSpeed * 1.5f * GravityDirection;
                }
                SetFallStart();
            }
        }

        public void WingMovement()
        {
            if (WingType > 0 && RocketType > 0 && Velocity.Y != 0)
            {
                const int Increase = 6;
                WingFlightTime += RocketTime * Increase;
                if (WingFlightTime > WingMaxFlightTime + RocketMaxTime * Increase)
                {
                    WingFlightTime = WingMaxFlightTime + RocketMaxTime * Increase;
                }
                RocketTime = 0;
            }
            bool DoWingMovement = false;
            if (WingType > 0 && (!Wet || (!HasFlag(GuardianFlags.SwimmingAbility) && !HasFlag(GuardianFlags.SwimmingAbility))) && Jump && JumpHeight == 0 && ExtraJump == GetExtraJumpCount && Velocity.Y != 0)
                DoWingMovement = true;
            if ((WingType == 22 || WingType == 28 || WingType == 30 || WingType == 32 || WingType == 29 || WingType == 33 || WingType == 35 || WingType == 37) && Jump && MoveDown && WingFlightTime > 0f)
                DoWingMovement = true;
            if (WingType > 0)
                SetFallStart();
            if (DoWingMovement)
            {
                if (WingFlightTime > 0)
                {
                    float num = 0.1f;
                    float num2 = 0.5f;
                    float num3 = 1.5f;
                    float num4 = 0.5f;
                    float num5 = 0.1f;
                    if (WingType == 26)
                    {
                        num2 = 0.75f;
                        num5 = 0.15f;
                        num4 = 1f;
                        num3 = 2.5f;
                        num = 0.125f;
                    }
                    if (WingType == 37)
                    {
                        num2 = 0.75f;
                        num5 = 0.15f;
                        num4 = 1f;
                        num3 = 2.5f;
                        num = 0.125f;
                    }
                    if (WingType == 29 || WingType == 32)
                    {
                        num2 = 0.85f;
                        num5 = 0.15f;
                        num4 = 1f;
                        num3 = 3f;
                        num = 0.135f;
                    }
                    if (WingType == 30 || WingType == 31)
                    {
                        num4 = 1f;
                        num3 = 2f;
                        num = 0.15f;
                    }
                    //num *= Mass;
                    //num2 *= Mass;
                    //num3 *= Mass;
                    //num4 *= Mass;
                    //num5 *= Mass;
                    Velocity.Y -= Mass * GravityDirection;
                    Velocity.Y -= num * GravityDirection;
                    if (Velocity.Y * GravityDirection > 0f)
                    {
                        Velocity.Y -= num2 * GravityDirection;
                    }
                    else if (Velocity.Y * GravityDirection > -JumpSpeed * num4)
                    {
                        Velocity.Y -= num5 * GravityDirection;
                    }
                    if (Velocity.Y * GravityDirection < -JumpSpeed * num3)
                        Velocity.Y = -JumpSpeed * num3 * GravityDirection;
                    WingFlightTime--;
                }
                else
                {
                    Velocity.Y *= 0.9f;
                }
            }
            if (WingType != 4 && WingType != 22 && WingType != 0 && WingType != 24 && WingType != 28 && WingType != 30 && WingType != 33)
            {
                if (WingFrame == 3)
                {
                    if (!WingFlapSound)
                        Main.PlaySound(Terraria.ID.SoundID.Item32, CenterPosition);
                    WingFlapSound = true;
                }
                else
                {
                    WingFlapSound = false;
                }
            }
        }

        public void CheckForHurtTileCollision(bool CanHurt)
        {
            if (BodyAnimationFrame == Base.SittingFrame || BodyAnimationFrame == Base.ChairSittingFrame)
                return;
            bool AppliedDamage = false;
            bool FireImmune = HasFlag(GuardianFlags.FireblocksImmunity);
            Vector2 TileInfo = Collision.HurtTiles(CollisionPosition, Velocity, CollisionWidth, CollisionHeight, FireImmune);
            if (HasCooldown(GuardianCooldownManager.CooldownType.TileHurt))
            {
                return;
            }
            if (TileInfo.Y == 20 && !FireImmune)
            {
                AddBuff(67, 20, true);
            }
            else if (TileInfo.Y == 15)
            {
                if (GetCooldownValue(GuardianCooldownManager.CooldownType.SuffocationDelay) < 5)
                    IncreaseCooldownValue(GuardianCooldownManager.CooldownType.SuffocationDelay);
                else
                   AddBuff(68, 10, true);
            }
            else if (CanHurt && TileInfo.Y != 0)
            {
                string DeathMessage = " got tired of dancing on the spikes.";
                if (PlayerMounted)
                    DeathMessage = "'s last thing said was \"Watch where are we going!\"";
                else if (Main.rand.Next(3) == 0)
                    DeathMessage = " said that bringing a radio didn't helped at all.";
                this.Hurt((int)(TileInfo.Y * HealthHealMult), 0, false, true, DeathMessage);
                AppliedDamage = true;
            }
            if (AppliedDamage)
            {
                AddCooldown(GuardianCooldownManager.CooldownType.TileHurt, DefaultImmuneTime);
            }
        }

        public void UpdateLavaScript(bool CanHurt)
        {
            if (WofFood)
                return;
            bool Tolerance = false;
            if (HasFlag(GuardianFlags.LavaTolerance))
            {
                if (LavaWet)
                {
                    if (GetCooldownValue(GuardianCooldownManager.CooldownType.LavaToleranceCooldown) < 60 * 7)
                    {
                        IncreaseCooldownValue(GuardianCooldownManager.CooldownType.LavaToleranceCooldown);
                        Tolerance = true;
                    }
                    else
                    {
                        Tolerance = false;
                    }
                }
            }
            if (CanHurt && LavaWet && !Tolerance && !HasFlag(GuardianFlags.LavaImmunity) && !HasCooldown(GuardianCooldownManager.CooldownType.LavaHurt) && !IsBeingPulledByPlayer)
            {
                if (HasFlag(GuardianFlags.LavaTolerance))
                {
                    AddBuff(24, 210);
                }
                else
                {
                    AddBuff(24, 420);
                }
                int Damage = 80;
                if (HasFlag(GuardianFlags.LavaDamageReduction))
                    Damage = 50;
                this.Hurt((int)(Damage * HealthHealMult), 0, false, true, " melted, giving a thumbs up while sinking.");
                AddCooldown(GuardianCooldownManager.CooldownType.LavaHurt, GetImmuneTime);
            }
        }

        public void UpdateDrowningScript(bool CanHurt)
        {
            bool UnderLiquid = Collision.DrownCollision(TopLeftPosition, Width, Height, GravityDirection);
            bool RestoreBreath = true;
            Drowning = false;
            if (UnderLiquid)
            {
                bool MayDrown = !HasFlag(GuardianFlags.BreathUnderwater) && !HasFlag(GuardianFlags.Merfolk);
                if (MayDrown)
                {
                    Drowning = true;
                    RestoreBreath = false;
                    this.BreathCooldown++;
                    int MaxBreathCooldown = Base.BreathCooldown;
                    if (BreathCooldown >= MaxBreathCooldown)
                    {
                        BreathCooldown -= MaxBreathCooldown;
                        Breath--;
                        if (Breath == 0)
                        {
                            Main.PlaySound(23, this.CenterPosition);
                        }
                        if (this.Breath <= 0)
                        {
                            this.ResetHealthRegen();
                            this.Breath = 0;
                            this.HP -= 6;
                            if (CanHurt && this.HP <= 0)
                            {
                                this.Knockout(" couldn't hold more the breath.");
                            }
                        }
                    }

                    if (!LavaWet && !HoneyWet && Main.rand.Next(20) == 0)
                    {
                        Vector2 SpawnPosition = CenterPosition;
                        SpawnPosition.X += (SpriteWidth * 0.25f - 4f) * Direction;
                        SpawnPosition.Y -= SpriteHeight * 0.25f + 4f;
                        Dust.NewDust(SpawnPosition, 8, 8, 34, 0f, 0f, 0, default(Color), 1.2f);
                    }
                }
            }
            if(RestoreBreath)
            {
                this.Breath += 3;
                if (this.Breath > BreathMax)
                    this.Breath = BreathMax;
                BreathCooldown = 0;
            }
        }

        public void IncreaseFriendshipProgress(byte Value)
        {
            FriendshipProgression += Value;
        }

        public void CheckForLifeCrystals()
        {
            if (ItemAnimationTime > 0 || (Main.playerInventory && MainMod.SelectedGuardianInventorySlot == MainMod.GuardianItemSlotButtons.Inventory) || IsAttackingSomething || HasCooldown(GuardianCooldownManager.CooldownType.DelayedActionCooldown))
            {
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                if (this.Inventory[i].newAndShiny)
                    continue;
                if ((this.Inventory[i].type == Terraria.ID.ItemID.LifeCrystal || (this.Inventory[i].type == ModContent.ItemType<Items.Consumable.EtherHeart>() && Base.IsTerraGuardian)) && GetUsedLifeCrystal < MaxLifeCrystals)
                {
                    this.SelectedItem = i;
                    this.Action = true;
                    break;
                }
                if ((this.Inventory[i].type == Terraria.ID.ItemID.LifeFruit || (this.Inventory[i].type == ModContent.ItemType<Items.Consumable.EtherFruit>() && Base.IsTerraGuardian)) && GetUsedLifeCrystal == MaxLifeCrystals && GetUsedLifeFruit < MaxLifeFruit)
                {
                    this.SelectedItem = i;
                    this.Action = true;
                    break;
                }
                if (this.Inventory[i].type == Terraria.ID.ItemID.ManaCrystal && GetUsedManaCrystal < GuardianData.MaxManaCrystals)
                {
                    SelectedItem = i;
                    Action = true;
                    break;
                }
            }
        }

        public void IncreaseStuckTimer()
        {
            if (!StuckTimerChanged && Paths.Count == 0 && !KnockedOut && !KnockedOutCold)
            {
                if (!IsBeingControlledByPlayer)
                    StuckTimer++;
                StuckTimerChanged = true;
            }
        }

        public void TeleportHome()
        {
            int HouseX = -1, HouseY = -1;
            WorldMod.GuardianTownNpcState TownNpcInfo = GetTownNpcInfo;
            if (TownNpcInfo != null)
            {
                if (!TownNpcInfo.Homeless)
                {
                    HouseX = TownNpcInfo.HomeX * 16;
                    HouseY = TownNpcInfo.HomeY * 16;
                }
                else
                {
                    HouseX = (int)Position.X;
                    HouseY = (int)Position.Y;
                }
            }
            Position.X = HouseX;
            Position.Y = HouseY;
            SetFallStart();
            IdleActionTime = 1;
        }

        public void TeleportToGuardedPosition()
        {
            if (GuardingPosition.HasValue && !WofFacing)
            {
                Position = new Vector2(GuardingPosition.Value.X * 16, GuardingPosition.Value.Y * 16);
                Velocity = Vector2.Zero;
                ImmuneTime = GetImmuneTime;
                FallProtection = true;
                SetFallStart();
                SetAimPositionToCenter();
                SetStuckCheckPositionToMe();
            }
        }

        public void TeleportToPlayer(bool force = false, Player player = null)
        {
            if (player == null)
            {
                if (OwnerPos > -1)
                    player = Main.player[OwnerPos];
                else
                    return;
            }
            if (!force && (PlayerMounted || PlayerControl || SittingOnPlayerMount))
                return;
            if (!player.gross && WofFacing)
            {
                return;
            }
            IsBeingPulledByPlayer = false;
            if (Data.SitOnTheMount && player.mount.Active && !PlayerMounted && !PlayerControl)
                DoSitOnPlayerMount(true);
            Position = new Vector2(player.Center.X, player.position.Y + player.height);
            Velocity = player.velocity;
            ImmuneTime = GetImmuneTime;
            FallProtection = true;
            SetFallStart();
            SetAimPositionToCenter();
            SetStuckCheckPositionToMe();
            TargetID = -1;
            AttackingTarget = false;
        }

        private void Collision_Move(Vector2 OldDryVelocity, float SpeedPercentage = 0.5f)
        {
            if ((Collision.up && GravityDirection > 0) || (Collision.down && GravityDirection < 0))
                this.Velocity.Y = 0.01f * GravityDirection;
            Vector2 NewVelocity = this.Velocity * SpeedPercentage;
            CollisionX = CollisionY = false;
            if (OldDryVelocity.X != Velocity.X)
            {
                NewVelocity.X = this.Velocity.X;
                CollisionX = true;
            }
            if (OldDryVelocity.Y != Velocity.Y)
            {
                NewVelocity.Y = this.Velocity.Y;
                CollisionY = true;
            }
            if(!SittingOnPlayerMount && !(PlayerMounted && ReverseMount))
                Position = Position + NewVelocity;
        }

        public void Collision_WalkDownSlopes()
        {
            Vector4 NewPosition = Collision.WalkDownSlope(this.CollisionPosition, this.Velocity, CollisionWidth, CollisionHeight);
            Position += PositionDifference(NewPosition.XY());
            Velocity = NewPosition.ZW();
        }

        public void Collision_MoveSlopesAndStairFall(bool Fall)
        {
            Vector2 cPosition = CollisionPosition;
            int cWidth = CollisionWidth;
            int cHeight = CollisionHeight;
            Vector4 SlopeCollision = Collision.SlopeCollision(cPosition, Velocity, cWidth, cHeight, Mass, Fall);
            if (Collision.stair && Math.Abs(SlopeCollision.Y - Position.Y) > 8f)
            {
                if(!HasFlag(GuardianFlags.IgnoreGfx))
                    gfxOffY -= SlopeCollision.Y - cPosition.Y;
                StepSpeed = 4;
            }
            this.Position += PositionDifference(SlopeCollision.XY());
            this.Velocity = SlopeCollision.ZW();
        }

        public bool Collision_Water(bool Lava)
        {
            bool WaterCollision = Collision.WetCollision(CollisionPosition, CollisionWidth, CollisionHeight);
            bool LastHoneyWet = HoneyWet;
            if (WaterCollision && Collision.honey)
                HoneyWet = true;
            bool WasLastWet = Wet;
            Wet = false;
            if (WaterCollision)
            {
                if (!WasLastWet)
                {
                    if (!Lava)
                    {
                        if (this.HoneyWet)
                        {
                            for (int index1 = 0; index1 < 10; ++index1)
                            {
                                int index2 = Dust.NewDust(new Vector2(TopLeftPosition.X - 6f, (float)((double)this.TopLeftPosition.Y + (double)(HitBox.Height / 2) - 8.0)), HitBox.Width + 12, 24, 152, 0.0f, 0.0f, 0, new Color(), 1f);
                                --Main.dust[index2].velocity.Y;
                                Main.dust[index2].velocity.X *= 2.5f;
                                Main.dust[index2].scale = 1.3f;
                                Main.dust[index2].alpha = 100;
                                Main.dust[index2].noGravity = true;
                            }
                            Main.PlaySound(19, (int)this.Position.X, (int)this.Position.Y, 1, 1f, 0.0f);
                        }
                        else
                        {
                            for (int index1 = 0; index1 < 30; ++index1)
                            {
                                int index2 = Dust.NewDust(new Vector2(TopLeftPosition.X - 6f, (float)((double)TopLeftPosition.Y + (double)(HitBox.Height / 2) - 8.0)), HitBox.Width + 12, 24, Dust.dustWater(), 0.0f, 0.0f, 0, new Color(), 1f);
                                Main.dust[index2].velocity.Y -= 4f;
                                Main.dust[index2].velocity.X *= 2.5f;
                                Main.dust[index2].scale *= 0.8f;
                                Main.dust[index2].alpha = 100;
                                Main.dust[index2].noGravity = true;
                            }
                            Main.PlaySound(19, (int)Position.X, (int)Position.Y, 0, 1f, 0.0f);
                        }
                    }
                    else
                    {
                        for (int index1 = 0; index1 < 10; ++index1)
                        {
                            int index2 = Dust.NewDust(new Vector2(TopLeftPosition.X - 6f, (float)((double)TopLeftPosition.Y + (double)(HitBox.Height / 2) - 8.0)), HitBox.Width + 12, 24, 35, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].velocity.Y -= 1.5f;
                            Main.dust[index2].velocity.X *= 2.5f;
                            Main.dust[index2].scale = 1.3f;
                            Main.dust[index2].alpha = 100;
                            Main.dust[index2].noGravity = true;
                        }
                        Main.PlaySound(19, (int)Position.X, (int)Position.Y, 1, 1f, 0.0f);
                    }
                }
                if (!LavaWet && Main.expertMode && ZoneSnow)
                {
                    AddBuff(Terraria.ID.BuffID.Chilled, 5 * 60);
                }
                Wet = true;
            }
            if (HoneyWet)
            {
                AddBuff(48, 1800, true);
            }
            if (Wet && !LavaWet && HasFlag(GuardianFlags.OnFire))
            {
                RemoveBuff(24);
            }
            if (Wet && !LavaWet && HasFlag(GuardianFlags.MerfolkAcc))
            {
                AddBuff(34, 2, true);
            }
            if (!Wet && !LastHoneyWet && WasLastWet && !LavaWet)
                AddBuff(Terraria.ID.BuffID.Wet, 300);
            return WaterCollision;
        }

        public bool Collision_Lava()
        {
            bool LavaCollision = Collision.LavaCollision(CollisionPosition, CollisionWidth, CollisionHeight);
            if (LavaCollision)
            {
                this.LavaWet = true;
            }
            else
            {
                LavaWet = false;
            }
            return LavaCollision;
        }

        public void DrawFriendshipHeart(Vector2 Position, byte CustomLevel = 255, float CustomProgress = -1, float Opacity = 1f)
        {
            Main.spriteBatch.Draw(MainMod.FriendshipHeartTexture, Position, new Rectangle(0, 0, 24, 24), Color.White * Opacity);
            if (CustomLevel == 255)
            {
                CustomLevel = FriendshipLevel;
                CustomProgress = (float)FriendshipProgression / MaxFriendshipProgression;
            }
            if (CustomLevel >= MaxFriendshipLevel)
                CustomProgress = 1f;
            Position.X += 2;
            Position.Y += 2;
            int ProgressValue = (int)(CustomProgress * 20);
            Main.spriteBatch.Draw(MainMod.FriendshipHeartTexture, Position + new Vector2(0, 20 - ProgressValue), new Rectangle(24 + 2, 2 + 20 - ProgressValue, 20, ProgressValue), Color.White * Opacity);
            Position.X += 10;
            Position.Y += 12;
            Utils.DrawBorderString(Main.spriteBatch, FriendshipLevel.ToString(), Position, Color.White * Opacity, 0.75f, 0.5f, 0.5f);
        }

        public void GetEquipmentSlots()
        {
            HeadSlot = Equipments[0].headSlot;
            DrawHair = HeadSlot <= 0 || HeadSlot == 10 || HeadSlot == 12 || HeadSlot == 28 || HeadSlot == 62 || HeadSlot == 97 || HeadSlot == 106 || HeadSlot == 113 || HeadSlot == 116 || HeadSlot == 119 || HeadSlot == 133 || HeadSlot == 138 || HeadSlot == 139 || HeadSlot == 163 || HeadSlot == 178 || HeadSlot == 181 || HeadSlot == 191 || HeadSlot == 198;
            DrawAltHair = HeadSlot == 161 || HeadSlot == 14 || HeadSlot == 15 || HeadSlot == 16 || HeadSlot == 18 || HeadSlot == 21 || HeadSlot == 24 || HeadSlot == 25 || HeadSlot == 26 || HeadSlot == 40 || HeadSlot == 44 || HeadSlot == 51 || HeadSlot == 56 || HeadSlot == 59 || HeadSlot == 60 || HeadSlot == 67 || HeadSlot == 68 || HeadSlot == 69 || HeadSlot == 114 || HeadSlot == 121 || HeadSlot == 126 || HeadSlot == 130 || HeadSlot == 136 || HeadSlot == 140 || HeadSlot == 145 || HeadSlot == 158 || HeadSlot == 159 || HeadSlot == 184 || HeadSlot == 190 || HeadSlot == 92 || HeadSlot == 195;
            /*if (!DrawHair && HeadSlot != 23 && HeadSlot != 14 && HeadSlot != 56 && HeadSlot != 158 && HeadSlot != 28 && HeadSlot != 201)
            {
                DrawHair = true;
                DrawAltHair = false;
            }*/
            if (Equipments[0].type != 0 && Equipments[0].headSlot > 0)
            {
                if (this.Equipments[0].modItem != null)
                {
                    this.Equipments[0].modItem.DrawHair(ref DrawHair, ref DrawAltHair);
                }
            }
            LegSlot = Equipments[2].legSlot;
            DrawLegs = true;
            if (Equipments[2].type != 0 && Equipments[2].legSlot > 0)
            {
                if (this.Equipments[2].modItem != null)
                {
                    DrawLegs = this.Equipments[2].modItem.DrawLegs();
                }
            }
            BodySlot = Equipments[1].bodySlot;
            DrawBody = true;
            DrawArms = DrawHands = BodySlot <= 0;
            if (Equipments[1].type != 0 && Equipments[1].bodySlot > 0)
            {
                if (this.Equipments[1].modItem != null)
                {
                    DrawBody = this.Equipments[1].modItem.DrawBody();
                    DrawLegs = this.Equipments[1].modItem.DrawLegs();
                    this.Equipments[1].modItem.DrawHands(ref DrawHands, ref DrawArms);
                }
            }
            if (Base.IsTerrarian)
            {
                GuardianBase.TerrarianCompanionInfos tci = Base.TerrarianInfo;
                if (HeadSlot == -1 && tci.DefaultHelmet > 0)
                {
                    HeadSlot = tci.DefaultHelmet;
                }
                if (BodySlot == -1 && tci.DefaultArmor > 0)
                {
                    BodySlot = tci.DefaultArmor;
                }
                if (LegSlot == -1 && tci.DefaultLeggings > 0)
                {
                    LegSlot = tci.DefaultLeggings;
                }
            }
            bool werewolf = HasFlag(GuardianFlags.Werewolf), merfolk = HasFlag(GuardianFlags.Merfolk);
            for (int i = 0; i < 10; i++)
            {
                if (this.Inventory[i].type != 0)
                {
                    if (this.Inventory[i].headSlot > 0)
                    {
                        HeadSlot = this.Inventory[i].headSlot;
                        DrawHair = HeadSlot <= 0 || HeadSlot == 10 || HeadSlot == 12 || HeadSlot == 28 || HeadSlot == 62 || HeadSlot == 97 || HeadSlot == 106 || HeadSlot == 113 || HeadSlot == 116 || HeadSlot == 119 || HeadSlot == 133 || HeadSlot == 138 || HeadSlot == 139 || HeadSlot == 163 || HeadSlot == 178 || HeadSlot == 181 || HeadSlot == 191 || HeadSlot == 198;
                        DrawAltHair = HeadSlot == 161 || HeadSlot == 14 || HeadSlot == 15 || HeadSlot == 16 || HeadSlot == 18 || HeadSlot == 21 || HeadSlot == 24 || HeadSlot == 25 || HeadSlot == 26 || HeadSlot == 40 || HeadSlot == 44 || HeadSlot == 51 || HeadSlot == 56 || HeadSlot == 59 || HeadSlot == 60 || HeadSlot == 67 || HeadSlot == 68 || HeadSlot == 69 || HeadSlot == 114 || HeadSlot == 121 || HeadSlot == 126 || HeadSlot == 130 || HeadSlot == 136 || HeadSlot == 140 || HeadSlot == 145 || HeadSlot == 158 || HeadSlot == 159 || HeadSlot == 184 || HeadSlot == 190 || HeadSlot == 92 || HeadSlot == 195;
                        if (this.Inventory[i].modItem != null)
                        {
                            this.Inventory[i].modItem.DrawHair(ref DrawHair, ref DrawAltHair);
                        }
                    }
                    if (this.Inventory[i].bodySlot > 0)
                    {
                        BodySlot = this.Inventory[i].bodySlot;
                        DrawArms = DrawHands = false;
                        if (this.Inventory[i].modItem != null)
                        {
                            DrawBody = this.Inventory[i].modItem.DrawBody();
                            this.Inventory[i].modItem.DrawHands(ref DrawHands, ref DrawArms);
                        }
                    }
                    if (this.Inventory[i].legSlot > 0)
                    {
                        if (this.Inventory[i].modItem != null)
                        {
                            DrawLegs = this.Inventory[i].modItem.DrawLegs();
                        }
                        LegSlot = this.Inventory[i].legSlot;
                    }
                    if (this.Inventory[i].type == Terraria.ID.ItemID.MoonCharm)
                    {
                        werewolf = true;
                        merfolk = false;
                    }
                    if (this.Inventory[i].type == Terraria.ID.ItemID.NeptunesShell)
                    {
                        merfolk = true;
                        werewolf = false;
                    }
                }
            }
            if (!HideWereForm)
            {
                if (werewolf)
                {
                    HeadSlot = 38;
                    BodySlot = 21;
                    LegSlot = 20;
                    DrawHair = DrawAltHair = false;
                }
                if (merfolk)
                {
                    HeadSlot = 39;
                    BodySlot = 22;
                    LegSlot = 21;
                    DrawHair = DrawAltHair = false;
                }
            }
            if(HeadSlot == Terraria.ID.ArmorIDs.Head.LamiaFemale || HeadSlot == Terraria.ID.ArmorIDs.Head.LamiaMale)
            {
                DrawHair = DrawAltHair = false;
                DrawArms = DrawHands = true;
            }
        }

        public bool IsValidHeadVanity(int headID)
        {
            switch (headID)
            {
                case 23:
                case 28:
                case 62:
                case 97:
                case 106:
                case 113:
                case 116:
                case 119:
                case 133:
                case 138:
                case 139:
                case 163:
                case 178:
                case 181:
                case 191:
                case 198:
                case 161:
                case 14:
                case 15:
                case 16:
                case 18:
                case 21:
                case 24:
                case 25:
                case 26:
                case 40:
                case 44:
                case 51:
                case 56:
                case 59:
                case 60:
                case 67:
                case 68:
                case 69:
                case 114:
                case 121:
                case 126:
                case 130:
                case 136:
                case 140:
                case 145:
                case 158:
                case 159:
                case 184:
                case 190:
                case 92:
                case 195:
                case 63:
                case 94:
                case 95:
                case 143:
                case 199:
                case 81:
                case 144:
                case 96:
                case 182:
                case 64:
                case 100:
                case 65:
                    return true;
            }
            return false;
        }

        public bool IsValidHeadAccessoryVanity(int accID)
        {
            switch (accID)
            {
                case 1:
                case 7:
                    return true;
            }
            return false;
        }

        public int ReturnEquippableHeadVanityEquip(out bool IsAccessory)
        {
            IsAccessory = false;
            int VanityID = 0;
            if (this.Equipments[0].type != 0)
            {
                if (IsValidHeadVanity(this.Equipments[0].headSlot))
                {
                    VanityID = this.Equipments[0].headSlot;
                    IsAccessory = false;
                }
            }
            for (int eq = 3; eq < 8; eq++)
            {
                if (this.Equipments[eq].type != 0 && this.Equipments[eq].faceSlot > 0 && (IsValidHeadAccessoryVanity(this.Equipments[eq].faceSlot)))
                {
                    VanityID = this.Equipments[eq].faceSlot;
                    IsAccessory = true;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (this.Inventory[i].type != 0)
                {
                    if (this.Inventory[i].headSlot > 0 && (IsValidHeadVanity(this.Inventory[i].headSlot)))
                    {
                        VanityID = this.Inventory[i].headSlot;
                        IsAccessory = false;
                    }
                    if (this.Inventory[i].faceSlot > 0 && (IsValidHeadAccessoryVanity(this.Inventory[i].faceSlot)))
                    {
                        VanityID = this.Inventory[i].faceSlot;
                        IsAccessory = true;

                    }
                }
            }
            return VanityID;
        }

        public void AddDrawDataAfter(GuardianDrawData dd, int Position, bool InFrontOfPlayer)
        {
            if (dd.Texture == null)
                return;
            //if (!MountedOnPlayer || Downed)
            //    dd.IgnorePlayerRotation = true;
            if (InFrontOfPlayer)
            {
                if (Position + 1 >= DrawFront.Count)
                {
                    DrawFront.Add(dd);
                }
                else
                {
                    DrawFront.Insert(Position + 1, dd);
                }
            }
            else
            {
                if (Position + 1 >= DrawBehind.Count)
                {
                    DrawBehind.Add(dd);
                }
                else
                {
                    DrawBehind.Insert(Position + 1, dd);
                }
            }
        }

        /// <summary>
        /// Gets the position of the texture by type.
        /// </summary>
        /// <param name="TT">The type of the texture.</param>
        /// <param name="Position">Position in the array.</param>
        /// <param name="Front">Retuns true if It's a DrawFront sprite.</param>
        /// <returns>Returns true when finds the body part.</returns>
        public bool GetTextureSpritePosition(GuardianDrawData.TextureType TT, out int Position, out bool Front)
        {
            Position = 0;
            Front = true;
            for (int i = 0; i < DrawFront.Count; i++)
            {
                if (DrawFront[i].textureType == TT)
                {
                    Position = i;
                    return true;
                }
            }
            Front = false;
            for (int i = 0; i < DrawBehind.Count; i++)
            {
                if (DrawBehind[i].textureType == TT)
                {
                    Position = i;
                    return true;
                }
            }
            return false;
        }

        public void AddDrawData(GuardianDrawData dd, bool InFrontOfPlayer)
        {
            if (dd.Texture == null)
                return;
            //if (!MountedOnPlayer || Downed)
            //    dd.IgnorePlayerRotation = true;
            if (InFrontOfPlayer)
            {
                DrawFront.Add(dd);
            }
            else
            {
                DrawBehind.Add(dd);
            }
        }

        public void InsertDrawData(GuardianDrawData dd, int Position, bool InFrontOfPlayer)
        {
            if (dd.Texture == null)
                return;
            //if (!MountedOnPlayer || Downed)
            //    dd.IgnorePlayerRotation = true;
            if (InFrontOfPlayer)
            {
                DrawFront.Insert(Position, dd);
            }
            else
            {
                DrawBehind.Insert(Position, dd);
            }
        }

        public void TryToLoadGuardianEquipments(ref int HeadSlot, ref int ArmorSlot, ref int LegSlot, ref int FaceSlot, ref int FrontSlot, ref int BackSlot)
        {
            bool DoCache = false;
            Player dummy = Main.player[255];
            if (HeadSlot > 0 && !Main.armorHeadLoaded[HeadSlot])
            {
                dummy.head = HeadSlot;
                DoCache = true;
            }
            if (ArmorSlot > 0 && !Main.armorBodyLoaded[ArmorSlot])
            {
                dummy.body = ArmorSlot;
                DoCache = true;
            }
            if (LegSlot > 0 && !Main.armorLegsLoaded[LegSlot])
            {
                dummy.legs = LegSlot;
                DoCache = true;
            }
            if (FaceSlot > 0 && !Main.accFaceLoaded[FaceSlot])
            {
                dummy.face = (sbyte)FaceSlot;
                DoCache = true;
            }
            if (FrontSlot > 0 && !Main.accFrontLoaded[FrontSlot])
            {
                dummy.front = (sbyte)FrontSlot;
                DoCache = true;
            }
            if (BackSlot > 0 && !Main.accBackLoaded[BackSlot])
            {
                dummy.back = (sbyte)BackSlot;
                DoCache = true;
            }
            if (WingType > 0 && !Main.wingsLoaded[WingType])
            {
                dummy.wings = WingType;
                DoCache = true;
            }
            if (!Base.IsCustomSpriteCharacter && !Main.hairLoaded[Base.TerrarianInfo.HairStyle])
            {
                dummy.hair = Base.TerrarianInfo.HairStyle;
                DoCache = true;
            }
            if (DoCache)
            {
                Terraria.DataStructures.DrawData[] dds = Main.playerDrawData.ToArray();
                Main.instance.DrawPlayer(dummy, new Vector2(-50, -50), 0f, Vector2.Zero, 0f);
                Main.playerDrawData = dds.ToList();
            }
        }

        public static void CheckForBirthdays(Player player)
        {
            List<GuardianData> BirthdayTgs = new List<GuardianData>();
            foreach(GuardianData gd in player.GetModPlayer<PlayerMod>().MyGuardians.Values)
            {
                if (gd.IsBirthday && NpcMod.HasGuardianNPC(gd.ID, gd.ModID))
                {
                    BirthdayTgs.Add(gd);
                }
            }
            if(BirthdayTgs.Count > 0)
            {
                string Text = "Today is ";
                bool First = true;
                foreach(GuardianData g in BirthdayTgs)
                {
                    if (!First)
                        Text += ", ";
                    Text += g.Name;
                }
                Text += "'s birthday"+ (BirthdayTgs.Count > 1 ? "s" : "") + ".";
                Main.NewText(Text, MainMod.BirthdayColor);
                if (!Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
                    Terraria.GameContent.Events.BirthdayParty.ToggleManualParty();
            }
        }

        public static int DrawIndexStacker = 0, FirstDrawFrame = -1, LastDrawFrame = -1;
        public static bool DrawLeftBodyPartsInFrontOfPlayer = false, DrawRightBodyPartsInFrontOfPlayer = false;
        public static List<GuardianDrawData> DrawBehind = new List<GuardianDrawData>(),
            DrawFront = new List<GuardianDrawData>();
        public static int HeadSlot, BodySlot, LegSlot, FaceSlot = 0, FrontSlot = 0, BackSlot = 0;
        public static bool HeadVanityIsAcc = false;

        public static List<Terraria.DataStructures.DrawData> GetDrawFrontData
        {
            get
            {
                List<Terraria.DataStructures.DrawData> list = new List<Terraria.DataStructures.DrawData>();
                foreach (GuardianDrawData gdd in DrawFront)
                    list.Add(gdd.GetDrawData());
                return list;
            }
        }

        public static List<Terraria.DataStructures.DrawData> GetDrawBehindData
        {
            get
            {
                List<Terraria.DataStructures.DrawData> list = new List<Terraria.DataStructures.DrawData>();
                foreach (GuardianDrawData gdd in DrawBehind)
                    list.Add(gdd.GetDrawData());
                return list;
            }
        }

        public Rectangle GetAnimationFrameRectangle(int FrameID)
        {
            Rectangle rect = new Rectangle(FrameID * SpriteWidth, 0, SpriteWidth, SpriteHeight);
            int MaxFramesInARow = Base.FramesInRows * SpriteWidth;
            if (rect.X >= Base.FramesInRows)
            {
                int RowCount = rect.X / MaxFramesInARow;
                rect.X -= MaxFramesInARow * RowCount;
                rect.Y += RowCount * rect.Height;
            }
            return rect;
        }

        public Texture2D GetExtraTexture(string TextureID)
        {
            return Base.sprites.GetExtraTexture(TextureID);
        }

        public static bool DrawingIgnoringLighting = false;

        public void Draw(bool IgnoreLighting = false, bool DoShading = false)
        {
            DrawingIgnoringLighting = IgnoreLighting;
            DrawDataCreation(IgnoreLighting);
            List<GuardianDrawData> DrawBehindDone = DrawBehind,
                DrawFrontDone = DrawFront;
            foreach (GuardianDrawMoment gdm in MainMod.DrawMoment)
            {
                if (gdm.DrawTargetType == TargetTypes.Guardian && gdm.DrawTargetID == WhoAmID && MainMod.ActiveGuardians.ContainsKey(gdm.GuardianWhoAmID))
                {
                    DrawBehind = new List<GuardianDrawData>();
                    DrawFront = new List<GuardianDrawData>();
                    MainMod.ActiveGuardians[gdm.GuardianWhoAmID].DrawDataCreation();
                    if (!gdm.DrawAtWhoAmID)
                    {
                        DrawBehindDone.InsertRange(0, DrawBehind);
                        DrawFrontDone.AddRange(DrawFront);
                    }
                    else
                    {
                        DrawBehindDone.AddRange(DrawBehind);
                        DrawFrontDone.InsertRange(0, DrawFront);
                    }
                }
            }
            foreach (GuardianDrawData dd in DrawBehindDone)
            {
                DoDrawCompanionDrawData(dd, DoShading);
                //dd.Draw(Main.spriteBatch);
            }
            foreach (GuardianDrawData dd in DrawFrontDone)
            {
                DoDrawCompanionDrawData(dd, DoShading);
                //dd.Draw(Main.spriteBatch);
            }
            if (DoShading)
            {
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }
        }

        public static void DoDrawCompanionDrawData(GuardianDrawData dd, bool DoShading)
        {
            if (DoShading)
                GameShaders.Armor.Apply(dd.Shader, null, dd.GetDrawData());
            dd.Draw(Main.spriteBatch);
        }

        public List<Terraria.DataStructures.DrawData> GetTrailsDataAsDrawData()
        {
            List<GuardianDrawData> gdds = GetTrailsData();
            List<Terraria.DataStructures.DrawData> dds = new List<Terraria.DataStructures.DrawData>();
            foreach(GuardianDrawData gdd in gdds)
            {
                dds.Add(gdd.GetDrawData());
            }
            return dds;
        }

        public List<GuardianDrawData> GetTrailsData()
        {
            List<GuardianDrawData> ResultTrails = new List<GuardianDrawData>();
            if (TrailDelay > 0) //To avoid code hanging
            {
                for (int tl = 0; tl < TrailLength; tl++)
                {
                    int TrailPosition = (TrailLength - tl) * TrailDelay;
                    if(TrailPosition < Trails.Count)
                    {
                        TrailPositionLogger Trail = Trails[TrailPosition];
                        Trail.MaskGuardianInfosToTrail(this);
                        DrawBehind.Clear();
                        DrawFront.Clear();
                        DrawDataCreation();
                        float Opacity = (float)(TrailLength - tl) / TrailLength;
                        List<GuardianDrawData> ThisTrail = new List<GuardianDrawData>();
                        foreach (GuardianDrawData gdd in DrawBehind)
                        {
                            gdd.color *= Opacity;
                            ThisTrail.Add(gdd);
                        }
                        foreach (GuardianDrawData gdd in DrawFront)
                        {
                            gdd.color *= Opacity;
                            ThisTrail.Add(gdd);
                        }
                        ResultTrails.AddRange(ThisTrail);
                        Trail.RestoreGuardianInfos(this);
                    }
                }
            }
            if(PulsePower > 0)
            {
                float Pulse = PulsePower * PulseValue;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 PositionBackup = Position;
                    switch(i)
                    {
                        case 0:
                            Position.Y -= Pulse;
                            break;
                        case 1:
                            Position.X += Pulse;
                            break;
                        case 2:
                            Position.Y += Pulse;
                            break;
                        case 3:
                            Position.X -= Pulse;
                            break;
                    }
                    DrawBehind.Clear();
                    DrawFront.Clear();
                    DrawDataCreation();
                    List<GuardianDrawData> ThisTrail = new List<GuardianDrawData>();
                    foreach (GuardianDrawData gdd in DrawBehind)
                    {
                        gdd.color *= PulseValue;
                        ThisTrail.Add(gdd);
                    }
                    foreach (GuardianDrawData gdd in DrawFront)
                    {
                        gdd.color *= PulseValue;
                        ThisTrail.Add(gdd);
                    }
                    ResultTrails.AddRange(ThisTrail);
                    Position = PositionBackup;
                }
            }
            return ResultTrails;
        }

        public void DoWraithColoring(out Color SkinColor, out Color EyesColor, out Color EyeWhiteColor)
        {
            byte ColorMod = 39;
            //c.A = 100;
            EyesColor = Color.Red;
            SkinColor = new Color(ColorMod, ColorMod, ColorMod);
            EyeWhiteColor = Color.White;
            if (MainMod.NemesisFadeEffect < 0)
            {
                SkinColor.R = SkinColor.G = SkinColor.B = 0;
                //if (HeadSlot > 0 || ArmorSlot > 0 || LegSlot > 0)
                //    c.A = 0;
            }
            else
            {
                float Percentage = 1f;
                if (MainMod.NemesisFadeEffect < MainMod.NemesisFadingTime * 0.3f)
                {
                    Percentage = MainMod.NemesisFadeEffect / (MainMod.NemesisFadingTime * 0.3f);
                }
                else if (MainMod.NemesisFadeEffect >= MainMod.NemesisFadingTime * 0.7f)
                {
                    Percentage = 1f - (MainMod.NemesisFadeEffect - (MainMod.NemesisFadingTime * 0.7f)) / (MainMod.NemesisFadingTime * 0.3f);
                }
                if (Percentage < 0)
                    Percentage = 0;
                if (Percentage > 1)
                    Percentage = 1;
                SkinColor.R = (byte)(Percentage * SkinColor.R);
                SkinColor.G = (byte)(Percentage * SkinColor.G);
                SkinColor.B = (byte)(Percentage * SkinColor.B);
            }
            EyeWhiteColor = SkinColor;
        }

        public void DoLightingColoring(ref Color c)
        {
            int TileX = (int)(Position.X * DivisionBy16), TileY = (int)(CenterY * DivisionBy16);
            //int HeightMargin = (int)Height / 32;
            if (TileY * 16 < Main.screenPosition.Y)
            {
                TileY = (int)(Main.screenPosition.Y * DivisionBy16);
            }
            if (TileY * 16 > Main.screenHeight + Main.screenPosition.Y)
            {
                TileY = (int)((Main.screenHeight + Main.screenPosition.Y) * DivisionBy16);
            }
            if (TileY < 0)
                TileY = 0;
            if (TileY >= Main.maxTilesY)
                TileY = Main.maxTilesY - 1;
            c = Lighting.GetColor(TileX, TileY, c);
        }

        private static bool DrawHair = false, DrawAltHair = false, DrawBody = false, DrawArms = false, DrawHands = false, DrawLegs = false;

        public void DrawDataCreation(bool IgnoreLighting = false)
        {
            MyDrawOrder = CurrentDrawnOrderID++;
            DrawBehind.Clear();
            DrawFront.Clear();
            if (DoAction.InUse && DoAction.Invisibility)
                return;
            if (WofFood)
                return;
            DrawLeftBodyPartsInFrontOfPlayer = (PlayerMounted && ReverseMount) || PlayerControl || GrabbingPlayer || SittingOnPlayerMount || (AssistSlot == 0 && LeftArmAnimationFrame == Base.ReviveFrame) || UsingFurniture;
            DrawRightBodyPartsInFrontOfPlayer = false;
            Base.ForceDrawInFrontOfPlayer(this, ref DrawLeftBodyPartsInFrontOfPlayer, ref DrawRightBodyPartsInFrontOfPlayer);
            hitTileData.DrawFreshAnimations(Main.spriteBatch);
            bool DrawInvalidGuardian = Base.InvalidGuardian;
            if (!Base.InvalidGuardian)
            {
                if (Base.sprites.HasErrorLoadingOccurred)
                    DrawInvalidGuardian = true;
                else if (!Base.sprites.IsTextureLoaded)
                {
                    Base.sprites.LoadTextures();
                }
            }
            LastDrawFrame = -1;
            FirstDrawFrame = Main.playerDrawData.Count - 1;
            if (OwnerPos > -1 && Main.player[OwnerPos].mount != null &&  Main.player[OwnerPos].mount.Active && (Main.player[OwnerPos].mount.Type == Mount.MinecartWood || Main.player[OwnerPos].mount.Type == Mount.MinecartMech || Main.player[OwnerPos].mount.Type == Mount.Minecart))
            {
                for (int DrawData = 0; DrawData < Main.playerDrawData.Count; DrawData++)
                {
                    if (Main.playerDrawData[DrawData].texture == Main.player[OwnerPos].mount._data.frontTexture)
                    {
                        LastDrawFrame = DrawData;
                        break;
                    }
                }
            }
            DrawIndexStacker = 0;
            if (DrawInvalidGuardian)
            {
                SpriteEffects errorDir = LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                GuardianDrawData errordd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, MainMod.LosangleOfUnnown, Position - Main.screenPosition, null, Color.White, 0f, new Vector2(0, 48), 1f, SpriteEffects.None);
                DrawItem(Position, errorDir, true);
                AddDrawData(errordd, false);
                DrawItem(Position, errorDir, false);
                return;
            }
            FaceSlot = 0;
            FrontSlot = 0;
            BackSlot = 0;
            GetEquipmentSlots();
            HeadVanityIsAcc = false;
            if (Base.IsCustomSpriteCharacter)
            {
                BodySlot = LegSlot = HeadSlot = 0;
                int id = ReturnEquippableHeadVanityEquip(out HeadVanityIsAcc);
                if (HeadVanityIsAcc)
                {
                    FaceSlot = id;
                }
                else
                {
                    HeadSlot = id;
                }
            }
            if (HeadSlot == 201 && !Male)
                HeadSlot = 202;
            if (Data.OutfitID == 0 || !Base.OutfitList.Any(x => x.SkinID == Data.OutfitID && x.SkinUsesHead))
            {
                if (HeadSlot == 0 && Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
                {
                    HeadSlot = Terraria.ID.ArmorIDs.Head.PartyHat;
                }
                if (HeadSlot == 0 && Main.xMas)
                {
                    HeadSlot = 44;
                }
            }
            if ((Base.IsCustomSpriteCharacter && BodyAnimationFrame == Base.BedSleepingFrame) || 
                BodyAnimationFrame == Base.ThroneSittingFrame || BodyAnimationFrame == Base.DownedFrame)
            {
                HeadSlot = 0;
                FaceSlot = 0;
            }
            TryToLoadGuardianEquipments(ref HeadSlot, ref BodySlot, ref LegSlot, ref FaceSlot, ref FrontSlot, ref BackSlot);
            Vector2 NewPosition = PositionWithOffset - Main.screenPosition; //Position - Main.screenPosition;
            NewPosition.Y += (gfxOffY) * Scale * GravityDirection + 2;
            if (!Base.IsCustomSpriteCharacter)
                NewPosition.Y += 2;
            Vector2 Origin = new Vector2(SpriteWidth * 0.5f, SpriteHeight);
            if (GravityDirection < 0)
            {
                Origin.Y = 0;
                //NewPosition.Y -= CollisionHeight;
            }
            float Rotation = this.Rotation;
            float Alpha = (float)(255 - ImmuneAlpha) / 255;
            int Shader = BodyDye.dye;
            Color c = Color.White;
            Color armorColor = Color.White;
            if (!IgnoreLighting)
            {
                int TileX = (int)(Position.X * DivisionBy16), TileY = (int)(CenterY * DivisionBy16);
                //int HeightMargin = (int)Height / 32;
                if (TileY * 16 < Main.screenPosition.Y)
                {
                    TileY = (int)(Main.screenPosition.Y * DivisionBy16);
                }
                if (TileY * 16 > Main.screenHeight + Main.screenPosition.Y)
                {
                    TileY = (int)((Main.screenHeight + Main.screenPosition.Y) * DivisionBy16);
                }
                if (TileY < 0)
                    TileY = 0;
                if (TileY >= Main.maxTilesY)
                    TileY = Main.maxTilesY - 1;
                c = Lighting.GetColor(TileX, TileY, c);
                armorColor = Lighting.GetColor(TileX, TileY, armorColor);
            }
            bool IsTransformed = Base.IsTerrarian && HeadSlot >= 38 && HeadSlot <= 39;
            if (Base.Effect == GuardianBase.GuardianEffect.Wraith)
            {
                byte ColorMod = 255;
                if (IsTransformed)
                    ColorMod = 40;
                //c.A = 100;
                if (MainMod.NemesisFadeEffect < 0)
                {
                    c.R = c.G = c.B = 0;
                    //if (HeadSlot > 0 || ArmorSlot > 0 || LegSlot > 0)
                    //    c.A = 0;
                }
                else
                {
                    float Percentage = 1f;
                    if (MainMod.NemesisFadeEffect < MainMod.NemesisFadingTime * 0.3f)
                    {
                        Percentage = MainMod.NemesisFadeEffect / (MainMod.NemesisFadingTime * 0.3f);
                    }
                    else if (MainMod.NemesisFadeEffect >= MainMod.NemesisFadingTime * 0.7f)
                    {
                        Percentage = 1f - (MainMod.NemesisFadeEffect - (MainMod.NemesisFadingTime * 0.7f)) / (MainMod.NemesisFadingTime * 0.3f);
                    }
                    if (Percentage < 0)
                        Percentage = 0;
                    if (Percentage > 1)
                        Percentage = 1;
                    c.R = c.G = c.B = (byte)(Percentage * ColorMod);
                    //c.A = (byte)(100 + Percentage * (255 - 100));
                    //if (HeadSlot > 0 || ArmorSlot > 0 || LegSlot > 0)
                    //    c.A = (byte)(Percentage * 255);
                }
                if (IsTransformed)
                {
                    armorColor = c;
                }
            }
            SpriteEffects seffect = (LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            if (GravityDirection < 0)
            {
                if (seffect == SpriteEffects.FlipHorizontally)
                    seffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                else
                    seffect = SpriteEffects.FlipVertically;
            }
            if (Downed)
            {
                Origin.Y -= Origin.Y * 0.5f;
                NewPosition.Y -= Origin.Y;
                Rotation += KnockdownRotation;
            }
            if (!Downed)
            {
                bool HasCarpet = false;
                for (int i = 0; i < 10; i++)
                {
                    if (Inventory[i].type == Terraria.ID.ItemID.FlyingCarpet)
                    {
                        HasCarpet = true;
                    }
                }
                for (int i = 3; i < 8; i++)
                {
                    if (Equipments[i].type == Terraria.ID.ItemID.FlyingCarpet)
                    {
                        HasCarpet = true;
                    }
                }
                GuardianDrawData accd;
                if (HasCarpet)
                {
                    Vector2 CarpetPivot = new Vector2(32, 10);
                    accd = new GuardianDrawData(GuardianDrawData.TextureType.PreDrawEffect, Main.flyingCarpetTexture, NewPosition, new Rectangle(0, (int)(MainMod.CarpetAnimationTime) * 20, 64, 20), c, 0f, CarpetPivot, Scale, seffect);
                    AddDrawData(accd, false);
                }
            }
            DrawBuffEffects(true, ref c);
            c *= Alpha;
            DrawWings(seffect, armorColor);
            Base.GuardianPreDrawScript(this, NewPosition, c, armorColor, Rotation, Origin, Scale, seffect);
            if (Base.IsCustomSpriteCharacter)
            {
                DrawTerraGuardianData(NewPosition, c, armorColor, Origin, seffect, Rotation, Shader);
            }
            else
            {
                DrawTerrarianData(seffect, Rotation, c, armorColor, IgnoreLighting, Shader, ref NewPosition, ref Origin);
            }
            if (mount.Active)
            {
                List<GuardianDrawData> gdds = new List<GuardianDrawData>();
                gdds.AddRange(mount.Draw(0, this, NewPosition + Main.screenPosition, c, seffect, 0f));
                gdds.AddRange(mount.Draw(1, this, NewPosition + Main.screenPosition, c, seffect, 0f));
                DrawBehind.AddRange(gdds);

                gdds = new List<GuardianDrawData>();
                gdds.AddRange(mount.Draw(2, this, NewPosition + Main.screenPosition, c, seffect, 0f));
                gdds.AddRange(mount.Draw(3, this, NewPosition + Main.screenPosition, c, seffect, 0f));
                DrawFront.AddRange(gdds);
            }
            Base.GuardianPostDrawScript(this, NewPosition, c, armorColor, Rotation, Origin, Scale, seffect);
            GuardianDrawData dd;
            if (HasFlag(GuardianFlags.Frozen))
            {
                int HigherSize = Width;
                if (Height > HigherSize)
                    HigherSize = Height;
                float ThisScale = (HigherSize / 38) * Scale;
                if (ThisScale < 1f) ThisScale = 1f;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PosDrawEffect, Main.frozenTexture, Position - Main.screenPosition, null, Color.White * 0.5f, 0f, new Vector2(26, 52), ThisScale, SpriteEffects.None);
                AddDrawData(dd, DrawLeftBodyPartsInFrontOfPlayer);
            }
            if (HasFlag(GuardianFlags.Webbed))
            {
                int HigherSize = Width;
                if (Height > HigherSize)
                    HigherSize = Height;
                float ThisScale = (HigherSize / 64) * Scale;
                if (ThisScale < 1f) ThisScale = 1f;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PosDrawEffect, Main.extraTexture[32], CenterPosition - Main.screenPosition, null, Color.White, 0f, new Vector2(39, 38), ThisScale, SpriteEffects.None);
                AddDrawData(dd, DrawLeftBodyPartsInFrontOfPlayer);
            }
            if (HasFlag(GuardianFlags.Electrified))
            {
                Texture2D ElectricityTexture = Main.glowMaskTexture[25];
                int Frame = (int)MainMod.NemesisFadeEffect / 5;
                int FrameHeight = ElectricityTexture.Height / 7;
                for (int t = 0; t < 2; t++)
                {
                    Frame %= 7;
                    if (Frame > 1 && Frame < 5)
                    {
                        dd = new GuardianDrawData(GuardianDrawData.TextureType.PosDrawEffect, ElectricityTexture, CenterPosition - Main.screenPosition, new Rectangle(0, Frame * FrameHeight, ElectricityTexture.Width, FrameHeight), Color.White, 0f, new Vector2(ElectricityTexture.Width * 0.5f, FrameHeight * 0.5f), Scale, SpriteEffects.None);
                        AddDrawData(dd, DrawLeftBodyPartsInFrontOfPlayer);
                    }
                }
            }
            if (HasFlag(GuardianFlags.Confusion))
            {
                Texture2D ConfusionTexture = Main.confuseTexture;
                Vector2 EffectPosition = Position;
                EffectPosition.Y -= Height + 16 + ConfusionTexture.Height;
                EffectPosition.X -= ConfusionTexture.Width * 0.5f;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PosDrawEffect, ConfusionTexture, EffectPosition - Main.screenPosition, Color.White);
                AddDrawData(dd, DrawLeftBodyPartsInFrontOfPlayer);
            }
            DoAction.DrawAction(this);
            if (SubAttackInUse)
            {
                SpecialAttack.UpdateDrawing(this);
            }
            //DrawBuffWheel(); //Will live forever in our memory... Said nobody.
            /*Rectangle DisplayPosition = WeaponCollision;
            DisplayPosition.X -= (int)Main.screenPosition.X;
            DisplayPosition.Y -= (int)Main.screenPosition.Y;
            Main.spriteBatch.Draw(Main.blackTileTexture, DisplayPosition, Color.Red * 0.5f); //For debugging.   
			*/
            /*Rectangle HitboxDisplay = HitBox;
            HitboxDisplay.X -= (int)Main.screenPosition.X;
            HitboxDisplay.Y -= (int)Main.screenPosition.Y;
            dd = new GuardianDrawData(GuardianDrawData.TextureType.Unknown, Main.blackTileTexture, HitboxDisplay, null, Color.Red * 0.5f);
            AddDrawData(dd, false);*/
            //
            foreach (PathFinder.Breadcrumbs path in Paths)
            {
                Vector2 Pos = new Vector2(path.X * 16, path.Y * 16) - Main.screenPosition;
                Main.spriteBatch.Draw(Main.blackTileTexture, Pos, Color.Blue);
                string s = "";
                switch (path.NodeOrientation)
                {
                    case PathFinder.Node.DIR_UP:
                        s = "Up";
                        break;
                    case PathFinder.Node.DIR_RIGHT:
                        s = "Right";
                        break;
                    case PathFinder.Node.DIR_DOWN:
                        s = "Down";
                        break;
                    case PathFinder.Node.DIR_LEFT:
                        s = "Left";
                        break;
                }
                Utils.DrawBorderString(Main.spriteBatch, s, Pos, Color.White);
            }
            //
            //dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, MainMod.GuardianMouseTexture, new Vector2(AimDirection.X, AimDirection.Y) - Main.screenPosition, Color.White);
            //AddDrawData(dd, true);
            //DrawBehind.Insert(0, new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Main.blackTileTexture, new Rectangle((int)(HitBox.X - Main.screenPosition.X), (int)(HitBox.Y - Main.screenPosition.Y), HitBox.Width, HitBox.Height), null, Color.Red));
            //if(ItemAnimationTime > 0)
            //    DrawBehind.Insert(0, new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Main.blackTileTexture, new Rectangle((int)(WeaponCollision.X - Main.screenPosition.X), (int)(WeaponCollision.Y - Main.screenPosition.Y), WeaponCollision.Width, WeaponCollision.Height), null, Color.Red * 0.25f));
            foreach (GuardianDrawData gdd in DrawBehind)
            {
                if (!MountedOnPlayer || Downed)
                {
                    gdd.IgnorePlayerRotation = true;
                }
                if (gdd.Position != null)
                    gdd.Position = new Vector2((int)gdd.Position.Value.X, (int)gdd.Position.Value.Y);
            }
            foreach (GuardianDrawData gdd in DrawFront)
            {
                if (!MountedOnPlayer || Downed)
                {
                    gdd.IgnorePlayerRotation = true;
                }
                if (gdd.Position != null)
                    gdd.Position = new Vector2((int)gdd.Position.Value.X, (int)gdd.Position.Value.Y);
            }
        }

        public void DrawReviveBar()
        {
            if (KnockedOut && !HasFlag(GuardianFlags.HideKOBar))
            {
                Vector2 BarPosition = Position - Main.screenPosition;
                BarPosition.Y += 22f;
                BarPosition.X -= 144 * 0.5f;
                Main.spriteBatch.Draw(MainMod.GuardianHealthBar, BarPosition, new Rectangle(122 * 4, 0, 122, 16), Color.White);
                BarPosition.X += 22;
                BarPosition.Y += 4;
                int BarWidth = (int)((float)HP / MHP * 98);
                Main.spriteBatch.Draw(MainMod.GuardianHealthBar, BarPosition, new Rectangle(122 * 4 + 22, 16 + 4, BarWidth, 8), Color.White);
            }
        }

        public void DrawTerraGuardianData(Vector2 NewPosition, Color c, Color armorColor, Vector2 Origin, SpriteEffects seffect, float Rotation, int Shader)
        {
            Rectangle rect = GetAnimationFrameRectangle(RightArmAnimationFrame);
            GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.TGRightArm, Base.sprites.RightArmSprite, NewPosition, rect, c, Rotation, Origin, Scale, seffect);
            dd.Shader = Shader;
            AddDrawData(dd, false);
            //dd.Draw(Main.spriteBatch);
            //Main.spriteBatch.Draw(Base.sprites.RightArmSprite, NewPosition, rect, c, Rotation, Origin, Scale, seffect, 0f);
            DrawItem(NewPosition, seffect, true);
            rect = GetAnimationFrameRectangle(BodyAnimationFrame);
            dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, Base.sprites.BodySprite, NewPosition, rect, c, Rotation, Origin, Scale, seffect);
            dd.Shader = Shader;
            AddDrawData(dd, false);
            try
            {
                bool IsLoaded = (HeadVanityIsAcc ? (FaceSlot > 0 && Main.accFaceLoaded[FaceSlot]) : (HeadSlot > 0 && Main.armorHeadLoaded[HeadSlot]));
                if (IsLoaded)
                {
                    float HelmetScale = Scale;
                    int Frame = BodyAnimationFrame;
                    if (Frame == Base.BackwardStanding)
                        Frame = Base.StandingFrame;
                    if (Frame == Base.BackwardRevive)
                        Frame = Base.ReviveFrame;
                    Vector2 HelmetPosition = Base.HeadVanityPosition.GetPositionFromFrameVector(Frame);
                    bool Hide = HelmetPosition.X == HelmetPosition.Y && HelmetPosition.X <= -100;
                    if (!Hide)
                    {
                        HelmetPosition.X -= SpriteWidth * 0.5f;
                        if (LookingLeft)
                            HelmetPosition.X *= -1;
                        Texture2D HeadgearTexture = (HeadVanityIsAcc ? Main.accFaceTexture[FaceSlot] : Main.armorHeadTexture[HeadSlot]);
                        int FrameWidth = HeadgearTexture.Width, FrameHeight = HeadgearTexture.Height / 20;
                        HelmetPosition.Y = (-SpriteHeight + HelmetPosition.Y) * GravityDirection;
                        HelmetPosition *= HelmetScale;
                        Vector2 HelmetOrigin = new Vector2(FrameWidth * 0.5f, FrameHeight * 0.5f);
                        //if (GravityDirection < 0)
                        //    HelmetOrigin.Y = FrameHeight;
                        HelmetPosition += NewPosition;
                        dd = new GuardianDrawData(GuardianDrawData.TextureType.TGHeadAccessory, HeadgearTexture, HelmetPosition, new Rectangle(0, 0, FrameWidth, FrameHeight), armorColor, Rotation, HelmetOrigin, HelmetScale, seffect);
                        //dd.shader = Shader;
                        AddDrawData(dd, false);
                    }
                }
            }
            catch { }
            DrawChains();
            DrawWofExtras();
            if (Base.sprites.BodyFrontSprite != null) // (SittingOnPlayerMount || PlayerMounted) && 
            {
                if (Base.SpecificBodyFrontFramePositions)
                {
                    int Frame = Base.GetBodyFrontSprite(BodyAnimationFrame);
                    if (Frame > -1)
                        rect = GetAnimationFrameRectangle(Frame);
                    else
                        rect.X = -1;
                }
                if (rect.X != -1)
                {
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, Base.sprites.BodyFrontSprite, NewPosition, rect, c, Rotation, Origin, Scale, seffect);
                    dd.Shader = Shader;
                    AddDrawData(dd, true);
                }
            }
            if (Base.sprites.RightArmFrontSprite != null)
            {
                int Frame = RightArmAnimationFrame;
                if (Base.RightArmFrontFrameSwap.Count > 0)
                {
                    Frame = -1;
                    if (Base.RightArmFrontFrameSwap.ContainsKey(RightArmAnimationFrame))
                    {
                        Frame = Base.RightArmFrontFrameSwap[RightArmAnimationFrame];
                    }
                }
                if (Frame >= 0)
                {
                    rect = GetAnimationFrameRectangle(Frame);
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGRightArmFront, Base.sprites.RightArmFrontSprite, NewPosition, rect, c, Rotation, Origin, Scale, seffect);
                    dd.Shader = Shader;
                    AddDrawData(dd, DrawRightBodyPartsInFrontOfPlayer);
                    DrawItem(NewPosition, seffect, true);
                }
            }
            rect = GetAnimationFrameRectangle(LeftArmAnimationFrame);
            DrawItem(NewPosition, seffect, false);
            dd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, Base.sprites.LeftArmSprite, NewPosition, rect, c, Rotation, Origin, Scale, seffect);
            dd.Shader = Shader;
            AddDrawData(dd, DrawLeftBodyPartsInFrontOfPlayer);
        }

        public void DrawHead(Vector2 Position, float Scale = 1f, float XOffset = 0.5f, float YOffset = 0.5f, bool FaceCharacterDirection = false)
        {
            if (Base.InvalidGuardian)
            {
                Position.X -= MainMod.LosangleOfUnnown.Width * XOffset * Scale;
                Position.Y -= MainMod.LosangleOfUnnown.Height * XOffset * Scale;
                new GuardianDrawData(GuardianDrawData.TextureType.TGHead, MainMod.LosangleOfUnnown, Position,
                 null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None).Draw(Main.spriteBatch);
                return;
            }
            if (!Base.sprites.IsTextureLoaded)
                Base.sprites.LoadTextures();
            TryToLoadGuardianEquipments(ref HeadSlot, ref BodySlot, ref LegSlot, ref FaceSlot, ref FrontSlot, ref BackSlot);
            if (Base.IsTerrarian)
            {
                Position.Y -= 16f;
                DrawTerrarianHeadData(Position, Scale);
                return;
            }
            Vector2 Origin = Vector2.Zero;
            Texture2D HeadTexture = Base.sprites.HeadSprite;
            if (HeadTexture == null)
            {
                HeadTexture = MainMod.LosangleOfUnnown;
                Scale *= 32f / HeadTexture.Height;
            }
            SpriteEffects spriteEffects = SpriteEffects.None;
            if(FaceCharacterDirection && LookingLeft)
            {
                XOffset = 1f - XOffset;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Position.X -= HeadTexture.Width * XOffset * Scale;
            Position.Y -= HeadTexture.Height * YOffset * Scale;
            List<GuardianDrawData> gddlist = new List<GuardianDrawData>();
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGHead, HeadTexture, Position,
                null, Color.White, 0f, Origin, Scale, spriteEffects);
            gddlist.Add(gdd);
            Base.GuardianModifyDrawHeadScript(this, Position, Color.White, Scale, spriteEffects, Origin, ref gddlist);
            foreach (GuardianDrawData d in gddlist)
            {
                d.Draw(Main.spriteBatch);
            }
        }

        public void DrawTerrarianHeadData(Vector2 Position, float Scale = 1f)
        {
            if (!Base.IsTerrarian)
                return;
            List<GuardianDrawData> gddlist = new List<GuardianDrawData>();
            SpriteEffects seffect = SpriteEffects.None;
            Rectangle headrect = new Rectangle(0, 0, 40, 56);
            Color HairColor = Base.TerrarianInfo.HairColor,
                EyesColor = Base.TerrarianInfo.EyeColor,
                EyesWhiteColor = Color.White,
                SkinColor = Base.TerrarianInfo.SkinColor,
                ArmorColor = Color.White;
            int SkinVariant = Base.TerrarianInfo.GetSkinVariant(Male);
            GetEquipmentSlots();
            if (Base.Effect == GuardianBase.GuardianEffect.Wraith)
            {
                DoWraithColoring(out SkinColor, out EyesColor, out EyesWhiteColor);
                ArmorColor = SkinColor;
            }
            bool IsTransformed = !Base.IsCustomSpriteCharacter && HeadSlot >= 38 && HeadSlot <= 39;
            Vector2 Origin = new Vector2(20, 0);
            gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlHead, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Head], Position, headrect, SkinColor, 0f, Origin, Scale, seffect));
            gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlEye, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Eyes], Position, headrect, EyesColor, 0f, Origin, Scale, seffect));
            gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlEyeWhite, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.EyeWhites], Position, headrect, EyesWhiteColor, 0f, Origin, Scale, seffect));
            if (Base.TerrarianInfo.HairStyle >= 0 && Main.hairLoaded[Base.TerrarianInfo.HairStyle])
            {
                if (DrawHair)
                {
                    gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlHair, Main.playerHairTexture[Base.TerrarianInfo.HairStyle], Position, headrect, HairColor, 0f, Origin, Scale, seffect));
                }
                else if (DrawAltHair)
                {
                    gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlHair, Main.playerHairAltTexture[Base.TerrarianInfo.HairStyle], Position, headrect, HairColor, 0f, Origin, Scale, seffect));
                }
            }
            if (HeadSlot > 0 && Main.armorHeadLoaded[HeadSlot])
            {
                gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlArmorHead, Main.armorHeadTexture[HeadSlot], Position, headrect, ArmorColor, 0f, Origin, Scale, seffect));
                if (Base.Effect == GuardianBase.GuardianEffect.Wraith && HeadSlot == 38)
                {
                    gddlist.Add(new GuardianDrawData(GuardianDrawData.TextureType.PlEye, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Eyes], Position, headrect, EyesColor, 0f, Origin, Scale, seffect));
                }
            }
            Base.GuardianModifyDrawHeadScript(this, Position, SkinColor, Scale, seffect, Origin, ref gddlist);
            foreach (GuardianDrawData gdd in gddlist)
                gdd.Draw(Main.spriteBatch);
        }

        public void DrawTerrarianData(SpriteEffects seffect, float Rotation, Color color, Color armorColor, bool IgnoreLightingColor, int Shader, ref Vector2 Position, ref Vector2 Origin)
        {
            Rectangle legrect = new Rectangle(0, 56 * BodyAnimationFrame, 40, 56),
                bodyrect = new Rectangle(0, 56 * LeftArmAnimationFrame, 40, 56),
                hairrect = new Rectangle(0, 56 * LeftArmAnimationFrame - 336, 40, 56),
                eyerect = new Rectangle(0, 0, hairrect.Width, hairrect.Height);
            if (hairrect.Y < 0)
                hairrect.Y = 0;
            bool ShowHair = HeadSlot != Terraria.ID.ArmorIDs.Head.LamiaFemale && HeadSlot != Terraria.ID.ArmorIDs.Head.LamiaMale;
            int SkinVariant = Base.TerrarianInfo.GetSkinVariant(Male);
            Origin.X = 20;
            Origin.Y = 56;
            byte EyeState = 0;
            if (!Downed)
            {
                if (KnockedOut)
                {
                    Origin.Y *= 0.5f;
                    Position.Y -= 20 - 6;
                    EyeState = 2;
                }
                else if (IsSleeping)
                {
                    Origin.Y *= 0.5f;
                    Position.Y -= 20 + 6;
                    EyeState = 2;
                }
            }
            Color HairColor = Base.TerrarianInfo.HairColor,
                EyesColor = Base.TerrarianInfo.EyeColor,
                EyesWhiteColor = Color.White,
                SkinColor = Base.TerrarianInfo.SkinColor,
                UndershirtColor = Base.TerrarianInfo.UnderShirtColor,
                ShirtColor = Base.TerrarianInfo.ShirtColor,
                PantsColor = Base.TerrarianInfo.PantsColor,
                ShoesColor = Base.TerrarianInfo.ShoeColor,
                ArmorColoring = Color.White;
            bool IsTransformed = HeadSlot >= 38 && HeadSlot <= 39;
            if (Base.Effect == GuardianBase.GuardianEffect.Wraith)
            {
                DoWraithColoring(out SkinColor, out EyesColor, out EyesWhiteColor);
                ShirtColor = UndershirtColor = PantsColor = ShoesColor = SkinColor;
                if (IsTransformed)
                {
                    ArmorColoring = SkinColor;
                }
            }
            if (!IgnoreLightingColor)
            {
                DoLightingColoring(ref HairColor);
                DoLightingColoring(ref SkinColor);
                DoLightingColoring(ref EyesColor);
                DoLightingColoring(ref EyesWhiteColor);
                DoLightingColoring(ref UndershirtColor);
                DoLightingColoring(ref ShirtColor);
                DoLightingColoring(ref PantsColor);
                DoLightingColoring(ref ShoesColor);
                DoLightingColoring(ref ArmorColoring);
            }
            HairColor = GetImmuneAlpha(HairColor, 0);
            SkinColor = GetImmuneAlpha(SkinColor, 0f);
            EyesColor = GetImmuneAlpha(EyesColor, 0f);
            EyesWhiteColor = GetImmuneAlpha(EyesWhiteColor, 0f);
            UndershirtColor = GetImmuneAlpha(UndershirtColor, 0f);
            ShirtColor = GetImmuneAlpha(ShirtColor, 0f);
            PantsColor = GetImmuneAlpha(PantsColor, 0f);
            ShoesColor = GetImmuneAlpha(ShoesColor, 0f);
            ArmorColoring = GetImmuneAlpha(ArmorColoring, 0f);
            DrawBuffEffects(false, ref HairColor);
            DrawBuffEffects(false, ref EyesColor);
            DrawBuffEffects(false, ref EyesWhiteColor);
            DrawBuffEffects(false, ref UndershirtColor);
            DrawBuffEffects(false, ref ShirtColor);
            DrawBuffEffects(false, ref PantsColor);
            DrawBuffEffects(false, ref ShoesColor);
            DrawBuffEffects(false, ref ArmorColoring);
            bool HideLegs = LegSlot == 143 || LegSlot == 106 || LegSlot == 140;
            DrawWings(seffect, ArmorColoring);
            DrawChains();
            DrawWofExtras();
            GuardianDrawData dd;
            if (!HideLegs && DrawLegs)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlLegSkin, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.LegSkin], Position, legrect, SkinColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, SittingOnPlayerMount);
            }
            if (DrawBody)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlBodySkin, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.TorsoSkin], Position, bodyrect, SkinColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, false);
            }
            if (LegSlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorLegs, Main.armorLegTexture[LegSlot], Position, legrect, armorColor, Rotation, Origin, Scale, seffect);
                dd.Shader = Shader;
                AddDrawData(dd, SittingOnPlayerMount);
            }
            else
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultPants, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Pants], Position, legrect, PantsColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, SittingOnPlayerMount);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultShoes, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Shoes], Position, legrect, ShoesColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, SittingOnPlayerMount);
            }
            if (BodySlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorBody, (Male ? Main.armorBodyTexture[BodySlot] : Main.femaleBodyTexture[BodySlot]), Position, bodyrect, ArmorColoring, Rotation, Origin, Scale, seffect);
                dd.Shader = Shader;
                AddDrawData(dd, false);
            }
            else
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultUndershirt, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Undershirt], Position, bodyrect, UndershirtColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, false);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultShirt, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Shirt], Position, bodyrect, ShirtColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, false);
            }
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHead, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Head], Position, bodyrect, SkinColor, Rotation, Origin, Scale, seffect);
            AddDrawData(dd, false);
            float EyePositionBonus = 0;
            if ((hairrect.Y + 336 >= 7 * hairrect.Height && hairrect.Y + 336 < 10 * hairrect.Height) ||
                hairrect.Y + 336 >= 14 * hairrect.Height && hairrect.Y + 336 < 17 * hairrect.Height)
            {
                EyePositionBonus -= 2;
            }
            Position.Y += EyePositionBonus;
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEye, MainMod.EyeTexture, Position, new Rectangle(40 * EyeState, 56, 40, 46), (EyeState == 2 ? SkinColor : EyesColor), Rotation, Origin, Scale, seffect);
            AddDrawData(dd, false);
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEyeWhite, MainMod.EyeTexture, Position, new Rectangle(40 * EyeState, 0, 40, 46), (EyeState == 2 ? SkinColor : EyesWhiteColor), Rotation, Origin, Scale, seffect);
            AddDrawData(dd, false);
            Position.Y -= EyePositionBonus;
            /*dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEye, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Eyes], Position, bodyrect, EyesColor, Rotation, Origin, Scale, seffect);
            AddDrawData(dd, false);
            dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEyeWhite, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.EyeWhites], Position, bodyrect, EyesWhiteColor, Rotation, Origin, Scale, seffect);
            AddDrawData(dd, false);*/
            if (Base.TerrarianInfo.HairStyle >= 0)
            {
                if (DrawHair)
                {
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHair, Main.playerHairTexture[Base.TerrarianInfo.HairStyle], Position, hairrect, HairColor, Rotation, Origin, Scale, seffect);
                    AddDrawData(dd, false);
                }
                else if (DrawAltHair)
                {
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHair, Main.playerHairAltTexture[Base.TerrarianInfo.HairStyle], Position, hairrect, HairColor, Rotation, Origin, Scale, seffect);
                    AddDrawData(dd, false);
                }
            }
            if (HeadSlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorHead, Main.armorHeadTexture[HeadSlot], Position, bodyrect, ArmorColoring, Rotation, Origin, Scale, seffect);
                dd.Shader = Shader;
                AddDrawData(dd, false);
                if (Base.Effect == GuardianBase.GuardianEffect.Wraith && HeadSlot == 38)
                {
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.PlEye, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.Eyes], Position, bodyrect, EyesColor, Rotation, Origin, Scale, seffect);
                    AddDrawData(dd, false);
                }
            }
            DrawItem(Position, seffect, true);
            DrawItem(Position, seffect, false);
            if (DrawArms)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlBodyArmSkin, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmSkin], Position, bodyrect, SkinColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, true);
            }
            if (BodySlot > 0)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlArmorArm, Main.armorArmTexture[BodySlot], Position, bodyrect, ArmorColoring, Rotation, Origin, Scale, seffect);
                dd.Shader = Shader;
                AddDrawData(dd, true);
            }
            else
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultUndershirtArm, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmUndershirt], Position, bodyrect, UndershirtColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, true);
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlDefaultShirtArm, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmShirt], Position, bodyrect, ShirtColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, true);
            }
            if (DrawHands)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.PlHand, Main.playerTextures[SkinVariant, Terraria.ID.PlayerTextureID.ArmHand], Position, bodyrect, SkinColor, Rotation, Origin, Scale, seffect);
                AddDrawData(dd, true);
            }
        }

        public void DrawSocialMessages()
        {
            Vector2 NewPosition = Position - Main.screenPosition;
            NewPosition.Y += (gfxOffY + 2) * Scale * GravityDirection;
            if (!Base.IsCustomSpriteCharacter)
                NewPosition.Y += 2;
            if (EmotionDisplayTime > 0)
            {
                float Opacity = 0f;
                if (EmotionDisplayTime >= 2.5f)
                {
                    Opacity = 1f - (EmotionDisplayTime - 2.5f) * 2;
                }
                else if (EmotionDisplayTime > 0.5f)
                    Opacity = 1f;
                else
                    Opacity = 1f - (0.5f - EmotionDisplayTime) * 2;
                Vector2 EmotionPosition = new Vector2(Position.X - 16, Position.Y - SpriteHeight * Scale - 32);
                Main.spriteBatch.Draw(MainMod.EmotionTexture, EmotionPosition - Main.screenPosition, new Rectangle(32 * (int)CurrentEmotion, 0, 32, 32), Color.White * Opacity);
                FriendshipHeartDisplayTime = 0;
                EmotionDisplayTime -= 0.0166667f;
            }
            else
            {
                if (FriendshipHeartDisplayTime <= 0 && (LastFriendshipLevel != FriendshipLevel || LastFriendshipValue != FriendshipProgression))
                {
                    FriendshipHeartDisplayTime = 5;
                }
                if (FriendshipHeartDisplayTime > 0)
                {
                    LastFriendshipLevel = FriendshipLevel;
                    LastFriendshipValue = FriendshipProgression;
                    float Opacity = 0f;
                    FriendshipHeartDisplayTime -= 0.0166667f;
                    if (FriendshipHeartDisplayTime > 4f)
                        Opacity = 1f - (FriendshipHeartDisplayTime - 4);
                    else if (FriendshipHeartDisplayTime > 1)
                        Opacity = 1f;
                    else
                    {
                        Opacity = FriendshipHeartDisplayTime;
                    }
                    Vector2 HeartPosition = new Vector2(Position.X - 12, Position.Y - SpriteHeight * Scale - 32);
                    DrawFriendshipHeart(HeartPosition - Main.screenPosition, 255, -1, Opacity);
                }
            }
            if (MessageTime > 0)
            {
                Vector2 TextPosition = NewPosition;
                TextPosition.Y -= SpriteHeight + 22;
                int Lines;
                string[] Message = Utils.WordwrapString(ChatMessage, Main.fontMouseText, 400, 5, out Lines);
                TextPosition.Y -= 22f * Lines;
                for (int l = 0; l <= Lines; l++)
                {
                    Utils.DrawBorderString(Main.spriteBatch, Message[l], TextPosition, Color.White, 1f, 0.5f);
                    TextPosition.Y += 22;
                }
            }
        }

        public void DrawBuffEffects(bool DoEffects, ref Color color)
        {
            float DrawRPercent = 1f, DrawGPercent = 1f, DrawBPercent = 1f, DrawAPercent = 1f;
            if (DoEffects && HasFlag(GuardianFlags.Honey) && Main.rand.Next(30) == 0)
            {
                int pos = Dust.NewDust(TopLeftPosition, Width, Height, 152,0,0,150, default(Color), 1f);
                Dust d = Main.dust[pos];
                d.velocity.Y = 0.3f;
                d.scale += 3 * 0.1f;
                d.alpha = 100;
                d.noGravity = true;
                d.velocity += Velocity * 0.1f;
            }
            if (DoEffects && HasBuff(ModContent.BuffType<giantsummon.Buffs.Love>()) && Main.rand.Next(15) == 0)
            {
                Vector2 velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                velocity.Normalize();
                velocity.X *= 0.66f;
                int gore = Gore.NewGore(TopLeftPosition + new Vector2(Main.rand.Next(Width + 1), Main.rand.Next(Height + 1)), velocity * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                Main.gore[gore].sticky = false;
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.Y -= 0.6f;
            }
            if (DoEffects && HasFlag(GuardianFlags.DryadWard) && Velocity.X != 0 && Main.rand.Next(4) == 0)
            {
                Vector2 SpawnPositon = TopLeftPosition;
                SpawnPositon.X -= 2;
                SpawnPositon.Y += Height - 2;
                int pos = Dust.NewDust(SpawnPositon, Width + 4, 4, 163, 0, 0, 100, default(Color), 1.5f);
                Dust d = Main.dust[pos];
                d.noGravity = true;
                d.noLight = true;
                d.velocity = Vector2.Zero;
            }
            if (HasFlag(GuardianFlags.Poisoned))
            {
                DrawRPercent *= 0.8f;
                DrawBPercent *= 0.8f;
                if (DoEffects && Main.rand.Next(50) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition, Width, Height, 46, 0, 0, 150, default(Color), 0.2f);
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].fadeIn = 1.9f;
                }
            }
            if (HasFlag(GuardianFlags.Venom))
            {
                DrawGPercent *= 0.45f;
                DrawRPercent *= 0.75f;
                if (DoEffects && Main.rand.Next(10) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition, Width, Height, 171, 0, 0, 100, default(Color), 0.5f);
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].fadeIn = 1.5f;
                }
            }
            if (HasFlag(GuardianFlags.OnFire))
            {
                DrawBPercent *= 0.6f;
                DrawGPercent *= 0.7f;
                if (DoEffects && Main.rand.Next(4) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 4, 6, Velocity.X * 0.4f, Velocity.Y * 0.4f, 100, default(Color), 3f);
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].velocity *= 1.8f;
                    Main.dust[pos].velocity.Y -= 0.5f;
                }
            }
            if (DoEffects && HasFlag(GuardianFlags.Dripping))
            {
                if (Main.rand.Next(2) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 2, 211, 0f, 0f, 50, default(Color), 0.8f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[pos].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[pos].alpha += 25;
                    Main.dust[pos].noLight = true;
                    Main.dust[pos].velocity *= 0.2f;
                    Main.dust[pos].velocity.Y += 0.2f;
                }
                else
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 8, Height + 8, 211, 0f, 0f, 50, default(Color), 1.1f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[pos].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[pos].alpha += 25;
                    Main.dust[pos].noLight = true;
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].velocity *= 0.2f;
                    Main.dust[pos].velocity.Y += 1f;
                }
            }
            if (HasFlag(GuardianFlags.DrippingSlime))
            {
                DrawRPercent *= 0.8f;
                DrawGPercent *= 0.8f;
                if (DoEffects && Main.rand.Next(4) != 0 && Main.rand.Next(2) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 2, 4, 0f, 0f, 175, new Color(0, 80, 255, 100), 1.4f);
                    if (Main.rand.Next(2) == 0)
                        Main.dust[pos].alpha += 25;
                    if (Main.rand.Next(2) == 0)
                        Main.dust[pos].alpha += 25;
                    Main.dust[pos].noLight = true;
                    Main.dust[pos].velocity *= 0.2f;
                    Main.dust[pos].velocity.Y += 0.2f;
                }
            }
            if (HasFlag(GuardianFlags.Ichor))
                DrawBPercent = 0;
            if (DoEffects && HasFlag(GuardianFlags.Electrified))
            {
                int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 4, 226, 0f, 0f, 100, default(Color), 0.5f);
                Main.dust[pos].velocity *= 1.6f;
                Main.dust[pos].velocity.Y -= 1f;
                Main.dust[pos].position = Vector2.Lerp(Main.dust[pos].position, CenterPosition, 0.5f);
            }
            if (HasFlag(GuardianFlags.Burned))
            {
                DrawRPercent = 1f;
                DrawGPercent *= 0.6f;
                DrawBPercent *= 0.7f;
                if (DoEffects)
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 4, 6, Velocity.X * 0.4f, Velocity.Y * 0.4f, 100, default(Color), 2f);
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].velocity *= 1.8f;
                    Main.dust[pos].velocity.Y -= 0.75f;
                }
            }
            if (HasFlag(GuardianFlags.FrostBurn))
            {
                DrawRPercent *= 0.5f;
                DrawGPercent *= 0.7f;
                if (DoEffects && Main.rand.Next(4) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 4, 135, Velocity.X * 0.4f, Velocity.Y * 0.4f, 100, default(Color), 3f);
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].velocity *= 1.8f;
                    Main.dust[pos].velocity.Y -= 0.5f;
                }
            }
            if (HasFlag(GuardianFlags.OnCursedFire))
            {
                DrawGPercent *= 0.7f;
                DrawBPercent *= 0.6f;
                if (DoEffects && Main.rand.Next(4) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 4, 75, Velocity.X * 0.4f, Velocity.Y * 0.4f, 100, default(Color), 3f);
                    Main.dust[pos].noGravity = true;
                    Main.dust[pos].velocity *= 1.8f;
                    Main.dust[pos].velocity.Y -= 0.5f;
                }
            }
            if (HasFlag(GuardianFlags.Cursed))
            {
                DrawRPercent *= 0.65f;
                DrawGPercent *= 0.8f;
            }
            if (HasCooldown(GuardianCooldownManager.CooldownType.CursedEffect))
            {
                float DebuffValue = (float)GetCooldownValue(GuardianCooldownManager.CooldownType.CursedEffect) / 20;
                if (DebuffValue > 1f) DebuffValue = 1f;
                DrawRPercent -= DrawRPercent * 0.65f * 0.75f * DebuffValue;
                DrawGPercent -= DrawGPercent * 0.8f * 0.75f * DebuffValue;
            }
            if (HasFlag(GuardianFlags.Blind))
            {
                DrawRPercent *= 0.7f;
                DrawGPercent *= 0.65f;
            }
            if (HasFlag(GuardianFlags.Bleeding))
            {
                if (DoEffects && !Downed && Main.rand.Next(30) == 0)
                {
                    int pos = Dust.NewDust(TopLeftPosition, Width, Height, 5, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[pos].velocity.Y += 0.5f;
                    Main.dust[pos].velocity *= 0.25f;
                }
                DrawGPercent *= 0.9f;
                DrawBPercent *= 0.9f;
            }
            if (DoEffects && HasFlag(GuardianFlags.PalladiumRegen) && HP < MHP && !Main.gamePaused) //?
            {
                Vector2 Pos = TopLeftPosition;
                Pos.X += Main.rand.Next(Width);
                Pos.Y += Main.rand.Next(Height);
            }
            if (DoEffects && HasFlag(GuardianFlags.LovestruckPotion) && !Main.gamePaused && Main.rand.Next(5) == 0)
            {
                Vector2 MoveDirection = new Vector2(Main.rand.Next(10, 11), Main.rand.Next(10, 11));
                MoveDirection.Normalize();
                MoveDirection *= 0.66f;
                int pos = Gore.NewGore(TopLeftPosition + new Vector2(Main.rand.Next(Width + 1), Main.rand.Next(Height + 1)), MoveDirection * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                Main.gore[pos].sticky = true;
                Main.gore[pos].velocity *= 0.4f;
                Main.gore[pos].velocity.Y -= 0.6f;
            }
            if (HasFlag(GuardianFlags.StinkyPotion) && !Main.gamePaused)
            {
                DrawRPercent *= 0.7f;
                DrawBPercent *= 0.55f;
                if (DoEffects && Main.rand.Next(5) == 0)
                {
                    Vector2 MoveDirection = new Vector2(Main.rand.Next(10, 11), Main.rand.Next(10, 11));
                    MoveDirection.Normalize();
                    MoveDirection.X *= 0.66f;
                    MoveDirection.Y = Math.Abs(MoveDirection.Y);
                    MoveDirection *= Main.rand.Next(3, 5) * 0.25f;
                    int pos = Dust.NewDust(TopLeftPosition, Width, Height, 188, MoveDirection.X, MoveDirection.Y * 0.5f, 100, default(Color), 1.5f);
                    Main.dust[pos].velocity *= 0.1f;
                    Main.dust[pos].velocity.Y -= 0.5f;
                }
            }
            if (HasFlag(GuardianFlags.OgreSpit))
            {
                DrawRPercent *= 0.6f;
                DrawBPercent *= 0.45f;
                if (DoEffects && Main.rand.Next(5) == 0)
                {
                    int type = Utils.SelectRandom<int>(Main.rand, new int[]{4, 256});
                    Dust dust = Main.dust[Dust.NewDust(TopLeftPosition, Width, Height, type, 0f, 0f, 100, default(Microsoft.Xna.Framework.Color), 1f)];
                    dust.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
                    dust.fadeIn = 0.5f;
                    dust.velocity *= 0.05f;
                    dust.noLight = true;
                    if (dust.type == 4)
                        dust.color = new Color(80, 170, 40, 120);
                }
                if (DoEffects && Main.rand.Next(5) == 0)
                {
                    Vector2 SpawnPos = TopLeftPosition;
                    SpawnPos.X += Main.rand.NextFloat() * Width;
                    SpawnPos.Y += Main.rand.NextFloat() * Height;
                    int pos = Gore.NewGore(SpawnPos, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, new int[]{
                     1024, 1025, 1026   
                    }), 0.65f);
                    Main.gore[pos].velocity *= 0.05f;
                }
            }
            if (HasFlag(GuardianFlags.WitheredArmor))
            {
                DrawRPercent *= 0.75f;
                DrawGPercent *= 0.5f;
            }
            if (DoEffects && HasFlag(GuardianFlags.WitheredWeapon))
            {
                int pos = Dust.NewDust(TopLeftPosition - Vector2.One * 2, Width + 4, Height + 4, 272, 0f, 0f, 50, default(Color), 0.5f);
                Main.dust[pos].velocity *= 1.6f;
                Main.dust[pos].velocity.Y -= 1f;
                Main.dust[pos].position = Vector2.Lerp(Main.dust[pos].position, CenterPosition, 0.5f);
            }
            if (HasFlag(GuardianFlags.Petrified))
            {
                DrawRPercent = DrawGPercent = DrawBPercent = 0.5f;
            }
            if (DrawRPercent != 1 || DrawGPercent != 1 || DrawBPercent != 1 || DrawAPercent != 1)
            {
                if (HasFlag(GuardianFlags.OnFire) || HasFlag(GuardianFlags.OnCursedFire) || HasFlag(GuardianFlags.FrostBurn))
                {
                    GetImmuneAlpha(color, 0f);
                }
                else
                {
                    color.R = (byte)(color.R * DrawRPercent);
                    color.G = (byte)(color.G * DrawGPercent);
                    color.B = (byte)(color.B * DrawBPercent);
                    color.A = (byte)(color.A * DrawAPercent);
                }
            }
        }

        public Color GetImmuneAlpha(Color color, float AlphaReduction)
        {
            float Percentage = (float)(255 - ImmuneAlpha) / 255f;
            if (AlphaReduction > 0)
                Percentage *= 1f - AlphaReduction;
            if (ImmuneAlpha > 125)
                return Color.Transparent;
            return Color.Multiply(color, Percentage);
        }

        public void DrawBuffWheel()
        {
            if (Buffs.Count < 1) return;
            float BuffSum = (1f / Buffs.Count) * 6.283185307179586f;
            float CurrentBuffRotation = 0f;
            foreach (BuffData b in Buffs)
            {
                Vector2 BuffPosition = new Vector2(-((float)Math.Sin(CurrentBuffRotation) * Width) - 16, -((float)Math.Cos(CurrentBuffRotation) * Height) - 16) + CenterPosition - Main.screenPosition;
                CurrentBuffRotation += BuffSum;
                Main.spriteBatch.Draw(Main.buffTexture[b.ID], BuffPosition, Color.White * 0.5f);
            }
        }

        public void DrawWings(SpriteEffects seffects, Color c)
        {
            if (Ducking || BodyAnimationFrame == Base.BedSleepingFrame || BodyAnimationFrame == Base.ThroneSittingFrame || BodyAnimationFrame == Base.DownedFrame
                || BodyAnimationFrame == Base.ReviveFrame || BodyAnimationFrame == Base.PetrifiedFrame || WingType < 1 || !Main.wingsLoaded[WingType]) return;
            Vector2 WingCenter = Vector2.Zero;//CenterPosition - Main.screenPosition;
            WingCenter.X = PositionWithOffset.X - Main.screenPosition.X;
            WingCenter.Y = PositionWithOffset.Y - Main.screenPosition.Y;
            int WingFramePosX, WingFramePosY;
            Base.WingPosition.GetPositionFromFrame(BodyAnimationFrame, out WingFramePosX, out WingFramePosY);
            if (WingFramePosX <= -1000 || WingFramePosY <= -1000)
                return;
            if (WingFramePosX == 0 && WingFramePosY == 0)
            {
                WingCenter.X += Width * 0.1f * -Direction;
                WingCenter.Y -= Height * 0.5f * GravityDirection;
                //WingCenter.Y += 4;
            }
            else
            {
                //if (GravityDirection < 0)
                //    WingFramePosY = (int)(SpriteHeight - WingCenter.Y);
                float WingPosX = WingFramePosX - SpriteWidth * 0.5f;
                if (LookingLeft)
                    WingPosX *= -1;//SpriteWidth - WingPosX;
                WingCenter.X += WingPosX * Scale;
                WingCenter.Y += (-SpriteHeight + WingFramePosY) * GravityDirection * Scale;
                //else WingCenter.Y -= WingFramePosY;
                //else WingCenter.Y += SpriteHeight;
            }
            Texture2D wing = Main.wingsTexture[WingType];
            float WingSize = 1f;
            if (Base.Size == GuardianBase.GuardianSize.Large)
                WingSize = 1.5f;
            if (Base.Size == GuardianBase.GuardianSize.Small)
                WingSize = 0.5f;
            int WingHeight = (int)(wing.Height * 0.25f);
            Vector2 WingOrigin = new Vector2(wing.Width * 0.5f, WingHeight * 0.5f);
            GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.Wings, wing, WingCenter, new Rectangle(0, WingHeight * WingFrame, wing.Width, WingHeight), c, Rotation, WingOrigin, WingSize * Scale, seffects);
            AddDrawData(dd, false);
        }

        public void DrawChains()
        {
            if (IsBeingPulledByPlayer && OwnerPos > -1)
            {
                Vector2 PlayerCenter = Main.player[OwnerPos].Center;
                Vector2 MyCenter = CenterPosition;
                Vector2 ChainDirection = PlayerCenter - MyCenter;
                float Rotation = (float)Math.Atan2(ChainDirection.Y, ChainDirection.X) - 1.57f;
                bool CreateNewChain = true;
                while (CreateNewChain)
                {
                    float RemainingDistance = (float)Math.Sqrt(ChainDirection.X * ChainDirection.X + ChainDirection.Y * ChainDirection.Y);
                    if (RemainingDistance < 12)
                    {
                        CreateNewChain = false;
                    }
                    else
                    {
                        RemainingDistance = (float)Main.chainTexture.Height / RemainingDistance;
                        ChainDirection.X *= RemainingDistance;
                        ChainDirection.Y *= RemainingDistance;
                        MyCenter.X += ChainDirection.X;
                        MyCenter.Y += ChainDirection.Y;
                        ChainDirection.X = PlayerCenter.X - MyCenter.X;
                        ChainDirection.Y = PlayerCenter.Y - MyCenter.Y;
                        Color color = Lighting.GetColor((int)(MyCenter.X * DivisionBy16), (int)(MyCenter.Y * DivisionBy16));
                        GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.PreDrawEffect, Main.chainTexture, MyCenter - Main.screenPosition, null, color, Rotation, new Vector2(Main.chainTexture.Width, Main.chainTexture.Height) * 0.5f, 1f, SpriteEffects.None);
                        AddDrawData(dd, true);
                    }
                }
            }
        }

        public void DrawWofExtras()
        {
            if (Main.wof >= 0 && WofFacing && WofTongued && !Downed)
            {
                NPC wof = Main.npc[Main.wof];
                Vector2 WofCenter = wof.Center;
                Vector2 MyCenter = CenterPosition;
                if (Downed)
                    MyCenter.Y += Height * 0.666f;
                Vector2 ChainDirection = WofCenter - MyCenter;
                float Rotation = (float)Math.Atan2(ChainDirection.Y, ChainDirection.X) - 1.57f;
                bool CreateNewChain = true;
                while (CreateNewChain)
                {
                    float RemainingDistance = (float)Math.Sqrt(ChainDirection.X * ChainDirection.X + ChainDirection.Y * ChainDirection.Y);
                    if (RemainingDistance < 40f)
                    {
                        CreateNewChain = false;
                    }
                    else
                    {
                        RemainingDistance = (float)Main.chain12Texture.Height / RemainingDistance;
                        ChainDirection.X *= RemainingDistance;
                        ChainDirection.Y *= RemainingDistance;
                        MyCenter.X += ChainDirection.X;
                        MyCenter.Y += ChainDirection.Y;
                        ChainDirection.X = WofCenter.X - MyCenter.X;
                        ChainDirection.Y = WofCenter.Y - MyCenter.Y;
                        Color color = Lighting.GetColor((int)(MyCenter.X * DivisionBy16), (int)(MyCenter.Y * DivisionBy16));
                        GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.PreDrawEffect, Main.chain12Texture, MyCenter - Main.screenPosition, null, color, Rotation, new Vector2(Main.chain12Texture.Width, Main.chain12Texture.Height) * 0.5f, 1f, SpriteEffects.None);
                        AddDrawData(dd, false);
                    }
                }
            }
        }

        private Vector2[] TestWeaponOrigins = new Vector2[] { new Vector2(66, 34), new Vector2(42, 36), new Vector2(24, 46), new Vector2(36, 54), new Vector2(58, 54) };

        public void DrawItem(Vector2 Position, SpriteEffects seffect, bool RightArm)
        {
            if (HasFlag(GuardianFlags.Petrified) && Base.PetrifiedFrame > -1)
                return;
            if ((int)ItemUseType < 0)
                return;
            GuardianDrawData dd;
            bool drawOnFront = (!RightArm && DrawLeftBodyPartsInFrontOfPlayer) || (RightArm && DrawRightBodyPartsInFrontOfPlayer);
            Vector2 ItemPosition = Position;
            bool DualWield = false;
            if (this.IsDualWielding && ((HeldOffHand == HeldHand.Right && RightArm) || (HeldOffHand == HeldHand.Left && !RightArm)))
            {
                DualWield = true;
                ItemPosition.X += OffHandPositionX;
                ItemPosition.Y += OffHandPositionY;
                if (OffHandPositionX < -100 || OffHandPositionY < -100)
                    return;
            }
            else
            {
                ItemPosition.X += ItemPositionX;
                ItemPosition.Y += ItemPositionY;
                if (ItemPositionX < -100 || ItemPositionY < -100)
                    return;
            }
            bool CorrectHand = ((HeldItemHand == HeldHand.Both || HeldItemHand == HeldHand.Left) && !RightArm) || (HeldItemHand == HeldHand.Right && RightArm) || DualWield;
            if (CorrectHand)
            {
                bool ShowItem = ItemAnimationTime > 0;
                if (ShowItem)
                {
                    if (SelectedItem > -1)
                    {
                        Item Item = Inventory[SelectedItem];
                        float ItemRotation = this.ItemRotation;
                        if (DualWield)
                            ItemRotation = OffhandRotation;
                        if (!Item.noUseGraphic)
                        {
                            bool Inclined = Inclined45Degrees(Item);
                            int TileX = (int)(this.Position.X * DivisionBy16), TileY = (int)(CenterY * DivisionBy16);
                            Color c = Lighting.GetColor(TileX, TileY, Color.White);
                            if (ItemUseType == ItemUseTypes.HeavyVerticalSwing || ItemUseType == ItemUseTypes.LightVerticalSwing)
                            {
                                Vector2 ItemOrigin = GetItemOrigin(Item, true);//giantsummon.GetGuardianItemData(Item.type).ItemOrigin;
                                if (LookingLeft)
                                {
                                    if (Inclined)
                                    {
                                        ItemOrigin.X = Main.itemTexture[Item.type].Width - ItemOrigin.X;
                                    }
                                    else
                                    {
                                        ItemOrigin.X = Item.width - ItemOrigin.X;
                                    }
                                }
                                Main.spriteBatch.Draw(Main.itemTexture[Item.type], ItemPosition, null, c, ItemRotation, ItemOrigin, ItemScale, seffect, 0f);
                            }
                            else if (ItemUseType == ItemUseTypes.HorizontalSlash)
                            {

                            }
                            else if (ItemUseType == ItemUseTypes.AimingUse)
                            {
                                Vector2 ItemOrigin = GetItemOrigin(Item);//giantsummon.GetGuardianItemData(Item.type).ItemOrigin;
                                float NewRotation = ItemRotation;
                                SpriteEffects sfx = seffect;
                                if (!LookingLeft)
                                {
                                    ItemOrigin.X = Main.itemTexture[Item.type].Width - ItemOrigin.X;
                                    ItemOrigin.Y = Main.itemTexture[Item.type].Height - ItemOrigin.Y;
                                    //sfx = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                                    if (GravityDirection < 0)
                                        sfx = SpriteEffects.FlipHorizontally;
                                    else
                                        sfx = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                                }
                                else
                                {
                                    ItemOrigin.X = Main.itemTexture[Item.type].Width - ItemOrigin.X;
                                }
                                if (Item.staff[Item.type])
                                    NewRotation += 0.7853981633974483f * Direction;
                                /*if (sfx.HasFlag(SpriteEffects.FlipHorizontally))
                                    ItemOrigin.X = Item.width - ItemOrigin.X;
                                if (sfx.HasFlag(SpriteEffects.FlipVertically))
                                    ItemOrigin.Y = Item.height - ItemOrigin.Y;*/
                                dd = new GuardianDrawData(GuardianDrawData.TextureType.MainHandItem, Main.itemTexture[Item.type], ItemPosition, null, c, NewRotation, ItemOrigin, ItemScale, sfx);
                                AddDrawData(dd, drawOnFront);
                            }
                            else if (ItemUseType == ItemUseTypes.ItemDrink2h)
                            {
                                Vector2 ItemOrigin = GetItemOrigin(Item);
                                dd = new GuardianDrawData(GuardianDrawData.TextureType.MainHandItem, Main.itemTexture[Item.type], ItemPosition, null, c, ItemRotation, ItemOrigin, ItemScale, SpriteEffects.None);
                                AddDrawData(dd, drawOnFront);
                            }
                            else if (ItemUseType == ItemUseTypes.OverHeadItemUse)
                            {
                                Texture2D Texture = Main.itemTexture[Item.type];
                                Vector2 ItemOrigin = new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f);
                                dd = new GuardianDrawData(GuardianDrawData.TextureType.MainHandItem, Main.itemTexture[Item.type], ItemPosition, null, c, ItemRotation, ItemOrigin, ItemScale, SpriteEffects.None);
                                AddDrawData(dd, drawOnFront);
                            }
                        }
                    }
                }
            }
            if (SelectedOffhand > -1 && !DualWield && ShowOffHand && (((HeldOffHand == HeldHand.Both || HeldOffHand == HeldHand.Left) && !RightArm) || (HeldOffHand == HeldHand.Right && RightArm)))
            {
                Item Item = Inventory[SelectedOffhand];
                bool Inclined = Inclined45Degrees(Item);
                int TileX = (int)(this.Position.X * DivisionBy16), TileY = (int)(CenterY * DivisionBy16);
                Color c = Lighting.GetColor(TileX, TileY, Color.White);
                Vector2 ItemOrigin = GetItemOrigin(Item);//giantsummon.GetGuardianItemData(Item.type).ItemOrigin;
                float NewRotation = OffhandRotation;
                SpriteEffects sfx = seffect;
                if (LookingLeft)
                {
                    //ItemOrigin.X = Item.width - ItemOrigin.X;
                    //ItemOrigin.Y = Item.height - ItemOrigin.Y;
                }
                /*if (sfx.HasFlag(SpriteEffects.FlipHorizontally))
                    ItemOrigin.X = Item.width - ItemOrigin.X;
                if (sfx.HasFlag(SpriteEffects.FlipVertically))
                    ItemOrigin.Y = Item.height - ItemOrigin.Y;*/
                drawOnFront = HeldOffHand == HeldHand.Left && DrawLeftBodyPartsInFrontOfPlayer;
                Vector2 OffhandPosition = new Vector2(OffHandPositionX, OffHandPositionY) + Position;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.OffHandItem, Main.itemTexture[Item.type], OffhandPosition, null, c, OffhandRotation, ItemOrigin, Scale * Item.scale, sfx);
                AddDrawData(dd, drawOnFront);
            }
            if (HeldProj > -1 && CorrectHand) //Has problems drawing...
            {
                int ProjType = Main.projectile[HeldProj].type;
                bool MayDraw = (ProjType == 460 || ProjType == 535 || ProjType == 600);
                MayDraw = true;
                ProjectileLoader.DrawHeldProjInFrontOfHeldItemAndArms(Main.projectile[HeldProj], ref MayDraw);
                //Draw projectile...
                if (MayDraw)
                {
                    int ProjOwner = Main.projectile[HeldProj].owner;
                    if (ProjOwner > -1)
                    {
                        int ProjPosBackup = Main.player[ProjOwner].heldProj;
                        Main.player[ProjOwner].heldProj = HeldProj;
                        Main.instance.DrawProj(HeldProj);
                        Main.player[ProjOwner].heldProj = ProjPosBackup;
                    }
                    else
                        HeldProj = -1;
                }
            }
        }

        public enum ItemUseTypes
        {
            CursedAttackAttempt = -2,
            ClawAttack = -1,
            HeavyVerticalSwing = 0,
            ItemDrink2h = 1,
            HorizontalSlash = 2,
            OverHeadItemUse = 3,
            AimingUse = 4,
            LightVerticalSwing = 5 //Like Terraria's vertical swing. 1 handed though.
        }

        public enum Emotions : byte
        {
            Neutral,
            Happy,
            Angry,
            Sad,
            Alarmed,
            BadIntentioned,
            Ashamed,
            Sweat,
            Sleepy,
            Question
        }

        public enum IdleActions : byte
        {
            Listening,
            Wait,
            Wander,
            WanderHome,
            UseNearbyFurniture,
            UseNearbyFurnitureHome,
            TryGoingSleep,
            GoHome,
            LookingAtTheBackground,
            DefendVillage,
            JoinOldOneArmy
        }
    }

    public enum HeldHand : byte
    {
        Both,
        Left,
        Right
    }

    public enum CombatTactic : byte
    {
        Charge = 2,
        Assist = 1,
        Snipe = 0
    }

    public enum PrioritaryBehavior: byte
    {
        None,
        Jump
    }

    public enum DrawMoment: byte
    {
        DontDraw,
        DrawAfterDrawingTiles,
        DrawBeforeDrawingNpcs
    }
}
