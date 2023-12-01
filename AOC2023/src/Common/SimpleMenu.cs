using System;
using System.Collections.Generic;
using System.Linq;
using Lutra.Graphics;
using Lutra.Input;
using Lutra.Utility;

namespace AOC2023.Common;

public class SimpleMenu : Entity
{
    private Dictionary<string, Action> Mappings;
    private List<RichText> MenuText = new List<RichText>();
    public RichTextConfig SelectedStyle;
    public RichTextConfig NormalStyle;

    public int CurrentlySelected = 0;

    public SimpleMenu(float xCenter, float yCenter, Dictionary<string, Action> mappings)
    {
        X = xCenter;
        Y = yCenter;
        
        Mappings = mappings;

        NormalStyle = new RichTextConfig();
        NormalStyle.Font = AssetManager.GetFont("monogram-extended.ttf", false);
        NormalStyle.FontSize = 32;
        NormalStyle.ShadowX = 1;
        NormalStyle.ShadowY = 1;
        NormalStyle.ShadowColor = Color.Black;
        NormalStyle.CharColor = Color.White;

        SelectedStyle = new RichTextConfig();
        SelectedStyle.Font = AssetManager.GetFont("monogram-extended.ttf", false);
        SelectedStyle.FontSize = 32;
        SelectedStyle.ShadowX = 2;
        SelectedStyle.ShadowY = 2;
        SelectedStyle.ShadowColor = Color.Black;
        SelectedStyle.CharColor = Color.Yellow;

        int runningYOffset = -(Mappings.Count / 2 * (NormalStyle.FontSize + 2));

        foreach (var mapping in Mappings)
        {
            RichText newText = new RichText(mapping.Key, NormalStyle);
            newText.X = 0;
            newText.Y = runningYOffset;
            newText.Refresh();
            newText.CenterTextOrigin();
            newText.CenterOrigin();
            AddGraphic(newText);
            MenuText.Add(newText);

            runningYOffset += (NormalStyle.FontSize + 2);
        }

        Select(CurrentlySelected);
    }

    public void Select(int id)
    {
        MenuText[CurrentlySelected].SetConfig(NormalStyle);
        MenuText[id].SetConfig(SelectedStyle);

        CurrentlySelected = id;
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.KeyPressed(Key.Down))
        {
            Select((CurrentlySelected + 1) % Mappings.Count);
        }
        
        if (InputManager.KeyPressed(Key.Up))
        {
            Select((CurrentlySelected - 1) % Mappings.Count);
        }

        if (InputManager.KeyPressed(Key.Enter))
        {
            Mappings[Mappings.Keys.ToArray()[CurrentlySelected]]?.Invoke();
        }
    }
}