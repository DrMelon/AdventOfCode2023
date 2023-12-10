using System;
using System.Collections.Generic;
using System.Linq;
using Lutra.Utility;

namespace AOC2023.Scenes;

public class DayFiveScene : DayScene
{
    public DayFiveScene()
    {
        Day = 5;
    }

    public struct MappingRange
    {
        public long SourceStartIndex;
        public long DestinationStartIndex;
        public long Length;
    }

    public List<long> SeedList = new();
    public List<List<MappingRange>> Ranges = new();

    public void InterpretInput()
    {
        // First line of input is the list of seeds.
        SeedList = Input[0].Split(':')[1].Trim().Split(' ').Select(seedString => long.Parse(seedString)).ToList();
        Ranges = new();
        bool processingMap = false;
        List<MappingRange> currentRangeMap = new();
        for (int lineIdx = 1; lineIdx < Input.Count; lineIdx++)
        {
            string line = Input[lineIdx];
            if (line is "\n" or "")
            {
                // Blank line
                if (processingMap)
                {
                    processingMap = false;
                    
                    // Finish up current range group.
                    Ranges.Add(currentRangeMap);
                }
                continue;
            }
            else if (line.Contains("map"))
            {
                processingMap = true;
                currentRangeMap = new();
            }
            else
            {
                string[] rangeNumbers = line.Trim().Split(' '); 
                MappingRange newRange = new MappingRange
                {
                    DestinationStartIndex = long.Parse(rangeNumbers[0]),
                    SourceStartIndex = long.Parse(rangeNumbers[1]),
                    Length = long.Parse(rangeNumbers[2]),
                };
                currentRangeMap.Add(newRange);
            }
        }

        if (processingMap)
        {
            Ranges.Add(currentRangeMap); // Final range added
        }
    }

    public static long MapNumber(List<MappingRange> mappings, long number)
    {
        foreach (var mapping in mappings)
        {
            if (number >= mapping.SourceStartIndex && number < mapping.SourceStartIndex + mapping.Length)
            {
                long mappedNumber = mapping.DestinationStartIndex + (number - mapping.SourceStartIndex);
                
                return mappedNumber;
            }
        }

        return number;
    }
    
    public static long MapNumberInverted(List<MappingRange> mappings, long number)
    {
        foreach (var mapping in mappings)
        {
            if (number >= mapping.DestinationStartIndex && number < mapping.DestinationStartIndex + mapping.Length)
            {
                long mappedNumber = mapping.SourceStartIndex + (number - mapping.DestinationStartIndex);
                return mappedNumber;
            }
        }

        return number;
    }

    public override void StarOne()
    {
        base.StarOne();

        InterpretInput();

        long lowestLocationNumber = long.MaxValue;

        foreach (var seed in SeedList)
        {
            Util.Log($"Mapping seed {seed}:");
            long currentMappedNumber = seed;
            foreach (var mappingRanges in Ranges)
            {
                currentMappedNumber = MapNumber(mappingRanges, currentMappedNumber);
            }

            if (currentMappedNumber < lowestLocationNumber)
            {
                Util.Log($" * Lowest so far: {currentMappedNumber}");
                lowestLocationNumber = currentMappedNumber;
            }
        }

        SimpleOutput.String = $"Lowest Location Number: {lowestLocationNumber}";
    }

    public override void StarTwo()
    {
        base.StarTwo();
        
        InterpretInput();

        // Invert ranges
        Ranges.Reverse();
        long lowestLocationNumber = 0;
        bool earlyStop = false;
        for(long i = 0; i < long.MaxValue && !earlyStop; i++)
        {
            // Reverse map this number. 
            if (i % 1000000 == 0)
            {
                Util.Log($"Trying location {i}");
            }
            long currentMappedNumber = i;
            foreach (var mappingRanges in Ranges)
            {
                currentMappedNumber = MapNumberInverted(mappingRanges, currentMappedNumber);
            }
            
            // If it landed in the range of seeds given, then we're golden.
            for (int seedRange = 0; seedRange < SeedList.Count; seedRange+=2)
            {
                if (currentMappedNumber >= SeedList[seedRange] && currentMappedNumber < SeedList[seedRange] + SeedList[seedRange + 1])
                {
                    lowestLocationNumber = i;
                    earlyStop = true;
                    break;
                }
            }
        }

        SimpleOutput.String = $"Lowest Location Number in Seed Ranges: {lowestLocationNumber}";
        
    }
}