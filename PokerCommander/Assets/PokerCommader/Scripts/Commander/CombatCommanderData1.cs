using System;
using UnityEngine;

[Serializable]
public struct CombatCommanderData
{
    public CommanderData Data;
    public bool ActiveInRound;
    public BetState BetState;

    public int Currency;

    public bool ActiveInBet()
    {
        switch (BetState)
        {
            case BetState.In:
            case BetState.Checked:
                return true;
            case BetState.Bet:
            case BetState.Folded:
            case BetState.Raised:
            case BetState.Called:
            case BetState.Out:
                return false;
        }

        return false;
    }
}
    