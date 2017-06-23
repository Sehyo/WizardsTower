using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tile_Engine;

namespace WizardsTower
{
    public abstract class EntitySpells
    {
        protected byte trueCost;
        public byte cost;
        public short type;
        public Texture2D tex;
        public virtual void useSpell(ref short c)
        {
        }
        public virtual void useSpell(Hero user, BaseGameEntity target)
        {
        }
        public virtual void useSpell(ref short c, Vector2 p, ref short h)
        {
        }
        //stuff
    }
    #region Monk
    class Meditate : EntitySpells
    {
        public Meditate(Texture2D t)
        {
            trueCost = 0;
            cost = trueCost;
            tex = t;
            type = 2;
        }
        public override void useSpell(Hero user, BaseGameEntity target)
        {

        }
    }

    class Sacrifice : EntitySpells
    {
        public Sacrifice(Texture2D t)  
        {
            trueCost = 1;
            cost = trueCost;
            tex = t;
            type = 1;
        }
        public override void useSpell(ref short c, Vector2 p, ref short h)
        {
            if (c < cost)
                Console.WriteLine("oom");
            else
            {
                short dmg = h;
                h = (short)Math.Ceiling((double)h / 2);

                foreach (Monster item in Wizard.units)//Check every adjacent tile to the users position and deal damage to them.
                {
                    if (item.pos == p ||
                        (item.pos.X == p.X && item.pos.Y == p.Y + 1) ||
                        (item.pos.X == p.X && item.pos.Y == p.Y - 1) ||
                        (item.pos.X == p.X + 1 && item.pos.Y == p.Y) ||
                        (item.pos.X == p.X - 1 && item.pos.Y == p.Y) ||
                        (item.pos.X == p.X + 1 && item.pos.Y == p.Y + 1) ||
                        (item.pos.X == p.X - 1 && item.pos.Y == p.Y - 1) ||
                        (item.pos.X == p.X + 1 && item.pos.Y == p.Y - 1) ||
                        (item.pos.X == p.X - 1 && item.pos.Y == p.Y + 1))
                    {
                        item.hp -= dmg;
                    }
                }
                c -= cost;
            }
        }
    }

    class Heal : EntitySpells
    {
        public Heal(Texture2D t)
        {
            tex = t;
            type = 0;
            trueCost = 2;
            cost = trueCost;
        }
        public override void useSpell(ref short c)
        {
            Player p = StupidClass.players[StupidClass.CurrentTurn];
            short dmg = (short)(2 + (p.hero.intelligence * 1.5));

            if (c < cost)
                Console.WriteLine("oom");
            else
            {
                for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                {
                    for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                    {
                        if (TileMap.mCells[i, s] == p.tTile1)
                        {
                            foreach (Player item in StupidClass.players)
                            {
                                if (item.xPos == i && item.yPos == s)
                                {
                                    item.hero.hp += dmg;
                                    item.hero.UpdateStats();
                                    c -= cost;
                                    return;
                                }
                            }
                            foreach (Monster item in Wizard.units)
                            {
                                if (item.pos.X == i && item.pos.Y == s)
                                {
                                    item.hp -= dmg;
                                    item.UpdateStats();
                                    c -= cost;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}
