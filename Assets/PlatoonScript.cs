using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatoonScript : MonoBehaviour
{
      private Animator _animator
      [SerializableField] private float _minValue,_maxValue;

      private void StartInitializer()
      {
            _animator = GetComponent <Animator> ();

            StartCoroutine(AnimationRoutine);
      }

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

      private iEnumerator AnimationRoutine()
      {
        float index=Random.Range(_minValue,_maxValue);

        yield return new WaitForSeconds(index);
      }

}
