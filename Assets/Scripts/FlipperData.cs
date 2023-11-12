using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperData : MonoBehaviour
{
    public float flipForce;
    public float setFlip;
    public ControlData controlData;

    void Start()
    {
        setFlip = flipForce;
        if (!controlData.isActive)
        {
            flipForce = 0f;
        }
    }

    public void FlipOn()
    {
        if (!controlData.isActive)
        {
            flipForce = setFlip;
        }
    }

    public void FlipOff()
    {
        if (controlData.isActive)
        {
            flipForce = 0f;
        }
    }
}
