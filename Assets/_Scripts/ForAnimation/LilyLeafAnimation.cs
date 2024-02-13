using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyLeafAnimation : MonoBehaviour
{
    private bool isCollided;
    private Animator _animator;

    private void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
    }
    private void OnCollisionEnter(Collision other)
    {
        
        if (isCollided == false)
        {
            _animator.SetTrigger("Hitted");
            isCollided = true;
        }
        
    }
    private void OnCollisionExit(Collision other)
    {
        isCollided = false;
    }
}
