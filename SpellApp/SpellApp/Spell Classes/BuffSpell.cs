using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class BuffSpell : Spell
    {
        private float duration;
        private float persDuration;
        public bool IsActive = false;
        private float multiplier;
        private float maxCooldown;
        public float cooldown;

        public BuffSpell(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            string SpellName,
            float Duration,
            float Cooldown,
            float Multiplier)
            :base (location,
                 texture,
                 initialFrame,
                 SpellName)
        {
            duration = Duration;
            persDuration = Duration;
            multiplier = Multiplier;
            maxCooldown = Cooldown;
        }

        public float MaxCooldown
        {
            get { return maxCooldown; }
            set { maxCooldown = value; }
        }

        public float Buff()
        {
            if (IsActive)
            {
                return multiplier;
            }
            else
            {
                return 1.0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (cooldown > 0)
            {
                cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (IsActive)
            {
                persDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (persDuration <= 0f)
            {
                IsActive = false;
                persDuration = duration;
                cooldown = maxCooldown;
            }
        }
    }
}
