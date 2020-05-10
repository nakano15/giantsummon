using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    class UtilityMethods
    {
        public static float Bezier(float t, float a, float b, float c)
        {
            float ab = MathHelper.Lerp(a, b, t);
            float bc = MathHelper.Lerp(b, c, t);
            return MathHelper.Lerp(ab, bc, t);
        }

        public static Vector2 Bezier2D(float t, Vector2 a, Vector2 b, Vector2 c)
        {
            var ab = Vector2.Lerp(a, b, t);
            var bc = Vector2.Lerp(b, c, t);
            return Vector2.Lerp(ab, bc, t);
        }
    }
}
