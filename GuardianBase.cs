using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using giantsummon.Creatures;

namespace giantsummon
{
    public class GuardianBase
    {
        private static Dictionary<string, GuardianBaseContainer> GuardianList = new Dictionary<string, GuardianBaseContainer>();
        public delegate void ModGuardianDB(int ID, out GuardianBase guardian);
        public InvalidGuardianPoints invalidGuardianPoints = new InvalidGuardianPoints(true);
        public delegate void GuardianModDel(TerraGuardian guardian);
        public delegate void GuardianBehaviorModDel(TerraGuardian guardian, ref bool AllowAIMovement);
        
        public bool InvalidGuardian = false;
        public string Name = "", Description = "";
        public int Width = 32, Height = 82, DuckingHeight = 52;
        public int SpriteWidth = 96, SpriteHeight = 96, FramesInRows = 20;
        public float Mass = 0.5f, MaxSpeed = 4.5f, Acceleration = 0.1f, SlowDown = 0.3f;
        public bool Male = true;
        public int MaxJumpHeight = 15;
        public float Accuracy = 0.9f;
        public float JumpSpeed = 7.08f;
        public int InitialMHP = 100, LifeCrystalHPBonus = 20, LifeFruitHPBonus = 5;
        public int InitialMP = 20, ManaCrystalMPBonus = 20;
        public int MaxBreath = 200, BreathCooldown = 7;
        public int BlockRate = 0, DodgeRate = 0;
        public float MountBurdenPercentage = 0.05f;
        public bool DontUseRightHand = false, DontUseHeavyWeapons = false, CanChangeGender = false, OneHanded2HWeaponWield = false, IsWraith = false;
        public GuardianEffect Effect = GuardianEffect.None;
        private CompanionType companionType = CompanionType.TerraGuardian;
        public TerrarianCompanionInfos TerrarianInfo = null;
        public List<RequestBase> RequestDB = new List<RequestBase>();
        public List<Reward> RewardsList = new List<Reward>();

        public GuardianSprites sprites;
        public SoundData HurtSound, DeadSound;
        public GuardianAnimationPoints LeftHandPoints = new GuardianAnimationPoints(),
            RightHandPoints = new GuardianAnimationPoints(),
            MountShoulderPoints = new GuardianAnimationPoints(),
            LeftArmOffSet = new GuardianAnimationPoints(),
            RightArmOffSet = new GuardianAnimationPoints(),
            HeadVanityPosition = new GuardianAnimationPoints(),
            WingPosition = new GuardianAnimationPoints();
        public Point SittingPoint = Point.Zero, SleepingOffset = Point.Zero;
        public GuardianSize Size = GuardianSize.Medium;
        public int Age = 15;
        public bool CanDuck = true, ReverseMount = false, DrinksBeverage = true;
        public List<ItemPair> InitialItems = new List<ItemPair>();
        //Animation frames related stuff
        public int StandingFrame = 0;
        public int[] WalkingFrames = new int[]{0};
        public int JumpFrame = 0;
        public int[] HeavySwingFrames = new int[] { 0,0,0 };
        public int[] ItemUseFrames = new int[] { 0,0,0,0 };
        public int DuckingFrame = 0;
        public int[] DuckingSwingFrames = new int[] { 0,0,0 };
        public int[] SittingItemUseFrames = null;
        public int SittingFrame = 0, ChairSittingFrame = -1, ThroneSittingFrame = -1, BedSleepingFrame = -1, ReviveFrame = -1, DownedFrame = -1, PetrifiedFrame = -1;
        public int PlayerMountedArmAnimation = -1;
        public Dictionary<int, int> BodyFrontFrameSwap = new Dictionary<int, int>(), RightArmFrontFrameSwap = new Dictionary<int,int>();
        public float HeavySwingFrameTime = 1f, WalkAnimationFrameTime = 0f;
        public float MaxWalkSpeedTime { get { return WalkAnimationFrameTime * WalkingFrames.Length; } }
        public List<int> DrawLeftArmInFrontOfHead = new List<int>();
        public bool SpecificBodyFrontFramePositions = false;
        /// <summary>
        /// Don't lie. :)
        /// </summary>
        protected ushort PopularityContestsWon = 0, ContestSecondPlace = 0, ContestThirdPlace = 0;
        public virtual string CallUnlockMessage { get { return "*It seems like I can call this guardian whenever I want.*"; } }
        public virtual string MountUnlockMessage { get { return "*It seems like I can mount this guardian whenever I want.*"; } }
        public virtual string ControlUnlockMessage { get { return "*It seems like I can control It's movement whenever I want.*"; } }

        public virtual string FriendLevelMessage { get { return "*The guardian now considers you a friend.*"; } }
        public virtual string BestFriendLevelMessage { get { return "*The guardian now considers you It's best friend.*"; } }
        public virtual string BFFLevelMessage { get { return "*The guardian now considers you It's best friend forever.*"; } }
        public virtual string BuddyForLifeLevelMessage { get { return "*The guardian now considers you It's buddy for life.*"; } }

        public virtual string LeavingWorldMessageGuardianSummoned { get { return " things were packed out of the world."; } }
        public virtual string LeavingWorldMessage { get { return " has moved out of the world."; } }

        public byte KnownLevel = 2, FriendsLevel = 5, BestFriendLevel = 12, BestFriendForeverLevel = 18,  BuddiesForLife = 25;
        public byte CallUnlockLevel = 0, LootingUnlockLevel = 3, MaySellYourLoot = 4, MountUnlockLevel = 5, StopMindingAFK = 7, MountDamageReductionLevel = 9, ControlUnlockLevel = 10, FriendshipBondUnlockLevel = 12, FallDamageReductionLevel = 15;
        
        public const int Rococo = 0,
            Blue = 1,
            Sardine = 2,
            Zacks = 3,
            Nemesis = 4,
            Alex = 5,
            Brutus = 6,
            Bree = 7,
            Mabel = 8,
            Domino = 9,
            Leopold = 10,
            Vladimir = 11,
            Malisha = 12,
            Michelle = 13;

        public void SetTerraGuardian()
        {
            companionType = CompanionType.TerraGuardian;
            TerrarianInfo = null;
        }

        public void SetTerrarian()
        {
            companionType = CompanionType.Terrarian;
            TerrarianInfo = new TerrarianCompanionInfos();

            Width = 20;
            Height = 42;
            //DuckingHeight = 52;
            SpriteWidth = 40;
            SpriteHeight = 56;
            Size = GuardianSize.Medium;

            CanDuck = false;
            ReverseMount = false;
            DontUseRightHand = true;
            DontUseHeavyWeapons = true;

            HurtSound = new SoundData(Terraria.ID.SoundID.PlayerHit);
            DeadSound = new SoundData(Terraria.ID.SoundID.PlayerKilled);

            MountUnlockLevel = 255;
            ControlUnlockLevel = 255;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            JumpFrame = 5;
            PlayerMountedArmAnimation = 10;
            HeavySwingFrames = new int[] { 1, 2, 4 };
            ItemUseFrames = new int[] { 1, 2, 3, 4 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 6;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 1, 2 });

            //Left Hand
            LeftHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(17 * 2, 31 * 2);
            LeftHandPoints.AddFramePoint2x(1, 5, 10);
            LeftHandPoints.AddFramePoint2x(2, 12, 10);
            LeftHandPoints.AddFramePoint2x(3, 13, 17);
            LeftHandPoints.AddFramePoint2x(4, 12, 20);

            //Right Hand
            RightHandPoints.AddFramePoint2x(0, 14, 19);
            RightHandPoints.AddFramePoint2x(1, 15, 17);
            RightHandPoints.AddFramePoint2x(2, 12, 19);
            RightHandPoints.AddFramePoint2x(3, 12, 17);
            RightHandPoints.AddFramePoint2x(4, 12, 17);

            RightHandPoints.AddFramePoint2x(5, 15, 9);

            RightHandPoints.AddFramePoint2x(6, 15, 17);
            RightHandPoints.AddFramePoint2x(7, 16, 16);
            RightHandPoints.AddFramePoint2x(8, 16, 16);
            RightHandPoints.AddFramePoint2x(9, 16, 16);
            RightHandPoints.AddFramePoint2x(10, 15, 17);
            RightHandPoints.AddFramePoint2x(11, 15, 17);
            RightHandPoints.AddFramePoint2x(12, 15, 17);
            RightHandPoints.AddFramePoint2x(13, 15, 17);
            RightHandPoints.AddFramePoint2x(14, 14, 16);
            RightHandPoints.AddFramePoint2x(15, 13, 16);
            RightHandPoints.AddFramePoint2x(16, 13, 16);
            RightHandPoints.AddFramePoint2x(17, 14, 17);
            RightHandPoints.AddFramePoint2x(18, 15, 17);
            RightHandPoints.AddFramePoint2x(19, 15, 17);

            //Mount Position
            //MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(10 * 2, 21 * 2);
            //MountShoulderPoints.AddFramePoint2x(6, 10, 21);

            //Sitting Position
            SittingPoint = new Point(10 * 2, 21 * 2);
        }

        public bool IsTerraGuardian { get { return companionType == CompanionType.TerraGuardian; } }
        public bool IsTerrarian { get { return companionType == CompanionType.Terrarian; } }

        public void AddNewRequest(string Name, int RequestScore, string BriefText = "", string AcceptText = "", string DenyText = "", string CompleteInfo = "", string RequestActiveTalkText = "")
        {
            RequestBase rb = new RequestBase(Name, RequestScore, BriefText, AcceptText, DenyText, CompleteInfo, RequestActiveTalkText);
            RequestDB.Add(rb);
        }

        public void AddRequestRequirement(RequestBase.RequestRequirementDel requirement)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].Requirement = requirement;
            }
        }

        public void AddHuntObjective(int MobID, int Stack, float StackPerFriendshipLevel = 0.333f)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddHuntObjective(MobID, Stack, StackPerFriendshipLevel);
            }
        }

        public void AddItemCollectionObjective(int ItemID, int Stack, float StackPerFriendshipLevel = 0.333f)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddItemCollectionRequest(ItemID, Stack, StackPerFriendshipLevel);
            }
        }

        public void AddExploreObjective(float InitialDistance = 1000f, float ExtraDistancePerFriendshipLevel = 100f, bool RequiresRequesterSummoned = true)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddExploreRequest(InitialDistance, ExtraDistancePerFriendshipLevel, RequiresRequesterSummoned);
            }
        }

        public void AddEventKillParticipation(int EventID, int Kills, float ExtraKills = 0f) //TODO - Change this event participation, to add a new objective specific for killing event monsters.
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddEventKillsRequest(EventID, Kills, ExtraKills);
            }
        }

        public void AddEventParticipationObjective(int EventID, int Waves, float ExtraWaves = 0f)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddEventParticipationRequest(EventID, Waves, ExtraWaves);
            }
        }

        public void AddRequesterSummonedRequirement()
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddRequesterRequirement();
            }
        }

        public void AddCompanionRequirement(int ID, string ModID = "")
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddCompanionRequirement(ID, ModID);
            }
        }

        public void AddKillBossRequest(int BossID, int GemLevelBonus = 0)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddKillBossRequest(BossID, GemLevelBonus);
            }
        }

        /// <summary>
        /// Don't forget to after setting up this request, use AddObjectDroppingMonsters() to setup the monsters and drop rate for the items.
        /// </summary>
        public void AddObjectCollectionRequest(string ObjectName, int ObjectCount, float ExtraObjectCountPerFriendshipLevel = 0.333f)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddObjectCollectionRequest(ObjectName, ObjectCount, ExtraObjectCountPerFriendshipLevel);
            }
        }

        public void AddObjectDroppingMonster(int MonsterID, float Rate)
        {
            if (RequestDB.Count > 0)
            {
                RequestDB[RequestDB.Count - 1].AddObjectDroppingMonster(MonsterID, Rate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="Stack"></param>
        /// <param name="ItemScore">Score value necessary for this item to be in the loot pool.</param>
        /// <param name="Chance">From 0 to 1. Chance of the item being picked if the score is higher or equal to this item score.</param>
        /// <param name="MaxExtraStack">Becareful with this, the extra stack deduces from the score based on the rng of extra stack got.</param>
        public void AddReward(int ItemID, int Stack, int ItemScore, float Chance = 1f, int MaxExtraStack = 0)
        {
            Reward rwd = new Reward();
            rwd.ItemID = ItemID;
            rwd.InitialStack = Stack;
            rwd.RewardScore = ItemScore;
            rwd.RewardChance = Chance;
            rwd.MaxExtraStack = MaxExtraStack;
            RewardsList.Add(rwd);
        }

        public int GetBodyFrontSprite(int Default)
        {
            if (BodyFrontFrameSwap.ContainsKey(Default))
                return BodyFrontFrameSwap[Default];
            return -1;
        }

        public int GetPopularityContestsWon()
        {
            return PopularityContestsWon;
        }

        public int GetPopularityContestsSecondPlace()
        {
            return ContestSecondPlace;
        }

        public int GetPopularityContestsThirdPlace()
        {
            return ContestThirdPlace;
        }

        public virtual void Attributes(TerraGuardian g)
        {

        }

        public virtual void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {

        }

        public virtual void ForceDrawInFrontOfPlayer(TerraGuardian guardian, ref bool LeftArmInFront, ref bool RightArmInFront)
        {

        }

        public virtual void GuardianUpdateScript(TerraGuardian guardian)
        {

        }

        public virtual void GuardianPreDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {

        }

        public virtual void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {

        }

        public virtual void ManageExtraDrawScript(GuardianSprites sprites)
        {

        }

        public virtual void GuardianBehaviorModScript(TerraGuardian guardian)
        {

        }

        public virtual void GuardianHorizontalMovementScript(TerraGuardian guardian, ref float MaxSpeed, ref float Acceleration, ref float SlowDown, ref float DashSpeed)
        {

        }

        public virtual bool GuardianWhenAttackedPlayer(TerraGuardian guardian, int Damage, bool Critical, Player Attacker)
        {
            return true;
        }

        public virtual bool GuardianWhenAttackedNPC(TerraGuardian guardian, int Damage, NPC Attacker)
        {
            return true;
        }

        public virtual bool GuardianWhenAttackedGuardian(TerraGuardian guardian, int Damage, bool Critical, TerraGuardian Attacker)
        {
            return true;
        }

        public virtual bool GuardianWhenAttackedProjectile(TerraGuardian guardian, int Damage, bool Critical, Projectile Proj)
        {
            return true;
        }

        public virtual bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
        {
            return true;
        }

        public virtual string GreetMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] liked to meet you.*";
        }

        public virtual string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] seems to have nothing to ask you for.*";
        }

        public virtual string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] wants you to do something.*";
        }

        public virtual string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] thanked you deeply.*";
        }

        public virtual string NormalMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] stares at you, waiting for you to do something.*";
        }

        public virtual string HomelessMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] would like to have a house to live.*";
        }

        public virtual string TalkMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] said something to you.*";
        }

        public virtual string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            return "*[name] invites you to party.*";
        }

        /// <summary>
        /// Depending on what calls it, make use of the correct thing.
        /// Is player tells if the target is a player or not.
        /// 
        /// If It's a player, use RevivePlayer if you want to add conditionals if the player is revived.
        /// </summary>
        /// <param name="IsPlayer"></param>
        /// <param name="RevivePlayer"></param>
        /// <param name="ReviveGuardian"></param>
        /// <returns></returns>
        public virtual string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            return "*[name] says to not worry, you'll be well soon.*";
        }

        public virtual void GuardianActionUpdate(TerraGuardian guardian, GuardianActions action)
        {

        }

        public virtual void GuardianActionUpdateAnimation(TerraGuardian guardian, GuardianActions action, ref bool UsingLeftArm, ref bool UsingRightArm)
        {

        }

        public virtual void GuardianActionDraw(TerraGuardian guardian, GuardianActions action)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimationID">0 = body, 1 = left arm, 2 = right arm</param>
        public virtual void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {

        }

        public void CalculateHealthToGive(int ResultMHP, float LCBonusPercentage = 1f, float LFBonusPercentage = 1f)
        {
            int LCHealth = (int)(ResultMHP * (0.6f * LCBonusPercentage / 15)), LFHealth = (int)(ResultMHP * (0.2f * LFBonusPercentage / 20));
            InitialMHP = ResultMHP - (LCHealth * 15 + LFHealth * 20);
            LifeCrystalHPBonus = LCHealth;
            LifeFruitHPBonus = LFHealth;
            //Main.NewText("Initial MHP: " + InitialMHP + "  LC:" + (LCHealth * 15) + "  LF: " + (LFHealth * 20));
        }

        public struct InvalidGuardianPoints
        {
            public GuardianAnimationPoints HandPoints,
                HeadVanityPosition;
            public int[] HeavySwingFrames, ItemUseFrames;

            public InvalidGuardianPoints(bool value = true)
            {
                HandPoints = new GuardianAnimationPoints();
                HeadVanityPosition = new GuardianAnimationPoints();

                ItemUseFrames = new int[] { 1, 2, 3, 4 };
                HeavySwingFrames = new int[] { 5, 6, 7 };

                HandPoints.AddFramePoint2x(1, 1, 2);
                HandPoints.AddFramePoint2x(2, 11, 2);
                HandPoints.AddFramePoint2x(3, 11, 11);
                HandPoints.AddFramePoint2x(4, 11, 18);

                HandPoints.AddFramePoint2x(5, 0, 0);
                HandPoints.AddFramePoint2x(6, 11, 0);
                HandPoints.AddFramePoint2x(7, 11, 20);

                HeadVanityPosition.DefaultCoordinate2x = new Point(6, 3);
            }
        }

        public struct ItemPair
        {
            public int ItemID, Stack;

            public ItemPair(int ID, int Stack)
            {
                ItemID = ID;
                this.Stack = Stack;
            }
        }

        public GuardianBase()
        {
        }

        public static GuardianBase GetGuardianBase(int ID, string modid = "") //Add Mod Id
        {
            if (modid == "")
                modid = MainMod.mod.Name;
            if (!GuardianList.ContainsKey(modid))
            {
                GuardianBaseContainer gbc = new GuardianBaseContainer(modid);
                GuardianList.Add(modid, gbc);
            }
            GuardianList[modid].GetGuardian(ID);
            return GuardianList[modid].GetGuardian(ID);
        }

        public static int GetTotalGuardianDataCount()
        {
            int c = 0;
            foreach (GuardianBaseContainer gbc in GuardianList.Values)
            {
                c += gbc.GetGuardianCount;
            }
            return c;
        }

        public void GetBetweenHandsPosition(int Frame, out int X, out int Y)
        {
            Microsoft.Xna.Framework.Point p = GetBetweenHandsPosition(Frame);
            X = p.X;
            Y = p.Y;
        }

        public Microsoft.Xna.Framework.Point GetBetweenHandsPosition(int Frame)
        {
            if (DontUseRightHand)
                return LeftHandPoints.GetPositionFromFramePoint(Frame);
            Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point(), l = LeftHandPoints.GetPositionFromFramePoint(Frame), r = RightHandPoints.GetPositionFromFramePoint(Frame);
            p.X = l.X + (int)((r.X - l.X) * 0.5f);
            p.Y = l.Y + (int)((r.Y - l.Y) * 0.5f);
            return p;
        }

        public void AddInitialItem(int ID, int Stack = 1)
        {
            InitialItems.Add(new ItemPair(ID, Stack));
        }

        public static GuardianBase GuardianDB(int id, Mod mod) //In the future, maybe other mods support?
        {
            bool InvalidGuardian = false;
            GuardianBase gb = null;
            if (mod == null)
            {
                InvalidGuardian = true;
            }
            else if (mod != MainMod.mod)
            {
                if (!MainMod.TryGettingGuardianInfo(id, mod, out gb))
                {
                    InvalidGuardian = true;
                }
            }
            else
            {
                switch (id)
                {
                    default:
                        InvalidGuardian = true;
                        break;
                    case 0:
                        gb = new RococoBase();
                        break;
                    case 1:
                        gb = new BlueBase();
                        break;
                    case 2:
                        gb = new SardineBase();
                        break;
                    case 3:
                        gb = new ZacksBase();
                        break;
                    case 4:
                        gb = new NemesisBase();
                        break;
                    case 5:
                        gb = new AlexBase();
                        break;
                    case 6:
                        gb = new BrutusBase();
                        break;
                    case 7:
                        gb = new BreeBase();
                        break;
                    case 8:
                        gb = new MabelBase();
                        break;
                    case 9:
                        gb = new DominoBase();
                        break;
                    case 10:
                        gb = new LeopoldBase();
                        break;
                    case 11:
                        gb = new VladimirBase();
                        break;
                    case 12:
                        gb = new MalishaBase();
                        break;
                    case 13:
                        gb = new MichelleBase();
                        break;
                }
            }
            if (gb == null)
            {
                gb = new GuardianBase();
                InvalidGuardian = true;
            }
            gb.InvalidGuardian = InvalidGuardian;
            if (gb.SittingPoint.X == 0 && gb.SittingPoint.Y == 0)
            {
                gb.SittingPoint.X = gb.SpriteWidth / 2;
                gb.SittingPoint.Y = gb.SpriteHeight;
            }
            if (!InvalidGuardian)
            {
                gb.sprites = new GuardianSprites("Creatures/" + gb.Name, mod);
                gb.ManageExtraDrawScript(gb.sprites);
            }
            else
                gb.SetInvalidGuardianStatus();
            if (gb.HurtSound == null)
                gb.HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit1);
            if (gb.DeadSound == null)
                gb.DeadSound = new SoundData(5);
            //Setting animation creation data here
            if (gb.WalkAnimationFrameTime == 0)
            {
                gb.WalkAnimationFrameTime = 32f / gb.WalkingFrames.Length;
            }
            return gb;
        }

        public void SetInvalidGuardianStatus()
        {
            Name = "Unknown";
            Description = "Your memories of it are fragmented, making it formless.";
            Size = GuardianSize.Medium;
            Width = 24;
            Height = 48;
            DuckingHeight = 48;
            SpriteWidth = 24;
            SpriteHeight = 48;
            CanDuck = false;
            MountUnlockLevel = 255;
            ItemUseFrames = invalidGuardianPoints.ItemUseFrames;
            HeadVanityPosition = invalidGuardianPoints.HeadVanityPosition;
            LeftHandPoints = invalidGuardianPoints.HandPoints;
            RightHandPoints = invalidGuardianPoints.HandPoints;
            HeadVanityPosition = invalidGuardianPoints.HeadVanityPosition;
        }

        public static void UnloadGuardians()
        {
            string[] Keys = GuardianList.Keys.ToArray();
            foreach (string k in Keys)
            {
                GuardianList[k].Dispose();
                GuardianList.Remove(k);
            }
        }

        public static void UpdateContainers()
        {
            string[] keys = GuardianList.Keys.ToArray();
            foreach (string key in keys)
            {
                GuardianList[key].UpdateContainers();
            }
        }

        public class TerrarianCompanionInfos
        {
            public Color HairColor, SkinColor, EyeColor, ShirtColor, UnderShirtColor, PantsColor, ShoeColor;
            public int HairStyle, SkinVariant;

            public TerrarianCompanionInfos()
            {
                HairColor = new Color(215, 90, 55);
                SkinColor = new Color(255, 125, 90);
                EyeColor = new Color(105, 90, 75);
                ShirtColor = new Color(175, 165, 140);
                UnderShirtColor = new Color(160, 180, 215);
                PantsColor = new Color(255, 230, 175);
                ShoeColor = new Color(160, 105, 60);
                HairStyle = 1;
                SkinVariant = 0;
            }

            public int GetSkinVariant(bool Male)
            {
                if (Male)
                {
                    switch (SkinVariant)
                    {
                        case 0:
                            return Terraria.ID.PlayerVariantID.MaleStarter;
                        case 1:
                            return Terraria.ID.PlayerVariantID.MaleSticker;
                        case 2:
                            return Terraria.ID.PlayerVariantID.MaleGangster;
                        case 3:
                            return Terraria.ID.PlayerVariantID.MaleCoat;
                        case 4:
                            return Terraria.ID.PlayerVariantID.MaleDress;
                    }
                }
                else
                {
                    switch (SkinVariant)
                    {
                        case 0:
                            return Terraria.ID.PlayerVariantID.FemaleStarter;
                        case 1:
                            return Terraria.ID.PlayerVariantID.FemaleSticker;
                        case 2:
                            return Terraria.ID.PlayerVariantID.FemaleGangster;
                        case 3:
                            return Terraria.ID.PlayerVariantID.FemaleCoat;
                        case 4:
                            return Terraria.ID.PlayerVariantID.FemaleDress;
                    }
                }
                return 0;
            }
        }

        public enum CompanionType
        {
            TerraGuardian,
            Terrarian
        }

        public enum GuardianEffect
        {
            None,
            Wraith
        }

        public enum GuardianSize
        {
            Small,
            Medium,
            Large
        }
    }
}
