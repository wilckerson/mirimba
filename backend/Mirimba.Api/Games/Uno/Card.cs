using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class Card
    {
        public string Description { get; private set; }
        public bool IsSpecial
        {
            get
            {
                return
                    Description.Contains("+2")
                    || Description.Contains("+4")
                    || Description.Contains("Inverter")
                    || Description.Contains("MudarCor")
                    || Description.Contains("Pular");
            }
        }

        public Card(string description)
        {
            this.Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
