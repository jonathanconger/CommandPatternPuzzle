using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	// Fields
	#region
	[SerializeField]
	public List<Sprite> cardSprites;

	[SerializeField]
	GameObject cardPrefab;

	[SerializeField]
	public GameObject[] topSlotPositions;

	[SerializeField]
	public GameObject[] bottomSlotPositions;

	[HideInInspector]
	public List<string>[] topCards;
	[HideInInspector]
	public List<string>[] bottomCards;

	private List<string> topSlot0 = new List<string>();
	private List<string> topSlot1 = new List<string>();
	private List<string> topSlot2 = new List<string>();
	private List<string> topSlot3 = new List<string>();

	private List<string> bottomSlot0 = new List<string>();
	private List<string> bottomSlot1 = new List<string>();
	private List<string> bottomSlot2 = new List<string>();
	private List<string> bottomSlot3 = new List<string>();
	private List<string> bottomSlot4 = new List<string>();
	private List<string> bottomSlot5 = new List<string>();
	private List<string> bottomSlot6 = new List<string>();

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
	#endregion

	public List<string> cardDeck = new List<string>();

	private void Start()
	{
		topCards = new List<string>[] { topSlot0, topSlot1, topSlot2, topSlot3 };
		bottomCards = new List<string>[] { bottomSlot0, bottomSlot1, bottomSlot2, bottomSlot3, bottomSlot4, bottomSlot5, bottomSlot6 };
		
		cardDeck = CreateDeck();
		ShuffleDeck();
		SortCards();
		DealCards();
	}

    private List<string> CreateDeck()
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

	private void ShuffleDeck()
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

	private void DealCards()
	{
		for(int i = 0; i < 7; i++)
		{
			float yOffset = 0;
			float zOffset = 0.03f;

			foreach (string card in bottomCards[i])
			{
				GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomSlotPositions[i].transform.position.x, bottomSlotPositions[i].transform.position.y - yOffset, bottomSlotPositions[i].transform.position.z - zOffset), Quaternion.identity, bottomSlotPositions[i].transform);
				newCard.name = card;

				//add card face
				foreach (Sprite cardFace in cardSprites)
				{
					if (cardFace.name == card)
					{
						newCard.GetComponent<UpdateSprite>().cardFace = cardFace;
						break;
					}
				}

				if (card == bottomCards[i][bottomCards[i].Count - 1])
					newCard.GetComponent<UpdateSprite>().FaceUp();

				yOffset += 0.5f;
				zOffset += 0.03f;
			}
		}
	}

	private void SortCards()
	{
		for(int i = 0; i < 7; i++)
		{
			for (int j = i; j < 7; j++)
			{
				print(cardDeck.Count);
				bottomCards[j].Add(cardDeck[cardDeck.Count - 1]);
				cardDeck.RemoveAt(cardDeck.Count - 1);
			}
		}
	}

	private void PrintDeck()
	{
		foreach (string card in cardDeck)
		{
			print(card);
		}
	}
}
