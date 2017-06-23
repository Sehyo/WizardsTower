using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WizardsTower
{
    class WizardSpells
    {
        protected short cost = 1;
        public Texture2D tex;
        public short type = 0;//0, 1=contentmanager
        public virtual void castSpell(ref short c) { }
        public virtual void castSpell(ref short c, Vector2 v) { }
        public virtual void castSpell(ref short c, ContentManager Content) { }
        public virtual void castSpell(ref short c, Vector2 v1, Vector2 v2) { }
    }
    #region Spells
    class Darkness : WizardSpells
    {
        //Mwahahaahahhaaaa
        public Darkness()
        {
            cost = 15;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                c -= cost;

                //Make every tile non-visible, including the ones people are standing on.
                StupidClass.playerVTiles.Clear();

                Console.WriteLine("Success, darkness has been cast");
            }
        }
    }
    class Fire : WizardSpells
    {
        public Fire()
        {
            cost = 2;
            type = 1;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Vector2 v;
                foreach (Player item in StupidClass.players)
                {
                    if (item.isWizard)
                    {
                        for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        {
                            for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                            {
                                if (TileMap.mCells[i, s] == item.tTile1)
                                {
                                    v = new Vector2(i, s);

                                    TileMap.mCells[(int)v.X, (int)v.Y].CodeValue = "fire";
                                    TileMap.mCells[(int)v.X, (int)v.Y].CVCount = 5;
                                    c -= cost;
                                    Console.WriteLine("Success, fire has been cast");
                                    #region crapmation
                                    // bool tileAlreadyHadEffect = false;
                              //      foreach (spellEffect SpellEffect in LvlManager.goodSpellList)
                                 //   {
                                       // if (SpellEffect.X == (int)v.X && SpellEffect.Y == (int)v.Y)
                                      //  {
                                         //   tileAlreadyHadEffect = true;
                                          //  SpellEffect.changeTypeOfEffect("fire", Content);
                                        //}
                                //    }
                                   // if (!tileAlreadyHadEffect)
                                    //    LvlManager.goodSpellList.Add(new spellEffect(Content, (int)v.X, (int)v.Y, "fire"));
                                    // tileAlreadyHadEffect = false;
                                    #endregion
                                    return;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Error: Please select a tile with Space and leftbutton");
            }
        }
    }
    class Glue : WizardSpells
    {
        public Glue()
        {
            cost = 3;
            type = 1;
        }
        public override void castSpell(ref short c)
        {//FIX
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Vector2 v;
                foreach (Player item in StupidClass.players)
                {
                    if (item.isWizard)
                    {
                        for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        {
                            for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                            {
                                if (TileMap.mCells[i, s] == item.tTile1)
                                {
                                    v = new Vector2(i, s);

                                    TileMap.mCells[(int)v.X, (int)v.Y].CodeValue = "glue";
                                    TileMap.mCells[(int)v.X, (int)v.Y].CVCount = 3;

                                    c -= cost;
                                    Console.WriteLine("Success, glue has been cast");
                                    #region crapmation
                                    #endregion
                                    return;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Error: Please select a tile with Space and leftbutton");
            }
        }
    }
    class Teleport : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public Teleport()
        {
            cost = 40;
            type = 0;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                foreach (Player item in StupidClass.players)
                {
                    if (item.isWizard)  //Leta upp trollkarlen
                    {
                        for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        {
                            for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                            {
                                if (TileMap.mCells[i, s] == item.tTile1) //Check every tile and compare them to the tile the wizard has selected
                                {
                                    foreach (Player target in StupidClass.players)
                                    {
                                        if (!target.isWizard)
                                        {
                                            if (target.xPos == i && target.yPos == s)   //Kolla om en spelare står på ruta 1
                                            {
                                                for (int x = 0; x < TileMap.mCells.GetLength(0); x++)
                                                {
                                                    for (int y = 0; y < TileMap.mCells.GetLength(1); y++)
                                                    {
                                                        if (item.tTile2 != null)
                                                        {
                                                            if (TileMap.mCells[x, y] == item.tTile2) //
                                                            {
                                                                if (TileMap.mCells[x, y].Passable)
                                                                {
                                                                    target.xPos = x;
                                                                    target.yPos = y;
                                                                    c -= cost;
                                                                    Console.WriteLine("Success, you will arrive at target destination shortly.");
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("We won't teleport you into a wall for your own safety");
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Tile2 isn't selected");
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Error: No unit on target, or no target");
            }
        }
    }
    class IceAge : WizardSpells
    {
        public IceAge()
        {
            cost = 30;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                foreach (MapSquare item in TileMap.mCells)
                {
                    if (item.Passable)
                    {
                        item.CodeValue = "iceage";
                        item.CVCount = 2;
                    }
                }
                c -= cost;
                Console.WriteLine("Success, iceage has been cast");
            }
        }
    }

    #endregion
    //Traps

    //Summons

    //current WIP

    //WIP SPELLS

    class MagicCage : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c, Vector2 v)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                TileMap.mCells[(int)v.X, (int)v.Y].CodeValue = "mcage";
                c -= cost;
            }
        }
    }
    class LavaBoulder : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c, Vector2 p1, Vector2 p2)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                //spawn att p1, direction p2
                Console.WriteLine("Success");
            }
        }
    }

  
    class Swap : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c, Vector2 p1, Vector2 p2)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                //swap p1 p2 units
                Console.WriteLine("Success");
            }
        }
    }
    class TimeWarp : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c, Vector2 p)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                //activate timeswap in unit
                Console.WriteLine("Success");
            }
        }
    }
    class ChainLightning : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SuperTimeSwap : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class TimeBomb : WizardSpells
    {        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c, Vector2 v)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                TileMap.mCells[(int)v.X, (int)v.Y].CodeValue = "bomb";
                c -= cost;
            }
        }
    }

    //WIP TRAPS
    class SpikeTrap : WizardSpells
    {  
        /*public ENTERNAMEHERE()
        {
            cost = 9;
        }*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class StoneBoulder : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class Cage : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class Log : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class PoisonDartTrap : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class DartTrap : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }

    //WIP SUMMONS
    class SummonHydra : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonMinotaur : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonZombie : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonOgre : WizardSpells
    {
        public SummonOgre()
        {
            cost = 35;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                foreach (Player item in StupidClass.players)
                {
                    if (item.isWizard)
                    {
                        for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        {
                            for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                            {
                                if (TileMap.mCells[i, s] == item.tTile1)
                                {
                                    if (TileMap.mCells[i, s].Passable)
                                    {
                                        c -= cost;
                                        Wizard.units.Add(new Ogre(StupidClass.content, i, s));
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wall detected, no cast");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    class SummonArcher : WizardSpells
    {
        public SummonArcher()
        {
            cost = 4;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                foreach (Player item in StupidClass.players)
                {
                    if (item.isWizard)
                    {
                        for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        {
                            for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                            {
                                if (TileMap.mCells[i, s] == item.tTile1)
                                {
                                    if (TileMap.mCells[i, s].Passable)
                                    {
                                        c -= cost;
                                        Wizard.units.Add(new SkeletonArcher(StupidClass.content, i, s));
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wall detected, no cast");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    class SummonSpider : WizardSpells
    {        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonImp : WizardSpells
    {
        public SummonImp()
        {
            cost = 5;
        }
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
                foreach (Player item in StupidClass.players)
                {
                    if (item.isWizard)
                    {
                        for (int i = 0; i < TileMap.mCells.GetLength(0); i++)
                        {
                            for (int s = 0; s < TileMap.mCells.GetLength(1); s++)
                            {
                                if (TileMap.mCells[i, s] == item.tTile1)
                                {
                                    if (TileMap.mCells[i, s].Passable)
                                    {
                                        c -= cost;
                                        Wizard.units.Add(new Imp(StupidClass.content, i, s));
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wall detected, no cast");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class SummonMimic : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonTreasureGoblin : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonKnight : WizardSpells
    {
                /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonOrc : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonArmor : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonTreant : WizardSpells
    {        /*public ENTERNAMEHERE()
{
    cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonGolem : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonTroll : WizardSpells
    {
        /*public ENTERNAMEHERE()
{
cost = 9;
}*/
        public override void castSpell(ref short c)
        {
            if (c < cost)
            {
                Console.WriteLine("Not enough mana");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
    class SummonMegaSnail : WizardSpells
    {
        //?????
    }
}