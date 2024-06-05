using Siren;
using UnityEngine;

/// <summary>
/// FlowState responsible for running the Game Logic and managing sub states for game play
/// </summary>
public class FSGame : FlowState
{
    private const int k_playerNum = 2;
    
    private GameUI m_ui;
    private GameContext m_gameContext;
    private FlowStateMachine m_gameplayStates;

    //Scenario
    private Scenario m_scenario;
    private int m_actId = 0;
    
    public FSGame(GameContext gameContext, Scenario scenario)
    {
        //UI
        m_gameContext = gameContext;
        m_gameplayStates = new FlowStateMachine(this);
        
        //Game
        m_scenario = scenario;
    }

    public override void OnInitialise()
    {
        m_ui = m_gameContext.UIManager.LoadUIScreen<GameUI>("UI/Screens/GameUI", this);
        m_ui.InitUI();
    }

    public override void OnActive()
    {
        BattleData battleData = new BattleData()
        {
            Participants = new[] {new StringId("player"), new StringId("sir_oslo"), new StringId("bandit_leader")}
        };
        
        FlowStateMachine.Push(new FSPokerBattle(m_gameContext, battleData));
    }
    
    public override void ActiveUpdate()
    {
        m_gameplayStates.Update();
        m_ui.UpdateUI();
        
        //temp input 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    
    public override void ActiveFixedUpdate()
    {
        m_gameplayStates.FixedUpdate();
    }

    private void Pause()
    {
        FlowStateMachine.Push(new FSPauseMenu(this, m_gameContext.UIManager));
    }
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "mainMenu":
            {
                FlowStateMachine.Pop();
                break;
            }
            case "pauseMenu":
            {
                Pause();
                break;
            }
        }
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
        m_gameplayStates.PopAllStates();
    }

    public override TransitionState UpdateDismiss()
    {
        m_gameplayStates.Update();
        if(m_gameplayStates.StateCount == 0)
        {
            return TransitionState.COMPLETED;
        }
        return TransitionState.IN_PROGRESS;
    }

    public override void FinishDismiss()
    {
        m_ui.DestroyUI();
        Object.Destroy(m_ui.gameObject);
    }
}
