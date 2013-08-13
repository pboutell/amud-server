using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Room
    {
        public List<Room> exits = new List<Room>();
        public List<Player> players = new List<Player>();
        public List<NPC> npcs = new List<NPC>();
        public List<Item> items = new List<Item>();

        public string name;
        public string description;
        
        public Room(string name, string description)
        {
            this.name = name;
            this.description = description;
            initExits(this.exits);
        }

        public Room(string name, string description, List<Room> exits)
        {
            this.name = name;
            this.description = description;
            initExits(exits);
        }

        public bool hasExit(int direction)
        {
            return exits.ElementAtOrDefault(direction) != null;
        }

        public bool newExit(int direction)
        {
            if (hasExit(direction))
            {
                return false;
            }
            else
            {
                Room newRoom = new Room("New Room", "This room has not been finished yet.");
                exits[direction] = newRoom;
                newRoom.exits[Direction.oppositeExit(direction)] = this;
                World.rooms.Add(newRoom);
                return true;
            }
        }

        public string listOtherPlayers(Player player)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Player p in players)
            {
                if (p != player)
                {
                    buffer.Append(p.name);
                    buffer.Append(" is standing here.\r\n");
                }
            }

            return buffer.ToString();
        }

        public string listNPCs()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (NPC n in npcs)
            {
                buffer.AppendFormat("A {0} is standing here.\r\n", n.name);
            }

            return buffer.ToString();
        }

        public string listItems()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Item i in items)
            {
                buffer.AppendFormat("A {0} is sitting on the ground.\r\n", i.name);
            }

            return buffer.ToString();
        }

        public Player getPlayerByName(string search)
        {
            foreach (Player p in players)
            {
                if (p.name == search.TrimEnd('\r', '\n'))
                {
                    return p;
                }
            }

            return null;
        }

        public NPC getNPCByName(string search)
        {
            foreach (NPC n in npcs)
            {
                if (n.name == search.TrimEnd('\r', '\n'))
                {
                    return n;
                }
            }

            return null;
        }

        public Item getItemByName(string search)
        {
            foreach (Item i in items)
            {
                if (i.name == search.TrimEnd('\r', '\n'))
                {
                    return i;
                }
            }
            return null;
        }

        public void addPlayer(Player player)
        {
            sendToRoom(player.name + " enters the room.\r\n");
            players.Add(player);
            player.room = this;
        }

        public void removePlayer(Player player)
        {
            players.Remove(player);
            sendToRoom(player.name + " exits the room.\r\n");
        }

        public void addNPC(NPC npc)
        {
            npcs.Add(npc);
        }

        public void removeNPC(NPC npc)
        {
            npcs.Remove(npc);
        }

        private void initExits(List<Room> exits) 
        {
            for (int x = 0; x < 4; x++)
            {
                if (exits.ElementAtOrDefault(x) == null)
                {
                    this.exits.Add(null);
                }
                else
                {
                    this.exits.Add(exits.ElementAtOrDefault(x));
                }
            }
        }

        public string exitsToString()
        {
            StringBuilder exits = new StringBuilder();

            exits.Append("[ ");
            int appended = 0;
            for (int x = 0; x < 4; x++)
            {
                if (this.hasExit(x))
                {
                    exits.Append(Direction.directionToName(x));
                    appended++;
                    exits.Append(" ");
                }
            }

            if (appended < 1)
                exits.Append("none ");

            exits.Append("]");

            return exits.ToString();
        }

        public void sendToRoom(string message)
        {
            foreach (Player p in players)
            {
                p.client.send("\r\n\n" + message);
            }
        }
    }
}
