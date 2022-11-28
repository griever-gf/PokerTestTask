using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MainTableController mainTableController;
    public BalanceAndBetController balanceAndBetController;
    public BonusTableController bonusTableController;
    public CameraFitterAndMover cameraFitterAndMover;

    void Awake() //game initialisation
    {
        balanceAndBetController.AttachViewAndModel();
        mainTableController.SetExtraLinks(balanceAndBetController.model, balanceAndBetController.view);
        mainTableController.AttachViewsAndModels();
        mainTableController.GameStartPreparations();
        bonusTableController.SetExtraLinks(balanceAndBetController.model, mainTableController.view);
        bonusTableController.AttachViewAndModel();


        //SwitchToBonusTable();
}

    public void SwitchToBonusTable()
    {
        cameraFitterAndMover.MoveToBonusTable();
        balanceAndBetController.view.ChangeControlsVisibility(false);
        bonusTableController.StartBonusGame();
    }

    public void SwitchToStandardTable()
    {
        cameraFitterAndMover.MoveToStandardTable();
        balanceAndBetController.view.ChangeControlsVisibility(true);
    }
}
