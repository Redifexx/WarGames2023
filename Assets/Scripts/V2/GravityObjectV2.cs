using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObjectV2 : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] public float gravity = 0f;
    [SerializeField] public Vector3 gravityDir;
    [SerializeField] private ConstantForce cForce;
    [SerializeField] public bool isGravDown;
    [SerializeField] public bool isFlipper;
    [SerializeField] public bool isHeld;
    [SerializeField] public bool isCrate;
    [SerializeField] public float flipForce;
    [SerializeField] public float gravMult;
    [SerializeField] public Collider curCollider;
    [SerializeField] public List<GameObject> allForces;
    [SerializeField] public Vector3 curGrav;
    Rigidbody rb;

    //New
    //[SerializeField] SphereCollider sphereCol;
    //public LayerMask gravMask;
    public float checkInterval = 0.1f;
    private float timeSinceLastCheck = 0f;


    private void Start()
    {
        gravMult = 1f;
        allForces = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        isHeld = false;
        if (this.gameObject.CompareTag("Crate"))
        {
            isCrate = true;
        }
        else
        {
            isCrate = false;
        }
        isGravDown = true;
        UpdateField(CalcDist());
    }

    private void FixedUpdate()
    {
        UpdateField(CalcDist());
        rb.AddForce(-curGrav * gravMult, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider collision)
    {
        int gravMask = LayerMask.NameToLayer("Grav");
        if (collision.gameObject.layer == gravMask)
        {
            allForces.Add(collision.gameObject);
            UpdateField(CalcDist());
        }
    }

    void OnTriggerExit(Collider collision)
    {
        int gravMask = LayerMask.NameToLayer("Grav");
        if (collision.gameObject.layer == gravMask)
        {
            allForces.Remove(collision.gameObject);
            UpdateField(CalcDist());
        }
    }

    public void UpdateField(List<float> allDists_)
    {
        List<Vector3> allDirs = new List<Vector3>();
        List<Vector3> allVectors = new List<Vector3>();
        List<float> allGravs = new List<float>();
        int count = 0;
        foreach (GameObject obj in allForces)
        {
            //allDirs.Add(obj.GetComponent<GravityDataV2>().gravDir);
            Vector3 tempNormal = obj.GetComponent<GravityDataV2>().gravNormal;
            allVectors.Add(tempNormal);
            float curGrav = (obj.GetComponent<GravityDataV2>().gravity) + 2.0f - (3.0f * Mathf.Pow(allDists_[count], 1.0f/2.0f));
            allGravs.Add(curGrav);
            count++;
            allDirs.Add(tempNormal * curGrav);
        }
        curGrav = CalcAvg(allDirs);
    }

    public void UpdateOBJFlipForce(float flipForce_)
    {
        flipForce = flipForce_;
        //isFlipper = isFlipper_;
    }

    public void LetGo()
    {
        //UpdateOBJGrav(curCollider, 1f);
        gravMult = 1f;
    }

    public Vector3 CalcAvg(List<Vector3> allDirs_)
    {
        Vector3 avgVector = Vector3.zero;
        int countX = 0;
        int countY = 0;
        int countZ = 0;
        if (allDirs_ != null && allDirs_.Count > 0)
        {
            float sumX = 0f;
            float sumY = 0f;
            float sumZ = 0f;

            foreach (Vector3 vector in allDirs_)
            {
                if (vector.x != 0f)
                {
                    countX++;
                }
                if (vector.y != 0f)
                {
                    countY++;
                }
                if (vector.z != 0f)
                {
                    countZ++;
                }

                sumX += vector.x;
                sumY += vector.y;
                sumZ += vector.z;
            }

            if (countX == 0)
            {
                countX++;
            }
            if (countY == 0)
            {        
                countY++;
            }
            if (countZ == 0)
            {        
                countZ++;
            }
            int count = allDirs_.Count;
            avgVector = new Vector3(sumX / countX, sumY / countY, sumZ / countZ);
        }
        return avgVector;
    }

    public List<float> CalcDist()
    {
        List<float> allDists = new List<float>();
        foreach (GameObject obj in allForces)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -obj.GetComponent<GravityDataV2>().gravNormal, out hit, 35.0f))
            {
                Debug.Log(hit.distance + " name: " + obj.name);
                if (hit.collider.gameObject.name == obj.name)
                {
                    allDists.Add(hit.distance);
                }
                else
                {
                    allDists.Add(0f);
                }
            }
        }
        return allDists;
    }
}
