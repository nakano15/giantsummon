using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon
{
    public class GuardianMount : Mount
    {
        private static Player dummyPlayer = new Player();

        public bool CanMount(int m, TerraGuardian tg)
        {
            int Height = tg.CollisionHeight + mounts[m].heightBoost;
            Vector2 PosCheck = tg.Position;
            PosCheck.X -= tg.CollisionWidth * 0.5f;
            PosCheck.Y -= Height;
            return Collision.IsClearSpotTest(PosCheck, 16, tg.CollisionWidth, tg.CollisionHeight, false, false, 1, true, false);
        }

        public void SetMount(int m, TerraGuardian tg, bool FaceLeft = false)
        {
            if (this._type == m || m <= -1 || m >= maxMounts)
                return;
            if (m == 8) //Re-Logic...
                return;
            if (m == 5 && tg.Wet)
                return;
            if (this._active)
            {
                tg.Rotation = 0;
                _mountSpecificData = null;
            }
            else
            {
                this._active = true;
            }
            this._flyTime = 0;
            this._type = m;
            this._data = mounts[m];
            this._fatigueMax = (float)this._data.fatigueMax;
            if (this.Cart && !FaceLeft && !this.Directional)
            {
                tg.AddBuff(this._data.extraBuff, 3600);
                this._flipDraw = true;
            }
            else
            {
                tg.AddBuff(this._data.buff, 3600);
                this._flipDraw = false;
            }
            if (this._type == 9 && this._abilityCooldown < 20)
            {
                this._abilityCooldown = 20;
            }
            if (this._type == 7 || this._type == 8)
            {
                //mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)(mountedPlayer.height / 2));
            }
            /*if (this._type == 8)
            {
                this._mountSpecificData = new Mount.DrillMountData();
            }*/
            bool SkipDust = false;
            Terraria.ModLoader.MountLoader.SetMount(this, dummyPlayer, ref SkipDust);
        }

        public void Dismount(TerraGuardian tg)
        {
            if (!this._active)
            {
                return;
            }
            this._active = false;
            tg.RemoveBuff(this._data.buff);
            this._mountSpecificData = null;
            if (Cart)
            {
                tg.RemoveBuff(_data.extraBuff);
            }

            this.Reset();
        }

        public void UpdateFrame(TerraGuardian tg, int state, Vector2 Velocity)
        {
            return;
            MaskPlayerToTg(tg);
            UpdateFrame(dummyPlayer, state, Velocity);
        }

        public void MaskPlayerToTg(TerraGuardian tg, bool Draw = false)
        {
            if (!Draw)
            {
                dummyPlayer.width = tg.CollisionWidth;
                dummyPlayer.height = tg.CollisionHeight;
            }
            else
            {
                dummyPlayer.width = 20;
                dummyPlayer.height = 42 + HeightBoost;
            }
            dummyPlayer.position.X = tg.PositionWithOffset.X - dummyPlayer.width * 0.5f;
            dummyPlayer.position.Y = tg.Position.Y - 14 + tg.OffsetY + tg.Base.CharacterPositionYDiscount * tg.Scale - dummyPlayer.height;// - (tg.Base.SittingPoint.Y * tg.Scale - tg.Height);
            dummyPlayer.fullRotationOrigin = new Vector2(dummyPlayer.width, dummyPlayer.height) * 0.5f;
            dummyPlayer.direction = tg.Direction;
        }

        public void UpdateEffects(TerraGuardian tg)
        {

        }

        public GuardianDrawData[] Draw(int drawType, TerraGuardian tg, Vector2 Position, Color drawColor, SpriteEffects playerEffect, float shadow)
        {
            MaskPlayerToTg(tg, true);
            Position = dummyPlayer.position;
            List<GuardianDrawData> gddlist = new List<GuardianDrawData>();
            {
                List<DrawData> ddList = new List<DrawData>();
                Draw(ddList, drawType, dummyPlayer, Position, drawColor, playerEffect, shadow);
                foreach(DrawData dd in ddList)
                {
                    GuardianDrawData gdd = new GuardianDrawData(dd);
                    gdd.textureType = GuardianDrawData.TextureType.Mount;
                    gddlist.Add(gdd);
                }
            }
            return gddlist.ToArray();
        }
    }
}
