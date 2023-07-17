using System;

/// <summary>
/// Data describing a nation in the game
/// </summary>
[Serializable]
public struct NationData
{
    public StringId Id;
    public string Name;
    public SerialisableDictionary<string, AllegianceType> Allegiances;
}
