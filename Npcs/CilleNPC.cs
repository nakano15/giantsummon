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
    public class CilleNPC : GuardianActorNPC
    {
        public static bool MetAtLeastOnce = false;
        private byte TalkTimes = 255, DialogueStep = 0;
        private ushort RaceStartDelay = 0;
        private bool PlayerMetCille = false;

        public CilleNPC() : base(GuardianBase.Cille)
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cheetah Guardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.rarity = 1;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.dontCountMe = true;
        }

        public override bool CanChat()
        {
            return TalkTimes != 3;
        }

        public override void AI()
        {
            if(TalkTimes == 255)
            {
                TalkTimes = 0;
                Main.NewText("A TerraGuardian was spotted " + GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[Main.myPlayer].Center) + " of " + Main.player[Main.myPlayer].name + "'s position.");
            }
            if(Main.netMode < 2 && Main.player[Main.myPlayer].talkNPC == -1)
            {
                if(DialogueStep > 0)
                {
                    DialogueStep = 0;
                    if (TalkTimes < 2)
                    {
                        TalkTimes++;
                    }
                }
                if(TalkTimes == 3 && RaceStartDelay > 0)
                {
                    if(DialogueStep == 0)
                    {
                        DialogueStep = 1;
                        SayMessage("*Ready?*");
                        Main.player[Main.myPlayer].AddBuff(Terraria.ID.BuffID.Frozen, RaceStartDelay);
                        Main.player[Main.myPlayer].Bottom = npc.Bottom;
                    }
                    if(RaceStartDelay > 0)
                    {
                        RaceStartDelay--;
                        if(RaceStartDelay == 0)
                        {
                            SayMessage("*Go!*");
                        }
                    }
                }
            }
            if (TalkTimes == 3 && RaceStartDelay == 0)
            {
                int CheckX = (int)(npc.Center.X * (1f / 16)), CheckY = (int)((npc.position.Y + npc.height - 1) * (1f / 16));
                byte OpenSpace = 0;
                for (int x = 2; x < 4; x++)
                {
                    for (int y = 5; y > 0; y--)
                    {
                        int TileX = CheckX + x * npc.direction, TileY = CheckY - y;
                        Tile tile = Main.tile[TileX, TileY];
                        if (tile != null)
                        {
                            if (tile.active() && Main.tileSolid[tile.type])
                            {
                                OpenSpace = 0;
                            }
                            else
                            {
                                OpenSpace++;
                                if (OpenSpace >= 3)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (OpenSpace < 3)
                {
                    npc.direction *= -1;
                }
                if (npc.direction < 0)
                {
                    MoveLeft = true;
                }
                else
                {
                    MoveRight = true;
                }
                if (npc.velocity.Y == 0)
                {
                    bool HasGroundUnder = false;
                    for (int x = 0; x < 2; x++)
                    {
                        int TileX = CheckX + 1 + x * npc.direction, TileY = CheckY + 1;
                        if (Main.tile[TileX, TileY] != null && Main.tile[TileX, TileY].active() && Main.tileSolid[Main.tile[TileX, TileY].type])
                        {
                            HasGroundUnder = true;
                            break;
                        }
                    }
                    if (!HasGroundUnder)
                        Jump = true;
                }
                //Check who's far away from the other.
                if (Main.netMode < 2)
                {
                    float DistanceDiference = Math.Abs(npc.Center.X - Main.player[Main.myPlayer].Center.X);
                    if(DistanceDiference > 500)
                    {
                        if(npc.direction < 0)
                        {
                            if((npc.direction < 0 && Main.player[Main.myPlayer].Center.X > npc.position.X) ||
                                (npc.direction > 0 && Main.player[Main.myPlayer].Center.X < npc.position.X))
                            {
                                TalkTimes = 5;
                                Main.NewText("Cheetah Guardian: *You lost. Now please leave me alone.*");
                            }
                            else
                            {
                                TalkTimes = 6;
                                Main.NewText("Cheetah Guardian: *Alright, you won... Come talk to me..*");
                            }
                        }
                    }
                }
            }
            Idle = Walk = TalkTimes != 3;
            base.AI();
        }

        public override string GetChat()
        {
            PlayerMetCille = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
            switch (TalkTimes)
            {
                case 0:
                    if (PlayerMetCille)
                        return "*I remember you.. You're that Terrarian.*";
                    if (MetAtLeastOnce)
                    {
                        return "*You again... Please, leave me alone.*";
                    }
                    return "*...Go away.. Please...*";
                case 1:
                    if (PlayerMetCille)
                    {
                        TalkTimes = 8;
                        return "*You still persist into talking with me..*";
                    }
                    return "*You again...*";
                case 2:
                    return "*You really want to talk to me... Right..?*";
                case 4:
                    return "*W-what? How did you... Please, go away.*";
                case 5:
                    return "*Please, leave me alone.*";
                case 6:
                    return "*I never expected someone to win a race against me...*";
                case 7:
                    return "*I want to be alone now..*";
                case 8:
                    return "*Could you leave me alone?*";
                case 9:
                    return "*...*";
            }
            return base.GetChat();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (TalkTimes)
            {
                case 0:
                    if (PlayerMetCille)
                    {
                        button = "You're here too.";
                    }
                    else
                    {
                        switch (DialogueStep)
                        {
                            case 0:
                                button = "Who are you?";
                                break;
                        }
                    }
                    break;

                case 1:
                    switch (DialogueStep)
                    {
                        case 0:
                            button = "Why you're avoiding talking to me?";
                            break;
                        case 1:
                            button = "That's actually really hostile";
                            break;
                        case 2:
                            button = "Hurting people? Why?";
                            break;
                    }
                    break;

                case 2:
                    switch (DialogueStep)
                    {
                        case 0:
                            button = "Yes, I want.";
                            break;
                        case 1:
                            button = "Sure thing";
                            button2 = "A race?";
                            break;
                        case 2:
                            button = "Yes";
                            button2 = "No";
                            break;
                    }
                    break;

                case 6:
                    if (DialogueStep == 0)
                    {
                        button = "Will we finally be able to talk?";
                    }
                    break;

                case 7:
                    if (DialogueStep == 0)
                    {
                        button = "At least give me your name.";
                    }
                    break;

                case 8:
                    button = "You didn't changed at all..";
                    break;
            }
        }

        public override bool CheckActive()
        {
            return TalkTimes < 3 || TalkTimes > 6;
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            string NewDialogueMessage = null;
            switch (TalkTimes)
            {
                case 0:
                    {
                        switch (DialogueStep)
                        {
                            case 0:
                                if (PlayerMetCille)
                                {
                                    NewDialogueMessage = "*Yes.. Still, please leave me be.*";
                                    DialogueStep = 0;
                                    TalkTimes = 8;
                                }
                                else
                                {
                                    NewDialogueMessage = "*Nobody. Please, go away..*";
                                    DialogueStep++;
                                }
                                MetAtLeastOnce = true;
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        switch (DialogueStep)
                        {
                            case 0:
                                NewDialogueMessage = "*...Because I might hurt you..*";
                                DialogueStep++;
                                break;
                            case 1:
                                NewDialogueMessage = "*No... It's not that... I tend to end up hurting people I'm close to...*";
                                DialogueStep++;
                                break;
                            case 2:
                                NewDialogueMessage = "*I really don't want to talk to you.. Please, leave me alone.*";
                                DialogueStep++;
                                break;
                        }
                    }
                    break;
                case 2:
                    {
                        switch (DialogueStep)
                        {
                            case 0:
                                NewDialogueMessage = "*Okay. What about this. I really miss racing against someone. Could you beat me on a race?*";
                                DialogueStep++;
                                break;
                            case 1:
                                if (firstButton)
                                {
                                    NewDialogueMessage = "*It's really simple. If you run ahead of me enough time so I can't catch up, you win. I win if I manage to do that to you. What do you say?*";
                                }
                                else
                                {
                                    NewDialogueMessage = "*Back then, I used to love racing against other people.. It's just a simple race, if you manage to run ahead of me enough so I can't catch up, you win. I win if I do the that to you. Would you race against me?*";
                                }
                                DialogueStep++;
                                break;
                            case 2:
                                if (firstButton)
                                {
                                    TalkTimes = 3;
                                    DialogueStep = 0;
                                    RaceStartDelay = 3 * 60;
                                    NewDialogueMessage = "*Once you stop talking to me, I'll start the race in 3 seconds.*";
                                }
                                else
                                {
                                    NewDialogueMessage = "*Then leave me alone.*";
                                    DialogueStep++;
                                }
                                break;
                        }
                    }
                    break;
                case 6:
                    {
                        switch (DialogueStep)
                        {
                            case 0:
                                {
                                    DialogueStep++;
                                    NewDialogueMessage = "*No. Please, leave me alone.*";
                                    NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                                    TalkTimes = 7;
                                    PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
                                }
                                break;
                        }
                    }
                    break;
                case 7:
                    {
                        switch (DialogueStep)
                        {
                            case 0:
                                {
                                    DialogueStep++;
                                    NewDialogueMessage = "*Cille...*";
                                }
                                break;
                        }
                    }
                    break;
                case 8:
                    {
                        NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                        TalkTimes = 9;
                        Main.npcChatText = "*... Sorry... You're not safe while close to me..*";
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                    }
                    break;
            }
            if (NewDialogueMessage != null)
                Main.npcChatText = NewDialogueMessage;
        }

        public override void ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, SpriteEffects seffects)
        {
            GuardianDrawData outfitbody = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Base.sprites.GetExtraTexture(Companions.CilleBase.DefaultOutfitBodyID),
                Position, BodyRect, color, npc.rotation, Origin, npc.scale, seffects);
            GuardianDrawData outfitleftarm = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Base.sprites.GetExtraTexture(Companions.CilleBase.DefaultOutfitLeftArmID),
                Position, LArmRect, color, npc.rotation, Origin, npc.scale, seffects);
            GuardianDrawData outfitrightarm = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Base.sprites.GetExtraTexture(Companions.CilleBase.DefaultOutfitRightArmID),
                Position, RArmRect, color, npc.rotation, Origin, npc.scale, seffects);
            for(int i = dds.Count - 1; i >= 0; i--)
            {
                switch (dds[i].textureType)
                {
                    case GuardianDrawData.TextureType.TGLeftArm:
                        dds.Insert(i + 1, outfitleftarm);
                        break;
                    case GuardianDrawData.TextureType.TGRightArm:
                        dds.Insert(i + 1, outfitrightarm);
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        dds.Insert(i + 1, outfitbody);
                        break;
                }
            }
        }

        public static bool CanSpawn()
        {
            bool PlayerWithAtLeastOneLifeCrystal = false;
            for(int i = 0; i < 255; i++)
            {
                if(Main.player[i].active && Main.player[i].statLifeMax > 100)
                {
                    PlayerWithAtLeastOneLifeCrystal = true;
                    break;
                }
            }
            return PlayerWithAtLeastOneLifeCrystal;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.playerInTown && Main.dayTime && !Main.eclipse && !NpcMod.HasMetGuardian(GuardianID,GuardianModID) && 
                !NpcMod.HasGuardianNPC(GuardianID, GuardianModID) && !NPC.AnyNPCs(ModContent.NPCType<CilleNPC>()) && CanSpawn() &&
                Main.moonPhase != 0 && Main.moonPhase != 4)
            {
                return 1f / 200;
            }
            return 0;
        }
    }
}
