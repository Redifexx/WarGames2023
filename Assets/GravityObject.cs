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
    [SerializeField] public float flipForce;
    [SerializeField] public float gravMult;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cForce = GetComponent<ConstantForce>();
        isHeld = false;
    }

    private void FixedUpdate()
    {
        if (isFlipper)
        {
            rb.AddForce(transform.up * flipForce, ForceMode.Force);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("GravField"))
        {
            gravity = collision.gameObject.GetComponent<GravityData>().gravity;
            gravityDir = Vector3.down;
            if (isHeld)
            {
                gravMult = 0f;
            }
            else
            {
                gravMult = 1f;
            }
            if (collision.gameObject.GetComponent<GravityData>().isDown)
            {
                cForce.force = gravityDir * gravity * gravMult;
                isGravDown = true;
            }
            else
            {
                cForce.force = -gravityDir * gravity * gravMult;
                isGravDown = false;
            }
        }
        if (collision.gameObject.CompareTag("Flipper"))
        {
            flipForce = collision.gameObject.GetComponent<FlipperData>().flipForce;
            if (collision.gameObject.GetComponent<FlipperData>().isActive)
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
}
