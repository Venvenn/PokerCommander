using UnityEngine;

/// <summary>
/// Scriptable object used to store commander data
/// </summary>
[CreateAssetMenu(menuName = "Data/CommanderData", fileName = "CommanderData")]
public class CommanderDataSO : ScriptableObject
{
    public CommanderData CommanderData;
}
