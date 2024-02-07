using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] List<DefinitionLayout> layouts = null;
    [SerializeField] string layoutName;

    void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        #endregion
    }

    public DefinitionLayout CurrentLayout
    {
        get
        {
            foreach (DefinitionLayout layout in layouts)
            {
                if (layout.name == layoutName)
                {
                    return layout;
                }
            }
            return null;
        }
    }
}
