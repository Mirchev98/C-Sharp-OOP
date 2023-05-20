using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using PlanetWars.Core.Contracts;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.MilitaryUnits.Entities;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Planets.Entities;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Models.Weapons.Entities;
using PlanetWars.Repositories.Entities;
using PlanetWars.Utilities.Messages;

namespace PlanetWars.Core
{
    public class Controller : IController
    {
        private PlanetRepository planets;

        public Controller()
        {
            planets = new PlanetRepository();
        }
        public string CreatePlanet(string name, double budget)
        {
            Planet planet = new Planet(name, budget);

            var existingPlanet = planets.FindByName(name);

            if (existingPlanet != null)
            {
                return string.Format(OutputMessages.ExistingPlanet, name);
            }

            planets.AddItem(planet);

            return string.Format(OutputMessages.NewPlanet, name);
        }

        public string AddUnit(string unitTypeName, string planetName)
        {
            var existingPlanet = planets.FindByName(planetName);

            if (existingPlanet == default)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnexistingPlanet, planetName));
            }

            if (unitTypeName != nameof(StormTroopers) && unitTypeName != nameof(AnonymousImpactUnit) && unitTypeName != nameof(SpaceForces))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.ItemNotAvailable, unitTypeName));
            }

            var existingUnit = existingPlanet.Army.FirstOrDefault(x => x.GetType().Name == unitTypeName);

            if (existingUnit != default)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnitAlreadyAdded, unitTypeName,
                    planetName));
            }

            IMilitaryUnit unit;

            if (unitTypeName == nameof(SpaceForces))
            {
                unit = new SpaceForces();
            }
            else if (unitTypeName == nameof(StormTroopers))
            {
                unit = new StormTroopers();
            }
            else
            {
                unit = new AnonymousImpactUnit();
            }

            existingPlanet.Spend(unit.Cost);
            existingPlanet.AddUnit(unit);

            return string.Format(OutputMessages.UnitAdded, unitTypeName, planetName);
        }

        public string AddWeapon(string planetName, string weaponTypeName, int destructionLevel)
        {
            var planet = planets.FindByName(planetName);

            if (planet == default)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnexistingPlanet, planetName));
            }

            IWeapon existringWeapon = planet.Weapons.FirstOrDefault(x => x.GetType().Name == weaponTypeName);

            if (existringWeapon != default)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.WeaponAlreadyAdded,
                    weaponTypeName, planetName));
            }

            if (weaponTypeName != nameof(BioChemicalWeapon) && weaponTypeName != nameof(NuclearWeapon) && weaponTypeName != nameof(SpaceMissiles))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.ItemNotAvailable, weaponTypeName));
            }

            IWeapon weapon;

            if (weaponTypeName == nameof(BioChemicalWeapon))
            {
                weapon = new BioChemicalWeapon(destructionLevel);
            }
            else if (weaponTypeName == nameof(NuclearWeapon))
            {
                weapon = new NuclearWeapon(destructionLevel);
            }
            else
            {
                weapon = new SpaceMissiles(destructionLevel);
            }

            planet.Spend(weapon.Price);
            planet.AddWeapon(weapon);

            return string.Format(OutputMessages.WeaponAdded, planetName, weaponTypeName);
        }

        public string SpecializeForces(string planetName)
        {
            var planet = planets.FindByName(planetName);

            if (planet == default)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnexistingPlanet, planetName));
            }

            if (planet.Army.Count == 0)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.NoUnitsFound));
            }

            double cost = 1.25;

            planet.TrainArmy();

            planet.Spend(1.25);

            return string.Format(OutputMessages.ForcesUpgraded, planetName);
        }

        public string SpaceCombat(string planetOne, string planetTwo)
        {
            IPlanet firstPlanet = planets.FindByName(planetOne);
            IPlanet secondPlanet = planets.FindByName(planetTwo);

            var firstNuclear = firstPlanet.Weapons.FirstOrDefault(x => x.GetType().Name == nameof(NuclearWeapon));
            var secondNuclear = secondPlanet.Weapons.FirstOrDefault(x => x.GetType().Name == nameof(NuclearWeapon));

            var firstPlanetHalfBudget = firstPlanet.Budget / 2;
            var secondPlanetHalfBudget = secondPlanet.Budget / 2;

            IPlanet winner;
            IPlanet loser;

            if (firstPlanet.MilitaryPower == secondPlanet.MilitaryPower)
            {
                if (firstNuclear != null && secondNuclear == null)
                {
                     winner = firstPlanet;
                     loser = secondPlanet;
                }
                else if (firstNuclear == null && secondNuclear != null)
                {
                     winner = secondPlanet;
                     loser = firstPlanet;
                }
                else
                {
                    firstPlanet.Spend(firstPlanetHalfBudget);
                    secondPlanet.Spend(secondPlanetHalfBudget);

                    return OutputMessages.NoWinner;
                }
            }
            else if (firstPlanet.MilitaryPower > secondPlanet.MilitaryPower)
            {
                 winner = firstPlanet;
                 loser = secondPlanet;
            }
            else
            {
                 winner = secondPlanet;
                 loser = firstPlanet;
            }

            winner.Spend(firstPlanetHalfBudget);
            winner.Profit(secondPlanetHalfBudget);
            double profit = loser.Weapons.Sum(x => x.Price) + loser.Army.Sum(x => x.Cost);

            winner.Profit(profit);

            planets.RemoveItem(loser.Name);

            return string.Format(OutputMessages.WinnigTheWar, winner.Name, loser.Name);
        }

        public string ForcesReport()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("***UNIVERSE PLANET MILITARY REPORT***");

            foreach (var planet in this.planets.Models.OrderByDescending(x => x.MilitaryPower).ThenBy(x => x.Name))
            {
                sb.AppendLine(planet.PlanetInfo());
            }

            return sb.ToString().Trim();
        }
    }
}
