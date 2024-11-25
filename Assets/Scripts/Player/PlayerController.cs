using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public CharacterController player;
    private Vector3 movePlayer;

    private PlayerInput playerInput;
    private Vector3 input;

    public float gravedad = 9.8f;
    public float fallVelocity;

    public float jumpVelocity;

    public Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    // Fall
    public bool isOnBigSlope = false;
    public Vector3 hitNormal; // Necesitamos la normal para saber en que superficie estamos
    public float slideVelocity;
    public float slopeForceDown;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<CharacterController>();
        fallVelocity = 0;
    }

    void Update()
    {
        Movement();
        camDirection();
        SetGravity();
        player.Move(movePlayer * Time.deltaTime);
    }

    private void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravedad * Time.deltaTime;
            /*movePlayer.y = fallVelocity;*/

            if (Input.GetKeyDown(KeyCode.Space))
            {
                fallVelocity = jumpVelocity;
            }
        }
        else
        {
            fallVelocity -= gravedad * Time.deltaTime;
            /*movePlayer.y = fallVelocity;*/
        }
        movePlayer.y = fallVelocity;
        SlideDown();
    }

    public void SlideDown()
    {
        isOnBigSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnBigSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;
            movePlayer.y = -slideVelocity;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private void Movement()
    {
        Vector2 inputV2 = playerInput.actions["Move"].ReadValue<Vector2>();
        input = new Vector3(inputV2.x, 0, inputV2.y);
        input = Vector3.ClampMagnitude(input, 1);
        //movePlayer = input.x * cameraRight + input.z * cameraForward;
        movePlayer = input.x * cameraRight + input.z * cameraForward;
        movePlayer *= moveSpeed;
        player.transform.LookAt(player.transform.position + movePlayer);
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputV2 = value.Get<Vector2>();
        input = new Vector3(inputV2.x, 0, inputV2.y);
        input = Vector3.ClampMagnitude(input, 1);
        movePlayer = input.x * cameraRight + input.z * cameraForward;
        movePlayer *= moveSpeed;
        player.transform.LookAt(player.transform.position + movePlayer);
    }

    private void camDirection()
    {
        cameraForward = mainCamera.transform.forward;
        cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }
}
