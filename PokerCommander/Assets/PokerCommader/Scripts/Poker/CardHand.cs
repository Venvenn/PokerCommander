/// <summary>
/// Data Struct for storing the cards contained with in a players hand. 
/// </summary>
public struct CardHand
{
    public Card[] Cards;

    public CardHand(int cardsInHand)
    {
        Cards = new Card[cardsInHand];
    }

    public CardHand(int cardsInHand, ulong bitMap)
    {
        Cards = new Card[cardsInHand];
        Cards = BitMapToHand(bitMap, cardsInHand);
    }

    public ulong GetBitmap()
    {
        return HandToBitmap();
    }

    private Card[] BitMapToHand(ulong bitmap, int cardNumber)
    {
        Card[] cards = new Card[cardNumber];

        int id = 0;
        for (int r = 0; r < Card.s_ranks.Length; r++)
        {
            for (int s = 0; s < Card.s_suits.Length; s++)
            {
                var shift = r * 4 + s;
                if (((1ul << shift) & bitmap) != 0)
                {
                    cards[id] = Card.StringToCard(Card.s_ranks[r].ToString() + Card.s_suits[s].ToString());
                    id++;
                }
            }
        }

        return cards;
    }

    private ulong HandToBitmap()
    {
        ulong bitmap = 0;
        for (int i = 0; i < Cards.Length; i++)
        {
            bitmap |= Cards[i].GetId();
        }

        return bitmap;
    }
    
    
}
