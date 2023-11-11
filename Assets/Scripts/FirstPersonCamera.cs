using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public PlayerController playerMovement;

    public float senX;
    public float senY;
    public float multiplier = 1f;

    public Transform orientation;

    public float xRot;
    public float yRot;
    public float zRot = 0f;
    public float targetZRot = 0f;
    public float lerpSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isStrafing)
        {
            //zRot = -playerMovement.horizontalInput * zRot;
        }
        float mouseX = (Input.GetAxis("Mouse X")) * Time.deltaTime * senX * multiplier;
        float mouseY = (Input.GetAxis("Mouse Y")) * Time.deltaTime * senY * multiplier;

        if (playerMovement.isGravDown)
        {
            yRot += mouseX;
            xRot -= mouseY;
        }
        else
        {
            yRot -= mouseX;
            xRot += mouseY;
        }

        xRot = Mathf.Clamp(xRot, -90f, 90f);
        zRot = Mathf.Lerp(zRot, targetZRot, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);

    }
}
