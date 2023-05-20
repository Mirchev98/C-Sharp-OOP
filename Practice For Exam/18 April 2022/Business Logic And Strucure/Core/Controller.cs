using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Heroes.Core.Contracts;
using Heroes.Models.Contracts;
using Heroes.Models.Heroes;
using Heroes.Models.Map;
using Heroes.Models.Weapons;
using Heroes.Repositories;
using Heroes.Repositories.Contracts;

namespace Heroes.Core
{
    public class Controller : IController
    {
        private IRepository<IHero> heroes;
        private IRepository<IWeapon> weapons;
        public Controller()
        {
            heroes = new HeroRepository();
            weapons = new WeaponRepository();
        }
        public string CreateWeapon(string type, string name, int durability)
        {
            if (weapons.FindByName(name) != null)
            {
                throw new InvalidOperationException($"The weapon {name} already exists.");
            }

            IWeapon weapon;

            if (type == nameof(Mace))
            {
                weapon = new Mace(name, durability);
            }
            else if (type == nameof(Claymore))
            {
                weapon = new Claymore(name, durability);
            }
            else
            {
                throw new InvalidOperationException("Invalid weapon type.");
            }

            weapons.Add(weapon);

            return $"A {type.ToLower()} {name} is added to the collection.";
        }

        public string CreateHero(string type, string name, int health, int armour)
        {
            if (heroes.FindByName(name) != null)
            {
                throw new InvalidOperationException($"The hero {name} already exists.");
            }

            IHero hero;

            if (type == nameof(Barbarian))
            {
                hero = new Barbarian(name, health, armour);
                heroes.Add(hero);
                return $"Successfully added Barbarian {name} to the collection.";
            }
            
            if (type == nameof(Knight))
            {
                hero = new Knight(name, health, armour);
                heroes.Add(hero);
                return $"Successfully added Sir {name} to the collection.";
            }
            
            
            throw new InvalidOperationException("Invalid hero type.");
            
        }

        public string AddWeaponToHero(string weaponName, string heroName)
        {
            IHero hero = heroes.FindByName(heroName);

            if (hero == null)
            {
                throw new InvalidOperationException($"Hero {heroName} does not exist.");
            }

            IWeapon weapon = weapons.FindByName(weaponName);

            if (weapon == null)
            {
                throw new InvalidOperationException($"Weapon {weaponName} does not exist.");
            }

            if (hero.Weapon != null)
            {
                throw new InvalidOperationException($"Hero {heroName} is well-armed.");
            }

            hero.AddWeapon(weapon);
            weapons.Remove(weapon);

            return $"Hero {heroName} can participate in battle using a {weapon.GetType().Name.ToLower()}.";
        }

        public string StartBattle()
        {
            Map map = new Map();

            List<IHero> armedHeroes = heroes.Models.Where(x => x.Weapon != null && x.IsAlive).ToList();

            return map.Fight(armedHeroes);
        }

        public string HeroReport()
        {
            var orderedHeroes = heroes.Models.OrderBy(x => x.GetType().Name).ToList();
            orderedHeroes = orderedHeroes.OrderByDescending(x => x.Health).ToList();
            orderedHeroes = orderedHeroes.OrderBy(x => x.Name).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var hero in orderedHeroes)
            {
                sb.AppendLine($"{hero.GetType().Name}: {hero.Name}");
                sb.AppendLine($"--Health: {hero.Health}");
                sb.AppendLine($"--Armour: {hero.Armour}");

                sb.AppendLine(hero.Weapon != null ? $"--Weapon: {hero.Weapon.Name}" : "--Weapon: Unarmed");
            }

            return sb.ToString().Trim();
        }
    }
}
