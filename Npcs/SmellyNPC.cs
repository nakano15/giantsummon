using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    class SmellyNPC : GuardianActorNPC
    {
        private byte DialogueStep = 1;
        public const int BarCount = 15;
        private bool HasMaterials = false, MetBefore = false;

        public SmellyNPC() : 
            base(GuardianBase.CaptainStench)
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
        }

        public bool PlayerHasMaterials()
        {
            return Main.player[Main.myPlayer].CountItem(Terraria.ID.ItemID.GoldBar) >= BarCount ||
                Main.player[Main.myPlayer].CountItem(Terraria.ID.ItemID.PlatinumBar) >= BarCount;
        }

        public override bool CanChat()
        {
            return true;
        }

        public override string GetChat()
        {
            switch (DialogueStep)
            {
                case 1:
                    if(MetBefore = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID))
                    {
                        return "ahoy there mate, are you back to recruit me on your ventures yeah?";
                    }
                    return "Hello there stranger, I'm captain stench and my ship was hit by this big rock here while I was scouting this planet.My cadets were tragically killed during the crash and im the only survivor.";
                case 2:
                    return "Before landing here I was scouting this planet and there seems to be lots of rare and useful materials underneath the soil, yeah ? ";
                case 3:
                    return "Do you by any chance have some rare metals like gold or platinum?";
                case 4:
                    HasMaterials = PlayerHasMaterials();
                    return "If you do it would really help me out as my equipment is damaged from the crash and i am unable to defend myself. If you give me 15 gold or platinum bars I will be in debt to you and a VERY useful companion if needed.";
            }
            return "";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (DialogueStep)
            {
                case 1:
                    if (MetBefore)
                        button = "Aye";
                    else
                        button = "Continue";
                    break;
                case 3:
                    button = "Continue";
                    break;
                case 2:
                    button = "Yes there is";
                    break;
                case 4:
                    if (HasMaterials)
                    {
                        button = "Give Materials";
                    }
                    else
                    {
                        button = "I dont have the materials right now";
                    }
                    button2 = "No";
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            switch (DialogueStep)
            {
                case 1:
                    if (MetBefore)
                    {
                        NpcMod.AddGuardianMet(GuardianBase.CaptainStench);
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.CaptainStench);
                    }
                    else
                    {
                        DialogueStep = 2;
                        Main.npcChatText = GetChat();
                    }
                    break;
                case 2:
                    DialogueStep = 3;
                    Main.npcChatText = GetChat();
                    break;
                case 3:
                    DialogueStep = 4;
                    Main.npcChatText = GetChat();
                    break;
                case 4:
                    if (firstButton && HasMaterials)
                    {
                        Main.npcChatText = "Thank you, these materials will be put to good use. Rest assured you won't be disappointed. Now then im ready for adventure when you are.";
                        int BarsLeftToRemove = BarCount;
                        Player player = Main.player[Main.myPlayer];
                        if (Main.player[Main.myPlayer].CountItem(Terraria.ID.ItemID.GoldBar) >= BarCount)
                        {
                            const int ToRemoveItemID = Terraria.ID.ItemID.GoldBar;
                            for (int i = 0; i < 50; i++)
                            {
                                if(player.inventory[i].type  == ToRemoveItemID)
                                {
                                    int ToDiscount = BarsLeftToRemove;
                                    if (ToDiscount > player.inventory[i].stack)
                                        ToDiscount = player.inventory[i].stack;
                                    player.inventory[i].stack -= ToDiscount;
                                    BarsLeftToRemove -= ToDiscount;
                                    if (player.inventory[i].stack == 0)
                                        player.inventory[i].SetDefaults(0);
                                    if (BarsLeftToRemove <= 0)
                                        break;
                                }
                            }
                        }
                        else
                        {
                            const int ToRemoveItemID = Terraria.ID.ItemID.PlatinumBar;
                            for (int i = 0; i < 50; i++)
                            {
                                if (player.inventory[i].type == ToRemoveItemID)
                                {
                                    int ToDiscount = BarsLeftToRemove;
                                    if (ToDiscount > player.inventory[i].stack)
                                        ToDiscount = player.inventory[i].stack;
                                    player.inventory[i].stack -= ToDiscount;
                                    BarsLeftToRemove -= ToDiscount;
                                    if (player.inventory[i].stack == 0)
                                        player.inventory[i].SetDefaults(0);
                                    if (BarsLeftToRemove <= 0)
                                        break;
                                }
                            }
                        }
                        PlayerMod.AddPlayerGuardian(player, GuardianBase.CaptainStench);
                        NpcMod.AddGuardianMet(GuardianBase.CaptainStench);
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.CaptainStench);
                    }
                    else
                    {
                        Main.npcChatText = "Oh that's unfortunate. IF you ever stumble apon the metals please consider supplying them to me as I will be left stranded and defenseless here on a foriegn planet.";
                        DialogueStep = 5;
                    }
                    break;
            }
        }

        private bool FirstFrame = true;

        public override void AI()
        {
            if (FirstFrame)
            {
                if (Main.rand.NextFloat() < 0.99f)
                {
                    Main.NewText(Main.player[Main.myPlayer].name + " noticed a cloaked figure " + GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[Main.myPlayer].Center) + " of their position.");
                }
                else
                {
                    Main.NewText("A cloaked figure to the "+ GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[Main.myPlayer].Center) + " has came to skunk you.");
                }

                FirstFrame = false;
            }
            bool SomeoneTalkingToMe = false;
            for(int i = 0; i < 255; i++)
            {
                if(Main.player[i].active && Main.player[i].talkNPC == npc.whoAmI)
                {
                    SomeoneTalkingToMe = true;
                    break;
                }
            }
            if (DialogueStep > 1 && !SomeoneTalkingToMe)
                DialogueStep = 1;
            Idle = true;
            base.AI();
        }

        public override void ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, SpriteEffects seffects)
        {
            if (npc.direction > -1)
            {
                GuardianDrawData Scouter = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Base.sprites.GetExtraTexture(Creatures.CaptainStenchBase.ScouterTextureID), Position, BodyRect, color, npc.rotation, Origin, npc.scale, seffects);
                for(int i = 0; i < dds.Count; i++)
                {
                    if(dds[i].textureType == GuardianDrawData.TextureType.TGBody)
                    {
                        dds.Insert(i+1, Scouter);
                        break;
                    }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.water && !NpcMod.HasGuardianNPC(GuardianID) && !NpcMod.HasMetGuardian(GuardianID) && spawnInfo.player.ZoneMeteor && !NPC.AnyNPCs(ModContent.NPCType<SmellyNPC>()))
            {
                return 1f / 6;
            }
            return 0;
        }
    }
}
