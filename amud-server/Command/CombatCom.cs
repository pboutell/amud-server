using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doKill(string[] args, Player player) 
        {
            if (args.Length == 1)
            {
                player.client.send("Kill whom?");
                return;
            }

            Character target = player.room.getCharacterByName(args[1]);

            if (target != null)
            {
                player.client.send("You attack " + target.name + " with all your might!");
                player.combat.target = target;
                player.combat.isFighting = true;
            }
            else
            {
                player.client.send("I do not see anybody by that name here.");
            }
        }
    }
}
