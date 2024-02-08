using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
   
    [SerializeField] private int _coinToAdd;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ENTRA?!?!?!?");
        if (other.gameObject.TryGetComponent(out IPlayer player))
        {
            Debug.Log("NON PENSO PROPRIO");
            EventManager.OnCoinIncrease?.Invoke(_coinToAdd);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NON ENTRA");
    }

}
