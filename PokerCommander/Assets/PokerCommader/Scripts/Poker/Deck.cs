using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private Stack<int> m_deck;
    private Stack<int> m_discardPile;

    private Card[] m_cardData;
    
    private SeededRandom m_random;

    public Deck(SeededRandom seededRandom)
    {
        m_random = seededRandom;
        SetUpDeck();
    }
    
    public void SetUpDeck()
    {
        m_cardData = Resources.Load<CardDataObject>("Data/CardData").Cards;
        ResetDeck();
    }

    public Card DrawCard()
    {
        int cardId = m_deck.Pop();
        return m_cardData[cardId];
    }
    
    public void DiscardCard(int id)
    {
        m_discardPile.Push(id);
    }
    
    public CardHand DrawHand(int handSize)
    {
        CardHand cardHand = new CardHand(handSize);

        for (int i = 0; i < cardHand.Cards.Length; i++)
        {
            cardHand.Cards[i] = DrawCard();
        }

        return cardHand;
    }

    public void ResetDeck()
    {
        m_deck = new Stack<int>(m_cardData.Length);
        m_discardPile = new Stack<int>();
        for (int i = 0; i < m_cardData.Length; i++)
        {
            m_cardData[i].Id = i;
            m_deck.Push(i);
        }
        m_deck = CardShuffleSystem.FisherYatesShuffle(m_deck,ref m_random.Random);
    }
}
