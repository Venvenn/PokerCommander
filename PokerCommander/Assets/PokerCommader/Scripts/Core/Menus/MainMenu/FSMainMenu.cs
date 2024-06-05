using Siren;
using UnityEngine;

public class FSMainMenu : FlowState
{
    private MainMenuUI m_ui;
    private GameContext m_gameContext;
    
    public FSMainMenu(GameContext gameContext)
    {
        m_gameContext = gameContext;
    }

    public override void OnInitialise()
    {
        //Set up scene
        m_ui = m_gameContext.UIManager.LoadUIScreen<MainMenuUI>("UI/Screens/MainMenuUI", this);
   
        //Set up UI
        m_ui.GetComponent<FlowUIGroup>().AttachFlowState(this);
    }

    public override void OnActive()
    {
        m_ui.gameObject.SetActive(true);
    }

    private void NewGame()
    {
        Scenario scenario = Resources.Load<Scenario>("Data/Scenarios/MainStory/MainStory");
        FlowStateMachine.Pop();
        FlowStateMachine.Push(new FSGame(m_gameContext, scenario));
    }
    
    private void Continue()
    {
    }
    
    private void Settings()
    {
    }

    
    private void QuitGame()
    {
        Application.Quit();
    }
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "newGame":
            {
                NewGame();
                break;
            }
            case "continue":
            {
                Continue();
                break;
            }
            case "settings":
            {
                Settings();
                break;
            }
            case "quit":
            {
                QuitGame();
                break;
            }
        }
    }

    public override void OnInactive()
    {
        m_ui.gameObject.SetActive(false);
    }

    public override void OnDismiss()
    {
        Object.Destroy(m_ui.gameObject);
    }
}
