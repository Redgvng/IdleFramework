using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary;

namespace Pickers.Domain
{
    public class Picker
    {
        public Parameter speed, inhalePower;
        public Picker(double initial_speed, double initial_inhalePower)
        {
            speed = new Parameter(initial_speed);
            inhalePower = new Parameter(initial_inhalePower);
        }

        public double Speed() => speed.Number;
        public double InhalePower() => inhalePower.Number;
    }
}