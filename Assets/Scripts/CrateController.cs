using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public float cratePower;
    public bool isCharging;
    public Collider curCol;
    public Color startColor = Color.red;
    public Color endColor = Color.green;
    public Renderer cube;
    public Material[] materials;
    public float maxPower = 100f;
    public Light pointLight;
    void Start()
    {
        cratePower = 0f;
        isCharging = false;
        cube = GetComponent<Renderer>();
        materials = cube.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging)
        {
            if (cratePower < 100f)
            {
                Charge(curCol.GetComponent<ChargerData>().chargingSpeed);
                cratePower = Mathf.Clamp(cratePower, 0f, maxPower);
                float t = Mathf.InverseLerp(0f, maxPower, cratePower);
                Color lerpedColor = Color.Lerp(startColor, endColor, t);
                materials[1].SetColor("_EmissiveColor", lerpedColor * 20f);
                pointLight.color = lerpedColor;
            }
            else
            {
                if (cratePower > 100f)
                {
                    cratePower = 100f;
                }
                isCharging = false;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        int layerMask = 1 << LayerMask.NameToLayer("Charger");
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            Debug.Log("trig entererd " + collision.gameObject.name + " object: " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Charger"))
            {
                Debug.Log("trig enter");
                if (cratePower < 100f)
                {
                    Debug.Log("trig power");
                    curCol = collision;
                    isCharging = true;
                }
            }
        }
        if(collision.gameObject.CompareTag("QuantumPad"))
        {
            collision.gameObject.GetComponent<QuantumController>().CrateCheck(true, cratePower);
        }
        /*
            Debug.Log(LayerMask.LayerToName(collision.gameObject.layer) + "obj: " + collision.gameObject.name);
        if (collision.gameObject.layer == layerMask)
        {
            Debug.Log("trig entererd " + collision.gameObject.name + " object: " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Charger"))
            {
                Debug.Log("trig enter");
                if (cratePower < 100f)
                {
                    Debug.Log("trig power");
                    curCol = collision;
                    isCharging = true;
                }
            }
        }
        */
    }

    void OnTriggerExit(Collider collision)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Controllable");
        if (collision.gameObject.layer == layerMask && collision.gameObject.CompareTag("Charger"))
        {
            Debug.Log("trig exit");
            isCharging = false;
        }
        if (collision.gameObject.CompareTag("QuantumPad"))
        {
            collision.gameObject.GetComponent<QuantumController>().CrateCheck(false, cratePower);
        }
    }

    public void Charge(float chargeSpeed_)
    {
        cratePower += (chargeSpeed_ * Time.deltaTime);
    }
}
