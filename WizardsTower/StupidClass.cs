using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tile_Engine;
using Microsoft.Xna.Framework.Content;

namespace WizardsTower
{
    public static class StupidClass
    {
        public static Player[] players;
        public static int CurrentTurn;
        public static Texture2D redBox;
        public static Texture2D greenBox;
        public static float inputTimer = 0;
        public static SpriteFont font;
        public static ContentManager content;
        public static List<Vector2> playerVTiles = new List<Vector2>();

        public static void changeVTiles()
        {
            if (CurrentTurn >= 4)
            {
                foreach (MapSquare item in TileMap.mCells)
                {
                    item.visible = true;
                    item.CVUpdate();
                }
            }
            else
            {
                foreach (MapSquare item in TileMap.mCells)
                {
                    item.visible = false;
                }
                foreach (Vector2 item in playerVTiles)
                {
                    TileMap.mCells[(int)item.X, (int)item.Y].visible = true;
                }
            }
        }

        public static void StupidFunction(ref Player[] p, ref int t, ContentManager c)
        {
            players = p;
            CurrentTurn = t;
            content = c;
        }
    }
}
