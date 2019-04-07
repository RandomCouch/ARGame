using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    [SerializeField]
    private Button _nextButton;

    [SerializeField]
    private Button _prevButton;

    [SerializeField]
    private RectTransform _content;

    [SerializeField]
    private Pagination _pagination;

    [SerializeField]
    private GameObject _ShapeUIPrefab;

    [SerializeField]
    private ShapeScriptableObject[] _data;

    [SerializeField]
    private GameObject _confirmationModal;

    private ScrollRect _scrollRect;

    public int curPage = 0;
    public int maxPages;

    public float adjustedFinalPosition = 1;

    private bool _trackScrolling = true;

    // Start is called before the first frame update
    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _prevButton.onClick.AddListener(OnPreviousButtonClicked);

        _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        
        RectTransform parentRect = _scrollRect.GetComponent<RectTransform>();
        Debug.Log("Parent rect height = " + parentRect.rect.height);
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, parentRect.rect.height);

        _scrollRect.horizontalNormalizedPosition = 0;

        SetupShapeOptions();

        _pagination.SetupPagination(maxPages);
        _pagination.TogglePagination(curPage);
    }

    private void SetupShapeOptions()
    {
        for(int i = 0; i < _data.Length; i++)
        {
            GameObject shapeUIBlock = Instantiate(_ShapeUIPrefab, _content.transform);
            ShapeUIBlock shapeUIBlockScript = shapeUIBlock.GetComponent<ShapeUIBlock>();
            shapeUIBlockScript.Initialize(_data[i]);
            shapeUIBlockScript.OnClicked += OnShapeButtonClicked;
        }
    }

    private void OnShapeButtonClicked(string name, Sprite shapeSprite)
    {
        ConfirmationModal cModalScript = _confirmationModal.GetComponent<ConfirmationModal>();
        cModalScript.UpdateShape(name, shapeSprite);
        _confirmationModal.SetActive(true);
    }

    private void OnScrollValueChanged(Vector2 normalizedPos)
    {
        if (_trackScrolling)
        {
            int targetPage;
            if(_scrollRect.horizontalNormalizedPosition == adjustedFinalPosition)
            {
                targetPage = maxPages;
            }
            else
            {
                targetPage = Mathf.RoundToInt(_scrollRect.horizontalNormalizedPosition * (float)maxPages);
                Debug.Log("Target page: " + targetPage);
            }

            targetPage = Mathf.Clamp(targetPage, 0, maxPages);

            if(curPage != targetPage)
            {
                curPage = targetPage;
                _pagination.TogglePagination(curPage);
            }
            
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(_scrollRect.horizontalNormalizedPosition, 0, adjustedFinalPosition);

            
        }
    }

    private void OnNextButtonClicked()
    {
        _trackScrolling = false;
        curPage++;
        curPage = Mathf.Clamp(curPage, 0, maxPages);
        _pagination.TogglePagination(curPage);

        UpdateScrollPosition();
        _trackScrolling = true;
    }
    private void OnPreviousButtonClicked()
    {
        _trackScrolling = false;
        curPage--;
        curPage = Mathf.Clamp(curPage, 0, maxPages);
        _pagination.TogglePagination(curPage);

        UpdateScrollPosition();
        _trackScrolling = true;
    }

    private void UpdateScrollPosition()
    {
       

        float scrollPos = (float)curPage / (float)maxPages;

        if (scrollPos == 1)
        {
            scrollPos = adjustedFinalPosition;
        }

        _scrollRect.horizontalNormalizedPosition = scrollPos;
        Debug.Log("Set scroll position to " + scrollPos);
    }

}
