using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	private GameManager gameManager;

	private void Start()
	{
		gameManager = GetComponent<GameManager>();
	}

	private void Update()
	{
		GetMouseClick();
	}

	private void GetMouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit)
			{
				switch (hit.collider.tag)
				{
					case "Deck":
						DeckClick();
						break;
					case "Card":
						CardClick();
						break;
					case "TopSlot":
						TopSlotClick();
						break;
					case "BottomSlot":
						BottomSlotClick();
						break;
					default:
						break;
				}
			}
		}
	}

	private void DeckClick()
	{
		print("Deck clicked.");
		gameManager.DealDeckTriplets();
	}

	private void CardClick()
	{
		print("Card clicked.");
	}
	
	private void TopSlotClick()
	{
		print("Top slot clicked.");
	}

	private void BottomSlotClick()
	{
		print("Bottom slot clicked.");
	}
}
