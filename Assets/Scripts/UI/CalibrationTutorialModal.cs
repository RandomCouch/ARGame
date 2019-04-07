using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationTutorialModal : MonoBehaviour
{
    [SerializeField]
    private Button _continueButton;
    // Start is called before the first frame update
    void Awake()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        //Close modal
        PlayerPrefs.SetInt("ftux", 1);
        gameObject.SetActive(false);
    }
}
