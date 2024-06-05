using Siren;
using UnityEngine;

/// <summary>
/// Shared context used for containing managers and systems that need to be frequently passed around.
/// </summary>
public class GameContext : SharedContext
{
    public UIManager UIManager;
    public ContentDatabase ContentDatabase;
    public SeededRandom SeededRandom;
    public YarnVariables YarnVariables;

    public GameContext()
    {
        UIManager = new UIManager("UI/UIScreens");
        ContentDatabase = new ContentDatabase();
        SeededRandom = new SeededRandom();
        YarnVariables = Object.FindObjectOfType<YarnVariables>();
    }
}
