using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions 
{
    public class Day1
    {
        private readonly Func<decimal, decimal> _model;
        
        public Day1(bool useSimpleModel = true) 
        {
            _model =
                useSimpleModel 
                ? (Func<decimal, decimal>)(mass => CalculateFuelForMass(mass)) 
                : mass => Complex(mass);
        }

        public decimal CalculateRequiredFuel(IEnumerable<decimal> moduleMasses) => 
            moduleMasses
                .Select(_model)
                .Sum();

        private decimal Complex(decimal mass)
        {
            decimal fuelForMass = CalculateFuelForMass(mass);
            if (fuelForMass >= 0)
            {
                fuelForMass += Complex(fuelForMass);
            }

            return fuelForMass > 0 ? fuelForMass : 0;
        }
        
        private decimal CalculateFuelForMass(decimal mass) => Math.Round(mass / 3, MidpointRounding.ToZero) - 2;
    }
}