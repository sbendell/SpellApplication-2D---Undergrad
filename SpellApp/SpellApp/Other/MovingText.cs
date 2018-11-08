using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class MovingText : Text
    {
        private bool MoveLeft = true;
        private Vector2 movingX = new Vector2(1, 0);
        private Vector2 movingY = new Vector2(0, 1);
        private float interval = 30f;
        private int remainingduration = 120;

        public MovingText(
            SpriteFont Font,
            Vector2 startLocation,
            string alignTo,
            string text)
            :base (Font, startLocation, alignTo)
        {
            TextValue = text;
        }

        public void ChangeDirection(GameTime gameTime)
        {
            interval++;

            if (interval > 60)
            {
                MoveLeft = !MoveLeft;
                interval = 0;
            }
        }

        public void MoveDirection()
        {
            if (MoveLeft)
            {
                Location -= movingX;
            }
            else
            {
                Location += movingX;
            }

            Location -= movingY;
        }

        public bool IsActive
        {
            get
            {
                return (remainingduration > 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            ChangeDirection(gameTime);
            MoveDirection();
            remainingduration--;
        }
    }
}
