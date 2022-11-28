using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<CardData> deckCards;

    public Deck()
    {
        
    }

    public CardData GetRandomCard()
    {
        System.Random rnd = new System.Random();
        int rndIdx = rnd.Next(0, deckCards.Count);
        CardData value = deckCards[rndIdx];
        deckCards.RemoveAt(rndIdx);
        return (value);
    }

    public void FillDeck()
    {
        int valuesLen = Enum.GetNames(typeof(CardValue)).Length;
        int suitsLen = Enum.GetNames(typeof(CardSuit)).Length;
        if (deckCards != null) deckCards.Clear();
        deckCards = new List<CardData>();
        for (int i = 0; i < valuesLen; i++)
            for (int j = 0; j < suitsLen; j++)
            {
                deckCards.Add(new CardData((CardValue)(i + 2), (CardSuit)(j + 1)));
            }
    }
}
