using Siren;
using UnityEngine;

public class FSPokerBets : FlowState
{
    private PokerBetsUI m_ui;
    private UIManager m_uiManager;
    
    private CardBacksDataObject m_cardBacksData;
    private SeededRandom m_seededRandom;
    
    private ContentDatabase m_contentDatabase;

    private CombatCommanderData[] m_participants;
    private CardBack m_cardBack;
    private PokerPhase m_currentPhase;

    private int m_turnToBet = -1;
    
    public FSPokerBets(GameContext gameContext, CombatCommanderData[] participants)
    {
        
        m_seededRandom = gameContext.SeededRandom;
        
        //UI
        m_uiManager = gameContext.UIManager;

        //Data 
        m_participants = participants;
        m_contentDatabase = gameContext.ContentDatabase;
        
        m_cardBacksData = Resources.Load<CardBacksDataObject>("Data/CardBackData");
        m_cardBack = new CardBack();
        m_cardBack.SetCardBack(m_cardBacksData.CardBack[0]);
        
        m_currentPhase = PokerPhase.DealHands;
    }

    public override void OnInitialise()
    {
        m_ui = m_uiManager.LoadUIScreen<PokerBetsUI>("UI/Screens/PokerBetsUI", this);
        m_ui.InitUI();
    }

    public override void OnActive()
    {
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
