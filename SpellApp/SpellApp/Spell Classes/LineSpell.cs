using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class LineSpell : LocationSpell
    {
        public LineSpell(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            string SpellName,
            double SpellPower,
            float sppwmlt,
            int duration,
            int SpellID)
            :base (location, 
                 texture,
                 initialFrame,
                 SpellName,
                 SpellPower,
                 sppwmlt,
                 duration,
                 SpellID)
        {

        }
        public Vector2 Top
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, 0);
            }
        }
        public Vector2 Bottom
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, frameHeight);
            }
        }
    }
}
