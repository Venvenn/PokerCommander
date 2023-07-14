using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pot
{
    public int m_potValue;
    public int m_currentBetValue;
    public int m_consideredBetValue;
    public int m_currentBlindValue;

    public Pot(int blindValue)
    {
        m_potValue = 0;
        m_currentBetValue = 0;
        m_consideredBetValue = 0;
        m_currentBlindValue = blindValue;
    }
}
