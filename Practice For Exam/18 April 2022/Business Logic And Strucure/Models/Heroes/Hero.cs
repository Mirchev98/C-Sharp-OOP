using System;
using System.Collections.Generic;
using System.Text;
using Heroes.Models.Contracts;

namespace Heroes.Models.Heroes
{
    public abstract class Hero : IHero
    {
        private string name;
        private int health;
        private int armour;
        private IWeapon weapon;
        protected Hero(string name, int health, int armour)
        {
            Name = name;
            Health = health;
            Armour = armour;
        }

        public string Name
        {
            get => name;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Hero name cannot be null or empty.");
                }

                name = value;
            }
        }

        public int Health
        {
            get => health;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Hero health cannot be below 0.");
                }

                health = value;
            }
        }
        public int Armour
        {
            get => armour;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Hero armour cannot be below 0.");
                }

                armour = value;
            }
        }
        public IWeapon Weapon
        {
            get => weapon;

            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Weapon cannot be null.");
                }

                weapon = value;
            }
        }

        public bool IsAlive
        {
            get
            {
                if (this.Health > 0)
                {
                    return true;
                }

                return false;
            }
        }
        public void TakeDamage(int points)
        {

            int damage = points;

            if (Armour - damage <= 0)
            {
                damage -= Armour;
                Armour = 0;
            }
            else
            {
                Armour -= damage;
                damage = 0;
            }

            if (Health - damage <= 0)
            {
                Health = 0;
            }
            else
            {
                Health -= damage;
            }

        }

        public void AddWeapon(IWeapon weapon)
        {
            Weapon = weapon;
        }
    }
}
