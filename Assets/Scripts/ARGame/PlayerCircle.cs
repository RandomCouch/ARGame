using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircle : MonoBehaviour
{
    private Rigidbody2D _rb2d;

    private Vector3 _lastPosition = Vector3.zero;
    private Vector3 _localVelocity = Vector3.zero;

    private bool _canScore = false;
    private bool _touchingWall = false;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        _localVelocity = (transform.localPosition - _lastPosition) / Time.deltaTime;

        _lastPosition = transform.localPosition;

        
    }

    public float GetVelocity()
    {
        return _localVelocity.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Stage")
        {
            //Lose 3 points
            if (_canScore)
            {
                GameManager.Instance.AddToPlayerScore(-10);
            }
            _touchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Stage")
        {
            //Lose 3 points
            _touchingWall = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EndArea")
        {
            //Game finished
            _canScore = false;
            Debug.Log("Reached end area");
            GameManager.Instance.StopGameTimer();
            GameManager.Instance.gameState = GameManager.GameState.EndGame;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "StartArea")
        {
            //Start the timer and enable scoring
            Debug.Log("Left start area");
            _canScore = true;
            StartCoroutine(ScorePerSecond());
            //Start scoring coroutine
            GameManager.Instance.StartGameTimer();
        }
    }

    private IEnumerator ScorePerSecond()
    {
        while (_canScore)
        {
            if(_localVelocity.magnitude > 0f)
            {
                GameManager.Instance.AddToPlayerScore(Mathf.CeilToInt(_localVelocity.magnitude));
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
