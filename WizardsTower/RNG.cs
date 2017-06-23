using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace WizardsTower
{
    static class RNG
    {
        public static byte RollDice(byte numberSides)   //"Random" generator
        {
            if (numberSides <= 0)
                throw new ArgumentOutOfRangeException("numberSides");
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[1];
            do
            {
                rngCsp.GetBytes(randomNumber);
            }
            while (!IsFairRoll(randomNumber[0], numberSides));
            return (byte)((randomNumber[0] % numberSides) + 1);
        }
        private static bool IsFairRoll(byte roll, byte numSides)
        {
            int fullSetsOfValues = Byte.MaxValue / numSides;
            return roll < numSides * fullSetsOfValues;
        }
        //RollDice((byte)X);
    }
}
