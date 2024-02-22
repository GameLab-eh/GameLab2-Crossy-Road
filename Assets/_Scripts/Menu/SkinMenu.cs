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
    [SerializeField] private GameObject _skinRollerMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _skinPopUp;
    [SerializeField] private TMP_Text _skinPopUpText;
    [SerializeField] private GameObject[] skinProjected;
    [SerializeField] private Material[] skinsMaterials;

    private void OnEnable()
    {
        EventManager.OnReload += StartInizializer;
        EventManager.OnskinObtained += SkinMaterialChanger;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= StartInizializer;
        EventManager.OnskinObtained -= SkinMaterialChanger;
    }
    private void Start()
    {
        StartInizializer();
    }
    private void StartInizializer()
    {
        SkinMaterialChanger();
    }
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
        _skinRollerMenu.SetActive(false);
    }
    public void OnSkinRollerMenu()
    {
        _skinMenu.SetActive(false);
        _skinRollerMenu.SetActive(true);
    }
    private IEnumerator skinPopUpRoutine()
    {
        yield return new WaitForSeconds(4f);
        _skinPopUp.SetActive(false);
    }

    private void SkinMaterialChanger()
    {

        for (int i = 0; i < skinProjected.Length; i++)
        {
            if (GameManager.Instance.skinsUnlocked[i])
            {
                MeshRenderer renderer = skinProjected[i].GetComponent<MeshRenderer>();
                if (renderer != null && skinsMaterials[i] != null)
                {
                    Material[] materials = renderer.materials;
                    materials[0] = skinsMaterials[i];
                    renderer.materials = materials;
                }
            }
        }
    }


}
