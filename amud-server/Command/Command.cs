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

    partial class Commands
    {
        public Dictionary<string, Command> commandDict = new Dictionary<string, Command>();

        public Commands()
        {
            commandDict.Add("north", new Command("nort", "move north", doNorth, false));
            commandDict.Add("east", new Command("south", "move east", doEast, false));
            commandDict.Add("south", new Command("east", "move south", doSouth, false));
            commandDict.Add("west", new Command("west", "move west", doWest, false));
            commandDict.Add("look", new Command("look", "look <thing?>", doLook, false));
            commandDict.Add("say", new Command("say", "say <thing>", doSay, false));
            commandDict.Add("quit", new Command("quit", "quit the game", doQuit, false));
            commandDict.Add("dig", new Command("dig", "dig <direction>", doDig, false));
        }
    }
}
