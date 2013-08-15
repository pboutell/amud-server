using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Player : Character
    {
        public CommandParser parser { get; private set; }
        public Client client { get; private set; }

        public Player (Client client, string name)
        {
            this.client = client;
            this.name = name;
            this.parser = new CommandParser(this);
            this.stats = new CharacterStats(1000, 1000);

            items.addToInventory(new Item("sword", "a short sword", 5, "right hand"));
            World.rooms.First().addPlayer(this);
        }

        public void prompt()
        {
            StringBuilder prompt = new StringBuilder();
            prompt.AppendFormat("%W(%w: %w{0}%y/%w{1}%yhp%W <%M-%W> %w{2}%y/%w{3}%ymp%w :%W)%M# ",
                                stats.health, stats.maxHealth, stats.mana, stats.maxMana);

            client.sendNoNewline(prompt.ToString());
        }

        public void update()
        {
            StringBuilder buffer = new StringBuilder();

            if (combat.target != null && combat.isFighting == true && combat.target.room == room)
            {
                int damageDone = 0;
                damageDone = combat.attack(combat.target);
                buffer.AppendFormat("\r\nYou attack {0} dealing %B{1}%x damage!\r\n", combat.target.description, damageDone);

                while (messagePipe.Count > 0)
                {
                    buffer.AppendLine(messagePipe.Dequeue());
                }

                client.send(buffer.ToString());
            }
            else
            {
                messagePipe.Clear();
            }

            if (stats.health <= 0)
            {
                die();
            }
        }

        public void die()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("\r\nYou have died!");
            buffer.AppendLine("Would you like your to have your possessions identified?");

            client.send(buffer.ToString());

            combat.target = null;
            combat.isFighting = false;

            buffer.Clear();
            buffer.AppendFormat("%rHere lies the corpse of %W{0}", name);
            room.addItem(new Item("corpse", buffer.ToString(), 20, "none"));
        }
    }
}
