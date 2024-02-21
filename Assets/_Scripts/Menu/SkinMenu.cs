using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : MonoBehaviour
{
    private bool[] _selectedSkin;
    [SerializeField] private GameObject _skinMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _skinPopUp;
    [SerializeField] private TMP_Text _skinPopUpText;

    public void Selector(int index)
    {
        
        if (GameManager.Instance.skinsUnlocked[index])
        {
            EventManager.OnSkinChoice?.Invoke(index);
            _skinPopUpText.text = "Skin Selected!!!";
        }
        else
        {
            _skinPopUpText.text = "Skin Not Unlocked :(";
        }
        _skinPopUp.SetActive(true);
        StartCoroutine(skinPopUpRoutine());
    }
    public void OnPlayMenu()
    {
        _inGameMenu.SetActive(true);
        _skinMenu.SetActive(false);
        if (_skinPopUp.activeSelf)
        {
            _skinPopUp.SetActive(false);
        }
    }
    public void OnSkinMenu()
    {
        _skinMenu.SetActive(true);
        _inGameMenu.SetActive(false);
    }
    private IEnumerator skinPopUpRoutine()
    {
        yield return new WaitForSeconds(4f);
        _skinPopUp.SetActive(false);
    }


}
