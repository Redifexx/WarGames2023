using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    [Header("Physics Parameters")]
    [SerializeField] private bool hasRemote;

    public Renderer remoteScreen;
    public Material screenDark;
    public Material screenOn;
    public Material screenOff;
    public Material bulbOn;

    public Animator animator;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        hasRemote = false;
        remoteScreen.material = screenDark;
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
                if (hit.collider.gameObject.CompareTag("Controllable"))
                {
                    if (hit.collider.gameObject.GetComponent<ControlData>().isActive)
                    {
                        if (remoteScreen.material != screenOn)
                        {
                            remoteScreen.material = screenOn;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<GravityData>().GravOff();
                            player.UpdatePlayerGrav(hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>());
                            hit.collider.gameObject.GetComponent<ControlData>().isActive = false;
                        }
                    }
                    else
                    {
                        if (remoteScreen.material != screenOff)
                        {
                            remoteScreen.material = screenOff;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<GravityData>().GravOn();
                            player.UpdatePlayerGrav(hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>());
                            hit.collider.gameObject.GetComponent<ControlData>().isActive = true;
                        }
                    }
                }
            }
            else
            {
                if (remoteScreen.material != screenDark)
                {
                    remoteScreen.material = screenDark;
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
