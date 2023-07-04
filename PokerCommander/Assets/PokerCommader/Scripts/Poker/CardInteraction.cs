using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInteractionManager
{
    public CardHand[] m_cardHand;
    private CardSystem m_cardSystem;

    public CardInteractionManager(CardSystem cardSystem, int playerNum)
    {
        m_cardSystem = cardSystem;
        m_cardHand = new CardHand[playerNum];

        for (int i = 0; i < playerNum; i++)
        {
            m_cardHand[i] = m_cardSystem.DrawHand();
        }
    }
}
