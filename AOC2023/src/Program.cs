using Lutra.Utility;
using AOC2023.Scenes;

// As an example, make sure we preload the test font we use in this project
AssetManager.PreloadAssets += () =>
{
    AssetManager.GetFont("monogram-extended.ttf", false);
};

// GameOptions allows us to set some of the initial state of the Game.
Game game = new Game(new GameOptions()
{
    Title = "Advent Of Code 2023",
    Width = 1280,
    Height = 720,
    ScaleXY = 1f,
    TargetFrameRate = 60.0
});

game.Start(new MenuScene());
