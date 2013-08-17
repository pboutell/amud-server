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
        public CharacterStats stats { get; set; }
        public Inventory items { get; set; }
        public Combat combat { get; set; }
        public Room room { get; set; }
        public Queue<string> messagePipe { get; set; }
        public World world { get; set; }

        public string name { get; set; }
        public string description { get; set; }

        public Character()
        {
            this.combat = new Combat(this);
            this.room = new Room("Limbo", "You shouldn't be here.");
            this.items = new Inventory();
            this.messagePipe = new Queue<string>();
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

            if (combat.target != null)
            {
                buffer.AppendFormat("\r\n{0}, has been struck down by {1}!\r\n",
                                    name, combat.target.name);
                room.sendToRoom(buffer.ToString());

                combat.target.combat.target = null;
                combat.target.combat.isFighting = false;
            }

            makeCorpse();

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
