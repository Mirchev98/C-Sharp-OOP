using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.MilitaryUnits.Entities;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Models.Weapons.Entities;
using PlanetWars.Repositories.Entities;
using PlanetWars.Utilities.Messages;

namespace PlanetWars.Models.Planets.Entities
{
    public class Planet : IPlanet
    {
        private UnitRepository units;
        private WeaponRepository weapons;
        private string name;
        private double budget;

        public Planet(string name, double budget)
        {
            Name = name;
            Budget = budget;

            units = new UnitRepository();
            weapons  = new WeaponRepository();
        }

        public string Name
        {
            get => name;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidPlanetName);
                }

                name = value;
            }
        }

        public double Budget
        {
            get => budget;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidBudgetAmount);
                }

                budget = value;
            }
        }

        public double MilitaryPower => Math.Round(CalculateMilitaryPower(), 3);

        public IReadOnlyCollection<IMilitaryUnit> Army => units.Models;
        public IReadOnlyCollection<IWeapon> Weapons => weapons.Models;
        public void AddUnit(IMilitaryUnit unit)
        {
            units.AddItem(unit);
        }

        public void AddWeapon(IWeapon weapon)
        {
            weapons.AddItem(weapon);
        }

        public void TrainArmy()
        {
            foreach (var militaryUnit in units.Models)
            {
                militaryUnit.IncreaseEndurance();
            }
        }

        public void Spend(double amount)
        {
            if (amount > Budget)
            {
                throw new InvalidOperationException(ExceptionMessages.UnsufficientBudget);
            }

            Budget -= amount;
        }

        public void Profit(double amount)
        {
            Budget += amount;
        }

        public string PlanetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Planet: {this.Name}");
            sb.AppendLine($"--Budget: {Budget} billion QUID");
            sb.Append("--Forces: ");

            if (this.Army.Count == 0)
            {
                sb.AppendLine("No units");
            }
            else
            {
                var units = new Queue<string>();

                foreach (var unit in Army)
                {
                    units.Enqueue(unit.GetType().Name);
                }

                sb.AppendLine(string.Join(", ", units));
            }


            sb.Append("--Combat equipment: ");

            if (this.Weapons.Count == 0)
            {
                sb.AppendLine("No weapons");
            }
            else
            {
                var weapons = new Queue<string>();

                foreach (var weapon in Weapons)
                {
                    weapons.Enqueue(weapon.GetType().Name);
                }

                sb.AppendLine(string.Join(", ", weapons));
            }

            sb.AppendLine($"--Military Power: {this.MilitaryPower}");

            return sb.ToString().Trim();
        }

        private double CalculateMilitaryPower()
        {
            double result = this.units.Models.Sum(x => x.EnduranceLevel) + this.weapons.Models.Sum(x => x.DestructionLevel);

            if (this.units.Models.Any(x => x.GetType().Name == nameof(AnonymousImpactUnit)))
            {
                result *= 1.3;
            }
            if (this.weapons.Models.Any(x => x.GetType().Name == nameof(NuclearWeapon)))
            {
                result *= 1.45;
            }

            return result;
        }
    }
}
