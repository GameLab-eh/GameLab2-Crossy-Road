using UnityEngine;

public class rotation : MonoBehaviour
{
    public float velocitaRotazione = 30f;

    // Update viene chiamato una volta per ogni frame
    void Update()
    {
        // Rotazione sull'asse Y (puoi scegliere un altro asse o combinare più assi)
        transform.Rotate(Vector3.left * velocitaRotazione * Time.deltaTime);
    }
}