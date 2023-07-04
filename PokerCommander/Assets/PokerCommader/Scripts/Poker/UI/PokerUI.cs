using System.Collections.Generic;
using Siren;
using UnityEngine;

public class PokerUI : FlowScreenUI
{
    [SerializeField]
    private Transform[] m_handZones;
    [SerializeField]
    private CardHandUI m_handPrefab;

    private CardBack m_cardBack;
    private List<CardHandUI> m_cardHands = new List<CardHandUI>();
    
    public void InitUI(NationData playerNation, CommanderData[] participants, CardBack cardBack)
    {
        m_cardBack = cardBack;
        
        for (int i = 0; i < participants.Length; i++)
        {
            AllegianceType allegianceType = playerNation.Allegiances[participants[i].Allegiance.Id];
            SpawnHand((int)allegianceType, participants[i]);
        }
    }

    public void SpawnHand(int spawnZone, CommanderData commanderData)
    {
        CardHandUI handUI = Instantiate(m_handPrefab, m_handZones[spawnZone]);
        handUI.SetCharacter(commanderData.Icon, commanderData.Name);
        
        m_cardHands.Add(handUI);
    }

    public void SetCardsInHand(CardHand cardHand, int id)
    {
        m_cardHands[id].SeCards(cardHand, m_cardBack.CardBackSprite, id == 0);
    }
    
    public override void UpdateUI()
    {
    }

    public override void DestroyUI()
    {
        Destroy(gameObject);
    }
}
