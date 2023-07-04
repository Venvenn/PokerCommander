using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct Card
{
    public Sprite Sprite;
    public string Name;
    public int Value;
    
    [HideInInspector]
    public int Id;
}
