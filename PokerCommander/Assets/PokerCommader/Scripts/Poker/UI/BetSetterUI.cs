using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BetSetterUI : MonoBehaviour
{
    [SerializeField]
    private Slider m_slider;
    [SerializeField]
    private TextMeshProUGUI m_valueText;
    [SerializeField]
    private Button m_increaseBet;
    [SerializeField]
    private Button m_decreaseBet;
    [SerializeField]
    private Button m_resetBet;

    private Transform m_valueBox;
    private Transform m_betAreaBox;
    
    public void SetValueText(int value,int minValue, int maxValue)
    {
        m_slider.value = Mathf.InverseLerp(minValue, maxValue, value);
        m_valueText.text = value.ToString();
        m_valueBox.gameObject.SetActive(value > 0);
    }
}
