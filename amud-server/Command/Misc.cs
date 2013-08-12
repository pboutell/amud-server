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
            StringBuilder buffer = new StringBuilder();

            if (args.Length > 1)
            {
                foreach (Player p in player.room.players)
                {
                    if (p.name.Equals(args[1].TrimEnd('\r', '\n')))
                    {
                        buffer.AppendLine("\r\nYou look at " + p.name);
                        break;
                    }
                }

                foreach (NPC n in player.room.npcs)
                {
                    if (n.name.Equals(args[1].TrimEnd('\r', '\n')))
                    {
                        buffer.AppendFormat("\r\nYou look at a {0}\r\n{1}\r\n", n.name, n.description);
                        break;
                    }
                }

                if (buffer.ToString().Length < 1)
                {
                    player.client.send("\r\nYou do not see that here!\r\n");
                }
                else
                {
                    player.client.send(buffer.ToString());
                }
            }
            else
            {
                buffer.AppendLine();
                buffer.AppendLine(player.room.name);
                buffer.AppendLine(player.room.description);
                buffer.AppendLine();

                buffer.Append(player.room.listOtherPlayers(player));
                buffer.Append(player.room.listNPCs());

                buffer.AppendLine();
                buffer.AppendLine(player.room.exitsToString());

                player.client.send(buffer.ToString());
            }
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
