using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheHouse;

namespace TheHouse
{
    public class StoreCommand : Command
    {
        public StoreCommand() : base()
        {
            Name = "store";
        }

        public override bool Execute(Player player)
        {
            if (HasSecondWord())
            {
                string itemName = SecondWord + (HasThirdWord() ? " " + ThirdWord : "");
                player.Store(itemName);
            }
            else
            {
                player.WarningMessage("\n What are you trying to store?");
            }

            return false;
        }
    }
}

