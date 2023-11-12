using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSway : MonoBehaviour
{

    public float swayAmount;
    public float smoothTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRot = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, smoothTime * Time.deltaTime);
    }
}
