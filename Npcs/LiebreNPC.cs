using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Npcs
{
    class LiebreNPC : GuardianActorNPC
    {
        public static byte EncounterTimes = 0;
        private bool JustSpawned = true;
        private bool SpottedPlayer = false;
        private bool PlayerLeft = false;
        private bool FinishedTalking = false;
        private byte DialogueStep = 0;
        private ushort TalkTime = 0;

        public LiebreNPC() : base(GuardianBase.Liebre)
        {

        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("???");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
        }

        public override void AI()
        {
            if (JustSpawned)
            {
                JustSpawned = false;
                Main.NewText("The air suddenly grows cold... ");
            }
            if (DialogueStep > 0 && Main.LocalPlayer.talkNPC != npc.whoAmI)
                DialogueStep = 0;
            if (FinishedTalking)
            {
                Idle = true;
            }
            else if (!SpottedPlayer)
            {
                Idle = true;
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                if (player.active && !player.dead)
                {
                    Vector2 Distance = player.Center - npc.Center;
                    if (Math.Abs(Distance.X) < 180f && Math.Abs(Distance.Y) < 120f &&
                        Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                    {
                        Idle = false;
                        if (!PlayerLeft)
                        {
                            switch (EncounterTimes)
                            {
                                case 0:
                                    SayMessage("*A Terrarian..*");
                                    break;
                                case 1:
                                    SayMessage("*Hm... Interesting place..*");
                                    break;
                                case 2:
                                    SayMessage("*This... This place...*");
                                    break;
                                case 3:
                                    SayMessage("*Terrarian, can we talk?*");
                                    break;
                            }
                        }
                        else
                        {
                            switch (EncounterTimes)
                            {
                                case 0:
                                    SayMessage("*Is that Terrarian again..*");
                                    break;
                                case 3:
                                    SayMessage("*Now we can talk?*");
                                    break;
                            }
                        }
                        TalkTime = 180;
                        SpottedPlayer = true;
                    }
                }
            }
            else
            {
                Idle = false;
                Player player = Main.player[npc.target];
                Vector2 Distance = player.Center - npc.Center;
                if(npc.velocity.X == 0)
                    if (player.Center.X < npc.Center.X)
                        npc.direction = -1;
                    else
                        npc.direction = 1;
                if (TalkTime > 0 && EncounterTimes == 0)
                {
                    TalkTime--;
                    if (TalkTime == 0)
                    {
                        if (Math.Abs(Distance.X) < 180f && Math.Abs(Distance.Y) < 120f &&
                            Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                        {
                            if (!PlayerLeft)
                                SayMessage("*Terrarian, can we talk?*");
                            else
                                SayMessage("*Please don't run away again, I must speak to you.*");
                        }
                        else
                        {
                            if (!PlayerLeft)
                                SayMessage("*Wait, Terrarian, I need to talk to you.*");
                            else
                                SayMessage("*Wait, don't go again!*");
                        }
                    }
                }
                if (Math.Abs(Distance.X) >= 600 || Math.Abs(Distance.Y) >= 240)
                {
                    SpottedPlayer = false;
                    if (!PlayerLeft)
                        SayMessage("*I guess I scared them...*");
                    else
                        SayMessage("*They left... Again...*");
                    PlayerLeft = true;
                }
            }
            base.AI();
        }

        public override void ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, SpriteEffects seffects)
        {
            float PlasmaOpacity = 1f - (float)(color.R + color.G + color.B) / (255 * 3);
            bool RightArmPlaced = false, BodyPlaced = false, LeftArmPlaced = false;
            for(int i = 0; i < dds.Count; i++)
            {
                switch (dds[i].textureType)
                {
                    case GuardianDrawData.TextureType.TGRightArm:
                        if (!RightArmPlaced)
                        {
                            dds[i].color *= PlasmaOpacity;
                            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra,
                                Base.sprites.GetExtraTexture(Creatures.LiebreBase.SkeletonRightArmID), Position,
                                RArmRect, color, npc.rotation, Origin, npc.scale, seffects);
                            dds.Insert(i, gdd);
                            RightArmPlaced = true;
                            gdd = DrawEquippedScythe(Position, color, 0, Origin, npc.scale, seffects);
                            dds.Insert(i, gdd);
                            RightArmPlaced = true;
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        if (!BodyPlaced)
                        {
                            dds[i].color *= PlasmaOpacity;
                            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, 
                                Base.sprites.GetExtraTexture(Creatures.LiebreBase.SkeletonBodyID), Position,
                                BodyRect, color, npc.rotation, Origin, npc.scale, seffects);
                            dds.Insert(i, gdd);
                            BodyPlaced = true;
                        }
                        break;
                    case GuardianDrawData.TextureType.TGLeftArm:
                        if (!LeftArmPlaced)
                        {
                            dds[i].color *= PlasmaOpacity;
                            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra,
                                Base.sprites.GetExtraTexture(Creatures.LiebreBase.SkeletonLeftArmID), Position,
                                LArmRect, color, npc.rotation, Origin, npc.scale, seffects);
                            dds.Insert(i, gdd);
                            LeftArmPlaced = true;
                        }
                        break;
                }
            }
        }

        private GuardianDrawData DrawEquippedScythe(Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            const int ScytheDiagonalHoldX = 12, ScytheDiagonalHoldY = 48;
            Vector2 ScythePosition = Base.RightHandPoints.GetPositionFromFrameVector(RightArmAnimationFrame);
            ScythePosition.X -= Base.SpriteWidth * 0.5f;
            if (npc.direction < 0)
                ScythePosition.X *= -1;
            ScythePosition.Y = -Base.SpriteHeight + ScythePosition.Y;
            ScythePosition *= npc.scale;
            ScythePosition += DrawPosition;
            SpriteEffects ScytheEffect = SpriteEffects.None;
            float ScytheRotation = 0f;
            byte ScytheType = 0;
            switch (RightArmAnimationFrame)
            {
                default:
                    ScytheType = 1;
                    break;
                case 16:
                    ScytheRotation = -1.57079633f * npc.direction;
                    break;
                case 17:
                    break;
            }
            Vector2 ScytheOrigin = new Vector2(ScytheDiagonalHoldX, ScytheDiagonalHoldY);
            if (npc.direction == 1)
            {
                switch (ScytheType)
                {
                    case 0:
                        ScytheEffect = SpriteEffects.FlipHorizontally;
                        ScytheOrigin.X = 66 - ScytheOrigin.X;
                        break;
                    case 1:
                        ScytheEffect = SpriteEffects.FlipHorizontally;
                        ScytheOrigin.X = 66 - ScytheOrigin.X;
                        break;
                }
            }
            Texture2D ScytheTexture = Base.sprites.GetExtraTexture(Creatures.LiebreBase.ScytheID);
            return new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, ScytheTexture, ScythePosition, new Rectangle(ScytheType * 66, 0, 66, 66), color, ScytheRotation, ScytheOrigin, Scale, ScytheEffect);
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public static bool CanSpawn
        {
            get
            {
                return NPC.downedBoss3;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CanSpawn && !spawnInfo.water && !Main.dayTime && !Main.bloodMoon && !NPC.AnyNPCs(ModContent.NPCType<LiebreNPC>()) && !NpcMod.HasGuardianNPC(GuardianBase.Liebre) && !NpcMod.HasMetGuardian(GuardianBase.Liebre))
            {
                Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
                if (Lighting.Brightness(spawnInfo.spawnTileX, spawnInfo.spawnTileY) < 0.15f)
                {
                    switch (EncounterTimes)
                    {
                        case 0:
                            if (!spawnInfo.player.ZoneDungeon && !spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson)
                            {
                                return 1f / 200;
                            }
                            break;
                        case 1:
                            if (spawnInfo.player.ZoneCorrupt || spawnInfo.player.ZoneCrimson)
                            {
                                return 1f / 250;
                            }
                            break;
                        case 2:
                            if (spawnInfo.player.ZoneDungeon)
                            {
                                return 1f / 300;
                            }
                            break;
                        case 3:
                            if(spawnInfo.playerInTown)
                                return 1f / 400;
                            break;
                    }
                    //Can spawn :D
                }
            }
            return base.SpawnChance(spawnInfo);
        }

        public override string GetChat()
        {
            if(PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Liebre))
            {
                string Text = "*Ah, you're here too. Well, now you know I'm here too.*";
                Main.npcChatText = Text;
                WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Liebre);
                NpcMod.AddGuardianMet(GuardianBase.Liebre);
                return "";
            }
            int EncounterTimes = LiebreNPC.EncounterTimes;
            if (FinishedTalking)
                EncounterTimes--;
            switch (EncounterTimes)
            {
                case 0:
                    if (!FinishedTalking)
                        return "*Please don't be affraid, I'm not after your soul or anything. I'm here because TerraGuardians have been moving here.*";
                    return "*I will be moving away from here soon, if you're worried.*";
                case 1:
                    if (FinishedTalking)
                        return "*I'm still intrigued about this place, so It may take a while before I leave.*";
                    return "*So, we meet again, Terrarian. What kind of place is this? " + (WorldGen.crimson ? "I feel the energy of a godly creature in this organic place" : "This place seems infested with sickness and parasites") + "*";
                case 2:
                    if (FinishedTalking)
                        return "*I still need some time to process this. I think better when I'm alone.*";
                    return "*What is this place? This place... It's so horrible.*";
                case 3:
                    if (FinishedTalking)
                        return "*Terrarian, haven't we introduced ourselves before?*";
                    return "*Terrarian, I have something to tell you.*";
            }
            return base.GetChat();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (FinishedTalking)
                return;
            switch (EncounterTimes)
            {
                case 0:
                    switch (DialogueStep)
                    {
                        case 0:
                            button = "You're here because of the TerraGuardians?";
                            break;
                        case 1:
                            button = "Where they belong?";
                            break;
                        case 2:
                            button = "How different?";
                            break;
                        case 3:
                            button = "I don't mind at all";
                            button2 = "Of course I mind! You're scary!";
                            break;
                    }
                    break;
                case 1:
                    switch (DialogueStep)
                    {
                        case 0:
                            button = "This is the " + (WorldGen.crimson ? "Crimson" : "Corruption") + ".";
                            break;
                        case 1:
                            button = "Continue";
                            break;
                        case 2:
                            button = "Exploring?";
                            break;
                        case 3:
                            button = "That's actually cool.";
                            break;
                    }
                    break;
                case 2:
                    switch (DialogueStep)
                    {
                        case 0:
                            button = "We call this the Dungeon.";
                            break;
                        case 1:
                            button = "I have no idea. Why?";
                            break;
                        case 2:
                            button = "I don't.";
                            break;
                        case 3:
                            button = "Huh?";
                            break;
                    }
                    break;
                case 3:
                    switch (DialogueStep)
                    {
                        case 0:
                            button = "S-something?!";
                            break;
                        case 1:
                            button = "Involve me?";
                            break;
                        case 2:
                            button = "You what?!";
                            break;
                        case 3:
                            button = "I-Includding T-Terrarians?!";
                            break;
                        case 4:
                            button = "Uh.. Okay...?";
                            break;
                    }
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (FinishedTalking)
                return;
            switch (EncounterTimes)
            {
                case 0:
                    switch (DialogueStep)
                    {
                        case 0:
                            DialogueStep = 1;
                            Main.npcChatText = "*Yes, I must ensure that in the case some of them ends up having their time up, I can take them where they belong.*";
                            break;
                        case 1:
                            DialogueStep = 2;
                            Main.npcChatText = "*Yes. Just like you, there is some place you'll be taken when you die. In the TerraGuardians case, it's a different place.*";
                            break;
                        case 2:
                            DialogueStep = 3;
                            Main.npcChatText = "*I will not tell you. Anyways, I hope you don't mind if I stay around your world for the time being.*";
                            break;
                        case 3:
                            DialogueStep = 4;
                            if (firstButton)
                                Main.npcChatText = "*Thank you. You will probably not even notice me around, or at least see me few times.*";
                            else
                                Main.npcChatText = "*Hmph. Don't worry, you wont notice me around. Maybe sometimes. Be sure to not stay wherever has a TerraGuardian about to die, then.*";
                            FinishedTalking = true;
                            EncounterTimes++;
                            break;
                    }
                    break;
                case 1:
                    switch (DialogueStep)
                    {
                        case 0:
                            DialogueStep = 1;
                            if (WorldGen.crimson)
                                Main.npcChatText = "*The Crimson? Hm, the name actually makes sense.*";
                            else
                                Main.npcChatText = "*The Corruption? Hm, interesting choice of name.*";
                            break;
                        case 1:
                            DialogueStep = 2;
                            Main.npcChatText = "*I was exploring this world, since there isn't a need for me to take any action right now.*";
                            break;
                        case 2:
                            DialogueStep = 3;
                            Main.npcChatText = "*Yes. Whenever I'm not necessary, I like to explore the world I will harvest from. That way I don't feel bored.*";
                            break;
                        case 3:
                            DialogueStep = 4;
                            Main.npcChatText = "*Indeed it is. Anyways, I will return to my travels.*";
                            FinishedTalking = true;
                            EncounterTimes++;
                            break;
                    }
                    break;
                case 2:
                    switch (DialogueStep)
                    {
                        case 0:
                            DialogueStep = 1;
                            Main.npcChatText = "*Who built this place?*";
                            break;
                        case 1:
                            DialogueStep = 2;
                            Main.npcChatText = "*Can't you feel? The grudge, the horrors, the despair... All those souls...*";
                            break;
                        case 2:
                            DialogueStep = 3;
                            Main.npcChatText = "*I can understand that... You're not like me anyways...*";
                            break;
                        case 3:
                            DialogueStep = 4;
                            Main.npcChatText = "*I'm sorry... But leave me be for a while... I need to process all this..*";
                            FinishedTalking = true;
                            EncounterTimes++;
                            break;
                    }
                    break;
                case 3:
                    switch (DialogueStep)
                    {
                        case 0:
                            DialogueStep = 1;
                            Main.npcChatText = "*At ease, Terrarian, It isn't particularly about you, beside It will involve you.*";
                            break;
                        case 1:
                            DialogueStep = 2;
                            Main.npcChatText = "*I spoke to the ones who sent me here about the dungeon, and they appointed me to act in this entire world.*";
                            break;
                        case 2:
                            DialogueStep = 3;
                            Main.npcChatText = "*Yes. They want to avoid more souls from being twisted, by whatever caused that in the dungeon, so I will be making delivery of souls of the deceased from here, includding Terrarians.*";
                            break;
                        case 3:
                            DialogueStep = 4;
                            Main.npcChatText = "*Don't worry, I will not harm anyone, at least unless something threatens to attack me.*";
                            break;
                        case 4:
                            EncounterTimes++;
                            DialogueStep = 5;
                            Main.npcChatText = "*I guess we will have enough time to know each other. You can call me Liebre, which was my name. I am now Terra Realm's reaper.*";
                            PlayerMod.AddPlayerGuardian(Main.LocalPlayer, GuardianBase.Liebre);
                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Liebre);
                            NpcMod.AddGuardianMet(GuardianBase.Liebre);
                            return;
                    }
                    break;
            }
        }
    }
}
