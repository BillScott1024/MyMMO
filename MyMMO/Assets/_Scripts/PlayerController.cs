using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void MyJumpDelegate();
    public Rigidbody target;
    public float speed = 1.0f;
    public float walkSpeedDownscale = 2.0f;
    public float turnSpeed = 2.0f;
    public float mouseTurnSpeed = 0.3f;
    public float jumpSpeed = 1.0f;
    public LayerMask groundLayers = -1;
    public float groundedCheckOffset = 0.7f;
    public bool showGizmos = true;
    public bool requireLock = true;
    public bool controlLock = true;
    public MyJumpDelegate onJump = null;
    private const float inputThreshold = 0.01f;
    private const float groundDrag = 5.0f;
    private const float directionalJumpFactor = 0.7f;
    private const float groundedDistance = 0.5f;
    private bool grounded;
    private bool walking;

    float SidestepAxisInput
    {
        get
            {
            if (Input.GetMouseButtonDown(1))
            {
                float sidestep = Input.GetAxis("Sidestep");
                float horizontal = Input.GetAxis("Horizontal");
                return Mathf.Abs(sidestep) > Mathf.Abs(horizontal) ? sidestep : horizontal;
            }
            else
            {
                return Input.GetAxis("Sidestep");
            }
        }
    }

    public bool Grounded
    {
        get { return grounded; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GetComponent<Rigidbody>();
        }
        if (target == null)
        {
            Debug.LogError("变量target未赋值");
            enabled = false;
            return;
        }
        target.freezeRotation = true;
        walking = false;
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount;
        if (Input.GetMouseButton(1) && (!requireLock || controlLock || Cursor.lockState == CursorLockMode.Locked))
        {         
            if (controlLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
                rotationAmount = Input.GetAxis("Mouse X") * mouseTurnSpeed * Time.deltaTime;
            }
            else
            {
                if (controlLock)
                {
                    Cursor.lockState = CursorLockMode.None;
                    
                }
                rotationAmount = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            }
            target.transform.RotateAround(target.transform.up, rotationAmount);
            if (Input.GetButtonDown("ToggleWalk")) {
                walking = !walking;
            }
        
        }
    }

    void FixedUpdate()
    // Handle movement here since physics will only be calculated in fixed frames anyway
    {
        grounded = Physics.Raycast(
            target.transform.position + target.transform.up * -groundedCheckOffset,
            target.transform.up * -1,
            groundedDistance,
            groundLayers
        );
        // Shoot a ray downward to see if we're touching the ground

        if (grounded)
        {
            target.drag = groundDrag;
            // Apply drag when we're grounded

            if (Input.GetButton("Jump"))
            // Handle jumping
            {
                target.AddForce(
                    jumpSpeed * target.transform.up +
                        target.velocity.normalized * directionalJumpFactor,
                    ForceMode.VelocityChange
                );
                // When jumping, we set the velocity upward with our jump speed
                // plus some application of directional movement

                if (onJump != null)
                {
                    onJump();
                }
            }
            else
            // Only allow movement controls if we did not just jump
            {
                Vector3 movement = Input.GetAxis("Vertical") * target.transform.forward +
                    SidestepAxisInput * target.transform.right;

                float appliedSpeed = walking ? speed / walkSpeedDownscale : speed;
                // Scale down applied speed if in walk mode

                if (Input.GetAxis("Vertical") < 0.0f)
                // Scale down applied speed if walking backwards
                {
                    appliedSpeed /= walkSpeedDownscale;
                }

                if (movement.magnitude > inputThreshold)
                // Only apply movement if we have sufficient input
                {
                    target.AddForce(movement.normalized * appliedSpeed, ForceMode.VelocityChange);
                }
                else
                // If we are grounded and don't have significant input, just stop horizontal movement
                {
                    target.velocity = new Vector3(0.0f, target.velocity.y, 0.0f);
                    return;
                }
            }
        }
        else
        {
            target.drag = 0.0f;
            // If we're airborne, we should have no drag
        }
    }

}
