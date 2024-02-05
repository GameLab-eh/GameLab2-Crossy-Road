using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicProps : Props
{
    [SerializeField, Min(0)] float speed;
    [SerializeField, Min(0)] float delay = 0f;

    int bounds;

    private void Awake()
    {
        bounds = (int)(LevelManager.Instance.ChunckWidth / 2) + 2;
    }

    private void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);

        if (transform.position.x > (bounds + 1) || transform.position.x < (-bounds - 1))
        {
            StartCoroutine(CoroutineWait());
            transform.position = new(transform.position.x > (bounds + 1) ? -bounds : bounds, 0, transform.position.z);
        }
    }

    public void Reverse()
    {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    IEnumerator CoroutineWait()
    {
        float spedbackup = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = spedbackup;
    }
}
