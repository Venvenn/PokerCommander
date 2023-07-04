using System.Collections.Generic;
using Siren;
using UnityEngine;
using UnityEngine.UI;

public class CommandZoneUI : MonoBehaviour
{
    [SerializeField]
    private Button m_checkButton;
    [SerializeField]
    private Button m_foldButton;
    [SerializeField]
    private Button m_raiseButton;
    [SerializeField]
    private Button m_betButton;
    [SerializeField]
    private Button m_callButton;

    public void InitUI(NationData playerNation, CombatCommanderData[] participants, CardBack cardBack)
    {

    }
}
