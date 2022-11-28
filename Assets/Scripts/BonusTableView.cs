using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusTableView : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public Transform pointCardsSpawn;
    GameObject[] handCards;

    public event Action OnLeftChoiceClick;
    public event Action OnRightChoiceClick;
    public event Action OnCollectChoiceClick;
    public event Action<int> OnCardTurned;

    public Button buttonLeftChoice;
    public Button buttonRightChoice;
    public Button buttonCollectChoice;
    public TextMeshProUGUI labelHeader;
    public TextMeshProUGUI labelCondition;
    public TextMeshProUGUI labelTotalWin;

    public void Awake()
    {
        buttonCollectChoice.onClick.AddListener(delegate
        {
            ChangeControlsVisibility(false);
            OnCollectChoiceClick.Invoke();
        });
        buttonLeftChoice.onClick.AddListener(delegate
        {
            ChangeButtonsAvalibility(false);
            OnLeftChoiceClick.Invoke();
        });
        buttonRightChoice.onClick.AddListener(delegate
        {
            ChangeButtonsAvalibility(false);
            OnRightChoiceClick.Invoke();
        });
    }

    public void StartCardTurning(int idx)
    {
        handCards[idx].GetComponent<CardView>().StartTurnAnimation();
    }

    public void SpawnCardsAndPrepareButtons(CardData[] hand, BonusTableModel.BonusGameType bonus_type)
    {
        cardSpawner.SpawnHandCards(hand, pointCardsSpawn, ref handCards);
        for (int i = 0; i < hand.Length; i++)
        {
            int buf = i;
            handCards[i].GetComponent<CardView>().OnTurnFinished += (delegate { //ChangeButtonsAvalibility(true);
                                                                                OnCardTurned.Invoke(buf); });
        }

        switch (bonus_type)
        {
            case BonusTableModel.BonusGameType.BySuit:
                labelCondition.text = String.Format("Pick {0} or {1} card to double your win!", "red", "black");
                buttonLeftChoice.GetComponentInChildren<TextMeshProUGUI>().text = "Red";
                buttonRightChoice.GetComponentInChildren<TextMeshProUGUI>().text = "Black";
                break;
            case BonusTableModel.BonusGameType.ByValue:
                labelCondition.text = String.Format("Pick {0} or {1} card to double your win!", "high", "low");
                buttonLeftChoice.GetComponentInChildren<TextMeshProUGUI>().text = "Low (A-6)";
                buttonRightChoice.GetComponentInChildren<TextMeshProUGUI>().text = "High (8-K)";
                break;
        }
        ChangeControlsVisibility(true);
        ChangeButtonsAvalibility(true);
    }

    public void RefreshTotalPrizeLabel(int value)
    {
        labelTotalWin.text = "Total win $ " + value;
    }

    void ChangeControlsVisibility(bool visibility)
    {
        buttonLeftChoice.gameObject.SetActive(visibility);
        buttonRightChoice.gameObject.SetActive(visibility);
        buttonCollectChoice.gameObject.SetActive(visibility);
        labelHeader.gameObject.SetActive(visibility);
        labelCondition.gameObject.SetActive(visibility);
        labelTotalWin.gameObject.SetActive(visibility);
    }

    void ChangeButtonsAvalibility(bool avalibility)
    {
        buttonLeftChoice.interactable = avalibility;
        buttonRightChoice.interactable = avalibility;
        buttonCollectChoice.interactable = avalibility;
    }

    public void UpdateBetButtonsAvalibility(bool no_more_bets)
    {
        buttonLeftChoice.interactable = !no_more_bets;
        buttonRightChoice.interactable = !no_more_bets;
        buttonCollectChoice.interactable = true;
        if (no_more_bets)
            labelCondition.text = "No more bets!";
    }
}