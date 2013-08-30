using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace amud_server
{
    [Serializable]
    class Merchant : NPC
    {
        public Merchant()
        {
            base.gold = 1000;
            base.isMobile = false;
        }

        public override void update(DateTime time)
        {
            base.update(time);
        }

        public override bool shop(string[] args, Player player)
        {
            if (args.Length > 1)
            {
                if (args[1].TrimEnd('\r', '\n') == "buy")
                {
                    if (args.ElementAtOrDefault(2) != null)
                    {
                        buyItem(args[2].TrimEnd('\r', '\n'), player);
                    }
                    else
                    {
                        say("What would you like to purchase?");
                        player.client.send(listBuy());
                    }
                }
                else if (args[1].TrimEnd('\r', '\n') == "sell")
                {
                    if (args.ElementAtOrDefault(2) != null)
                    {
                        sellItem(args[2].TrimEnd('\r', '\n'), player);
                    }
                    else
                    {
                        say("I can give you these prices for your stuff.");
                        player.client.send(listSell(player));
                    }
                }
            }
            else
            {
                say("Hello! please have a look at my merchandise.");
                player.client.send(listBuy());
            }

            return true;
        }

        private string listBuy()
        {
            StringBuilder buffer = new StringBuilder();
            int x = 1;

            buffer.AppendLine("\r\n%YItems Available:");
            foreach (Item i in items.inventory)
            {
                buffer.AppendFormat("\r\n\t%B{0}%W) %w{1,3}%Yg %W[ %w{2,8}%W ]  %B{3}%x",
                                    x++,
                                    i.value*2,
                                    i.name,
                                    i.description);
            }

            return buffer.ToString();
        }

        private string listSell(Player player)
        {
            StringBuilder buffer = new StringBuilder();
            int x = 1;

            buffer.AppendLine("\r\n%YYou can sell:");
            foreach (Item i in player.items.inventory)
            {
                buffer.AppendFormat("\r\n\t%B{0}%W) %w{1,3}%Yg %W[ %w{2,8}%W ]  %B{3}%x",
                                    x++,
                                    i.value,
                                    i.name,
                                    i.description);
            }

            return buffer.ToString();
        }

        private void buyItem(string search, Player player)
        {
            StringBuilder buffer = new StringBuilder();
            
            Item item = items.getItemByName(search);
            
            if (item != null)
            {
                int sellPrice = item.value * 2;
                if (player.gold > sellPrice)
                {
                    player.items.addToInventory(item);
                    gold += sellPrice;
                    player.gold -= sellPrice;
                    buffer.AppendFormat("\r\n%BYou purchase %W{0} %Bfor %Y{1} %Bgold.", item.description, sellPrice);
                    say("Thank you for your business.");
                    player.client.send(buffer.ToString());
                }
                else
                {
                    say("Please, don't waste my time. Come back when you have some coin.");
                }
            }
            else
            {
                say("I apologize, I am fresh out of " + search);
            }
        }

        private void sellItem(string search, Player player)
        {
            StringBuilder buffer = new StringBuilder();

            Item item = player.items.getItemByName(search);

            if (item != null)
            {
                if (gold > item.value)
                {
                    player.items.removeFromInventory(item);
                    items.addToInventory(item);
                    player.gold += item.value;
                    gold -= item.value;
                    buffer.AppendFormat("\r\n%BYou sell %W{0} to {1}, %Bfor %Y{2} %Bgold.", item.description, name, item.value);
                    say("Thank you for your business.");
                    player.client.send(buffer.ToString());
                }
                else
                {
                    say("I can't purchase that at the moment, I'm a little short.");
                }
            }
            else
            {
                player.client.send("I can't find that item");
            }
        }
    }
}
