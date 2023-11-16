using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDataV2 : MonoBehaviour
{
    public ControlDataV2 controlData;
    public float setGravity;
    public bool isDown;
    public float gravity;
    public Vector3 gravNormal;
    public Vector3 gravDir;

    void Start()
    {
        //use transform.up of normal transform
        gravNormal = this.transform.GetChild(0).up;
        if (Mathf.Abs(gravNormal.x) < 0.1f)
        {
            gravNormal.x = 0f;
        }
        if (Mathf.Abs(gravNormal.y) < 0.1f)
        {
            gravNormal.y = 0f;
        }
        if (Mathf.Abs(gravNormal.z) < 0.1f)
        {
            gravNormal.z = 0f;
        }
        gravNormal.Normalize();
        setGravity = gravity;
        if (!controlData.isActive)
        {
            gravity = 0f;
        }
        gravDir = gravNormal * gravity;
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
