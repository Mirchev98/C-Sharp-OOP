using System;
using System.Collections.Generic;
using System.Text;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;

namespace ChristmasPastryShop.Models.Booths
{
    public class Booth : IBooth
    {
        private int capacity;
        private double turnover;
        public Booth(int boothId, int capacity)
        {
            BoothId = boothId;
            Capacity = capacity;
            DelicacyMenu = new DelicacyRepository();
            CocktailMenu = new CocktailRepository();
        }
        public int BoothId { get; private set; }

        public int Capacity
        {
            get => capacity;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.CapacityLessThanOne);
                }

                capacity = value;
            }
        }
        public IRepository<IDelicacy> DelicacyMenu { get; private set; }
        public IRepository<ICocktail> CocktailMenu { get; private set; }
        public double CurrentBill { get; private set; }
        public double Turnover => turnover;
        public bool IsReserved { get; private set; }
        public void UpdateCurrentBill(double amount)
        {
            CurrentBill += amount;
        }

        public void Charge()
        {
            turnover += CurrentBill;
            CurrentBill = 0;
        }

        public void ChangeStatus()
        {
            if (IsReserved)
            {
                IsReserved = false;
            }
            else
            {
                IsReserved = true;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Booth: {this.BoothId}");
            sb.AppendLine($"Capacity: {this.Capacity}");
            sb.AppendLine($"Turnover: {this.Turnover:F2} lv");
            sb.AppendLine($"-Cocktail menu:");

            foreach (var cocktail in CocktailMenu.Models)
            {
                sb.AppendLine($"--{cocktail.ToString()}");
            }

            sb.AppendLine($"-Delicacy menu:");

            foreach (var delicacy in DelicacyMenu.Models)
            {
                sb.AppendLine($"--{delicacy.ToString()}");
            }

            return sb.ToString().Trim();
        }
    }
}
