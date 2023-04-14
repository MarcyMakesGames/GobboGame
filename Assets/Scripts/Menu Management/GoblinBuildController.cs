using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinBuildController : MonoBehaviour
{
    [SerializeField] private List<Sprite> headSprites;
    [SerializeField] private List<Sprite> bodySprites;
    [Space]
    [SerializeField] private Image headSpriteRenderer;
    [SerializeField] private Image bodySpriteRenderer;

    public void InitializePawnSprites(PawnStatusContainer statusContainer)
    {
        switch (statusContainer.HeadType)
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
