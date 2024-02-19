using System.Collections;
using UnityEngine;

public class DynamicProps : Props
{
    [SerializeField, Min(0)] float speed;
    [SerializeField, Min(0)] float startDelay = 0f;
    [SerializeField, Min(0)] float delay = 0f;

    int bounds;

    public delegate void TrainSignal(bool value, float z);
    public static event TrainSignal run = null;

    bool start = false;

    private void Awake()
    {
        bounds = (int)(GameManager.Instance.MapManager.RowWidth / 2) + 2;
        bounds += transform.name == "Train(Clone)" ? Size : 0;
    }

    private void Start()
    {
        if (startDelay == 0f) return;
        StartCoroutine(CoroutineWait(startDelay));
        start = true;
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
        if (start) run?.Invoke(false, transform.position.z);
        float speedbackup = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        if (start) run?.Invoke(true, transform.position.z);
        speed = speedbackup;
    }

    public float Speed => speed;
    public void StartDelay(float value) => startDelay = value;
}
