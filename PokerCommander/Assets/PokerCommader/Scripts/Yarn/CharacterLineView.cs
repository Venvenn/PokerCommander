using System;
using UnityEngine;
using Yarn.Unity;

public class CharacterLineView : LineView
{
    [SerializeField]
    private Transform m_charNamePlate;

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        m_charNamePlate.gameObject.SetActive(!string.IsNullOrEmpty(dialogueLine.CharacterName));
        base.RunLine(dialogueLine, onDialogueLineFinished);
    }
}
