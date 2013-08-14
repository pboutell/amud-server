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
                buffer.AppendFormat("You look at {0}\r\n", p.name);

            NPC n = player.room.getNPCByName(what);
            if (n != null)
                buffer.AppendFormat("You look at a {0}\r\n{1}\r\n", n.name, n.description);

            if (buffer.ToString().Length < 1)
                player.client.send("You do not see that here!\r\n");
            else
                player.client.send(buffer.ToString());
        }

        private void lookAll(Player player)
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendFormat("%W{0}\r\n{1}\r\n%w{2}%M{3}\r\n%R{4}%y\r\n{5}",
                                    player.room.name,
                                    player.room.exitsToString(),
                                    player.room.description,
                                    player.room.listOtherPlayers(player),
                                    player.room.listNPCs(),
                                    player.room.listItems());

            player.client.send(buffer.ToString());
        }

        private void doQuit(string[] args, Player player)
        {
            player.client.playing = false;
            player.client.sendToRest(player.name + " has left the game!\r\n");
            player.client.send("thank you, come again!");
            player.client.disconnect();
        }
    }
}
