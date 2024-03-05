using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public List<Card> Deck = new List<Card>();

    public Card GenerateRandomCard()
    {
        Card temp = new Card();

        temp.Sections = new List<CardSection>();
        int numSections = UnityEngine.Random.Range(1, 3);
        for(int i = 0; i < numSections; i++)
        {
            CardSection section = new CardSection() {tiles= new List<ItemType>() };
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