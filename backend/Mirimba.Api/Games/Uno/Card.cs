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
                    Description.Contains("block")
                    || Description.Contains("reverse")
                    || Description.Contains("plus2")
                    || Description.Contains("change-color")
                    || Description.Contains("plus4");
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
