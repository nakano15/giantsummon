using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class CharacterInfo
    {
        private int MyPosition = 0;
        private TerraGuardian.TargetTypes CharType = TerraGuardian.TargetTypes.Player;
        private bool ValidCharacter = false;
        public bool IsValidCharacter { get { return ValidCharacter; } }
        private float _Distance = 0f;
        private Vector2 _Distance2D = Vector2.Zero;
        public float Distance { get { return _Distance; } }
        public Vector2 Distance2D { get { return _Distance2D; } }
        public int GetMyPosition { get { return MyPosition; } }
        public TerraGuardian.TargetTypes GetMyType { get { return CharType; } }

        public CharacterInfo(int Position, TerraGuardian.TargetTypes CharType)
        {
            ChangeCharacter(Position, CharType);
        }

        public bool ChangeCharacter(int Position, TerraGuardian.TargetTypes CharType)
        {
            MyPosition = Position;
            this.CharType = CharType;
            ValidCharacter = CheckIfCharacterIsValid();
            return ValidCharacter;
        }

        public bool CheckIfCharacterIsValid()
        {
            bool Valid = false;
            switch (CharType)
            {
                case TerraGuardian.TargetTypes.Player:
                    if (MyPosition >= 0 && MyPosition < 255)
                        Valid = true;
                    break;
                case TerraGuardian.TargetTypes.Npc:
                    if (MyPosition >= 0 && MyPosition < 200)
                        Valid = true;
                    break;
                case TerraGuardian.TargetTypes.Guardian:
                    if (MainMod.ActiveGuardians.ContainsKey(MyPosition))
                        Valid = true;
                    break;
            }
            return Valid;
        }

        public void CalculateDistance(TerraGuardian g)
        {
            CalculateDistance(g.CenterPosition);
        }

        public void CalculateDistance(Vector2 Position)
        {
            _Distance2D = (Center - Position);
            _Distance = _Distance2D.Length();
        }

        public bool Active
        {
            get
            {
                if (ValidCharacter)
                {
                    switch (CharType)
                    {
                        case TerraGuardian.TargetTypes.Player:
                            return Main.player[MyPosition].active;
                        case TerraGuardian.TargetTypes.Npc:
                            return Main.npc[MyPosition].active;
                        case TerraGuardian.TargetTypes.Guardian:
                            return MainMod.ActiveGuardians.ContainsKey(MyPosition);
                    }
                }
                return false;
            }
        }

        public int Health
        {
            get
            {
                if (ValidCharacter)
                {
                    switch (CharType)
                    {
                        case TerraGuardian.TargetTypes.Player:
                            return Main.player[MyPosition].statLife;
                        case TerraGuardian.TargetTypes.Npc:
                            return Main.npc[MyPosition].life;
                        case TerraGuardian.TargetTypes.Guardian:
                            return MainMod.ActiveGuardians[MyPosition].HP;
                    }
                }
                return 0;
            }
        }

        public int MaxHealth
        {
            get
            {
                if (ValidCharacter)
                {
                    switch (CharType)
                    {
                        case TerraGuardian.TargetTypes.Player:
                            return Main.player[MyPosition].statLifeMax2;
                        case TerraGuardian.TargetTypes.Npc:
                            return Main.npc[MyPosition].lifeMax;
                        case TerraGuardian.TargetTypes.Guardian:
                            return MainMod.ActiveGuardians[MyPosition].MHP;
                    }
                }
                return 0;
            }
        }

        public Vector2 Position
        {
            get
            {
                if (ValidCharacter)
                {
                    switch (CharType)
                    {
                        case TerraGuardian.TargetTypes.Player:
                            return Main.player[MyPosition].position;
                        case TerraGuardian.TargetTypes.Npc:
                            return Main.npc[MyPosition].position;
                        case TerraGuardian.TargetTypes.Guardian:
                            return MainMod.ActiveGuardians[MyPosition].TopLeftPosition;
                    }
                }
                return Vector2.Zero;
            }
        }

        public int Width
        {
            get
            {
                if (ValidCharacter)
                {
                    switch (CharType)
                    {
                        case TerraGuardian.TargetTypes.Player:
                            return Main.player[MyPosition].width;
                        case TerraGuardian.TargetTypes.Npc:
                            return Main.npc[MyPosition].width;
                        case TerraGuardian.TargetTypes.Guardian:
                            return MainMod.ActiveGuardians[MyPosition].Width;
                    }
                }
                return 0;
            }
        }

        public int Height
        {
            get
            {
                if (ValidCharacter)
                {
                    switch (CharType)
                    {
                        case TerraGuardian.TargetTypes.Player:
                            return Main.player[MyPosition].height;
                        case TerraGuardian.TargetTypes.Npc:
                            return Main.npc[MyPosition].height;
                        case TerraGuardian.TargetTypes.Guardian:
                            return MainMod.ActiveGuardians[MyPosition].Height;
                    }
                }
                return 0;
            }
        }

        public Vector2 Center
        {
            get
            {
                Vector2 CenterPos = Position;
                CenterPos.X += Width * 0.5f;
                CenterPos.Y += Height * 0.5f;
                return CenterPos;
            }
        }

        public bool IsFriendly(TerraGuardian guardian)
        {
            switch (CharType)
            {
                case TerraGuardian.TargetTypes.Player:
                    return !guardian.IsPlayerHostile(Main.player[MyPosition]);
                case TerraGuardian.TargetTypes.Npc:
                    return !guardian.IsNpcHostile(Main.npc[MyPosition]);
                case TerraGuardian.TargetTypes.Guardian:
                    return !guardian.IsGuardianHostile(MainMod.ActiveGuardians[MyPosition]);
            }
            return false;
        }
    }
}
