using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Siren;
using UnityEngine;

public class FSActStart : FlowState
{
    private GameContext m_gameContext;
    private ActStartUI m_ui;

    private Act m_act;
    private Task m_fadeOutTask;

    public FSActStart(GameContext gameContext, Act act)
    {
        m_gameContext = gameContext;
        m_act = act;
    }
    
    public override async void OnInitialise()
    {
        m_ui = m_gameContext.UIManager.LoadUIScreen<ActStartUI>("UI/Screens/ActStartUI", this);
        m_ui.Init(m_act);
        
        await m_ui.FadeIn(0.5f, 1);
    }

    public override TransitionState UpdateInitialise()
    {
        return base.UpdateInitialise();
    }

    public override async void FinishInitialise()
    {
        await Task.Delay(5000);
        FlowStateMachine.Pop();
        FlowStateMachine.Push(new FSChapter(m_gameContext, m_act));
    }

    public override void OnActive()
    {
        m_ui.gameObject.SetActive(true);
    }

    public override void OnInactive()
    {
        m_ui.gameObject.SetActive(false);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
    }

    public override void ActiveFixedUpdate()
    {
        base.ActiveFixedUpdate();
    }

    public override async void OnDismiss()
    {
        m_fadeOutTask = m_ui.FadeOut(5);
        await m_fadeOutTask;
    }

    public override TransitionState UpdateDismiss()
    {
        return m_fadeOutTask.IsCompleted ? TransitionState.COMPLETED : TransitionState.IN_PROGRESS;
    }

    public override void FinishDismiss()
    {
        Object.Destroy(m_ui.gameObject);
    }

    public override void ReceiveFlowMessages(object message)
    {
        base.ReceiveFlowMessages(message);
    }
}
