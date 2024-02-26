using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.PlayerController
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] private Transform _player, playerDieingPosition;
        [SerializeField] private float _cameraForwardSpeed, _cameraChasingPlayerSpeed;
        private bool _isPlayerAlive = true, _isWatchingPlayer, _haveToWatchPlayer, _isPlayerDeadForOthers, isPlayerMoved;
        [SerializeField] private float _maxCameraDistance, _cameraBackSpeed, _playerDieingDistance;

        private void OnEnable()
        {
            EventManager.OnPlayerDeath += AlreadyDead;
            EventManager.OnPlayerFirstMove += PlayerMoved;
        }
        private void OnDisable()
        {
            EventManager.OnPlayerDeath -= AlreadyDead;
            EventManager.OnPlayerFirstMove -= PlayerMoved;
        }


        private void Update()
        {
            if (isPlayerMoved)
            {
                if (!_isPlayerDeadForOthers)
                {
                    if (_isPlayerAlive)
                    {
                        Vector3 newPosition = transform.position + Vector3.forward * _cameraForwardSpeed * Time.deltaTime;
                        transform.position = newPosition;
                        
                        if (_player.position.z > (transform.position.z + _maxCameraDistance))
                        {
                            Vector3 newPos = transform.position + Vector3.forward * _cameraChasingPlayerSpeed * Time.deltaTime;
                            transform.position = newPos;
                        }
                        if (_player.position.x != transform.position.x +2f)
                        {
                            Vector3 newPos = transform.position;
                            newPos.x = _player.position.x + 2f;
                            transform.position = Vector3.MoveTowards(transform.position, newPos, _cameraChasingPlayerSpeed * Time.deltaTime);
                        }
                    }
                    if (_player.position.z < transform.position.z + _playerDieingDistance)
                    {
                        _isPlayerAlive = false;
                        EventManager.OnPlayerOutOfCam?.Invoke();
                        playerDieingPosition = _player;
                    }
                }
                if (!_isWatchingPlayer && !_isPlayerAlive)
                { 
                    Vector3 newPosition = transform.position + Vector3.back * (_cameraForwardSpeed * _cameraBackSpeed) * Time.deltaTime; 
                    transform.position = newPosition;
                }
                if (playerDieingPosition.position.z > (transform.position.z + _maxCameraDistance) && !_isPlayerAlive)
                {
                    _isWatchingPlayer = true;
                }
            }

        }
        private void AlreadyDead()
        {
            _isPlayerDeadForOthers = true;
        }
        private void PlayerMoved()
        {
            isPlayerMoved = true;
        }
        
    }
}
