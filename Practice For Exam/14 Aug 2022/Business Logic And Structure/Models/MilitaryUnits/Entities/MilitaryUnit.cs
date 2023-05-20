using System;
using System.Collections.Generic;
using System.Text;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Utilities.Messages;

namespace PlanetWars.Models.MilitaryUnits.Entities
{
    public abstract class MilitaryUnit : IMilitaryUnit
    {
        private double cost;
        private int enduranceLevel;

        private MilitaryUnit()
        {
            EnduranceLevel = 1;
        }
        protected MilitaryUnit(double cost) : this()
        {
            Cost = cost;
        }

        public double Cost
        {
            get => cost;

            private set => cost = value;
        }

        public int EnduranceLevel
        {
            get => enduranceLevel;

            private set => enduranceLevel = value;
        }
        public void IncreaseEndurance()
        {
            EnduranceLevel += 1;

            if (EnduranceLevel > 20)
            {
                EnduranceLevel = 20;
                throw new ArgumentException(ExceptionMessages.EnduranceLevelExceeded);
            }
        }
    }
}
