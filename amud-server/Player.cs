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
        public string name { get; private set; }
        public Room room { get; set; }

        public Player (Client client, string name)
        {
            this.client = client;
            this.name = name;
            this.parser = new CommandParser(this);
            this.stats = new CharacterStats(20, 20);
            
            World.rooms.First().addPlayer(this);
        }

        public void prompt()
        {
            StringBuilder prompt = new StringBuilder();
            prompt.AppendFormat("(:{0}/{1}hp<->{2}/{3}mp:)# ", stats.health, stats.maxHealth, stats.mana, stats.maxMana);

            client.sendNoNewline(prompt.ToString());
        }
    }
}
