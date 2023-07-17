using System;

/// <summary>
/// Dynamic data describing a commander participant in a poker battle.
/// </summary>
[Serializable]
public struct CombatCommanderData
{
    public CommanderData Data;
    public int Currency;
    
    public bool ActiveInRound;
    public BetState BetState;
    public int BetThisRound;

    public bool ActiveInBet(int currentBet)
    {
        switch (BetState)
        {
            case BetState.In:
                return true;
            case BetState.Checked:
            case BetState.Bet:
            case BetState.Called:
            case BetState.Raised:
                return currentBet > BetThisRound;
            case BetState.Folded:
            case BetState.Out:
                return false;
        }

        return false;
    }
}
    