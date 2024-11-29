using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public PickUpObject pickUpObject;

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

    public bool entraEnCaida = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<CharacterController>();
        fallVelocity = 0;

        entraEnCaida = false;
    }

    void Update()
    {
        //Movement();
        //camDirection();
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
        SlideDown2();
    }

    public void SlideDown()
    {
        isOnBigSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnBigSlope)
        {
            entraEnCaida = true;
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;
            //movePlayer.y = -slideVelocity;
            movePlayer.y = -slopeForceDown;
        }
        else
        {

            if (entraEnCaida)
            {
                entraEnCaida = false;
                movePlayer.x = 0;
                movePlayer.z = 0;
                movePlayer.y = 0;
            }
        }
    }

    public void SlideDown2()
    {
        isOnBigSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnBigSlope)
        {
            entraEnCaida = true;
            // Calcular la dirección del deslizamiento en funcion de la pendiente
            Vector3 SlopeDirection = Vector3.ProjectOnPlane(Vector3.down, hitNormal).normalized;
            // Aplicar deslizamiento en direccion de la pendiente
            movePlayer = Vector3.Lerp(movePlayer, SlopeDirection * slideVelocity, Time.deltaTime * 10f);
            // Añadir una fuerza hacia abajo para simular gravedad extra en la pendiente
            movePlayer.y -= slopeForceDown * Time.deltaTime;
        }
        else
        {
            if (entraEnCaida)
            {
                // Desacelerar gradualmente el movimiento
                entraEnCaida = false;
                movePlayer.x = 0;
                movePlayer.z = 0;
                movePlayer.y = 0;
            }
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
        //movePlayer = input.x * cameraRight + input.z * cameraForward;
        movePlayer = input;
        movePlayer *= moveSpeed;
        //player.transform.LookAt(player.transform.position + movePlayer);
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
