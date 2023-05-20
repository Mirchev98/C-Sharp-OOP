using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Heroes.Models.Contracts;
using Heroes.Models.Heroes;

namespace Heroes.Models.Map
{
    public class Map : IMap
    {
        public string Fight(ICollection<IHero> players)
        {
            List<IHero> knights = players.Where(x => x.GetType().Name == nameof(Knight)).ToList();
            List<IHero> barbarians = players.Where(x => x.GetType().Name == nameof(Barbarian)).ToList();

            while (knights.All(x => x.IsAlive == true) && barbarians.All(x => x.IsAlive == true))
            {
                foreach (var knight in knights)
                {
                    if (knight.IsAlive)
                    {
                        foreach (var barbarian in barbarians)
                        {
                            barbarian.TakeDamage(knight.Weapon.DoDamage());
                        }
                    }
                }

                foreach (var barbarian in barbarians)
                {
                    if (barbarian.IsAlive)
                    {
                        foreach (var knight in knights)
                        {
                            knight.TakeDamage(barbarian.Weapon.DoDamage());
                        }
                    }
                }
            }

            if (knights.Any(x => x.IsAlive) || barbarians.All(x => x.IsAlive != true))
            {
                return $"The knights took {knights.Count(x => x.IsAlive == false)} casualties but won the battle.";
            }
            
            return $"The barbarians took {barbarians.Count(x => x.IsAlive == false)} casualties but won the battle.";

        }
    }
}
