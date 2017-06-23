using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;


namespace WizardsTower
{
    public static class SoundManager
    {/*
        private static SoundEffect characterVoice; // <---- Make soundbank or sumthing
        private static SoundEffect die;
        private static SoundEffect completeLevel;
        private static SoundEffect backgroundMusic;
        private static SoundEffect levelMusic;
        public static SoundEffect intro;
        public static SoundEffect introTheme;
        public static SoundEffectInstance backgroundMusicInstance;
        public static SoundEffectInstance introInstance;
        public static SoundEffectInstance introNarrator;
        public static SoundEffectInstance levelMusicInstance;
        public static void Initialize(ContentManager content)
        {
            try {
            // Example Code on how to add sound files
            #region Example Code
            /*
                backgroundMusic = content.Load<SoundEffect>(@"Sounds\backgroundMusic");      
                backgroundMusicInstance = backgroundMusic.CreateInstance();
             * 
            #endregion
            // LATER ONE IN SEPARATE FUNCTION USE .Stop() TO RESET AN AUDIO FILE FROM CURRENT POINT TO START
            }
            catch
            {
                Debug.Write("SoundManager initialization failed, sadface :'(");
            }
            
        }
       public static void playBackgroundMusic()
        {
            try
            {
                
                if(!backgroundMusicInstance.IsLooped) // To prevent IsLooped to run twice, if it's ran twice the backgroundMusic will fail to play.
                {
                    backgroundMusicInstance.Volume = 0.25f;
                    backgroundMusicInstance.IsLooped = true;
                }
                levelMusicInstance.Play();
            }
            catch
            {
                Debug.Write("SoundManager failed to play backgroundMusic");
            }
        }
        public static void pauseBackgroundMusic()
        {
            levelMusicInstance.Pause();
        }
    } 
  /*  public class SoundEffectInstance : IDisposable
    {

    } */
    }
}