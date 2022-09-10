using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    [SerializeField]
    public Sprite cardFace;

    [SerializeField]
    public Sprite cardBack;

    private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

    public void FaceUp()
	{
        _spriteRenderer.sprite = cardFace;
	}

    public void FaceDown()
	{
        _spriteRenderer.sprite = cardBack;
	}
}
