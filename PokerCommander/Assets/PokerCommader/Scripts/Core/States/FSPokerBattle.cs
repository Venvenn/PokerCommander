using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSPokerBattle : FlowState
{
    private PokerUI m_ui;
    private UIManager m_uiManager;

    private CardSystem m_cardSystem;
    private CardInteractionManager m_cardInteractionManager;

    private GameObject m_cardObjectPrefab;
    private CardDataObject m_cardData;
    private CardBacksDataObject m_cardBacksData;

    private BattleData m_battleData;
    private ContentDatabase m_contentDatabase;

    private CommanderData[] m_participants;
    private CardBack m_cardBack;
    
    public FSPokerBattle(GameContext gameContext, BattleData battleData)
    {
        Debug.Assert(battleData.Participants != null && battleData.Participants.Length > 0, "Battle Has No Participants!");
        
        //UI
        m_uiManager = gameContext.UIManager;
        m_cardSystem = new CardSystem();
        m_cardInteractionManager = new CardInteractionManager(m_cardSystem, battleData.Participants.Length);
        
        //Data 
        m_battleData = battleData;
        m_contentDatabase = gameContext.ContentDatabase;
        
        m_cardObjectPrefab = Resources.Load<GameObject>("Prefabs/BaseCard");
        m_cardData = Resources.Load<CardDataObject>("Data/CardData");
        m_cardBacksData = Resources.Load<CardBacksDataObject>("Data/CardBackData");
        m_cardBack = new CardBack();
        m_cardBack.SetCardBack(m_cardBacksData.CardBack[0]);
    }

    public override void OnInitialise()
    {
        m_participants = new CommanderData[m_battleData.Participants.Length];

        for (int i = 0; i < m_participants.Length; i++)
        {
            m_participants[i] = m_contentDatabase.m_commanders[m_battleData.Participants[i].Id];
        }

        NationData playerNation = m_contentDatabase.m_nations[m_participants[0].Allegiance.Id];
        
        m_ui = m_uiManager.LoadUIScreen<PokerUI>("UI/Screens/PokerUI", this);
        m_ui.InitUI(playerNation, m_participants, m_cardBack);
    }

    public override void OnActive()
    {
        DealHand();
    }

    private void DealHand()
    {
        for (int i = 0; i < m_participants.Length; i++)
        {
            m_ui.SetCardsInHand(m_cardInteractionManager.m_cardHand[i], i);
        }
    }

    public override void ActiveUpdate()
    {
        m_ui.UpdateUI();
    }
    
    public override void ActiveFixedUpdate()
    {
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
    }

    public override void FinishDismiss()
    {
        m_ui.DestroyUI();
    }
}
