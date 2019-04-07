using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private ARSession _arSession;

    [SerializeField]
    private ARPlaneManager _arPlaneManager;

    [SerializeField]
    private ARPointCloudManager _arPointCloudManager;

    [SerializeField]
    private GameObject _pickImageView;

    [SerializeField]
    private PlaceOnPlane _shapeSpawner;

    [SerializeField]
    private ARCalibrationView _arCalibrationView;

    [SerializeField]
    private ARGameView _arGameView;

    [SerializeField]
    private GameObject _helperCube;

    [SerializeField]
    private PlayerSphere _playerSphere;

    private bool _arSpriteSpawned = false;

    private int _playerScore = 0;

    public Sprite targetSprite;

    private GameState _gameState;

    public GameState gameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            _gameState = value;
            OnGameStateChanged();
        }
    }

    public enum GameState
    {
        Picking,
        Calibration,
        Calibrated,
        EndGame
    }

    private void Start()
    {
        Instance = this;
        gameState = GameState.Picking;
        //_arPlaneManager.planeAdded += OnPlaneAdded;
        _shapeSpawner.OnShapeSpawned += OnShapeSpawned;
    }

    private void OnGameStateChanged()
    {
        switch (_gameState)
        {
            case GameState.Picking:
                _pickImageView.SetActive(true);
                _arSession.gameObject.SetActive(false);
                _arCalibrationView.gameObject.SetActive(false);
                _shapeSpawner.enabled = false;
                _helperCube.SetActive(false);
                _arGameView.gameObject.SetActive(false);
                _playerSphere.enabled = false;
                HidePlanes();
                break;
            case GameState.Calibration:
                _arSession.gameObject.SetActive(true);
                _arCalibrationView.gameObject.SetActive(true);
                _pickImageView.SetActive(false);
                _shapeSpawner.enabled = true;
                _playerSphere.enabled = false;
                _arGameView.gameObject.SetActive(false);
                _helperCube.SetActive(false);
                //Spawn sprite on first AR plane 
                ShowPlanes();

                _arGameView.ResetGameUI();
                _shapeSpawner.ResetSpawnedObject();
                break;
            case GameState.Calibrated:
                HidePlanes();
                _helperCube.SetActive(true);
                _arCalibrationView.gameObject.SetActive(false);
                _shapeSpawner.enabled = false;
                _arGameView.gameObject.SetActive(true);
                _playerSphere.enabled = true;
                break;
            case GameState.EndGame:
                _arGameView.ShowEndGameModal();
                _helperCube.SetActive(false);

                break;
        }
    }

    private void OnShapeSpawned(Transform shape)
    {
        ARGame shapeGame = shape.gameObject.GetComponent<ARGame>();
        if(shapeGame != null)
        {
            _arCalibrationView.SetTargetTransform(shape);
            _arCalibrationView.ShowControls();
            shapeGame.SetStageSprite(targetSprite);
        }
        
    }
    
    public void HidePlanes()
    {
        List<ARPlane> planes = new List<ARPlane>();
        _arPlaneManager.GetAllPlanes(planes);
        if (planes.Count > 0)
        {
            foreach (ARPlane p in planes)
            {
                p.gameObject.SetActive(false);
            }
        }
        if (_arPointCloudManager.pointCloud != null)
            _arPointCloudManager.pointCloud.gameObject.SetActive(false);

        
        _arPlaneManager.enabled = false;
        _arPointCloudManager.enabled = false;
    }

    public void ShowPlanes()
    {
        _arPlaneManager.enabled = true;
        _arPointCloudManager.enabled = true;

        if (_arPointCloudManager.pointCloud != null)
            _arPointCloudManager.pointCloud.gameObject.SetActive(true);

        List<ARPlane> planes = new List<ARPlane>();
        _arPlaneManager.GetAllPlanes(planes);
        if (planes.Count > 0)
        {
            foreach (ARPlane p in planes)
            {
                p.gameObject.SetActive(true);
            }
        }
    }

    public int GetPlayerScore()
    {
        return _playerScore;
    }

    public void AddToPlayerScore(int score)
    {
        _playerScore += score;
        _arGameView.UpdatePlayerScore(_playerScore);
    }

    public void StartGameTimer()
    {
        _arGameView.StartTimer();
    }

    public void StopGameTimer()
    {
        _arGameView.StopTimer();
    }
}
