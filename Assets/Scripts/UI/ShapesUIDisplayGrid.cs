using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapesUIDisplayGrid : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup _gridLayout;
    
    public int displayColumns = 2;

    private Vector2 _originalCellSize;

    void Start()
    {
        //Cell size must be equal to half the content window size, minus 50 
        //Debug.Log("Current aspect ratio: " + Camera.main.aspect);
        RectTransform _parentRect = _gridLayout.transform.parent.parent.GetComponent<RectTransform>();

        float desiredCellSize = _parentRect.rect.width / displayColumns;
        //consider the padding 
        desiredCellSize -= _gridLayout.padding.left - _gridLayout.padding.right;
        //..and spacing
        desiredCellSize -= _gridLayout.spacing.x * (displayColumns - 1);

        _originalCellSize = _gridLayout.cellSize;
        Vector2 newCellSize = _originalCellSize;
        newCellSize.x = desiredCellSize;
        newCellSize.y = desiredCellSize;
        _gridLayout.cellSize = newCellSize;
    }
}
