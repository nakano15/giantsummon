using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianAnimationPoints
    {
        public Point DefaultCoordinate = Point.Zero;
        private Dictionary<int, AnimationPoint> SpecificCoordinates = new Dictionary<int, AnimationPoint>();
        public Point DefaultCoordinate2x { set { DefaultCoordinate.X = value.X * 2; DefaultCoordinate.Y = value.Y * 2; } }
        public bool DefaultShowWeapon = false;
        public float DefaultRotation = 0f;

        public void GetPositionFromFrame(int Frame, out float x, out float y)
        {
            int x2, y2;
            GetPositionFromFrame(Frame, out x2, out y2);
            x = x2;
            y = y2;
        }

        public void GetPositionFromFrame(int Frame, out int x, out int y)
        {
            Point p = GetPositionFromFramePoint(Frame);
            x = p.X;
            y = p.Y;
        }

        public Vector2 GetPositionFromFrameVector(int Frame)
        {
            Point p = GetPositionFromFramePoint(Frame);
            return new Vector2(p.X, p.Y);
        }

        public bool GetShowWeapon(int Frame)
        {
            if (!SpecificCoordinates.ContainsKey(Frame))
                return DefaultShowWeapon;
            return SpecificCoordinates[Frame].ShowWeapon;
        }

        public float GetItemRotation(int Frame)
        {
            if (!SpecificCoordinates.ContainsKey(Frame))
                return DefaultRotation;
            return SpecificCoordinates[Frame].Rotation;
        }

        public Point GetPositionFromFramePoint(int Frame)
        {
            if (!SpecificCoordinates.ContainsKey(Frame))
                return DefaultCoordinate;
            return SpecificCoordinates[Frame].GetPoint();
        }

        public void AddFramePoint2x(int Frame, int x, int y, float Rotation = 0f, bool ShowWeapon = false)
        {
            AddFramePoint(Frame, new Point(x * 2, y * 2), Rotation, ShowWeapon);
        }

        public void AddFramePoint(int Frame, int x, int y, float Rotation = 0f, bool ShowWeapon = false)
        {
            AddFramePoint(Frame, new Point(x, y), Rotation, ShowWeapon);
        }

        public void AddFramePoint(int Frame, Point Position, float Rotation = 0f, bool ShowWeapon = false)
        {
            if (!SpecificCoordinates.ContainsKey(Frame))
            {
                AnimationPoint ap = new AnimationPoint(Position.X, Position.Y, Rotation, ShowWeapon);
                SpecificCoordinates.Add(Frame, ap);
            }
            else
            {
                SpecificCoordinates[Frame].X = Position.X;
                SpecificCoordinates[Frame].Y = Position.Y;
            }
        }

        public class AnimationPoint
        {
            public int X, Y;
            public float Rotation;
            public bool ShowWeapon;

            public Point GetPoint()
            {
                return new Point(X, Y);
            }

            public AnimationPoint(int X, int Y, float Rotation = 0f, bool ShowWeapon = false)
            {
                this.X = X;
                this.Y = Y;
                this.Rotation = Rotation;
                this.ShowWeapon = ShowWeapon;
            }
        }
    }
}
