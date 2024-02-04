using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool _isMoving = false;
    private Vector3 _origPos, _targetPos;
    [SerializeField] private float _timeToMove;

    private void Awake()
    {
        transform.position = new Vector3(-10f, 0f, 0f);
    }
    private void Update()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !_isMoving)
            StartCoroutine(MovePlayer(Vector3.forward));
        
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !_isMoving)
            StartCoroutine(MovePlayer(Vector3.back));
        
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !_isMoving)
            StartCoroutine(MovePlayer(Vector3.left));
        
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !_isMoving)
            StartCoroutine(MovePlayer(Vector3.right));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        _isMoving = true;

        float _elapsedTime = 0;
        _origPos = transform.position;
        //making target pos fixed to an int value
        _targetPos = _origPos + direction;          
        _targetPos.x = Mathf.Round(_targetPos.x);
        _targetPos.y = Mathf.Round(_targetPos.y);
        _targetPos.z = Mathf.Round(_targetPos.z);
        
        //player movement logic
        while (_elapsedTime < _timeToMove)
        {
            transform.position = Vector3.Lerp(_origPos, _targetPos, (_elapsedTime / _timeToMove));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPos;
        
        _isMoving = false;
    }
    
}
