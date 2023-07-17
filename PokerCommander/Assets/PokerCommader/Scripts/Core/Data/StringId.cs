using System;
using UnityEngine;

/// <summary>
/// String id used to more easily store a id that can be used across multiple data stores
/// </summary>
[Serializable]
public struct StringId
{
    [SerializeField]
    public string Id;

    public StringId(string id)
    {
        Id = id;
    }
}
