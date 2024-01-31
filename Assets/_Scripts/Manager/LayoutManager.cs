using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    [SerializeField] List<Theme> themeList;
    [SerializeField] int changeFrenquency;

    public int ChangeFrenquency() {  return changeFrenquency; }

    public Theme GetTheme(int value) { return themeList[value]; }
}
