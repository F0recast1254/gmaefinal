using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class InventoryCommand : Command
    {
        public InventoryCommand() : base()
        {
            Name = "inventory";
        }

        public override bool Execute(Player player)
        {
            player.DisplayInventory();
            return false;
        }
    }
}