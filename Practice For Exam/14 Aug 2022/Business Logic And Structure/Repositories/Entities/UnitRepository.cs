using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Repositories.Contracts;

namespace PlanetWars.Repositories.Entities
{
    public class UnitRepository : IRepository<IMilitaryUnit>
    {
        private List<IMilitaryUnit> units;

        public UnitRepository()
        {
            units = new List<IMilitaryUnit>();
        }
        public IReadOnlyCollection<IMilitaryUnit> Models => units;
        public void AddItem(IMilitaryUnit model)
        {
            units.Add(model);
        }

        public IMilitaryUnit FindByName(string name)
        {
            return units.FirstOrDefault(x => x.GetType().Name == name);
        }

        public bool RemoveItem(string name)
        {
            return units.Remove(units.FirstOrDefault(x => x.GetType().Name == name));
        }
    }
}
