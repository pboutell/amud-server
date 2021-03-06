﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Inventory
    {
        public Dictionary<string, Item> equipped { get; private set; }
        public List<Item> inventory { get; private set; }

        public Inventory()
        {
            this.equipped = new Dictionary<string, Item>();
            this.inventory = new List<Item>();
            this.equipped.Add("none", new Item("dummmy", "", 0, "", 0));
        }

        public void addToInventory(Item item)
        {
            inventory.Add(item);
        }

        public void removeFromInventory(Item item)
        {
            inventory.Remove(item);
        }

        public Item removeFromEquipped(string search)
        {
            Item item = null;

            foreach(KeyValuePair<string, Item> e in equipped)  
            {
                if (e.Value.name.StartsWith(search))
                {
                    equipped.TryGetValue(e.Key, out item);
                    item = e.Value;
                }
            }
            if (item != null)
            {
                equipped.Remove(item.wearLocation);
                inventory.Add(item);
            }

            return item;
        }

        public bool addToEquipped(string location, Item item)
        {
            if (!equipped.Keys.Contains(location))
            {
                equipped.Add(location, item);
                inventory.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string inventoryToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("  %w:%y[ %WInventory %y]%W:\r\n");

            foreach (Item i in inventory)
            {
                buffer.AppendFormat("  %W( %y{0,11}%W )    %W{1}\r\n", i.name, i.description);
            }

            return buffer.ToString();
        }

        public string equippedToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("  %w:%b[ %WEquipment %b]%W:");
            buffer.AppendLine();
            foreach (KeyValuePair<string, Item> e in equipped)
            {
                // Hax: this key wear location is only going to go so far.
                if (e.Key != "none" || equipped.Count == 1)
                {
                    buffer.AppendFormat("  %W( %b{0, 11} %W)    %W{1}\r\n", e.Key, e.Value.description);
                }
            }

            return buffer.ToString();
        }

        public Item getItemByName(string search)
        {
            foreach (Item i in inventory)
            {
                if (i.name.StartsWith(search) && search.Length > 0)
                {
                    return i;
                }
            }

            return null;
        }
    }
}
