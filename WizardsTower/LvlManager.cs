using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tile_Engine;


namespace WizardsTower
{
    public static class LvlManager
    {
        #region Decs
        private static ContentManager Content;
        private static Player player;
        private static int cLvl;
        private static Vector2 backLocation;
        #region ExampleCode
        // If you create a class for enemies, initialize them here like this.
        //private static List<NAMEOFCLASS> NAME = new List<NAMEOFCLASS>();
        public static List<spellEffect> goodSpellList = new List<spellEffect>();
        #endregion
        private static int healthTime = 0;
        #endregion

        #region Props
        public static int CLvl
        {
            get { return cLvl; }
        }
        public static Vector2 BackLocation
        {
            set { backLocation = value; }
            get { return backLocation; }
        }
        #endregion
        #region Init
        public static void Initialize(ContentManager content,Player pPlayer)
        {
            Content = content;
            player = pPlayer;
        }
        #endregion
        #region Publics
        public static void LLevel(int lvlNumber)
        {
           // TileMap.LoadMap((System.IO.FileStream)TitleContainer.OpenStream(@"Content\Levels\MAP" + lvlNumber.ToString().PadLeft(3,'0') + ".MAP"));
            TileMap.LoadMap((System.IO.FileStream)TitleContainer.OpenStream(@"Content\Levels\MAP001.MAP"));

            for(int z = 0;z < TileMap.mWidth;z++)
            {
                for(int awesome = 0;awesome < TileMap.mHeight;awesome++)
                {
                    if(TileMap.CellCodeValue(z,awesome) == "START")
                    {
                        player.WorldLocation = new Vector2(z * TileMap.tWidth,awesome * TileMap.tHeight);
                    }
                    #region ExampleCode
                    // After initializing and defining enemies and or objects / powerups at top, you have to do this
                    // So the map knows to add it to the game, here below is an example.
                    /*
                    if(TileMap.CellCodeValue(z, awesome) == "MAPCODE")
                    {
                        NAME.Add(new NAMEOFCLASS(Content, z, awesome));
                    } */
                    #endregion
                }
            }
            cLvl = lvlNumber;
            backLocation = player.WorldLocation;
        }
        public static void RLevel()
        {
            Vector2 sRespawn = backLocation;
            LLevel(cLvl);
            backLocation = sRespawn;
            player.WorldLocation = backLocation;
        }
        public static void checkAttackHit()
        {
            #region ExampleCode
            // This function can be used to check if spell and stuff hits an enemy
            // BUT IF TO CHECK IF SOMETHING HITS AN ENEMY, TO THIS FOR EACH ENEMY LIST
            // --EXAMPLE---
            /*
                foreach(NAMEOFCLASS WHATEVER in NAME)
                {                                          
                    if(WHATEVER.CollisionRectangle.Intersects(RECTANGLEOFWHATMIGHTHITENEMY.CollisionRectangle))
                    {
                    // DOESNT HAVE TO BE .dead can BE MINUS HP OR WHATEVER
                        WHATEVER.dead = true;
                    }
                } */
            #endregion
        }
        public static void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if(healthTime > 0)
                healthTime -= gameTime.ElapsedGameTime.Milliseconds;
            // Checks for lethal blocks
            Vector2 cCell = TileMap.GetCellByPixel(player.WorldCenter);
            if(TileMap.CellCodeValue(cCell).StartsWith("LETHAL"))
            {
                #region ExampleCode
                //player.Health -= 10000; // This is to check if player goes on a tile that has the MAPCODE "LETHAL", this kills the player
                #endregion
            }
            foreach(spellEffect SpellEffect in goodSpellList)
            {
                SpellEffect.Update(gameTime);
            }
            #region ExampleCode
            // Check if LETHAL blocks -- ends here
            // IF WE HAVE POWERUPS, THIS IS HOW TO CHECK IF THE PLAYER IS INTERSECTING A POWER UP
            // IF HE IS HE WILL PICK IT UP AND ACTIVATE IT AUTOMATICALLY
            /*for (int x = POWERUPLISTNAME.Count - 1; x >= 0; x--)
            {
                POWERUPLISTNAME[x].Update(gameTime);
                if (player.CollisionRectangle.Intersects(POWERUPLISTNAME[x].CollisionRectangle))
                {
                    POWERUPLISTNAME.RemoveAt(x);
                    CODE FOR HANDLING WHAT THIS POWERUP WILL DO
                }
            }*/
            // THE CODE FOR OTHER THINGS WOULD BE KIND OF THE SAME AS FOR OTHER THINGS :P
            #endregion
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            #region ExampleCode
            // THIS IS AN EXAMPLE OF HOW TO DRAW
            // LEVEL ITEMS, LIKE ENEMIES AND POWERUPS AND WHATEVER
            // ITS EXACTLY THE SAME FOR EVERYTHING
            /*
                foreach(CLASSNAME WHATEVER in NAMEOFLIST)
                    WHATEVER.Draw(spriteBatch); */
            #endregion
            foreach(spellEffect SpellEffect in goodSpellList)
                SpellEffect.Draw(spriteBatch);
        }
        #endregion
    }
}