using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoblinBuildController : MonoBehaviour
{
    [SerializeField] private List<Sprite> headSprites;
    [SerializeField] private List<Sprite> bodySprites;
    [SerializeField] private TMP_InputField goblinName;
    [Space]
    [SerializeField] private Image headSpriteRenderer;
    [SerializeField] private Image bodySpriteRenderer;

    private int headIndex = 0;
    private int bodyIndex = 0;

    public void InitializePawn()
    {
        PawnManager.instance.SetName(goblinName.text, null, null);
        PawnManager.instance.SetSprites(headIndex, bodyIndex);
    }

    public void NextHead()
    {
        headIndex++;
        
        if(headIndex == headSprites.Count)
            headIndex = 0;

        UpdateHead();
    }

    public void PrevHead() 
    {
        headIndex--;

        if(headIndex < 0)
            headIndex = headSprites.Count - 1;

        UpdateHead();
    }

    public void NextBody()
    {
        bodyIndex++;

        if (bodyIndex == bodySprites.Count)
            bodyIndex = 0;

        UpdateBody();
    }

    public void PrevBody()
    {
        bodyIndex--;

        if(bodyIndex < 0)
            bodyIndex = bodySprites.Count - 1;

        UpdateBody();
    }

    public void UpdateHead()
    {
        headSpriteRenderer.sprite = headSprites[headIndex];
    }

    public void UpdateBody()
    {
        bodySpriteRenderer.sprite = bodySprites[bodyIndex];
    }
}
