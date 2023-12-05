using System;
using System.Collections.Generic;
using AOC2023.Common;
using Lutra.Graphics;
using Lutra.Utility;

namespace AOC2023.Scenes;

public class DayScene : Scene
{
    protected List<string> Input;
    protected SimpleMenu DayMenu;
    protected RichText SimpleOutput;
    protected Image BG;
    public int Day = 0;
    
    public override void Begin()
    {
        base.Begin();

        BG = new Image("img/cloudbg.png");
        BG.Repeat = true;
        BG.Scale = 2;
        AddGraphic(BG);
        
        DayMenu = new SimpleMenu(Game.HalfWidth, Game.HalfHeight, new Dictionary<string, Action>()
        {
            {"Load Test Input A", LoadTestInputA},
            {"Load Test Input B", LoadTestInputB},
            {"Load Real Input", LoadRealInput},
            {"Star 1", StarOneInternal},
            {"Star 2", StarTwoInternal},
            {"<- Back", () => Game.RemoveScene()}
        });

        

        SimpleOutput = new RichText(" ", DayMenu.NormalStyle);
        SimpleOutput.X = 64;
        SimpleOutput.Y = Game.Height - 64;
        
        AddGraphic(SimpleOutput);
        
        Add(DayMenu);
    }
    
    public void LoadTestInputA()
    {
        Input = InputLoader.LoadInputFromAssets($"day{Day}_test_a.txt");
        SimpleOutput.String = $"Loaded: Test Input A, {Input.Count} lines.";
        SimpleOutput.Refresh();
    }
    
    public void LoadTestInputB()
    {
        Input = InputLoader.LoadInputFromAssets($"day{Day}_test_b.txt");
        SimpleOutput.String = $"Loaded: Test Input B, {Input.Count} lines.";
        SimpleOutput.Refresh();
    }

    public void LoadRealInput()
    {
        Input = InputLoader.LoadInputFromAssets($"day{Day}.txt");
        SimpleOutput.String = $"Loaded: Real Input, {Input.Count} lines.";
        SimpleOutput.Refresh();
    }

    public void StarOneInternal()
    {
        if (Input == null)
            return;

        StarOne();
    }

    public void StarTwoInternal()
    {
        if (Input == null)
            return;

        StarTwo();
    }

    public virtual void StarOne()
    {
        
    }

    public virtual void StarTwo()
    {
        
    }

    public override void UpdateFirst()
    {
        base.UpdateFirst();

        BG.X = (BG.X - 1) % (BG.Width * BG.ScaleX);
    }
}