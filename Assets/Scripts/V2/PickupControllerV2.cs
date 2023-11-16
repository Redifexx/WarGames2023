using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerV2 : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    public RemoteControllerV2 remote;
    public PlayerControllerV2 player;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !remote.hasRemote)
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    Debug.Log("Hit!");
                    if (!hit.collider.gameObject.CompareTag("Crate"))
                    {
                        PickupObject(hit.transform.gameObject);
                    }
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
            pickObj.GetComponent<GravityObjectV2>().isHeld = true;
            player.isHolding = true;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    void DropObject()
    {
        heldObj.GetComponent<GravityObjectV2>().isHeld = false;
        heldObj.GetComponent<GravityObjectV2>().LetGo();
        player.isHolding = false;
        heldObjRB.drag = 0;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObjRB.transform.parent = null;
        heldObj = null;
    }
}
