using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    Debug.Log("Hit!");
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }
        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            pickObj.GetComponent<GravityObject>().isHeld = true;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    void DropObject()
    {
        heldObj.GetComponent<GravityObject>().isHeld = false;
        heldObjRB.drag = 0;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObjRB.transform.parent = null;
        heldObj = null;
    }
}