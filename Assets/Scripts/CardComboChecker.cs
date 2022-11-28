using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CardComboChecker
{
    public enum Combo { None, JacksOrBetter, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush };

    //maybe need to split for submethods
    public static Combo CheckForCombo(CardData[] cards)
    {
        int len = cards.Length;
        CardSuit startSuit = cards[0].GetSuit();
        CardValue startValue = cards[0].GetValue();

        //check for (royal/stright) flush incremental
        bool isCombo = false;
        for (int i = 1; i < len; i++)
        {
            if ((cards[i].GetValue() == (CardValue)((int)startValue + i)) && (cards[i].GetSuit() == startSuit))
            {
                isCombo = true;
            }
            else
            {
                isCombo = false;
                break;
            }
        }
        if (isCombo)
        {
            if (startValue == CardValue.Ten)
                return Combo.RoyalFlush;
            else
                return Combo.StraightFlush;
        }

        //check for (royal/stright) flush decremental
        isCombo = false;
        for (int i = 1; i < len; i++)
        {
            if ((cards[i].GetValue() == (CardValue)((int)startValue - i)) && (cards[i].GetSuit() == startSuit))
            {
                isCombo = true;
            }
            else
            {
                isCombo = false;
                break;
            }
        }
        if (isCombo)
        {
            if (startValue == CardValue.Ace)
                return Combo.RoyalFlush;
            else
                return Combo.StraightFlush;
        }

        isCombo = false;
        List<List<CardValue>> sameCardValueDeterminer = new List<List<CardValue>>();
        for (int i = 0; i < len; i++)
        {
            bool isCardValuePresent = false;
            for (int j = 0; j < sameCardValueDeterminer.Count; j++)
            {
                if (cards[i].GetValue() == sameCardValueDeterminer[j][0])
                {
                    isCardValuePresent = true;
                    sameCardValueDeterminer[j].Add(cards[i].GetValue());
                    break;
                }
            }
            if (!isCardValuePresent)
            {
                sameCardValueDeterminer.Add(new List<CardValue>());
                sameCardValueDeterminer[sameCardValueDeterminer.Count - 1].Add(cards[i].GetValue());
            }
        }

        //check for four of a kind & full house
        bool isThreeWithSameValuePresent = false;
        bool isTwoWithSameValuePresent = false;
        int twoWithSameValueCounter = 0;
        for (int i = 0; i < sameCardValueDeterminer.Count; i++)
            switch (sameCardValueDeterminer[i].Count)
            {
                case 4:
                    return Combo.FourOfAKind;
                case 2:
                    isTwoWithSameValuePresent = true;
                    twoWithSameValueCounter++;
                    break;
                case 3:
                    isThreeWithSameValuePresent = true;
                    break;
            }
        if (isTwoWithSameValuePresent && isThreeWithSameValuePresent)
            return Combo.FullHouse;

        //check for simple flush
        isCombo = true;
        for (int i = 1; i < len; i++)
            if (cards[i].GetSuit() != startSuit)
            {
                isCombo = false;
                break;
            }
        if (isCombo)
            return Combo.Flush;

        //List<CardData> orderedCards = new List<CardData>(cards).OrderBy(o => o.GetValue()).ToList();

        //check for straight incremental
        isCombo = false;
        int startValueInt = (int)startValue;
        if (startValue == CardValue.Ace)
            startValueInt = 1;
        for (int i = 1; i < len; i++)
        {
            if (cards[i].GetValue() == (CardValue)(startValueInt + i))
            {
                isCombo = true;
            }
            else
            {
                isCombo = false;
                break;
            }
        }
        if (isCombo)
            return Combo.Straight;

        //check for straight decremental
        isCombo = false;
        for (int i = 1; i < len; i++)
        {
            if (cards[i].GetValue() == (CardValue)((int)startValue - i))
            {
                isCombo = true;
            }
            else
                if ((cards[i].GetValue() == CardValue.Ace) && (((int)startValue - i) == 1))
                    isCombo = true;
                else
                { 
                    isCombo = false;
                    break;
                }
        }
        if (isCombo)
            return Combo.Straight;

        if (isThreeWithSameValuePresent)
            return Combo.ThreeOfAKind;
        if (twoWithSameValueCounter == 2)
            return Combo.TwoPair;
        if (isTwoWithSameValuePresent)
            for (int i = 0; i < sameCardValueDeterminer.Count; i++)
                switch (sameCardValueDeterminer[i].Count)
                {
                    case 2:
                        if (sameCardValueDeterminer[i][0] >= CardValue.Jack)
                            return Combo.JacksOrBetter;
                        break;
                }
        return Combo.None;
    }

}
