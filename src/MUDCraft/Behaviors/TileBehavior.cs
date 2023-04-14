using System;
using System.Collections.Generic;
using WheelMUD.Core;

namespace StarMUDium.Behaviors
{
    public class TileBehavior : Behavior
    {
        public int coordX { get; private set; }
        public int coordY { get; private set; }
        public string fullLocation { get; private set; }
        public string areaName { get; private set; }
        public TileBehavior()
            : base(null)
        {
        }

        public TileBehavior(string areaName, int coordX, int coordY, Dictionary<string, object> instanceProperties) : base(instanceProperties)
        {

        }

        public void SetLocation(string areaName, int coordX, int coordY)
        {
            this.areaName = areaName;
            this.coordX = coordX;  
            this.coordY = coordY;
            fullLocation = areaName + "/" + coordX + "/" + coordY;
        }

        protected override void SetDefaultProperties()
        {
            
        }
    }
}
