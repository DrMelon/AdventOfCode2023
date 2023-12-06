using System.Collections.Generic;
using System.Linq;
using AOC2023.Common;
using Lutra.Graphics;
using Lutra.Utility;

namespace AOC2023.Scenes;

public class DayThreeScene : DayScene
{
    public DayThreeScene()
    {
        Day = 3;
    }
    
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
            result.CharactersLinear = new char[result.Width * result.Height];

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
        public List<Gear> Gears;
    }

    public struct Gear
    {
        public int GearX;
        public int GearY;
        public int GearRatio;
        public int PartNumberId;
    }

    public InputAsGrid InputGrid;
    private List<PartNumber> FoundParts;

    public override void Begin()
    {
        base.Begin();
        
        var Elf = new Image("img/elf_wrench.png");
        Elf.Scale = 2;
        Elf.X = 128;
        Elf.Y = Game.Height - 140;
        Elf.Dropshadow = 2;
        Elf.CenterOrigin();
        
        var PartPaper = new Image("img/parts.png");
        PartPaper.Scale = 2;
        PartPaper.X = Game.Width - 128;
        PartPaper.Y = Game.Height - 140;
        PartPaper.Dropshadow = 2;
        PartPaper.CenterOrigin();
        
        AddGraphic(Elf);
        AddGraphic(PartPaper);
    }

    public override void StarOne()
    {
        base.StarOne();
        
        // Load input as grid
        InputGrid = InputAsGrid.FromInput(Input);
        
        // Along each line, find groups of digits, to form part numbers.
        FoundParts = new();
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
                        currentPart.Gears = new();
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
                        FoundParts.Add(currentPart);
                        partNumberStarted = false;
                        Util.Log($"Part Number {currentPart.Number} found.");
                    }
                }
            }
            // End of line - if we're halfway through a number, finish it.
            if (partNumberStarted)
            {
                // End the current part number
                currentPart.Number = int.Parse(currentPartDigits);
                FoundParts.Add(currentPart);
                partNumberStarted = false;
                Util.Log($"Part Number {currentPart.Number} found.");

            }
            
        }
        
        // Then, for each part number, check the neighbours of each digit in that group to see if there are any symbols.
        for(int partIdx = 0; partIdx < FoundParts.Count; partIdx++)
        {
            PartNumber partNumber = FoundParts[partIdx];
            for (int i = 0; i < partNumber.LengthInDigits; i++)
            {
                int x = partNumber.XStartPosInGrid + i;
                int y = partNumber.YStartPosInGrid;

                if (HasNeighbourSymbol(x, y, ref partNumber, partIdx))
                {
                    Util.Log($"Part Number {partNumber.Number} has symbol found near {x}, {y}.");
                    partNumber.Valid = true;
                }
            }
            FoundParts[partIdx] = partNumber;
        }
        
        // If there is a symbol, it's a valid part number and should be kept for the sum. Otherwise, discard it.
        int validPartNumberSum = FoundParts.Where(partNumber => partNumber.Valid).Sum(partNumber => partNumber.Number);
        SimpleOutput.String = $"Sum of valid part numbers: {validPartNumberSum}";
    }

    public override void StarTwo()
    {
        base.StarTwo();

        // Run Star One first to populate FoundParts.
        StarOne();
        
        // Find Gears - '*' symbols with exactly 2 neighbouring valid part numbers.
        var allGears = FoundParts.SelectMany(number => number.Gears).ToArray();

        List<Gear> foundGears = new();

        // Find gears which share an X and Y but have different part numbers
        foreach(var gearA in allGears)
        {
            foreach(var gearB in allGears)
            {
                bool isUniqueGear = gearA.GearX == gearB.GearX && gearA.GearY == gearB.GearY &&
                                    gearA.PartNumberId != gearB.PartNumberId;

                if (foundGears.Any(gear => gear.GearX == gearA.GearX && gear.GearY == gearA.GearY))
                {
                    // Reject gears we've already seen.
                    continue;
                }
                
                if (isUniqueGear)
                {
                    if(FoundParts[gearA.PartNumberId].Valid && FoundParts[gearA.PartNumberId].Valid)
                    {
                        foundGears.Add(gearA with { GearRatio = FoundParts[gearA.PartNumberId].Number * FoundParts[gearB.PartNumberId].Number, PartNumberId = -1 });
                        Util.Log($"Found Gear: {gearA.GearX}, {gearA.GearY}. Ratio: {foundGears.Last().GearRatio}");
                    }
                }
            }
        }

        int gearSum = foundGears.Sum(gear => gear.GearRatio);
        SimpleOutput.String = $"Sum of all gear ratios: {gearSum}";
    }

    private bool HasNeighbourSymbol(int startX, int startY, ref PartNumber partNumber, int partNumberId)
    {
        for (int x = startX - 1; x <= startX + 1; x++)
        {
            for (int y = startY - 1; y <= startY + 1; y++)
            {
                char gotChar = InputGrid.GetCharAt(x, y);
                if (!CharHelpers.IsDigit(gotChar) && gotChar != '.' && gotChar != ' ')
                {
                    if (gotChar == '*')
                    {
                        if (partNumber.Gears == null)
                            partNumber.Gears = new();
                        
                        partNumber.Gears.Add(new Gear
                        {
                            GearRatio = 0,
                            GearX = x,
                            GearY = y,
                            PartNumberId = partNumberId,
                        });
                        
                    }
                    return true;
                }
            }
        }

        return false;
    }
}