using System;
using System.Collections.Generic;
using Siren;
using UnityEngine;

public class TexasHoldemPokerUI : FlowScreenUI
{
    [SerializeField]
    private Transform[] m_handZones;
    
    [SerializeField]
    private CardTableUI m_cardTable;
    
    [SerializeField]
    private CardHandUI m_handPrefab;

    [SerializeField]
    private CommandZoneUI m_commandZoneUi;
    
    
    private CardBack m_cardBack;
    private List<CardHandUI> m_cardHands = new List<CardHandUI>();
    
    public void InitUI(NationData playerNation, CombatCommanderData[] participants, CardBack cardBack)
    {
        m_commandZoneUi.gameObject.SetActive(false);
        m_cardBack = cardBack;
        
        for (int i = 0; i < participants.Length; i++)
        {
            AllegianceType allegianceType = playerNation.Allegiances[participants[i].Data.Allegiance.Id];
            SpawnHand((int)allegianceType, participants[i].Data);
        }
    }

    public void SpawnHand(int spawnZone, CommanderData commanderData)
    {
        CardHandUI handUI = Instantiate(m_handPrefab, m_handZones[spawnZone]);
        handUI.SetCharacter(commanderData.Icon, commanderData.Name);
        
        m_cardHands.Add(handUI);
    }
    
    public void SetCardsInHands(CardHand[] cardHand)
    {
        for (int i = 0; i < cardHand.Length; i++)
        {
            m_cardHands[i].SeCards(cardHand[i], m_cardBack.CardBackSprite, i == 0);
        }
    }
    
    public void SetCardsInTable(CardTable cardTable)
    {
        m_cardTable.SetCards(cardTable);
    }

    public void EnableCommandZone()
    {
        m_commandZoneUi.gameObject.SetActive(true);
    }
    
    public void DisableCommandZone()
    {
        m_commandZoneUi.gameObject.SetActive(false);
    }

    public void Reset()
    {
        m_cardTable.ClearTable();
    }

    public override void UpdateUI()
    {
    }

    public override void DestroyUI()
    {
        Destroy(gameObject);
    }
}
