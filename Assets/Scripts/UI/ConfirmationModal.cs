using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ConfirmationModal : MonoBehaviour
{
    [SerializeField]
    private Text _shapeNameText;

    [SerializeField]
    private Image _shapeImage;

    [SerializeField]
    private Button _cancelButton;

    [SerializeField]
    private Button _continueButton;

    void Start()
    {
        _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    public void UpdateShape(string shapeName, Sprite shapeSprite)
    {
        _shapeImage.sprite = shapeSprite;
        _shapeNameText.text = shapeName;
    }

    private void OnCancelButtonClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnContinueButtonClicked()
    {
        GameManager.Instance.targetSprite = _shapeImage.sprite;
        GameManager.Instance.gameState = GameManager.GameState.Calibration;
        gameObject.SetActive(false);
    }
}
