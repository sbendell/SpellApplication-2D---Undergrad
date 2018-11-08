using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class SpellManager
    {
        public List<Spell> projSpells = new List<Spell>();
        public List<LocationSpell> locationSpells = new List<LocationSpell>();
        public List<LineSpell> lineSpells = new List<LineSpell>();

        private Rectangle screenBounds;
        private static Texture2D texture;
        private static int frameCount;
        private static float spellSpeed = 480f;

        public BuffSpell ArcanePower;
        public Spell Regenerate;

        int spellID = 0;

        public readonly Rectangle fireballInitialFrame = new Rectangle(0, 0, 20, 46);
        public readonly Rectangle lightningBoltInitialFrame  = new Rectangle(0, 51, 38, 38);
        public readonly Rectangle arcanePowerInitialFrame = new Rectangle(0, 94, 64, 72);
        public readonly Rectangle lightningLineInitialFrame = new Rectangle(0, 171, 48, 146);
        public readonly Rectangle arcaneExplosionInitialFrame = new Rectangle(0, 322, 90, 90);
        public readonly Rectangle regenerateInitialFrame = new Rectangle(0, 417, 64, 72);

        public SpellManager(
            Texture2D Texture,
            int FrameCount,
            Rectangle ScreenBounds)
        {
            texture = Texture;
            frameCount = FrameCount;
            this.screenBounds = ScreenBounds;

            ArcanePower = new BuffSpell(
                new Vector2(-500, -500),
                texture,
                arcanePowerInitialFrame,
                "ArcanePower",
                10f,
                60f,
                1.2f);

            for (int x = 1; x < frameCount; x++)
            {
                ArcanePower.AddFrame(new Rectangle(
                    arcanePowerInitialFrame.X + (arcanePowerInitialFrame.Width * x),
                    arcanePowerInitialFrame.Y,
                    arcanePowerInitialFrame.Width,
                    arcanePowerInitialFrame.Height));
            }

            Regenerate = new Spell(
                new Vector2(-500, -500),
                texture,
                regenerateInitialFrame,
                "Regenerate");

            for (int x = 1; x < 10; x++)
            {
                Regenerate.AddFrame(new Rectangle(
                    regenerateInitialFrame.X + (regenerateInitialFrame.Width * x),
                    regenerateInitialFrame.Y,
                    regenerateInitialFrame.Width,
                    regenerateInitialFrame.Height));
            }
        }

        public void CastSpell(
            Vector2 startLocation,
            Vector2 mouseLocation,
            string spell,
            double spellpower)
        {
            Vector2 tempVelocity;
            tempVelocity = (mouseLocation - startLocation);

            if (spell == "Fireball")
            {
                DamageSpell thisFireball = new DamageSpell(
                startLocation,
                texture,
                fireballInitialFrame,
                spell,
                spellpower,
                2f);

                for (int x = 1; x < frameCount; x++)
                {
                    thisFireball.AddFrame(new Rectangle(
                        fireballInitialFrame.X + (fireballInitialFrame.Width * x),
                        fireballInitialFrame.Y,
                        fireballInitialFrame.Width,
                        fireballInitialFrame.Height));
                }
                thisFireball.Velocity = Movement.Direction(tempVelocity) * spellSpeed;
                thisFireball.Rotation = Movement.Rotate(tempVelocity, startLocation, mouseLocation);
                thisFireball.CollisionRadius = 12;
                projSpells.Add(thisFireball);
            }
            if (spell == "LightningBolt")
            {
                DamageSpell thisLightningBolt = new DamageSpell(
                startLocation,
                texture,
                lightningBoltInitialFrame,
                spell,
                spellpower,
                8f);

                for (int x = 1; x < frameCount; x++)
                {
                    thisLightningBolt.AddFrame(new Rectangle(
                        lightningBoltInitialFrame.X + (lightningBoltInitialFrame.Width * x),
                        lightningBoltInitialFrame.Y,
                        lightningBoltInitialFrame.Width,
                        lightningBoltInitialFrame.Height));
                }
                thisLightningBolt.Velocity = Movement.Direction(tempVelocity) * spellSpeed;
                thisLightningBolt.CollisionRadius = 17;
                projSpells.Add(thisLightningBolt);
            }

            if (spell == "LightningLine")
            {
                spellID += 1;

                LineSpell thisLightningLine = new LineSpell(
                startLocation,
                texture,
                lightningLineInitialFrame,
                spell,
                spellpower,
                3f,
                30,
                spellID);

                for (int x = 1; x < frameCount; x++)
                {
                    thisLightningLine.AddFrame(new Rectangle(
                        lightningLineInitialFrame.X + (lightningLineInitialFrame.Width * x),
                        lightningLineInitialFrame.Y,
                        lightningLineInitialFrame.Width,
                        lightningLineInitialFrame.Height));
                }
                thisLightningLine.Rotation = Movement.Rotate(tempVelocity, startLocation, mouseLocation);
                thisLightningLine.Location += (Movement.Direction(tempVelocity) * 72f);
                lineSpells.Add(thisLightningLine);
            }
            if (spell == "ArcaneExplosion")
            {
                spellID += 1;

                LocationSpell thisArcaneExplosion = new LocationSpell(
                mouseLocation,
                texture,
                arcaneExplosionInitialFrame,
                spell,
                spellpower,
                3f,
                55,
                spellID);

                for (int x = 1; x < 10; x++)
                {
                    thisArcaneExplosion.AddFrame(new Rectangle(
                        arcaneExplosionInitialFrame.X + (arcaneExplosionInitialFrame.Width * x),
                        arcaneExplosionInitialFrame.Y,
                        arcaneExplosionInitialFrame.Width,
                        arcaneExplosionInitialFrame.Height));
                }
                locationSpells.Add(thisArcaneExplosion);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int x = projSpells.Count - 1; x >= 0; x--)
            {
                projSpells[x].Scale += 0.005f;
                projSpells[x].Update(gameTime);
                if (!screenBounds.Contains(projSpells[x].Center))
                {
                    projSpells.RemoveAt(x);
                }
            }
            for (int x = locationSpells.Count - 1; x >= 0; x--)
            {
                if (locationSpells[x].IsActive)
                {
                    locationSpells[x].Update(gameTime);
                }
                else
                {
                    locationSpells.RemoveAt(x);
                }
            }
            for (int x = lineSpells.Count - 1; x >= 0; x--)
            {
                if (lineSpells[x].IsActive)
                {
                    lineSpells[x].Update(gameTime);
                }
                else
                {
                    lineSpells.RemoveAt(x);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Spell spell in projSpells)
            {
                spell.Draw(spriteBatch);
            }
            foreach (LocationSpell spell in locationSpells)
            {
                spell.Draw(spriteBatch);
            }
            foreach (LineSpell spell in lineSpells)
            {
                spell.Draw(spriteBatch);
            }
        }
    }
}
