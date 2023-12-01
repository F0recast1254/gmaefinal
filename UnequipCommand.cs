using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheHouse;

namespace TheHouse
{
    public class UnequipCommand : Command
    {
        public UnequipCommand() : base()
        {
            Name = "unequip";
        }

        public override bool Execute(Player player)
        {
            if (HasSecondWord())
            {
                player.Unequip(SecondWord);
            }
            else
            {
                player.WarningMessage("\n What are you trying to unequip?");
            }

            return false;
        }
    }
}

