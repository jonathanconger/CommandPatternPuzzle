using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[HideInInspector]
	public GameObject previousSelection;

	private GameManager gameManager;
	private float timer;
	private float doubleClickTime = 0.3f;
	private int clickCount = 0;

	private void Start()
	{
		gameManager = GetComponent<GameManager>();
		previousSelection = this.gameObject;
	}

	private void Update()
	{
		if (clickCount == 1)
		{
			timer += Time.deltaTime;
		}
		else if (clickCount == 3)
		{
			timer = 0;
			clickCount = 1;
		}

		if (timer > doubleClickTime)
		{
			timer = 0;
			clickCount = 0;
		}

		GetMouseClick();
	}

	private void GetMouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			clickCount++;
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
						TopSlotClick(hit.collider.gameObject);
						break;
					case "BottomSlot":
						BottomSlotClick(hit.collider.gameObject);
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

		if (!selectedObject.GetComponent<UpdateSprite>().bIsFaceUp)
		{
			if(!IsNotOnTop(selectedObject))
			{
				selectedObject.GetComponent<UpdateSprite>().FaceUp();
				previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
				previousSelection = this.gameObject;
			}
		}
		else if (selectedObject.GetComponent<Card>().bIsInDeck)
		{
			if (!IsNotOnTop(selectedObject))
			{
				if (previousSelection == selectedObject)
				{
					if (DoubleClick())
					{
						// check if can be auto stacked
						AutoStack(selectedObject);
					}
				}
				else
				{
					previousSelection = selectedObject;
					previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
				}
			}
		}
		// card clicked is facedown
		// card is not blocked
		// flip card

		// card clicked on is in the deck with triplets
		// card is not blocked
		// select card

		// card is face up
		// no card currently selected
		// select card

		if (previousSelection == this.gameObject)
		{
			previousSelection = selectedObject;
			previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
		}
		else if (previousSelection != selectedObject)
		{
			bool bIsStackable = IsStackable(selectedObject);
			print(bIsStackable);
			if (IsStackable(selectedObject))
			{
				Stack(selectedObject);
			}
			else
			{
				previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
				previousSelection = selectedObject;
				previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
			}
		}
		// have a previously selected card and another card is selected
			// check if it can be stacked
				// stack card (command?)
			// else
				// select new card instead (replace previous)

			// card already selected and is the same card as previous
				// check for double click
					// send to top row if eligible
	}
	
	private void TopSlotClick(GameObject selectedObject)
	{
		print("Top slot clicked.");
		if(previousSelection.CompareTag("Card"))
		{
			if(previousSelection.GetComponent<Card>().value == 1)
			{
				Stack(selectedObject);
			}
		}
	}

	private void BottomSlotClick(GameObject selectedObject)
	{
		print("Bottom slot clicked.");

		// card is king and slot is empty
		if (previousSelection.CompareTag("Card"))
		{
			if (previousSelection.GetComponent<Card>().value == 13)
			{
				Stack(selectedObject);
			}
		}
	}

	private bool IsStackable(GameObject selectedObject)
	{
		Card previous = previousSelection.GetComponent<Card>();
		Card current = selectedObject.GetComponent<Card>();

		if (!current.bIsInDeck)
		{
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
			// stacking on the bottom pile
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
		}

		return false;
	}

	private void Stack(GameObject selectedObject)
	{
		Card previous = previousSelection.GetComponent<Card>();
		Card current = selectedObject.GetComponent<Card>();
		float yOffset = 0.5f;

		// on top of king or empty bottom, stack cards in place

		// stack with y offset
		if (current.bIsOnTop || (!current.bIsOnTop && previous.value == 13))
		{
			yOffset = 0;
		}

		previousSelection.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y - yOffset, selectedObject.transform.position.z - 0.01f);
		previous.transform.parent = selectedObject.transform;

		if (previous.bIsInDeck)
		{
			gameManager.displayedTriplets.Remove(previousSelection.name);
		}
		else if ( previous.bIsOnTop && current.bIsOnTop && previous.value == 1)
		{
			Card topCard = gameManager.topSlotPositions[previous.row].GetComponent<Card>();
			topCard.value = 0;
			topCard.suit = null;
		}
		else if (previous.bIsOnTop)
		{
			gameManager.topSlotPositions[previous.row].GetComponent<Card>().value = previous.value - 1;
		}
		else
		{
			gameManager.bottomCards[previous.row].Remove(previousSelection.name);
		}

		previous.bIsInDeck = false; // cannot add cards to triplets
		previous.row = current.row;

		if (current.bIsOnTop)
		{
			gameManager.topSlotPositions[previous.row].GetComponent<Card>().value = previous.value;
			gameManager.topSlotPositions[previous.row].GetComponent<Card>().suit = previous.suit;
			previous.bIsOnTop = true;
		}
		else
		{
			previous.bIsOnTop = false;
		}

		// reset previousSelection
		previousSelection.GetComponent<UpdateSprite>().ToggleSelection();
		previousSelection = this.gameObject;
	}

	private bool IsNotOnTop(GameObject selectedObject)
	{
		Card current = selectedObject.GetComponent<Card>();

		if (current.bIsInDeck)
		{
			if (current.name == gameManager.displayedTriplets[gameManager.displayedTriplets.Count - 1])
				return false;
			else
				return true;
		}
		else
		{
			if (current.name == gameManager.bottomCards[current.row][gameManager.bottomCards[current.row].Count - 1])
				return false;
			else
				return true;
		}
	}

	private bool DoubleClick()
	{
		if (timer < doubleClickTime && clickCount == 2)
			return true;
		else
			return false;
	}

	private void AutoStack(GameObject selectedObject)
	{
		Card previous = previousSelection.GetComponent<Card>();

		for (int i = 0; i < gameManager.topSlotPositions.Length; i++)
		{
			Card slot = gameManager.topSlotPositions[i].GetComponent<Card>();
			Card selected = selectedObject.GetComponent<Card>();

			if (selected.value == 1)
			{
				if (slot.value == 0)
				{
					// stack ace in first available position
					previousSelection = selectedObject;
					Stack(slot.gameObject);
					break;
				}
			}
			else
			{
				if ((slot.suit == previous.suit) && (slot.value == previous.value - 1))
				{
					// find the top slot that can take the selected card
					if (HasNoChildren(previousSelection))
					{
						previousSelection = selectedObject;
						string lastCardName = slot.suit + "_" + slot.value.ToString();

						switch (slot.value)
						{
							case 1:
								lastCardName = slot.suit + "_Ace";
								break;
							case 11:
								lastCardName = slot.suit + "_Jack";
								break;
							case 12:
								lastCardName = slot.suit + "_Queen";
								break;
							case 13:
								lastCardName = slot.suit + "_King";
								break;
							default:
								break;
						}

						GameObject lastCard = GameObject.Find(lastCardName);
						Stack(lastCard);
						break;
					}
				}
			}
		}
	}

	private bool HasNoChildren(GameObject card)
	{
		int childCount = 0;
		foreach(Transform child in card.transform)
		{
			childCount++;
		}

		if (childCount == 0)
			return true;
		else
			return false;

	}
}
