using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public Animator door;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    public void OpenDoor()
    {
        isOpen = true;
        Debug.Log("OPEN IS TRUE");
        door.SetBool("isOpen", true);
    }
}
