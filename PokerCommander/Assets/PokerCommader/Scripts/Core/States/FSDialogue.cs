using Siren;
using Unity.Mathematics;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Flowstate responsible for starting and running a dialogue encounter
/// </summary>
public class FSDialogue : FlowState
{
    private DialogueUI m_ui;
    private GameContext m_gameContext;

    private YarnProject m_yarnProject;

    public FSDialogue(GameContext gameContext, YarnProject yarnProject)
    {
        m_gameContext = gameContext;
        m_yarnProject = yarnProject;
    }

    public override void OnInitialise()
    {
        m_ui = m_gameContext.UIManager.LoadUIScreen<DialogueUI>("UI/Screens/DialogueUI", this);
        m_ui.InitUI(DialogueComplete);
        m_ui.StartDialogue(m_yarnProject);
    }

    public override void OnActive()
    {
    }
    
    public override void ActiveUpdate()
    {
        m_ui.UpdateUI();
    }
    
    public override void ActiveFixedUpdate()
    {
    }
    
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
        }
    }

    private void DialogueComplete()
    {
       FlowStateMachine.Pop();
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
    }

    public override void FinishDismiss()
    {
        m_ui.DestroyUI();
        Object.Destroy(m_ui.gameObject);
    }
}
