using System;

/// <summary>
/// Data about a battle. stores relevant info the battle simulation will require
/// </summary>
[Serializable]
public struct BattleData
{
    public StringId[] Participants; 
}
