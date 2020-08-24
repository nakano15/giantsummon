using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class MichelleBase : GuardianBase
    {
        /// <summary>
        /// -Likes cute critters.
        /// -Friendly towards anyone.
        /// -Not that great of an adventurer.
        /// -Likes the company of TerraGuardians.
        /// </summary>
        public MichelleBase()
        {
            Name = "Michelle";
            Description = "Your personal TerraGuardians fan girl.";
            Age = 16;
            Male = false;
            Accuracy = 0.27f;
            DrinksBeverage = false;
            CanChangeGender = false;
            SetTerrarian();
            CallUnlockLevel = 0;

            TerrarianInfo.HairStyle = 21;
            TerrarianInfo.SkinVariant = 1;
            TerrarianInfo.HairColor = new Microsoft.Xna.Framework.Color(55, 215, 255);
            TerrarianInfo.EyeColor = new Microsoft.Xna.Framework.Color(196, 10, 227);
            TerrarianInfo.SkinColor = new Microsoft.Xna.Framework.Color(237, 118, 85);

            TerrarianInfo.ShirtColor = new Microsoft.Xna.Framework.Color(248, 28, 28);
            TerrarianInfo.UnderShirtColor = new Microsoft.Xna.Framework.Color(30, 249, 20);
            TerrarianInfo.PantsColor = new Microsoft.Xna.Framework.Color(179, 36, 245);
            TerrarianInfo.ShoeColor = new Microsoft.Xna.Framework.Color(206, 29, 29);

            PopularityContestsWon = 1;
            ContestSecondPlace = 1;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);
        }

        public override string FriendLevelMessage
        {
            get
            {
                return "Oh hi! Came to call me to an adventure?";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "This world got so much more fun after the TerraGuardians arrived.";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "The people of the world seems to be getting along very well, no worries.";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "Say, we're adventuring too much, right? Let's take a few days of vacation with some of our friends, and spend some time just having fun around?";
            }
        }

        public override string LeavingWorldMessage
        {
            get
            {
                return " has logged out.";
            }
        }

        public override string LeavingWorldMessageGuardianSummoned
        {
            get
            {
                return "'s things were already packed.";
            }
        }

        public override string GreetMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Hello! Are you a begginer too?");
            Mes.Add("Oh, Hi! I didn't knew there were someone else in this world.");
            Mes.Add("Are you an adventurer? Cool! Me too. We could be friends!");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("What now?!");
                Mes.Add("Don't you have anything else to do?");
                Mes.Add("What's with that look? Are you indirectly calling me ugly?!");
            }
            else
            {
                bool HasTerraGuardians = NpcMod.GetTerraGuardianNPCCount() > 0;
                Mes.Add("Oh, checking out? I'm fine, thanks for caring.");
                if (HasTerraGuardians)
                {
                    Mes.Add("This place is like a dream came true!");
                    Mes.Add("I can't stop petting those TerraGuardians, they are soooooooooo cute!!");
                    Mes.Add("How are we able to understand what the TerraGuardians says? It's like as If what they say comes from inside my mind.");
                }
                if (Main.dayTime)
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("What are those things?! They aren't cute at all! Maybe Mothron but... Everything else...");
                    }
                    else
                    {
                        Mes.Add("(As soon as she started singing, several animals started to gather around her.)");
                    }
                }
                else
                {
                    Mes.Add("I surelly would like to get a nap, and enjoy this night.");
                    if(HasTerraGuardians)
                        Mes.Add("I wonder if any TerraGuardian would let me accompany them during the night.");
                    Mes.Add("Yawn~ I'm surelly getting sleepy.");
                }
                if (Main.raining)
                {
                    Mes.Add("Eww... Rain... I hope I don't catch flu.");
                    Mes.Add("The weather seems ugly outside.");
                }

                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                {
                    Mes.Add("What is wrong with my look? [nn:" + Terraria.ID.NPCID.Stylist + "] keeps making fun of my look.");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                {
                    Mes.Add("It's absurd that [nn:" + Terraria.ID.NPCID.Merchant + "] doesn't have any Orange or Yellow Potions for sale. Don't he knows that they heal more?");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic))
                {
                    Mes.Add("I was kicked out of [nn:" + Terraria.ID.NPCID.Mechanic + "]'s room... She didn't liked when I turned off the light switches after she turned them on...");
                }

                if (NpcMod.HasGuardianNPC(GuardianBase.Rococo))
                {
                    Mes.Add("I like [gn:" + GuardianBase.Rococo + "] because he's so easy to be friend of. Soon We'll be BFF.");
                    Mes.Add("[gn:" + GuardianBase.Rococo + "] took me outside someday to watch a meteor shower, in the top of a mountain. It was so beatiful...");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Blue))
                {
                    Mes.Add("I like touching [gn:" + GuardianBase.Blue + "]'s hair, but she seems to not like it when I do that.");
                    Mes.Add("Say, do you think I'll find someone for me? Like [gn:" + GuardianBase.Blue + "] found.");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Sardine))
                {
                    Mes.Add("[gn:" + GuardianBase.Sardine + "] has so many interesting adventure stories, I wonder if someday I'll have many stories to tell, too.");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Alex))
                {
                    Mes.Add("I keep giving treats to [gn:" + GuardianBase.Alex + "]. He deserves, he's a really good boy, beside one time he stuck my head in the sand when he jumped on me. I think I still have sand in my nose.");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Zacks))
                {
                    Mes.Add("[gn:" + GuardianBase.Zacks + "] is so creepy! He's a cute wolf, but creepy.");
                    Mes.Add("I think that [gn:" + GuardianBase.Zacks + "] is actually a good person, but I keep fearing him because he's a Zombie.");
                    Mes.Add("Wasn't zombies supposed to burn during the day? [gn:" + GuardianBase.Zacks + "] seems fine while walking around during the day. At least I saw them burning in another univese.");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Brutus))
                {
                    Mes.Add("I broke [gn:" + GuardianBase.Brutus + "]'s seriousness easily by petting his head. He started to purr afterwards, too.");
                    Mes.Add("[gn:" + GuardianBase.Brutus + "]'s stories about the Ether Realm are amazing! I want to meet that place someday.");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Bree))
                {
                    Mes.Add("I have a new goal, become BFF of [gn:" + GuardianBase.Bree + "]. Wait, why that face?");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Mabel))
                {
                    Mes.Add("Why the male people of your town keeps drooling at [gn:" + GuardianBase.Mabel + "]?");
                    Mes.Add("Miss North Pole contest? Maybe [gn:" + GuardianBase.Mabel + "] could help me get in It too? It sounds fun!");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Domino))
                {
                    Mes.Add("Whenever I try petting [gn:" + GuardianBase.Domino + "], he tries to bite my hand.");
                    Mes.Add("Why is [gn:" + GuardianBase.Domino + "] so difficult to deal with?");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
                {
                    Mes.Add("What a cute bunny [gn:" + GuardianBase.Leopold + "] is. I'd like to hug him so hard!");
                    Mes.Add("[gn:" + GuardianBase.Leopold + "] asked me earlier If I could do a test to check for Hyperactivity. ???");
                    if (!player.Male)
                        Mes.Add("Do you like touching [gn:" + GuardianBase.Leopold + "]'s tail too? It's soft!");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Vladimir))
                {
                    Mes.Add("You didn't saw me last night? Sorry, I was sleeping on [gn:" + GuardianBase.Vladimir + "]'s arms during the entire night. His hug is warm.");
                    Mes.Add("Everytime [gn:" + GuardianBase.Vladimir + "] hugs someone, he looks very happy and satisfied. I think he really loves that.");
                    Mes.Add("A number of people in the town thinks that [gn:" + GuardianBase.Vladimir + "] hugging people is exquisite, but I saw them being hugged by him too.");
                }
                if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
                {
                    Mes.Add("I'm trying my best not to hate [gn:" + Malisha + "], but she keeps turning me into a different critter whenever I try to pet her.");
                    Mes.Add("Do you think [gn:" + Malisha + "] hates me? Yeah, I think not too.");
                }
                int GNPCCount = NpcMod.GetTerraGuardianNPCCount();
                if (GNPCCount >= 10)
                {
                    Mes.Add("So many TerraGuardians around! It's like my personal heaven! Thank you! Thank you for finding them!");
                    Mes.Add("I love this place, so many cute and different looking TerraGuardians! This place is my gift!");
                }
                if (GNPCCount >= 5)
                {
                    Mes.Add("There's a lot of TerraGuardians here, I like that! Too many options of things to pet.");
                    Mes.Add("Everywhere I go, I see a TerraGuardian walking around going to some place, or hanging around somewhere. I love It!");
                }
                if (GNPCCount >= 1 && GNPCCount < 5)
                {
                    Mes.Add("I liked to meet the TerraGuardians, I wonder If there are more around.");
                }
                if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("Yes! I can share my room with you. We could play pillow fight sleeping.");
                    Mes.Add("I like having you as a room mate, but I would like having a TerraGuardian more.");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Say, do you think I need a change in my look?");
            Mes.Add("I keep wondering, what new thing we could add to improve the town.");
            Mes.Add("I have to say, I don't really feel like fitting for adventuring. I think I'm more into meeting new people.");
            if(NpcMod.HasGuardianNPC(GuardianBase.Nemesis))
                Mes.Add("I asked [gn:" + GuardianBase.Nemesis + "] to follow me on an adventure, It followed me almost exactly like I moved. If I ran, It ran too. If I walked, It walked too. I felt so annoyed that I dismissed him.");
            if (NpcMod.GetTerraGuardianNPCCount() >= 10)
            {
                Mes.Add("So many TerraGuardians around! I'm never sad or alone when one of them is around.");
            }
            if (NpcMod.GetTerraGuardianNPCCount() >= 5)
            {
                Mes.Add("I like the company of the TerraGuardians.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Um... No, I'm fine.");
            Mes.Add("Maybe another time. Right now I have everything I need.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Hey, Hey! I need your help for something. Can you give me a hand?");
            Mes.Add("Saaaaaay... Can you help me with something?");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Yay! Thanks! Hehe.");
            Mes.Add("You did it?! You're my hero! Thanks!");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("I barelly can hold my excitement.");
            if(NpcMod.GetTerraGuardianNPCCount() > 0)
                Mes.Add("Everyone's here for my party, even the TerraGuardians. I'm... I'm going to cry...");
            Mes.Add("You organized all this? You really know what I wanted. Teehee.");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("You prepared a surprise for me? What is it? What is it?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer)
            {
                Mes.Add("Come on! You can't be a hero by lying down.");
                Mes.Add("I think that's not the worst beating you can get.");
                Mes.Add("You're not doing that on purpose, right?");
            }
            else
            {
                if (ReviveGuardian.Base.IsTerraGuardian)
                {
                    Mes.Add("*Petting intensifies*");
                    Mes.Add("Sooooooo cuuuuute!!");
                    Mes.Add("Awww... You're even cuter when knocked out.");
                }
                else
                {
                    Mes.Add("Are you fine? I'll try helping you.");
                    Mes.Add("Here, I'll try doing something about those wounds.");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "Come on, for how long will are you going to sleep?";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
