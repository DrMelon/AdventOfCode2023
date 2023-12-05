using System.Collections.Generic;
using AOC2023.Common;
using Lutra.Utility;

namespace AOC2023.Scenes;

public class DayThreeScene : DayScene
{
    public struct InputAsGrid
    {
        public char[] CharactersLinear;
        public int Width;
        public int Height;

        public char GetCharAt(int x, int y)
        {
            if (x > Width - 1 || y > Height - 1 || x < 0 || y < 0)
            {
                return ' ';
            }

            return CharactersLinear[Util.OneDee(Width, x, y)];
        }
        
        public char GetCharAtWrapped(int x, int y)
        {
            x = x % Width;
            y = y % Height;

            return CharactersLinear[Util.OneDee(Width, x, y)];
        }

        public void SetCharAt(int x, int y, char ch)
        {
            x = x % Width;
            y = y % Height;

            CharactersLinear[Util.OneDee(Width, x, y)] = ch;
        }

        public static InputAsGrid FromInput(List<string> input)
        {
            InputAsGrid result = new InputAsGrid();
            result.Width = input[0].Trim().Length;
            result.Height = input.Count;

            for(int y = 0; y < input.Count; y++)
            {
                string line = input[y].Trim();
                for(int x = 0; x < line.Length; x++)
                {
                    result.SetCharAt(x, y, line[x]);
                }
            }

            return result;
        }
    }

    public struct PartNumber
    {
        public bool Valid;
        public int Number;
        public int XStartPosInGrid, YStartPosInGrid;
        public int LengthInDigits;
    }

    public InputAsGrid InputGrid;

    public override void StarOne()
    {
        base.StarOne();
        
        // Load input as grid
        InputGrid = InputAsGrid.FromInput(Input);
        
        // Along each line, find groups of digits, to form part numbers.
        List<PartNumber> foundParts = new();
        for (int lineNumber = 0; lineNumber < InputGrid.Height; lineNumber++)
        {
            bool partNumberStarted = false;
            PartNumber currentPart = new();
            string currentPartDigits = "";
            for (int x = 0; x < InputGrid.Width; x++)
            {
                // Scan for digits.
                char currentScan = InputGrid.GetCharAt(x, lineNumber);
                bool isDigit = CharHelpers.IsDigit(currentScan);

                if (isDigit)
                {
                    if (!partNumberStarted)
                    {
                        partNumberStarted = true;
                        currentPart = new PartNumber();
                        currentPart.XStartPosInGrid = x;
                        currentPart.YStartPosInGrid = lineNumber;
                        currentPartDigits = "";
                    }

                    currentPartDigits += currentScan;
                    currentPart.LengthInDigits++;
                }
                else
                {
                    if (partNumberStarted)
                    {
                        // End the current part number
                        currentPart.Number = int.Parse(currentPartDigits);
                        foundParts.Add(currentPart);
                        partNumberStarted = false;
                    }
                }
            }
            // End of line - if we're halfway through a number, finish it.
            if (partNumberStarted)
            {
                // End the current part number
                currentPart.Number = int.Parse(currentPartDigits);
                foundParts.Add(currentPart);
                partNumberStarted = false;
            }
            
        }
        
        // Then, for each part number, check the neighbours of each digit in that group to see if there are any symbols.
        foreach (var partNumber in foundParts)
        {
            
        }
        // If there is a symbol, it's a valid part number and should be kept for the sum. Otherwise, discard it.
    }

    public override void StarTwo()
    {
        base.StarTwo();
    }
}