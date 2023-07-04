using System.Collections;
using System.Collections.Generic;
using Siren;
using UnityEngine;

public class GameContext : SharedContext
{
    public UIManager UIManager;
    public ContentDatabase ContentDatabase;
    public SeededRandom SeededRandom;

    public GameContext()
    {
        UIManager = new UIManager("UI/UIScreens");
        ContentDatabase = new ContentDatabase();
        SeededRandom = new SeededRandom();
    }
}
