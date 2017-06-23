using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardsTower
{
    public abstract class Items
    {
        public string name = "";
        public string description;
        public byte type = 0; //0 = wtfhax, 1 = weapon/shield, 2 legs, 3 armor, 4 helm, 5 amulet, 6 ring.
        public Texture2D icon;
    }
    public abstract class Equipment : Items
    {
        public short meleeAttack, rangedAttack, magicAttack, armor, range = 1;
        public EntityPassives targetDebuffs;

        public string[] setParts = new string[8];

        public virtual void setBonusEndTurn(Hero hero)
        {
        }
        #region CalcSet
        public void calculateSet(Hero hero)
        {
            int completeSet = 0;
            int setCount = 0;
            foreach (string item in setParts)
            {
                if(item != null)
                    completeSet++;
            }

            for (int i = 0; i < hero.equipment.Length; i++)
            {
                if (hero.equipment[i] != null)
                {
                    if (hero.equipment[i].name == setParts[i])
                    {
                        setCount++;
                    }
                }
            }
            if (setCount == completeSet)
            {
                setBonusEndTurn(hero);
            }
        }
        #endregion
        public virtual void bonuses(Hero hero)//Råa bonusar, typ +5str
        {
            hero.armor += armor;
        }
        public virtual void multiBonuses(Hero hero)//Multipliers ex str*1.5 för att få med bonusarna.
        {
        }

        //Stats av olika slag
        //extra saker i funktioner
    }
    #region Weapons 1
    #region Fist
    public class Fists : Equipment
    {
        public Fists()
        {
            type = 1;
            name = "Fist";
            description = "Your fist. \nDamage: 2";
            meleeAttack = 1;
        }
        public override void bonuses(Hero hero)
        {
            base.bonuses(hero);

        }
        public override void multiBonuses(Hero hero)
        {
        }
    }
    #endregion

    #region MonsterWpn
    public class MonsterWpn : Equipment
    {
        public MonsterWpn()
        {
            type = 1;
            name = "Monster Weapon";
            description = "This weapon is for changing stats like range in monsters. \n You are a cheater if you have aquired this. Or something fucked up.";
            meleeAttack = 0;
        }
    }


    #endregion

    #endregion

    #region Legs 2
    public class MonkStartLegs : Equipment
    {
        public MonkStartLegs()
        {
            type = 2;
            name = "Monk Pants";
            description = "Temple? \n Armor: 2";
            armor = 2;

            //set
            setParts[3] = "Monk Pants";
            setParts[5] = "Amulet of the Wild";
            setParts[6] = "Holy Ring of Zen";

        }
        public override void setBonusEndTurn(Hero hero)
        {
            //heal everything +1
        }
    }
#endregion

    #region Armors 3

#endregion

    #region Helmets 4

#endregion
    
    #region Amulets 5
    public class MonkStartAmulet : Equipment
    {
        byte dC = 3;
        public MonkStartAmulet()
        {
            type = 5;
            name = "Amulet of the Wild";
            description = "druid of the wild? \n Increases dodge chance by " + dC + "%";
        }
        public override void bonuses(Hero hero)
        {
            hero.dodgeChance += dC;
        }
    }

#endregion

    #region Rings 6

    public class MonkStartRing : Equipment
    {
        public MonkStartRing()
        {
            type = 6;
            name = "Holy Ring of Zen";
            description = "STUFFS \n When the wielder reaches 50% HP, the ring breaks and heals him for 25% of his maximum HP";
        }
        public override void multiBonuses(Hero hero)
        {
            if (hero.hp <= hero.maxHp * .5)
            {
                hero.hp += (short)(hero.maxHp * .25);
            }
        }
    }

#endregion
}
