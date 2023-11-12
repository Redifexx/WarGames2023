using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool isStrafing;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool isGrounded;

    [Header("Player Step Climb")]

    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    [Header("Physics")]
    [SerializeField] public float gravity = 0f;
    [SerializeField] public float gravityRot = 0f;
    [SerializeField] public Vector3 gravityDir;
    [SerializeField] private ConstantForce cForce;
    [SerializeField] public bool isGravDown;
    [SerializeField] public bool isFlipper;
    [SerializeField] public float flipForce;

    [SerializeField] private float verticalSpeed = 0f;
    [SerializeField] private float jumpSpeed = 0f;

    [Header("Camera Stuff")]
    [SerializeField] private FirstPersonCamera playerCamera;

    Vector3 moveDir;
    Vector3 jumpDir;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        cForce = GetComponent<ConstantForce>();
        isFlipper = false;
        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    private void Update()
    {
        if (isGravDown)
        {
            isGrounded = Physics.Raycast(transform.position, gravityDir, playerHeight * 0.5f + 0.2f, whatIsGround);
        }
        else
        {
            isGrounded = Physics.Raycast(transform.position, -gravityDir, playerHeight * 0.5f + 0.2f, whatIsGround);
        }
        MyInput();
        SpeedControl();


        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //stepClimb();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0f)
        {
            isStrafing = true;
        }
        else
        {
            isStrafing = false;
        }

        if (Input.GetKey(jumpKey) && readyToJump && (isGrounded))
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (isGravDown)
        {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }
        else
        {
            moveDir = orientation.forward * verticalInput + -orientation.right * horizontalInput;
        }

        if (isFlipper)
        {
            rb.AddForce(transform.up * flipForce, ForceMode.Force);
        }

        //Gravity
        //rb.AddForce(gravityDir * gravity, ForceMode.Force);

        if (isGrounded)
        {
            verticalSpeed = 0f;
            //gravityDir = new Vector3(0, verticalSpeed, 0);
            if (Input.GetKey(runKey))
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 30f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
        else if (!(isGrounded))
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (isGravDown)
        {
            rb.AddForce(-gravityDir * jumpForce, ForceMode.Impulse);
        }
        else 
        {
            rb.AddForce(gravityDir * jumpForce, ForceMode.Impulse);
        }
        //gravDir fix later
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, orientation.TransformDirection(Vector3.forward), out hitLower, 1f))
        {
            Debug.Log("Low hit");
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, orientation.TransformDirection(Vector3.forward), out hitUpper, 0.5f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.Log("High hit");
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("GravField"))
        {
            gravity = collision.gameObject.GetComponent<GravityData>().gravity;
            //Gravity based on Y direction
            gravityDir = Vector3.down;
            UpdatePlayerGrav(collision);
        }
        if (collision.gameObject.CompareTag("Flipper"))
        {
            flipForce = collision.gameObject.GetComponent<FlipperData>().flipForce;
            if (collision.gameObject.GetComponent<ControlData>().isActive)
            {
                isFlipper = true;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Flipper"))
        {
            isFlipper = false;
        }
    }

    public void UpdatePlayerGrav(Collider col)
    {
        if (col.gameObject.GetComponent<GravityData>().isDown)
        {
            cForce.force = gravityDir * gravity;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            isGravDown = true;
            playerCamera.targetZRot = 0f;
        }
        else
        {
            cForce.force = -gravityDir * gravity;
            transform.rotation = Quaternion.Euler(0f, 180f, 180f);
            isGravDown = false;
            playerCamera.targetZRot = 180f;
        }
    }
}
