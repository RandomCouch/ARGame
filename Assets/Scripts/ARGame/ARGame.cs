using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _stageSprite;

    [SerializeField]
    private PlaceOnPlane _stageSpawner;

    public void SetStageSprite(Sprite stageSprite)
    {
        SpriteRenderer spriteRenderer = _stageSprite.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stageSprite;
        _stageSprite.AddComponent<PolygonCollider2D>();

    }
    
}
