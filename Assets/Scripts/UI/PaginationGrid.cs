using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationGrid : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup _gridLayout;

    public int referenceWidth;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Screen width: " + Screen.width);

        Vector2 newCellSize = _gridLayout.cellSize;
        newCellSize.x = newCellSize.x * Screen.width / referenceWidth;
        newCellSize.y = newCellSize.y * Screen.width / referenceWidth;

        _gridLayout.cellSize = newCellSize;
    }
    
}
