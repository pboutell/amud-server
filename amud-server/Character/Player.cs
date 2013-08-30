using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Player : Character
    {
        [NonSerialized]
        public Parser parser;

        [NonSerialized]
        public Client client;

        public Player (Client client, string name)
        {
            this.client = client;
            this.name = name;
            this.parser = new Parser(this);
            this.stats = new Stats(200, 200);
            this.gold = 100;

            items.addToInventory(new Item("sword", "a short sword", 5, "right hand", 5));
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
            updateCombat();

            if (stats.health <= 0)
            {
                die();
            }
        }

        public void die()
        {
            characterDie();
            StringBuilder buffer = new StringBuilder();
         
            buffer.AppendLine("\r\nYou have died!");
            buffer.AppendLine("Do you want your possessions identified?");
            client.send(buffer.ToString());

            room.removePlayer(this);
            stats.health = 5;
            world.rooms.First().addPlayer(this);
        }

        private void updateCombat()
        {
            StringBuilder buffer = new StringBuilder();

            if (combat.target != null && combat.isFighting == true && combat.target.room == room)
            {
                int damageDone = 0;
                damageDone = combat.attack(combat.target);

                buffer.AppendFormat("{0} attacks you doing %R{1}%x damage!", 
                                    combat.target.name, 
                                    damageDone);
                combat.target.messagePipe.Enqueue(buffer.ToString());
                buffer.Clear();

                while (messagePipe.Count > 0)
                {
                    buffer.AppendFormat("\r\nYou attack {0} dealing %B{1}%x damage!\r\n", 
                                        combat.target.name, 
                                        damageDone);
                    buffer.AppendLine(messagePipe.Dequeue());
                }

                if (buffer.Length > 1)
                {
                    client.send(buffer.ToString());
                }
            }
            else
            {
                messagePipe.Clear();
            }
        }
    }
}
