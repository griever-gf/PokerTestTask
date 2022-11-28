using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardValue { Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Lady = 12, King = 13, Ace = 14 }
public enum CardSuit { Hearts = 1, Diamonds = 2, Clovers = 3, Spades = 4 }

public class CardData
{
    private CardValue value;
    private CardSuit suit;
    bool isHolded; //возможно стоит разделить на подклассы от абстрактного класса

    public CardData(CardValue val, CardSuit st)
    {
        value = val;
        suit = st;
        isHolded = false;
    }

    public bool EqualToCard(CardData cd)
    {
        if (cd == null) return false;
        return ((cd.value == value) && (cd.suit == suit));
    }

    public CardValue GetValue()
    {
        return value;
    }

    public CardSuit GetSuit()
    {
        return suit;
    }

    public void ReverseHoldStatus()
    {
        isHolded = !isHolded;
    }

    public bool GetIsHolded()
    {
        return isHolded;
    }

    public void SetHoldStatusFree()
    {
        isHolded = false;
    }
}
