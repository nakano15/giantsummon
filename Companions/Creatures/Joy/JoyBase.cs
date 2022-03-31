using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Companions
{
    public class JoyBase : PigGuardianFragmentBase
    {
        public JoyBase() : base(HappinessPigGuardianID)
        {
            Name = "Joy";
            PossibleNames = new string[] { "Cheery", "Fortue", "Glee", "Joy" };
            Description = "";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 64;
            //FramesInRows = 28;
            //DuckingHeight = 54;
            //Each pig should definitelly have the same size, birthday age and time, so I moved those infos.
            Genderless = true;
            InitialMHP = 110; //320
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 5;
            Accuracy = 0.67f;
            Mass = 0.40f;
            MaxSpeed = 3.62f;
            Acceleration = 0.12f;
            SlowDown = 0.35f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            DontUseHeavyWeapons = true;
            DrinksBeverage = false;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.YellowPhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.AmberStaff, 1));

            //Animation Frames

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 2);
            LeftHandPoints.AddFramePoint2x(11, 22, 9);
            LeftHandPoints.AddFramePoint2x(12, 24, 17);
            LeftHandPoints.AddFramePoint2x(13, 20, 22);

            LeftHandPoints.AddFramePoint2x(17, 24, 26);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 2);
            RightHandPoints.AddFramePoint2x(11, 25, 9);
            RightHandPoints.AddFramePoint2x(12, 27, 17);
            RightHandPoints.AddFramePoint2x(13, 23, 22);

            RightHandPoints.AddFramePoint2x(17, 26, 26);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16 + 2, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23 + 2, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23 + 2, 18);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                default: return "*Yay! I've met a new person! I'll be your best buddy forever!*";
                case 1: return "*I've never met someone like you before, will you be my friend?*";
                case 2: return "*Hello! I'm your newest friend from now on.*";
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Do you like picnics? I'm sure you would love it, let's try it sometime with a group of buddies.*");
            Mes.Add("*I want you to know that whatever happens, I will always be there for you if you ever need anything, pal.*");
            Mes.Add("*If you don't want me to help in battle, the least I can do is cheer you on from the sidelines.*");
            Mes.Add("*I'm always happy to just wake up to see another day. Everyday is a new day to spread positivity.*");
            Mes.Add("*Every week I will craft a heart crystal for you just to show how much I appreciate you as a friend.*");
            Mes.Add("*Don't be afraid to ask me to share anything because sharing is caring.*");

            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            return "*Don't worry. I have your back buddy!*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case GuardianBase.MessageIDs.AcquiredBurningDebuff:
                    return "*There's nothing like the warm feeling on fire.*";
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*The numbness, it brings me pleasure.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
