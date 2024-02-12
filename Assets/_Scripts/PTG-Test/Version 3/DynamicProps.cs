using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class DynamicProps : Props
{
    [SerializeField, Min(0)] float speed;
    [SerializeField, Min(0)] float startDelay = 0f;
    [SerializeField, Min(0)] float delay = 0f;

    int bounds;

    private void Awake()
    {
        bounds = (int)(GameManager.Instance.MapManager.RowWidth / 2) + 2;
    }

    private void Start()
    {
        StartCoroutine(CoroutineWait(startDelay));
    }

    private void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * Vector3.left);

        if (transform.position.x > (bounds + 1) || transform.position.x < (-bounds - 1))
        {
            StartCoroutine(CoroutineWait(delay));
            transform.position = new(transform.position.x > (bounds + 1) ? -bounds : bounds, 0, transform.position.z);
        }
    }

    public void Reverse()
    {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    IEnumerator CoroutineWait(float delay)
    {
        float spedbackup = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = spedbackup;
    }

    public float Speed => speed; 
    public void StartDelay(float value) => startDelay = value;
}
