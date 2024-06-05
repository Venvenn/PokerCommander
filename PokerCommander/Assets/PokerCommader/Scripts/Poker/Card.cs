using System;
using UnityEngine;

[Serializable]
public struct Card : IEquatable<Card>
{
    private static int[] s_rankPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41 };
    private static int[] s_suitPrimes = { 43, 47, 53, 59 };
    public static char[] s_ranks = "23456789tjqka".ToCharArray();
    public static char[] s_suits = "shdc".ToCharArray();
    
    public Sprite Sprite;
    public Rank Rank;
    public Suit Suit;

    public int PrimeRank => s_rankPrimes[(int)Rank];
    public int PrimeSuit => s_suitPrimes[(int)Suit];
    public string Name => $"{Rank} Of {Suit}";

    public static Card StringToCard(string cardString)
    {
        var chars = cardString.ToUpper().ToCharArray();
        if (chars.Length != 2) throw new ArgumentException("Card string must be length 2");

        Rank rank;
        Suit suit;
        switch (chars[0])
        {
            case '2': rank = Rank.Two; break;
            case '3': rank = Rank.Three; break;
            case '4': rank = Rank.Four; break;
            case '5': rank = Rank.Five; break;
            case '6': rank = Rank.Six; break;
            case '7': rank = Rank.Seven; break;
            case '8': rank = Rank.Eight; break;
            case '9': rank = Rank.Nine; break;
            case 't': rank = Rank.Ten; break;
            case 'j': rank = Rank.Jack; break;
            case 'q': rank = Rank.Queen; break;
            case 'k': rank = Rank.King; break;
            case 'a': rank = Rank.Ace; break;
            default: throw new ArgumentException("Card string rank not valid");
        }
        switch (chars[1])
        {
            case 's': suit = Suit.Spades; break;
            case 'h': suit = Suit.Hearts; break;
            case 'd': suit = Suit.Diamonds; break;
            case 'c': suit = Suit.Clubs; break;
            default: throw new ArgumentException("Card string suit not valid");
        }
        
        return new Card
        {
            Rank = rank,
            Suit = suit
        };
    }

    public ulong GetId()
    {
        return 1ul << (int) Rank * 4 + (int)Suit;
    }
    
    public bool Equals(Card other)
    {
        return Rank == other.Rank && Suit == other.Suit;
    }

    public override bool Equals(object obj)
    {
        return obj is Card other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int) Rank, (int) Suit);
    }
    
    public override string ToString()  
    {
        char[] ranks = "23456789tjqka".ToCharArray();
        char[] suits = { 's', 'h', 'd', 'c' };

        return ranks[(int)Rank].ToString() + suits[(int)Suit].ToString();
    }
}
