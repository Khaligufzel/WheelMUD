//-----------------------------------------------------------------------------
// <copyright file="DefaultPerceivedRoomRenderer.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team.  See LICENSE.txt.  This file is
//   subject to the Microsoft Public License.  All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using StarMUDium.Behaviors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelMUD.Server;

namespace WheelMUD.Core
{
    [RendererExports.PerceivedRoom(200)]
    public class StarMUDiumPerceivedTileRenderer : RendererDefinitions.PerceivedRoom
    {
        private readonly int xMinimapLength = 11;
        private readonly int yMinimapLength = 11;

        List<string> minimapLines = new List<string>();
        List<string> descriptionLines = new List<string>();

        private string temporaryText = "Welcome to your new favorite MUD text-based game! In this game, you will embark on an exciting adventure filled with crafting and space exploration. The focus of the game";

        public override OutputBuilder Render(TerminalOptions terminalOptions, Thing viewer, Thing tile)
        {
            var senses = viewer.FindBehavior<SensesBehavior>();
            var output = new OutputBuilder();
            if (senses.CanPerceiveThing(tile))
            {
                //output.AppendLine($"<%red%><%b%>{tile.Name}<%n%>");
                //output.AppendLine(tile.Description);

                TileBehavior tileBehavior = tile.FindBehavior<TileBehavior>();

                //List<string> outputLines = GenerateTwoColumns(GenerateMinimap(tileBehavior), temporaryText);
                //List<string> outputLines = GenerateTwoColumns(GenerateMinimap(tileBehavior), temporaryText);

                //foreach (string line in outputLines)
                //{
                //    output.AppendLine(line);
                //}

                minimapLines = GenerateMinimapList(tileBehavior);
                descriptionLines = WrapLines(temporaryText);


                if (minimapLines.Count >= descriptionLines.Count)
                {
                    for (int i = 0; i < minimapLines.Count; i++)
                    {
                        if (i < descriptionLines.Count)
                        {
                            minimapLines[i] += descriptionLines[i];



                        }

                        output.AppendLine(minimapLines[i]);
                    }
                }

                else
                {
                    for (int i = 0; i < descriptionLines.Count; i++)
                    {
                        if (i < minimapLines.Count)
                        {
                            minimapLines[i] += descriptionLines[i];


                            output.AppendLine(minimapLines[i]);
                        }
                        else
                        {
                            output.AppendLine("              " + descriptionLines[i]);
                        }
                        
                    }
                }

                //output.AppendLine(temporaryText);
                //output.AppendLine(GenerateTwoColumnOutput(GenerateMinimap(tileBehavior), temporaryText));

                // Minimap generation
                //for (int y = 0; y < yMinimapLength; y++)
                //{
                //    for (int x = 0; x < xMinimapLength; x++)
                //    {
                //        if(ThingManager.Instance.FindThing("tiles/" + tileBehavior.areaName + "/" + (tileBehavior.coordX - 5 + x) + "/" + (tileBehavior.coordY + 5 - y)) != null)
                //        {

                //            // In the middle of the minimap we should see character symbol
                //            if (x == 5 && y == 5)
                //            {
                //                output.Append("&");
                //            }
                //            else
                //            {
                //                //output.Append(tileBehavior.tileSymbol);
                //                output.Append(tileBehavior.tileSymbol);
                //            }
                //        }

                //        else
                //        {
                //            output.Append(" ");
                //        }

                //        //output.Append("x");
                //    }

                //    output.AppendLine("<%yellow%> | <%n%>");
                //}

                //for (int y = 0; y < yMinimapLength; y++)
                //{
                //    for (int x = 0; x < xMinimapLength; x++)
                //    {
                //        if (ThingManager.Instance.FindThing("tiles/" + tileBehavior.areaName + "/" + (tileBehavior.coordX - 5 + x) + "/" + (tileBehavior.coordY + 5 - y)) != null)
                //        {

                //            // In the middle of the minimap we should see character symbol
                //            if (x == 5 && y == 5)
                //            {
                //                output.Append("&");
                //            }
                //            else
                //            {
                //                //output.Append(tileBehavior.tileSymbol);
                //                output.Append(tileBehavior.tileSymbol);
                //            }
                //        }

                //        else
                //        {
                //            output.Append(" ");
                //        }

                //        //output.Append("x");
                //    }

                //    output.AppendLine("<%yellow%> | <%n%>");
                //}
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

        public static List<string> WrapLines(string input)
        {
            List<string> output = new List<string>();

            string[] words = input.Split(' ');
            int wordCount = 0;
            string line = "";

            foreach (string word in words)
            {
                if (wordCount == 6)
                {
                    output.Add(line);
                    line = "";
                    wordCount = 0;
                }

                if (line == "")
                {
                    line = word;
                    wordCount = 1;
                }
                else
                {
                    line += " " + word;
                    wordCount++;
                }
            }

            if (line != "")
            {
                output.Add(line);
            }

            return output;
        }

        //private List<string> GenerateTwoColumns(string minimap, string description)
        //{

        //}

        private string GenerateMinimap(TileBehavior tileBehavior)
        {
            string miniMap = "";

            for (int y = 0; y < yMinimapLength; y++)
            {
                for (int x = 0; x < xMinimapLength; x++)
                {
                    if (ThingManager.Instance.FindThing("tiles/" + tileBehavior.areaName + "/" + (tileBehavior.coordX - 5 + x) + "/" + (tileBehavior.coordY + 5 - y)) != null)
                    {

                        // In the middle of the minimap we should see character symbol
                        if (x == 5 && y == 5)
                        {
                            miniMap += "&";
                        }
                        else
                        {
                            //output.Append(tileBehavior.tileSymbol);
                            miniMap += tileBehavior.tileSymbol;
                        }
                    }

                    else
                    {
                        miniMap += " ";
                    }

                    //output.Append("x");
                }

                miniMap += "<%yellow%> | <%n%>";


            }

            return miniMap;
        }

        private List<string> GenerateMinimapList(TileBehavior tileBehavior)
        {
            List<string> lines = new List<string>();

            

            for (int y = 0; y < yMinimapLength; y++)
            {
                string miniMap = "";
                for (int x = 0; x < xMinimapLength; x++)
                {
                    if (ThingManager.Instance.FindThing("tiles/" + tileBehavior.areaName + "/" + (tileBehavior.coordX - 5 + x) + "/" + (tileBehavior.coordY + 5 - y)) != null)
                    {

                        // In the middle of the minimap we should see character symbol
                        if (x == 5 && y == 5)
                        {
                            miniMap += "&";
                        }
                        else
                        {
                            //output.Append(tileBehavior.tileSymbol);
                            miniMap += tileBehavior.tileSymbol;
                        }
                    }

                    else
                    {
                        miniMap += " ";
                    }

                    
                }

                miniMap += "<%yellow%> | <%n%>";
                lines.Add(miniMap);

            }

            return lines;
        }







        //public static string GenerateTwoColumnsWrapped(string column1, string column2, int wordsPerLine, int column1Width, int column2Width)
        //{
        //    string[] words1 = column1.Split(' ');
        //    string[] words2 = column2.Split(' ');

        //    string line1 = "";
        //    string line2 = "";
        //    string output = "";

        //    for (int i = 0; i < words1.Length || i < words2.Length; i++)
        //    {
        //        if (i < words1.Length)
        //        {
        //            if (line1.Length + words1[i].Length > column1Width)
        //            {
        //                output += line1.PadRight(column1Width) + "  " + line2.PadRight(column2Width) + Environment.NewLine;
        //                line1 = "";
        //                line2 = "";
        //            }
        //            line1 += words1[i] + " ";
        //        }

        //        if (i < words2.Length)
        //        {
        //            if (line2.Length + words2[i].Length > column2Width)
        //            {
        //                output += line1.PadRight(column1Width) + "  " + line2.PadRight(column2Width) + Environment.NewLine;
        //                line1 = "";
        //                line2 = "";
        //            }
        //            line2 += words2[i] + " ";
        //        }
        //    }

        //    output += line1.PadRight(column1Width) + "  " + line2.PadRight(column2Width);

        //    Console.WriteLine($"Words1: {string.Join(", ", words1)}");
        //    Console.WriteLine($"Words2: {string.Join(", ", words2)}");
        //    Console.WriteLine($"Output: {output}");

        //    return output;
        //}

        //public static string GenerateTwoColumnsWrapped(string column1, string column2, int wordsPerLine, int column1Width, int column2Width)
        //{
        //    // Pad the first column string with spaces to ensure it is 14 characters long
        //    column1 = column1.PadRight(14);

        //    List<string> list1 = new List<string>();
        //    List<string> list2 = new List<string>();

        //    // Split the input strings into separate minimapLines
        //    using (StringReader reader1 = new StringReader(column1))
        //    using (StringReader reader2 = new StringReader(column2))
        //    {
        //        string line1 = null, line2 = null;
        //        while (line1 != null || line2 != null)
        //        {
        //            line1 = reader1.ReadLine();
        //            line2 = reader2.ReadLine();
        //            list1.Add(line1 ?? "");
        //            list2.Add(line2 ?? "");
        //        }
        //    }

        //    int maxLength = Math.Max(list1.Count, list2.Count);

        //    StringBuilder output2 = new StringBuilder();
        //    // Iterate through each row and output the two columns side by side
        //    for (int i = 0; i < maxLength; i++)
        //    {
        //        string item1 = (i < list1.Count) ? WrapText(list1[i], wordsPerLine) : "";
        //        string item2 = (i < list2.Count) ? WrapText(list2[i], wordsPerLine) : "";

        //        string[] lines1 = item1.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        //        string[] lines2 = item2.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        //        for (int j = 0; j < Math.Max(lines1.Length, lines2.Length); j++)
        //        {
        //            string line1 = (j < lines1.Length) ? lines1[j] : "";
        //            string line2 = (j < lines2.Length) ? lines2[j] : "";

        //            output2.Append(line1.PadRight(column1Width));
        //            output2.Append(line2.PadRight(column2Width));
        //            output2.AppendLine();
        //        }
        //    }

        //    return output2.ToString();
        //}



    }
}
