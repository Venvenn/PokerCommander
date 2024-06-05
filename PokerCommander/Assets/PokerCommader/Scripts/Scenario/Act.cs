using UnityEngine;

[CreateAssetMenu(fileName = "Act", menuName = "Data/Act")]
public class Act : ScriptableObject
{
    public string ActName;
    public Sprite Background;
    public ChapterGraph[] Chapters;
}
