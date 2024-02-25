using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatoonScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private float _minValue,_maxValue;


    private void Start()
    {
        StartInitializer();
    }

    private void OnEnable()
    {
        EventManager.OnReload += StartInitializer;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= StartInitializer;
    }
    private void StartInitializer()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        float index = Random.Range(_minValue, _maxValue);
        yield return new WaitForSeconds(index);

        _animator.SetTrigger("Delay");
    }

}
