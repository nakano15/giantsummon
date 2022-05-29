using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using giantsummon.Companions;

namespace giantsummon
{
    public class GuardianBase
    {
        private static Dictionary<string, GuardianBaseContainer> GuardianList = new Dictionary<string, GuardianBaseContainer>();
        public delegate void ModGuardianDB(int ID, out GuardianBase guardian);
        public InvalidGuardianPoints invalidGuardianPoints = new InvalidGuardianPoints(true);
        public delegate void GuardianModDel(TerraGuardian guardian);
        public delegate void GuardianBehaviorModDel(TerraGuardian guardian, ref bool AllowAIMovement);
        public const string TerraGuardianGroupID = "guardian", 
            TerrarianGroupID = "terrarian", 
            TerraGuardianCaitSithGroupID = "caitsithguardian", 
            GiantDogGuardianGroupID = "giantdogguardian";
        public const string DefaultCreaturesDirectory = "Companions/Creatures/",
            DefaultTerrariansDirectory = "Companions/Terrarians/";

        public virtual GuardianData GetGuardianData(int ID = -1, string ModID = "") { return new GuardianData(ID, ModID);}

        public GuardianID[] IsSameAs = new GuardianID[0]; //Companions that are literally the same or result of another, should have this.
        public Group GetGroup { get { return MainMod.GetGroup(GroupID); } }
        public bool InvalidGuardian = false;
        public string Name = "", Description = "";
        public string SpritesDirectory = "";
        public string WikiPageLink = null;
        public string[] PossibleNames = new string[0];
        public string GroupID = TerraGuardianGroupID;
        public int Width = 32, Height = 82, DuckingHeight = 52;
        public int SpriteWidth = 96, SpriteHeight = 96, FramesInRows = 20;
        public float Mass = 0.5f, MaxSpeed = 4.5f, Acceleration = 0.1f, SlowDown = 0.3f;
        public float TownNpcSlot = 1f;
        public float CompanionSlotWeight = 1f;
        public Genders Gender = Genders.Male;
        public bool Male { get
            {
                return Gender == Genders.Male;
            }
            set
            {
                Gender = (value ? Genders.Male : Genders.Female);
            }
        }
        public bool Genderless
        {
            get
            {
                return Gender == Genders.Genderless;
            }
            set
            {
                Gender = (value ? Genders.Genderless : Genders.Male);
            }
        }
        public int MaxJumpHeight = 15;
        public float Scale = 1f;
        public bool ForceScale = false;
        /// <summary>
        /// If the companion is contributed by a player, set their name to that variable, and a heart showing the name of the contributor upon passing the mouse over will appear.
        /// </summary>
        public string CompanionContributorName = "";
        public float GetScale { get { if (MainMod.UseCompanionsDefinedScaleChange || ForceScale) return Scale; else return 1f; } }
        public float Accuracy = 0.9f, Agility = 0.5f, Trigger = 0.5f;
        public float JumpSpeed = 7.08f;
        public float InitialMHP = 100, LifeCrystalHPBonus = 20, LifeFruitHPBonus = 5;
        public float InitialMP = 20, ManaCrystalMPBonus = 20;
        public int MaxBreath = 200, BreathCooldown = 7;
        public int BlockRate = 0, DodgeRate = 0;
        public float MountBurdenPercentage = 0.05f;
        public bool UsesRightHandByDefault = false, DontUseLeftHand = false, DontUseRightHand = false, ForceWeaponUseOnMainHand = false;
        public bool DontUseHeavyWeapons = false;
        public bool CanChangeGender = false;
        public bool OneHanded2HWeaponWield = false;
        public bool IsWraith = false;
        public GuardianEffect Effect = GuardianEffect.None;
        public TerrarianCompanionInfos TerrarianInfo = null;
        public List<RequestReward> RewardsList = new List<RequestReward>();
        public List<SkinReqStruct> SkinList = new List<SkinReqStruct>(), OutfitList = new List<SkinReqStruct>();
        public bool IsNocturnal = false;
        public bool SleepsAtBed = true;
        public bool SpecialAttackBasedCombat = false;
        public List<GuardianSpecialAttack> SpecialAttackList = new List<GuardianSpecialAttack>();
        public int CharacterPositionYDiscount = 0;
        public GuardianRoles Roles = GuardianRoles.None;
        public CombatTactic DefaultTactic = CombatTactic.Assist;
        
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
        public Point SittingPoint2x
        {
            set
            {
                SittingPoint = new Point(value.X * 2, value.Y * 2);
            }
        }
        public Point SleepingOffset2x
        {
            set
            {
                SleepingOffset = new Point(value.X * 2, value.Y * 2);
            }
        }
        public GuardianSize Size = GuardianSize.Medium;
        public int Age = 15;
        public double Birthday = 0;
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
        public int BackwardStanding = -1, BackwardRevive = -1;
        public Dictionary<int, int> BodyFrontFrameSwap = new Dictionary<int, int>(), RightArmFrontFrameSwap = new Dictionary<int,int>();
        public float HeavySwingFrameTime = 1f, WalkAnimationFrameTime = 0f;
        public float MaxWalkSpeedTime { get { return WalkAnimationFrameTime * WalkingFrames.Length; } }
        public List<int> DrawLeftArmInFrontOfHead = new List<int>();
        public bool SpecificBodyFrontFramePositions = false;
        public List<DialogueTopic> Topics = new List<DialogueTopic>();

        public string GetGroupID { get { return GroupID; } }
        /// <summary>
        /// Don't lie. :)
        /// </summary>
        protected ushort PopularityContestsWon = 0, ContestSecondPlace = 0, ContestThirdPlace = 0;
        public virtual string CallUnlockMessage { get { return "*It seems like I can call this guardian whenever I want.*"; } }
        public virtual string MountUnlockMessage { get { return "*It seems like I can mount this guardian whenever I want.*"; } }
        public virtual string ControlUnlockMessage { get { return "*It seems like I can control It's movement whenever I want.*"; } }
        public virtual string MoveInUnlockMessage { get { return "*This companion seems interessed into living on your world.*"; } }

        public virtual string LeavingWorldMessageGuardianSummoned { get { return " things were packed out of the world."; } }
        public virtual string LeavingWorldMessage { get { return " has moved out of the world."; } }

        public byte KnownLevel = 2, FriendsLevel = 5, BestFriendLevel = 12, BestFriendForeverLevel = 18,  BuddiesForLife = 25;
        public byte CallUnlockLevel = 0, LootingUnlockLevel = 3, MaySellYourLoot = 4, MountUnlockLevel = 5, StopMindingAFK = 7, MountDamageReductionLevel = 9, ControlUnlockLevel = 10, FriendshipBondUnlockLevel = 12, FallDamageReductionLevel = 15, MoveInLevel = 0;
        
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
            Michelle = 13,
            Wrath = 14,
            Alexander = 15,
            Fluffles = 16,
            Minerva = 17,
            Daphne = 18,
            Liebre = 19,
            Bapha = 20,
            Glenn = 21,
            CaptainStench = 22,
            Cinnamon = 23,
            Quentin = 24,
            Miguel = 25,
            Luna = 26,
            Fear = 27,
            Sadness = 28,
            Joy = 29,
            Green = 30,
            Cille = 31,
            Castella = 32;

        public struct DialogueTopic
        {
            public string TopicText;
            public Action TopicMethod;
            public Func<TerraGuardian, PlayerMod, bool> Requirement;

            public DialogueTopic(string TopicText, Action TopicMethod, Func<TerraGuardian, PlayerMod, bool> Requirement = null)
            {
                this.TopicText = TopicText;
                this.TopicMethod = TopicMethod;
                if (Requirement == null)
                    this.Requirement = delegate (TerraGuardian tg, PlayerMod pl) { return true; };
                else
                    this.Requirement = Requirement;
            }
        }

        /// <summary>
        /// Adds a topic you can talk about with your companion.
        /// Create a method that holds the dialogue of this topic, then direct It on TopicMethod.
        /// Make use of Dialogue methods to get the dialogues and other actions you can use during the chatting.
        /// </summary>
        /// <param name="TopicText">The message displayed when this topic is disponible.</param>
        /// <param name="TopicMethod">The script that holds the dialogue with this companion, use Dialogue methods for It.</param>
        public void AddTopic(string TopicText, Action TopicMethod, Func<TerraGuardian, PlayerMod, bool> TopicRequirement = null)
        {
            Topics.Add(new DialogueTopic(TopicText, TopicMethod, TopicRequirement));
        }

        public const byte SEASON_SUMMER = 0, SEASON_AUTUMN = 1, SEASON_WINTER = 2, SEASON_SPRING = 3;
        /// <summary>
        /// Aids when setting up the companion birthday.
        /// </summary>
        /// <param name="Season">There are 4 seasons, going from 0 to 3.</param>
        /// <param name="Day">There are 30 days, going from 1 to 30</param>
        public void SetBirthday(byte Season, byte Day)
        {
            if(Day > 0)
                Day--;
            Season %= 4;
            Day %= GuardianGlobalInfos.QuarterOfAYear;
            const int DaysInASeason = GuardianGlobalInfos.QuarterOfAYear;
            Birthday = DaysInASeason * Season + (double)Day;
        }

        /// <summary>
        /// Declares that this companion is actually a TerraGuardian, and nullifies TerrarianInfo data.
        /// </summary>
        public void SetTerraGuardian()
        {
            GroupID = TerraGuardianGroupID;
            TerrarianInfo = null;
        }

        /// <summary>
        /// Declares that this companion is actually a Terrarian, and creates TerrarianInfo data, plus sets up other 
        /// things related to the companion.
        /// </summary>
        public void SetTerrarian()
        {
            TerrarianInfo = new TerrarianCompanionInfos();
            GroupID = TerrarianGroupID;

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
            BedSleepingFrame = 0;
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

            //WalkAnimationFrameTime = (8f / 60) * 1.3f; //1f / 8;
        }

        /// <summary>
        /// Allows you to register a custom skin to the companion.
        /// Use ManageExtraDrawScript to load the textures of the skin.
        /// Use GuardianPostDrawScript to draw or change sprites for the skin on the companion.
        /// </summary>
        /// <param name="ID">The ID of the skin. Set It to a unique number different from 0, unless you want the companion to already be using It when acquired.</param>
        /// <param name="Name">The name of the skin. Will be displayed on the skin list.</param>
        /// <param name="requirement">Allows you to add a requirement to unlock this skin.</param>
        public void AddSkin(byte ID, string Name, SkinReqStruct.SkinRequirementDel requirement)
        {
            SkinReqStruct skin = new SkinReqStruct(ID, Name, requirement);
            SkinList.Add(skin);
        }

        /// <summary>
        /// Allows you to register a custom outfit to the companion.
        /// Use ManageExtraDrawScript to load the textures of the outfit.
        /// Use GuardianPostDrawScript to draw the outfit on the companion.
        /// </summary>
        /// <param name="ID">The ID of the outfit. Set It to a unique number different from 0, unless you want the companion to already be using It when acquired.</param>
        /// <param name="Name">The name of the outfit. Will be displayed on the outfit list.</param>
        /// <param name="requirement">Allows you to add a requirement to unlock this outfit.</param>
        public void AddOutfit(byte ID, string Name, SkinReqStruct.SkinRequirementDel requirement)
        {
            AddOutfit(ID, Name, requirement, false);
        }

        /// <summary>
        /// Allows you to register a custom outfit to the companion.
        /// Use ManageExtraDrawScript to load the textures of the outfit.
        /// Use GuardianPostDrawScript to draw the outfit on the companion.
        /// </summary>
        /// <param name="ID">The ID of the outfit. Set It to a unique number different from 0, unless you want the companion to already be using It when acquired.</param>
        /// <param name="Name">The name of the outfit. Will be displayed on the outfit list.</param>
        /// <param name="requirement">Allows you to add a requirement to unlock this outfit.</param>
        /// <param name="SkinUsesHead">Tells if the outfit actually uses the character head.</param>
        public void AddOutfit(byte ID, string Name, SkinReqStruct.SkinRequirementDel requirement, bool SkinUsesHead)
        {
            SkinReqStruct skin = new SkinReqStruct(ID, Name, requirement, SkinUsesHead);
            OutfitList.Add(skin);
        }

        public bool IsTerraGuardian { get { return GetGroup.RecognizeAsTerraGuardian; } }
        public bool IsTerrarian { get { return !GetGroup.CustomSprite; } }
        public bool IsCustomSpriteCharacter { get { return GetGroup.CustomSprite; } }

        /// <summary>
        /// Allows you to add custom rewards for this companion requests.
        /// </summary>
        /// <param name="ItemID">The id of the item given as reward.</param>
        /// <param name="Stack">How many of this item is given by default.</param>
        /// <param name="ItemScore">Score value necessary for this item to be in the loot pool. If the score is higher or equal to this value, the item will have the chance of dropping.</param>
        /// <param name="Chance">From 0 to 1 in decimal values. Chance of the item being picked if the score is higher or equal to this item score.</param>
        /// <param name="MaxExtraStack">Becareful with this, the extra stack deduces from the score based on the rng of extra stack got.</param>
        public void AddReward(int ItemID, int Stack, float Chance = 0.1f)
        {
            RequestReward rwd = new RequestReward();
            rwd.itemID = ItemID;
            rwd.Stack = Stack;
            rwd.AcquisitionChance = Chance;
            RewardsList.Add(rwd);
        }

        /// <summary>
        /// Returns wether there is a body front sprite for this animation.
        /// Returns -1 if there isn't, and returns some value if there is.
        /// </summary>
        /// <param name="Frame">The frame you want to check if has a body front sprite.</param>
        /// <returns></returns>
        public int GetBodyFrontSprite(int Frame)
        {
            if (BodyFrontFrameSwap.ContainsKey(Frame))
                return BodyFrontFrameSwap[Frame];
            return -1;
        }

        /// <summary>
        /// Gets the number of times the popularity contest were won (1st place).
        /// Don't lie :3.
        /// </summary>
        public int GetPopularityContestsWon()
        {
            return PopularityContestsWon;
        }

        /// <summary>
        /// Gets the number of times the companion won the popularity contest in 2nd place.
        /// Don't lie :3.
        /// </summary>
        public int GetPopularityContestsSecondPlace()
        {
            return ContestSecondPlace;
        }

        /// <summary>
        /// Gets the number of times the companion won the popularity contest in 3rd place.
        /// Don't lie :3.
        /// </summary>
        public int GetPopularityContestsThirdPlace()
        {
            return ContestThirdPlace;
        }

        /// <summary>
        /// Extremelly necessary if your companion has a shop to offer.
        /// </summary>
        /// <param name="GuardianID">Gets the ID of the guardian, use this when creating a shop.</param>
        /// <param name="GuardianModID">Gets the ModID of the guardian, use this when creating a shop.</param>
        public virtual void SetupShop(int GuardianID, string GuardianModID)
        {

        }

        /// <summary>
        /// Only setup this on SetupShop() script.
        /// Use the values acquired from SetupShop() variable to fill the values It needs.
        /// </summary>
        /// <returns></returns>
        protected GuardianShopHandler.GuardianShop CreateShop(int GuardianID, string GuardianModID)
        {
            return GuardianShopHandler.CreateShop(GuardianID, GuardianModID);
        }

        /// <summary>
        /// Only use this script on SetupShop(). Use the GuardianShop variable returned from CreateShop() as the shop value.
        /// </summary>
        /// <param name="shop">Use the value from CreateShop() to get this companion shop.</param>
        /// <param name="ItemID">The id of the item to be sold.</param>
        /// <param name="Price">The price at which the item will be sold. By default, It will sell by the item price.</param>
        /// <param name="Name">The name of the item to be sold. By default, It will sell the item by It's normal name.</param>
        /// <param name="FixedSellStack">Allows you to change how many of this item will be sold each time It's disponible for sale. By default, there is no limit.</param>
        /// <returns></returns>
        protected GuardianShopHandler.GuardianShopItem Shop_AddItem(GuardianShopHandler.GuardianShop shop, int ItemID, int Price = -1, string Name = "", int FixedSellStack = 1)
        {
            return shop.AddNewItem(ItemID, Price, Name, FixedSellStack);
        }
        
        /// <summary>
        /// This is updated everytime the companion status is updated.
        /// Use this to give extra attributes to the companion, or change their status.
        /// </summary>
        /// <param name="g">The companion whose status is being changed. Change directly the stats from the companion Itself.</param>
        public virtual void Attributes(TerraGuardian g)
        {

        }

        /// <summary>
        /// Changes the flag wether the left or right arm is being used.
        /// If you set the flag to true, the scripts changing their animation will not play.
        /// </summary>
        /// <param name="guardian"></param>
        /// <param name="UsingLeftArm"></param>
        /// <param name="UsingRightArm"></param>
        public virtual void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {

        }

        /// <summary>
        /// Changes if the left arm or the right arm should be drawn in front of the player.
        /// </summary>
        public virtual void ForceDrawInFrontOfPlayer(TerraGuardian guardian, ref bool LeftArmInFront, ref bool RightArmInFront)
        {

        }

        /// <summary>
        /// Is called during the companion extra updating.
        /// </summary>
        /// <param name="guardian">The guardian who called this script.</param>
        public virtual void GuardianUpdateScript(TerraGuardian guardian)
        {

        }

        /// <summary>
        /// Called when the companion status is resetted, before changing the status.
        /// Attributes is called sometime after this.
        /// </summary>
        /// <param name="guardian">The companion reference.</param>
        public virtual void GuardianResetStatus(TerraGuardian guardian)
        {

        }

        /// <summary>
        /// Is called before the sprites list is populated. Use this if you only wants to draw something behind the companion sprites.
        /// Use TerraGuardian.DrawFront list to modify what is going to be drawn in front of the player, when the player is mounted.
        /// Use TerraGuardian.DrawBehind list to modify what is going to be drawn behind of the player, when the player is mounted.
        /// </summary>
        /// <param name="guardian">The guardian owner of this.</param>
        /// <param name="DrawPosition">The position the sprites are drawn.</param>
        /// <param name="color">The color in the position of the player, taking in consideration light and darkness.</param>
        /// <param name="armorColor">The color the armors of the companion will be drawn.</param>
        /// <param name="Rotation">The rotation of this sprite.</param>
        /// <param name="Origin">The origin of the sprite. Is used to change the orientation at which the sprite is drawn.</param>
        /// <param name="Scale">The scale of the sprite.</param>
        /// <param name="seffect">Wether It's flipped or not.</param>
        public virtual void GuardianPreDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {

        }

        /// <summary>
        /// Is called after all the sprites and their drawing orders are setup on the companion.
        /// Use TerraGuardian.DrawFront list to modify what is going to be drawn in front of the player, when the player is mounted.
        /// Use TerraGuardian.DrawBehind list to modify what is going to be drawn behind of the player, when the player is mounted.
        /// Use the other arguments in this script to aid you into adding more sprites to your companion, if necessary.
        /// </summary>
        /// <param name="guardian">The guardian owner of this.</param>
        /// <param name="DrawPosition">The position the sprites are drawn.</param>
        /// <param name="color">The color in the position of the player, taking in consideration light and darkness.</param>
        /// <param name="armorColor">The color the armors of the companion will be drawn.</param>
        /// <param name="Rotation">The rotation of this sprite.</param>
        /// <param name="Origin">The origin of the sprite. Is used to change the orientation at which the sprite is drawn.</param>
        /// <param name="Scale">The scale of the sprite.</param>
        /// <param name="seffect">Wether It's flipped or not.</param>
        public virtual void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {

        }

        /// <summary>
        /// Allows you to modify the head sprites to be drawn of the companion.
        /// This is used for when the mod needs to draw only the companion head, for hud element stuff.
        /// </summary>
        /// <param name="guardian">The guardian owner of this.</param>
        /// <param name="DrawPosition">The default position the sprites will be drawn.</param>
        /// <param name="color">The color change at the player position, taking in consideration shadows too.</param>
        /// <param name="Scale">The sprite scale.</param>
        /// <param name="seffect">The direction the sprite is facing.</param>
        /// <param name="gdd">Modify this list to change how the companion head will be drawn. You can even alter elements. Check textureType variable inside the list contents to check what It will draw.</param>
        public virtual void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect, Vector2 Origin, ref List<GuardianDrawData> gdds)
        {

        }

        /// <summary>
        /// Use this to add extra textures to the companion sprites. This is used in case the companion you are making will need 
        /// extra sprites to draw. Use sprites.AddExtraTextures(ID,File); to load the sprites.
        /// Be sure to save the ID as a constant string, you will need It to call the texture.
        /// </summary>
        /// <param name="sprites">The object that holds the companion sprites.</param>
        public virtual void ManageExtraDrawScript(GuardianSprites sprites)
        {

        }

        /// <summary>
        /// Useful if you want to program extra behaviors for this companion.
        /// Is played when their behavior scripts is running.
        /// </summary>
        /// <param name="guardian">The guardian that is going to execute the custom behavior.</param>
        public virtual void GuardianBehaviorModScript(TerraGuardian guardian)
        {

        }

        /// <summary>
        /// Allows you to modify the horizontal movement speed of the companion.
        /// </summary>
        /// <param name="guardian">This is the companion receiving the speed changes.</param>
        /// <param name="MaxSpeed">The maximum speed the companion can reach when running.</param>
        /// <param name="Acceleration">The acceleration the companion has.</param>
        /// <param name="SlowDown">The deceleration the companion receives when not moving.</param>
        /// <param name="DashSpeed">The speed when the companion is running (by using boots accessory).</param>
        public virtual void GuardianHorizontalMovementScript(TerraGuardian guardian, ref float MaxSpeed, ref float Acceleration, ref float SlowDown, ref float DashSpeed)
        {

        }

        /// <summary>
        /// This is called when the companion is attacked by the player.
        /// </summary>
        /// <param name="guardian">The guardian attacked by the player.</param>
        /// <param name="Damage">The damage received from the attack.</param>
        /// <param name="Critical">If the damage is critical.</param>
        /// <param name="Attacker">The player who attacked this companion.</param>
        /// <returns>Return true if should inflict damage.</returns>
        public virtual bool GuardianWhenAttackedPlayer(TerraGuardian guardian, int Damage, bool Critical, Player Attacker)
        {
            return true;
        }

        /// <summary>
        /// This is called when the companion is attacked by a npc.
        /// </summary>
        /// <param name="guardian">The companion hit by the attack.</param>
        /// <param name="Damage">The damage received.</param>
        /// <param name="Attacker">The npc that attacked.</param>
        /// <returns>Return true if the companion should receive damage.</returns>
        public virtual bool GuardianWhenAttackedNPC(TerraGuardian guardian, int Damage, NPC Attacker)
        {
            return true;
        }

        /// <summary>
        /// This plays when the companion is attacked by another companion.
        /// </summary>
        /// <param name="guardian">The guardian that was hit by the attack.</param>
        /// <param name="Damage">The damage received.</param>
        /// <param name="Critical">If the damage is critical or not.</param>
        /// <param name="Attacker">The guardian that attacked this guardian.</param>
        /// <returns>Return true if should inflict damage.</returns>
        public virtual bool GuardianWhenAttackedGuardian(TerraGuardian guardian, int Damage, bool Critical, TerraGuardian Attacker)
        {
            return true;
        }

        /// <summary>
        /// Is called when the compannion is hit by a projectile.
        /// </summary>
        /// <param name="guardian">The guardian that called this trigger.</param>
        /// <param name="Damage">The damage the projectile inflicted on the companion.</param>
        /// <param name="Critical">If It's a critical hit or not that hit the companion. Becareful! The Damage value is post critical damage bonus.</param>
        /// <param name="Proj">The projectile that hit the companion.</param>
        /// <returns>Return true if the companion should be hurt by this projectile.</returns>
        public virtual bool GuardianWhenAttackedProjectile(TerraGuardian guardian, int Damage, bool Critical, Projectile Proj)
        {
            return true;
        }

        /// <summary>
        /// Allows you to change how the companion will behave when a trigger activates.
        /// Check TriggerHandler.cs script to see what each value is for, depending on the trigger.
        /// </summary>
        /// <param name="guardian">The guardian that received the trigger.</param>
        /// <param name="trigger">The type of the trigger called.</param>
        /// <param name="Value"></param>
        /// <param name="Value2"></param>
        /// <param name="Value3"></param>
        /// <param name="Value4"></param>
        /// <param name="Value5"></param>
        /// <returns>Return false if you don't want the companion to try executing a default behavior for this trigger.</returns>
        public virtual bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, Trigger.TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
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
            return "*[name] wants you to [objective].*";
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
        /// <param name="IsPlayer">Tells if the one revived is a player</param>
        /// <param name="RevivePlayer">Only make use of this, if IsPlayer is true. Use this to get info about the player being revived.</param>
        /// <param name="ReviveGuardian">Only make use of this, if IsPlayer is false. Use this to get info about the guardian being revived.</param>
        /// <returns></returns>
        public virtual string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            return "*[name] says to not worry, you'll be well soon.*";
        }

        /// <summary>
        /// This method allows you to change the animation frame the companion will play, depending on the conditions you make.
        /// Depending on how the animation of your companion works, check which Body Part the script has been called for, before changing Frame.
        /// If for example, you want to change the arms animation, check if It's either the Left or Right arm body part, before changing the frame.
        /// </summary>
        /// <param name="guardian">The guardian whose animation is being changed. Use this for gathering infos.</param>
        /// <param name="BodyPartID">It's related to the body part that is currently calling this. 0 = body, 1 = left arm, 2 = right arm</param>
        /// <param name="Frame">The animation frame for the body part above. Change It to change the current animation of the companion.</param>
        public virtual void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {

        }

        /// <summary>
        /// Adds extra dialogue options to your companion. Use this for adding extra functions for your companion.
        /// </summary>
        /// <param name="guardian">The guardian whose dialogue belongs.</param>
        /// <returns>Returns the list of dialogue options your companion will get.</returns>
        public virtual List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            return new List<DialogueOption>();
        }

        /// <summary>
        /// It's used to change what is the request the mod will give, when one is ready.
        /// Setting ForceMissionID to -1, will make the mod decide which request will give.
        /// Return false to make the companion not give a request.
        /// </summary>
        /// <param name="Guardian">The guardian that will give the mission. You may use It to get infos, like companion friendship level and exp.</param>
        /// <param name="ForcedMissionID">Setting to a value other above -1, will force the companion to give It's special request, regardless of the request requirement. You may use this to give hidden requests to the companion, or requests specific to friendship leveling.</param>
        /// <param name="IsTalkRequest">If set to true, will disregard the above, and will instead spawn a Talk request.</param>
        /// <returns>Return false if you don't want the companion to give request.</returns>
        public virtual bool AlterRequestGiven(GuardianData Guardian, out int ForcedMissionID, out bool IsTalkRequest)
        {
            IsTalkRequest = false;
            ForcedMissionID = -1;
            return true;
        }

        public void CalculateHealthToGive(int ResultMHP, float LCBonusPercentage = 1f, float LFBonusPercentage = 1f)
        {
            if(LCBonusPercentage + LFBonusPercentage != 1f)
            {
                float Discount = 1f / (LCBonusPercentage + LFBonusPercentage);
                LCBonusPercentage *= Discount;
                LFBonusPercentage *= Discount;
            }
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
            GuardianBase gb = GuardianList[modid].GetGuardian(ID);
            return gb;
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
            Point p = GetBetweenHandsPosition(Frame);
            X = p.X;
            Y = p.Y;
        }

        public Vector2 GetBetweenHandsPositionVector(int Frame)
        {
            return GetBetweenHandsPosition(Frame).ToVector2();
        }

        public Point GetBetweenHandsPosition(int Frame)
        {
            if (DontUseRightHand)
                return LeftHandPoints.GetPositionFromFramePoint(Frame);
            if (DontUseLeftHand)
                return RightHandPoints.GetPositionFromFramePoint(Frame);
            Point p = new Point(), 
                l = LeftHandPoints.GetPositionFromFramePoint(Frame), 
                r = RightHandPoints.GetPositionFromFramePoint(Frame);
            if (LeftHandPoints.HasSpecificCoordinate(Frame) && RightHandPoints.HasSpecificCoordinate(Frame))
            {
                p.X = l.X + (int)((r.X - l.X) * 0.5f);
                p.Y = l.Y + (int)((r.Y - l.Y) * 0.5f);
            }else if (LeftHandPoints.HasSpecificCoordinate(Frame))
            {
                p.X = l.X;
                p.Y = l.Y;
            }
            else
            {
                p.X = r.X;
                p.Y = r.Y;
            }
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
                    case Rococo:
                        gb = new RococoBase();
                        break;
                    case Blue:
                        gb = new BlueBase();
                        break;
                    case Sardine:
                        gb = new SardineBase();
                        break;
                    case Zacks:
                        gb = new ZacksBase();
                        break;
                    case Nemesis:
                        gb = new NemesisBase();
                        break;
                    case Alex:
                        gb = new AlexBase();
                        break;
                    case Brutus:
                        gb = new BrutusBase();
                        break;
                    case Bree:
                        gb = new BreeBase();
                        break;
                    case Mabel:
                        gb = new MabelBase();
                        break;
                    case Domino:
                        gb = new DominoBase();
                        break;
                    case Leopold:
                        gb = new LeopoldBase();
                        break;
                    case Vladimir:
                        gb = new VladimirBase();
                        break;
                    case Malisha:
                        gb = new MalishaBase();
                        break;
                    case Michelle:
                        gb = new MichelleBase();
                        break;
                    case Wrath:
                        gb = new WrathBase();
                        break;
                    case Alexander:
                        gb = new AlexanderBase();
                        break;
                    case Fluffles:
                        gb = new FlufflesBase();
                        break;
                    case Minerva:
                        gb = new MinervaBase();
                        break;
                    case Daphne:
                        gb = new DaphneBase();
                        break;
                    case Liebre:
                        gb = new LiebreBase();
                        break;
                    case Bapha:
                        gb = new BaphaBase();
                        break;
                    case Glenn:
                        gb = new GlennBase();
                        break;
                    case CaptainStench:
                        gb = new CaptainStenchBase();
                        break;
                    case Cinnamon:
                        gb = new CinnamonBase();
                        break;
                    case Quentin:
                        gb = new QuentinBase();
                        break;
                    case Miguel:
                        gb = new MiguelBase();
                        break;
                    case Luna:
                        gb = new LunaBase();
                        break;
                    case Fear:
                        gb = new FearBase();
                        break;
                    case Sadness:
                        gb = new SadnessBase();
                        break;
                    case Joy:
                        gb = new JoyBase();
                        break;
                    case Green:
                        gb = new GreenBase();
                        break;
                    case Cille:
                        gb = new CilleBase();
                        break;
                    case Castella:
                        gb = new CastellaBase();
                        break;
                }
            }
            if (gb == null)
            {
                gb = new GuardianBase();
                InvalidGuardian = true;
            }
            gb.InvalidGuardian = InvalidGuardian;
            if(gb.SpritesDirectory == "")
            {
                if (!gb.InvalidGuardian)
                {
                    gb.SpritesDirectory = "Companions/";
                    if (!gb.IsTerrarian)
                    {
                        gb.SpritesDirectory = DefaultCreaturesDirectory;
                    }
                    else
                    {
                        gb.SpritesDirectory = DefaultTerrariansDirectory;
                    }
                }
            }
            if (gb.SittingPoint.X == 0 && gb.SittingPoint.Y == 0)
            {
                gb.SittingPoint.X = gb.SpriteWidth / 2;
                gb.SittingPoint.Y = gb.SpriteHeight;
            }
            if (!InvalidGuardian)
            {
                gb.sprites = new GuardianSprites(gb, mod);
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

        public virtual bool BeforeUsingItem(TerraGuardian tg, ref int SelectedItem)
        {
            return true;
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
            HeavySwingFrames = invalidGuardianPoints.HeavySwingFrames;
            HeadVanityPosition = invalidGuardianPoints.HeadVanityPosition;
            LeftHandPoints = invalidGuardianPoints.HandPoints;
            RightHandPoints = invalidGuardianPoints.HandPoints;
        }

        public virtual bool RoomNeeds()
        {
            return WorldMod.BasicRoomNeeds();
        }

        public class TerrarianCompanionInfos
        {
            public Color HairColor, SkinColor, EyeColor, ShirtColor, UnderShirtColor, PantsColor, ShoeColor;
            public int HairStyle, SkinVariant;
            public int DefaultHelmet = 0, DefaultArmor = 0, DefaultLeggings = 0;

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

        public virtual void ModifyVelocity(TerraGuardian tg, ref Vector2 Velocity)
        {

        }

        public virtual string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            Weight = 1f;
            return "*They are pleased to meet " + WhoJoined.Name + ".*";
        }

        public virtual string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            Weight = 1f;
            return "*They greeted " + WhoJoined.Name + ".*";
        }

        public virtual string CompanionLeavesGroupMessage(TerraGuardian WhoReacts, GuardianData WhoLeft, out float Weight)
        {
            Weight = 1f;
            return "*They sent farewell to " + WhoLeft.Name + ".*";
        }

        public virtual string GetSpecialMessage(string MessageID)
        {
            return "";
        }

        public GuardianSpecialAttack AddNewSubAttack(GuardianSpecialAttack Special)
        {
            SpecialAttackList.Add(Special);
            return SpecialAttackList[SpecialAttackList.Count - 1];
        }

        public virtual ushort GuardianSubAttackBehaviorAI(TerraGuardian Owner, CombatTactic tactic, Vector2 TargetPosition, Vector2 TargetVelocity, int TargetWidth, int TargetHeight,
            ref bool Approach, ref bool Retreat, ref bool Jump, ref bool Couch, out bool DefaultBehavior)
        {
            DefaultBehavior = true;
            return ushort.MaxValue;
        }

        public GuardianDrawData GetTextureDrawData(GuardianDrawData.TextureType textureType)
        {
            for (int i = 0; i < TerraGuardian.DrawBehind.Count; i++)
            {
                if (TerraGuardian.DrawBehind[i].textureType == textureType)
                {
                    return TerraGuardian.DrawBehind[i];
                }
            }
            for (int i = 0; i < TerraGuardian.DrawFront.Count; i++)
            {
                if (TerraGuardian.DrawFront[i].textureType == textureType)
                {
                    return TerraGuardian.DrawFront[i];
                }
            }
            return null;
        }

        public void RemoveTextureDrawData(GuardianDrawData.TextureType textureType)
        {
            for (int i = 0; i < TerraGuardian.DrawBehind.Count; i++)
            {
                if (TerraGuardian.DrawBehind[i].textureType == textureType)
                {
                    TerraGuardian.DrawBehind.RemoveAt(i);
                }
            }
            for (int i = 0; i < TerraGuardian.DrawFront.Count; i++)
            {
                if (TerraGuardian.DrawFront[i].textureType == textureType)
                {
                    TerraGuardian.DrawFront.RemoveAt(i);
                }
            }
        }

        public void InjectTextureBefore(GuardianDrawData.TextureType textureType, GuardianDrawData DrawDatas)
        {
            InjectTexturesAt(textureType, new GuardianDrawData[] { DrawDatas }, null);
        }

        public void InjectTextureAfter(GuardianDrawData.TextureType textureType, GuardianDrawData DrawDatas)
        {
            InjectTexturesAt(textureType, null, new GuardianDrawData[] { DrawDatas });
        }

        public void InjectTexturesBefore(GuardianDrawData.TextureType textureType, GuardianDrawData[] DrawDatas)
        {
            InjectTexturesAt(textureType, DrawDatas, null);
        }

        public void InjectTexturesAfter(GuardianDrawData.TextureType textureType, GuardianDrawData[] DrawDatas)
        {
            InjectTexturesAt(textureType, null, DrawDatas);
        }

        public void InjectTexturesAt(GuardianDrawData.TextureType textureType, GuardianDrawData[] PreDraw, GuardianDrawData[] PostDraw)
        {
            for(int SpriteList = 0; SpriteList < 2; SpriteList++)
            {
                List<GuardianDrawData> CurrentDrawData;
                switch (SpriteList)
                {
                    default:
                        CurrentDrawData = TerraGuardian.DrawBehind;
                        break;
                    case 1:
                        CurrentDrawData = TerraGuardian.DrawFront;
                        break;
                }
                int EarliestLayerShowUp = -1, LatestLayerShowUp = -1;
                for (int i = 0; i < CurrentDrawData.Count; i++)
                {
                    if (CurrentDrawData[i].textureType == textureType)
                    {
                        if (EarliestLayerShowUp == -1)
                            EarliestLayerShowUp = i;
                        LatestLayerShowUp = i;
                        //break;
                    }
                }
                if (PostDraw != null && LatestLayerShowUp > -1)
                {
                    if (LatestLayerShowUp + 1 >= CurrentDrawData.Count)
                        CurrentDrawData.AddRange(PostDraw);
                    else
                        CurrentDrawData.InsertRange(LatestLayerShowUp + 1, PostDraw);
                }
                if (PreDraw != null && EarliestLayerShowUp > -1) CurrentDrawData.InsertRange(EarliestLayerShowUp, PreDraw);
            }
            /*for (int i = 0; i < TerraGuardian.DrawBehind.Count; i++)
            {
                if (TerraGuardian.DrawBehind[i].textureType == textureType)
                {
                    if (PostDraw != null) TerraGuardian.DrawBehind.InsertRange(i + 1, PostDraw);
                    if (PreDraw != null) TerraGuardian.DrawBehind.InsertRange(i, PreDraw);
                    break;
                }
            }
            for (int i = 0; i < TerraGuardian.DrawFront.Count; i++)
            {
                if (TerraGuardian.DrawFront[i].textureType == textureType)
                {
                    if (PostDraw != null) TerraGuardian.DrawFront.InsertRange(i + 1, PostDraw);
                    if (PreDraw != null) TerraGuardian.DrawFront.InsertRange(i, PreDraw);
                    break;
                }
            }*/
        }

        public void ReplaceTexture(GuardianDrawData.TextureType textureType, Microsoft.Xna.Framework.Graphics.Texture2D texture, Color? replaceColor = null)
        {
            for(byte sprite = 0; sprite < 2; sprite++)
            {
                List<GuardianDrawData> gddList = null;
                if (sprite == 0)
                    gddList = TerraGuardian.DrawBehind;
                else
                    gddList = TerraGuardian.DrawFront;
                foreach(GuardianDrawData gdd in gddList)
                {
                    if(gdd.textureType == textureType)
                    {
                        gdd.Texture = texture;
                        if (replaceColor.HasValue)
                            gdd.color = replaceColor.Value;
                    }
                }
            }
        }

        public void ReplaceDrawData(GuardianDrawData.TextureType textureType, GuardianDrawData gdd)
        {
            for (byte sprite = 0; sprite < 2; sprite++)
            {
                List<GuardianDrawData> gddList = null;
                if (sprite == 0)
                    gddList = TerraGuardian.DrawBehind;
                else
                    gddList = TerraGuardian.DrawFront;
                for (int i = 0; i < gddList.Count; i++)
                {
                    if (gddList[i].textureType == textureType)
                        gddList[i] = gdd;
                }
            }
        }

        public static void ClearGuardianContainers()
        {
            foreach(string s in GuardianList.Keys)
            {
                GuardianList[s].ClearContainer();
            }
            GuardianList.Clear();
        }

        public class MessageIDs
        {
            public const string LeopoldMessage1 = "Mes.LeopoldAnswer1";
            public const string LeopoldMessage2 = "Mes.LeopoldAnswer2";
            public const string LeopoldMessage3 = "Mes.LeopoldAnswer3";
            public const string RescueMessage = "Mes.Rescue";
            public const string StoreOpenMessage = "Mes.Store.BrowseStore",
                StoreSaleHappeningMessage = "Mes.Store.SaleHappening",
                StoreBuyMessage = "Mes.Store.BuyMessage",
                StoreFullInventoryMessage = "Mes.Store.FullInvMessage",
                StoreNoCoinsMessage = "Mes.Store.NotEnoughCoins";
            public const string BuddySelected = "Mes.BuddyPicked";
            public const string GuardianWokeUpByPlayerMessage = "Mes.WakeUpMessage",
                GuardianWokeUpByPlayerRequestActiveMessage = "Mes.WakeUpMessageRequestActive";
            public const string AfterAskingCompanionToJoinYourGroupSuccess = "Mes.Follower.JoinResponse",
                AfterAskingCompanionToJoinYourGroupFullParty = "Mes.Follower.FullParty", 
                AfterAskingCompanionToJoinYourGroupFail = "Mes.Follower.JoinResponseFail";
            public const string AfterAskingCompanionToLeaveYourGroupAskIfYoureSure = "Mes.Follower.LeavePartyAskIfSure",
                AfterAskingCompanionToLeaveYourGroupSuccessAnswer = "Mes.Follower.LeavePartyYesAnswer",
                AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace = "Mes.Follower.LeavePartyYesAnswerDangerous",
                AfterAskingCompanionToLeaveYourGroupNoAnswer = "Mes.Follower.LeavePartyNoAnswer";
            public const string RequestAccepted = "Mes.RequestAccept",
                RequestCantAcceptTooManyRequests = "Mes.TooManyRequests",
                RequestRejected = "Mes.RequestRejected",
                RequestPostpone = "Mes.RequestPostpone",
                RequestFailed = "Mes.RequestFailed",
                RequestAsksIfCompleted = "Mes.HasRequestBeenCompleted",
                RequestRemindObjective = "Mes.RemindObjective";
            public const string RestAskForHowLong = "Mes.RestTimeQuestion",
                RestNotPossible = "Mes.RestNotPossible",
                RestWhenGoingSleep = "Mes.RestGoodNight";
            /// <summary>
            /// This message contains the special [shop] keyword, which gives the name of the town npc the companion wants your character to approach.
            /// </summary>
            public const string AskPlayerToGetCloserToShopNpc = "Mes.AskToGetCloserToShopNpc",
                AskPlayerToWaitAMomentWhileCompanionIsShopping = "Mes.AskPlayerToWaitWhileShopping";
            public const string GenericYes = "Mes.Yes",
                GenericNo = "Mes.No",
                GenericThankYou = "Mes.ThankYou";
            public const string ChatAboutSomething = "Mes.ChatAboutSomething",
                NevermindTheChatting = "Mes.NevermindChatting";
            public const string CancelRequestAskIfSure = "Mes.AskIfSureOnCancelRequest",
                CancelRequestYesAnswered = "Mes.WhenYesAnswerOnRequestCancel",
                CancelRequestNoAnswered = "Mes.WhenNoAnswerOnRequestCancel";
            public const string AfterAskingIfCompanionCanVisitNextDayYesAnswer = "Mes.VisitAccept",
                AfterAskingIfCompanionCanVisitNextDayNoAnswer = "Mes.VisitReject";
            public const string AlexanderSleuthingStart = "Mes.Alexander.SleuthStart",
                AlexanderSleuthingFail = "Mes.Alexander.SleuthFail",
                AlexanderSleuthingProgress = "Mes.Alexander.SleuthProgress1",
                AlexanderSleuthingProgressNearlyDone = "Mes.Alexander.SleuthProgress2",
                AlexanderSleuthingProgressFinished = "Mes.Alexander.SleuthProgressFinished";
            public const string ReviveByOthersHelp = "Mes.Revive.HelpedByOthers",
                RevivedByRecovery = "Mes.Revive.Alone";
            public const string AcquiredPoisonedDebuff = "Mes.Debuff.Poisoned",
                AcquiredBurningDebuff = "Mes.Debuff.Burning",
                AcquiredDarknessDebuff = "Mes.Debuff.Darkness",
                AcquiredConfusedDebuff = "Mes.Debuff.Confused",
                AcquiredCursedDebuff = "Mes.Debuff.Cursed",
                AcquiredSlowDebuff = "Mes.Debuff.Slow",
                AcquiredWeakDebuff = "Mes.Debuff.Weak",
                AcquiredBrokenArmorDebuff = "Mes.Debuff.BrokenArmor",
                AcquiredHorrifiedDebuff = "Mes.Debuff.Horrified",
                AcquiredIchorDebuff = "Mes.Debuff.Ichor",
                AcquiredChilledDebuff = "Mes.Debuff.Chilled",
                AcquiredWebbedDebuff = "Mes.Debuff.Webbed",
                AcquiredFeralBiteDebuff = "Mes.Debuff.FeralBite";
            public const string AcquiredDefenseBuff = "Mes.Buff.Endurance",
                AcquiredWellFedBuff = "Mes.Buff.WellFed",
                AcquiredDamageBuff = "Mes.Buff.Wrath",
                AcquiredSpeedBuff = "Mes.Buff.Swiftness",
                AcquiredHealthIncreaseBuff = "Mes.Buff.Lifeforce",
                AcquiredCriticalBuff = "Mes.Buff.Rage",
                AcquiredMeleeWeaponBuff = "Mes.Buff.PoisonFlask",
                AcquiredHoneyBuff = "Mes.Buff.Honey";
            public const string AcquiredTipsyDebuff = "Mes.Buff.Drunk";
            public const string FoundLifeCrystalTile = "Mes.Tile.LC",
                FoundPressurePlateTile = "Mes.Tile.PressurePlate",
                FoundMineTile = "Mes.Tile.Mine",
                FoundDetonatorTile = "Mes.Tile.Detonator",
                FoundPlanteraTile = "Mes.Tile.Plantera",
                FoundTreasureTile = "Mes.Tile.Treasure",
                FoundGemTile = "Mes.Tile.Gem",
                FoundRareOreTile = "Mes.Tile.RareOre",
                FoundVeryRareOreTile = "Mes.Tile.VeryRareOre",
                FoundMinecartRailTile = "Mes.Tile.Minecart";
            public const string TeleportHomeMessage = "Mes.Teleport.Home",
                SomeoneJoinsTeamMessage = "Mes.Team.SomeoneJoins",
                PlayerMeetsSomeoneNewMessage = "Mes.Player.MetSomeoneNew",
                CompanionInvokesAMinion = "Mes.Item.InvokeMinion",
                WhenOldOneArmyStarts = "Mes.Event.DD2Start",
                LeaderDiesMessage = "Mes.Party.LeaderDies",
                LeaderFallsMessage = "Mes.Party.LeaderFalls",
                AllyFallsMessage = "Mes.Party.AllyFalls",
                SpotsRareTreasure = "Mes.Item.RareLoot",
                LeavingToSellLoot = "Mes.Action.SellLoot",
                PlayerAtDangerousHealthLevel = "Mes.Party.AllyPlayerDangerousHealth",
                CompanionHealthAtDangerousLevel = "Mes.Party.MyHealthIsLow",
                RunningOutOfPotions = "Mes.Item.RunningOutOfPotions",
                UsesLastPotion = "Mes.Item.RanOutOfPotions",
                SpottedABoss = "Mes.NPC.BossSpotted",
                DefeatedABoss = "Mes.NPC.BossDefeated",
                InvasionBegins = "Mes.Event.InvasionBegins",
                RepelledInvasion = "Mes.Event.InvasionRepelled",
                EventBegins = "Mes.Event.EventBegins",
                EventEnds = "Mes.Event.EventEnds";
            public const string VladimirRecruitPlayerGetsHugged = "Mes.Recruit.VladmirHug";
            public const string FeatMentionPlayer = "Mes.Feat.MentionPlayer",
                FeatMentionBossDefeat = "Mes.Feat.BossKill",
                FeatFoundSomethingGood = "Mes.Feat.FoundGoodItem",
                FeatEventFinished = "Mes.Feat.EventFinished",
                FeatMetSomeoneNew = "Mes.Feat.MetSomeone",
                FeatPlayerDied = "Mes.Feat.PlayerDied",
                FeatOpenTemple = "Mes.Feat.OpenedTemple",
                FeatCoinPortal = "Mes.Feat.CoinPortal",
                FeatPlayerMetMe = "Mes.Feat.PlayerMetMe",
                FeatCompletedAnglerQuests = "Mes.Feat.AnglerQuestsCompleted",
                FeatKilledMoonLord = "Mes.Feat.KilledMoonLord",
                FeatStartedHardMode = "Mes.Feat.StartedHardMode",
                FeatMentionSomeonePickedAsBuddy = "Mes.Feat.PickedAsBuddy",
                FeatSpeakerPlayerPickedMeAsBuddy = "Mes.Feat.SpeakerPickedMeAsBuddy",
                FeatMentionSomeoneMovingIntoAWorld = "Mes.Feat.SomeoneMovedToAWorld";
            public const string PopContestMessage = "Mes.Contest",
                PopContestIntroduction = "Mes.Contest.Intro",
                PopContestLinkOpen = "Mes.Contest.LinkOpen",
                PopContestOnReturnToOtherTopics = "Mes.Contest.Return",
                PopContestResultMessage = "Mes.Contest.ResultMes",
                PopContestResultLinkClickMessage = "Mes.Contest.ResultLinkClick",
                PopContestResultNevermindMessage = "Mes.Contest.ResultNevermind";
            public const string RescueComingMessage = "Mes.Rescue.TellTheyreComing",
                RescueGotMessage = "Mes.Rescue.TargetAcquired";
            public const string DeliveryItemMissing = "Mes.Delivery.MissingItem",
                DeliveryInventoryFull = "Mes.Delivery.InventoryFull",
                DeliveryGiveItem = "Mes.Delivery.GiveItem";
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
            Large,
            ExtraLarge
        }

        [Flags]
        public enum GuardianRoles
        {
            None,
            PopularityContestHost
        }
    }

    public enum Genders : byte
    {
        Male,
        Female,
        Genderless
    }
}
