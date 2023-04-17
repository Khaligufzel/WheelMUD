//-----------------------------------------------------------------------------
// <copyright file="DefaultPerceivedRoomRenderer.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team.  See LICENSE.txt.  This file is
//   subject to the Microsoft Public License.  All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using StarMUDium.Behaviors;
using System.Linq;
using WheelMUD.Server;

namespace WheelMUD.Core
{
    [RendererExports.PerceivedRoom(200)]
    public class StarMUDiumPerceivedTileRenderer : RendererDefinitions.PerceivedRoom
    {
        private readonly int xMinimapLength = 11;
        private readonly int yMinimapLength = 11;

        public override OutputBuilder Render(TerminalOptions terminalOptions, Thing viewer, Thing tile)
        {
            var senses = viewer.FindBehavior<SensesBehavior>();
            var output = new OutputBuilder();
            if (senses.CanPerceiveThing(tile))
            {
                //output.AppendLine($"<%red%><%b%>{tile.Name}<%n%>");
                //output.AppendLine(tile.Description);

                TileBehavior tileBehavior = tile.FindBehavior<TileBehavior>();


                // Minimap generation
                for (int y = 0; y < yMinimapLength; y++)
                {
                    for (int x = 0; x < xMinimapLength; x++)
                    {
                        if(ThingManager.Instance.FindThing("tiles/" + tileBehavior.areaName + "/" + (tileBehavior.coordX - 5 + x) + "/" + (tileBehavior.coordY + 5 - y)) != null)
                        {
                            if (x == 5 && y == 5)
                            {
                                output.Append("&");
                            }
                            else
                            {
                                //output.Append(tileBehavior.tileSymbol);
                                output.Append(tileBehavior.tileSymbol);
                            }
                        }

                        else
                        {
                            output.Append(" ");
                        }

                        //output.Append("x");
                    }

                    output.AppendLine();
                }
            }
            else
            {
                //output.AppendLine("You cannot perceive much of note about the room.");
            }

            // TODO: Perhaps group things in the room by things you can pick up, things that are alive, etc?
            //   var entities = senses.PerceiveEntities();  and also render players nicely; "(AFK)" etc?
            //   var items = senses.PerceiveItems();        and also track tick or build sense-specific strings (like hearing only while blind...)

            // Handle exits differently from other Thing types
            // TODO: Also render closable exits like doors nicely; "(closed)"?
            // TODO: For viewer that is PlayerBehavior with negotiated MXP connection, render with embedded command links for click-to-execute support?
            var outputExits = from exit in senses.PerceiveExits() select $"<%magenta%>{exit}<%n%>";

            // TODO: Color the parts of the thing names which are also legit keywords for the thing...
            // TODO: For viewer that is PlayerBehavior with negotiated MXP connection, render with embedded command links for click-to-execute support?
            var outputThings = from thing in tile.Children
                               where senses.CanPerceiveThing(thing) && thing != viewer && !thing.HasBehavior<ExitBehavior>()
                               select $"  {thing.FullName}<%n%>";

            if (outputExits.Any() || outputThings.Any())
            {
                output.AppendLine("<%yellow%>Here you notice:<%n%>");
                if (outputExits.Any())
                {
                    output.AppendLine($"  Routes: {string.Join(", ", outputExits)}");
                }
                foreach (var outputThing in outputThings)
                {
                    output.AppendLine(outputThing);
                }
            }
            else
            {
                output.AppendLine("<%yellow%>TEST<%n%>");
            }

            return output;
        }
    }
}
