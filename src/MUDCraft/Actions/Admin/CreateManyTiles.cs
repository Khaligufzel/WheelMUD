using ServiceStack;
using StarMUDium.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using WheelMUD.Core;

namespace StarMUDium.Actions.Admin
{
    [CoreExports.GameAction(200)]
    [ActionPrimaryAlias("createmanytiles", CommandCategory.Admin)]
    [ActionDescription("Temporary command to create 10k tiles.")]
    [ActionSecurity(SecurityRole.fullAdmin)]
    public class CreateManyTiles : GameAction
    {
        /// <summary>List of reusable guards which must be passed before action requests may proceed to execution.</summary>
        private static readonly List<CommonGuards> ActionGuards = new List<CommonGuards>
        {
        };


        public override void Execute(ActionInput actionInput)
        {
            var session = actionInput.Session;
            var actor = actionInput.Actor;


            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {

                    TileBehavior tileBehavior = new TileBehavior();


                    // TODO Check if input is valid
                    tileBehavior.SetLocation("testarea", x, y);


                    var tile = new Thing(tileBehavior)
                    {
                        Id = "tiles/" + tileBehavior.fullLocation

                    };

                    session.WriteLine(tile.Id);

                    tile.Save();
                    if (PlacesManager.Instance.World.Add(tile))
                    {
                        
                    }
                
                }

                
            }

            PlacesManager.Instance.World.Save();
        }

        public override string Guards(ActionInput actionInput)
        {
            return VerifyCommonGuards(actionInput, ActionGuards);
        }
    }
}
