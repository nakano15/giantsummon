using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon
{
    public class GuardianDrawData
    {
        public TextureType textureType = TextureType.Unknown;
        public Texture2D Texture;
        public Vector2? Position = null;
        public Rectangle? Destination = null;
        public Rectangle? Source = null;
        public Color color = Color.White;
        public float Rotation = 0f;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public SpriteEffects Seffects = SpriteEffects.None;
        public int Shader = 0;
        public bool IgnorePlayerRotation = false;
        public DrawData? dd = null;

        public GuardianDrawData(TextureType tt, Texture2D Texture, Vector2 Position, Color color)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Position = Position;
            this.color = color;
        }

        public GuardianDrawData(TextureType tt, Texture2D Texture, Vector2 Position, Rectangle? Source, Color color)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Position = Position;
            this.color = color;
            this.Source = Source;
        }

        public GuardianDrawData(TextureType tt, Texture2D Texture, Vector2 Position, Rectangle? Source, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Position = Position;
            this.color = color;
            this.Source = Source;
            this.Rotation = Rotation;
            this.Origin = Origin;
            this.Scale *= Scale;
            this.Seffects = seffect;
        }

        public GuardianDrawData(TextureType tt, Texture2D Texture, Vector2 Position, Rectangle? Source, Color color, float Rotation, Vector2 Origin, Vector2 Scale, SpriteEffects seffect)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Position = Position;
            this.color = color;
            this.Source = Source;
            this.Rotation = Rotation;
            this.Origin = Origin;
            this.Scale = Scale;
            this.Seffects = seffect;
        }

        public GuardianDrawData(TextureType tt, Texture2D Texture, Rectangle Destination, Color color)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Destination = Destination;
            this.color = color;
        }

        public GuardianDrawData(TextureType tt, Texture2D Texture, Rectangle Destination, Rectangle? Source, Color color)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Destination = Destination;
            this.color = color;
            this.Source = Source;
        }

        public GuardianDrawData(TextureType tt, Texture2D Texture, Rectangle Destination, Rectangle? Source, Color color, float Rotation, Vector2 Origin, SpriteEffects seffect)
        {
            this.textureType = tt;
            this.Texture = Texture;
            this.Destination = Destination;
            this.color = color;
            this.Source = Source;
            this.Rotation = Rotation;
            this.Origin = Origin;
            this.Seffects = seffect;
        }

        public GuardianDrawData(DrawData dd)
        {
            this.dd = dd;
        }

        public void Draw(SpriteBatch sb)
        {
            if (dd.HasValue)
            {
                dd.Value.Draw(sb);
                return;
            }
            if (Position.HasValue)
            {
                sb.Draw(Texture, Position.Value, Source, color, Rotation, Origin, Scale, Seffects, 0f);
                return;
            }
            if (Destination.HasValue)
            {
                sb.Draw(Texture, Destination.Value, Source, color, Rotation, Origin, Seffects, 0f);
                return;
            }
        }

        public DrawData GetDrawData()
        {
            if (this.dd.HasValue)
                return this.dd.Value;
            DrawData dd = new DrawData();
            if (Position.HasValue)
            {
                dd = new DrawData(Texture, Position.Value, Source, color, Rotation, Origin, Scale, Seffects, 0);
            }
            if (Destination.HasValue)
            {
                dd = new DrawData(Texture, Destination.Value, Source, color, Rotation, Origin, Seffects, 0);
            }
            dd.ignorePlayerRotation = IgnorePlayerRotation;
            dd.shader = Shader;
            return dd;
        }

        public enum TextureType : byte
        {
            Unknown,

            TGHeadAccessory,
            TGHead,
            TGBody,
            TGBodyFront,
            TGLeftArm,
            TGRightArm,
            TGRightArmFront,
            TGExtra,

            MainHandItem,
            OffHandItem,

            PreDrawEffect,
            Effect,
            PosDrawEffect,

            PlHair,
            PlHead,
            PlEye,
            PlEyeWhite,
            PlBodySkin,
            PlBodyArmSkin,
            PlHand,
            PlLegSkin,
            PlDefaultShirt,
            PlDefaultUndershirt,
            PlDefaultShirtArm,
            PlDefaultUndershirtArm,
            PlDefaultPants,
            PlDefaultShoes,

            PlArmorHead,
            PlArmorBody,
            PlArmorArm,
            PlArmorLegs,

            Wings
        }
    }
}
