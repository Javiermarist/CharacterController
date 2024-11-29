using UnityEngine;

public class MoverRigidBody : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float targetMass;

    // Como el player no tiene collider debemos hacerlo asi
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic && hit.moveDirection.y > -0.3)
        {
            targetMass = body.mass;
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.linearVelocity = pushDir * moveSpeed / targetMass;
        }
    }
}
