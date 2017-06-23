using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace WizardsTower
{
    static class Wizard
    {
        public static List<Monster> units = new List<Monster>();
        public static List<WizardSpells> spells = new List<WizardSpells>();

        #region Write/read
        /*try{

        using (StreamReader sr = new StreamReader(highscorelist[levelCount]))
                        {
                            while ((scoreLine = sr.ReadLine()) != null)//reads one line at a time
                            {
                                scoreParts = scoreLine.Split(',');//splits it into parts at every ,
                                foreach (string item in scoreParts)
                                {
                                    scoreAll.Add(item);
                                }
                            }
                        }
}
catch
{}

        using (StreamWriter writer = new StreamWriter(highscorelist[levelCount]))
                    {
                        foreach (int item in lI)
                        {
                            writer.WriteLine(item);
                        }
                    }   
        */
        #endregion

        public static short ap = 10, maxAp = 100, apPerTurn = 10;

        public static void initialize(ContentManager c)
        {
            spells.Add(new Darkness());
            spells.Add(new Fire());
            spells.Add(new Glue());
            spells.Add(new Teleport());
            spells.Add(new IceAge());
            spells.Add(new SummonOgre());
            spells.Add(new SummonArcher());
            spells.Add(new SummonImp());
        }

        static public void endTurn()
        {
            if (ap + apPerTurn < maxAp)
                ap += apPerTurn;
            else
                ap = maxAp;
        }

        static public void update()
        {
            foreach (BaseGameEntity item in units)
            {
                
            }
        }
    
    }
}
