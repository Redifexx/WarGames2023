using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityData : MonoBehaviour
{
    public ControlData controlData;
    public float gravity;
    public float setGravity;
    public bool isDown;

    void Start()
    {
        setGravity = gravity;
        if (!controlData.isActive)
        {
            gravity = 0f;
        }
    }

    public void GravOn()
    {
        if (!controlData.isActive)
        {
            gravity = setGravity;
        }
    }

    public void GravOff()
    {
        if (controlData.isActive)
        {
            gravity = 0;
        }
    }
}
