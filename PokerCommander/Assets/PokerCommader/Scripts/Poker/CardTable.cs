using System.Collections.Generic;

/// <summary>
/// Data for cards dealt to the table
/// </summary>
public struct CardTable
{
    private List<Card> m_cards;

    public CardTable(int cardCount)
    {
        m_cards = new List<Card>(cardCount);
    }

    public void AddCard(Card card)
    {
        m_cards.Add(card);
    }
    
    public Card[] GetCards()
    {
        return m_cards.ToArray();
    }

    public void ClearTable()
    {
        m_cards.Clear();
    }
}
