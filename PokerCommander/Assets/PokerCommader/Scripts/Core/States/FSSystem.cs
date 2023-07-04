using Siren;
using UnityEngine;

public class FSSystem : FlowState
{
    private FlowStateMachine m_gameStates;
    private GameContext m_gameContext;
    public FSSystem()
    {
        //UI
        m_gameStates = new FlowStateMachine(this);
        m_gameContext = new GameContext();
    }

    public override void OnInitialise()
    {
        m_gameStates.Push(new FSMainMenu(m_gameContext));
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {
        m_gameStates.Update();
    }
    
    public override void ActiveFixedUpdate()
    {
        m_gameStates.FixedUpdate();
    }
    
    public override void ReceiveFlowMessages(object message)
    {
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
        m_gameStates.PopAllStates();
    }

    public override TransitionState UpdateDismiss()
    {
        m_gameStates.Update();
        if(m_gameStates.StateCount == 0)
        {
            return TransitionState.COMPLETED;
        }
        return TransitionState.IN_PROGRESS;
    }

    public override void FinishDismiss()
    {
    }
}
