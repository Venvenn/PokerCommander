using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardHand
{
    public Card[] Cards;

    public CardHand(int cardsInHand)
    {
        Cards = new Card[cardsInHand];
    }
}
