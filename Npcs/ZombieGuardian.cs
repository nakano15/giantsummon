using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class ZombieGuardian : ModNPC
    {
        private GuardianBase Base { get { return GuardianBase.GetGuardianBase(3); } }
        private int JumpHeightValue = 0;
        private byte TileStuckTimer = 0;
        private float LastX = 0;
        private const float PullMaxTime = 45f, DialogueLineTime = 3 * 60;
        private Vector2 PlayerPullStartPosition = Vector2.Zero;
        private float PlayerPullTime = 0f;
        private byte LeftHandFrame = 0, RightHandFrame = 0;
        private ModTargetting Target = new ModTargetting();
        private int SwordSwingTime { get { if (Main.expertMode) return 47; else return 62; } }
        private float WeaponRotation = 0f;
        private int ItemPositionX = 0, ItemPositionY = 0;
        const int ItemWidth = 22, ItemHeight = 96, ItemOriginX = 10, ItemOriginY = 88;
        private int SwordAttackReactionTime { get { if (Main.expertMode) { return 15; } return 30; } }
        private int PosSwordAttackRecoveryTime { get { if (Main.expertMode) { return 15; } return 60; } }
        private bool DeceivedOnce = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zombie Guardian");
        }

        public override void SetDefaults() //The status will be set by the AI.
        {
            npc.width = 30;
            npc.height = 86;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 3000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.knockBackResist = 0.33f;
            npc.aiStyle = -1;
            npc.boss = true;
            music = Terraria.ID.MusicID.Boss2;
        }

        private int AiState { get { return (int)npc.ai[0]; } set { npc.ai[0] = value; } }
        private float AiValue { get { return (int)npc.ai[1]; } set { npc.ai[1] = value; } }
        private float StuckCounter { get { return (int)npc.ai[2]; } set { npc.ai[2] = value; } }

        public byte GetBossLevel()
        {
            byte BossLevel = 0;
            if (NPC.downedMoonlord)
                BossLevel = 5;
            else if (NPC.downedGolemBoss)
                BossLevel = 4;
            else if (NPC.downedMechBossAny)
                BossLevel = 3;
            else if (Main.hardMode)
                BossLevel = 2;
            else if (NPC.downedBoss3)
                BossLevel = 1;
            return BossLevel;
        }

        public void Pull(int Position, bool Guardian)
        {
            AiState = 4; //Disabled because the player collision system put everything to waste.
            AiValue = 0;
            Target.TargetPosition = Position;
            Target.TargettingPlayer = !Guardian;
        }

        public override void AI()
        {
            byte BossLevel = GetBossLevel();
            switch (BossLevel)
            {
                default:
                    npc.lifeMax = 3000;
                    npc.damage = 15;
                    npc.defense = 5;
                    break;
                case 1:
                    npc.lifeMax = 4500;
                    npc.damage = 45;
                    npc.defense = 20;
                    break;
                case 2:
                    npc.lifeMax = 9000;
                    npc.damage = 56;
                    npc.defense = 24;
                    break;
                case 3:
                    npc.lifeMax = 18000;
                    npc.damage = 64;
                    npc.defense = 28;
                    break;
                case 4:
                    npc.lifeMax = 36000;
                    npc.damage = 78;
                    npc.defense = 32;
                    break;
                case 5:
                    npc.lifeMax = 42000;
                    npc.damage = 106;
                    npc.defense = 36;
                    break;
            }
            npc.knockBackResist = 0.33f;
            npc.defDamage = npc.damage;
            npc.defDefense = npc.defense;
            npc.friendly = false;
            bool RiseFromGroundAI = false;
            if (AiState == 0)
            {
                RiseFromGroundAI = true;
            }
            else
            {
                if (AiState == 1 && AiValue % 5 == 0)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active)
                        {
                            if (!Main.player[i].dead && !Main.player[i].ghost)
                            {
                                if (npc.playerInteraction[i])
                                {
                                    if (Math.Abs(Main.player[i].Center.X - npc.Center.X) >= 368f || Math.Abs(Main.player[i].Center.Y - npc.Center.Y) >= 256f)
                                    {
                                        Pull(i, false);
                                    }
                                }
                                else if (Math.Abs(Main.player[i].Center.X - npc.Center.X) <= NPC.sWidth && Math.Abs(Main.player[i].Center.Y - npc.Center.Y) <= NPC.sHeight)
                                {
                                    npc.playerInteraction[i] = true;
                                }
                            }
                            TerraGuardian Guardian = Main.player[i].GetModPlayer<PlayerMod>().Guardian;
                            if (Guardian.Active && !Guardian.Downed)
                            {
                                if (Math.Abs(Guardian.CenterPosition.X - npc.Center.X) >= 368f || Math.Abs(Guardian.CenterPosition.Y - npc.Center.Y) >= 256f)
                                {
                                    Pull(i, true);
                                }
                            }
                        }
                    }
                }
                npc.behindTiles = false;
                npc.noGravity = false;
                npc.noTileCollide = false;
                npc.dontTakeDamage = false;
                npc.dontTakeDamageFromHostiles = false;
                npc.dontCountMe = false;
                npc.hide = false;
                bool MoveRight = npc.direction == 1;
                if (AiValue == 0 && AiState != 100 && AiState != 4)
                {
                    //Target = ModTargetting.GetClosestTarget(npc.Center);
                    float HPPercentage = 1f;
                    foreach (ModTargetting mt in ModTargetting.GetClosestTargets(npc.Center, 368f, true))
                    {
                        float LifeValue = 1f;
                        if (mt.IsKnockedOut)
                            LifeValue = 0;
                        else
                            LifeValue = (float)mt.Life / mt.MaxLife;
                        if (LifeValue < HPPercentage)
                        {
                            HPPercentage = LifeValue;
                            Target = mt;
                        }
                    }
                    if (Target.IsKnockedOut && AiState != 99)
                    {
                        AiState = 8;
                    }
                }
                if (Target.IsActive && !Target.IsDefeated)
                {
                    MoveRight = Target.Center.X - npc.Center.X >= 0;
                }
                if (AiValue == 0)
                {
                    if (MoveRight)
                        npc.direction = 1;
                    else
                        npc.direction = -1;
                }
                bool MoveForward = false, Jump = false, CheckIfPlayerIsAway = false;
                bool HasZacks = Target.Character.GetModPlayer<PlayerMod>().HasGuardian(3);
                switch (AiState)
                {
                    case 1: //Try chasing player
                        {
                            if (HasZacks)
                            {
                                AiState = 200;
                                AiValue = 0;
                            }
                            CheckIfPlayerIsAway = true;
                            AiValue++;
                            if (AiValue >= 180)
                            {
                                if (!Target.IsActive || Target.IsDefeated) //If there is no target, keep walking. (Player dead, for example)
                                {
                                    AiValue = 0;
                                }
                                else
                                {
                                    if (npc.velocity.X == 0)
                                    {
                                        //Change to one of the attack AIs.
                                        AiState = 5 + Main.rand.Next(3);
                                        AiValue = 0;
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            else
                            {
                                MoveForward = true;
                            }
                        }
                        break;
                    case 2: //Move Underground
                        {
                            npc.friendly = true;
                            npc.damage = 0;
                            npc.dontTakeDamage = true;
                            npc.dontTakeDamageFromHostiles = true;
                            npc.dontCountMe = true;
                            npc.behindTiles = true;
                            npc.hide = false;
                            AiValue++;
                            if (AiValue == npc.height)
                            {
                                AiValue = 0;
                                AiState = 3;
                            }
                        }
                        break;
                    case 3: //Come from Underground
                        {
                            RiseFromGroundAI = true;
                        }
                        break;
                    case 4: //Pull Player
                    case 16: //Pull Player Friendly
                        {
                            if (Target.IsDefeated || !Target.IsActive)
                            {
                                AiState = 1;
                                AiValue = 0;
                            }
                            else if (AiValue >= PullMaxTime)
                            {
                                if (PlayerPullStartPosition.X == 0 || PlayerPullStartPosition.Y == 0)
                                {
                                    PlayerPullStartPosition = Target.Position;
                                    PlayerPullTime = (PlayerPullStartPosition - npc.Center).Length() / 8f;
                                }
                                npc.damage = 0;
                                npc.defDamage = 0;
                                npc.dontTakeDamageFromHostiles = true;
                                float Percentage = (AiValue - PullMaxTime) / (int)PlayerPullTime;
                                if (HasZacks)
                                {
                                    npc.dontTakeDamage = true;
                                    npc.friendly = true;
                                }
                                if (Percentage >= 1f)//(Main.player[npc.target].getRect().Intersects(npc.getRect()))
                                {
                                    Vector2 NewPosition = Vector2.Zero;
                                    NewPosition.X = npc.Center.X - Target.Width * 0.5f + npc.width * 0.5f * npc.direction;
                                    NewPosition.Y = npc.position.Y - Target.Height * 0.25f;
                                    Target.Position = NewPosition;
                                    Target.Velocity = Vector2.Zero;
                                    Target.Direction = -npc.direction;
                                    if (HasZacks)
                                    {
                                        int NewAITime = (int)(AiValue - (PullMaxTime + PlayerPullTime));
                                        AiValue++;
                                        if (NewAITime == 30)
                                        {
                                            Main.NewText("*Hahaha, I can feel your heart racing now.*");
                                        }
                                        else if (NewAITime == 30 + DialogueLineTime)
                                        {
                                            Main.NewText("*I saw you on the woods, and thought that would be fun to give you a scare.*");
                                        }
                                        else if (NewAITime == 60 + DialogueLineTime * 2)
                                        {
                                            AiState = 200;
                                            AiValue = 30 + DialogueLineTime * 2 - 1;
                                        }
                                        npc.ai[2]++;
                                    }
                                    else if (Target.ImmuneTime == 0)
                                    {
                                        //Hurt player
                                        int DefBackup = Target.Defense;
                                        float DefRateBackup = Target.DefenseRate;
                                        Target.Defense = 0;
                                        Target.DefenseRate = 0;
                                        Target.Hurt((int)(Target.MaxLife * 0.2), npc.direction, " has been turned into zombie food.");
                                        if (Main.expertMode)
                                        {
                                            npc.life += (int)(npc.lifeMax * 0.05);
                                            if (npc.life > npc.lifeMax)
                                                npc.life = npc.lifeMax;
                                            if (Target.TargettingPlayer)
                                            {
                                                Target.Character.AddBuff(Terraria.ID.BuffID.Bleeding, 60);
                                            }
                                            else
                                            {
                                                Target.Guardian.AddBuff(Terraria.ID.BuffID.Bleeding, 60);
                                            }
                                        }
                                        Target.Defense = DefBackup;
                                        Target.DefenseRate = DefRateBackup;
                                        AiValue++;
                                        if (Target.IsDefeated)
                                        {
                                            AiState = 1;
                                            AiValue = 0;
                                        }
                                        else if (AiValue >= PullMaxTime + (int)PlayerPullTime + 3)
                                        {
                                            AiState = 1;
                                            AiValue = 0;
                                            Target.Velocity = new Vector2(npc.direction * 7.5f, -9.25f);
                                        }
                                    }
                                }
                                else
                                {
                                    Target.Position = PlayerPullStartPosition + (npc.Center - PlayerPullStartPosition) * Percentage;
                                    Target.FallStart = (int)Target.Position.Y / 16;
                                    if (Target.ItemAnimation == 0)
                                    {
                                        if (Target.Velocity.X >= 0)
                                            Target.Direction = 1;
                                        else
                                            Target.Direction = -1;
                                    }
                                    AiValue++;
                                }
                            }
                            else
                            {
                                if(PlayerPullStartPosition.X != 0 || PlayerPullStartPosition.Y != 0)
                                    PlayerPullStartPosition = Vector2.Zero;
                                AiValue++;
                            }
                        }
                        break;
                    case 5: //Blood Vomit Attack
                        {
                            if (AiValue == 0)
                            {
                                for (int p = 0; p < 255; p++)
                                {
                                    if (!Main.player[p].active || !npc.playerInteraction[p])
                                    {
                                        continue;
                                    }
                                    if (!Main.player[p].dead && !Main.player[p].ghost)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Target.SetTargetToPlayer(Main.player[p]);
                                        }
                                    }
                                    TerraGuardian Guardian = Main.player[p].GetModPlayer<PlayerMod>().Guardian;
                                    if (Guardian.Active && !Guardian.Downed)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Target.SetTargetToPlayer(Main.player[p]);
                                            Target.TargettingPlayer = false;
                                        }
                                    }
                                }
                                MoveRight = Target.Center.X >= npc.Center.X;
                            }
                            npc.damage = npc.defDamage = 0;
                            Vector2 VomitSpawnPosition = npc.Center;
                            VomitSpawnPosition.Y -= npc.height * 0.25f + 4;
                            VomitSpawnPosition.X += npc.width * 0.25f * npc.direction;
                            const float MaxVomitTime = 90;
                            if (AiValue == 60)
                            {
                                Main.PlaySound(Terraria.ID.SoundID.Zombie, npc.Center);
                            }
                            if (AiValue < 30)
                            {
                                VomitSpawnPosition.X -= 2;
                                VomitSpawnPosition.Y -= 2;
                                Dust.NewDust(VomitSpawnPosition, 4, 4, 5, Main.rand.Next(-20, 21) * 0.01f, Main.rand.Next(10, 41) * 0.01f);
                            }
                            else if (AiValue >= 60 + MaxVomitTime)
                            {
                                if (AiValue >= 60 + 60 + MaxVomitTime)
                                {
                                    AiState = 1;
                                    AiValue = 0;
                                }
                            }
                            else if (AiValue >= 60 && AiValue % 3 == 0)
                            {
                                float SpawnDirection = 1.570796326794897f;
                                float Percentage = (float)(AiValue - 60) / MaxVomitTime;
                                SpawnDirection -= 3.141592653589793f * Percentage * npc.direction;
                                //if (npc.direction < 0)
                                //    SpawnDirection += 3.141592653589793f;
                                float VomitSpeed = 8f;
                                int Damage = 30 + BossLevel * 10;
                                Projectile.NewProjectile(VomitSpawnPosition, new Vector2((float)Math.Cos(SpawnDirection) * VomitSpeed, (float)Math.Sin(SpawnDirection) * VomitSpeed), ModContent.ProjectileType<Projectiles.BloodVomit>(), Damage, 16f);
                            }

                            AiValue++;
                        }
                        break;

                    case 6: //Heavy Attack Swing
                        {
                            float AnimationPercentage = (AiValue - SwordAttackReactionTime) / SwordSwingTime;
                            if (AnimationPercentage > 1f)
                                AnimationPercentage = 1f;
                            if (AnimationPercentage < 0)
                                AnimationPercentage = 0f;
                            int Frame = 0;
                            if (AnimationPercentage < 0.45f)
                            {
                                Frame = Base.HeavySwingFrames[0];
                            }
                            else if (AnimationPercentage < 0.65f)
                            {
                                Frame = Base.HeavySwingFrames[1];
                            }
                            else
                            {
                                Frame = Base.HeavySwingFrames[2];
                            }
                            Base.GetBetweenHandsPosition(Frame, out ItemPositionX, out ItemPositionY);
                            if (npc.direction < 0)
                                ItemPositionX = Base.SpriteWidth - ItemPositionX;
                            ItemPositionX -= (int)(Base.SpriteWidth * 0.5f);
                            ItemPositionY -= Base.SpriteHeight;
                            ItemPositionX += (int)npc.Center.X;
                            ItemPositionY += (int)npc.Bottom.Y;

                            float RotationValue = (float)Math.Sin(AnimationPercentage * 1.35f) * (300 * AnimationPercentage);
                            WeaponRotation = MathHelper.ToRadians(-158 + RotationValue);
                            WeaponRotation *= npc.direction;
                            if (AnimationPercentage > 0 && AnimationPercentage < 1)
                            {
                                Rectangle WeaponCollision = new Rectangle();
                                WeaponCollision.Width = (int)(ItemHeight * Math.Sin(WeaponRotation) + ItemWidth * Math.Cos(WeaponRotation));
                                WeaponCollision.Height = (int)(ItemHeight * Math.Cos(WeaponRotation) + ItemWidth * Math.Sin(WeaponRotation)) * -1;
                                WeaponCollision.X -= (int)((ItemHeight - ItemOriginY) * Math.Sin(WeaponRotation) + (ItemWidth - ItemOriginX) * Math.Cos(WeaponRotation));
                                WeaponCollision.Y -= (int)((ItemHeight - ItemOriginY) * Math.Cos(WeaponRotation) + (ItemWidth - ItemOriginX) * Math.Sin(WeaponRotation)) * -1;
                                if (WeaponCollision.Width < 0)
                                {
                                    WeaponCollision.X += WeaponCollision.Width;
                                    WeaponCollision.Width *= -1;
                                }
                                if (WeaponCollision.Height < 0)
                                {
                                    WeaponCollision.Y += WeaponCollision.Height;
                                    WeaponCollision.Height *= -1;
                                }
                                WeaponCollision.X += ItemPositionX;
                                WeaponCollision.Y += ItemPositionY;
                                //Check if hit something.
                                int SlashDamage = (int)(npc.damage * 1.2f);
                                for (int i = 0; i < 255; i++)
                                {
                                    if (Main.player[i].active)
                                    {
                                        if (!Main.player[i].dead && !Main.player[i].ghost && Main.player[i].immuneTime == 0 && Main.player[i].getRect().Intersects(WeaponCollision))
                                        {
                                            Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[i].name + " was cutdown in half by a Zombie Guardian."), SlashDamage, npc.direction);
                                            if (Main.expertMode)
                                            {
                                                Main.player[i].AddBuff(Terraria.ID.BuffID.BrokenArmor, 30);
                                            }
                                        }
                                    }
                                }
                                foreach (int key in MainMod.ActiveGuardians.Keys)
                                {
                                    TerraGuardian Guardian = MainMod.ActiveGuardians[key];
                                    if (Guardian.Active && !Guardian.Downed && Guardian.HitBox.Intersects(WeaponCollision))
                                    {
                                        Guardian.Hurt(SlashDamage, npc.direction, false, false, " was cutdown in half by a Zombie Guardian.");
                                        if (Main.expertMode)
                                        {
                                            Guardian.AddBuff(Terraria.ID.BuffID.BrokenArmor, 30);
                                        }
                                    }
                                }
                            }
                            AiValue++;
                            if (AiValue >= SwordSwingTime + SwordAttackReactionTime + PosSwordAttackRecoveryTime)
                            {
                                AiState = 1;
                                AiValue = 0;
                            }
                        }
                        break;

                    case 7: //Rear attack.
                        {
                            int TimeUntilUse = 90, TimePosLease = 30;
                            if (Main.expertMode)
                            {
                                TimeUntilUse = 35;
                                TimePosLease = 10;
                            }
                            npc.damage = npc.defDamage = 0;
                            if (AiValue < TimeUntilUse)
                            {
                                AiValue++;
                                if (AiValue == TimeUntilUse * 0.5f)
                                {
                                    npc.direction *= -1;
                                }
                            }
                            else
                            {
                                if (AiValue == TimeUntilUse)
                                {
                                    Vector2 SpawnPosition = npc.Center;
                                    SpawnPosition.Y += npc.height * 0.25f;
                                    Vector2 InitialVelocity = new Vector2(-0.3f * npc.direction, 0.33f);
                                    Projectile.NewProjectile(SpawnPosition, InitialVelocity, ModContent.ProjectileType<Projectiles.ZombieFart>(), 0, 0);
                                    Main.PlaySound(SoundID.Item62, npc.Center);
                                }
                                else if (AiValue >= TimePosLease + TimeUntilUse)
                                {
                                    AiState = 1;
                                    AiValue = 0;
                                }
                                AiValue++;
                            }
                        }
                        break;

                    case 8: //Devour/Revive defeated character
                        {
                            if (Target.IsKnockedOut && !Target.IsDefeated)
                            {
                                npc.damage = npc.defDamage = 0;
                                if (Main.expertMode)
                                    npc.defense *= 2;
                                npc.knockBackResist = 0;
                                if (!npc.getRect().Intersects(Target.GetCollision))
                                {
                                    MoveForward = true;
                                    AiValue = 0;
                                }
                                else
                                {
                                    if (npc.velocity.X == 0)
                                    {
                                        if (HasZacks)
                                        {
                                            npc.dontTakeDamage = true;
                                            Target.ReviveCharacter(3);
                                            AiValue++;
                                        }
                                        else
                                        {
                                            AiValue++;
                                            int HealthRestoreValue = Target.FinishCharacter(3, " was devoured by Zombie Guardian.");
                                            //if (HealthRestoreValue > 1)
                                            //    HealthRestoreValue /= 2;
                                            if (HealthRestoreValue > 0)
                                            {
                                                CombatText.NewText(Target.GetCollision, Color.Red, "Chomp", false, true);
                                            }
                                            npc.life += HealthRestoreValue;
                                            if (Target.IsDefeated)
                                            {
                                                AiValue = 0;
                                                AiState = 1;
                                                CombatText.NewText(npc.getRect(), Color.Red, "Buuuuuuuuurp");
                                            }
                                            /*if (AiValue >= (!Main.expertMode ? 10 : 5) * 60)
                                            {
                                                Target.ForceKill(" was devoured by Zombie Guardian.");
                                                npc.life += (int)(npc.lifeMax * 0.2f);
                                                AiValue = 0;
                                                AiState = 1;
                                            }*/
                                        }
                                    }
                                }
                            }
                            else
                            {
                                AiValue = 0;
                                AiState = 1;
                            }
                        }
                        break;

                    case 99: //Stops attacking and turns to talkable.
                        {
                            npc.friendly = true;
                            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
                            npc.life = npc.lifeMax;
                            if (MoveRight)
                                npc.direction = 1;
                            else
                                npc.direction = -1;
                            //npc.townNPC = true;
                            //npc.homeless = true;
                            //npc.homeTileX = npc.homeTileY = 0;
                        }
                        break;

                    case 100: //Ending Animation.
                        {
                            npc.damage = 0;
                            npc.defDamage = 0;
                            npc.friendly = true;
                            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
                            Vector2 NpcPosCenter = npc.position;
                            NpcPosCenter.X += npc.width * 0.5f;
                            NpcPosCenter.Y += npc.height;
                            //Player Target = Main.player[npc.target];
                            TerraGuardian Guardian = Main.player[Target.TargetPosition].GetModPlayer<PlayerMod>().Guardian;
                            if (PlayerMod.HasGuardianSummoned(Main.player[Target.TargetPosition], 1))
                            {
                                foreach (TerraGuardian g in Main.player[Target.TargetPosition].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                                {
                                    if (g.Active && g.ID == 1 && g.ModID == mod.Name)
                                        Guardian = g;
                                }
                            }
                            bool TargetIsBlue = Guardian.ID == 1 && Guardian.Active && !Target.TargettingPlayer,
                                PlayerHasBlue = PlayerMod.PlayerHasGuardianSummoned(Target.Character, 1);
                            if (!PlayerHasBlue)
                            {
                                npc.npcSlots = 1;
                                string Text = "*The zombie got enraged.*";
                                if (Main.netMode == 0)
                                    Main.NewText(Text);
                                else
                                {
                                    //Send to the network
                                }
                                //AiState = 4;
                                //AiValue = 0;
                                Pull(Target.TargetPosition, false);
                                npc.life = (int)(npc.lifeMax * 0.25f);
                                return;
                            }
                            npc.npcSlots = 200;
                            if (Target.IsDefeated || !Target.IsActive)
                            {
                                AiState = 3;
                                AiValue = 0;
                                npc.life = (int)(npc.lifeMax * 0.25f);
                            }
                            if (AiValue < 5 || (AiValue >= 120 + 25 && AiValue < 120 + 30))
                            {
                                int Px, Py;
                                Base.GetBetweenHandsPosition(17, out Px, out Py);
                                if (npc.direction < 0)
                                    Px = Base.Width - Px;
                                Px -= (int)(Base.Width * 0.5f);
                                Vector2 NewPosition = NpcPosCenter;
                                NewPosition.X -= Target.Width * 0.5f;
                                NewPosition.Y -= Target.Height * 0.5f;
                                NewPosition.X += Px;
                                NewPosition.Y += -Base.Height + Py;
                                Target.Position = NewPosition;
                                if (Target.ItemAnimation == 0)
                                    Target.Direction = -npc.direction;
                            }
                            else if (AiValue < 120 + 25)
                            {
                                int Px, Py;
                                Base.GetBetweenHandsPosition(16, out Px, out Py);
                                if (npc.direction < 0)
                                    Px = Base.Width - Px;
                                Px -= (int)(Base.Width * 0.5f);
                                Vector2 NewPosition = NpcPosCenter;
                                NewPosition.X -= Target.Width * 0.5f;
                                NewPosition.Y -= Target.Height * 0.5f;
                                NewPosition.X += Px;
                                NewPosition.Y += -Base.Height + Py;
                                Target.Position = NewPosition;
                                if (Target.ItemAnimation == 0)
                                    Target.Direction = -npc.direction;
                            }
                            if (AiValue == 30)
                            {
                                string Text = (TargetIsBlue ? "*"+ PlayerMod.GetPlayerGuardian(Target.Character, 1).Name +" told the zombie to stop and look at her face, then asked if It didn't recognize her.*" : "*" + NpcMod.GetGuardianNPCName(1) + " told the zombie to stop that at once, asked if he forgot about her.*");
                                if (Main.netMode == 0)
                                    Main.NewText(Text);
                                else
                                {
                                    //Send to the network
                                }
                            }
                            else if (AiValue == DialogueLineTime + 30)
                            {
                                string Text = "*The zombie ended up tame after seeing " + PlayerMod.GetPlayerGuardian(Target.Character, 1).Name + ".*";
                                if (Main.netMode == 0)
                                    Main.NewText(Text);
                                else
                                {
                                    //Send to the network
                                }
                            }
                            else if (AiValue == DialogueLineTime * 2 + 30)
                            {
                                string Text = "*The zombie thanked you for bringing back his old self, and said that want to help you on your quest, so he can stay with "+NpcMod.GetGuardianNPCName(1)+". He told you that his name is "+Base.Name+".*";
                                if (Main.netMode == 0)
                                    Main.NewText(Text);
                                else
                                {
                                    //Send to the network
                                }
                            }
                            else if (AiValue >= DialogueLineTime * 2 + 30)
                            {
                                NpcMod.AddGuardianMet(3);
                                Main.player[Target.TargetPosition].GetModPlayer<PlayerMod>().AddNewGuardian(3);
                                PlayerMod.GetPlayerGuardian(Main.player[Target.TargetPosition], 3).IncreaseFriendshipProgress(1);
                                WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Zacks);
                                //npc.Transform(ModContent.NPCType<GuardianNPC.List.ZombieWolfGuardian>());
                                return;
                                //NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<GuardianNPC.List.ZombieWolfGuardian>());
                                //npc.active = false;
                            }
                            AiValue++;
                        }
                        break;
                    case 200:
                        {
                            npc.npcSlots = 200;
                            npc.damage = 0;
                            npc.defDamage = 0;
                            npc.friendly = true;
                            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
                            Vector2 NpcPosCenter = npc.position;
                            NpcPosCenter.X += npc.width * 0.5f;
                            NpcPosCenter.Y += npc.height;
                            if (AiValue == 30)
                            {
                                Main.NewText("*Boo!*");
                            }
                            else if (AiValue == 30 + DialogueLineTime)
                            {
                                Main.NewText("*Did I scared you?*");
                            }
                            else if (AiValue == 30 + DialogueLineTime * 2)
                            {
                                Main.NewText("*Sorry for scaring you, but It was really fun.*");
                            }
                            else if (AiValue == 30 + DialogueLineTime * 3)
                            {
                                Main.NewText("*If you need my help, I'm here.*");
                            }
                            else if (AiValue == 30 + DialogueLineTime * 4)
                            {
                                NpcMod.AddGuardianMet(3);
                                WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Zacks);
                                //npc.Transform(ModContent.NPCType<GuardianNPC.List.ZombieWolfGuardian>());
                                return;
                            }
                            AiValue++;
                        }
                        break;
                }
                if (false && CheckIfPlayerIsAway)
                {
                    if (Math.Abs(Target.Center.X - npc.Center.X) >= 368f || Math.Abs(Target.Center.Y - npc.Center.Y) >= 256f)
                    {
                        AiState = 4; //Disabled because the player collision system put everything to waste.
                        //AiState = 3;
                        AiValue = 0;
                    }
                }
                if (MoveForward)
                {
                    float MaxSpeed = 1f - (npc.life >= npc.lifeMax ? 1f : (float)npc.life / npc.lifeMax);
                    if (Main.expertMode)
                    {
                        MaxSpeed = 0.5f + MaxSpeed * 0.5f;
                    }
                    else
                    {
                        MaxSpeed = 0.33f + MaxSpeed * 0.25f;
                    }
                    MaxSpeed *= Base.MaxSpeed;
                    if ((npc.direction == 1 && !MoveRight) || (npc.direction == -1 && MoveRight))
                    {
                        StuckCounter++;
                    }
                    npc.direction = MoveRight ? 1 : -1;
                    npc.velocity.X += npc.direction * Base.Acceleration;
                    if (npc.velocity.X * npc.direction > MaxSpeed)
                        npc.velocity.X = MaxSpeed * npc.direction;
                    if (npc.collideX && npc.velocity.Y == 0)
                    {
                        Jump = true;
                        StuckCounter++;
                    }
                    if (StuckCounter >= 3)
                    {
                        StuckCounter = 0;
                        AiState = 2;
                        AiValue = 0;
                    }
                    /*if (StuckCounter >= 150)
                    {
                        StuckCounter = 0;
                        AiState = 2;
                        AiValue = 0;
                    }*/
                    if (LastX == npc.position.X)
                    {
                        TileStuckTimer++;
                        if (TileStuckTimer >= 50)
                        {
                            StuckCounter = 0;
                            AiState = 2;
                            AiValue = 0;
                        }
                    }
                    LastX = npc.position.X;
                }
                else if (npc.velocity.Y == 0)
                {
                    //npc.velocity.X -= npc.direction * Base.SlowDown;
                    //if (-npc.velocity.X * npc.direction < 0)
                    npc.velocity.X = 0;
                    StuckCounter = 0;
                    TileStuckTimer = 0;
                }
                if (Jump && npc.velocity.Y == 0)
                    JumpHeightValue = Base.MaxJumpHeight;
                if (JumpHeightValue > 0)
                {
                    JumpHeightValue--;
                    npc.velocity.Y = -Base.JumpSpeed;
                    if (npc.collideY)
                    {
                        JumpHeightValue = 0;
                        npc.velocity.Y = 0;
                    }
                }
                float StepSpeed = 2f, gfxOffY = 0f;
                Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref StepSpeed, ref gfxOffY);
                //npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);
                Vector4 SlopedCollision = Collision.SlopeCollision(npc.position, npc.velocity, npc.width, npc.height, 1f, false);
                npc.position = SlopedCollision.XY();
                npc.velocity = SlopedCollision.ZW();
            }
            if (RiseFromGroundAI)
            {
                npc.friendly = true;
                npc.damage = 0;
                npc.dontTakeDamage = true;
                npc.dontTakeDamageFromHostiles = true;
                npc.dontCountMe = true;
                npc.behindTiles = true;
                npc.hide = true;
                npc.velocity.X = 0;
                if (AiValue == 0) //Try moving to behind the player, under the ground.
                {
                    if (AiState == 0) npc.life = npc.lifeMax;
                    Target = ModTargetting.GetClosestTarget(npc.Center);
                    npc.Center = Target.Center;
                    if (Target.IsActive && !Target.IsDefeated)
                    {
                        int TileX = (int)(Target.Center.X / 16) + Target.Direction * -4, TileY = (int)((Target.Position.Y + Target.Height + 1) / 16);
                        byte Attempts = 5;
                        bool UpClear = false, DownHasTile = false, IsGrassOrDirtTile = false;
                        while (Attempts > 0)
                        {
                            UpClear = true;
                            for (int y = 0; y < npc.height / 16 + 1; y++)
                            {
                                for (int x = 0; x < npc.width / 16 + 1; x++)
                                {
                                    bool IsClear = !Main.tile[TileX - 1 + x, TileY - 1 - y].active() || (Main.tile[TileX - 1 + x, TileY - 1 - y].active() && !Main.tileSolid[Main.tile[TileX - 1 + x, TileY - 1 - y].type]);
                                    if (!IsClear)
                                    {
                                        UpClear = false;
                                        break;
                                    }
                                }
                            }
                            DownHasTile = Main.tile[TileX, TileY].active() || Main.tile[TileX + 1, TileY].active();
                            IsGrassOrDirtTile = DownHasTile && (Main.tile[TileX, TileY].type == TileID.Dirt || Main.tile[TileX, TileY].type == TileID.Mud ||
                                Main.tile[TileX, TileY].type == TileID.Grass || Main.tile[TileX, TileY].type == TileID.CorruptGrass ||
                                Main.tile[TileX, TileY].type == TileID.FleshGrass || Main.tile[TileX, TileY].type == TileID.HallowedGrass ||
                                Main.tile[TileX, TileY].type == TileID.JungleGrass || Main.tile[TileX, TileY].type == TileID.MushroomGrass);
                            Attempts--;
                            if (UpClear && DownHasTile && IsGrassOrDirtTile)
                                break;
                            else if (!UpClear)
                            {
                                TileY--;
                            }
                            else if (!DownHasTile)
                            {
                                TileY++;
                            }
                        }
                        if (UpClear && DownHasTile) //Teleports the npc to the tile position, then change the AI Value to 1, where it will rise form the floor.
                        {
                            if (!IsGrassOrDirtTile) //No spawn on 
                            {
                                //npc.active = false;
                                return;
                            }
                            npc.position.X = TileX * 16 - npc.width * -Main.player[npc.target].direction * -0.5f;
                            npc.position.Y = (TileY) * 16 - npc.height;
                            if (Target.Center.X < npc.Center.X)
                            {
                                npc.direction = -1;
                            }
                            else
                            {
                                npc.direction = 1;
                            }
                            AiValue++;
                        }
                    }
                }
                else //Rise from the ground.
                {
                    npc.hide = false;
                    npc.noGravity = false;
                    npc.noTileCollide = false;
                    //npc.position.Y--;
                    if (AiValue < npc.height)
                        AiValue++;
                    else
                    {
                        AiState = 1;
                        AiValue = 0;
                        Target = ModTargetting.GetClosestTarget(npc.Center);
                        if (Target.GetCollision.Intersects(npc.Hitbox))
                        {
                            AiState = 4;
                        }
                    }
                    //if (npc.Center.Y - Target.Center.Y >= 16 * 5) //Try again if the npc spawned or went too far away from the player.
                    //    AiValue = 0;
                }
            }
        }

        public override bool CanChat()
        {
            return AiState == 99;
        }

        public override string GetChat()
        {
            string Message = "*It turned less violent. Maybe I could try talking to it?*";
            if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.ID == 1 && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.Active)
            {
                Message = "*"+NpcMod.GetGuardianNPCName(1)+" is asking you if she could try talking to it.*";
            }
            else if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().HasGuardian(1))
            {
                Message = "*It seems less violent due to the presence of something in this world, what would it be?*";
            }
            return Message;
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            //
            if (firstButton)
            {
                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 1))
                {
                    //Trigger animation that results in it being recruited.
                    AiState = 100;
                    AiValue = 0;
                    Target.SetTargetToPlayer(Main.player[Main.myPlayer]);
                }
                else
                {
                    Target = ModTargetting.GetClosestTarget(npc.Center);
                    Pull(Target.TargetPosition, !Target.TargettingPlayer);
                    //AiState = 4;
                    AiValue = PullMaxTime - 1;
                    npc.life = (int)(npc.lifeMax * 0.25);
                }
                DeceivedOnce = true;
            }
            else
            {
                npc.life = 1;
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
            }
            //Main.player[Main.myPlayer].talkNPC = -1;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (AiState == 99)
            {
                button = "Try talking";
                button2 = "Finish it";
            }
        }

        public override bool CheckDead()
        {
            if (AiState == 99 || DeceivedOnce)
            {
                npc.life = 0;
                return true;
            }
            else
            {
                npc.life = 99999;
                AiState = 99;
                AiValue = 0;
            }
            return false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            double newDamage = damage - defense * 0.5;
            if (crit)
                newDamage *= 2;
            double DamageDeductor = 0.2 + GetBossLevel() * 0.1;
            damage -= newDamage * (1 - DamageDeductor);
            if (damage < 1)
                damage = 1;
            return false;
        }

        public override void PostAI()
        {

        }

        public override void FindFrame(int frameHeight)
        {
            int FrameY = npc.frame.Y;
            npc.frame.Width = Base.SpriteWidth;
            npc.frame.Height = Base.SpriteHeight;
            if (AiState == 99)
            {
                FrameY = Base.DownedFrame;
            }
            else if (npc.velocity.Y != 0)
            {
                FrameY = Base.JumpFrame;
                //npc.frameCounter = 0;
            }
            else if (npc.velocity.X != 0)
            {
                float MaxAnimationTime = Base.MaxWalkSpeedTime;
                float AnimationSpeed = Math.Abs(npc.velocity.X);
                if ((npc.velocity.X > 0 && npc.direction < 0) || (npc.velocity.X < 0 && npc.direction > 0))
                    AnimationSpeed *= -1;
                npc.frameCounter += AnimationSpeed / Base.MaxSpeed;
                if (npc.frameCounter < 0)
                    npc.frameCounter += MaxAnimationTime;
                if (npc.frameCounter >= MaxAnimationTime)
                    npc.frameCounter -= MaxAnimationTime;
                FrameY = (int)(npc.frameCounter / Base.WalkAnimationFrameTime);
                if (FrameY >= 0 && FrameY < Base.WalkingFrames.Length)
                    FrameY = Base.WalkingFrames[FrameY];
            }
            else
            {
                FrameY = Base.StandingFrame;
                npc.frameCounter = 0;
            }
            LeftHandFrame = RightHandFrame = (byte)FrameY;
            switch (AiState)
            {
                case 4: //Pull Player AI
                case 16:
                    if (AiValue < 5)
                    {
                        LeftHandFrame = 14;
                    }
                    else if (AiValue < 10)
                    {
                        LeftHandFrame = 15;
                    }
                    else if (AiValue < 15)
                    {
                        LeftHandFrame = 16;
                    }
                    else if (AiValue < 20)
                    {
                        LeftHandFrame = 17;
                    }
                    if (AiValue >= PullMaxTime + (int)PlayerPullTime)
                    {
                        LeftHandFrame = RightHandFrame = 15;
                    }
                    break;
                case 6:
                    {
                        float AnimationPercentage = (AiValue - SwordAttackReactionTime) / SwordSwingTime;
                        if (AnimationPercentage > 1f)
                            AnimationPercentage = 1f;
                        if (AnimationPercentage < 0)
                            AnimationPercentage = 0f;
                        int Frame = 0;
                        if (AnimationPercentage < 0.45f)
                        {
                            Frame = Base.HeavySwingFrames[0];
                        }
                        else if (AnimationPercentage < 0.65f)
                        {
                            Frame = Base.HeavySwingFrames[1];
                        }
                        else
                        {
                            Frame = Base.HeavySwingFrames[2];
                        }
                        FrameY = Frame;
                        LeftHandFrame = RightHandFrame = (byte)Frame;
                    }
                    break;
                case 8:
                    {
                        if (npc.velocity.X == 0)
                        {
                            FrameY = LeftHandFrame = RightHandFrame = (byte)Base.ReviveFrame;
                        }
                    }
                    break;
                case 100:
                    if (AiValue < 5 || (AiValue >= 120 + 25 && AiValue < 120 + 30))
                    {
                        LeftHandFrame = RightHandFrame = 17;
                    }
                    else if (AiValue < 120 + 25)
                    {
                        LeftHandFrame = RightHandFrame = 16;
                    }
                    break;
            }
            npc.frame.Y = 0;
            npc.frame.X = FrameY;
            if (npc.frame.X >= Base.FramesInRows)
            {
                npc.frame.Y += npc.frame.X / Base.FramesInRows;
                npc.frame.X -= npc.frame.Y * Base.FramesInRows;
            }
        }

        public void FrameGetter(byte Frame, out int X, out int Y)
        {
            X = Frame;
            Y = 0;
            if (X >= Base.FramesInRows)
            {
                Y += X / Base.FramesInRows;
                X -= Y * Base.FramesInRows;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (AiState == 0 && AiValue == 0) return false;
            if (Base.sprites.HasErrorLoadingOccurred)
                return false;
            else if (!Base.sprites.IsTextureLoaded)
            {
                Base.sprites.LoadTextures();
            }
            Vector2 DrawPosition = npc.position - Main.screenPosition;
            DrawPosition.X += npc.width * 0.5f;
            DrawPosition.Y -= Base.SpriteHeight - npc.height;
            if (AiState == 0 || AiState == 3)
            {
                DrawPosition.Y += npc.height - AiValue;
            }
            else if (AiState == 2)
            {
                DrawPosition.Y += AiValue;
            }
            Vector2 Origin = new Vector2(Base.SpriteWidth * 0.5f, 0f);
            int FX = 0, FY = 0;
            FrameGetter(RightHandFrame, out FX, out FY);
            spriteBatch.Draw(Base.sprites.RightArmSprite, DrawPosition, new Rectangle(Base.SpriteWidth * FX, Base.SpriteHeight * FY, Base.SpriteWidth, Base.SpriteHeight), drawColor, 0f, Origin, 1f, (npc.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
            spriteBatch.Draw(Base.sprites.BodySprite, DrawPosition, new Rectangle(Base.SpriteWidth * npc.frame.X, Base.SpriteHeight * npc.frame.Y, Base.SpriteWidth, Base.SpriteHeight), drawColor, 0f, Origin, 1f, (npc.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
            if (AiState == 6)
                DrawWeapon(drawColor);
            FrameGetter(LeftHandFrame, out FX, out FY);
            spriteBatch.Draw(Base.sprites.LeftArmSprite, DrawPosition, new Rectangle(Base.SpriteWidth * FX, Base.SpriteHeight * FY, Base.SpriteWidth, Base.SpriteHeight), drawColor, 0f, Origin, 1f, (npc.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
            if (AiState == 4)
            {
                DrawChain();
            }
            return false;
        }

        private void DrawWeapon(Color c)
        {
            Vector2 ItemPosition = new Vector2(ItemPositionX, ItemPositionY);
            Vector2 ItemOrigin = new Vector2(ItemOriginX, ItemOriginY);//giantsummon.GetGuardianItemData(Item.type).ItemOrigin;
            if (npc.direction < 0)
            {
                ItemOrigin.X = ItemWidth - ItemOrigin.X;
            }
            Main.spriteBatch.Draw(MainMod.TwoHandedSwordSprite, ItemPosition - Main.screenPosition, null, c, WeaponRotation, ItemOrigin, 1f, (npc.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }

        private void DrawChain()
        {
            Vector2 ChainStartPosition = npc.Center, ChainEndPosition = ChainStartPosition;
            ChainStartPosition.X -= 8 * npc.direction;
            ChainStartPosition.Y -= 8;
            float Percentage = AiValue / PullMaxTime;
            if (Percentage > 1f)
                Percentage = 1f;
            else
                ChainEndPosition.Y += Bezier(Percentage, 0f, -60f, 0f);
            ChainEndPosition += (Target.Center - npc.Center) * Percentage;
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
                    Main.spriteBatch.Draw(Main.chain12Texture, ChainEndPosition - Main.screenPosition, null, color, Rotation, new Vector2(Main.chain12Texture.Width * 0.5f, Main.chain12Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float Chance = 0;
            if (!NpcMod.HasMetGuardian(3) && !NPC.AnyNPCs(ModContent.NPCType<ZombieGuardian>()) && !NPC.AnyNPCs(ModContent.NPCType<GuardianNPC.List.ZombieWolfGuardian>()) && (Main.bloodMoon || (!Main.dayTime && PlayerMod.PlayerHasGuardian(spawnInfo.player, 3))) && Math.Abs(spawnInfo.player.Center.X / 16 - Main.spawnTileX) >= Main.maxTilesX / 3 && !spawnInfo.player.ZoneUnderworldHeight && !spawnInfo.player.ZoneDirtLayerHeight && !spawnInfo.player.ZoneRockLayerHeight)
            {
                Chance = 0.03f;
            }
            return Chance;
        }

        public static float Bezier(float t, float a, float b, float c)
        {
            float ab = MathHelper.Lerp(a, b, t);
            float bc = MathHelper.Lerp(b, c, t);
            return MathHelper.Lerp(ab, bc, t);
        }
    }
}
