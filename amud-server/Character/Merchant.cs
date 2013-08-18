using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace amud_server
{
    [Serializable]
    class Merchant : NPC
    {
        public bool isMobile { get; private set; }

        public Merchant() :base()
        {
            isMobile = false;
        }

        public override void updateMovement(DateTime time)
        {
            if (isMobile)
            {
                base.updateMovement(time);
            }
        }

        public override bool shop(string[] args, Player player)
        {
            player.client.send(listBuy());
            return true;
        }

        private string listBuy()
        {
            StringBuilder buffer = new StringBuilder();
            int x = 0;

            say("Hello! please have a look at my merchandise.");

            buffer.AppendLine("\r\nItems Available:");
            foreach (Item i in items.inventory)
            {
                buffer.AppendFormat("\r\n{0})  {1,5}g  {2,8} {3}\r\n",
                                    x,
                                    i.value,
                                    i.name,
                                    i.description);
            }

            return buffer.ToString();
        }
    }
}
