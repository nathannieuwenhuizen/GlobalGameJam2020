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

    [SerializeField]
    private float gravitationalSpeed = 1f;


    private BoxCollider boxCol;

    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.mass = Random.Range(minMass, maxMass);

        Twitch();
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
    public Vector2 TrackDirection()
    {
        return -Vector2.Perpendicular(new Vector2(transform.localPosition.x, transform.localPosition.z).normalized);
    }
    void Update()
    {
        if (!Collected && GameManager.instance.gameIsRunning)
        {
            transform.position += (new Vector3(TrackDirection().x, 0, TrackDirection().y) * gravitationalSpeed);
        }
    }
    public void Twitch()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
    private void FixedUpdate() {
        if (!collected && Random.Range(0,200) == 1) {
            Twitch();
        }
    }
}
