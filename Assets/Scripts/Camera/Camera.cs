using UnityEngine;

public class Camera : MonoBehaviour
{
    public Vector3 offset;
    private Transform target;
    [Range(0,1)] public float lerpValue;
    [SerializeField] private float sensibilidad;

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {

    }
}
