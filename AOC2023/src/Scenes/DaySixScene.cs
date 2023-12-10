using System;
using System.Collections.Generic;
using System.Linq;
using Lutra.Utility;

namespace AOC2023.Scenes;

public class DaySixScene : DayScene
{
    public DaySixScene()
    {
        Day = 6;
    }

    public struct Race
    {
        public long TimeLimit;
        public long BestDistance;
    }

    public struct BoatResult
    {
        public long DistanceTravelled;
        public bool WonRace;
    }

    public BoatResult GetBoatResultForChargeTime(long chargeTime, Race race)
    {
        var distance = chargeTime * (race.TimeLimit - chargeTime);
        return new BoatResult
        {
            DistanceTravelled = distance,
            WonRace = race.BestDistance < distance,
        };
    }

    public void ProcessInputlongoRaceInformation()
    {
        // Line 0 is the total times for each race.
        var totalTimes = Input[0].Split(':')[1].Trim().Split(' ').Where(entry => entry != "").ToArray();
        // Line 1 is the best distance for each race.
        var bestDistances = Input[1].Split(':')[1].Trim().Split(' ').Where(entry => entry != "").ToArray();

        long totalNumberOfRaces = totalTimes.Length;

        RaceInformation = new();
        
        for (long i = 0; i < totalNumberOfRaces; i++)
        {
            RaceInformation.Add(new Race
            {
                TimeLimit = long.Parse(totalTimes[i]),
                BestDistance = long.Parse(bestDistances[i]),
            });
        }
    }
    
    public void ProcessInputlongoRaceInformationStarTwo()
    {
        // Line 0 is the total times for each race.
        var totalTime = Input[0].Split(':')[1].Trim().Replace(" ", "").Trim();
        // Line 1 is the best distance for each race.
        var bestDistance = Input[1].Split(':')[1].Trim().Replace(" ", "").Trim();

        RaceInformation = new();

            RaceInformation.Add(new Race
            {
                TimeLimit = long.Parse(totalTime),
                BestDistance = long.Parse(bestDistance),
            });
    }

    public List<Race> RaceInformation = new();
    public List<BoatResult> ResultsForRaces = new();
    public List<long> NumberOfWaysToWin = new();

    public override void StarOne()
    {
        base.StarOne();
        
        // Find number of ways to win for each race.
        ProcessInputlongoRaceInformation();
        CalculateRaceWins();
        
        long multiplyAllTogether = NumberOfWaysToWin.Aggregate((long)1, (x, y) => x * y);
        SimpleOutput.String = $"Number of Ways to Win, Mult Together: {multiplyAllTogether}";
        

    }
    
    public override void StarTwo()
    {
        base.StarTwo();
        
        ProcessInputlongoRaceInformationStarTwo();
        var waysToWin = CalculateRaceWinsMathematically(RaceInformation[0]);
        
        SimpleOutput.String = $"Number of Ways to Win: {waysToWin}";

    }

    public long CalculateRaceWinsMathematically(Race race)
    {
        // Where C = charge time
        // Where t = total race time
        // Where D = distance to beat
        // C^2 must be less than Ct - D
        // chargeTime^2 must be less than (chargeTime * TotalTime) - BestDistance
        // can i express it in terms of totaltime & best distance? then i can get a min and max of chargetime...
        // C^2 < Ct - D
        // C^2 + D < Ct 
        // (C^2 + D) / t < C
        // C > (C^2 + D) / t
        // ...
        // D < Ct - C^2... something something...
        // wait wait wait is this a, yknow, one of those. guys
        // the thing. the quadratic equation...
        // D < tC + -1C^2 ...
        // -1C^2 + tC + 0 > D
        // -1C^2 + tC - D > 0 /// THIS IS A QUADRATIC INEQUALITY!!
        // 1C^2 - tC + D < 0
        // ax^2 + bx + c < 0
        // need to find the values of x that make each bracket equal zero after factorising into brackets.
        

        // a = 1 
        // b = t
        // c = D
        
        // solution = -t +- sqrt(t^2 - 4*D) / 2*-1
        
        // t = 71530
        // D = 940200

        long minChargeTime = ((race.TimeLimit - (long)Math.Ceiling(Math.Sqrt((Math.Pow(race.TimeLimit, 2)) - (4 * race.BestDistance)))) / 2);
        long maxChargeTime = ((race.TimeLimit + (long)Math.Floor(Math.Sqrt((Math.Pow(race.TimeLimit, 2)) - (4 * race.BestDistance)))) / 2);
    
        Util.Log($"Min/Max charge time for race {race.TimeLimit}:{race.BestDistance} -- {minChargeTime}, {maxChargeTime}");
        Util.Log($"Number of possible ways to win: {maxChargeTime-minChargeTime}");
        
        
        return maxChargeTime - minChargeTime;
 
    }

    public void CalculateRaceWins()
    {
        ResultsForRaces = new();
        NumberOfWaysToWin = new();
        foreach (var race in RaceInformation)
        {
            long numberOfWaysToWin = 0;
            for (long chargeTime = 0; chargeTime <= race.TimeLimit; chargeTime++)
            {
                var boatResult = GetBoatResultForChargeTime(chargeTime, race);
                if (boatResult.WonRace)
                {
                    Util.Log($"Boat Charge {chargeTime} won Race {boatResult.DistanceTravelled} > {race.BestDistance} in total time{chargeTime}.");
                    numberOfWaysToWin++;
                }
                ResultsForRaces.Add(boatResult);
            }
            
            NumberOfWaysToWin.Add(numberOfWaysToWin);
        }

        
    }
}