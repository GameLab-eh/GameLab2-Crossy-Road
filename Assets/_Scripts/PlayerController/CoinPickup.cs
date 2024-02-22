using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
   
    [SerializeField] private int _coinToAdd;

    private void OnTriggerEnter(Collider other)
    {
        _coinToAdd = 30;
        if (other.gameObject.layer == 31)
        {
            _coinToAdd = 1;
            EventManager.OnCoinIncrease?.Invoke(_coinToAdd);
            Destroy(gameObject);
        }
        
    }
    
}
