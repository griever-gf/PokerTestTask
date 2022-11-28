using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BalanceAndBetView))]
public class BalanceAndBetController : MonoBehaviour
{
    public BalanceAndBetView view { get; private set; }
    public BalanceAndBetModel model { get; private set; }

    public void AttachViewAndModel()
    {
        model = new BalanceAndBetModel();
        view = GetComponent<BalanceAndBetView>();

        model.OnBalanceChanged += view.UpdateBalance;
        model.OnBetChanged += view.UpdateBet;
        view.OnBetDescrease += model.DecreaseBet;
        view.OnBetIncrease += model.IncreaseBet;
        model.OnGameOver += view.DisableBetButtons;
        model.OnPrizeCalculated += (combo, score_upd) => view.SetBetButtonsAvailibility(score_upd <= 0);
    }
}
