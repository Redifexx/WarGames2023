using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumController : MonoBehaviour
{
    public bool isOccupied;
    public int objectCount;
    public bool hasCrate;
    public DoorController door;

    // Start is called before the first frame update
    void Start()
    {
        hasCrate = false;
        isOccupied = false;
        objectCount = 0;
    }

    public void Check()
    {
        if (objectCount > 0)
        {
            isOccupied = true;
        }
        else
        {
            isOccupied = false;
        }
    }

    public void CrateCheck(bool hasCrate_, float cratePower_)
    {
        hasCrate = hasCrate_;
        if (hasCrate && cratePower_ == 100f)
        {
            if (!isOccupied)
            {
                Debug.Log("CHECK");
                door.OpenDoor();
            }
        }
    }
}
