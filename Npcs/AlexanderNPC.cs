using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Npcs
{
    public class AlexanderNPC : GuardianActorNPC
    {
        private int NpcRecruitStep = IdleStep, DialogueDuration = 0;

        public AlexanderNPC() : base(GuardianBase.Alexander, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = true;
            npc.friendly = true;
            npc.knockBackResist = 0;
            npc.dontCountMe = true;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            switch (NpcRecruitStep)
            {
                case InvestigatingPlayerStep:
                    {
                        Player player = Main.player[npc.target];
                        if (player.GetModPlayer<PlayerMod>().KnockedOut)
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.ReviveFrame;
                        }
                        else
                        {
                            int AnimationDuration = DialogueDuration % 180;
                            if (AnimationDuration >= 70)
                            {
                                if ((AnimationDuration >= 75 && AnimationDuration < 80) || 
                                    (AnimationDuration >= 85 && AnimationDuration < 90) ||
                                    (AnimationDuration >= 95 && AnimationDuration < 100) ||
                                    (AnimationDuration >= 105 && AnimationDuration < 110) ||
                                    (AnimationDuration >= 115 && AnimationDuration < 120) ||
                                    (AnimationDuration >= 125 && AnimationDuration < 130))
                                {
                                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 29;
                                }
                                else
                                {
                                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 28;
                                }
                            }
                            else
                            {
                                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 27;
                            }
                        }
                    }
                    break;
            }
        }

        public override void AI()
        {
            int NextStep = NpcRecruitStep;
            switch (NpcRecruitStep)
            {
                case IdleStep:
                    {
                        Idle = true;
                        Rectangle SightRange = new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 300, 300);
                        SightRange.Y -= (int)(SightRange.Height * 0.5f);
                        if (npc.direction < 0)
                            SightRange.X -= SightRange.Width;
                        for(int p = 0; p < 255; p++)
                        {
                            if(Main.player[p].active && !Main.player[p].dead)
                            {
                                if(Main.player[p].getRect().Intersects(SightRange) && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[p].position, Main.player[p].width, Main.player[p].height))
                                {
                                    npc.target = p;
                                    NextStep = CallingOutPlayerStep;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case CallingOutPlayerStep:
                    {
                        Idle = false;
                        if(DialogueDuration == 0)
                        {
                            if (Main.player[npc.target].GetModPlayer<PlayerMod>().KnockedOut)
                            {
                                SayMessage("*You there! Hang on!*");
                            }
                            else
                            {
                                SayMessage("*Hey! You there! Stop!*");
                            }
                        }
                        if(DialogueDuration >= 150)
                        {
                            NextStep = ChasingPlayerStep;
                        }
                        else
                        {
                            DialogueDuration++;
                        }
                        if (!Main.player[npc.target].active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Hm... " + (Main.player[npc.target].Male ? "He" : "She") + " disappeared...*");
                        }
                        else if (Main.player[npc.target].dead)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Well... That's... Horrible...*");
                        }
                    }
                    break;
                case ChasingPlayerStep:
                    {
                        Player player = Main.player[npc.target];
                        if (player.Center.X < npc.Center.X)
                            MoveLeft = true;
                        else
                            MoveRight = true;
                        if (!player.GetModPlayer<PlayerMod>().KnockedOut && npc.velocity.Y == 0 && Math.Abs(player.Center.X - npc.Center.X) < 80)
                        {
                            Jump = true;
                        }
                        if (player.getRect().Intersects(npc.getRect()))
                        {
                            NextStep = InvestigatingPlayerStep;
                        }
                        if (!player.active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Hm... " + (Main.player[npc.target].Male ? "He" : "She") + " disappeared...*");
                        }
                        else if (player.dead)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Damn, I'm too late...*");
                        }
                    }
                    break;
                case InvestigatingPlayerStep:
                    {
                        if (DialogueDuration == 0)
                            FindFrame(0);
                        Player player = Main.player[npc.target];
                        player.immuneTime = 5;
                        player.immuneNoBlink = true;
                        Vector2 PlayerPosition = Base.LeftHandPoints.GetPositionFromFrameVector(LeftArmAnimationFrame);
                        player.fullRotationOrigin = new Vector2(40, 56) * 0.5f;
                        if (npc.direction > 0)
                        {
                            player.fullRotation = 1.570796f;
                            player.direction = -1;
                        }
                        else
                        {
                            player.fullRotation = -1.570796f;
                            PlayerPosition.X = (Base.SpriteWidth * 0.5f) - PlayerPosition.X;
                            player.direction = 1;
                        }
                        PlayerPosition += npc.position;
                        PlayerPosition.X += npc.width * 0.5f;
                        PlayerPosition.Y = npc.position.Y + npc.height - 20 + player.height * 0.5f;
                        player.velocity = Vector2.Zero;
                        player.Center = PlayerPosition;
                        player.statLife++;
                        if (player.mount.Active)
                            player.mount.Dismount(player);
                        player.gfxOffY = 0;
                        player.AddBuff(Terraria.ID.BuffID.Cursed, 5);
                        if (player.GetModPlayer<PlayerMod>().KnockedOut)
                        {
                            if (DialogueDuration == 0)
                            {
                                SayMessage("*Better I take care of those wounds first.*");
                            }
                            if (DialogueDuration > 1)
                                DialogueDuration = 1;
                            player.GetModPlayer<PlayerMod>().ReviveBoost++;
                        }
                        else
                        {
                            if (DialogueDuration % 180 == 0)
                            {
                                switch(DialogueDuration / 180)
                                {
                                    case 0:
                                        SayMessage("*Got you! Now don't move.*");
                                        break;
                                    case 1:
                                        SayMessage("*Hm...*");
                                        break;
                                    case 2:
                                        SayMessage("*Interesting...*");
                                        break;
                                    case 3:
                                        SayMessage("*Uh huh...*");
                                        break;
                                    case 4:
                                        SayMessage("*But you're not the one I'm looking for...*");
                                        break;
                                    case 5:
                                        NextStep = IntroductingToPlayerStep;
                                        player.fullRotation = 0;
                                        player.fullRotationOrigin = new Vector2(40, 56) * 0.5f;
                                        player.position = npc.Bottom;
                                        player.position.Y -= player.height;
                                        break;
                                }
                            }
                        }
                        DialogueDuration++;
                        if (!player.active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*" + (Main.player[npc.target].Male ? "He" : "She") + " disappeared!*");
                        }
                    }
                    break;

                case IntroductingToPlayerStep:
                    {
                        if(DialogueDuration == 0)
                        {
                            Main.player[npc.target].talkNPC = npc.whoAmI;
                            bool HasMetTerraGuardians = false;
                            foreach (GuardianData gd in Main.player[npc.target].GetModPlayer<PlayerMod>().MyGuardians.Values)
                            {
                                if (gd.Base.IsTerraGuardian)
                                {
                                    HasMetTerraGuardians = true;
                                    break;
                                }
                            }
                            Main.npcChatText = "*Terrarian, "+(Main.player[npc.target].Male ? "Male" : "Female")+", "+(HasMetTerraGuardians ? "and you seems to have met some TerraGuardians.*" : "and It must be a shock for you to see me.*");
                            DialogueDuration = 1;
                        }
                        if (!Main.player[npc.target].active)
                            npc.TargetClosest(false);
                        if(DialogueDuration > 1 && Main.player[npc.target].talkNPC != npc.whoAmI)
                        {
                            DialogueDuration = 1;
                        }
                    }
                    break;
            }
            if(NextStep != NpcRecruitStep)
            {
                NpcRecruitStep = NextStep;
                DialogueDuration = 0;
            }
            base.AI();
        }

        public override bool CanChat()
        {
            return NpcRecruitStep == 5;
        }

        public override string GetChat()
        {
            return "**";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (DialogueDuration)
            {
                case 1:
                    button = "Why did you jumped on me?";
                    button2 = "If you wanted to know that, you could have just asked.";
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            switch (DialogueDuration)
            {
                case 1:
                    break;
            }
        }

        private const byte IdleStep = 0,
            CallingOutPlayerStep = 1,
            ChasingPlayerStep = 2,
            InvestigatingPlayerStep = 3,
            IntroductingToPlayerStep = 4;
    }
}
