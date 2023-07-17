using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A manager for running texas holdem style poker interactions. 
/// </summary>
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

    public List<int> GetBestHand()
    {
        int bestHandValue = int.MinValue;
        List<int> bestHandIds = new List<int>();
        for (int i = 0; i < m_cardHand.Length; i++)
        {
            Card[] tableCards = m_cardTable.GetCards();
            CardHand combinedHand = new CardHand(m_cardHand[i].Cards.Length + tableCards.Length);

            for (int j = 0; j < m_cardHand[i].Cards.Length; j++)
            {
                combinedHand.Cards[j] = m_cardHand[i].Cards[j];
            }
            for (int j = 0; j < tableCards.Length; j++)
            {
                combinedHand.Cards[j+m_cardHand[i].Cards.Length] = tableCards[j];
            }
            
            int handValue = HandEvaluator.EvaluateHand(combinedHand, Application.persistentDataPath+"/cardTable.json", 2);
            if (handValue >= bestHandValue )
            {
                if (handValue == bestHandValue)
                {
                    bestHandIds.Add(i);
                }
                bestHandValue = handValue;
            }
        }

        return bestHandIds;
    }
}
