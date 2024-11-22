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

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<CharacterController>();
        fallVelocity = 0;
    }

    void Update()
    {
        Movement();
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
    }

    private void Movement()
    {
        Vector2 inputV2 = playerInput.actions["Move"].ReadValue<Vector2>();
        input = new Vector3(inputV2.x, 0, inputV2.y);
        input = Vector3.ClampMagnitude(input, 1);
        movePlayer = input * moveSpeed;
    }
}
