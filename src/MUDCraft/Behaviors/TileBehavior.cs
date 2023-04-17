using System;
using System.Collections.Generic;
using System.Linq;
using WheelMUD.Core;

namespace StarMUDium.Behaviors
{
    public class TileBehavior : Behavior
    {
        private readonly MoveFromTileBehaviorCommands commands;

        public string tileSymbol { get; private set; }
        public int coordX { get; private set; }
        public int coordY { get; private set; }
        public string fullLocation { get; private set; }
        public string areaName { get; private set; }
        public TileBehavior()
            : base(null)
        {
            commands = new MoveFromTileBehaviorCommands(this);
        }

        public TileBehavior(string areaName, int coordX, int coordY, Dictionary<string, object> instanceProperties) : base(instanceProperties)
        {
            commands = new MoveFromTileBehaviorCommands(this);
        }

        public enum Terrain
        {
            Grass,
            Wall
        }


        public Terrain terrain { get; private set; }

        public void SetTerrain(Terrain terrain)
        {
            Parent.Name = terrain.ToString();
            this.terrain = terrain;
            tileSymbol = SetUpSymbol(terrain);
            
        }

        public void SetLocation(string areaName, int coordX, int coordY)
        {
            this.areaName = areaName;
            this.coordX = coordX;
            this.coordY = coordY;
            fullLocation = areaName + "/" + coordX + "/" + coordY;
        }

        private string SetUpSymbol(Terrain terrain)
        {
            switch (terrain)
            {
                case Terrain.Grass: return "<%green%>,<%n%>";
                case Terrain.Wall: return "#";
            }


            return ".";
        }


        public bool MoveThrough(ActionInput actionInput)
        {
            Thing destination;

            actionInput.Session.WriteLine("Trying to move.");

            // If the thing isn't currently mobile, bail.
            var movableBehavior = actionInput.Actor.FindBehavior<MovableBehavior>();
            if (movableBehavior == null)
            {
                // TODO: Add messaging to thingToMove?
                return false;
            }


            if (GetDestinationCoords(actionInput.FullText) == null)
            {
                actionInput.Session.WriteLine("Can't find coords.");
                return false;
            }

            else
            {
                if(ThingManager.Instance.FindThing("tiles/" + areaName + "/" + GetDestinationCoords(actionInput.FullText)) != null)
                {
                    destination = ThingManager.Instance.FindThing("tiles/" + areaName + "/" + GetDestinationCoords(actionInput.FullText));
                }

                else
                {
                    return false;
                }
            }




            var leaveContextMessage = new ContextualString(actionInput.Actor, actionInput.Actor.Parent)
            {
                ToOriginator = null,
                ToReceiver = $"{actionInput.Actor.Name} moves.",
                ToOthers = $"{actionInput.Actor.Name} moves.",
            };
            var arriveContextMessage = new ContextualString(actionInput.Actor, destination)
            {
                ToOriginator = $"You move.",
                ToReceiver = $"{actionInput.Actor.Name} arrives.",
                ToOthers = $"{actionInput.Actor.Name} arrives.",
            };
            var leaveMessage = new SensoryMessage(SensoryType.Sight, 100, leaveContextMessage);
            var arriveMessage = new SensoryMessage(SensoryType.Sight, 100, arriveContextMessage);

            return movableBehavior.Move(destination, Parent, leaveMessage, arriveMessage);
        }

        public static readonly Dictionary<string, string> PrimaryToSecondaryCommandMap = new Dictionary<string, string>()
        {
            { "north", "n" },
            { "northeast", "ne" },
            { "east", "e" },
            { "southeast", "se" },
            { "south", "s" },
            { "southwest", "sw" },
            { "west", "w" },
            { "northwest", "nw" },
            { "up", "u" },
            { "down", "d" },
            { "enter", "en" },
            { "exit", "ex" }
        };

        private string GetSecondaryExitAlias(string primaryExitCommand)
        {
            switch (primaryExitCommand)
            {
                case "north": return "n";
                case "east": return "e";
                case "south": return "s";
                case "west": return "w";
                case "northeast": return "ne";
                case "southeast": return "se";
                case "southwest": return "sw";
                case "northwest": return "nw";
                case "up": return "u";
                case "down": return "d";
            }

            return null;
        }

        private string GetDestinationCoords(string moveCommandToCoords)
        {
            switch (moveCommandToCoords)
            {
                case "n": return coordX + "/" + (coordY + 1);
                case "e": return (coordX + 1) + "/" + coordY;
                case "s": return coordX + "/" + (coordY - 1);
                case "w": return (coordX - 1) + "/" + coordY;
                case "ne": return (coordX + 1) + "/" + (coordY + 1);
                case "se": return (coordX + 1) + "/" + (coordY - 1);
                case "sw": return (coordX - 1) + "/" + (coordY - 1);
                case "nw": return (coordX - 1) + "/" + (coordY + 1);
            }

            return null;
        }

        protected override void SetDefaultProperties()
        {

        }



        private class MoveFromTileBehaviorCommands : GameAction
        {
            /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
            private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
            {
                CommonGuards.InitiatorMustBeAlive,
                CommonGuards.InitiatorMustBeConscious,
                CommonGuards.InitiatorMustBeBalanced,
                CommonGuards.InitiatorMustBeMobile
            };

            /// <summary>The ExitBehavior this class belongs to.</summary>
            private readonly TileBehavior tileBehavior;

            /// <summary>Initializes a new instance of the ExitBehaviorCommands class.</summary>
            /// <param name="tileBehavior">The ExitBehavior this class belongs to.</param>
            public MoveFromTileBehaviorCommands(TileBehavior tileBehavior)
                : base()
            {
                this.tileBehavior = tileBehavior;
            }

            /// <summary>Execute the action.</summary>
            /// <param name="actionInput">The full input specified for executing the command.</param>
            public override void Execute(ActionInput actionInput)
            {             
                tileBehavior.MoveThrough(actionInput);
            }

            /// <summary>Checks against the guards for the command.</summary>
            /// <param name="actionInput">The full input specified for executing the command.</param>
            /// <returns>A string with the error message for the user upon guard failure, else null.</returns>
            public override string Guards(ActionInput actionInput)
            {
                string commonFailure = VerifyCommonGuards(actionInput, ActionGuards);
                if (commonFailure != null)
                {
                    return commonFailure;
                }

                if (actionInput.Actor.FindBehavior<MovableBehavior>() == null)
                {
                    return "You do not have the ability to move.";
                }

                return null;
            }
        }
    }


}
