using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Models.Weapons.Entities;
using PlanetWars.Repositories.Contracts;

namespace PlanetWars.Repositories.Entities
{
    public class WeaponRepository : IRepository<IWeapon>
    {
        private List<IWeapon> weapons;
        public WeaponRepository()
        {
            weapons = new List<IWeapon>();
        }

        public IReadOnlyCollection<IWeapon> Models
        {
            get => weapons;

        }
        public void AddItem(IWeapon model)
        {
           weapons.Add(model);
        }

        public IWeapon FindByName(string name)
        {
            IWeapon weapon = weapons.FirstOrDefault(x => x.GetType().Name == name);

            return weapon;
        }

        public bool RemoveItem(string name)
        {
            return weapons.Remove(weapons.FirstOrDefault(x => x.GetType().Name == name));
        }
    }
}
