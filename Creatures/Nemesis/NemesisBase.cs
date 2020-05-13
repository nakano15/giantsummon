using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class NemesisBase : GuardianBase
    {
        /// <summary>
        /// -Doesn't cares about anything.
        /// -Emotions stolen.
        /// -Mirrors Terrarian trying to get some personality from doing so.
        /// -Questions the outfit choices the player gives It.
        /// -Feels indiferent on relationship with town citizens.
        /// </summary>

        public NemesisBase()
        {
            Name = "Nemesis";
            Description = "It's cryptic to know who the Nemesis is, or was.\nNeither if is a \"he\" or a \"she\".";
            Age = 256;
            Male = true;
            InitialMHP = 100; //500
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 10;
            Accuracy = 0.32f;
            Mass = 0.3f;
            MaxSpeed = 3f;
            Acceleration = 0.08f;
            SlowDown = 0.2f;
            MaxJumpHeight = 15;
            JumpSpeed = 5.01f;
            DrinksBeverage = true;
            CanChangeGender = true;
            Effect = GuardianEffect.Wraith;
            SetTerrarian();
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit54);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath52);
            CallUnlockLevel = 0;

            TerrarianInfo.HairStyle = 15;

            PopularityContestsWon = 0;
            ContestSecondPlace = 1;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "If you have something dangerous to do that can possibly end on demise, you can send me to do it instead. I don't care.";
            }
        }

        public override string FriendLevelMessage
        {
            get
            {
                return "You have been doing so many things for me. Good.";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "What do you think when you give me those outfits to wear? People seems to have mixed reactions depending on how I'm dressed.";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "I have long lost my emotions since I died, so I'm trying to mirror you trying to have some. It isn't working, but you seem to benefit from my companionship, so I keep following you.";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "I don't think I have a goal, or even a purpose for existing, so I guess I will keep following you. At least until I find a purpose.";
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "You and I are tied together, now.";
                case 1:
                    return "You seems strong enough, I will be your shadow.";
                case 2:
                    return "Who am I doesn't matter, what matters is that now I'm yours.";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "The only thing I want to do is follow you.";
            return "I don't need anything right now.";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "I want you to bring this to me. Doesn't matter why.";
            return "I have got a little task for you, If you don't mind, I can come with you to do it.";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "I have got a little task for you, If you don't mind, I can come with you to do it.";
            return "I can't cheer for you doing what I asked you to do.";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
            {
                Mes.Add("I have no emotion, no memory, neither a physical body.");
            }
            else
            {
                Mes.Add("This night looks perfect for some random violence.");
                Mes.Add("Don't worry about me, I don't feel fear, or any other emotion.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Those creatures seems to have spawned from a flat screen.");
            }
            Mes.Add("I can wear any outfit you give me, just leave it on the first slots of my inventory.");
            Mes.Add("I don't have any goal, so I set my own goal to be of helping you on your quest.");
            Mes.Add("I have become so numb.");
            Mes.Add("Should I worry that you could make me look ridiculous?");
            Mes.Add("I am now your shadow, whenever you need, I will go with you, even if it means my demise.");
            Mes.Add("I were in this world for too long, I have seen several things that happened here.");
            if (Main.dayTime && !Main.eclipse)
                Mes.Add("The clothings I have are like my body parts, since they are visible all time. But that doesn't seems to help making the other citizens feel less alarmed about my presence.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide))
            {
                Mes.Add("[nn:" + Terraria.ID.NPCID.Guide + "] doesn't appreciate the fact that I know more things than him.");
            }
            if (!Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("It's night, I don't feel sleepy.");
                Mes.Add("Doesn't matter if It's day or night, I still am partially invisible anytime.");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("Everyone seems to be expressing themselves on this party. Me? I'll just stay drinking.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
                Mes.Add("You have emotions, right? What should I be feeling after hearing that [nn:" + Terraria.ID.NPCID.Clothier + "] is a lady killer?");
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("[gn:1] keeps forgetting to look where she sits.");
            if (NpcMod.HasGuardianNPC(2))
                Mes.Add("I told [gn:2] that I don't feel anything about drinking, after he asked me about going out for a drink sometime.");
            if (NpcMod.HasGuardianNPC(3))
                Mes.Add("Everyone seems uncomfortable about having [gn:3] and me around. I don't know where is the problem.");
            if (NpcMod.HasGuardianNPC(0))
                Mes.Add("[gn:0] always runs away when he sees me. Did I do something to him?");
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("Before you ask: No, I'm not " + AlexRecruitScripts.AlexOldPartner + ", but I once saw a cheerful woman playing with him during my night stalking a long time ago.");
                Mes.Add("I don't know what it feels by tossing a ball to make [gn:5] fetch it. I just do it because he askes me to.");
            }
            if (NpcMod.HasGuardianNPC(2) && NpcMod.HasGuardianNPC(7))
                Mes.Add("I don't know what is love, or what it is to feel love, but I think [gn:2] and [gn:7] have a very divergent and disturbed relationship.");
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("I always win the stare contest, because [gn:8] ends up laughing after a few minutes staring my face. I don't know why.");
                Mes.Add("I think [gn:8] is super effective on the town, since she atracts attention of almost everyone in the town. Me? I don't care. \"Sips coffee\"");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("I would like to have some place which I could stay when I'm not following you.");
            Mes.Add("You have some place where I could stay, right?");
            if (!Main.dayTime)
            {
                Mes.Add("It will be hard for you to find me in a place like this. Give me some place to stay.");
                Mes.Add("I am nothing like those night creatures, let me stay in someplace.");
            }
            if (Main.raining)
                Mes.Add("This is not a good situation, give me some place dry.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Why does people here look at me like as if I would kill them in their sleep?");
            Mes.Add("I see all this colorful environment, but can't feel anything.");
            if (!PlayerMod.HasGuardianSummoned(player, 4))
            {
                Mes.Add("Take me with you on your quest, sometime.");
                if (Main.raining)
                    Mes.Add("The rain passes through my body, but the armor still can take the drops.");
            }
            Mes.Add("The dungeon in this world? It is a place where cultists sacrificed people to awaken some ancient god. A Terrarian has defeated that ancient god, but parts of it remains in this world.");
            if (!PlayerMod.HasGuardianSummoned(player, 0))
            {
                Mes.Add("I don't know what it is to feel fun, [gn:0]. So stop doing jokes.");
                Mes.Add("I were wanting to talk to you, [gn:0]. Why do you take people trash with you?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 4) && Main.rand.NextDouble() < 0.5)
                return "You want to give me something? Ok, I will open it then.";
            return "It doesn't matter how old I am, I nearly don't exist.";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("I wont copy that, seems shameful.");
                Mes.Add("Are you supposed to stay lying down on the floor?");
                Mes.Add("Those wounds will be taken care of.");
            }
            else
            {
                Mes.Add("You will be back soon.");
                Mes.Add("I'll take care of this.");
                Mes.Add("You look exausted.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
