using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTableModel: TableWithHand
{
    public event Action<CardData[], BonusGameType> OnHandFilled;
    public event Action<int> OnBonusScoreChanged;
    public event Action<int> OnBonusGameEnd;
    public event Action<bool> OnBonusStageCalculated;
    public event Action<int> OnCurrentCardRequest;

    public enum BonusGameType { BySuit, ByValue };
    BonusGameType bonusGameType;
    int scoreMultiplier;
    //int baseScore;
    int baseComboScore;
    int baseBet;
    enum CheckForBonus { None, Left, Right };
    CheckForBonus currentCheckForBonus;
    int currentCardIndex;

    public void BonusGameDataPreparations(int base_combo_score, int base_bet)
    {
        tableDeck.FillDeck();
        FillHand();

        System.Random rnd = new System.Random();
        Array values = Enum.GetValues(typeof(BonusGameType));
        bonusGameType = (BonusGameType)values.GetValue(rnd.Next(values.Length));

        OnHandFilled.Invoke(handCards, bonusGameType);
        scoreMultiplier = 1;
        baseComboScore = base_combo_score;
        baseBet = base_bet;
        OnBonusScoreChanged.Invoke(baseBet * baseComboScore * scoreMultiplier);
        currentCardIndex = 0;
    }

    public void CollectScores()
    {
        if (scoreMultiplier != -1)
            OnBonusGameEnd.Invoke(baseComboScore * scoreMultiplier);
        else
            OnBonusGameEnd.Invoke(scoreMultiplier);
    }

    public void SetCheckForBonus(bool is_left)
    {
        if (is_left)
            currentCheckForBonus = CheckForBonus.Left;
        else
            currentCheckForBonus = CheckForBonus.Right;
        OnCurrentCardRequest.Invoke(currentCardIndex);
        currentCardIndex++;
    }

    public void CalculateBonusStage(int card_idx)
    {
        CardSuit suit = handCards[card_idx].GetSuit();
        CardValue val = handCards[card_idx].GetValue();
        bool isBonus = false;
        if ((currentCheckForBonus == CheckForBonus.Left)&&(bonusGameType == BonusGameType.BySuit))
        {
            isBonus = ((suit == CardSuit.Diamonds) || (suit == CardSuit.Hearts));
        }
        else if ((currentCheckForBonus == CheckForBonus.Right) && (bonusGameType == BonusGameType.BySuit))
        {
            isBonus = ((suit == CardSuit.Clovers) || (suit == CardSuit.Spades));
        }
        else if ((currentCheckForBonus == CheckForBonus.Left) && (bonusGameType == BonusGameType.ByValue))
        {
            isBonus = ((val < CardValue.Seven) || (val == CardValue.Ace));
        }
        else if ((currentCheckForBonus == CheckForBonus.Right) && (bonusGameType == BonusGameType.ByValue))
        {
            isBonus = ((val > CardValue.Seven) && (val < CardValue.Ace));
        }
        if (isBonus)
        {
            scoreMultiplier *= 2;
            OnBonusScoreChanged.Invoke(baseBet * baseComboScore * scoreMultiplier);
        }
        else
        {
            scoreMultiplier = -1;
            OnBonusScoreChanged.Invoke(baseBet * scoreMultiplier);
        }
        bool noMoreMultiplyingAttempts = (scoreMultiplier == -1) || (card_idx == CARD_NUMBER - 1);
        OnBonusStageCalculated.Invoke(noMoreMultiplyingAttempts);
    }
}
