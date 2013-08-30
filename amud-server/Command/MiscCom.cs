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

            Character c = player.room.getCharacterByName(what);
            if (c != null)
            {
                buffer.AppendFormat("You look at {0}.\r\n{1}\r\n\n{2}", 
                                    c.name, 
                                    c.description, 
                                    c.items.equippedToString());
            }

            Item item = player.room.getItemByName(what);
            if (item != null)
            {
                buffer.AppendFormat("\r\nYou look at {0}.\r\n{1}\r\n", item.name, item.description);
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
            player.client.sendToRest(player.name + " has left the game!");
            player.client.send("thank you, come again!");
            player.client.disconnect();
        }

        private void doTime(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("\r\n%c{0}\r\n%C{1}\r\n", 
                                player.world.worldTime.ToShortTimeString(), 
                                player.world.worldTime.ToLongDateString());
            player.client.send(buffer.ToString());
        }

        private void doHelp(string[] args, Player player)
        {
            Commands commands = new Commands();
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("Available Commands:\r\n");
            foreach (Command c in commands.all)
            {
                buffer.AppendFormat("    [ {0,12} ] {1}\r\n", c.name, c.description);
            }
            player.client.send(buffer.ToString());
        }

        private void doShop(string[] args, Player player)
        {
            player.room.findShop(args, player);
        }
    }
}
