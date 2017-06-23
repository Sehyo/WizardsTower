using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardsTower
{
    public class AttackParameters
    {
        public byte dmg;
        public Equipment weapon;
        public bool dodged;
        //dodged = true, miss
        public AttackParameters(byte d, Equipment wpn)
        {
            dmg = d;
            weapon = wpn;
        }
    }
}
