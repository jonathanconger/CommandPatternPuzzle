using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	enum cardSuits
	{
		Clubs,
		Diamonds,
		Hearts,
		Spades
	}

	enum cardValues
	{
		Ace,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King
	}

	public List<string> cardDeck = new List<string>();

	private void Awake()
	{
		PlaceCards();
	}

	public void PlaceCards()
	{
		cardDeck = CreateDeck();
		ShuffleDeck();
	}

    public static List<string> CreateDeck()
	{
        List<string> newDeck = new List<string>();

        foreach(string suit in Enum.GetNames(typeof(cardSuits)))
		{
			foreach(string value in Enum.GetNames(typeof(cardValues)))
			{
				newDeck.Add(suit + "_" + value);
			}
		}

		return newDeck;
	}

	public void ShuffleDeck()
	{
		//Fisher-Yates shuffle from https://stackoverflow.com/questions/273313/randomize-a-listt
		int i = cardDeck.Count;
		while (i > 1)
		{
			i--;
			int index = Random.Range(0, i);
			string temp = cardDeck[index];
			cardDeck[index] = cardDeck[i];
			cardDeck[i] = temp;
		}
	}

	void PrintDeck()
	{
		foreach (string card in cardDeck)
		{
			print(card);
		}
	}
}
