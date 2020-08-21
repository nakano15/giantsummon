using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class WrathNPC : GuardianActorNPC
    {
        public bool WentBersek = false, PlayerLost = false, Defeated = false;
        public byte BodySlamResist = 0;
        public ModTargetting Target = new ModTargetting();
        public int ActionTime = 0;
        public Behaviors behavior = Behaviors.Charge;
        public Vector2 ChargeDashDestination = Vector2.Zero;
        public bool FallHurt = false;
        public byte DialogueStep = 0;
        public bool LastButtonWasAccept = false;
        public bool ForceLeave = false;

        public WrathNPC()
            : base(GuardianBase.Wrath, "", "Angry Pig Cloud")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.friendly = false;
            npc.dontTakeDamageFromHostiles = true;
            npc.dontTakeDamage = true;
            npc.knockBackResist = 0.3f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angry Pig Cloud");
        }

        bool PlayerHasWrath { get { return npc.target < 255 && PlayerMod.PlayerHasGuardian(Main.player[npc.target], GuardianBase.Wrath); } }

        public override void AI()
        {
            if (ForceLeave)
            {
                if (Target.Character.talkNPC != npc.whoAmI)
                {
                    if (Target.Center.X < npc.Center.X)
                    {
                        MoveLeft = true;
                    }
                    else
                    {
                        MoveRight = true;
                    }
                }
            }
            else if (Defeated)
            {
                npc.friendly = true;
                npc.dontTakeDamageFromHostiles = true;
                npc.dontTakeDamage = true;
            }
            else if (!WentBersek)
            {
                npc.friendly = false;
                npc.dontTakeDamageFromHostiles = true;
                npc.dontTakeDamage = true;
                npc.TargetClosest(false);
                if (npc.target < 255 && IsInPerceptionRange(Main.player[npc.target], 512, 360) && Collision.CanHitLine(Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height,
                    npc.position, npc.width, npc.height))
                {
                    Target.SetTargetToPlayer(Main.player[npc.target]);
                    WentBersek = true;
                    PigStatus(out npc.lifeMax, out npc.damage, out npc.defense);
                    npc.life = npc.lifeMax;
                    npc.defDamage = npc.damage;
                    npc.defDefense = npc.defense;
                    behavior = Behaviors.Charge;
                    ActionTime = 0;
                    if (!PlayerHasWrath)
                    {
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                SayMessage("*You look like the perfect person for me to discount my rage.*");
                                break;
                            case 1:
                                SayMessage("*I'm so glad to see you, time to unleash my rage!*");
                                break;
                            case 2:
                                SayMessage("*Grrr!! You're going to help me get rid of this rage, even if you don't want!*");
                                break;
                            case 3:
                                SayMessage("*You showed up in a bad moment.*");
                                break;
                            case 4:
                                SayMessage("*Grrrreat! You just stand there!*");
                                break;
                        }
                    }
                    else
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                SayMessage("*It's you again, I can use you to remove this unending anger!*");
                                break;
                            case 1:
                                SayMessage("*You appeared right in time, prepare for my attacks!*");
                                break;
                            case 2:
                                SayMessage("*Grr!! Help me!! I'm still burning out of rage!*");
                                break;
                        }
                    }
                }
                else
                {
                    Idle = true;
                }
            }
            else if (PlayerLost)
            {
                Idle = false;
                npc.friendly = true;
                npc.dontTakeDamageFromHostiles = true;
                npc.dontTakeDamage = true;
                npc.noTileCollide = false;
                npc.noGravity = false;
                if (BodySlamResist > 0)
                    BodySlamResist = 0;
                if (ActionTime > 0)
                {
                    ActionTime++;
                    if (ActionTime >= 150)
                    {
                        if (BodySlamResist > 0)
                        {
                            Player player = Target.Character;
                            player.Center = npc.Center;
                        }
                        if (PlayerHasWrath)
                        {
                            if (Target.IsKnockedOut)
                            {
                                Target.Character.GetModPlayer<PlayerMod>().ReviveBoost += 3;
                                if (!Target.IsKnockedOut)
                                {
                                    ActionTime = 1;
                                    SayMessage("*There! You look okay now.*");
                                }
                            }
                        }
                        else
                        {
                            if (Target.Center.X > npc.Center.X)
                            {
                                MoveLeft = true;
                            }
                            else
                            {
                                MoveRight = true;
                            }
                        }
                    }
                    else if(BodySlamResist > 0)
                    {
                        Player player = Target.Character;
                        player.Center = npc.Bottom;
                        player.fullRotation = -MathHelper.ToRadians(90) * player.direction;
                        player.fullRotationOrigin.X = player.width * 0.5f;
                        player.fullRotationOrigin.Y = player.height * 0.5f;
                    }
                }
                else if (Math.Abs(Target.Center.X - npc.Center.X) > 40 && BodySlamResist == 0)
                {
                    if (Target.Center.X < npc.Center.X)
                    {
                        MoveLeft = true;
                    }
                    else
                    {
                        MoveRight = true;
                    }
                }
                else
                {
                    ActionTime++;
                    if (PlayerHasWrath)
                    {
                        SayMessage("*It still didn't worked! Let me try helping you stand.*");
                    }
                    else
                    {
                        SayMessage("*It didn't worked! I'm still furious! I'm outta here.*");
                    }
                }
            }
            else
            {
                PigStatus(out npc.lifeMax, out npc.damage, out npc.defense);
                if (!IsInPerceptionRange(Target.Character))
                {
                    if (PlayerLost)
                    {
                        SayMessage("*Loser!*");
                    }
                    else
                    {
                        SayMessage("*Coward!*");
                    }
                    WentBersek = false;
                    PlayerLost = false;
                    Defeated = false;
                    BodySlamResist = 0;
                    ActionTime = 0;
                }
                else if (!Target.IsKnockedOut)
                {
                    Idle = false;
                    npc.friendly = false;
                    npc.dontTakeDamageFromHostiles = true;
                    npc.dontTakeDamage = false;
                    npc.noTileCollide = false;
                    npc.noGravity = false;
                    npc.damage = npc.defDamage;
                    Target.Character.GetModPlayer<PlayerMod>().FriendlyDuelDefeat = true;
                    switch (behavior)
                    {
                        case Behaviors.Charge:
                            {
                                npc.knockBackResist = 0.15f;
                                npc.damage = npc.defDamage;
                                if (Target.Position.X < npc.Center.X)
                                    MoveLeft = true;
                                else
                                    MoveRight = true;
                                if (Math.Abs(Target.Position.Y - npc.Center.Y) < 68)
                                {
                                    if (Target.Velocity.Y < 0 && npc.velocity.Y == 0)
                                    {
                                        npc.velocity.Y = -5f;
                                    }
                                }
                                ActionTime++;
                                if (ActionTime >= 360)
                                {
                                    if (Math.Abs(Target.Center.Y - npc.Center.Y) >= 120 || Math.Abs(Target.Center.X - npc.Center.X) >= 240 || Main.rand.Next(3) == 0)
                                    {
                                        behavior = Behaviors.DestructiveRush;
                                        ActionTime = 0;
                                    }
                                    else
                                    {
                                        behavior = Behaviors.BodySlam;
                                        ActionTime = 0;
                                        BodySlamResist = 0;
                                    }
                                }
                            }
                            break;
                        case Behaviors.PosJumpCooldown:
                            {
                                npc.knockBackResist = 0;
                                npc.friendly = true;
                                ActionTime++;
                                if (ActionTime >= 90)
                                {
                                    if (Main.rand.NextDouble() < 0.4)
                                        behavior = Behaviors.BulletReflectingBelly;
                                    else
                                        behavior = Behaviors.Charge;
                                    ActionTime = 0;
                                }
                            }
                            break;
                        case Behaviors.BodySlam:
                            {
                                npc.knockBackResist = 0;
                                ActionTime++;
                                npc.friendly = true;
                                if (ActionTime == 90)
                                {
                                    FallHurt = false;
                                    npc.velocity.Y = -20;
                                }
                                else if (ActionTime > 91)
                                {
                                    if (BodySlamResist == 0)
                                    {
                                        if (Target.Position.X < npc.Center.X)
                                            MoveLeft = true;
                                        else
                                            MoveRight = true;
                                    }
                                    if (npc.velocity.Y > 0 && Target.Position.Y > npc.Bottom.Y)
                                    {
                                        npc.noTileCollide = true;
                                    }
                                    npc.noGravity = true;
                                    if (npc.noTileCollide || !npc.collideY)
                                    {
                                        if (Main.expertMode)
                                        {
                                            npc.velocity.Y += 0.5f;
                                            if (npc.velocity.Y > 6f)
                                                npc.velocity.Y = 6f;
                                        }
                                        else
                                        {
                                            npc.velocity.Y += 0.4f;
                                            if (npc.velocity.Y > 5f)
                                                npc.velocity.Y = 5f;
                                        }
                                    }
                                    if (BodySlamResist > 0)
                                    {
                                        npc.noTileCollide = false;
                                        Player player = Target.Character;
                                        player.AddBuff(Terraria.ID.BuffID.Cursed, 3);
                                        DrawInFrontOfPlayers.Add(player.whoAmI);
                                        player.Center = npc.Bottom;
                                        player.fullRotation = -MathHelper.ToRadians(90) * player.direction;
                                        player.fullRotationOrigin.X = player.width * 0.5f;
                                        player.fullRotationOrigin.Y = player.height * 0.5f;
                                        if (npc.collideY)
                                        {
                                            if (!FallHurt)
                                            {
                                                FallHurt = true;
                                                player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " was flattened by " + npc.GivenOrTypeName + "."), npc.defDamage * 2, npc.direction, false, false, true);
                                                if (!player.dead && !Target.IsKnockedOut && player.whoAmI == Main.myPlayer)
                                                    Main.NewText("Press 'Jump' repeatedly to escape.");
                                                switch (Main.rand.Next(3))
                                                {
                                                    case 0:
                                                        SayMessage("*What is It? Too heavy for you?*");
                                                        break;
                                                    case 1:
                                                        SayMessage("*I think I heard your bone cracking.*");
                                                        break;
                                                    case 2:
                                                        SayMessage("*Feel the weight of my fury!*");
                                                        break;
                                                }
                                            }
                                            player.AddBuff(Terraria.ID.BuffID.Suffocation, 3);
                                            if (player.controlJump && !player.releaseJump)
                                            {
                                                BodySlamResist--;
                                                if (BodySlamResist == 0)
                                                {
                                                    behavior = Behaviors.PosJumpCooldown;
                                                    ActionTime = 0;
                                                    player.velocity.Y = -6f;
                                                    player.fullRotation = 0f;
                                                    player.fullRotationOrigin.X = 0;
                                                    player.fullRotationOrigin.Y = 0;
                                                }
                                            }
                                        }
                                    }
                                    else if (!npc.noTileCollide && npc.velocity.Y == 0)
                                    {
                                        behavior = Behaviors.PosJumpCooldown;
                                        ActionTime = 0;
                                    }
                                    else if (Target.GetCollision.Intersects(npc.getRect()))
                                    {
                                        BodySlamResist = 10;
                                    }
                                }
                            }
                            break;
                        case Behaviors.DestructiveRush:
                            {
                                npc.knockBackResist = 0;
                                if (ActionTime == 10)
                                {
                                    Vector2 EndPosition = (npc.position - Target.Position) * 0.5f;
                                    Vector2 Direction = EndPosition;
                                    Direction.Normalize();
                                    EndPosition = Target.Position + Direction * 32f;
                                    while (Collision.SolidCollision(EndPosition, npc.width, npc.height))
                                    {
                                        EndPosition += Direction * -16;
                                    }
                                    ChargeDashDestination = EndPosition;
                                    float MaxDist = ChargeDashDestination.Length() / 16;
                                    for (int dist = 0; dist < MaxDist; dist++)
                                    {
                                        float Percentage = (float)dist / MaxDist;
                                        Vector2 DotPosition = npc.position + (ChargeDashDestination - npc.position) * Percentage;
                                        Dust.NewDust(DotPosition, npc.width, npc.height, Terraria.ID.DustID.SomethingRed, 1f - ((float)Main.rand.NextDouble() * 2), 1f - ((float)Main.rand.NextDouble() * 2));
                                    }
                                    npc.direction = ChargeDashDestination.X < npc.position.X ? -1 : 1;
                                }
                                else if (ActionTime == 55)
                                {
                                    float MaxDist = ChargeDashDestination.Length() / 5;
                                    Rectangle rect = npc.getRect(), PlayerRect = Target.GetCollision;
                                    for (int dist = 0; dist < MaxDist; dist++)
                                    {
                                        float Percentage = (float)dist / MaxDist;
                                        Vector2 DotPosition = npc.position + (ChargeDashDestination - npc.position) * Percentage;
                                        rect.X = (int)DotPosition.X;
                                        rect.Y = (int)DotPosition.Y;
                                        if (rect.Intersects(PlayerRect))
                                        {
                                            Target.Hurt((int)(npc.damage * 2.5f), npc.direction, " couldn't endure the Destructive Rush.");
                                        }
                                    }
                                    Vector2 Direction = (ChargeDashDestination - npc.position);
                                    Direction.Normalize();
                                    npc.position = ChargeDashDestination;
                                    npc.velocity = Direction * 6f;
                                }
                                else if (ActionTime >= 100)
                                {
                                    behavior = Behaviors.PosJumpCooldown;
                                    ActionTime = 0;
                                }
                                ActionTime++;
                            }
                            break;
                        case Behaviors.BulletReflectingBelly:
                            if (ActionTime == 30)
                            {
                                SayMessage("*Go ahead, try shootting at me.*");
                            }
                            if (ActionTime >= 30)
                            {
                                npc.reflectingProjectiles = true;
                                npc.defense *= 2;
                            }
                            if (ActionTime >= 240)
                            {
                                npc.reflectingProjectiles = false;
                                behavior = Behaviors.PosJumpCooldown;
                                ActionTime = 0;
                            }
                            ActionTime++;
                            break;
                    }
                }
                else
                {
                    PlayerLost = true;
                    ActionTime = 0;
                }
            }
            base.AI();
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            bool CloudForm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID];
            if (!ForceLeave && Defeated)
            {
                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = (CloudForm ? 24 : 15);
            }
            else if (!ForceLeave && PlayerLost && ActionTime < 150 && BodySlamResist > 0)
            {
                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = (CloudForm ? 23 : 16);
            }
            else if (!ForceLeave && Target.IsKnockedOut && PlayerLost && PlayerHasWrath && npc.velocity.X == 0)
            {
                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = (CloudForm ? 25 : 17);
            }
            else if (!WentBersek || ForceLeave)
            {
                if (CloudForm)
                {
                    if (npc.velocity.X != 0)
                    {
                        if ((npc.velocity.X < 0 && npc.direction > 0) || (npc.velocity.X > 0 && npc.direction < 0))
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 21;
                        }
                        else
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 20;
                        }
                    }
                    else
                    {
                        BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 19;
                    }
                    if (npc.velocity.Y != 0)
                    {
                        LeftArmAnimationFrame = RightArmAnimationFrame = 9;
                    }
                }
            }
            else
            {
                switch (behavior)
                {
                    case Behaviors.Charge:
                        if (CloudForm)
                        {
                            if (npc.velocity.X != 0)
                            {
                                if ((npc.velocity.X < 0 && npc.direction > 0) || (npc.velocity.X > 0 && npc.direction < 0))
                                {
                                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 21;
                                }
                                else
                                {
                                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 20;
                                }
                            }
                            else
                            {
                                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 19;
                            }
                            if (npc.velocity.Y != 0)
                            {
                                LeftArmAnimationFrame = RightArmAnimationFrame = 9;
                            }
                        }
                        break;
                    case Behaviors.BodySlam:
                        if (ActionTime < 90)
                        {
                            byte Frame = 0;
                            if (ActionTime >= 80)
                            {
                                Frame = 1;
                            }
                            else if (ActionTime >= 70)
                            {
                                Frame = 2;
                            }
                            else if (ActionTime >= 30)
                            {
                                Frame = 3;
                            }
                            else if (ActionTime >= 20)
                            {
                                Frame = 2;
                            }
                            else if (ActionTime >= 10)
                            {
                                Frame = 1;
                            }
                            LeftArmAnimationFrame = RightArmAnimationFrame = 10 + Frame;
                            if (CloudForm)
                                BodyAnimationFrame = 19;
                        }
                        else
                        {
                            if (BodySlamResist > 0)
                            {
                                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = (CloudForm ? 23 : 16);
                            }
                            else
                            {
                                if (CloudForm) BodyAnimationFrame = 19;
                                else BodyAnimationFrame = 9;
                                LeftArmAnimationFrame = RightArmAnimationFrame = 9;
                            }
                        }
                        break;
                    case Behaviors.PosJumpCooldown:
                        LeftArmAnimationFrame = RightArmAnimationFrame = (int)((ActionTime * 0.25f) % 9);
                        if (CloudForm) BodyAnimationFrame = 19;
                        break;
                    case Behaviors.DestructiveRush:
                        if (ActionTime > 10 && ActionTime < 55)
                        {
                            LeftArmAnimationFrame = RightArmAnimationFrame = (int)(((ActionTime - 10) * 0.5f) % 9);
                            if (CloudForm) BodyAnimationFrame = 19;
                        }
                        else
                        {
                            if (npc.velocity.Y != 0)
                            {
                                LeftArmAnimationFrame = RightArmAnimationFrame = 9;
                                if (CloudForm) BodyAnimationFrame = 19;
                            }
                            else
                            {
                                LeftArmAnimationFrame = RightArmAnimationFrame = 19;
                                if (CloudForm) BodyAnimationFrame = 19;
                            }
                        }
                        break;
                    case Behaviors.BulletReflectingBelly:
                        if (ActionTime < 30)
                        {
                            LeftArmAnimationFrame = RightArmAnimationFrame = 21;
                            if (CloudForm) BodyAnimationFrame = 20;
                        }
                        else
                        {
                            LeftArmAnimationFrame = RightArmAnimationFrame = 22;
                            if (CloudForm) BodyAnimationFrame = 19;
                        }
                        break;
                }
            }
        }

        public override bool CheckDead()
        {
            Defeated = true;
            npc.life = npc.lifeMax;
            return false;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            drawColor *= 0.8f;
            if (!Defeated && npc.velocity.Y == 0 && BodySlamResist == 0 && BodyAnimationFrame != 25 && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID])
                YOffset = ((float)Math.Sin(Main.GlobalTime * 2)) * 5;
            if (BodySlamResist > 0)
                YOffset = 6;
            return base.PreDraw(spriteBatch, drawColor);
        }

        public override bool CanChat()
        {
            return Defeated || PlayerLost;
        }

        public override string GetChat()
        {
            DialogueStep = 0;
            if (Defeated)
            {
                if (PlayerHasWrath)
                {
                    return "*Grr!! That wont help me get less furious!*";
                }
                else
                {
                    return "*Arrgh!! That didn't helped! I'm even more furious now!*";
                }
            }
            return "*I'm so angry that I even forgot what I should be saying!*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            GetDialogueMessages(DialogueStep, out button, out button2);
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            LastButtonWasAccept = firstButton;
            if (firstButton)
            {
                if (PlayerHasWrath)
                {
                    if (DialogueStep == 3)
                    {
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                        return;
                    }
                }
                else
                {
                    if (DialogueStep == 5)
                    {
                        if (LastButtonWasAccept)
                        {
                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                            return;
                        }
                        else
                        {
                            ForceLeave = true;
                        }
                    }
                }
            }
            DialogueStep++;
            Main.npcChatText = GetDialogueMessages(DialogueStep);
        }

        public string GetDialogueMessages(byte DialogueStep)
        {
            string b1, b2;
            return GetDialogueMessages(DialogueStep, out b1, out b2);
        }

        public string GetDialogueMessages(byte DialogueStep, out string Button1Text, out string Button2Text)
        {
            Button1Text = Button2Text = "";
            if (PlayerHasWrath)
            {
                if (PlayerLost)
                {
                    switch (DialogueStep)
                    {
                        case 0:
                            Button1Text = "I thought the deal wasn't of causing problems around?";
                            return "*My anger continues, and I still don't have any clue on how to deal with It!*";
                        case 1:
                            Button1Text = "How can It be different?";
                            return "*Yes, but that was on the other world! Here is different.*";
                        case 2:
                            Button1Text = "No! Wait, It's okay.";
                            return "*I don't know why! And trying to think about that is making me even more furious.*";
                        case 3:
                            Button1Text = "Uh... Thanks?";
                            return "*I'm here if you need my help. I'd be glad to use my rage on your opposition.*";
                    }
                }
                else
                {
                    switch (DialogueStep)
                    {
                        case 0:
                            Button1Text = "I thought the deal wasn't of causing problems around?";
                            return "*My anger continues, and I still don't have any clue on how to deal with It!*";
                        case 1:
                            Button1Text = "How can It be different?";
                            return "*Yes, but that was on the other world! Here is different.*";
                        case 2:
                            Button1Text = "Don't you dare attacking me again";
                            return "*I don't know why! And trying to think about that is making me even more furious.*";
                        case 3:
                            Button1Text = "Uh... Thanks?";
                            return "*I won't. I'm here if you need my help. I'd be glad to use my rage on your opposition.*";
                    }
                }
            }
            else
            {
                switch (DialogueStep)
                {
                    case 0:
                        Button1Text = "You just tried to kill me";
                        return "*Arrgh!! That didn't helped! I'm even more furious now!*";
                    case 1:
                        Button1Text = "Unending rage?";
                        return "*No! I didn't! I have this unending rage, so I wanted to see if I could end It by unleashing on someone.*";
                    case 2:
                        Button1Text = "How could you have rage until since you woke up?";
                        return "*Yes! I have this unending rage since I woke up some time ago!*";
                    case 3:
                        Button1Text = "May I help you in some way?";
                        return "*I don't know either! I don't even remember things from before I woke up, and that makes me even more furious!*";
                    case 4:
                        Button1Text = "Okay, I'll try to help you.";
                        Button2Text = "No, I'll not help you.";
                        return "*Yes! Try ending this rage, I can barelly concentrate because of It.*";
                    case 5:
                        if (LastButtonWasAccept)
                        {
                            Button1Text = "Alright";
                            return "*Thanks, I'll try my best not to cause problems here, but I can't promisse anyhting.*";
                        }
                        else 
                        {
                            return "*I should have guessed you wouldn't help me! I don't know why I wasted my time.*";
                        }
                }
            }
            return "";
        }

        public void PigStatus(out int HP, out int Damage, out int Defense)
        {
            if (NPC.downedGolemBoss)
            {
                HP = 3000;
                Damage = 150;
                Defense = 50;
            }
            else if (NPC.downedMechBossAny)
            {
                HP = 1500;
                Damage = 90;
                Defense = 35;
            }
            else if (Main.hardMode)
            {
                HP = 1200;
                Damage = 60;
                Defense = 25;
            }
            else if (NPC.downedBoss3)
            {
                HP = 900;
                Damage = 40;
                Defense = 20;
            }
            else
            {
                HP = 400;
                Damage = 20;
                Defense = 10;
            }
        }

        public enum Behaviors : byte
        {
            Charge,
            BodySlam,
            PosJumpCooldown,
            DestructiveRush,
            BulletReflectingBelly
        }
    }
}
