using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[HideInInspector]
	public GameObject previousSelection;

	private GameManager gameManager;

	private void Start()
	{
		gameManager = GetComponent<GameManager>();
		previousSelection = this.gameObject;
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
						CardClick(hit.collider.gameObject);
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

	private void CardClick(GameObject selectedObject)
	{
		print("Card clicked.");

		if (previousSelection == this.gameObject)
		{
			previousSelection = selectedObject;
			previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
		}
		else if (previousSelection != selectedObject)
		{
			bool bIsStackable = IsStackable(selectedObject);
			print("Stackable: " + bIsStackable);

			//if (IsStackable(selectedObject))
			if (bIsStackable)
			{
				// stack cards
			}
			else
			{
				// select new card
				previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
				previousSelection = selectedObject;
				selectedObject.GetComponent<UpdateSprite>().ToggleSelection();
			}

		}
	}
	
	private void TopSlotClick()
	{
		print("Top slot clicked.");
	}

	private void BottomSlotClick()
	{
		print("Bottom slot clicked.");
	}

	private bool IsStackable(GameObject selectedObject)
	{
		Card previous = previousSelection.GetComponent<Card>();
		Card current = selectedObject.GetComponent<Card>();

		// stacking in the top pile
		if (current.bIsOnTop)
		{
			if (previous.suit == current.suit || (previous.value == 1 && current.suit == null))
			{
				if (previous.value == current.value + 1)
					return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			bool bCardOneRed = true;
			bool bCardTwoRed = true;

			if (previous.suit == "Clubs" || previous.suit == "Spades")
				bCardOneRed = false;
			if (current.suit == "Clubs" || current.suit == "Spaces")
				bCardTwoRed = false;

			if (bCardOneRed == bCardTwoRed)
				return false;
			else
				return true;
		}

		// stacking in the bottom pile
		return false;
	}
}
