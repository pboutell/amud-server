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
        public string description;
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
        public List<Command> all { get; private set; }

        public Commands()
        {
            all = new List<Command>();
            //Commands fall through order matters.
            all.Add(new Command("north", "move north", doWalk, false));
            all.Add(new Command("east", "move east", doWalk, false));
            all.Add(new Command("south", "move south", doWalk, false));
            all.Add(new Command("west", "move west", doWalk, false));
            all.Add(new Command("help", "list commands", doHelp, false));
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
            all.Add(new Command("time", "world time", doTime, false));
            all.Add(new Command("drop", "drop item from inventory", doDrop, false));
            all.Add(new Command("kill", "kill something", doKill, false));
            all.Add(new Command("quit", "quit the game", doQuit, false));
            all.Add(new Command("shop", "shop", doShop, false));
            all.Add(new Command("create", "create <mob|item> : not implemented", doCreate, true));
            all.Add(new Command("clone", "clone <mob|item> : not implemented", doClone, true));
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
