using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinMenu : MonoBehaviour
{
    private bool[] _selectedSkin;
    [SerializeField] private GameObject _skinMenu;
    [SerializeField] private GameObject _inGameMenu;

    public void Selector(int index)
    {
        EventManager.OnSkinChoice?.Invoke(index);
    }
    public void OnPlayMenu()
    {
        _inGameMenu.SetActive(true);
        _skinMenu.SetActive(false);
    }
    public void OnSkinMenu()
    {
        _skinMenu.SetActive(true);
        _inGameMenu.SetActive(false);
    }


}
