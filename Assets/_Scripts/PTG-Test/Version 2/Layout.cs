using System.Collections.Generic;
using UnityEngine;

public class Layout : MonoBehaviour
{
    [SerializeField] List<Theme> themeList;
    [SerializeField, Tooltip("number of chunck before change")] int chunckDelay;
    [SerializeField] int obstacleDensity = 2;
    [SerializeField] AnimationCurve difficultyCurve;

    public Theme Theme(int index) => themeList[index];
    public List<Theme> ThemeList => themeList;
    public string Name => this.name;
    public int ObstacleDensity => obstacleDensity;
    public AnimationCurve DifficultyCurve => difficultyCurve;
    public int ChunckDelay => chunckDelay;
}
