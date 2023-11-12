using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PowerHandler : MonoBehaviour
{
    public string layerToFind = "Controllable";
    public RoomData roomData;
    public float maxPower;
    public float curPower;
    public GameObject[] layerObjects;
    public TextMeshProUGUI powerText;
    void Start()
    {
        maxPower = 0f;
        curPower = 0f;
    }

    public void SetPower(float maxPower_)
    {
        Debug.Log("SET POWER");
        maxPower = maxPower_;
        curPower = 0f;
        layerObjects = GameObject.FindObjectsOfType<GameObject>()
            .Where(obj => obj.layer == LayerMask.NameToLayer(layerToFind))
            .ToArray();

        foreach (GameObject obj in layerObjects)
        {
            Debug.Log(obj.name);
            if (obj.GetComponent<ControlData>().isActive)
            {
                curPower += obj.GetComponent<ControlData>().powerDraw;
            }
        }

        powerText.text = curPower.ToString();
    }

    public void UpdatePower()
    {
        Debug.Log("UPDATE POWER");
        curPower = 0f;
        foreach (GameObject obj in layerObjects)
        {
            if (obj.GetComponent<ControlData>().isActive)
            {
                Debug.Log(obj.GetComponent<ControlData>().powerDraw);
                curPower += obj.GetComponent<ControlData>().powerDraw;
            }
        }

        powerText.text = curPower.ToString();
    }
}
