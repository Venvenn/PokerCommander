using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class CardSystem
{
    private const int k_cardsInHand = 2;
    
    private Stack<int> m_deck;
    private Stack<int> m_discardPile;
    private Random m_random;
    private Card[] m_cardData;


    public CardSystem()
    {
        SetUpDeck();
    }
    
    public void SetUpDeck()
    {
        m_random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue));
        m_cardData = Resources.Load<CardDataObject>("Data/CardData").Cards;
        m_deck = new Stack<int>(m_cardData.Length);
        m_discardPile = new Stack<int>();
        
        for (int i = 0; i < m_cardData.Length; i++)
        {
            m_cardData[i].Id = i;
            m_deck.Push(i);
        }
        
        m_deck = CardShuffleSystem.FisherYatesShuffle(m_deck, m_random);
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
    
    public CardHand DrawHand()
    {
        CardHand cardHand = new CardHand(k_cardsInHand);

        for (int i = 0; i < cardHand.Cards.Length; i++)
        {
            cardHand.Cards[i] = DrawCard();
        }

        return cardHand;
    }
}
