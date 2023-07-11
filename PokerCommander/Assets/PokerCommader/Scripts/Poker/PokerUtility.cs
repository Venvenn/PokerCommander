using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PokerUtility
{
    public static int GetNextPlayerId(int currentPlayer, CombatCommanderData[] participants)
    {
        bool found = false;
        while (!found)
        {
            currentPlayer = currentPlayer < participants.Length - 1 ? currentPlayer + 1 : 0;

            if (participants[currentPlayer].ActiveInRound)
            {
                found = true;
            }
        }

        return currentPlayer;
    }
}
