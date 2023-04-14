using ServiceStack;
using StarMUDium.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using WheelMUD.Core;

namespace StarMUDium.Actions.Admin
{
    [CoreExports.GameAction(200)]
    [ActionPrimaryAlias("createtile", CommandCategory.Admin)]
    [ActionDescription("Temporary command to create a tile for testing.")]
    [ActionSecurity(SecurityRole.fullAdmin)]
    public class CreateTile : GameAction
    {
        /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
            CommonGuards.RequiresAtLeastThreeArguments
        };


        public override void Execute(ActionInput actionInput)
        {
            var session = actionInput.Session;
            var actor = actionInput.Actor;

            // Remove "weapon" from input tail and use the rest as the name.
            //var tileName = actionInput.Tail[6..].Trim().ToLower();

            TileBehavior tileBehavior = new TileBehavior();

            tileBehavior.SetLocation(actionInput.Params[0], actionInput.Params[1].ToInt(), actionInput.Params[2].ToInt());


            var tile = new Thing(tileBehavior)
            {
                //Name = weaponName,
                //SingularPrefix = "a",
                Id = "tiles/"+tileBehavior.fullLocation

            };

            session.WriteLine(tile.Id);

            tile.Save();
            //PlacesManager.Instance.World.Add(tile);

        }

        public override string Guards(ActionInput actionInput)
        {
            return VerifyCommonGuards(actionInput, ActionGuards);
        }
    }
}
