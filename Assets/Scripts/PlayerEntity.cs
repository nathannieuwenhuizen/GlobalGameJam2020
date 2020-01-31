using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{

    private List<PlayerPart> holdingParts;
    private Rigidbody rb;


    [SerializeField]
    private int playerIndex = 0;


    [SerializeField]
    private int startParts = 5;


    [SerializeField]
    private float shootBoostSpeed;
    [SerializeField]
    private float normalSpeed = 100f;

    [SerializeField]
    private Transform partsParent;

    [SerializeField]
    private GameObject partPrefab;

    private Vector3 lookRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        holdingParts = new List<PlayerPart> { };
        for (int i = 0; i < startParts; i++)
        {
            GameObject tempPrefab = Instantiate(partPrefab);
            GainPart(tempPrefab.GetComponent<PlayerPart>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerPart>())
        {
            PlayerPart part = collision.gameObject.GetComponent<PlayerPart>();
            if (!part.Collected)
            {
                GainPart(part);
            }
        }
    }

    public void GainPart(PlayerPart collectedPart)
    {
        holdingParts.Add(collectedPart);

        collectedPart.gameObject.transform.parent = partsParent;
        collectedPart.Collected = true;

        collectedPart.transform.localPosition = new Vector3( Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), -.7f);
        collectedPart.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void ThrowPart()
    {
        if (holdingParts.Count == 0)
        {
            return;
        }
        PlayerPart thrownPart = holdingParts[holdingParts.Count - 1];
        holdingParts.Remove(thrownPart);
        thrownPart.Collected = false;
        thrownPart.transform.parent = null;

        thrownPart.Throw(transform.forward);
        Debug.Log("throw! rb: " + rb);

        rb.AddForce(transform.forward * shootBoostSpeed);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ThrowPart();
        }
    }
    void FixedUpdate()
    {
        lookRotation = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddForce(lookRotation * normalSpeed);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {

        } else {
            rb.rotation = Quaternion.LookRotation(lookRotation);
        }
    }
}
