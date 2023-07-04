using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
