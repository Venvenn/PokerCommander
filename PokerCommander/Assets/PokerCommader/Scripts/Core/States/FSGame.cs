using Siren;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Flowstate responsible for running the Game Logic and managing sub states for game play
/// </summary>
public class FSGame : FlowState
{
    private const int k_playerNum = 2;
    
    private GameUI m_ui;
    private GameContext m_gameContext;
    private FlowStateMachine m_gameplayStates;
    
    public FSGame(GameContext gameContext)
    {
        //UI
        m_gameContext = gameContext;
        m_gameplayStates = new FlowStateMachine(this);
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
        
        m_gameplayStates.Push(new FSPokerBattle(m_gameContext, battleData));
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
