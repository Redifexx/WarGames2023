using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayUp : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Vector3 upDirection = transform.up;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, upDirection * 10f);
    }
}
