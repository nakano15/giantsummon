using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class WrathBase : PigGuardianFragmentBase
    {
        public WrathBase() //I'll need to think how I'll make the cloud form of them work, and toggle.
            : base(AngerPigGuardianID)
        {
            Name = "Wrath";
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very volatile.";
            Size = GuardianSize.Medium;
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 68;
            FramesInRows = 28;
            //DuckingHeight = 54;
            Age = 11;
            Male = true;
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
            SetTerraGuardian();

            MountUnlockLevel = 255;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = 14;
            ChairSittingFrame = 14;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 15 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 18;
            DownedFrame = 15;
            ReviveFrame = 18;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(15, 24);

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(1, 0);
            RightArmFrontFrameSwap.Add(2, 1);
            RightArmFrontFrameSwap.Add(3, 2);
            RightArmFrontFrameSwap.Add(4, 2);
            RightArmFrontFrameSwap.Add(5, 1);
            RightArmFrontFrameSwap.Add(6, 0);
            RightArmFrontFrameSwap.Add(7, 0);
            RightArmFrontFrameSwap.Add(8, 0);
            //RightArmFrontFrameSwap.Add(9, 0);
            //RightArmFrontFrameSwap.Add(10, 0);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(22, 1);
            BodyFrontFrameSwap.Add(23, 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 3);
            LeftHandPoints.AddFramePoint2x(11, 22, 9);
            LeftHandPoints.AddFramePoint2x(12, 23, 17);
            LeftHandPoints.AddFramePoint2x(13, 22, 20);

            LeftHandPoints.AddFramePoint2x(17, 24, 26);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 3);
            RightHandPoints.AddFramePoint2x(11, 25, 9);
            RightHandPoints.AddFramePoint2x(12, 26, 17);
            RightHandPoints.AddFramePoint2x(13, 23, 20);

            RightHandPoints.AddFramePoint2x(17, 26, 26);

            GetRequests();
        }

        public void GetRequests()
        {
            AddNewRequest("Solidification", 350, 
                "*This form is revolting, I'm boiling out of rage due to this. There must be a way of making me solid again, maybe that nerdy guy can help me in this, let's talk to him.*",
                "*Great, or else I would give you a beating.*",
                "*Oh you...! (Insert several different insults here)*",
                "*What are you waiting for?! I'M SICK OF BEING A CLOUD YOU FOOL!*");
            AddRequestRequirement(delegate(Player player)
            {
                return PlayerMod.PlayerHasGuardian(player, Leopold) && !player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs;
            });
            AddTalkToGuardianRequest("*Hm... Actually, I do know about his condition. It seems like his body has vaporized at the moment his personality was split. I can try doing something to make his personality solid, but I can only merge his personalities if you find them.*", Leopold);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.05f;
            g.Defense -= 4;
            if (GetIfIsCloudForm(g))
            {
                //if (!g.HasFlag(GuardianFlags.NoGravity))
                //    g.AddFlag(GuardianFlags.NoGravity);
                //if (!g.HasFlag(GuardianFlags.NoTileCollision))
                //    g.AddFlag(GuardianFlags.NoTileCollision);
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm so furious, why I'm furious? I don't know! That's what makes me more furious!*");
            Mes.Add("*Grrr. GRRRRR!! Grrrrrrrrrrr!*");
            Mes.Add("*Who are you?! What?! Something's funny with my face?! Want to taste my punch?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm so angry, actually I'm FURIOUS!*");
            Mes.Add("*Stay away! I'm not in the mood.*");
            Mes.Add("*Grrr! I'm so furious!*");
            Mes.Add("*Whaaaaaaaat?!*");
            Mes.Add("*I'm trying my best to be less angry, but I can't!*");
            if (player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID])
            {
                Mes.Add("*Don't you dare doing any joke about my current form. DON'T. YOU. DARE!*");
                Mes.Add("*I really hate being a cloud, and don't you dare breath near me.*");
            }
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Perfect! I can discount my rage on them!*");
                    Mes.Add("*Bring them on! I'll take care of them!*");
                }
                else
                {
                    Mes.Add("*That lush green grass and the bird chirping sounds is driving me nuts!*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*What?! You thought I would be more angry this night? Get lost!*");
                    Mes.Add("*Maybe beating them very hard will make me less angry!*");
                }
                else
                {
                    Mes.Add("*Urgh! All those \"Grahs\" during the night are infuriating me! I'm nearly going outside and shutting them up!*");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*Don't you have anything else to do? Go away!*");
                Mes.Add("*Want me to put your head in the toilet and then push the flush? Then GO AWAY!*");
            }
            if (Main.raining)
            {
                Mes.Add("*Great, It couldn't get worse, right?*");
                Mes.Add("*I tried! Even being in the rain wont make me less furious!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Alex))
                Mes.Add("*GO AWAY! Oh, my bad. I thought It was [gn:"+GuardianBase.Alex+"] wanting to play.*");
            if (NpcMod.HasGuardianNPC(GuardianBase.Brutus))
                Mes.Add("My rage wont lower! I even asked [gn:"+GuardianBase.Brutus+"] if he would let me beat him to try lowering my rage, and It didn't work!");
            if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
            {
                Mes.Add("How many times I have to tell you, that I'M NOT GOING TO PARTICIPATE OF ANY... Oh.. My bad, I thought you were [gn:"+GuardianBase.Malisha+"].");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
            {
                if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
                    Mes.Add("*It seems like the only way of lowering my rage, is if I get fused together with my other emotions. But where could they be?*");
                else
                    Mes.Add("*I wonder if the nerdy guy knows how I could get off this form.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Sardine))
            {
                Mes.Add("*[gn:" + Sardine + "] has the kind of thing that keeps me busy with, that's why I'm not punching random people here.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Domino))
            {
                Mes.Add("*If I ever see [gn:" + Domino + "] making another joke about me, I'll beat him so hard that he'll need plastic surgery!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Vladimir))
            {
                Mes.Add("The hugs [gn:"+GuardianBase.Vladimir+"] gives doesn't works either! I can't get less angry with them!");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Sometimes I shake out of rage. If you ever see me in that state, don't get close.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm too furious to try doing this right now. Could you do It instead?*");
            Mes.Add("*Grrr!! There is something I should do that is making me furious, but I can't do that myself. Would you do It?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*NO!*");
            Mes.Add("*I don't! There isn't anything!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Good. I can't feel happy about this, so take this as a... Friendly rage.*");
            Mes.Add("*Okay, I wont hurt you for a few hours, is that a good reward?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*No, I'm not happy, I'm still angry, you know.*");
            Mes.Add("*All those people dancing around, meanwhile I'm here, this is making me so furious.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, Wrath))
                Mes.Add("*I hope the gift is good.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*Come on, wake up!*");
                Mes.Add("*I didn't agree to baby sit you.*");
                Mes.Add("*I hope you don't die on me, I will be very angry with you if you do so.*");
            }
            else
            {
                Mes.Add("*Hey, get up, now!*");
                Mes.Add("*I hope you aren't doing that on purpose.*");
                Mes.Add("*This is already getting me furious.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
