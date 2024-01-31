using System.Collections;
using UnityEditor;
using UnityEngine;

public class ObstacleMotion : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float trainDelay = 2f;

    private int boundRigth = -3;
    private int boundLeft;
    private int x;
    private int origin = -2;
    private Vector3 dir = Vector3.forward;

    private void Awake()
    {
        boundLeft = LevelManager.Instance.GetChunckWidth() + 2;

        x = (int)transform.position.x;
        boundRigth = -3;
        origin = -2;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * dir);

        if (transform.position.z > boundLeft || transform.position.z < boundRigth)
        {
            if (transform.CompareTag("Train")) StartCoroutine(CoroutineWait());
            transform.position = new(x, 0, origin);
        }
    }

    public void Reverse()
    {
        dir = -Vector3.forward;
        origin = boundLeft;
    }

    IEnumerator CoroutineWait()
    {
        float spedbackup = speed;
        speed = 0;
        yield return new WaitForSeconds(trainDelay);

        speed = spedbackup;
    }
}
