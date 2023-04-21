using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpriteController : MonoBehaviour
{
    public int HeadType { get => headIndex; }
    public int BodyType { get => bodyIndex; }

    [SerializeField] private List<Sprite> headSprites;
    [SerializeField] private List<Sprite> bodySprites;
    [Space]
    [SerializeField] private SpriteRenderer headSpriteRenderer;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;

    private int headIndex = 0;
    private int bodyIndex = 0;

    public void InitializePawnSprites(PawnStatusContainer statusContainer)
    {
        switch(statusContainer.HeadType)
        {
            case 0:
                headSpriteRenderer.sprite = headSprites[0];
                break;

            case 1:
                headSpriteRenderer.sprite = headSprites[1];
                break;
        
            case 2:
                headSpriteRenderer.sprite = headSprites[2];
                break;

            default:
                break;
        }

        switch (statusContainer.BodyType)
        {
            case 0:
                bodySpriteRenderer.sprite= bodySprites[0];
                break;

            case 1:
                bodySpriteRenderer.sprite = bodySprites[1];
                break;

            case 2:
                bodySpriteRenderer.sprite = bodySprites[2];
                break;

            default:
                break;
        }
    }

    public void InitializePawnSprites(int headSprite, int bodySprite)
    {
        headIndex = headSprite;
        bodyIndex = bodySprite;

        switch (headSprite)
        {
            case 0:
                headSpriteRenderer.sprite = headSprites[0];
                break;

            case 1:
                headSpriteRenderer.sprite = headSprites[1];
                break;

            case 2:
                headSpriteRenderer.sprite = headSprites[2];
                break;

            default:
                break;
        }

        switch (bodySprite)
        {
            case 0:
                bodySpriteRenderer.sprite = bodySprites[0];
                break;

            case 1:
                bodySpriteRenderer.sprite = bodySprites[1];
                break;

            case 2:
                bodySpriteRenderer.sprite = bodySprites[2];
                break;

            default:
                break;
        }
    }
}
