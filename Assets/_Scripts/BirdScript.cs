using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    private Transform _player;
    private Vector3 _origPos, _targetPos;
    [SerializeField] private float _timeToMove;
    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
        EventManager.OnBirdAction += MoveRoutineStarter;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnBirdAction -= MoveRoutineStarter;
    }

    private void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void MoveRoutineStarter()
    {
        StartCoroutine(MoveOnPlayer());
    }
    private IEnumerator MoveOnPlayer()
    {
        float _elapsedTime = 0;
        _origPos = transform.position;
        
        while (_elapsedTime < _timeToMove)
        {
            _targetPos = _player.position + (new Vector3(0,1,0));
            transform.position = Vector3.Lerp(_origPos, _targetPos, (_elapsedTime / _timeToMove));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        EventManager.OnBirdArrived?.Invoke();
    }
}
