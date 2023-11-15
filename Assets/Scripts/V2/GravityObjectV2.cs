using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObjectV2 : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] public float gravity = 0f;
    [SerializeField] public Vector3 gravityDir;
    [SerializeField] private ConstantForce cForce;
    [SerializeField] public bool isGravDown;
    [SerializeField] public bool isFlipper;
    [SerializeField] public bool isHeld;
    [SerializeField] public bool isCrate;
    [SerializeField] public float flipForce;
    [SerializeField] public float gravMult;
    [SerializeField] public Collider curCollider;
    Rigidbody rb;

    //New
    //[SerializeField] SphereCollider sphereCol;
    public LayerMask gravMask;
    public float checkInterval = 0.1f;

    private float timeSinceLastCheck = 0f;


    private void Start()
    {
        //sphereCol = GetComponent<SphereCollider>();
        gravMask = 1 << LayerMask.NameToLayer("Grav");
        rb = GetComponent<Rigidbody>();
        cForce = GetComponent<ConstantForce>();
        isHeld = false;
        if (this.gameObject.CompareTag("Crate"))
        {
            isCrate = true;
        }
        else
        {
            isCrate = false;
        }
        isGravDown = true;
    }

    private void FixedUpdate()
    {
        timeSinceLastCheck += Time.fixedDeltaTime;

        if (timeSinceLastCheck >= checkInterval)
        {
            // Get the radius of the SphereCollider
            float sphereRadius = 50f;

            // Use Physics.OverlapSphere with the determined radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius, gravMask);

            foreach (Collider collider in colliders)
            {
                // Do something with the GameObjects within the sphere collider
                Debug.Log("Object in sphere collider: " + collider.gameObject.name);
            }

            timeSinceLastCheck = 0f;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("GravField"))
        {
            curCollider = collision;
            gravityDir = Vector3.down;
            if (isHeld)
            {
                gravMult = 0f;
            }
            else
            {
                gravMult = 1f;
            }
            UpdateOBJGrav(collision, gravMult);
        }
        if (collision.gameObject.CompareTag("Flipper"))
        {
            flipForce = collision.gameObject.GetComponent<FlipperData>().flipForce;
            isFlipper = true;
        }
        if (collision.gameObject.CompareTag("QuantumPad") && !isCrate)
        {
            collision.gameObject.GetComponent<QuantumController>().objectCount++;
            collision.gameObject.GetComponent<QuantumController>().Check();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Flipper"))
        {
            flipForce = 0f;
            isFlipper = false;
        }
        if (collision.gameObject.CompareTag("QuantumPad") && !isCrate)
        {
            collision.gameObject.GetComponent<QuantumController>().objectCount--;
            collision.gameObject.GetComponent<QuantumController>().Check();
        }
    }

    public void UpdateOBJGrav(Collider col, float gravMult_)
    {
        gravity = col.gameObject.GetComponent<GravityData>().gravity;
        if (col.gameObject.GetComponent<GravityData>().isDown)
        {
            cForce.force = gravityDir * gravity * gravMult_;
            isGravDown = true;
        }
        else
        {
            cForce.force = -gravityDir * gravity * gravMult_;
            isGravDown = false;
        }
    }

    public void UpdateOBJFlipForce(float flipForce_)
    {
        flipForce = flipForce_;
        //isFlipper = isFlipper_;
    }

    public void LetGo()
    {
        //UpdateOBJGrav(curCollider, 1f);
    }
}
