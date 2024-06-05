using Siren;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class DialogueUI : FlowScreenUI
{
    [SerializeField]
    private DialogueRunner m_dialogueRunner;
    
    public void InitUI(UnityAction onComplete)
    {
        m_dialogueRunner.onDialogueComplete.AddListener(onComplete);
    }

    public override void UpdateUI()
    {
    }

    public override void DestroyUI()
    {
    }

    public void StartDialogue(YarnProject yarnProject)
    {
        m_dialogueRunner.SetProject(yarnProject);
        m_dialogueRunner.yarnProject = yarnProject;
        m_dialogueRunner.StartDialogue("Start");
    }
    
}
