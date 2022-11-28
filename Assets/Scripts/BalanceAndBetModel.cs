using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceAndBetModel
{
    public int currentBalance { get; private set; }
    public int currentBet { get; private set; }
    public int balanceUpdateComboPartBuffer { get; private set; }
    const int MIN_BET = 1;
    const int MAX_BET = 5;
    const int START_BALANCE = 30;

    public event Action<int> OnBalanceChanged;
    public event Action<int> OnBetChanged;
    public event Action<CardComboChecker.Combo, int> OnPrizeCalculated;
    public event Action OnGameOver;
    public event Action<int> OnAfterBonusBalanceUpdate;

    public void PrepareForUpdateBalanceAfterPlay(CardComboChecker.Combo combo)
    {
        int scoreUpdate = 0;
        switch (combo)
        {
            case CardComboChecker.Combo.None:
                scoreUpdate = -1;
                break;
            case CardComboChecker.Combo.JacksOrBetter:
                scoreUpdate = 1;
                break;
            case CardComboChecker.Combo.TwoPair:
                scoreUpdate = 2;
                break;
            case CardComboChecker.Combo.ThreeOfAKind:
                scoreUpdate = 3;
                break;
            case CardComboChecker.Combo.Straight:
                scoreUpdate = 4;
                break;
            case CardComboChecker.Combo.Flush:
                scoreUpdate = 6;
                break;
            case CardComboChecker.Combo.FullHouse:
                scoreUpdate = 9;
                break;
            case CardComboChecker.Combo.FourOfAKind:
                scoreUpdate = 25;
                break;
            case CardComboChecker.Combo.StraightFlush:
                scoreUpdate = 50;
                break;
            case CardComboChecker.Combo.RoyalFlush:
                scoreUpdate = 250;
                break;
        }
        if (combo == CardComboChecker.Combo.None)
        {
            scoreUpdate *= currentBet;
            currentBalance += scoreUpdate;
            OnBalanceChanged.Invoke(currentBalance);
        }
        else
        {
            balanceUpdateComboPartBuffer = scoreUpdate;
            scoreUpdate *= currentBet;
        }

        if (currentBalance <= 0)
            OnGameOver.Invoke();
        else
            OnPrizeCalculated.Invoke(combo, scoreUpdate);
    }

    public bool UpdateBalanceFromCalculatedPrizeBuffer()
    {
        currentBalance += (balanceUpdateComboPartBuffer * currentBet);
        OnBalanceChanged.Invoke(currentBalance);
        balanceUpdateComboPartBuffer = 0;

        if (currentBalance <= 0)
        {
            OnGameOver.Invoke();
            return true;
        }
        else
            return false;
    }

    public void ResetBalance()
    {
        currentBalance = START_BALANCE;
        OnBalanceChanged.Invoke(currentBalance);
    }

    public void ResetBet()
    {
        currentBet = MIN_BET;
        OnBetChanged.Invoke(currentBet);
    }

    public void ResetBalanceAndBet()
    {
        ResetBalance();
        ResetBet();
    }

    public void IncreaseBet()
    {
        if (currentBet < MAX_BET)
            currentBet++;
        else
            currentBet = MAX_BET;
        OnBetChanged.Invoke(currentBet);
    }

    public void DecreaseBet()
    {
        if (currentBet > MIN_BET)
            currentBet--;
        else
            currentBet = MIN_BET;
        OnBetChanged.Invoke(currentBet);
    }

    public void SetBalanceUpdateComboPartBuffer(int value)
    {
        balanceUpdateComboPartBuffer = value;
        OnAfterBonusBalanceUpdate.Invoke(balanceUpdateComboPartBuffer * currentBet);
    }
}
