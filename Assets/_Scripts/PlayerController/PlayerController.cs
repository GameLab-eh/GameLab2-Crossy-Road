using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // velocità di movimento del player

    void Update()
    {
        // Input per movimento in avanti
        if (Input.GetKey(KeyCode.W))
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        // Calcola il vettore di movimento in avanti
        Vector3 moveDirection = transform.right * speed * Time.deltaTime;

        // Applica il movimento
        transform.Translate(moveDirection, Space.World);
    }
}
