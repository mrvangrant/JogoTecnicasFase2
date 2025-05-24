
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace JogoTecnicas
{
    public static class Sound
    {
        private static SoundEffect saltarSom;
        private static SoundEffect morrerSom;
        private static Song musicaSom;

        public static void LoadContent(ContentManager content)
        {
           
            musicaSom = content.Load<Song>("Delightful Stroll");
            saltarSom = content.Load<SoundEffect>("jump");
            morrerSom = content.Load<SoundEffect>("hurt");
        }

        public static void PlayJump()
        {
            saltarSom.Play();
        }

        public static void PlayDeath()
        {
            morrerSom.Play();
        }

        public static void PlayBackgroundMusic()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(musicaSom);
            }
        }

    
    }
}