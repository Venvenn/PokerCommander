using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public static class CardShuffleSystem
{
    public static Stack<int> FisherYatesShuffle(Stack<int> deckStack, Random random)
    {
        int[] deck = deckStack.ToArray();
        
        for (int n = deck.Length - 1; n > 0; --n)
        {
            int k = random.NextInt(n + 1);
            int temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }
        
        return new Stack<int>(deck);
    }
}
