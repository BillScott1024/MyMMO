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
                    rotationAmount = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
                }
            }
            target.transform.RotateAround(target.transform.up, rotationAmount);
            
        
        }
    }
}
