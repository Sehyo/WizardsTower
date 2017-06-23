using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;

namespace WizardsTower {
    public class GameObject {
        #region Declarations
        protected Vector2 worldLocation;
        protected int frameWidth, frameHeight;
        protected bool enabled, flipped = false, onGround; // if flipped is true the sprite will be mirrored
        protected Rectangle collisionRectangle;
        protected int collideWidth, collideHeight;
        protected bool codeBasedBlocks = true;
        protected float drawDepth = 0.85f;
        public Dictionary<string, AnitmaionLines> animations = new Dictionary<string, AnitmaionLines>();
        protected string currentAnimation;
        private Random rand = new Random();
        protected int moveX, moveY;
        protected Vector2 moveAmount;
        private Color[] colors = { Color.White, Color.Black, Color.LightBlue, Color.LightCyan, Color.LightGreen, Color.LightSeaGreen, Color.LightYellow };
        #endregion
        #region Properties
        public bool Enabled
        {
            set { enabled = value; }
            get { return enabled; }
        }
        public Vector2 WorldLocation
        {
            set { worldLocation = value; }
            get { return worldLocation; }
        }
        public Vector2 WorldCenter
        {
            get
            {
                return new Vector2(
                    (int)worldLocation.X + (int)(frameWidth / 2),
                    (int)worldLocation.Y + (int)frameHeight / 2);
            }
        }
        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                    (int)worldLocation.X,
                    (int)worldLocation.Y,
                    frameWidth,
                    frameHeight);
            }
        }
        public Rectangle CollisionRectangle
        {
            set { collisionRectangle = value; }
            get { return new Rectangle(
                (int)worldLocation.X + collisionRectangle.X,
                (int)WorldRectangle.Y + collisionRectangle.Y,
                collisionRectangle.Width,
                collisionRectangle.Height); }
        }
        #endregion
        #region Helpers
        internal void updateAnimation(GameTime gameTime)
        {
            if(animations.ContainsKey(currentAnimation))
            {
                if(animations[currentAnimation].FinishedPlaying)
                {
                    PlayAnimation(animations[currentAnimation].NextAnimation);
                }
                else
                {
                    animations[currentAnimation].Update(gameTime);
                }
            }
        }
        #endregion
        #region Public Methods
        public void PlayAnimation(string name)
        {
            if(!(name == null) && animations.ContainsKey(name))
            {
                currentAnimation = name;
                animations[name].Play();
            }
        }
        public virtual void Update(GameTime gameTime, ref int currentTurn)
        {
        }
        public virtual void Update(GameTime gameTime)
        {
            if(!enabled)
                return;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            moveAmount = new Vector2(moveX, moveY);

            moveAmount = horizontalCollisionTest(moveAmount);
            moveAmount = verticalCollisionTest(moveAmount);
           // updateAnimation(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(!enabled)
                return;
            if (animations.ContainsKey(currentAnimation))
            {
                SpriteEffects effect = SpriteEffects.None;
                if (flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }

                spriteBatch.Draw(animations[currentAnimation].Texture, Camera.WorldToScreen(WorldRectangle), animations[currentAnimation].FrameRectangle, Color.White, 0.0f, Vector2.Zero, effect, drawDepth);
            }
            else { Console.WriteLine("Animation key {0} is not contained in animations.", currentAnimation); }
        }
        #endregion
        #region MapBased Collision Detection Methods
        private Vector2 horizontalCollisionTest(Vector2 moveAmount)
        {
            if(moveAmount.X == 0)
                return moveAmount;
            //Vector2 mapCell1 = TileMap.GetCellByPixel(corner1), mapCell2 = TileMap.GetCellByPixel(corner2);
            Vector2 mapCell1 = TileMap.GetCellByPixel(new Vector2(WorldLocation.X,WorldLocation.Y));
            if(moveX > 0)
                mapCell1 += new Vector2(1, 0);
            else if(moveX < 0)
                mapCell1 -= new Vector2(1, 0);
            if(!TileMap.CellIsPassable(mapCell1))
            {
                moveAmount.X = 0;
            }

            if(codeBasedBlocks)
            { // I like cookies
                if(TileMap.CellCodeValue(mapCell1) == "BLOCK")
                {
                    moveAmount.X = 0;
                }
            }
            return moveAmount;     
        }
        private Vector2 verticalCollisionTest(Vector2 moveAmount)
        {
            if(moveAmount.Y == 0)
                return moveAmount;
            //Vector2 mapCell1 = TileMap.GetCellByPixel(corner1), mapCell2 = TileMap.GetCellByPixel(corner2);
            Vector2 mapCell1 = TileMap.GetCellByPixel(new Vector2(WorldLocation.X,WorldLocation.Y));
            if(moveY > 0)
                mapCell1 += new Vector2(0, 1);
            else if(moveY < 0)
                mapCell1 -= new Vector2(0, 1);
            if(!TileMap.CellIsPassable(mapCell1))
            {
                moveAmount.Y = 0;
                Console.WriteLine("Cell: " + mapCell1 + "is not passible");
            }
           // else
            //    Console.WriteLine("Cell: " + mapCell1 + "is passible");
            if(codeBasedBlocks)
            { // I like cookies
                if(TileMap.CellCodeValue(mapCell1) == "BLOCK")
                {
                    moveAmount.Y = 0;
                }
            }
            return moveAmount;    
        }
        #endregion
    }
}