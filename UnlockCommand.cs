using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheHouse;

namespace TheHouse
{
    public class UnlockCommand : Command
    {
        public UnlockCommand() : base()
        {
            this.Name = "unlock";
        }
        public override bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Unlock(this.SecondWord);
            }
            else
            {
                player.WarningMessage("Unlock what?");
            }
            return false;


        }
    }
}
