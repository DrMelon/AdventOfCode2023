using System;
using System.Collections.Generic;
using System.Linq;
using Lutra.Graphics;

namespace AOC2023.Scenes;

public class DayFourScene : DayScene
{
    public DayFourScene()
    {
        Day = 4;
    }

    public struct ScratchCard
    {
        public List<int> WinningNumbers;
        public List<int> PlayedNumbers;
        public int StackSize;

        public int CountScore()
        {
            int matches = CountMatches();
            int score = 0;
            if (matches > 0)
            {
                score = 1; // Add a point for first match.
                if (matches > 1)
                {
                    score <<= (matches - 1); // Double score for each match past the first.
                }
            }

            return score;
        }

        public int CountMatches()
        {
            return PlayedNumbers.Intersect(WinningNumbers).Count();
        }

        public static ScratchCard FromInputLine(string line)
        {
            // First split out "Game X" string.
            var gameLine = line.Split(":")[1].Trim();
            
            // Divide into two using the '|' character.
            // Ensure double-whitespaces are removed.
            var splitGame = gameLine.Split('|');
            var winningNumbers = splitGame[0].Trim().Replace("  ", " "); 
            var playedNumbers = splitGame[1].Trim().Replace("  ", " ");


            ScratchCard newCard = new ScratchCard()
            {
                WinningNumbers = winningNumbers.Split(' ').Select(num => int.Parse(num)).ToList(),
                PlayedNumbers = playedNumbers.Split(' ').Select(num => int.Parse(num)).ToList(),
                StackSize = 1
            };

            return newCard;
        }
    }

    public override void Begin()
    {
        base.Begin();
        
        var Elf = new Image("img/elf_scratch.png");
        Elf.Scale = 2;
        Elf.X = 128;
        Elf.Y = Game.Height - 140;
        Elf.Dropshadow = 2;
        Elf.CenterOrigin();
        
        AddGraphic(Elf);
    }

    public override void StarOne()
    {
        base.StarOne();

        List<ScratchCard> ScratchCards = new();
        
        foreach (var line in Input)
        {
            // Get scratchcard for each line.
            ScratchCards.Add(ScratchCard.FromInputLine(line));
        }

        int sumOfPointsFromCards = ScratchCards.Sum(scratchCard => scratchCard.CountScore());

        SimpleOutput.String = $"Scratch cards total score: {sumOfPointsFromCards}";
    }

    public override void StarTwo()
    {
        base.StarTwo();
        
        List<ScratchCard> ScratchCards = new();
        
        foreach (var line in Input)
        {
            // Get scratchcard for each line.
            ScratchCards.Add(ScratchCard.FromInputLine(line));
        }
        
        // For each scratchcard, count its matches. Increase the stack size of the next X cards by one if it's a match.
        // Increase the stack size by the current stack size - because if there are 3 copies of this card and each copy wins for the next 2,
        // then the stack size of the next 2 cards must increase by 3 each. 
        for (int scratchCardIndex = 0; scratchCardIndex < ScratchCards.Count; scratchCardIndex++)
        {
            int matches = ScratchCards[scratchCardIndex].CountMatches();
            int currentStackSize = ScratchCards[scratchCardIndex].StackSize;

            for (int nextCardIndex = 0; nextCardIndex < matches; nextCardIndex++)
            {
                // Increase stack size.
                var nextCard = ScratchCards[scratchCardIndex + nextCardIndex + 1];
                nextCard.StackSize += currentStackSize;
                ScratchCards[scratchCardIndex + nextCardIndex + 1] = nextCard;
            }
        }
        
        // Now count up all cards by summing stacksizes.
        int totalCardNumber = ScratchCards.Sum(card => card.StackSize);
        SimpleOutput.String = $"Total Card Count: {totalCardNumber}";

    }
}