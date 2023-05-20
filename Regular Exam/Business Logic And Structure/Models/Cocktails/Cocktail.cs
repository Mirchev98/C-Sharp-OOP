using System;
using System.Collections.Generic;
using System.Text;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Utilities.Messages;

namespace ChristmasPastryShop.Models.Cocktails
{
    public abstract class Cocktail : ICocktail
    {
        private string name;
        private double price;

        protected Cocktail(string cocktailName, string size, double price)
        {
            Name = cocktailName;
            Size = size;
            Price = price;
        }
        public string Name
        {
            get => name;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.NameNullOrWhitespace);
                }

                name = value;
            }
        }
        public string Size { get; private set; }

        public double Price
        {
            get => price;

            private set
            {
                if (this.Size == "Large")
                {
                    price = value;
                }
                else if (this.Size == "Middle")
                {
                    price = value * 2 / 3;
                }
                else if (this.Size == "Small")
                {
                    price = value * 1 / 3;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{this.Name} ({this.Size}) - {this.Price:F2} lv");

            return sb.ToString().Trim();
        }
    }
}
