using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MainTableView))]
public class MainTableController: MonoBehaviour
{
    public MainTableView view { get; private set; }
    public MainTableModel model { get; private set; }
    public GameController gameController;  
    BalanceAndBetModel balanceAndBetModel;
    BalanceAndBetView balanceAndBetView;

    public void SetExtraLinks(BalanceAndBetModel bb_model, BalanceAndBetView bb_view)
    {
        balanceAndBetModel = bb_model;
        balanceAndBetView = bb_view;
    }

    public void AttachViewsAndModels()
    {
        model = new MainTableModel();
        view = GetComponent<MainTableView>();

        model.OnHandRefreshed += view.UpdateHandCards;
        model.OnHandRefreshed += ((card_data, phase) => balanceAndBetView.ChangeBetButtonsAvailibility(phase));
        model.OnHandCardHoldStatusChange += view.UpdateHandCardHoldStatus;
        model.OnHandFinalized += balanceAndBetModel.PrepareForUpdateBalanceAfterPlay;
        model.OnPhaseCheck += view.UpdateDealButtonAvalibility;

        view.OnCardsTurnFinished += model.CheckForPhaseAndCombo;
        view.OnCardClicked += model.ChangeHandCardHoldStatus;
        view.OnDealClick += model.SupplyHandWithRandomCards;
        view.OnStartAgainClick += GameStartPreparations;
        view.OnCollectClick += CollectBonusAndSupplyHandIfNotGameOver;
        view.OnDoublePrizeClick += gameController.SwitchToBonusTable;

        balanceAndBetModel.OnPrizeCalculated += view.SpawnResultsPopup;
        balanceAndBetModel.OnGameOver += view.SpawnGameOverPopup;
        balanceAndBetModel.OnAfterBonusBalanceUpdate += view.SpawnAfterBonusPopup;

        balanceAndBetView.OnBetDescrease += model.TryToSwitchToBetMode;
        balanceAndBetView.OnBetIncrease += model.TryToSwitchToBetMode;
        
    }

    public void GameStartPreparations()
    {
        balanceAndBetModel.ResetBalanceAndBet();
        model.SwitchToBetMode();
    }

    void CollectBonusAndSupplyHandIfNotGameOver()
    {
        bool isGameOver = balanceAndBetModel.UpdateBalanceFromCalculatedPrizeBuffer();
        if (!isGameOver)
            model.SwitchToBetMode();
        else
            view.SpawnGameOverPopupAfterBonusLose();
    }
}
