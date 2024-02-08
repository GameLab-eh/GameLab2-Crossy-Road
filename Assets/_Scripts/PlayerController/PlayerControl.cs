using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IPlayer
{
    //checks
    private bool _isMoving, _isOnMovingTarget, _isAlive = true;
    
    //movements
    private Vector3 _origPos, _targetPos;
    [SerializeField] private float _timeToMove;

    [SerializeField] private LayerMask _obstacleLayer;
    //movements on logs/boat/ecc..
    private Vector3 _movingTargetPos;
    private GameObject _movingTargetGameObject;
    [SerializeField] private float _moveTowardsBoatMiddle;
    
    private void Awake()
    {
        //transform.position = new Vector3(-5f, 0.5f, 0f);
    }
    private void Update()
    {
        if (_isAlive)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !_isMoving)
                if (!CheckObstacleInDirection(Vector3.forward))
                    StartCoroutine(MovePlayer(Vector3.forward, false));
            
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !_isMoving)
                if (!CheckObstacleInDirection(Vector3.back))
                    StartCoroutine(MovePlayer(Vector3.back, false));
            
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !_isMoving)
                if (!CheckObstacleInDirection(Vector3.left))
                    StartCoroutine(MovePlayer(Vector3.left, false));
            
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !_isMoving)
                if (!CheckObstacleInDirection(Vector3.right))
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

    private void OnCollisionEnter(Collision other)
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

    private void FallingInRiver()
    {
        _isAlive = false;
        _targetPos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
        StartCoroutine(MovePlayer(Vector3.down, true));
    }

    bool CheckObstacleInDirection(Vector3 direction) 
    {
        if (Physics.Raycast(transform.position, direction, 1f, _obstacleLayer)) 
        {
            return true;
        }
        return false;
    }

}

public interface IPlayer{}
