using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class ZacksBase : GuardianBase
    {
        public const string OldHeadID = "oldhead", OldBodyID = "oldbody", OldLeftArmID = "oldleftarm";
        public const byte OldBodyTextureID = 1;

        /// <summary>
        /// -Blue's Boyfriend.
        /// -Doesn't fears cheating on his relationship with Blue.
        /// -Fears being left behind.
        /// -Pal for any time.
        /// -Likes messing with Sardine.
        /// -Wants to play with Alex.
        /// -Tries very hard not to go out of control during Bloodmoons.
        /// </summary>

        public ZacksBase()
        {
            Name = "Zacks";
            Description = "He didn't used to be a zombie, but something happened.\nHe's also Blue's boyfriend.";
            Size = GuardianSize.Large;
            Width = 30;
            Height = 86;
            //DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 16;
            Male = true;
            InitialMHP = 185; //1275
            LifeCrystalHPBonus = 50;
            LifeFruitHPBonus = 17;
            Accuracy = 0.32f;
            Mass = 0.5f;
            MaxSpeed = 3.9f;
            Acceleration = 0.12f;
            SlowDown = 0.52f;
            MaxJumpHeight = 17;
            JumpSpeed = 6.15f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            IsNocturnal = false;
            SetTerraGuardian();
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit1);
            DeadSound = new SoundData(Terraria.ID.SoundID.ZombieMoan);
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 10, 11, 12, 14, 15 });
            CallUnlockLevel = 2;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 2;

            AddInitialItem(Terraria.ID.ItemID.BloodButcherer, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            PlayerMountedArmAnimation = 10;
            HeavySwingFrames = new int[] { 11, 12, 13 };
            ItemUseFrames = new int[] { 14, 15, 16, 17 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 18;
            ChairSittingFrame = 19;
            ThroneSittingFrame = 20;
            BedSleepingFrame = 21;
            SleepingOffset.X = 16;
            ReviveFrame = 22;
            DownedFrame = 23;
            PetrifiedFrame = 24;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(18, 0);

            //Left Hand
            LeftHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(17 * 2, 32 * 2);
            LeftHandPoints.AddFramePoint2x(11, 27, 2);
            LeftHandPoints.AddFramePoint2x(12, 38, 15);
            LeftHandPoints.AddFramePoint2x(13, 40, 37);

            LeftHandPoints.AddFramePoint2x(14, 21, 2);
            LeftHandPoints.AddFramePoint2x(15, 33, 12);
            LeftHandPoints.AddFramePoint2x(16, 37, 22);
            LeftHandPoints.AddFramePoint2x(17, 32, 30);

            LeftHandPoints.AddFramePoint2x(22, 38, 43);

            //Right Hand
            RightHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(30 * 2, 32 * 2);
            RightHandPoints.AddFramePoint2x(11, 30, 2);
            RightHandPoints.AddFramePoint2x(12, 41, 15);
            RightHandPoints.AddFramePoint2x(13, 42, 37);

            RightHandPoints.AddFramePoint2x(14, 23, 2);
            RightHandPoints.AddFramePoint2x(15, 36, 12);
            RightHandPoints.AddFramePoint2x(16, 39, 22);
            RightHandPoints.AddFramePoint2x(17, 35, 30);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(16 * 2, 13 * 2);
            MountShoulderPoints.AddFramePoint2x(12, 24, 19);
            MountShoulderPoints.AddFramePoint2x(13, 27, 25);

            //Sitting Position
            SittingPoint = new Point(22 * 2, 36 * 2);

            //Armor Head Points
            HeadVanityPosition.DefaultCoordinate2x = new Point(21, 10);
            HeadVanityPosition.AddFramePoint2x(12, 30 - 2, 14 + 2);
            HeadVanityPosition.AddFramePoint2x(13, 33 - 2, 21 + 2);

            HeadVanityPosition.AddFramePoint2x(20, 21 + 1, 8);

            HeadVanityPosition.AddFramePoint2x(22, 35, 39);

            //Wing
            WingPosition.DefaultCoordinate2x = new Point(22, 21);

            //Skins
            AddSkin(OldBodyTextureID, "Old Body", delegate(GuardianData gd, Player pl) { return true; });
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(OldHeadID, "head_old");
            sprites.AddExtraTexture(OldBodyID, "body_old");
            sprites.AddExtraTexture(OldLeftArmID, "left_arm_old");
        }

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect, ref List<GuardianDrawData> gdd)
        {
            if (guardian.SkinID == OldBodyTextureID)
            {
                foreach (GuardianDrawData gdd2 in gdd)
                {
                    ReplaceTexturesForOldTexture(gdd2);
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            if (guardian.SkinID == OldBodyTextureID)
            {
                foreach (GuardianDrawData gdd2 in TerraGuardian.DrawFront)
                {
                    ReplaceTexturesForOldTexture(gdd2);
                }
                foreach (GuardianDrawData gdd2 in TerraGuardian.DrawBehind)
                {
                    ReplaceTexturesForOldTexture(gdd2);
                }
            }
        }

        public void ReplaceTexturesForOldTexture(GuardianDrawData gdd)
        {
            switch (gdd.textureType)
            {
                case GuardianDrawData.TextureType.TGHead:
                    gdd.Texture = sprites.GetExtraTexture(OldHeadID);
                    break;
                case GuardianDrawData.TextureType.TGBody:
                    gdd.Texture = sprites.GetExtraTexture(OldBodyID);
                    break;
                case GuardianDrawData.TextureType.TGLeftArm:
                    gdd.Texture = sprites.GetExtraTexture(OldLeftArmID);
                    break;
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (guardian.BodyAnimationFrame == ThroneSittingFrame && guardian.LookingLeft)
                guardian.FaceDirection(false);
            if (guardian.Wet && !guardian.HasFlag(GuardianFlags.BreathUnderwater) && !guardian.HasFlag(GuardianFlags.Merfolk))
            {
                guardian.BreathCooldown += 2;
            }
            if (!guardian.DoAction.InUse)
            {
                if (Main.moonPhase == 0 && !Main.bloodMoon && Main.time >= 3600)
                {
                    if (guardian.OwnerPos == -1 || guardian.HasPlayerAFK)
                    {
                        guardian.StartNewGuardianAction(FullMoonBehaviorID);
                    }
                }
            }
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "*I... Want to be able to help you... Not be a burden... Take me on your adventures whenever you can...*";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*You know, you will be safer If I carry you on my shoulder. At least I don't feel pain. Just... Plug your nose.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*You know, If you have any dangerous thing to do, send me to do it. I'm already dead, anyway.*";
            }
        }
        
        public override void Attributes(TerraGuardian g)
        {
            g.DefenseRate += 0.1f;
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
            if (g.KnockedOut)
            {
                g.AddFlag(GuardianFlags.CantBeHurt);
                g.AddFlag(GuardianFlags.DontTakeAggro);
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "*He's asking me who am I. A zombie, asking me, who am I. Not trying to eat me. What is happening to this world?*";
                case 1:
                    return "*The rotten corpse can't really ruin the distorted smile it gives for meeting me. Should I run away?*";
                case 2:
                    return "*What kind of zombie is that? At least isn't trying to maul or bite me.*";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] is saying that he's entire right now.*";
            return "*[name] said that his hunger is still on bearable levels.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] said that he needs some things for his personal collection.*";
            return "*[name] wants you to do something, but certainly isn't his final wish.*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] is so happy at you for doing his request that could die again. He then apologized for the bad pun.*";
            return "*[name] got the things I brought him and is trying to fake his joy with a neutral look.*";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            bool BlueInTheWorld = NpcMod.HasGuardianNPC(1), SardineInTheWorld = NpcMod.HasGuardianNPC(2);
            if (!Main.bloodMoon)
            {
                if (BlueInTheWorld)
                {
                    Mes.Add("*[name] said to not worry about your and your citizens safety, since he has an agreement with [gn:1] that he would not devour anyone in her presence.*");
                    if (player.head == 17)
                    {
                        Mes.Add("*[name] tells you to beware when using that hood. He says that you can end up being hugged by a known bunny lover.*");
                    }
                }
                else
                {
                    Mes.Add("*I still remember the agreement we made, I wont eat your citizens, or you. I promisse.*");
                }
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*[name] said that the sun doesn't do good on his rotten skin, so he preffers to stay on areas with shade.*");
                    Mes.Add("*[name] misses being alive, and feels bad about not being able to move his left leg.*");
                }
                else
                {
                    Mes.Add("*[name] said that he preffers the day like this, but with less monsters.*");
                    Mes.Add("*[name] is puzzled about what kind of creatures are those.*");
                }
            }
            else
            {
                if (!NPC.downedBoss1)
                    Mes.Add("*[name] questions himself if that Giant Eye gazing at us is edible. But is looking weird at me after I asked what Giant Eye.*");
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss)
                    Mes.Add("*[name] says that he will try a vegan meal. Is asking me when I will take him to the Jungle.*");
                if (!Main.bloodMoon)
                {
                    Mes.Add("*[name] is telling me that beside not feeling any kind of pain on his body, he feels an unending hunger that only eases by eating something.*");
                    Mes.Add("*[name] is asking if you will leave him behind, too.*");
                    Mes.Add("*[name] told me that he always feels hunger for flesh, It simply doesn't end.*");
                    Mes.Add("*[name] is asking if he could go outside... For... A walk?*");
                }
                else
                {
                    Mes.Add("*[name] is feeling very aggressive tonight, he says he would be able to eat something whole.*");
                    Mes.Add("*[name] is trying very hard to hold his hunger.*");
                    Mes.Add("*[name] said that this night remembers him of when he died. He don't want to share the horrors he passed through with you. That seems to make him very angry.*");
                }
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*[name] has got the thriller or something.*");
                Mes.Add("*What kind of dance moves are those? Seems like 90's ones?*");
                if (BlueInTheWorld)
                    Mes.Add("*[name] is dancing with [gn:1]. They look joyful when dancing.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("*[name] is telling me that [nn:" + Terraria.ID.NPCID.Merchant + "] asked if he had any unused skin he wanted to sell.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                Mes.Add("*[name] is telling me that [nn:" + Terraria.ID.NPCID.Nurse + "] asked if he wanted to donate some blood, if there is any useable blood left.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add("*[name] said that he went to [nn:" + Terraria.ID.NPCID.Stylist + "] and asked her to do some treatment with his fur, she then replied that can't do miracles. And recommended a taxidermist.*");
            }
            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("*[name] said that If [gn:0] comes annoy him again, he'll try kicking his behind.*");
                Mes.Add("*[name] is telling [gn:0] that if he comes back with any funny jokes again, he'll kick his behind.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*[name] feels bad when goes to talk with [gn:1], because she looks partially joyful and partially saddened.*");
                Mes.Add("*[name] is thinking about [gn:1], he wishes to be alive and entire again just to be with her.*");
            }
            else
            {
                Mes.Add("*[name] misses being with someone.*");
                if(Main.moonPhase == 0 && !Main.dayTime)
                    Mes.Add("*[name] is trying to howl at the moon, but his lungs were too damaged to be able to do that.*");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("*[name] is telling me that he's playing some game with [gn:2], should I start getting worried?*");
                Mes.Add("*[name] is saying that he's getting better at lasso with the help of [gn:2]. But I wonder, where did he got a rope. Wait...*");
                Mes.Add("*[name] asks if you are worried if [gn:2] will turn into a zombie, he told you that you should not worry, at least If he doesn't die, he'll be fine.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*[name] says that dislodged It's left knee when kicking [gn:0]'s behind, and is asking if you can help him put it into place.*");
                Mes.Add("*[name] seems to be paying attention to what [gn:0] is saying, but as soon as he said something stupid, the conversation ended.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*[name] told you to take real good care of [gn:1] on your adventures, or else he'll take care of you.*");
                Mes.Add("*[name] is telling you both to take care, since he doesn't want you and [gn:1] to be another Blood moon miniboss.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*[name] shows a distorted smile while looking in [gn:2] direction, making him back away slowly.*");
                Mes.Add("*[name] said that wants to play a game, and now [gn:2] is begging me that we should go, now.*");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*[name] says that really wanted to play with [gn:5], but his left leg doesn't really helps, so he always dismisses him.*");
                if (NpcMod.HasGuardianNPC(2))
                {
                    Mes.Add("*[name] says that ever since [gn:5] has arrived, he barelly were able to play with [gn:2].*");
                }
                if (NpcMod.HasGuardianNPC(1))
                    Mes.Add("*[name] says that [gn:1] haven't been playing with [gn:5] latelly. He wonders what happened.*");
            }
            if (NpcMod.HasGuardianNPC(7))
            {
                Mes.Add("*Do you know why everytime [gn:7] sees me, she only shows one emotion, spooked?*");
                Mes.Add("*I tried greeting [gn:7], she ran away yelling like as if she saw a zombie or something. Wait...*");
                if (NpcMod.HasGuardianNPC(2))
                    Mes.Add("*You say that [gn:2] is [gn:7]'s husband? I guess the fear they have of me is from their family?*");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("*I don't know if It's because I'm a wolf, a zombie, or if I'm male. But It gets quite hot when [gn:8] is around.*");
                Mes.Add("*[gn:8] said that heard a faint howling earlier? As if. I was practicing... Howling. I'm a wolf, after all.*");
            }
            if (NpcMod.HasGuardianNPC(9))
            {
                Mes.Add("*I hate [gn:" + Domino + "], he loves making bad jokes to people.*");
            }
            if (NpcMod.HasGuardianNPC(10))
            {
                Mes.Add("*If you worry about [gn:" + Leopold + "], don't worry, I wont eat him. But It is fun to make him panic.*");
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("*Sometimes I feel jealous about [gn:" + Leopold + "], [gn:"+Blue+"] hugs him way more than she does to me. But I don't really like the idea of spending hours around her arms too. So I guess I feel a bit of pity?*");
                }
            }
            if (NpcMod.HasGuardianNPC(Vladimir) && NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*I went earlier to ask [gn:"+Vladimir+"] why [gn:"+Blue+"] visits him so much... I didn't knew how much pain I cause to her... And how much joy I brought to her once I returned to her side...*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*Why [gn:" + Michelle + "] doesn't talk with me?*");
                Mes.Add("*I think [gn:" + Michelle + "] seems to be a cool person, but she always ignores me...*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*It's quite nice having a new girl on the town. No, I'm not cheating [gn:" + Blue + "] If that's what is on your mind.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*Go back before It's too late! Things here are dreadful even for me!*");
                Mes.Add("*You don't know what I'm passing through here.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Yes, I can share my room with you, I can't sleep at night, anyway.*");
                Mes.Add("*If you're worried about being devoured during the night, don't worry, I wont. I know how to search for food outside.*");
                Mes.Add("*There is not much I can do during the night. Either I watch the window, or you sleep. I think I saw you putting your thumb on your mouth, one night. Though.*");
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Blue))
            {
                Mes.Add("*So, you're sharing room with [gn:"+Blue+"], huh... I may wonder why she wouldn't want to share her room with me.*");
                Mes.Add("*Say.. You're sharing room with [gn:"+Blue+"], right? How's she?*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("*I'm sorry [nickname], I can't move. I think I pulled It up to It's limit.*");
                Mes.Add("*I'm paralized, I can't move at all.*");
                Mes.Add("*I can't move any part of my body.*");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("*I'm awaken. I can't sleep, ever since I turned into a zombie.*");
                Mes.Add("*There isn't much I can do while in the bed, so I only watch the ceiling, and expect something interesting to happen.*");
                Mes.Add("*There are all kinds of sounds that happens when people sleeps, you wouldn't believe If I told you them all.*");
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("*Since I stay up all night, the least I could do is make sure that [gn:" + Blue + "] sleep safe and sound.*");
                    Mes.Add("*It comforts me a bit to watch [gn:" + Blue + "] sleep, knowing that she's safe, and here with me.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*[name] is saying that just because he's a zombie he doesn't need to live outdoors.*");
            Mes.Add("*[name] doesn't want to be alone anymore, he wants a house with people to talk with.*");
            Mes.Add("*[name] told me that wont have the other villagers as a meal. Give him some place to live.*");
            Mes.Add("*[name] is saying that if you doesn't want to give him a house because of his smell and \"decay state\", there is nothing he can do about it, but It's not nice to be left in the wilderness.*");
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("*[name] says that he wants to be with [gn:1], so please build him a house next to her.*");
            if (Main.dayTime)
                Mes.Add("*[name] says that the sun is burning the rest of his skin, build a house for him.*");
            else
                Mes.Add("*[name] doesn't want to stay on this deplorable state anymore, give him some place to live.*");
            if (Main.raining)
                Mes.Add("*[name] is saying that the water is bad for his innards. Give him some place to live.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*[name] says that being unable to move his left leg hinders him a bit, but he'll try his best to follow you anyway.*");
            Mes.Add("*[name] is asking you if he should consider doing fishing as a hobby. He got the worms, anyway.*");
            Mes.Add("*[name] seems to be worried, trying to keep up with me, as if I was going to leave him behind.*");
            Mes.Add("*You asked [name] why zombies comes out from the floor, he answers that he doesn't knows, since he was never buried.*");
            Mes.Add("*[name] is asking you if will eventually find the Terrarian that left him to the zombies.*");
            if(PlayerMod.PlayerHasGuardian(player, GuardianBase.Sardine))
                Mes.Add("*I asked [name] what he uses to make his lasso. He told me that used his intestine for that. After knowing that, not only I got striked by an instant regret, but also think that shouldn't tell this to [gn:2].*");
            if (PlayerMod.HasGuardianSummoned(player, 3) && PlayerMod.GetPlayerSummonedGuardian(player, 3).Wet)
            {
                Mes.Add("*[name] is saying that there's water even where you wouldn't believe. And he preffers not to give details.*");
            }
            if (NpcMod.HasGuardianNPC(5) && NpcMod.HasGuardianNPC(1))
                Mes.Add("*[name] says that after knowing of " + NpcMod.GetGuardianNPCName(5) + "'s loss, he thinks he should take it easy with " + NpcMod.GetGuardianNPCName(1) + ", since she might have felt nearly the same.*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*[name] is asking me why [gn:0] is so annoying, looks like he's not even grown up.*");
                Mes.Add("*[name] is showing his fist to [gn:0], and said that It's reserved for the next time he comes with a funny zombie joke.*");
                if(NPC.AnyNPCs(Terraria.ID.NPCID.Merchant) && !PlayerMod.PlayerHasGuardianSummoned(player, 1) &&NpcMod.HasGuardianNPC(1))
                    Mes.Add("*[name] wonders if he could gift [gn:1], to try cheering her up.*");
            }
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*[name] started to be happy by seeying [gn:1], but once he saw the bits of despair in her face, he started to get saddened too.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*[name] is asking [gn:2] if he wants to play \"The Walking Guardian\", as soon as he said that, [gn:2] started to scream and run away.*");
            }
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                    Mes.Add("*[name] told me that the night he died, he were walking in the forest following a Terrarian, until a blood moon started. The Terrarian left him in the middle of the hordes of zombies.*");
                else
                    Mes.Add("*[name] told me that the night he died, he were walking in the forest following a Terrarian, until a blood moon started. The Terrarian left him in the middle of the hordes of zombies, whose resulted in his horrible death. He doesn't remember anything past that, other than coming to his senses and seeying yours and Blue's face.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*[name] says that he doesn't knows why [gn:1] never mentions this to anybody, but she really loves bunnies. Told me to try giving her one and thank him later.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 3) && Main.rand.Next(2) == 0)
                return "*[name] is saying that he's so excited wanting to know what you will give him as a gift, that he could... Oops, he couldn't hold any longer.*";
            return "*[name] is asking if in his current state, growing older is a good thing.*";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            bool IsBlue = !IsPlayer && ReviveGuardian.ModID == Guardian.ModID && ReviveGuardian.ID == GuardianBase.Blue;
            if (!IsBlue)
            {
                Mes.Add("*You look tasty when knocked out...*");
                Mes.Add("*Do you mind if I take a little bite...*");
                Mes.Add("*Don't worry buddy, you'll wake up soon.*");
                Mes.Add("*The quality of your blood is good.*");
            }
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID)
            {
                switch (ReviveGuardian.ID)
                {
                    case GuardianBase.Mabel:
                        Mes.Add("*Hmmmm... Venison...*");
                        Mes.Add("*I'd like to take a bite but... I'm already with someone else...*");
                        Mes.Add("*You're making me a bit uncomfortable.*");
                        break;
                    case GuardianBase.Blue:
                        Mes.Add("*How did this happened?!*");
                        Mes.Add("*"+ReviveGuardian.Name+"! "+ReviveGuardian.Name+"! Wake up! Talk to me!*");
                        Mes.Add("*I wont let you die "+ReviveGuardian.Name+"! I promisse you!*");
                        break;
                    case GuardianBase.Sardine:
                        Mes.Add("*"+ReviveGuardian.Name+", I'll eat you if you don't wake up. ... It's not fun when you're knocked out.*");
                        Mes.Add("*It's really odd to see you not being scared or running away... Please wake up soon...*");
                        Mes.Add("*Maybe If I pretend to be biting him will make him wake up faster?*");
                        break;
                    case GuardianBase.Alex:
                        Mes.Add("*I wont let you die too. You already had one grief.*");
                        Mes.Add("*Hang on buddy, your old owner can wait.*");
                        Mes.Add("*If I could have played with him while he was awake...*");
                        break;
                    case GuardianBase.Leopold:
                        Mes.Add("*I wonder the surprise he will have when he wakes up.*");
                        Mes.Add("*Maybe I should avoid healing him from behind, I don't want to receive an easter egg or something.*");
                        Mes.Add("*You're sleeping so peacefully... Wait until you wake up. Hehe....*");
                        Mes.Add("*I'll try showing my teeth right directly in front of his face. This should be fun when he wakes up.*");
                        break;
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
        {
            /*switch (trigger)
            {
                case TriggerTypes.PlayerDowned:
                    Player player = Main.player[Value];
                    if (!guardian.DoAction.InUse && !guardian.IsPlayerHostile(player))
                    {
                        guardian.StartNewGuardianAction(new ZacksPullSomeoneAction(player), PullSomeoneID);
                    }
                    return true;
                case TriggerTypes.GuardianDowned:
                    TerraGuardian tg = MainMod.ActiveGuardians[Value];
                    if (!guardian.DoAction.InUse && !guardian.IsGuardianHostile(tg))
                    {
                        guardian.StartNewGuardianAction(new ZacksPullSomeoneAction(tg), PullSomeoneID);
                    }
                    return true;
            }*/
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
        }

        public const int FullMoonBehaviorID = 0, PullSomeoneID = 1;

        public override void GuardianActionUpdate(TerraGuardian guardian, GuardianActions action)
        {
            if (!action.IsGuardianSpecificAction)
                return;
            switch (action.ID)
            {
                case FullMoonBehaviorID:
                    {
                        if (Main.dayTime || Main.time >= 28800)
                        {
                            action.InUse = false;
                            return;
                        }
                        if (guardian.OwnerPos > -1 && guardian.AfkCounter < 60)
                        {
                            action.InUse = false;
                            return;
                        }
                        if (guardian.IsAttackingSomething)
                            return;
                        const byte TimeVariableID = 0, BehaviorVariableID = 1;
                        int Time = action.GetIntegerValue(TimeVariableID),
                            Behavior = action.GetIntegerValue(BehaviorVariableID);
                        switch (Behavior)
                        {
                            case 0: //Wander
                                {
                                    if (Time <= 0)
                                    {
                                        Tile tile = Framing.GetTileSafely((int)guardian.Position.X / 16, (int)guardian.CenterPosition.Y / 16);
                                        if (tile.wall > 0)
                                        {
                                            Time = 400;
                                        }
                                        else if (guardian.HasDoorOpened)
                                        {
                                            Time = 50;
                                        }
                                        else
                                        {
                                            Behavior = 1;
                                            Time = 2000;
                                            break;
                                        }
                                    }
                                    if (guardian.OwnerPos == -1)
                                    {
                                        guardian.WalkMode = true;
                                        if (guardian.LookingLeft)
                                            guardian.MoveLeft = true;
                                        else
                                            guardian.MoveRight = true;
                                    }
                                    Time--;
                                }
                                break;
                            case 1: //Howl
                                {
                                    if (Time <= 0)
                                    {
                                        guardian.LookingLeft = Main.rand.NextDouble() < 0.5;
                                        Behavior = 0;
                                        Time = 400;
                                        break;
                                    }
                                    if (Time == 890 || Time == 1300 || Time == 1900)
                                    {
                                        guardian.SaySomething("Aw.. Aw... Woo...");
                                    }
                                    Time--;
                                }
                                break;
                        }
                        action.SetIntegerValue(TimeVariableID, Time);
                        action.SetIntegerValue(BehaviorVariableID, Behavior);
                    }
                    break;
            }
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "*Sorry pal, but you're not dying today.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*I was sick of lying down in the bed, anyway.*";
                        case 1:
                            return "*It's good to stretch the legs a bit.*";
                        case 2:
                            return "*The roof was well known for me, what is It that you want?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Oh yeah, I had a request for you, did you complete It?*";
                        case 1:
                            return "*I remember that I gave you a request. Is It done?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I can help you on your travels, just don't leave me behind.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I think you have way too many people with you.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Sorry, but I don't feel like I can follow you right now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*Are you sure? I don't really like this place. I can't really die but... Being incapacitated isn't cool.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Oh, okay. I'm going back home then.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Then I'll try returning home. Be sure to visit me safe and sound.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Great choice, [nickname].*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public class ZacksPullSomeoneAction : GuardianActions
        {
            public ZacksPullSomeoneAction(Player player)
            {
                Players.Add(player);
            }

            public ZacksPullSomeoneAction(TerraGuardian guardian)
            {
                Guardians.Add(guardian);
            }

            public Vector2 PullStartPosition = Vector2.Zero;
            public const int PullMaxTime = 45; //Like his miniboss version

            public override void Update(TerraGuardian Me)
            {
                if (Time >= PullMaxTime)
                {
                    if (Time == PullMaxTime)
                    {
                        if (Players.Count > 0)
                        {
                            PullStartPosition = Players[0].Center;
                        }
                        else if (Guardians.Count > 0)
                        {
                            PullStartPosition = Guardians[0].CenterPosition;
                        }
                    }
                    if (Players.Count > 0)
                    {
                        Player player = Players[0];
                        Vector2 MoveDirection = Me.CenterPosition - player.Center;
                        MoveDirection.Normalize();
                        Me.LookingLeft = player.Center.X < Me.Position.X;
                        player.position += MoveDirection * 8f;
                        player.fallStart = (int)player.position.Y / 16;
                        if (player.getRect().Intersects(Me.HitBox))
                        {
                            player.velocity = Vector2.Zero;
                            InUse = false;
                        }
                    }
                    else if (Guardians.Count > 0)
                    {
                        TerraGuardian guardian = Guardians[0];
                        Vector2 MoveDirection = Me.CenterPosition - guardian.CenterPosition;
                        MoveDirection.Normalize();
                        Me.LookingLeft = guardian.Position.X < Me.Position.X;
                        guardian.Position += MoveDirection * 8f;
                        guardian.SetFallStart();
                        if (guardian.HitBox.Intersects(Me.HitBox))
                        {
                            guardian.Velocity = Vector2.Zero;
                            InUse = false;
                        }
                    }
                }
            }

            public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
            {
                int HandFrame = 0;
                if (Time < 5)
                {
                    HandFrame = 14;
                }
                else if (Time < 10)
                {
                    HandFrame = 15;
                }
                else if (Time < 15)
                {
                    HandFrame = 16;
                }
                else if (Time < 20)
                {
                    HandFrame = 17;
                }
                if (Time >= PullMaxTime)
                {
                    HandFrame = 15;
                }
                if (!UsingRightArmAnimation)
                    guardian.RightArmAnimationFrame = HandFrame;
                else if (!UsingLeftArmAnimation)
                    guardian.LeftArmAnimationFrame = HandFrame;
            }

            public override void Draw(TerraGuardian guardian)
            {
                Vector2 EndPosition = Vector2.Zero;
                if (Players.Count > 0)
                {
                    EndPosition = Players[0].Center;
                }
                else if (Guardians.Count > 0)
                {
                    EndPosition = Guardians[0].CenterPosition;
                }
                if (Time < PullMaxTime)
                {
                    float Percentage = (float)Time / PullMaxTime;
                    EndPosition = guardian.CenterPosition + (EndPosition - guardian.CenterPosition) * Percentage;
                }
                DrawIntestine(guardian, EndPosition);
            }

            public void DrawIntestine(TerraGuardian Guardian, Vector2 ChainEndPosition)
            {
                Vector2 ChainStartPosition = Guardian.CenterPosition;
                ChainStartPosition.X -= 8 * Guardian.Direction;
                ChainStartPosition.Y -= 8;
                float DifX = ChainStartPosition.X - ChainEndPosition.X, DifY = ChainStartPosition.Y - ChainEndPosition.Y;
                bool DrawMoreChain = true;
                float Rotation = (float)Math.Atan2(DifY, DifX) - 1.57f;
                while (DrawMoreChain)
                {
                    float sqrt = (float)Math.Sqrt(DifX * DifX + DifY * DifY);
                    if (sqrt < 40)
                        DrawMoreChain = false;
                    else
                    {
                        sqrt = (float)Main.chain12Texture.Height / sqrt;
                        DifX *= sqrt;
                        DifY *= sqrt;
                        ChainEndPosition.X += DifX;
                        ChainEndPosition.Y += DifY;
                        DifX = ChainStartPosition.X - ChainEndPosition.X;
                        DifY = ChainStartPosition.Y - ChainEndPosition.Y;
                        Microsoft.Xna.Framework.Color color = Lighting.GetColor((int)ChainEndPosition.X / 16, (int)ChainEndPosition.Y / 16);
                        Main.spriteBatch.Draw(Main.chain12Texture, ChainEndPosition - Main.screenPosition, null, color, Rotation, new Vector2(Main.chain12Texture.Width * 0.5f, Main.chain12Texture.Height * 0.5f), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
