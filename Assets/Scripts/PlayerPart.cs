using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPart : MonoBehaviour
{

    private Rigidbody rb;
    private bool collected = false;

    public float minMass = 1f;
    public float maxMass = 3f;

    [SerializeField]
    private float throwSpeed = 20f;

    [SerializeField]
    private float maxRotationalForce = 5f;

    private BoxCollider boxCol;
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.mass = Random.Range(minMass, maxMass);
    }

    public bool Collected
    {
        get
        {
            return collected;
        }
        set
        {
            collected = value;
            if (boxCol == null)
            {
                boxCol = GetComponent<BoxCollider>();
            }
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            if (value)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;

            }

            Debug.Log("rb: " + rb);
            boxCol.enabled = !value;
        }
    }
    public void Throw(Vector3 dir)
    {
        rb.isKinematic = false;
        rb.AddForce(-dir.normalized * throwSpeed);
        rb.angularVelocity = new Vector3(
            Random.Range(-maxRotationalForce, maxRotationalForce),
            Random.Range(-maxRotationalForce, maxRotationalForce),
            Random.Range(-maxRotationalForce, maxRotationalForce)
            );

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
