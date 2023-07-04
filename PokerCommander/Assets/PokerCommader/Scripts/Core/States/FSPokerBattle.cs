using Siren;
using UnityEngine;

public class FSPokerBattle : FlowState
{
    private TexasHoldemPokerUI m_ui;
    private UIManager m_uiManager;

    private Deck m_deck;
    private TexasHoldemInteractionManager m_texasHoldemInteractionManager;

    private GameObject m_cardObjectPrefab;
    private CardDataObject m_cardData;
    private CardBacksDataObject m_cardBacksData;
    private SeededRandom m_seededRandom;
    
    private BattleData m_battleData;
    private ContentDatabase m_contentDatabase;

    private CombatCommanderData[] m_participants;
    private CardBack m_cardBack;
    private PokerPhase m_currentPhase;

    private int m_dealChipId = -1;
    
    public FSPokerBattle(GameContext gameContext, BattleData battleData)
    {
        Debug.Assert(battleData.Participants != null && battleData.Participants.Length > 0, "Battle Has No Participants!");
        
        m_deck = new Deck(gameContext.SeededRandom);
        m_seededRandom = gameContext.SeededRandom;
        
        //UI
        m_uiManager = gameContext.UIManager;
        m_texasHoldemInteractionManager = new TexasHoldemInteractionManager(m_deck, battleData.Participants.Length);
        
        //Data 
        m_battleData = battleData;
        m_contentDatabase = gameContext.ContentDatabase;

        
        m_cardObjectPrefab = Resources.Load<GameObject>("Prefabs/BaseCard");
        m_cardData = Resources.Load<CardDataObject>("Data/CardData");
        m_cardBacksData = Resources.Load<CardBacksDataObject>("Data/CardBackData");
        m_cardBack = new CardBack();
        m_cardBack.SetCardBack(m_cardBacksData.CardBack[0]);
        
        m_currentPhase = PokerPhase.DealHands;
    }

    public override void OnInitialise()
    {
        m_participants = new CombatCommanderData[m_battleData.Participants.Length];

        for (int i = 0; i < m_participants.Length; i++)
        {
            m_participants[i].Data = m_contentDatabase.m_commanders[m_battleData.Participants[i].Id];
            m_participants[i].ActiveInRound = true;
        }

        m_dealChipId = m_seededRandom.Random.NextInt(0, m_participants.Length);
        
        NationData playerNation = m_contentDatabase.m_nations[m_participants[0].Data.Allegiance.Id];
        
        m_ui = m_uiManager.LoadUIScreen<TexasHoldemPokerUI>("UI/Screens/TexasHoldemPokerUI", this);
        m_ui.InitUI(playerNation, m_participants, m_cardBack);
    }

    public override void OnActive()
    {
        RunRoundPhase();
    }

    public void RunRoundPhase()
    {
        switch (m_currentPhase)
        {
            case PokerPhase.DealHands:
                m_texasHoldemInteractionManager.DealHand();
                m_ui.SetCardsInHands(m_texasHoldemInteractionManager.m_cardHand);
                m_currentPhase = PokerPhase.Blinds;
                break;
            case PokerPhase.Blinds:
                PayBlinds();
                m_currentPhase = PokerPhase.Preflop;
                break;
            case PokerPhase.Preflop:
                //Bet
                break;
            case PokerPhase.Flop:
                m_texasHoldemInteractionManager.DealFlop();
                //Bet
                break;
            case PokerPhase.Turn:
                m_texasHoldemInteractionManager.DealTurn();
                //Bet
                break;
            case PokerPhase.River:
                m_texasHoldemInteractionManager.DealRiver();
                //Bet
                break;
            case PokerPhase.Reset:
                
                break;
        }
    }
    
    public void RunBetPhase()
    {
        //Player 
        if (m_dealChipId == 0) 
        {
            //Do nothing
        }
        
        //Temp Ai
    }
    

    private void PayBlinds()
    {
        int smallBlindId = PokerUtility.GetNextPlayerId(m_dealChipId, m_participants);
        int bigBlindId = PokerUtility.GetNextPlayerId(smallBlindId, m_participants);
        
        //pay blinds
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