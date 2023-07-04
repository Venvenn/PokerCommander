using System;
using Siren;

[Serializable]
public struct NationData
{
    public StringId Id;
    public string Name;
    public SerialisableDictionary<string, AllegianceType> Allegiances;
}
