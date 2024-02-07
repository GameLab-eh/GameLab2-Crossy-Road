using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class MapGenerator : MonoBehaviour
{
    DefinitionLayout layout;

    private void Awake()
    {
        layout = GameManager.Instance.CurrentLayout;
    }


}
