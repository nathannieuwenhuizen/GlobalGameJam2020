using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{

    private List<PlayerPart> holdingParts;
    private Rigidbody rb;

    public bool reachedMidway = false;
    private int currentLap = 1;

    public int playerIndex = 0;

    public bool canMove = false;


    [SerializeField]
    private int startParts = 5;


    [SerializeField]
    private float shootBoostSpeed;
    [SerializeField]
    private float normalSpeed = 100f;
    [SerializeField]
    private float gravitationalSpeed = 1f;

    [SerializeField]
    private Transform partsParent;

    public GameObject partPrefab;

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


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<PlayerPart>())
        {
            PlayerPart part = other.gameObject.GetComponent<PlayerPart>();
            if (!part.Collected)
            {
                GainPart(part);
            }
        }

        string tag = other.gameObject.tag;
        if (tag == "midway")
        {
            reachedMidway = true;
        }
        if (tag == "finish" && reachedMidway)
        {
            reachedMidway = false;
            currentLap++;
            GameManager.instance.NextLap(this);
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
        if (Input.GetButtonDown("Fire_P"+ playerIndex) && canMove)
        {
            ThrowPart();
        }
        if (facesClockwise())
        {
            Debug.Log("Faces forwards");
        } else
        {
            Debug.Log("Faces backwards");
        }

    }

    public bool facesClockwise()
    {
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        float angle = Vector2.Angle(
            forward,
            TrackDirection()
            );
        return angle > 90;
    }
    public Vector2 TrackDirection()
    {
        return -Vector2.Perpendicular(new Vector2(transform.localPosition.x, transform.localPosition.z).normalized);
    }

    void FixedUpdate()
    {
        //getting input to aim
        lookRotation = new Vector3(Input.GetAxis("Horizontal_P" + playerIndex), 0, -Input.GetAxis("Vertical_P" + playerIndex));
        if (canMove)
        {
            //look rotational speed
            rb.AddForce(lookRotation * normalSpeed);

            //gravitational speed
            Debug.Log("track direction: " + TrackDirection());
            transform.position += (new Vector3(TrackDirection().x, 0, TrackDirection().y) * gravitationalSpeed);
        }

        //rotates
        if (Input.GetAxis("Horizontal_P" + playerIndex) == 0 && Input.GetAxis("Vertical_P" + playerIndex) == 0) {

        } else {
            rb.rotation = Quaternion.LookRotation(lookRotation);
        }
    }

    public int CurrentLap
    {
        get { return currentLap; }
        set { currentLap = value; }
    }
}
