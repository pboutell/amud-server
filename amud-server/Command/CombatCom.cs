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
            StringBuilder buffer = new StringBuilder();

            if (args.Length == 1)
            {
                player.client.send("Kill whom?");
                return;
            }

            Character target = player.room.getCharacterByName(args[1]);

            if (target == player)
            {
                buffer.AppendLine("\r\nYou kill yourself!");
                buffer.AppendLine("Do you want your possessions identified?");
            }
            else if (target != null)
            {
                buffer.AppendFormat("You attack {0} with all your might!", target.name);
                player.combat.target = target;
                player.combat.isFighting = true;
            }
            else
            {
                buffer.AppendLine("I do not see anybody by that name here.");
            }

            player.client.send(buffer.ToString());
        }
    }
}
