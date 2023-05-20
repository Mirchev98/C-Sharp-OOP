using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace PlanetWars.Tests
{
    public class Tests
    {
        [TestFixture]
        public class PlanetWarsTests
        {
            [SetUp]
            public void SetUp()
            {
                Planet testPlanet = new Planet("Planet", 1000);
            }

            [Test]
            public void WeaponsConstructorShouldSetDataProperly()
            {
                string expectedName = "powerWeapon";
                double expectedPrice = 0.1;
                int expectedDestr = 9;

                Weapon weapon = new Weapon(expectedName, expectedPrice, expectedDestr);

                Assert.AreEqual(expectedName, weapon.Name);
                Assert.AreEqual(expectedPrice, weapon.Price);
                Assert.AreEqual(expectedDestr, weapon.DestructionLevel);
            }

            [Test]
            public void WeaponShouldThrowOnInvalidPrice()
            {
                int throwingPice = -1;

                Assert.Throws<ArgumentException>(() =>
                {
                    Weapon weapon = new Weapon("gosho", throwingPice, 5);
                }, "Price cannot be negative.");
            }

            [Test]
            public void WeaponShouldBeNuclear()
            {
                Weapon weapon = new Weapon("Gosho", 100, 15);

                Assert.AreEqual(true, weapon.IsNuclear);
            }

            [Test]
            public void WeaponShouldIncreasePower()
            {
                Weapon weapon = new Weapon("Gosho", 100, 5);

                int expectedPower = 6;

                weapon.IncreaseDestructionLevel();

                Assert.AreEqual(expectedPower, weapon.DestructionLevel);
            }

            [Test]
            public void PlanetConstructorShouldSetDataProperly()
            {
                string expectedName = "Gosho";
                int expectedBudget = 10;

                Planet goshoPlanet = new Planet(expectedName, expectedBudget);

                Assert.AreEqual(expectedName, goshoPlanet.Name);
                Assert.AreEqual(expectedBudget, goshoPlanet.Budget);
            }

            [TestCase(null)]
            [TestCase("")]
            public void PlanetShouldThrowOnInvalidName(string name)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    Planet planet = new Planet(name, 10);
                }, "Invalid planet Name");
            }

            [TestCase(-5)]
            [TestCase(-1)]
            public void PlanetShouldThrowOnInvalidBudget(int budget)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    Planet planet = new Planet("Pencho", budget);
                }, "Budget cannot drop below Zero!");

            }

            [Test] 
            public void PlanetWeaponsShouldReturnListOfWeapons()
            {
                Planet planet = new Planet("Gosho", 10000);

                

                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);
                Weapon weaponTwo = new Weapon("Gosho", 10.9, 100);
                Weapon weaponThree = new Weapon("Dimitrichko", 10.9, 100);

                IReadOnlyCollection<Weapon> weapons = new List<Weapon>()
                {
                    weaponOne,
                    weaponTwo,
                    weaponThree
                };

                planet.AddWeapon(weaponOne);
                planet.AddWeapon(weaponTwo);
                planet.AddWeapon(weaponThree);

                Assert.AreEqual(weapons, planet.Weapons);
            }

            [Test]
            public void PlanetWeaponsShouldThrowWhenAddingAddedWeapon()
            {
                Planet planet = new Planet("Gosho", 10000);

                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);
                Weapon weaponTwo = new Weapon("Gosho", 10.9, 100);
                Weapon weaponThree = new Weapon("Dimitrichko", 10.9, 100);



                planet.AddWeapon(weaponOne);
                planet.AddWeapon(weaponTwo);
                planet.AddWeapon(weaponThree);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.AddWeapon(weaponOne);
                }, $"There is already a {weaponOne.Name} weapon.");
            }

            [Test]
            public void PlanetWeaponsShouldRemoveProper()
            {
                Planet planet = new Planet("Gosho", 10000);



                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);
                Weapon weaponTwo = new Weapon("Gosho", 10.9, 100);
                Weapon weaponThree = new Weapon("Dimitrichko", 10.9, 100);

                IReadOnlyCollection<Weapon> weapons = new List<Weapon>()
                {
                    weaponTwo,
                    weaponThree
                };

                planet.AddWeapon(weaponOne);
                planet.AddWeapon(weaponTwo);
                planet.AddWeapon(weaponThree);

                planet.RemoveWeapon(weaponOne.Name);

                Assert.AreEqual(weapons, planet.Weapons);
            }

            [Test]
            public void PlanetWeaponsShouldUpgradeProper()
            {
                Planet planet = new Planet("Gosho", 10000);

                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);

                planet.AddWeapon(weaponOne);

                var weaponTwo = weaponOne;

                weaponTwo.IncreaseDestructionLevel();

                int expectedPower = weaponTwo.DestructionLevel + 1;

                planet.UpgradeWeapon(weaponOne.Name);

                IReadOnlyCollection<Weapon> weapons = planet.Weapons;

                int actualPower = weapons.FirstOrDefault(x => x.Name == weaponOne.Name).DestructionLevel;

                Assert.AreEqual(expectedPower, actualPower);
            }

            [Test]
            public void planetShouldThrowWhenUpgradingNonExistingWeapon()
            {
                Planet planet = new Planet("Gosho", 10000);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.UpgradeWeapon("PenchoWeapon");
                }, "PenchoWeapon does not exist in the weapon repository of Gosho");
            }

            [Test]
            public void MilitaryPowerOfPlanetShouldBeCalculatedProperly()
            {
                Planet planet = new Planet("Gosho", 10000);



                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);
                Weapon weaponTwo = new Weapon("Gosho", 10.9, 100);
                Weapon weaponThree = new Weapon("Dimitrichko", 10.9, 100);

                int total = weaponOne.DestructionLevel + weaponTwo.DestructionLevel + weaponThree.DestructionLevel;

                planet.AddWeapon(weaponOne);
                planet.AddWeapon(weaponTwo);
                planet.AddWeapon(weaponThree);

                Assert.AreEqual(total, planet.MilitaryPowerRatio);
            }

            [Test]
            public void PlanetShouldProfit()
            {
                Planet planet = new Planet("Gosho", 1000);

                int expectedBudget = 2000;

                planet.Profit(1000);

                Assert.AreEqual(expectedBudget, planet.Budget);
            }

            [Test]
            public void PlanetShouldSpend()
            {
                Planet planet = new Planet("Gosho", 1000);

                int expectedBudget = 500;

                planet.SpendFunds(500);

                Assert.AreEqual(expectedBudget, planet.Budget);
            }

            [Test]
            public void PlanetShouldThrowWhenSpendingMoreThanBudget()
            {
                Planet planet = new Planet("Gosho", 1000);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.SpendFunds(1500);
                }, "Not enough funds to finalize the deal.");
            }

            [Test]
            public void PlanetShouldDestroy()
            {
                Planet planet = new Planet("Gosho", 10000);
                Planet planetTwo = new Planet("Pencho", 1);


                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);
                Weapon weaponTwo = new Weapon("Gosho", 10.9, 100);
                Weapon weaponThree = new Weapon("Dimitrichko", 10.9, 100);

                planet.AddWeapon(weaponOne);
                planet.AddWeapon(weaponTwo);
                planet.AddWeapon(weaponThree);

                string expectedOutput = "Pencho is destructed!";

                Assert.AreEqual(expectedOutput, planet.DestructOpponent(planetTwo));
            }

            [Test]
            public void PlanetShouldThrowOnDestroybiggerPlanet()
            {
                Planet planet = new Planet("Gosho", 10000);
                Planet planetTwo = new Planet("Pencho", 1);


                Weapon weaponOne = new Weapon("Pencho", 10.9, 100);
                Weapon weaponTwo = new Weapon("Gosho", 10.9, 100);
                Weapon weaponThree = new Weapon("Dimitrichko", 10.9, 100);

                planetTwo.AddWeapon(weaponOne);
                planetTwo.AddWeapon(weaponTwo);
                planetTwo.AddWeapon(weaponThree);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.DestructOpponent(planetTwo);
                }, "Pencho is too strong to declare war to!");
            }
        }
    }
}
