using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARCalibrationView : MonoBehaviour
{
    [SerializeField]
    private Button _confirmButton;

    [SerializeField]
    private GameObject _tutorialModal;

    [SerializeField]
    private Slider _rotationSlider;

    [SerializeField]
    private Slider _scaleSlider;

    [SerializeField]
    private Button _backButton;
    
    private Transform _targetTransform;

    void Start()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _rotationSlider.onValueChanged.AddListener(OnRotationSliderValueChanged);
        _scaleSlider.onValueChanged.AddListener(OnScaleSliderValueChanged);
        
        if (!PlayerPrefs.HasKey("ftux"))
        {
            _tutorialModal.SetActive(true);
        }
    }

    private void OnBackButtonClicked()
    {
        GameManager.Instance.gameState = GameManager.GameState.Picking;
    }

    private void OnRotationSliderValueChanged(float val)
    {
        Vector3 newEulerAngles = _targetTransform.eulerAngles;
        newEulerAngles.y = val;
        _targetTransform.eulerAngles = newEulerAngles;
    }

    private void OnScaleSliderValueChanged(float val)
    {
        Vector3 newScale = new Vector3(val, val, val);
        _targetTransform.localScale = newScale;
    }

    private void OnConfirmButtonClicked()
    {
        GameManager.Instance.gameState = GameManager.GameState.Calibrated;
    }

    public void ShowControls()
    {
        _confirmButton.gameObject.SetActive(true);
        _rotationSlider.gameObject.SetActive(true);
        _scaleSlider.gameObject.SetActive(true);
    }

    public void SetTargetTransform(Transform target)
    {
        _targetTransform = target;
        float rotationValue = target.eulerAngles.y;
        float scaleValue = target.localScale.x;

        _rotationSlider.value = rotationValue;
        _scaleSlider.value = scaleValue;
    }

}
