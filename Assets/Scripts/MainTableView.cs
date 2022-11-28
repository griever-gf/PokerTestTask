using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainTableView : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public Transform pointCardsSpawn;
    GameObject[] handCards;

    public event Action OnDealClick;
    public event Action OnStartAgainClick;
    public event Action OnCollectClick;
    public event Action OnDoublePrizeClick;
    public event Action<int> OnCardClicked;
    public event Action OnCardsTurnFinished;

    public GameObject panelWon;
    public GameObject panelNoCombo;
    public GameObject panelGameOver;
    public GameObject panelCollectAfterBonusGame;
    public Button buttonDeal;
    public Button buttonStartGameAgain;
    public Button buttonCollectPrize;
    public Button buttonDoublePrize;
    public Button buttonCollectAfterBonus;

    public void Awake()
    {
        buttonDeal.onClick.AddListener(DealButtonPressed);
        buttonStartGameAgain.onClick.AddListener(StartGameAgainPressed);
        buttonCollectPrize.onClick.AddListener(CollectButtonPressed);
        buttonDoublePrize.onClick.AddListener(DoublePrizePressed);
        buttonCollectAfterBonus.onClick.AddListener(CollectButtonPressed);
    }

    public void UpdateHandCards(CardData[] hand, MainTableModel.PlayPhase phase)
    {
        bool deactivateUIDuringTurnFlag = false;
        HideAllPossiblePopups();
        if (phase != MainTableModel.PlayPhase.NewDistributionAfterBet)
        {
            cardSpawner.SpawnHandCards(hand, pointCardsSpawn, ref handCards);
            for (int i = 0; i < hand.Length; i++)
                if (!hand[i].GetIsHolded())
                {
                    //add onclick event if card in the first phase
                    if ((phase == MainTableModel.PlayPhase.NewDistribution) || (phase == MainTableModel.PlayPhase.BetSetting))
                    {
                        int buf = i;
                        handCards[i].GetComponent<CardView>().OnClick += delegate { OnCardClicked.Invoke(buf); };
                    }
                    //start turn animation if not the bet set phase
                    if (phase != MainTableModel.PlayPhase.BetSetting)
                    {
                        if (!deactivateUIDuringTurnFlag)
                        {
                            handCards[i].GetComponent<CardView>().OnTurnFinished += TurnFisnishedEvent;
                            deactivateUIDuringTurnFlag = true;
                        }
                        handCards[i].GetComponent<CardView>().StartTurnAnimation();
                    }
                }
        }
        else
        {
            deactivateUIDuringTurnFlag = true;
            handCards[0].GetComponent<CardView>().OnTurnFinished += TurnFisnishedEvent;
            for (int i = 0; i < hand.Length; i++)
                handCards[i].GetComponent<CardView>().StartTurnAnimation();
        }
        buttonDeal.interactable = !deactivateUIDuringTurnFlag;
        if ((phase == MainTableModel.PlayPhase.AdditionToHoldedCards) && !deactivateUIDuringTurnFlag)
            TurnFisnishedEvent();

    }

    public void UpdateHandCardHoldStatus(int idx, bool visibility)
    {
        handCards[idx].GetComponent<CardView>().ChangeHoldLabelVisibility(visibility);
    }

    public void SpawnResultsPopup(CardComboChecker.Combo combo, int prize)
    {
        if (combo == CardComboChecker.Combo.None)
        {
            panelNoCombo.SetActive(true);
            buttonDeal.interactable = true;
        }
        else
        {
            panelWon.SetActive(true);
            buttonDeal.interactable = false;
            SetTextOnPanel(panelWon, "Victory!" + Environment.NewLine + combo.ToString() + Environment.NewLine + "Prize: " + prize.ToString());
        }
    }

    void SetTextOnPanel(GameObject panel, string value)
    {
        TextMeshProUGUI txtLabel = null;
        TextMeshProUGUI[] children = panel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI c in children)
            if (c.transform.parent == panel.transform)
            {
                txtLabel = c;
                break;
            }
        if (txtLabel != null)
            txtLabel.text = value;
    }

    public void SpawnAfterBonusPopup(int bonus)
    {
        ShowAllUIControls();
        panelCollectAfterBonusGame.SetActive(true);
        SetTextOnPanel(panelCollectAfterBonusGame, "Bonus game result: " + Environment.NewLine + "$ " + bonus.ToString());
    }

    public void SpawnGameOverPopup()
    {
        buttonDeal.interactable = false;
        panelGameOver.SetActive(true);
    }

    public void SpawnGameOverPopupAfterBonusLose()
    {
        HideAllPossiblePopups();
        SpawnGameOverPopup();
    }

    public void DealButtonPressed()
    {
        buttonDeal.interactable = false;
        OnDealClick.Invoke();
    }

    public void CollectButtonPressed()
    {
        OnCollectClick.Invoke();
    }

    public void StartGameAgainPressed()
    {
        OnStartAgainClick.Invoke();
    }

    public void DoublePrizePressed()
    {
        HideAllUIControls();
        OnDoublePrizeClick.Invoke();
    }

    void HidePopup(GameObject popup)
    {
        if (popup.activeSelf)
            popup.SetActive(false);
    }

    void HideAllPossiblePopups()
    {
        HidePopup(panelNoCombo);
        HidePopup(panelWon);
        HidePopup(panelGameOver);
        HidePopup(panelCollectAfterBonusGame);
    }

    void TurnFisnishedEvent()
    {
        OnCardsTurnFinished.Invoke();
    }

    public void UpdateDealButtonAvalibility(MainTableModel.PlayPhase phase)
    {
        switch (phase)
        {
            case MainTableModel.PlayPhase.BetSetting:
            case MainTableModel.PlayPhase.NewDistributionAfterBet:
            case MainTableModel.PlayPhase.NewDistribution:
                buttonDeal.interactable = true;
                break;
        }
    }

    public void HideAllUIControls()
    {
        HideAllPossiblePopups();
        buttonDeal.gameObject.SetActive(false);
    }

    public void ShowAllUIControls()
    {
        buttonDeal.gameObject.SetActive(true);
    }
}
