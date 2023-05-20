using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChristmasPastryShop.Core.Contracts;
using ChristmasPastryShop.Models.Booths;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;

namespace ChristmasPastryShop.Core
{
    public class Controller : IController
    {
        private IRepository<IBooth> booths;

        public Controller()
        {
            booths = new BoothRepository();
        }
        public string AddBooth(int capacity)
        {
            int iD = booths.Models.Count + 1;

            IBooth booth = new Booth(iD, capacity);
            booths.AddModel(booth);

            return string.Format(OutputMessages.NewBoothAdded, iD, capacity);
        }

        public string AddDelicacy(int boothId, string delicacyTypeName, string delicacyName)
        {
            IBooth booth = booths.Models.First(x => x.BoothId == boothId);

            IDelicacy delicacy;

            if (delicacyTypeName == "Gingerbread")
            {
                delicacy = new Gingerbread(delicacyName);
            }
            else if (delicacyTypeName == "Stolen")
            {
                delicacy = new Stolen(delicacyName);
            }
            else
            {
                return string.Format(OutputMessages.InvalidDelicacyType, delicacyName);
            }

            if (booth.DelicacyMenu.Models.FirstOrDefault(x => x.Name == delicacy.Name) != null)
            {
                return string.Format(OutputMessages.DelicacyAlreadyAdded, delicacyName);
            }

            booth.DelicacyMenu.AddModel(delicacy);

            return string.Format(OutputMessages.NewDelicacyAdded, delicacyTypeName, delicacyName);
        }

        public string AddCocktail(int boothId, string cocktailTypeName, string cocktailName, string size)
        {
            IBooth booth = booths.Models.First(x => x.BoothId == boothId);

            ICocktail cocktail;

            if (cocktailTypeName == "Hibernation")
            {
                cocktail = new Hibernation(cocktailName, size);
            }
            else if (cocktailTypeName == "MulledWine")
            {
                cocktail = new MulledWine(cocktailName, size);
            }
            else
            {
                return string.Format(OutputMessages.InvalidCocktailType, cocktailTypeName);
            }

            if (size != "Small" && size != "Middle" && size != "Large")
            {
                return string.Format(OutputMessages.InvalidCocktailSize, size);
            }

            if (booth.CocktailMenu.Models.FirstOrDefault(x => x.Name == cocktailName && x.Size == size) != null)
            {
                return string.Format(OutputMessages.CocktailAlreadyAdded, size, cocktailName);
            }

            booth.CocktailMenu.AddModel(cocktail);

            return string.Format(OutputMessages.NewCocktailAdded, cocktail.Size, cocktail.Name, cocktailTypeName);
        }

        public string ReserveBooth(int countOfPeople)
        {
            List<IBooth> orderedBooths = booths.Models.Where(x => x.Capacity >= countOfPeople && !x.IsReserved)
                .OrderBy(x => x.Capacity).ThenByDescending(x => x.BoothId).ToList();

            if (orderedBooths.Count == 0)
            {
                return string.Format(OutputMessages.NoAvailableBooth, countOfPeople);
            }

            IBooth booth = orderedBooths[0];

            booth.ChangeStatus();

            return string.Format(OutputMessages.BoothReservedSuccessfully, booth.BoothId, countOfPeople);
        }

        public string TryOrder(int boothId, string order)
        {
            IBooth booth = booths.Models.First(x => x.BoothId == boothId);

            string[] input = order.Split("/", StringSplitOptions.RemoveEmptyEntries);

            string itemName = input[1];
            int orderedPieces = int.Parse(input[2]);


            ICocktail cocktail = null;
            IDelicacy delicacy = null;

            if (input[0] == "Hibernation" || input[0] == "MulledWine")
            {
                cocktail = booth.CocktailMenu.Models.First(x => x.Name == itemName);
            }
            else if (input[0] == "Gingerbread" || input[0] == "Stolen")
            {
                delicacy = booth.DelicacyMenu.Models.FirstOrDefault(x => x.Name == itemName);
            }
            else
            {
                return string.Format(OutputMessages.NotRecognizedType, input[0]);
            }

            if (cocktail != null)
            {
                string size = input[3];
                
                if (booth.CocktailMenu.Models.FirstOrDefault(x => x.Name == cocktail.Name) == null)
                {
                    return string.Format(OutputMessages.NotRecognizedItemName, input[0], itemName);
                }

                if (booth.CocktailMenu.Models.FirstOrDefault(x => x.Size == size) == null)
                {
                    return string.Format(OutputMessages.CocktailStillNotAdded, size, cocktail.Name);
                }

                booth.UpdateCurrentBill(cocktail.Price * orderedPieces);

                return string.Format(OutputMessages.SuccessfullyOrdered, booth.BoothId, orderedPieces, cocktail.Name);
            }
            else
            {
                if (delicacy == null)
                {
                    return string.Format(OutputMessages.NotRecognizedItemName, input[0], itemName);
                }

                booth.UpdateCurrentBill(delicacy.Price * orderedPieces);

                return string.Format(OutputMessages.SuccessfullyOrdered, booth.BoothId, orderedPieces, delicacy.Name);
            }
        }

        public string LeaveBooth(int boothId)
        {
            IBooth booth = booths.Models.First(x => x.BoothId == boothId);

            double currentBill = booth.CurrentBill;

            booth.Charge();
            booth.ChangeStatus();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Bill {currentBill:f2} lv");
            sb.AppendLine($"Booth {boothId} is now available!");

            return sb.ToString().Trim();
        }

        public string BoothReport(int boothId)
        {
            IBooth booth = booths.Models.First(x => x.BoothId == boothId);

            return booth.ToString();
        }
    }
}
