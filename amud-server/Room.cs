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

        private List<Character> characters = new List<Character>();
        private List<Player> players = new List<Player>();
        private List<NPC> npcs = new List<NPC>();
        private List<Item> items = new List<Item>();

        public string name { get; private set; }
        public string description { get; private set; }
        
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

        public string playersToString(Player player)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Player p in players)
            {
                if (p != player)
                {
                    if (!p.client.playing)
                    {
                        buffer.Append("%w( %rdisconnected%w )%x ");
                    }
                    buffer.AppendFormat("{0} is standing here.\r\n", player.name);
                }
            }

            return buffer.ToString().TrimEnd('\r', '\n');
        }

        public string NPCsToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (NPC n in npcs)
            {
                buffer.AppendFormat("A {0} is standing here.\r\n", n.name);
            }

            return buffer.ToString().TrimEnd('\r', '\n');
        }

        public string itemsToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Item i in items)
            {
                if (i.name.EndsWith("s"))
                    buffer.AppendFormat("Some {0} are laying on the ground.\r\n", i.name);
                else
                    buffer.AppendFormat("A {0} is laying on the ground.\r\n", i.name);
            }

            return buffer.ToString().TrimEnd('\r', '\n');
        }

        public Character getCharacterByName(string search)
        {
            foreach (Character c in characters)
            {
                if (c.name.StartsWith(search.TrimEnd('\r', '\n')))
                {
                    return c;
                }
            }

            return null;
        }

        public Player getPlayerByName(string search)
        {
            foreach (Player p in players)
            {
                if (p.name.StartsWith(search.TrimEnd('\r', '\n')))
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
                if (n.name.StartsWith(search.TrimEnd('\r', '\n')))
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
                if (i.name.StartsWith(search.TrimEnd('\r', '\n')))
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
            characters.Add(player);
            player.room = this;
        }

        public void removePlayer(Player player)
        {
            characters.Remove(player);
            players.Remove(player);
            sendToRoom(player.name + " exits the room.\r\n");
        }

        public void addNPC(NPC npc)
        {
            npc.room = this;
            characters.Add(npc);
            npcs.Add(npc);
        }

        public void removeNPC(NPC npc)
        {
            characters.Remove(npc);
            npcs.Remove(npc);
        }

        public void addItem(Item item)
        {
            items.Add(item);
        }

        public void removeItem(Item item)
        {
            items.Remove(item);
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

            exits.Append("%W[%Mexits:%Y");
            int appended = 0;
            for (int x = 0; x < 4; x++)
            {
                if (this.hasExit(x))
                {
                    exits.Append(" ");
                    exits.Append(Direction.directionToName(x));
                    appended++;
                }
            }

            if (appended < 1)
                exits.Append(" none");

            exits.Append("%W]");

            return exits.ToString();
        }

        public void sendToRoom(string message)
        {
            foreach (Player p in players)
            {
                p.client.send(message);
            }
        }
    }
}
