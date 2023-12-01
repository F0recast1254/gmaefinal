using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheHouse;

namespace TheHouse
{
    public class RetrieveCommand : Command
    {
        public RetrieveCommand() : base()
        {
            Name = "retrieve";
        }

        public override bool Execute(Player player)
        {
            if (HasSecondWord())
            {
                player.Retrieve(SecondWord);
            }
            else
            {
                player.WarningMessage("\n What are you trying to retrieve?");
            }

            return false;
        }
    }
}