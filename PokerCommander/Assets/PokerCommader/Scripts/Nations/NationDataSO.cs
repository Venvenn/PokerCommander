using UnityEngine;

/// <summary>
/// Scriptable object used to store nation data
/// </summary>
[CreateAssetMenu(menuName = "Data/NationData", fileName = "NationData")]
public class NationDataSO : ScriptableObject
{
    public NationData NationData;
}
