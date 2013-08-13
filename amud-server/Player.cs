using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Player : Character
    {
        public CommandParser parser;
        public Client client;
        public Room room { get; set; }

        public Player (Client client, string name)
        {
            this.client = client;
            this.name = name;
            this.parser = new CommandParser(this);
            this.stats = new CharacterStats(20, 20);

            items.addToInventory(new Item("sword", "a short sword", 5, "right hand"));

            World.rooms.First().addPlayer(this);
        }

        public void prompt()
        {
            StringBuilder prompt = new StringBuilder();
            prompt.AppendFormat("%W(%y:%w{0}%y/%w{1}%yhp%W<%M-%W>%w{2}%y/%w{3}%ymp%y:%W)%M# ", 
                                stats.health, stats.maxHealth, stats.mana, stats.maxMana);

            client.sendNoNewline(prompt.ToString());
        }
    }
}
