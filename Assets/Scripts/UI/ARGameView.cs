using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ARGameView : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private GameObject _endGameModal;

    [SerializeField]
    private Text _finalScoreText;

    [SerializeField]
    private Button _submitScoreButton;

    [SerializeField]
    private Button _retryButton;

    [SerializeField]
    private InputField _nameInput;

    [SerializeField]
    private Text _resultText;

    [SerializeField]
    private Button _backButton;

    private int _seconds = 0;

    private bool _timerActive = false;

    private void Awake()
    {
        _submitScoreButton.onClick.AddListener(OnSubmitScoreButtonClicked);
        _retryButton.onClick.AddListener(OnRetryButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        GameManager.Instance.gameState = GameManager.GameState.Calibration;
    }

    private void OnSubmitScoreButtonClicked()
    {
        _resultText.gameObject.SetActive(true);
        if(_nameInput.text != "")
        {
            //Send POST request
            _resultText.text = "Submitting...";
            _resultText.color = Color.gray;
            StartCoroutine(SubmitScoreRequest());
        }
        else
        {
            //invalid name
            _resultText.text = "Invalid name, please try again";
            _resultText.color = Color.red;
        }
    }
 
    private IEnumerator SubmitScoreRequest()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name=" + _nameInput.text + "&score=" + _scoreText.text + "&time=" + _timeText.text));

        UnityWebRequest www = UnityWebRequest.Post("http://apps.iversoft.ca/internal-leaderboard/api.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Submitted score successfully!");
            _resultText.text = "Score submitted successfully!";
            _resultText.color = Color.green;

            _submitScoreButton.GetComponentInChildren<Text>().text = "Submitted";
            _submitScoreButton.GetComponent<Image>().color = Color.gray;
            _submitScoreButton.interactable = false;
        }
    }

    private void OnRetryButtonClicked()
    {
        //Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private IEnumerator TimerCoroutine()
    {
        while (_timerActive)
        {
            //count up the time every second 
            yield return new WaitForSeconds(1f);
            _seconds++;
            UpdateTimeText(_seconds);
        }
    }

    public void StartTimer()
    {
        _timerActive = true;
        StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        _timerActive = false;
        StopCoroutine(TimerCoroutine());
    }

    public void UpdatePlayerScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void UpdateTimeText(int time)
    {
        string timeString = time.ToString();
        if(time < 10)
        {
            timeString = "0" + time.ToString();
        }
        _timeText.text = timeString;
    }
    
    public void ShowEndGameModal()
    {
        _finalScoreText.text = "You scored " + _scoreText.text + " in " + _timeText.text + " seconds!";
        _endGameModal.SetActive(true);
    }

    public void ResetGameUI()
    {
        //set score and time to zero and stop coroutines
        StopTimer();
        _scoreText.text = "0";
        _timeText.text = "00";
    }
}
