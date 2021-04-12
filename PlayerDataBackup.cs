using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public struct PlayerDataBackup : IDisposable
    {
        TerraGuardian guardian;
        Player player;
        Vector2 Position, Velocity, ItemPosition;
        int Width, Height;
        int Direction;
        int numMinions, maxMinions;
        bool Wet, Active, Dead;
        bool channel;
        int MouseX, MouseY;
        bool FrostBurn;
        byte MeleeEnchantment;
        int ItemAnimation, ItemAnimationMax, ItemTime, HeldProj;
        float SlotsMinion;
        float townNPCs, activeNPCs;
        float ItemRotation;
        int[] BuffTypes, BuffTimes;
        BitsByte Zone1, Zone2, Zone3, Zone4;
        int phantasmTime;
        bool ghostHeal, ghostHurt;
        float LifeStealRate, GhostDamage;
        //Pets and Summons backup
        /*public bool bunny, penguin, puppy, grinch, turtle, eater, skeletron, hornet, tiki, lizard, parrot, truffle, sapling, cSapling, spider, squashling, wisp, dino,
            babyFaceMonster, slime, eyeSpring, snowman, blackCat, zephyrfish, pygmy, spiderMinion, miniMinotaur;*/

        public PlayerDataBackup(Player player, TerraGuardian guardian)
        {
            this.player = player;
            this.guardian = guardian;
            Position = player.position;
            Velocity = player.velocity;
            Direction = player.direction;
            ItemPosition = player.itemLocation;
            Width = player.width;
            Height = player.height;
            numMinions = player.numMinions;
            maxMinions = player.maxMinions;
            SlotsMinion = player.slotsMinions;
            Wet = player.wet;
            Active = player.active;
            MouseX = Main.mouseX;
            MouseY = Main.mouseY;
            Dead = player.dead;
            FrostBurn = player.frostArmor;
            MeleeEnchantment = player.meleeEnchant;
            ItemAnimation = player.itemAnimation;
            ItemAnimationMax = player.itemAnimationMax;
            ItemTime = player.itemTime;
            ItemRotation = player.itemRotation;
            HeldProj = player.heldProj;
            channel = player.channel;
            townNPCs = player.townNPCs;
            activeNPCs = player.activeNPCs;
            Zone1 = player.zone1;
            Zone2 = player.zone2;
            Zone3 = player.zone3;
            Zone4 = player.zone4;
            BuffTypes = (int[])player.buffType.Clone();
            BuffTimes = (int[])player.buffTime.Clone();
            ghostHeal = player.ghostHeal;
            ghostHurt = player.ghostHurt;
            LifeStealRate = player.lifeSteal;
            GhostDamage = player.ghostDmg;
            
            phantasmTime = player.phantasmTime;

            //BackupPetSummonData(player, guardian);

            player.position = guardian.TopLeftPosition;
            player.velocity = guardian.Velocity;
            player.direction = guardian.Direction;
            player.width = guardian.Width;
            player.height = guardian.Height;
            player.itemLocation = new Vector2(guardian.ItemPositionX, guardian.ItemPositionY) + guardian.Position;
            //player.numMinions = guardian.NumMinions + player.maxMinions;
            //player.maxMinions += guardian.MaxMinions;
            //player.slotsMinions = guardian.MinionSlotCount;
            player.wet = guardian.Wet;
            player.active = guardian.Active;
            player.dead = guardian.Downed;
            Main.mouseX += guardian.AimDirection.X - (Main.mouseX + (int)Main.screenPosition.X);
            Main.mouseY += guardian.AimDirection.Y - (Main.mouseY + (int)Main.screenPosition.Y);
            if (guardian.GravityDirection < 0)
                Main.mouseY += (int)(guardian.AimDirection.Y - guardian.CenterPosition.Y) * 2;
            player.frostArmor = guardian.HasFlag(GuardianFlags.FrostSetEffect);
            player.meleeEnchant = guardian.MeleeEnchant;
            player.itemAnimation = guardian.ItemAnimationTime;
            player.itemAnimationMax = guardian.ItemMaxAnimationTime;
            player.itemTime = guardian.ItemUseTime;
            player.itemRotation = guardian.ItemRotation;
            player.heldProj = guardian.HeldProj;
            //player.townNPCs = guardian.TownNpcs;
            player.activeNPCs = guardian.ActiveNpcs;
            player.channel = guardian.Channeling;

            player.phantasmTime = guardian.GetCooldownValue(GuardianCooldownManager.CooldownType.PhantasmCooldown);

            player.zone1 = guardian.Zone1;
            player.zone2 = guardian.Zone2;
            player.zone3 = guardian.Zone3;
            player.zone4 = guardian.Zone4;

            player.ghostHeal = guardian.HasFlag(GuardianFlags.SpectreHealSetEffect);
            player.ghostHurt = guardian.HasFlag(GuardianFlags.SpectreSplashSetEffect);
            player.lifeSteal = guardian.LifeStealRate;
            player.ghostDmg = guardian.GhostDamage;
        }

        /*public void BackupPetSummonData(Player player, TerraGuardian guardian)
        {
            //Get
            bunny = player.bunny;
            penguin = player.penguin;
            puppy = player.puppy;
            grinch = player.grinch;
            turtle = player.turtle;
            eater = player.eater;
            skeletron = player.skeletron;
            hornet = player.hornet;
            tiki = player.tiki;
            lizard = player.lizard;
            parrot = player.parrot;
            truffle = player.truffle;
            sapling = player.sapling;
            cSapling = player.cSapling;
            spider = player.spider;
            squashling = player.squashling;
            wisp = player.wisp;
            dino = player.dino;
            babyFaceMonster = player.babyFaceMonster;
            slime = player.slime;
            eyeSpring;
            snowman;
            blackCat;
            zephyrfish;
            pygmy;
            spiderMinion;
            miniMinotaur;
            //Set
            bunny;
            penguin;
            puppy;
            grinch;
            turtle;
            eater;
            skeletron;
            hornet;
            tiki;
            lizard;
            parrot;
            truffle;
            sapling;
            cSapling;
            spider;
            squashling;
            wisp;
            dino;
            babyFaceMonster;
            slime;
            eyeSpring;
            snowman;
            blackCat;
            zephyrfish;
            pygmy;
            spiderMinion;
            miniMinotaur;
        }*/

        public void RestorePlayerStatus()
        {
            //guardian.NumMinions = player.numMinions - maxMinions;
            guardian.HeldProj = player.heldProj;
            for (int b = 0; b < player.buffType.Length; b++)
            {
                if (player.buffType[b] != BuffTypes[b] && player.buffTime[b] > 0)
                {
                    guardian.AddBuff(player.buffType[b], player.buffTime[b]);
                }
            }
            guardian.Position.X = player.position.X + player.width * 0.5f;
            guardian.Position.Y = player.position.Y + player.height;
            guardian.Velocity = player.velocity;
            guardian.SetCooldownValue(GuardianCooldownManager.CooldownType.PhantasmCooldown, player.phantasmTime);

            player.position = Position;
            player.velocity = Velocity;
            player.direction = Direction;
            player.itemLocation = ItemPosition;
            player.width = Width;
            player.height = Height;
            //player.numMinions = numMinions;
            //player.maxMinions = maxMinions;
            //player.slotsMinions = SlotsMinion;
            player.wet = Wet;
            player.active = Active;
            player.dead = Dead;
            Main.mouseX = MouseX;
            Main.mouseY = MouseY;
            player.frostArmor = FrostBurn;
            player.meleeEnchant = MeleeEnchantment;
            player.itemAnimation = ItemAnimation;
            player.itemAnimationMax = ItemAnimationMax;
            player.itemTime = ItemTime;
            player.itemRotation = ItemRotation;
            player.heldProj = HeldProj;
            player.channel = channel;
            player.townNPCs = townNPCs;
            player.activeNPCs = activeNPCs;
            player.zone1 = Zone1;
            player.zone2 = Zone2;
            player.zone3 = Zone3;
            player.zone4 = Zone4;
            player.buffType = BuffTypes;
            player.buffTime = BuffTimes;

            player.phantasmTime = phantasmTime;

            player.ghostHeal = ghostHeal;
            player.ghostHurt = ghostHurt;
            player.lifeSteal = LifeStealRate;
            player.ghostDmg = GhostDamage;
        }

        /// <summary>
        /// If you make use of "using", this will automatically restore the player status after the use.
        /// </summary>
        public void Dispose()
        {
            RestorePlayerStatus();
        }
    }
}
