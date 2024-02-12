using System;
using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IPlayer
{
    //checks
    private bool _isMoving, _isOnMovingTarget, _isAlive = true, _isAbleToFall;
    
    //movements
    private Vector3 _origPos, _targetPos;
    [SerializeField] private float _timeToMove;
    private GameObject _mesh;
    private Animator _animator;
    
    
    [SerializeField] private LayerMask _obstacleLayer, _waterLayer;
    //movements on logs/boat/ecc..
    private Vector3 _movingTargetPos;
    private GameObject _movingTargetGameObject;
    [SerializeField] private float _moveTowardsBoatMiddle;
    
    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
    }
    
    private void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        _animator = GetComponent<Animator>();
        _mesh = GameObject.Find("Cube");
    }

    private void Update()
    {
        if (_isAlive)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !_isMoving)
            {
                PlayerRotate(0f);
                if (!CheckObstacleInDirection(Vector3.forward))
                {
                    _animator.SetTrigger("Hop");
                    StartCoroutine(MovePlayer(Vector3.forward, false));
                }
                else
                {
                    _animator.SetInteger("ObjectHit", 1);
                }
            }

            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !_isMoving)
            {
                PlayerRotate(180f);
                if (!CheckObstacleInDirection(Vector3.back))
                {
                    _animator.SetTrigger("Hop");
                    StartCoroutine(MovePlayer(Vector3.back, false));
                }
                else
                {
                    _animator.SetInteger("ObjectHit", 2);
                }
            }

            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !_isMoving)
            {
                PlayerRotate(270f);
                if (!CheckObstacleInDirection(Vector3.left))
                {
                    _animator.SetTrigger("Hop");
                    StartCoroutine(MovePlayer(Vector3.left, false));
                }
                else
                {
                    _animator.SetInteger("ObjectHit", 3);
                }
            }

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !_isMoving)
            {
                PlayerRotate(90f);
                if (!CheckObstacleInDirection(Vector3.right))
                {
                    _animator.SetTrigger("Hop");
                    StartCoroutine(MovePlayer(Vector3.right, false));
                }
                else
                {
                    _animator.SetInteger("ObjectHit", 4);
                }
            }
            

            
            if (_isOnMovingTarget)
            {
                _movingTargetPos = SetMovingTargetPos();
                transform.position = Vector3.MoveTowards(transform.position, _movingTargetPos, _moveTowardsBoatMiddle);
                
            }
            
        }
        else
        {
            EventManager.OnPlayerDeath?.Invoke();
        }
        
    }

    private IEnumerator MovePlayer(Vector3 direction, bool isFallingInRiver)
    {
        
        _isMoving = true;
        _isAbleToFall = false;
        float _elapsedTime = 0;
        if (!isFallingInRiver)
        {
             _origPos = transform.position;
             _targetPos = _origPos + direction;
                
             //making target pos fixed to an int value
             _targetPos.z = Mathf.Round(_targetPos.z);
             _targetPos.x = Mathf.Round(_targetPos.x);
        }

            
            //player movement logic
        while (_elapsedTime < _timeToMove)
        {
            transform.position = Vector3.Lerp(_origPos, _targetPos, (_elapsedTime / _timeToMove));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPos;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Coin")
        {
            EventManager.OnCoinIncrease?.Invoke(1);
            Debug.Log("got it");
        }
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "Train")
        {
            if (_isAlive)
            {
                _animator.SetTrigger("CarHit");
                _isAlive = false;
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Boat")
        {
            _movingTargetGameObject = other.gameObject;
            _isOnMovingTarget = true;
        }
        if (other.gameObject.tag == "River" && _isAbleToFall && !_isOnMovingTarget && _isAlive)
        {
            _isAlive = false;
            _animator.SetTrigger("WaterFall");
        }
    }
    private void OnCollisionExit(Collision other)
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

    private bool CheckObstacleInDirection(Vector3 direction) 
    {
        if (Physics.Raycast(transform.position, direction, 1f, _obstacleLayer)) 
        {
            return true;
        }
        return false;
    }

    private void PlayerRotate(float Angle)
    {
        _mesh.transform.eulerAngles = new Vector3(0f, Angle, 0f);
    }
    public void CanFallTrigger()
    {
        _isAbleToFall = true;
    }
    public void FinishMove()
    {
        _isMoving = false;
    }
    public void ResetAnimationHitObject()
    {
        _animator.SetInteger("ObjectHit", 0);
    }
    

}

public interface IPlayer{}
