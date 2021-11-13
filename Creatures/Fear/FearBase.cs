using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Creatures
{
    public class FearBase : PigGuardianFragmentBase
    {
        public FearBase() : 
            base(FearPigGuardianID)
        {
            Name = "Fear";
            PossibleNames = new string[] { "Frighty", "Dread", "Anxie", "Pani" };
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very scaredy.";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 72;
            SpriteHeight = 68;
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

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.PurplePhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));

            //Animation Frames

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 12, 4);
            LeftHandPoints.AddFramePoint2x(11, 24, 11);
            LeftHandPoints.AddFramePoint2x(12, 26, 19);
            LeftHandPoints.AddFramePoint2x(13, 23, 25);

            LeftHandPoints.AddFramePoint2x(17, 27, 29);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 17, 4);
            RightHandPoints.AddFramePoint2x(11, 27, 11);
            RightHandPoints.AddFramePoint2x(12, 29, 19);
            RightHandPoints.AddFramePoint2x(13, 25, 24);

            RightHandPoints.AddFramePoint2x(17, 28, 28);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Point(16 + 2, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23 + 2, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23 + 2, 18);
        }

        public override string CallUnlockMessage => base.CallUnlockMessage;

        public override string ControlUnlockMessage => base.ControlUnlockMessage;

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Waaah!!! Who are you?! Are you friendly or not?!");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            return base.GetSpecialMessage(MessageID);
        }
    }
}
