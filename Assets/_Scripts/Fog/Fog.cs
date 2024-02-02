using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] GameObject _backFog;
    [SerializeField] GameObject _leftFog;
    [SerializeField] GameObject _rigthFog;

    [SerializeField] Transform _player;

    private void Start()
    {
        float width = LevelManager.Instance.ChunckWidth;
        _backFog.transform.localScale = new((width / 10f), _backFog.transform.localScale.y, _backFog.transform.localScale.z);
        _leftFog.transform.position = new(_leftFog.transform.position.x, _leftFog.transform.position.y, width - (width / 4));
        _rigthFog.transform.position = new(_leftFog.transform.position.x, _leftFog.transform.position.y, width / 4);
    }

    private void Update()
    {
        float x = _player.position.x;
        _leftFog.transform.position = new(x, _leftFog.transform.position.y, _leftFog.transform.position.z);
        _rigthFog.transform.position = new(x, _rigthFog.transform.position.y, _rigthFog.transform.position.z);
    }
}
