using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardsTower {
    public class AnitmaionLines {
        #region Declarations
        private int frameWidth, frameHeight, currentFrame;
        private Texture2D texture;

        private float frameTimer = 0f, frameDelay = 0.05f;

        private bool loopAnimation = true, finishedPlaying = false;

        private string name;
        private string nextAnimation;
        #endregion
        #region Properties
        public int FrameWidth
        {
            set { frameWidth = value; }
            get { return frameWidth; }
        }
        public int FrameHeight
        {
            set { frameHeight = value; }
            get { return frameHeight; }
        }
        public Texture2D Texture
        {
            set { texture = value; }
            get { return texture; }
        }
        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        public string NextAnimation
        {
            set { nextAnimation = value; }
            get { return nextAnimation; }
        }
        public bool LoopAnimation
        {
            set { loopAnimation = value; }
            get { return loopAnimation; }
        }
        public bool FinishedPlaying
        {
            get { return finishedPlaying; }
        }
        public int FrameCount
        {
            get { return texture.Width / frameWidth; }
        }
        public float FrameLength
        {
            get { return frameDelay; }
            set { frameDelay = value; }
        }
        public Rectangle FrameRectangle
        {
            get { return new Rectangle((currentFrame * frameWidth), 0, frameWidth, frameHeight); }
        }
        #endregion
        #region Constructor
        public AnitmaionLines(Texture2D texture, int frameWidth, string name)
        {
            this.texture = texture;
            this.frameWidth = frameWidth;
            this.frameHeight = texture.Height;
            this.name = name;
        }
        #endregion
        #region Public Methods
        public void Play()
        {
            currentFrame = 0;
            finishedPlaying = false;
        }
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameTimer += elapsed;
            if(frameTimer >= frameDelay)
            {
                currentFrame++;
                if (currentFrame >= FrameCount)
                {
                    if (loopAnimation)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = FrameCount - 1;
                        finishedPlaying = true;
                    }
                }
                frameTimer = 0f;
            }
            
        }
        #endregion
    }
}