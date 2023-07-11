using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexasHoldemInteractionManager
{
    private const int k_cardsInHand = 2;
    private const int k_flopSize = 3;
    private const int k_tableTotalCards = 5;
    
    public CardHand[] m_cardHand;
    public CardTable m_cardTable;
    private Deck m_deck;

    public TexasHoldemInteractionManager(Deck deck, int playerNum)
    {
        m_deck = deck;
        m_cardHand = new CardHand[playerNum];
        m_cardTable = new CardTable(k_tableTotalCards);
    }
    
    public void DealHand()
    {
        for (int i = 0; i < m_cardHand.Length; i++)
        {
            m_cardHand[i] = m_deck.DrawHand(k_cardsInHand);
        }
    }
    
    public void DealFlop()
    {
        for (int i = 0; i < k_flopSize; i++)
        {
            Card newCard = m_deck.DrawCard();
            m_cardTable.AddCard(newCard); 
        }
    }
    
    public void DealTurn()
    {
        Card newCard = m_deck.DrawCard();
        m_cardTable.AddCard(newCard);
    }
    
    public void DealRiver()
    {
        Card newCard = m_deck.DrawCard();
        m_cardTable.AddCard(newCard);
    }

    public void Reset()
    {
        m_cardHand = new CardHand[m_cardHand.Length];
        m_cardTable = new CardTable(k_tableTotalCards);
    }
}
