using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Combat
    {
        public bool isFighting { get; set; }
        public Character target { get; set; }
        public Character character { get; private set; }

        public Combat(Character character)
        {
            this.character = character;
            this.target = null;
        }

        public int attack(Character target)
        {
            int damageDone = 0;

            target.takeDamage(character, damageDone = World.randomNumber.Next(2, 20));

            return damageDone;
        }
    }
}
