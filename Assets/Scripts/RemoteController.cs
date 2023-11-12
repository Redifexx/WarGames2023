using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    [Header("Physics Parameters")]
    [SerializeField] private bool hasRemote;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hasRemote = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasRemote)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                EquipRemote();
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                UnEquipRemote();
            }
            int layerMask = 1 << LayerMask.NameToLayer("Controllable");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100f, layerMask))
            {
                Debug.Log("ray");
                if (hit.collider.gameObject.CompareTag("Controllable"))
                {
                    Debug.Log("EDITABLE");
                }
                
            }
        }
    }

    void EquipRemote()
    {
        hasRemote = true;
        Debug.Log("EQUIPPED!");
        animator.SetBool("isEquipped", hasRemote);
    }

    void UnEquipRemote()
    {
        hasRemote = false;
        Debug.Log("UNEQUIPPED!");
        animator.SetBool("isEquipped", hasRemote);
    }
}
