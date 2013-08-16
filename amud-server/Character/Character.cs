using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Character
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

        public void characterDie()
        {
            StringBuilder buffer = new StringBuilder();

            makeCorpse();

            if (combat.target != null)
            {
                buffer.AppendFormat("\r\n{0}, has been struck down by {1}!\r\n",
                                    name, combat.target.name);
                room.sendToRoom(buffer.ToString());

                combat.target.combat.target = null;
                combat.target.combat.isFighting = false;
            }
           
            combat.target = null;
            combat.isFighting = false;
        }

        private void makeCorpse()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendFormat("%rthe decaying corpse of %W{0}", name);
            Item item = new Item("corpse", buffer.ToString(), 20, "none");
            room.addItem(item);
        }
    }
}
