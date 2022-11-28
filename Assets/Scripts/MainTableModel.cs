using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTableModel: TableWithHand
{
    public event Action<CardData[], PlayPhase> OnHandRefreshed;
    public event Action<int, bool> OnHandCardHoldStatusChange;
    public event Action<CardComboChecker.Combo> OnHandFinalized;
    public event Action<PlayPhase> OnPhaseCheck;

    public enum PlayPhase {NewDistribution, AdditionToHoldedCards, Start, BetSetting, NewDistributionAfterBet };
    PlayPhase phase;

    public void SupplyHandWithRandomCards()
    {
        switch (phase)
        {
            case PlayPhase.Start:
                phase = PlayPhase.BetSetting;
                tableDeck.FillDeck();
                break;
            case PlayPhase.BetSetting:
                phase = PlayPhase.NewDistributionAfterBet;
                break;
            case PlayPhase.AdditionToHoldedCards:
                phase = PlayPhase.NewDistribution;
                tableDeck.FillDeck();
                break;
            case PlayPhase.NewDistribution:
            case PlayPhase.NewDistributionAfterBet:
                phase = PlayPhase.AdditionToHoldedCards;
                break;
        }

        if (phase != PlayPhase.NewDistributionAfterBet)
            FillHand();
        OnHandRefreshed.Invoke(handCards, phase);

        if (phase == PlayPhase.AdditionToHoldedCards)
        {
            for (int i = 0; i < CARD_NUMBER; i++)
                if (handCards[i].GetIsHolded())
                {
                    handCards[i].SetHoldStatusFree();
                    OnHandCardHoldStatusChange.Invoke(i, false);
                }
        }
    }

    public void CheckForPhaseAndCombo()
    {
        if (phase == PlayPhase.AdditionToHoldedCards)
        {
            CardComboChecker.Combo combo = CardComboChecker.CheckForCombo(handCards);
            OnHandFinalized.Invoke(combo);
        }
        OnPhaseCheck.Invoke(phase);
    }

    public void ChangeHandCardHoldStatus(int index)
    {
        switch (phase)
        {
            case PlayPhase.NewDistribution:
            case PlayPhase.NewDistributionAfterBet:
                handCards[index].ReverseHoldStatus();
                OnHandCardHoldStatusChange.Invoke(index, handCards[index].GetIsHolded());
                break;
        }
    }

    public void TryToSwitchToBetMode()
    {
        switch (phase)
        {
            case PlayPhase.AdditionToHoldedCards:
                SwitchToBetMode();
                break;
        }
    }

    public void SwitchToBetMode()
    {
        phase = PlayPhase.Start;
        SupplyHandWithRandomCards();
    }
}