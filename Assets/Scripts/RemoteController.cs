using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoteController : MonoBehaviour
{
    [Header("Physics Parameters")]
    [SerializeField] public bool hasRemote;

    public Renderer remoteScreen;
    public Material screenDark;
    public Material screenOn;
    public Material screenOff;
    public Material bulbOn;

    public Animator animator;
    public PlayerController player;
    public PowerHandler powerHandler;

    // Start is called before the first frame update
    void Start()
    {
        hasRemote = false;
        remoteScreen.material = screenDark;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasRemote && !player.isHolding)
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
                    if (hit.collider.gameObject.GetComponent<ControlData>().isActive)
                    {
                        if (remoteScreen.material != screenOn)
                        {
                            remoteScreen.material = screenOn;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (hit.collider.gameObject.CompareTag("GravFloor"))
                            {
                                hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<GravityData>().GravOff();
                                if (hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>().CompareTag("GravField"))
                                {
                                    player.UpdatePlayerGrav(hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>());
                                    GameObject[] layerObjects = GameObject.FindObjectsOfType<GameObject>()
                                        .Where(obj => obj.layer == LayerMask.NameToLayer("Rigids"))
                                        .ToArray();
                                    foreach (GameObject obj in layerObjects)
                                    {
                                        obj.GetComponent<GravityObject>().UpdateOBJGrav(hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>(), 1f);
                                    }
                                }
                                hit.collider.gameObject.GetComponent<ControlData>().isActive = false;
                                powerHandler.UpdatePower();
                            }
                            if (hit.collider.gameObject.CompareTag("Flipper"))
                            {
                                hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<FlipperData>().FlipOff();
                                if (hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>().CompareTag("Flipper"))
                                {
                                    player.UpdateFlipForce(hit.collider.gameObject.GetComponent<FlipperData>().flipForce);
                                    GameObject[] layerObjects = GameObject.FindObjectsOfType<GameObject>()
                                            .Where(obj => obj.layer == LayerMask.NameToLayer("Rigids"))
                                            .ToArray();
                                    foreach (GameObject obj in layerObjects)
                                    {
                                        obj.GetComponent<GravityObject>().UpdateOBJFlipForce(hit.collider.gameObject.GetComponent<FlipperData>().flipForce);
                                    }
                            }
                                hit.collider.gameObject.GetComponent<ControlData>().isActive = false;
                                powerHandler.UpdatePower();
                            }
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
                            if (hit.collider.gameObject.CompareTag("GravFloor"))
                            {
                                hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<GravityData>().GravOn();
                                if (hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>().CompareTag("GravField"))
                                {
                                    player.UpdatePlayerGrav(hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>());
                                    GameObject[] layerObjects = GameObject.FindObjectsOfType<GameObject>()
                                            .Where(obj => obj.layer == LayerMask.NameToLayer("Rigids"))
                                            .ToArray();
                                    foreach (GameObject obj in layerObjects)
                                    {
                                        obj.GetComponent<GravityObject>().UpdateOBJGrav(hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>(), 1f);
                                    }
                                }
                                powerHandler.UpdatePower();
                                hit.collider.gameObject.GetComponent<ControlData>().isActive = true;
                                powerHandler.UpdatePower();
                            }
                            if (hit.collider.gameObject.CompareTag("Flipper"))
                            {
                                hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<FlipperData>().FlipOn();
                                if (hit.collider.gameObject.GetComponent<ControlData>().controlledObj.GetComponent<Collider>().CompareTag("Flipper"))
                                {
                                    player.UpdateFlipForce(hit.collider.gameObject.GetComponent<FlipperData>().flipForce);
                                    GameObject[] layerObjects = GameObject.FindObjectsOfType<GameObject>()
                                                .Where(obj => obj.layer == LayerMask.NameToLayer("Rigids"))
                                                .ToArray();
                                    foreach (GameObject obj in layerObjects)
                                    {
                                        obj.GetComponent<GravityObject>().UpdateOBJFlipForce(hit.collider.gameObject.GetComponent<FlipperData>().flipForce);
                                    }
                            }
                                hit.collider.gameObject.GetComponent<ControlData>().isActive = true;
                                powerHandler.UpdatePower();
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
