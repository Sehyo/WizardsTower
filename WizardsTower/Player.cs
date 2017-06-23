using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Tile_Engine;

namespace WizardsTower
{
    public class Player : GameObject
    {
        float mouseX,mouseY;
        ContentManager Content;
        private bool dead = false;
        bool stopStuff = false;
        bool showArrows = false;
        bool directionMode = false;
        bool highlightTiles = false;
        public Vector2 mouseAtTile = new Vector2();
        public HeroUI ui = new HeroUI();
        private int health = 10;
        int timeCap = 150;
        String[] consoleInput;
        float[,] arrowLocations = new float[4, 1];
        public Texture2D arrows, moveButton, attackButton;

        #region Hero
        public Hero hero = new Monk();
        #endregion

        public bool isWizard = false;
        public MapSquare tTile1;
        public MapSquare tTile2;

        public int xPos, yPos;
        Rectangle mouseRectangle;
        enum GameState
        {
            idle,
            playing
        }
        GameState gameState;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public void receiveTextureFromClient(Texture2D ourTexture, Hero ourHero)
        {
            animations["afk"].Texture = ourTexture;
        }

        #region Constructooor
        public Player(ContentManager content)
        {
            Content = content;
            xPos = 1;
            yPos = 1;
            enabled = true;
            frameWidth = 64;
            frameHeight = 64;
            arrows = content.Load<Texture2D>(@"Textures\UInHUD\Arrows");
            moveButton = content.Load<Texture2D>(@"Textures\UInHUD\movebutton");
            attackButton = content.Load<Texture2D>(@"Textures\UInHUD\attackbutton");
            animations.Add("afk", new AnitmaionLines(content.Load<Texture2D>(@"Textures\Sprites\Player\BKnight"), 64, "afk"));
            animations["afk"].LoopAnimation = true;
            animations["afk"].FrameLength = 1f;
            PlayAnimation("afk");
            gameState = GameState.playing;
            StupidClass.inputTimer = 0.0f;

            List<Vector2> VL = new List<Vector2>();
            VL.Add(new Vector2(xPos, yPos));
            VL.Add(new Vector2(xPos + 1, yPos));
            VL.Add(new Vector2(xPos - 1, yPos));
            VL.Add(new Vector2(xPos, yPos + 1));
            VL.Add(new Vector2(xPos, yPos - 1));
            VL.Add(new Vector2(xPos + 1, yPos + 1));
            VL.Add(new Vector2(xPos - 1, yPos - 1));
            VL.Add(new Vector2(xPos + 1, yPos - 1));
            VL.Add(new Vector2(xPos - 1, yPos + 1));

            foreach (var Titem in VL)
            {
                bool add = true;
                foreach (Vector2 item in StupidClass.playerVTiles)
                {
                    if (item == Titem)
                        add = false;
                }
                if (add)
                    StupidClass.playerVTiles.Add(Titem);
            }
        }
        #endregion
        #region Publics
        public override void Update(GameTime gameTime, ref int currentTurn)
        {
            //TileMap.makeMapBlack();
            worldLocation.X = xPos * frameWidth;
            worldLocation.Y = yPos * frameHeight;
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 10, 10);
            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            hero.UpdateStats(this);
            if(gameState == GameState.playing)
                moveAndDirection(gameTime,ref mouseState,ref keyboardState,ref currentTurn);





            List<Vector2> VL = new List<Vector2>();
            if (xPos > 0 && yPos > 0)
            {
                VL.Add(new Vector2(xPos, yPos));
                VL.Add(new Vector2(xPos + 1, yPos));
                VL.Add(new Vector2(xPos - 1, yPos));
                VL.Add(new Vector2(xPos, yPos + 1));
                VL.Add(new Vector2(xPos, yPos - 1));
                VL.Add(new Vector2(xPos + 1, yPos + 1));
                VL.Add(new Vector2(xPos - 1, yPos - 1));
                VL.Add(new Vector2(xPos + 1, yPos - 1));
                VL.Add(new Vector2(xPos - 1, yPos + 1));
            }

            foreach (var Titem in VL)
            {
                bool add = true;
                foreach (Vector2 item in StupidClass.playerVTiles)
                {
                    if (item == Titem)
                        add = false;
                }
                if (add)
                    StupidClass.playerVTiles.Add(Titem);
            }
        }

        private void moveAndDirection(GameTime gameTime, ref MouseState mouseState, ref KeyboardState keyboardState, ref int currentTurn)
        {
            if (!dead)
            {
                if (hero.hp <= 0)
                    dead = true;

                if (keyboardState.IsKeyDown(Keys.Space))
                    highlightTiles = true;
                else
                    highlightTiles = false;

                #region Highlight & Select Tiles
                if (highlightTiles)
                {
                    short s = 10;
                    mouseState = Mouse.GetState();

                    for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        for (int c = 0; c < TileMap.mCells.GetLength(1); c++)
                            try
                            {
                                //Crashes if mouse goes outside the map while still inside the window.
                                if (TileMap.mCells[Camera.ScreenToWorld(mouseRectangle).X / 64, Camera.ScreenToWorld(mouseRectangle).Y / 64] == TileMap.mCells[i, c])
                                {
                                    Console.WriteLine(TileMap.mCells[i, c].CodeValue);
                                    TileMap.mCells[i, c].Tcolor.R = 150;
                                    TileMap.mCells[i, c].Tcolor.G = 255;
                                    TileMap.mCells[i, c].Tcolor.B = 150;

                                    if (mouseState.LeftButton == ButtonState.Pressed)
                                        if (TileMap.mCells[i, c] != tTile2)
                                        {
                                            tTile1 = TileMap.mCells[i, c];
                                        }
                                        else
                                        {
                                            tTile1 = TileMap.mCells[i, c];
                                            tTile2 = null;
                                        }

                                    if (mouseState.RightButton == ButtonState.Pressed)
                                        if (TileMap.mCells[i, c] != tTile1)
                                        {
                                            tTile2 = TileMap.mCells[i, c];
                                        }
                                        else
                                        {
                                            tTile2 = TileMap.mCells[i, c];
                                            tTile1 = null;
                                        }
                                }
                                else
                                    TileMap.mCells[i, c].Tcolor = Color.White;
                            }
                            catch
                            {
                                break;
                            }
                }
                else
                {
                    foreach (MapSquare item in TileMap.mCells)
                    {
                        item.Tcolor = Color.White;
                    }
                }
                #endregion
                //Mark targeted tile 1
                if (tTile1 != null)
                {
                    tTile1.Tcolor.R = 150;
                    tTile1.Tcolor.G = 150;
                    tTile1.Tcolor.B = 255;
                }
                //mark Ttile 2
                if (tTile2 != null)
                {
                    tTile2.Tcolor.R = 255;
                    tTile2.Tcolor.G = 150;
                    tTile2.Tcolor.B = 150;
                }
                if (StupidClass.CurrentTurn < 5 && StupidClass.players[StupidClass.CurrentTurn] == this)
                {
                    if (keyboardState.IsKeyDown(Keys.Up))
                        Camera.Move(new Vector2(0, -8));
                    if (keyboardState.IsKeyDown(Keys.Down))
                        Camera.Move(new Vector2(0, 8));
                    if (keyboardState.IsKeyDown(Keys.Left))
                        Camera.Move(new Vector2(-8, 0));
                    if (keyboardState.IsKeyDown(Keys.Right))
                        Camera.Move(new Vector2(8, 0));

                    /*Console.Clear();
                    Console.WriteLine(hero);
                    Console.WriteLine("Level: " + hero.level + "\nExp: " + hero.experience + "/" + hero.maxExp);
                    Console.WriteLine("Hp: " + hero.hp + "/" + hero.maxHp);
                    Console.WriteLine("Ap: " + hero.ap + "/" + hero.maxAp);
                    Console.WriteLine("Chance to dodge: " + hero.dodgeChance);
                    Console.WriteLine("Damage: " + hero.damage);
                    Console.WriteLine("Range: " + hero.equipment[0].range);*/
                }
                if (!isWizard)
                {
                    #region Make tiles visible
                    if (xPos > 0 && yPos > 0)
                    {
                        TileMap.mCells[xPos, yPos].visible = true;
                        TileMap.mCells[xPos + 1, yPos].visible = true;
                        TileMap.mCells[xPos - 1, yPos].visible = true;
                        TileMap.mCells[xPos, yPos + 1].visible = true;
                        TileMap.mCells[xPos, yPos - 1].visible = true;
                        TileMap.mCells[xPos + 1, yPos + 1].visible = true;
                        TileMap.mCells[xPos - 1, yPos - 1].visible = true;
                        TileMap.mCells[xPos + 1, yPos - 1].visible = true;
                        TileMap.mCells[xPos - 1, yPos + 1].visible = true;
                    }
                    #endregion
                    //temp skip turn
                    if (StupidClass.CurrentTurn < 5)
                    {
                        if (keyboardState.IsKeyDown(Keys.Escape) && StupidClass.players[StupidClass.CurrentTurn] == this && StupidClass.inputTimer <= 0.0f)
                        {
                            hero.ap = 0;
                            StupidClass.inputTimer = 5000;
                        }

                        if (StupidClass.players[StupidClass.CurrentTurn] == this)
                            ui.Update(ref hero.ap, mouseState, ref hero);

                        if (mouseState.LeftButton == ButtonState.Pressed && StupidClass.players[StupidClass.CurrentTurn] == this)
                        {
                            moveY = 0;
                            moveX = 0;
                            if (StupidClass.inputTimer <= 0.0f)
                            {
                                if (directionMode)
                                {
                                    if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64))))
                                    {
                                        hero.dir = 0;
                                        hero.Attack(hero, 0);
                                        directionMode = false;
                                        StupidClass.inputTimer = 4000;
                                        hero.ap--;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64))))
                                    {
                                        hero.dir = 3;
                                        hero.Attack(hero, 0);
                                        directionMode = false;
                                        hero.ap--;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                                    {
                                        hero.dir = 1;
                                        hero.Attack(hero, 0);
                                        directionMode = false;
                                        hero.ap--;
                                        StupidClass.inputTimer = 4000;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64))))
                                    {
                                        hero.dir = 2;
                                        hero.Attack(hero, 0);
                                        directionMode = false;
                                        hero.ap--;
                                        StupidClass.inputTimer = 4000;
                                    }
                                }
                                else if (showArrows)
                                {
                                    if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64))))
                                    {
                                        --moveY;
                                        stopStuff = true;
                                        stuff(gameTime);
                                        StupidClass.inputTimer = 4000;
                                        showArrows = false;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64))))
                                    {
                                        --moveX;
                                        stopStuff = true;
                                        stuff(gameTime);
                                        StupidClass.inputTimer = 4000;
                                        showArrows = false;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                                    {
                                        ++moveX;
                                        stopStuff = true;
                                        stuff(gameTime);
                                        StupidClass.inputTimer = 4000;
                                        showArrows = false;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64))))
                                    {
                                        ++moveY;
                                        stopStuff = true;
                                        stuff(gameTime);
                                        StupidClass.inputTimer = 4000;
                                        showArrows = false;
                                    }
                                }
                                else // Show move and attack buttons
                                {
                                    if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64))))
                                    {
                                        directionMode = false;
                                        showArrows = true;
                                        StupidClass.inputTimer = 3000;
                                    }
                                    else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                                    {
                                        // ATTACK BUTTON ACTIVATED
                                        showArrows = false;
                                        directionMode = true;
                                        StupidClass.inputTimer = 3000;
                                    }
                                }
                            }
                        }
                        else
                            stopStuff = true;
                    }
                    if (StupidClass.inputTimer > 0 && StupidClass.CurrentTurn < 5 && StupidClass.players[StupidClass.CurrentTurn] == this)
                        StupidClass.inputTimer -= 150;
                    if (stopStuff)
                    {
                        moveX = 0;
                        moveY = 0;
                        updateAnimation(gameTime);
                        stopStuff = false;
                    }

                    if (hero.ap <= 0)
                    {
                        currentTurn++;

                        hero.UpdateStats();
                        hero.TurnUpdate();
                        StupidClass.changeVTiles();
                    }

                }
                else//ADD WIZARD UPDATE HERE
                {
                    foreach (Monster item in Wizard.units)
                    {
                        item.UpdateStats();
                        if (item.hp <= 0)
                        {
                            Wizard.units.Remove(item);
                            foreach (Player player in StupidClass.players)
                            {
                                player.hero.experience += (short)(item.level * 2);
                            }
                            break;
                        }
                    }
                    if (StupidClass.CurrentTurn < 5)
                    {
                        if (StupidClass.players[StupidClass.CurrentTurn] == this)
                        {
                            if (keyboardState.IsKeyDown(Keys.B))
                                Wizard.ap += 1;
                            foreach (Monster item in Wizard.units)
                            {
                                item.Move(gameTime, ref mouseState);
                            }
                        }
                        if (keyboardState.IsKeyDown(Keys.Escape) && StupidClass.players[StupidClass.CurrentTurn] == this && StupidClass.inputTimer <= 0.0f)
                        {
                            StupidClass.inputTimer = 5000;

                            foreach (Monster item in Wizard.units)
                            {
                                item.turnUpdate();
                            }
                            currentTurn++;
                            Wizard.endTurn();
                            StupidClass.changeVTiles();
                        }
                        if (StupidClass.players[StupidClass.CurrentTurn] == this && StupidClass.inputTimer <= 0)
                        {
                            MouseState ms = Mouse.GetState();
                            WizardUI.Update(ref Wizard.ap, ms, tTile1, tTile2, Content);
                        }
                        if (StupidClass.inputTimer > 0 && StupidClass.CurrentTurn < 5 && StupidClass.players[StupidClass.CurrentTurn] == this)
                            StupidClass.inputTimer -= 150;
                    }
                }
            }
            else if (StupidClass.CurrentTurn < 5 && StupidClass.players[StupidClass.CurrentTurn] == this)
            {
                currentTurn++;
                StupidClass.changeVTiles();
            }
            else
            {
                xPos = 0;
                yPos = 0;
            }
        }

        public void stuff(GameTime gameTime)
        {
            base.Update(gameTime);
            if (TileMap.mCells[(int)(xPos + moveAmount.X), (int)(yPos + moveAmount.Y)].CodeValue == "iceage")
            {
                while (TileMap.mCells[(int)(xPos + moveAmount.X), (int)(yPos + moveAmount.Y)].CodeValue == "iceage")
                {
                    xPos += (int)moveAmount.X;
                    yPos += (int)moveAmount.Y;

                    if (xPos > 0 && yPos > 0)
                    {
                        TileMap.mCells[xPos, yPos].visible = true;
                        TileMap.mCells[xPos + 1, yPos].visible = true;
                        TileMap.mCells[xPos - 1, yPos].visible = true;
                        TileMap.mCells[xPos, yPos + 1].visible = true;
                        TileMap.mCells[xPos, yPos - 1].visible = true;
                        TileMap.mCells[xPos + 1, yPos + 1].visible = true;
                        TileMap.mCells[xPos - 1, yPos - 1].visible = true;
                        TileMap.mCells[xPos + 1, yPos - 1].visible = true;
                        TileMap.mCells[xPos - 1, yPos + 1].visible = true;
                    }
                    List<Vector2> VL = new List<Vector2>();
                    if (xPos > 0 && yPos > 0)
                    {
                        VL.Add(new Vector2(xPos, yPos));
                        VL.Add(new Vector2(xPos + 1, yPos));
                        VL.Add(new Vector2(xPos - 1, yPos));
                        VL.Add(new Vector2(xPos, yPos + 1));
                        VL.Add(new Vector2(xPos, yPos - 1));
                        VL.Add(new Vector2(xPos + 1, yPos + 1));
                        VL.Add(new Vector2(xPos - 1, yPos - 1));
                        VL.Add(new Vector2(xPos + 1, yPos - 1));
                        VL.Add(new Vector2(xPos - 1, yPos + 1));
                    }

                    foreach (var Titem in VL)
                    {
                        bool add = true;
                        foreach (Vector2 item in StupidClass.playerVTiles)
                        {
                            if (item == Titem)
                                add = false;
                        }
                        if (add)
                            StupidClass.playerVTiles.Add(Titem);
                    }
                }
            }
            else//Move without iceage
            {
                bool move = true;
                foreach (Player item in StupidClass.players)
                {
                    if (item.hero.pos.X == xPos + moveAmount.X && item.hero.pos.Y == yPos + moveAmount.Y)
                    {
                        move = false;
                        break;
                    }
                }
                foreach (Monster item in Wizard.units)
                {
                    if (item.pos.X == xPos + moveAmount.X && item.pos.Y == yPos + moveAmount.Y)
                    {
                        move = false;
                        break;
                    }
                }
                if (move)
                {
                    xPos += (int)moveAmount.X;
                    yPos += (int)moveAmount.Y;
                    hero.ap--;
                }
            }
            // reposCam();
            if (StupidClass.inputTimer > 0)
            {
                List<Vector2> VL = new List<Vector2>();
                if (xPos > 0 && yPos > 0)
                {
                    VL.Add(new Vector2(xPos, yPos));
                    VL.Add(new Vector2(xPos + 1, yPos));
                    VL.Add(new Vector2(xPos - 1, yPos));
                    VL.Add(new Vector2(xPos, yPos + 1));
                    VL.Add(new Vector2(xPos, yPos - 1));
                    VL.Add(new Vector2(xPos + 1, yPos + 1));
                    VL.Add(new Vector2(xPos - 1, yPos - 1));
                    VL.Add(new Vector2(xPos + 1, yPos - 1));
                    VL.Add(new Vector2(xPos - 1, yPos + 1));
                }

                foreach (var Titem in VL)
                {
                    bool add = true;
                    foreach (Vector2 item in StupidClass.playerVTiles)
                    {
                        if (item == Titem)
                            add = false;
                    }
                    if (add)
                        StupidClass.playerVTiles.Add(Titem);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!enabled || dead)
                return;
            if (!isWizard)
            {
                if (animations.ContainsKey(currentAnimation))
                {
                    SpriteEffects effect = SpriteEffects.None;
                    if (flipped)
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                    spriteBatch.Draw(animations[currentAnimation].Texture, Camera.WorldToScreen(WorldRectangle), animations[currentAnimation].FrameRectangle, Color.White, 0.0f, Vector2.Zero, effect, drawDepth);
                }
                else
                {
                    Console.WriteLine("Animation key {0} is not contained in animations.", currentAnimation);
                }
                if (StupidClass.CurrentTurn < 5)
                    if (gameState == GameState.playing && StupidClass.players[StupidClass.CurrentTurn] == this)
                    {
                        drawArrows(spriteBatch); 
                        ui.Draw(spriteBatch);
                    }
            }
            else
            {
                foreach (Monster item in Wizard.units)
                {
                    if (TileMap.mCells[(int)item.pos.X, (int)item.pos.Y].visible)
                    {
                        spriteBatch.Draw(item.texture, Camera.WorldToScreen(new Rectangle(item.WorldRectangle.X, item.WorldRectangle.Y, 64, 64)), null,
                            Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0.6f);

                        spriteBatch.Draw(StupidClass.redBox,
                        Camera.WorldToScreen(new Rectangle(item.WorldRectangle.X + 5, item.WorldRectangle.Y - 5, item.hpRed.Width, item.hpRed.Height))
                        , null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0.5f);
                        spriteBatch.Draw(StupidClass.greenBox,
                        Camera.WorldToScreen(new Rectangle(item.WorldRectangle.X + 5, item.WorldRectangle.Y - 5, item.hpGreen.Width, item.hpGreen.Height)),
                        null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0.4f);
                    }
                }
                if (StupidClass.CurrentTurn < 5 && StupidClass.players[StupidClass.CurrentTurn] == this)
                {
                    //ADD WIZARD UI HERE
                    //spriteBatch.Draw(StupidClass.greenBox, new Rectangle(550, 440, 200, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);

                    // for (int i = 0; i < Wizard.spells.Count-1; i++)
                    // {
                    //      spriteBatch.Draw(Wizard.spells[i].tex, new Rectangle(10+(i*70), 425, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);
                    // }

                    foreach (Monster item in Wizard.units)
                    {
                        item.drawArrows(spriteBatch);
                    }
                    WizardUI.Draw(spriteBatch);
                }
            }
            #region HpBar
            spriteBatch.Draw(StupidClass.redBox,
                Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 5, WorldRectangle.Y - 5, hero.hpRed.Width, hero.hpRed.Height))
                , null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(StupidClass.greenBox,
                Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 5, WorldRectangle.Y - 5, hero.hpGreen.Width, hero.hpGreen.Height)),
                null,
                Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0.4f);
            #endregion
        }

        private void drawArrows(SpriteBatch spriteBatch)
        {
            if (showArrows || directionMode)
            {
                spriteBatch.Draw(arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                spriteBatch.Draw(arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64)), new Rectangle(64, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                spriteBatch.Draw(arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64)), new Rectangle(128, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                spriteBatch.Draw(arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64)), new Rectangle(192, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
            }
            else
            {
                spriteBatch.Draw(moveButton, Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                spriteBatch.Draw(attackButton, Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
            }
        }
        #endregion
        #region Helpers
        public void checkLvlTrans()
        {
            Vector2 cCell = TileMap.GetCellByPixel(WorldCenter);
            if (TileMap.CellCodeValue(cCell).StartsWith("T_"))
            {
                string[] code = TileMap.CellCodeValue(cCell).Split('_');
                if (code.Length != 4)
                    return;
                LvlManager.LLevel(int.Parse(code[1]));
                WorldLocation = new Vector2(int.Parse(code[2]) * TileMap.tWidth, int.Parse(code[3]) * TileMap.tHeight);
                LvlManager.BackLocation = worldLocation;
            }
        }
        public void reset()
        {
            LvlManager.LLevel(int.Parse("0"));
            WorldLocation = new Vector2(int.Parse("0") * TileMap.tWidth, int.Parse("0") * TileMap.tHeight);
            LvlManager.BackLocation = worldLocation;
        }

        private void reposCam()
        {

                int sLocX = (int)Camera.WorldToScreen(worldLocation).X;
                int sLocY = (int)Camera.WorldToScreen(worldLocation).Y;
                if (sLocX > 384)
                    Camera.Move(new Vector2(sLocX - 384, 0));
                if (sLocX < 384)
                    Camera.Move(new Vector2(sLocX - 384, 0));
                if (sLocY > 320)
                    Camera.Move(new Vector2(0, sLocY - 320));
                if (sLocY < 320)
                    Camera.Move(new Vector2(0, sLocY - 320));
        
        }
        #endregion
    }
}