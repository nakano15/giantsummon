using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Companions
{
    public class QuentinBase : GuardianBase
    {
        public string HeadTextureID = "head";

        public QuentinBase()
        {
            Name = "Quentin";
            Description = "He is a young green bunny who dreams of becoming \na powerful wizard one day, his hobbies are reading \nfiction books and telling stories.";
            Age = 15;
            SetBirthday(SEASON_AUTUMN, 3);
            CompanionContributorName = "totote97";
            Male = true;
            InitialMHP = 85; //500
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 10;
            InitialMP = 50;
            ManaCrystalMPBonus = 25 + 2; //Pft, mana fruit
            Accuracy = 0.32f;
            Mass = 0.3f;
            MaxSpeed = 3f;
            Acceleration = 0.08f;
            SlowDown = 0.2f;
            MaxJumpHeight = 15;
            JumpSpeed = 5.01f;
            DrinksBeverage = true;
            //CanChangeGender = true;
            //Effect = GuardianEffect.Wraith;
            //IsNocturnal = true;
            SetTerrarian();
            Scale += 0.03f;
            //HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit54);
            //DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath52);
            CallUnlockLevel = 0;

            TerrarianInfo.HairStyle = 15;
            TerrarianInfo.SkinVariant = 0;
            TerrarianInfo.HairColor = new Color(153, 229, 80); //old: 215, 90, 55
            TerrarianInfo.EyeColor = new Color(0, 0, 0); //old: 105, 90, 75
            TerrarianInfo.SkinColor = new Color(153, 229, 80); //old: 203, 255, 90
            TerrarianInfo.ShirtColor = new Color(153, 229, 80); //old: 203, 255, 90
            TerrarianInfo.UnderShirtColor = new Color(153, 229, 80); //old: 203, 255, 90
            TerrarianInfo.PantsColor = new Color(153, 229, 80); //old: 203, 255, 90
            TerrarianInfo.ShoeColor = new Color(153, 229, 80); //old: 203, 255, 90

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            AddSkin(1, "Remove Hat", delegate (GuardianData gd, Player player) { return true; });
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HeadTextureID, "QuentinHead");
        }

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, SpriteEffects seffect, Vector2 Origin, ref List<GuardianDrawData> gdds)
        {
            Texture2D texture = sprites.GetExtraTexture(HeadTextureID);
            Vector2 Position = DrawPosition;
            Position.Y -= 2;
            byte SkinID = guardian.SkinID;
            if (TerraGuardian.HeadSlot > 0)
                SkinID = 1;
            for (int i = gdds.Count - 1; i >= 0; i--)
            {
                if (gdds[i].textureType == GuardianDrawData.TextureType.PlEye ||
                    gdds[i].textureType == GuardianDrawData.TextureType.PlEyeWhite)
                {
                    gdds.RemoveAt(i);
                }
                if (gdds[i].textureType == GuardianDrawData.TextureType.PlHead)
                {
                    Rectangle rect = new Rectangle(0, 0, 40, 58);
                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, Position, rect,
                        Color.White, 0f, Origin, Scale, seffect);
                    if (i + 1 >= gdds.Count)
                        gdds.Add(gdd);
                    else
                        gdds.Insert(i + 1, gdd);
                    rect.Y = rect.Height * (SkinID == 1 ? 2 : 1);
                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, Position, rect,
                         Color.White, 0f, Origin, Scale, seffect);
                    if (i + 2 >= gdds.Count)
                        gdds.Add(gdd);
                    else
                        gdds.Insert(i + 2, gdd);
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Rectangle rect = new Rectangle(0, 0, 40, 58);
            Vector2 Position = new Vector2(DrawPosition.X, DrawPosition.Y);
            Vector2 NewOrigin = new Vector2(Origin.X, Origin.Y);
            Position.Y -= 2 * guardian.GravityDirection;
            //Position.X += 2 * guardian.Direction;
            byte SkinID = guardian.SkinID;
            if (TerraGuardian.HeadSlot > 0)
                SkinID = 1;
            if (guardian.ItemAnimationTime == 0 && 
                ((guardian.LeftArmAnimationFrame >= 7 && guardian.LeftArmAnimationFrame < 10) ||
                (guardian.LeftArmAnimationFrame >= 14 && guardian.LeftArmAnimationFrame < 17)))
                Position.Y -= 2 * guardian.GravityDirection;
            rect.Y = rect.Height * (SkinID == 1 ? 2 : 1);
            Texture2D texture = sprites.GetExtraTexture(HeadTextureID);
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, Position, rect,
                color, Rotation, NewOrigin, Scale, seffect);
            InjectTextureAfter(GuardianDrawData.TextureType.PlHead, gdd);
            rect.Y = 0;
            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, Position, rect,
                color, Rotation, NewOrigin, Scale, seffect);
            InjectTextureAfter(GuardianDrawData.TextureType.PlHead, gdd);
            RemoveTextureDrawData(GuardianDrawData.TextureType.PlEye);
            RemoveTextureDrawData(GuardianDrawData.TextureType.PlEyeWhite);
        }

        #region Dialogues

        public override string ControlUnlockMessage => "If you really need it, I can lead the group.";

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "let's discover together the mysteries of magic.";
                case 1:
                    return "I may still be an apprentice but I assure you I can be of help.";
                case 2:
                    return "I am a mage's apprentice trying to gather more knowledge.";
            }
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextFloat() < 0.5f)
                return "I don't need anything yet I'm just enjoying this adventure.";
            return "I don't need your help right now, don't worry.";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextFloat() < 0.5f)
                return "I want you to [objective] for me. Its for research purposes.";
            return "I have a mission for you, and I don't mind helping you with it.. [objective].";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextFloat() < 0.5f)
                return "I always knew that you could achieve it without problem.";
            return "Wonderful, I never doubt that you could make it.";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("since I joined you I feel that I have become more powerful, if we continue like this soon we will be unstoppable.");
            Mes.Add("my master was a great sorcerer, I miss him a lot since I got here.");
            Mes.Add("what? What do you mean I look like a clown?.");
            Mes.Add("I am a bunny, not a rabbit or a hare, learn to distinguish them.");
            Mes.Add("this hat and this robe were gifts from my Master for my birthday.");
            Mes.Add("I am amazed at the amount of mysteries that still remain to be unveiled in this new world.");

            if (Main.bloodMoon)
            {
                Mes.Add("Did the moon just turn red? I hope nothing bad happens tonight.");
                Mes.Add("What are those scary things? as if zombies weren't bad enough.");
                Mes.Add("even the water looks scary tonight.");
            }
            if (Main.eclipse)
            {
                Mes.Add("The only thing that terrifies me more than a dark night is when it's just as dark during the day.");
                Mes.Add("You saw the size of that thing, I hope it doesn't come any closer to us.");
                Mes.Add("some of these monsters look like the ones in horror movies my master used to watch.");
            }
            switch (Main.invasionType)
            {
                case Terraria.ID.InvasionID.PirateInvasion:
                    Mes.Add("I always knew that the Pale Pirate would come seeking revenge for stealing his treasure.");
                    Mes.Add("don't let the Pale Pirate capture me please.");
                    break;
            }
            if(guardian.GetTileCount(Terraria.ID.TileID.MinecartTrack) > 0)
            {
                Mes.Add("this reminds me when me and my master got on the Tricky Train to try to stop it before it fell off a ravine.");
            }
            if (guardian.HasNpcBeenSpotted(Terraria.ID.NPCID.Guide))
            {
                Mes.Add("That man always dresses in such a boring way, wasn't there more striking clothes in his closet?.");
            }
            if (guardian.HasNpcBeenSpotted(Terraria.ID.NPCID.Merchant))
            {
                Mes.Add("I love that old man, he is very nice to me and he sells that blue juice that makes me feel like I regain my powers with every sip.");
            }
            if (guardian.HasNpcBeenSpotted(Terraria.ID.NPCID.Wizard))
            {
                Mes.Add("Finally i find another wizard here, I hope that he is willing to teach me more about magic.");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                Mes.Add("when i asked leopold if he wanted to help me my magic show, he didn't speak to me for a week, I don't know why he got so upset.");
                Mes.Add(" Is nice to see another bunny after so much time");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("I really love parties, nothing better than filling my stomach with cake.");
            }
            bool Forest = guardian.ZoneOverworldHeight;
            if (guardian.ZoneHoly)
            {
                Forest = false;
                Mes.Add("Hmm those colorful trees look like cotton candy, can I try one?.");
                Mes.Add("I tried to pet one of those unicorns and it almost attacked me with its horn.");
            }
            if(guardian.ZoneCorrupt || guardian.ZoneCrimson)
            {
                Forest = false;
                Mes.Add("this place looks even more scary than forest on night.");
                Mes.Add("I feel very observed and not in a good way.");
            }
            if (guardian.ZoneSnow)
            {
                Forest = false;
                Mes.Add("with this cold my mustache freezes.");
                Mes.Add("Brrr... my robe is not thick enough to shelter me from this cold.");
            }
            if (guardian.ZoneJungle)
            {
                Forest = false;
                Mes.Add("why are we here? It is full of carnivorous plants and bats everywhere.");
            }
            if (guardian.ZoneDungeon)
            {
                Mes.Add("this place only brings back bad memories.");
            }
            if (Forest)
            {
                Mes.Add("I always try to talk to the rabbits around here but they seem to be very shy.");
            }
            if(Main.raining)
            {
                Mes.Add("If I can't find a place to shelter from this rain, my robe will shrink.");
            }

            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (!Main.dayTime)
                Mes.Add("You know, i am afraid of being alone in a dark night, thats why i try to collect as many glowsticks when i see a jellyfish or buy some to that merchant old man.");
            if (player.statManaMax > 20)
                Mes.Add("I can sense your magic power is growing stronger, at least share some of those crystals the next time you have some to spare, after all i need to grow stronger too to achieve my objectives.");
            Mes.Add("I hate the beach, the sand gets everywhere and my books end up ruined with the mix of sand and water.");
            Mes.Add("Once i tried to fish but i caught a tuna so big that when i tried to get him out of the water it almost swallowed me in one bite.");
            Mes.Add("when I grow up I will open a magic store or a bookstore or better yet, a magic bookstore");
            Mes.Add("I only take baths in the lakes and rivers, the sea water makes my body completely bristle, also in the sea are sharks.");
            Mes.Add("I heard that there are caves in this world where everything glows a deep blue color and giant mushrooms grow, you must take me to see that.");
            if (NpcMod.HasGuardianNPC(Miguel))
                Mes.Add("That horse keep saying that i am weakling, when he is gonna get that i am not a fighter,i am a wizard, i train my mind, not my body.");
            if (NpcMod.HasGuardianNPC(Minerva))
                Mes.Add("I love that minerva joined us, she cooks really well and make some really tasty cookies for the tea time.");
            if (NpcMod.HasGuardianNPC(Cinnamon))
                Mes.Add("How i can I explain to cinnamon that I don't hate her but the spice of the same name?");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            if(Main.rand.NextFloat() < 0.5f)
                return "You know where i could find a tower or at least a house not made of wood to live?, I fear i would end I would end up burning it if not.";
            return "Every wizard needs a lair where he can practice his magic.";
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if(!PlayerMod.HasGuardianBeenGifted(player, guardian) && Main.rand.NextFloat() < 0.5f)
                return "you say you have a gift for me? I love them, what surprises will it hide.";
            return "my master always told me that with age comes wisdom.";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "You will be back.";
                case 1:
                    return "I'll take care of you, don't worry.";
                case 2:
                    return "You look terrible, let me help you.";
            }
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {

                }
            }
            Weight = 1f;
            return "Yay!! New friends.";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "I'm going to try to heal you, don't move too much.";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    if(Main.rand.NextFloat() < 0.5f)
                        return "good morning friend, a new day means a new adventure.";
                    return "ooh, i was dreaming about candies and chocolate.";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    if (Main.rand.NextFloat() < 0.5f)
                        return "Did you complete my task?";
                    return "What about my quest?";

                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "Of, course, let me grab my things.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "No, i hate crowds.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "i am busy now.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "Are you sure about that? i thought we were having fun.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "No problem then.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "I know the way."; ;
                case MessageIDs.AfterAskingIfCompanionCanVisitNextDayNoAnswer:
                    return "good.";
                case MessageIDs.RequestAccepted:
                    return "Awesome.";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "No. you look too busy to take care of this.";
                case MessageIDs.RequestRejected:
                    return "I hope you can help me another time then.";
                case MessageIDs.RequestPostpone:
                    return "don't worry I'm not in a hurry.";
                case MessageIDs.RequestFailed:
                    return "I never expected this result, well there will always be more chances to achieve it.";
                case MessageIDs.RequestAsksIfCompleted:
                    return "I sense that you did my request. Am I right?";
                case MessageIDs.RequestRemindObjective:
                    return "Don't worry! The great [name] will remind you of your objective. You have to [objective].";
                case MessageIDs.RestAskForHowLong:
                    return "is really important to rest well so you should sleep for at least 8 hours to have energy for our next adventure";
                case MessageIDs.RestNotPossible:
                    return "i don´t think is a good time to sleep.";
                case MessageIDs.RestWhenGoingSleep:
                    return "zzz";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "[shop]'s shop has something i want, please get it for me.";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "Yes, this one.";
                case MessageIDs.GenericYes:
                    return "Yep.";
                case MessageIDs.GenericNo:
                    return "Nope.";
                case MessageIDs.GenericThankYou:
                    return "Thanks.";
                case MessageIDs.GenericNevermind:
                    return "Nevermind then.";
                case MessageIDs.ChatAboutSomething:
                    return "Speak.";
                case MessageIDs.NevermindTheChatting:
                    return "Okay.";
                case MessageIDs.CancelRequestAskIfSure:
                    return "Are you sure you can't do it?";
                case MessageIDs.CancelRequestYesAnswered:
                    return "Okay. then i am gonna find another to do It.";
                case MessageIDs.CancelRequestNoAnswered:
                    return "Then It was just a mistake of what to say.";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "Thanks i feel a lot better now.";
                case MessageIDs.RevivedByRecovery:
                    return "I thought i wasn't gonna make it.";
            }
            return base.GetSpecialMessage(MessageID);
        }

        #endregion
    }
}
