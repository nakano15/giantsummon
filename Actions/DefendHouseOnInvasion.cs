using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Actions
{
    public class DefendHouseOnInvasion : GuardianActions
    {
        public Vector2 GuardPosition = Vector2.Zero;

        public DefendHouseOnInvasion()
        {
            ID = (int)ActionIDs.DefendHouseAction;
        }

        public override void Update(TerraGuardian guardian)
        {
            if(guardian.OwnerPos > -1 || Main.invasionType <= 0)
            {
                InUse = false;
                return;
            }
            if (guardian.UsingFurniture)
            {
                guardian.LeaveFurniture();
            }
            switch (Step)
            {
                case 0:
                    {
                        int DefendX = 0, DefendY = 0;
                        WorldMod.GuardianTownNpcState townnpc = guardian.GetTownNpcInfo;
                        bool MoveLeft = Main.rand.Next(2) == 0;
                        if (townnpc != null && !townnpc.Homeless)
                        {
                            DefendX = townnpc.HomeX;
                            DefendY = townnpc.HomeY;
                            WorldMod.GuardianBuildingInfo house = townnpc.HouseInfo;
                            if(house != null)
                            {
                                float HouseXStack = 0;
                                byte Count = 0;
                                foreach(WorldMod.GuardianTownNpcState tgh in house.GuardiansLivingHere)
                                {
                                    if (!tgh.Homeless)
                                    {
                                        HouseXStack += tgh.HomeX;
                                        Count++;
                                    }
                                }
                                if(Count > 0)
                                {
                                    HouseXStack /= Count;
                                    MoveLeft = (DefendX >= HouseXStack);
                                }
                            }
                        }
                        else
                        {
                            NPC NearestNpc = null;
                            float NearestDistance = 1000;
                            for(int i = 0; i < 200; i++)
                            {
                                if(Main.npc[i].active && Main.npc[i].townNPC && Main.npc[i].type != Terraria.ID.NPCID.OldMan && !Main.npc[i].homeless)
                                {
                                    float Distance = (Main.npc[i].Center - guardian.CenterPosition).Length();
                                    if(Distance < NearestDistance)
                                    {
                                        NearestNpc = Main.npc[i];
                                        NearestDistance = Distance;
                                    }
                                }
                            }
                            if(NearestNpc != null)
                            {
                                DefendX = NearestNpc.homeTileX;
                                DefendY = NearestNpc.homeTileY;
                            }
                            else
                            {
                                DefendX = (int)(guardian.Position.X * (1f / 16));
                                DefendX = (int)(guardian.Position.Y * (1f / 16));
                            }
                        }
                        bool FoundDefendPosition = false;
                        byte AttemptTime = 40;
                        while (!FoundDefendPosition)
                        {
                            AttemptTime--;
                            if (AttemptTime == 0)
                                break;
                            Tile tile = Framing.GetTileSafely(DefendX, DefendY);
                            if(tile != null && tile.active() && Main.tileSolid[tile.type])
                            {
                                DefendY--;
                                continue;
                            }
                            if (Main.wallHouse[tile.wall])
                            {
                                DefendX += (MoveLeft ? -1 : 1);
                            }
                            else
                            {
                                tile = Framing.GetTileSafely(DefendX, DefendY + 1);
                                if(tile != null && (!tile.active() || !Main.tileSolid[tile.type]))
                                {
                                    DefendY++;
                                    continue;
                                }
                                bool Failed = false;
                                for(int x = -4; x < 5; x++)
                                {
                                    for(int y = -1; y < 2; y++)
                                    {
                                        if (Failed)
                                            break;
                                        if(x != 0 || y != 0)
                                        {
                                            tile = Framing.GetTileSafely(DefendX + x, DefendY + y);
                                            if (tile != null && Main.wallHouse[tile.wall])
                                            {
                                                DefendX += (MoveLeft ? -1 : 1);
                                                Failed = true;
                                            }
                                        }
                                    }
                                }
                                if (!Failed)
                                {
                                    FoundDefendPosition = true;
                                }
                            }
                        }
                        if(AttemptTime == 0)
                        {
                            DefendX = (int)(guardian.Position.X * (1f / 16));
                            DefendY = (int)(guardian.Position.Y * (1f / 16));
                        }
                        GuardPosition = new Vector2(DefendX, DefendY) * 16;
                        ChangeStep();
                    }
                    break;

                case 1:
                    {
                        if (guardian.TargetID == -1 && guardian.TalkPlayerID == -1)
                        {
                            guardian.MoveLeft = guardian.MoveRight = false;
                            guardian.WalkMode = false;
                            if (Math.Abs(guardian.Position.X - GuardPosition.X) > 20)
                            {
                                if (GuardPosition.X < guardian.Position.X)
                                {
                                    guardian.MoveLeft = true;
                                }
                                else
                                {
                                    guardian.MoveRight = true;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    break;
            }
        }
    }
}
