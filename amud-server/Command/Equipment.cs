using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doWear(string[] args, Player player)
        {
            if (args.Length == 1)
            {
                player.client.send("Wear what?");
            }
            else
            {
                Item item = player.items.getItemByName(args[1].TrimEnd('\r', '\n'));

                if (item != null)
                {
                    if (player.items.addToEquipped(item.wearLocation, item))
                    {
                        player.client.send("You equip " + item.description + ".\r\n");
                    }
                    else
                    {
                        player.client.send("You can't wear that there!\r\n");
                    }
                }
            }
        }

        private void doInventory(string[] args, Player player)
        {
            player.client.send(player.items.inventoryToString());
        }

        private void doEquipment(string[] args, Player player)
        {
            player.client.send(player.items.equippedToString());
        }

        private void doDrop(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();

            if (args.Length == 1)
            {
                player.client.send("Drop what?");
            }
            else
            {
                Item item = player.items.getItemByName(args[1].TrimEnd('\r', '\n'));

                if (item != null)
                {
                    player.items.removeFromInventory(item);
                    player.room.items.Add(item);
                    buffer.AppendFormat("You drop a {0} onto the ground.", item.description);
                    player.client.send(buffer.ToString());
                }
                else
                {
                    player.client.send("I can't find that item.");
                }
            }
        }

        private void doRemove(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();

            if (args.Length == 1)
            {
                player.client.send("Remove what?");
            }
            else
            {
                Item item = player.items.removeFromEquipped(args[1].TrimEnd('\r', '\n'));

                if (item != null)
                {
                    buffer.AppendFormat("Removed {0} from {1}.", item.description, item.wearLocation);
                    player.client.send(buffer.ToString());
                }
                else
                {
                    player.client.send("I don't know how to remove that.");
                }
            }
        }

        private void doPickup(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();

            if (args.Length == 1)
            {
                player.client.send("Pickup what?");
            }
            else
            {
                Item item = player.room.getItemByName(args[1].TrimEnd('\r', '\n'));

                if (item != null)
                {
                    buffer.AppendFormat("You pick up {0}, off the ground.", item.description);
                    player.client.send(buffer.ToString());
                    player.room.items.Remove(item);
                    player.items.addToInventory(item);
                }
                else
                {
                    player.client.send("I don't see that anywhere.");
                }
            }
        }
    }
}
