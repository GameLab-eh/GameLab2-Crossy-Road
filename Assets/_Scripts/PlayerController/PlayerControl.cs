using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IPlayer
{
    [SerializeField] private GameObject SkinButton;

    //checks
    private bool _isMoving, _isOnMovingTarget, _isAlive = true, _isAbleToFall, _isMovedFirstTime;
    [SerializeField] private GameObject SkinMenu;

    //movements
    private Vector3 _origPos, _targetPos;
    [SerializeField] private float _timeToMove, _timeToMoveBird;
    [SerializeField] private GameObject _mesh;
    private Animator _animator;


    [SerializeField] private LayerMask _obstacleLayer, _waterLayer;

    //movements on logs/boat/ecc..
    private Vector3 _movingTargetPos;
    private GameObject _movingTargetGameObject;
    [SerializeField] private float _moveTowardsBoatMiddle;
    private float _myOldXValue;
    [SerializeField] private float xMapLimitMax, xMapLimitMin;

    //variables for score
    private int myZValue;
    private int myMaxZValue;

    //audio effects
    private AudioSource audioSource;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip fallInWater;
    [SerializeField] AudioClip step;
    [SerializeField] AudioClip coinPickup;
    [SerializeField] AudioClip stepOnLog;

    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
        EventManager.OnSkinChoice += MeshChanger;
        EventManager.OnBirdArrived += BirdRoutineStarter;
        EventManager.OnPlayerOutOfCam += PlayerOutOfCam;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnSkinChoice -= MeshChanger;
        EventManager.OnBirdArrived -= BirdRoutineStarter;
        EventManager.OnPlayerOutOfCam -= PlayerOutOfCam;
    }

    private void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        StartCoroutine(MeshChangerRoutine());
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
                    StartCoroutine(MovePlayer(Vector3.forward));
                    PlayEffect(step);
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
                    StartCoroutine(MovePlayer(Vector3.back));
                    PlayEffect(step);
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
                    if (_isOnMovingTarget && _myOldXValue > transform.position.x)
                    {
                        StartCoroutine(MovePlayer(new Vector3(-0.2f, 0, 0)));
                        Debug.Log("hello");
                    }
                    else
                    {
                        StartCoroutine(MovePlayer(Vector3.left));
                    }
                    PlayEffect(step);
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
                    if (_isOnMovingTarget && _myOldXValue < transform.position.x)
                    {
                        StartCoroutine(MovePlayer(new Vector3(0.2f, 0, 0)));
                        Debug.Log("hello");
                    }
                    else
                    {
                        StartCoroutine(MovePlayer(Vector3.right));
                    }
                    PlayEffect(step);
                }
                else
                {
                    _animator.SetInteger("ObjectHit", 4);
                }
            }

            if (transform.position.x >= xMapLimitMax || transform.position.x <= xMapLimitMin)
            {
                _isAlive = false;
                Debug.Log("arriva l'aquila");
                EventManager.OnBirdAction?.Invoke();
            }

        }
        else
        {
            EventManager.OnPlayerDeath?.Invoke();
        }
        if (_isOnMovingTarget)
        {
            _movingTargetPos = SetMovingTargetPos();
            transform.position = Vector3.MoveTowards(transform.position, _movingTargetPos, _moveTowardsBoatMiddle);
        }

    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        if (!_isMovedFirstTime)
        {
            _isMovedFirstTime = true;
            EventManager.OnPlayerFirstMove?.Invoke();
        }
        if (SkinMenu.activeSelf)
        {
            yield break;
        }
        SkinButton.SetActive(false);

        _isMoving = true;
        _isAbleToFall = false;
        float _elapsedTime = 0;
        _origPos = transform.position;
        _targetPos = _origPos + direction;
        if (_isOnMovingTarget && Mathf.Abs((int)direction.z) == 1)
        {
            _targetPos.x = Mathf.Round(_targetPos.x);
        }
        _targetPos.z = Mathf.Round(_targetPos.z);


        while (_elapsedTime < _timeToMove)
        {
            transform.position = Vector3.Lerp(_origPos, _targetPos, (_elapsedTime / _timeToMove));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPos;
        _myOldXValue = transform.position.x;
        ScoreCaller();

    }

    private void ScoreCaller()
    {
        myZValue = (int)transform.position.z;
        if (myZValue > myMaxZValue)
        {
            myMaxZValue = myZValue;
            EventManager.OnPlayerMoveUp?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Coin")
        {
            PlayEffect(coinPickup);
            EventManager.OnCoinIncrease?.Invoke(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "Train")
        {
            if (_isAlive)
            {
                PlayEffect(death);
                _animator.SetTrigger("CarHit");
                _isAlive = false;
            }
        }
        else if(other.gameObject.tag == "River")
        {
            PlayEffect(fallInWater);
        }
        else if (other.gameObject.tag == "Boat" && other.gameObject.tag != "River")
        {
            PlayEffect(stepOnLog);
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Boat")
        {
            _movingTargetGameObject = other.gameObject;
            _isOnMovingTarget = true;
        }
        if (_isAbleToFall && !_isOnMovingTarget && _isAlive && other.gameObject.tag == "River")
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
    public void MeshChanger(int skinIndex)
    {
        StartCoroutine(MeshChangerRoutine());
    }
    private IEnumerator MeshChangerRoutine()
    {
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        _mesh = GameObject.FindGameObjectWithTag("Skin");
    }
    private void BirdRoutineStarter()
    {
        StartCoroutine(MoveWithBird());
    }
    private IEnumerator MoveWithBird()
    {
        yield return new WaitForNextFrameUnit();
        float _elapsedTime = 0;
        _origPos = transform.position;
        _targetPos = _origPos - new Vector3(0, -10, 15);

        while (_elapsedTime < _timeToMoveBird)
        {
            transform.position = Vector3.Lerp(_origPos, _targetPos, (_elapsedTime / _timeToMoveBird));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
    private void PlayerOutOfCam()
    {
        _isAlive = false;
        EventManager.OnBirdAction?.Invoke();
    }

    private void PlayEffect(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }
}

public interface IPlayer { }
