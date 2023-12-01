using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class EquipCommand : Command
    {
        public EquipCommand() : base()
        {
            Name = "equip";
        }

        public override bool Execute(Player player)
        {
            if (player.CanEquip())
            {
                if (HasSecondWord())
                {
                    player.Equip(SecondWord);
                }
                else
                {
                    player.WarningMessage("\n What are you trying to equip?");
                }
            }
            else
            {
                player.WarningMessage("\n Your hands are empty, or the item you're holding is not equipable.");
            }

            return false;
        }
    }
}

