using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TableWithHand
{
    protected const int CARD_NUMBER = 5;
    protected CardData[] handCards;
    protected Deck tableDeck;

    protected TableWithHand()
    {
        tableDeck = new Deck();
        handCards = new CardData[CARD_NUMBER];
    }

    protected void FillHand()
    {
        for (int i = 0; i < CARD_NUMBER; i++)
        {
            if (handCards[i] == null)
                handCards[i] = tableDeck.GetRandomCard();
            else if (!handCards[i].GetIsHolded())
                handCards[i] = tableDeck.GetRandomCard();
        }
    }
}
