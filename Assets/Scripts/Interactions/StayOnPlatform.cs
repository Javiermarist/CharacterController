using UnityEngine;

public class StayOnPlatform : MonoBehaviour
{
    private Transform platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            platform = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (platform != null && other.CompareTag("MovingPlatform"))
        {
            // Sincronizar la posición del jugador con la de la plataforma
            transform.position = platform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            platform = null;
        }
    }
}
