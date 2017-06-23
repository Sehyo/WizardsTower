using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Tile_Engine;


namespace WizardsTower
{
    static class WizardUI
    {
        //10+(i*70), 425
        static Texture2D manabar;
        static Texture2D mana;
        static Texture2D uiTex;
        public static Texture2D arrows;
        static short currentPage = 0;
        static Rectangle manaRec = new Rectangle(552, 440, 200, 32);
        static List<WizardUIButton> buttons = new List<WizardUIButton>();
        static ArrowButton[] abuttons = new ArrowButton[2];
        public static void init(Texture2D m, Texture2D mb, Texture2D a, Texture2D ui)
        {
            //505
            buttons.Add(new WizardUIButton(new Rectangle(10, 425, 64, 64), Wizard.spells[0],0));
            buttons.Add(new WizardUIButton(new Rectangle(80, 425, 64, 64), Wizard.spells[1],0));
            buttons.Add(new WizardUIButton(new Rectangle(150, 425, 64, 64), Wizard.spells[2],0));
            buttons.Add(new WizardUIButton(new Rectangle(220, 425, 64, 64), Wizard.spells[3],0));
            buttons.Add(new WizardUIButton(new Rectangle(290, 425, 64, 64), Wizard.spells[4],0));
            buttons.Add(new WizardUIButton(new Rectangle(10, 425, 64, 64), Wizard.spells[5],1));
            buttons.Add(new WizardUIButton(new Rectangle(80, 425, 64, 64), Wizard.spells[6], 1));
            buttons.Add(new WizardUIButton(new Rectangle(150, 425, 64, 64), Wizard.spells[7], 1));

            abuttons[0] = new ArrowButton(true);
            abuttons[1] = new ArrowButton(false);

            uiTex = ui;
            manabar = mb;
            mana = m;
            arrows = a;
        }
        public static void Draw(SpriteBatch sb)
        {//spriteBatch.Draw(Wizard.spells[i].tex, new Rectangle(10+(i*70), 425, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            sb.Draw(uiTex, new Rectangle(0, 420, 800, 180), null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0.5f);

            foreach (WizardUIButton item in buttons)
            {
                if (item.page == currentPage)
                    sb.Draw(item.active.tex, item.box, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            }
            sb.Draw(mana, manaRec, new Rectangle(0, 0, 1, 32), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            sb.Draw(manabar, new Rectangle(550, 440, 200, 32), new Rectangle(0, 0, 200, 32), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.09f);
            sb.DrawString(StupidClass.font, Wizard.ap + " / " + Wizard.maxAp, new Vector2(622, 450), Color.White);

            sb.DrawString(StupidClass.font, "Current Page: " + (currentPage+1), new Vector2(400, 500), Color.White);
            sb.Draw(arrows, new Rectangle(400, 530, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.4f);
            sb.Draw(arrows, new Rectangle(400, 425, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);
        }
        public static void Update(ref short c, MouseState ms, MapSquare t1, MapSquare t2, ContentManager conm)
        {
            if (Wizard.ap > Wizard.maxAp)
                Wizard.ap = Wizard.maxAp;
            foreach (WizardUIButton item in buttons)
            {
                if (ms.LeftButton == ButtonState.Pressed && item.page == currentPage)
                {
                    if (ms.X > item.box.X && ms.X < item.box.X + item.box.Width && ms.Y > item.box.Y && ms.Y < item.box.Y + item.box.Width)
                    {
                            StupidClass.inputTimer = 3000;
                            item.Press(ref c);
                    }
                }
            }
            foreach (ArrowButton item in abuttons)
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    if (ms.X > item.box.X && ms.X < item.box.X + item.box.Width && ms.Y > item.box.Y && ms.Y < item.box.Y + item.box.Width)
                    {
                        StupidClass.inputTimer = 3000;
                        currentPage += item.pageInc;
                    }
                }
            }

            if (currentPage < 0)
                currentPage = 4;
            else if (currentPage > 4)
                currentPage = 0;

            float cm = c;
            float mm = Wizard.maxAp;

            if ((50 / (mm / cm)) < 98)
                manaRec.Width = (int)((98 / (mm / cm))*2);
            else
                manaRec.Width = 196;
        }
    }
    class ArrowButton
    {
        public Rectangle box;
        public short pageInc = 0;
        public ArrowButton(bool b)
        {
            if (b)
            {
                box = new Rectangle(400, 530, 64, 64);
                pageInc = -1;
            }
            else
            {
                box = new Rectangle(400, 425, 64, 64);
                pageInc = 1;
            }
        }
    }
    class WizardUIButton
    {
        public Rectangle box;
        public WizardSpells active;
        public short page;
        public WizardUIButton(Rectangle r, WizardSpells s, short p)
        {
            box = r;
            active = s;
            page = p;
        }
        public void Press(ref short c)
        {
                active.castSpell(ref c);
        }
    }
}
