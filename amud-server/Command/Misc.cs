using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doLook(string[] args, Player player)
        {
            player.sendToPlayer(player.room.name + "\r\n");
            player.sendToPlayer(player.room.description + "\r\n\n");
            player.sendToPlayer(getExits(player));

            foreach (Player p in player.players)
            {
                if (p != player)
                {
                    player.sendToPlayer(p.Name + " is standing here.\r\n");
                }
            }
        }

        private string getExits(Player player)
        {
            StringBuilder exits = new StringBuilder();

            exits.Append("[ ");
            int appended = 0;
            for (int x = 0; x < 4; x++)
            {
                if (player.room.exits.ElementAtOrDefault(x) != null)
                {
                    exits.Append(Direction.directionName(x));
                    appended++;
                    exits.Append(" ");
                }
            }

            if (appended < 1)
                exits.Append("none ");

            exits.Append("]\r\n\n");

            return exits.ToString();
        }

        private void doQuit(string[] args, Player player)
        {
            player.sendToRest(player.Name + " has left the game!\r\n");
            player.sendToPlayer("thank you, come again!\r\n");
            player.disconnect();
        }

    }
}
