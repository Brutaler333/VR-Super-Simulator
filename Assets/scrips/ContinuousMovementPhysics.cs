using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    public float speed = 3;
    public float turnSpeed = 33;
    private float jumpVelocity = 3;
    public float jumpHeight = 1.5f;
    public bool onlyMoveWhenGrounded = false;

    /*public bool jumpWithHand = true;*/
    public float minJumpWithHandSpeed = 10;
    public float maxJumpWithHandSpeed = 15;

    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;

    public Rigidbody rb;
    public Rigidbody leftHandRB;
    public Rigidbody rightHandRB;
    public LayerMask groundLayer;

    public Transform directionSource;
    public Transform turnSource;

    public CapsuleCollider bodyCollider;
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;
    private bool isGrounded;
    
    void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;

        bool jumpInput = jumpInputSource.action.WasPressedThisFrame();

        /*if(!jumpWithHand)
        {
            if(jumpInput && isGrounded)
            {
                jumpVelocity = Mathf.Sqrt(2* -Physics.gravity.y * jumpHeight);    
                rb.linearVelocity = Vector3.up * jumpVelocity;
            }
        }
        else
        {
            bool inputJumpPressed = jumpInputSource.action.IsPressed();
            float handSpeed = ((leftHandRB.linearVelocity - rb.linearVelocity).magnitude + (rightHandRB.linearVelocity - rb.linearVelocity).magnitude) / 2;
            if(inputJumpPressed && isGrounded && handSpeed > minJumpWithHandSpeed)
            {
                rb.linearVelocity = Vector3.up * Mathf.Clamp(handSpeed, minJumpWithHandSpeed, maxJumpWithHandSpeed);
            }
        }*/
        bool inputJumpPressed = jumpInputSource.action.IsPressed();
        float handSpeed = ((leftHandRB.linearVelocity - rb.linearVelocity).magnitude + (rightHandRB.linearVelocity - rb.linearVelocity).magnitude) / 2;
        if(inputJumpPressed && isGrounded && handSpeed > minJumpWithHandSpeed)
        {
            rb.linearVelocity = Vector3.up * jumpVelocity;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        /*if (isGrounded || (onlyMoveWhenGrounded  && isGrounded))
        {

        }*/
        Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);
        Vector3 targetMovePosition = rb.position + direction * Time.fixedDeltaTime * speed;
        Vector3 axis = Vector3.up;
        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MoveRotation(rb.rotation * q);
        Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;
        rb.MovePosition(newPosition);
    }

    public bool CheckIfGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.5f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
}
