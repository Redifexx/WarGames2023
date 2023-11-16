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
        //sphereCol = GetComponent<SphereCollider>();
        //gravMask = 1 << LayerMask.NameToLayer("Grav");
        rb = GetComponent<Rigidbody>();
        //cForce = GetComponent<ConstantForce>();
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
        UpdateOBJGrav(1);
    }

    private void FixedUpdate()
    {
        /*
        timeSinceLastCheck += Time.fixedDeltaTime;

        if (timeSinceLastCheck >= checkInterval)
        {
            // Get the radius of the SphereCollider
            float sphereRadius = 50f;

            // Use Physics.OverlapSphere with the determined radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius, gravMask);

            foreach (Collider collider in colliders)
            {
                // Do something with the GameObjects within the sphere collider
                Debug.Log("Object in sphere collider: " + collider.gameObject.name);
            }

            timeSinceLastCheck = 0f;
        }
        */
        rb.AddForce(-curGrav * gravMult, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider collision)
    {
        int gravMask = LayerMask.NameToLayer("Grav");
        if (collision.gameObject.layer == gravMask)
        {
            allForces.Add(collision.gameObject);
            UpdateOBJGrav(1);
        }
        /*
        if (collision.gameObject.CompareTag("GravField"))
        {
            curCollider = collision;
            gravityDir = Vector3.down;
            if (isHeld)
            {
                gravMult = 0f;
            }
            else
            {
                gravMult = 1f;
            }
            UpdateOBJGrav(collision, gravMult);
        }
        if (collision.gameObject.CompareTag("Flipper"))
        {
            flipForce = collision.gameObject.GetComponent<FlipperData>().flipForce;
            isFlipper = true;
        }
        if (collision.gameObject.CompareTag("QuantumPad") && !isCrate)
        {
            collision.gameObject.GetComponent<QuantumController>().objectCount++;
            collision.gameObject.GetComponent<QuantumController>().Check();
        }
        */
    }

    void OnTriggerExit(Collider collision)
    {
        int gravMask = LayerMask.NameToLayer("Grav");
        if (collision.gameObject.layer == gravMask)
        {
            allForces.Remove(collision.gameObject);
            UpdateOBJGrav(1);
        }
        /*
        if (collision.gameObject.CompareTag("Flipper"))
        {
            flipForce = 0f;
            isFlipper = false;
        }
        if (collision.gameObject.CompareTag("QuantumPad") && !isCrate)
        {
            collision.gameObject.GetComponent<QuantumController>().objectCount--;
            collision.gameObject.GetComponent<QuantumController>().Check();
        }
        */
    }

    public void UpdateOBJGrav(float gravMult_)
    {
        List<Vector3> allDirs = new List<Vector3>();
        List<Vector3> allVectors = new List<Vector3>();
        List<float> allGravs = new List<float>();
        foreach (GameObject obj in allForces)
        {
            allDirs.Add(obj.GetComponent<GravityDataV2>().gravDir);
            allVectors.Add(obj.GetComponent<GravityDataV2>().gravNormal);
            allGravs.Add(obj.GetComponent<GravityDataV2>().gravity);
        }
        curGrav = CalcAvg(allDirs);
        //cForce.force = -curGrav * gravMult_;
        /*
        gravity = col.gameObject.GetComponent<GravityData>().gravity;
        if (col.gameObject.GetComponent<GravityData>().isDown)
        {
            cForce.force = gravityDir * gravity * gravMult_;
            isGravDown = true;
        }
        else
        {
            cForce.force = -gravityDir * gravity * gravMult_;
            isGravDown = false;
        }
        */
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

    public Vector3 CalcDot(List<Vector3> allForces_, List<float> allGravs_)
    {
        Vector3 dotVector = Vector3.zero;
        for (int i = 0; i < allForces_.Count; i++)
        {
            for (int j = i + 1; j < allForces_.Count; j++)
            {
                // Calculate the dot product between normalized gravity vectors
                float dotProduct = Vector3.Dot(allForces_[i], allForces_[j]);

                // Combine the gravity vectors based on their strengths and dot product
                Vector3 combined = allForces_[i] * allGravs_[i] + allForces_[j] * allGravs_[j] * dotProduct;

                // Accumulate the combined gravity vectors
                dotVector += combined;
            }
        }
        return dotVector;
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
}
