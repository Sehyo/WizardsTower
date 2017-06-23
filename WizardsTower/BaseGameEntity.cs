using System;
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
    #region GameEntity
    public abstract class BaseGameEntity : GameObject
    {
        public bool isAttack;
        public int dir, range, damage;
        public EntityPassives targetDebuff = new EntityPassives();
        public Vector2 pos = new Vector2(3, 3);
        public short maxHp = 10, hp = 10, baseHp = 10,
             maxAp, ap, baseAp = 3,

             moveCost = 1,
             attackCost = 1,

             level = 1,

             meleeAttack,
             rangedAttack,
             magicAttack,
             magicReduction,
             critChance,
             baseCritChance = 10,
             dodgeChance,
             baseDodgeChance = 5,
             armor;

        public Rectangle hpRed = new Rectangle(0, 0, 50, 5);
        public Rectangle hpGreen = new Rectangle(0, 0, 50, 5);

        public bool heroFaction, isUndead, isDead;
        public BaseGameEntity lastHitBy;
        public List<EntityPassives> passives = new List<EntityPassives>();
        public Equipment[] equipment = new Equipment[8];
        public Texture2D texture;
        //0weapon, 1 weapon, 2armor, 3legs, 4helm, 5amulet, 6ring


        //För att uppdatera equipment/buffs/maxHP etc
        //Buff1{5,3,1} ökar maxhp med 5, maxmp 3, maxap1
        //virutal void doShit(){}
        public void Init()
        {
            hp = maxHp;
            ap = maxAp;
        }
        public virtual void UpdateStats()
        {
            if (hp > 0)
            {
                //Gör till float pga det konverterar till 1 annars
                float mhp = maxHp;
                float chp = hp;

                //Fixa brädden på hpbar
                if ((50 / (mhp / chp)) < 50)
                    hpGreen.Width = (int)(50 / (mhp / chp));
                else
                    hpGreen.Width = 50;
            }
            else
                hpGreen.Width = 0;


            if (hp <= 0)
                isDead = true;

            if (equipment[0] == null)
                equipment[0] = new Fists();
            if (equipment[1] == null)
                equipment[1] = new Fists();

            critChance = baseCritChance;
            dodgeChance = baseDodgeChance;
        }
        public virtual void UpdateStats(Player p)
        {
            if (hp > 0)
            {
                //Gör till float pga det konverterar till 1 annars
                float mhp = maxHp;
                float chp = hp;
                //Fixa brädden på hpbar
                if ((50 / (mhp / chp)) < 50)
                    hpGreen.Width = (int)(50 / (mhp / chp));
                else
                    hpGreen.Width = 50;
            }
            else
                hpGreen.Width = 0;

            if (hp <= 0)
                isDead = true;

            if (equipment[0] == null)
                equipment[0] = new Fists();
            if (equipment[1] == null)
                equipment[1] = new Fists();

            critChance = baseCritChance;
            dodgeChance = baseDodgeChance;
        }


        #region Move
        /*
        private void moveAndDirection(GameTime gameTime, ref MouseState mouseState, ref KeyboardState keyboardState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                moveY = 0;
                moveX = 0;
                if (StupidClass.inputTimer <= 0.0f)
                {
                    if (directionMode)
                    {
                        if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64))))
                            hero.dir = 0;
                        else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64))))
                            hero.dir = 3;
                        else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                            hero.dir = 1;
                        else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64))))
                            hero.dir = 2;
                    }
                    else if (showArrows)
                    {
                        if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64))))
                        {
                            --moveY;
                            stopStuff = true;
                            stuff(gameTime);
                            StupidClass.inputTimer = 3000;
                        }
                        else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64))))
                        {
                            --moveX;
                            stopStuff = true;
                            stuff(gameTime);
                            StupidClass.inputTimer = 3000;
                        }
                        else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                        {
                            ++moveX;
                            stopStuff = true;
                            stuff(gameTime);
                            StupidClass.inputTimer = 3000;
                        }
                        else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64))))
                        {
                            ++moveY;
                            stopStuff = true;
                            stuff(gameTime);
                            StupidClass.inputTimer = 3000;
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
            if (StupidClass.inputTimer > 0)
                StupidClass.inputTimer -= 200;
            if (stopStuff)
            {
                moveX = 0;
                moveY = 0;
                updateAnimation(gameTime);
                stopStuff = false;
            }
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
         
         */
        #endregion


        #region Attack
        public virtual void Attack(BaseGameEntity user, byte weapon) //wpn 0 = main, 1 = offhand. (melee/range)
        {
            short dmg;
            //Check if it is a range weapon, use range dmg if it is.

            if (user.equipment[weapon] != null && user.equipment[weapon].range > 1)
                dmg = user.rangedAttack;
            else
                dmg = user.meleeAttack;

            //Check if it is a crit, double damage
            byte rng = RNG.RollDice(100);
            if (rng < user.critChance)
                dmg *= 2;

            //BaseGameEntity target = getunitat pos+direction
            //for range, gå åt direction, ta närmsta unit.
            //if target != null {
            rng = RNG.RollDice(100);

            //return (new AttackParameters((byte)dmg, user.equipment[weapon]));
            AttackParameters AttackParamCalc = new AttackParameters((byte)dmg, user.equipment[weapon]);
           
            targetDebuff = AttackParamCalc.weapon.targetDebuffs;
            damage = AttackParamCalc.dmg;
            range = AttackParamCalc.weapon.range;
            // Change later - Add arrows
            #region dmgCalc
            /*//if because otherwise it would heal someone with high armor.

                //Apply debuffs on target
                if (user.equipment[weapon] != null)
                    target.passives.Add(user.equipment[weapon].targetDebuffs);

                target.lastHitBy = user;
                */
            //user?
            //return(dmg, equipment[weapon].range, equipment[weapon].targetDebuffs;

            foreach (Player snubbe in StupidClass.players)
            {
                int posY = (int)pos.Y;
                int posX = (int)pos.X;
                if (posX != snubbe.hero.pos.X || posY != snubbe.hero.pos.Y)
                {
                    switch (dir)
                    {
                        case 0:
                            while (--posY >= (pos.Y - range))
                            {
                                if (posX == snubbe.hero.pos.X && posY == snubbe.hero.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE
                                    if (rng > snubbe.hero.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.hero.armor > 0)
                                        {
                                            snubbe.hero.hp -= (short)(dmg - snubbe.hero.armor);
                                        }
                                    }//add dodged
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        case 1:
                            while (++posX <= (pos.X + range))
                            {

                                if (posX == snubbe.hero.pos.X && posY == snubbe.hero.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE
                                    if (rng > snubbe.hero.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.hero.armor > 0)
                                        {
                                            snubbe.hero.hp -= (short)(dmg - snubbe.hero.armor);
                                        }
                                    }
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        case 2:
                            while (++posY <= (pos.Y + range))
                            {

                                if (posX == snubbe.hero.pos.X && posY == snubbe.hero.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE'
                                    if (rng > snubbe.hero.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.hero.armor > 0)
                                        {
                                            snubbe.hero.hp -= (short)(dmg - snubbe.hero.armor);
                                        }
                                    }
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        case 3:
                            while (--posX >= (pos.X - range))
                            {

                                if (posX == snubbe.hero.pos.X && posY == snubbe.hero.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE                                    Console.WriteLine("HIT");
                                    if (rng > snubbe.hero.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.hero.armor > 0)
                                        {
                                            snubbe.hero.hp -= (short)(dmg - snubbe.hero.armor);
                                        }
                                    }
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Non existing direction received.");
                            break;
                    }
                }
            }
        }
            #endregion
    }
#endregion
#endregion

    #region Heroes
    public abstract class Hero : BaseGameEntity         //todo lägg dit olika gubbar med abilities/bas stats etc
    {
        public short experience = 0,
             maxExp = 10,
             strength, trueStr,
             agility, trueAgi,
             intelligence, trueInt,
             skillPoints;
        public List<EntitySpells> spells = new List<EntitySpells>();
        public Items[] inventory = new Items[16];

        public Hero()
        {
            heroFaction = true;
        }

        public override void Attack(BaseGameEntity user, byte weapon)
        {
                        short dmg;
            //Check if it is a range weapon, use range dmg if it is.

            if (user.equipment[weapon] != null && user.equipment[weapon].range > 1)
                dmg = user.rangedAttack;
            else
                dmg = user.meleeAttack;

            //Check if it is a crit, double damage
            byte rng = RNG.RollDice(100);
            if (rng < user.critChance)
                dmg *= 2;

            //BaseGameEntity target = getunitat pos+direction
            //for range, gå åt direction, ta närmsta unit.
            //if target != null {
            rng = RNG.RollDice(100);

            //return (new AttackParameters((byte)dmg, user.equipment[weapon]));
            AttackParameters AttackParamCalc = new AttackParameters((byte)dmg, user.equipment[weapon]);
           
            targetDebuff = AttackParamCalc.weapon.targetDebuffs;
            damage = AttackParamCalc.dmg;
            range = AttackParamCalc.weapon.range;
            // Change later - Add arrows
            #region dmgCalc
            /*//if because otherwise it would heal someone with high armor.

                //Apply debuffs on target
                if (user.equipment[weapon] != null)
                    target.passives.Add(user.equipment[weapon].targetDebuffs);

                target.lastHitBy = user;
                */
            //user?
            //return(dmg, equipment[weapon].range, equipment[weapon].targetDebuffs;

            foreach (Monster snubbe in Wizard.units)
            {
                int posY = (int)pos.Y;
                int posX = (int)pos.X;
                if (posX != snubbe.pos.X || posY != snubbe.pos.Y)
                {
                    switch (dir)
                    {
                        case 0:
                            while (--posY >= (pos.Y - range))
                            {
                                if (posX == snubbe.pos.X && posY == snubbe.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE
                                    if (rng > snubbe.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.armor > 0)
                                        {
                                            snubbe.hp -= (short)(dmg - snubbe.armor);
                                        }
                                    }//add dodged
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        case 1:
                            while (++posX <= (pos.X + range))
                            {

                                if (posX == snubbe.pos.X && posY == snubbe.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE
                                    if (rng > snubbe.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.armor > 0)
                                        {
                                            snubbe.hp -= (short)(dmg - snubbe.armor);
                                        }
                                    }
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        case 2:
                            while (++posY <= (pos.Y + range))
                            {

                                if (posX == snubbe.pos.X && posY == snubbe.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE'
                                    if (rng > snubbe.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.armor > 0)
                                        {
                                            snubbe.hp -= (short)(dmg - snubbe.armor);
                                        }
                                    }
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        case 3:
                            while (--posX >= (pos.X - range))
                            {

                                if (posX == snubbe.pos.X && posY == snubbe.pos.Y)
                                {
                                    // WE FOUND A DUDE IN HERE. HUEHUEHEUHEUHEUEHUEHUEHEUHEUHEUEHUEHUHEHEHUEHEUHEHUEHUEHUEHEHUHEUHEHOOOHUHEEHUE                                    Console.WriteLine("HIT");
                                    if (rng > snubbe.dodgeChance)
                                    {
                                        if (user.meleeAttack - snubbe.armor > 0)
                                        {
                                            snubbe.hp -= (short)(dmg - snubbe.armor);
                                        }
                                    }
                                    else
                                        Console.WriteLine("Dodge");
                                    break;
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Non existing direction received.");
                            break;
                    }
                }
            }
        }
            #endregion
        

        #region TurnUpdate
        public void TurnUpdate()
        {
            //UI for moving etc etc
            // insert ui update?
            ap = maxAp;
            #region CodeValues
            if (TileMap.mCells[(int)pos.X, (int)pos.Y].CodeValue == "fire")
                hp -= 4;
            else if (TileMap.mCells[(int)pos.X, (int)pos.Y].CodeValue == "glue")
                ap = (short)Math.Floor(ap * 0.5);


            #endregion

            foreach (Equipment item in equipment)
            {
                if(item!= null)
                    item.calculateSet(this);
            }
            foreach (EntityPassives item in passives)
            {
                    item.updatePassive(this);
            }
        }
        #endregion
        public void CharacterScreenUpdate()
        {
            //if mouse.x&&mouse.y is within area of item && mouse.button1 is down
            //picked up item = item-in-area
            //swap pos
        }



        #region Levelup
        public void LevelUp()
        {
            //temp levelup
            trueStr += 1;
            trueAgi += 1;
            trueInt += 1;
            level++;

            UpdateStats();
            hp = maxHp;
            hp += 5;
            ap = maxAp;
            experience = 0;

            skillPoints += 1;
            //dostuff
        }
        #endregion

        #region UpdateStats
        public override void UpdateStats(Player p)
        {
            base.UpdateStats(p);

            pos.X = p.xPos;
            pos.Y = p.yPos;

            //Reset base stats
            strength = trueStr;
            agility = trueAgi;
            intelligence = trueInt;
            armor = 0;

            if (experience >= maxExp)
                LevelUp();

            //Add bonuses from items ex: +2 str, +1armor
            foreach (Equipment item in equipment)
            {
                if(item != null)
                    item.bonuses(this);
            }
            //Calculate stats

            dodgeChance += (short)(agility * 2);
            critChance += (short)(agility * 2);
            maxHp = (short)(baseHp + strength * 5);
            maxAp = (short)(baseAp + (Math.Floor((double)agility / 3)));

            //Calculate Attack dmg
            meleeAttack = (short)(strength + equipment[0].meleeAttack + (equipment[1].meleeAttack * .5));
            magicAttack = (short)(intelligence + equipment[0].magicAttack + equipment[1].magicAttack);
            rangedAttack = (short)(agility + equipment[0].rangedAttack + equipment[1].rangedAttack);

            //foreach magic, magiccost -= magicreduction if(magiccost < 1) magiccost = 1;

            foreach (Equipment item in equipment)   //Add multipliers now because 0*204234231 = 0.
            {
                if (item != null)
                    item.multiBonuses(this);
            }
        #endregion
        }
    }
    #endregion

            #region Monk
    public class Monk : Hero
    {
        public Monk()
        {
            trueStr = 2;
            trueAgi = 2;
            trueInt = 1;
            //Add monk spells passives
        }
    }
    #endregion

            #region Bandit
    public class Bandit : Hero
    {

        public Bandit()
        {
            trueStr = 1;
            trueAgi = 3;
            trueInt = 1;
            //Add bandit spells passives
        }

    }

    #endregion

            #region Swordsman
    public class Swordsman : Hero
    {

        public Swordsman()
        {
            trueStr = 3;
            trueAgi = 1;
            trueInt = 1;
            //Add monk spells passives
        }

    }
    #endregion

            #region Wizard's Apprentice
    public class MiniWizard : Hero
    {

        public MiniWizard()
        {
            trueStr = 1;
            trueAgi = 1;
            trueInt = 3;
            //Add MiniWizard spells passives
        }

    }


    #endregion


        #region Monsters
    public class Monster : BaseGameEntity
    {
        public void turnUpdate()
        {
            ap = baseAp;
        }
        public override void UpdateStats()
        {
            base.UpdateStats();
            if (hp > 0)
            {
                //Gör till float pga det konverterar till 1 annars
                float mhp = maxHp;
                float chp = hp;
                //Fixa brädden på hpbar
                if ((50 / (mhp / chp)) < 50)
                    hpGreen.Width = (int)(50 / (mhp / chp));
                else
                    hpGreen.Width = 50;
            }
            else
                hpGreen.Width = 0;
        }

        public void stuff(GameTime gt)
        {
            base.enabled = true;
            base.Update(gt);
            bool move = true;
            foreach (Player item in StupidClass.players)
            {
                if (item.hero.pos.X == pos.X + moveAmount.X && item.hero.pos.Y == pos.Y + moveAmount.Y)
                {
                    move = false;
                    break;
                }
            }
            foreach (Monster item in Wizard.units)
            {
                if (item.pos.X == pos.X + moveAmount.X && item.pos.Y == pos.Y + moveAmount.Y)
                {
                    move = false;
                    break;
                }
            }
            if (move)
            {
                pos.X += (int)moveAmount.X;
                pos.Y += (int)moveAmount.Y;
                ap--;
            }
        }
        public void drawArrows(SpriteBatch spriteBatch)
        {
            if (!hideEverything)
            {
                if (showArrows || directionMode)
                {
                    spriteBatch.Draw(WizardUI.arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                    spriteBatch.Draw(WizardUI.arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, (float)(3.14 + 3.14 / 2), new Vector2(64, 0), SpriteEffects.None, drawDepth);
                    spriteBatch.Draw(WizardUI.arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, (float)(3.14 / 2), new Vector2(0, 64), SpriteEffects.None, drawDepth);
                    spriteBatch.Draw(WizardUI.arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, drawDepth);
                }
                else
                {
                    spriteBatch.Draw(WizardUI.arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                    spriteBatch.Draw(WizardUI.arrows, Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64)), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, drawDepth);
                }
            }
        }
         #region Move
    
        bool directionMode = false;
        bool showArrows = false;
        bool hideEverything = false;
        bool stopStuff = false;
        public void Move(GameTime gameTime, ref MouseState mouseState) //0^,1>,2v,3<
        {
            worldLocation.X = pos.X * 64;
            worldLocation.Y = pos.Y * 64;

            //Console.WriteLine(" " + worldLocation.X);

             mouseState = Mouse.GetState();
             Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

             //InsertMoveFunction
             if (ap > 0)
             {
                 hideEverything = false;
                 if (mouseState.LeftButton == ButtonState.Pressed)
                 {
                     moveY = 0;
                     moveX = 0;

                     if (StupidClass.inputTimer <= 0.0f)
                     {
                         if (directionMode)
                         {
                             if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y - 64, 64, 64))))
                             {
                                 dir = 0;
                                 Attack(this, 0);
                                 directionMode = false;
                                 StupidClass.inputTimer = 4000;
                                 ap--;
                             }
                             else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64))))
                             {
                                 dir = 3;
                                 Attack(this, 0);
                                 directionMode = false;
                                 ap--;
                             }
                             else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                             {
                                 dir = 1;
                                 Attack(this, 0);
                                 directionMode = false;
                                 ap--;
                                 StupidClass.inputTimer = 4000;
                             }
                             else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64))))
                             {
                                 dir = 2;
                                 Attack(this, 0);
                                 directionMode = false;
                                 ap--;
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
                                 StupidClass.inputTimer = 3000;
                                 showArrows = false;
                             }
                             else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X - 64, WorldRectangle.Y, 64, 64))))
                             {
                                 --moveX;
                                 stopStuff = true;
                                 stuff(gameTime);
                                 StupidClass.inputTimer = 3000;
                                 showArrows = false;
                             }
                             else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X + 64, WorldRectangle.Y, 64, 64))))
                             {
                                 ++moveX;
                                 stopStuff = true;
                                 stuff(gameTime);
                                 StupidClass.inputTimer = 3000;
                                 showArrows = false;
                             }
                             else if (mouseRectangle.Intersects(Camera.WorldToScreen(new Rectangle(WorldRectangle.X, WorldRectangle.Y + 64, 64, 64))))
                             {
                                 ++moveY;
                                 stopStuff = true;
                                 stuff(gameTime);
                                 StupidClass.inputTimer = 3000;
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
             else
             {
                 hideEverything = true;
             }

             if (stopStuff)
             {
                 moveX = 0;
                 moveY = 0;
                 stopStuff = false;
             }
         }
        //depending on level add exp.
       //EXP in range of kill
    }
        #endregion
    #endregion

            #region Bosses & Mini bosses (Strong mobs, with spells)
    #region Hydra
    public class Hydra : Monster
    {
        //boss monster
        //3 heads > 3ap
        public Hydra()
        {
            moveCost = 2;
            level = 10;
            baseAp = 3;
            baseHp = 100;
            meleeAttack = 10;
            rangedAttack = 6;
            equipment[0] = new MonsterWpn();
            equipment[0].range = 5;
            equipment[1] = new MonsterWpn();
            equipment[1].range = 2;
        }
    }
    #endregion
    #region Ogre
    public class Ogre : Monster
    {
        //boss monster
        public Ogre(ContentManager content, int x, int y)
        {
            level = 6;
            baseAp = 2;
            baseHp = 60;
            maxHp = baseHp;
            hp = baseHp;
            meleeAttack = 10;
            texture = content.Load<Texture2D>(@"Textures\Sprites\Monsters\Ogre");
            equipment[0] = new MonsterWpn();
            pos.X = x;
            pos.Y = y;
        }
    }
    #endregion
    #region Minotaur
    public class Minotaur : Monster
    {
        public Minotaur()
        {
            level = 7;
            baseAp = 4;
            baseHp = 35;
            meleeAttack = 5;
        }
    }
    #endregion

    #endregion

            #region Normal monsters

    #region Skeleton
    public class SkeletonArcher : Monster
    {
        public SkeletonArcher(ContentManager content, int x, int y)
        {
            level = 1;
            baseAp = 3;
            baseHp = 8;
            maxHp = baseHp;
            hp = baseHp;
            meleeAttack = 1;
            rangedAttack = 2;
            texture = content.Load<Texture2D>(@"Textures\Sprites\Monsters\Skeleton");
            equipment[0] = new MonsterWpn();
            equipment[0].range = 3;
            pos.X = x;
            pos.Y = y;
        }
    }
    #endregion
    #region Zombie
    public class Zombie : Monster
    {
        public Zombie()
        {
            level = 2;
            baseAp = 1;
            baseHp = 20;
            hp = baseHp;
            meleeAttack = 4;
        }
    }
    #endregion
    #region Goblin
    public class Goblin : Monster
    {
        public Goblin()
        {
            level = 1;
            baseAp = 1;
            baseHp = 2;
            hp = baseHp;
            meleeAttack = 1;
        }
    }
    #endregion
    #region Giant Spider
    public class Spider : Monster
    {
        public Spider()
        {
            level = 3;
            ap = 4;
            baseHp = 8;
            hp = baseHp;
            meleeAttack = 2;
        }
    }

#endregion
    #region Imp
    public class Imp : Monster
    {
        public Imp(ContentManager content, int x, int y)
        {
            level = 1;
            baseAp = 4;
            baseHp = 4;
            maxHp = baseHp;
            hp = baseHp;
            meleeAttack = 2;
            rangedAttack = 1;
            texture = content.Load<Texture2D>(@"Textures\Sprites\Monsters\Imp");
            equipment[0] = new MonsterWpn();
            pos.X = x;
            pos.Y = y;
        }
    }
#endregion

#endregion
}