//-----------------------------------------------------------------------------
// <copyright file="WRMRaces.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team. See LICENSE.txt. This file is
//   subject to the Microsoft Public License. All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using WheelMUD.Core;

namespace StarMUDium
{
    /// <summary>Class that represents the Human race.</summary>
    [ExportGameRace]
    public class HumanRace : StarMUDiumRace
    {
        /// <summary>Initializes a new instance of the <see cref="HumanRace"/> class.</summary>
        public HumanRace() : base(
            "Human",
            "Humans are you standard bipedal humanoid. They are born without any natural abilities, but they make up this shortcoming by being the most versatile race in this realm. They can pretty much learn anything.")
        {
        }
    }

    /// <summary>Class that represents the Elven race.</summary>
    [ExportGameRace]
    public class XorlanRace : StarMUDiumRace
    {
        /// <summary>Initializes a new instance of the <see cref="ElfRace"/> class.</summary>
        public XorlanRace() : base(
            "Xorlan",
            "The Xorlans are a race of humanoid aliens that have a translucent, jelly-like skin that is constantly changing colors. They communicate through a series of chirps and whistles that they make by vibrating the air inside their bodies. They are a highly empathic race and can sense the emotions of others through subtle vibrations in the air.")
        {
        }
    }

    /// <summary>Class that represents the Dwarven race.</summary>
    [ExportGameRace]
    public class QuentariRace : StarMUDiumRace
    {
        /// <summary>Initializes a new instance of the <see cref="DwarfRace"/> class.</summary>
        public QuentariRace() : base(
            "Quentari",
            "The Quentari are a race of humanoid aliens that have a highly developed sense of smell. Their nose is located in the center of their forehead, and they use it to navigate their surroundings and communicate with each other. They are a highly spiritual and philosophical race that values meditation and introspection.")
        {
        }
    }

    /// <summary>Class that represents the Halfling race.</summary>
    [ExportGameRace]
    public class ValtorianRace : StarMUDiumRace
    {
        /// <summary>Initializes a new instance of the <see cref="HalflingRace"/> class.</summary>
        public ValtorianRace() : base(
            "Valtorian",
            "The Valtorians are a race of humanoid aliens that have a metallic, reflective skin. They can manipulate the metal on their bodies, allowing them to change their shape and appearance at will. They are a highly adaptable and resourceful race, known for their ability to survive in even the harshest environments.")
        {
        }
    }

    /// <summary>Class that represents the Lizardmen race.</summary>
    [ExportGameRace]
    public class ZalvanRace : StarMUDiumRace
    {
        /// <summary>Initializes a new instance of the <see cref="LizardmanRace"/> class.</summary>
        public ZalvanRace() : base(
            "Zalvan",
            "The Zalvans are a race of humanoid aliens that have a symbiotic relationship with a small, insect-like creature that lives inside their bodies. The creature provides the Zalvans with heightened senses and enhanced reflexes, while the Zalvans provide the creature with a safe and nurturing environment. They are a fiercely loyal and protective race that values family above all else.")
        {
        }
    }

}