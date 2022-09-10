using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    [SerializeField]
    public Sprite cardFace;

    [SerializeField]
    public Sprite cardBack;

    [HideInInspector]
    public bool bIsFaceUp;

    private bool bIsSelected = false;
    private SpriteRenderer spriteRenderer;

	private void Awake()
	{
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        bIsFaceUp = false;
	}

    public void FaceUp()
	{
        spriteRenderer.sprite = cardFace;
        bIsFaceUp = true;
	}

    public void FaceDown()
	{
        spriteRenderer.sprite = cardBack;
        bIsFaceUp = false;
	}

    public void ToggleSelection()
	{
        if (bIsSelected)
		{
            bIsSelected = false;
            spriteRenderer.color = Color.white;
		}
		else
		{
            bIsSelected = true;
            spriteRenderer.color = Color.green;
		}
	}
}
