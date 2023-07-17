using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple database for storing unchanging data loaded from scitable objects
/// </summary>
public class ContentDatabase
{
    public Dictionary<string, CommanderData> m_commanders;
    public Dictionary<string, NationData> m_nations;

    public ContentDatabase()
    {
        CommanderDataSO[] commanderData = Resources.LoadAll<CommanderDataSO>("Data/Characters");
        m_commanders = new Dictionary<string, CommanderData>(commanderData.Length);
        for (int i = 0; i < commanderData.Length; i++)
        {
            CommanderData data = commanderData[i].CommanderData;
            m_commanders[data.Id.Id] = data;
        }
        
        NationDataSO[] nationsData = Resources.LoadAll<NationDataSO>("Data/Nations");
        m_nations = new Dictionary<string, NationData>(nationsData.Length);
        for (int i = 0; i < nationsData.Length; i++)
        {
            NationData data = nationsData[i].NationData;
            m_nations[data.Id.Id] = data;
        }
    }
}
