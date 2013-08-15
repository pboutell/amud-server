using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Character
    {
        public CharacterStats stats;
        public Inventory items = new Inventory();
        public Combat combat;
        public Room room;
        public Queue<string> messagePipe = new Queue<string>();

        public string name { get; set; }
        public string description { get; set; }

        public Character()
        {
            this.combat = new Combat(this);
            this.room = new Room("Limbo", "You shouldn't be here.");
        }

        public void takeDamage(Character attacker, int damage)
        {
            combat.target = attacker;
            combat.isFighting = true;

            stats.health -= damage;
        }
    }
}
