using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doDig(string[] args, Player player)
        {
            if (args.Length == 1)
            {
                player.parser.parse("dig --help");
                return;
            }

            int direction = Direction.directionToNumber(args[1]);

            if (direction >= 0 && direction < 4)
            {
                if (!player.world.newExit(direction, player.room))
                {
                    player.client.send("Can't dig that way!");
                }
                else
                {
                    player.client.send("Dug " + args[1]);
                }
            }
            else
            {
                player.client.send("I don't know how to dig that direction!");
            }
        }

      
        //TODO: Clean this up
        private void doCreate(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();

            if (args.Length == 1)
            {
                player.parser.parse("create --help");
                return;
            }

            if (args[1] == "mob")
            {
                player.client.send("I don't know how to create mob yet.");
            }
            else if (args[1] == "item")
            {
                if (args.ElementAtOrDefault(2) != null)
                {
                    if (args[2].Length > 10)
                    {
                        player.client.send("please select a shorter name.");
                        return;
                    }

                    Item newItem = new Item(args[2].TrimEnd('\r', '\n'), "", 5, "none", 1);
                    player.items.addToInventory(newItem);
                    buffer.AppendFormat("{0} creates a {1} out of thin air!", player.name, newItem.name);
                    player.room.sendToRoom(buffer.ToString());
                }
            }
            else
            {
            }
        }

        private void doClone(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();

            if (args.Length == 1)
            {
                player.parser.parse("clone --help");
                return;
            }

            if (args[1] == "mob")
            {
                if (args.ElementAtOrDefault(2) != null)
                {
                    NPC clone = player.world.findNPC(args[2].TrimEnd('\r', '\n'));
                    if (clone != null)
                    {
                        buffer.AppendFormat("{0} creates {1} out of thin air!", player.name, clone.description);
                        player.room.addNPC(clone);
                        player.world.mobs.Add(clone);
                        player.room.sendToRoom(buffer.ToString());
                    }
                }
                else
                {
                    player.client.send("Which mob would you like to clone?");
                }
            }
            else if (args[1] == "item")
            {
                player.client.send("I don't know how to clone items yet.");
            }
            else
            {
                player.client.send("I don't know how to clone that");
                player.parser.parse("clone --help");
            }
        }
    }
}
