using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour
{
    [SerializeField] List<GameObject> terrains;

    public List<GameObject> Terrains() { return terrains; }
}
