using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkinRollMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _skinObtainedText;
    [SerializeField] private GameObject _skinObtained, _bertAnimation, _skinPopUp;
    [SerializeField] private int _skinCost;
    [SerializeField] private Transform skinSpawnPoint;
    private int _skinIndex;
    private GameObject toDestroy;
    

    private void Start()
    {
        int _coins = UIManager._coins;
        _coinText.text = "Coins: " + _coins;
    }
    private void OnEnable()
    {
        EventManager.OnCoinIncrease += CoinUp;
    }
    private void OnDisable()
    {
        EventManager.OnCoinIncrease -= CoinUp;
    }
    
    private void CoinUp(int coinToAdd)
    {
        int _coins = UIManager._coins;
        _coinText.text = "Coins: " + _coins;
    }

    public void SkinObtainer()
    {
        if (AllSkinObtained())
        {
            _skinObtainedText.text = "Mi dispiace ma hai già tutte le skin, aspetta un nuovo aggiornamento per... GOTTA CATCH 'EM ALL";
            _skinObtained.SetActive(true);
            StartCoroutine(skinObtainedRoutine());
        }
        else if (UIManager._coins >= _skinCost)
        {
            SkinRoller();
        }
        else 
        {
            int missingCoins = _skinCost-UIManager._coins;
            _skinObtainedText.text = "Non hai abbastanza coin, te ne mancano: " + missingCoins;
            _skinObtained.SetActive(true);
            StartCoroutine(skinObtainedRoutine());
        }
    }
    private void SkinRoller()
    {
        _skinIndex = Random.Range(1, GameManager.Instance.skinsUnlocked.Length);
        Debug.Log(_skinIndex);
        if (GameManager.Instance.skinsUnlocked[_skinIndex])
        {
            SkinRoller();
        }
        else
        {
            if (_bertAnimation.activeSelf || _skinPopUp.activeSelf)
            {
                _bertAnimation.SetActive(false);
                _skinPopUp.SetActive(false);
            }
            GameManager.Instance.skinsUnlocked[_skinIndex] = true;
            EventManager.OnskinObtained?.Invoke();
            EventManager.OnCoinIncrease?.Invoke(-_skinCost);
            int skinIndexForHuman = _skinIndex + 1;
            _skinObtainedText.text = "hai pagato " + _skinCost + " e ricevuto la skin numero " + skinIndexForHuman;
            _skinObtained.SetActive(true);
            _bertAnimation.SetActive(true);
            if (toDestroy != null)
            {
                Destroy(toDestroy);
            }
            StartCoroutine(SkinShower());

        }
    }
    private IEnumerator skinObtainedRoutine()
    {
        yield return new WaitForSeconds(4f);
        _skinObtained.SetActive(false);
    }
    private bool AllSkinObtained()
    {
        foreach (bool skinUnlocked in GameManager.Instance.skinsUnlocked)
        {
            if (!skinUnlocked)
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerator SkinShower()
    {
        yield return new WaitForSeconds(2.10f);
        _skinPopUp.SetActive(true);
        GameObject newObj = Instantiate(GameManager.Instance._skins[_skinIndex], skinSpawnPoint.position, quaternion.identity);
        newObj.transform.localScale += new Vector3(5f,5f,5f);
        SkinOBLITERATOR(newObj);
    }
    private void SkinOBLITERATOR(GameObject obj)
    {
        toDestroy = obj;
    }
    public void OnSkinMenu()
    {
        _skinObtained.SetActive(true);
        if (_bertAnimation.activeSelf || _skinPopUp.activeSelf)
        {
            _bertAnimation.SetActive(false);
            _skinPopUp.SetActive(false);
        }
        if (toDestroy != null)
        {
            Destroy(toDestroy);
        }
    }
}
