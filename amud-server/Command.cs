using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Command
    {
        private string commandName;
        private string description;
        public Action<string[], Player> method;
        private bool administrative;

        public Command(string commandName, string description, Action<string[], Player> method, bool administrative)
        {
            this.commandName = commandName;
            this.description = description;
            this.method = method;
            this.administrative = administrative;
        }
    }

    class Commands
    {
        public Dictionary<string, Command> commandDict = new Dictionary<string, Command>();

        public Commands()
        {
            commandDict.Add("look", new Command("look", "Look at something or just in general", doLook, false));
            commandDict.Add("say", new Command("say", "Say something", doSay, false));
            commandDict.Add("quit", new Command("quit", "Quit the game", doQuit, false));
        }

        private void doLook(string[] args, Player player) 
        {
            player.sendToPlayer("You have a look around.\n\n\r");

            foreach (Player p in player.players)
            {
                if (p != player)
                {
                    player.sendToPlayer(p.Name + " is standing here.\n\n\r");
                }
            }
        }

        private void doSay(string[] args, Player player)
        {
            StringBuilder text = new StringBuilder();
            
            foreach (string s in args.Skip(1))
            {
                text.Append(s);
                text.Append(" ");
            }
           
            player.sendToPlayer("you say \"" + text.ToString().Trim() + ".\"\n\n\r");
            player.sendToRest(player.Name + " says \"" + text.ToString().Trim() + ".\"\n\n\r");
        }

        private void doQuit(string[] args, Player player)
        {
            player.sendToRest(player.Name + " has left the game!\n\n\r");
            player.sendToPlayer("You have left the game!\n\n\r");
            player.disconnect();
        }
    }
}
