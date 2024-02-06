using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //checks
    private bool _isMoving, _isOnMovingTarget, _isAlive = true;
    
    //movements
    private Vector3 _origPos, _targetPos;
    [SerializeField] private float _timeToMove;
    //movements on logs/boat/ecc..
    private Vector3 _movingTargetPos;
    private GameObject _movingTargetGameObject;
    [SerializeField] private float _moveTowardsBoatMiddle;
    
    private void Awake()
    {
        transform.position = new Vector3(-10f, 0f, 0f);
    }
    private void Update()
    {
        if (_isAlive)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !_isMoving)
                StartCoroutine(MovePlayer(Vector3.forward, false));
            
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !_isMoving)
                StartCoroutine(MovePlayer(Vector3.back, false));
            
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !_isMoving)
                StartCoroutine(MovePlayer(Vector3.left, false));
            
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !_isMoving)
                StartCoroutine(MovePlayer(Vector3.right, false));
            if (_isOnMovingTarget)
            {
                _movingTargetPos = SetMovingTargetPos();
                transform.position = Vector3.MoveTowards(transform.position, _movingTargetPos, _moveTowardsBoatMiddle);
            }
        }
        
    }

    private IEnumerator MovePlayer(Vector3 direction, bool isFallingInRiver)
    {
        _isMoving = true;
        float _elapsedTime = 0;
        if (!isFallingInRiver)
        {
        _origPos = transform.position;
        _targetPos = _origPos + direction;
        
        //making target pos fixed to an int value
        _targetPos.x = Mathf.Round(_targetPos.x);
        _targetPos.y = Mathf.Round(_targetPos.y);
        _targetPos.z = Mathf.Round(_targetPos.z);
        }

        
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boat")
        {
            _movingTargetGameObject = other.gameObject;
            _isOnMovingTarget = true;
        }
        if (other.gameObject.tag == "River")
        {
            Debug.Log("fall in river");
            //FallingInRiver();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Boat")
        {
            _isOnMovingTarget = false;
        }
    }
    private Vector3 SetMovingTargetPos()
    {
        return _movingTargetGameObject.transform.position;
    }

    private void FallingInRiver()
    {
        _isAlive = false;
        _targetPos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
        StartCoroutine(MovePlayer(Vector3.down, true));
    }

}
