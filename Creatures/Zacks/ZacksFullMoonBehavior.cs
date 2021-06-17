using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures.Zacks
{
    public class ZacksFullMoonBehavior : GuardianActions
    {
        public new int Time = 0;
        public byte Behavior = 0;

        public override void Update(TerraGuardian guardian)
        {
            if (Main.dayTime || Main.time >= 28800)
            {
                InUse = false;
                return;
            }
            if (guardian.OwnerPos > -1 && guardian.AfkCounter < 60)
            {
                InUse = false;
                return;
            }
            if (guardian.IsAttackingSomething)
                return;
            switch (Behavior)
            {
                case 0: //Wander
                    {
                        if (Time <= 0)
                        {
                            Tile tile = Framing.GetTileSafely((int)guardian.Position.X / 16, (int)guardian.CenterY / 16);
                            if (tile.wall > 0)
                            {
                                Time = 400;
                            }
                            else if (guardian.HasDoorOpened)
                            {
                                Time = 50;
                            }
                            else
                            {
                                Behavior = 1;
                                Time = 2000;
                                break;
                            }
                        }
                        if (guardian.OwnerPos == -1)
                        {
                            guardian.WalkMode = true;
                            if (guardian.LookingLeft)
                                guardian.MoveLeft = true;
                            else
                                guardian.MoveRight = true;
                        }
                        Time--;
                    }
                    break;
                case 1: //Howl
                    {
                        if (Time <= 0)
                        {
                            guardian.LookingLeft = Main.rand.NextDouble() < 0.5;
                            Behavior = 0;
                            Time = 400;
                            break;
                        }
                        if (Time == 890 || Time == 1300 || Time == 1900)
                        {
                            guardian.SaySomething("Aw.. Aw... Woo...");
                        }
                        Time--;
                    }
                    break;
            }
        }
    }
}
