using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardsTower
{
    public class EntityPassives
    {
        public short duration;//set duration to -1 for infinite;

        public void updatePassive(BaseGameEntity user)
        {
            if (duration > 0)
                duration--;
            else if (duration == 0)
                user.passives.Remove(this);
        }
        public virtual void addStats(Hero user)
        {
        }
        public virtual void addStats(BaseGameEntity user)
        {
        }

        public virtual void specialBonuses()
        {
        }
    }
    #region FoF
    public class FistOfFury : EntityPassives
    {
        public FistOfFury()
        {
            duration = 1;
        }
        public override void addStats(Hero user)
        {
            base.addStats(user);
            if (user.equipment[0].name == "Fist" && user.equipment[1].name == "Fist")
            {
                user.meleeAttack += (short)(user.strength + user.agility);
            }
            else if(user.equipment[0].name == "Fist" || user.equipment[1].name == "Fist")
                user.meleeAttack += (short)(user.strength * .5 + user.agility * .5);
            else
                user.meleeAttack += (short)(user.strength * .25 + user.agility * .25);

        }
    }
#endregion
}
