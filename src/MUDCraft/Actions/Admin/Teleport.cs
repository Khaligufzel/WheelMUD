using StarMUDium.Behaviors;
using System;
using System.Collections.Generic;
using WheelMUD.Core;

namespace StarMUDium.Actions.Admin
{
    [CoreExports.GameAction(200)]
    [ActionPrimaryAlias("teleport", CommandCategory.Admin)]
    [ActionDescription("Teleport to a tile. Area x y")]
    [ActionExample("teleport mars 10 10")]
    [ActionSecurity(SecurityRole.fullAdmin)]
    public class Teleport : GameAction
    {
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
            CommonGuards.RequiresAtLeastThreeArguments
        };


        public override void Execute(ActionInput actionInput)
        {
            var session = actionInput.Session;
            if (session == null) return; // This action only makes sense for player sessions.

            // If input is a simple number, assume we mean a room
            // TODO: IMPROVE TARGETING!
            //var targetPlace = int.TryParse(actionInput.Tail, out var roomNum) ? ThingManager.Instance.FindThing("rooms/" + roomNum) : ThingManager.Instance.FindThingByName(actionInput.Tail, false, true);

            var targetPlace = ThingManager.Instance.FindThing("tiles/" + actionInput.Params[0] + "/" + actionInput.Params[1] + "/" + actionInput.Params[2]);



            var movableBehavior = actionInput.Actor.FindBehavior<MovableBehavior>();

            var adminName = actionInput.Actor.Name;

            var leaveContextMessage = new ContextualString(actionInput.Actor, actionInput.Actor.Parent)
            {
                ToOriginator = null,
                ToReceiver = $"{adminName} disappears into nothingness.",
                ToOthers = $"{adminName} disappears into nothingness.",
            };
            var arriveContextMessage = new ContextualString(actionInput.Actor, targetPlace)
            {
                ToOriginator = $"You teleported to {targetPlace.Id}.",
                ToReceiver = $"{adminName} appears from nothingness.",
                ToOthers = $"{adminName} appears from nothingness.",
            };
            var leaveMessage = new SensoryMessage(SensoryType.Sight, 100, leaveContextMessage);
            var arriveMessage = new SensoryMessage(SensoryType.Sight, 100, arriveContextMessage);




            if (movableBehavior != null && movableBehavior.Move(targetPlace, actionInput.Actor, leaveMessage, arriveMessage))
            {

            }
                if (targetPlace == null)
                {
                    session.WriteLine("Tile or Entity not found.");
                    return;
                }

                //if (targetPlace.FindBehavior<TileBehavior>() == null)
                //{
                //    // If the target's parent is a room, go there instead
                //    if (targetPlace.Parent != null && targetPlace.Parent.FindBehavior<TileBehavior>() != null)
                //    {
                //        targetPlace = targetPlace.Parent;
                //    }
                //    else
                //    {
                //        session.WriteLine("Target is not a room and is not in a room!");
                //        return;
                //    }
                //}

                //var adminName = actionInput.Actor.Name;
                //var leaveContextMessage = new ContextualString(actionInput.Actor, actionInput.Actor.Parent)
                //{
                //    ToOriginator = null,
                //    ToReceiver = $"{adminName} disappears into nothingness.",
                //    ToOthers = $"{adminName} disappears into nothingness.",
                //};
                //var arriveContextMessage = new ContextualString(actionInput.Actor, targetPlace)
                //{
                //    ToOriginator = $"You teleported to {targetPlace.Name}.",
                //    ToReceiver = $"{adminName} appears from nothingness.",
                //    ToOthers = $"{adminName} appears from nothingness.",
                //};
                //var leaveMessage = new SensoryMessage(SensoryType.Sight, 100, leaveContextMessage);
                //var arriveMessage = new SensoryMessage(SensoryType.Sight, 100, arriveContextMessage);

                // If we successfully move (IE the move may get canceled if the user doesn't have permission
                // to enter a particular location, some other behavior cancels it, etc), then perform a 'look'
                // command to get immediate feedback about the new location.
                // TODO: This should not 'enqueue' a command since, should the player have a bunch of 
                //     other commands entered, the 'look' feedback will not immediately accompany the 'goto' 
                //     command results like it should.

                //var movableBehavior = actionInput.Actor.FindBehavior<MovableBehavior>();

                //if (movableBehavior != null && movableBehavior.Move(targetPlace, actionInput.Actor, leaveMessage, arriveMessage))
                //{
                //    CommandManager.Instance.EnqueueAction(new ActionInput("look", actionInput.Session, actionInput.Actor));
                //}
            
        }
        public override string Guards(ActionInput actionInput)
        {
            return VerifyCommonGuards(actionInput, ActionGuards);
        }


    }
}
