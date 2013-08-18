using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace amud_server
{
    partial class Commands
    {
        private void doShop(string[] args, Player player)
        {
            player.room.findShop(args, player);
        }
    }
}
