using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
public class CardDataObject : ScriptableObject
{
    public Card[] Cards;
}
