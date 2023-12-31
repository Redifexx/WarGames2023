using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
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

    private void Start()
    {
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
        if (isFlipper)
        {
            if (isGravDown)
            {
                rb.AddForce(Vector3.up * flipForce, ForceMode.Force);
            }
            else
            {
                rb.AddForce(Vector3.down * flipForce, ForceMode.Force);
            }
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
