using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardView : MonoBehaviour
{
    public GameObject spriteHold;
    public event Action OnClick;
    public event Action OnTurnFinished;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (IsCardOpened())
            OnClick?.Invoke();
    }

    public void ChangeHoldLabelVisibility(bool isVisible)
    {
        spriteHold.SetActive(isVisible);
    }

    public void StartTurnAnimation()
    {
        animator.SetTrigger("StartTurn");
    }

    public bool IsCardOpened()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("CardOpened");
    }

    void TurnFinished()
    {
        OnTurnFinished?.Invoke();
    }
}
