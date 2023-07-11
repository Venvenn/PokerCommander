using System.Collections.Generic;
using Siren;
using TMPro;
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

    [SerializeField] 
    private TextMeshProUGUI m_currencyText;

    
    public void InitUI(NationData playerNation, CombatCommanderData[] participants, CardBack cardBack)
    {

    }

    public void UpdateCurrencyValue(int newValue)
    {
        m_currencyText.text = newValue.ToString();
    }
}
