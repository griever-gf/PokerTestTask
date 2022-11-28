using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public Sprite[] cardsHearts;
    public Sprite[] cardsDiamonds;
    public Sprite[] cardsClovers;
    public Sprite[] cardsSpades;
    public GameObject prefabCard;

    GameObject SpawnCard (CardSuit suit, CardValue value)
    {
        switch (suit)
        {
            case CardSuit.Hearts:
                return SetCardSpriteFromArray(value, cardsHearts);
            case CardSuit.Diamonds:
                return SetCardSpriteFromArray(value, cardsDiamonds);
            case CardSuit.Clovers:
                return SetCardSpriteFromArray(value, cardsClovers);
            case CardSuit.Spades:
                return SetCardSpriteFromArray(value, cardsSpades);
        }
        return null;
    }

    GameObject SetCardSpriteFromArray(CardValue val, Sprite[] sprites)
    {
        int valuesLen = Enum.GetNames(typeof(CardValue)).Length;
        for (int i = 0; i < valuesLen; i++)
            if ((CardValue)(i + 2) == val)
            {
                GameObject card = Instantiate(prefabCard);
                card.GetComponent<SpriteRenderer>().sprite = sprites[i];
                return card;
            }
        return null;
    }

    public void SpawnHandCards(CardData[] hand, Transform pointCardsSpawn, ref GameObject[] handCards)
    {
        if (handCards == null)
            handCards = new GameObject[hand.Length];
        else
            for (int i = 0; i < hand.Length; i++)
                if (!hand[i].GetIsHolded())
                    Destroy(handCards[i]);

        for (int i = 0; i < hand.Length; i++)
            if (!hand[i].GetIsHolded())
            {
                handCards[i] = SpawnCard(hand[i].GetSuit(), hand[i].GetValue());
                float spriteWidth = handCards[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x * handCards[i].transform.localScale.x;
                handCards[i].transform.position = pointCardsSpawn.position + new Vector3((i - hand.Length / 2) * spriteWidth * 1.1f, 0);
                handCards[i].transform.SetParent(pointCardsSpawn);
            }
    }
}
