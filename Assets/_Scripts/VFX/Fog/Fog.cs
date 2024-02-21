using UnityEngine;

public class Fog : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] GameObject _leftFog;
    [SerializeField] GameObject _rigthFog;

    [SerializeField] Transform _player;

    private void Start()
    {
        float width = GameManager.Instance.MapManager.GameRowWidth;
        _leftFog.transform.position = new(-(width / 2), _leftFog.transform.position.y, _leftFog.transform.position.z);
        _rigthFog.transform.position = new(width / 2, _leftFog.transform.position.y, _leftFog.transform.position.z);
    }

    private void Update()
    {
        float z = _player.position.z;
        _leftFog.transform.position = new(_leftFog.transform.position.x, _leftFog.transform.position.y, z);
        _rigthFog.transform.position = new(_rigthFog.transform.position.x, _rigthFog.transform.position.y, z);
    }
}
