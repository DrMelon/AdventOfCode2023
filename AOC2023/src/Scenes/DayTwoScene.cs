using System;
using System.Collections.Generic;
using System.Linq;
using Lutra.Graphics;
using Lutra.Utility;

namespace AOC2023.Scenes;

public class DayTwoScene : DayScene
{
    private Image Elf;
    private Image RedCube;
    private Image GreenCube;
    private Image BlueCube;
    
    public DayTwoScene()
    {
        Day = 2;
    }

    public struct CubeGame
    {
        public int ID;
        public List<CubeHandful> Handfuls;
    }

    public struct CubeHandful
    {
        public int Red;
        public int Green;
        public int Blue;
    }

    public override void Begin()
    {
        base.Begin();

        Elf = new Image("img/elf_with_bag.png");
        Elf.Scale = 2;
        Elf.X = 128;
        Elf.Y = Game.Height - 140;
        Elf.Dropshadow = 1;
        Elf.CenterOrigin();
        
        GreenCube = new Image("img/cube.png");
        GreenCube.Scale = 2;
        GreenCube.X = 128 + (21*2) - (16);
        GreenCube.Y = Game.Height - 140 -(14*2);
        GreenCube.Dropshadow = 1;
        GreenCube.Color = Color.Green;
        GreenCube.CenterOrigin();
        
        BlueCube = new Image("img/cube.png");
        BlueCube.Scale = 2;
        BlueCube.X = 128 + (21*2) + 16;
        BlueCube.Y = Game.Height - 140 -(14*2);
        BlueCube.Dropshadow = 1;
        BlueCube.Color = Color.Blue;
        BlueCube.CenterOrigin();
        
        RedCube = new Image("img/cube.png");
        RedCube.Scale = 2;
        RedCube.X = 128 + (21*2);
        RedCube.Y = Game.Height - 140 -(12*2);
        RedCube.Dropshadow = 1;
        RedCube.Color = Color.Red;
        RedCube.CenterOrigin();
        
        AddGraphic(Elf);
        
        AddGraphic(GreenCube);
        AddGraphic(BlueCube);
        AddGraphic(RedCube);
    }

    public override void StarOne()
    {
        base.StarOne();

        int idSumOfValidGames = 0;
        int validGameCount = 0;
        
        foreach(var line in Input)
        {
            var cubeGame = ParseLineIntoCubeGame(line);
            if (IsGamePossibleWithThisManyColours(cubeGame, 12, 13, 14))
            {
                idSumOfValidGames += cubeGame.ID;
                validGameCount++;
                Util.Log($"Cube game {cubeGame.ID} is valid. Total now {idSumOfValidGames}.");
            }
        }

        SimpleOutput.String = $"Valid Cube Game ID Sum: {idSumOfValidGames}. Total Valid Games: {validGameCount}";
    }

    public override void StarTwo()
    {
        base.StarTwo();
        
        long sumOfCubePowers = 0;
        
        foreach(var line in Input)
        {
            var cubeGame = ParseLineIntoCubeGame(line);
            var minHandful = GetMinimumCubeCountsForGame(cubeGame);
            var power = GetPowerOfHandful(minHandful);

            sumOfCubePowers += power;
            Util.Log($"Minimum handful for game {cubeGame.ID}: R {minHandful.Red}, G {minHandful.Green}, B {minHandful.Blue}. Power: {power}.");
        }

        SimpleOutput.String = $"Sum of Minimum Cube Handful Powers: {sumOfCubePowers}";
    }

    private CubeGame ParseLineIntoCubeGame(string line)
    {
        string[] splitOnColon = line.Split(':');
        string idString = splitOnColon[0].Substring(4);
        string[] handfulStrings = splitOnColon[1].Split(';');

        List<CubeHandful> cubeHandfuls = new();
        foreach (var handfulString in handfulStrings)
        {
            cubeHandfuls.Add(ParseHandfulStringIntoHandful(handfulString));
        }

        return new CubeGame
        {
            ID = int.Parse(idString),
            Handfuls = cubeHandfuls
        };
    }

    private CubeHandful ParseHandfulStringIntoHandful(string handfulString)
    {
        CubeHandful handful = new CubeHandful();
        string[] handfulInfoStrings = handfulString.Split(',');
        foreach (var handfulInfoString in handfulInfoStrings)
        {
            string trimmed = handfulInfoString.Trim();
            var infoPackets = trimmed.Split(',');
            foreach (var infoPacket in infoPackets)
            {
                string[] infoPacketUnpacked = infoPacket.Split(' ');
                int amount = int.Parse(infoPacketUnpacked[0]);

                switch (infoPacketUnpacked[1])
                {
                    case "red":
                        handful.Red = amount;
                        break;
                    case "green":
                        handful.Green = amount;
                        break;
                    case "blue":
                        handful.Blue = amount;
                        break;
                }
            }
        }

        return handful;
    }

    public bool IsGamePossibleWithThisManyColours(CubeGame game, int redAmt, int greenAmt, int blueAmt)
    {
        return (game.Handfuls.Max(handful => handful.Red) <= redAmt &&
                game.Handfuls.Max(handful => handful.Green) <= greenAmt &&
                game.Handfuls.Max(handful => handful.Blue) <= blueAmt);
    }

    public CubeHandful GetMinimumCubeCountsForGame(CubeGame game)
    {
        CubeHandful result = new CubeHandful();
        foreach (var cubehandful in game.Handfuls)
        {
            result.Red = Math.Max(cubehandful.Red, result.Red);
            result.Green = Math.Max(cubehandful.Green, result.Green);
            result.Blue = Math.Max(cubehandful.Blue, result.Blue);
        }

        return result;
    }

    public int GetPowerOfHandful(CubeHandful handful)
    {
        return handful.Red * handful.Green * handful.Blue;
    }

    public override void Update()
    {
        RedCube.Y = Game.Height - 140 -(12*2) + (MathF.Sin(Game.Timer) * 4);
        GreenCube.Y = Game.Height - 140 -(14*2) + (MathF.Sin(Game.Timer + 2.0f) * 5);
        BlueCube.Y = Game.Height - 140 -(14*2) + (MathF.Sin(Game.Timer + 4.0f) * 5);
    }
}