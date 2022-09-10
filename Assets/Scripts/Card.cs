using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	[HideInInspector]
	public bool bIsOnTop = false;
	[HideInInspector]
	public int row;
	[HideInInspector]
	public bool bIsInDeck;
	[HideInInspector]
	public string suit;
	[HideInInspector]
	public int value;

	private string valueString;

	private void Start()
	{
		if (CompareTag("Card"))
		{
			int underscoreIndex = transform.name.IndexOf("_") - 1;
			SetCardSuit(transform.name.Substring(0, underscoreIndex));
			SetCardValue(transform.name.Substring(underscoreIndex + 1));
		}
	}
	private void SetCardSuit(string suitString)
	{
		suit = suitString;
	}

	private void SetCardValue(string valueString)
	{
		switch (valueString)
		{
			case "Ace":
				value = 1;
				break;
			case "Two":
				value = 2;
				break;
			case "Three":
				value = 3;
				break;
			case "Four":
				value = 4;
				break;
			case "Five":
				value = 5;
				break;
			case "Six":
				value = 6;
				break;
			case "Seven":
				value = 7;
				break;
			case "Eight":
				value = 8;
				break;
			case "Nine":
				value = 9;
				break;
			case "Ten":
				value = 10;
				break;
			case "Jack":
				value = 11;
				break;
			case "Queen":
				value = 12;
				break;
			case "King":
				value = 13;
				break;
			default:
				break;
		}
	}
}
