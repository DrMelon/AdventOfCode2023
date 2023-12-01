using System;
using System.Collections.Generic;
using AOC2023.Common;

namespace AOC2023.Scenes;

public class MenuScene : Scene
{
    // A little menu to select from the AoC 2023 Days.
    private SimpleMenu mainMenu;

    public override void Begin()
    {
        base.Begin();

        mainMenu = new SimpleMenu(Game.HalfWidth, Game.HalfHeight, new Dictionary<string, Action>()
        {
            {"Day One", () => Game.AddScene(new DayOneScene())},
            {"Exit", () => Game.Close()}
        });
        
        Add(mainMenu);
    }
}