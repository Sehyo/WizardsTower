using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tile_Engine;

namespace WizardsTower
{
    public class HeroUI
    {
        
        //10+(i*70), 425
        short currentPage = 0;
        Rectangle manaRec = new Rectangle(552, 440, 200, 32);
        List<HeroUIButton> buttons = new List<HeroUIButton>();
        short cAp;
        public void init(HeroUIButton[] b)
        {
            foreach (HeroUIButton item in b)
            {
                buttons.Add(item);
            }
        }
        public void Draw(SpriteBatch sb)
        {
            //spriteBatch.Draw(Wizard.spells[i].tex, new Rectangle(10+(i*70), 425, 64, 64), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            sb.Draw(StupidClass.greenBox, new Rectangle(0, 420, 800, 180), new Rectangle(0,0,800,180), Color.White, 0f, new Vector2(0,0), SpriteEffects.None, 0.2f);
            foreach (HeroUIButton item in buttons)
            {
                sb.Draw(item.active.tex, item.box, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            }
            sb.DrawString(StupidClass.font, "AP: " + cAp, new Vector2(650, 500), Color.Black);
        }
        public void Update(ref short c, MouseState ms, ref Hero h)
        {
            if (StupidClass.inputTimer <= 0)
            {
                foreach (HeroUIButton item in buttons)
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        if (ms.X > item.box.X && ms.X < item.box.X + item.box.Width && ms.Y > item.box.Y && ms.Y < item.box.Y + item.box.Width)
                        {
                            StupidClass.inputTimer = 3000;
                            item.Press(ref c, ref h);
                        }
                    }
                }
            }
            cAp = c;
        }
    }

    public class HeroUIButton
    {
        public Rectangle box;
        public EntitySpells active;
        public HeroUIButton(Rectangle r, EntitySpells s)
        {
            box = r;
            active = s;
        }
        public void Press(ref short c, ref Hero h)
        {
            if (active.type == 0)
                active.useSpell(ref c);
            else if (active.type == 1)
                active.useSpell(ref c, h.pos, ref h.hp);
        }
    }
}
