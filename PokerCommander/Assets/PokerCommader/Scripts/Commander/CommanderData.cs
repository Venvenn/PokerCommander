using System;
using UnityEngine;

/// <summary>
/// Static Data about a commander, loaded from SO
/// </summary>
[Serializable]
public struct CommanderData
{
    public StringId Id;
    public string Name;
    public Sprite Icon;
    public StringId Rank;
    public StringId Allegiance;
}
