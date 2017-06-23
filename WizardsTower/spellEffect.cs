using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;
namespace WizardsTower
{
    public class spellEffect : GameObject
    {
        public int X,Y;
        public void changeTypeOfEffect(string spellType, ContentManager Content)
        {
            switch(spellType)
            {
                case "fire":
                    animations.Add("fire",new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\TileEffects\newFire"),
                        48,
                        "fire"));
                    animations["fire"].LoopAnimation = true;
                    animations["fire"].FrameLength = 0.15f;
                    PlayAnimation("fire");
                    drawDepth = 0.875f;
                    CollisionRectangle = new Rectangle(0,0,64,64);
                    enabled = true;
                    break;
                default:
                    enabled = false;
                    break;
            }
        }
        public spellEffect(ContentManager Content,int cellX,int cellY, string spellType)
        {
            X = cellX;
            Y = cellY;
            worldLocation.X = TileMap.tWidth * cellX;
            worldLocation.Y = TileMap.tHeight * cellY;
            frameWidth = TileMap.tWidth;
            frameHeight = TileMap.tHeight;
            switch(spellType)
            {
                case "fire":
                    animations.Add("fire", new AnitmaionLines(Content.Load<Texture2D>(@"Textures\Sprites\TileEffects\newFire"),
                        48,
                        "fire"));
                        animations["fire"].LoopAnimation = true;
                        animations["fire"].FrameLength = 0.15f;
                        PlayAnimation("fire");
                        drawDepth = 0.875f;
                        CollisionRectangle = new Rectangle(0,0,64,64);
                        enabled = true;
                    break;
                default:
                    goto case "fire";
                    break;
            }
        }

    }
}
