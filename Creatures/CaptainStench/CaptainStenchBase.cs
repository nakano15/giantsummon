using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Creatures
{
    public class CaptainStenchBase : GuardianBase
    {
        public const string PlasmaFalchionTextureID = "plasmafalchion_", PhantomBlinkTextureID = "phantomblink", ScouterTextureID = "scouter", HeadNoScouterTextureID = "headnoscouter",
            RubyGPTextureID = "rubygp", DiamondGPTextureID = "diamondgp";
        public const int NumberOfSwords = 7;
        public const int StandardFalchion = 0, AmethystFalchion = 1, TopazFalchion = 2, SapphireFalchion = 3, EmeraldFalchion = 4, RubyFalchion = 5, DiamondFalchion = 6;
        public const int VerticalSwingSubAttack = 0, GPSubAttack = 1, ArmBlasterSubAttack = 2, PhantomRushSubAttack = 3;

        private const float TopazFalchionAttackSpeedMult = 1.5f, SapphireFalchionAttackSpeedMult = 0.7f;

        public CaptainStenchBase()
        {
            Name = "CaptainStench";
            PossibleNames = new string[] { "Cpt. Stench" }; //Captain Sally Stench
            Description = "";
            Size = GuardianSize.Large;
            Width = 22;
            Height = 66;
            CharacterPositionYDiscount = 22;
            //DuckingHeight = 62;
            SpriteWidth = 160;
            SpriteHeight = 140;
            FramesInRows = 12;
            Age = 32;
            SetBirthday(SEASON_SUMMER, 1);
            Male = false;
            InitialMHP = 150; //1100
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 5;
			InitialMP = 40;
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
            DontUseHeavyWeapons = true;
            SpecialAttackBasedCombat = true;
            UsesRightHandByDefault = true;
            ForceWeaponUseOnMainHand = true;
            SetTerraGuardian();

            this.MountUnlockLevel = 255;
            this.ControlUnlockLevel = 255;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = 18;
            ItemUseFrames = new int[] { 22, 22, 23, 24 };
            HeavySwingFrames = new int[] { 43, 44, 45 };
            //DuckingFrame = 23;
            //DuckingSwingFrames = new int[] { 24, 25, 26 };
            //SittingFrame = 15;
            ChairSittingFrame = 75;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 73;
            BedSleepingFrame = 74;
            DownedFrame = 25;
            ReviveFrame = 26;

            //
            RightArmFrontFrameSwap.Add(1, 1);
            for(int i = 10; i < 18; i++)
                RightArmFrontFrameSwap.Add(i, 1);
            RightArmFrontFrameSwap.Add(20, 2);
            RightArmFrontFrameSwap.Add(21, 3);
            RightArmFrontFrameSwap.Add(22, 4);
            RightArmFrontFrameSwap.Add(23, 5);
            RightArmFrontFrameSwap.Add(24, 6);
            RightArmFrontFrameSwap.Add(25, 7);
            RightArmFrontFrameSwap.Add(26, 8);
            for(int i = 35; i < 43; i++)
                RightArmFrontFrameSwap.Add(i, 9);
            RightArmFrontFrameSwap.Add(43, 10);
            RightArmFrontFrameSwap.Add(44, 0);
            RightArmFrontFrameSwap.Add(45, 11);
            //
            RightArmFrontFrameSwap.Add(46, 12);
            RightArmFrontFrameSwap.Add(47, 12);
            RightArmFrontFrameSwap.Add(48, 12);
            RightArmFrontFrameSwap.Add(49, 13);
            RightArmFrontFrameSwap.Add(50, 14);
            RightArmFrontFrameSwap.Add(51, 15);
            RightArmFrontFrameSwap.Add(52, 16);
            RightArmFrontFrameSwap.Add(53, 17);
            RightArmFrontFrameSwap.Add(54, 17);
            RightArmFrontFrameSwap.Add(55, 15);
            RightArmFrontFrameSwap.Add(56, 16);
            //
            RightArmFrontFrameSwap.Add(57, 1);
            RightArmFrontFrameSwap.Add(58, 1);
            RightArmFrontFrameSwap.Add(59, 1);
            //
            RightArmFrontFrameSwap.Add(60, 3);
            RightArmFrontFrameSwap.Add(61, 3);
            RightArmFrontFrameSwap.Add(62, 3);
            //
            RightArmFrontFrameSwap.Add(63, 20);
            RightArmFrontFrameSwap.Add(64, 20);
            RightArmFrontFrameSwap.Add(65, 20);
            RightArmFrontFrameSwap.Add(66, 20);
            RightArmFrontFrameSwap.Add(67, 21);
            RightArmFrontFrameSwap.Add(68, 21);
            RightArmFrontFrameSwap.Add(69, 21);
            RightArmFrontFrameSwap.Add(70, 21);
            RightArmFrontFrameSwap.Add(71, 21);
            RightArmFrontFrameSwap.Add(72, 21);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(57, 46, 44);
            LeftHandPoints.AddFramePoint2x(58, 46, 47);
            LeftHandPoints.AddFramePoint2x(59, 48, 34);

            LeftHandPoints.AddFramePoint2x(60, 42, 43);
            LeftHandPoints.AddFramePoint2x(61, 45, 37);
            LeftHandPoints.AddFramePoint2x(62, 44, 31);

            //Right Arm
            RightHandPoints.AddFramePoint2x(22, 50, 34);
            RightHandPoints.AddFramePoint2x(23, 50, 40);
            RightHandPoints.AddFramePoint2x(24, 48, 45);

            RightHandPoints.AddFramePoint2x(26, 53, 52);

            SubAttacksSetup();
            TopicList();
        }

        public void TopicList()
        {
            AddTopic("Repair Phantom Device", delegate ()
            {
                CaptainStenchData data = (CaptainStenchData)Dialogue.GetSpeaker().Data;
                switch (data.PhantomDeviceMiniquestProgress)
                {
                    case 0: //Broken
                        {
                            const int BarCount = 15;
                            Dialogue.ShowDialogueOnly("It will give us some work. It isn't unrepairable, though.\n" +
                                "I will need " + BarCount + " Platinum or Gold Bars to repair it.");
                            int GoldBarCount = Dialogue.CountItem(Terraria.ID.ItemID.GoldBar),
                            PlatinumBarCount = Dialogue.CountItem(Terraria.ID.ItemID.PlatinumBar);
                            if (GoldBarCount >= BarCount ||
                                PlatinumBarCount >= BarCount)
                            {
                                Dialogue.AddOption("I got the items.", delegate (TerraGuardian tg)
                                {
                                    bool IsGoldBarTheStackThatPassedMaxBarCount = GoldBarCount >= BarCount;
                                    if (IsGoldBarTheStackThatPassedMaxBarCount)
                                    {
                                        Dialogue.TakeItem(Terraria.ID.ItemID.GoldBar, BarCount);
                                    }
                                    else
                                    {
                                        Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumBar, BarCount);
                                    }
                                    data.PhantomDeviceMiniquestProgress = 1;
                                    Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier1>());
                                    Dialogue.ShowEndDialogueMessage("Great. I managed to fix It. I will now be able to use Phantom Dash.\n" +
                                        "I can upgrade It to allow me to better use the Phantom Dash.", false);
                                });
                            }
                            Dialogue.AddOption("I'll try gathering them.", delegate (TerraGuardian tg)
                            {
                                Dialogue.ShowEndDialogueMessage("Alright.", false);
                            });
                        }
                        break;
                    case 1: //Tier 1
                        {
                            const int ManaCrystalNecessary = 1;
                            Dialogue.ShowDialogueOnly("For this upgrade, not only I will need the Phantom Device Tier 1, but will also need " + ManaCrystalNecessary + " mana crystal.");
                            if (Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier1>()) &&
                            Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalNecessary)
                            {
                                Dialogue.AddOption("Here's everything.", delegate (TerraGuardian tg)
                                {
                                    Dialogue.TakeItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier1>());
                                    Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalNecessary);
                                    Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier2>());
                                    Dialogue.ShowEndDialogueMessage("Not bad! The Phantom Device got improved.\n" +
                                        "I can improve It some more, if you're up to It.\n" +
                                        "I can make use of Phantom Rush if I equip that in one of my accessory slots.", false);
                                    data.PhantomDeviceMiniquestProgress = 2;
                                });
                            }
                            else if (!Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier1>()) &&
                            !Dialogue.GuardianHasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier1>(), CaptainSmelly))
                            {
                                Dialogue.AddOption("I lost the Phantom Device...", delegate(TerraGuardian tg)
                                {
                                    const int BarCount = 20, GoldCoinCount = 10;
                                    Dialogue.ShowDialogueOnly("You what?! Then I will need you to collect " + BarCount + " Gold or Platinum Bars.\n" +
                                        "And since you managed to lose my Phantom Device, you'll have to give me "+ GoldCoinCount + " Gold Coins.");
                                    bool HasGoldBars = Dialogue.CountItem(Terraria.ID.ItemID.GoldBar) >= BarCount,
                                         HasPlatinumBars = Dialogue.CountItem(Terraria.ID.ItemID.PlatinumBar) >= BarCount;
                                    if ((HasGoldBars || HasPlatinumBars) && Dialogue.GetPlayerCoinValues() >= Dialogue.GetCoinValues(0, 0, GoldCoinCount))
                                    {
                                        Dialogue.AddOption("Here... Take It...", delegate(TerraGuardian tg2)
                                        {
                                            if (HasGoldBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.GoldBar, BarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumBar, BarCount);
                                            Dialogue.TakeCoins(Dialogue.GetCoinValues(0, 0, GoldCoinCount));
                                            Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier1>());
                                            Dialogue.ShowEndDialogueMessage("Now think twice before you lose my Phantom Device.");
                                        });
                                    }
                                    Dialogue.AddOption("Nah, too expensive.", delegate(TerraGuardian tg2)
                                    {
                                        Dialogue.ShowEndDialogueMessage("Then the next time don't lose something that isn't yours.");
                                    });
                                });
                            }
                            Dialogue.AddOption("Too hard...", delegate (TerraGuardian tg)
                            {
                                Dialogue.ShowEndDialogueMessage("How? Serious?", false);
                            });
                        }
                        break;
                    case 2: //Tier 2
                        {
                            const int ManaCrystalNecessary = 2;
                            Dialogue.ShowDialogueOnly("You know the drill. I will need a Phantom Device Tier 2, but will also need " + ManaCrystalNecessary + " mana crystals.");
                            if (Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier2>()) &&
                            Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalNecessary)
                            {
                                Dialogue.AddOption("Here's everything.", delegate (TerraGuardian tg)
                                {
                                    Dialogue.TakeItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier2>());
                                    Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalNecessary);
                                    Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier3>());
                                    Dialogue.ShowEndDialogueMessage("It looks even better than the last one!\n" +
                                        "But I think we can still do better.", false);
                                    data.PhantomDeviceMiniquestProgress = 3;
                                });
                            }
                            else if (!Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier2>()) &&
                            !Dialogue.GuardianHasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier2>(), CaptainSmelly))
                            {
                                Dialogue.AddOption("I lost the Phantom Device...", delegate (TerraGuardian tg)
                                {
                                    const int BarCount = 20, GoldCoinCount = 20, ManaCrystalCount = 1;
                                    Dialogue.ShowDialogueOnly("You what?! Then I will need you to collect " + BarCount + " Gold or Platinum Bars and "+ManaCrystalCount+" Mana Crystal.\n" +
                                        "And since you managed to lose my Phantom Device, you'll have to give me " + GoldCoinCount + " Gold Coins.");
                                    bool HasGoldBars = Dialogue.CountItem(Terraria.ID.ItemID.GoldBar) >= BarCount,
                                         HasPlatinumBars = Dialogue.CountItem(Terraria.ID.ItemID.PlatinumBar) >= BarCount;
                                    if ((HasGoldBars || HasPlatinumBars) && Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalCount && 
                                    Dialogue.GetPlayerCoinValues() >= Dialogue.GetCoinValues(0, 0, GoldCoinCount))
                                    {
                                        Dialogue.AddOption("Here... Take It...", delegate (TerraGuardian tg2)
                                        {
                                            if (HasGoldBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.GoldBar, BarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumBar, BarCount);
                                            Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalCount);
                                            Dialogue.TakeCoins(Dialogue.GetCoinValues(0, 0, GoldCoinCount));
                                            Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier2>());
                                            Dialogue.ShowEndDialogueMessage("Now think twice before you lose my Phantom Device.");
                                        });
                                    }
                                    Dialogue.AddOption("Nah, too expensive.", delegate (TerraGuardian tg2)
                                    {
                                        Dialogue.ShowEndDialogueMessage("Then the next time don't lose something that isn't yours.");
                                    });
                                });
                            }
                            Dialogue.AddOption("Too hard...", delegate (TerraGuardian tg)
                            {
                                Dialogue.ShowEndDialogueMessage("Bad luck with fallen stars?", false);
                            });
                        }
                        break;
                    case 3: //Tier 3
                        {
                            const int ManaCrystalNecessary = 3, BarsNecessary = 10;
                            Dialogue.ShowDialogueOnly("We'll need a Phantom Device Tier 3 for this one, " +BarsNecessary+ " Cobalt or Palladium Bars, and also " + ManaCrystalNecessary + " mana crystals for this upgrade.");
                            bool HasCobaltBars = Dialogue.CountItem(Terraria.ID.ItemID.CobaltBar) >= BarsNecessary,
                                 HasPalladiumBars = Dialogue.CountItem(Terraria.ID.ItemID.PalladiumBar) >= BarsNecessary;
                            if (Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier3>()) &&
                            (HasCobaltBars || HasPalladiumBars) &&
                            Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalNecessary)
                            {
                                Dialogue.AddOption("Here's everything.", delegate (TerraGuardian tg)
                                {
                                    if (HasCobaltBars)
                                        Dialogue.TakeItem(Terraria.ID.ItemID.CobaltBar, BarsNecessary);
                                    else
                                        Dialogue.TakeItem(Terraria.ID.ItemID.PalladiumBar, BarsNecessary);
                                    Dialogue.TakeItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier3>());
                                    Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalNecessary);
                                    Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier4>());
                                    Dialogue.ShowEndDialogueMessage("This is amazing! It's even better than before!\n" +
                                        "I think I can overclock this. May be dangerous, but we can try. Right?", false);
                                    data.PhantomDeviceMiniquestProgress = 4;
                                });
                            }
                            else if (!Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier3>()) &&
                            !Dialogue.GuardianHasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier3>(), CaptainSmelly))
                            {
                                Dialogue.AddOption("I lost the Phantom Device...", delegate (TerraGuardian tg)
                                {
                                    const int BarCount = 20, GoldCoinCount = 30, ManaCrystalCount = 3;
                                    Dialogue.ShowDialogueOnly("You what?! Then I will need you to collect " + BarCount + " Gold or Platinum Bars and " + ManaCrystalCount + " Mana Crystals.\n" +
                                        "And since you managed to lose my Phantom Device, you'll have to give me " + GoldCoinCount + " Gold Coins.");
                                    bool HasGoldBars = Dialogue.CountItem(Terraria.ID.ItemID.GoldBar) >= BarCount,
                                         HasPlatinumBars = Dialogue.CountItem(Terraria.ID.ItemID.PlatinumBar) >= BarCount;
                                    if ((HasGoldBars || HasPlatinumBars) && Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalCount &&
                                    Dialogue.GetPlayerCoinValues() >= Dialogue.GetCoinValues(0, 0, GoldCoinCount))
                                    {
                                        Dialogue.AddOption("Here... Take It...", delegate (TerraGuardian tg2)
                                        {
                                            if (HasGoldBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.GoldBar, BarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumBar, BarCount);
                                            Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalCount);
                                            Dialogue.TakeCoins(Dialogue.GetCoinValues(0, 0, GoldCoinCount));
                                            Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier3>());
                                            Dialogue.ShowEndDialogueMessage("Now think twice before you lose my Phantom Device.");
                                        });
                                    }
                                    Dialogue.AddOption("Nah, too expensive.", delegate (TerraGuardian tg2)
                                    {
                                        Dialogue.ShowEndDialogueMessage("Then the next time don't lose something that isn't yours.");
                                    });
                                });
                            }
                            Dialogue.AddOption("Too hard...", delegate (TerraGuardian tg)
                            {
                                Dialogue.ShowEndDialogueMessage("You're just lazy, aren't you?", false);
                            });
                        }
                        break;
                    case 4: //Tier 4
                        {
                            const int BarCount = 10;
                            Dialogue.ShowDialogueOnly("To overclock the Phantom Device, I'll need a Tier 4 version of It, a Gold or Platinum Watch, and a Lightning Boots.\n" +
                                "And also " + BarCount + " Chlorophyte Bars.");
                            bool HasGoldWatch = Dialogue.HasItem(Terraria.ID.ItemID.GoldWatch);
                            bool HasPlatinumWatch = Dialogue.HasItem(Terraria.ID.ItemID.PlatinumWatch);
                            if (Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier4>()) &&
                            (HasGoldWatch || HasPlatinumWatch) && Dialogue.CountItem(Terraria.ID.ItemID.ChlorophyteBar) >= 10 && Dialogue.HasItem(Terraria.ID.ItemID.LightningBoots))
                            {
                                Dialogue.AddOption("Here's everything.", delegate (TerraGuardian tg)
                                {
                                    Dialogue.TakeItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier4>());
                                    if (HasGoldWatch)
                                    {
                                        Dialogue.TakeItem(Terraria.ID.ItemID.GoldWatch);
                                    }
                                    else
                                    {
                                        Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumWatch);
                                    }
                                    Dialogue.TakeItem(Terraria.ID.ItemID.ChlorophyteBar, BarCount);
                                    Dialogue.TakeItem(Terraria.ID.ItemID.LightningBoots);
                                    Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier5>());
                                    Dialogue.ShowEndDialogueMessage("Look at this! It's beautiful! I will cause so much damage with this thing.", false);
                                    data.PhantomDeviceMiniquestProgress = 5;
                                });
                            }
                            else if (!Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier4>()) &&
                            !Dialogue.GuardianHasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier4>(), CaptainSmelly))
                            {
                                Dialogue.AddOption("I lost the Phantom Device...", delegate (TerraGuardian tg)
                                {
                                    const int SecondBarCount = 20, GoldCoinCount = 40, ManaCrystalCount = 7, HMBarCount = 10;
                                    Dialogue.ShowDialogueOnly("You what?! Then I will need you to collect " + SecondBarCount + " Gold or Platinum Bars, "+HMBarCount+" Cobalt or Palladium Bars and " + ManaCrystalCount + " Mana Crystals.\n" +
                                        "And since you managed to lose my Phantom Device, you'll have to give me " + GoldCoinCount + " Gold Coins.");
                                    bool HasGoldBars = Dialogue.CountItem(Terraria.ID.ItemID.GoldBar) >= SecondBarCount,
                                         HasPlatinumBars = Dialogue.CountItem(Terraria.ID.ItemID.PlatinumBar) >= SecondBarCount;
                                    bool HasCobaltBars = Dialogue.CountItem(Terraria.ID.ItemID.CobaltBar) >= HMBarCount,
                                         HasPalladiumBars = Dialogue.CountItem(Terraria.ID.ItemID.PalladiumBar) >= HMBarCount;
                                    if ((HasGoldBars || HasPlatinumBars) && (HasCobaltBars || HasPalladiumBars) && Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalCount &&
                                    Dialogue.GetPlayerCoinValues() >= Dialogue.GetCoinValues(0, 0, GoldCoinCount))
                                    {
                                        Dialogue.AddOption("Here... Take It...", delegate (TerraGuardian tg2)
                                        {
                                            if (HasGoldBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.GoldBar, SecondBarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumBar, SecondBarCount);
                                            if (HasCobaltBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.CobaltBar, HMBarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PalladiumBar, HMBarCount);
                                            Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalCount);
                                            Dialogue.TakeCoins(Dialogue.GetCoinValues(0, 0, GoldCoinCount));
                                            Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier4>());
                                            Dialogue.ShowEndDialogueMessage("Now think twice before you lose my Phantom Device.");
                                        });
                                    }
                                    Dialogue.AddOption("Nah, too expensive.", delegate (TerraGuardian tg2)
                                    {
                                        Dialogue.ShowEndDialogueMessage("Then the next time don't lose something that isn't yours.");
                                    });
                                });
                            }
                            Dialogue.AddOption("Too hard...", delegate (TerraGuardian tg)
                            {
                                Dialogue.ShowEndDialogueMessage("You're just lazy, aren't you?", false);
                            });
                        }
                        break;
                    case 5: //Tier 5
                        {
                            Dialogue.ShowDialogueOnly("There's no more upgrades I can do on the Phantom Device.");
                            if (!Dialogue.HasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier5>()) &&
                            !Dialogue.GuardianHasItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier5>(), CaptainSmelly))
                            {
                                Dialogue.AddOption("I lost the Phantom Device...", delegate (TerraGuardian tg)
                                {
                                    const int SecondBarCount = 20, GoldCoinCount = 50, ManaCrystalCount = 7, HMBarCount = 10, ChlorophyteBarCount = 10;
                                    Dialogue.ShowDialogueOnly("You what?! Then I will need you to collect " + SecondBarCount + " Gold or Platinum Bars, " + HMBarCount + " Cobalt or Palladium Bars and " + ManaCrystalCount + " Mana Crystals.\n" +
                                        "I will also need a Gold or Platinum Watch, and a Lightning Boots.\n" +
                                        "And since you managed to lose my Phantom Device, you'll have to give me " + GoldCoinCount + " Gold Coins.");
                                    bool HasGoldBars = Dialogue.CountItem(Terraria.ID.ItemID.GoldBar) >= SecondBarCount,
                                         HasPlatinumBars = Dialogue.CountItem(Terraria.ID.ItemID.PlatinumBar) >= SecondBarCount;
                                    bool HasCobaltBars = Dialogue.CountItem(Terraria.ID.ItemID.CobaltBar) >= HMBarCount,
                                         HasPalladiumBars = Dialogue.CountItem(Terraria.ID.ItemID.PalladiumBar) >= HMBarCount;
                                    bool HasGoldWatch = Dialogue.HasItem(Terraria.ID.ItemID.GoldWatch),
                                        HasPlatinumWatch = Dialogue.HasItem(Terraria.ID.ItemID.PlatinumWatch);
                                    if ((HasGoldBars || HasPlatinumBars) && (HasCobaltBars || HasPalladiumBars) && (HasGoldWatch || HasPlatinumWatch) && Dialogue.HasItem(Terraria.ID.ItemID.LightningBoots) &&
                                    Dialogue.CountItem(Terraria.ID.ItemID.ChlorophyteBar) >= ChlorophyteBarCount && Dialogue.CountItem(Terraria.ID.ItemID.ManaCrystal) >= ManaCrystalCount &&
                                    Dialogue.GetPlayerCoinValues() >= Dialogue.GetCoinValues(0, 0, GoldCoinCount))
                                    {
                                        Dialogue.AddOption("Here... Take It...", delegate (TerraGuardian tg2)
                                        {
                                            if (HasGoldBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.GoldBar, SecondBarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumBar, SecondBarCount);
                                            if (HasCobaltBars)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.CobaltBar, HMBarCount);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PalladiumBar, HMBarCount);
                                            if (HasGoldWatch)
                                                Dialogue.TakeItem(Terraria.ID.ItemID.GoldWatch);
                                            else
                                                Dialogue.TakeItem(Terraria.ID.ItemID.PlatinumWatch);
                                            Dialogue.TakeItem(Terraria.ID.ItemID.LightningBoots);
                                            Dialogue.TakeItem(Terraria.ID.ItemID.ManaCrystal, ManaCrystalCount);
                                            Dialogue.TakeItem(Terraria.ID.ItemID.ChlorophyteBar, ChlorophyteBarCount);
                                            Dialogue.TakeCoins(Dialogue.GetCoinValues(0, 0, GoldCoinCount));
                                            Dialogue.GiveItem(Terraria.ModLoader.ModContent.ItemType<CaptainStench.PhantomDevices.PhantomDeviceTier5>());
                                            Dialogue.ShowEndDialogueMessage("Now think twice before you lose my Phantom Device.");
                                        });
                                    }
                                    Dialogue.AddOption("Nah, too expensive.", delegate (TerraGuardian tg2)
                                    {
                                        Dialogue.ShowEndDialogueMessage("Then the next time don't lose something that isn't yours.");
                                    });
                                });
                            }
                            Dialogue.AddOption("Let's talk about something else, then.", delegate(TerraGuardian tg)
                            {
                                Dialogue.ShowEndDialogueMessage("What else you want to talk about?");
                            });
                        }
                        break;
                }
            }, delegate (TerraGuardian tg, PlayerMod p)
             {
                 CaptainStenchData data = (CaptainStenchData)tg.Data;
                 return data.PhantomDeviceMiniquestProgress >= 0;
             });
        }

        public override void Attributes(TerraGuardian g)
        {
            //g.AddFlag(GuardianFlags.FeatherfallPotion);
            g.AddFlag(GuardianFlags.DisableMountSharing);
        }

        public override void GuardianResetStatus(TerraGuardian guardian)
        {
            (guardian.Data as CaptainStenchData).DeviceID = 0;
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            for (int i = 0; i < 7; i++)
            {
                sprites.AddExtraTexture(PlasmaFalchionTextureID + i, "PlasmaFalchion/plasma_falchion_" + i);
            }
            sprites.AddExtraTexture(PhantomBlinkTextureID, "phantom_blink");
            sprites.AddExtraTexture(ScouterTextureID, "scouter");
            sprites.AddExtraTexture(HeadNoScouterTextureID, "head_no_scouter");
            sprites.AddExtraTexture(RubyGPTextureID, "RubyGP");
            sprites.AddExtraTexture(DiamondGPTextureID, "DiamondGP");
        }

        public override void ModifyVelocity(TerraGuardian tg, ref Vector2 Velocity)
        {
            if(!tg.BeingPulledByPlayer && Velocity.Y > 4)
            {
                Velocity.Y = 4;
                tg.SetFallStart();
            }
        }

        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<GuardianMouseOverAndDialogueInterface.DialogueOption> NewOptions = new List<GuardianMouseOverAndDialogueInterface.DialogueOption>();
            {
                CaptainStenchData data = (CaptainStenchData)guardian.Data;
                if(data.PhantomDeviceMiniquestProgress == -1)
                {
                    GuardianMouseOverAndDialogueInterface.DialogueOption option = new GuardianMouseOverAndDialogueInterface.DialogueOption("What is that thing you're carrying.", 
                        delegate(TerraGuardian tg)
                        {
                            data.PhantomDeviceMiniquestProgress = 0;
                            Dialogue.ShowEndDialogueMessage("*That thing is the Phantom Device. It broke during the crash, and now It needs repair.*", false);
                        });
                    NewOptions.Add(option);
                }
            }
            NewOptions.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("Weapon Infusion", delegate (TerraGuardian tg)
            {
                CaptainStenchData data = (CaptainStenchData)tg.Data;
                GuardianMouseOverAndDialogueInterface.Options.Clear();
                {
                    string Mes = "";
                    switch (data.SwordID)
                    {
                        case StandardFalchion:
                            Mes = "nothing.";
                            break;
                        case AmethystFalchion:
                            Mes = "Amethyst power.";
                            break;
                        case TopazFalchion:
                            Mes = "Topaz power.";
                            break;
                        case SapphireFalchion:
                            Mes = "Sapphire power.";
                            break;
                        case EmeraldFalchion:
                            Mes = "Emerald power.";
                            break;
                        case RubyFalchion:
                            Mes = "Ruby power.";
                            break;
                        case DiamondFalchion:
                            Mes = "Diamond power.";
                            break;
                    }
                    GuardianMouseOverAndDialogueInterface.SetDialogue("My Falchion is infused with " +Mes + "\nWhat should I infuse my weapon with?\nGem will be used upon changing Infusion.");
                }
                for (byte i = 0; i < NumberOfSwords; i++)
                {
                    byte InfusionID = i;
                    string Mes = "";
                    string Description = "";
                    int ItemID = 0;
                    switch (i)
                    {
                        case 0:
                            Mes = "Remove Infusion";
                            Description = "Removes any infusion from the weapon.";
                            break;
                        case 1:
                            Mes = "Infuse with Amethyst";
                            Description = "Amethyst: Passive-Sword attacks apply shadowflame debuff. \n"+
                                "Gem Power-Blade wave that does damage and bypasses enemies and blocks.";
                            ItemID = Terraria.ID.ItemID.Amethyst;
                            break;
                        case 2:
                            Mes = "Infuse with Topaz";
                            Description = "Topaz- Passive: Sword attacks gain tremendous knock back but attack speed is lowered. \n" +
"                               Gem Power-Flings Topaz shards that peirce enemies shredding armor value and applying bleeding debuff.";
                            ItemID = Terraria.ID.ItemID.Topaz;
                            break;
                        case 3:
                            Mes = "Infuse with Sapphire";
                            Description = "Sapphire: Passive- Gain exponential amount of attack speed but lowers damage output by -20%. \n" +
                                "Gem Power-Fires a energy missle that explodes on contact dealing AOE damage and applying electried debuff.";
                            ItemID = Terraria.ID.ItemID.Sapphire;
                            break;
                        case 4:
                            Mes = "Infuse with Emerald";
                            Description = "Emerald: Passive- Gains 50% critical chance on sword attacks additonally critical strikes receive a 3x damage boost(3x damage boost applies to gem power also)\n" +
                                "Gem Power-Conjures up a slow moving tornado that hits many times in a 3 second duration, each hit is a garunteed critical strike that scales with 25 % of total weapon damage";
                            ItemID = Terraria.ID.ItemID.Emerald;
                            break;
                        case 5:
                            Mes = "Infuse with Ruby";
                            Description = "Ruby: Passive- Attacks gain lifesteal \n" +
                                "Gem Power-Whiplike attack that drains enemies of their hp and heals smelly for 5 % of her max health per enemy hit additionally if the sweat spot of the attack hits 50 % max health is restored";
                            ItemID = Terraria.ID.ItemID.Ruby;
                            break;
                        case 6:
                            Mes = "Infuse with Diamond";
                            Description = "Diamond: Passive- Sword attacks deal 5% max health bonus damage, additionally all sword strikes have a 50% chance to cause dazzed debuff\n" +
                                "Gem Power-Shines bright flash that does 10 % max health true damage with 100 % chance to cause confused debuff.";
                            ItemID = Terraria.ID.ItemID.Diamond;
                            break;
                    }
                    if ((i == 0 && data.SwordID > 0) || (ItemID > 0 && Main.player[Main.myPlayer].HasItem(ItemID)))
                    {
                        GuardianMouseOverAndDialogueInterface.AddOption(Mes, delegate (TerraGuardian tg2)
                        {
                            GuardianMouseOverAndDialogueInterface.SetDialogue(Description + "\n\nShould I infuse my falchion with this gem?");
                            GuardianMouseOverAndDialogueInterface.Options.Clear();
                            GuardianMouseOverAndDialogueInterface.AddOption("Yes", delegate (TerraGuardian tg3)
                            {
                                int item = ItemID;
                                for (int j = 0; j < 50; j++)
                                {
                                    if (Main.player[Main.myPlayer].inventory[j].type == item)
                                    {
                                        Main.player[Main.myPlayer].inventory[j].stack--;
                                        if (Main.player[Main.myPlayer].inventory[j].stack <= 0)
                                            Main.player[Main.myPlayer].inventory[j].SetDefaults(0);
                                        break;
                                    }
                                }
                                data.HoldingWeaponTime = 150;
                                data.SwordID = InfusionID;
                                GuardianMouseOverAndDialogueInterface.SetDialogue("Done. What else?");
                                GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg2);
                            });
                            GuardianMouseOverAndDialogueInterface.AddOption("No", delegate (TerraGuardian tg3)
                            {
                                GuardianMouseOverAndDialogueInterface.SetDialogue("Well, anything else then?");
                                GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg2);
                            });
                        });
                    }
                }
                GuardianMouseOverAndDialogueInterface.AddOption("What does Weapon infusion do?", delegate (TerraGuardian tg2)
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("I can infuse my Falchion with gemstone powers to cause different effects when I use it.\n" +
                        "I need to have a gemstone before I can do the infusion.");
                    GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg2);
                });
                GuardianMouseOverAndDialogueInterface.AddOption("Nevermind", delegate (TerraGuardian tg2)
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("Thanks for wasting my time.");
                    GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg2);
                });
            }));
            return NewOptions;
        }

        private int GetCalculatedSwordDamage(TerraGuardian tg)
        {
            int Damage = 15;
            if (tg.SelectedItem > -1)
            {
                Damage += (int)(tg.Inventory[tg.SelectedItem].damage * ((float)tg.Inventory[tg.SelectedItem].useAnimation / tg.Inventory[tg.SelectedItem].useTime));
            }
            CaptainStenchData data = (CaptainStenchData)tg.Data;
            if (data.SwordID > 0)
                Damage = (int)(Damage * 1.5f);
            return (int)(Damage * tg.MeleeDamageMultiplier);
        }

        private void SubAttackBegginingScript(TerraGuardian tg)
        {
            CaptainStenchData data = (CaptainStenchData)tg.Data;
            switch (data.SwordID)
            {
                case TopazFalchion:
                    tg.SubAttackSpeed = TopazFalchionAttackSpeedMult;
                    break;
                case SapphireFalchion:
                    tg.SubAttackSpeed = SapphireFalchionAttackSpeedMult;
                    break;
            }
        }

        public void SubAttacksSetup()
        {
            VSwingSetup();
            GPSetup();
            ArmBlasterSetup();
            PhantomRushSetup();
        }

        public void PhantomRushSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Melee);
            special.MinRange = 0;
            special.MaxRange = 200;
            special.ManaCost = 20;
            special.CanMove = false;
            special.Cooldown = 10 * 60;
            for (int i = 63; i < 73; i++)
            {
                AddNewSubAttackFrame(3, i, i, i);
            }
            special.WhenSubAttackBegins = delegate (TerraGuardian tg)
            {
                tg.TrailDelay = 3;
                tg.TrailLength = 5;
            };
            special.WhenFrameUpdatesScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
                tg.Velocity = Vector2.Zero;
                if (Frame >= 4)
                {
                    tg.ImmuneTime = 30;
                    tg.ImmuneNoBlink = true;
                    //tg.Velocity.X = tg.Direction * 10;
                    tg.Position.X += tg.Direction * 20;
                    if (Collision.SolidCollision(tg.TopLeftPosition, tg.CollisionWidth, tg.CollisionHeight))
                    {
                        if (tg.LookingLeft)
                        {
                            tg.Position.X = (int)(tg.Position.X * 0.0625f) * 16 + 16 + tg.CollisionWidth * 0.5f;
                        }
                        else
                        {
                            tg.Position.X = (int)(tg.Position.X * 0.0625f) * 16 + tg.CollisionWidth - tg.CollisionWidth * 0.5f;
                        }
                    }
                    /*float Stack = 0f;
                    for(int i = 0; i < 3; i++)
                    {
                        float SumX = i * 16;
                        if (Collision.SolidCollision(tg.TopLeftPosition + new Vector2(SumX, 0), tg.CollisionWidth, tg.CollisionHeight))
                        {
                            if (tg.LookingLeft)
                            {
                                tg.Position.X = SumX + 16 + tg.CollisionWidth * 0.5f;
                            }
                            else
                            {
                                tg.Position.X = SumX + tg.CollisionWidth - tg.CollisionWidth * 0.5f;
                            }
                            Stack = 0;
                            break;
                        }
                        Stack += 16;
                        if (Stack > 40)
                            Stack = 40;
                    }
                    tg.Position.X += Stack * tg.Direction;*/
                    Rectangle rect = tg.HitBox;
                    int Damage = (int)(GetCalculatedSwordDamage(tg) * 1.2f);
                    float Knockback = 1.5f;
                    int CriticalRate = 65 + tg.MeleeCriticalRate;
                    int SwordID = (tg.Data as CaptainStenchData).SwordID;
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(rect))
                        {
                            if (!Main.npc[n].dontTakeDamage)
                            {
                                int HitDirection = tg.Direction;
                                if ((HitDirection == -1 && tg.Position.X < Main.npc[n].Center.X) ||
                                    (HitDirection == 1 && tg.Position.X > Main.npc[n].Center.X))
                                {
                                    HitDirection *= -1;
                                }
                                bool Critical = (Main.rand.Next(100) < CriticalRate);
                                int NewDamage = Damage;
                                if (tg.OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
                                {
                                    float DamageMult = Main.player[tg.OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                                    NewDamage = (int)(NewDamage * DamageMult);
                                }
                                double result = Main.npc[n].StrikeNPC(NewDamage, Knockback, HitDirection, Critical);
                                if (result > 0)
                                {
                                    if (SwordID == AmethystFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.4)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.ShadowFlame, 10 * 60);
                                    }
                                    else if (SwordID == RubyFalchion)
                                    {
                                        int HealthRecover = (int)(Math.Max(1, result * 0.05f));
                                        tg.RestoreHP(HealthRecover);
                                    }
                                    else if (SwordID == DiamondFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.2)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.Dazed, 3 * 60);
                                    }
                                }
                                Main.PlaySound(Main.npc[n].HitSound, Main.npc[n].Center);
                                tg.AddNpcHit(n);
                                tg.IncreaseDamageStacker((int)result, Main.npc[n].lifeMax);
                                if (result > 0)
                                {
                                    if (tg.HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        tg.IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    tg.OnHitSomething(Main.npc[n]);
                                    tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (SwordID == AmethystFalchion)
                                        tg.AddSkillProgress((float)result * 0.15f, GuardianSkills.SkillTypes.Mysticism);
                                    if (Critical)
                                        tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Luck); //(float)result
                                }
                            }
                        }
                    }
                }
            };
        }

        public void VSwingSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Melee); //V Swing
            special.MinRange = 0;//16;
            special.MaxRange = 99; //52
            special.CanMove = true;
            for (int i = 43; i < 45; i++)
            {
                AddNewSubAttackFrame(4, -1, -1, i);
            }
            AddNewSubAttackFrame(8, -1, -1, 45);
            special.CalculateAttackDamage = delegate (TerraGuardian tg)
            {
                return GetCalculatedSwordDamage(tg);
            };
            special.WhenSubAttackBegins = delegate (TerraGuardian tg)
            {
                SubAttackBegginingScript(tg);
            };
            special.WhenFrameUpdatesScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
                if (Frame == 1)
                {
                    CaptainStenchData data = (CaptainStenchData)tg.Data;
                    Rectangle AttackHitbox = new Rectangle(-16 * tg.Direction + (int)tg.Position.X, -110 + (int)tg.Position.Y, 78, 94);
                    if (tg.LookingLeft)
                        AttackHitbox.X -= AttackHitbox.Width;
                    int Damage = tg.SubAttackDamage;
                    int CriticalRate = 4 + tg.MeleeCriticalRate;
                    float Knockback = 6f;
                    byte SwordID = data.SwordID;
                    if (SwordID > 0)
                    {
                        switch (SwordID)
                        {
                            case AmethystFalchion:
                                break;
                            case TopazFalchion:
                                Knockback += 12f;
                                break;
                            case SapphireFalchion:
                                Knockback *= 0.11f;
                                break;
                            case EmeraldFalchion:
                                CriticalRate += 50;
                                break;
                        }
                    }
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(AttackHitbox))
                        {
                            if (!Main.npc[n].dontTakeDamage)
                            {
                                int HitDirection = tg.Direction;
                                if ((HitDirection == -1 && tg.Position.X < Main.npc[n].Center.X) ||
                                    (HitDirection == 1 && tg.Position.X > Main.npc[n].Center.X))
                                {
                                    HitDirection *= -1;
                                }
                                bool Critical = (Main.rand.Next(100) < CriticalRate);
                                int NewDamage = Damage;
                                if (tg.OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
                                {
                                    float DamageMult = Main.player[tg.OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                                    NewDamage = (int)(NewDamage * DamageMult);
                                }
                                double result = Main.npc[n].StrikeNPC(NewDamage, Knockback, HitDirection, Critical);
                                if (result > 0)
                                {
                                    if (SwordID == AmethystFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.4)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.ShadowFlame, 10 * 60);
                                    }
                                    else if (SwordID == RubyFalchion)
                                    {
                                        int HealthRecover = (int)(Math.Max(1, result * 0.05f));
                                        tg.RestoreHP(HealthRecover);
                                    }
                                    else if (SwordID == DiamondFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.5)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.Dazed, 3 * 60);
                                    }
                                }
                                Main.PlaySound(Main.npc[n].HitSound, Main.npc[n].Center);
                                tg.AddNpcHit(n);
                                tg.IncreaseDamageStacker((int)result, Main.npc[n].lifeMax);
                                if (result > 0)
                                {
                                    if (tg.HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        tg.IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    tg.OnHitSomething(Main.npc[n]);
                                    tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (SwordID == AmethystFalchion)
                                        tg.AddSkillProgress((float)result * 0.15f, GuardianSkills.SkillTypes.Mysticism);
                                    if (Critical)
                                        tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Luck); //(float)result
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            };
        }

        public void GPSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Melee); //GP
            special.SetCooldown(15);
            special.MinRange = 0;
            special.MaxRange = 62;
            special.CalculateAttackDamage = delegate (TerraGuardian tg)
            {
                return (int)(GetCalculatedSwordDamage(tg));
            };
            const int AnimationTime = 6;
            special.CanMove = false;
            for (int i = 46; i < 56; i++)
            {
                AddNewSubAttackFrame(AnimationTime, i, i, i);
            }
            special.WhenSubAttackBegins = delegate (TerraGuardian tg)
            {
                SubAttackBegginingScript(tg);
            };
            special.WhenFrameUpdatesScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
            };
            special.WhenFrameBeginsScript = delegate (TerraGuardian tg, int Frame)
            {
                if (Frame == 5)
                {
                    CaptainStenchData data = (CaptainStenchData)tg.Data;
                    Rectangle AttackHitbox = new Rectangle((int)(-32 * tg.Direction * tg.Scale) + (int)tg.Position.X, (int)(-102 * tg.Scale+ tg.Position.Y), (int)(104 * tg.Scale), (int)(98 * tg.Scale));
                    if (tg.LookingLeft)
                        AttackHitbox.X -= AttackHitbox.Width;
                    int Damage = tg.SubAttackDamage;
                    int CriticalRate = 4 + tg.MeleeCriticalRate;
                    float Knockback = 6f;
                    byte SwordID = data.SwordID;
                    Main.PlaySound(2, tg.CenterPosition, 1);
                    if (SwordID > 0)
                    {
                        switch (SwordID)
                        {
                            case AmethystFalchion:
                                {
                                    Vector2 SpawnPos = tg.PositionWithOffset;
                                    SpawnPos.Y -= 39 * tg.Scale; //78
                                    int p = Projectile.NewProjectile(SpawnPos, new Vector2(16f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.AmethystGP>(),
                                        Damage, Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.PlaySound(2, tg.CenterPosition, 101);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                            case TopazFalchion:
                                {
                                    Knockback += 12f;
                                    for (int s = 0; s < 4; s++)
                                    {
                                        Vector2 ShardSpawnPosition = tg.PositionWithOffset;
                                        ShardSpawnPosition.X += Main.rand.Next((int)(tg.Width * -0.5f), (int)(tg.Width * 0.5f));
                                        ShardSpawnPosition.Y -= Main.rand.Next(8, tg.Height - 8);
                                        int p = Projectile.NewProjectile(ShardSpawnPosition, new Vector2(4f * tg.Direction, 0), 
                                            Terraria.ModLoader.ModContent.ProjectileType<Projectiles.TopazGP>(), (int)(Damage * 0.25f), Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                        Main.projectile[p].scale = tg.Scale;
                                        Main.projectile[p].netUpdate = true;
                                        Main.PlaySound(2, tg.CenterPosition, 101);
                                    }
                                }
                                break;
                            case SapphireFalchion:
                                {
                                    Knockback *= 0.11f;
                                    int p = Projectile.NewProjectile(tg.CenterPosition, new Vector2(8f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.SapphireGP>(),
                                        Damage, Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                    Main.PlaySound(2, tg.CenterPosition, 39);
                                }
                                break;
                            case EmeraldFalchion:
                                {
                                    CriticalRate += 50;
                                    Vector2 SpawnPosition = tg.PositionWithOffset;
                                    SpawnPosition.Y -= 40 * tg.Scale; //78
                                    int p = Projectile.NewProjectile(SpawnPosition, new Vector2(1f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.EmeraldGP>(),
                                        (int)(Damage * 0.25f), Knockback * 0.9f, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                            case DiamondFalchion:
                                {
                                    Damage += (int)(tg.MHP * 0.05f);
                                }
                                break;
                        }
                    }
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(AttackHitbox))
                        {
                            if (!Main.npc[n].dontTakeDamage)
                            {
                                int HitDirection = tg.Direction;
                                if ((HitDirection == -1 && tg.Position.X < Main.npc[n].Center.X) ||
                                    (HitDirection == 1 && tg.Position.X > Main.npc[n].Center.X))
                                {
                                    HitDirection *= -1;
                                }
                                bool Critical = (Main.rand.Next(100) < CriticalRate);
                                int NewDamage = Damage;
                                if (tg.OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
                                {
                                    float DamageMult = Main.player[tg.OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                                    NewDamage = (int)(NewDamage * DamageMult);
                                }
                                double result = Main.npc[n].StrikeNPC(NewDamage, Knockback, HitDirection, Critical);
                                if (result > 0)
                                {
                                    if (SwordID == AmethystFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.4)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.ShadowFlame, 10 * 60);
                                    }
                                    else if (SwordID == RubyFalchion)
                                    {
                                        float HealthRecover = 0.1f;
                                        Rectangle SweetSpotPosition = new Rectangle((int)(tg.Position.X + tg.Direction * (48 + 40) * tg.Scale), (int)(tg.CenterY - 40 * tg.Scale), (int)(32 * tg.Scale), (int)(32 * tg.Scale));
                                        if (tg.LookingLeft)
                                            SweetSpotPosition.X -= SweetSpotPosition.Width;
                                        if (Main.npc[n].getRect().Intersects(SweetSpotPosition))
                                        {
                                            HealthRecover = 0.5f;
                                            Main.PlaySound(1, tg.CenterPosition);
                                            for(int i = 0; i < 25; i++)
                                            {
                                                Dust.NewDust(Main.npc[n].position, Main.npc[n].width, Main.npc[n].height, Terraria.ID.DustID.Blood);
                                            }
                                        }
                                        if(HealthRecover * result >= 1)
                                            tg.RestoreHP((int)(HealthRecover * result));
                                        else
                                            tg.RestoreHP(1);
                                        tg.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.DrainingHealth>(), 60);
                                    }
                                    else if (SwordID == DiamondFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.5)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.Dazed, 3 * 60);
                                    }
                                }
                                Main.PlaySound(Main.npc[n].HitSound, Main.npc[n].Center);
                                tg.AddNpcHit(n);
                                tg.IncreaseDamageStacker((int)result, Main.npc[n].lifeMax);
                                if (result > 0)
                                {
                                    if (tg.HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        tg.IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    tg.OnHitSomething(Main.npc[n]);
                                    tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (SwordID == AmethystFalchion)
                                        tg.AddSkillProgress((float)result * 0.15f, GuardianSkills.SkillTypes.Mysticism);
                                    if (Critical)
                                        tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Luck); //(float)result
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            };
            special.WhenCompanionIsBeingDrawn = delegate (TerraGuardian tg, int Frame, int Time)
            {
                CaptainStenchData data = (CaptainStenchData)tg.Data;
                switch (data.SwordID)
                {
                    case RubyFalchion:
                        {
                            if(Frame >= 4)
                            {
                                int WhipFrame = Frame - 4;
                                Texture2D texture = sprites.GetExtraTexture(RubyGPTextureID);
                                if(WhipFrame >= 0 && WhipFrame < 6)
                                {
                                    Vector2 WhipPos = tg.CenterPosition - Main.screenPosition;
                                    WhipPos.X += 40 * tg.Direction;
                                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, WhipPos,
                                        new Rectangle(160 * WhipFrame, 0, 160, 160), Color.White, 0f, new Vector2(80, 80), tg.Scale, 
                                        (tg.LookingLeft ? SpriteEffects.FlipHorizontally: SpriteEffects.None));
                                    TerraGuardian.DrawFront.Add(gdd);
                                }
                                if (tg.HasBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.DrainingHealth>()))
                                {
                                    int SiphonFrame = Frame - 5;
                                    if(SiphonFrame >= 0 && SiphonFrame < 7)
                                    {
                                        GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, tg.Position - Main.screenPosition,
                                            new Rectangle(160 * SiphonFrame, 160, 160, 160), Color.White, 0f, new Vector2(80, 160), tg.Scale,
                                            (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                        TerraGuardian.DrawFront.Add(gdd);
                                    }
                                }
                            }
                        }
                        break;
                    case DiamondFalchion:
                        {
                            int FlashFrame = (int)(((Frame - 4) * AnimationTime + Time) * (1f / AnimationTime) * 0.5f);
                            if(FlashFrame >= 0 && FlashFrame < 8)
                            {
                                Texture2D texture = sprites.GetExtraTexture(DiamondGPTextureID);
                                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, tg.CenterPosition - Main.screenPosition,
                                    new Rectangle(200 * FlashFrame, 0, 200, 200), Color.White, 0f, new Vector2(100, 100), tg.Scale,
                                    (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                TerraGuardian.DrawFront.Add(gdd);
                                FlashFrame++;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, tg.CenterPosition - Main.screenPosition,
                                    new Rectangle(200 * FlashFrame, 200, 200, 200), Color.White, 0f, new Vector2(100, 100), tg.Scale,
                                    (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                TerraGuardian.DrawFront.Add(gdd);
                            }
                        }
                        break;
                }
            };
        }

        public void ArmBlasterSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Ranged); //Arm Blaster
            special.CanMove = true;
            special.ManaCost = 2;
            special.MinRange = 0;//100;
            special.MaxRange = 1000;
            AddNewSubAttackFrame(8, -1, 57, -1);
            special.CalculateAttackDamage = delegate (TerraGuardian tg)
            {
                int Damage = 5;
                if (tg.SelectedItem > -1)
                {
                    float Dps = (float)tg.Inventory[tg.SelectedItem].useAnimation / (float)tg.Inventory[tg.SelectedItem].useTime;
                    Damage += (int)(tg.Inventory[tg.SelectedItem].damage * Dps * 0.35f);
                }
                return (int)(Damage * tg.RangedDamageMultiplier);
            };
            special.WhenFrameBeginsScript = delegate (TerraGuardian tg, int FrameID)
            {
                //Shoot something
                Vector2 ProjectilePosition = Vector2.Zero;
                Vector2 AimPosition = tg.AimDirection.ToVector2() - tg.CenterPosition;
                float Angle = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(AimPosition.Y, AimPosition.X)));
                if (AimPosition.X < 0)
                    Angle = (float)Math.PI - Angle;
                int LeftArmFrame = 57;
                if(Angle < 30 * 0.017453f) //Middle
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 61;
                }
                else if (AimPosition.Y > 0) //Down
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 60;
                    else
                        LeftArmFrame = 58;
                }
                else //Up
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 62;
                    else
                        LeftArmFrame = 59;
                }
                switch (LeftArmFrame)
                {
                    case 57:
                        ProjectilePosition = new Vector2(46, 43);
                        break;
                    case 58:
                        ProjectilePosition = new Vector2(46, 47);
                        break;
                    case 59:
                        ProjectilePosition = new Vector2(48, 34);
                        break;
                    case 60:
                        ProjectilePosition = new Vector2(42, 43);
                        break;
                    case 61:
                        ProjectilePosition = new Vector2(46, 37);
                        break;
                    case 62:
                        ProjectilePosition = new Vector2(45, 30);
                        break;
                }
                ProjectilePosition *= 2;
                ProjectilePosition.X = ProjectilePosition.X - SpriteWidth * 0.5f;
                if (tg.LookingLeft)
                    ProjectilePosition.X *= -1;
                ProjectilePosition.Y = -SpriteHeight + ProjectilePosition.Y;
                ProjectilePosition = tg.PositionWithOffset + ProjectilePosition * tg.Scale;
                AimPosition.Normalize();
                for (int i = 0; i < 4; i++)
                    Dust.NewDust(ProjectilePosition, 2, 2, 132, AimPosition.X, AimPosition.Y);
                int Damage = tg.SubAttackDamage;
                int ID = Projectile.NewProjectile(ProjectilePosition, AimPosition * 14f, Terraria.ModLoader.ModContent.ProjectileType<Projectiles.CannonBlast>(),
                    Damage, 0.03f, tg.GetSomeoneToSpawnProjectileFor);
                Main.PlaySound(2, tg.CenterPosition, 20);
                Main.projectile[ID].scale = tg.Scale;
                Main.projectile[ID].netUpdate = true;
                tg.SetProjectileOwnership(ID);
            };
            special.AnimationReplacer = delegate (TerraGuardian tg, int FrameID, int FrameTime, ref int BodyFrame, ref int LeftArmFrame, ref int RightArmFrame)
            {
                Vector2 AimPosition = tg.AimDirection.ToVector2() - tg.CenterPosition;
                float Angle = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(AimPosition.Y, AimPosition.X)));
                if (AimPosition.X < 0)
                    Angle = (float)Math.PI - Angle;
                LeftArmFrame = 57;
                if (Angle < 30 * 0.017453f) //Middle
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 61;
                }
                else if (AimPosition.Y > 0) //Down
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 60;
                    else
                        LeftArmFrame = 58;
                }
                else //Up
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 62;
                    else
                        LeftArmFrame = 59;
                }
            };
        }

        public override int GuardianSubAttackBehaviorAI(TerraGuardian Owner, CombatTactic tactic, Vector2 TargetPosition, Vector2 TargetVelocity, int TargetWidth, int TargetHeight,
            ref bool Approach, ref bool Retreat, ref bool Jump, ref bool Couch, out bool DefaultBehavior)
        {
            int ID = -1;
            float Distance = Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Owner.Position.X),
                DistanceYTargetTop = Owner.Position.Y - TargetPosition.Y,
                DistanceYTargetBottom = Owner.Position.Y - TargetPosition.Y + TargetHeight;
            DefaultBehavior = false;
            switch (tactic)
            {
                case CombatTactic.Charge:
                    if (Distance > 52 + TargetWidth * 0.5f)
                    {
                        Approach = true;
                        Retreat = false;
                    }
                    else if(Distance < 38 + TargetWidth * 0.5f)
                    {
                        Approach = false;
                        Retreat = true;
                    }
                    break;
                case CombatTactic.Assist:
                    if (Distance < 52 + TargetWidth * 0.5f)
                    {
                        Retreat = true;
                        Approach = false;
                    }
                    else
                    {
                        Approach = true;
                        Retreat = false;
                    }
                    break;
                case CombatTactic.Snipe:
                    if(Distance < 280)
                    {
                        Retreat = true;
                        Approach = false;
                    }
                    else if (Distance > 320)
                    {
                        Retreat = false;
                        Approach = true;
                    }
                    break;
            }
            CaptainStenchData data = (CaptainStenchData)Owner.Data;
            //if (DistanceYLower <= Owner.Height && (Owner.Velocity.Y == 0 || Owner.JumpHeight > 0))
            //    Jump = true;
            if (data.DeviceID > 0 && !Owner.SubAttackInCooldown(3) && Distance < 40 * 6 + TargetWidth * 0.5f && DistanceYTargetBottom < Owner.Height && DistanceYTargetTop <= TargetHeight)
            {
                ID = 3;
            }
            else if (!Owner.SubAttackInCooldown(1) && Distance < 62 + TargetWidth * 0.5f && DistanceYTargetBottom >= -98 && DistanceYTargetTop >= -18)
            {
                ID = 1;
            }
            else if (Distance < 52 + TargetWidth * 0.5f && DistanceYTargetBottom >= -90 && DistanceYTargetTop >= -4)//DistanceYLower <= 90 && DistanceYUpper <= 4)
            {
                ID = 0;
            }
            else
            {
                ID = 2;
            }
            //if(Owner.Velocity.Y == 0)Main.NewText("DistanceX: "+Distance+" DistanceY: " + DistanceYTargetTop + "~" + DistanceYTargetBottom);
            return ID;
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new CaptainStenchData(ID, ModID);
        }

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, SpriteEffects seffect, ref List<GuardianDrawData> gdds)
        {
            if (guardian.LookingLeft)
            {
                foreach (GuardianDrawData gdd in gdds)
                {
                    if (gdd.textureType == GuardianDrawData.TextureType.TGHead)
                    {
                        gdd.Texture = sprites.GetExtraTexture(HeadNoScouterTextureID);
                    }
                }
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            CaptainStenchData data = (CaptainStenchData)guardian.Data;
            if (guardian.IsAttackingSomething)
                data.HoldingWeaponTime = 5 * 60;
            else if(data.HoldingWeaponTime > 0)
            {
                data.HoldingWeaponTime--;
            }
            if(data.DeviceID > 0)
            {
                if(guardian.SubAttackInCooldown(PhantomRushSubAttack))
                {
                    if(data.PhantomDeviceUseTimes < data.DeviceID)
                    {
                        data.PhantomDeviceUseTimes++;
                        if(data.PhantomDeviceUseTimes < data.DeviceID)
                           guardian.ResetSubAttackCooldown(PhantomRushSubAttack);
                    }
                    else if(data.DeviceID == 5 && data.PhantomDeviceUseTimes == 5)
                    {
                        data.PhantomDeviceUseTimes = 6;
                        guardian.ChangeSubAttackCooldown(PhantomRushSubAttack, 5 * 60);
                    }
                }
                else
                {
                    if(data.PhantomDeviceUseTimes >= data.DeviceID)
                        data.PhantomDeviceUseTimes = 0;
                }
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            CaptainStenchData data = (CaptainStenchData)guardian.Data;
            bool UsingWeapon = data.HoldingWeaponTime > 0;
            if(guardian.Velocity.Y > 0)
            {
                Frame++;
            }
            else if (guardian.Velocity.X != 0 && guardian.Velocity.Y == 0 && guardian.DashCooldown < 0)
            {
                Frame += 25;
            }
            if (UsingWeapon)
            {
                if (Frame == 0)
                    Frame++;
                if (Frame >= 2 && Frame < 10)
                    Frame += 8;
                if (Frame == 18 || Frame == 19)
                    Frame += 2;
                if (Frame >= 27 && Frame < 35)
                    Frame += 8;
            }
            else
            {
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            CaptainStenchData data = (CaptainStenchData)guardian.Data;
            bool UsingWeapon = data.HoldingWeaponTime > 0;
            for (int i = TerraGuardian.DrawBehind.Count - 1; i >= 0; i--)
            {
                const bool DrawnBehind = true;
                switch (TerraGuardian.DrawBehind[i].textureType)
                {
                    //case GuardianDrawData.TextureType.TGRightArm:
                    case GuardianDrawData.TextureType.TGRightArmFront:
                        {
                            if (UsingWeapon && !guardian.Downed)
                                PlaceSwordSpriteAt(i, DrawnBehind, data.SwordID, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        {
                            if (!guardian.LookingLeft)
                            {
                                PlaceScouterSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                            }
                            if (guardian.BodyAnimationFrame >= 63 && guardian.BodyAnimationFrame < 72)
                                PlacePhantomBlinkSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                }
            }
            for (int i = TerraGuardian.DrawFront.Count - 1; i >= 0; i--)
            {
                const bool DrawnBehind = false;
                switch (TerraGuardian.DrawFront[i].textureType)
                {
                    //case GuardianDrawData.TextureType.TGRightArm:
                    case GuardianDrawData.TextureType.TGRightArmFront:
                        {
                            if (UsingWeapon && !guardian.Downed)
                                PlaceSwordSpriteAt(i, DrawnBehind, data.SwordID, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        {
                            if (!guardian.LookingLeft)
                            {
                                PlaceScouterSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                            }
                            if (guardian.BodyAnimationFrame >= 63 && guardian.BodyAnimationFrame < 71)
                                PlacePhantomBlinkSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                }
            }
        }

        public void PlacePhantomBlinkSpriteAt(int Position, bool DrawBehind, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Position++;
            int Frame = guardian.BodyAnimationFrame - 63;
            if (Frame >= 8)
                return;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(PhantomBlinkTextureID), DrawPosition, new Rectangle(Frame * SpriteWidth, 0, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public void PlaceScouterSpriteAt(int Position, bool DrawBehind, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Position++;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(ScouterTextureID), DrawPosition, guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public void PlaceSwordSpriteAt(int Position, bool DrawBehind, int SwordID, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            int FrameX = 0, FrameY = 0;
            switch (guardian.RightArmAnimationFrame)
            {
                case 25: //Downed
                case 26: //Reviving
                case 75: //Chair
                case 73: //Throne
                case 74: //Bed
                    return;
                case 2:
                    FrameX = 0;
                    break;
                case 10:
                    FrameX = 1;
                    break;
                case 11:
                    FrameX = 2;
                    break;
                case 12:
                    FrameX = 3;
                    break;
                case 13:
                    FrameX = 4;
                    break;
                case 14:
                    FrameX = 5;
                    break;
                case 15:
                    FrameX = 6;
                    break;
                case 16:
                    FrameX = 7;
                    break;
                case 17:
                    FrameX = 8;
                    break;
                case 20:
                    FrameX = 9;
                    break;
                case 21:
                    FrameX = 10;
                    break;
                case 35:
                    FrameX = 11;
                    break;
                case 36:
                    FrameX = 12;
                    break;
                case 37:
                    FrameX = 13;
                    break;
                case 38:
                    FrameX = 14;
                    break;
                case 39:
                    FrameX = 15;
                    break;
                case 40:
                    FrameX = 16;
                    break;
                case 41:
                    FrameX = 17;
                    break;
                case 42:
                    FrameX = 18;
                    break;
                    //
                case 43:
                    FrameX = 19;
                    break;
                case 44:
                    FrameX = 20;
                    break;
                case 45:
                    FrameX = 21;
                    break;
                    //
                case 46:
                    FrameX = 22;
                    break;
                case 47:
                    FrameX = 23;
                    break;
                case 48:
                    FrameX = 24;
                    break;
                case 49:
                    FrameX = 25;
                    break;
                case 50:
                    FrameX = 26;
                    break;
                case 51:
                    FrameX = 27;
                    break;
                case 52:
                    FrameX = 28;
                    break;
                case 53:
                    FrameX = 29;
                    break;
                case 54:
                    FrameX = 30;
                    break;
                case 55:
                    FrameX = 31;
                    break;
                case 56:
                    FrameX = 32;
                    break;
                //
                case 63:
                    FrameX = 33;
                    break;
                case 64:
                    FrameX = 34;
                    break;
                case 65:
                    FrameX = 35;
                    break;
                case 66:
                    FrameX = 36;
                    break;
                case 67:
                    FrameX = 37;
                    break;
                case 68:
                    FrameX = 38;
                    break;
                case 69:
                    FrameX = 39;
                    break;
                case 70:
                    FrameX = 40;
                    break;
                case 71:
                    FrameX = 41;
                    break;
                case 72:
                    FrameX = 42;
                    break;
            }
            if(FrameX >= FramesInRows)
            {
                FrameY += FrameX / FramesInRows;
                FrameX -= FrameY * FramesInRows;
            }
            //FrameY += ((NumberOfSwords - 1) - SwordID) * 2;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(PlasmaFalchionTextureID + SwordID), DrawPosition, new Rectangle(FrameX * SpriteWidth, FrameY * SpriteHeight, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Before coming to this planet i was a space pirate captain, going around taking things from other worlds.");
            Mes.Add("Unfortunately I am the only one who survived the crash of my ship. My cadets won't get to enjoy the treasures I will find.");
            CaptainStenchData data = (CaptainStenchData)guardian.Data;
            if(data.SwordID == 0)
                Mes.Add("My plasma falchion seems to have a empty socket for a gemstone to fit in wonder what it would do?");
            Mes.Add("I wonder if amber can be used as a infusion?......Probably not since it contains fossilized creatures, I will need to test it in the future ");
            Mes.Add("Shredding threw enemies gets me excited knowing they will drop loot");
            Mes.Add("I guess im retired from space travel as my only family is dead now. This world will be my last plunder land, I'll be sure to make it enjoyable.");
            Mes.Add("The weapons I use were stolen of course but I take what I want. That isn't to say I steal out of malicious intent, I just get it how I live ya'know? some people are just collateral damage to my pillage.");
            Mes.Add("As a young gal I never had nothing, thieving has been ingrained in me since childhood");
            Mes.Add("If you ever come across any gemstones make sure to share them with me. I want to find out the true power of my blade.");
            if(data.PhantomDeviceMiniquestProgress == -1)
                Mes.Add("My phantom device has been broken, if you have any spare platinum/gold bars you could help fix it.");
            bool AnyMetal = false, AnyGemstone = false;
            if (Main.screenTileCounts[Terraria.ID.TileID.Topaz] > 0 ||
                Main.screenTileCounts[Terraria.ID.TileID.Amethyst] > 0 ||
                Main.screenTileCounts[Terraria.ID.TileID.Sapphire] > 0 ||
                Main.screenTileCounts[Terraria.ID.TileID.Emerald] > 0 ||
                Main.screenTileCounts[Terraria.ID.TileID.Ruby] > 0 ||
                Main.screenTileCounts[Terraria.ID.TileID.Diamond] > 0 ||
                Main.screenTileCounts[Terraria.ID.TileID.AmberGemspark] > 0)
            {
                AnyGemstone = true;
            }
            else
            {
                for (int i = 0; i < Main.screenTileCounts.Length; i++)
                {
                    if (Terraria.ID.TileID.Sets.Ore[i] && Main.screenTileCounts[i] > 0)
                    {
                        AnyMetal = true;
                        break;
                    }
                }
            }
            if(AnyGemstone || AnyMetal)
            {
                Mes.Add("My scouter is picking up high volumes of rare materials here just keep up your mining it will be all ours soon enough.");
            }
            for(int n = 0; n < 200; n++)
            {
                if(Main.npc[n].active && Main.npc[n].damage > 0 && !Main.npc[n].friendly)
                {
                    Mes.Add("My scouter is showing a hostile entities, watch out for a ambush.");
                    break;
                }
            }
            if(Main.screenTileCounts[Terraria.ID.TileID.Traps] > 0 || Main.screenTileCounts[Terraria.ID.TileID.GeyserTrap] > 0)
            {
                Mes.Add("My scouter is picking up danger signals in this area watch out for traps.");
            }
            if(PlayerMod.HasGuardianSummoned(player, Rococo))
            {
                Mes.Add("Even though the racoon is a weird one, he sure does have some combat skill.");
                Mes.Add("I cant understand what the racoon is saying can you translate it for me, yeah?");
            }
            if(PlayerMod.HasGuardianSummoned(player, Blue))
            {
                Mes.Add("Blue reminds me of one of my cadets that died in the crash....Red his name was....ironic.. and he was a wolf....");
            }
            if(PlayerMod.HasGuardianSummoned(player, Zacks))
            {
                Mes.Add("Zacks reminds me of one of my cadets that died in the crash...Red his name was...kinda ironic since he was a wolf also");
            }
            if(PlayerMod.HasGuardianSummoned(player, Bree) && PlayerMod.GetPlayerGuardian(player, Bree).SkinID != Creatures.BreeBase.BaglessSkinID)
            {
                Mes.Add("I wonder whats in the white cats bag...Something valuable probably...");
            }
            if(PlayerMod.HasGuardianSummoned(player, Domino))
            {
                Mes.Add("This mouse looks like he might sell some black market supplies.");
            }
            if(PlayerMod.HasGuardianSummoned(player, Brutus))
            {
                Mes.Add("This guy reminds me of space patrol, i have been dodging them for years now and will do so again if he tries something.");
            }
            if(PlayerMod.PlayerHasGuardianSummoned(player, Alex) || PlayerMod.PlayerHasGuardianSummoned(player, Alexander) || PlayerMod.PlayerHasGuardianSummoned(player, Daphne))
            {
                Mes.Add("Tell this mutt to stop sniffing me! i know i dont smell the best but its annoying feeling wet noses touch my tail.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            if (IsPlayer)
            {
                return "Oh no! don't end up like my cadets we have so much more treasure to find!";
            }
            return base.ReviveMessage(Guardian, IsPlayer, RevivePlayer, ReviveGuardian);
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.ReviveByOthersHelp:
                    return "Once more im in debt to you I hope my performance in combat wont be this lackluster next time...";
                case MessageIDs.RevivedByRecovery:
                    return "A helping hand would have been useful you know....";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public class CaptainStenchData : GuardianData
        {
            public int HoldingWeaponTime = 0;
            public byte SwordID = 0, DeviceID = 0;
            public byte PhantomDeviceUseTimes = 0;
            public sbyte PhantomDeviceMiniquestProgress = -1;

            public CaptainStenchData(int ID, string ModID) : base(ID, ModID)
            {

            }

            public override void SaveCustom(TagCompound tag, int UniqueID)
            {
                tag.Add(UniqueID + "_SmellySwordID", SwordID);
                tag.Add(UniqueID + "_SmellyDeviceQuestStep", (int)PhantomDeviceMiniquestProgress);
            }

            public override void LoadCustom(TagCompound tag, int ModVersion, int UniqueID)
            {
                if (ModVersion > 85)
                {
                    SwordID = tag.GetByte(UniqueID + "_SmellySwordID");
                    PhantomDeviceMiniquestProgress = (sbyte)tag.GetInt(UniqueID + "_SmellyDeviceQuestStep");
                }
            }
        }
    }
}
