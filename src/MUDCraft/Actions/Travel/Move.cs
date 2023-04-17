using StarMUDium.Behaviors;
using System.Collections.Generic;
using WheelMUD.Core;

namespace StarMUDium.Actions.Travel
{
    /// <summary>Main command for moving from tile to tile.</summary>
    /// <remarks>
    /// TODO: I don't think that's a good way of doing it...
    /// </remarks>
    [CoreExports.GameAction(0)]
    [ActionPrimaryAlias("north", CommandCategory.Travel)]
    [ActionAlias("n", CommandCategory.Item)]
    [ActionAlias("east", CommandCategory.Item)]
    [ActionAlias("e", CommandCategory.Item)]
    [ActionAlias("south", CommandCategory.Item)]
    [ActionAlias("s", CommandCategory.Item)]
    [ActionAlias("west", CommandCategory.Item)]
    [ActionAlias("w", CommandCategory.Item)]
    [ActionAlias("northeast", CommandCategory.Item)]
    [ActionAlias("ne", CommandCategory.Item)]
    [ActionAlias("southeast", CommandCategory.Item)]
    [ActionAlias("se", CommandCategory.Item)]
    [ActionAlias("southwest", CommandCategory.Item)]
    [ActionAlias("sw", CommandCategory.Item)]
    [ActionAlias("northwest", CommandCategory.Item)]
    [ActionAlias("nw", CommandCategory.Item)]
    [ActionDescription("Move to another tile.")]
    [ActionSecurity(SecurityRole.player | SecurityRole.mobile)]
    public class Move : GameAction
    {
        private int coordX;
        private int coordY;
        private string area;
        TileBehavior tileBehavior;
        Thing tile;




        /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
            CommonGuards.InitiatorMustBeAlive,
            CommonGuards.InitiatorMustBeConscious,
            CommonGuards.InitiatorMustBeBalanced,
            CommonGuards.InitiatorMustBeMobile,
        };








        /// <summary>Executes the command.</summary>
        /// <param name="actionInput">The full input specified for executing the command.</param>
        public override void Execute(ActionInput actionInput)
        {
            tileBehavior = actionInput.Actor.Parent.FindBehavior<TileBehavior>();
            tile = actionInput.Actor.Parent;

            coordX = tileBehavior.coordX;
            coordY = tileBehavior.coordY;
            area = tileBehavior.areaName;

            if (MoveThrough(actionInput))
            {
                CommandManager.Instance.EnqueueAction(new ActionInput("look", actionInput.Session, actionInput.Actor));
            }

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

            string destinationCoords = GetDestinationCoords(GetSecondaryExitAlias(actionInput.FullText));

            if (GetDestinationCoords(GetSecondaryExitAlias(actionInput.FullText)) == null)
            {
                actionInput.Session.WriteLine("Can't find coords.");
                return false;
            }

            else
            {
                if (ThingManager.Instance.FindThing("tiles/" + area + "/" + destinationCoords) != null)
                {
                    destination = ThingManager.Instance.FindThing("tiles/" + area + "/" + destinationCoords);
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
                ToOriginator = $"You move to {destination.Id}.",
                ToReceiver = $"{actionInput.Actor.Name} arrives.",
                ToOthers = $"{actionInput.Actor.Name} arrives.",
            };
            var leaveMessage = new SensoryMessage(SensoryType.Sight, 100, leaveContextMessage);
            var arriveMessage = new SensoryMessage(SensoryType.Sight, 100, arriveContextMessage);

            return movableBehavior.Move(destination, tile, leaveMessage, arriveMessage);
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

            return primaryExitCommand;
        }




        /// <summary>Checks against the guards for the command.</summary>
        /// <param name="actionInput">The full input specified for executing the command.</param>
        /// <returns>A string with the error message for the user upon guard failure, else null.</returns>
        public override string Guards(ActionInput actionInput)
        {
            var commonFailure = VerifyCommonGuards(actionInput, ActionGuards);
            if (commonFailure != null)
            {
                return commonFailure;
            }



            // If we got this far, we couldn't find an appropriate enterable thing in the room.
            return null;
        }
    }
}