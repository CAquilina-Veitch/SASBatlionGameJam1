using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public List<Card> Deck = new List<Card>();
    public List<Card> ActiveDeck = new List<Card>();

    public Card GenerateRandomCard()
    {
        Card temp = new Card();

        temp.Sections = new List<CardSection>();
        int numSections = UnityEngine.Random.Range(1, 3);
        for(int i = 0; i < numSections; i++)
        {
            CardSection section = new CardSection() {tiles = new List<ItemType>() };
            int type = UnityEngine.Random.Range(1, 5);
            int num = UnityEngine.Random.Range(1, 4);
            for(int j = 0; j < num; j++)
            {
                Debug.Log(section.tiles);
                Debug.Log(section);
                section.tiles.Add((ItemType)type);
            }
            temp.Sections.Add(section);
        }

        return temp;
    }
    private void Start()
    {
        Deck.Add(GenerateRandomCard());
        ActiveDeck = Deck;
    }


    public Transform optionParent;
    List<Transform> options = new List<Transform>();
    public void TurnShowThree()
    {
        List<Card> three = new List<Card>();
        for(int i =0; i < 3; i++)
        {
            three.Add(DrawCard());
        }
    }

    public void GenerateOptionPositions(int num)
    {
        for (int i = 0; i < num; i++)
        {

        }
    }

    public Card DrawCard()
    {
        if (ActiveDeck.Count < 1)
        {
            ActiveDeck = Deck;
        }
        Card temp = ActiveDeck.Rand();
        ActiveDeck.Remove(temp);
        return temp;
    }



}

[Serializable]
public class Card
{
    public List<CardSection> Sections = new List<CardSection>();
}
[Serializable]
public struct CardSection
{
    public List<ItemType> tiles;
}