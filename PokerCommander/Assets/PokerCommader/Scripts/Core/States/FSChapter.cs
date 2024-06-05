using Siren;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// FlowState responsible for running the Chapter graph, setting up dialogue and combat.
/// </summary>
public class FSChapter : FlowState
{
    private GameContext m_gameContext;
    private Act m_act;

    private int m_chapterId;
    private BaseChapterNode m_currentNode;
    
    public FSChapter(GameContext gameContext, Act act)
    {
        m_gameContext = gameContext;
        m_act = act;
        m_chapterId = 0;
    }

    public override void OnInitialise()
    {
    }

    public override void OnActive()
    {
        ProgressChapter();
    }

    private void ProgressChapter()
    {
        m_currentNode = m_act.Chapters[m_chapterId].NextNode(m_currentNode);

        switch (m_currentNode)
        {
            case DialogueNode dialogueNode:
            {
                FlowStateMachine.Push(new FSDialogue(m_gameContext, dialogueNode.YarnProject));
                break;
            }
            case CombatNode combatNode:
            {
                FlowStateMachine.Push(new FSPokerBattle(m_gameContext, combatNode.BattleData));
                break;
            }
            case EndNode endNode:
            {
                m_chapterId++;
                if (m_chapterId < m_act.Chapters.Length)
                {
                    ProgressChapter();
                    m_currentNode = null;
                }
                else
                {
                    FlowStateMachine.Pop();
                }
                break;
            }
        }
    }
    
    public override void ActiveUpdate()
    {

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

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
    }
    

    public override void FinishDismiss()
    {
    }
}
