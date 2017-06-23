using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Tile_Engine;

namespace WizardsTower
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player[] players = new Player[5];

        static public int currentTurn = 0;


        SpriteFont font;
        private Random rand = new Random();
        private Color[] colors = { Color.White, Color.Black, Color.LightBlue, Color.LightCyan, Color.LightGreen, Color.LightSeaGreen, Color.LightYellow };


        enum gameState
        {
            playing
        };
        gameState GameState = gameState.playing;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
           //this.graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();
        }
        protected override void Initialize()
        {
            base.Initialize();

            foreach (MapSquare item in TileMap.mCells)
            {
                item.Tcolor = Color.White;
            }
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TileMap.Initialize(Content.Load<Texture2D>(@"Textures\PlatformTiles"));
            Camera.WorldRectangle = new Rectangle(0, 0, 200 * 64, 200 * 64);
            Camera.Position = Vector2.Zero;
            Camera.ViewPortWidth = 800;
            Camera.ViewPortHeight = 600;

            players[0] = new Player(Content);
            players[0].xPos = 1;
            players[0].animations["afk"] = new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\Player\Wizard"), 64, "afk");
            //make wizard
            players[1] = new Player(Content);
            players[1].xPos = 2;
            players[1].hero = new Swordsman();
            players[1].animations["afk"] = new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\Player\Knight"), 64, "afk");


            players[2] = new Player(Content);
            players[2].xPos = 3;
            players[2].hero = new Monk();
            players[2].animations["afk"] = new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\Player\Monk"), 64, "afk");

            HeroUIButton[] ui1 = new HeroUIButton[2];
            ui1[0] = new HeroUIButton(new Rectangle(10, 425, 64, 64), new Heal(Content.Load<Texture2D>(@"Textures\UInHUD\HeroSpells\heal")));
            ui1[1] = new HeroUIButton(new Rectangle(84, 425, 64, 64), new Sacrifice(Content.Load<Texture2D>(@"Textures\UInHUD\HeroSpells\heal")));

            players[2].ui.init(ui1);

            players[3] = new Player(Content);
            players[3].xPos = 4;
            players[3].hero = new Bandit();
            players[3].animations["afk"] = new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\Player\Thief"), 64, "afk");
            players[4] = new Player(Content);
            players[4].xPos = 5;
            players[4].hero = new MiniWizard();
            players[4].animations["afk"] = new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\Player\MiniWizard"), 64, "afk");

            players[0].isWizard = true;
            players[0].xPos = 0;
            players[0].yPos = 0;
            foreach (Player item in players)
            {
                item.hero.UpdateStats(item);
                item.hero.Init();
                if (item.hero.agility == 3 || item.hero.intelligence == 3)
                    item.hero.equipment[0].range = 3;
            }


            players[4].hero.ap = 0;
            currentTurn = 4;
            
            LvlManager.Initialize(Content, players[0]);
            LvlManager.LLevel(0);
            font = Content.Load<SpriteFont>(@"Fonts\Font");
            StupidClass.font = font;

            StupidClass.greenBox = Content.Load<Texture2D>(@"Textures\UInHUD\greenbox");
            StupidClass.redBox = Content.Load<Texture2D>(@"Textures\UInHUD\redbox");

            TileMap.effectTextures.Add(Content.Load<Texture2D>(@"Textures\Sprites\Lines\invisLine"));
            TileMap.effectTextures.Add(Content.Load<Texture2D>(@"Textures\Sprites\TileEffects\fire"));
            
            Wizard.initialize(Content);
            Wizard.spells[0].tex = Content.Load<Texture2D>(@"Textures\UInHUD\WizardSpells\darkness");
            Wizard.spells[1].tex = Content.Load<Texture2D>(@"Textures\UInHUD\WizardSpells\fire");
            Wizard.spells[2].tex = Content.Load<Texture2D>(@"Textures\UInHUD\WizardSpells\glue");
            Wizard.spells[3].tex = Content.Load<Texture2D>(@"Textures\UInHUD\WizardSpells\teleport");
            Wizard.spells[4].tex = Content.Load<Texture2D>(@"Textures\UInHUD\WizardSpells\iceage");
            Wizard.spells[5].tex = Content.Load<Texture2D>(@"Textures\Sprites\Monsters\Ogre");
            Wizard.spells[6].tex = Content.Load<Texture2D>(@"Textures\Sprites\Monsters\Skeleton");
            Wizard.spells[7].tex = Content.Load<Texture2D>(@"Textures\Sprites\Monsters\Imp");

            TileMap.te.Add(Content.Load<Texture2D>(@"Textures\Sprites\TileEffects\fire"));
            TileMap.te.Add(Content.Load<Texture2D>(@"Textures\Sprites\TileEffects\glue"));
            TileMap.te.Add(Content.Load<Texture2D>(@"Textures\Sprites\TIleEffects\ice"));

            WizardUI.init(Content.Load<Texture2D>(@"Textures\UInHUD\mana"), Content.Load<Texture2D>(@"Textures\UInHUD\manabar"),
            Content.Load<Texture2D>(@"Textures\UInHUD\warrows"), Content.Load<Texture2D>(@"Textures\UInHUD\WUI"));
        }
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GameState == gameState.playing)
            {
              //  Console.Clear();

                StupidClass.StupidFunction(ref players, ref currentTurn, Content);

                if (currentTurn > 4)
                    currentTurn = 0;
                foreach (Player item in players)
                {
                    item.Update(gameTime, ref currentTurn);
                }
                
                StupidClass.StupidFunction(ref players, ref currentTurn, Content);

                LvlManager.Update(gameTime);
                KeyboardState keyState = Keyboard.GetState();
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            GraphicsDevice.Clear(Color.Blue);

            if (GameState == gameState.playing)
            {
                TileMap.Draw(spriteBatch);
                foreach (var item in players)
                {
                    item.Draw(spriteBatch);
                }
                LvlManager.Draw(spriteBatch);
                spriteBatch.GraphicsDevice.Clear(Color.Black);
            } 

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}