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
            if (args.Length > 1)
            {
                lookAt(player, args[1]);
            }
            else
            {
                lookAll(player);
            }
        }

        private void lookAt(Player player, string what)
        {
            StringBuilder buffer = new StringBuilder();

            Player p = player.room.getPlayerByName(what);
            if (p != null)
            {
                buffer.AppendFormat("You look at {0}.\r\n\n{1}", p.name, p.items.equippedToString());
            }

            NPC n = player.room.getNPCByName(what);
            if (n != null)
            {
                buffer.AppendFormat("You look at a {0}\r\n{1}.\r\n\n{2}", n.name, n.description, n.items.equippedToString());
            }

            if (buffer.ToString().Length < 1)
                player.client.send("You do not see that here!");
            else
                player.client.send(buffer.ToString());
        }

        private void lookAll(Player player)
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendFormat("%W{0}\r\n{1}\r\n%w{2}\r\n%R{3}\r\n%B{4}\r\n%M{5}",
                                    player.room.name,
                                    player.room.exitsToString(),
                                    player.room.description,
                                    player.room.NPCsToString(),
                                    player.room.itemsToString(),
                                    player.room.playersToString(player));

            player.client.send(buffer.ToString().TrimEnd('\r', '\n'));
        }

        private void doQuit(string[] args, Player player)
        {
            player.client.playing = false;
            player.client.sendToRest(player.name + " has left the game!");
            player.client.send("thank you, come again!");
            player.client.disconnect();
        }
    }
}
