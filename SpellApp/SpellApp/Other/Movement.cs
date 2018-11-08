using System;
using Microsoft.Xna.Framework;

namespace SpellApp
{
    public static class Movement
    {
        public static Vector2 Direction(Vector2 tempVelocity)
        {
            float tempMagnitudeSqrd;
            tempMagnitudeSqrd = ((tempVelocity.X * tempVelocity.X) + (tempVelocity.Y * tempVelocity.Y));
            float tempMagnitude = (float)Math.Sqrt(tempMagnitudeSqrd);
            return (tempVelocity / tempMagnitude);
        }

        public static float Rotate(Vector2 tempVelocity, Vector2 startLocation, Vector2 mouseLocation)
        {
            float rotation;
            rotation = (float)Math.Atan(tempVelocity.X / -tempVelocity.Y);
            if ((mouseLocation - startLocation).Y >= 0)
            {
                rotation -= MathHelper.Pi;
            }
            return rotation;
        }
    }
}
