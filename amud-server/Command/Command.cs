using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Command 
    {
        public string name { get; private set; }
        private string description;
        private Action<string[], Player> method;
        private bool administrative;

        public Command(string name, string description, Action<string[], Player> method, bool administrative)
        {
            this.name = name;
            this.description = description;
            this.method = method;
            this.administrative = administrative;
        }

        public void dispatch(string[] args, Player player)
        {
            this.method.Invoke(args, player);
        }

       
    }

    partial class Commands
    {
        private List<Command> all = new List<Command>();

        public Commands()
        {
            //Commands fall through order matters.
            all.Add(new Command("north", "move north", doNorth, false));
            all.Add(new Command("east", "move east", doEast, false));
            all.Add(new Command("south", "move south", doSouth, false));
            all.Add(new Command("west", "move west", doWest, false));
            all.Add(new Command("look", "look <thing?>", doLook, false));
            all.Add(new Command("say", "say <thing>", doSay, false));
            all.Add(new Command("chat", "chat <thing>", doChat, false));
            all.Add(new Command("wear", "wear <item>", doWear, false));
            all.Add(new Command("wield", "wear <item>", doWear, false));
            all.Add(new Command("inventory", "list your stuff", doInventory, false));
            all.Add(new Command("equipment", "items worn", doEquipment, false));
            all.Add(new Command("remove", "remove worn item", doRemove, false));
            all.Add(new Command("get", "pickup item", doPickup, false));
            all.Add(new Command("pickup", "pickup item", doPickup, false));
            all.Add(new Command("drop", "drop item from inventory", doDrop, false));
            all.Add(new Command("kill", "kill something", doKill, false));
            all.Add(new Command("quit", "quit the game", doQuit, false));
            all.Add(new Command("dig", "dig <direction>", doDig, true));
        }

        public Command lookupCommand(string search)
        {
            foreach (Command command in all)
            {
                if (command.name.StartsWith(search) || command.name.Equals(search))
                {
                    return command;
                }
            }
            
            return null;
        }
    }
}
