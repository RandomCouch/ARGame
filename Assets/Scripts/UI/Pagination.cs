using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagination : MonoBehaviour
{
    [SerializeField]
    private GameObject _paginationIconPrefab;

    private Dictionary<int, PaginationIcon> _paginationIcons;
    
    public void SetupPagination(int maxPages)
    {
        _paginationIcons = new Dictionary<int, PaginationIcon>();
        for (int i = 0; i <= maxPages; i++)
        {
            GameObject paginationIconGO = Instantiate(_paginationIconPrefab, transform);
            PaginationIcon paginationIconScript = paginationIconGO.GetComponent<PaginationIcon>();

            _paginationIcons.Add(i, paginationIconScript);
        }
    }

    public void TogglePagination(int curPage)
    {
        //Toggle all pagination icons first
        foreach(KeyValuePair<int, PaginationIcon> data in _paginationIcons)
        {
            data.Value.Toggle(false);
        }

        if (_paginationIcons.ContainsKey(curPage))
        {
            _paginationIcons[curPage].Toggle(true);
        }
    }
}
