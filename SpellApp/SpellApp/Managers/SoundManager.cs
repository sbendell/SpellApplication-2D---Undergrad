using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace SpellApp
{
    public static class SoundManager
    {
        private static SoundEffect playerCastFireball;
        private static SoundEffect playerCastLightningBall;
        private static SoundEffect playerCastLightningLine;
        private static SoundEffect music;
        private static SoundEffectInstance instance;

        public static bool SoundOn = true;
        public static bool MusicOn = true;

        public static float volume = 1.0f;

        public static void Initialize(ContentManager content)
        {
            playerCastFireball = content.Load<SoundEffect>(@"Fireball");
            playerCastLightningBall = content.Load<SoundEffect>(@"LightningBall");
            playerCastLightningLine = content.Load<SoundEffect>(@"LightningLine");
            music = content.Load<SoundEffect>(@"17 Map Theme");
            instance = music.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
        }

        public static void PlayerCastFireball()
        {
            if (SoundOn)
            {
                playerCastFireball.Play();
            }
        }

        public static void PlayerCastLightningBall()
        {
            if (SoundOn)
            {
                playerCastLightningBall.Play();
            }
        }

        public static void PlayerCastLightningLine()
        {
            if (SoundOn)
            {
                playerCastLightningLine.Play();
            }
        }

        public static void PlayMusic()
        {
            if (MusicOn)
            {
                instance.Resume();
            }
            else
            {
                instance.Pause();
            }
            instance.Volume = volume;
        }
    }
}
