using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBack 
{
    private Sprite m_cardBack;
    public Sprite CardBackSprite => m_cardBack;

    public void SetCardBack(Sprite cardBack)
    {
        m_cardBack = cardBack;
    }
}
