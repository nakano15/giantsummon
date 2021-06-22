using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Creatures
{
    public class DaphneBase : GuardianBase
    {
        public const string HaloTextureID = "halotexture";

        public DaphneBase()
        {
            Name = "Daphne";
            Description = "";
            Size = GuardianSize.Large;
            Width = 35 * 2;
            Height = 33 * 2;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 53;
            SetBirthday(SEASON_SUMMER, 17);
            Male = false;
            InitialMHP = 175; //1125
            LifeCrystalHPBonus = 30;
            LifeFruitHPBonus = 25;
            Accuracy = 0.36f;
            Mass = 0.7f;
            MaxSpeed = 5.65f;
            Acceleration = 0.33f;
            SlowDown = 0.83f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.71f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = false;
            DontUseRightHand = true;
            SetTerraGuardian();
            GroupID = GiantDogGuardianGroupID;
            //HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            //DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);
            CallUnlockLevel = 0;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 15;
            ChairSittingFrame = 14;
            SittingItemUseFrames = new int[] { 16, 17, 18 };
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 10, 11, 15 });
            ThroneSittingFrame = 14;
            BedSleepingFrame = 19;
            SleepingOffset.X = 16;
            ReviveFrame = 21;
            DownedFrame = 20;

            BackwardStanding = 22;
            BackwardRevive = 23;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 0);

            SittingPoint2x = new Microsoft.Xna.Framework.Point(20, 40);

            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(28, 24);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 39, 31);
            LeftHandPoints.AddFramePoint2x(11, 42, 32);
            LeftHandPoints.AddFramePoint2x(12, 43, 38);
            LeftHandPoints.AddFramePoint2x(13, 41, 42);

            LeftHandPoints.AddFramePoint2x(15, 25, 33);
            LeftHandPoints.AddFramePoint2x(16, 30, 18);
            LeftHandPoints.AddFramePoint2x(17, 32, 25);
            LeftHandPoints.AddFramePoint2x(18, 29, 30);

            LeftHandPoints.AddFramePoint2x(21, 40, 40);

            //Right Arm

            //Head Equipment Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(38, 21);
            HeadVanityPosition.AddFramePoint2x(15, 25, 16);
            HeadVanityPosition.AddFramePoint2x(16, 25, 16);
            HeadVanityPosition.AddFramePoint2x(17, 25, 16);
            HeadVanityPosition.AddFramePoint2x(18, 25, 16);

            HeadVanityPosition.AddFramePoint2x(19, 40, 39);
            HeadVanityPosition.AddFramePoint2x(20, 40, 39);
            GetRequests();
        }

        public void GetRequests()
        {
            //0
            AddNewRequest("A Little Treat", 200,
                "(You think you should give her a little treat to eat, you wonder if some cooked fish would be good.)",
                "(Once you said what you were going to give her, she got all happy, and waits for you to bring It.)",
                "(At the last moment, you changed your mind, and didn't told her about what you planned.)",
                "(She seems to have really enjoyed the food you gave to her.)",
                "(I need a fishing rod for this... And a lake in the forest...)",
                "(How did I managed to fail this?)");
            AddItemCollectionObjective(Terraria.ID.ItemID.CookedFish, 1, 0);
            //1
            AddNewRequest("Going for a Walk", 200,
                "(She seems to be wanting to go out for a walk. Doesn't matter where I take her, I just have to make sure she's safe and sound)",
                "(I told her that we were going out for a walk, and she loved the idea.)",
                "(You really doesn't feel like going anywhere right now...)",
                "(She have enjoyed the walk, but seems a bit exausted. Or not, she's already jumping again.)",
                "(I just need to take her for a walk, how hard can this be?)",
                "(How did I managed to fail this?)");
            AddExploreObjective(2000);
        }

        public override bool AlterRequestGiven(GuardianData Guardian, out int ForcedMissionID, out bool IsTalkRequest)
        {
            if(Main.rand.Next(3) > 0)
            {
                IsTalkRequest = false;
                ForcedMissionID = Main.rand.Next(2);
                return true;
            }
            return base.AlterRequestGiven(Guardian, out ForcedMissionID, out IsTalkRequest);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HaloTextureID, "halo");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Vector2 HaloDrawPosition = Vector2.Zero;
            switch (guardian.BodyAnimationFrame)
            {
                default:
                    HaloDrawPosition = new Vector2(37, 15);
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                    HaloDrawPosition = new Vector2(24, 10);
                    break;
                case 19:
                case 20:
                    HaloDrawPosition = new Vector2(39, 35);
                    break;
            }
            HaloDrawPosition.Y -= 7;
            HaloDrawPosition.X -= guardian.Base.SpriteWidth * 0.25f - 1;
            if (guardian.LookingLeft)
                HaloDrawPosition.X *= -1;
            HaloDrawPosition.Y -= guardian.Base.SpriteHeight * 0.5f;
            HaloDrawPosition = HaloDrawPosition * 2 * guardian.Scale + DrawPosition;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HaloTextureID), HaloDrawPosition, new Rectangle(0, 0, 26, 12), Color.White, Rotation, new Vector2(13, 6), Scale, SpriteEffects.None);
            TerraGuardian.DrawFront.Add(gdd);
            if (MainMod.NemesisFadeEffect > 0)
            {
                float TransparencyValue = (float)MainMod.NemesisFadeEffect / (MainMod.NemesisFadingTime * 0.5f);
                if(TransparencyValue > 1f)
                {
                    TransparencyValue = 1f - TransparencyValue + 1f;
                }
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HaloTextureID), HaloDrawPosition, new Rectangle(26, 0, 26, 12), Color.White * TransparencyValue, Rotation, new Vector2(13, 6), Scale, SpriteEffects.None);
                TerraGuardian.DrawFront.Add(gdd);
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Bark! Bark! *Wagging tail*");
            Mes.Add("*Chasing her own tail.*");
            Mes.Add("Bark! *She seems happy to see you.*");
            if (!Main.dayTime)
            {
                Mes.Add("Grrr.... *She's growling at something outside.*");
                Mes.Add("Yaaaawn~");
            }
            if (Main.moonPhase == 0)
                Mes.Add("Awoooooooooo~!!");
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*She's not staring at you*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You petted her, and then she lied down on the floor, wanting her belly rubbed.*");
            Mes.Add("*She starts licking your face out of happiness.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            return "Bark! Bark! (She looks okay right now.)";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            return "(She's staring at you, attentiously.)";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            return "(She started licking you, she seems really happy.)";
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.Next(2) == 0)
                return "(She seems really happy.)";
            return "(Is she trying to sing along the music?)";
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            return "(As she fixes her eyes at you, she approaches and snifs you. After doing so, she started to wag her tail and bark.)";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "Whine... Whine... Whine...";
                case 1:
                    return "(Licking the injured person)";
                case 2:
                    return "... Bark! Bark! Whine... Whine....";
            }
        }
    }
}
