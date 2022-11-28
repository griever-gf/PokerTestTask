using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BalanceAndBetView : MonoBehaviour
{
    public TextMeshProUGUI labelBalance;
    public TextMeshProUGUI labelBet;
    public Button buttonBetDecrease;
    public Button buttonBetIncrease;

    public event Action OnBetDescrease;
    public event Action OnBetIncrease;

    public void Awake()
    {
        buttonBetDecrease.onClick.AddListener(delegate { OnBetDescrease.Invoke(); });
        buttonBetIncrease.onClick.AddListener(delegate { OnBetIncrease.Invoke(); });
    }

    public void UpdateBalance(int value)
    {
        labelBalance.text = "Balance: " + value.ToString();
    }

    public void UpdateBet(int value)
    {
        labelBet.text = "Bet: " + value.ToString();
    }

    public void EnableBetButtons()
    {
        buttonBetDecrease.interactable = true;
        buttonBetIncrease.interactable = true;
    }

    public void DisableBetButtons()
    {
        buttonBetDecrease.interactable = false;
        buttonBetIncrease.interactable = false;
    }

    public void SetBetButtonsAvailibility(bool val)
    {
        buttonBetDecrease.interactable = val;
        buttonBetIncrease.interactable = val;
    }

    public void ChangeBetButtonsAvailibility(MainTableModel.PlayPhase phase)
    {
        switch (phase)
        {
            case MainTableModel.PlayPhase.BetSetting:
                EnableBetButtons();
                return;
            case MainTableModel.PlayPhase.NewDistribution:
            case MainTableModel.PlayPhase.NewDistributionAfterBet:
                DisableBetButtons();
                return;
            default:
                return;
        }
    }

    public void ChangeControlsVisibility(bool visibility)
    {
        labelBalance.gameObject.SetActive(visibility);
        labelBet.gameObject.SetActive(visibility);
        buttonBetDecrease.gameObject.SetActive(visibility);
        buttonBetIncrease.gameObject.SetActive(visibility);
    }
}
