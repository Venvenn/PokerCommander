using Siren;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class FSPokerBattle : FlowState
{
    private const int k_startBlindValue = 1;
    
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
    private Pot m_pot;
    
    private int m_dealChipId = -1;
    
    private int m_currentBetId = -1;

    
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
        m_ui = m_uiManager.LoadUIScreen<TexasHoldemPokerUI>("UI/Screens/TexasHoldemPokerUI", this);

        m_participants = new CombatCommanderData[m_battleData.Participants.Length];

        for (int i = 0; i < m_participants.Length; i++)
        {
            m_participants[i].Data = m_contentDatabase.m_commanders[m_battleData.Participants[i].Id];
            m_participants[i].ActiveInRound = true;
            m_participants[i].Currency = 1000;
            m_participants[i].BetState = BetState.In;
        }

        m_dealChipId = m_seededRandom.Random.NextInt(0, m_participants.Length);

        NationData playerNation = m_contentDatabase.m_nations[m_participants[0].Data.Allegiance.Id];
        m_ui.InitUI(playerNation, m_participants, m_cardBack);
        m_ui.CommandZoneUI.SetTextValue(m_pot.m_currentBetValue);
        for (int i = 0; i < m_participants.Length; i++)
        {
            m_ui.SetHandsCurrency(i, m_participants[i].Currency);
        }
    }

    public override void OnActive()
    {
        m_currentBetId = m_dealChipId;
        m_currentBetId = m_dealChipId;
        RunRoundPhase();
    }

    public void RunRoundPhase()
    {
        switch (m_currentPhase)
        {
            case PokerPhase.DealHands:
                m_pot = new Pot(k_startBlindValue * 2);
                m_texasHoldemInteractionManager.DealHand();
                m_ui.SetCardsInHands(m_texasHoldemInteractionManager.m_cardHand);
                m_currentPhase = PokerPhase.Blinds;
                RunRoundPhase();
                break;
            case PokerPhase.Blinds:
                PayBlinds();
                m_currentPhase = PokerPhase.Preflop;
                RunRoundPhase();
                break;
            case PokerPhase.Preflop:
                EnterBetPhase();
                break;
            case PokerPhase.Flop:
                m_texasHoldemInteractionManager.DealFlop();
                m_ui.SetCardsInTable(m_texasHoldemInteractionManager.m_cardTable);
                EnterBetPhase();
                break;
            case PokerPhase.Turn:
                m_texasHoldemInteractionManager.DealTurn();
                m_ui.SetCardsInTable(m_texasHoldemInteractionManager.m_cardTable);
                EnterBetPhase();
                break;
            case PokerPhase.River:
                m_texasHoldemInteractionManager.DealRiver();
                m_ui.SetCardsInTable(m_texasHoldemInteractionManager.m_cardTable);
                EnterBetPhase();
                break;
            case PokerPhase.Reset:
                Reset();
                RunRoundPhase();
                break;
        }
        
    }

    private async void Reset()
    {
        await Task.Delay(5000);
        
        m_deck.ResetDeck();
        m_ui.Reset();
        
        for (int i = 0; i < m_participants.Length; i++)
        {
            m_participants[i].ActiveInRound = true;
            m_participants[i].BetState = BetState.In;
        }
        
        m_dealChipId = PokerUtility.GetNextPlayerId(m_dealChipId, m_participants);
        
        m_texasHoldemInteractionManager.Reset();
        m_currentPhase = PokerPhase.DealHands;
    }
    
    
    private void EnterBetPhase()
    {
        m_currentBetId = m_dealChipId;

        for (int i = 0; i < m_participants.Length; i++)
        {
            m_participants[i].BetState = m_participants[i].ActiveInRound ? m_participants[i].BetState : BetState.Out;
        }

        RunBetPhase();
    }
    
    private async void RunBetPhase()
    {
        bool stillIn = true;

        while (stillIn)
        {
            stillIn = false;
            bool wentRound = false;
            for (int i = m_currentBetId, j = m_participants.Length; i < j; i++)
            {
                if (m_participants[i].ActiveInBet(m_pot.m_currentBetValue))
                {
                    stillIn = true;
                    
                    m_pot.m_consideredBetValue = m_pot.m_currentBetValue;
                    
                    //Player 
                    if (i == 0)
                    {
                        m_currentBetId = i;
                        m_ui.EnableCommandZone();
                        m_ui.CommandZoneUI.UpdateCurrencyValue(m_pot.m_consideredBetValue - m_participants[i].BetThisRound);
                        return;
                    }
                    RunAIBet(i);
                    await Task.Delay(2000);
                }
                
                if (!wentRound && i + 1 >= j)
                {
                    wentRound = true;
                    i = -1;
                    j = m_currentBetId;
                }
            }
        }

        m_ui.DisableCommandZone();
        m_currentPhase++;

        for (int i = 0; i < m_participants.Length; i++)
        {
            m_participants[i].BetState = m_participants[i].BetState = m_participants[i].ActiveInRound ? BetState.In : BetState.Out;
            m_participants[i].BetThisRound = 0;
        }

        m_pot.m_consideredBetValue = 0;
        m_pot.m_currentBetValue = 0;
        RunRoundPhase();
    }


    private void RunAIBet(int id)
    {
        if (m_participants[id].Currency > 0)
        {
            if (m_pot.m_currentBetValue == 0)
            {
                m_participants[id].BetState = BetState.Checked;
            }
            else
            {
                m_participants[id].BetState = BetState.Called;
                Bet(id, m_pot.m_consideredBetValue);
            }
        }
        else
        {
            //Temp Ai, Move On TODO: AI
            m_participants[id].BetState = BetState.Folded;
        }
    }

    private void PayBlinds()
    {
        int smallBlindId = PokerUtility.GetNextPlayerId(m_dealChipId, m_participants);
        int bigBlindId = PokerUtility.GetNextPlayerId(smallBlindId, m_participants);
        
        //pay blinds
        Bet(smallBlindId, Mathf.FloorToInt(m_pot.m_currentBlindValue * 0.5f));
        m_participants[smallBlindId].BetState = BetState.In;
        Bet(bigBlindId, m_pot.m_currentBlindValue);
        m_participants[bigBlindId].BetState = BetState.Checked;

        m_pot.m_currentBetValue = m_pot.m_currentBlindValue;
        m_currentBetId = PokerUtility.GetNextPlayerId(smallBlindId, m_participants);;
        m_ui.CommandZoneUI.UpdateCurrencyValue(m_pot.m_currentBetValue);
    }

    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "raise":
                m_ui.CommandZoneUI.ToggleBetSetterActive(true);
                m_participants[m_currentBetId].BetState = BetState.Raised;
                break;
            case "bet":
                m_ui.CommandZoneUI.ToggleBetSetterActive(true);
                m_participants[m_currentBetId].BetState = BetState.Bet;
                break;
            case "check":
                m_participants[m_currentBetId].BetState = BetState.Checked;
                AdvanceBet();
                break;
            case "fold":
                m_participants[m_currentBetId].BetState = BetState.Folded;
                m_participants[m_currentBetId].ActiveInRound = false;
                AdvanceBet();
                break;
            case "call":
                m_pot.m_consideredBetValue = m_pot.m_currentBetValue - m_participants[m_currentBetId].BetThisRound;
                m_participants[m_currentBetId].BetState = BetState.Called;
                AdvanceBet();
                break;
            case "increaseBet":
                m_pot.m_consideredBetValue = Mathf.Min(m_participants[m_currentBetId].Currency,m_pot.m_consideredBetValue + m_pot.m_currentBlindValue);
                break;
            case "decreaseBet":
                m_pot.m_consideredBetValue = Mathf.Max(m_pot.m_currentBlindValue,m_pot.m_consideredBetValue - m_pot.m_currentBlindValue);
                break;
            case "resetBet":
                m_pot.m_consideredBetValue = m_pot.m_currentBlindValue;
                m_ui.CommandZoneUI.UpdateCurrencyValue(m_pot.m_consideredBetValue);
                break;
            case "confirmBet":
                m_ui.CommandZoneUI.ToggleBetSetterActive(false);
                AdvanceBet();
                break;
            case "cancelBet":
                m_participants[m_currentBetId].BetState = BetState.In;
                m_pot.m_consideredBetValue = m_pot.m_currentBetValue = m_pot.m_consideredBetValue;
                m_ui.CommandZoneUI.ToggleBetSetterActive(false);
                break;
            case SliderFlowMessage sliderFlowMessage:
                m_pot.m_consideredBetValue = (int) Mathf.Lerp(m_pot.m_currentBetValue, m_participants[m_currentBetId].Currency, sliderFlowMessage.SliderValue);
                m_ui.CommandZoneUI.UpdateCurrencyValue(m_pot.m_consideredBetValue);
                break;
        }
    }

    private void Bet(int participantId, int currencyValue)
    {
        m_participants[participantId].Currency -= currencyValue;
        m_participants[participantId].BetThisRound += currencyValue;
        m_pot.m_potValue += currencyValue;
        
        m_ui.SetHandsCurrency(participantId, m_participants[participantId].Currency);
        m_ui.SetPotCurrency(m_pot.m_potValue);
    }

    private void AdvanceBet()
    {
        Bet(m_currentBetId, m_pot.m_consideredBetValue);
        m_pot.m_currentBetValue = m_pot.m_consideredBetValue;
        
        m_currentBetId = PokerUtility.GetNextPlayerId(m_currentBetId, m_participants);
        RunBetPhase();
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
