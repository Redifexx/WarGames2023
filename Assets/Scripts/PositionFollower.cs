using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position + offset, ref velocity, 0.1f);
    }
}
