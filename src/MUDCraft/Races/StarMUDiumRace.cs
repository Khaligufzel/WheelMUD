//-----------------------------------------------------------------------------
// <copyright file="WRMRace.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team. See LICENSE.txt. This file is
//   subject to the Microsoft Public License. All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using WheelMUD.Core;

namespace StarMUDium
{
    /// <summary>
    /// WRM-specific race class; implements race-related details specific to the WRM game system.
    /// Provides a base class for individual WRM races (which may have their own specific event handlers and such).
    /// </summary>
    public class StarMUDiumRace : GameRace
    {
        /// <summary>Initializes a new instance of the <see cref="StarMUDiumRace"/> class.</summary>
        /// <param name="name">The name of the race.</param>
        /// <param name="description">The description of the race.</param>
        public StarMUDiumRace(string name, string description) : base()
        {
            Name = name;
            Description = description;
        }

    }
}