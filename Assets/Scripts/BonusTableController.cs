using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BonusTableView))]
public class BonusTableController : MonoBehaviour
{
    public BonusTableView view { get; private set; }
    public BonusTableModel model { get; private set; }
    public GameController gameController;
    BalanceAndBetModel balanceAndBetModel;
    MainTableView mainTableView;

    public void AttachViewAndModel()
    {
        model = new BonusTableModel();
        view = GetComponent<BonusTableView>();

        model.OnHandFilled += view.SpawnCardsAndPrepareButtons;
        model.OnBonusScoreChanged += view.RefreshTotalPrizeLabel;
        model.OnCurrentCardRequest += view.StartCardTurning;
        model.OnBonusStageCalculated += view.UpdateBetButtonsAvalibility;
        model.OnBonusGameEnd += balanceAndBetModel.SetBalanceUpdateComboPartBuffer;
        model.OnBonusGameEnd += (val => gameController.SwitchToStandardTable());

        view.OnCollectChoiceClick += model.CollectScores;
        view.OnLeftChoiceClick += (() => model.SetCheckForBonus(true));
        view.OnRightChoiceClick += (() => model.SetCheckForBonus(false));
        view.OnCardTurned += model.CalculateBonusStage;
    }

    public void SetExtraLinks(BalanceAndBetModel bb_model, MainTableView mt_view)
    {
        balanceAndBetModel = bb_model;
        mainTableView = mt_view;
    }

    public void StartBonusGame()
    {
        model.BonusGameDataPreparations(balanceAndBetModel.balanceUpdateComboPartBuffer, balanceAndBetModel.currentBet);
    }
}
